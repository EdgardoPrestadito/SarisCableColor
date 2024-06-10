using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Impacto;
using OrionCoreCableColor.Models.Prioridades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class CatalogosController : Controller
    {
        // GET: Catalogos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BandejaUrgencia()
        {
            return View(); 
        }

        public ActionResult BandejaPrioridades()
        {
            return View();
        }
        public JsonResult PrioridadesBandejaLista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_ListarPrioridad().Select(x => new PrioridadesViewModel
                    {
                        fiIDPrioridad = x.fiIDPrioridad,
                        fcDescripcionPrioridad = x.fcDescripcionPrioridad,
                        fiNivelPrioridad = x.fiNivelPrioridad,
                        fiTiempo = x.fiTiempo,
                        fiActivo = x.fiActivo 

                    }).ToList(), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;
                    return jsonResult;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public ActionResult CrearPropiedades()// mi dislexia no supo poner Prioridades y puso propiedades :p ATT: Edgardo Mancia
        {
            using (var contextSaris = new SARISEntities1())
            {
                return PartialView(new PrioridadesViewModel());
            }
        }

        public ActionResult ImpactoBandeja()
        {
            return View();
        }

        public JsonResult ImpactoBandejaLista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_ListarImpacto().Select(x => new ImpactoViewModel
                    {
                        fiIDImpacto = x.fiIDImpacto,
                        fcDescripcionImpacto = x.fcDescripcionImpacto,
                        fiAfectacion = x.fiAfectacion,
                        fiActivo = x.fiActivo,
                        fiNivel = x.fiNivel

                    }).ToList(), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;
                    return jsonResult;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult Crearinpacto()
        {
            return PartialView();
        }

    }
}