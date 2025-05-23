﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Indicadores
{
    public class ListaIndicadoresViewModel
    {
        [Display(Name = "Id")]
        public int fiIDTipoRequerimiento { get; set; }

        public string fcTipoRequerimiento { get; set; }

        public Nullable<int> fiIDCategoriaDesarrollo { get; set; }

        public string fcDescripcionCategoria { get; set; }

        public string fcToken { get; set; }

        public Nullable<int> fiUbicacion { get; set; }


        public string fcDescripcionUbicacion { get; set; }

    }
}