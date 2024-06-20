using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Ciudad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class CiudadController : BaseController
    {
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
                    var jsonResult = Json(context.sp_Ciudad_Listado().Select(x => new CiudadViewModel
                    {
                        fcPais = x.fcPais,
                        fcRegion = x.fcRegion,
                        fIIDPais = x.fIIDPais,
                        fiIDRegion = x.fiIDRegion,
                        fiIDCiudad = x.fiIDCiudad,
                        fcCiudad = x.fcCiudad

                    }).ToList(), JsonRequestBehavior.AllowGet); ;
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

                return PartialView(new CiudadViewModel());
            }
        }



        [HttpPost]
        public ActionResult AgregarCiudad(CiudadViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Ciudad_Insertar(model.fIIDPais, model.fiIDRegion, model.fcCiudad.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Ciudad", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Ciudad", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Ciudad", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Ciudad", "Error al Editar");
                }

            }


        }

        [HttpPost]
        public ActionResult EditarCiudades(CiudadViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Ciudad_Editar(model.fIIDPais, model.fiIDRegion, model.fiIDCiudad, model.fcCiudad.Trim()).FirstOrDefault();

                switch (result.fiRequest)
                {
                    case 0:
                        var jsonResult = Json((false, "Editar Ciudad", "Error al Editar"), JsonRequestBehavior.AllowGet);
                        jsonResult.MaxJsonLength = Int32.MaxValue;
                        return jsonResult;
                    case 1:
                        var jsonResultt = Json((true, "Editar Ciudad", result.fcRequest), JsonRequestBehavior.AllowGet);
                        jsonResultt.MaxJsonLength = Int32.MaxValue;
                        return jsonResultt;
                    case 2:
                        var jsonResulttt = Json((false, "Editar Ciudad", result.fcRequest), JsonRequestBehavior.AllowGet);
                        jsonResulttt.MaxJsonLength = Int32.MaxValue;
                        return jsonResulttt;
                    default:
                        var jsonResultttt = Json((false, "Editar Ciudad", "Error al Editar"), JsonRequestBehavior.AllowGet);
                        jsonResultttt.MaxJsonLength = Int32.MaxValue;
                        return jsonResultttt;
                }
            }

        }
        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Ciudad_Listado().FirstOrDefault(x => x.fiIDCiudad == id);
                ViewBag.ListaPaises = context.sp_Pais_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDPais.ToString(), Text = $"{x.fcPais}" }).ToList();
                ViewBag.ListaRegiones = context.sp_Region_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDRegion.ToString(), Text = $"{x.fcRegion}" }).ToList();

                return PartialView("Crear", new CiudadViewModel { fiIDCiudad = model.fiIDCiudad, fcCiudad = model.fcCiudad, EsEditar = true });

            }
        }



    }
}