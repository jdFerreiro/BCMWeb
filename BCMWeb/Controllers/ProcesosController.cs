using BCMWeb.Data.EF;
using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        [SessionExpire]
        [HandleError]
        public ActionResult Index(long IdModulo)
        {
            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            return RedirectToAction(firstModulo.Action, firstModulo.Controller, new { modId = firstModulo.IdModulo });
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarIF(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarIF(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarIFPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIfUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIAImpactoFinanciero reg = new tblBIAImpactoFinanciero()
                        {
                            Descripcion = string.Empty,
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                            Impacto = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            UnidadTiempo = string.Empty
                        };

                        db.tblBIAImpactoFinanciero.Add(reg);
                    }
                    else
                    {
                        tblBIAImpactoFinanciero reg = db.tblBIAImpactoFinanciero.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                                                           && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                                                           && x.IdProceso == dataImpacto.IdProceso
                                                                                           && x.IdImpactoFinanciero == dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarIFPartialView");
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarIo(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarIo(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarIoPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIoUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIAImpactoOperacional reg = new tblBIAImpactoOperacional()
                        {
                            Descripcion = string.Empty,
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                            ImpactoOperacional = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            UnidadTiempo = string.Empty
                        };

                        db.tblBIAImpactoOperacional.Add(reg);
                    }
                    else
                    {
                        tblBIAImpactoOperacional reg = db.tblBIAImpactoOperacional.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                                                           && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                                                           && x.IdProceso == dataImpacto.IdProceso
                                                                                           && x.IdImpactoOperacional== dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarIoPartialView");
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarMTD(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarMTD(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarMTDPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMTDUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIAMTD reg = new tblBIAMTD()
                        {
                            Observacion = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                        };

                        db.tblBIAMTD.Add(reg);
                    }
                    else
                    {
                        tblBIAMTD reg = db.tblBIAMTD.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                            && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                            && x.IdProceso == dataImpacto.IdProceso
                                                            && x.IdMTD == dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarMTDPartialView");
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarRTO(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarRTO(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarRTOPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditRTOUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIARTO reg = new tblBIARTO()
                        {
                            Observacion = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                        };

                        db.tblBIARTO.Add(reg);
                    }
                    else
                    {
                        tblBIARTO reg = db.tblBIARTO.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                            && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                            && x.IdProceso == dataImpacto.IdProceso
                                                            && x.IdRTO == dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarRTOPartialView");
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarRPO(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarRPO(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarRPOPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditRPOUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIARPO reg = new tblBIARPO()
                        {
                            Observacion = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                        };

                        db.tblBIARPO.Add(reg);
                    }
                    else
                    {
                        tblBIARPO reg = db.tblBIARPO.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                            && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                            && x.IdProceso == dataImpacto.IdProceso
                                                            && x.IdRPO == dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarRPOPartialView");
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarWRT(long modId)
        {

            Session["modId"] = modId;

            ProcesoImpactoModel model = new ProcesoImpactoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult AjustarWRT(ProcesoImpactoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AjustarWRTPartialView(ProcesoImpactoModel model)
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditWRTUpdatePartial(ProcesoImpactoModel dataImpacto)
        {
            if (ModelState.IsValid)
            {
                using (Entities db = new Entities())
                {
                    if (dataImpacto.IdImpacto == 0)
                    {
                        tblBIAWRT reg = new tblBIAWRT()
                        {
                            Observacion = (dataImpacto.Impacto == null ? string.Empty : dataImpacto.Impacto),
                            IdDocumentoBIA = dataImpacto.IdDocumentoBIA,
                            IdEmpresa = dataImpacto.IdEmpresa,
                            IdEscala = dataImpacto.IdTipoEscala,
                            IdProceso = dataImpacto.IdProceso,
                            IdTipoFrecuencia = 9,
                        };

                        db.tblBIAWRT.Add(reg);
                    }
                    else
                    {
                        tblBIAWRT reg = db.tblBIAWRT.Where(x => x.IdEmpresa == dataImpacto.IdEmpresa
                                                            && x.IdDocumentoBIA == dataImpacto.IdDocumentoBIA
                                                            && x.IdProceso == dataImpacto.IdProceso
                                                            && x.IdWRT == dataImpacto.IdImpacto).FirstOrDefault();

                        reg.IdEscala = dataImpacto.IdTipoEscala;
                    }

                    db.SaveChanges();
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("AjustarWRTPartialView");
        }

        [SessionExpire]
        [HandleError]
        public ActionResult Criticos(long modId)
        {

            Session["modId"] = modId;

            CriticoModel model = new CriticoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            Session["ValoresIF"] = "";
            Session["ValoresIO"] = "";
            Session["ValoresMTD"] = "";
            Session["ValoresRTO"] = "";
            Session["ValoresRPO"] = "";
            Session["ValoresWRT"] = "";

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult Criticos(CriticoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            Session["ValoresIF"] = model.ImpactoFinancieroSelected;
            Session["ValoresIO"] = model.ImpactoOperacionalSelected;
            Session["ValoresMTD"] = model.MTDSelected;
            Session["ValoresRTO"] = model.RTOSelected;
            Session["ValoresRPO"] = model.RPOSelected;
            Session["ValoresWRT"] = model.WRTSelected;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ImpactoFinancieroPartialView()
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ImpactoOperacionalPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult MTDPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult RPOPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult RTOPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult WRTPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult CriticosPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Riesgo(long modId)
        {

            Session["modId"] = modId;

            CriticoModel model = new CriticoModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            Session["ValoresProbabilidad"] = "";
            Session["ValoresImpacto"] = "";
            Session["ValoresSeveridad"] = "";
            Session["ValoresFuente"] = "";
            Session["ValoresControl"] = "";

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult Riesgo(CriticoModel model)
        {
            long modId = long.Parse(Session["modId"].ToString());
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            Session["ValoresProbabilidad"] = model.ImpactoFinancieroSelected;
            Session["ValoresImpacto"] = model.ImpactoOperacionalSelected;
            Session["ValoresSeveridad"] = model.MTDSelected;
            Session["ValoresFuente"] = model.RTOSelected;
            Session["ValoresControl"] = model.RPOSelected;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ProbabilidadPartialView()
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ImpactoPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult SeveridadPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult FuentePartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ControlPartialView(CriticoModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult RiesgoPartialView(CriticoModel model)
        {
            return PartialView();
        }
    }
}