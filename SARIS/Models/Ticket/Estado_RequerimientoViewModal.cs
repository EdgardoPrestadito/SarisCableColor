using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class Estado_RequerimientoViewModal
    {
        public int fiIDRequerimiento { get; set; }
        public int fIDIUsuario { get; set; }
        public DateTime fdFechaInicioEstado { get; set; }
        public DateTime fdFechaFinEstado { get; set; }
        public int fiIDEstado { get; set; }
        public int fiIDApp { get; set; }
        public string fcToken { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public string fcDescripcion { get; set; }
        public string fcNombreCorto { get; set; }
        public int fiHorasTrabajadas { get; set; }
        public int fiMinutios { get; set; }
        public string fcComentario { get; set; }
    }
}