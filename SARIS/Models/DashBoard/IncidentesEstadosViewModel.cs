using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.DashBoard
{
    public class IncidentesEstadosViewModel
    {
        public string fcArea { get; set; }
        public int fiCreado { get; set; }
        //public int fiAsignado { get; set; }
        public int fiEnproceso { get; set; }
        public int fiResuelto { get; set; }
        //public int fiCerrado { get; set; }
        //public int fiCancelado { get; set; }
        //public int fiReasignado { get; set; }
        public int fiPausado { get; set; }
    }
}