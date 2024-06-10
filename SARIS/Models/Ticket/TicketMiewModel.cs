using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketMiewModel
    {
        public int fiIDRequerimiento { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public int fiIDUsuarioSolicitante { get; set; }
        public string fcNombreSolicitante { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public byte fiIDEstadoRequerimiento { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public DateTime fdFechaAsignacion { get; set; }
        public int fiIDUsuarioAsignado { get; set; }
        public string fcNombreAsignado { get; set; }
        public DateTime fdFechadeCierre { get; set; }
        public int fiTiempodeDesarrollo { get; set; }
        public int fiCategoriadeDesarrollo { get; set; }
        public int fiTipoRequerimiento { get; set; }
        public byte fiIDAreaSolicitante { get; set; }
        public string fcNombreAreaSolicitante { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public string fcDescripcionCategoria { get; set; }
        //public int fiIdAreaSolicitante { get; set; }
        public decimal fnValoracionRequerimiento { get; set; }
        public DateTime fdFechaUltimaModificacion { get; set; }
        public int fiIDUsuarioUltimaModificacion { get; set; }
        public int fiHorasTrabajadas { get; set; }
        public int fiAreaAsignada { get; set; }
        public string fcNombreAreaAsignada { get; set; }
    }
}