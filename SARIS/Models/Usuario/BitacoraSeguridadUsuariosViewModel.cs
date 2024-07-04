using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Usuario
{
    public class BitacoraSeguridadUsuariosViewModel
    {
        public int fiIdBitacoraSeguridad { get; set; }
        public string fcNombreCorto { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public string fcPantallaAfectada { get; set; }
        public string fcObservacion { get; set; }
    }
}