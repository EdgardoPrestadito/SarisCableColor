using OrionCoreCableColor.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    public class LogController : BaseController
    {
        // GET: Log
        public ActionResult LogNumerosCorreos()
        {
            return View();
        }



        [HttpGet]
        public JsonResult LogCorreoNumerosListado()
        {

            using (var conetion = new SARISEntities1())
            {

                var lista = EnviarListaJson(conetion.sp_LogCorreosNumeros_Listado().ToList());
                //EnviarNotificacion("DESDE BASE SOLICITUDES");
                return lista;
            }

        }
    }
}