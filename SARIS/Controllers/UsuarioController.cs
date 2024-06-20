using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using OrionCoreCableColor.DbConnection.CMContext;
using OrionCoreCableColor.Models;
using OrionCoreCableColor.Models.Usuario;
using System.Data.Entity;
using System.Threading.Tasks;
using OrionCoreCableColor.DbConnection;
using Microsoft.AspNet.Identity;

namespace OrionCoreCableColor.Controllers
{
    public class UsuarioController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult CargarListaUsuarios()
        {
            try
            {
                using (var context = new OrionSecurityEntities())
                {
                    var jsonResult = Json(context.Usuarios.Select(x => new ListaDeUsuariosViewModel
                    {
                        fiIdUsuario = x.fiIDUsuario,
                        fcPrimerNombre = x.fcPrimerNombre,
                        fcSegundoNombre = x.fcSegundoNombre,
                        fcSegundoApellido = x.fcSegundoApellido,
                        fcPrimerApellido = x.fcPrimerApellido,
                        //FechaNacimiento = x.FechaNacimiento,
                        fiEstado = x.fiEstado,
                        fcBuzondeCorreo = x.fcBuzondeCorreo,
                        fcTelefonoMovil = x.fcTelefonoMovil,
                        UserName = x.AspNetUsers.UserName,
                        NombreRol = x.RolesPorUsuario.Any() ? x.RolesPorUsuario.FirstOrDefault().Roles.Nombre ?? "" : "",
                        fiAreaAsignada = x.fiAreaAsignada,
                        fcAreaAsignada = x.Area.fcDescripcion

                    }).Where(b => b.fcBuzondeCorreo != "kevin.santos@miprestadito.com").ToList(), JsonRequestBehavior.AllowGet);
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
        public ActionResult CrearUsuario()
        {
            using (var contextSaris = new SARISEntities1())
            {


                using (var context = new OrionSecurityEntities())
                {
                    var usuarioLogueado = GetUser();
                    //var rol = 1; // GetRol(usuarioLogueado.IdRol);
                    //var permisos = GetPermisos(usuarioLogueado.IdRol);
                    ViewBag.ListaAreas = contextSaris.sp_Requerimiento_Areas(1, 1, 1).ToList().Select(x => new SelectListItem { Value = x.fiIDArea.ToString(), Text = x.fcDescripcion }).ToList();
                    ViewBag.ListaRoles = context.Roles.Where(x => x.Activo).Select(x => new SelectListItem { Value = x.Pk_IdRol.ToString(), Text = x.Nombre }).ToList();
                    //if (rol == "Orion_Contratista")
                    //{
                    //    ViewBag.ListaRoles = context.Roles.Where(x => x.Activo && x.Pk_IdRol == 3).Select(x => new SelectListItem { Value = x.Pk_IdRol.ToString(), Text = x.Nombre }).ToList();
                    //}

                    return View(new CrearUsuarioViewModel { fdFechaAlta = DateTime.Now });
                }
            }
        }


        //[HttpGet]
        //public ActionResult AgregarUsuarioTecnico(int id)
        //{
        //    using (var contextOrion = new ORIONDBEntities())
        //    {
        //        using (var context = new OrionSecurityEntities())
        //        {
        //            ViewBag.ListaEmpresas = contextOrion.sp_Empresas_Maestro_Listar().ToList().Select(x => new SelectListItem { Value = x.fiIDEmpresa.ToString(), Text = x.fcNombreComercial }).ToList();
        //            ViewBag.ListaRoles = context.Roles.Where(x => x.Activo && x.Pk_IdRol == 3).Select(x => new SelectListItem { Value = x.Pk_IdRol.ToString(), Text = x.Nombre }).ToList();
        //            return View("CrearUsuario", new CrearUsuarioViewModel { fdFechaAlta = DateTime.Now, fbTecnico = true, fiIdUsuario = id, });
        //        }
        //    }

        //}

        [HttpPost]
        public async Task<ActionResult> CrearUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new OrionSecurityEntities())
                {
                    var user = new ApplicationUser { UserName = model.UserName.Trim(), Email = model.fcBuzondeCorreo.Trim() };
                    var result = await UserManager.CreateAsync(user, model.fcPassword.Trim());
                    var usuario = context.AspNetUsers.FirstOrDefault(x => x.UserName == model.UserName);
                    if (result.Succeeded)
                    {

                        var nuevoUsuario = context.Usuarios.Add(new Usuarios_Maestro
                        {
                            fcPrimerNombre = model.fcPrimerNombre.Trim(),
                            fcSegundoNombre = model.fcSegundoNombre.Trim(),
                            fcPrimerApellido = model.fcPrimerApellido.Trim(),
                            fcSegundoApellido = model.fcSegundoApellido.Trim(),
                            //FechaNacimiento = model.FechaNacimiento,
                            fcNombreCorto = model.UserName,
                            fcCentrodeCosto = "0100",
                            fiIDPuesto = (short)model.IdRol,
                            fiTipoUsuario = 1,
                            fiIDDepartamento = 1,
                            fiIDJefeInmediato = 1,
                            fcUsuarioDominio = model.UserName,
                            fiIDDominioRed = 1,
                            fcPassword = usuario.PasswordHash,
                            fcPasswordToken = user.PasswordHash,
                            fdFechaUltimoCambiodePassword = DateTime.Now,
                            fcBuzondeCorreo = model.fcBuzondeCorreo.Trim(),
                            fiIDDominioCorreo = 1,
                            fcDireccionFisica = "direccion",
                            fcDocumentoIdentificacion = model.fcDocumentoIdentificacion,
                            fdFechaAlta = DateTime.Now,
                            fiIDUsuarioSolicitante = 1,
                            fiIDUsuarioCreador = 1,
                            fiEstado = 1,
                            fdFechaBaja = null,
                            fcTelefonoMovil = model.fcTelefonoMovil,
                            fcToken = Guid.NewGuid().ToString(),
                            fcIdAspNetUser = usuario.Id,
                            fiIDEmpresa = model.fiIDEmpresa ?? 0,
                            fiAreaAsignada = model.fiAreaAsignada




                        });

                        nuevoUsuario.RolesPorUsuario.Add(new RolesPorUsuario
                        {
                            Fk_IdRol = model.IdRol
                        });

                        var ListaPermisos = context.PrivilegiosPorRol.Where(x => x.Fk_IdRol == model.IdRol).Select(z => z.AspNetRoles.Name).ToList();
                        foreach (var permiso in ListaPermisos)
                        {
                            await UserManager.AddToRoleAsync(user.Id, permiso);
                        }

                        var resultado = context.SaveChanges() > 0;
                        return EnviarResultado(resultado, "Crear Usuario");
                    }
                    else
                    {
                        return EnviarResultado(result.Succeeded, result.Errors.FirstOrDefault());

                    }
                }
            }
            catch (Exception e)
            {
                return EnviarResultado(false, e.Message);
            }
        }




