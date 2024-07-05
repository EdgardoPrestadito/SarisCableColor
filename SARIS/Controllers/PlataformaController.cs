using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Plataformas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class PlataformaController : BaseController
    {
        // GET: Plataforma
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
                    var jsonResult = Json(context.sp_Plataformas_Listado().Select(x => new PlataformaViewModel
                    {
                        fiIDPlataforma = x.fiIDPlataforma,
                        fcNombrePlataforma = x.fcNombrePlataforma,
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


        [HttpGet]
        public ActionResult Crear()
        {
            using (var context = new SARISEntities1())
            {
                return PartialView(new PlataformaViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(PlataformaViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Plataformas_Insertar(model.fcNombrePlataforma.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Plataforma", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Plataforma", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Plataforma", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Plataforma", "Error al Editar");
                }

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Plataformas_Listado().FirstOrDefault(x => x.fiIDPlataforma == id);

                return PartialView("Crear", new PlataformaViewModel { fiIDPlataforma = model.fiIDPlataforma, fcNombrePlataforma = model.fcNombrePlataforma, EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(PlataformaViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Plataformas_Editar(model.fcNombrePlataforma.Trim(), model.fiIDPlataforma).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Plataforma", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Plataforma", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Plataforma", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Plataforma", "Error al Editar");
                }
            }

        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Plataformas_Eliminar(id).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Eliminar Plataforma", "Error al Eliminar");
                    case 1:
                        return EnviarResultado(true, "Eliminar Plataforma", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Eliminar Plataforma", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Eliminar Plataforma", "Error al Eliminar");
                }
            }

        }
    }
}