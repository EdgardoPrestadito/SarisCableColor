using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketMiewModel
    {
        public string fcDescripcionRequerimiento { get; set; }
        public int fiIDRequerimiento { get; set; }
        public System.DateTime fdFechaCreacion { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public byte fiIDAreaSolicitante { get; set; }
        public byte fiIDEstadoRequerimiento { get; set; }
        public int fiIDUsuarioAsignado { get; set; }
        public int fiIDUsuarioSolicitante { get; set; }
        public System.DateTime fdFechaAsignacion { get; set; }
        public System.DateTime fdFechadeCierre { get; set; }
        public string fcBuzondeCorreo { get; set; }
        public string fcDescripcionCategoria { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public Nullable<int> fiTipoRequerimiento { get; set; }
        public Nullable<int> fiCategoriadeDesarrollo { get; set; }
        public int fiTiempodeDesarrollo { get; set; }
        public Nullable<int> fiIDUrgencia { get; set; }
        public string fcDescripcionUrgencia { get; set; }
        public Nullable<int> fiIDImpacto { get; set; }
        public string fcDescripcionImpacto { get; set; }
        public Nullable<int> fiIDPrioridad { get; set; }
        public string fcDescripcionPrioridad { get; set; }

        public string fcNombreSolicitante { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public string fcNombreAsignado { get; set; }
        public string fcNombreAreaSolicitante { get; set; }
        public decimal fnValoracionRequerimiento { get; set; }
        public DateTime fdFechaUltimaModificacion { get; set; }
        public int fiIDUsuarioUltimaModificacion { get; set; }
        public int fiHorasTrabajadas { get; set; }
        public int fiAreaAsignada { get; set; }
        public string fcNombreAreaAsignada { get; set; }
        public int fiIdTicketPadre { get; set; }

        public DateTime fdFechaAlarmaDeteccion { get; set; }
        public int fiPlataforma { get; set; }
        public int fiServicioAfectados { get; set; }
        public int fiMotivoEstado { get; set; }

        public string fcPlataforma { get; set; }
        public string fcServicioAfectados { get; set; }
        public string fcMotivoEstado { get; set; }
        public int fiCategoriaResolucion { get; set; }
        public int fiSubCategoriaResolucion { get; set; }
        public int fiAfectacion { get; set; }
        public int fiTiempo { get; set; }
        public string fcUsuarioModificador { get; set; }
        public string fcNombreMotivo { get; set; }
        public string fcConfiguracion { get; set; }
        public string fcNombreServicio { get; set; }
        public string fcPais { get; set; }
        public string fcRegion { get; set; }
        public string fcCiudad { get; set; }
        public string fcLongitudLatitud { get; set; }
        public string fiSLA { get; set; }
        public string fcNombreResuluctor { get; set; }
        public DateTime fdFechaResuluctor { get; set; }
        public string fcAreaResoluctor { get; set; }
        public int fiIDPausa { get; set; }
        public string fcCategoriaResolucion { get; set; }
        public string fcSubCategoriaResolucion { get; set; }
        public string fcClacificacionCI { get; set; }
        public string fcMotivoPausa { get; set; }
        public string fcComentarioResoluctor { get; set; }

    }
}