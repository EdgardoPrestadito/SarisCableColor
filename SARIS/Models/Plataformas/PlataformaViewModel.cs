using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Plataformas
{
    public class PlataformaViewModel
    {
        public int fiIDPlataforma { get; set; }
        public string fcNombrePlataforma { get; set; }
        public string fcDescripcionPlataforma { get; set; }
        public Nullable<int> fiActivo { get; set; }
        public bool EsEditar { get; internal set; }
    }
}