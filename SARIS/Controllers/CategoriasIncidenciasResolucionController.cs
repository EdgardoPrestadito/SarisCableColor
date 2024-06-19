using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.CategoriaIncidencias;
using OrionCoreCableColor.Models.CategoriaIncidenciasResolucion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class CategoriasIncidenciasResolucionController : BaseController
    {
        // GET: CategoriasIncidenciasResolucion
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
                    var jsonResult = Json(context.sp_Categorias_IndicidenciasResolucion_Listado().Select(x => new CategoriasIncidenciasResolucionViewModel
                    {
                        fiIDCategoriaResolucion = x.fiIDCategoriaResolucion,
                        fcDescripcionCategoria = x.fcDescripcionCategoria,
                        fiEstado = x.fiEstado,
                        fcToken = x.fcToken,
                        fiIDArea = x.fiIDArea,
                        fcDescripcion = x.fcDescripcion

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
                ViewBag.Areas = context.sp_Areas_Lista().Where(x => x.fiActivo).ToList().Select(x => new SelectListItem { Value = x.fiIDArea.ToString(), Text = $"{x.fcDescripcion}" }).ToList();

                return PartialView(new CategoriasIncidenciasResolucionViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(CategoriasIncidenciasResolucionViewModel model)
        {
            using (var context = new SARISEntities1())
            {

                var result = context.sp_Categorias_IndicidenciasResolucion_Insertar(model.fcDescripcionCategoria.Trim(), 0).FirstOrDefault();


                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Categoria", "Error al Crear");
                    case 1:
                        return EnviarResultado(true, "Crear Categoria", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Categoria", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Categoria", "Error al Crear");
                }

            }


        }


        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var categoria = context.sp_Categorias_IndicidenciasResolucion_Listado().FirstOrDefault(x => x.fiIDCategoriaResolucion == id);
                if (categoria != null)
                {
                    ViewBag.Areas = context.sp_Areas_Lista().Where(x => x.fiActivo).ToList().Select(x => new SelectListItem { Value = x.fiIDArea.ToString(), Text = $"{x.fcDescripcion}" }).ToList();

                    return PartialView("Crear", new CategoriasIncidenciasResolucionViewModel { fiIDCategoriaResolucion = categoria.fiIDCategoriaResolucion, fcDescripcionCategoria = categoria.fcDescripcionCategoria, fiIDArea = categoria.fiIDArea, EsEditar = true });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Editar(CategoriasIncidenciasResolucionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Categorias_IndicidenciasResolucion_Editar(model.fiIDCategoriaResolucion, model.fcDescripcionCategoria.Trim(),0).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Categoria", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Categoria", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Categoria", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Categoria", "Error al Editar");
                }
            }
        }

        public ActionResult EliminarCategoria(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Categorias_Indicidencias_Desactivar(id).FirstOrDefault();
                var success = result.fiRequest == 1;

                return EnviarResultado(success, "Eliminar Categoria", success ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");
            }
        }
    }
}