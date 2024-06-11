using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Indicadores
{
    public class IndicadoresCrearViewModel
    {
        [Display(Name = "Id")]
        public int fiIDTipoRequerimiento { get; set; }

        [Display(Name = "Categoria")]
        public int fiIDCategoriaDesarrollo { get; set; }


        [Display(Name = "Descripcion Sub Categoria")]
        [Required]

        public string fcTipoRequerimiento { get; set; }

        [Display(Name = "Estado")]

        public string fcToken { get; set; }

        public bool EsEditar { get; set; }

        [Display(Name = "Ubicacion")]
        public int fiUbicacion { get; set; }

        public string fcDescripcionUbicacion { get; set; }



    }
}