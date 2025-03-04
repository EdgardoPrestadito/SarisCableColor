if (ticket.fiIDEstadoRequerimiento == 5 && TicketHijosCorreo.Count > 0)
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
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
                                    MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                }

                                if (ticket.fiIDEstadoRequerimiento == 6)
                                {
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
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
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
                                    MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorSolicitanteHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                }

                                if (ticket.fiIDEstadoRequerimiento == 6)
                                {
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
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
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
                                    MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroJefeAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                }
                                if (ticket.fiIDEstadoRequerimiento == 6)
                                {
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
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
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
                                    MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, CorreoNumeroSupervisorAsignadaHijo[z].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                }
                                if (ticket.fiIDEstadoRequerimiento == 6)
                                {
                                    retraso = random.Next(10000, 20001);
                                    await Task.Delay(retraso);
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
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoGrenciaAsignada, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcNumeroGerencia, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoSolicitante, TicketHijosCorreo[x].fcTituloRequerimiento, "Cerrado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);

                            }

                            if (ticket.fiIDEstadoRequerimiento == 6)
                            {
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoGrenciaAsignada, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcNumeroGerencia, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
                                MensajeriaApi.EnviarNumeroTicket(TicketHijosCorreo[x].fcNombreCorto, datosticket.fiIDRequerimiento, TicketHijosCorreo[x].fcTelefonoMovil, TicketHijosCorreo[x].fcTituloRequerimiento, "Cancelado", TicketHijosCorreo[x].fcDescripcionCategoria, TicketHijosCorreo[x].fcTipoRequerimiento, ticket.fcDescripcionEstado, TicketHijosCorreo[x].fcDescripcionPrioridad, ticket.fccomentario, TicketHijosCorreo[x].fcDescripcionCategoriaResolucion, TicketHijosCorreo[x].fcTipoRequerimientoResolucion, TicketHijosCorreo[x].fcAreaSolicitante);
                                retraso = random.Next(10000, 20001);
                                await Task.Delay(retraso);
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