using OrionCoreCableColor.Models.Pais;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using OrionCoreCableColor.Models.Region;
using OrionCoreCableColor.Models.Ciudad;
using OrionCoreCableColor.Models.CI_Configuracion;
using OrionCoreCableColor.Models.Servicios;

namespace OrionCoreCableColor.Models.Ticket
{
    public class ReporteRCAViewModel
    {
        public DateTime fdFechaAlarmaDeteccion { get; set; }
        public string fcDescripcionRequerimiento { get; set; }
        public string fjServiciosAfectados { get; set; }
        public string fjCISAfectados { get; set; }
        public string fjPaisesAFectados { get; set; }
        public string fjRegionesAfectadas { get; set; }
        public string fjCiudadesAfectadas { get; set; }
        public int fiIDRequerimiento { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public DateTime fdFechaReporte { get; set; }

        public DateTime fdFechadeCierre { get; set; }
        public string fcCategoriaResolucion { get; set; }
        public string fcSubCategoriaResolucion { get; set; }
        public string fcComentarioResolucion { get; set; }
        public DateTime fdfechaResolucion { get; set; }
        public string fcCausadelaFalla { get; set; }
        public string fcTiempoTotalAfectacion { get; set; }

        public List<PaisViewModel> flpaises
        {
            get => string.IsNullOrEmpty(fjPaisesAFectados)? null :  JsonConvert.DeserializeObject<List<PaisViewModel>>(fjPaisesAFectados); 
        }
        public List<RegionViewModel> flRegiones
        {
            get => string.IsNullOrEmpty(fjRegionesAfectadas) ? null : JsonConvert.DeserializeObject<List<RegionViewModel>>(fjRegionesAfectadas);
        }
        public List<CiudadViewModel> flCiudades
        {
            get => string.IsNullOrEmpty(fjCiudadesAfectadas) ? null : JsonConvert.DeserializeObject<List<CiudadViewModel>>(fjCiudadesAfectadas);
        }
        public List<CI_ConfiguracionViewModel> flCISAfectados
        {
            get => string.IsNullOrEmpty(fjCISAfectados) ? null : JsonConvert.DeserializeObject<List<CI_ConfiguracionViewModel>>(fjCISAfectados);
        }
        public List<ServiciosAfectadosViewModel> flServiciosAfectados
        {
            get => string.IsNullOrEmpty(fjServiciosAfectados) ? null : JsonConvert.DeserializeObject<List<ServiciosAfectadosViewModel>>(fjServiciosAfectados);
        }

        public List<Estado_RequerimientoViewModal> flBitacoraIncidente { get; set; }



    }
}