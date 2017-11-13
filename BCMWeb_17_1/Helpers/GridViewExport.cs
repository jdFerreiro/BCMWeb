using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Web.Hosting;
using DevExpress.Data;
using DevExpress.Utils;
using System.Web.UI;
using System.Collections;
using DevExpress.XtraPrinting;

namespace BCMWeb
{
    public enum GridViewExportFormat { None, Pdf, Xls, Xlsx, Rtf, Csv }
    public delegate ActionResult GridViewExportMethod(GridViewSettings settings, object dataObject);

    public class GridViewExportIniciativas
    {
        static string ExcelDataAwareGridViewSettingsKey = "51172A18-2073-426B-A5CB-136347B3A79F";
        static string FormatConditionsExportGridViewSettingsKey = "14634B6F-E1DC-484A-9728-F9608615B628";
        static Dictionary<GridViewExportFormat, GridViewExportMethod> exportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> ExportFormatsInfo
        {
            get
            {
                if (exportFormatsInfo == null)
                    exportFormatsInfo = CreateExportFormatsInfo();
                return exportFormatsInfo;
            }
        }

        static IDictionary Context { get { return System.Web.HttpContext.Current.Items; } }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                {
                    GridViewExportFormat.Xls,
                    (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                {
                    GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf },
                {
                    GridViewExportFormat.Csv,
                    (settings, data) => GridViewExtension.ExportToCsv(settings, data, new CsvExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> dataAwareExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> DataAwareExportFormatsInfo
        {
            get
            {
                if (dataAwareExportFormatsInfo == null)
                    dataAwareExportFormatsInfo = CreateDataAwareExportFormatsInfo();
                return dataAwareExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateDataAwareExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Xls, GridViewExtension.ExportToXls },
                { GridViewExportFormat.Xlsx, GridViewExtension.ExportToXlsx },
                { GridViewExportFormat.Csv, GridViewExtension.ExportToCsv }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> formatConditionsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> FormatConditionsExportFormatsInfo
        {
            get
            {
                if (formatConditionsExportFormatsInfo == null)
                    formatConditionsExportFormatsInfo = CreateFormatConditionsExportFormatsInfo();
                return formatConditionsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateFormatConditionsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf},
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data) },
                { GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG})
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> advancedBandsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> AdvancedBandsExportFormatsInfo
        {
            get
            {
                if (advancedBandsExportFormatsInfo == null)
                    advancedBandsExportFormatsInfo = CreateAdvancedBandsExportFormatsInfo();
                return advancedBandsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateAdvancedBandsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Xlsx, (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }

        public static string GetExportButtonTitle(GridViewExportFormat format)
        {
            if (format == GridViewExportFormat.None)
                return string.Empty;
            return string.Format("Export to {0}", format.ToString().ToUpper());
        }

        public static GridViewSettings CreateGeneralMasterGridSettings(object DataToBind)
        {
            return CreateGeneralMasterGridSettings(GridViewDetailExportMode.None, DataToBind);
        }
        public static GridViewSettings CreateGeneralMasterGridSettings(GridViewDetailExportMode exportMode, object DataToBind)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "masterGrid";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "CategoryID";
            settings.Columns.Add("CategoryID");
            settings.Columns.Add("CategoryName");
            settings.Columns.Add("Description");
            settings.Columns.Add(c => {
                c.FieldName = "Picture";
                c.ColumnType = MVCxGridViewColumnType.BinaryImage;
                BinaryImageEditProperties properties = (BinaryImageEditProperties)c.PropertiesEdit;
                properties.ImageWidth = 120;
                properties.ImageHeight = 80;
                properties.ExportImageSettings.Width = 90;
                properties.ExportImageSettings.Height = 60;
            });

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.ExportMode = exportMode;

            settings.SettingsExport.GetExportDetailGridViews = (s, e) => {
                int categoryID = (int)DataBinder.Eval(e.DataItem, "CategoryID");
                GridViewExtension grid = new GridViewExtension(CreateGeneralDetailGridSettings(categoryID));
                grid.Bind(DataToBind);
                e.DetailGridViews.Add(grid);
            };

            return settings;
        }

        public static GridViewSettings CreateGeneralDetailGridSettings(int uniqueKey)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "detailGrid" + uniqueKey;
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "ProductID";
            settings.Columns.Add("ProductID");
            settings.Columns.Add("ProductName");
            settings.Columns.Add("UnitPrice");
            settings.Columns.Add("QuantityPerUnit");

            settings.SettingsDetail.MasterGridName = "masterGrid";

            return settings;
        }

        static GridViewSettings exportGridViewSettings;
        public static GridViewSettings ExportGridViewSettings
        {
            get
            {
                if (exportGridViewSettings == null)
                    exportGridViewSettings = CreateExportGridViewSettings();
                return exportGridViewSettings;
            }
        }
        static GridViewSettings CreateExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvIniciativas";
            settings.KeyFieldName = "IdIniciativa";
            settings.CallbackRouteValues = new { Controller = "PlanTrabajo", Action = "IniciativaPartialView" };
            settings.Width = Unit.Percentage(100);

            settings.TotalSummary.Add(SummaryItemType.Count, "NroIniciativa").DisplayFormat = "#,##0";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoEstimado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoReal").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoAbonado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoPendiente").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Average, "PorcentajeAvance").DisplayFormat = "{0:n2}%";

            settings.Columns.Add(c =>
            {
                c.FieldName = "NroIniciativa";
                c.Caption = Resources.IniciativaResource.NroIniciativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.Width = 80;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Nombre";
                c.Caption = Resources.IniciativaResource.Nombre;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Descripcion";
                c.Caption = Resources.IniciativaResource.Descripcion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "UnidadOrganizativa";
                c.Caption = Resources.IniciativaResource.UnidadOrganizativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Responsable";
                c.Caption = Resources.IniciativaResource.Responsable;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioEstimada";
                c.Caption = Resources.IniciativaResource.FechaInicioEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreEstimada";
                c.Caption = Resources.IniciativaResource.FechaCierreEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioReal";
                c.Caption = Resources.IniciativaResource.FechaInicioReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreReal";
                c.Caption = Resources.IniciativaResource.FechaCierreReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoEstimado";
                c.Caption = Resources.IniciativaResource.PresupuestoEstimado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoReal";
                c.Caption = Resources.IniciativaResource.PresupuestoReal;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoAbonado";
                c.Caption = Resources.IniciativaResource.MontoAbonado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoPendiente";
                c.Caption = Resources.IniciativaResource.MontoPendiente;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasEstimadas";
                c.Caption = Resources.IniciativaResource.HorasEstimadas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasInvertidas";
                c.Caption = Resources.IniciativaResource.HorasInvertidas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PorcentajeAvance";
                c.Caption = Resources.IniciativaResource.PorcentajeAvance;
                c.PropertiesEdit.DisplayFormatString = "{0:n2}%";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdEstatus";
                c.Caption = Resources.IniciativaResource.Estatus;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetEstadosIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Urgente";
                c.Caption = Resources.IniciativaResource.Urgente;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPrioridadesIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Observacion";
                c.Caption = Resources.IniciativaResource.Observacion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            return settings;
        }

        public static GridViewSettings ExcelDataAwareExportGridViewSettings
        {
            get
            {
                GridViewSettings settings = Context[ExcelDataAwareGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[ExcelDataAwareGridViewSettingsKey] = settings = CreateExcelDataAwareExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateExcelDataAwareExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvIniciativas";
            settings.KeyFieldName = "IdIniciativa";
            settings.Width = Unit.Percentage(100);

            settings.TotalSummary.Add(SummaryItemType.Count, "NroIniciativa").DisplayFormat = "#,##0";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoEstimado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoReal").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoAbonado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoPendiente").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Average, "PorcentajeAvance").DisplayFormat = "{0:n2}%";

            settings.Columns.Add(c =>
            {
                c.FieldName = "NroIniciativa";
                c.Caption = Resources.IniciativaResource.NroIniciativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.Width = 80;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Nombre";
                c.Caption = Resources.IniciativaResource.Nombre;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Descripcion";
                c.Caption = Resources.IniciativaResource.Descripcion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "UnidadOrganizativa";
                c.Caption = Resources.IniciativaResource.UnidadOrganizativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Responsable";
                c.Caption = Resources.IniciativaResource.Responsable;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioEstimada";
                c.Caption = Resources.IniciativaResource.FechaInicioEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreEstimada";
                c.Caption = Resources.IniciativaResource.FechaCierreEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioReal";
                c.Caption = Resources.IniciativaResource.FechaInicioReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreReal";
                c.Caption = Resources.IniciativaResource.FechaCierreReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoEstimado";
                c.Caption = Resources.IniciativaResource.PresupuestoEstimado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoReal";
                c.Caption = Resources.IniciativaResource.PresupuestoReal;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoAbonado";
                c.Caption = Resources.IniciativaResource.MontoAbonado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoPendiente";
                c.Caption = Resources.IniciativaResource.MontoPendiente;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasEstimadas";
                c.Caption = Resources.IniciativaResource.HorasEstimadas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasInvertidas";
                c.Caption = Resources.IniciativaResource.HorasInvertidas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PorcentajeAvance";
                c.Caption = Resources.IniciativaResource.PorcentajeAvance;
                c.PropertiesEdit.DisplayFormatString = "{0:n2}%";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdEstatus";
                c.Caption = Resources.IniciativaResource.Estatus;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetEstadosIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Urgente";
                c.Caption = Resources.IniciativaResource.Urgente;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPrioridadesIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Observacion";
                c.Caption = Resources.IniciativaResource.Observacion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            return settings;
        }
        public static GridViewSettings FormatConditionsExportGridViewSettings
        {
            get
            {
                var settings = Context[FormatConditionsExportGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[FormatConditionsExportGridViewSettingsKey] = settings = CreateFormatConditionsExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateFormatConditionsExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvIniciativas";
            settings.KeyFieldName = "IdIniciativa";
            settings.CallbackRouteValues = new { Controller = "PlanTrabajo", Action = "IniciativaPartialView" };
            settings.Width = Unit.Percentage(100);

            settings.TotalSummary.Add(SummaryItemType.Count, "NroIniciativa").DisplayFormat = "#,##0";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoEstimado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "PresupuestoReal").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoAbonado").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Sum, "MontoPendiente").DisplayFormat = "#,##0.00";
            settings.TotalSummary.Add(SummaryItemType.Average, "PorcentajeAvance").DisplayFormat = "{0:n2}%";

            settings.Columns.Add(c =>
            {
                c.FieldName = "NroIniciativa";
                c.Caption = Resources.IniciativaResource.NroIniciativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.Width = 80;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Nombre";
                c.Caption = Resources.IniciativaResource.Nombre;
                c.FixedStyle = GridViewColumnFixedStyle.Left;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Descripcion";
                c.Caption = Resources.IniciativaResource.Descripcion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "UnidadOrganizativa";
                c.Caption = Resources.IniciativaResource.UnidadOrganizativa;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Responsable";
                c.Caption = Resources.IniciativaResource.Responsable;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 250;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioEstimada";
                c.Caption = Resources.IniciativaResource.FechaInicioEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreEstimada";
                c.Caption = Resources.IniciativaResource.FechaCierreEstimada;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicioReal";
                c.Caption = Resources.IniciativaResource.FechaInicioReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaCierreReal";
                c.Caption = Resources.IniciativaResource.FechaCierreReal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = 100;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoEstimado";
                c.Caption = Resources.IniciativaResource.PresupuestoEstimado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PresupuestoReal";
                c.Caption = Resources.IniciativaResource.PresupuestoReal;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoAbonado";
                c.Caption = Resources.IniciativaResource.MontoAbonado;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "MontoPendiente";
                c.Caption = Resources.IniciativaResource.MontoPendiente;
                c.PropertiesEdit.DisplayFormatString = "#,##0.00";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasEstimadas";
                c.Caption = Resources.IniciativaResource.HorasEstimadas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "HorasInvertidas";
                c.Caption = Resources.IniciativaResource.HorasInvertidas;
                c.PropertiesEdit.DisplayFormatString = "##0";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "PorcentajeAvance";
                c.Caption = Resources.IniciativaResource.PorcentajeAvance;
                c.PropertiesEdit.DisplayFormatString = "{0:n2}%";
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.Settings.AllowAutoFilter = DefaultBoolean.False;
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdEstatus";
                c.Caption = Resources.IniciativaResource.Estatus;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetEstadosIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Urgente";
                c.Caption = Resources.IniciativaResource.Urgente;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPrioridadesIniciativa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "Observacion";
                c.Caption = Resources.IniciativaResource.Observacion;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 450;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            return settings;
        }

    }
    public class GridViewExportCuadroIO
    {
        static string ExcelDataAwareGridViewSettingsKey = "51172A18-2073-426B-A5CB-136347B3A79F";
        static string FormatConditionsExportGridViewSettingsKey = "14634B6F-E1DC-484A-9728-F9608615B628";
        static Dictionary<GridViewExportFormat, GridViewExportMethod> exportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> ExportFormatsInfo
        {
            get
            {
                if (exportFormatsInfo == null)
                    exportFormatsInfo = CreateExportFormatsInfo();
                return exportFormatsInfo;
            }
        }
        static IDictionary Context { get { return System.Web.HttpContext.Current.Items; } }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                {
                    GridViewExportFormat.Xls,
                    (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                {
                    GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf },
                {
                    GridViewExportFormat.Csv,
                    (settings, data) => GridViewExtension.ExportToCsv(settings, data, new CsvExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                }
            };
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> dataAwareExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> DataAwareExportFormatsInfo
        {
            get
            {
                if (dataAwareExportFormatsInfo == null)
                    dataAwareExportFormatsInfo = CreateDataAwareExportFormatsInfo();
                return dataAwareExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateDataAwareExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Xls, GridViewExtension.ExportToXls },
                { GridViewExportFormat.Xlsx, GridViewExtension.ExportToXlsx },
                { GridViewExportFormat.Csv, GridViewExtension.ExportToCsv }
            };
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> formatConditionsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> FormatConditionsExportFormatsInfo
        {
            get
            {
                if (formatConditionsExportFormatsInfo == null)
                    formatConditionsExportFormatsInfo = CreateFormatConditionsExportFormatsInfo();
                return formatConditionsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateFormatConditionsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf},
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data) },
                { GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG})
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> advancedBandsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> AdvancedBandsExportFormatsInfo
        {
            get
            {
                if (advancedBandsExportFormatsInfo == null)
                    advancedBandsExportFormatsInfo = CreateAdvancedBandsExportFormatsInfo();
                return advancedBandsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateAdvancedBandsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Xlsx, (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }
        public static string GetExportButtonTitle(GridViewExportFormat format)
        {
            if (format == GridViewExportFormat.None)
                return string.Empty;
            return string.Format("Export to {0}", format.ToString().ToUpper());
        }
        public static GridViewSettings CreateGeneralMasterGridSettings(object DataToBind)
        {
            return CreateGeneralMasterGridSettings(GridViewDetailExportMode.None, DataToBind);
        }
        public static GridViewSettings CreateGeneralMasterGridSettings(GridViewDetailExportMode exportMode, object DataToBind)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "gvIO";
            settings.SettingsDetail.MasterGridName = "gvCuadro";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "IdProceso";
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Proceso";
                c.Caption = Resources.ReporteResource.captionProceso;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "DescEscala";
                c.Caption = Resources.ReporteResource.captionDescripcion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Escala";
                c.Caption = Resources.ReporteResource.captionValoracion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
            });


            return settings;
        }
        public static GridViewSettings CreateGeneralDetailGridSettings(int uniqueKey)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "detailGrid" + uniqueKey;
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "ProductID";
            settings.Columns.Add("ProductID");
            settings.Columns.Add("ProductName");
            settings.Columns.Add("UnitPrice");
            settings.Columns.Add("QuantityPerUnit");

            settings.SettingsDetail.MasterGridName = "masterGrid";

            return settings;
        }
        static GridViewSettings exportGridViewSettings;
        public static GridViewSettings ExportGridViewSettings
        {
            get
            {
                if (exportGridViewSettings == null)
                    exportGridViewSettings = CreateExportGridViewSettings();
                return exportGridViewSettings;
            }
        }
        static GridViewSettings CreateExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvIO";
            settings.SettingsDetail.MasterGridName = "gvCuadro";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "IdProceso";
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Proceso";
                c.Caption = Resources.ReporteResource.captionProceso;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "DescEscala";
                c.Caption = Resources.ReporteResource.captionDescripcion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Escala";
                c.Caption = Resources.ReporteResource.captionValoracion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
            });

            return settings;
        }
        public static GridViewSettings ExcelDataAwareExportGridViewSettings
        {
            get
            {
                GridViewSettings settings = Context[ExcelDataAwareGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[ExcelDataAwareGridViewSettingsKey] = settings = CreateExcelDataAwareExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateExcelDataAwareExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvIO";
            settings.SettingsDetail.MasterGridName = "gvCuadro";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "IdProceso";
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Proceso";
                c.Caption = Resources.ReporteResource.captionProceso;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "DescEscala";
                c.Caption = Resources.ReporteResource.captionDescripcion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Escala";
                c.Caption = Resources.ReporteResource.captionValoracion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
            });


            return settings;
        }
        public static GridViewSettings FormatConditionsExportGridViewSettings
        {
            get
            {
                var settings = Context[FormatConditionsExportGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[FormatConditionsExportGridViewSettingsKey] = settings = CreateFormatConditionsExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateFormatConditionsExportGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Name = "gvIO";
            settings.SettingsDetail.MasterGridName = "gvCuadro";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "IdProceso";
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Proceso";
                c.Caption = Resources.ReporteResource.captionProceso;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "DescEscala";
                c.Caption = Resources.ReporteResource.captionDescripcion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.CellStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            });
            settings.Columns.Add(c =>
            {
                c.Settings.AllowSort = DefaultBoolean.True;
                c.FieldName = "Escala";
                c.Caption = Resources.ReporteResource.captionValoracion;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
            });


            return settings;
        }
    }
    public class GridViewExportProgramaciones
    {
        static string ExcelDataAwareGridViewSettingsKey = "51172A18-2073-426B-A5CB-136347B3A79F";
        static string FormatConditionsExportGridViewSettingsKey = "14634B6F-E1DC-484A-9728-F9608615B628";
        static Dictionary<GridViewExportFormat, GridViewExportMethod> exportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> ExportFormatsInfo
        {
            get
            {
                if (exportFormatsInfo == null)
                    exportFormatsInfo = CreateExportFormatsInfo();
                return exportFormatsInfo;
            }
        }

        static IDictionary Context { get { return System.Web.HttpContext.Current.Items; } }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                {
                    GridViewExportFormat.Xls,
                    (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                {
                    GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf },
                {
                    GridViewExportFormat.Csv,
                    (settings, data) => GridViewExtension.ExportToCsv(settings, data, new CsvExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> dataAwareExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> DataAwareExportFormatsInfo
        {
            get
            {
                if (dataAwareExportFormatsInfo == null)
                    dataAwareExportFormatsInfo = CreateDataAwareExportFormatsInfo();
                return dataAwareExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateDataAwareExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Xls, GridViewExtension.ExportToXls },
                { GridViewExportFormat.Xlsx, GridViewExtension.ExportToXlsx },
                { GridViewExportFormat.Csv, GridViewExtension.ExportToCsv }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> formatConditionsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> FormatConditionsExportFormatsInfo
        {
            get
            {
                if (formatConditionsExportFormatsInfo == null)
                    formatConditionsExportFormatsInfo = CreateFormatConditionsExportFormatsInfo();
                return formatConditionsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateFormatConditionsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf},
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data) },
                { GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG})
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> advancedBandsExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> AdvancedBandsExportFormatsInfo
        {
            get
            {
                if (advancedBandsExportFormatsInfo == null)
                    advancedBandsExportFormatsInfo = CreateAdvancedBandsExportFormatsInfo();
                return advancedBandsExportFormatsInfo;
            }
        }
        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateAdvancedBandsExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                { GridViewExportFormat.Xls, (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Xlsx, (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx {ExportType = DevExpress.Export.ExportType.WYSIWYG}) },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf }
            };
        }

        public static string GetExportButtonTitle(GridViewExportFormat format)
        {
            if (format == GridViewExportFormat.None)
                return string.Empty;
            return string.Format("Export to {0}", format.ToString().ToUpper());
        }

        public static GridViewSettings CreateGeneralMasterGridSettings(object DataToBind)
        {
            return CreateGeneralMasterGridSettings(GridViewDetailExportMode.None, DataToBind);
        }
        public static GridViewSettings CreateGeneralMasterGridSettings(GridViewDetailExportMode exportMode, object DataToBind)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "masterGrid";
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "CategoryID";
            settings.Columns.Add("CategoryID");
            settings.Columns.Add("CategoryName");
            settings.Columns.Add("Description");
            settings.Columns.Add(c => {
                c.FieldName = "Picture";
                c.ColumnType = MVCxGridViewColumnType.BinaryImage;
                BinaryImageEditProperties properties = (BinaryImageEditProperties)c.PropertiesEdit;
                properties.ImageWidth = 120;
                properties.ImageHeight = 80;
                properties.ExportImageSettings.Width = 90;
                properties.ExportImageSettings.Height = 60;
            });

            settings.SettingsDetail.ShowDetailRow = true;
            settings.SettingsDetail.ExportMode = exportMode;

            settings.SettingsExport.GetExportDetailGridViews = (s, e) => {
                int categoryID = (int)DataBinder.Eval(e.DataItem, "CategoryID");
                GridViewExtension grid = new GridViewExtension(CreateGeneralDetailGridSettings(categoryID));
                grid.Bind(DataToBind);
                e.DetailGridViews.Add(grid);
            };

            return settings;
        }

        public static GridViewSettings CreateGeneralDetailGridSettings(int uniqueKey)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "detailGrid" + uniqueKey;
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "ProductID";
            settings.Columns.Add("ProductID");
            settings.Columns.Add("ProductName");
            settings.Columns.Add("UnitPrice");
            settings.Columns.Add("QuantityPerUnit");

            settings.SettingsDetail.MasterGridName = "masterGrid";

            return settings;
        }

        static GridViewSettings exportGridViewSettings;
        public static GridViewSettings ExportGridViewSettings
        {
            get
            {
                if (exportGridViewSettings == null)
                    exportGridViewSettings = CreateExportGridViewSettings();
                return exportGridViewSettings;
            }
        }
        static GridViewSettings CreateExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvProgramacion";
            settings.KeyFieldName = "IdProgramacion";
            settings.CallbackRouteValues = new { Controller = "PMT", Action = "ProgramacionPartialView" };
            settings.Width = Unit.Percentage(100);

            settings.Columns.Add(c =>
            {
                c.FieldName = "IdModuloProgramacion";
                c.Caption = Resources.DocumentoResource.captionProgramacionModulo;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                //c.Width = Unit.Percentage(35);
                c.Width = Unit.Percentage(60);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Nombre";
                    p.ValueField = "IdModulo";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetModulosPrincipalesEmpresa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicio";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaInicio;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaFinal";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaFinal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdTipoFrecuencia";
                c.Caption = Resources.DocumentoResource.captionFrecuencia;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPMTTipoFrecuencia();
                });
            });

