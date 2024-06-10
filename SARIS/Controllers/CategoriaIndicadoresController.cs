using Microsoft.AspNet.Identity.Owin;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.DbConnection.CMContext;
using OrionCoreCableColor.Models.CategoriaIncidencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class CategoriaIndicadoresController : BaseController
    {
        // GET: CategoriaIndicadores
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
                    var jsonResult = Json(context.sp_Categorias_Indicidencias_Listado().Select(x => new ListaCategoriaIncidencias
                    {
                        fiIDCategoriaDesarrollo = x.fiIDCategoriaDesarrollo,
                        fcDescripcionCategoria = x.fcDescripcionCategoria,
                        fiEstado = x.fiEstado,
                        fcToken = x.fcToken,


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

            using (var contextSaris = new SARISEntities1())
            {
                return PartialView(new ListaCategoriaIncidencias());
            }
        }



        [HttpPost]
        public ActionResult Crear(ListaCategoriaIncidencias model)
        {
            using (var context = new SARISEntities1())
            {
              
                var result = context.sp_Categorias_Indicidencias_Insertar(model.fcDescripcionCategoria.Trim(), 1).FirstOrDefault();

                var success = result > 0;

                return EnviarResultado(success, "Crear Categoria", success ? "Se Creó Satisfactoriamente" : "Error al Crear ");

            }


        }


        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var categoria = context.sp_Categorias_Indicidencias_Listado().FirstOrDefault(x => x.fiIDCategoriaDesarrollo == id);
                if (categoria != null)
                {

                    return PartialView("Crear", new ListaCategoriaIncidencias {fiIDCategoriaDesarrollo = categoria.fiIDCategoriaDesarrollo , fcDescripcionCategoria = categoria.fcDescripcionCategoria, EsEditar = true });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Editar(ListaCategoriaIncidencias model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Categorias_Indicidencias_Editar(model.fiIDCategoriaDesarrollo, model.fcDescripcionCategoria.Trim(), 1).FirstOrDefault();
                
                var success = result > 0;

                return EnviarResultado(success, "Editar Categoria", success ? "Se Editó Satisfactoriamente" : "Error al editar ");


            }

        }

        public ActionResult EliminarCategoria(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Categorias_Indicidencias_Desactivar(id).FirstOrDefault();
                var success = result > 0;

                return EnviarResultado(success, "Eliminar Categoria", success ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");
            }
        }



    }
}