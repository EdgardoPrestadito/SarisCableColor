using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class DatosIncidentesPendientesViewModel
    {
        public string fcRegion { get; set; }
        public string Estado { get; set; }
        public int Orden { get; set; }
        public int fiMenos24Horas { get; set; }
        public decimal fiPorcentajeMenos24Horas { get; set; }
        public int fi1A3Dias { get; set; }
        public decimal fiPorcentaje1A3Dias { get; set; }
        public int fi1Semana { get; set; }
        public decimal fiPorcentaje1Semana { get; set; }
        public int fiMas1Semana { get; set; }
        public decimal fiPorcentajeMas1Semana { get; set; }
        public int fiMas1Mes { get; set; }
        public decimal fiPorcentajeMas1Mes { get; set; }
        public int fiTotalRequerimientos { get; set; }
    }
}