        [HttpPost]
        public async Task<ActionResult> CrearTecnico(CrearUsuarioViewModel model)
        {
            try
            {
                using (var contextSaris = new SARISEntities1())
                {

                    model.fiAreaAsignada = contextSaris.sp_UsuarioMaestro_ObtenerIdAreaAsignada(GetIdUser()).FirstOrDefault() ?? 0;
                    using (var context = new OrionSecurityEntities())
                    {
                        var user = new ApplicationUser { UserName = model.UserName.Trim(), Email = model.fcBuzondeCorreo.Trim() };
                        var result = await UserManager.CreateAsync(user, model.fcPassword.Trim());
                        var usuario = context.AspNetUsers.FirstOrDefault(x => x.UserName == model.UserName);
                        if (result.Succeeded)
                        {

                            var nuevoUsuario = context.Usuarios.Add(new Usuarios_Maestro
                            {
                                fcPrimerNombre = model.fcPrimerNombre.Trim(),
                                fcSegundoNombre = model.fcSegundoNombre.Trim(),
                                fcPrimerApellido = model.fcPrimerApellido.Trim(),
                                fcSegundoApellido = model.fcSegundoApellido.Trim(),
                                //FechaNacimiento = model.FechaNacimiento,
                                fcNombreCorto = model.UserName,
                                fcCentrodeCosto = "0100",
                                fiIDPuesto = 1,
                                fiTipoUsuario = 1,
                                fiIDDepartamento = 1,
                                fiIDJefeInmediato = 1,
                                fcUsuarioDominio = model.UserName,
                                fiIDDominioRed = 1,
                                fcPassword = usuario.PasswordHash,
                                fcPasswordToken = user.PasswordHash,
                                fdFechaUltimoCambiodePassword = DateTime.Now,
                                fcBuzondeCorreo = model.fcBuzondeCorreo.Trim(),
                                fiIDDominioCorreo = 1,
                                fcDireccionFisica = "direccion",
                                fcDocumentoIdentificacion = model.fcDocumentoIdentificacion,
                                fdFechaAlta = DateTime.Now,
                                fiIDUsuarioSolicitante = 1,
                                fiIDUsuarioCreador = 1,
                                fiEstado = 1,
                                fdFechaBaja = null,
                                fcTelefonoMovil = model.fcTelefonoMovil,
                                fcToken = Guid.NewGuid().ToString(),
                                fcIdAspNetUser = usuario.Id,
                                fiIDEmpresa = model.fiIDEmpresa,




                            });

                            nuevoUsuario.RolesPorUsuario.Add(new RolesPorUsuario
                            {
                                Fk_IdRol = model.IdRol
                            });

                            var ListaPermisos = context.PrivilegiosPorRol.Where(x => x.Fk_IdRol == model.IdRol).Select(z => z.AspNetRoles.Name).ToList();
                            foreach (var permiso in ListaPermisos)
                            {
                                await UserManager.AddToRoleAsync(user.Id, permiso);
                            }

                            var resultado = context.SaveChanges() > 0;
                            //using (var contexto = new SARISEntities1())
                            //{
                            //    contexto.sp_TecnicosPorContratista_Crear(model.fiIdUsuario, nuevoUsuario.fiIDUsuario, GetIdUser());
                            //}
                            return EnviarResultado(resultado, "Crear Usuario");
                        }
                        else
                        {
                            return EnviarResultado(result.Succeeded, result.Errors.FirstOrDefault());

                        }
                    }
                }
            }
            catch (Exception e)
            {
                return EnviarResultado(false, e.Message);
            }
        }




