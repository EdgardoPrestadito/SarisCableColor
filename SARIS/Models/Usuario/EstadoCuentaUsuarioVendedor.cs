using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Usuario
{
    public class EstadoCuentaUsuarioVendedor
    {
        public DateTime fdFechaMovimiento { get; set; }
        public string fcTipoMovimiento  { get; set; }
        public string fcReferencia { get; set; }
        public decimal fnValorMovimiento { get; set; }

        public List<DetalleEstadoCuentaUsuarioVendedor> DetalleCuentaVendedor { get; set; }

    }

    public class DetalleEstadoCuentaUsuarioVendedor
    {
        public DateTime fdFechaMovimiento { get; set; }
        public string fcTipoMovimiento { get; set; }
        public string fcReferencia { get; set; }
        public decimal fnValorMovimiento { get; set; }

    }
}