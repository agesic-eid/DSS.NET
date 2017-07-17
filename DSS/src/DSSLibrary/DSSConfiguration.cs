using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DSSLibrary
{
    public sealed class DSSConfiguration
    {
        public string ResponseConsumerUrl { get; set; }

        public string ServiceProviderName { get; set; }

        public CertificateConfiguration SigningCertificate { get; set; }

        public CertificateConfiguration DSSCertificate { get; set; }

        public string DSSServiceUrl { get; set; }

        public Properties PropertiesAttributes { get; set; }
    }

    public sealed class CertificateConfiguration
    {
        public StoreName StoreName { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public string Subject { get; set; }
    }

    public sealed class Properties
    {
        public string IdUsuario { get; set; }

        public string DocumentCertificatePrefixes { get; set; }

        public string ThumbprintRootAGESIC { get; set; }
    }
}
