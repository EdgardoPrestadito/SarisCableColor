using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Plataformas
{
    public class PlataformaViewModel
    {
        public int fiIDPlataforma { get; set; }
        [Required]
        [Display(Name = "Nombre de Plataforma")]
        public string fcNombrePlataforma { get; set; }
        public string fcDescripcionPlataforma { get; set; }
        public Nullable<int> fiActivo { get; set; }
        public bool EsEditar { get; internal set; }
    }
}