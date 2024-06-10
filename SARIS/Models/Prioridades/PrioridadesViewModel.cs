using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Prioridades
{
    public class PrioridadesViewModel
    {
        public int fiIDPrioridad { get; set; }
        public string fcDescripcionPrioridad { get; set; }
        public int fiNivelPrioridad { get; set; }
        public int fiTiempo { get; set; }
        public int fiActivo { get; set; }
        public int fiUsuarioCreador { get; set; }
        public DateTime fdfechaCreacion { get; set; }
        public int fiUsuarioModificador { get; set; }
        public DateTime fdfechaModificacion { get; set; }
        public bool EsEditar { get; set; }
    }
}