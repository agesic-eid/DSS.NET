using DSSLibrary;
using EjemploDSS.Helpers;
using EjemploDSS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EjemploDSS.Controllers
{
    public class SignatureController : Controller
    {
        private IMemoryCache _cache;

        private DSSManager dss;
        private DSSConfiguration configuration;

        public SignatureController(DSSManager dss, DSSConfiguration configuration, IMemoryCache memoryCache)
        {
            this.dss = dss;
            this.configuration = configuration;
            _cache = memoryCache;
        }

        public IActionResult Sign()
        {
           return View();
        }

        public void CacheAsistForRequest(string requestId, string docType)
        {
            string cacheEntry;

            if (!_cache.TryGetValue(requestId, out cacheEntry))
            {
                cacheEntry = docType;

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(600)); //10 minutos

                //Le asocio al requestId el tipo de documento que se va a firmar
                _cache.Set(requestId, cacheEntry, cacheEntryOptions);
            }
        }

        [HttpPost]
        public IActionResult Sign(Document document, IFormFile contents)
        {
            if (ModelState.IsValid)
            {
                using (BinaryReader reader = new BinaryReader(contents.OpenReadStream()))
                {
                    document.Contents = reader.ReadBytes((int)reader.BaseStream.Length);
                }
                XmlDocument signRequestXml = new XmlDocument();
                switch (document.DocumentType)
                {
                    case DocumentType.CAdES:
                        int requestID = 540991; //Ejemplo en CAdES

                        CacheAsistForRequest(requestID.ToString(), DocumentType.CAdES.ToString());

                        if (contents.ContentType == "text/plain") {
                            signRequestXml = dss.GenerateSignRequestCAdES(document.Contents);
                            break;
                        } else
                        {
                            throw new ExcepcionHelper("El archivo tiene un formato incorrecto.");
                        }

                    case DocumentType.PAdES:
                        int requestIDPades = 888910; //Ejemplo en PAdES

                        CacheAsistForRequest(requestIDPades.ToString(), DocumentType.PAdES.ToString());

                        if (contents.ContentType == "application/pdf")
                        {
                            signRequestXml = dss.GenerateSignRequestPAdES(document.Contents);
                            break;
                        } else
                        {
                            throw new ExcepcionHelper("El archivo tiene un formato incorrecto.");
                        }
                    case DocumentType.XAdES:
                        int requestIDXades = 333654; //Ejemplo en XAdES

                        CacheAsistForRequest(requestIDXades.ToString(), DocumentType.XAdES.ToString());

                        if (contents.ContentType == "text/xml" || contents.ContentType == "application/xml")
                        {
                            signRequestXml = dss.GenerateSignRequestXAdES(document.Contents);
                            break;
                        }
                        else
                        {
                            throw new ExcepcionHelper("El archivo tiene un formato incorrecto.");
                        }
                    case DocumentType.CAdES_Hash:
                        int requestIDHash = 777785; //Ejemplo en CAdES Hash

                        CacheAsistForRequest(requestIDHash.ToString(), DocumentType.CAdES_Hash.ToString());

                        signRequestXml = dss.GenerateSignRequestCAdES_Hash(document.Contents);
                        break;
                };
                SignRequest signRequest = new SignRequest
                {
                    Action = configuration.DSSServiceUrl,
                    EncodedSignRequest = Convert.ToBase64String(Encoding.UTF8.GetBytes(signRequestXml.OuterXml))
                };
                return View("SignRequest", signRequest);
            }
            return View("CambioInfoPassConfirmacion");
        }

        private static string FormatXML(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace,
                Encoding = new UTF8Encoding(false)
            };

            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                doc.Save(writer);
                var xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }
        }

        [HttpPost]
        public IActionResult ProcesarRespuestaFirma(string signResponse)
        {
            XmlDocument signResponseXml = new XmlDocument();

            if (ModelState.IsValid)
            {
                signResponseXml.PreserveWhitespace = true;
                signResponseXml.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(signResponse)));
                bool valid = dss.ValidateSignResponse(signResponseXml);
                if (valid)
                {
                    string usuario = "99999999"; // Representa el id de la persona logueada

                    XmlElement signResp = (XmlElement)signResponseXml.GetElementsByTagName("SignResponse", "urn:oasis:names:tc:dss:1.0:core:schema")[0];
                    string requestId = signResp.GetAttribute("RequestID");

                    string cacheType;
                    _cache.TryGetValue(requestId, out cacheType);

                    switch (cacheType)
                    {
                        case "CAdES":
                            XmlElement signatureNode = (XmlElement)signResponseXml.GetElementsByTagName(DSSConstants.DOCUMENT_NODE, "urn:oasis:names:tc:dss:1.0:core:schema")[0];
                            byte[] signedDocument = Convert.FromBase64String(signatureNode.InnerText);

                            Helpers.ValidationHelper.ValidarFirmaContratoCAdES(usuario, signedDocument, configuration.PropertiesAttributes.DocumentCertificatePrefixes, configuration.PropertiesAttributes.ThumbprintRootAGESIC);
                            break;
                        case "PAdES":
                            XmlElement signatureNodePades = (XmlElement)signResponseXml.GetElementsByTagName(DSSConstants.DOCUMENT_NODE, "urn:oasis:names:tc:dss:1.0:core:schema")[0];
                            byte[] signedDocumentPades = Convert.FromBase64String(signatureNodePades.InnerText);

                            Helpers.ValidationHelper.ValidarFirmaContratoPAdES(usuario, signedDocumentPades, configuration.PropertiesAttributes.DocumentCertificatePrefixes, configuration.PropertiesAttributes.ThumbprintRootAGESIC);
                            break;
                        case "XAdES":
                            XmlElement signatureNodeXades = (XmlElement)signResponseXml.GetElementsByTagName(DSSConstantsXAdES.DOCUMENT_NODE_XML, "urn:oasis:names:tc:dss:1.0:core:schema")[0];
                            byte[] signedDocumentXades = Convert.FromBase64String(signatureNodeXades.InnerText);

                            XmlDocument Base64XML = new XmlDocument();
                            Base64XML.PreserveWhitespace = true;
                            Base64XML.LoadXml(Encoding.UTF8.GetString(signedDocumentXades));

                            Helpers.ValidationHelper.ValidarFirmaContratoXAdES(usuario, Base64XML, configuration.PropertiesAttributes.DocumentCertificatePrefixes, configuration.PropertiesAttributes.ThumbprintRootAGESIC);
                            break;
                        case "CAdES_Hash":
                            XmlElement signatureNodeHash = (XmlElement)signResponseXml.GetElementsByTagName(DSSConstants.DOCUMENT_NODE, "urn:oasis:names:tc:dss:1.0:core:schema")[0];
                            byte[] signedDocumentHash = Convert.FromBase64String(signatureNodeHash.InnerText);

                            Helpers.ValidationHelper.ValidarFirmaContratoCAdES_Hash(usuario, signedDocumentHash, configuration.PropertiesAttributes.DocumentCertificatePrefixes, configuration.PropertiesAttributes.ThumbprintRootAGESIC);
                            break;
                        default:
                            ViewBag.EsError = true;
                            ViewBag.Detalle = "Se ha detectado un error:";
                            ViewBag.SubDetalle = "El usuario logueado no recibió correctamente el documento. Intente nuevamente.";
                            return View("SignRequestResult");
                    }

                    _cache.Remove(requestId);
                }
                else
                {
                    ViewBag.EsError = true;
                    ViewBag.Detalle = "Se ha detectado un error:";
                    ViewBag.SubDetalle = "La firma del xml SignResponse no es válida.";
                    return View("SignRequestResult");
                }
            }
            ViewBag.EsError = false;
            ViewBag.Detalle = "El documento ha sido firmado y validado correctamente.";
            ViewBag.SubDetalle = "El xml SignResponse procesado fue el siguiente:";
            ViewBag.Documento = FormatXML(signResponseXml.OuterXml);
            return View("SignRequestResult");
        }
    }
}