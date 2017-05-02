using BCMWeb.Models;
using DevExpress.Web.Mvc;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class ReportesController : Controller
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
        public static void CreateTreeViewNodesRecursive(DataTable data, MVCxTreeViewNodeCollection nodes, long parentId)
        {
            if (data == null || data.Rows.Count == 0)
            {
                data = Metodos.GetDataTableUnidadesOrganizativas();
            }
            var rows = data.Select("[IdUnidadPadre] = " + parentId);

            foreach (DataRow row in rows)
            {
                MVCxTreeViewNode node = nodes.Add(row["Nombre"].ToString(), row["IdUnidad"].ToString());
                CreateTreeViewNodesRecursive(data, node.Nodes, Convert.ToInt64(row["IdUnidad"]));
            }
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Cuadro(long modId)
        {

            Session["modId"] = modId;

            ReporteModel model = new ReporteModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            ViewData["IdUnidadOrganizativa"] = 0;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult Cuadro(ReporteModel model)
        {

            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = long.Parse(_modId);
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            switch (model.IdModuloActual)
            {
                case 13010100:
                    model.DataCuadro = Metodos.GetImpactoOperacional(model.IdUnidadOrganizativa);
                    break;
                case 13010400:
                    model.DataCuadro = Metodos.GetValoresImpacto(model.IdUnidadOrganizativa);
                    break;
                case 13010700:
                    model.DataCuadro = Metodos.GetProcesoMes(model.IdUnidadOrganizativa);
                    break;
            }

            ViewData["IdUnidadOrganizativa"] = model.IdUnidadOrganizativa;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult PartialUnidadesOrganizativas(ReporteModel model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult CuadroPartialView(long IdUnidadOrganizativa)
        {
            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            ReporteModel model = new ReporteModel();
            model.returnPage = Url.Action("Index", "Documento", new { IdModulo });
            model.IdModulo = IdModulo;
            model.IdModuloActual = long.Parse(_modId);
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            model.IdUnidadOrganizativa = IdUnidadOrganizativa;
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            switch (model.IdModuloActual)
            {
                case 13010100:
                    model.DataCuadro = Metodos.GetImpactoOperacional(model.IdUnidadOrganizativa);
                    break;
                case 13010400:
                    model.DataCuadro = Metodos.GetValoresImpacto(model.IdUnidadOrganizativa);
                    break;
                case 13010700:
                    // model.DataCuadro = Metodos.GetValoresImpacto(model.IdUnidadOrganizativa);
                    break;
            }

            ViewData["IdUnidadOrganizativa"] = model.IdUnidadOrganizativa;

            return PartialView(model);
        }
        [SessionExpire]
        [HandleError]
        //public ActionResult CuadroIOPartialView(long IdUnidadOrganizativa, long IdProceso)
        //{
        //    ViewData["IdUnidadOrganizativa"] = IdUnidadOrganizativa;

        //    return PartialView("CuadroIOPartialView", Metodos.GetImpactoOperacional(IdUnidadOrganizativa, IdProceso));
        //}
        public ActionResult CuadroIOPartialView(long IdUnidadOrganizativa)
        {
            ViewData["IdUnidadOrganizativa"] = IdUnidadOrganizativa;

            return PartialView("CuadroIOPartialView", Metodos.GetImpactoOperacional(IdUnidadOrganizativa));
        }
        [SessionExpire]
        [HandleError]
        //public ActionResult CuadroVIPartialView(long IdUnidadOrganizativa, long IdProceso)
        //{
        //    ViewData["IdUnidadOrganizativa"] = IdUnidadOrganizativa;

        //    return PartialView("CuadroVIPartialView", Metodos.GetValoresImpacto(IdUnidadOrganizativa, IdProceso));
        //}
        public ActionResult CuadroVIPartialView(long IdUnidadOrganizativa)
        {
            ViewData["IdUnidadOrganizativa"] = IdUnidadOrganizativa;

            return PartialView("CuadroVIPartialView", Metodos.GetValoresImpacto(IdUnidadOrganizativa));
        }
        [SessionExpire]
        [HandleError]
        public ActionResult CuadroPM(long modId)
        {

            Session["modId"] = modId;

            ReporteModel model = new ReporteModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.returnPage = Url.Action("Index", "Documento", new { IdModulo });
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
        public ActionResult CuadroPM(ReporteModel model)
        {

            string _modId = Session["modId"].ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));
            long IdModulo = IdTipoDocumento * 1000000;

            model.returnPage = Url.Action("Index", "Documento", new { IdModulo });
            model.IdModulo = IdModulo;
            model.IdModuloActual = long.Parse(_modId);
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            model.DataCuadro = Metodos.GetProcesoMes(model.IdUnidadOrganizativa);
            ViewData["IdUnidadOrganizativa"] = model.IdUnidadOrganizativa;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Tabla(long modId)
        {

            Session["modId"] = modId;

            ReporteModel model = new ReporteModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.returnPage = Url.Action("Index", "Documento", new { IdModulo });
            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);

            switch (modId)
            {
                case 13010200:
                    model.DataCuadro = Metodos.GetNroProcesosByImpactoOperacional();
                    break;
                case 13010500:
                    model.DataCuadro = Metodos.GetNroProcesosByValorImpacto();
                    break;
                case 13010800:
                    model.DataCuadro = Metodos.GetNroProcesosByGranImpacto();
                    break;
                case 13030100:
                case 13030200:
                case 13030300:
                case 13030400:
                case 13030500:
                    model.DataCuadro = Metodos.GetDataAmenazasProbabilidad();
                    break;
            }

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult TablaPUOPartialView()
        {
            return PartialView("TablaPUOPartialView", Metodos.GetNroProcesosByImpactoOperacional());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult TablaVIPartialView()
        {
            return PartialView("TablaVIPartialView", Metodos.GetNroProcesosByValorImpacto());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult TablaGIPartialView()
        {
            return PartialView("TablaGIPartialView", Metodos.GetNroProcesosByGranImpacto());
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Grafico(long modId)
        {

            Session["modId"] = modId;

            ReporteModel model = new ReporteModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.returnPage = Url.Action("Index", "Documento", new { IdModulo });
            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);
            int iEscala = 0;

            switch (model.IdModuloActual)
            {
                case 13010300:
                    model.EscalaGrafico = Metodos.GetTipoEscalaGrafico(eTipoEscala.ImpactoOperacional);
                    model.IdEscalaGrafico = model.EscalaGrafico.First().IdTipoEscala;
                    model.DataCuadro = Metodos.GetDataGraficoImpactoOperacional(model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    break;
                case 13010600:
                    model.EscalaGrafico = Metodos.GetEscalaGrafico();
                    model.IdEscalaGrafico = model.EscalaGrafico.First().IdTipoEscala;
                    model.DataCuadro = Metodos.GetDataGraficoValorImpacto((eTipoEscala)model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    //model.DataMTD = Metodos.GetDataGrafico();
                    //model.DataRPO = Metodos.GetDataGraficoRPO();
                    //model.DataRTO = Metodos.GetDataGraficoRTO();
                    //model.DataWRT = Metodos.GetDataGraficoWRT();
                    break;
                case 13010900:
                    model.EscalaGrafico = Metodos.GetSemestres();
                    model.IdEscalaGrafico = model.EscalaGrafico.First().IdTipoEscala;
                    model.DataCuadro = Metodos.GetDataGraficoGranImpacto(model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    break;
            }

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult Grafico(ReporteModel model)
        {
            model.returnPage = Url.Action("Index", "Documento", new { model.IdModulo });
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);
            int iEscala = 0;

            switch (model.IdModuloActual)
            {
                case 13010300:
                    model.EscalaGrafico = Metodos.GetTipoEscalaGrafico(eTipoEscala.ImpactoOperacional);
                    model.DataCuadro = Metodos.GetDataGraficoImpactoOperacional(model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    break;
                case 13010600:
                    model.EscalaGrafico = Metodos.GetEscalaGrafico();
                    model.DataCuadro = Metodos.GetDataGraficoValorImpacto((eTipoEscala)model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    break;
                case 13010900:
                    model.EscalaGrafico = Metodos.GetSemestres();
                    model.DataCuadro = Metodos.GetDataGraficoGranImpacto(model.IdEscalaGrafico);
                    iEscala = model.EscalaGrafico.FindIndex(x => x.IdTipoEscala == model.IdEscalaGrafico);
                    ViewData["TituloGrafico"] = model.PageTitle + " - " + model.EscalaGrafico[iEscala].TipoEscala;
                    break;
            }

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoPUOPartialView(long IdEscalaGrafico, long IdModulo)
        {
            string PageTitle = Metodos.GetModuloName(IdModulo);
            List<TipoEscalaGrafico> EscalaGrafico = Metodos.GetTipoEscalaGrafico(eTipoEscala.ImpactoOperacional);
            int iEscala = EscalaGrafico.FindIndex(x => x.IdTipoEscala == IdEscalaGrafico);
            ViewData["TituloGrafico"] = PageTitle + " - " + EscalaGrafico[iEscala].TipoEscala;
            return PartialView("GraficoPUOPartialView", Metodos.GetDataGraficoImpactoOperacional(IdEscalaGrafico));
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoMTDPartialView(long IdEscalaGrafico, long IdModulo)
        {
            string PageTitle = Metodos.GetModuloName(IdModulo);
            List<TipoEscalaGrafico> EscalaGrafico = Metodos.GetEscalaGrafico();
            int iEscala = EscalaGrafico.FindIndex(x => x.IdTipoEscala == IdEscalaGrafico);
            ViewData["TituloGrafico"] = PageTitle + " - " + EscalaGrafico[iEscala].TipoEscala;
            return PartialView("GraficoVIPartialView", Metodos.GetDataGraficoValorImpacto((eTipoEscala)IdEscalaGrafico));
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoRPOPartialView()
        {
            return PartialView("GraficoVIPartialView", Metodos.GetDataGraficoRPO());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoRTOPartialView()
        {
            return PartialView("GraficoVIPartialView", Metodos.GetDataGraficoRTO());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoWRTPartialView()
        {
            return PartialView("GraficoVIPartialView", Metodos.GetDataGraficoWRT());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult GraficoGIPartialView(long IdEscalaGrafico, long IdModulo)
        {
            string PageTitle = Metodos.GetModuloName(IdModulo);
            List<TipoEscalaGrafico> EscalaGrafico = Metodos.GetSemestres();
            int iEscala = EscalaGrafico.FindIndex(x => x.IdTipoEscala == IdEscalaGrafico);
            ViewData["TituloGrafico"] = PageTitle + " - " + EscalaGrafico[iEscala].TipoEscala;
            return PartialView("GraficoGIPartialView", Metodos.GetDataGraficoGranImpacto(IdEscalaGrafico));
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ExportChart(long IdModuloActual)
        {
            switch (IdModuloActual)
            {
                case 13010300:
                    ChartControlSettings settings = ChartHelpers.GetChartSettings();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        settings.SaveToStream(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        ChartControl chartControl = new ChartControl();
                        chartControl.LoadFromStream(stream);
                        chartControl.Width = Convert.ToInt16(settings.Width.Value);
                        chartControl.Height = Convert.ToInt16(settings.Height.Value);
                        chartControl.DataSource = new DataView(ChartHelpers.GenerateDataIO(Metodos.GetDataGraficoImpactoOperacional()));
                        var pcl = new PrintableComponentLink(new PrintingSystem());
                        pcl.Component = ((IChartContainer)chartControl).Chart;
                        pcl.Landscape = true;
                        pcl.CreateDocument();

                        using (var exstream = new MemoryStream() )
                        {
                            pcl.PrintingSystem.ExportToPdf(exstream);
                            byte[] buf = new byte[(int)exstream.Length];
                            exstream.Seek(0, SeekOrigin.Begin);
                            exstream.Read(buf, 0, buf.Length);

                            return File(buf, "application/pdf", "chart" + Guid.NewGuid().ToString() + ".pdf");
                        }
                    }
                case 13010600:
                    //< div id = "multichart" >

                    //     < div class="MTD">
                    //        <div class="Titulo">
                    //            @Html.Raw(Resources.ReporteResource.captionMDTHeader)
                    //        </div>
                    //        @Html.Partial("GraficoMTDPartialView", Model.DataMTD)
                    //    </div>
                    //    <div class="RPO">
                    //        <div class="Titulo">
                    //            @Html.Raw(Resources.ReporteResource.captionRPOHeader)
                    //        </div>
                    //        @Html.Partial("GraficoRPOPartialView", Model.DataRPO)
                    //    </div>
                    //    <div class="RTO">
                    //        <div class="Titulo">
                    //            @Html.Raw(Resources.ReporteResource.captionRTOHeader)
                    //        </div>
                    //        @Html.Partial("GraficoRTOPartialView", Model.DataRTO)
                    //    </div>
                    //    <div class="WRT">
                    //        <div class="Titulo">
                    //            @Html.Raw(Resources.ReporteResource.captionWRTHeader)
                    //        </div>
                    //        @Html.Partial("GraficoWRTPartialView", Model.DataWRT)
                    //    </div>
                    //</div>
                    break;
                case 13010900:
                    //chartControl.DataSource = new DataView(ChartHelpers.GenerateDataIO(Metodos.GetNroProcesosByImpactoOperacional()));
                    break;
            }
            return null;
        }
        [SessionExpire]
        [HandleError]
        public ActionResult DiagImpacto(long modId)
        {
            DocumentoDiagrama model = new DocumentoDiagrama();

            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Length == 7  ? _modId.Substring(0,1) : _modId.Substring(0,2));
            long IdModulo = IdTipoDocumento * 1000000;
            Session["modId"] = modId;

            model.IdModuloActual = modId;
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, Metodos.GetModuloName(modId));

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult DiagImpacto(DocumentoDiagrama model)
        {
            string _modId = model.IdModuloActual.ToString();
            int IdTipoDocumento = int.Parse(_modId.Length == 7 ? _modId.Substring(0, 1) : _modId.Substring(0, 2));
            long IdModulo = IdTipoDocumento * 1000000;
            long modId = long.Parse(Session["modId"].ToString());

            model.IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            model.IdTipoDocumento = IdTipoDocumento;
            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            Session["IdDocumento"] = Metodos.GetIdDocumentoByProceso(model.IdProceso);


            DocumentoProcesoModel Proceso = Metodos.GetProceso(model.IdProceso);
            model.Interdependencias = Metodos.GetInterdependenciasDiagrama(model.IdProceso);
            model.Clientes_Productos = Metodos.GetClientesProductosDiagrama(model.IdProceso);
            model.Entradas = Metodos.GetEntradasDiagrama(model.IdProceso);
            model.PersonalClave = Metodos.GetPersonasClaveDiagrama(model.IdProceso);
            model.Proveedores = Metodos.GetProveedoresDiagrama(model.IdProceso);
            model.Tecnologia = Metodos.GetTecnologiaDiagrama(model.IdProceso);
            model.NroProceso = Proceso.NroProceso.ToString();
            model.Nombre = Proceso.Nombre;
            model.Descripcion = Proceso.Descripcion;

            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult PartialProcesosView()
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult PartialProcesosView(DocumentoDiagrama model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult PartialDiagramaProceso()
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult PartialDiagramaProceso(DocumentoDiagrama model)
        {
            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult ComboBoxGrafico()
        {
            return PartialView();
        }

    }
}