using OrionCoreCableColor.DbConnection.CMContext;

using OrionCoreCableColor.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.App_Helper
{
    public static class MemoryLoadManager
    {
    
        public static List<ListaDeUsuariosViewModel> ListaUsuarios { get; set; }
        public static string URL = HttpContext.Current.Server.MapPath("");
        public static bool Produccion = ConfigurationManager.AppSettings["Produccion"] == "true" ? true : false;
        public static string UrlEquifax = ConfigurationManager.AppSettings["UrlEquifax"];
        public static string Helper = ConfigurationManager.AppSettings["Helper"];
        public static string UrlWeb = ConfigurationManager.AppSettings["UrlAplicacion"];
        public static string UrlContrato = ConfigurationManager.AppSettings["UrlContrato"];
        public static string EmailSystemAdministrator = "kevin.santos@miprestadito.com";
        public static string VirtualPathServerToEmailTemplates = "~/Documento/Recursos/Plantilla/";
        public static string VirtualPathServerToCustomerAttachment = "~/Documento/Recursos/Solicitud/";
        public static int UsuarioSystemOrionCoreSeguridad = 281;
        

        
        public static void LoadMemory()
        {
            
            using (var context = new OrionSecurityEntities())
            {
                var list = context.Usuarios.ToList().Select(x => new ListaDeUsuariosViewModel
                {
                    fiIdUsuario = x.fiIDUsuario,
                    fcPrimerNombre = x.fcPrimerNombre,
                    fcPrimerApellido = x.fcPrimerApellido,
                    //FechaNacimiento = x.FechaNacimiento,
                    fiEstado = x.fiEstado,
                    fcBuzondeCorreo = x.AspNetUsers.Email,
                    fcTelefonoMovil = x.fcTelefonoMovil,
                    UserName = x.AspNetUsers.UserName,
                    NombreRol = x.RolesPorUsuario.Any() ? x.RolesPorUsuario.FirstOrDefault().Roles.Nombre ?? "" : "",
                    fcIdAspNetUser = x.fcIdAspNetUser,
                    // = x.IdUsuarioCoreSeguridad,
                    fcUrlImage = x.fcUrlImage,
                    IdRol = x.RolesPorUsuario.Any() ? x.RolesPorUsuario.FirstOrDefault().Roles.Pk_IdRol : 0,
                    //InfoUsuario = new List<InfoUsuarioViewModel>(),
                    fiIDEmpresa=0
                }).ToList();

                ListaUsuarios = list;


            }
        }
    }
}