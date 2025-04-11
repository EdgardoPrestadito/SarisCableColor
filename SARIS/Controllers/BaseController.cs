using Microsoft.AspNet.SignalR;
using OrionCoreCableColor.App_Helper;
using OrionCoreCableColor.App_Services.EmailService;
using OrionCoreCableColor.App_Services.ReportesService;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.DbConnection.CMContext;
using OrionCoreCableColor.Models.Base;
using OrionCoreCableColor.Models.EmailTemplateService;
using OrionCoreCableColor.Models.Ticket;
using OrionCoreCableColor.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;


namespace OrionCoreCableColor.Controllers
{
    public class BaseController : Controller
    {

        private DbServiceConnection _connection;
        public BaseController()
        {
            
            
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            return base.BeginExecuteCore(callback, state);
        }

        public void enviarMiUsuario(InfoUsuarioViewModel model)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.enviarMiUsuario(model);
        }


        [HttpPost]
        public JsonResult RegistrarUsuariosLogueados(InfoUsuarioViewModel model, string usuarioPeticion)
        {
          

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.insertarUsuarios(model, usuarioPeticion);



            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //public List<ListaDeUsuariosViewModel> GetUsuariosLogueados()
        //{
        //    //return MemoryLoadManager.ListaUsuarios.Where(x => x.InfoUsuario.Any()).ToList();
        //}
        [AllowAnonymous]
        public void EnviarNotificacion(string mensajeNotificacion)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.enviarNotificacion(mensajeNotificacion);
        }

