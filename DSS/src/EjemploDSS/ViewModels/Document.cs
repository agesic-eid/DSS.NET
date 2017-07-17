using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjemploDSS.ViewModels
{
    public class Document
    {
        [Required, Display(Name ="Tipo de documento")]
        public DocumentType DocumentType { get; set; }

        [Required, Display(Name ="Documento")]
        public byte[] Contents { get; set; }
    }
}
