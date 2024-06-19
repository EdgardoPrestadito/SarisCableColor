using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Clacificacion_CI
{
    public class Clacificacion_CIViewModel
    {
        public int fiIDCI { get; set; }
        public string fcCI { get; set; }
        public bool EsEditar { get; internal set; }
    }
}