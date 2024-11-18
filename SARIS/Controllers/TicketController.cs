using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.Ticket;
using System.IO;
using OrionCoreCableColor.App_Helper;
using System.Threading.Tasks;
using OrionCoreCableColor.App_Services.EmailService;
using OrionCoreCableColor.Models.EmailTemplateService;
using OrionCoreCableColor.Models.Indicadores;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class TicketController : BaseController
    {
        // GET: Ticket
        //[Authorize(Roles = "Acceso_Al_Sistema")]
        public ActionResult Index()
        {
            ViewBag.idUsuario = GetIdUser();
            return View();
        }

        public ActionResult BandejaTicketCerradosCancelados()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarTicket()
        {
            var listaEquifaxGarantia = new List<TicketMiewModel>();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_Requerimientos_Bandeja {1},{1},{GetIdUser()}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        listaEquifaxGarantia = db.ObjectContext.Translate<TicketMiewModel>(reader).ToList();
                    }
                    connection.Close();
                    var usuariologueado = GetUser();
                    if (usuariologueado.IdRol == 1 || usuariologueado.IdRol == 2) // ponerlos en una configuracion para evitar poner datos en duro att Edgardo mancia 2024-06-25
                    {
                        return EnviarListaJson(listaEquifaxGarantia);
                    }
                    else
                    {
                        using (var conexion = new SARISEntities1())
                        {
                            var IDuser = GetIdUser();
                            var areas = conexion.sp_usuarioVerArea_Lista_ByUsuario(IDuser).FirstOrDefault().fcIdAreas ?? ""; 
                            var newareas = areas.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                            var listareas = listaEquifaxGarantia.Where(x => newareas.Any(y => y == x.fiAreaAsignada) || x.fiIDUsuarioSolicitante == IDuser).ToList();
                            return EnviarListaJson(listareas);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        [HttpGet]
        public JsonResult ListarTicketCerrados()
        {
            var listaEquifaxGarantia = new List<TicketMiewModel>();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    var usuario = GetUser();
                    command.CommandText = $"EXEC sp_ListadoTicket_CerradosCancelado {usuario.fiIdUsuario},{usuario.IdRol}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        listaEquifaxGarantia = db.ObjectContext.Translate<TicketMiewModel>(reader).ToList();

                    }
                    connection.Close();

                    return EnviarListaJson(listaEquifaxGarantia.Where(a => a.fiIDEstadoRequerimiento == 5));
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        [HttpGet]
        public JsonResult ListarTicketCancelados()
        {
            var listaEquifaxGarantia = new List<TicketMiewModel>();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    var usuario = GetUser();
                    command.CommandText = $"EXEC sp_ListadoTicket_CerradosCancelado {usuario.fiIdUsuario},{usuario.IdRol}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        listaEquifaxGarantia = db.ObjectContext.Translate<TicketMiewModel>(reader).ToList();

                    }

                    connection.Close();
                    
                    return EnviarListaJson(listaEquifaxGarantia.Where(a => a.fiIDEstadoRequerimiento == 6));
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        [HttpGet]
        public ActionResult ModalBitacora(int id)
        {
            return PartialView(id);

        }

        public ActionResult ModalBitacoraMejora(int id)
        {
            using (var connection = new SARISEntities1())
            {
                ViewBag.Documentos = connection.sp_Requerimiento_Documentos_ObtenerPorIdRequerimiento(id, 1, 1, 1).ToList();
                return PartialView(id);
            }


        }

        [HttpGet]
        public JsonResult BitacoraEstado(int Ticket)
        {
            var listaEquifaxGarantia = new List<Estado_RequerimientoViewModal>();

            try
            {
                using (var connection = new SARISEntities1())
                {
                    var cont = connection.sp_Requerimientos_Bitacoras_Historial_ByID(Ticket).ToList();

                    return EnviarListaJson(cont);
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public ActionResult ActualizarTicket(int idticket)
        {
            using (var contexto = new SARISEntities1())
            {
                try
                {
                    var cont = contexto.sp_Requerimiento_Maestro_Detalle(1, 1, GetIdUser(), idticket).FirstOrDefault();
                    var tick = new TicketMiewModel();
                    tick.fcDescripcionRequerimiento = cont.fcDescripcionRequerimiento;
                    tick.fiIDRequerimiento = cont.fiIDRequerimiento;
                    tick.fdFechaCreacion = cont.fdFechaCreacion;
                    tick.fcTituloRequerimiento = cont.fcTituloRequerimiento;
                    tick.fiIDAreaSolicitante = cont.fiIDAreaSolicitante;
                    tick.fiIDEstadoRequerimiento = cont.fiIDEstadoRequerimiento == Convert.ToByte(7) ? Convert.ToByte(3) : cont.fiIDEstadoRequerimiento;
                    tick.fiIDUsuarioAsignado = cont.fiIDUsuarioAsignado;
                    tick.fdFechaAsignacion = cont.fdFechaAsignacion;
                    tick.fdFechadeCierre = cont.fdFechadeCierre;
                    tick.fiTipoRequerimiento = (int)cont.fiTipoRequerimiento;
                    tick.fiCategoriadeDesarrollo = (int)cont.fiCategoriadeDesarrollo;
                    tick.fiIDUrgencia = (int)cont.fiIDUrgencia;
                    tick.fcDescripcionUrgencia = cont.fcDescripcionUrgencia;
                    tick.fiIDImpacto = (int)cont.fiIDImpacto;
                    tick.fcDescripcionImpacto = cont.fcDescripcionImpacto;
                    tick.fiIDPrioridad = (int)cont.fiIDPrioridad;
                    tick.fcDescripcionPrioridad = cont.fcDescripcionPrioridad;
                    tick.fiIdTicketPadre = (int)cont.fiIDRequerimientoPadre;
                    tick.fcPlataforma = cont.fcNombrePlataforma;
                    tick.fcServicioAfectados = cont.fcNombreServicio;
                    tick.fdFechaAlarmaDeteccion = (cont.fdFechaAlarmaDeteccion is null) ? DateTime.Now : (DateTime)cont.fdFechaAlarmaDeteccion;
                    tick.fiIDUsuarioSolicitante = (int)cont.fiIDUsuarioSolicitante;

                    //tick.fiMotivoEstado = (int)cont.fiMotivoEstado;

                    ViewBag.fiMotivoEstado = (int)cont.fiMotivoEstado;

                    var estadosquenovan = contexto.sp_Configuraciones("NoMostrarEstados").FirstOrDefault().fcValorLlave.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                    ViewBag.ListarArea = contexto.sp_Areas_Lista().Select(x => new SelectListItem { Value = x.fiIDArea.ToString(), Text = x.fcDescripcion }).ToList();
                    ViewBag.ListaCategorias = contexto.sp_Categorias_Indicidencias_Listado().Select(a => new SelectListItem { Value = a.fiIDCategoriaDesarrollo.ToString(), Text = a.fcDescripcionCategoria }).ToList();
                    ViewBag.IdIncidencia = tick.fiTipoRequerimiento;
                    ViewBag.TicketPadre = tick.fiIdTicketPadre;

                    ViewBag.CategoriaResolucion = cont.fiCategoriaResolucion;
                    ViewBag.SubCategoriaResolucion = cont.fiSubCategoriaResolucion;
                    var puede = false;

                    ViewBag.IdServicios = contexto.sp_RequerimientoPorServicioByRequerimiento(idticket).ToList();
                    ViewBag.IdsCis = contexto.sp_CIporIncidencias_listos(idticket).ToList();
                    var idrolestodopoderosos = contexto.sp_Configuraciones("RolesquePuedenverTodo").FirstOrDefault().fcValorLlave.Split(',').Select(a => Convert.ToInt32(a)).ToList();
                    var usuarioID = GetIdUser();
                    var user = GetUser();
                    var usuariologueado = contexto.sp_Usuarios_Maestro_PorIdUsuario(GetIdUser()).FirstOrDefault();

                    /*
                        se hace esto pero lo mejor es hacerlo desde la base de datos 
                        el saber el jefe inmediato de los usuario asignado y el usuario solicitante 
                        att: Edgardo Mancia 26/08/2024
                     */

                    var infousuarioasignado = contexto.sp_Usuarios_Maestro_PorIdUsuario(tick.fiIDUsuarioAsignado).FirstOrDefault();
                    var infousuarioaSolicitante = contexto.sp_Usuarios_Maestro_PorIdUsuario(tick.fiIDUsuarioSolicitante).FirstOrDefault();

                    if (usuarioID == cont.fiIDUsuarioSolicitante || idrolestodopoderosos.Contains(user.IdRol) || (usuariologueado.fiAreaAsignada == tick.fiIDAreaSolicitante) || (infousuarioasignado.fiIDJefeInmediato == usuarioID || infousuarioaSolicitante.fiIDJefeInmediato == usuarioID))
                    {
                        ViewBag.Estados = contexto.sp_Estados_Lista().Where(a => !estadosquenovan.Any(b => b == a.fiIDEstado)).Select(x => new SelectListItem { Value = x.fiIDEstado.ToString(), Text = x.fcDescripcionEstado }).ToList();
                        puede = true;
                    }
                    else
                    {
                        ViewBag.Estados = contexto.sp_Estados_Lista().Where(a => a.fiIDEstado != 5 && a.fiIDEstado != 6 && !estadosquenovan.Any(b => b == a.fiIDEstado)).Select(x => new SelectListItem { Value = x.fiIDEstado.ToString(), Text = x.fcDescripcionEstado }).ToList();
                        
                    }

                    ViewBag.PuedeEditarCategoria = puede;
                    ViewBag.Usuario = contexto.sp_Usuarios_Maestro_PorIdUsuarioSupervisor(1).Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = x.fcPrimerNombre + " " + x.fcPrimerApellido }).ToList();
                    ViewBag.idticket = idticket;
                    ViewBag.UsuarioLogueado = GetIdUser();
                    ViewBag.DatosDocumentoListado = contexto.sp_Requerimiento_Documentos_ObtenerPorIdRequerimiento(idticket, 1, 1, GetIdUser()).ToList();
                    ViewBag.fiIDEstadoRequerimiento = cont.fiIDEstadoRequerimiento == Convert.ToByte(7) ? Convert.ToByte(3) : cont.fiIDEstadoRequerimiento;
                    ViewBag.fiTipoRequerimiento = cont.fiTipoRequerimiento;

                    return PartialView(tick);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarTicket(TicketMiewModel ticket)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    try
                    {
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
                        var idarea = (ticket.fiAreaAsignada == 0) ? 6 : ticket.fiAreaAsignada; // aqui decimo que si el id area no es asignada que lo ponga en pendiente y en dado casi si es asignada entonces que lo deje tal cual
                        //cambiar despues Los datos que se envian en duro para que sea mas dinamico las cosas 
                        var usuarioLogueado = contexto.sp_Usuarios_Maestro_PorIdUsuario(GetIdUser()).FirstOrDefault();

                        var save = contexto.sp_Requerimiento_Alta(1, 1, GetIdUser(), ticket.fcTituloRequerimiento, ticket.fcDescripcionRequerimiento, ticket.fiIDEstadoRequerimiento, ticket.fiTipoRequerimiento, idarea, $"El usuario {usuarioLogueado.fcPrimerNombre} {usuarioLogueado.fcPrimerApellido} a Creado El Incidente, {ticket.fccomentario}", ticket.fiIDImpacto, ticket.fiIDUrgencia, ticket.fiIDPrioridad, ticket.fiIdTicketPadre, ticket.fdFechaAlarmaDeteccion, ticket.fiPlataforma, ticket.fiServicioAfectados, 0).FirstOrDefault();

                        

                        if (ticket.serviciosAfectados != null && ticket.serviciosAfectados.Any())
                        {
                            foreach (var item in ticket.serviciosAfectados) // aqui se guarda los servicios afectados
                            {
                                var guardarServicios = contexto.sp_IncidenciasPorServicioAfectado(save.IdIngresado, (int)item, GetIdUser());
                            }
                        }
                        if (ticket.Ciaguardar != null && ticket.Ciaguardar.Any())
                        {
                            foreach (var item in ticket.Ciaguardar)//aqui se guarda los Cis que no se que son la verdad
                            {
                                var guardarcis = contexto.sp_IncidenciasPorCI_Insertar(save.IdIngresado, (int)item, GetIdUser());
                            }
                        }

                        var datosticket = Datosticket((int)save.IdIngresado);
                        //GuardarBitacoraGeneralhistorial(GetIdUser(),datosticket.fiIDRequerimiento,datosticket.fiIDUsuarioSolicitante, comentarioticket,1,datosticket.fiIDEstadoRequerimiento,datosticket.fiIDUsuarioAsignado);

                        //if (GetIdUser() == datosticket.fiIDRequerimiento )
                        //{
                        //    agregarCreacionTicket((int)save.IdIngresado);//esto es SignalR
                        //}
                        var user = GetUser();
                        var correo = contexto.sp_DatosTicket_Correo(datosticket.fiIDRequerimiento).FirstOrDefault();
                        var correosNumeros = contexto.sp_CorreosNumeros_AreaRol(usuarioLogueado.fiAreaAsignada, 3).ToList();


                        foreach (var item in ticket.flListadoImagenes)
                        {

                            var arch = ConvertirBase64AImagen(item.fcbase64, item.fcNombreArchivo);//esto funciona tan bien que convierte imagenes, pdf, word, pdf, gatos, perros y los sube bien
                            var token = contexto.sp_ObtenerTokenBitacora_porIDTicket((int)save.IdIngresado).FirstOrDefault();
                            var guardardocumentos = contexto.sp_Requerimientos_Adjuntos_Guardar((int)save.IdIngresado, item.fcNombreArchivo, item.fcExtension, MemoryLoadManager.UrlWeb + @"/Documentos\Ticket\Ticket_" + (int)save.IdIngresado + "/" + arch.FileName, MemoryLoadManager.UrlWeb + @"/Documentos/Ticket/Ticket_" + (int)save.IdIngresado + "/" + arch.FileName, 1, 1, GetIdUser(), token);

                            string carpeta = @"\Documentos\Ticket\Ticket_" + (int)save.IdIngresado;
                            var urlPdf = MemoryLoadManager.URL + carpeta;
                            var ruta = carpeta + @"\";
                            ruta = ruta.Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");

                            var exists = System.IO.Directory.Exists(urlPdf);

                            //string nombreArchivo = "archivo.txt";
                            UploadFileServer148(@"Documentos\Ticket\Ticket_" + (int)save.IdIngresado, arch);

                        }

                        var _emailTemplateService = new EmailTemplateService();
                        //correo area
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
                            fcCorreoElectronico = correo.fcCorreoArea,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaSolicitante += correo.fcCorreoArea;
                        MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcNumeroGerencia, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                        pcNumeroGerenciaSolicitante += correo.fcNumeroGerencia;
                        for (int i = 0; i < correosNumeros.Count; i++)
                        {
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correosNumeros[i].fcTelefonoMovil, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            pcNumerosJefesAsignada += " / " + correosNumeros[i].fcTelefonoMovil;
                        }
                        using (var connection = (new SARISEntities1()).Database.Connection)
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                                    $"EXEC sp_LogCorreosNumeros_Crear {datosticket.fiIDRequerimiento},{1},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                            using (var reader = command.ExecuteReader())
                            {

                            }

                            //MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoSolicitante, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comentarioticket, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion);
                            //MensajeDeTexto.EnviarLinkGeoLocation(model.Nombre, model.IdCliente, model.Telefono, "");
                            return EnviarListaJson(save);
                        }

                    }
                    catch (Exception ex)
                    {
                        return EnviarResultado(false, ex.Message.ToString(), "No se pudo registrar la incidencia");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo registrar el ticket");
            }
        }


        public ActionResult BandejaTicket()
        {
            return View();
        }

        [Authorize(Roles = "SARIS_CREARTICKET")]
        public ActionResult RegistrarTicket()
        {
            return PartialView();
        }

        public async Task<JsonResult> ActualizarDatos(TicketMiewModel ticket)
        {
            //var resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
            //var mensajeError = string.Empty;
            //var estado = Requerimiento.fiIDEstadoRequerimiento;
            try
            {
                var correo = new DatosCorreos();
                var TicketHijosCorreo = new List<DatosCorreos>();
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_DatosTicket_Correo {ticket.fiIDRequerimiento} ";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        
                        correo = db.ObjectContext.Translate<DatosCorreos>(reader).FirstOrDefault();
                       
                    }
                    if (ticket.fiIDEstadoRequerimiento == 5) 
                    {
                        command.CommandText = $"EXEC sp_Requerimientos_IDRequerimientoPadre {ticket.fiIDRequerimiento}, {GetIdUser()}";
                        using (var reader = command.ExecuteReader())
                        {
                            var db = ((IObjectContextAdapter)new SARISEntities1());

                            TicketHijosCorreo = db.ObjectContext.Translate<DatosCorreos>(reader).ToList();

                        }
                    }
                    
                    
                    connection.Close();
                }
                
                using (var contexto = new SARISEntities1())
                {
                    var datosticket = Datosticket(ticket.fiIDRequerimiento);//contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), ticket.fiIDRequerimiento).FirstOrDefault();
                    //----------
                    
                    var _emailTemplateService = new EmailTemplateService();
                    var AreaAsignada = (int)correo.fiAreaAsignada;
                    var AreaSolicitante = (int)correo.fiIDAreaSolicitante;
                    

                    var CorreoNumeroSupervisorAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 6).ToList();
                    var CorreoNumeroJefeAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 3).ToList();

                    if (ticket.fiIDEstadoRequerimiento == 5 || ticket.fiIDEstadoRequerimiento == 6)
                    {
                        var IdesAnteriores = contexto.sp_SaberUsuario_Area_Estado_Anterior_Requerimiento(ticket.fiIDRequerimiento).FirstOrDefault();

                        CorreoNumeroSupervisorAsignada = contexto.sp_CorreosNumeros_AreaRol(IdesAnteriores.fiAreaAsignada, 6).ToList();
                        CorreoNumeroJefeAsignada = contexto.sp_CorreosNumeros_AreaRol(IdesAnteriores.fiAreaAsignada, 3).ToList();

                    }
                    
                    

                    var CorreoNumeroJefeSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitante, 3).ToList();
                    var CorreoNumeroSupervisorSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitante, 6).ToList();

                    
                    if (TicketHijosCorreo.Count > 0) 
                    {
                        for (int i = 0; i < TicketHijosCorreo.Count; i++)
                        {
                            CorreoNumeroSupervisorAsignada.AddRange(contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[i].fiAreaAsignada, 6).ToList());
                            CorreoNumeroJefeAsignada.AddRange(contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[i].fiAreaAsignada, 3).ToList());
                            CorreoNumeroJefeSolicitante.AddRange(contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[i].fiIDAreaSolicitante, 3).ToList());
                            CorreoNumeroSupervisorSolicitante.AddRange(contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[i].fiIDAreaSolicitante, 6).ToList());

                        }
                        
                    }
                    
                    if (ticket.fiIDEstadoRequerimiento == 5 || ticket.fiIDEstadoRequerimiento == 6 )
                    {
                         
                            string pcCorreoGerenciaSolicitante = ""
                              ,pcNumeroGerenciaSolicitante=""
                              ,pcCorreosUsuarioSolicitante = ""
                              ,pcNumerosUsuarioSolicitante = ""
                              ,pcCorreosJefesSolicitante = ""
                              ,pcNumerosJefesSolicitante = ""
                              ,pcCorreosSupervisorSolicitante = ""
                              ,pcNumerosSupervisorSolicitante = ""
                              ,pcCorreoAreaSolicitante = ""
                              ,pcCorreoGerenciaAsignada = ""
                              ,pcNumeroGerenciaAsignada = ""
                              ,pcCorreoUsuarioAsignado = ""
                              ,pcNumeroUsuarioAsignado = ""
                              ,pcCorreosJefesAsignada = ""
                              ,pcNumerosJefesAsignada = ""
                              ,pcCorreosSupervisoresAsignado = ""
                              ,pcNumerosSupervisorAsignado = ""
                              ,pcCorreoAreaAsignada = "";
                        //correo Gerencia
                        
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
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = correo.fcCorreoGerencia,
                                fcDescripcionCategoria = correo.fcDescripcionCategoria,
                                fcTipoRequerimiento = correo.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correo.fiIDUrgencia,
                                fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correo.fiIDImpacto,
                                fcDescripcionImpacto = correo.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correo.fiIDPrioridad,
                                fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });

                            pcCorreoGerenciaSolicitante += correo.fcCorreoGerencia;
                        
                        

                        //Correo numero JefeArea Solicitante
                        for (int i = 0; i < CorreoNumeroJefeSolicitante.Count; i++)
                        {
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
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeSolicitante[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correo.fcDescripcionCategoria,
                                fcTipoRequerimiento = correo.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correo.fiIDUrgencia,
                                fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correo.fiIDImpacto,
                                fcDescripcionImpacto = correo.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correo.fiIDPrioridad,
                                fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            

                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitante[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }

                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitante[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            
                            pcCorreosJefesSolicitante += " / " + CorreoNumeroJefeSolicitante[i].fcBuzondeCorreo;
                            pcNumerosJefesSolicitante += " / " + CorreoNumeroJefeSolicitante[i].fcTelefonoMovil;
                        }

                        //Correo numero Supervisor Solicitante
                        for (int i = 0; i < CorreoNumeroSupervisorSolicitante.Count; i++)
                        {
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
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorSolicitante[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correo.fcDescripcionCategoria,
                                fcTipoRequerimiento = correo.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correo.fiIDUrgencia,
                                fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correo.fiIDImpacto,
                                fcDescripcionImpacto = correo.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correo.fiIDPrioridad,
                                fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitante[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }

                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitante[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            
                            pcCorreosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitante[i].fcBuzondeCorreo;
                            pcNumerosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitante[i].fcTelefonoMovil;
                        }

                        //Correo Usuario Solicitante
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
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoUsuarioSolicitante,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreosUsuarioSolicitante += correo.fcCorreoUsuarioSolicitante;
                        //Correo Gerencia Asignada
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
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoGerenciaAsignada,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoGerenciaAsignada += correo.fcCorreoGerenciaAsignada;
                        //Correo Jefes Asignada
                        for (int i = 0; i < CorreoNumeroJefeAsignada.Count; i++)
                        {
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
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeAsignada[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correo.fcDescripcionCategoria,
                                fcTipoRequerimiento = correo.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correo.fiIDUrgencia,
                                fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correo.fiIDImpacto,
                                fcDescripcionImpacto = correo.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correo.fiIDPrioridad,
                                fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            
                            pcCorreosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcBuzondeCorreo;
                            pcNumerosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcTelefonoMovil;
                        }
                        //Correo Supervisores asignados
                        for (int i = 0; i < CorreoNumeroSupervisorAsignada.Count; i++)
                        {
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
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correo.fcDescripcionCategoria,
                                fcTipoRequerimiento = correo.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correo.fiIDUrgencia,
                                fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correo.fiIDImpacto,
                                fcDescripcionImpacto = correo.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correo.fiIDPrioridad,
                                fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            }
                            
                            pcCorreosSupervisoresAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo;
                            pcNumerosSupervisorAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil;
                        }


                        //correo usuario asignado
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
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoElectronico,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoUsuarioAsignado += correo.fcCorreoElectronico;
                        //correo area solicitante
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
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoArea,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaSolicitante += correo.fcCorreoArea;
                        //correo area Asignada
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
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = correo.fcCorreoAreaAsignada,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaAsignada += correo.fcCorreoAreaAsignada;
                        if (ticket.fiIDEstadoRequerimiento == 5)
                        {
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoGrenciaAsignada, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcNumeroGerencia, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoMovil, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoSolicitante, correo.fcTituloRequerimiento, "Cerrado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);

                        }

                        if (ticket.fiIDEstadoRequerimiento == 6)
                        {
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoGrenciaAsignada, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcNumeroGerencia, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoMovil, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoSolicitante, correo.fcTituloRequerimiento, "Cancelado", correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, ticket.fcDescripcionEstado, correo.fcDescripcionPrioridad, ticket.fccomentario, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);

                        }
                        
                        pcNumeroGerenciaAsignada += correo.fcTelefonoGrenciaAsignada;
                        pcNumeroGerenciaSolicitante += correo.fcNumeroGerencia;
                        pcNumeroUsuarioAsignado += correo.fcTelefonoMovil;
                        pcNumerosUsuarioSolicitante += correo.fcTelefonoSolicitante;

                        using (var connection = (new SARISEntities1()).Database.Connection)
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            if (ticket.fiIDEstadoRequerimiento == 5) 
                            {
                                command.CommandText = 
                                    $"EXEC sp_LogCorreosNumeros_Crear {ticket.fiIDRequerimiento},{5},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                                using (var reader = command.ExecuteReader())
                                {

                                }
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                command.CommandText = 
                                    $"EXEC sp_LogCorreosNumeros_Crear {ticket.fiIDRequerimiento},{6},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                                using (var reader = command.ExecuteReader())
                                {

                                }
                            }
                            

                            connection.Close();


                        
                        }
                    }


                    //----------
                    //------- notificaciones hijos
                    if (ticket.fiIDEstadoRequerimiento == 5 && TicketHijosCorreo.Count>0)
                    {
                        for (int x = 0; x < TicketHijosCorreo.Count; x++)
                        {
                            
                            var CorreoNumeroSupervisorAsignadaHijo = contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[x].fiAreaAsignada, 6).ToList();
                            var CorreoNumeroJefeAsignadaHijo = contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[x].fiAreaAsignada, 3).ToList();
                            var CorreoNumeroJefeSolicitanteHijo = contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[x].fiIDAreaSolicitante, 3).ToList();
                            var CorreoNumeroSupervisorSolicitanteHijo = contexto.sp_CorreosNumeros_AreaRol(TicketHijosCorreo[x].fiIDAreaSolicitante, 6).ToList();
                        


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
                        //correo Gerencia

                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoGerencia,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });

                        pcCorreoGerenciaSolicitante += TicketHijosCorreo[x].fcCorreoGerencia;



                        //Correo numero JefeArea Solicitante
                        for (int z = 0; z < CorreoNumeroJefeSolicitanteHijo.Count; z++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                                fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                                fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                                fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                                fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                                fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                                fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                                fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeSolicitanteHijo[z].fcBuzondeCorreo,
                                fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                                fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                                fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                                fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                                fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                                fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                                fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                                fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });


                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            pcCorreosJefesSolicitante += " / " + CorreoNumeroJefeSolicitanteHijo[z].fcBuzondeCorreo;
                            pcNumerosJefesSolicitante += " / " + CorreoNumeroJefeSolicitanteHijo[z].fcTelefonoMovil;
                        }

                        //Correo numero Supervisor Solicitante
                        for (int z = 0; z < CorreoNumeroSupervisorSolicitanteHijo.Count; z++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                                fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                                fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                                fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                                fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                                fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                                fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                                fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorSolicitanteHijo[z].fcBuzondeCorreo,
                                fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                                fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                                fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                                fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                                fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                                fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                                fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                                fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            pcCorreosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitanteHijo[z].fcBuzondeCorreo;
                            pcNumerosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitanteHijo[z].fcTelefonoMovil;
                        }

                        //Correo Usuario Solicitante
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoUsuarioSolicitante,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreosUsuarioSolicitante += TicketHijosCorreo[x].fcCorreoUsuarioSolicitante;
                        //Correo Gerencia Asignada
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoGerenciaAsignada,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoGerenciaAsignada += TicketHijosCorreo[x].fcCorreoGerenciaAsignada;
                        //Correo Jefes Asignada
                        for (int z = 0; z < CorreoNumeroJefeAsignadaHijo.Count; z++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                                fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                                fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                                fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                                fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                                fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                                fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                                fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeAsignadaHijo[z].fcBuzondeCorreo,
                                fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                                fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                                fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                                fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                                fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                                fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                                fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                                fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            pcCorreosJefesAsignada += " / " + CorreoNumeroJefeAsignadaHijo[z].fcBuzondeCorreo;
                            pcNumerosJefesAsignada += " / " + CorreoNumeroJefeAsignadaHijo[z].fcTelefonoMovil;
                        }
                        //Correo Supervisores asignados
                        for (int z = 0; z < CorreoNumeroSupervisorAsignadaHijo.Count; z++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                                fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                                fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                                fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                                fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                                fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                                fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                                fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                                fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = ticket.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorAsignadaHijo[z].fcBuzondeCorreo,
                                fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                                fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                                fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                                fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                                fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                                fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                                fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                                fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            }

                            pcCorreosSupervisoresAsignado += " / " + CorreoNumeroSupervisorAsignadaHijo[z].fcBuzondeCorreo;
                            pcNumerosSupervisorAsignado += " / " + CorreoNumeroSupervisorAsignadaHijo[z].fcTelefonoMovil;
                        }


                        //correo usuario asignado
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoElectronico,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoUsuarioAsignado += TicketHijosCorreo[x].fcCorreoElectronico;
                        //correo area solicitante
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoArea,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaSolicitante += TicketHijosCorreo[x].fcCorreoArea;
                        //correo area Asignada
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = TicketHijosCorreo[x].fiIDRequerimiento,
                            fcTituloRequerimiento = TicketHijosCorreo[x].fcTituloRequerimiento,
                            fcDescripcionRequerimiento = TicketHijosCorreo[x].fcDescripcionRequerimiento,
                            fdFechaCreacion = TicketHijosCorreo[x].fdFechaCreacion,
                            fiIDAreaSolicitante = (int)TicketHijosCorreo[x].fiIDAreaSolicitante,
                            fcAreaSolicitante = TicketHijosCorreo[x].fcAreaSolicitante,
                            fiIDUsuarioSolicitante = TicketHijosCorreo[x].fiIDUsuarioSolicitante,
                            fcNombreCorto = TicketHijosCorreo[x].fcNombreCorto,
                            fiIDEstadoRequerimiento = ticket.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = ticket.fcDescripcionEstado,
                            fcCorreoElectronico = TicketHijosCorreo[x].fcCorreoAreaAsignada,
                            fcDescripcionCategoria = TicketHijosCorreo[x].fcDescripcionCategoria,
                            fcTipoRequerimiento = TicketHijosCorreo[x].fcTipoRequerimiento,
                            fiIDUrgencia = (int)TicketHijosCorreo[x].fiIDUrgencia,
                            fcDescripcionUrgencia = TicketHijosCorreo[x].fcDescripcionUrgencia,
                            fiIDImpacto = (int)TicketHijosCorreo[x].fiIDImpacto,
                            fcDescripcionImpacto = TicketHijosCorreo[x].fcDescripcionImpacto,
                            fiIDPrioridad = (int)TicketHijosCorreo[x].fiIDPrioridad,
                            fcDescripcionPrioridad = TicketHijosCorreo[x].fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaAsignada += TicketHijosCorreo[x].fcCorreoAreaAsignada;
                        if (ticket.fiIDEstadoRequerimiento == 5)
                        {
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoGrenciaAsignada, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcNumeroGerencia, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoSolicitante, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);

                        }

                        if (ticket.fiIDEstadoRequerimiento == 6)
                        {
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoGrenciaAsignada, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcNumeroGerencia, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                            MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoSolicitante, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);

                        }

                        pcNumeroGerenciaAsignada += TicketHijosCorreo[x].fcTelefonoGrenciaAsignada;
                        pcNumeroGerenciaSolicitante += TicketHijosCorreo[x].fcNumeroGerencia;
                        pcNumeroUsuarioAsignado += TicketHijosCorreo[x].fcTelefonoMovil;
                        pcNumerosUsuarioSolicitante += TicketHijosCorreo[x].fcTelefonoSolicitante;

                        using (var connection = (new SARISEntities1()).Database.Connection)
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            if (ticket.fiIDEstadoRequerimiento == 5)
                            {
                                command.CommandText =
                                    $"EXEC sp_LogCorreosNumeros_Crear {TicketHijosCorreo[x].fiIDRequerimiento},{5},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                                using (var reader = command.ExecuteReader())
                                {

                                }
                            }
                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                command.CommandText =
                                    $"EXEC sp_LogCorreosNumeros_Crear {ticket.fiIDRequerimiento},{6},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                    $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                    $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                    $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                    $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                                using (var reader = command.ExecuteReader())
                                {

                                }
                            }


                            connection.Close();



                        }
                        }
                    }
                    //----- fin
                    GuardarBitacoraGeneralhistorial(GetIdUser(), ticket.fiIDRequerimiento, datosticket.fiIDUsuarioSolicitante, ticket.fccomentario, 1, ticket.fiIDEstadoRequerimiento, datosticket.fiIDUsuarioAsignado, (int)datosticket.fiAreaAsignada);
                    if (TicketHijosCorreo.Count > 0)
                    {
                        for (int i = 0; i < TicketHijosCorreo.Count; i++)
                        {
                            GuardarBitacoraGeneralhistorial(GetIdUser(), TicketHijosCorreo[i].fiIDRequerimiento, TicketHijosCorreo[i].fiIDUsuarioSolicitante, ticket.fccomentario, 1, ticket.fiIDEstadoRequerimiento, datosticket.fiIDUsuarioAsignado, (int)TicketHijosCorreo[i].fiAreaAsignada);
                        }
                    }
                    var ticketpadre = ticket.fiIdTicketPadre;
                    var fiCategoriaResolucion = ticket.fiCategoriaResolucion;
                    var fiSubCategoriaResolucion = ticket.fiSubCategoriaResolucion;
                    if (ticket.fiIDEstadoRequerimiento == 5)
                    {
                        fiSubCategoriaResolucion = (int)datosticket.fiSubCategoriaResolucion;
                        fiCategoriaResolucion = (int)datosticket.fiCategoriadeDesarrollo;
                    }
                    var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), ticket.fiIDRequerimiento, ticket.fcTituloRequerimiento, ticket.fcDescripcionRequerimiento, ticket.fiIDEstadoRequerimiento, DateTime.Now, datosticket.fiIDUsuarioAsignado, 0, ticket.fiTipoRequerimiento, 1, datosticket.fiAreaAsignada, ticketpadre, ticket.fiMotivoEstado, fiCategoriaResolucion, fiSubCategoriaResolucion);

                    ///////////////////////////////////////////////////////////////////////////////////////// aqui va lo que nuevo

                    

                    if (ticket.serviciosAfectados != null)
                    {
                        foreach (var item in ticket.serviciosAfectados)
                        {
                            var guardarServicios = contexto.sp_IncidenciasPorServicioAfectado(ticket.fiIDRequerimiento, item, GetIdUser());
                        }
                    }
                    if (ticket.Ciaguardar != null)
                    {
                        foreach (var item in ticket.Ciaguardar)
                        {
                            var guardarcis = contexto.sp_IncidenciasPorCI_Insertar(ticket.fiIDRequerimiento, item, GetIdUser());
                        }
                    }

                    foreach (var item in ticket.flListadoImagenes)
                    {

                        var arch = ConvertirBase64AImagen(item.fcbase64, item.fcNombreArchivo);//esto funciona tan bien que convierte imagenes, pdf, word, pdf, gatos, perros y los sube bien
                        var token = contexto.sp_ObtenerTokenBitacora_porIDTicket(ticket.fiIDRequerimiento).FirstOrDefault();
                        var guardardocumentos = contexto.sp_Requerimientos_Adjuntos_Guardar(ticket.fiIDRequerimiento, item.fcNombreArchivo, item.fcExtension, MemoryLoadManager.UrlWeb + @"/Documentos\Ticket\Ticket_" + ticket.fiIDRequerimiento + "/" + arch.FileName, MemoryLoadManager.UrlWeb + @"/Documentos/Ticket/Ticket_" + ticket.fiIDRequerimiento + "/" + arch.FileName, 1, 1, GetIdUser(), token);

                        string carpeta = @"\Documentos\Ticket\Ticket_" + ticket.fiIDRequerimiento;
                        var urlPdf = MemoryLoadManager.URL + carpeta;
                        var ruta = carpeta + @"\";
                        ruta = ruta.Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");

                        var exists = System.IO.Directory.Exists(urlPdf);

                        //string nombreArchivo = "archivo.txt";
                        UploadFileServer148(@"Documentos\Ticket\Ticket_" + ticket.fiIDRequerimiento, arch);

                    }


                    if (ticket.fiIDEstadoRequerimiento == 5)
                    {
                        //eliminarTicketAbierto(ticket.fiIDRequerimiento); //singalr
                        //agregarDatosTicketCerrados(ticket.fiIDRequerimiento);//singalr
                    }
                    else
                    {
                        //ObtenerDataTicket(ticket.fiIDRequerimiento); //Esto es el SignalR
                    }

                    var correos = contexto.sp_DatosTicket_Correo(ticket.fiIDRequerimiento).FirstOrDefault();
                    AreaAsignada = (int)correos.fiAreaAsignada;
                    int AreaSolicitantes = (int)correos.fiIDAreaSolicitante;
                    CorreoNumeroJefeAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 3).ToList();
                    CorreoNumeroJefeSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitantes, 3).ToList();

                    CorreoNumeroSupervisorAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 6).ToList();
                    CorreoNumeroSupervisorSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitantes, 6).ToList();
                    if (correos.fiIDEstadoRequerimiento == 3 || correos.fiIDEstadoRequerimiento == 11 || correos.fiIDEstadoRequerimiento == 4)
                    {
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

                        //Correo Area
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = correos.fiIDRequerimiento,
                            fcTituloRequerimiento = correos.fcTituloRequerimiento,
                            fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                            fdFechaCreacion = correos.fdFechaCreacion,
                            fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                            fcAreaSolicitante = correos.fcAreaSolicitante,
                            fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                            fcNombreCorto = correos.fcNombreCorto,
                            fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = correos.fcDescripcionEstado,
                            fcCorreoElectronico = correos.fcCorreoArea,
                            fcDescripcionCategoria = correos.fcDescripcionCategoria,
                            fcTipoRequerimiento = correos.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correos.fiIDUrgencia,
                            fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correos.fiIDImpacto,
                            fcDescripcionImpacto = correos.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correos.fiIDPrioridad,
                            fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoAreaSolicitante += correos.fcCorreoArea;

                        //Correo numero JefeArea Solicitante
                        for (int i = 0; i < CorreoNumeroJefeSolicitante.Count; i++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = correos.fiIDRequerimiento,
                                fcTituloRequerimiento = correos.fcTituloRequerimiento,
                                fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                                fdFechaCreacion = correos.fdFechaCreacion,
                                fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                                fcAreaSolicitante = correos.fcAreaSolicitante,
                                fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                                fcNombreCorto = correos.fcNombreCorto,
                                fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = correos.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeSolicitante[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correos.fcDescripcionCategoria,
                                fcTipoRequerimiento = correos.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correos.fiIDUrgencia,
                                fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correos.fiIDImpacto,
                                fcDescripcionImpacto = correos.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correos.fiIDPrioridad,
                                fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitante[i].fcTelefonoMovil, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                            pcCorreosJefesSolicitante += " / " + CorreoNumeroJefeSolicitante[i].fcBuzondeCorreo;
                            pcNumerosJefesSolicitante += " / " + CorreoNumeroJefeSolicitante[i].fcTelefonoMovil;

                        }

                        //Correo numero Supervisor Solicitante
                        for (int i = 0; i < CorreoNumeroSupervisorSolicitante.Count; i++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = correos.fiIDRequerimiento,
                                fcTituloRequerimiento = correos.fcTituloRequerimiento,
                                fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                                fdFechaCreacion = correos.fdFechaCreacion,
                                fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                                fcAreaSolicitante = correos.fcAreaSolicitante,
                                fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                                fcNombreCorto = correos.fcNombreCorto,
                                fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = correos.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorSolicitante[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correos.fcDescripcionCategoria,
                                fcTipoRequerimiento = correos.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correos.fiIDUrgencia,
                                fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correos.fiIDImpacto,
                                fcDescripcionImpacto = correos.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correos.fiIDPrioridad,
                                fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitante[i].fcTelefonoMovil, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                            pcCorreosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitante[i].fcBuzondeCorreo;
                            pcNumerosSupervisorSolicitante += " / " + CorreoNumeroSupervisorSolicitante[i].fcTelefonoMovil;
                        }

                        //Correo Jefes Asignada
                        for (int i = 0; i < CorreoNumeroJefeAsignada.Count; i++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = correos.fiIDRequerimiento,
                                fcTituloRequerimiento = correos.fcTituloRequerimiento,
                                fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                                fdFechaCreacion = correos.fdFechaCreacion,
                                fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                                fcAreaSolicitante = correos.fcAreaSolicitante,
                                fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                                fcNombreCorto = correos.fcNombreCorto,
                                fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = correos.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroJefeAsignada[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correos.fcDescripcionCategoria,
                                fcTipoRequerimiento = correos.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correos.fiIDUrgencia,
                                fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correos.fiIDImpacto,
                                fcDescripcionImpacto = correos.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correos.fiIDPrioridad,
                                fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignada[i].fcTelefonoMovil, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                            pcCorreosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcBuzondeCorreo;
                            pcNumerosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcTelefonoMovil;

                        }
                        //Correo Supervisores asignados
                        for (int i = 0; i < CorreoNumeroSupervisorAsignada.Count; i++)
                        {
                            await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                            {
                                fiIDRequerimiento = correos.fiIDRequerimiento,
                                fcTituloRequerimiento = correos.fcTituloRequerimiento,
                                fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                                fdFechaCreacion = correos.fdFechaCreacion,
                                fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                                fcAreaSolicitante = correos.fcAreaSolicitante,
                                fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                                fcNombreCorto = correos.fcNombreCorto,
                                fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                                fcDescripcionEstado = correos.fcDescripcionEstado,
                                fcCorreoElectronico = CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo,
                                fcDescripcionCategoria = correos.fcDescripcionCategoria,
                                fcTipoRequerimiento = correos.fcTipoRequerimiento,
                                fiIDUrgencia = (int)correos.fiIDUrgencia,
                                fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                                fiIDImpacto = (int)correos.fiIDImpacto,
                                fcDescripcionImpacto = correos.fcDescripcionImpacto,
                                fiIDPrioridad = (int)correos.fiIDPrioridad,
                                fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                                fcComentario = ticket.fccomentario
                            });
                            MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                            pcCorreosSupervisoresAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo;
                            pcNumerosSupervisorAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil;

                        }

                        //Correo usuario solicitante
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = correos.fiIDRequerimiento,
                            fcTituloRequerimiento = correos.fcTituloRequerimiento,
                            fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                            fdFechaCreacion = correos.fdFechaCreacion,
                            fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                            fcAreaSolicitante = correos.fcAreaSolicitante,
                            fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                            fcNombreCorto = correos.fcNombreCorto,
                            fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = correos.fcDescripcionEstado,
                            fcCorreoElectronico = correos.fcCorreoUsuarioSolicitante,
                            fcDescripcionCategoria = correos.fcDescripcionCategoria,
                            fcTipoRequerimiento = correos.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correos.fiIDUrgencia,
                            fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correos.fiIDImpacto,
                            fcDescripcionImpacto = correos.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correos.fiIDPrioridad,
                            fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreosUsuarioSolicitante += " / " + correos.fcCorreoUsuarioSolicitante;
                        //Correo usuario Asignado
                        await _emailTemplateService.SendEmailToSolicitud(new EmailTemplateTicketModel
                        {
                            fiIDRequerimiento = correos.fiIDRequerimiento,
                            fcTituloRequerimiento = correos.fcTituloRequerimiento,
                            fcDescripcionRequerimiento = correos.fcDescripcionRequerimiento,
                            fdFechaCreacion = correos.fdFechaCreacion,
                            fiIDAreaSolicitante = (int)correos.fiIDAreaSolicitante,
                            fcAreaSolicitante = correos.fcAreaSolicitante,
                            fiIDUsuarioSolicitante = correos.fiIDUsuarioSolicitante,
                            fcNombreCorto = correos.fcNombreCorto,
                            fiIDEstadoRequerimiento = correos.fiIDEstadoRequerimiento,
                            fcDescripcionEstado = correos.fcDescripcionEstado,
                            fcCorreoElectronico = correos.fcCorreoElectronico,
                            fcDescripcionCategoria = correos.fcDescripcionCategoria,
                            fcTipoRequerimiento = correos.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correos.fiIDUrgencia,
                            fcDescripcionUrgencia = correos.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correos.fiIDImpacto,
                            fcDescripcionImpacto = correos.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correos.fiIDPrioridad,
                            fcDescripcionPrioridad = correos.fcDescripcionPrioridad,
                            fcComentario = ticket.fccomentario
                        });
                        pcCorreoUsuarioAsignado += " / " + correos.fcCorreoElectronico;
                        MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, correos.fcTelefonoMovil, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                        MensajeriaApi.EnviarNumeroTicket(correos.fcNombreCorto, datosticket.fiIDRequerimiento, correos.fcTelefonoSolicitante, correos.fcTituloRequerimiento, correos.fcDescripcionRequerimiento, correos.fcDescripcionCategoria, correos.fcTipoRequerimiento, correos.fcDescripcionEstado, correos.fcDescripcionPrioridad, ticket.fccomentario, correos.fcDescripcionCategoriaResolucion, correos.fcTipoRequerimientoResolucion, correos.fcAreaSolicitante);
                        pcNumeroUsuarioAsignado += " / " + correos.fcTelefonoMovil;
                        pcNumerosUsuarioSolicitante += " / " + correos.fcTelefonoSolicitante;

                        using (var connection = (new SARISEntities1()).Database.Connection)
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                                    $"EXEC sp_LogCorreosNumeros_Crear {ticket.fiIDRequerimiento},{4},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
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





                    return EnviarResultado(true, "", "Ticket Actualizado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo Actualizar el ticket");
                throw;
            }
        }

        static HttpPostedFileBase ConvertirBase64AImagen(string base64String, string nombreArchivo)
        {

            HttpPostedFileBase archivo = new ByteArrayHttpPostedFile(base64String, nombreArchivo);
            //GuardarImagen(bytes, archivo);
            return archivo;

        }


        public async Task<JsonResult> GuardarVideo(int idSolicitd, HttpPostedFileBase img)
        {
            using (var context = new SARISEntities1())
            {

                try
                {
                    if (img != null)
                    {
                        var documentoURL = @"\Documentos\Ticket\Ticket_" + idSolicitd;
                        var urlPdf = MemoryLoadManager.URL + documentoURL;
                        var ruta = documentoURL + @"\";
                        ruta = ruta.Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
                        var pdfFile = urlPdf + @"\" + img.FileName;
                        var exists = System.IO.Directory.Exists(urlPdf);
                        UploadFileServer148(@"Documentos\Ticket\Ticket_" + idSolicitd, img);
                    }
                    var result = 0;
                    var token = context.sp_ObtenerTokenBitacora_porIDTicket(idSolicitd).FirstOrDefault();

                    var guardardocumentos = context.sp_Requerimientos_Adjuntos_Guardar(idSolicitd, img.FileName, Path.GetExtension(img.FileName), MemoryLoadManager.UrlWeb + @"/Documentos\Ticket\Ticket_" + idSolicitd + "/" + img.FileName, MemoryLoadManager.UrlWeb + @"/Documentos/Ticket/Ticket_" + idSolicitd + "/" + img.FileName, 1, 1, GetIdUser(), token);

                    return EnviarResultado(true, "Mensajes", "Guardados Exitosamente");


                }
                catch (Exception ex)
                {
                    return EnviarException(ex, "Error");
                }


            }



        }


        public JsonResult Guardardocumentos(List<TicketAdjuntarImagenViewModel> modelo)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    foreach (var item in modelo)
                    {

                        var arch = ConvertirBase64AImagen(item.pcRutaArchivo, item.pcNombreArchivo);//esto funciona tan bien que convierte imagenes, pdf, word, pdf, gatos, perros y los sube bien
                        var token = contexto.sp_ObtenerTokenBitacora_porIDTicket(item.piIDRequerimiento).FirstOrDefault();
                        var guardardocumentos = contexto.sp_Requerimientos_Adjuntos_Guardar(item.piIDRequerimiento, item.pcNombreArchivo, item.pcTipoArchivo, MemoryLoadManager.UrlWeb + @"/Documentos\Ticket\Ticket_" + item.piIDRequerimiento + "/" + arch.FileName, MemoryLoadManager.UrlWeb + @"/Documentos/Ticket/Ticket_" + item.piIDRequerimiento + "/" + arch.FileName, item.piIDSesion, item.piIDApp, GetIdUser(), token);

                        string carpeta = @"\Documentos\Ticket\Ticket_" + item.piIDRequerimiento;
                        var urlPdf = MemoryLoadManager.URL + carpeta;
                        var ruta = carpeta + @"\";
                        ruta = ruta.Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");

                        var exists = System.IO.Directory.Exists(urlPdf);

                        //string nombreArchivo = "archivo.txt";
                        UploadFileServer148(@"Documentos\Ticket\Ticket_" + item.piIDRequerimiento, arch);

                    }
                    return EnviarResultado(true, "", "Documentos guardados con Exito");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(true, ex.Message.ToString(), "No se pudo Actualizar el ticket");
                throw;
            }


        }

        public ActionResult VistaActualizarArea(int idticket, int estadoticket)
        {
            ViewBag.IdTicket = idticket;
            ViewBag.EstadoTicket = estadoticket;
            using (var contexto = new SARISEntities1())
            {
                var datosticket = Datosticket(idticket); //contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                //ViewBag.idArea = 
            }

            return PartialView();
        }

        public async Task<JsonResult> ActualizarArea(int idticket, int idArea, int estadoTicket, string comenta)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
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
                    var areaasignada = contexto.sp_Requerimiento_Areas(1, 1, GetIdUser()).FirstOrDefault(a => a.fiIDArea == idArea).fcDescripcion; // buscar el area a la cual se le asigno
                    var usuarioLogueado = contexto.sp_Usuarios_Maestro_PorIdUsuario(GetIdUser()).FirstOrDefault();

                    var datosticket = Datosticket(idticket);//contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                    //var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, Convert.ToByte(3), DateTime.Now, 3013, 0, datosticket.fiTipoRequerimiento, 1, idArea, datosticket.fiIDRequerimientoPadre, 0, 0, 0);
                    //ObtenerDataTicket(idticket); // aqui va el signalR
                    var usuariopendiente = contexto.sp_Configuraciones("UsuarioPendiente").FirstOrDefault().fcValorLlave;//.fcValorLlave.Select(a => Convert.ToInt32(a)).FirstOrDefault();
                    
                    if (datosticket.fiIDEstadoRequerimiento == 1)
                    {
                        var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, 7, DateTime.Now, 3185, 0, datosticket.fiTipoRequerimiento, 1, idArea, datosticket.fiIDRequerimientoPadre, 0, 0, 0);
                    }
                    else
                    {
                        var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, datosticket.fiIDEstadoRequerimiento, DateTime.Now, 3185, 0, datosticket.fiTipoRequerimiento, 1, idArea, datosticket.fiIDRequerimientoPadre, 0, 0, 0);

                    }
                    datosticket = Datosticket(idticket);
                    GuardarBitacoraGeneralhistorial(GetIdUser(), idticket, GetIdUser(), $"{usuarioLogueado.fcPrimerNombre} {usuarioLogueado.fcPrimerApellido} Reasigna al Area {areaasignada} por: " + comenta, 1, 7, Convert.ToInt32(datosticket.fiIDUsuarioAsignado), idArea);// se cambio a que se envie el Sisten Bot
                    var correo = contexto.sp_DatosTicket_Correo(datosticket.fiIDRequerimiento).FirstOrDefault();
                    var _emailTemplateService = new EmailTemplateService();
                    var AreaAsignada = correo.fiAreaAsignada;
                    var AreaSolicitante = correo.fiIDAreaSolicitante;
                    var CorreoNumeroJefeAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 3).ToList();
                    var CorreoNumeroJefeSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitante, 3).ToList();

                    var CorreoNumeroSupervisorAsignada = contexto.sp_CorreosNumeros_AreaRol(AreaAsignada, 6).ToList();
                    var CorreoNumeroSupervisorSolicitante = contexto.sp_CorreosNumeros_AreaRol(AreaSolicitante, 6).ToList();
                    //correo area Asignada
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
                        fcCorreoElectronico = correo.fcCorreoAreaAsignada,
                        fcDescripcionCategoria = correo.fcDescripcionCategoria,
                        fcTipoRequerimiento = correo.fcTipoRequerimiento,
                        fiIDUrgencia = (int)correo.fiIDUrgencia,
                        fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                        fiIDImpacto = (int)correo.fiIDImpacto,
                        fcDescripcionImpacto = correo.fcDescripcionImpacto,
                        fiIDPrioridad = (int)correo.fiIDPrioridad,
                        fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                        fcComentario = comenta
                    });
                    pcCorreoAreaAsignada += correo.fcCorreoAreaAsignada;

                    //Correo Jefes Asignada
                    for (int i = 0; i < CorreoNumeroJefeAsignada.Count; i++)
                    {
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
                            fcCorreoElectronico = CorreoNumeroJefeAsignada[i].fcBuzondeCorreo,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = comenta

                        });

                        MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comenta, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                        pcCorreosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcBuzondeCorreo;
                        pcNumerosJefesAsignada += " / " + CorreoNumeroJefeAsignada[i].fcTelefonoMovil;

                    }
                    //Correo Supervisores asignados
                    for (int i = 0; i < CorreoNumeroSupervisorAsignada.Count; i++)
                    {
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
                            fcCorreoElectronico = CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo,
                            fcDescripcionCategoria = correo.fcDescripcionCategoria,
                            fcTipoRequerimiento = correo.fcTipoRequerimiento,
                            fiIDUrgencia = (int)correo.fiIDUrgencia,
                            fcDescripcionUrgencia = correo.fcDescripcionUrgencia,
                            fiIDImpacto = (int)correo.fiIDImpacto,
                            fcDescripcionImpacto = correo.fcDescripcionImpacto,
                            fiIDPrioridad = (int)correo.fiIDPrioridad,
                            fcDescripcionPrioridad = correo.fcDescripcionPrioridad,
                            fcComentario = comenta

                        });

                        MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comenta, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                        pcCorreosSupervisoresAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcBuzondeCorreo;
                        pcNumerosSupervisorAsignado += " / " + CorreoNumeroSupervisorAsignada[i].fcTelefonoMovil;
                    }
                    using (var connection = (new SARISEntities1()).Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText =
                                $"EXEC sp_LogCorreosNumeros_Crear {correo.fiIDRequerimiento},{2},'{pcCorreoGerenciaSolicitante}','{pcNumeroGerenciaSolicitante}'," +
                                $"'{pcCorreosUsuarioSolicitante}','{pcNumerosUsuarioSolicitante}','{pcCorreosJefesSolicitante}','{pcNumerosJefesSolicitante}'," +
                                $"'{pcCorreosSupervisorSolicitante}','{pcNumerosSupervisorSolicitante}','{pcCorreoAreaSolicitante}','{pcCorreoGerenciaAsignada}'," +
                                $"'{pcNumeroGerenciaAsignada}','{pcCorreoUsuarioAsignado}','{pcNumeroUsuarioAsignado}','{pcCorreosJefesAsignada}'," +
                                $"'{pcNumerosJefesAsignada}','{pcCorreosSupervisoresAsignado}','{pcNumerosSupervisorAsignado}','{pcCorreoAreaAsignada}'";
                        using (var reader = command.ExecuteReader())
                        {

                        }

                        connection.Close();


                    }
                    return EnviarResultado(true, "", "Ticket Actualizado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo Actualizar el ticket");
                throw;
            }
        }

        public ActionResult VistaAsignarUsuario(int idticket, int estadoticket, int idarea)
        {
            ViewBag.IdTicket = idticket;
            ViewBag.EstadoTicket = estadoticket;
            ViewBag.Area = idarea;
            using (var contexto = new SARISEntities1())
            {
                var datosticket = contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                //ViewBag.idArea = 
            }

            return PartialView();
        }

        public async Task<JsonResult> ActualizarUsuario(int idticket, int usuario, int estadoTicket, string comenta)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
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
                    var UsuarioAsignado = contexto.sp_Usuarios_Maestro_PorIdUsuario(usuario).FirstOrDefault(); // buscar el area a la cual se le asigno
                    var usuarioLogueado = contexto.sp_Usuarios_Maestro_PorIdUsuario(GetIdUser()).FirstOrDefault();

                    var datosticket = Datosticket(idticket);//contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).FirstOrDefault();
                    //guardar la bitacora 
                    GuardarBitacoraGeneralhistorial(GetIdUser(), idticket, datosticket.fiIDUsuarioSolicitante,$"El Usuario: {usuarioLogueado.fcNombreCorto} Asigno al Usuario {UsuarioAsignado.fcNombreCorto} por: {comenta}", 1, 7, usuario, (int)datosticket.fiAreaAsignada);//el estado de ticket esta en 7 para que pueda guardar la bitacora

                    if (datosticket.fiIDEstadoRequerimiento == 1)
                    {
                        var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, 3, DateTime.Now, usuario, 0, datosticket.fiTipoRequerimiento, 1, datosticket.fiAreaAsignada, datosticket.fiIDRequerimientoPadre, 0, 0, 0);
                    }
                    else
                    {
                        var actua = contexto.sp_Requerimiento_Maestro_Actualizar(GetIdUser(), datosticket.fiIDRequerimiento, datosticket.fcTituloRequerimiento, datosticket.fcDescripcionRequerimiento, datosticket.fiIDEstadoRequerimiento, DateTime.Now, usuario, 0, datosticket.fiTipoRequerimiento, 1, datosticket.fiAreaAsignada, datosticket.fiIDRequerimientoPadre, 0, 0, 0);
                    }
                    //ObtenerDataTicket(idticket);//aqui esta el signalR
                    if (GetIdUser() != usuario) //aqui el signalR por si al reasignar un usuario se le quite de la bandeja de el 
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
                        fcComentario = comenta
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
                    MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoMovil, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comenta, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion, correo.fcAreaSolicitante);
                    //MensajeriaApi.EnviarNumeroTicket(correo.fcNombreCorto, datosticket.fiIDRequerimiento, correo.fcTelefonoSolicitante, correo.fcTituloRequerimiento, correo.fcDescripcionRequerimiento, correo.fcDescripcionCategoria, correo.fcTipoRequerimiento, correo.fcDescripcionEstado, correo.fcDescripcionPrioridad, comenta, correo.fcDescripcionCategoriaResolucion, correo.fcTipoRequerimientoResolucion);
                    pcNumeroUsuarioAsignado =  correo.fcTelefonoMovil;
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

                    return EnviarResultado(true, "", "Ticket Usuario Actualizado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo Actualizar el usuario");
                throw;
            }
        }

        public JsonResult EliminarTicket(int idticket)
        {
            try
            {
                using (var contexto = new SARISEntities1())
                {
                    var result = contexto.sp_Eliminar_Requerimiento(idticket).FirstOrDefault();
                    //eliminarTicketAbierto(idticket);SignalR
                    return EnviarResultado(true, "Eliminado!", "Ticket Eliminado Exitosamente");
                }
            }
            catch (Exception ex)
            {
                return EnviarResultado(false, ex.Message.ToString(), "No se pudo Eliminar el ticket");
                throw;
            }
        }

        public JsonResult GuardarBitacoraGeneralhistorial(int idusuario, int idticket, int idusuariosolicitante, string comentario, int idapp, int idestado, int idusuarioasignado,int idarea)
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
        public ActionResult DetalleTicket(int idticket)
        {
            using (var contexto = new SARISEntities1())
            {
                var idusuario = GetIdUser();
                var cont = contexto.sp_DetalleBitacoraInformacion(GetIdUser(), idticket).FirstOrDefault();
                var tick = new TicketInformacionViewModel();
                tick.fcClaseColor = cont.fcClaseColor;
                tick.fcDescripcionCategoria = cont.fcDescripcionCategoria;
                tick.fcDescripcionEstado = cont.fcDescripcionEstado;
                tick.fcDescripcionRequerimiento = cont.fcDescripcionRequerimiento;
                tick.fcNombreUsuarioSolicitante = cont.fcNombreUsuarioSolicitante;
                tick.fcTipoRequerimiento = cont.fcTipoRequerimiento;
                tick.fdFechaCreacion = cont.fdFechaCreacion;
                tick.fiIDRequerimiento = cont.fiIDRequerimiento;
                tick.fcTituloRequerimiento = cont.fcTituloRequerimiento;
                tick.fiIDImpacto = (int)cont.fiIDImpacto;
                tick.fcDescripcionImpacto = cont.fcDescripcionImpacto;
                tick.fiIDPrioridad = (int)cont.fiIDPrioridad;
                tick.fcDescripcionPrioridad = cont.fcDescripcionPrioridad;
                tick.fiIDUrgencia = (int)cont.fiIDUrgencia;
                tick.fcDescripcionUrgencia = cont.fcDescripcionUrgencia;
                tick.fcDescripcionCategoriaResolucion = cont.fcDescripcionCategoriaResolucion;
                tick.fcDescripcionSubCategoriaResolucion = cont.fcDescripcionSubCategoriaResolucion;
                tick.fcNombreMotivo = cont.fcNombreMotivo;

                tick.fcPlataforma = cont.fcNombrePlataforma;
                tick.fcServicioAfectados = cont.fcNombreServicio;
                tick.fdFechaAlarmaDeteccion = (cont.fdFechaAlarmaDeteccion is null) ? DateTime.Now : (DateTime)cont.fdFechaAlarmaDeteccion;


                ViewBag.DatosDocumentoListado = contexto.sp_DetalleBitacoraInformacionArchivos(GetIdUser(), idticket).ToList();
                ViewBag.ServiciosAfectados = contexto.sp_RequerimientoPorServicioByRequerimiento(idticket).ToList();
                ViewBag.CI = contexto.sp_CIporIncidencias_listos(idticket).ToList();
                return PartialView(tick);
            }
        }

        public ActionResult AgregarServicios()
        {
            return PartialView();
        }

        public ActionResult SubTablaIncidentesTicket(int idticket) // se hiba a hacer para la subtabla pero por alguna razon no funcionaba el onclick del detalle asi dejarlo para otra ocacion que si agarre el onclick
        {
            using (var contexto = new SARISEntities1())
            {
                ViewBag.ServiciosAfectados = contexto.sp_RequerimientoPorServicioByRequerimiento(idticket).ToList();
                ViewBag.CI = contexto.sp_CIporIncidencias_listos(idticket).ToList();
                return PartialView();
            }

        }
        ///////////////////////////// LLenar campos
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

        public ActionResult AgregarCI()
        {
            return PartialView();
        }

    }
}