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
    //[Authorize(Roles = "Acceso_Al_Sistema")]
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
                       fcNivel = x.fcNivel,
                       fiIDUsuarioResponsable = x.fiIDUsuarioResponsable,
                       fiAutomatico = x.fiAutomatico

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
                ViewBag.ListaGerencias = context.sp_Requerimientos_Catalogo_Generencias_Listado().Where(x => x.fiEstado).ToList().Select(x => new SelectListItem { Value = x.fiIDGerencia.ToString(), Text = $"{x.fcNombreGenerencia} - {x.fcNombreCorto}" }).ToList();
                ViewBag.ListaNiveles = context.sp_Requerimientos_Niveles_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDNivel.ToString(), Text = $"{x.fcNivel}" }).ToList();

                ViewBag.ListaUsuarios = context.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto} - {x.fcPuesto}"}).ToList();
                return PartialView(new AreasCrearViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(AreasCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Areas_Insertar(model.fcDescripcion.Trim(), model.fcCorreoElectronico.Trim(), model.fiIDUsuarioResponsable, model.fiIDGerencia, model.fiNivel).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Área", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Área", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                }
            }
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Areas_Lista().FirstOrDefault(x => x.fiIDArea == id);

                ViewBag.ListaGerencias = context.sp_Requerimientos_Catalogo_Generencias_Listado().Where(x => x.fiEstado).ToList().Select(x => new SelectListItem { Value = x.fiIDGerencia.ToString(), Text = $"{x.fcNombreGenerencia} - {x.fcNombreCorto}" }).ToList();
                ViewBag.ListaNiveles = context.sp_Requerimientos_Niveles_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDNivel.ToString(), Text = $"{x.fcNivel}" }).ToList();

                ViewBag.ListaUsuarios = context.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto}  {x.fcPuesto}" }).ToList();

                return PartialView("Crear", new AreasCrearViewModel { fiIDArea = model.fiIDArea, fcDescripcion = model.fcDescripcion.Trim(), fiIDGerencia = model.fiIDGerencia ?? 0 , fcCorreoElectronico = model.fcCorreoElectronico.Trim(), fcNombreCorto = model.fcNombreCorto.Trim(), fiIDUsuarioResponsable = model.fiIDUsuarioResponsable, fiNivel = model.fiNivel ?? 0 , EsEditar = true});
               
            }
        }

        [HttpPost]
        public ActionResult CambiarEstadoAuto(int Area, int Estatus)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_CambiarAutomaticoArea(Area, Estatus);
                if (result > 0) 
                {
                    return EnviarResultado(true, "Asignacion", "Se cambio la asignacion automatica");
                }
                return EnviarResultado(false, "Error", "Contacte al administrador");
            }
        }

        [HttpPost]
        public ActionResult Editar(AreasCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Areas_Editar(model.fiIDArea, model.fcDescripcion.Trim(), model.fcCorreoElectronico.Trim(), model.fiIDUsuarioResponsable, model.fiIDGerencia,model.fiNivel).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Área", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Área", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                }
            }
        }


        public ActionResult EliminarArea(int id)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Areas_Desactivar(id).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Área", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Área", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Área", "Error al Editar");
                }

              
            }
        }

    }
}