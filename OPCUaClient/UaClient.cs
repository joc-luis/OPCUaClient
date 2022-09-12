using Opc.Ua;
using Opc.Ua.Client;
using OPCUaClient.Objects;
using OPCUaClient.Exceptions;
using Opc.Ua.Configuration;

namespace OPCUaClient
{
    /// <summary>
    /// Client for OPCUA Server
    /// </summary>
    public class UaClient
    {

        private EndpointDescription EndpointDescription;
        private EndpointConfiguration EndpointConfig;
        private ConfiguredEndpoint Endpoint;
        private Session Session = null;
        private UserIdentity userIdentity;
        private ApplicationConfiguration AppConfig;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="appName">
        /// Name of the application
        /// </param>
        /// <param name="serverUrl">
        /// Url of server
        /// </param>
        /// <param name="security">
        /// Enable or disable the security
        /// </param>
        /// <param name="untrusted">
        /// Accept untrusted certificates
        /// </param>
        /// <param name="user">
        /// User of the OPCUA Server
        /// </param>
        /// <param name="password">
        /// Password of the user
        /// </param>
        public UaClient(String appName, String serverUrl, bool security, bool untrusted, String user = "", String password = "")
        {
            String path = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, "Application"));
            Directory.CreateDirectory(Path.Combine(path, "Trusted"));
            Directory.CreateDirectory(Path.Combine(path, "TrustedPeer"));
            Directory.CreateDirectory(Path.Combine(path, "Rejected"));
            String hostName = System.Net.Dns.GetHostName();
     
