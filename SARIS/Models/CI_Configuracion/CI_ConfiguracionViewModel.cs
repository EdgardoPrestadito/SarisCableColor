using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.CI_Configuracion
{
    public class CI_ConfiguracionViewModel
    {
        public Nullable<int> fiIDPais { get; set; }
        public string fcPais { get; set; }
        public Nullable<int> fiIDRegion { get; set; }
        public string fcRegion { get; set; }
        public Nullable<int> fiIDCiudad { get; set; }
        public string fcCiudad { get; set; }
        public Nullable<int> fiIDCI { get; set; }
        public int fiIDConfiguracion { get; set; }
        public string fcConfiguracion { get; set; }
        public string fcLatitud { get; set; }
        public string fcLongitud { get; set; }
        public string fcCI { get; set; }
        public bool EsEditar { get; set; }


    }
}