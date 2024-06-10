using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class IndicadoresController : BaseController
    {
        // GET: Indicadores
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
                    var jsonResult = Json(context.sp_Indicadores_Lista().Select(x => new ListaIndicadoresViewModel
                    {
                        fiIDTipoRequerimiento = x.fiIDTipoRequerimiento,
                        fcTipoRequerimiento = x.fcTipoRequerimiento,
                        fcToken = x.fcToken,
                        fcDescripcionCategoria = x.fcDescripcionCategoria



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
            
                return PartialView(new IndicadoresCrearViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(IndicadoresCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Indicadores_Insertar(model.fcTipoRequerimiento.Trim(), model.fiIDCategoriaDesarrollo,1);
                var result = newModel.FirstOrDefault() > 0;


                return EnviarResultado(true, "Crear Incidencia", result ? "Se Creó Satisfactoriamente" : "Error al eliminar");

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var indicador = context.sp_Indicadores_Lista().FirstOrDefault(x => x.fiIDTipoRequerimiento == id);
                if (indicador != null)
                {
                    ViewBag.ListaCategorias = context.sp_Categorias_Indicidencias_Listado().Where(a => a.fiEstado == 1).ToList().Select(x => new SelectListItem { Value = x.fiIDCategoriaDesarrollo.ToString(), Text = x.fcDescripcionCategoria }).ToList();

                    return PartialView("Crear", new IndicadoresCrearViewModel { fiIDTipoRequerimiento = indicador.fiIDTipoRequerimiento, fcTipoRequerimiento = indicador.fcTipoRequerimiento.Trim(), EsEditar = true });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Editar(IndicadoresCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var indicador = context.sp_Indicadores_Lista().FirstOrDefault(x => x.fcTipoRequerimiento == model.fcTipoRequerimiento && x.fiIDTipoRequerimiento != model.fiIDTipoRequerimiento);


                var newModel = context.sp_Indicadores_Editar(model.fiIDTipoRequerimiento, model.fcTipoRequerimiento.Trim(), model.fiIDCategoriaDesarrollo, 1);
                var result = newModel.FirstOrDefault() > 0;

                return EnviarResultado(result, "Editar Incidencia", result ? "Se Editó Satisfactoriamente" : "Error al eliminar");

            }

        }

        public ActionResult EliminarIndicador(int id)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Indicadores_Desactivar(id);
                var result = newModel.FirstOrDefault() > 0;
                
                return EnviarResultado(result, "Eliminar Incidencia", result ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");

            }
        }

    }
}