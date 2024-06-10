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

        [Display(Name = "Categoria de Incidencia")]
        public int fiIDCategoriaDesarrollo { get; set; }


        [Display(Name = "Tipo de Requerimiento")]
        [Required]

        public string fcTipoRequerimiento { get; set; }

        [Display(Name = "Estado")]

        public string fcToken { get; set; }

        public bool EsEditar { get; set; }


    }
}