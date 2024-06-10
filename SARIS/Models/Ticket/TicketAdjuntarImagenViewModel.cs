using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketAdjuntarImagenViewModel
    {
        public int piIDRequerimiento { get; set; }
        public string pcNombreArchivo { get; set; }
        public string pcTipoArchivo { get; set; }
        public string pcRutaArchivo { get; set; }
        public string pcURL { get; set; }
        public int piIDSesion { get; set; }
        public int piIDApp { get; set; }
        public int piIDUsuario { get; set; }
        public bool pbEsImagen { get; set; }

    }
}