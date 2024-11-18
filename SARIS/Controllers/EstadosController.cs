using OrionCoreCableColor.App_Helper;
using OrionCoreCableColor.App_Services.EmailService;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.EmailTemplateService;
using OrionCoreCableColor.Models.Estados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public ActionResult CambiarEstatus()
        {
            using (var context = new SARISEntities1())
            {
                var actividad = context.sp_UsuarioMaestro_VerActividad(GetIdUser()).FirstOrDefault();
                if (actividad.fiActivoTrabajo == 0)
                {
                    var newModel = context.sp_UsuarioMaestro_CambiarActividad(GetIdUser(), 1);
                    return EnviarResultado(true, "Estatus", "Abierto");
                }
                else 
                {
                    var newModel = context.sp_UsuarioMaestro_CambiarActividad(GetIdUser(), 0);
                    return EnviarResultado(false, "Estatus", "Fuera de Linea");
                }
                

                

            }


        }

        [HttpGet]
        public ActionResult verEstatus()
        {
            using (var context = new SARISEntities1())
            {
                var actividad = context.sp_UsuarioMaestro_VerActividad(GetIdUser()).FirstOrDefault();
                if (actividad.fiActivoTrabajo == 0)
                {
                    
                    return EnviarResultado(false, "Estatus", "Abierto");
                }
                else
                {
                    
                    return EnviarResultado(true, "Estatus", "Fuera de Linea");
                }




            }


        }

        public async Task<JsonResult> ActualizarUsuario()
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    var TicketPendientes = contexto.sp_Requerimientos_Pendientes_Usuario(GetIdUser()).ToList();
                    for (int i = 0; i < TicketPendientes.Count(); i++)
                    {
                        int idticket = (int)TicketPendientes[i];
                        var datosticket = Datosticket(idticket);
                        var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, 3, DateTime.Now, 0, 0, datosticket.fiTipoRequerimiento, 1, datosticket.fiAreaAsignada, datosticket.fiIDRequerimientoPadre, 0, 0, 0);
                        string pcCorreoGerenciaSolicitante = ""
                                  , pcNumeroGerenciaSolicitante = ""
                                  , pcCorreosUsuarioSolicitante = ""
                                  , pcNumerosUsuarioSolicitante = ""
                                  , pcCorreosJefesSolicitante = ""
                                  , pcNumerosJefesSolicitante = ""
                                  , pcCorreosSupervisorSolicitante = ""
                                  , pcNumerosSupervisorSolicitante = ""
                                  , pcCorreoAreaSolicitante = ""
                                  , pcCorreoGerenciaAsignada = ""
                                  , pcNumeroGerenciaAsignada = ""
                                  , pcCorreoUsuarioAsignado = ""
                                  , pcNumeroUsuarioAsignado = ""
                                  , pcCorreosJefesAsignada = ""
                                  , pcNumerosJefesAsignada = ""
                                  , pcCorreosSupervisoresAsignado = ""
                                  , pcNumerosSupervisorAsignado = ""
                                  , pcCorreoAreaAsignada = "";
                        //saber el string del nombre del usuario
                        datosticket = Datosticket(idticket);
                        var UsuarioAsignado = contexto.sp_Usuarios_Maestro_PorIdUsuario(datosticket.fiIDUsuarioAsignado).FirstOrDefault(); // buscar el area a la cual se le asigno
                        var usuarioLogueado = contexto.sp_Usuarios_Maestro_PorIdUsuario(GetIdUser()).FirstOrDefault();

                        //contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                        //guardar la bitacora 
                        GuardarBitacoraGeneralhistorial(GetIdUser(), idticket, datosticket.fiIDUsuarioSolicitante, $"El Usuario: {usuarioLogueado.fcNombreCorto} Asigno al Usuario {UsuarioAsignado.fcNombreCorto} por: Indiente Reasignado por Salida de usuario", 1, 7, datosticket.fiIDUsuarioAsignado, (int)datosticket.fiAreaAsignada);//el estado de ticket esta en 7 para que pueda guardar la bitacora


                        //ObtenerDataTicket(idticket);//aqui esta el signalR
                        if (GetIdUser() != datosticket.fiIDUsuarioAsignado) //aqui el signalR por si al reasignar un usuario se le quite de la bandeja de el 
                        {
                            //eliminarTicketAbierto(datosticket.fiIDRequerimiento);SignalR
                        }

                        var correo = contexto.sp_DatosTicket_Correo(datosticket.fiIDRequerimiento).FirstOrDefault();
                        var _emailTemplateService = new EmailTemplateService();
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = correo.fiIDRequerimiento,
                            fcTituloRequerimiento = correo.fcTituloRequerimiento,
                            fcDescripcionRequerimiento = correo.fcDescripcionRequerimiento,
                            fdFechaCreacion = correo.fdFechaCreacion,
                            fiIDAreaSolicitante = (int)correo.fiIDAreaSolicitante,
                            fcAreaSolicitante = correo.fcAreaSolicitante,
                            fiIDUsuarioSolicitante = correo.fiIDUsuarioSolicitante,
                            fcNombreCorto = correo.fcNombreCorto,
                            fiIDEstadoRequerimiento = correo.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = correo.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoElectronico,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = "Indiente Reasignado por Salida de usuario"
                        });
                        pcCorreoUsuarioAsignado = correo.fcCorreoElectronico;

                        //await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        //{
                        //    fiIDRequerimiento = correo.fiIDRequerimiento,
                        //    fcTituloRequerimiento = correo.fcTituloRequerimiento,
                        //    fcDescripcionRequerimiento = correo.fcDescripcionRequerimiento,
                        //    fdFechaCreacion = correo.fdFechaCreacion,
                        //    fiIDAreaSolicitante = correo.fiIDAreaSolicitante,
                        //    fcAreaSolicitante = correo.fcAreaSolicitante,
                        //    fiIDUsuarioSolicitante = correo.fiIDUsuarioSolicitante,
                        //    fcNombreCorto = correo.fcNombreCorto,
                        //    fiIDEstadoRequerimiento = correo.fiIDEstadoRequerimiento,
                        //    fcDescripcionEstado = correo.fcDescripcionEstado,
                        //    fcCorreoElectronico = correo.fcCorreoUsuarioSolicitante,
                        //    fcDescripcionCategoria = correo.fcDescripcionCategoria,
                        //    fcTipoRequerimiento = correo.fcTipoRequerimiento,
                        //    fiIDUrgencia = (int)correo.fiIDUrgencia,
                        //    fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                        //    fiIDImpacto = (int)correo.fiIDImpacto,
                        //    fcDescripcionImpacto = correo.fcDescripcionImpacto,
                        //    fiIDPrioridad = (int)correo.fiIDPrioridad,
                        //    fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                        //    fcComentario = comenta
                        //});
                        MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoMovil, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, "Indiente Reasignado por Salida de usuario", correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                        //MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoSolicitante, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comenta, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion);
                        pcNumeroUsuarioAsignado = correo.fcTelefonoMovil;
                        using (var connection = (new SARISEntities1()).Database.Connection)
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                                    $"EXEC sp_LogCorreosNumeros_Crear {datosticket.fiIDRequerimiento},{3},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                            using (var reader = command.ExecuteReader())
                            {

                            }

                            connection.Close();


                        }
                    }
                    

                    return EnviarResultado(true, "", "Ticket Usuario Actualizado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo Actualizar el usuario");
                throw;
            }
        }

        public sp_Requerimientos_Bandeja_ByID_Result Datosticket(int idticket)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    var datosticket = contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                    return datosticket;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult GuardarBitacoraGeneralhistorial(int idusuario, int idticket, int idusuariosolicitante, string comentario, int idapp, int idestado, int idusuarioasignado, int idarea)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    var result = contexto.sp_Requerimiento_Bitacoras_Agregar(GetIdUser(), idticket, idusuariosolicitante, comentario, idapp, idestado, idusuarioasignado, idarea);

                    return EnviarListaJson(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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