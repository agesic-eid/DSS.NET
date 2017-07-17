using System;
using System.Deployment.Internal.CodeSigning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace DSSLibrary
{
    internal static class SignatureHelper
    {
        static SignatureHelper()
        {
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), DSSConstants.VALUE_SIGNATURE_METHOD_XML);
        }

        private static StoreName GetStoreNameFromString(string storeName)
        {
            StoreName sn;
            switch (storeName)
            {
                case "My":
                    sn = StoreName.My;
                    break;
                case "AddressBook":
                    sn = StoreName.AddressBook;
                    break;
                case "AuthRoot":
                    sn = StoreName.AuthRoot;
                    break;
                case "CertificateAuthority":
                    sn = StoreName.CertificateAuthority;
                    break;
                case "Disallowed":
                    sn = StoreName.Disallowed;
                    break;
                case "Root":
                    sn = StoreName.Root;
                    break;
                case "TrustedPeople":
                    sn = StoreName.TrustedPeople;
                    break;
                case "TrustedPublisher":
                    sn = StoreName.TrustedPublisher;
                    break;
                default:
                    sn = StoreName.My;
                    break;
            }
            return sn;
        }

        private static StoreLocation GetStoreLocationFromString(string storeLocation)
        {
            StoreLocation sl;
            if (storeLocation == StoreLocation.LocalMachine.ToString())
            {
                sl = StoreLocation.LocalMachine;
            }
            else
            {
                sl = StoreLocation.CurrentUser;
            }
            return sl;
        }

        private static X509Certificate2 GetCertificate2(string storeName, string storeLocation, string subject)
        {
            StoreName sn = GetStoreNameFromString(storeName);
            StoreLocation sl = GetStoreLocationFromString(storeLocation);

            X509Store store = new X509Store(sn, sl);

            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, subject, false);
            store.Close();

            return certs[0];
        }

        private static X509Certificate2 GetCertificate(CertificateConfiguration conf)
        {
            using (X509Store store = new X509Store(conf.StoreName, conf.StoreLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName,
                                                                           conf.Subject, false);
                return certs[0];
            }
        }

        internal static void SignXmlDocument(XmlDocument doc, XmlNode nodeWhereToPlaceSignature, DSSConfiguration configuration)
        {
            X509Certificate2 certificate = GetCertificate(configuration.SigningCertificate);
            CspParameters cspParams = new CspParameters(24) { KeyContainerName = "XML_DISG_RSA_KEY" };
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(cspParams);
            key.FromXmlString(certificate.PrivateKey.ToXmlString(true));

            SignedXml signed = new SignedXml(doc);
            signed.SigningKey = key;
            signed.SignedInfo.SignatureMethod = DSSConstants.VALUE_SIGNATURE_METHOD_XML;

            Reference reference = new Reference();
            reference.Uri = "";

            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.DigestMethod = DSSConstants.VALUE_DIGEST_METHOD_XML;

            signed.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signed.KeyInfo = keyInfo;

            signed.ComputeSignature();

            XmlNode signatureNode = doc.ImportNode(signed.GetXml(), true);
            nodeWhereToPlaceSignature.AppendChild(signatureNode);
        }

        internal static bool ValidateSignResponse(XmlDocument signResponse, DSSConfiguration configuration)
        {
            X509Certificate2 certificate = GetCertificate(configuration.DSSCertificate);

            XmlElement signatureNode = (XmlElement)signResponse.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
            SignedXml signed = new SignedXml(signatureNode);
            signed.LoadXml(signatureNode);

            return signed.CheckSignature(certificate, false);
        }
    }
}
