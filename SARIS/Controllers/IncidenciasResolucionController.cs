using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.IncidenciasResolucion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class IncidenciasResolucionController : BaseController
    {
        // GET: IncidenciasResolucion
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarListaIndicadores()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_IndicadoresResolucion_Lista().Select(x => new IncidenciasResolucionViewModel
                    {
                        fiIDTipoRequerimientoResolucion = x.fiIDTipoRequerimientoResolucion,
                        fcTipoRequerimiento = x.fcTipoRequerimiento,
                        fcToken = x.fcToken,
                        fiIDCategoriaResolucion = x.fiIDCategoriaResolucion,
                        fcDescripcionCategoria = x.fcDescripcionCategoria,
                        fiUbicacion = x.fiUbicacion,
                        fcDescripcionUbicacion = x.fcDescripcionUbicacion,


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
                ViewBag.ListaCategorias = context.sp_Categorias_Indicidencias_Listado().Where(a => a.fiEstado == 1).ToList().Select(x => new SelectListItem { Value = x.fiIDCategoriaDesarrollo.ToString(), Text = x.fcDescripcionCategoria }).ToList();
                ViewBag.Ubicaciones = context.sp_Ubicacion_SubCategoria_Listado().Where(a => a.fiActivo == 1).ToList().Select(x => new SelectListItem { Value = x.fiUbicacion.ToString(), Text = x.fcDescripcionUbicacion }).ToList();

                return PartialView(new IncidenciasResolucionViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(IncidenciasResolucionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_IndicadoresResolucion_Insertar(model.fcTipoRequerimiento.Trim(), model.fiIDCategoriaResolucion, model.fiUbicacion).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Sub Categoria", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Sub Categoria", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Sub Categoria", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Sub Categoria", "Error al Editar");
                }

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_IndicadoresResolucion_Lista().FirstOrDefault(x => x.fiIDTipoRequerimientoResolucion == id);
           
                    ViewBag.ListaCategorias = context.sp_Categorias_IndicidenciasResolucion_Listado().Where(a => a.fiEstado == 1).ToList().Select(x => new SelectListItem { Value = x.fiIDCategoriaResolucion.ToString(), Text = x.fcDescripcionCategoria + " | " + x.fcDescripcion }).ToList();
                    ViewBag.Ubicaciones = context.sp_Ubicacion_SubCategoria_Listado().Where(a => a.fiActivo == 1).ToList().Select(x => new SelectListItem { Value = x.fiUbicacion.ToString(), Text = x.fcDescripcionUbicacion}).ToList();

                    return PartialView("Crear", new IncidenciasResolucionViewModel { fiIDTipoRequerimientoResolucion = model.fiIDTipoRequerimientoResolucion, fcTipoRequerimiento = model.fcTipoRequerimiento.Trim(), fiIDCategoriaResolucion = model.fiIDCategoriaResolucion ?? 0 ,EsEditar = true });
            }
        }

        [HttpPost]
        public ActionResult Editar(IncidenciasResolucionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_IndicadoresResolucion_Editar(model.fiIDTipoRequerimientoResolucion, model.fcTipoRequerimiento.Trim(), model.fiIDCategoriaResolucion, model.fiUbicacion).FirstOrDefault();
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
                var result = context.sp_Indicadores_Desactivar(id).FirstOrDefault();
                var success = result.fiRequest == 1;

                return EnviarResultado(success, "Eliminar Sub Categoria", success ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");

            }
        }
    }
}