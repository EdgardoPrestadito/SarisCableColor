using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.EmailTemplateService
{
    public class EmailTemplateTicketModel
    {
        public int fiIDRequerimiento { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public int fiIDAreaSolicitante { get; set; }
        public string fcAreaSolicitante { get; set; }
        public int fiIDUsuarioSolicitante { get; set; }
        public string fcNombreCorto { get; set; }
        public int fiIDEstadoRequerimiento { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcCorreoElectronico { get; set; }
        public string fcDescripcionCategoria { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public string fcTelefonoMovil { get; set; }
        public int fiIDUrgencia { get; set; }
        public string fcDescripcionUrgencia { get; set; }
        public int fiIDImpacto { get; set; }
        public string fcDescripcionImpacto { get; set; }
        public int fiIDPrioridad { get; set; }
        public string fcDescripcionPrioridad { get; set; }
        public string fcComentario { get; set; }
    }
}