        public string EnviarFormularioSignalR<T>(ObjSignalRModalViewModel model) where T : class
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
                hubContext.Clients.All.mostrarModal(new { model = (T)model.Obj, url = model.Url, user = model.InfoUsuario });
                return "Bueno";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string EnviarFormularioSignalRStruct<T>(ObjSignalRModalViewModel model) where T : struct
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
                hubContext.Clients.All.mostrarModal(new { model = model.Obj, url = model.Url, user = model.InfoUsuario });
                return "Bueno";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public JsonResult EnviarResultado(bool resultado, string Titulo)
        {
            return Json(new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = resultado ? "Action Successful" : "Error",
                Estado = resultado

            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EnviarResultado(bool resultado, string Titulo, string Mensaje)
        {
            return Json(new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = Mensaje,
                Estado = resultado,
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EnviarResultado(bool resultado, string Titulo, string Mensaje, int Id)
        {
            return Json(new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = Mensaje,
                Estado = resultado,
                Id = Id
            }, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public void EscribirEnLogJson(string mensaje)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.recibirNoficicacionJson(new NotificacionViewModel
            {
                fiIDUsuario = GetIdUser(),
                fdFechaTransaccion = DateTime.Now,
                fcUsuario = ((User?.Identity?.Name ?? "") == "") ? "sistembot" : User?.Identity?.Name ?? "sistembot",
                fcOperacion = mensaje,
                fcTipoTransaccion = "Transaccion",
                fcClase = "success"
            });
        }
        public JsonResult EnviarResultado(bool resultado, string Titulo, string Mensaje, string Correlativo)
        {
            return Json(new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = Mensaje,
                Estado = resultado,
                Correlativo = Correlativo
            }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult EnviarException(Exception e, string Titulo)
        {

            if (MemoryLoadManager.Produccion)
            {
                RegistrarError(e);
            }


            return Json(new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = (e.InnerException?.Message ?? e.Message) + ": " + e.StackTrace,
                Estado = false,

            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EnviarListaJson(object e)
        {
            var jsonResult = Json(e, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;

        }

        public List<string> PreguntasAleatorias(List<string> lista, string opcionCorrecta, int tipoLista)
        {
            Random rand = new Random();
            //preguntas


            //si es 1 hacer randon de respuestas
            if (tipoLista == 1)
            {
                lista = lista.Where(x => x != opcionCorrecta).Select(x => x).ToList();
                var listaArreglo = lista.ToArray();
                int cantidaddeElementos = lista.Count;
                int numeroAlzar = rand.Next(1, cantidaddeElementos);
                int numeroAlzar2 = rand.Next(1, cantidaddeElementos);
                string opcion1AlAzar = listaArreglo[numeroAlzar];
                string opcion2AlAzar = listaArreglo[numeroAlzar2];
                while (numeroAlzar2 == numeroAlzar)
                {
                    numeroAlzar2 = rand.Next(1, cantidaddeElementos);
                    opcion2AlAzar = listaArreglo[numeroAlzar2];
                }

                var ListaRetornar = new List<string> { opcion1AlAzar, opcion2AlAzar, opcionCorrecta };

                ListaRetornar = ListaRetornar.OrderBy(x => rand.Next()).ToList();
                return ListaRetornar;
            }
            // sino solo retornar lista
            else
            {
                return lista;
            }


        }
        public string[] PreguntasATomar()
        {
            Random rand = new Random();
            List<int> preguntasAgregada = new List<int>();
            int numeroPregunta = rand.Next(1, 7);
            preguntasAgregada.Add(numeroPregunta);
            int numeroPregunta2 = rand.Next(1, 7);
            while (preguntasAgregada.Contains(numeroPregunta2))
            {
                numeroPregunta2 = rand.Next(1, 7);
            }
            preguntasAgregada.Add(numeroPregunta2);

            int numeroPregunta3 = rand.Next(1, 7);
            preguntasAgregada.Add(numeroPregunta3);
            while (preguntasAgregada.Contains(numeroPregunta3))
            {
                numeroPregunta3 = rand.Next(1, 7);
            }


            
            var ListadoSeleccionadoPreguntas = new List<string>() { "Vive", "Trabaja", "Parentesco", "Estado Civil", "Ciudad", "Tiene Hijos" }.ToArray();
            string pregunta1Seleccionada = ListadoSeleccionadoPreguntas[numeroPregunta - 1];
            string pregunta2Seleccionada = ListadoSeleccionadoPreguntas[numeroPregunta2 - 1];
            string pregunta3Seleccionada = ListadoSeleccionadoPreguntas[numeroPregunta3 - 1];

            return new List<string>() { pregunta1Seleccionada, pregunta2Seleccionada, pregunta3Seleccionada }.ToArray();



        }


        public List<int> ListaDias()
        {
            var lista = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                lista.Add(i);
            }
            return lista;
        }
        public List<int> ListaMeses()
        {
            var lista = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                lista.Add(i);
            }
            return lista;
        }
        public List<int> ListaAnio()
        {
            int anioActual = DateTime.Now.Year;
            int aniolimite = anioActual - 40;
            var lista = new List<int>();
            for (int i = anioActual; i >= aniolimite; i--)
            {
                lista.Add(i);
            }
            return lista;
        }

        public void agregarCreacionTicket(int idticket)// para cuendo se crea el ticket
        {
            //var model = contexto.sp_Solicitudes_Bandeja_ObtenerPorIDSolicitud(fiIDSolicitud).FirstOrDefault();
            //var model = _connection.SarisContext.sp_Requerimientos_Bandeja_ByID(1,1,idequifax).FirstOrDefault();

            var listaTicket = new List<TicketMiewModel>();
            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_Requerimientos_Bandeja_ByID {1},{1},{GetIdUser()},{idticket}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());


                        listaTicket = db.ObjectContext.Translate<TicketMiewModel>(reader).ToList();
                    }

                    connection.Close();

                }
            }
            catch (Exception e)
            {
                throw;
            }

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.agregarrow(listaTicket.FirstOrDefault());

        }

        public void agregarDatosTicketCerrados(int idticket)//para cuando el ticket pasa a estado de cerrado
        {
            using (var contexto = new SARISEntities1())
            {
                var model = contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).ToList();
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
                hubContext.Clients.All.agregarrowTicketCerrado(model.FirstOrDefault());
            }

            //hubContext.Clients.All.agregarrowTicketCerrado(listaTicket.FirstOrDefault());

        }

        public void ObtenerDataTicket(int idticket)
        {
            using (var contexto = new SARISEntities1())
            {
                var model = contexto.sp_Requerimientos_Bandeja_ByID(1, 1, GetIdUser(), idticket).ToList();
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
                hubContext.Clients.All.actualizarBandeja(model.FirstOrDefault());
            }
        }

        public void eliminarTicketAbierto(int IdTicket) // adaptarlo al de ticket
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.eliminarrow(IdTicket);
        }


