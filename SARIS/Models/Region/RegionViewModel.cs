using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Region
{
    public class RegionViewModel
    {
        [Display(Name = "Pais")]
        [Required]
        public int fIIDPais { get; set; }
        public string fcPais { get; set; }
        public int fiIDRegion { get; set; }
        [Display(Name = "Region")]
        [Required]
        public string fcRegion { get; set; }
        public bool EsEditar { get; internal set; }
    }
}