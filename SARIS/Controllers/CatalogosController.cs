using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Ciudad;
using OrionCoreCableColor.Models.Impacto;
using OrionCoreCableColor.Models.Prioridades;
using OrionCoreCableColor.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    [Authorize(Roles = "Acceso_Al_Sistema")]
    public class CatalogosController : BaseController
    {
        // GET: Catalogos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BandejaUrgencia()
        {
            return View(); 
        }

        public ActionResult BandejaPrioridades()
        {
            return View();
        }
        public JsonResult PrioridadesBandejaLista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_ListarPrioridad().Select(x => new PrioridadesViewModel
                    {
                        fiIDPrioridad = x.fiIDPrioridad,
                        fcDescripcionPrioridad = x.fcDescripcionPrioridad,
                        fiNivelPrioridad = x.fiNivelPrioridad,
                        fiTiempo = x.fiTiempo,
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
        public ActionResult CrearPropiedades()// mi dislexia no supo poner Prioridades y puso propiedades :p ATT: Edgardo Mancia
        {
            using (var contextSaris = new SARISEntities1())
            {
                return PartialView(new PrioridadesViewModel());
            }
        }

        public ActionResult ImpactoBandeja()
        {
            return View();
        }

        public JsonResult ImpactoBandejaLista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_ListarImpacto().Select(x => new ImpactoViewModel
                    {
                        fiIDImpacto = x.fiIDImpacto,
                        fcDescripcionImpacto = x.fcDescripcionImpacto,
                        fiAfectacion = x.fiAfectacion,
                        fiActivo = x.fiActivo,
                        fiNivel = x.fiNivel

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

        public ActionResult Crearinpacto()
        {
            return PartialView();
        }

        public ActionResult BandejaUsuariosVerArea()
        {
            return View();
        }

        public JsonResult UsuarioVerAreaLista()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_usuarioVerArea_Lista().Select(x => new UsuariopoAreasViewModel
                    {
                        fiIdUsuarioverArea = x.fiIdUsuarioverArea,
                        fcNombre = x.fcNombre,
                        fcAreas = x.fcAreas,
                        fcIdAreas = x.fcIdAreas


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

        public ActionResult AsignarAreasAUsuario()
        {
            using (var context = new SARISEntities1())
            {
                ViewBag.ListaAreas = context.sp_Areas_Lista().ToList();
                var tick = new UsuariopoAreasViewModel();
                tick.EsEditar = false;
                return PartialView(tick);
            }
        }

        public ActionResult AsignarAreaUsuarioPorUsuario(int idUsuario,int areacontratacion)
        {
            using(var context = new SARISEntities1())
            {
                ViewBag.idusuario = idUsuario;
                ViewBag.idareacontratacion = areacontratacion;
                ViewBag.ListaAreas = context.sp_Areas_Lista().Where(a => a.fiIDArea != areacontratacion).ToList();
                var areasasignadas = context.sp_usuarioVerArea_Lista_ByUsuario(idUsuario).FirstOrDefault().fcIdAreas ?? ""; var newareas = areasasignadas.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                ViewBag.ListaAreasAsignadas = newareas;
                return PartialView();
            }
        }

        public JsonResult EditarAreasaVerPorUsuario(int idusuario, string idareas)
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var result = context.sp_usuarioVerArea_Insertar(idusuario, idareas, GetIdUser()).FirstOrDefault();
                    switch (result.fiRequest)
                    {
                        case 0:
                            return EnviarResultado(false, "Areas Asignadas a ver", "Error al Editar");
                        case 1:
                            return EnviarResultado(true, "Areas Asignadas a ver", result.fcRequest);
                        case 2:
                            return EnviarResultado(true, "Areas Asignadas a ver", result.fcRequest);
                        default:
                            return EnviarResultado(false, "Areas Asignadas a ver", "Error al Editar");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}