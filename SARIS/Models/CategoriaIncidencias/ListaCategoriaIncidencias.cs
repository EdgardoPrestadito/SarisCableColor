using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.CategoriaIncidencias
{
    public class ListaCategoriaIncidencias
    {
        [Display(Name = "ID")]
        [Required]
        public int fiIDCategoriaDesarrollo { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string fcDescripcionCategoria { get; set; }

        [Display(Name = "Token")]
        public string fcToken { get; set; }

        [Display(Name = "Activo")]

        public int fiEstado { get; set; }

        public bool EsEditar { get; set; }

        [Display(Name = "Area")]
        [Required]
        public Nullable<short> fiIDArea { get; set; }
    

        public string fcDescripcion { get; set; }

    }
}