        public ListaDeUsuariosViewModel GetUser()
        {
            return MemoryLoadManager.ListaUsuarios.FirstOrDefault(x => x.UserName == User.Identity.Name);
        }

        public async Task<ListaDeUsuariosViewModel> GetUserAsync()
        {
            return MemoryLoadManager.ListaUsuarios.FirstOrDefault(x => x.UserName == User.Identity.Name);

        }

        public string GetRol(int idRol)
        {
            using (var context = new OrionSecurityEntities())
            {
                return context.Roles.FirstOrDefault(x => x.Pk_IdRol == idRol).Nombre;
            }
        }

        public List<AspNetRoles> GetPermisos(int idRol)
        {
            using (var contexto = new OrionSecurityEntities())
            {
                return contexto.PrivilegiosPorRol.Where(x => x.Fk_IdRol == idRol).Select(x => x.AspNetRoles).ToList();
            }
        }


        [HttpPost]
        public JsonResult FormFileResponse()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     para obtener los archivos guardados en el servidor
        /// </summary>
        /// <param name="tipo">img para imagenes y pdf para pdfs</param>
        /// <param name="carpeta"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public string GetContentDocument(string tipo, string carpeta, string nombreArchivo)
        {
            using (var client = new WebClient())
            {
                var uri = $@"{MemoryLoadManager.Helper}?type={tipo}&carpeta={carpeta}&documento={nombreArchivo}";
                //var content = client.DownloadData(uri);
                //return Convert.ToBase64String(content);
                return uri;
            }
        }

