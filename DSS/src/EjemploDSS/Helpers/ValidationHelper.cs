using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using DSSLibrary;
using System.Security.Cryptography.Xml;

namespace EjemploDSS.Helpers
{
    public sealed class ValidationHelper
    {
        private DSSConfiguration configuration;

        public ValidationHelper(DSSConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void ValidarFirmaContratoCAdES(string idUsuario, byte[] contratoFirmado, string prefijos, string thumbprintRootAGESIC)
        {
            SignedCms verifyCms = new SignedCms();
            verifyCms.Decode(contratoFirmado);

            var pkc = verifyCms.Certificates;

            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
            cert.Import(verifyCms.Certificates[0].RawData);
            System.Security.Cryptography.X509Certificates.X509Chain chain = new System.Security.Cryptography.X509Certificates.X509Chain();
            chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
            if (!chain.Build(cert))
            {
                throw new ExcepcionHelper("El certificado utilizado no es válido.");
            }
            else
            {
                System.Security.Cryptography.X509Certificates.X509Certificate2 root = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                if (!root.Thumbprint.Equals(thumbprintRootAGESIC, StringComparison.InvariantCultureIgnoreCase))
                    throw new ExcepcionHelper("El certificado no fue emitido por la CA de AGESIC.");
            }

            bool valido = false;
            string nroDoc = idUsuario.Substring(idUsuario.LastIndexOf('-') + 1);
            foreach (string docPrefix in prefijos.Split(';'))
                if (verifyCms.Certificates[0].SubjectName.Name.IndexOf(docPrefix + nroDoc, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    valido = true;
            if (!valido)
                throw new ExcepcionHelper("El certificado no corresponde a la persona.");

            Match m = Regex.Match(verifyCms.Certificates[0].SubjectName.Name, "C\\s*=\\s*UY");
            if (!m.Success)
                throw new ExcepcionHelper("El certificado no fue emitido por Uruguay.");
        }

        public static void ValidarFirmaContratoXAdES(string idUsuario, XmlDocument contratoFirmado, string prefijos, string thumbprintRootAGESIC)
        {
            XmlElement signatureNode = (XmlElement)contratoFirmado.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
            SignedXml signed = new SignedXml(signatureNode);
            signed.LoadXml(signatureNode);

            if (signed.KeyInfo.Count > 0)
            {
                IEnumerable<System.Security.Cryptography.Xml.KeyInfoX509Data> listKeyInfoData = signed.KeyInfo.OfType<System.Security.Cryptography.Xml.KeyInfoX509Data>();
                
                List<System.Security.Cryptography.Xml.KeyInfoX509Data> listData = listKeyInfoData.ToList<System.Security.Cryptography.Xml.KeyInfoX509Data>();

                System.Security.Cryptography.Xml.KeyInfoX509Data keyData = listData[0];

                System.Security.Cryptography.X509Certificates.X509Certificate2 certificate = (System.Security.Cryptography.X509Certificates.X509Certificate2) keyData.Certificates[0];

                System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
                cert.Import(certificate.RawData);
                System.Security.Cryptography.X509Certificates.X509Chain chain = new System.Security.Cryptography.X509Certificates.X509Chain();
                chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
                if (!chain.Build(cert))
                {
                    throw new ExcepcionHelper("El certificado utilizado no es válido.");
                }
                else
                {
                    System.Security.Cryptography.X509Certificates.X509Certificate2 root = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                    if (!root.Thumbprint.Equals(thumbprintRootAGESIC, StringComparison.InvariantCultureIgnoreCase))
                        throw new ExcepcionHelper("El certificado no fue emitido por la CA de AGESIC.");
                }

                bool valido = false;
                string nroDoc = idUsuario.Substring(idUsuario.LastIndexOf('-') + 1);
                foreach (string docPrefix in prefijos.Split(';'))
                    if (certificate.SubjectName.Name.IndexOf(docPrefix + nroDoc, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        valido = true;
                if (!valido)
                    throw new ExcepcionHelper("El certificado no corresponde a la persona.");

                Match m = Regex.Match(certificate.SubjectName.Name, "C\\s*=\\s*UY");
                if (!m.Success)
                    throw new ExcepcionHelper("El certificado no fue emitido por Uruguay.");
            }else
            {
                throw new ExcepcionHelper("El documento no está firmado.");
            }
        }

        public static void ValidarFirmaContratoCAdES_Hash(string idUsuario, byte[] contratoFirmado, string prefijos, string thumbprintRootAGESIC)
        {
            SignedCms verifyCms = new SignedCms();
            verifyCms.Decode(contratoFirmado);

            var pkc = verifyCms.Certificates;

            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
            cert.Import(verifyCms.Certificates[0].RawData);
            System.Security.Cryptography.X509Certificates.X509Chain chain = new System.Security.Cryptography.X509Certificates.X509Chain();
            chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
            if (!chain.Build(cert))
            {
                throw new ExcepcionHelper("El certificado utilizado no es válido.");
            }
            else
            {
                System.Security.Cryptography.X509Certificates.X509Certificate2 root = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                if (!root.Thumbprint.Equals(thumbprintRootAGESIC, StringComparison.InvariantCultureIgnoreCase))
                    throw new ExcepcionHelper("El certificado no fue emitido por la CA de AGESIC.");
            }

            bool valido = false;
            string nroDoc = idUsuario.Substring(idUsuario.LastIndexOf('-') + 1);
            foreach (string docPrefix in prefijos.Split(';'))
                if (verifyCms.Certificates[0].SubjectName.Name.IndexOf(docPrefix + nroDoc, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    valido = true;
            if (!valido)
                throw new ExcepcionHelper("El certificado no corresponde a la persona.");

            Match m = Regex.Match(verifyCms.Certificates[0].SubjectName.Name, "C\\s*=\\s*UY");
            if (!m.Success)
                throw new ExcepcionHelper("El certificado no fue emitido por Uruguay.");
        }

        public static void ValidarFirmaContratoPAdES(string idUsuario, byte[] contratoFirmado, string prefijos, string thumbprintRootAGESIC)
        {
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(contratoFirmado);
            iTextSharp.text.pdf.AcroFields af = reader.AcroFields;
            var names = af.GetSignatureNames();

            if (names.Count == 0)
            {
                throw new ExcepcionHelper("El PDF no se encuentra firmado.");
            }

            foreach (string name in names)
            {
                if (!af.SignatureCoversWholeDocument(name))
                {
                    throw new ExcepcionHelper(string.Format("La firma: {0} no abarca todo el documento.", name));
                }

                iTextSharp.text.pdf.security.PdfPKCS7 pk = af.VerifySignature(name);
                var cal = pk.SignDate;
                var pkc = pk.Certificates;

                if (!pk.Verify())
                {
                    throw new ExcepcionHelper("La firma no pudo ser verificada.");
                }

                System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
                cert.Import(pk.SigningCertificate.GetEncoded());
                System.Security.Cryptography.X509Certificates.X509Chain chain = new System.Security.Cryptography.X509Certificates.X509Chain();
                chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
                if (!chain.Build(cert))
                {
                    throw new ExcepcionHelper("El certificado utilizado no es válido.");
                }
                else
                {
                    System.Security.Cryptography.X509Certificates.X509Certificate2 root = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                    if (!root.Thumbprint.Equals(thumbprintRootAGESIC, StringComparison.InvariantCultureIgnoreCase))
                        throw new ExcepcionHelper("El certificado no fue emitido por la CA de AGESIC.");
                }

                bool valido = false;
                string nroDoc = idUsuario.Substring(idUsuario.LastIndexOf('-') + 1);
                foreach (string docPrefix in prefijos.Split(';'))
                    if (pk.SigningCertificate.SubjectDN.ToString().IndexOf(docPrefix + nroDoc, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        valido = true;
                if (!valido)
                    throw new ExcepcionHelper("El certificado no corresponde a la persona.");

                Match m = Regex.Match(pk.SigningCertificate.SubjectDN.ToString(), "C\\s*=\\s*UY");
                if (!m.Success)
                    throw new ExcepcionHelper("El certificado no fue emitido por Uruguay.");
            }
        }
    }
}
