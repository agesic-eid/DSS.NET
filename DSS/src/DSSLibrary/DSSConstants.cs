using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSLibrary
{
    public static class DSSConstants
    {
        public const string XML_VERSION = "1.0";
        public const string XML_ENCODING = "UTF-8";
        public const string XML_STANDALONE = "no";

        public const string NS_DSS = "urn:oasis:names:tc:dss:1.0:core:schema";
        public const string NS_PROFILE_GEMALTO = "urn:oasis:names:tc:dssx:1.0:profiles:gemalto:demo";
        public const string NS_ADES = "urn:oasis:names:tc:dss:1.0:profiles:AdES:schema#";
        public const string NS_DS = "http://www.w3.org/2000/09/xmldsig#";

        public const string SIGN_REQUEST = "SignRequest";
        public const string SIGN_REQUEST_PROFILE = "Profile";
        public const string SIGN_REQUEST_REQUEST_ID = "RequestID";

        public const string OPTIONAL_INPUTS = "OptionalInputs";

        public const string CLAIMED_IDENTITY = "ClaimedIdentity";

        public const string SUPPORTING_INFO = "SupportingInfo";

        public const string SIGNATURE_TYPE = "SignatureType";

        public const string SIGNATURE_METHODS = "SignatureMethods";
        public const string SIGNATURE_METHOD = "SignatureMethod";

        public const string SIGNATURE_FORM = "SignatureForm";

        public const string SIGN_RESPONSE_CONSUMER_URL = "SignResponseConsumerURL";

        public const string INPUT_DOCUMENTS = "InputDocuments";

        public const string DOCUMENT = "Document";

        public const string BASE64DATA = "Base64Data";

        public const string MIME_TYPE = "MimeType";

        public const string NAME = "Name";
        public const string NAME_FORMAT = "Format";

        public const string SIGNATURE = "Signature";

        public const string VALUE_NAME_FORMAT = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";
        public const string VALUE_SIGNATURE_FORM = "urn:oasis:names:tc:dss:1.0:profiles:AdES:forms:BES";
        public const string VALUE_SIGNATURE_METHOD = "SmartCard";

        public const string VALUE_SIGNATURE_METHOD_XML = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        public const string VALUE_DIGEST_METHOD_XML = "http://www.w3.org/2001/04/xmlenc#sha256";

        public const string RESULT_MAJOR = "ResultMajor";
        public const string SUCCESS = "urn:oasis:names:tc:dss:1.0:resultmajor:Success";
        public const string DOCUMENT_NODE = "Base64Signature";
    }

    public static class DSSConstantsCAdES
    {
        public const string VALUE_SIGNATURE_TYPE = "urn:ietf:rfc:3369";
        public const string VALUE_MIME_TYPE = "text/plain";
        public const string VALUE_SIGN_REQUEST_PROFILE = "urn:gemalto:dss:profiles:cades";
    }

    public static class DSSConstantsPAdES
    {
        public const string VALUE_SIGNATURE_TYPE = "urn:etsi:ts:102:778";
        public const string VALUE_MIME_TYPE = "application/pdf";
        public const string VALUE_SIGN_REQUEST_PROFILE = "urn:gemalto:dss:profiles:cades";
    }

    public static class DSSConstantsXAdES
    {
        public const string VALUE_SIGNATURE_TYPE = "urn:ietf:rfc:3275";
        public const string VALUE_SIGN_REQUEST_PROFILE = "urn:gemalto:dss:profiles:xades";
        public const string ESCAPED_XML = "EscapedXML";
        public const string DOCUMENT_ID = "ID";
        public const string SIGNED_REFERENCES = "SignedReferences";
        public const string SIGNED_REF = "SignedReference";
        public const string REF_ID = "RefId";
        public const string WHICH_DOCUMENT = "WhichDocument";
        public const string DOCUMENT_NODE_XML = "Base64XML";
    }

    public static class DSSConstantsCAdES_Hash
    {
        public const string VALUE_SIGNATURE_TYPE = "urn:ietf:rfc:3369";
        public const string VALUE_SIGN_REQUEST_PROFILE = "urn:gemalto:dss:profiles:cades";
        public const string DOCUMENT_HASH = "DocumentHash";
        public const string ATTR_DIGEST_METHOD_XML = "Algorithm";
        public const string VALUE_ATTR_DIGEST_METHOD = "http://www.w3.org/2000/09/xmldsig#";
        public const string ATTR_DIGEST_METHOD = "xmlns:ds";
        public const string DIGEST_METHOD = "DigestMethod";
        public const string DIGEST_VALUE = "DigestValue";
    }
}
