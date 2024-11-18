using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class LlenarCamposController : BaseController
    {
        // GET: LlenarCampos
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SelectAreas()
        {
            using (var contexto = new SARISEntities1())
            {
                var SelecAreas = contexto.sp_Areas_Lista().Select(x => new SelectListItem { Value = x.fiIDArea.ToString(), Text = x.fcDescripcion }).ToList();
                return EnviarListaJson(SelecAreas);
            }
        }

        public JsonResult SelectIncidencias()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Indicadores_Lista().Select(x => new ListaIndicadoresViewModel
                {
                    fiIDTipoRequerimiento = x.fiIDTipoRequerimiento,
                    fcTipoRequerimiento = x.fcTipoRequerimiento,
                    fcToken = x.fcToken


                }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectIncidenciasByCategorias(string fccategoria)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Indicadores_Lista().Where(a => a.fcDescripcionCategoria == fccategoria).Select(x => new ListaIndicadoresViewModel
                {
                    fiIDTipoRequerimiento = x.fiIDTipoRequerimiento,
                    fcTipoRequerimiento = x.fcTipoRequerimiento,
                    fcToken = x.fcToken


                }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


        public JsonResult SelectUsuarios(int idarea)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Usuarios_Maestro_PorArea(idarea).Select(x => new SelectListItem 
                { 
                    Value = x.fiIDUsuario.ToString(), 
                    Text = x.fcPrimerNombre + " " + x.fcPrimerApellido 
                }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult AutoSelectUsuarios(int idarea)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = contexto.sp_Requerimientos_UsuarioLibre(idarea).FirstOrDefault();
                
                return EnviarListaJson(jsonResult);
            }
        }

        public JsonResult SelectUsuariosAll()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Usuarios_Maestro_Lista().Select(x => new SelectListItem
                {
                    Value = x.fiIDUsuario.ToString(),
                    Text = x.fcPrimerNombre + " " + x.fcPrimerApellido
                }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectCategorias()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Categorias_Indicidencias_Listado().Where(a => a.fiEstado == 1).Select(x => new ListaIndicadoresViewModel
                {
                    fiIDTipoRequerimiento = x.fiIDCategoriaDesarrollo,
                    fcTipoRequerimiento = x.fcDescripcionCategoria,
                    fcToken = x.fcToken


                }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectUrgencia()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_ListarUrgencia().Where(a => a.fiActivo == 1).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectImpacto()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_ListarImpacto().Where(a => a.fiActivo == 1).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectPrioridad(int Urgencia, int Impacto)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Prioridad_urgenciaXimpacto(Urgencia, Impacto).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelecticketPadrese(int idticket)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_ListaTicket_Padre(idticket).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectServiciosAfectados() 
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_ServiciosAfectados_Listado().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


        public JsonResult SelectLlenarRegionPorPais(int idpais)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Region_Listado().Where(a => a.fIIDPais == idpais).Select(x => new SelectListItem { Value = x.fiIDRegion.ToString(), Text = $"{x.fcRegion}" }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectLlenarCiudadPorRegioin(int IdRegion)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Ciudad_Listado().Where(a => a.fiIDRegion == IdRegion).Select(x => new SelectListItem { Value = x.fiIDCiudad.ToString(), Text = $"{x.fcCiudad}" }).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectPlataforma()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Plataformas_Listado().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectMotivos(int idestado)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Motivos_Estado_Listado(idestado).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


        public JsonResult SelectTipoCI()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_ClacificacionCI().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectPais()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Pais().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }
        public JsonResult SelectRegion(int Pais)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Region(Pais).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SelectCiudad(int Pais, int Region)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Ciudad(Pais, Region).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


        [HttpGet]
        public JsonResult listadoCI(int Pais,int Region, int Ciudad, int CI)
        {

            try
            {
                using (var connection = new SARISEntities1())
                {
                    var cont = connection.sp_ConfiguracionCI(Pais, Region, Ciudad, CI).ToList();
                    return EnviarListaJson(cont);
                }
                
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public JsonResult SelectCategoriaResolucion()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Categorias_IndicidenciasResolucion_Listado().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }
        
        public JsonResult SelectSubCategoriaResolucion(int Categoria)
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_CatalogoSubCategoriaResolucion_porCategoriaResolucion(Categoria).ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        
    }
}