using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketCerradosyCanceladorViewModel
    {
        public int fiIDRequerimiento { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public int fiIDUsuarioSolicitante { get; set; }
        public int fcNombreSolicitante { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public int fiIDEstadoRequerimiento { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public DateTime fdFechaAsignacion { get; set; }
        public int fiIDUsuarioAsignado { get; set; }
        public string fcNombreAsignado { get; set; }
        public DateTime fdFechadeCierre { get; set; }
        public int fiTiempodeDesarrollo { get; set; }
        public int fiCategoriadeDesarrollo { get; set; }
        public int fcDescripcionCategoria { get; set; }
        public int fiTipoRequerimiento { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public int fiIDAreaSolicitante { get; set; }
        public string fcDescripcion { get; set; }
        public string fcNombreAreaSolicitante { get; set; }
        public int fiAreaAsignada { get; set; }
        public string fcNombreAreaAsignada { get; set; }
        public double fnValoracionRequerimiento { get; set; }
        public DateTime fdFechaUltimaModificacion { get; set; }
        public int fiIDUsuarioUltimaModificacion { get; set; }
        public int fiIDUrgencia { get; set; }
        public string fcDescripcionUrgencia { get; set; }
        public int fiIDImpacto { get; set; }
        public string fcDescripcionImpacto { get; set; }
        public int fiIDPrioridad { get; set; }
        public string fcDescripcionPrioridad { get; set; }
        public DateTime fdFechaAlarmaDeteccion { get; set; }
        public int fiAfectacion { get; set; }
        public int fiTiempo { get; set; }
        public string fcUsuarioModificador { get; set; }
        public int fiMotivoEstado { get; set; }
        public string fcNombreMotivo { get; set; }
        public int fIDIUsuario { get; set; }

    }
}