using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Base
{
    public class TicketNotificacionViewModel
    {
        public int fiIDUsuarioPropietario { get; set; }
        public string fcUsuarioPropietario { get; set; }
        public int fiIDUsuarioReceptor { get; set; }
        public string fcUsuarioReceptor { get; set; }
        public string fiIDArea { get; set; }
        public string fcArea { get; set; }
        public string fcTitulo { get; set; }
        public string fcMensaje { get; set; }
        public DateTime fdFechaHora { get; set; }
    }
}