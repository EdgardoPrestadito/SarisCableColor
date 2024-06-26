using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    [Authorize(Roles = "Acceso_Al_Sistema")]
    public class RegionController : BaseController  
    {
        // GET: Region
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
                    var jsonResult = Json(context.sp_Region_Listado().Select(x => new RegionViewModel
                    {
                        fiIDRegion = x.fiIDRegion,
                        fcRegion = x.fcRegion,
                        fIIDPais = x.fIIDPais,
                        fcPais = x.fcPais

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


                return PartialView(new RegionViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(RegionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Region_Insertar(model.fIIDPais, model.fcRegion.Trim()).FirstOrDefault();
                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Crear Region", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Crear Region", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Crear Region", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Crear Region", "Error al Editar");
                }

            }

        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {
                var model = context.sp_Region_Listado().FirstOrDefault(x => x.fiIDRegion == id);
                ViewBag.ListaPaises = context.sp_Pais_Listado().ToList().Select(x => new SelectListItem { Value = x.fiIDPais.ToString(), Text = $"{x.fcPais}" }).ToList();

                return PartialView("Crear", new RegionViewModel { fiIDRegion = model.fiIDRegion, fcRegion = model.fcRegion, fIIDPais = model.fIIDPais ,EsEditar = true });

            }
        }

        [HttpPost]
        public ActionResult Editar(RegionViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Region_Editar(model.fiIDRegion, model.fIIDPais, model.fcRegion.Trim()).FirstOrDefault();

                switch (result.fiRequest)
                {
                    case 0:
                        return EnviarResultado(false, "Editar Region", "Error al Editar");
                    case 1:
                        return EnviarResultado(true, "Editar Region", result.fcRequest);
                    case 2:
                        return EnviarResultado(false, "Editar Region", result.fcRequest);
                    default:
                        return EnviarResultado(false, "Editar Region", "Error al Editar");
                }
            }

        }
    }
}