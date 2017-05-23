using BCMWeb.Data.EF;
using BCMWeb.Security;
using DevExpress.Web.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace BCMWeb
{
    [Authorize]
    public static class ProcesarDocumento
    {
        private static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        private static string Culture = HttpContext.Current.Request.UserLanguages[0];
        private static Encriptador _Encriptar = new Encriptador();

        [SessionExpire]
        [HandleError]
        public static void ProcesarContenidoDocumento(long ModuloId, byte[] Contenido)
        {
            if (Contenido != null)
            {
                // string _filePath = SaveFile(new MemoryStream(Contenido));
                MemoryStream msContent = new MemoryStream(Contenido);
                long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

                try
                {
                    switch (ModuloId)
                    {
                        case 4010100: // ficha del BIA
                            ProcessFichaEstandar(msContent);
                            //ProcessFichaBIA(msContent);
                            break;
                        case 4020200: // Entradas del Proceso
                            ProcesarEntradasdelProceso(msContent);
                            break;
                        case 4020300: // Proveedores
                            ProcesarProveedores(msContent);
                            break;
                        case 4020400: // Interdependencias
                            ProcesarInterdependencias(msContent);
                            break;
                        case 4020500: // Clientes y Productos
                            ProcesarClientesyProductos(msContent);
                            break;
                        case 4020600: // Tecnología
                            ProcesarTecnología(msContent);
                            break;
                        case 4020700: // Información Esencial
                            ProcesarInformaciónEsencial(msContent);
                            break;
                        case 4020800: // Personal Clave
                            ProcesarPersonalClave(msContent);
                            break;
                        case 4040100: // Impacto Financiero
                            ProcesarImpactoFinanciero(msContent);
                            break;
                        case 4040200: // Impacto Operacional
                            ProcesarImpactoOperacional(msContent);
                            break;
                        case 4030300: // Escalas
                            ProcesarAnalisisRiesgo(msContent);
                            break;
                        case 4050100: // TMC
                            ProcesarTMC(msContent);
                            break;
                        case 4050200: // TOR
                            ProcesarTOR(msContent);
                            break;
                        case 4050300: // POR
                            ProcesarPOR(msContent);
                            break;
                        case 4050400: // TRT
                            ProcesarTRT(msContent);
                            break;
                        case 4060100: // Procedimientos Alternos
                            ProcesarProcedimientosAlternos(msContent);
                            break;
                        case 4070100: // Ubicación Principal
                            ProcesarUbicaciónPrincipal(msContent);
                            break;
                        case 4070200: // Ubicación Alterna
                            ProcesarUbicaciónAlterna(msContent);
                            break;
                        case 4080100: // Grandes Impactos
                            ProcesarGrandesImpactos(msContent);
                            break;
                        case 3030100: // Amenazas
                            if (IdEmpresa == 9)
                                ProcesarAnalisisRiesgo(msContent);
                            break;
                        case 3040100: // Amenazas
                            if (IdEmpresa == 14)
                                ProcesarAnalisisRiesgo(msContent);
                            break;
                        case 7000101: // Ficha del BCP
                        case 1010100:
                        case 2010100:
                        case 3010100:
                        case 5010100:
                        case 6010100:
                        case 8010100:
                        case 9010100:
                        case 10010100:
                           if (IdEmpresa > 13) 
                                ProcessFichaEstandar(msContent);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //DeleteFile(_filePath);
            }
        }

        private static void ProcesarGrandesImpactos(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAGranImpacto> Actuales = db.tblBIAGranImpacto.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                          && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    List<tblBIAProceso> _Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA).ToList();
                    db.tblBIAGranImpacto.RemoveRange(Actuales);
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        string[,] _Datos = new string[_Procesos.Count, 12];
                        int _NroTabla = 0;

                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                _NroTabla++;

                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (int _Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    if (_NroTabla == 1) { 
                                        TableRow _tableRow = (TableRow)_tableRows[_Row];

                                        if (_tableRow.HasChildren)
                                        {
                                            List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                            for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                        {
                                                            if (!string.IsNullOrEmpty(_textoCelda))
                                                                _textoCelda += '/';
                                                            _textoCelda += _cellParagraph.InnerText.Trim();

                                                        }
                                                    } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                    {
                                                        if ((_Row - 1) < _Procesos.Count)
                                                            _Datos[_Row - 1, _Cell] = _Procesos[_Row - 1].IdProceso.ToString();
                                                    }

                                                } // End if (_celda.HasChildren)
                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        }
                                        else
                                        {
                                            _Row = 0;
                                            _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                            for (_Row = 1; _Row < _tableRows.Count(); _Row++)
                                            {
                                                _tableRow = (TableRow)_tableRows[_Row];

                                                string _Explicacion = string.Empty;

                                                if (_tableRow.HasChildren)
                                                {
                                                    List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                                    for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                                    {
                                                        TableCell _celda = (TableCell)_tableCells[_Cell];
                                                        if (_celda.HasChildren)
                                                        {
                                                            string _textoCelda = string.Empty;
                                                            List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                            foreach (var _cellParagraph in _tableCellParagraph)
                                                            {
                                                                if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                                {
                                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                                        _textoCelda += " ";
                                                                    _textoCelda += _cellParagraph.InnerText.Trim();

                                                                }
                                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                            switch (_Cell)
                                                            {
                                                                case 0:
                                                                    IdProceso = 0;
                                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                                    {
                                                                        _NombreProceso = _textoCelda;
                                                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                                        if (procBIA != null)
                                                                            IdProceso = procBIA.IdProceso;
                                                                    }
                                                                    break;
                                                                case 1:
                                                                    _Explicacion = _textoCelda;
                                                                    break;
                                                            }
                                                        } // End if (_celda.HasChildren)
                                                    } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                                    for (int _iDato = 0; _iDato < 12; _iDato++)
                                                    {
                                                        if (!string.IsNullOrEmpty(_Datos[_Row - 1, _iDato]))
                                                        {
                                                            tblBIAGranImpacto reg = new tblBIAGranImpacto
                                                            {
                                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                                IdEmpresa = _IdEmpresa,
                                                                IdProceso = long.Parse(_Datos[_Row - 1, _iDato]),
                                                                IdMes = _iDato + 1,
                                                                Explicacion = _Explicacion,
                                                                Observacion = string.Empty
                                                            };

                                                            db.tblBIAGranImpacto.Add(reg);

                                                        }
                                                    }

                                                } // End if (_tableCell.HasChildren)
                                            } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                        } // End if (_tableRow.HasChildren)                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarUbicaciónAlterna(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIARespaldoSecundario> Actuales = db.tblBIARespaldoSecundario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                              && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIARespaldoSecundario.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Ubicacion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += " ";
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        IdProceso = 0;
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _NombreProceso = _textoCelda;
                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA != null)
                                                                IdProceso = procBIA.IdProceso;
                                                        }
                                                        break;
                                                    case 1:
                                                        _Ubicacion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIARespaldoSecundario reg = new tblBIARespaldoSecundario
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Ubicacion = _Ubicacion
                                        };

                                        db.tblBIARespaldoSecundario.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarUbicaciónPrincipal(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIARespaldoPrimario> Actuales = db.tblBIARespaldoPrimario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                              && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIARespaldoPrimario.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Ubicacion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += " ";
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)
                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        IdProceso = 0;
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _NombreProceso = _textoCelda;
                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA != null)
                                                                IdProceso = procBIA.IdProceso;
                                                        }
                                                        break;
                                                    case 1:
                                                        _Ubicacion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIARespaldoPrimario reg = new tblBIARespaldoPrimario
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Ubicacion = _Ubicacion
                                        };

                                        db.tblBIARespaldoPrimario.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarProcedimientosAlternos(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAProcesoAlterno> Actuales = db.tblBIAProcesoAlterno.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                          && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAProcesoAlterno.RemoveRange(Actuales);
                    string _ProcesoAlterno = string.Empty;

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                   case 0:
                                                        IdProceso = 0;
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _NombreProceso = _textoCelda;
                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA != null)
                                                                IdProceso = procBIA.IdProceso;
                                                        }
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda)) 
                                                        _ProcesoAlterno = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProcesoAlterno reg = new tblBIAProcesoAlterno
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            ProcesoAlterno = _ProcesoAlterno
                                        };

                                        db.tblBIAProcesoAlterno.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarTRT(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAWRT> Actuales = db.tblBIAWRT.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAWRT.RemoveRange(Actuales);
                    db.SaveChanges();

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();
                                long[] _IdEscala = { };

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        long ValorEscala = 0;

                                        for (int _Cell = 1; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            string _textoCelda = string.Empty;
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                            foreach (var _cellParagraph in _tableCellParagraph)
                                            {
                                                if (!string.IsNullOrEmpty(_textoCelda))
                                                    _textoCelda += '/';

                                                if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    _textoCelda += _cellParagraph.InnerText.Trim();

                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                            switch (_Cell)
                                            {
                                                case 0:
                                                    try
                                                    {
                                                        _NroProceso = (int.Parse(_textoCelda));
                                                    }
                                                    catch
                                                    {
                                                        _Row = GetNextRow(_tableRows, _Row, out _NroProceso);

                                                    }
                                                    _NroProceso = ((int)_Documento.NroDocumento * 100) + _NroProceso;
                                                    break;
                                                case 1:
                                                    _NombreProceso = _textoCelda;
                                                    break;
                                                case 2:
                                                case 3:
                                                    ValorEscala = 5;
                                                    break;
                                                case 4:
                                                    ValorEscala = 4;
                                                    break;
                                                case 5:
                                                    ValorEscala = 3;
                                                    break;
                                                case 6:
                                                    ValorEscala = 2;
                                                    break;
                                                case 7:
                                                case 8:
                                                    ValorEscala = 1;
                                                    break;
                                            } // End switch

                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                         && x.Nombre == _NombreProceso).FirstOrDefault();

                                        if (procBIA != null)
                                        {

                                            tblEscala Escala = db.tblEscala.Where(x => x.IdEmpresa == procBIA.IdEmpresa
                                                                                    && x.IdTipoEscala == 6
                                                                                    && x.Valor == ValorEscala).FirstOrDefault();

                                            tblBIAWRT reg = new tblBIAWRT
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = procBIA.IdEmpresa,
                                                IdProceso = procBIA.IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = Escala.IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIAWRT.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarPOR(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIARPO> Actuales = db.tblBIARPO.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIARPO.RemoveRange(Actuales);
                    db.SaveChanges();

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();
                                long[] _IdEscala = { };

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        long ValorEscala = 0;

                                        for (int _Cell = 1; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            string _textoCelda = string.Empty;
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                            foreach (var _cellParagraph in _tableCellParagraph)
                                            {
                                                if (!string.IsNullOrEmpty(_textoCelda))
                                                    _textoCelda += '/';

                                                if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    _textoCelda += _cellParagraph.InnerText.Trim();

                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                            switch (_Cell)
                                            {
                                                case 0:
                                                    try
                                                    {
                                                        _NroProceso = (int.Parse(_textoCelda));
                                                    }
                                                    catch
                                                    {
                                                        _Row = GetNextRow(_tableRows, _Row, out _NroProceso);

                                                    }
                                                    _NroProceso = ((int)_Documento.NroDocumento * 100) + _NroProceso;
                                                    break;
                                                case 1:
                                                    _NombreProceso = _textoCelda;
                                                    break;
                                                case 2:
                                                case 3:
                                                    ValorEscala = 5;
                                                    break;
                                                case 4:
                                                    ValorEscala = 4;
                                                    break;
                                                case 5:
                                                    ValorEscala = 3;
                                                    break;
                                                case 6:
                                                    ValorEscala = 2;
                                                    break;
                                                case 7:
                                                case 8:
                                                    ValorEscala = 1;
                                                    break;
                                            } // End switch

                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                         && x.Nombre == _NombreProceso).FirstOrDefault();

                                        if (procBIA != null)
                                        {

                                            tblEscala Escala = db.tblEscala.Where(x => x.IdEmpresa == procBIA.IdEmpresa
                                                                                    && x.IdTipoEscala == 5
                                                                                    && x.Valor == ValorEscala).FirstOrDefault();

                                            tblBIARPO reg = new tblBIARPO
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = procBIA.IdEmpresa,
                                                IdProceso = procBIA.IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = Escala.IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIARPO.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarTOR(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIARTO> Actuales = db.tblBIARTO.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIARTO.RemoveRange(Actuales);
                    db.SaveChanges();

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();
                                long[] _IdEscala = { };

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        long ValorEscala = 0;

                                        for (int _Cell = 1; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            string _textoCelda = string.Empty;
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                            foreach (var _cellParagraph in _tableCellParagraph)
                                            {
                                                if (!string.IsNullOrEmpty(_textoCelda))
                                                    _textoCelda += '/';

                                                if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    _textoCelda += _cellParagraph.InnerText.Trim();

                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                            switch (_Cell)
                                            {
                                                case 0:
                                                    try
                                                    {
                                                        _NroProceso = (int.Parse(_textoCelda));
                                                    }
                                                    catch
                                                    {
                                                        _Row = GetNextRow(_tableRows, _Row, out _NroProceso);

                                                    }
                                                    _NroProceso = ((int)_Documento.NroDocumento * 100) + _NroProceso;
                                                    break;
                                                case 1:
                                                    _NombreProceso = _textoCelda;
                                                    break;
                                                case 2:
                                                case 3:
                                                    ValorEscala = 5;
                                                    break;
                                                case 4:
                                                    ValorEscala = 4;
                                                    break;
                                                case 5:
                                                    ValorEscala = 3;
                                                    break;
                                                case 6:
                                                    ValorEscala = 2;
                                                    break;
                                                case 7:
                                                case 8:
                                                    ValorEscala = 1;
                                                    break;
                                            } // End switch

                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                         && x.Nombre == _NombreProceso).FirstOrDefault();

                                        if (procBIA != null)
                                        {

                                            tblEscala Escala = db.tblEscala.Where(x => x.IdEmpresa == procBIA.IdEmpresa
                                                                                    && x.IdTipoEscala == 4
                                                                                    && x.Valor == ValorEscala).FirstOrDefault();

                                            tblBIARTO reg = new tblBIARTO
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = procBIA.IdEmpresa,
                                                IdProceso = procBIA.IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = Escala.IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIARTO.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarTMC(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAMTD> Actuales = db.tblBIAMTD.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAMTD.RemoveRange(Actuales);
                    db.SaveChanges();

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();
                                long[] _IdEscala = { };

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        long ValorEscala = 0;

                                        for (int _Cell = 1; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            string _textoCelda = string.Empty;
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                            foreach (var _cellParagraph in _tableCellParagraph)
                                            {
                                                if (!string.IsNullOrEmpty(_textoCelda))
                                                    _textoCelda += '/';

                                                if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    _textoCelda += _cellParagraph.InnerText.Trim();

                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                            switch (_Cell)
                                            {
                                                case 0:
                                                    try
                                                    {
                                                        _NroProceso = (int.Parse(_textoCelda));
                                                    }
                                                    catch
                                                    {
                                                        _Row = GetNextRow(_tableRows, _Row, out _NroProceso);

                                                    }
                                                    _NroProceso = ((int)_Documento.NroDocumento * 100) + _NroProceso;
                                                    break;
                                                case 1:
                                                    _NombreProceso = _textoCelda;
                                                    break;
                                                case 2:
                                                case 3:
                                                    ValorEscala = 5;
                                                    break;
                                                case 4:
                                                    ValorEscala = 4;
                                                    break;
                                                case 5:
                                                    ValorEscala = 3;
                                                    break;
                                                case 6:
                                                    ValorEscala = 2;
                                                    break;
                                                case 7:
                                                case 8:
                                                    ValorEscala = 1;
                                                    break;
                                            } // End switch

                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                         && x.Nombre == _NombreProceso).FirstOrDefault();

                                        if (procBIA != null)
                                        {

                                            tblEscala Escala = db.tblEscala.Where(x => x.IdEmpresa == procBIA.IdEmpresa
                                                                                    && x.IdTipoEscala == 3
                                                                                    && x.Valor == ValorEscala).FirstOrDefault();

                                            tblBIAMTD reg = new tblBIAMTD
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = procBIA.IdEmpresa,
                                                IdProceso = procBIA.IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = Escala.IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIAMTD.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarAnalisisRiesgo(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            string _Descripcion = string.Empty;
            int _startRow = 1;
            List<objAmenaza> Amenazas = new List<objAmenaza>();

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();

                    List<tblBIAAmenaza> Actuales = db.tblBIAAmenaza.Where(x => x.IdEmpresa == _IdEmpresa
                                                                            && x.IdDocumento == _IdDocumento).ToList();

                    db.tblBIAAmenaza.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        var _tables = wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList();

                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        objAmenaza _Amenaza;
                                        _Amenaza = new objAmenaza();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                        _textoCelda += _cellParagraph.InnerText.Trim();
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NroProceso = (int.Parse(_textoCelda));
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;


                                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                         && x.Nombre == _NombreProceso).FirstOrDefault();
                                                        if (procBIA != null)
                                                        {
                                                            _Amenaza.IdDocumentoBIA = procBIA.IdDocumentoBia;
                                                            _Amenaza.IdProceso = procBIA.IdProceso;
                                                        }
                                                        break;
                                                    case 2:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Descripcion = _textoCelda;
                                                        }
                                                        break;
                                                    case 3:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Evento = _textoCelda;
                                                        }
                                                        break;
                                                    case 4:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Probabilidad = short.Parse(_textoCelda);
                                                        }
                                                        break;
                                                    case 5:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Impacto = short.Parse(_textoCelda);
                                                        }
                                                        break;
                                                    case 6:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Control = short.Parse(_textoCelda);
                                                        }
                                                        break;
                                                    case 7:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Severidad = short.Parse(_textoCelda);
                                                        }
                                                        break;
                                                    case 8:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            _Amenaza.Fuente = _textoCelda;
                                                        }
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        _Amenaza.Implantado = string.Empty;
                                        _Amenaza.Implantar = string.Empty;
                                        int _Estado = _Amenaza.Probabilidad + _Amenaza.Impacto + _Amenaza.Control;
                                        if (_Estado >= 6)
                                            _Amenaza.Estado = 3;
                                        else if (_Estado == 4 || _Estado == 5)
                                            _Amenaza.Estado = 2;
                                        else
                                            _Amenaza.Estado = 1;

                                        if (_Amenaza.IdProceso > 0 && _Amenaza.IdDocumentoBIA > 0)
                                        {
                                            tblBIAProceso test = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa && x.IdDocumentoBia == _Amenaza.IdDocumentoBIA && x.IdProceso == _Amenaza.IdProceso).FirstOrDefault();
                                            tblBIAAmenaza dataAmenaza = new tblBIAAmenaza
                                            {
                                                IdEmpresa = _IdEmpresa,
                                                IdDocumento = _IdDocumento,
                                                IdDocumentoBIA = _Amenaza.IdDocumentoBIA,
                                                IdProceso = _Amenaza.IdProceso,
                                                IdTipoDocumento = _Documento.IdTipoDocumento,
                                                Descripcion = _Descripcion,
                                                Control = _Amenaza.Control,
                                                ControlesImplantar = _Amenaza.Implantar,
                                                Estado = _Amenaza.Estado,
                                                Evento = _Amenaza.Evento,
                                                Fuente = _Amenaza.Fuente,
                                                Impacto = _Amenaza.Impacto,
                                                Probabilidad = _Amenaza.Probabilidad,
                                                Severidad = _Amenaza.Severidad,
                                                TipoControlImplantado = _Amenaza.Implantado
                                            };

                                            db.tblBIAAmenaza.Add(dataAmenaza);
                                            try
                                            {
                                                db.SaveChanges();
                                            }
                                            catch (Exception ex)
                                            {
                                                if (_Amenaza.IdDocumentoBIA != 81)
                                                    throw ex;
                                            } // End catch
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarImpactoOperacional(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            string _UnidadTiempo = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    List<tblBIAImpactoOperacional> Actuales = db.tblBIAImpactoOperacional.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAImpactoOperacional.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Impacto = string.Empty;
                                    string _Descripcion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                          _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _UnidadTiempo = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Impacto = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _Descripcion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblCultura_TipoFrecuencia Cultura_TipoFrecuencia =
                                                        db.tblCultura_TipoFrecuencia.Where(x => (x.Culture == Culture || x.Culture == "es-VE") && _UnidadTiempo.Contains(x.Descripcion)).FirstOrDefault();

                                                    tblBIAImpactoOperacional reg = new tblBIAImpactoOperacional
                                                    {
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdTipoFrecuencia = Cultura_TipoFrecuencia.IdTipoFrecuencia,
                                                        IdProceso = procBIA.IdProceso,
                                                        Descripcion = _Descripcion,
                                                        ImpactoOperacional = _Impacto,
                                                        UnidadTiempo = _UnidadTiempo,
                                                        IdEscala = null
                                                    };

                                                    db.tblBIAImpactoOperacional.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarImpactoFinanciero(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 1;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    List<tblBIAImpactoFinanciero> Actuales = db.tblBIAImpactoFinanciero.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAImpactoFinanciero.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _UnidadTiempo = string.Empty;
                                    string _Impacto = string.Empty;
                                    string _Descripcion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _UnidadTiempo = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Impacto = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _Descripcion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblCultura_TipoFrecuencia Cultura_TipoFrecuencia =
                                                        db.tblCultura_TipoFrecuencia.Where(x => (x.Culture == Culture || x.Culture == "es-VE") && _UnidadTiempo.Contains(x.Descripcion)).FirstOrDefault();

                                                    tblBIAImpactoFinanciero reg = new tblBIAImpactoFinanciero
                                                    {
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdTipoFrecuencia = Cultura_TipoFrecuencia.IdTipoFrecuencia,
                                                        IdProceso = procBIA.IdProceso,
                                                        Descripcion = _Descripcion,
                                                        Impacto = _Impacto,
                                                        UnidadTiempo = _UnidadTiempo,
                                                        IdEscala = null
                                                    };

                                                    db.tblBIAImpactoFinanciero.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarPersonalClave(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAPersonaClave> Actuales = db.tblBIAPersonaClave.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                  && x.IdDocumento == _IdDocumento
                                                                                                  && x.IdTipoDocumento == _IdTipoDocumento).ToList();

                    db.tblBIAPersonaClave.RemoveRange(Actuales);


                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Nombre = string.Empty;
                                    string _Identificacion = string.Empty;
                                    string _TelefonoOficina = string.Empty;
                                    string _TelefonoCelular = string.Empty;
                                    string _TelefonoHabitacion = string.Empty;
                                    string _CorreosElectronicos = string.Empty;
                                    string _DireccionHabitacion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _Nombre = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Identificacion = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _TelefonoOficina = _textoCelda;
                                                        break;
                                                    case 5:
                                                        _TelefonoCelular = _textoCelda;
                                                        break;
                                                    case 6:
                                                        _TelefonoHabitacion = _textoCelda;
                                                        break;
                                                    case 7:
                                                        _CorreosElectronicos = _textoCelda;
                                                        break;
                                                    case 8:
                                                        _DireccionHabitacion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblDocumentoPersonaClave _dataPersonaClave = db.tblDocumentoPersonaClave.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                                         && x.IdDocumento == _IdDocumento
                                                                                                                         && x.IdTipoDocumento == _IdTipoDocumento
                                                                                                                         && x.Nombre == _Nombre).FirstOrDefault();

                                                    if (_dataPersonaClave == null)
                                                    {
                                                        _dataPersonaClave = new tblDocumentoPersonaClave
                                                        {
                                                            Cedula = _Identificacion,
                                                            Correo = _CorreosElectronicos,
                                                            DireccionHabitacion = _DireccionHabitacion,
                                                            IdDocumento = _IdDocumento,
                                                            IdEmpresa = _IdEmpresa,
                                                            IdTipoDocumento = _IdTipoDocumento,
                                                            Nombre = _Nombre,
                                                            Principal = null,
                                                            TelefonoCelular = _TelefonoCelular,
                                                            TelefonoHabitacion = _TelefonoHabitacion,
                                                            TelefonoOficina = _TelefonoOficina
                                                        };

                                                        db.tblDocumentoPersonaClave.Add(_dataPersonaClave);
                                                        db.SaveChanges();
                                                    }

                                                    tblBIAPersonaClave reg = db.tblBIAPersonaClave.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                                && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA
                                                                                                                && x.IdProceso == procBIA.IdProceso
                                                                                                                && x.IdPersonaClave == _dataPersonaClave.IdPersona).FirstOrDefault();

                                                    if (reg == null)
                                                    {
                                                        reg = new tblBIAPersonaClave
                                                        {
                                                            IdDocumento = _IdDocumento,
                                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                            IdEmpresa = _IdEmpresa,
                                                            IdTipoDocumento = _IdTipoDocumento,
                                                            IdPersonaClave = _dataPersonaClave.IdPersona,
                                                            IdProceso = procBIA.IdProceso,
                                                        };
                                                        db.tblBIAPersonaClave.Add(reg);
                                                        db.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarInformaciónEsencial(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _Ubicacion = string.Empty;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIADocumentacion> Actuales = db.tblBIADocumentacion.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                          && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIADocumentacion.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Informacion = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _Informacion = _textoCelda;
                                                        break;
                                                    case 3:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _Ubicacion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblBIADocumentacion reg = new tblBIADocumentacion
                                                    {
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdProceso = procBIA.IdProceso,
                                                        Nombre = _Informacion,
                                                        Ubicacion = _Ubicacion
                                                    };

                                                    db.tblBIADocumentacion.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarTecnología(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _Usuarios = string.Empty;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAAplicacion> Actuales = db.tblBIAAplicacion.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                  && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAAplicacion.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Aplicaciones = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _Aplicaciones = _textoCelda;
                                                        break;
                                                    case 3:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _Usuarios = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblBIAAplicacion reg = new tblBIAAplicacion
                                                    {
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdProceso = procBIA.IdProceso,
                                                        Nombre = _Aplicaciones,
                                                        Usuario = _Usuarios
                                                    };

                                                    db.tblBIAAplicacion.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarClientesyProductos(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAClienteProceso> Actuales = db.tblBIAClienteProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                          && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAClienteProceso.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _UnidadTrabajo = string.Empty;
                                    string _ResponsableUnidad = string.Empty;
                                    string _ProcesoUnidad = string.Empty;
                                    string _Servicio = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _UnidadTrabajo = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _ResponsableUnidad = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _ProcesoUnidad = _textoCelda;
                                                        break;
                                                    case 5:
                                                        _Servicio = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblBIAClienteProceso reg = new tblBIAClienteProceso
                                                    {
                                                        Proceso = _ProcesoUnidad,
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdProceso = procBIA.IdProceso,
                                                        Producto = _Servicio,
                                                        Responsable = _ResponsableUnidad,
                                                        Unidad = _UnidadTrabajo
                                                    };

                                                    db.tblBIAClienteProceso.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarInterdependencias(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAInterdependencia> Actuales = db.tblBIAInterdependencia.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                          && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAInterdependencia.RemoveRange(Actuales);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Organizaciones = string.Empty;
                                    string _Contacto = string.Empty;
                                    string _Servicio = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _Organizaciones = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Servicio = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _Contacto = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {
                                                    tblBIAInterdependencia reg = new tblBIAInterdependencia
                                                    {
                                                        Contacto = _Contacto,
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdProceso = procBIA.IdProceso,
                                                        Organizacion = _Organizaciones,
                                                        Servicio = _Servicio
                                                    };

                                                    db.tblBIAInterdependencia.Add(reg);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarProveedores(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string _NrosProceso = string.Empty;
            int _NroProceso = 0;
            string _NombreProceso = string.Empty;
            string _DescripcionProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdDocumento == _IdDocumento
                                                                         && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();
                    List<tblBIAProveedor> Proveedores = db.tblBIAProveedor.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                   && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAProveedor.RemoveRange(Proveedores);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Organizacion = string.Empty;
                                    string _Servicio = string.Empty;
                                    string _Contacto = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += '/';
                                                        _textoCelda += _cellParagraph.InnerText.Trim();

                                                    }
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NrosProceso = _textoCelda;
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        _Organizacion = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Servicio = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _Contacto = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        string[] _aNrosProceso = _NrosProceso.Split('/');

                                        foreach (string _nroProceso in _aNrosProceso)
                                        {
                                            try
                                            {
                                                _NroProceso = ((int)_Documento.NroDocumento * 100) + int.Parse(_nroProceso);

                                                tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                if (procBIA != null)
                                                {

                                                    tblBIAProveedor Proveedor = new tblBIAProveedor
                                                    {
                                                        Contacto = _Contacto,
                                                        IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                        IdEmpresa = _IdEmpresa,
                                                        IdProceso = procBIA.IdProceso,
                                                        Organizacion = _Organizacion,
                                                        Servicio = _Servicio
                                                    };

                                                    db.tblBIAProveedor.Add(Proveedor);
                                                }
                                                else
                                                {
                                                    throw new Exception("No existe el proceso");
                                                }
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void ProcesarEntradasdelProceso(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            string _DescripcionProceso = string.Empty;
            int _startRow = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    tblDocumento _Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                  && x.IdDocumento == _IdDocumento
                                                                  && x.IdTipoDocumento == _IdTipoDocumento).FirstOrDefault();

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();
                    tblBIADocumento _DocBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                         && x.IdTipoDocumento == _IdTipoDocumento
                                                                         && x.IdDocumento == _IdDocumento).FirstOrDefault();
                    List<tblBIAProceso> Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa && x.IdDocumentoBia == _IdDocumento).ToList();

                    foreach (tblBIAProceso Proceso in Procesos)
                    {
                        db.tblBIAAmenaza.RemoveRange(Proceso.tblBIAAmenaza.ToList());
                        db.tblBCPDocumento.RemoveRange(Proceso.tblBCPDocumento.ToList());
                        db.tblBIAAplicacion.RemoveRange(Proceso.tblBIAAplicacion.ToList());
                        db.tblBIAClienteProceso.RemoveRange(Proceso.tblBIAClienteProceso.ToList());
                        db.tblBIADocumentacion.RemoveRange(Proceso.tblBIADocumentacion.ToList());
                        db.tblBIAEntrada.RemoveRange(Proceso.tblBIAEntrada.ToList());
                        db.tblBIAGranImpacto.RemoveRange(Proceso.tblBIAGranImpacto.ToList());
                        db.tblBIAImpactoFinanciero.RemoveRange(Proceso.tblBIAImpactoFinanciero.ToList());
                        db.tblBIAImpactoOperacional.RemoveRange(Proceso.tblBIAImpactoOperacional.ToList());
                        db.tblBIAInterdependencia.RemoveRange(Proceso.tblBIAInterdependencia.ToList());
                        db.tblBIAMTD.RemoveRange(Proceso.tblBIAMTD.ToList());
                        db.tblBIAPersonaClave.RemoveRange(Proceso.tblBIAPersonaClave.ToList());
                        db.tblBIAPersonaRespaldoProceso.RemoveRange(Proceso.tblBIAPersonaRespaldoProceso.ToList());
                        db.tblBIAProcesoAlterno.RemoveRange(Proceso.tblBIAProcesoAlterno.ToList());
                        db.tblBIAProveedor.RemoveRange(Proceso.tblBIAProveedor.ToList());
                        db.tblBIARespaldoPrimario.RemoveRange(Proceso.tblBIARespaldoPrimario.ToList());
                        db.tblBIARespaldoSecundario.RemoveRange(Proceso.tblBIARespaldoSecundario.ToList());
                        db.tblBIARPO.RemoveRange(Proceso.tblBIARPO.ToList());
                        db.tblBIARTO.RemoveRange(Proceso.tblBIARTO.ToList());
                        db.tblBIAUnidadTrabajoProceso.RemoveRange(Proceso.tblBIAUnidadTrabajoProceso.ToList());
                        db.tblBIAWRT.RemoveRange(Proceso.tblBIAWRT.ToList());
                    }

                    db.tblBIAProceso.RemoveRange(Procesos);
                    db.SaveChanges();

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        List<DocumentFormat.OpenXml.OpenXmlElement> _Tables = wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList();
                        foreach (var _Elemento in _Tables)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                _startRow = GetNextRow(_tableRows, _startRow, out _NroProceso);

                                for (_Row = _startRow; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _UnidadSolicitante = string.Empty;
                                    string _ProcesoActiva = string.Empty;
                                    string _NombreResponsableActiva = string.Empty;

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                        for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        {
                                            TableCell _celda = (TableCell)_tableCells[_Cell];
                                            if (_celda.HasChildren)
                                            {
                                                string _textoCelda = string.Empty;
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_cellParagraph.InnerText.Trim()))
                                                        _textoCelda += _cellParagraph.InnerText.Trim();
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {
                                                            try
                                                            {
                                                                _NroProceso = (int.Parse(_textoCelda));
                                                            }
                                                            catch
                                                            {
                                                                _Row = GetNextRow(_tableRows, _Row, out _NroProceso);

                                                            }
                                                            _NroProceso = ((int)_Documento.NroDocumento * 100) + _NroProceso;
                                                        }
                                                        break;
                                                    case 1:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        break;
                                                    case 2:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {

                                                            _DescripcionProceso = _textoCelda;

                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.Nombre == _NombreProceso).FirstOrDefault();

                                                            if (procBIA == null)
                                                            {
                                                                procBIA = new tblBIAProceso
                                                                {
                                                                    Critico = false,
                                                                    Descripcion = _DescripcionProceso,
                                                                    FechaCreacion = DateTime.UtcNow,
                                                                    FechaUltimoEstatus = DateTime.UtcNow,
                                                                    IdDocumentoBia = _DocBIA.IdDocumentoBIA,
                                                                    IdEmpresa = _IdEmpresa,
                                                                    IdEstadoProceso = (int)eEstadoProceso.Activo,
                                                                    IdUnidadOrganizativa = _DocBIA.IdUnidadOrganizativa,
                                                                    Nombre = _NombreProceso,
                                                                    NroProceso = _NroProceso,
                                                                };

                                                                db.tblBIAProceso.Add(procBIA);
                                                                db.SaveChanges();
                                                            }
                                                            IdProceso = procBIA.IdProceso;
                                                        }
                                                        break;
                                                    case 3:
                                                        _UnidadSolicitante = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _ProcesoActiva = _textoCelda;
                                                        break;
                                                    case 5:
                                                        _NombreResponsableActiva = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAEntrada Entrada = new tblBIAEntrada
                                        {
                                            Evento = _ProcesoActiva,
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Responsable = _NombreResponsableActiva,
                                            Unidad = _UnidadSolicitante,
                                        };

                                        db.tblBIAEntrada.Add(Entrada);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                _startRow = 0;
                            } // End if (_tableRow.HasChildren)
                        } // End foreach (Table)
                    } // End using wordDocument
                    db.SaveChanges();
                } // End using (Entities db = new Entities())
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static int GetNextRow(List<OpenXmlElement> _tableRows, int _startRow, out int _NroProceso)
        {
            bool findCell = false;
            _NroProceso = 0;

            while (!findCell)
            {
                TableRow _tableRow = (TableRow)_tableRows[_startRow];

                if (_tableRow.HasChildren)
                {
                    List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                    TableCell _celda = (TableCell)_tableCells[0];
                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                    string _textoCelda = string.Empty;
                    foreach (var _cellParagraph in _tableCellParagraph)
                    {
                        _textoCelda += _cellParagraph.InnerText;
                    } // End foreach (var _cellParagraph in _tableCellParagraph)

                    try
                    {
                        _NroProceso = (int.Parse(_textoCelda));
                        findCell = true;
                    }
                    catch
                    {
                        _startRow += 1;
                        if (_startRow >= _tableRows.Count())
                            break;
                    }
                }
            }

            return _startRow;
        }
        //private static void ProcessFichaBIA(MemoryStream msContent)
        //{
        //    long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
        //    long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
        //    int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

        //    string[] Entrevistados = new string[] { };
        //    string[] EmailEntrevistados = new string[] { };
        //    string[] CargosEntrevistados = new string[] { };
        //    string[] TelefonosEntrevistados = new string[] { };
        //    string[] Entrevistadores = new string[] { };
        //    string InicioEntrevista = string.Empty;
        //    string FinalEntrevista = string.Empty;
        //    string NombreRespaldo = string.Empty;
        //    string CorreoRespaldo = string.Empty;
        //    string UnidadOrganizativa = string.Empty;
        //    string Ubicacion = string.Empty;


        //    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
        //    {
        //        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
        //        {
        //            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
        //            if (_Table.HasChildren)
        //            {
        //                foreach (var _tableChild in _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList())
        //                {
        //                    TableRow _tableRow = (TableRow)_tableChild;
        //                    if (_tableRow.HasChildren)
        //                    {
        //                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
        //                        TableCell _tableCell = (TableCell)_tableCells.FirstOrDefault();
        //                        if (_tableCell.HasChildren)
        //                        {
        //                            string _textoCelda = string.Empty;

        //                            List<OpenXmlElement> _TableCellProperties = _tableCell.ChildElements.Where(x => x.GetType().Name == "TableCellProperties").ToList();
        //                            List<OpenXmlElement> _tableCellParagraph = _tableCell.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();

        //                            foreach (var _cellParagraph in _tableCellParagraph)
        //                            {
        //                                if (!string.IsNullOrEmpty(_textoCelda))
        //                                    _textoCelda += ", ";
        //                                _textoCelda += _cellParagraph.InnerText;
        //                            } // End foreach (var _cellParagraph in _tableCellParagraph)
        //                            if (!string.IsNullOrEmpty(_textoCelda))
        //                            {
        //                                TableCell _tableCellContent = (TableCell)_tableCells.LastOrDefault();
        //                                List<OpenXmlElement> _ContentProperties = _tableCellContent.ChildElements.Where(x => x.GetType().Name == "TableCellProperties").ToList();
        //                                List<OpenXmlElement> _ContentParagraph = _tableCellContent.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
        //                                string _contentTextoCelda = string.Empty;
        //                                foreach (var _cellParagraph in _ContentParagraph)
        //                                {
        //                                    if (!string.IsNullOrEmpty(_contentTextoCelda))
        //                                        _contentTextoCelda += ", ";
        //                                    _contentTextoCelda += _cellParagraph.InnerText;
        //                                } // End foreach (var _cellParagraph in _ContentParagraph)

        //                                string[] _datos = _textoCelda.Split(':');

        //                                if (_textoCelda.ToLower().Contains("nombre:"))
        //                                {
        //                                    Entrevistados = _datos[1].Trim().Split('/');
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("unidad"))
        //                                {
        //                                    UnidadOrganizativa = _datos[1].Trim();
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("ubicación"))
        //                                {
        //                                    Ubicacion = _datos[1].Trim();
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("e-mail"))
        //                                {
        //                                    EmailEntrevistados = _datos[1].Trim().Split('/');
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("cargo"))
        //                                {
        //                                    CargosEntrevistados = _datos[1].Trim().Split('/');
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("teléfono"))
        //                                {
        //                                    TelefonosEntrevistados = _datos[1].Trim().Split('/');
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("por"))
        //                                {
        //                                    Entrevistadores = _datos[1].Trim().Split('\n');
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("respaldo"))
        //                                {
        //                                    if (_datos[1].Trim().Split('/').Count() > 0)
        //                                    {
        //                                        string[] _split = _datos[1].Trim().Split('/');
        //                                        if (_split.Count() > 1)
        //                                            CorreoRespaldo = _split[1];
        //                                        NombreRespaldo = _split[0];
        //                                    }
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("fecha"))
        //                                {
        //                                    InicioEntrevista = _textoCelda.Trim();
        //                                    FinalEntrevista = _textoCelda.Trim();
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("inicio"))
        //                                {
        //                                    string[] _splitTime = _textoCelda.Split(':');
        //                                    int Hora = int.Parse(_splitTime[0]);
        //                                    if (_splitTime[2].Contains("pm")) Hora += 12;
        //                                    string Minuto = _splitTime[1].Split(' ')[0];
        //                                    InicioEntrevista += string.Format(" {0}:{1}", Hora.ToString("00"), Minuto);
        //                                }
        //                                else if (_textoCelda.ToLower().Contains("final"))
        //                                {
        //                                    string[] _splitTime = _textoCelda.Split(':');
        //                                    int Hora = int.Parse(_splitTime[0]);
        //                                    if (_splitTime[2].Contains("pm")) Hora += 12;
        //                                    string Minuto = _splitTime[1].Split(' ')[0];
        //                                    FinalEntrevista += string.Format(" {0}:{1}", Hora.ToString("00"), Minuto);
        //                                }
        //                            } // End if (!string.IsNullOrEmpty(_textoCelda))
        //                        } // End if (_tableCell.HasChildren)
        //                    } // End if (_tableRow.HasChildren)
        //                } // End foreach (TableRow)
        //            } // End if (_Table.HasChildren)
        //        } // End foreach (Table)
        //    } // End using wordDocument

        //    using (Entities db = new Entities())
        //    {
        //        try
        //        {
        //            string[] _Unidades = UnidadOrganizativa.Split('/');
        //            long _IdUnidadPadre = 0;
        //            long _IdUnidadOrganizativa = 0;
        //            long _IdPersonaResponsable = 0;

        //            foreach (string _nombreUnidad in _Unidades)
        //            {
        //                tblUnidadOrganizativa Unidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == _IdEmpresa
        //                                                                                && x.Nombre == _nombreUnidad.Trim()).FirstOrDefault();
        //                if (Unidad == null)
        //                {
        //                    Unidad = new tblUnidadOrganizativa()
        //                    {
        //                        IdEmpresa = _IdEmpresa,
        //                        IdUnidadPadre = _IdUnidadPadre,
        //                        Nombre = _nombreUnidad
        //                    };
        //                    db.tblUnidadOrganizativa.Add(Unidad);
        //                    db.SaveChanges();

        //                    if (_nombreUnidad != _Unidades.Last())
        //                        _IdUnidadPadre = Unidad.IdUnidadOrganizativa;
        //                }
        //                _IdUnidadOrganizativa = Unidad.IdUnidadOrganizativa;
        //            }

        //            List<long> IdCargos = new List<long>();

        //            foreach (string _Cargo in CargosEntrevistados)
        //            {
        //                tblCargo tblCargo = db.tblCargo.Where(x => x.IdEmpresa == _IdEmpresa
        //                                                        && x.Descripcion == _Cargo).FirstOrDefault();
        //                if (tblCargo == null)
        //                {
        //                    tblCargo = new tblCargo
        //                    {
        //                        Descripcion = _Cargo,
        //                        IdEmpresa = _IdEmpresa
        //                    };
        //                    db.tblCargo.Add(tblCargo);
        //                    db.SaveChanges();
        //                }
        //                IdCargos.Add(tblCargo.IdCargo);
        //            }
        //            int nroReg = -1;

        //            foreach (string _Entrevistado in Entrevistados)
        //            {
        //                nroReg++;
        //                tblPersona Persona = db.tblPersona.Where(x => x.IdEmpresa == 14
        //                                                           && x.Nombre == _Entrevistado).FirstOrDefault();
        //                if (Persona == null)
        //                {
        //                    long _IdUsuario = 0;
        //                    if (EmailEntrevistados.Count() > nroReg)
        //                    {
        //                        tblUsuario usuario = db.tblUsuario.Where(x => x.Email == EmailEntrevistados[nroReg]).FirstOrDefault();
        //                        if (usuario == null)
        //                        {
        //                            string Contraseña = Membership.GeneratePassword(8, 1);
        //                            string _encriptedPassword = _Encriptar.Encriptar(Contraseña, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

        //                            usuario = new tblUsuario
        //                            {
        //                                CodigoUsuario = EmailEntrevistados[nroReg],
        //                                ClaveUsuario = _encriptedPassword,
        //                                Email = EmailEntrevistados[nroReg],
        //                                EstadoUsuario = (short)eEstadoUsuario.Activo,
        //                                FechaEstado = DateTime.UtcNow,
        //                                FechaUltimaConexion = null,
        //                                Nombre = _Entrevistado,
        //                                PrimeraVez = true,
        //                            };
        //                            db.tblUsuario.Add(usuario);
        //                        }

        //                        _IdUsuario = usuario.IdUsuario;
        //                    }

        //                    Persona = new tblPersona
        //                    {
        //                        IdCargo = IdCargos[nroReg],
        //                        IdEmpresa = _IdEmpresa,
        //                        Identificacion = string.Empty,
        //                        IdUnidadOrganizativa = _IdUnidadOrganizativa,
        //                        IdUsuario = _IdUsuario,
        //                        Nombre = _Entrevistado,
        //                    };
        //                    db.tblPersona.Add(Persona);

        //                    if (EmailEntrevistados.Count() > nroReg)
        //                    {
        //                        tblPersonaCorreo PersonaEmail = db.tblPersonaCorreo.Where(x => x.IdEmpresa == _IdEmpresa
        //                                                                                            && x.IdTipoCorreo == (long)eTipoEmail.Corporativo
        //                                                                                            && x.IdPersona == Persona.IdPersona
        //                                                                                            && x.Correo == EmailEntrevistados[nroReg]).FirstOrDefault();

        //                        if (PersonaEmail == null)
        //                        {
        //                            PersonaEmail = new tblPersonaCorreo
        //                            {
        //                                Correo = EmailEntrevistados[nroReg],
        //                                IdEmpresa = _IdEmpresa,
        //                                IdPersona = Persona.IdPersona,
        //                                IdTipoCorreo = (long)eTipoEmail.Corporativo,
        //                            };
        //                            db.tblPersonaCorreo.Add(PersonaEmail);
        //                        }
        //                    }

        //                    if (TelefonosEntrevistados.Count() > nroReg)
        //                    {
        //                        tblPersonaTelefono PersonaTelefono = db.tblPersonaTelefono.Where(x => x.IdEmpresa == _IdEmpresa
        //                                                                                                    && x.IdTipoTelefono == (long)eTipoTelefono.Corporativo
        //                                                                                                    && x.IdPersona == Persona.IdPersona
        //                                                                                                    && x.Telefono == TelefonosEntrevistados[nroReg]).FirstOrDefault();

        //                        if (PersonaTelefono == null)
        //                        {
        //                            PersonaTelefono = new tblPersonaTelefono
        //                            {
        //                                IdEmpresa = _IdEmpresa,
        //                                IdPersona = Persona.IdPersona,
        //                                IdTipoTelefono = (long)eTipoTelefono.Corporativo,
        //                                Telefono = TelefonosEntrevistados[nroReg]
        //                            };
        //                            db.tblPersonaTelefono.Add(PersonaTelefono);
        //                        }
        //                    }
        //                    db.SaveChanges();
        //                    if (_IdPersonaResponsable == 0) _IdPersonaResponsable = Persona.IdPersona;
        //                 }
        //            }

        //            foreach (string _Entrevistador in Entrevistadores)
        //            {

        //            }

        //            tblBIADocumento BIADocumento = new tblBIADocumento
        //            {
        //                IdCadenaServicio = 0,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdTipoDocumento = _IdTipoDocumento,
        //                IdUnidadOrganizativa = _IdUnidadOrganizativa
        //            };

        //            tblDocumento Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
        //                                                            && x.IdTipoDocumento == _IdTipoDocumento
        //                                                            && x.IdDocumento == _IdDocumento).FirstOrDefault();

        //            Documento.IdPersonaResponsable = _IdPersonaResponsable;

        //            db.tblDocumentoAprobacion.RemoveRange(Documento.tblDocumentoAprobacion);
        //            db.tblDocumentoCertificacion.RemoveRange(Documento.tblDocumentoCertificacion);
        //            if (Documento.tblDocumentoEntrevista != null)
        //            {
        //                tblDocumentoEntrevista Entrevista = Documento.tblDocumentoEntrevista.FirstOrDefault();
        //                if (Entrevista != null)
        //                {
        //                    if (Entrevista.tblDocumentoEntrevistaPersona != null)
        //                        db.tblDocumentoEntrevistaPersona.RemoveRange(Entrevista.tblDocumentoEntrevistaPersona);
        //                    db.tblDocumentoEntrevista.Remove(Entrevista);
        //                }
        //            }
        //            db.tblDocumentoPersonaClave.RemoveRange(Documento.tblDocumentoPersonaClave);


        //            tblDocumentoAprobacion _Aprobacion = new tblDocumentoAprobacion
        //            {
        //                Aprobado = null,
        //                Fecha = null,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdPersona = _IdPersonaResponsable,
        //                IdTipoDocumento = _IdTipoDocumento,
        //                Procesado = false,
        //            };
        //            db.tblDocumentoAprobacion.Add(_Aprobacion);
        //            tblDocumentoCertificacion _Certificacion = new tblDocumentoCertificacion
        //            {
        //                Certificado = null,
        //                Fecha = null,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdPersona = _IdPersonaCertificador,
        //                IdTipoDocumento = _IdTipoDocumento,
        //                Procesado = false
        //            };
        //            string[] _fechaInicioArray = FechaInicioEntrevista.Split(' ');
        //            string[] _fechaFinalArray = FechaFinalEntrevista.Split(' ');

        //            string[] _HoraInicioStringArray = _fechaInicioArray[1].Split(':');
        //            string[] _HoraFinalStringArray = _fechaFinalArray[1].Split(':');

        //            if (_HoraInicioStringArray[0].Length == 1) _HoraInicioStringArray[0] = string.Format("0{0}", _HoraInicioStringArray[0]);
        //            if (_fechaInicioArray[2] == "pm") _HoraInicioStringArray[0] = (int.Parse(_HoraInicioStringArray[0]) + 12).ToString();

        //            if (_HoraFinalStringArray[0].Length == 1) _HoraFinalStringArray[0] = string.Format("0{0}", _HoraFinalStringArray[0]);
        //            if (_fechaFinalArray[2] == "pm") _HoraFinalStringArray[0] = (int.Parse(_HoraFinalStringArray[0]) + 12).ToString();

        //            FechaInicioEntrevista = _fechaInicioArray[0] + " " + _HoraInicioStringArray[0] + ":" + _HoraInicioStringArray[1];
        //            FechaFinalEntrevista = _fechaFinalArray[0] + " " + _HoraFinalStringArray[0] + ":" + _HoraFinalStringArray[1];

        //            DateTime _FechaInicio = DateTime.ParseExact(FechaInicioEntrevista, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        //            DateTime _FechaFinal = DateTime.ParseExact(FechaFinalEntrevista, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

        //            tblDocumentoEntrevista newEntrevista = new tblDocumentoEntrevista
        //            {
        //                FechaFinal = _FechaFinal,
        //                FechaInicio = _FechaInicio,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdTipoDocumento = _IdTipoDocumento,
        //            };

        //            db.tblDocumentoEntrevista.Add(newEntrevista);

        //            tblDocumentoEntrevistaPersona Entrevistador = new tblDocumentoEntrevistaPersona
        //            {
        //                EsEntrevistador = true,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdEntrevista = newEntrevista.IdEntrevista,
        //                IdTipoDocumento = _IdTipoDocumento,
        //                Empresa = "Grupo Aplired",
        //                Nombre = NombreResponsableEntrevistador
        //            };

        //            tblDocumentoEntrevistaPersona Entrevistado = new tblDocumentoEntrevistaPersona
        //            {
        //                EsEntrevistador = true,
        //                IdDocumento = _IdDocumento,
        //                IdEmpresa = _IdEmpresa,
        //                IdEntrevista = newEntrevista.IdEntrevista,
        //                IdTipoDocumento = _IdTipoDocumento,
        //                Empresa = _Empresa.NombreComercial,
        //                Nombre = NombreResponsable
        //            };

        //            db.tblDocumentoEntrevistaPersona.Add(Entrevistador);
        //            db.tblDocumentoEntrevistaPersona.Add(Entrevistado);

        //            db.SaveChanges();
        //        } // End Try
        //        catch
        //        {

        //        } // End catch
        //    } // End using (Entities db = new Entities())

        //}
        private static void ProcessFichaEstandar(MemoryStream msContent)
        {
            eSeccionFicha _Seccion = eSeccionFicha.SinDefinir;
            List<int> _ColumnsData = new List<int>();
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            string NombreResponsable = string.Empty;
            string EmailResponsable = string.Empty;
            string EmailResponsableEntrevistador = string.Empty;
            string EmailCertificador = string.Empty;
            string DireccionResponsable = string.Empty;
            string CargoResponsable = string.Empty;
            string CargoCertificador = string.Empty;
            string TelefonoResponsable = string.Empty;
            string NombreResponsableEntrevistador = string.Empty;
            string FechaInicioEntrevista = string.Empty;
            string FechaFinalEntrevista = string.Empty;
            string NombreCertificador = string.Empty;
            string NombreUnidadOrganizativa = string.Empty;

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                {
                    foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                        if (_Table.HasChildren)
                        {
                            foreach (var _tableChild in _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList())
                            {
                                TableRow _tableRow = (TableRow)_tableChild;
                                if (_tableRow.HasChildren)
                                {
                                    List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                    TableCell _tableCell = (TableCell)_tableCells.FirstOrDefault();
                                    if (_tableCell.HasChildren)
                                    {
                                        string _textoCelda = string.Empty;

                                        List<OpenXmlElement> _TableCellProperties = _tableCell.ChildElements.Where(x => x.GetType().Name == "TableCellProperties").ToList();
                                        List<OpenXmlElement> _tableCellParagraph = _tableCell.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();

                                        foreach (var _cellParagraph in _tableCellParagraph)
                                        {
                                            if (!string.IsNullOrEmpty(_textoCelda))
                                                _textoCelda += ", ";
                                            _textoCelda += _cellParagraph.InnerText;
                                        } // End foreach (var _cellParagraph in _tableCellParagraph)
                                        if (!string.IsNullOrEmpty(_textoCelda))
                                        {
                                            TableCell _tableCellContent = (TableCell)_tableCells.LastOrDefault();
                                            List<OpenXmlElement> _ContentProperties = _tableCellContent.ChildElements.Where(x => x.GetType().Name == "TableCellProperties").ToList();
                                            List<OpenXmlElement> _ContentParagraph = _tableCellContent.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                            string _contentTextoCelda = string.Empty;
                                            foreach (var _cellParagraph in _ContentParagraph)
                                            {
                                                if (!string.IsNullOrEmpty(_contentTextoCelda))
                                                    _contentTextoCelda += ", ";
                                                _contentTextoCelda += _cellParagraph.InnerText;
                                            } // End foreach (var _cellParagraph in _ContentParagraph)

                                            switch (_textoCelda)
                                            {
                                                case "INFORMACIÓN GENERAL":
                                                    _Seccion = eSeccionFicha.InformacionGeneral;
                                                    break;
                                                case "EMPRESA Y UNIDAD (ES) RESPONSABLE (S)":
                                                    _Seccion = eSeccionFicha.Empresas_Unidades;
                                                    break;
                                                case "RESPONSABLE POR GRUPO APLIRED, C.A.":
                                                    _Seccion = eSeccionFicha.Responsable;
                                                    break;
                                                case "CERTIFICADOR DEL DOCUMENTO":
                                                    _Seccion = eSeccionFicha.Certificador;
                                                    break;
                                                case "APROBADOR DEL DOCUMENTO":
                                                    break;
                                                case "Unidad Organizativa":
                                                    NombreUnidadOrganizativa = _contentTextoCelda;
                                                    break;
                                                case "Ubicación":
                                                    if (_Seccion == eSeccionFicha.Empresas_Unidades)
                                                    {
                                                        DireccionResponsable = _contentTextoCelda;
                                                    }
                                                    break;
                                                case "Nombre del Responsable":
                                                    NombreResponsable = _contentTextoCelda;
                                                    break;
                                                case "Correo Electrónico":
                                                    string[] datosEmail = _contentTextoCelda.Split('\"');
                                                    string Email = string.Empty;
                                                    if (datosEmail.Count() > 1)
                                                        Email = datosEmail[2];
                                                    else
                                                        Email = _contentTextoCelda;

                                                    switch (_Seccion)
                                                    {
                                                        case eSeccionFicha.Empresas_Unidades:
                                                            EmailResponsable = Email;
                                                            break;
                                                        case eSeccionFicha.Responsable:
                                                            EmailResponsableEntrevistador = Email;
                                                            break;
                                                        case eSeccionFicha.Certificador:
                                                            EmailCertificador = Email;
                                                            break;
                                                    }
                                                    break;
                                                case "Cargo":
                                                    switch (_Seccion)
                                                    {
                                                        case eSeccionFicha.Empresas_Unidades:
                                                            CargoResponsable = _contentTextoCelda;
                                                            break;
                                                        case eSeccionFicha.Certificador:
                                                            CargoCertificador = _contentTextoCelda;
                                                            break;
                                                    }
                                                    break;
                                                case "Teléfono":
                                                    if (_Seccion == eSeccionFicha.Empresas_Unidades)
                                                    {
                                                        TelefonoResponsable = _contentTextoCelda;
                                                    }
                                                    break;
                                                case "Nombre y Apellido":
                                                    switch (_Seccion)
                                                    {
                                                        case eSeccionFicha.Responsable:
                                                            NombreResponsableEntrevistador = _contentTextoCelda;
                                                            break;
                                                        case eSeccionFicha.Certificador:
                                                            NombreCertificador = _contentTextoCelda;
                                                            break;
                                                    }
                                                    break;
                                                case "Fecha y Hora de Inicio":
                                                    FechaInicioEntrevista = _contentTextoCelda.Replace(".", "");
                                                    break;
                                                case "Fecha y Hora Final":
                                                    FechaFinalEntrevista = _contentTextoCelda.Replace(".", "");
                                                    break;
                                            } // End switch (_textoCelda.ToUpper())
                                        } // End if (!string.IsNullOrEmpty(_textoCelda))
                                    } // End if (_tableCell.HasChildren)
                                } // End if (_tableRow.HasChildren)
                            } // End foreach (TableRow)
                        } // End if (_Table.HasChildren)
                    } // End foreach (Table)
                } // End using wordDocument

                long _IdUnidadOrganizativa = 0;
                long _IdCargoResponsable = 0;
                long _IdCargoCertificador = 0;
                long _IdPersonaResponsable = 0;
                long _IdPersonaCertificador = 0;
                long _IdUsuarioResponsable = 0;
                long _IdUsuarioCertificador = 0;
                long _IdUsuarioEntrevistador = 0;
                long _IdPersonaEntrevistador = 0;

            try
            {
                using (Entities db = new Entities())
                {
                    string[] _Unidades = NombreUnidadOrganizativa.Split('–');
                    long _IdUnidadPadre = 0;

                    tblEmpresa _Empresa = db.tblEmpresa.Where(x => x.IdEmpresa == _IdEmpresa).FirstOrDefault();

                    foreach (string _nombreUnidad in _Unidades)
                    {
                        tblUnidadOrganizativa Unidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                        && x.Nombre == _nombreUnidad).FirstOrDefault();
                        if (Unidad == null)
                        {
                            Unidad = new tblUnidadOrganizativa()
                            {
                                IdEmpresa = _IdEmpresa,
                                IdUnidadPadre = _IdUnidadPadre,
                                Nombre = _nombreUnidad
                            };
                            db.tblUnidadOrganizativa.Add(Unidad);
                            db.SaveChanges();

                            if (_nombreUnidad != _Unidades.Last())
                                _IdUnidadPadre = Unidad.IdUnidadOrganizativa;
                        }
                        _IdUnidadOrganizativa = Unidad.IdUnidadOrganizativa;
                    }

                    tblCargo tblCargoResponsable = db.tblCargo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                        && x.Descripcion == CargoResponsable).FirstOrDefault();
                    if (tblCargoResponsable == null)
                    {
                        tblCargoResponsable = new tblCargo
                        {
                            Descripcion = CargoResponsable,
                            IdEmpresa = _IdEmpresa
                        };
                        db.tblCargo.Add(tblCargoResponsable);
                        db.SaveChanges();
                    }
                    _IdCargoResponsable = tblCargoResponsable.IdCargo;

                    tblCargo tblCargoCertificador = db.tblCargo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                        && x.Descripcion == CargoCertificador).FirstOrDefault();
                    if (tblCargoCertificador == null)
                    {
                        tblCargoCertificador = new tblCargo
                        {
                            Descripcion = CargoCertificador,
                            IdEmpresa = _IdEmpresa
                        };
                        db.tblCargo.Add(tblCargoCertificador);
                        db.SaveChanges();
                    }
                    _IdCargoCertificador = tblCargoCertificador.IdCargo;

                    tblPersona PersonaResponsable = db.tblPersona.Where(x => x.IdEmpresa == _IdEmpresa
                                                                        && x.Nombre == NombreResponsable).FirstOrDefault();
                    if (PersonaResponsable == null)
                    {

                        tblUsuario usuarioResponsble = db.tblUsuario.Where(x => x.Email == EmailResponsable).FirstOrDefault();
                        if (usuarioResponsble == null)
                        {
                            string Contraseña = Membership.GeneratePassword(8, 1);
                            string _encriptedPassword = _Encriptar.Encriptar(Contraseña, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

                            usuarioResponsble = new tblUsuario
                            {
                                CodigoUsuario = EmailResponsable,
                                ClaveUsuario = _encriptedPassword,
                                Email = EmailResponsable,
                                EstadoUsuario = (short)eEstadoUsuario.Activo,
                                FechaEstado = DateTime.UtcNow,
                                FechaUltimaConexion = null,
                                Nombre = NombreResponsable,
                                PrimeraVez = true,
                            };
                            db.tblUsuario.Add(usuarioResponsble);
                        }

                        _IdUsuarioResponsable = usuarioResponsble.IdUsuario;

                        PersonaResponsable = db.tblPersona.Where(x => x.IdEmpresa == 14
                                                                    && x.Nombre == NombreResponsable).FirstOrDefault();

                        if (PersonaResponsable == null)
                        {
                            PersonaResponsable = new tblPersona
                            {
                                IdCargo = _IdCargoResponsable,
                                IdEmpresa = _IdEmpresa,
                                Identificacion = string.Empty,
                                IdUnidadOrganizativa = _IdUnidadOrganizativa,
                                IdUsuario = usuarioResponsble.IdUsuario,
                                Nombre = NombreResponsable,
                            };
                            db.tblPersona.Add(PersonaResponsable);
                        }

                        tblPersonaDireccion PersonaResponsableDireccion = db.tblPersonaDireccion.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                        && x.IdTipoDireccion == (long)eTipoDireccion.Oficina
                                                                                                        && x.IdPersona == PersonaResponsable.IdPersona
                                                                                                        && x.Ubicacion == DireccionResponsable).FirstOrDefault();
                        if (PersonaResponsableDireccion == null)
                        {
                            PersonaResponsableDireccion = new tblPersonaDireccion
                            {
                                IdCiudad = (long)_Empresa.IdPaisEstadoCiudad,
                                IdEmpresa = _IdEmpresa,
                                IdEstado = (long)_Empresa.IdPaisEstado,
                                IdPais = (long)_Empresa.IdPais,
                                IdPersona = PersonaResponsable.IdPersona,
                                IdTipoDireccion = (long)eTipoDireccion.Oficina,
                                Ubicacion = DireccionResponsable
                            };
                            db.tblPersonaDireccion.Add(PersonaResponsableDireccion);
                        }

                        tblPersonaCorreo PersonaResponsableEmail = db.tblPersonaCorreo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdTipoCorreo == (long)eTipoEmail.Corporativo
                                                                                                && x.IdPersona == PersonaResponsable.IdPersona
                                                                                                && x.Correo == EmailResponsable).FirstOrDefault();

                        if (PersonaResponsableEmail == null)
                        {
                            PersonaResponsableEmail = new tblPersonaCorreo
                            {
                                Correo = EmailResponsable,
                                IdEmpresa = _IdEmpresa,
                                IdPersona = PersonaResponsable.IdPersona,
                                IdTipoCorreo = (long)eTipoEmail.Corporativo,
                            };
                            db.tblPersonaCorreo.Add(PersonaResponsableEmail);
                        }

                        tblPersonaTelefono PersonaResponsableTelefono = db.tblPersonaTelefono.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                    && x.IdTipoTelefono == (long)eTipoTelefono.Corporativo
                                                                                                    && x.IdPersona == PersonaResponsable.IdPersona
                                                                                                    && x.Telefono == TelefonoResponsable).FirstOrDefault();

                        if (PersonaResponsableTelefono == null)
                        {
                            PersonaResponsableTelefono = new tblPersonaTelefono
                            {
                                IdEmpresa = _IdEmpresa,
                                IdPersona = PersonaResponsable.IdPersona,
                                IdTipoTelefono = (long)eTipoTelefono.Corporativo,
                                Telefono = TelefonoResponsable
                            };
                            db.tblPersonaTelefono.Add(PersonaResponsableTelefono);
                        }
                        db.SaveChanges();
                    }

                    _IdPersonaResponsable = PersonaResponsable.IdPersona;
                    _IdUsuarioResponsable = (long)PersonaResponsable.IdUsuario;

                    if (NombreResponsable != NombreCertificador)
                    {
                        tblPersona PersonaCertificador = db.tblPersona.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                && x.Nombre == NombreCertificador).FirstOrDefault();
                        if (PersonaCertificador == null)
                        {
                            tblUsuario UsuarioCertificador = db.tblUsuario.Where(x => x.Email == EmailCertificador).FirstOrDefault();

                            if (UsuarioCertificador == null)
                            {
                                string Contraseña = Membership.GeneratePassword(8, 8);
                                string _encriptedPassword = _Encriptar.Encriptar(Contraseña, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

                                UsuarioCertificador = new tblUsuario
                                {
                                    CodigoUsuario = EmailCertificador,
                                    ClaveUsuario = _encriptedPassword,
                                    Email = EmailCertificador,
                                    EstadoUsuario = (short)eEstadoUsuario.Activo,
                                    FechaEstado = DateTime.UtcNow,
                                    FechaUltimaConexion = null,
                                    Nombre = NombreCertificador,
                                    PrimeraVez = true,
                                };
                                db.tblUsuario.Add(UsuarioCertificador);
                            }

                            _IdUsuarioCertificador = UsuarioCertificador.IdUsuario;

                            PersonaCertificador = new tblPersona
                            {
                                IdCargo = _IdCargoCertificador,
                                IdEmpresa = _IdEmpresa,
                                Identificacion = string.Empty,
                                IdUnidadOrganizativa = _IdUnidadOrganizativa,
                                IdUsuario = UsuarioCertificador.IdUsuario,
                                Nombre = NombreCertificador,
                            };
                            db.tblPersona.Add(PersonaCertificador);

                            tblPersonaCorreo PersonaCertificadorEmail = db.tblPersonaCorreo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                    && x.IdTipoCorreo == (long)eTipoEmail.Corporativo
                                                                                                    && x.IdPersona == PersonaCertificador.IdPersona
                                                                                                    && x.Correo == EmailCertificador).FirstOrDefault();

                            if (PersonaCertificadorEmail == null)
                            {
                                PersonaCertificadorEmail = new tblPersonaCorreo
                                {
                                    Correo = EmailCertificador,
                                    IdEmpresa = _IdEmpresa,
                                    IdPersona = PersonaCertificador.IdPersona,
                                    IdTipoCorreo = (long)eTipoEmail.Corporativo,
                                };
                                db.tblPersonaCorreo.Add(PersonaCertificadorEmail);
                            }
                            db.SaveChanges();

                        }

                        _IdPersonaCertificador = PersonaCertificador.IdPersona;
                        _IdUsuarioCertificador = (long)PersonaCertificador.IdUsuario;

                        tblEmpresaUsuario _EmpresaUsuarioResponsable = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                && x.IdUsuario == _IdUsuarioResponsable).FirstOrDefault();

                        List<long> IdModulos = db.tblModulo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdCodigoModulo == _IdTipoDocumento)
                                                            .Select(x => x.IdModulo).ToList();

                        if (_EmpresaUsuarioResponsable == null)
                        {
                            _EmpresaUsuarioResponsable = new tblEmpresaUsuario
                            {
                                FechaCreacion = DateTime.UtcNow,
                                IdEmpresa = _IdEmpresa,
                                IdNivelUsuario = (long)eNivelUsuario.Aprobador,
                                IdUsuario = _IdUsuarioResponsable,
                            };
                            db.tblEmpresaUsuario.Add(_EmpresaUsuarioResponsable);

                            foreach (long IdModulo in IdModulos)
                            {
                                tblModulo_Usuario ModuloUsuario = db.tblModulo_Usuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdUsuario == _IdUsuarioResponsable
                                                                                                && x.IdModulo == IdModulo).FirstOrDefault();

                                if (ModuloUsuario == null)
                                {
                                    ModuloUsuario = new tblModulo_Usuario
                                    {
                                        Actualizar = true,
                                        Eliminar = true,
                                        IdEmpresa = _IdEmpresa,
                                        IdModulo = IdModulo,
                                        IdUsuario = _IdUsuarioResponsable
                                    };
                                    db.tblModulo_Usuario.Add(ModuloUsuario);
                                }
                            }
                        }
                        tblEmpresaUsuario _EmpresaUsuarioCertificador = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdUsuario == _IdUsuarioCertificador).FirstOrDefault();

                        if (_EmpresaUsuarioCertificador == null)
                        {
                            _EmpresaUsuarioCertificador = new tblEmpresaUsuario
                            {
                                FechaCreacion = DateTime.UtcNow,
                                IdEmpresa = _IdEmpresa,
                                IdNivelUsuario = (long)eNivelUsuario.Certificador,
                                IdUsuario = _IdUsuarioCertificador,
                            };
                            db.tblEmpresaUsuario.Add(_EmpresaUsuarioCertificador);

                            foreach (long IdModulo in IdModulos)
                            {
                                tblModulo_Usuario ModuloUsuario = db.tblModulo_Usuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdUsuario == _IdUsuarioCertificador
                                                                                                && x.IdModulo == IdModulo).FirstOrDefault();

                                if (ModuloUsuario == null)
                                {
                                    ModuloUsuario = new tblModulo_Usuario
                                    {
                                        Actualizar = true,
                                        Eliminar = true,
                                        IdEmpresa = _IdEmpresa,
                                        IdModulo = IdModulo,
                                        IdUsuario = _IdUsuarioCertificador
                                    };
                                    db.tblModulo_Usuario.Add(ModuloUsuario);
                                }
                            }
                        }

                    }
                    else
                    {
                        tblEmpresaUsuario _EmpresaUsuarioResponsable = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                && x.IdUsuario == _IdUsuarioResponsable).FirstOrDefault();

                        List<long> IdModulos = db.tblModulo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdCodigoModulo == _IdTipoDocumento)
                                                            .Select(x => x.IdModulo).ToList();

                        if (_EmpresaUsuarioResponsable == null)
                        {
                            _EmpresaUsuarioResponsable = new tblEmpresaUsuario
                            {
                                FechaCreacion = DateTime.UtcNow,
                                IdEmpresa = _IdEmpresa,
                                IdNivelUsuario = (long)eNivelUsuario.Certificador,
                                IdUsuario = _IdUsuarioResponsable,
                            };
                            db.tblEmpresaUsuario.Add(_EmpresaUsuarioResponsable);

                            foreach (long IdModulo in IdModulos)
                            {
                                tblModulo_Usuario ModuloUsuario = db.tblModulo_Usuario.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdUsuario == _IdUsuarioResponsable
                                                                                                && x.IdModulo == IdModulo).FirstOrDefault();

                                if (ModuloUsuario == null)
                                {
                                    ModuloUsuario = new tblModulo_Usuario
                                    {
                                        Actualizar = true,
                                        Eliminar = true,
                                        IdEmpresa = _IdEmpresa,
                                        IdModulo = IdModulo,
                                        IdUsuario = _IdUsuarioResponsable
                                    };
                                    db.tblModulo_Usuario.Add(ModuloUsuario);
                                }
                            }
                        }
                    }
                    db.SaveChanges();

                    tblPersona PersonaEntrevistador = db.tblPersona.Where(x => x.IdEmpresa == _IdEmpresa
                                                                            && x.Nombre == NombreResponsableEntrevistador).FirstOrDefault();
                    if (PersonaEntrevistador == null)
                    {

                        tblUsuario usuarioResponsble = db.tblUsuario.Where(x => x.Email == EmailResponsableEntrevistador).FirstOrDefault();
                        if (usuarioResponsble == null)
                        {
                            string Contraseña = Membership.GeneratePassword(8, 1);
                            string _encriptedPassword = _Encriptar.Encriptar(Contraseña, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

                            usuarioResponsble = new tblUsuario
                            {
                                CodigoUsuario = EmailResponsableEntrevistador,
                                ClaveUsuario = _encriptedPassword,
                                Email = EmailResponsableEntrevistador,
                                EstadoUsuario = (short)eEstadoUsuario.Activo,
                                FechaEstado = DateTime.UtcNow,
                                FechaUltimaConexion = null,
                                Nombre = NombreResponsableEntrevistador,
                                PrimeraVez = true,
                            };
                            db.tblUsuario.Add(usuarioResponsble);
                        }

                        _IdUsuarioEntrevistador = usuarioResponsble.IdUsuario;

                        PersonaEntrevistador = db.tblPersona.Where(x => x.IdEmpresa == 14
                                                                    && x.Nombre == NombreResponsableEntrevistador).FirstOrDefault();

                        if (PersonaEntrevistador == null)
                        {
                            PersonaEntrevistador = new tblPersona
                            {
                                IdCargo = 0,
                                IdEmpresa = _IdEmpresa,
                                Identificacion = string.Empty,
                                IdUnidadOrganizativa = _IdUnidadOrganizativa,
                                IdUsuario = usuarioResponsble.IdUsuario,
                                Nombre = NombreResponsableEntrevistador,
                            };
                            db.tblPersona.Add(PersonaEntrevistador);
                        }

                        tblPersonaCorreo PersonaEntrevistadorEmail = db.tblPersonaCorreo.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                && x.IdTipoCorreo == (long)eTipoEmail.Corporativo
                                                                                                && x.IdPersona == PersonaEntrevistador.IdPersona
                                                                                                && x.Correo == EmailResponsableEntrevistador).FirstOrDefault();

                        if (PersonaEntrevistadorEmail == null)
                        {
                            PersonaEntrevistadorEmail = new tblPersonaCorreo
                            {
                                Correo = EmailResponsableEntrevistador,
                                IdEmpresa = _IdEmpresa,
                                IdPersona = PersonaEntrevistador.IdPersona,
                                IdTipoCorreo = (long)eTipoEmail.Corporativo,
                            };
                            db.tblPersonaCorreo.Add(PersonaEntrevistadorEmail);
                        }
                        db.SaveChanges();

                    }

                    _IdPersonaEntrevistador = PersonaEntrevistador.IdPersona;
                    _IdUsuarioEntrevistador = (long)PersonaEntrevistador.IdUsuario;

                    tblDocumento Documento = db.tblDocumento.Where(x => x.IdEmpresa == _IdEmpresa
                                                                    && x.IdTipoDocumento == _IdTipoDocumento
                                                                    && x.IdDocumento == _IdDocumento).FirstOrDefault();

                    Documento.IdPersonaResponsable = _IdPersonaResponsable;

                    db.tblDocumentoAprobacion.RemoveRange(Documento.tblDocumentoAprobacion);
                    db.tblDocumentoCertificacion.RemoveRange(Documento.tblDocumentoCertificacion);
                    if (Documento.tblDocumentoEntrevista != null)
                    {
                        tblDocumentoEntrevista Entrevista = Documento.tblDocumentoEntrevista.FirstOrDefault();
                        if (Entrevista != null)
                        {
                            if (Entrevista.tblDocumentoEntrevistaPersona != null)
                                db.tblDocumentoEntrevistaPersona.RemoveRange(Entrevista.tblDocumentoEntrevistaPersona);
                            db.tblDocumentoEntrevista.Remove(Entrevista);
                        }
                    }
                    db.tblDocumentoPersonaClave.RemoveRange(Documento.tblDocumentoPersonaClave);
                    db.SaveChanges();

                    tblDocumentoAprobacion _Aprobacion = new tblDocumentoAprobacion
                    {
                        Aprobado = null,
                        Fecha = null,
                        IdDocumento = _IdDocumento,
                        IdEmpresa = _IdEmpresa,
                        IdPersona = _IdPersonaResponsable,
                        IdTipoDocumento = _IdTipoDocumento,
                        Procesado = false,
                    };
                    db.tblDocumentoAprobacion.Add(_Aprobacion);
                    tblDocumentoCertificacion _Certificacion = new tblDocumentoCertificacion
                    {
                        Certificado = null,
                        Fecha = null,
                        IdDocumento = _IdDocumento,
                        IdEmpresa = _IdEmpresa,
                        IdPersona = _IdPersonaCertificador,
                        IdTipoDocumento = _IdTipoDocumento,
                        Procesado = false
                    };
                    string[] _fechaInicioArray = FechaInicioEntrevista.Split(' ');
                    string[] _fechaFinalArray = FechaFinalEntrevista.Split(' ');

                    string[] _HoraInicioStringArray = _fechaInicioArray[1].Split(':');
                    string[] _HoraFinalStringArray = _fechaFinalArray[1].Split(':');

                    if (_HoraInicioStringArray[0].Length == 1) _HoraInicioStringArray[0] = string.Format("0{0}", _HoraInicioStringArray[0]);
                    if (_fechaInicioArray[2] == "pm") _HoraInicioStringArray[0] = (int.Parse(_HoraInicioStringArray[0]) + 12).ToString();

                    if (_HoraFinalStringArray[0].Length == 1) _HoraFinalStringArray[0] = string.Format("0{0}", _HoraFinalStringArray[0]);
                    if (_fechaFinalArray[2] == "pm") _HoraFinalStringArray[0] = (int.Parse(_HoraFinalStringArray[0]) + 12).ToString();

                    FechaInicioEntrevista = _fechaInicioArray[0] + " " + _HoraInicioStringArray[0] + ":" + _HoraInicioStringArray[1];
                    FechaFinalEntrevista = _fechaFinalArray[0] + " " + _HoraFinalStringArray[0] + ":" + _HoraFinalStringArray[1];

                    DateTime _FechaInicio = DateTime.ParseExact(FechaInicioEntrevista, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime _FechaFinal = DateTime.ParseExact(FechaFinalEntrevista, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                    tblDocumentoEntrevista newEntrevista = new tblDocumentoEntrevista
                    {
                        FechaFinal = _FechaFinal,
                        FechaInicio = _FechaInicio,
                        IdDocumento = _IdDocumento,
                        IdEmpresa = _IdEmpresa,
                        IdTipoDocumento = _IdTipoDocumento,
                    };

                    db.tblDocumentoEntrevista.Add(newEntrevista);

                    tblDocumentoEntrevistaPersona Entrevistador = new tblDocumentoEntrevistaPersona
                    {
                        EsEntrevistador = true,
                        IdDocumento = _IdDocumento,
                        IdEmpresa = _IdEmpresa,
                        IdEntrevista = newEntrevista.IdEntrevista,
                        IdTipoDocumento = _IdTipoDocumento,
                        Empresa = "Grupo Aplired",
                        Nombre = NombreResponsableEntrevistador
                    };

                    tblDocumentoEntrevistaPersona Entrevistado = new tblDocumentoEntrevistaPersona
                    {
                        EsEntrevistador = true,
                        IdDocumento = _IdDocumento,
                        IdEmpresa = _IdEmpresa,
                        IdEntrevista = newEntrevista.IdEntrevista,
                        IdTipoDocumento = _IdTipoDocumento,
                        Empresa = _Empresa.NombreComercial,
                        Nombre = NombreResponsable
                    };

                    db.tblDocumentoEntrevistaPersona.Add(Entrevistador);
                    db.tblDocumentoEntrevistaPersona.Add(Entrevistado);

                    db.SaveChanges();
                } // End Using db
            } // End Try
            catch (Exception ex)
            {
                throw ex;
            } // End catch
        }
        private static void DeleteFile(string _filePath)
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
        private static string SaveFile(MemoryStream memStream)
        {
            long UserId = long.Parse(Session["UserId"].ToString());
            string _FolderName = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/DocsFolder");
            string _FileName= String.Format("{docTemp_{0}.docx", UserId.ToString());
            string _FullFileName = String.Format("{0}\\{1}", _FolderName, _FileName);

            if (File.Exists(_FullFileName)) File.Delete(_FullFileName);

            try
            {
                FileStream _file = new FileStream(_FullFileName, FileMode.Append);
                _file.Write(memStream.ToArray(), 0, (int)memStream.Length);
                _file.Flush();
                _file.Close();
                _file.Dispose();
                _file = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _FullFileName;
        }
    }
}