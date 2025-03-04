using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class IndicadoresViewModel
    {
        public int fiIncidentesActivos { get; set; }
        public int fiIncidentes1a4Horas { get; set; }
        public int fiIncidentes4Horas { get; set; }
        public int fiIncidentes8Horas { get; set; }
        public int fiIncidentesMas1Semana { get; set; }
        public int fiIncidentes24Horas { get; set; }
    }
}