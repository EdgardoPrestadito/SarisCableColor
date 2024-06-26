using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Pais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    [Authorize(Roles = "Acceso_Al_Sistema")]
    public class PaisController : BaseController
    {
        // GET: Pais
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
                    var jsonResult = Json(context.sp_Pais_Listado().Select(x => new PaisViewModel
                    {
                        fiIDPais = x.fiIDPais,
                        fcPais = x.fcPais

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
                return PartialView(new PaisViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(PaisViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Pais_Insertar(model.fcPais.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Pais", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Pais", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Pais", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Pais", "Error al Editar");
                }

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Pais_Listado().FirstOrDefault(x => x.fiIDPais == id);

                return PartialView("Crear", new PaisViewModel { fiIDPais = model.fiIDPais, fcPais = model.fcPais, EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(PaisViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Pais_Editar(model.fiIDPais, model.fcPais.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Pais", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Pais", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Pais", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Pais", "Error al Editar");
                }
            }

        }
    }
}