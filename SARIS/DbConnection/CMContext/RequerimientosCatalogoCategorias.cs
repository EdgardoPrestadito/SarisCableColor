using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.DbConnection.CMContext
{
    public partial class RequerimientosCatalogoCategorias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        //public RequerimientosCatalogoCategorias()
        //{
        //    UsuariosPorArea = new HashSet<Usuarios_Maestro>();
        //}


        [Key]
        public string fiIDCategoriaDesarrollo { get; set; }

        public string fcDescripcionCategoria { get; set; }

        public int fcToken { get; set; }

        public int fiEstado { get; set; }



    }
}