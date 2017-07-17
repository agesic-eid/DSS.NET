using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EjemploDSS.Helpers
{
    public class ExcepcionHelper : Exception
    {
        public ExcepcionHelper()
        {
        }

        public ExcepcionHelper(string mensaje)
            : base(mensaje)
        {
        }

        public ExcepcionHelper(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
