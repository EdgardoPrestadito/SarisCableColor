using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class DashboardViewModel
    {
        public List<AreasIncidentesViewModel> AreasIncidentes { get; set; }
        public List<AreaTiempoViewModel> AreasTiempo{ get; set; }
        public List<IndicadoresViewModel> BulletViewModel { get; set; }
        public List<ConteoIncidenteViewModel> ConteoIncidentes { get; set; }
        public List<IncidentesEstadosViewModel> ConteoIncidentesEstados { get; set; }
        public List<IncidentesPorEstados> ConteoPorEstado { get; set; }
        public List<IncidentesPrioridad> ConteoPorPrioridad { get; set; }
        public List<IncidentesRegiones> ConteoPorRegiones { get; set; }
        public List<DatosIncidentesPendientesViewModel> TalbaPendientes { get; set; }
    }
}