        /// <summary>
        ///     para subir documentos al server desde el local
        /// </summary>
        /// <param name="carpeta">primera y ultimo carecter nunca deben de ser \</param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool UploadFileServer148(string carpeta, HttpPostedFileBase file)
        {

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    try
                    {
                        content.Add(new StreamContent(file.InputStream), "documento", file.FileName);
                        var requestUri = $@"{MemoryLoadManager.Helper}?type=guardar&carpeta={carpeta}";
                        var result = client.PostAsync(requestUri, content).Result;


                        return true;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }
                }

            }


        }

        public void ping()
        {
            //var algo = new Ping();
            var reply = new Ping().Send("190.6.197.90", 1000);
            //
        }

        public async Task<bool> UploadFileServer148Async(string carpeta, HttpPostedFileBase file)
        {

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    try
                    {
                        content.Add(new StreamContent(file.InputStream), "documento", file.FileName);
                        var requestUri = $@"{MemoryLoadManager.Helper}?type=guardar&carpeta={carpeta}";
                        var result = await client.PostAsync(requestUri, content);


                        return true;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }
                }

            }

        }


        public void RegistrarError(Exception ex)
        {
            using (var log = new EventLog("Application"))
            {
                log.Source = "Application";
                log.WriteEntry((ex.InnerException?.Message ?? ex.Message) + ": " + ex.StackTrace, EventLogEntryType.Error, 101, 1);

                var correo = new SendEmailService();
                correo.SendEmailExceptionWithOutAsync(ex, "Error de sistema");
            }
        }




    //    var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
    //    var binData = Convert.FromBase64String(base64Data);

    //        using (var stream = new MemoryStream(binData))
    //        {
    //            var Images = new Bitmap(stream);

    //      Image image = Image.FromHbitmap(Images.GetHbitmap());

    //            foreach (Section section in doc.Sections)
    //            {
    //                foreach (Paragraph paragraph in section.Paragraphs)
    //                {
    //                    foreach (DocumentObject docObj in paragraph.ChildObjects)
    //                    {
    //                        if (docObj.DocumentObjectType == DocumentObjectType.Picture)
    //                        {
    //                            DocPicture picture = docObj as DocPicture;
    //                            if (picture.Title == "Firma")
    //                            {
    //                                //Replace the image
    //                                picture.LoadImage(image);
    //                            }
    //                        }
    //                    }
    //                }
    //                //Loop through the child elements of paragraph

    //            }


    //        }



        public List<SelectListItem> GetListTipoAfectacion()
        {
            return new List<SelectListItem> {
                new SelectListItem {
                    Value = "1",
                    Text = "AUMENTA INVENTARIO"
                },
                new SelectListItem {
                    Value = "-1",
                    Text = "REBAJA INVENTARIO"
                }
            };
        }


       

        public List<SelectListItem> GetListMonedas()
        {
            return new List<SelectListItem> {
                new SelectListItem {
                    Value = "1",
                    Text = "Moneda Nacional (L)"
                },
                new SelectListItem {
                    Value = "2",
                    Text = "Moneda Extranjera ($)"
                }
            };
        }


       


        public void GetUsuariosConectados()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.consultarUsuarios(User.Identity.Name);
        }



        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public ActionResult DownloadPdf()
        {
            try
            {
                var reportStream = TempData["ReportePDF"] as MemoryStream;
                //return new FileContentResult(reportStream.ToArray(), "application/pdf");
                var archivo = new FileContentResult(reportStream.ToArray(), "application/octet-stream");
                archivo.FileDownloadName = TempData["NombreArchivo"] as string;
                return archivo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [AllowAnonymous]
        public ActionResult GetPdf()
        {
            try
            {
                var reportStream = TempData["ReportePDF"] as MemoryStream;
                //return new FileContentResult(reportStream.ToArray(), "application/pdf");
                var archivo = new FileContentResult(reportStream.ToArray(), "application/pdf");
                //archivo.FileDownloadName = TempData["NombreArchivo"] as string;

                return archivo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public void EnviarWhatsapp(string telefono, string mensaje)
        {
            MensajeriaApi.MensajesDigitales(telefono, mensaje);
        }

        public ActionResult MensajeriaMasiva()
        {
            return View();
        }


        public int GetIdUser()
        {
            using (var contexto = new SARISEntities1())
            {
                return contexto.sp_Usuarios_Maestros_ObtenerIdUsuario(User?.Identity?.Name ?? "").FirstOrDefault() ?? 281;
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public void EnviarTicket(TicketNotificacionViewModel model)
        {
            model.fdFechaHora = DateTime.Now;
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.recibirTicket(model);
        }

        //public T GetConfiguracion<T>(string llave)
        //{
        //    using (var contexto = new ORIONDBEntities())
        //    {
        //        return (T)Convert.ChangeType(contexto.sp_Configuraciones().FirstOrDefault(x => x.NombreLlave == llave).ValorLLave, typeof(T));
        //    }
        //}


        //public List<T> GetConfiguracion<T>(string llave, char Separador)
        //{
        //    using (var contexto = new ORIONDBEntities())
        //    {
        //        return contexto.sp_Configuraciones().FirstOrDefault(x => x.NombreLlave == llave).ValorLLave.Split(Separador).Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();

        //    }
        //}




    }

    public class ByteArrayHttpPostedFile : HttpPostedFileBase
    { // a partir de un archivo en formato de bytes, se creo esta clase derivada de HttpPostedFileBase que envuelva los bytes del archivo
        private readonly byte[] _fileBytes;
        private readonly string _fileName;

        public ByteArrayHttpPostedFile(string base64String, string fileName)
        {
            _fileBytes = Convert.FromBase64String(ObtenerContenidoBase64(base64String));
            _fileName = fileName;
        }

        private string ObtenerContenidoBase64(string base64String)
        {
            int index = base64String.IndexOf(',');
            if (index >= 0)
            {
                return base64String.Substring(index + 1);
            }
            return base64String;
        }

        public override int ContentLength => _fileBytes.Length;

        public override string FileName => _fileName;

        public override Stream InputStream => new MemoryStream(_fileBytes);
    }


}
