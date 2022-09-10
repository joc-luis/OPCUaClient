using Opc.Ua;
using Opc.Ua.Client;
using OPCUaClient.Objects;
namespace OPCUaClient
{
    public class UaClient
    {

        private EndpointDescription EndpointDescription;
        private EndpointConfiguration EndpointConfig;
        private ConfiguredEndpoint Endpoint;
        private Session Session;
        private UserIdentity userIdentity;
        ApplicationConfiguration AppConfig;

        public UaClient(String appName, String serverUrl, bool security, String user = "", String password = "")
        {
            String path = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
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
                    ee.Accept = (ee.Error.StatusCode == StatusCodes.BadCertificateUntrusted);
                };
            }

            EndpointDescription = CoreClientUtils.SelectEndpoint(AppConfig, serverUrl, security);
            EndpointConfig = EndpointConfiguration.Create(AppConfig);
            Endpoint = new ConfiguredEndpoint(null, EndpointDescription, EndpointConfig);
        
        }


        public void Connect(uint lifeTime)
        {
            this.Disconnect();

            this.Session = Task.Run(async () => await Session.Create(AppConfig, Endpoint, false, false, AppConfig.ApplicationName, lifeTime, userIdentity, null)).GetAwaiter().GetResult();

            if (this.Session == null || !this.Session.Connected)
            {
                throw new Exception();
            }
        }

        public void Disconnect()
        {
            if (this.Session != null && this.Session.Connected)
            {
                this.Session.Close();
                this.Session.Dispose();
                this.Session = null;
            }
        }

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
            this.Session.Write(null, writeValues, out StatusCodeCollection statusCodes, out DiagnosticInfoCollection diagnosticInfo);
        }

        public void Write(Tag tag)
        {
            this.Write(tag.Address, tag.Value);
        }

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

            if (!StatusCode.IsGood(dataValues[0].StatusCode))
            {
                throw new Exception();
            }
            tag.Value = dataValues[0].Value;
            return tag;
        }

        public void Write(List<Tag> tags)
        {
            WriteValueCollection writeValues = new WriteValueCollection();

            tags.Select(tag => new WriteValue
            {
                NodeId = new NodeId(tag.Address, 2),
                AttributeId = Attributes.Value,
                Value = new DataValue()
                {
                    Value = tag.Value
                }
            });
            this.Session.Write(null, writeValues, out StatusCodeCollection statusCodes, out DiagnosticInfoCollection diagnosticInfo);
        }

        public List<Tag> Read(List<String> address)
        {
            var tags = new List<Tag>();
            ReadValueIdCollection readValues = new ReadValueIdCollection();
            readValues.AddRange(address.Select(a => new ReadValueId
            {
                NodeId = new NodeId(a, 2),
                AttributeId = Attributes.Value
            }));

            this.Session.Read(null, 0, TimestampsToReturn.Both, readValues, out DataValueCollection dataValues, out DiagnosticInfoCollection diagnosticInfo);

            tags.AddRange(dataValues.Select(d => new Tag
            {
                Value = d.Value,
                Address = d.ToString()
            }));

            return tags;
        }
    }
}