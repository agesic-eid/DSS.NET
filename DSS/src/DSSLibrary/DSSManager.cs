using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DSSLibrary
{
    public sealed class DSSManager
    {
        private DSSConfiguration configuration;

        public DSSManager(DSSConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //GENERAR REQUEST PARA PDF
        public XmlDocument GenerateSignRequestPAdES(byte[] documentToSign)
        {
            int requestID = 888910;

            XmlElement opIn, parent, node;
            XmlAttribute attribute;

            XmlDocument result = new XmlDocument();
            result.AppendChild(result.CreateXmlDeclaration(DSSConstants.XML_VERSION, DSSConstants.XML_ENCODING, DSSConstants.XML_STANDALONE));

            result.AppendChild(result.CreateElement(DSSConstants.SIGN_REQUEST, DSSConstants.NS_DSS));
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_PROFILE);
            attribute.Value = DSSConstantsPAdES.VALUE_SIGN_REQUEST_PROFILE;
            result.DocumentElement.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_REQUEST_ID);
            attribute.Value = requestID.ToString();
            result.DocumentElement.Attributes.Append(attribute);

            opIn = result.CreateElement(DSSConstants.OPTIONAL_INPUTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(opIn);

            parent = result.CreateElement(DSSConstants.CLAIMED_IDENTITY, DSSConstants.NS_DSS);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.NAME, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.NAME_FORMAT);
            attribute.Value = DSSConstants.VALUE_NAME_FORMAT;
            node.Attributes.Append(attribute);
            node.InnerText = configuration.ServiceProviderName;
            parent.AppendChild(node);

            node = result.CreateElement(DSSConstants.SUPPORTING_INFO, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.SIGN_RESPONSE_CONSUMER_URL, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = configuration.ResponseConsumerUrl;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_TYPE, DSSConstants.NS_DSS);
            parent.InnerText = DSSConstantsPAdES.VALUE_SIGNATURE_TYPE;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.SIGNATURE_METHODS, DSSConstants.NS_PROFILE_GEMALTO);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.SIGNATURE_METHOD, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = DSSConstants.VALUE_SIGNATURE_METHOD;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_FORM, DSSConstants.NS_ADES);
            parent.InnerText = DSSConstants.VALUE_SIGNATURE_FORM;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.INPUT_DOCUMENTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(parent);

            node = result.CreateElement(DSSConstants.DOCUMENT, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.BASE64DATA, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.MIME_TYPE);
            attribute.Value = DSSConstantsPAdES.VALUE_MIME_TYPE;
            node.Attributes.Append(attribute);
            node.InnerText = Convert.ToBase64String(documentToSign);
            parent.AppendChild(node);

            SignatureHelper.SignXmlDocument(result, opIn, configuration);           

            return result;
        }

        //GENERAR REQUEST PARA TXT
        public XmlDocument GenerateSignRequestCAdES(byte[] documentToSign)
        {
            int requestID = 540991;

            XmlElement opIn, parent, node;
            XmlAttribute attribute;

            XmlDocument result = new XmlDocument();
            result.AppendChild(result.CreateXmlDeclaration(DSSConstants.XML_VERSION, DSSConstants.XML_ENCODING, DSSConstants.XML_STANDALONE));

            result.AppendChild(result.CreateElement(DSSConstants.SIGN_REQUEST, DSSConstants.NS_DSS));
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_PROFILE);
            attribute.Value = DSSConstantsCAdES.VALUE_SIGN_REQUEST_PROFILE;
            result.DocumentElement.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_REQUEST_ID);
            attribute.Value = requestID.ToString();
            result.DocumentElement.Attributes.Append(attribute);

            opIn = result.CreateElement(DSSConstants.OPTIONAL_INPUTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(opIn);

            parent = result.CreateElement(DSSConstants.CLAIMED_IDENTITY, DSSConstants.NS_DSS);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.NAME, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.NAME_FORMAT);
            attribute.Value = DSSConstants.VALUE_NAME_FORMAT;
            node.Attributes.Append(attribute);
            node.InnerText = configuration.ServiceProviderName;
            parent.AppendChild(node);

            node = result.CreateElement(DSSConstants.SUPPORTING_INFO, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.SIGN_RESPONSE_CONSUMER_URL, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = configuration.ResponseConsumerUrl;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_TYPE, DSSConstants.NS_DSS);
            parent.InnerText = DSSConstantsCAdES.VALUE_SIGNATURE_TYPE;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.SIGNATURE_METHODS, DSSConstants.NS_PROFILE_GEMALTO);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.SIGNATURE_METHOD, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = DSSConstants.VALUE_SIGNATURE_METHOD;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_FORM, DSSConstants.NS_ADES);
            parent.InnerText = DSSConstants.VALUE_SIGNATURE_FORM;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.INPUT_DOCUMENTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(parent);

            node = result.CreateElement(DSSConstants.DOCUMENT, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.BASE64DATA, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.MIME_TYPE);
            attribute.Value = DSSConstantsCAdES.VALUE_MIME_TYPE;
            node.Attributes.Append(attribute);
            node.InnerText = Convert.ToBase64String(documentToSign);
            parent.AppendChild(node);

            SignatureHelper.SignXmlDocument(result, opIn, configuration);

            return result;
        }

        //GENERAR REQUEST PARA XML
        public XmlDocument GenerateSignRequestXAdES(byte[] documentToSign)
        {
            int requestID = 333654;
            string docID = "_d0fdb941-25ed-4533-9946-dc2fe003ad9c";
            string valueRefId = "_4a291d4d-f194-4672-9a23-5eeddfdb30a8";

            XmlElement opIn, parent, node;
            XmlAttribute attribute;

            XmlDocument result = new XmlDocument();
            result.AppendChild(result.CreateXmlDeclaration(DSSConstants.XML_VERSION, DSSConstants.XML_ENCODING, DSSConstants.XML_STANDALONE));

            result.AppendChild(result.CreateElement(DSSConstants.SIGN_REQUEST, DSSConstants.NS_DSS));
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_PROFILE);
            attribute.Value = DSSConstantsXAdES.VALUE_SIGN_REQUEST_PROFILE;
            result.DocumentElement.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_REQUEST_ID);
            attribute.Value = requestID.ToString();
            result.DocumentElement.Attributes.Append(attribute);

            opIn = result.CreateElement(DSSConstants.OPTIONAL_INPUTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(opIn);

            parent = result.CreateElement(DSSConstants.CLAIMED_IDENTITY, DSSConstants.NS_DSS);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.NAME, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.NAME_FORMAT);
            attribute.Value = DSSConstants.VALUE_NAME_FORMAT;
            node.Attributes.Append(attribute);
            node.InnerText = configuration.ServiceProviderName;
            parent.AppendChild(node);

            node = result.CreateElement(DSSConstants.SUPPORTING_INFO, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.SIGN_RESPONSE_CONSUMER_URL, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = configuration.ResponseConsumerUrl;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_TYPE, DSSConstants.NS_DSS);
            parent.InnerText = DSSConstantsXAdES.VALUE_SIGNATURE_TYPE;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.SIGNATURE_METHODS, DSSConstants.NS_PROFILE_GEMALTO);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.SIGNATURE_METHOD, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = DSSConstants.VALUE_SIGNATURE_METHOD;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_FORM, DSSConstants.NS_ADES);
            parent.InnerText = DSSConstants.VALUE_SIGNATURE_FORM;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstantsXAdES.SIGNED_REFERENCES, DSSConstants.NS_DSS);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstantsXAdES.SIGNED_REF, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstantsXAdES.REF_ID);
            attribute.Value = valueRefId;
            node.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstantsXAdES.WHICH_DOCUMENT);
            attribute.Value = docID;
            node.Attributes.Append(attribute);
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.INPUT_DOCUMENTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(parent);

            node = result.CreateElement(DSSConstants.DOCUMENT, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstantsXAdES.DOCUMENT_ID);
            attribute.Value = docID;
            node.Attributes.Append(attribute);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstantsXAdES.ESCAPED_XML, DSSConstants.NS_DSS);
            XmlDocument signResponseXml = new XmlDocument();
            signResponseXml.PreserveWhitespace = true;
            signResponseXml.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(Convert.ToBase64String(documentToSign))));
            node.InnerText = SecurityElement.Escape(signResponseXml.InnerXml);
            parent.AppendChild(node);

            SignatureHelper.SignXmlDocument(result, opIn, configuration);

            return result;
        }

        //GENERAR REQUEST PARA CADES HASH
        public XmlDocument GenerateSignRequestCAdES_Hash(byte[] documentToSign)
        {
            int requestID = 777785;

            XmlElement opIn, parent, node;
            XmlAttribute attribute;

            XmlDocument result = new XmlDocument();
            result.AppendChild(result.CreateXmlDeclaration(DSSConstants.XML_VERSION, DSSConstants.XML_ENCODING, DSSConstants.XML_STANDALONE));

            result.AppendChild(result.CreateElement(DSSConstants.SIGN_REQUEST, DSSConstants.NS_DSS));
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_PROFILE);
            attribute.Value = DSSConstantsCAdES_Hash.VALUE_SIGN_REQUEST_PROFILE;
            result.DocumentElement.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstants.SIGN_REQUEST_REQUEST_ID);
            attribute.Value = requestID.ToString();
            result.DocumentElement.Attributes.Append(attribute);

            opIn = result.CreateElement(DSSConstants.OPTIONAL_INPUTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(opIn);

            parent = result.CreateElement(DSSConstants.CLAIMED_IDENTITY, DSSConstants.NS_DSS);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.NAME, DSSConstants.NS_DSS);
            attribute = result.CreateAttribute(DSSConstants.NAME_FORMAT);
            attribute.Value = DSSConstants.VALUE_NAME_FORMAT;
            node.Attributes.Append(attribute);
            node.InnerText = configuration.ServiceProviderName;
            parent.AppendChild(node);

            node = result.CreateElement(DSSConstants.SUPPORTING_INFO, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstants.SIGN_RESPONSE_CONSUMER_URL, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = configuration.ResponseConsumerUrl;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_TYPE, DSSConstants.NS_DSS);
            parent.InnerText = DSSConstantsCAdES_Hash.VALUE_SIGNATURE_TYPE;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.SIGNATURE_METHODS, DSSConstants.NS_PROFILE_GEMALTO);
            opIn.AppendChild(parent);

            node = result.CreateElement(DSSConstants.SIGNATURE_METHOD, DSSConstants.NS_PROFILE_GEMALTO);
            node.InnerText = DSSConstants.VALUE_SIGNATURE_METHOD;
            parent.AppendChild(node);

            parent = result.CreateElement(DSSConstants.SIGNATURE_FORM, DSSConstants.NS_ADES);
            parent.InnerText = DSSConstants.VALUE_SIGNATURE_FORM;
            opIn.AppendChild(parent);

            parent = result.CreateElement(DSSConstants.INPUT_DOCUMENTS, DSSConstants.NS_DSS);
            result.DocumentElement.AppendChild(parent);

            node = result.CreateElement(DSSConstantsCAdES_Hash.DOCUMENT_HASH, DSSConstants.NS_DSS);
            parent.AppendChild(node);
            parent = node;

            node = result.CreateElement(DSSConstantsCAdES_Hash.DIGEST_METHOD, DSSConstants.NS_DS);
            attribute = result.CreateAttribute(DSSConstantsCAdES_Hash.ATTR_DIGEST_METHOD);
            attribute.Value = DSSConstantsCAdES_Hash.VALUE_ATTR_DIGEST_METHOD;
            node.Attributes.Append(attribute);
            attribute = result.CreateAttribute(DSSConstantsCAdES_Hash.ATTR_DIGEST_METHOD_XML);
            attribute.Value = DSSConstants.VALUE_DIGEST_METHOD_XML;
            node.Attributes.Append(attribute);

            parent.AppendChild(node);

            node = result.CreateElement(DSSConstantsCAdES_Hash.DIGEST_VALUE, DSSConstants.NS_DS);
            attribute = result.CreateAttribute(DSSConstantsCAdES_Hash.ATTR_DIGEST_METHOD);
            attribute.Value = DSSConstantsCAdES_Hash.VALUE_ATTR_DIGEST_METHOD;
            node.Attributes.Append(attribute);

            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] hashValue = mySHA256.ComputeHash(documentToSign);
            node.InnerText = Convert.ToBase64String(hashValue);

            parent.AppendChild(node);

            SignatureHelper.SignXmlDocument(result, opIn, configuration);

            return result;
        }

        public bool ValidateSignResponse(XmlDocument signResponse)
        {
            return SignatureHelper.ValidateSignResponse(signResponse, configuration);
        }
    }
}