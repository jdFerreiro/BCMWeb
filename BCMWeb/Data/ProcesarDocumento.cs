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
        private static Encriptador _Encriptar = new Encriptador();

        [SessionExpire]
        [HandleError]
        public static void ProcesarFicha(long ModuloId, byte[] Contenido)
        {

            // string _filePath = SaveFile(new MemoryStream(Contenido));
            MemoryStream msContent = new MemoryStream(Contenido);

            try
            {
                switch (ModuloId)
                {
                    case 4010100: // ficha del BIA
                        ProcessFichaEstandar(msContent);
                        //ProcessFichaBIA(msContent);
                        break;
                    case 4010200: // Entradas del Proceso
                        ProcesarEntradasdelProceso(msContent);
                        break;
                    case 4010300: // Proveedores
                        ProcesarProveedores(msContent);
                        break;
                    case 4010400: // Interdependencias
                        ProcesarInterdependencias(msContent);
                        break;
                    case 4010500: // Clientes y Productos
                        ProcesarClientesyProductos(msContent);
                        break;
                    case 4010600: // Tecnología
                        ProcesarTecnología(msContent);
                        break;
                    case 4010700: // Información Esencial
                        ProcesarInformaciónEsencial(msContent);
                        break;
                    case 4010800: // Personal Clave
                        ProcesarPersonalClave(msContent);
                        break;
                    case 4030100: // Impacto Financiero
                        ProcesarImpactoFinanciero(msContent);
                        break;
                    case 4030200: // Impacto Operacional
                        ProcesarImpactoOperacional(msContent);
                        break;
                    case 4030300: // Escalas
                        ProcesarAnalisisRiesgo(msContent);
                        break;
                    case 4040100: // TMC
                        ProcesarTMC(msContent);
                        break;
                    case 4040200: // TOR
                        ProcesarTOR(msContent);
                        break;
                    case 4040300: // POR
                        ProcesarPOR(msContent);
                        break;
                    case 4040400: // TRT
                        ProcesarTRT(msContent);
                        break;
                    case 4050100: // Procedimientos Alternos
                        ProcesarProcedimientosAlternos(msContent);
                        break;
                    case 4060100: // Ubicación Principal
                        ProcesarUbicaciónPrincipal(msContent);
                        break;
                    case 4060200: // Ubicación Alterna
                        ProcesarUbicaciónAlterna(msContent);
                        break;
                    case 4070100: // Grandes Impactos
                        ProcesarGrandesImpactos(msContent);
                        break;
                    //case 4080100: // Diagrama de Impacto
                    //    ProcesarDiagramadeImpacto(msContent);
                    //    break;
                    //case 4080200: // Diagrama de Tecnología
                    //    ProcesarDiagramadeTecnología(msContent);
                    //    break;
                    //case 4080300: // Diagrama de Personas Claves
                    //    ProcesarDiagramadePersonasClaves(msContent);
                    //    break;
                    case 7000101: // Ficha del BCP
                    case 1010100:
                    case 2010100:
                    case 3010100:
                    case 5010100:
                    case 6010100:
                    case 8010100:
                    case 9010100:
                    case 10010100:
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

        private static void ProcesarGrandesImpactos(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (int _Row = 1; _Row < _tableRows.Count(); _Row++)
                                {
                                    if (_NroTabla == 1) { 
                                        TableRow _tableRow = (TableRow)_tableRows[_Row];

                                        if (_tableRow.HasChildren)
                                        {
                                            List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();

                                            for (int _Cell = 0; _Cell < _Procesos.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += ", ";
                                                        _textoCelda += _cellParagraph.InnerText;
                                                    } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                    {
                                                        _Datos[_Row - 1, _Cell - 1] = _Procesos[_Row - 1].IdProceso.ToString();
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
                                                                if (!string.IsNullOrEmpty(_textoCelda))
                                                                    _textoCelda += ", ";
                                                                _textoCelda += _cellParagraph.InnerText;
                                                            } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                            switch (_Cell)
                                                            {
                                                                case 0:
                                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                                        _NombreProceso = _textoCelda;
                                                                    tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                                     && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                                     && x.NroProceso == _NroProceso).FirstOrDefault();

                                                                    if (procBIA == null)
                                                                    {
                                                                        procBIA = new tblBIAProceso
                                                                        {
                                                                            Critico = false,
                                                                            Descripcion = string.Empty,
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                    case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 1; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                switch (_Cell)
                                                {
                                                   case 0:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _NombreProceso = _textoCelda;
                                                        tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                for (_Row = 1; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        int _nroCeldas = _tableCells.Count();
                                        long[] _IdEscala = new long[_tableCells.Count()];
                                        long IdEscala = 0;

                                        if (_Row == 1)
                                        {
                                            for (int _Cell = 2; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                string _textoCelda = string.Empty;
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                tblEscala dataEscala = new tblEscala()
                                                {
                                                    IdEmpresa = _IdEmpresa,
                                                    Descripcion = _textoCelda,
                                                    FechaRegistro = DateTime.UtcNow,
                                                    IdTipoEscala = 6,
                                                };
                                                switch (_Cell)
                                                {
                                                    case 2:
                                                    case 3:
                                                        dataEscala.Valor = 5;
                                                        break;
                                                    case 4:
                                                        dataEscala.Valor = 4;
                                                        break;
                                                    case 5:
                                                        dataEscala.Valor = 3;
                                                        break;
                                                    case 6:
                                                        dataEscala.Valor = 2;
                                                        break;
                                                    case 7:
                                                    case 8:
                                                        dataEscala.Valor = 1;
                                                        break;
                                                }

                                                db.tblEscala.Add(dataEscala);
                                                db.SaveChanges();
                                                _IdEscala[_Cell] = dataEscala.IdEscala;

                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        }
                                        else
                                        {
                                            for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += ", ";
                                                        _textoCelda += _cellParagraph.InnerText;
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
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA == null)
                                                            {
                                                                procBIA = new tblBIAProceso
                                                                {
                                                                    Critico = false,
                                                                    Descripcion = string.Empty,
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
                                                            break;
                                                        default:
                                                            if (_textoCelda.ToLower() == "x")
                                                            {
                                                                IdEscala = _IdEscala[_Cell];
                                                            }
                                                            break;
                                                    }
                                                } // End if (_celda.HasChildren)
                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                            tblBIAWRT reg = new tblBIAWRT
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = _IdEmpresa,
                                                IdProceso = IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = IdEscala,
                                                IdTipoFrecuencia = null,
                                            };

                                            db.tblBIAWRT.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                for (_Row = 1; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        int _nroCeldas = _tableCells.Count();
                                        long[] _IdEscala = new long[_tableCells.Count()];
                                        long IdEscala = 0;

                                        if (_Row == 1)
                                        {
                                            for (int _Cell = 2; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                string _textoCelda = string.Empty;
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                tblEscala dataEscala = new tblEscala()
                                                {
                                                    IdEmpresa = _IdEmpresa,
                                                    Descripcion = _textoCelda,
                                                    FechaRegistro = DateTime.UtcNow,
                                                    IdTipoEscala = 5,
                                                };
                                                switch (_Cell)
                                                {
                                                    case 2:
                                                    case 3:
                                                        dataEscala.Valor = 5;
                                                        break;
                                                    case 4:
                                                        dataEscala.Valor = 4;
                                                        break;
                                                    case 5:
                                                        dataEscala.Valor = 3;
                                                        break;
                                                    case 6:
                                                        dataEscala.Valor = 2;
                                                        break;
                                                    case 7:
                                                    case 8:
                                                        dataEscala.Valor = 1;
                                                        break;
                                                }

                                                db.tblEscala.Add(dataEscala);
                                                db.SaveChanges();
                                                _IdEscala[_Cell] = dataEscala.IdEscala;

                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        }
                                        else
                                        {
                                            for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += ", ";
                                                        _textoCelda += _cellParagraph.InnerText;
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
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA == null)
                                                            {
                                                                procBIA = new tblBIAProceso
                                                                {
                                                                    Critico = false,
                                                                    Descripcion = string.Empty,
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
                                                            break;
                                                        default:
                                                            if (_textoCelda.ToLower() == "x")
                                                            {
                                                                IdEscala = _IdEscala[_Cell];
                                                            }
                                                            break;
                                                    }
                                                } // End if (_celda.HasChildren)
                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                            tblBIARPO reg = new tblBIARPO
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = _IdEmpresa,
                                                IdProceso = IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIARPO.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                for (_Row = 1; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        int _nroCeldas = _tableCells.Count();
                                        long[] _IdEscala = new long[_tableCells.Count()];
                                        long IdEscala = 0;

                                        if (_Row == 1)
                                        {
                                            for (int _Cell = 2; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                string _textoCelda = string.Empty;
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                tblEscala dataEscala = new tblEscala()
                                                {
                                                    IdEmpresa = _IdEmpresa,
                                                    Descripcion = _textoCelda,
                                                    FechaRegistro = DateTime.UtcNow,
                                                    IdTipoEscala = 4,
                                                };
                                                switch (_Cell)
                                                {
                                                    case 2:
                                                    case 3:
                                                        dataEscala.Valor = 5;
                                                        break;
                                                    case 4:
                                                        dataEscala.Valor = 4;
                                                        break;
                                                    case 5:
                                                        dataEscala.Valor = 3;
                                                        break;
                                                    case 6:
                                                        dataEscala.Valor = 2;
                                                        break;
                                                    case 7:
                                                    case 8:
                                                        dataEscala.Valor = 1;
                                                        break;
                                                }

                                                db.tblEscala.Add(dataEscala);
                                                db.SaveChanges();
                                                _IdEscala[_Cell] = dataEscala.IdEscala;

                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        }
                                        else
                                        {
                                            for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += ", ";
                                                        _textoCelda += _cellParagraph.InnerText;
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
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA == null)
                                                            {
                                                                procBIA = new tblBIAProceso
                                                                {
                                                                    Critico = false,
                                                                    Descripcion = string.Empty,
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
                                                            break;
                                                        default:
                                                            if (_textoCelda.ToLower() == "x")
                                                            {
                                                                IdEscala = _IdEscala[_Cell];
                                                            }
                                                            break;
                                                    }
                                                } // End if (_celda.HasChildren)
                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                            tblBIARTO reg = new tblBIARTO
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = _IdEmpresa,
                                                IdProceso = IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIARTO.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                for (_Row = 1; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    if (_tableRow.HasChildren)
                                    {
                                        List<OpenXmlElement> _tableCells = _tableRow.ChildElements.Where(x => x.GetType().Name == "TableCell").ToList();
                                        int _nroCeldas = _tableCells.Count();
                                        long[] _IdEscala = new long[_tableCells.Count()];
                                        long IdEscala = 0;

                                        if (_Row == 1)
                                        {
                                            for (int _Cell = 2; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                string _textoCelda = string.Empty;
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                foreach (var _cellParagraph in _tableCellParagraph)
                                                {
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
                                                } // End foreach (var _cellParagraph in _tableCellParagraph)

                                                tblEscala dataEscala = new tblEscala()
                                                {
                                                    IdEmpresa = _IdEmpresa,
                                                    Descripcion = _textoCelda,
                                                    FechaRegistro = DateTime.UtcNow,
                                                    IdTipoEscala = 3,
                                                };
                                                switch (_Cell)
                                                {
                                                    case 2:
                                                    case 3:
                                                        dataEscala.Valor = 5;
                                                        break;
                                                    case 4:
                                                        dataEscala.Valor = 4;
                                                        break;
                                                    case 5:
                                                        dataEscala.Valor = 3;
                                                        break;
                                                    case 6:
                                                        dataEscala.Valor = 2;
                                                        break;
                                                    case 7:
                                                    case 8:
                                                        dataEscala.Valor = 1;
                                                        break;
                                                }

                                                db.tblEscala.Add(dataEscala);
                                                db.SaveChanges();
                                                _IdEscala[_Cell] = dataEscala.IdEscala;

                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                        }
                                        else
                                        {
                                            for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)
                                            {
                                                TableCell _celda = (TableCell)_tableCells[_Cell];
                                                if (_celda.HasChildren)
                                                {
                                                    string _textoCelda = string.Empty;
                                                    List<OpenXmlElement> _tableCellParagraph = _celda.ChildElements.Where(x => x.GetType().Name == "Paragraph").ToList();
                                                    foreach (var _cellParagraph in _tableCellParagraph)
                                                    {
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                            _textoCelda += ", ";
                                                        _textoCelda += _cellParagraph.InnerText;
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
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

                                                            if (procBIA == null)
                                                            {
                                                                procBIA = new tblBIAProceso
                                                                {
                                                                    Critico = false,
                                                                    Descripcion = string.Empty,
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
                                                            break;
                                                        default:
                                                            if (_textoCelda.ToLower() == "x")
                                                            {
                                                                IdEscala = _IdEscala[_Cell];
                                                            }
                                                            break;
                                                    }
                                                } // End if (_celda.HasChildren)
                                            } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                            tblBIAMTD reg = new tblBIAMTD
                                            {
                                                IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                                IdEmpresa = _IdEmpresa,
                                                IdProceso = IdProceso,
                                                Observacion = string.Empty,
                                                IdEscala = IdEscala,
                                                IdTipoFrecuencia = 0,
                                            };

                                            db.tblBIAMTD.Add(reg);
                                        }
                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
            throw new NotImplementedException();
        }

        private static void ProcesarImpactoOperacional(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

                                        tblBIAImpactoOperacional reg = new tblBIAImpactoOperacional
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Descripcion = _Descripcion,
                                            ImpactoOperacional = _Impacto,
                                            UnidadTiempo = _UnidadTiempo
                                        };

                                        db.tblBIAImpactoOperacional.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

                                        tblBIAImpactoFinanciero reg = new tblBIAImpactoFinanciero
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Descripcion = _Descripcion,
                                            Impacto = _Impacto,
                                            UnidadTiempo = _UnidadTiempo
                                        };

                                        db.tblBIAImpactoFinanciero.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Nombre = string.Empty;
                                    string _Identificacion = string.Empty;
                                    string _TelefonoOficina = string.Empty;
                                    string _TelefonoCelular = string.Empty;
                                    string _TelefonoHabitacion = string.Empty;
                                    string _CorreosElectronicos = string.Empty;
                                    string _DireccionHabitacion = string.Empty;
                                    long _IdPersonaClave = 0;

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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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
                                        }

                                        tblBIAPersonaClave reg = new tblBIAPersonaClave
                                        {
                                            IdDocumento = _IdDocumento,
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdTipoDocumento = _IdTipoDocumento,
                                            IdPersonaClave = _dataPersonaClave.IdPersona,
                                            IdProceso = IdProceso,
                                        };

                                        db.tblBIAPersonaClave.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
        private static void ProcesarInformaciónEsencial(MemoryStream msContent)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long _IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int _IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Ubicacion = string.Empty;
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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
                                                        break;
                                                    case 2:
                                                        _Informacion = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Ubicacion = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIADocumentacion reg = new tblBIADocumentacion
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Nombre = _Informacion,
                                            Ubicacion = _Ubicacion
                                        };

                                        db.tblBIADocumentacion.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
                                {
                                    TableRow _tableRow = (TableRow)_tableRows[_Row];

                                    string _Aplicaciones = string.Empty;
                                    string _Usuarios = string.Empty;

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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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
                                                        break;
                                                    case 2:
                                                        _Aplicaciones = _textoCelda;
                                                        break;
                                                    case 3:
                                                        _Usuarios = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAAplicacion reg = new tblBIAAplicacion
                                        {
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Nombre = _Aplicaciones,
                                            Usuario = _Usuarios
                                        };

                                        db.tblBIAAplicacion.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

                                        tblBIAClienteProceso reg = new tblBIAClienteProceso
                                        {
                                            Proceso = _ProcesoUnidad,
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Producto = _ProcesoUnidad,
                                            Responsable = _ResponsableUnidad,
                                            Unidad = _UnidadTrabajo
                                        };

                                        db.tblBIAClienteProceso.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                                                                         && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                         && x.NroProceso == _NroProceso).FirstOrDefault();

                                                        if (procBIA == null)
                                                        {
                                                            procBIA = new tblBIAProceso
                                                            {
                                                                Critico = false,
                                                                Descripcion = string.Empty,
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

                                        tblBIAInterdependencia reg = new tblBIAInterdependencia
                                        {
                                            Contacto = _Contacto,
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Organizacion = _Organizaciones,
                                            Servicio = _Servicio
                                        };

                                        db.tblBIAInterdependencia.Add(reg);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                    List<tblBIAEntrada> Entradas = db.tblBIAEntrada.Where(x => x.IdEmpresa == _IdEmpresa
                                                                            && x.IdDocumentoBIA == _DocBIA.IdDocumentoBIA).ToList();

                    db.tblBIAEntrada.RemoveRange(Entradas);

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(msContent, false))
                    {
                        foreach (var _Elemento in wordDocument.MainDocumentPart.Document.Body.Where(x => x.GetType().Name == "Table").ToList())
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Table _Table = (DocumentFormat.OpenXml.Wordprocessing.Table)_Elemento;
                            if (_Table.HasChildren)
                            {
                                int _Row = 0;
                                List<OpenXmlElement> _tableRows = _Table.ChildElements.Where(x => x.GetType().Name == "TableRow").ToList();

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                        break;
                                                    case 2:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {

                                                            _DescripcionProceso = _textoCelda;

                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

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

            int _NroProceso = 0;
            long IdProceso = 0;
            string _NombreProceso = string.Empty;
            string _DescripcionProceso = string.Empty;

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

                                for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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
                                                    if (!string.IsNullOrEmpty(_textoCelda))
                                                        _textoCelda += ", ";
                                                    _textoCelda += _cellParagraph.InnerText;
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
                                                        break;
                                                    case 2:
                                                        if (!string.IsNullOrEmpty(_textoCelda))
                                                        {

                                                            _DescripcionProceso = _textoCelda;

                                                            tblBIAProceso procBIA = db.tblBIAProceso.Where(x => x.IdEmpresa == _IdEmpresa
                                                                                                             && x.IdDocumentoBia == _DocBIA.IdDocumentoBIA
                                                                                                             && x.NroProceso == _NroProceso).FirstOrDefault();

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
                                                        _Organizacion = _textoCelda;
                                                        break;
                                                    case 4:
                                                        _Servicio = _textoCelda;
                                                        break;
                                                    case 5:
                                                        _Contacto = _textoCelda;
                                                        break;
                                                }
                                            } // End if (_celda.HasChildren)
                                        } // End for (int _Cell = 0; _Cell < _tableCells.Count(); _Cell++)

                                        tblBIAProveedor Proveedor = new tblBIAProveedor
                                        {
                                            Contacto = _Contacto,
                                            IdDocumentoBIA = _DocBIA.IdDocumentoBIA,
                                            IdEmpresa = _IdEmpresa,
                                            IdProceso = IdProceso,
                                            Organizacion = _Organizacion,
                                            Servicio = _Servicio
                                        };

                                        db.tblBIAProveedor.Add(Proveedor);

                                    } // End if (_tableCell.HasChildren)
                                } // End for (_Row = 2; _Row < _tableRows.Count(); _Row++)
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