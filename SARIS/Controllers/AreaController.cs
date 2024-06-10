using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using OrionCoreCableColor.DbConnection.CMContext;
using OrionCoreCableColor.Models;
using OrionCoreCableColor.Models.Usuario;
using System.Data.Entity;
using System.Threading.Tasks;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Areas;

namespace OrionCoreCableColor.Controllers
{
    public class AreaController : BaseController
    {
        // GET: Area
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult CargarListaAreas()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_Areas_Lista().Select(x => new ListaAreasViewModel
                    {
                       fiIDArea = x.fiIDArea,
                       fcDescripcion  = x.fcDescripcion,
                       fcCorreoElectronico = x.fcCorreoElectronico,
                       fcNombreCorto = x.fcNombreCorto,
                       fiActivo = x.fiActivo,
                       fcNombreGenerencia = x.fcNombreGenerencia,
                       fiIDUsuarioResponsable = x.fiIDUsuarioResponsable


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
                ViewBag.ListaGerencias = contextSaris.sp_Requerimientos_Catalogo_Generencias_Listado().Where(x => x.fiEstado).ToList().Select(x => new SelectListItem { Value = x.fiIDGerencia.ToString(), Text = $"{x.fcNombreGenerencia} - {x.fcNombreCorto}" }).ToList();

                ViewBag.ListaUsuarios = contextSaris.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto} - {x.fcPuesto}"}).ToList();
                return PartialView(new AreasCrearViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(AreasCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Areas_Insertar(model.fcDescripcion.Trim(), model.fcCorreoElectronico.Trim(), model.fiIDUsuarioResponsable, model.fiIDGerencia);
                var result = newModel.FirstOrDefault();
                var success = result > 0;

                return EnviarResultado(success, "Crear Rol", success ? "Se Creo Satisfactoriamente" : "Error al Crear");
            }
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var area = context.sp_Areas_Lista().FirstOrDefault(x => x.fiIDArea == id);

                ViewBag.ListaGerencias = context.sp_Requerimientos_Catalogo_Generencias_Listado().Where(x => x.fiEstado).ToList().Select(x => new SelectListItem { Value = x.fiIDGerencia.ToString(), Text = $"{x.fcNombreGenerencia} - {x.fcNombreCorto}" }).ToList();

                ViewBag.ListaUsuarios = context.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto}  {x.fcPuesto}" }).ToList();

                return PartialView("Crear", new AreasCrearViewModel { fiIDArea = area.fiIDArea, fcDescripcion = area.fcDescripcion.Trim(), fiIDGerencia = area.fiIDGerencia?? 0 , fcCorreoElectronico = area.fcCorreoElectronico.Trim(), fcNombreCorto = area.fcNombreCorto.Trim(), fiIDUsuarioResponsable = area.fiIDUsuarioResponsable, EsEditar = true});
               
            }
        }

        [HttpPost]
        public ActionResult Editar(AreasCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Areas_Editar(model.fiIDArea, model.fcDescripcion.Trim(), model.fcCorreoElectronico.Trim(), model.fiIDUsuarioResponsable, model.fiIDGerencia);
                var result = newModel.FirstOrDefault();
                var success = result > 0;

                return EnviarResultado(success, "Editar Área", success ? "Se Edito Satisfactoriamente" : "Error al Editar");
            }
        }


        public ActionResult EliminarArea(int id)
        {
            using (var context = new SARISEntities1())
            {
                var area = context.sp_Areas_Desactivar(id).FirstOrDefault();
                var result = area > 0 ;
               
                return EnviarResultado(result, "Eliminar Área", result ? "Se Eliminó Satisfactoriamente" : "Error al eliminar");
            }
        }

    }
}