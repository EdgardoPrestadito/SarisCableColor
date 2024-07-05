using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Ubicaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class UbicacionesController : BaseController
    {
        // GET: Ubicaciones
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
                    var jsonResult = Json(context.sp_Ubicacion_SubCategoria_Listado().Select(x => new UbicacionesViewModel
                    {
                        fiUbicacion = x.fiUbicacion,
                        fcDescripcionUbicacion = x.fcDescripcionUbicacion,
                        fcGeolocalización = x.fcGeolocalización,
                        fcDescripciondeUbicacion = x.fcDescripciondeUbicacion,
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
                return PartialView(new UbicacionesViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(UbicacionesViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Ubicacion_SubCategoria_Insertar(model.fcDescripcionUbicacion.Trim(), model.fcDescripciondeUbicacion.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Ubicacion", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Ubicacion", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Ubicacion", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Ubicacion", "Error al Editar");
                }
            }
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Ubicacion_SubCategoria_Listado().FirstOrDefault(x => x.fiUbicacion == id);

                return PartialView("Crear", new UbicacionesViewModel { fiUbicacion = model.fiUbicacion, fcDescripcionUbicacion = model.fcDescripcionUbicacion.Trim(), fcDescripciondeUbicacion = model.fcDescripciondeUbicacion,EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(UbicacionesViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Ubicacion_SubCategoria_Editar(model.fiUbicacion, model.fcDescripcionUbicacion, model.fcDescripciondeUbicacion.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Sub Categoria", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Sub Categoria", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Sub Categoria", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Sub Categoria", "Error al Editar");
                }
            }

        }

        public ActionResult EliminarIndicador(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Ubicacion_SubCategoria_Desactivar(id).FirstOrDefault();
                var success = result.fiRequest == 1;

                return EnviarResultado(success, "Eliminar Sub Categoria", success ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");

            }
        }
    }
}