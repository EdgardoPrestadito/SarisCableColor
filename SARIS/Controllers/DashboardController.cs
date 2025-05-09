﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using OrionCoreCableColor.DbConnection;
using OrionCoreCableColor.Models.DashBoard;
using System.Data.SqlClient;

namespace OrionCoreCableColor.Controllers
{
    //[Authorize(Roles = "Acceso_Al_Sistema")]
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Indicadores(int idpais)
        {
            var Indicadores = new DashboardViewModel();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_DashboardGlobal {idpais}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        Indicadores.BulletViewModel = db.ObjectContext.Translate<IndicadoresViewModel>(reader).ToList();
                        reader.NextResult();
                        Indicadores.AreasIncidentes = db.ObjectContext.Translate<AreasIncidentesViewModel>(reader).ToList();
                        reader.NextResult();
                        Indicadores.AreasTiempo = db.ObjectContext.Translate<AreaTiempoViewModel>(reader).ToList();
                        reader.NextResult();
                        Indicadores.ConteoIncidentes = db.ObjectContext.Translate<ConteoIncidenteViewModel>(reader).ToList();
                        reader.NextResult();
                        Indicadores.ConteoIncidentesEstados = db.ObjectContext.Translate<IncidentesEstadosViewModel>(reader).ToList();
                        reader.NextResult();
                        Indicadores.ConteoPorEstado = db.ObjectContext.Translate<IncidentesPorEstados>(reader).ToList();
                        reader.NextResult();
                        Indicadores.ConteoPorPrioridad = db.ObjectContext.Translate<IncidentesPrioridad>(reader).ToList();
                        reader.NextResult();
                        Indicadores.ConteoPorRegiones = db.ObjectContext.Translate<IncidentesRegiones>(reader).ToList();
                        reader.NextResult();
                        Indicadores.TalbaPendientes = db.ObjectContext.Translate<DatosIncidentesPendientesViewModel>(reader).ToList();
                    }

                    connection.Close();

                    return EnviarListaJson(Indicadores);
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        [HttpGet]
        public JsonResult ListarDatosPendientes(int tabId)
        {
            var Indicadores = new List<DatosIncidentesPendientesViewModel>();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_DashboardGlobal {tabId}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        Indicadores = db.ObjectContext.Translate<DatosIncidentesPendientesViewModel>(reader).ToList();
                    }

                    connection.Close();

                    return EnviarListaJson(Indicadores);
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        [HttpGet]
        public JsonResult ListarDatosTotales(int fiIdpais)
        {
            var Indicadores = new DatosIncidentesPendientesViewModel();

            try
            {
                using (var connection = (new SARISEntities1()).Database.Connection)
                {

                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"EXEC sp_DashboardGlobal {fiIdpais}";
                    using (var reader = command.ExecuteReader())
                    {
                        var db = ((IObjectContextAdapter)new SARISEntities1());
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        reader.NextResult();
                        Indicadores = db.ObjectContext.Translate<DatosIncidentesPendientesViewModel>(reader).FirstOrDefault();
                    }

                    connection.Close();

                    return EnviarListaJson(Indicadores);
                }
            }
            catch (Exception e)
            {

                throw;
            }


        }

        [HttpPost]
        public JsonResult Tabs(int mes)
        {
            try
            {
                using (var context = new SARISEntities1())
                {
                    var pcNombreCorto = User.Identity.Name.ToString();

                    var result = context.sp_DashboardTabsHora(pcNombreCorto, mes).ToList();

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}