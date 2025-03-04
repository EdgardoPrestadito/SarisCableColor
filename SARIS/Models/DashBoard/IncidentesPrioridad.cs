using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class IncidentesPrioridad
    {
        public string fcDescripcionPrioridad { get; set; }
        public int fiMenos24Horas { get; set; }
        public int fi1A3Dias { get; set; }
        public int fi1Semana { get; set; }
        public int fiMas1Mes { get; set; }
        public int fiTotal { get; set; }
    }
}