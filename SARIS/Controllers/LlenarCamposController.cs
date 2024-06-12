using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
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

        public JsonResult SelectPlataforma()
        {
            using (var contexto = new SARISEntities1())
            {
                var jsonResult = Json(contexto.sp_Plataformas_Listado().ToList(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }


    }
}