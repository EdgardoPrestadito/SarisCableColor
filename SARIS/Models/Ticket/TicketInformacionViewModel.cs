﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ticket
{
    public class TicketInformacionViewModel
    {
        public int fiIDRequerimiento { get; set; }
        public string fcTituloRequerimiento { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public string fcTipoRequerimiento { get; set; }
        public string fcDescripcionCategoria { get; set; }
        public string fcNombreUsuarioSolicitante { get; set; }
        public string fcDescripcionEstado { get; set; }
        public string fcClaseColor { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public Nullable<int> fiIDUrgencia { get; set; }
        public string fcDescripcionUrgencia { get; set; }
        public Nullable<int> fiIDImpacto { get; set; }
        public string fcDescripcionImpacto { get; set; }
        public Nullable<int> fiIDPrioridad { get; set; }
        public string fcDescripcionPrioridad { get; set; }

        public DateTime fdFechaAlarmaDeteccion { get; set; }
        public int fiPlataforma { get; set; }
        public int fiServicioAfectados { get; set; }
        public int fiMotivoEstado { get; set; }

        public string fcPlataforma { get; set; }
        public string fcServicioAfectados { get; set; }
        public string fcMotivoEstado { get; set; }
        public string fcDescripcionCategoriaResolucion { get; set; }
        public string fcDescripcionSubCategoriaResolucion { get; set; }
        public string fcNombreMotivo { get; set; }
    }
}