            if (user.Length > 0)
            {
                userIdentity = new UserIdentity(user, password);
            }
            else
            {
                userIdentity = new UserIdentity();
            }
            AppConfig = new ApplicationConfiguration
            {
                ApplicationName = appName,
                ApplicationUri = Utils.Format(@"urn:{0}"+appName, hostName),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreLocation = @"Directory",
                        StorePath = Path.Combine(path, "Application"),
                        SubjectName = $"CN={appName}, DC={hostName}"
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "Trusted")
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "TrustedPeer")
                    },
                    RejectedCertificateStore = new CertificateTrustList
                    {
                        StoreType = @"Directory",
                        StorePath = Path.Combine(path, "Rejected")
                    },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true,
                    RejectSHA1SignedCertificates = false
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 20000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 5000 },
                TraceConfiguration = new TraceConfiguration
                {
                    DeleteOnLoad = true,
                },
                DisableHiResClock = false
            };
            AppConfig.Validate(ApplicationType.Client).GetAwaiter().GetResult();

            if (AppConfig.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                AppConfig.CertificateValidator.CertificateValidation += (s, ee) =>
                {
                    ee.Accept = (ee.Error.StatusCode == StatusCodes.BadCertificateUntrusted && untrusted);
                };
            }

            var application = new ApplicationInstance
            {
                ApplicationName = appName,
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = AppConfig
            };
            Utils.SetTraceMask(0);
            application.CheckApplicationInstanceCertificate(true, 2048).GetAwaiter().GetResult();

            EndpointDescription = CoreClientUtils.SelectEndpoint(AppConfig, serverUrl, security);
            EndpointConfig = EndpointConfiguration.Create(AppConfig);
            Endpoint = new ConfiguredEndpoint(null, EndpointDescription, EndpointConfig);
        
        }

        /// <summary>
        /// Open the connection with the OPCUA Server
        /// </summary>
        /// <param name="lifeTime">
        /// Duration of the session in seconds
        /// </param>
        /// <exception cref="ServerException"></exception>

        public void Connect(uint lifeTime)
        {
            this.Disconnect();

            this.Session = Task.Run(async () => await Session.Create(AppConfig, Endpoint, false, false, AppConfig.ApplicationName, lifeTime * 1000, userIdentity, null)).GetAwaiter().GetResult();

            if (this.Session == null || !this.Session.Connected)
            {
                throw new ServerException("Error creating a session on the server");
            }
        }

        /// <summary>
        /// Close the connection with the OPCUA Server
        /// </summary>
        public void Disconnect()
        {
            if (this.Session != null && this.Session.Connected)
            {
                this.Session.Close();
                this.Session.Dispose();
                this.Session = null;
            }
        }


        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="value">
        /// Value to write
        /// </param>
        /// <exception cref="WriteException"></exception>
        public void Write(String address, Object value)
        {
            
            WriteValueCollection writeValues = new WriteValueCollection();
            var writeValue = new WriteValue
            {
                NodeId = new NodeId(address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue()
            };
            writeValue.Value.Value = value;
            writeValues.Add(writeValue);
            this.Session.Write(null, writeValues, out StatusCodeCollection statusCodes, out DiagnosticInfoCollection diagnosticInfo);
            if (!StatusCode.IsGood(statusCodes[0]))
            {
                throw new WriteException("Error writing value. Code: " + statusCodes[0].Code.ToString());
            }
        }

        /// <summary>
        /// Write a value on a tag
        /// </summary>
        /// <param name="tag"> <see cref="Tag"/></param>
        public void Write(Tag tag)
        {
            this.Write(tag.Address, tag.Value);
        }

        /// <summary>
        /// Read a tag of the sepecific address
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <returns>
        /// <see cref="Tag"/>
        /// </returns>
        /// <exception cref="ReadException"></exception>
        public Tag Read(String address)
        {
            var tag = new Tag
            {
                Address = address,
                Value = null,
                Quality = true
            };
            ReadValueIdCollection readValues = new ReadValueIdCollection()
            {
                new ReadValueId
                {
                    NodeId = new NodeId(address, 2),
                    AttributeId = Attributes.Value
                }
            };

            this.Session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues, out DiagnosticInfoCollection diagnosticInfo);

           
            tag.Value = dataValues[0].Value;
            tag.Quality = StatusCode.IsGood(dataValues[0].StatusCode);
            return tag;
        }


        /// <summary>
        /// Write a lis of values
        /// </summary>
        /// <param name="tags"> <see cref="Tag"/></param>
        /// <exception cref="WriteException"></exception>
        public void Write(List<Tag> tags)
        {
            WriteValueCollection writeValues = new WriteValueCollection();

            writeValues.AddRange(tags.Select(tag => new WriteValue
            {
                NodeId = new NodeId(tag.Address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue()
                {
                    Value = tag.Value
                }
            }));
            
            this.Session.Write(null, writeValues, out StatusCodeCollection statusCodes, out DiagnosticInfoCollection diagnosticInfo);

            if (statusCodes.Where(sc => !StatusCode.IsGood(sc)).Any())
            {
                var status = statusCodes.Where(sc => !StatusCode.IsGood(sc)).First();
                throw new WriteException("Error writing value. Code: " + status.Code.ToString());
            }
        }


        /// <summary>
        /// Read a list of tags on the OPCUA Server
        /// </summary>
        /// <param name="address">
        /// List of address to read.
        /// </param>
        /// <returns>
        /// A list of tags <see cref="Tag"/>
        /// </returns>
        public List<Tag> Read(List<String> address)
        {
            var tags = new List<Tag>();
            int i = 0;
            
            ReadValueIdCollection readValues = new ReadValueIdCollection();
            readValues.AddRange(address.Select(a => new ReadValueId
            {
                NodeId = new NodeId(a, 2),
                AttributeId = Attributes.Value
            }));
            
            this.Session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues, out DiagnosticInfoCollection diagnosticInfo);

            address.ForEach(a =>
            {
                tags.Add(new Tag
                {
                    Address = a,
                    Value = dataValues[i].Value,
                    Quality = StatusCode.IsGood(dataValues[i].StatusCode)
                });
                i++;
            });

            return tags;
        }


        private Subscription Subscription(int miliseconds)
        {

            var subscription = new Subscription()
            {
                PublishingEnabled = true,
                PublishingInterval = miliseconds,
                Priority = 1,
                KeepAliveCount = 10,
                LifetimeCount = 20,
                MaxNotificationsPerPublish = 1000
            };

            return subscription;
            
        }


        /// <summary>
        /// Monitoring a tag and execute a function when the value change
        /// </summary>
        /// <param name="address">
        /// Address of the tag
        /// </param>
        /// <param name="miliseconds">
        /// Sets the time to check changes in the tag
        /// </param>
        /// <param name="monitor">
        /// Function to execute when the value changes.
        /// </param>
        public void Monitoring(String address, int miliseconds, MonitoredItemNotificationEventHandler monitor)
        {
            var subscription = this.Subscription(miliseconds);
            MonitoredItem monitored = new MonitoredItem();
            monitored.StartNodeId = new NodeId(address, 2);
            monitored.AttributeId = Attributes.Value;
            monitored.Notification += monitor;
            subscription.AddItem(monitored);
            this.Session.AddSubscription(subscription);
            subscription.Create();
            subscription.ApplyChanges();
        }
    }
}