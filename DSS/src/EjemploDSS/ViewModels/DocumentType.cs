using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EjemploDSS.ViewModels
{
    public enum DocumentType
    {
        [Display(Name="CAdES - Texto")]
        CAdES,
        [Display(Name= "PAdES - PDF")]
        PAdES,
        [Display(Name = "XAdES - XML")]
        XAdES,
        [Display(Name = "CAdES Hash")]
        CAdES_Hash
    }
}