        [HttpGet]
        public ActionResult EditarInfoUsuario(int Id)
        {
            using (var context = new OrionSecurityEntities())
            {

                var usuario = context.Usuarios.Find(Id);
                return PartialView(new CrearUsuarioViewModel
                {
                    fiIdUsuario = usuario.fiIDUsuario,
                    fcPrimerNombre = usuario.fcPrimerNombre,
                    fcSegundoNombre = usuario.fcSegundoNombre,
                    fcPrimerApellido = usuario.fcPrimerApellido,
                    fcSegundoApellido = usuario.fcSegundoApellido,
                    fcBuzondeCorreo = usuario.fcBuzondeCorreo,
                    fcTelefonoMovil = usuario.fcTelefonoMovil,
                    fcDocumentoIdentificacion = usuario.fcDocumentoIdentificacion
                });
            }
        }


        [HttpPost]
        public ActionResult EditarInfoUsuario(CrearUsuarioViewModel model)
        {
            using (var context = new OrionSecurityEntities())
            {
                var usuario = context.Usuarios.Find(model.fiIdUsuario);
                usuario.fcPrimerNombre = model.fcPrimerNombre;
                usuario.fcSegundoNombre = model.fcSegundoNombre;
                usuario.fcPrimerApellido = model.fcPrimerApellido;
                usuario.fcSegundoApellido = model.fcSegundoApellido;
                usuario.fcTelefonoMovil = model.fcTelefonoMovil;
                usuario.fcBuzondeCorreo = model.fcBuzondeCorreo;
                usuario.fcDocumentoIdentificacion = model.fcDocumentoIdentificacion;
                context.Entry(usuario).State = EntityState.Modified;
                var result = context.SaveChanges() > 0;

                return EnviarResultado(result, "Editar Usuario", result ? "Se edito Satisfactoriamente" : "Error al editar el usuario");
            }
        }


