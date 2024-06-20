using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.CI_Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class CI_ConfiguracionController : BaseController
    {
        // GET: CI_Configuracion
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
                    var jsonResult = Json(context.sp_CI_Configuracion_Listado().Select(x => new CI_ConfiguracionViewModel
                    {
                        fiIDPais            =   x.fiIDPais,
                        fcPais              =   x.fcPais,
                        fiIDRegion          =   x.fiIDRegion,
                        fcRegion            =   x.fcRegion,
                        fiIDCiudad          =   x.fiIDCiudad,
                        fcCiudad            =   x.fcCiudad,
                        fiIDConfiguracion   =   x.fiIDConfiguracion,
                        fcCI                =   x.fcCI,
                        fiIDCI              =   x.fiIDCI,
                        fcConfiguracion     =   x.fcConfiguracion,
                        fcLatitud           =   x.fcLatitud,
                        fcLongitud          =   x.fcLongitud
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
                ViewBag.ListaPaises = context.sp_Pais_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDPais.ToString(), Text = $"{x.fcPais}" }).ToList();
                ViewBag.ListaRegiones = context.sp_Region_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDRegion.ToString(), Text = $"{x.fcRegion}" }).ToList();
                ViewBag.ListaCI = context.sp_Clacificacion_CI_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDCI.ToString(), Text = $"{x.fcCI}" }).ToList();

                return PartialView(new CI_ConfiguracionViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(CI_ConfiguracionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_CI_Configuracion_Insertar(model.fiIDPais, model.fiIDRegion, model.fiIDCiudad, model.fiIDCI, model.fcConfiguracion, model.fcLatitud, model.fcLongitud).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear CI_Configuracion", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear CI_Configuracion", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear CI_Configuracion", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear CI_Configuracion", "Error al Editar");
                }

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_CI_Configuracion_Listado().FirstOrDefault(x => x.fiIDConfiguracion == id);
                ViewBag.ListaPaises = context.sp_Pais_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDPais.ToString(), Text = $"{x.fcPais}" }).ToList();
                ViewBag.ListaRegiones = context.sp_Region_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDRegion.ToString(), Text = $"{x.fcRegion}" }).ToList();
                ViewBag.ListaCI = context.sp_Clacificacion_CI_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDCI.ToString(), Text = $"{x.fcCI}" }).ToList();

                return PartialView("Crear", new CI_ConfiguracionViewModel {fiIDConfiguracion = model.fiIDConfiguracion, fiIDPais = model.fiIDPais, fiIDRegion = model.fiIDRegion, fiIDCiudad = model.fiIDCiudad, fiIDCI = model.fiIDCI, fcConfiguracion = model.fcConfiguracion, fcLatitud = model.fcLatitud, fcLongitud = model.fcLongitud, EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(CI_ConfiguracionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_CI_Configuracion_Editar(model.fiIDPais, model.fiIDRegion, model.fiIDCiudad, model.fiIDCI,model.fiIDConfiguracion , model.fcConfiguracion, model.fcLatitud, model.fcLongitud).FirstOrDefault();

                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar CI_Configuracion", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar CI_Configuracion", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar CI_Configuracion", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar CI_Configuracion", "Error al Editar");
                }
            }
        }
    }
}