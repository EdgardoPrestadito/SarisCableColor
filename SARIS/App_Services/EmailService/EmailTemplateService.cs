using NReco.PdfGenerator;
using OrionCoreCableColor.App_Helper;
using OrionCoreCableColor.App_Services.ContratoService;
using OrionCoreCableColor.App_Services.ReportesService;
using OrionCoreCableColor.Controllers;
using OrionCoreCableColor.Models.Base;
using OrionCoreCableColor.Models.EmailTemplateService;

using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace OrionCoreCableColor.App_Services.EmailService
{
    public class EmailTemplateService
    {

        private SendEmailService _emailService;

        public EmailTemplateService()
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["OrionConnections"].ConnectionString; // DataCrypt.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString);


            _emailService = new SendEmailService();

        }

        public string pathToOpen { get; set; }
        public string pathToSave { get; set; }

        public Dictionary<string, string> DictionaryList = new Dictionary<string, string>();

        private Attachment GeneratePDFAttachment()
        {
            Document doc = new Document();
            doc.LoadFromFile(pathToOpen);

            foreach (var item in DictionaryList)
            {
                doc.Replace("{" + item.Key + "}", item.Value, true, true);
            }

            doc.SaveToFile(pathToSave, FileFormat.PDF);

            var attachmentFile = new Attachment(pathToSave, MediaTypeNames.Application.Octet);
            doc.Close();
            return attachmentFile;
        }

        private Attachment GeneratePDFAttachmentFrimadoRevisado()
        {
            Document doc = new Document();
            doc.LoadFromFile(pathToOpen);
            string data = "";
            foreach (var item in DictionaryList)
            {
                if (item.Key == "fcFirma")
                {
                    data = item.Value;
                }
                else
                {
                    doc.Replace("{" + item.Key + "}", item.Value ?? "", true, true);
                }
            }

            var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            using (var stream = new MemoryStream(binData))
            {
                var Images = new Bitmap(stream);

                Image image = Image.FromHbitmap(Images.GetHbitmap());

                foreach (Section section in doc.Sections)
                {
                    foreach (Paragraph paragraph in section.Paragraphs)
                    {
                        foreach (DocumentObject docObj in paragraph.ChildObjects)
                        {
                            if (docObj.DocumentObjectType == DocumentObjectType.Picture)
                            {
                                DocPicture picture = docObj as DocPicture;
                                if (picture.Title == "Firma")
                                {
                                    //Replace the image
                                    picture.LoadImage(image);
                                }
                            }
                        }
                    }
                    //Loop through the child elements of paragraph

                }


            }

            doc.SaveToFile(pathToSave, FileFormat.PDF);

            var attachmentFile = new Attachment(pathToSave, MediaTypeNames.Application.Octet);
            doc.Close();
            return attachmentFile;
        }

        private Attachment GeneratePDFAttachmentFrimado()
        {
            Document doc = new Document();
            doc.LoadFromFile(pathToOpen);
            string data = "";
            foreach (var item in DictionaryList)
            {
                if (item.Key == "fcFirma")
                {
                    data = item.Value;
                }
                else
                {
                    doc.Replace("{" + item.Key + "}", item.Value ?? "", true, true);
                }
            }

            var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            using (var stream = new MemoryStream(binData))
            {
                var Images = new Bitmap(stream);

                Image image = Image.FromHbitmap(Images.GetHbitmap());

                foreach (Section section in doc.Sections)
                {
                    foreach (Paragraph paragraph in section.Paragraphs)
                    {
                        foreach (DocumentObject docObj in paragraph.ChildObjects)
                        {
                            if (docObj.DocumentObjectType == DocumentObjectType.Picture)
                            {
                                DocPicture picture = docObj as DocPicture;
                                if (picture.Title == "Firma")
                                {
                                    //Replace the image
                                    picture.LoadImage(image);
                                }
                            }
                        }
                    }
                    //Loop through the child elements of paragraph

                }


            }

            doc.SaveToFile(pathToSave, FileFormat.PDF);

            var attachmentFile = new Attachment(pathToSave, MediaTypeNames.Application.Octet);
            doc.Close();
            return attachmentFile;
        }



        public string ParseVariablesInEmailTemplate(string emailTemplate, Dictionary<string, string> variableValues)
        {

            var codeStartDelimiter = "{";
            var codeEndDelimiter = "}";

            var escapeCharacters = new[] { ".", "$", "{", "}", "[", "]", "^", "(", ")", "|", "*", "+", "?", @"\" };

            var delStartReg = $"{codeStartDelimiter}";
            var delEndReg = $"{codeEndDelimiter}";

            if (escapeCharacters.Contains(delStartReg)) delStartReg = $"\\{delStartReg}";
            if (escapeCharacters.Contains(delEndReg)) delEndReg = $"\\{delEndReg}";

            var regexp = new Regex(delStartReg + "[^" + delStartReg + delEndReg + "]*" + delEndReg);

            var matches = regexp.Matches(emailTemplate);

            foreach (Match match in matches)
            {
                var code = match.Value.Replace(codeStartDelimiter, "");
                code = code.Replace(codeEndDelimiter, "");

                var codeResult = variableValues[code];

                //try
                //{
                //    codeResult = _evaluateExpression.EvaluateCodeSnippet<string>(code).Result;
                //}
                //catch (Exception e)
                //{
                //    throw _exceptionFactory.GetException(25084, code);
                //}

                emailTemplate = emailTemplate.Replace(match.Value, codeResult);

            }

            return emailTemplate;

        }


        public async Task<bool> SendEmailToSolicitudVarios(EmailTemplateTicketModel model)
        {
            try
            {


                if (model.fiIDEstadoRequerimiento == 5)
                {
                    model.fcDescripcionRequerimiento = "Cerrado";
                }

                if (model.fiIDEstadoRequerimiento == 6)
                {
                    model.fcDescripcionRequerimiento = "Cancelado";
                }

                var htmlString = $@"
                                    <!DOCTYPE html>
                                    <html lang=""es"">
                                    <head>
                                        <meta charset=""UTF-8"">
                                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                        <title>Incidente</title>
                                        <style>
                                            body {{
                                                font-family: Arial, sans-serif;
                                                margin: 0;
                                                padding: 0;
                                                background-color: ##d3e5e2;
                                            }}
                                            table {{
                                                width: 80%;
                                                max-width: 600px;
                                                margin: 20px auto;
                                                background-color: #fff;
                                                border-radius: 8px;
                                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                            }}
                                            h1 {{
                                                color: #333;
                                                padding: 20px;
                                                margin: 0;
                                            }}
                                            p {{
                                                color: #666;
                                                margin: 5px;
                                            }}
                                            th, td {{
                                                padding: 10px;
                                                border-bottom: 1px solid #ddd;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th colspan=""2""><h1>Incidente #{model.fiIDRequerimiento}</h1></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td><strong>Usuario Solicitante:</strong></td>
                                                    <td>{model.fcNombreCorto}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Area Solicitante:</strong></td>
                                                    <td>{model.fcAreaSolicitante}</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td><strong>Título:</strong></td>
                                                    <td>{model.fcTituloRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Descripción:</strong></td>
                                                    <td>{model.fcDescripcionRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Comentario:</strong></td>
                                                    <td>{model.fcComentario}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Categoría:</strong></td>
                                                    <td>{model.fcDescripcionCategoria}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Incidente:</strong></td>
                                                    <td>{model.fcTipoRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Estado:</strong></td>
                                                    <td>{model.fcDescripcionEstado}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Prioridad:</strong></td>
                                                    <td>{model.fcDescripcionPrioridad}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Fecha de Creación:</strong></td>
                                                    <td>{model.fdFechaCreacion.ToString("MM/dd/yyyy hh:mm tt")}</td>
                                                </tr>
                                                <tr>
                                                    <td colspan=""2"">
                                                        <p>Por favor, tome nota del número de Incidente: <strong>#{model.fiIDRequerimiento}</strong>. Por favor no Contestar este Correo.</p>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </body>
                                    </html>

                                    ";



                var emailGeneratedToSend = new SendEmailViewModel
                {
                    EmailName = "Saris",
                    Subject = "Incidente",
                    Body = htmlString,
                    ListDestinationEmail = model.ListCustomerEmail,
                    List_CC = model.List_CC
                };

                var SendEmailResult = await _emailService.SendEmailManyDestinationsAsync(emailGeneratedToSend);
                return SendEmailResult;

            }
            catch (Exception e)
            {
                await _emailService.SendEmailException(e, "Send Email");

                return false;
            }
        }


        public async Task<bool> SendEmailPresonalizadoAVarios(EmailTemplateServiceModel model)
        {
            try
            {

                var emailGeneratedToSend = new SendEmailViewModel
                {
                    EmailName = "NovaNet",
                    Subject = model.Comment,
                    Body = model.HtmlBody,
                    ListDestinationEmail = model.ListCustomerEmail,
                    List_CC = model.List_CC
                };

                var SendEmailResult = await _emailService.SendEmailManyDestinationsAsync(emailGeneratedToSend);
                return SendEmailResult;
            }
            catch (Exception e)
            {
                //await _emailService.SendEmailException(e, "Send Email");
                var baseController = new BaseController();
                //baseController.EscribirEnLogJson(new NotificacionViewModel { fcClase = "danger", fcTipoTransaccion = "Error", fcOperacion = (e.InnerException?.Message ?? e.Message) + ": " + e.StackTrace });
                return false;
            }


        }



        public async Task<bool> SendEmailToSolicitud(EmailTemplateTicketModel model)
        {
            try
            {


                if (model.fiIDEstadoRequerimiento == 5)
                {
                    model.fcDescripcionRequerimiento = "Cerrado";
                }

                if (model.fiIDEstadoRequerimiento == 6)
                {
                    model.fcDescripcionRequerimiento = "Cancelado";
                }

                var htmlString = $@"
                                    <!DOCTYPE html>
                                    <html lang=""es"">
                                    <head>
                                        <meta charset=""UTF-8"">
                                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                        <title>Incidente</title>
                                        <style>
                                            body {{
                                                font-family: Arial, sans-serif;
                                                margin: 0;
                                                padding: 0;
                                                background-color: ##d3e5e2;
                                            }}
                                            table {{
                                                width: 80%;
                                                max-width: 600px;
                                                margin: 20px auto;
                                                background-color: #fff;
                                                border-radius: 8px;
                                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                            }}
                                            h1 {{
                                                color: #333;
                                                padding: 20px;
                                                margin: 0;
                                            }}
                                            p {{
                                                color: #666;
                                                margin: 5px;
                                            }}
                                            th, td {{
                                                padding: 10px;
                                                border-bottom: 1px solid #ddd;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th colspan=""2""><h1>Incidente #{model.fiIDRequerimiento}</h1></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td><strong>Usuario Solicitante:</strong></td>
                                                    <td>{model.fcNombreCorto}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Area Solicitante:</strong></td>
                                                    <td>{model.fcAreaSolicitante}</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td><strong>Título:</strong></td>
                                                    <td>{model.fcTituloRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Descripción:</strong></td>
                                                    <td>{model.fcDescripcionRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Comentario:</strong></td>
                                                    <td>{model.fcComentario}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Categoría:</strong></td>
                                                    <td>{model.fcDescripcionCategoria}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Incidente:</strong></td>
                                                    <td>{model.fcTipoRequerimiento}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Estado:</strong></td>
                                                    <td>{model.fcDescripcionEstado}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Prioridad:</strong></td>
                                                    <td>{model.fcDescripcionPrioridad}</td>
                                                </tr>
                                                <tr>
                                                    <td><strong>Fecha de Creación:</strong></td>
                                                    <td>{model.fdFechaCreacion.ToString("MM/dd/yyyy hh:mm tt")}</td>
                                                </tr>
                                                <tr>
                                                    <td colspan=""2"">
                                                        <p>Por favor, tome nota del número de Incidente: <strong>#{model.fiIDRequerimiento}</strong>. Por favor no Contestar este Correo.</p>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </body>
                                    </html>

                                    ";



                var emailGeneratedToSend = new SendEmailViewModel
                {
                    EmailName = "Saris",
                    Subject = "Incidencia",
                    Body = htmlString,
                    DestinationEmail = model.fcCorreoElectronico,
                };

                var SendEmailResult = await _emailService.SendEmailAsync(emailGeneratedToSend);



                return SendEmailResult;
            }
            catch (Exception e)
            {
                await _emailService.SendEmailException(e, "Send Email");

                return false;
            }


        }

       


        public async Task<bool> SendEmailPresonalizado(EmailTemplateServiceModel model)
        {
            try
            {

                var emailGeneratedToSend = new SendEmailViewModel
                {
                    EmailName = "NovaNet",
                    Subject = model.Comment,
                    Body = model.HtmlBody,
                    DestinationEmail = model.CustomerEmail,
                };

                var SendEmailResult = await _emailService.SendEmailAsync(emailGeneratedToSend);
                return SendEmailResult;
            }
            catch (Exception e)
            {
                await _emailService.SendEmailException(e, "Send Email");

                return false;
            }


        }



        public SendEmailViewModel GenerarCorreoGerencia(EmailTemplateServiceModel model)
        {

            Attachment fileAttachment = null;

            var CustomerAttachmentName = $"Reporte Gerencia Novanet del  {DateTime.Now:dd-mm-yyyy}.pdf";
            var fileStreamInventario = new StreamReader(model.Archivo.InputStream, System.Text.Encoding.Default, false);
            fileAttachment = new Attachment(fileStreamInventario.BaseStream, CustomerAttachmentName);

            var modelCorreo = new SendEmailViewModel
            {
                EmailName = "Reporte Gerencial",
                Subject = "Reporte Dia General Novanet",
                Body = $"Reporte de Gerencia ",
                DestinationEmail = model.CustomerEmail,

                List_CC = new List<string>(),
                firma = "",
                Attachment = fileAttachment
            };

            return modelCorreo;



        }


        public async Task<bool> SendEmailToGerencia(EmailTemplateServiceModel model)
        {
            try
            {
                var emailGeneratedToSend = GenerarCorreoGerencia(model);
                var SendEmailResult = await _emailService.SendEmailAsync(emailGeneratedToSend);
                return SendEmailResult;
            }
            catch (Exception e)
            {
                await _emailService.SendEmailException(e, "Send Email");

                return false;
            }


        }



    }
}
