
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace OrionCoreCableColor.App_Helper
{
    public static class MensajeriaApi
    {
        public static string MensajesDigitales(string pcTelefonoDestino, string pcMensaje, string pcURLAdjunto = "")
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://srv2.rob.chat/REST_API/RobChat/EnviarTexto");
            request.Headers.Add("comercioId", "");
            request.Headers.Add("token", "");
            request.Headers.Add("key", "");

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("20"), "comercioId");
            content.Add(new StringContent("RC20"), "token");
            content.Add(new StringContent("e8079b7057113deee2fa473177420de1"), "key");
            content.Add(new StringContent("504" + pcTelefonoDestino), "para");
            content.Add(new StringContent(pcMensaje), "mensaje");
            content.Add(new StringContent(pcURLAdjunto), "adjunto");
            request.Content = content;
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public static void EnviarNumeroTicket(string nombreUsuario, int IDTIcket, string pcTelefono, string titulo, string descripcion, string Categoria, string incidencia,string Estado,string Prioridad)
        {
            var mensaje = @"SARIS le informa" + "\n Estimado Usuario: " + nombreUsuario + " se le asigno un ticket\n" + "\n*Ticket #" + IDTIcket + "*" + "\n*Titulo:* " + titulo + "\n*Descripción:* " + descripcion + "\n*Categoria:* " + Categoria + "\n*Incidencia:* " + incidencia + "\n*Estado Ticket:* " + Estado + "\n*Prioridad:* " + Estado;
            MensajesDigitales(pcTelefono, mensaje, "");
        }

        public static void EnviarSMSPersonalizado(string nombreCliente, string pcTelefono, string Mensaje)
        {
            var msj = $"NOVANET le informa\n Estimado Cliente: {nombreCliente} \n {Mensaje}";
            MensajesDigitales(pcTelefono, msj);
        }

        public static void EnviarNumero(string nombreCliente, int idEquiFax, string pcTelefono)
        {
            var msj = $"NOVANET le informa \nEstimado Cliente: {nombreCliente} nos ayuda llenando los siguientes datos del siguiente link: {MemoryLoadManager.UrlWeb}/DatosCliente/ViewFormDatosCliente/{idEquiFax}";
            MensajesDigitales(pcTelefono,msj);
        }

        public static void EnviarNumeroReferencias(int fiIDReferencia, string nombrerefencia, string nombreCliente, int idEquiFax, string pcTelefono, string mensaje)
        {
            
            var msj = $"NOVANET le informa \nEstimado: {nombrerefencia} ha recibido este mensaje, porque la persona  {nombreCliente} lo ha puesto como referencia, nos ayuda llenando los siguientes datos del siguiente link: {MemoryLoadManager.UrlWeb}/DatosCliente/ViewFormReferenciasCliente/{fiIDReferencia}";
            MensajesDigitales(pcTelefono, msj);

        }

        public static void EnviarLinkPago(string nombreCliente, int idEquiFax, string pcTelefono, string LinkPago, string correo)
        {
            var msj = $"NOVANET le informa \nEstimado Cliente: {nombreCliente} Se le envio su link de pago a su correo Electronico: {correo}";
            MensajesDigitales(pcTelefono, msj);
            
        }

        public static void EnviarMensajeInstalacionaCliente(string nombreCliente, int idEquiFax, string pcTelefono, string tecnico, string informacionPaquete, string telefonoTecnico, string identidad)
        {
            try
            {
                string urlqr = MemoryLoadManager.UrlWeb + "/DatosCliente/ViewFormQR/" + idEquiFax;
               
                var msj = $"NOVANET le informa \nEstimado cliente: {nombreCliente} se le notifica que la instalacion de su servicio sera realizada por el tecnico: {tecnico}, con identidad: {identidad} \nSe comunicara con usted de este telefono: {telefonoTecnico} \n {informacionPaquete} \nTambien se le adjunta el codigo QR de su instalacion.";
                MensajesDigitales(pcTelefono, msj, HttpUtility.UrlEncode(urlqr));

            }
            catch (Exception e)
            {

                throw e;
            }

        }

    }
}