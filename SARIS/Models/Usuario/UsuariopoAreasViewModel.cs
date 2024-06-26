using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Usuario
{
    public class UsuariopoAreasViewModel
    {
        public int fiIdUsuarioverArea { get; set; }
        public string fcNombre { get; set; }
        public string fcAreas { get; set; }
        public string fcIdAreas { get; set; }
        public bool EsEditar { get; set; }
    }
}