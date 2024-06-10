using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Impacto
{
    public class ImpactoViewModel
    {
        public int fiIDImpacto { get; set; }
        public string fcDescripcionImpacto { get; set; }
        public int fiAfectacion { get; set; }
        public int fiActivo { get; set; }
        public int fiNivel { get; set; }
        public bool EsEditar { get; set; }
    }
}