            return settings;
        }

        public static GridViewSettings ExcelDataAwareExportGridViewSettings
        {
            get
            {
                GridViewSettings settings = Context[ExcelDataAwareGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[ExcelDataAwareGridViewSettingsKey] = settings = CreateExcelDataAwareExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateExcelDataAwareExportGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "gvProgramacion";
            settings.KeyFieldName = "IdProgramacion";
            settings.CallbackRouteValues = new { Controller = "PMT", Action = "ProgramacionPartialView" };
            settings.Width = Unit.Percentage(100);

            settings.Columns.Add(c =>
            {
                c.FieldName = "IdModuloProgramacion";
                c.Caption = Resources.DocumentoResource.captionProgramacionModulo;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                //c.Width = Unit.Percentage(35);
                c.Width = Unit.Percentage(60);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Nombre";
                    p.ValueField = "IdModulo";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetModulosPrincipalesEmpresa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicio";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaInicio;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaFinal";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaFinal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdTipoFrecuencia";
                c.Caption = Resources.DocumentoResource.captionFrecuencia;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPMTTipoFrecuencia();
                });
            });


            return settings;
        }
        public static GridViewSettings FormatConditionsExportGridViewSettings
        {
            get
            {
                var settings = Context[FormatConditionsExportGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[FormatConditionsExportGridViewSettingsKey] = settings = CreateFormatConditionsExportGridViewSettings();
                return settings;
            }
        }
        static GridViewSettings CreateFormatConditionsExportGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gvProgramacion";
            settings.KeyFieldName = "IdProgramacion";
            settings.CallbackRouteValues = new { Controller = "PMT", Action = "ProgramacionPartialView" };
            settings.Width = Unit.Percentage(100);

            settings.Columns.Add(c =>
            {
                c.FieldName = "IdModuloProgramacion";
                c.Caption = Resources.DocumentoResource.captionProgramacionModulo;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                //c.Width = Unit.Percentage(35);
                c.Width = Unit.Percentage(60);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Nombre";
                    p.ValueField = "IdModulo";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetModulosPrincipalesEmpresa();
                });
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaInicio";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaInicio;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "FechaFinal";
                c.Caption = Resources.DocumentoResource.captionProgramacionFechaFinal;
                c.ColumnType = MVCxGridViewColumnType.DateEdit;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
            });
            settings.Columns.Add(c =>
            {
                c.FieldName = "IdTipoFrecuencia";
                c.Caption = Resources.DocumentoResource.captionFrecuencia;
                c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                c.Width = Unit.Percentage(10);
                c.EditorProperties().ComboBox(p =>
                {
                    p.TextField = "Descripcion";
                    p.ValueField = "Id";
                    p.ValueType = typeof(long);
                    p.DataSource = Metodos.GetPMTTipoFrecuencia();
                });
            });


            return settings;
        }
    }
}