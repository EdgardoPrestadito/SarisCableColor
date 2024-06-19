using Microsoft.AspNet.Identity.Owin;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.DbConnection.CMContext;
using OrionCoreCableColor.Models.Gerencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class GerenciasController : BaseController
    {
        // GET: Gerencias
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
                    var jsonResult = Json(context.sp_Requerimientos_Catalogo_Generencias_Listado().Select(x => new GerenciasViewModel
                    {
                        fiIDGerencia = x.fiIDGerencia,
                        fcNombreGenerencia = x.fcNombreGenerencia,
                        fcNombreCorto = x.fcNombreCorto,
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
                ViewBag.ListaUsuarios = contextSaris.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto} - {x.fcPuesto}" }).ToList();

                return PartialView(new GerenciasViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(GerenciasViewModel model)
        {
            using (var context = new SARISEntities1())
            {

                var result = context.sp_Requerimientos_Catalogo_Generencias_Insertar(model.fcNombreGenerencia.Trim(), model.fiIDUsuarioResponsable).FirstOrDefault();

                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Gerencia", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Gerencia", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Gerencia", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Gerencia", "Error al Editar");
                }
            }


        }


        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var Gerencia = context.sp_Requerimientos_Catalogo_Generencias_Listado().FirstOrDefault(x => x.fiIDGerencia == id);
                ViewBag.ListaUsuarios = context.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem
                {
                    Value = x.fiIDUsuario.ToString(),
                    Text = $"{x.fcNombreCorto} - {x.fcPuesto}",
                    Selected = x.fiIDUsuario == Gerencia.fiIDUsuarioResponsable
                }).ToList();


                return PartialView("Crear", new GerenciasViewModel { fiIDGerencia = Gerencia.fiIDGerencia, fcNombreGenerencia = Gerencia.fcNombreGenerencia, EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(GerenciasViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Requerimientos_Catalogo_Generencias_Editar(model.fiIDGerencia, model.fcNombreGenerencia.Trim(), model.fiIDUsuarioResponsable).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Gerencia", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Gerencia", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Gerencia", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Gerencia", "Error al Editar");
                }
            }

        }

        public ActionResult EliminarGerencia(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Requerimientos_Catalogo_Generencias_Eliminar(id).FirstOrDefault();
                var success = result > 0;

                return EnviarResultado(success, "Eliminar Gerencia", success ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");
            }
        }

    }
}