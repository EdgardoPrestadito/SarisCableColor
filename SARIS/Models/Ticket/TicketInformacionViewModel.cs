using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketInformacionViewModel
    {
        public int fiIDRequerimiento { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public string fcDescripcionCategoria { get; set; }
        public string fcNombreUsuarioSolicitante { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public DateTime fdFechaCreacion { get; set; }
    }
}