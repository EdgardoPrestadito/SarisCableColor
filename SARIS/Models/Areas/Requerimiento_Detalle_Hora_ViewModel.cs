using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class Requerimiento_Detalle_Hora_ViewModel
    {
        public int fiIDRequerimiento { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionEstado { get; set; }
        public int fiTiempodeDesarrollo { get; set; }
        public int fiHorasdeTrabajodesarrollo { get; set; }
        public string fcCategoria { get; set; }
        public string fcEstadodelDesarrollo { get; set; }
        public int fiCuentaEstado { get; set; }
        public string fcNombreCorto { get; set; }
        public string fcUsuarioSolicitante { get; set; }
        public decimal fnPorcentajeEficiencia { get; set; }
        public string fcClaseColor { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public DateTime fdFechaAsignacion { get; set; }
        public DateTime fdFechadeCierre { get; set; }
        public string fcNombreAreaSolicitante { get; set; }
    }
}