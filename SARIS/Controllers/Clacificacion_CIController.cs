using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Clacificacion_CI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class Clacificacion_CIController : BaseController
    {
        // GET: Clacificacion_CI
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Lista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_Clacificacion_CI_Listado().Select(x => new Clacificacion_CIViewModel
                    {
                        fcCI    =   x.fcCI,
                        fiIDCI  =   x.fiIDCI

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


        [HttpGet]
        public ActionResult Crear()
        {
            using (var context = new SARISEntities1())
            {
                return PartialView(new Clacificacion_CIViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(Clacificacion_CIViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Clacificacion_CI_Insertar(model.fcCI.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Clasificacion CI", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Clasificacion CI", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Clasificacion CI", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Clasificacion CI", "Error al Editar");
                }

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Clacificacion_CI_Listado().FirstOrDefault(x => x.fiIDCI == id);

                return PartialView("Crear", new Clacificacion_CIViewModel { fiIDCI = model.fiIDCI, fcCI = model.fcCI, EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(Clacificacion_CIViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Clacificacion_CI_Editar(model.fiIDCI, model.fcCI.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Clasificacion CI", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Clasificacion CI", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Clasificacion CI", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Clasificacion CI", "Error al Editar");
                }
            }

        }
    }
}