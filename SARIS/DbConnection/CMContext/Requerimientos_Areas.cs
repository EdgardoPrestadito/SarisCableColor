using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.DbConnection.CMContext
{
    public partial class Requerimientos_Areas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        
        public Requerimientos_Areas()
        {
            UsuariosPorArea = new HashSet<Usuarios_Maestro>();
        }


        [Key]
        public int fiIDArea { get; set; }

        public string fcDescripcion { get; set; }
        public string fcCorreoElectronico { get; set; }
        public string fiIDUsuarioResponsable { get; set; }

        public virtual ICollection<Usuarios_Maestro> UsuariosPorArea { get; set; }


    }
}