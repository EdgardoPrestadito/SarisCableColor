using Microsoft.AspNet.SignalR;
using OrionCoreCableColor.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OrionCoreCableColor
{
    public class NotificacionesHub:Hub
    {
        private string NombreSala = "Notificaciones";
       
        public async Task  EnviarNotificacion(string mensajeNotificaciones)
        {
            try
            {

                // var user_ = Context.ConnectionId;
                await Task.Run(Clients.All.enviarNotificacion(mensajeNotificaciones));
                //this.Clients.All.enviarNotificacion(mensajeNotificaciones);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task InsertarUsuarios(InfoUsuarioViewModel model)
        {
            await Task.Run(Clients.All.insertarUsuarios(model));
        }

        public void AgregarSesion()
        {        
                 Groups.Add(Context.ConnectionId, NombreSala);              
        }
    }
}