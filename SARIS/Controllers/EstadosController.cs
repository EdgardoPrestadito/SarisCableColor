using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Estados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class EstadosController : BaseController
    {
        // GET: Estados
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult CargarListaEstados()
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var jsonResult = Json(context.sp_Estados_Lista().Select(x => new ListaEstadosViewModel
                    {
                        fiIDEstado = x.fiIDEstado,
                        fcDescripcionEstado = x.fcDescripcionEstado,
                        fcClaseColor = x.fcClaseColor,
                    

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

                ViewBag.Clase = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "success",
                        Text = "success"
                    },
                    new SelectListItem
                    {
                        Value = "primary",
                        Text = "primary"
                    },
                    new SelectListItem
                    {
                        Value = "warning",
                        Text = "warning"
                    },
                    new SelectListItem
                    {
                        Value = "danger",
                        Text = "danger"
                    },
                    new SelectListItem
                    {
                        Value = "info",
                        Text = "info"
                    },
                    new SelectListItem
                    {
                        Value = "secondary",
                        Text = "secondary"
                    },
                };
                return PartialView(new EstadosCrearViewModel());
            }
        }



        [HttpPost]
        public ActionResult Crear(EstadosCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Estados_Insertar(model.fcDescripcionEstado, model.fcClaseColor);

                return EnviarResultado(true, "Crear Estado", "Se Creó Satisfactoriamente");

            }


        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var context = new SARISEntities1())
            {

                ViewBag.Clase = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "success",
                        Text = "success"
                    },
                    new SelectListItem
                    {
                        Value = "primary",
                        Text = "primary"
                    },
                    new SelectListItem
                    {
                        Value = "warning",
                        Text = "warning"
                    },
                    new SelectListItem
                    {
                        Value = "danger",
                        Text = "danger"
                    },
                    new SelectListItem
                    {
                        Value = "info",
                        Text = "info"
                    },
                    new SelectListItem
                    {
                        Value = "secondary",
                        Text = "secondary"
                    },
                };
                var estado = context.sp_Estados_Lista().FirstOrDefault(x => x.fiIDEstado == id);
                if (estado != null)
                {
                    ViewBag.ListaUsuarios = context.sp_Usuarios_Maestro_Lista().ToList().Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcNombreCorto}  {x.fcPuesto}" }).ToList();

                    return PartialView("Crear", new EstadosCrearViewModel { fiIDEstado = estado.fiIDEstado, fcDescripcionEstado = estado.fcDescripcionEstado, fcClaseColor= estado.fcClaseColor, EsEditar = true });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Editar(EstadosCrearViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var newModel = context.sp_Estados_Editar (model.fiIDEstado, model.fcDescripcionEstado, model.fcClaseColor);
                var result = context.SaveChanges() > 0;

                return EnviarResultado(true, "Editar Estado", "Se edito estado satisfactoriamente");

            }

        }


        public ActionResult EliminarArea(int id)
        {
            using (var context = new SARISEntities1())
            {
                var area = context.sp_Areas_Desactivar(id);
                return EnviarResultado(true, "Eliminar Rol");
            }
        }

    }
}