        [HttpGet]
        public ActionResult EditarInfoUsuarioLaboral(int Id)
        {
            using (var contextSaris = new SARISEntities1())
            {
                using (var context = new OrionSecurityEntities())
                {

                    var usuario = context.Usuarios.Find(Id);

                    ViewBag.ListaAreas = contextSaris.sp_Requerimiento_Areas(1, 1, 1)
                    .ToList()
                    .Select(x => new SelectListItem
                    {
                        Value = x.fiIDArea.ToString(),
                        Text = x.fcDescripcion,
                        Selected = x.fiIDArea == usuario.fiAreaAsignada
                    })
                    .ToList();
                    ViewBag.ListaRoles = context.Roles.Where(x => x.Activo).Select(x => new SelectListItem { Value = x.Pk_IdRol.ToString(), Text = x.Nombre, Selected = x.Pk_IdRol == usuario.fiIDPuesto }).ToList();

                    ViewBag.ListaJefesArea = contextSaris.sp_Usuarios_Maestro_Lista().ToList().Where(x => x.fiEstado == 1 && x.fiIDPuesto != 4).Select(x => new SelectListItem { Value = x.fiIDUsuario.ToString(), Text = $"{x.fcPrimerNombre} {x.fcPrimerApellido}", Selected = x.fiIDUsuario == usuario.fiIDJefeInmediato }).ToList();

                    return PartialView(new CrearUsuarioViewModel
                    {
                        fiIdUsuario = Id,
                        fiIDJefeInmediato = usuario.fiIDJefeInmediato,
                        fiAreaAsignada = usuario.fiAreaAsignada,
                        IdRol = usuario.RolesPorUsuario.FirstOrDefault()?.Fk_IdRol ?? 0,
                    });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarInfoUsuarioLaboral(CrearUsuarioViewModel model)
        {
            using (var context = new SARISEntities1())
            {
                var result = context.sp_Usuario_EditarInfoUsuarioLaboral(model.fiIdUsuario, model.fiIDJefeInmediato, model.fiAreaAsignada, model.IdRol).FirstOrDefault();

                using (var contextO = new OrionSecurityEntities())
                {
                    var usuario = contextO.Usuarios.Find(model.fiIdUsuario);

                    var listaRolesPorEliminar = usuario.RolesPorUsuario.ToList();
                    contextO.RolesPorUsuario.RemoveRange(listaRolesPorEliminar);

                    usuario.RolesPorUsuario.Add(new RolesPorUsuario
                    {
                        Fk_IdRol = model.IdRol,
                    });

                    //aspnetroles
                    var roles = await UserManager.GetRolesAsync(usuario.AspNetUsers.Id);
                    await UserManager.RemoveFromRolesAsync(usuario.AspNetUsers.Id, roles.ToArray());

                    var ListaPermisos = contextO.PrivilegiosPorRol.Where(x => x.Fk_IdRol == model.IdRol).Select(z => z.AspNetRoles.Name).ToList();
                    foreach (var permiso in ListaPermisos)
                    {
                        await UserManager.AddToRoleAsync(usuario.AspNetUsers.Id, permiso);
                    }

                }

                var success = result > 0;

                return EnviarResultado(success, "Editar Información Laboral", success ? "Se Editó Satisfactoriamente" : "Error al editar ");

            }

        }

        [HttpGet]
        public async Task<ActionResult> EditarCuentaUsuario(int Id)
        {
            using (var context = new OrionSecurityEntities())
            {
                ViewBag.ListaRoles = context.Roles.Where(x => x.Activo).Select(x => new SelectListItem { Value = x.Pk_IdRol.ToString(), Text = x.Nombre }).ToList();

                var usuario = context.Usuarios.Find(Id);
                var roles = await UserManager.GetRolesAsync(usuario.fcIdAspNetUser);
                return PartialView(new CrearUsuarioViewModel
                {
                    fiIdUsuario = usuario.fiIDUsuario,
                    UserName = usuario.AspNetUsers.UserName,
                    fcBuzondeCorreo = usuario.fcBuzondeCorreo,
                    fcIdAspNetUser = usuario.fcIdAspNetUser,
                    IdRol = usuario.RolesPorUsuario.FirstOrDefault()?.Fk_IdRol ?? 0,
                    fiEstado = usuario.fiEstado
                });

            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarCuentaUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new OrionSecurityEntities())
                {
                    var usuario = context.Usuarios.Find(model.fiIdUsuario);

                    if (context.AspNetUsers.Any(x => x.UserName == model.fcBuzondeCorreo && x.Id != usuario.fcIdAspNetUser))
                    {
                        return EnviarResultado(false, "Editar Usuario", "El nombre de usuario ya existe.");
                    }

                    usuario.AspNetUsers.UserName = model.UserName;
                    usuario.fcBuzondeCorreo = model.fcBuzondeCorreo;
                    usuario.AspNetUsers.Email = model.fcBuzondeCorreo;
                    usuario.fiEstado = model.fiEstado;
                    context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;

                    //roles
                    var listaRolesPorEliminar = usuario.RolesPorUsuario.ToList();
                    context.RolesPorUsuario.RemoveRange(listaRolesPorEliminar);

                    usuario.RolesPorUsuario.Add(new RolesPorUsuario
                    {
                        Fk_IdRol = model.IdRol,
                    });

                    //aspnetroles
                    var roles = await UserManager.GetRolesAsync(usuario.AspNetUsers.Id);
                    await UserManager.RemoveFromRolesAsync(usuario.AspNetUsers.Id, roles.ToArray());

                    var ListaPermisos = context.PrivilegiosPorRol.Where(x => x.Fk_IdRol == model.IdRol).Select(z => z.AspNetRoles.Name).ToList();
                    foreach (var permiso in ListaPermisos)
                    {
                        await UserManager.AddToRoleAsync(usuario.AspNetUsers.Id, permiso);
                    }

                    var result = context.SaveChanges() > 0;
                    return EnviarResultado(result, "Editar Cuenta Usuario", result /*&& result2.Succeeded*/ ? "Se edito Satisfactoriamente" : "Error al editar el usuario");

                }
            }
            catch (Exception e)
            {
                return EnviarException(e, "Editar Usuario");

            }

        }


        [HttpGet]
        public ActionResult CambiarContrasenaUsuario(int Id)
        {
            using (var context = new OrionSecurityEntities())
            {
                return PartialView(new CambiarContraseñaUsuarioViewModel { Id = Id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CambiarContrasenaUsuario(CambiarContraseñaUsuarioViewModel model)
        {
            try
            {
                using (var context = new OrionSecurityEntities())
                {
                    var User = context.Usuarios.Find(model.Id);
                    string code = await UserManager.GeneratePasswordResetTokenAsync(User.fcIdAspNetUser);
                    var result = await UserManager.ResetPasswordAsync(User.fcIdAspNetUser, code, model.NewPassword);
                    return EnviarResultado(result.Succeeded, "Cambiar Contrasena", result.Succeeded ? "Se cambio Satisfactoriamente" : "Error al cambiar la contrasena");
                }
            }
            catch (Exception e)
            {
                return EnviarException(e, "Cambiar Contraseña");
            }

        }



        [HttpPost]
        public ActionResult DeshabilitarUsuario(int Id)
        {
            using (var context = new OrionSecurityEntities())
            {

                var usuario = context.Usuarios.Find(Id);
                usuario.fiEstado = usuario.fiEstado == 1 ? 0 : 1;
                var result = context.SaveChanges() > 0;
                return EnviarResultado(result, usuario.fiEstado == 0 ? "Habilitar Usuario" : "Deshabilitar Usuario", result ? "Modificado exitosamente" : "Error al modificar el usuario");

            }
        }



    }
}