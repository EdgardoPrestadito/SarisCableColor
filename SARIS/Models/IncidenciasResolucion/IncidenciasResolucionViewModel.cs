using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.IncidenciasResolucion
{
    public class IncidenciasResolucionViewModel
    {
        public int fiIDTipoRequerimientoResolucion { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public Nullable<int> fiIDCategoriaResolucion { get; set; }
        public string fcDescripcionCategoria { get; set; }
        public string fcToken { get; set; }
        public Nullable<int> fiUbicacion { get; set; }
        public string fcDescripcionUbicacion { get; set; }
        public bool EsEditar { get; set; }
    }
}