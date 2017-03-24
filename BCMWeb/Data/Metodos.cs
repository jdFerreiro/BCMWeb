using BCMWeb.Data.EF;
using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;


namespace BCMWeb
{

    public static class Metodos
    {
        private static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        private static string Culture = HttpContext.Current.Request.UserLanguages[0];
        private static Uri _ContextUrl = HttpContext.Current.Request.Url;
        private static string _AppUrl = _ContextUrl.AbsoluteUri.Replace(_ContextUrl.AbsolutePath, string.Empty);
        private static HttpServerUtility _Server = HttpContext.Current.Server;

        public static IList<EmpresaModel> GetEmpresasUsuario()
        {

            List<EmpresaModel> Data = new List<EmpresaModel>();

            if (Session != null && Session["UserId"] != null)
            {
                long UserId = long.Parse(Session["UserId"].ToString());

                using (Entities db = new Entities())
                {
                    Data = (from eu in db.tblEmpresaUsuario
                            where eu.IdUsuario == UserId
                            select new EmpresaModel
                            {
                                Direccion = eu.tblEmpresa.DireccionAdministrativa,
                                FechaInicio = eu.tblEmpresa.FechaInicioActividad,
                                FechaUltimoEstado = eu.tblEmpresa.FechaUltimoEstado,
                                IdEmpresa = eu.tblEmpresa.IdEmpresa,
                                IdEstatus = eu.tblEmpresa.IdEstado,
                                LogoUrl = eu.tblEmpresa.LogoURL,
                                NombreComercial = eu.tblEmpresa.NombreComercial,
                                NombreFiscal = eu.tblEmpresa.NombreFiscal,
                                RegistroFiscal = eu.tblEmpresa.RegistroFiscal
                            }).ToList();
                }

            }
            
            return Data;
        }
        public static string GetNombreModulo(long idModulo)
        {
            string _NombreModulo = string.Empty;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string _IdModulo = idModulo.ToString();
            int IdCodigoModulo = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));

            using (Entities db = new Entities())
            {
                _NombreModulo = db.tblModulo.Where(x => x.IdEmpresa == IdEmpresa && x.IdCodigoModulo == IdCodigoModulo && x.IdModuloPadre == 0).FirstOrDefault().Nombre;
            }

            return _NombreModulo;
        }
        public static IList<ModuloModel> GetModulosPrincipalesEmpresaUsuario()
        {
            List<ModuloModel> Data = new List<ModuloModel>();

            if (Session != null && Session["IdEmpresa"] != null && Session["UserId"] != null)
            {
                long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
                long UserId = long.Parse(Session["UserId"].ToString());

                using (Entities db = new Entities())
                {
                    Nullable<long> IdNivelUsuario = db.tblEmpresaUsuario
                        .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == UserId)
                        .Select(x => x.IdNivelUsuario)
                        .FirstOrDefault();

                    if (IdNivelUsuario != null)
                    {
                        Data = (from m in db.tblModulo_Usuario
                                where m.IdEmpresa == IdEmpresa && m.tblModulo.IdModuloPadre == 0 
                                        && m.IdUsuario == UserId && m.IdModulo < 99000000 && m.tblModulo.Activo
                                select new ModuloModel
                                {
                                    Action = m.tblModulo.Accion,
                                    Activo = m.tblModulo.Activo,
                                    CodigoModulo = (int)m.tblModulo.IdCodigoModulo,
                                    Controller = m.tblModulo.Controller,
                                    Descripcion = m.tblModulo.Descripcion,
                                    IdModulo = m.tblModulo.IdModulo,
                                    IdModuloPadre = 0,
                                    IdTipoElemento = m.tblModulo.IdTipoElemento,
                                    ImageRoot = m.tblModulo.imageRoot,
                                    Negocios = m.tblModulo.Negocios,
                                    Nombre = m.tblModulo.Nombre,
                                    Tecnologia = m.tblModulo.Tecnologia,
                                    Titulo = m.tblModulo.Titulo
                                }).ToList();
                    }
                }
            }
            
            return Data;
        }
        public static IList<DocumentoModel> GetDocumentosModulo(int IdTipoDocumento, bool Negocios)
        {
            string _ServerPath = _Server.MapPath(".").Replace("\\Account", string.Empty);
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdUser = long.Parse(Session["UserId"].ToString());
            long minModulo = IdTipoDocumento * 1000000;
            long maxModulo = IdTipoDocumento * 1999999;
            eSystemModules Modulo = (eSystemModules)IdTipoDocumento;
            string TipoDocumento = Modulo.ToString();

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;
            
            List<DocumentoModel> Documentos = new List<DocumentoModel>();
            using (Entities db = new Entities())
            {
                long ModulosEditables = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa
                            && x.IdUsuario == IdUser
                            && x.IdModulo >= minModulo
                            && x.IdModulo <= maxModulo
                            && x.Actualizar).Count();

                long ModulosEliminables = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa
                            && x.IdUsuario == IdUser
                            && x.IdModulo >= minModulo
                            && x.IdModulo <= maxModulo
                            && x.Eliminar).Count();

                tblEmpresaUsuario EmpresaUsuario = db.tblEmpresaUsuario
                    .Where(x => x.IdEmpresa == IdEmpresa
                            && x.IdUsuario == IdUser).FirstOrDefault();

                List<tblDocumento> dataDocumentos = (from d in db.tblDocumento
                                                     where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.Negocios == Negocios
                                                     select d).ToList();

                foreach (tblDocumento d in dataDocumentos)
                {
                    long docNroDocumento = d.NroDocumento;
                    long docVersion = d.NroVersion;

                    bool HasVersion = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                            && x.IdTipoDocumento == IdTipoDocumento
                                                            && x.NroDocumento == docNroDocumento
                                                            && x.NroVersion > docVersion).Count() > 0;
                    DocumentoModel Documento = new DocumentoModel
                    {
                        Editable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEditables > 0),
                        Eliminable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEliminables > 0),
                        Estatus = d.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion,
                        FechaCreacion = (DateTime)d.FechaCreacion.AddMinutes(Minutos),
                        FechaEstadoDocumento = (DateTime)d.FechaEstadoDocumento.AddMinutes(Minutos),
                        FechaUltimaModificacion = (DateTime)(d.FechaUltimaModificacion != null ? ((DateTime)d.FechaUltimaModificacion).AddMinutes(Minutos) : d.FechaUltimaModificacion),
                        IdDocumento = d.IdDocumento,
                        IdEstatus = d.IdEstadoDocumento,
                        IdPersonaResponsable = d.IdPersonaResponsable,
                        IdTipoDocumento = d.IdTipoDocumento,
                        Negocios = d.Negocios,
                        NroDocumento = d.NroDocumento,
                        NroVersion = d.NroVersion,
                        RequiereCertificacion = d.RequiereCertificacion,
                        Version = string.Format("{0}.{1}", d.VersionOriginal.ToString(), d.NroVersion.ToString()),
                        HasVersion = HasVersion,
                        VersionOriginal = (int)d.VersionOriginal,
                    };


                    eEstadoDocumento EstadoDocumento = (eEstadoDocumento)Documento.IdEstatus;
                    string _CodigoInforme = string.Format("{0}_{1}_{2}_{3}.{4}", TipoDocumento, Documento.NroDocumento.ToString("#000"), (EstadoDocumento == eEstadoDocumento.Certificado ? Documento.FechaEstadoDocumento.ToString("MM-yyyy") : DateTime.Now.ToString("MM-yyyy")), Documento.VersionOriginal, Documento.NroVersion);
                    string _FileName = string.Format("{0}.pdf", _CodigoInforme.Replace("-", "_"));
                    string _pathFile = String.Format("{0}\\PDFDocs\\{1}", _ServerPath, _FileName);

                    if (System.IO.File.Exists(_pathFile))
                        Documento.RutaDocumentoPDF = String.Format("{0}/PDFDocs/{1}", _AppUrl, _FileName);

                    Documentos.Add(Documento);
                }
            }
            return Documentos;
        }
        public static DocumentoModel GetDocumento(long IdDocumento, int IdTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            DocumentoModel Documento = new DocumentoModel();
            using (Entities db = new Entities())
            {
                tblDocumento data = (from d in db.tblDocumento
                                  where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.IdDocumento == IdDocumento
                                  select d).FirstOrDefault();
                if (data != null)
                {
                    Documento = new DocumentoModel
                    {
                        Estatus = data.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion,
                        FechaCreacion = (DateTime)data.FechaCreacion.AddMinutes(Minutos),
                        FechaEstadoDocumento = (DateTime)data.FechaEstadoDocumento.AddMinutes(Minutos),
                        FechaUltimaModificacion = (DateTime)((DateTime)data.FechaUltimaModificacion).AddMinutes(Minutos),
                        IdDocumento = data.IdDocumento,
                        IdEstatus = data.IdEstadoDocumento,
                        IdPersonaResponsable = data.IdPersonaResponsable,
                        IdTipoDocumento = data.IdTipoDocumento,
                        Negocios = data.Negocios,
                        NroDocumento = data.NroDocumento,
                        NroVersion = data.NroVersion,
                        RequiereCertificacion = data.RequiereCertificacion,
                        Version = string.Format("{0}.{1}", data.VersionOriginal.ToString(), data.NroVersion.ToString()),
                        VersionOriginal = (int)data.VersionOriginal
                    };
                }
            }
            return Documento;
        }
        public static bool EliminarDocumento(long IdDocumento, long IdTipoDocumento)
        {
            bool Eliminado = false;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                try
                {

                    tblDocumento documento = (from d in db.tblDocumento
                                              where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento && d.IdTipoDocumento == IdTipoDocumento
                                              select d).FirstOrDefault();

                    switch (IdTipoDocumento)
                    {
                        case 4:
                            foreach (tblBIADocumento docBIA in documento.tblBIADocumento)
                            {
                                foreach (tblBIAProceso procesoBIA in docBIA.tblBIAProceso)
                                {

                                    foreach (tblBCPDocumento docBCP in procesoBIA.tblBCPDocumento)
                                    {
                                        db.tblBCPReanudacionPersonaClave.RemoveRange(docBCP.tblBCPReanudacionPersonaClave.ToList());
                                        db.tblBCPReanudacionTarea.RemoveRange(docBCP.tblBCPReanudacionTarea.ToList());
                                        db.tblBCPRecuperacionPersonaClave.RemoveRange(docBCP.tblBCPRecuperacionPersonaClave.ToList());
                                        db.tblBCPRecuperacionRecurso.RemoveRange(docBCP.tblBCPRecuperacionRecurso.ToList());
                                        db.tblBCPRespuestaAccion.RemoveRange(docBCP.tblBCPRespuestaAccion.ToList());
                                        db.tblBCPRespuestaRecurso.RemoveRange(docBCP.tblBCPRespuestaRecurso.ToList());
                                        db.tblBCPRestauracionEquipo.RemoveRange(docBCP.tblBCPRestauracionEquipo.ToList());
                                        db.tblBCPRestauracionInfraestructura.RemoveRange(docBCP.tblBCPRestauracionInfraestructura.ToList());
                                        db.tblBCPRestauracionMobiliario.RemoveRange(docBCP.tblBCPRestauracionMobiliario.ToList());
                                        db.tblBCPRestauracionOtro.RemoveRange(docBCP.tblBCPRestauracionOtro.ToList());
                                    }
                                    foreach (tblBIAUnidadTrabajoProceso udadProceso in procesoBIA.tblBIAUnidadTrabajoProceso)
                                    {
                                        foreach (tblBIAUnidadTrabajoPersonas UTP in udadProceso.tblBIAUnidadTrabajoPersonas)
                                        {
                                            db.tblBIAClienteProceso.Remove(UTP.tblBIAClienteProceso);
                                            db.tblBIAUnidadTrabajoPersonas.Remove(UTP);
                                        }

                                        db.tblBIAUnidadTrabajoProceso.Remove(udadProceso);
                                    }

                                    db.tblBCPDocumento.RemoveRange(procesoBIA.tblBCPDocumento.ToList());
                                    db.tblBIAAmenaza.RemoveRange(procesoBIA.tblBIAAmenaza.ToList());
                                    db.tblBIAAplicacion.RemoveRange(procesoBIA.tblBIAAplicacion.ToList());
                                    db.tblBIADocumentacion.RemoveRange(procesoBIA.tblBIADocumentacion.ToList());
                                    db.tblBIAEntrada.RemoveRange(procesoBIA.tblBIAEntrada.ToList());
                                    db.tblBIAGranImpacto.RemoveRange(procesoBIA.tblBIAGranImpacto.ToList());
                                    db.tblBIAImpactoFinanciero.RemoveRange(procesoBIA.tblBIAImpactoFinanciero.ToList());
                                    db.tblBIAImpactoOperacional.RemoveRange(procesoBIA.tblBIAImpactoOperacional.ToList());
                                    db.tblBIAInterdependencia.RemoveRange(procesoBIA.tblBIAInterdependencia.ToList());
                                    db.tblBIAMTD.RemoveRange(procesoBIA.tblBIAMTD.ToList());
                                    db.tblBIAPersonaRespaldoProceso.RemoveRange(procesoBIA.tblBIAPersonaRespaldoProceso.ToList());
                                    db.tblBIAProcesoAlterno.RemoveRange(procesoBIA.tblBIAProcesoAlterno.ToList());
                                    db.tblBIAProveedor.RemoveRange(procesoBIA.tblBIAProveedor.ToList());
                                    db.tblBIARespaldoPrimario.RemoveRange(procesoBIA.tblBIARespaldoPrimario.ToList());
                                    db.tblBIARespaldoSecundario.RemoveRange(procesoBIA.tblBIARespaldoSecundario.ToList());
                                    db.tblBIARPO.RemoveRange(procesoBIA.tblBIARPO.ToList());
                                    db.tblBIARTO.RemoveRange(procesoBIA.tblBIARTO.ToList());
                                    db.tblBIAWRT.RemoveRange(procesoBIA.tblBIAWRT.ToList());
                                }

                                db.tblBIADocumento.Remove(docBIA);
                            }
                            break;
                        case 7:

                            foreach (tblBCPDocumento docBCP in documento.tblBCPDocumento)
                            {
                                db.tblBCPReanudacionPersonaClave.RemoveRange(docBCP.tblBCPReanudacionPersonaClave.ToList());
                                db.tblBCPReanudacionTarea.RemoveRange(docBCP.tblBCPReanudacionTarea.ToList());
                                db.tblBCPRecuperacionPersonaClave.RemoveRange(docBCP.tblBCPRecuperacionPersonaClave.ToList());
                                db.tblBCPRecuperacionRecurso.RemoveRange(docBCP.tblBCPRecuperacionRecurso.ToList());
                                db.tblBCPRespuestaAccion.RemoveRange(docBCP.tblBCPRespuestaAccion.ToList());
                                db.tblBCPRespuestaRecurso.RemoveRange(docBCP.tblBCPRespuestaRecurso.ToList());
                                db.tblBCPRestauracionEquipo.RemoveRange(docBCP.tblBCPRestauracionEquipo.ToList());
                                db.tblBCPRestauracionInfraestructura.RemoveRange(docBCP.tblBCPRestauracionInfraestructura.ToList());
                                db.tblBCPRestauracionMobiliario.RemoveRange(docBCP.tblBCPRestauracionMobiliario.ToList());
                                db.tblBCPRestauracionOtro.RemoveRange(docBCP.tblBCPRestauracionOtro.ToList());
                            }
                            db.tblBCPDocumento.RemoveRange(documento.tblBCPDocumento);
                            break;
                    }

                    foreach (tblDocumentoEntrevista entrevista in documento.tblDocumentoEntrevista)
                    {
                        db.tblDocumentoEntrevistaPersona.RemoveRange(entrevista.tblDocumentoEntrevistaPersona.ToList());
                    }
                    db.tblDocumentoAnexo.RemoveRange(documento.tblDocumentoAnexo.ToList());
                    db.tblDocumentoAprobacion.RemoveRange(documento.tblDocumentoAprobacion.ToList());
                    db.tblAuditoria.RemoveRange(documento.tblAuditoria.ToList());
                    db.tblDocumentoCertificacion.RemoveRange(documento.tblDocumentoCertificacion.ToList());
                    db.tblDocumentoContenido.RemoveRange(documento.tblDocumentoContenido.ToList());
                    db.tblDocumentoEntrevista.RemoveRange(documento.tblDocumentoEntrevista.ToList());
                    db.tblDocumentoPersonaClave.RemoveRange(documento.tblDocumentoPersonaClave.ToList());
                    db.tblDocumento.Remove(documento);

                    List<tblDocumento> nextDocumentos = db.tblDocumento
                        .Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == IdTipoDocumento && x.NroDocumento > documento.NroDocumento)
                        .ToList();

                    long LastNroDoc = documento.NroDocumento - 1;

                    List<tblDocumento> ListPrevDoc = db.tblDocumento
                            .Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == IdTipoDocumento && x.NroDocumento < documento.NroDocumento)
                            .OrderBy(x => x.NroDocumento).ToList();

                    if (ListPrevDoc != null && ListPrevDoc.Count > 0)
                    {
                        tblDocumento LastPrevDoc = ListPrevDoc.LastOrDefault();
                        LastNroDoc = LastPrevDoc.NroDocumento;
                    }

                    foreach (tblDocumento nextDoc in nextDocumentos)
                    {
                        LastNroDoc++;
                        nextDoc.NroDocumento = LastNroDoc;
                    }

                    db.SaveChanges();
                    Eliminado = true;
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                    Eliminado = false;
                }
            }

            if (Eliminado) Auditoria.RegistarAccion(eTipoAccion.Eliminar);

            return Eliminado;
        }
        public static long GetNextNroDocumento(long IdClaseDocumento, long IdTipoDocumento)
        {
            long NextNroDocumento = 0;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            bool Negocios = (IdClaseDocumento == 1 ? true : false);

            using (Entities db = new Entities())
            {
                List<tblDocumento> Documentos = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == IdTipoDocumento && x.Negocios == Negocios).ToList();
                NextNroDocumento = (Documentos == null || Documentos.Count() == 0 ? 0 : Documentos.Max(x => x.NroDocumento));
            }

            NextNroDocumento += 1;
            return NextNroDocumento;
        }
        public static int GetNextVersion(long idClaseDocumento, long idTipoDocumento, int VersionOriginal)
        {
            int NextVersion = 0;
            bool Negocios = (idClaseDocumento == 1);
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {

                List<tblDocumento> Documentos = db.tblDocumento
                    .Where(x => x.IdEmpresa == IdEmpresa
                        && x.IdTipoDocumento == idTipoDocumento
                        && x.VersionOriginal == VersionOriginal).ToList();

                NextVersion = (Documentos != null || Documentos.Count() == 0 ? 0 : Documentos.Max(x => x.NroVersion));

            }

            NextVersion += 1;
            return NextVersion;
        }
        public static IList<tblModulo> GetSubModulos(long IdModuloPadre)
        {
            List<tblModulo> SubModulos = new List<tblModulo>();
            long IdUsuario = long.Parse(Session["UserId"].ToString());
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());

            using (Entities db = new Entities())
            {
                long IdNivelUsuario = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario).FirstOrDefault().IdNivelUsuario;
                SubModulos = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdModuloPadre == IdModuloPadre && x.tblModulo.Activo)
                    .Select(x => x.tblModulo).ToList();

                if (IdModuloPadre == 99010000)
                {
                    if (IdDocumento > 0) {
                        eEstadoDocumento IdEstadoDocumento = (eEstadoDocumento)db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == IdDocumento).FirstOrDefault().IdEstadoDocumento;
                        switch (IdEstadoDocumento)
                        {
                            case eEstadoDocumento.Aprobando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Cargando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Certificado:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Certificando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                break;
                            case eEstadoDocumento.PorAprobar:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.PorCertificar:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                break;
                        }
                    }
                    else
                    {
                        SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                        SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                    }
                }
            }

            return SubModulos;
        }
        public static FirstModuloSelected GetFirstUserModulo(long IdModuloPadre)
        {
            long IdUsuario = long.Parse(Session["UserId"].ToString());
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string _IdModulo = IdModuloPadre.ToString();
            int _IdCodigoModulo = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));
            FirstModuloSelected firstModulo = null;

            using (Entities db = new Entities())
            {
                long IdNivelUsuario = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario).FirstOrDefault().IdNivelUsuario;

                firstModulo = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdCodigoModulo == _IdCodigoModulo && x.tblModulo.IdTipoElemento == 4)
                    .OrderBy(x => x.IdModulo)
                    .Select(x => new FirstModuloSelected
                    {
                        Action = x.tblModulo.Accion,
                        Controller = x.tblModulo.Controller,
                        IdModulo = x.IdModulo
                    })
                    .FirstOrDefault();
            }

            return firstModulo;
        }
        public static PersonaModel GetPersonaById(long IdPersona)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            PersonaModel Persona = new PersonaModel();

            using (Entities db = new Entities())
            {
                Persona = (from p in db.tblPersona
                           let CorreosPersona = p.tblPersonaCorreo.Select(x => new PersonaEmail
                           {
                               Email = x.Correo,
                               IdPersonaEmail = x.IdPersonaCorreo,
                               IdTipoEmail = x.IdTipoCorreo,
                               TipoEmail = x.tblTipoCorreo.tblCultura_TipoCorreo
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion
                           }).ToList()
                           let DireccionesPersona = p.tblPersonaDireccion.Select(x => new PersonaDireccion
                           {
                               CalleAvenida = x.CalleAvenida,
                               EdificioCasa = x.EdificioCasa,
                               IdCiudad = x.IdCiudad,
                               IdEstado = x.IdEstado,
                               IdPais = x.IdPais,
                               IdPersonaDireccion = x.IdPersonaDireccion,
                               IdTipoDireccion = x.IdTipoDireccion,
                               PisoNivel = x.PisoNivel,
                               TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion,
                                TorreAla = x.TorreAla,
                                Urbanizacion = x.Urbanizacion
                           }).ToList()
                           let TelefonosPersona = p.tblPersonaTelefono.Select(x => new PersonaTelefono
                           {
                               CodigoArea = x.CodigoArea,
                               Extension1 = x.Extension1,
                               Extension2 = x.Extension2,
                               IdPersonaTelefono = x.IdPersonaTelefono,
                               IdTipoTelefono = x.IdTipoTelefono,
                               TipoTelefono = x.tblTipoTelefono.tblCultura_TipoTelefono
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion
                           }).ToList()
                           where p.IdEmpresa == IdEmpresa && p.IdPersona == IdPersona
                           select new PersonaModel
                           {
                               Cargo = new CargoModel { IdCargo = p.tblCargo.IdCargo, NombreCargo = p.tblCargo.Descripcion },
                               CorreosElectronicos = CorreosPersona,
                               Direcciones = DireccionesPersona,
                               IdCargoPersona = p.IdCargo,
                               Identificacion = p.Identificacion,
                               IdPersona = p.IdPersona,
                               IdUnidadOrganizativaPersona = p.IdUnidadOrganizativa,
                               IdUsuario = p.IdUsuario,
                               Nombre = p.Nombre,
                               Telefonos = TelefonosPersona,
                               UnidadOrganizativa = new UnidadOrganizativaModel { IdUnidad = p.tblUnidadOrganizativa.IdUnidadOrganizativa, IdUnidadPadre = p.tblUnidadOrganizativa.IdUnidadPadre, NombreUnidadOrganizativa = p.tblUnidadOrganizativa.Nombre }
                           }).FirstOrDefault();
            }

            return Persona;
        }
        public static string GetNombreUnidadCompleto(long idUnidadOrganizativa)
        {
            string NombreCompleto = string.Empty;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            if (idUnidadOrganizativa > 0)
            {
                using (Entities db = new Entities())
                {
                    tblUnidadOrganizativa unidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == idUnidadOrganizativa).FirstOrDefault();
                    NombreCompleto = unidad.Nombre;

                    while (unidad.IdUnidadPadre != unidad.IdUnidadOrganizativa)
                    {
                        unidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == unidad.IdUnidadPadre).FirstOrDefault();
                        if (unidad != null)
                        {
                            NombreCompleto = unidad.Nombre + " / " + NombreCompleto;
                        }
                    }
                }
            }
            return NombreCompleto;
        }
        public static IList<PersonaModel> GetPersonas()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<PersonaModel> Personas = new List<PersonaModel>();

            using (Entities db = new Entities())
            {
                Personas = (from p in db.tblPersona
                           let CorreosPersona = p.tblPersonaCorreo.Select(x => new PersonaEmail
                           {
                               Email = x.Correo,
                               IdPersonaEmail = x.IdPersonaCorreo,
                               IdTipoEmail = x.IdTipoCorreo,
                               TipoEmail = x.tblTipoCorreo.tblCultura_TipoCorreo
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion
                           }).ToList()
                           let DireccionesPersona = p.tblPersonaDireccion.Select(x => new PersonaDireccion
                           {
                               CalleAvenida = x.CalleAvenida,
                               EdificioCasa = x.EdificioCasa,
                               IdCiudad = x.IdCiudad,
                               IdEstado = x.IdEstado,
                               IdPais = x.IdPais,
                               IdPersonaDireccion = x.IdPersonaDireccion,
                               IdTipoDireccion = x.IdTipoDireccion,
                               PisoNivel = x.PisoNivel,
                               TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion,
                               TorreAla = x.TorreAla,
                               Urbanizacion = x.Urbanizacion
                           }).ToList()
                           let TelefonosPersona = p.tblPersonaTelefono.Select(x => new PersonaTelefono
                           {
                               CodigoArea = x.CodigoArea,
                               Extension1 = x.Extension1,
                               Extension2 = x.Extension2,
                               IdPersonaTelefono = x.IdPersonaTelefono,
                               IdTipoTelefono = x.IdTipoTelefono,
                               TipoTelefono = x.tblTipoTelefono.tblCultura_TipoTelefono
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion
                           }).ToList()
                           where p.IdEmpresa == IdEmpresa
                           select new PersonaModel
                           {
                               Cargo = new CargoModel { IdCargo = p.tblCargo.IdCargo, NombreCargo = p.tblCargo.Descripcion },
                               CorreosElectronicos = CorreosPersona,
                               Direcciones = DireccionesPersona,
                               IdCargoPersona = p.IdCargo,
                               Identificacion = p.Identificacion,
                               IdPersona = p.IdPersona,
                               IdUnidadOrganizativaPersona = p.IdUnidadOrganizativa,
                               IdUsuario = p.IdUsuario,
                               Nombre = p.Nombre,
                               Telefonos = TelefonosPersona,
                               UnidadOrganizativa = new UnidadOrganizativaModel { IdUnidad = p.tblUnidadOrganizativa.IdUnidadOrganizativa, IdUnidadPadre = p.tblUnidadOrganizativa.IdUnidadPadre,  NombreUnidadOrganizativa = p.tblUnidadOrganizativa.Nombre }
                           }).OrderBy(x => x.Nombre).ToList();

            }

            Personas.Insert(0, new PersonaModel { IdPersona = 0, Nombre = Resources.BCMWebPublic.itemSelectValue });

            return Personas;
        }
        public static IList<UnidadOrganizativaModel> GetUnidades()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<UnidadOrganizativaModel> Unidades = new List<UnidadOrganizativaModel>();

            using (Entities db = new Entities())
            {
                Unidades = (from p in db.tblUnidadOrganizativa
                            where p.IdEmpresa == IdEmpresa
                            select new UnidadOrganizativaModel
                            {
                                IdUnidad =  p.IdUnidadOrganizativa,
                                IdUnidadPadre = p.IdUnidadPadre,
                                NombreUnidadOrganizativa = p.Nombre
                            }).OrderBy(x => x.NombreUnidadOrganizativa).ToList();

            }

            Unidades.Insert(0, new UnidadOrganizativaModel { IdUnidad = 0, NombreUnidadOrganizativa = Resources.BCMWebPublic.itemSelectValue });

            return Unidades;
        }
        public static IList<CargoModel> GetCargos()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<CargoModel> Cargos = new List<CargoModel>();

            using (Entities db = new Entities())
            {
                Cargos = (from p in db.tblCargo
                            where p.IdEmpresa == IdEmpresa
                            select new CargoModel
                            {
                                IdCargo = p.IdCargo,
                                NombreCargo = p.Descripcion
                            }).OrderBy(x => x.NombreCargo).ToList();

            }

            Cargos.Insert(0, new CargoModel { IdCargo = 0, NombreCargo = Resources.BCMWebPublic.itemSelectValue });

            return Cargos;
        }
        public static string GetPais(long IdPais)
        {
            string _Pais = string.Empty;

            using (Entities db = new Entities())
            {
                tblPais Pais = db.tblPais.Where(x => x.IdPais == IdPais).FirstOrDefault();
                if (Pais != null) 
                    _Pais = Pais.tblCultura_Pais.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Nombre;
            }

            return _Pais;
        }
        public static string GetEstado(long IdPais, long IdEstado)
        {
            string _Estado = string.Empty;

            using (Entities db = new Entities())
            {
                tblEstado Estado = db.tblEstado.Where(x => x.IdPais == IdPais && x.IdEstado == IdEstado).FirstOrDefault();
                if (Estado != null)
                    _Estado = Estado.tblCultura_Estado.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Nombre;
            }

            return _Estado;
        }
        public static string GetCiudad(long IdPais, long IdEstado, long IdCiudad)
        {
            string _Ciudad = string.Empty;

            using (Entities db = new Entities())
            {
                tblCiudad Ciudad = db.tblCiudad.Where(x => x.IdPais == IdPais && x.IdEstado == IdEstado && x.IdCiudad == IdCiudad).FirstOrDefault();
                if (Ciudad != null)
                    _Ciudad = Ciudad.tblCultura_Ciudad.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Nombre;
            }

            return _Ciudad;
        }
        public static string GetEstatusProceso(long IdEstatus)
        {
            string _Estatus = string.Empty;

            using (Entities db = new Entities())
            {
                tblEstadoProceso EstatusProceso = db.tblEstadoProceso.Where(x => x.IdEstadoProceso == IdEstatus).FirstOrDefault();
                if (EstatusProceso != null)
                    _Estatus = EstatusProceso.tblCultura_EstadoProceso.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion;
            }

            return _Estatus;
        }
        public static IList<TablaModel> GetTiposDireccion()
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblTipoDireccion.Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_TipoDireccion.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Descripcion,
                    Id = x.IdTipoDireccion
                }).ToList();
            }

            return data;
        }
        public static IList<TablaModel> GetTiposTelefono()
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblTipoTelefono.Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_TipoTelefono.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Descripcion,
                    Id = x.IdTipoTelefono
                }).ToList();
            }

            return data;
        }
        public static IList<TablaModel> GetTiposCorreo()
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblTipoCorreo.Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_TipoCorreo.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Descripcion,
                    Id = x.IdTipoCorreo
                }).ToList();
            }

            return data;
        }
        public static IList<TablaModel> GetPaises()
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblPais.Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_Pais.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Nombre,
                    Id = x.IdPais
                }).ToList();
            }

            return data;
        }
        public static IList<TablaModel> GetEstados(long IdPais)
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblEstado.Where(x => x.IdPais == IdPais).Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_Estado.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Nombre,
                    Id = x.IdEstado
                }).ToList();
            }

            return data;
        }
        public static IList<TablaModel> GetCiudades(long IdPais, long IdEstado)
        {
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblCiudad.Where(x => x.IdPais == IdPais && x.IdEstado == IdEstado)
                    .Select(x => new TablaModel
                    {
                        Descripcion = x.tblCultura_Ciudad.Where(c => c.Culture == Culture || c.Culture == "es-VE").FirstOrDefault().Nombre,
                        Id = x.IdCiudad
                    }).ToList();
            }

            return data;
        }
        public static CargoModel GetCargoById(long IdCargo)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            CargoModel data = new CargoModel();

            if (IdCargo > 0)
            {
                using (Entities db = new Entities())
                {
                    data = db.tblCargo.Where(x => x.IdEmpresa == IdEmpresa && x.IdCargo == IdCargo)
                        .Select(x => new CargoModel
                        {
                            IdCargo = x.IdCargo,
                            NombreCargo = x.Descripcion
                        }).FirstOrDefault();
                }
            }

            return data;
        }
        public static UnidadOrganizativaModel GetUnidadOrganizativaById(long idUnidad)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            UnidadOrganizativaModel data = new UnidadOrganizativaModel();

            if (idUnidad > 0)
            {
                using (Entities db = new Entities())
                {
                    data = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == idUnidad)
                        .Select(x => new UnidadOrganizativaModel
                        {
                            IdUnidad = x.IdUnidadOrganizativa,
                            IdUnidadPadre = x.IdUnidadPadre,
                            NombreUnidadOrganizativa = x.Nombre
                        }).FirstOrDefault();
                }
            }

            return data;
        }
        public static string GetModuloName(long IdModulo)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string _ModuloTitle;

            using (Entities db = new Entities())
            {
                tblModulo modulo = db.tblModulo.Where(x => x.IdEmpresa == IdEmpresa && x.IdModulo == IdModulo).FirstOrDefault();

                if (!string.IsNullOrEmpty(modulo.Titulo))
                {
                    _ModuloTitle = modulo.Titulo;
                }
                else
                {
                    _ModuloTitle = modulo.Nombre;
                }
            }

            return _ModuloTitle;
        }
        public static IEnumerable<PersonaEmail> GetCorreos()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPersona = 0;
            if (Session["IdPersona"] != null)
                IdPersona = long.Parse(Session["IdPersona"].ToString());
            List<PersonaEmail> Correos = new List<PersonaEmail>();

            using (Entities db = new Entities())
            {
                Correos = db.tblPersonaCorreo.Where(x => x.IdEmpresa == IdEmpresa && x.IdPersona == IdPersona)
                    .Select(x => new PersonaEmail
                    {
                        Email = x.Correo,
                        IdPersonaEmail = x.IdPersonaCorreo,
                        IdTipoEmail = x.IdTipoCorreo,
                        TipoEmail = x.tblTipoCorreo.tblCultura_TipoCorreo.Where(t => t.Culture == Culture || t.Culture == "es-VE").FirstOrDefault().Descripcion
                    }).ToList();
            }
            return Correos;
        }
        public static IEnumerable<PersonaTelefono> GetTelefonos()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPersona = 0;
            if (Session["IdPersona"] != null)
                IdPersona = long.Parse(Session["IdPersona"].ToString());
            List<PersonaTelefono> Telefonos = new List<PersonaTelefono>();

            using (Entities db = new Entities())
            {
                Telefonos = db.tblPersonaTelefono.Where(x => x.IdEmpresa == IdEmpresa && x.IdPersona == IdPersona)
                    .Select(x => new PersonaTelefono
                    {
                        CodigoArea = x.CodigoArea,
                        Extension1 = x.Extension1,
                        Extension2 = x.Extension2,
                        IdPersonaTelefono= x.IdPersonaTelefono,
                        IdTipoTelefono = x.IdTipoTelefono,
                        NroTelefono = x.NroTelefono,
                        TipoTelefono = x.tblTipoTelefono.tblCultura_TipoTelefono.Where(t => t.Culture == Culture || t.Culture == "es-VE").FirstOrDefault().Descripcion
                    }).ToList();
            }

            return Telefonos;
        }
        public static IEnumerable<PersonaDireccion> GetDirecciones()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPersona = 0;
            if (Session["IdPersona"] != null)
                IdPersona = long.Parse(Session["IdPersona"].ToString());
            List<PersonaDireccion> Direcciones = new List<PersonaDireccion>();

            using (Entities db = new Entities())
            {
                Direcciones = db.tblPersonaDireccion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPersona == IdPersona)
                    .Select(x => new PersonaDireccion
                    {
                        CalleAvenida = x.CalleAvenida,
                        EdificioCasa = x.EdificioCasa,
                        IdCiudad = x.IdCiudad,
                        IdEstado = x.IdEstado,
                        IdPais = x.IdPais,
                        IdPersonaDireccion = x.IdPersonaDireccion,
                        IdTipoDireccion = x.IdTipoDireccion,
                        PisoNivel = x.PisoNivel,
                        TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion.Where(t => t.Culture == Culture || t.Culture == "es-VE").FirstOrDefault().Descripcion,
                        TorreAla = x.TorreAla,
                        Urbanizacion = x.Urbanizacion
                    }).ToList();
            }

            return Direcciones;
        }
        public static byte[] GetContenidoDocumento(long IdModulo)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            byte[] data = new byte[] { };

            using (Entities db = new Entities())
            {
                
                tblDocumentoContenido datos = db.tblDocumentoContenido
                            .Where(x => x.IdEmpresa == IdEmpresa
                                        && x.IdDocumento == IdDocumento
                                        && x.IdTipoDocumento == IdTipoDocumento
                                        && x.IdSubModulo == IdModulo).FirstOrDefault();

                if (datos != null)
                {
                    data = (datos.ContenidoBin != null ? datos.ContenidoBin : null);
                }

            }

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);
            return data;
        }
        public static bool UpdateContenidoDocumento(long IdModulo, byte[] Contenido, eTipoAccion Accion)
        {
            bool Updated = false;
            string _IdModulo = IdModulo.ToString();
            long IdTipoDocumento = long.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            long IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            using (Entities db = new Entities())
            {
                tblDocumento dataDocumento = db.tblDocumento
                    .Where(x => x.IdEmpresa == IdEmpresa
                            && x.IdDocumento == IdDocumento
                            && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();

                if (dataDocumento == null)
                {
                    long IdUser = long.Parse(Session["UserId"].ToString());

                    tblPersona dataPersona = db.tblPersona
                        .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUser).FirstOrDefault();

                    dataDocumento = new tblDocumento
                    {
                        FechaCreacion = DateTime.UtcNow,
                        FechaEstadoDocumento = DateTime.UtcNow,
                        IdDocumento = 0,
                        IdEmpresa = IdEmpresa,
                        IdEstadoDocumento = 1,
                        IdTipoDocumento = IdTipoDocumento,
                        IdPersonaResponsable = 0,
                        RequiereCertificacion = true,
                        Negocios = (IdClaseDocumento == 1),
                        NroVersion = IdVersion,
                        NroDocumento = Metodos.GetNextNroDocumento(IdClaseDocumento, IdTipoDocumento),
                        VersionOriginal = Metodos.GetNextVersion(IdClaseDocumento, IdTipoDocumento, IdVersion)
                    };
                    db.tblDocumento.Add(dataDocumento);
                    db.SaveChanges();

                    IdDocumento = dataDocumento.IdDocumento;
                    Session["IdDocumento"] = IdDocumento;
                }


                tblDocumentoContenido docContenido = db.tblDocumentoContenido
                    .Where(x => x.IdEmpresa == IdEmpresa 
                            && x.IdDocumento == IdDocumento 
                            && x.IdTipoDocumento == IdTipoDocumento
                            && x.IdSubModulo == IdModulo).FirstOrDefault();

                if (docContenido == null)
                {
                    docContenido = new tblDocumentoContenido()
                    {
                        ContenidoBin = Contenido,
                        FechaCreacion = DateTime.UtcNow,
                        IdDocumento = IdDocumento,
                        IdEmpresa = IdEmpresa,
                        IdSubModulo = IdModulo,
                        IdTipoDocumento = IdTipoDocumento
                    };

                    db.tblDocumentoContenido.Add(docContenido);
                }
                else
                {
                    docContenido.ContenidoBin = Contenido;
                }

                dataDocumento.FechaUltimaModificacion = DateTime.UtcNow;

                db.SaveChanges();
                Updated = true;
            }

            if (Updated) Auditoria.RegistarAccion(Accion);
            return Updated;
        }
        public static void LoginUsuario(long UserId)
        {
            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == UserId).FirstOrDefault();
                usuario.EstadoUsuario = (int)eEstadoUsuario.Conectado;
                usuario.FechaEstado = DateTime.UtcNow;
                usuario.FechaUltimaConexion = DateTime.UtcNow;
                db.SaveChanges();
            }

            Auditoria.RegistrarLogin();
        }
        public static void Logout(long UserId)
        {
            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == UserId).FirstOrDefault();
                usuario.EstadoUsuario = (int)eEstadoUsuario.Activo;
                usuario.FechaEstado = DateTime.UtcNow;
                usuario.FechaUltimaConexion = DateTime.UtcNow;
                db.SaveChanges();
            }

            Auditoria.RegistrarLogout(UserId);
        }
        public static void openPDF()
        {
            eTipoAccion Accion = eTipoAccion.AbrirPDFWeb;

            if (RequestExtensions.IsMobileBrowser(HttpContext.Current.Request))
            {
                Accion = eTipoAccion.AbrirPDFMovil;
            }

            Auditoria.RegistarAccion(Accion);
        }
        public static List<AuditoriaModel> GetControlCambios()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            long IdModulo = IdTipoDocumento * 1000000;
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas)  *60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            List<AuditoriaModel> data = new List<AuditoriaModel>();

            using (Entities db = new Entities())
            {
                List<tblAuditoria> regAuditoria = (from d in db.tblAuditoria
                                                   where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento
                                                   select d).OrderByDescending(x => x.FechaRegistro).ToList();

                tblDocumento tblDocumento = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == IdTipoDocumento && x.IdDocumento == IdDocumento).FirstOrDefault();

                foreach (tblAuditoria d in regAuditoria)
                {
                    data.Add(new AuditoriaModel
                            {
                                Accion = d.Accion,
                                DireccionIP = d.DireccionIP,
                                Empresa = d.tblEmpresa.NombreComercial,
                                FechaRegistro = (DateTime)((DateTime)d.FechaRegistro).AddMinutes(Minutos),
                                Id = d.IdAuditoria,
                                EditDocumento = false,
                                IdDocumento = (long)d.IdDocumento,
                                IdEmpresa = (long)d.IdEmpresa,
                                IdModulo = 0,
                                IdModuloActual = 0,
                                IdTipoDocumento = (long)d.IdTipoDocumento,
                                IdUsuario = (long)d.IdUsuario,
                                Mensaje = d.Mensaje,
                                NombreUsuario = d.tblUsuario.Nombre,
                                NroDocumento = tblDocumento.NroDocumento,
                                TipoDocumento = string.Empty,
                            });
                }
            }
            return data;
        }
        public static void IniciarAprobacion()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            long IdModulo = IdTipoDocumento * 1000000;
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            using (Entities db = new Entities())
            {
                tblDocumento documento = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == IdDocumento && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();
                documento.IdEstadoDocumento = (int)eEstadoDocumento.PorAprobar;
                documento.FechaEstadoDocumento = DateTime.UtcNow;
                documento.FechaUltimaModificacion = DateTime.UtcNow;
                db.SaveChanges();
            }

            Auditoria.RegistarAccion(eTipoAccion.IniciarAprobacion);
        }
        public static List<DocumentoAprobacionModel> GetAprobaciones()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            long IdModulo = IdTipoDocumento * 1000000;
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            List<DocumentoAprobacionModel> Aprobaciones = new List<DocumentoAprobacionModel>();
            using (Entities db = new Entities())
            {

                tblDocumento documento = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                                && x.IdDocumento == IdDocumento
                                                                && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();

                if (documento == null || documento.tblDocumentoAprobacion == null || documento.tblDocumentoAprobacion.Count == 0)
                {
                    List<tblDocumentoAprobacion> regAprobaciones = new List<tblDocumentoAprobacion>();
                    List<tblEmpresaUsuario> data = new List<tblEmpresaUsuario>();

                    var NivelesUsuario = new long[] { 4, 6 };
                    data = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && NivelesUsuario.Contains(x.IdNivelUsuario)).ToList();

                    foreach (tblEmpresaUsuario d in data)
                    {
                        tblDocumentoAprobacion reg = new tblDocumentoAprobacion
                        {
                            Aprobado = false,
                            Fecha = null,
                            IdDocumento = IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdPersona = (d.tblUsuario.tblPersona != null && d.tblUsuario.tblPersona.Count > 0 ? d.tblUsuario.tblPersona.FirstOrDefault().IdPersona : d.IdUsuario),
                            IdTipoDocumento = IdTipoDocumento,
                            Procesado = false,
                        };

                        if (!regAprobaciones.Contains(reg))
                            regAprobaciones.Add(reg);
                    };
                    db.tblDocumentoAprobacion.AddRange(regAprobaciones);
                    db.SaveChanges();
                }
                
                Aprobaciones = db.tblDocumentoAprobacion.Select(x => new DocumentoAprobacionModel
                {
                    Aprobado = (bool)x.Aprobado,
                    FechaAprobacion = (x.Fecha != null ? (DateTime)DbFunctions.AddMinutes(x.Fecha, Minutos) : x.Fecha),
                    IdAprobacion = x.IdAprobacion,
                    IdDocumento = x.IdDocumento,
                    IdEmpresa = x.IdEmpresa,
                    IdPersona =(long)x.IdPersona,
                    IdTipoDocumento = x.IdTipoDocumento,
                    Procesado = x.Procesado,
                    Responsable = (x.IdPersona == x.tblDocumento.IdPersonaResponsable)
                }).ToList();
            }

            return Aprobaciones;
        }
        public static List<DocumentoCertificacionModel> GetCertificaciones()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            long IdModulo = IdTipoDocumento * 1000000;
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            List<DocumentoCertificacionModel> Certificaciones = new List<DocumentoCertificacionModel>();
            using (Entities db = new Entities())
            {

                tblDocumento documento = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                                && x.IdDocumento == IdDocumento
                                                                && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();

                if (documento == null || documento.tblDocumentoCertificacion == null || documento.tblDocumentoCertificacion.Count == 0)
                {
                    List<tblDocumentoCertificacion> regCertificaciones = new List<tblDocumentoCertificacion>();
                    List<tblEmpresaUsuario> data = new List<tblEmpresaUsuario>();

                    var NivelesUsuario = new long[] { 4, 6 };
                    data = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && NivelesUsuario.Contains(x.IdNivelUsuario)).ToList();

                    foreach (tblEmpresaUsuario d in data)
                    {
                        tblDocumentoCertificacion reg = new tblDocumentoCertificacion
                        {
                            Certificado = false,
                            Fecha = null,
                            IdDocumento = IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdPersona = (d.tblUsuario.tblPersona != null && d.tblUsuario.tblPersona.Count > 0 ? d.tblUsuario.tblPersona.FirstOrDefault().IdPersona : d.IdUsuario),
                            IdTipoDocumento = IdTipoDocumento,
                            Procesado = false,
                        };

                        if (!regCertificaciones.Contains(reg))
                            regCertificaciones.Add(reg);
                    };
                    db.tblDocumentoCertificacion.AddRange(regCertificaciones);
                    db.SaveChanges();
                }

                Certificaciones = db.tblDocumentoCertificacion.Select(x => new DocumentoCertificacionModel
                {
                    Certificado = (bool)x.Certificado,
                    FechaCertificacion = (x.Fecha != null ? (DateTime)DbFunctions.AddMinutes(x.Fecha, Minutos) : x.Fecha),
                    IdCertificacion = x.IdCertificacion,
                    IdDocumento = x.IdDocumento,
                    IdEmpresa = x.IdEmpresa,
                    IdPersona = (long)x.IdPersona,
                    IdTipoDocumento = x.IdTipoDocumento,
                    Procesado = x.Procesado,
                    Responsable = (x.IdPersona == x.tblDocumento.IdPersonaResponsable)
                }).ToList();
            }

            return Certificaciones;
        }
        public static void AprobarDocumento(long idDocumento, long idTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPersona = Metodos.GetUser_IdPersona();

            if (IdPersona == 0)
                IdPersona = long.Parse(Session["UserId"].ToString());

            using (Entities db = new Entities())
            {
                tblDocumento documento = db.tblDocumento
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento)
                    .FirstOrDefault();

                tblDocumentoAprobacion regAprobacion = db.tblDocumentoAprobacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento && x.IdPersona == IdPersona)
                    .FirstOrDefault();

                regAprobacion.Aprobado = true;
                regAprobacion.Procesado = true;
                regAprobacion.Fecha = DateTime.UtcNow;
                db.SaveChanges();

                int AprobacionPendiente = db.tblDocumentoAprobacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento && !(bool)x.Aprobado).Count();

                if (AprobacionPendiente == 0)
                {
                    documento.IdEstadoDocumento = (int)eEstadoDocumento.PorCertificar;
                }
                else if ((eEstadoDocumento)documento.IdEstadoDocumento == eEstadoDocumento.PorAprobar)
                {
                    documento.IdEstadoDocumento = (int)eEstadoDocumento.Aprobando;
                }
                documento.FechaEstadoDocumento = DateTime.UtcNow;
                documento.FechaUltimaModificacion = DateTime.UtcNow;
                db.SaveChanges();
            }
            Auditoria.RegistarAccion(eTipoAccion.AprobarDocumento);
        }
        public static void CertificarDocumento(long idDocumento, long idTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPersona = Metodos.GetUser_IdPersona();
            if (IdPersona == 0)
                IdPersona = long.Parse(Session["UserId"].ToString());

            using (Entities db = new Entities())
            {
                tblDocumento documento = db.tblDocumento
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento)
                    .FirstOrDefault();

                tblDocumentoCertificacion regCertificacion = db.tblDocumentoCertificacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento && x.IdPersona == IdPersona)
                    .FirstOrDefault();

                regCertificacion.Certificado = true;
                regCertificacion.Procesado = true;
                regCertificacion.Fecha = DateTime.UtcNow;
                db.SaveChanges();

                int CertificacionPendiente = db.tblDocumentoCertificacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == idDocumento
                            && x.IdTipoDocumento == idTipoDocumento && !(bool)x.Certificado).Count();

                if (CertificacionPendiente == 0)
                {
                    documento.IdEstadoDocumento = (int)eEstadoDocumento.Certificado;
                }
                else if ((eEstadoDocumento)documento.IdEstadoDocumento == eEstadoDocumento.PorCertificar)
                {
                    documento.IdEstadoDocumento = (int)eEstadoDocumento.Certificando;
                }
                documento.FechaEstadoDocumento = DateTime.UtcNow;
                documento.FechaUltimaModificacion = DateTime.UtcNow;
                db.SaveChanges();
            }
            Auditoria.RegistarAccion(eTipoAccion.CertificarDocumento);
        }
        public static long GetUser_IdPersona()
        {
            long UserId = long.Parse(Session["UserId"].ToString());
            long IdPersona = 0;
            
            using (Entities db = new Entities())
            {
                tblPersona persona = db.tblPersona.Where(x => x.IdUsuario == UserId).FirstOrDefault();

                if (persona != null) IdPersona = persona.IdPersona;
            }

            return IdPersona;
        }
        public static PersonaModel ConvertUsuarioToPersona(long IdUsuario)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            PersonaModel Persona = new PersonaModel();

            using (Entities db = new Entities())
            {
                Persona = (from u in db.tblUsuario
                           where u.IdUsuario == IdUsuario
                           select new PersonaModel
                           {
                               Identificacion = string.Empty,
                               IdPersona = u.IdUsuario,
                               IdUsuario = u.IdUsuario,
                               Nombre = u.Nombre,
                           }).FirstOrDefault();
            }

            return Persona;
        }
        public static long GenerarNuevaVersionDocumento(long IdDocumento, long IdVersionActual, long IdNroVersion)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoDocumento = long.Parse(Session["IdTipoDocumento"].ToString());
            eSystemModules Modulo = (eSystemModules)IdTipoDocumento;
            long IdNuevoDocumento = 0;

            using (Entities db = new Entities())
            {
                tblDocumento documentoActual = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == IdDocumento && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();

                if (documentoActual != null)
                {
                    tblDocumento nuevoDocumento = new tblDocumento
                    {
                        FechaCreacion = DateTime.UtcNow,
                        FechaEstadoDocumento = DateTime.UtcNow,
                        FechaUltimaModificacion = DateTime.UtcNow,
                        IdEmpresa = IdEmpresa,
                        IdEstadoDocumento = 1,
                        IdPersonaResponsable = documentoActual.IdPersonaResponsable,
                        IdTipoDocumento = documentoActual.IdTipoDocumento,
                        Negocios = documentoActual.Negocios,
                        NroDocumento = documentoActual.NroDocumento,
                        NroVersion = documentoActual.NroVersion + 1,
                        RequiereCertificacion = documentoActual.RequiereCertificacion,
                        VersionOriginal = documentoActual.VersionOriginal
                    };

                    db.tblDocumento.Add(nuevoDocumento);

                    foreach (tblDocumentoAnexo anexo in documentoActual.tblDocumentoAnexo)
                    {
                        tblDocumentoAnexo nuevoAnexo = new tblDocumentoAnexo
                        {
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdTipoDocumento = IdTipoDocumento,
                            Nombre = anexo.Nombre,
                            Ruta = anexo.Ruta,
                        };
                        db.tblDocumentoAnexo.Add(nuevoAnexo);
                    }
                    foreach (tblDocumentoAprobacion aprobacion in documentoActual.tblDocumentoAprobacion)
                    {
                        tblDocumentoAprobacion nuevaAprobacion = new tblDocumentoAprobacion
                        {
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdPersona = aprobacion.IdPersona,
                            IdTipoDocumento = IdTipoDocumento,
                            Procesado = false,
                        };
                        db.tblDocumentoAprobacion.Add(nuevaAprobacion);
                    }
                    foreach (tblDocumentoCertificacion aprobacion in documentoActual.tblDocumentoCertificacion)
                    {
                        tblDocumentoCertificacion nuevaCertificacion = new tblDocumentoCertificacion
                        {
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdPersona = aprobacion.IdPersona,
                            IdTipoDocumento = IdTipoDocumento,
                            Procesado = false,
                        };
                        db.tblDocumentoCertificacion.Add(nuevaCertificacion);
                    }
                    foreach (tblDocumentoContenido contenido in documentoActual.tblDocumentoContenido)
                    {
                        tblDocumentoContenido nuevoContenido = new tblDocumentoContenido
                        {
                            ContenidoBin = contenido.ContenidoBin,
                            FechaCreacion = DateTime.UtcNow,
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdSubModulo = contenido.IdSubModulo,
                            IdTipoDocumento = IdTipoDocumento
                        };
                        db.tblDocumentoContenido.Add(nuevoContenido);
                    }
                    foreach (tblDocumentoEntrevista entrevista in documentoActual.tblDocumentoEntrevista)
                    {
                        tblDocumentoEntrevista nuevaEntrevista = new tblDocumentoEntrevista
                        {
                            FechaFinal = entrevista.FechaFinal,
                            FechaInicio = entrevista.FechaInicio,
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdTipoDocumento = IdTipoDocumento,
                        };
                        db.tblDocumentoEntrevista.Add(nuevaEntrevista);

                        foreach (tblDocumentoEntrevistaPersona entrevistaPersonas in entrevista.tblDocumentoEntrevistaPersona)
                        {
                            tblDocumentoEntrevistaPersona nuevaEntrevistaPersona = new tblDocumentoEntrevistaPersona
                            {
                                Empresa = entrevistaPersonas.Empresa,
                                EsEntrevistador = entrevistaPersonas.EsEntrevistador,
                                IdDocumento = nuevoDocumento.IdDocumento,
                                IdEmpresa = IdEmpresa,
                                IdEntrevista = nuevaEntrevista.IdEntrevista,
                                IdTipoDocumento = IdTipoDocumento,
                                Nombre = entrevistaPersonas.Nombre,
                            };
                            db.tblDocumentoEntrevistaPersona.Add(nuevaEntrevistaPersona);
                        }
                    }
                    foreach (tblDocumentoPersonaClave personaClave in documentoActual.tblDocumentoPersonaClave)
                    {
                        tblDocumentoPersonaClave nuevaPersonaClave = new tblDocumentoPersonaClave
                        {
                            Cedula = personaClave.Cedula,
                            Correo = personaClave.Correo,
                            DireccionHabitacion = personaClave.DireccionHabitacion,
                            IdDocumento = nuevoDocumento.IdDocumento,
                            IdEmpresa = IdEmpresa,
                            IdTipoDocumento = IdTipoDocumento,
                            Nombre = personaClave.Nombre,
                            Principal = personaClave.Principal,
                            TelefonoCelular = personaClave.TelefonoCelular,
                            TelefonoHabitacion = personaClave.TelefonoHabitacion,
                            TelefonoOficina = personaClave.TelefonoOficina
                        };
                        db.tblDocumentoPersonaClave.Add(nuevaPersonaClave);
                    }

                    switch (Modulo)
                    {
                        case eSystemModules.BCP:
                            tblBCPDocumento documentoBCP = documentoActual.tblBCPDocumento.FirstOrDefault();
                            tblBCPDocumento nuevoBCP = new tblBCPDocumento
                            {
                                IdDocumento = nuevoDocumento.IdDocumento,
                                IdDocumentoBIA = documentoBCP.IdDocumentoBIA,
                                IdEmpresa = IdEmpresa,
                                IdProceso = documentoBCP.IdProceso,
                                IdTipoDocumento = IdTipoDocumento,
                                IdUnidadOrganizativa = documentoBCP.IdUnidadOrganizativa,
                                Proceso = documentoBCP.Proceso,
                                Responsable = documentoBCP.Responsable,
                                SubProceso = documentoBCP.SubProceso,
                            };
                            db.tblBCPDocumento.Add(nuevoBCP);

                            foreach (tblBCPReanudacionPersonaClave RPC in documentoBCP.tblBCPReanudacionPersonaClave)
                            {
                                tblBCPReanudacionPersonaClave newRPC = new tblBCPReanudacionPersonaClave
                                {
                                    Actividad = RPC.Actividad,
                                    Cedula = RPC.Cedula,
                                    Correo = RPC.Correo,
                                    DireccionHabitacion = RPC.DireccionHabitacion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    IdPersonaClavePrincipal = RPC.IdPersonaClavePrincipal,
                                    Nombre = RPC.Nombre,
                                    TelefonoCelular = RPC.TelefonoCelular,
                                    TelefonoHabitacion = RPC.TelefonoHabitacion,
                                    TelefonoOficina = RPC.TelefonoOficina
                                };
                                db.tblBCPReanudacionPersonaClave.Add(newRPC);
                            }
                            foreach (tblBCPReanudacionTarea Tarea in documentoBCP.tblBCPReanudacionTarea)
                            {
                                tblBCPReanudacionTarea newTarea = new tblBCPReanudacionTarea
                                {
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    Nombre = Tarea.Nombre
                                };
                                db.tblBCPReanudacionTarea.Add(newTarea);

                                foreach (tblBCPReanudacionTareaActividad TareaActividad in Tarea.tblBCPReanudacionTareaActividad)
                                {
                                    tblBCPReanudacionTareaActividad newTareaActividad = new tblBCPReanudacionTareaActividad
                                    {
                                        Descripcion = TareaActividad.Descripcion,
                                        IdBCPReanudacionTarea = newTarea.IdBCPReanudacionTarea,
                                        IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                        IdEmpresa = IdEmpresa,
                                        Procesado = TareaActividad.Procesado,
                                    };
                                    db.tblBCPReanudacionTareaActividad.Add(newTareaActividad);
                                }
                            }

                            foreach (tblBCPRecuperacionPersonaClave RPC in documentoBCP.tblBCPRecuperacionPersonaClave)
                            {
                                tblBCPRecuperacionPersonaClave newRPC = new tblBCPRecuperacionPersonaClave
                                {
                                    Actividad = RPC.Actividad,
                                    Cedula = RPC.Cedula,
                                    Correo = RPC.Correo,
                                    DireccionHabitacion = RPC.DireccionHabitacion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    IdPersonaClavePrincipal = RPC.IdPersonaClavePrincipal,
                                    Nombre = RPC.Nombre,
                                    TelefonoCelular = RPC.TelefonoCelular,
                                    TelefonoHabitacion = RPC.TelefonoHabitacion,
                                    TelefonoOficina = RPC.TelefonoOficina
                                };
                                db.tblBCPRecuperacionPersonaClave.Add(newRPC);
                            }

                            foreach (tblBCPRecuperacionRecurso RecRecurso in documentoBCP.tblBCPRecuperacionRecurso)
                            {
                                tblBCPRecuperacionRecurso newRecRecurso = new tblBCPRecuperacionRecurso
                                {
                                    Cantidad = RecRecurso.Cantidad,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    Nombre = RecRecurso.Nombre
                                };
                                db.tblBCPRecuperacionRecurso.Add(newRecRecurso);
                            }
                            foreach (tblBCPRespuestaAccion reg in documentoBCP.tblBCPRespuestaAccion)
                            {
                                tblBCPRespuestaAccion newReg = new tblBCPRespuestaAccion
                                {
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    IdPersona = reg.IdPersona,
                                    NivelEscala = reg.NivelEscala,
                                    NombreEscala = reg.NombreEscala
                                };
                                db.tblBCPRespuestaAccion.Add(reg);
                            }
                            foreach (tblBCPRespuestaRecurso reg in documentoBCP.tblBCPRespuestaRecurso)
                            {
                                tblBCPRespuestaRecurso newReg = new tblBCPRespuestaRecurso
                                {
                                    Cantidad = reg.Cantidad,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa,
                                    Nombre = reg.Nombre
                                };
                                db.tblBCPRespuestaRecurso.Add(reg);
                            }
                            foreach (tblBCPRestauracionEquipo reg in documentoBCP.tblBCPRestauracionEquipo)
                            {
                                tblBCPRestauracionEquipo newReg = new tblBCPRestauracionEquipo
                                {
                                    Cantidad = reg.Cantidad,
                                    Descripcion = reg.Descripcion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa
                                };
                                db.tblBCPRestauracionEquipo.Add(newReg);
                            }
                            foreach (tblBCPRestauracionInfraestructura reg in documentoBCP.tblBCPRestauracionInfraestructura)
                            {
                                tblBCPRestauracionInfraestructura newReg = new tblBCPRestauracionInfraestructura
                                {
                                    Cantidad = reg.Cantidad,
                                    Descripcion = reg.Descripcion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa
                                };
                                db.tblBCPRestauracionInfraestructura.Add(newReg);
                            }
                            foreach (tblBCPRestauracionMobiliario reg in documentoBCP.tblBCPRestauracionMobiliario)
                            {
                                tblBCPRestauracionMobiliario newReg = new tblBCPRestauracionMobiliario
                                {
                                    Cantidad = reg.Cantidad,
                                    Descripcion = reg.Descripcion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa
                                };
                                db.tblBCPRestauracionMobiliario.Add(newReg);
                            }
                            foreach (tblBCPRestauracionOtro reg in documentoBCP.tblBCPRestauracionOtro)
                            {
                                tblBCPRestauracionOtro newReg = new tblBCPRestauracionOtro
                                {
                                    Cantidad = reg.Cantidad,
                                    Descripcion = reg.Descripcion,
                                    IdDocumentoBCP = nuevoBCP.IdDocumentoBCP,
                                    IdEmpresa = IdEmpresa
                                };
                                db.tblBCPRestauracionOtro.Add(newReg);
                            }
                            break;
                        case eSystemModules.BIA:
                            tblBIADocumento docBIA = documentoActual.tblBIADocumento.FirstOrDefault();
                            tblBIADocumento newBIA = new tblBIADocumento
                            {
                                IdCadenaServicio = docBIA.IdCadenaServicio,
                                IdDocumento = nuevoDocumento.IdDocumento,
                                IdEmpresa = IdEmpresa,
                                IdTipoDocumento = IdTipoDocumento,
                                IdUnidadOrganizativa = docBIA.IdUnidadOrganizativa,
                            };
                            db.tblBIADocumento.Add(newBIA);

                            foreach (tblBIAComentario comentario in docBIA.tblBIAComentario)
                            {
                                tblBIAComentario newComentario = new tblBIAComentario
                                {
                                    Descripcion = comentario.Descripcion,
                                    IdDocumentoBia = newBIA.IdDocumentoBIA,
                                    IdEmpresa = IdEmpresa,
                                };
                                db.tblBIAComentario.Add(newComentario);
                            }
                            foreach (tblBIAProceso proceso in docBIA.tblBIAProceso)
                            {
                                tblBIAProceso newProceso = new tblBIAProceso
                                {
                                    Critico = proceso.Critico,
                                    Descripcion = proceso.Descripcion,
                                    FechaCreacion = proceso.FechaCreacion,
                                    FechaUltimoEstatus = proceso.FechaUltimoEstatus,
                                    IdDocumentoBia = newBIA.IdDocumentoBIA,
                                    IdEmpresa = IdEmpresa,
                                    IdEstadoProceso = proceso.IdEstadoProceso,
                                    IdUnidadOrganizativa = proceso.IdUnidadOrganizativa,
                                    Nombre = proceso.Nombre,
                                    NroProceso = proceso.NroProceso
                                };
                                db.tblBIAProceso.Add(newProceso);

                                foreach (tblBIAAmenaza amenaza in proceso.tblBIAAmenaza)
                                {
                                    tblBIAAmenaza newAmenaza = new tblBIAAmenaza
                                    {
                                        Descripcion = amenaza.Descripcion,
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                    };
                                    db.tblBIAAmenaza.Add(newAmenaza);

                                    foreach (tblBIAAmenazaEvento regEvento in amenaza.tblBIAAmenazaEvento)
                                    {
                                        tblBIAAmenazaEvento newEvento = new tblBIAAmenazaEvento
                                        {
                                            IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                            IdEmpresa = IdEmpresa,
                                            IdProceso = newProceso.IdProceso,
                                            IdAmenaza = newAmenaza.IdAmenaza,
                                            IdEventoRiesgo = regEvento.IdEventoRiesgo,
                                            Nombre = regEvento.Nombre,
                                        };
                                        db.tblBIAAmenazaEvento.Add(newEvento);

                                        foreach (tblBIAEventoControl control in regEvento.tblBIAEventoControl)
                                        {
                                            tblBIAEventoControl newReg = new tblBIAEventoControl
                                            {
                                                Descripcion = control.Descripcion,
                                                IdAmenaza = newAmenaza.IdAmenaza,
                                                IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                                IdEmpresa = IdEmpresa,
                                                IdAmenazaEvento = newEvento.IdAmenazaEvento,
                                                IdProceso = newProceso.IdProceso,
                                                Implantado = control.Implantado
                                            };
                                            db.tblBIAEventoControl.Add(newReg);
                                        }
                                    }
                                }
                                foreach (tblBIAAplicacion reg in proceso.tblBIAAplicacion)
                                {
                                    tblBIAAplicacion newReg = new tblBIAAplicacion
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Nombre = reg.Nombre,
                                        Usuario = reg.Usuario
                                    };
                                    db.tblBIAAplicacion.Add(newReg);
                                }
                                foreach (tblBIAUnidadTrabajoProceso reg in proceso.tblBIAUnidadTrabajoProceso)
                                {
                                    tblBIAUnidadTrabajoProceso newReg = new tblBIAUnidadTrabajoProceso
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdUnidadTrabajo = reg.IdUnidadTrabajo,
                                        Nombre = reg.Nombre
                                    };
                                    db.tblBIAUnidadTrabajoProceso.Add(newReg);

                                    foreach (tblBIAUnidadTrabajoPersonas UTP in reg.tblBIAUnidadTrabajoPersonas)
                                    {
                                        tblBIAClienteProceso Clte = new tblBIAClienteProceso
                                        {
                                            IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                            IdEmpresa = IdEmpresa,
                                            IdProceso = newProceso.IdProceso,
                                            Proceso = UTP.tblBIAClienteProceso.Proceso,
                                            Producto = UTP.tblBIAClienteProceso.Producto,
                                            Responsable = UTP.tblBIAClienteProceso.Responsable,
                                            Unidad = UTP.tblBIAClienteProceso.Unidad
                                        };
                                        db.tblBIAClienteProceso.Add(Clte);

                                        tblBIAUnidadTrabajoPersonas newUTP = new tblBIAUnidadTrabajoPersonas
                                        {
                                            IdClienteProceso = Clte.IdClienteProceso,
                                            IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                            IdEmpresa = IdEmpresa,
                                            IdProceso = newProceso.IdProceso,
                                            IdUnidadTrabajo = UTP.IdUnidadTrabajo,
                                            IdUnidadTrabajoProceso = newReg.IdUnidadTrabajoProceso,
                                            Nombre = UTP.Nombre
                                        };
                                        db.tblBIAUnidadTrabajoPersonas.Add(newUTP);
                                    }
                                }
                                foreach (tblBIADocumentacion reg in proceso.tblBIADocumentacion)
                                {
                                    tblBIADocumentacion newReg = new tblBIADocumentacion
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Nombre = reg.Nombre,
                                        Ubicacion = reg.Ubicacion
                                    };
                                    db.tblBIADocumentacion.Add(newReg);
                                }
                                foreach (tblBIAEntrada entrada in proceso.tblBIAEntrada)
                                {
                                    tblBIAEntrada newEntrada = new tblBIAEntrada
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Evento = entrada.Evento,
                                        Responsable = entrada.Responsable,
                                        Unidad = entrada.Unidad
                                    };
                                    db.tblBIAEntrada.Add(newEntrada);
                                }
                                foreach (tblBIAGranImpacto reg in proceso.tblBIAGranImpacto)
                                {
                                    tblBIAGranImpacto newReg = new tblBIAGranImpacto
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Explicacion = reg.Explicacion,
                                        IdMes = reg.IdMes,
                                        Observacion = reg.Observacion
                                    };
                                    db.tblBIAGranImpacto.Add(newReg);
                                }
                                foreach (tblBIAImpactoFinanciero reg in proceso.tblBIAImpactoFinanciero)
                                {
                                    tblBIAImpactoFinanciero newReg = new tblBIAImpactoFinanciero
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        IdImpactoFinanciero = reg.IdImpactoFinanciero
                                    };
                                    db.tblBIAImpactoFinanciero.Add(newReg);
                                }
                                foreach (tblBIAImpactoOperacional reg in proceso.tblBIAImpactoOperacional)
                                {
                                    tblBIAImpactoOperacional newReg = new tblBIAImpactoOperacional
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Descripcion = reg.Descripcion,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        ImpactoOperacional = reg.ImpactoOperacional
                                    };
                                    db.tblBIAImpactoOperacional.Add(newReg);
                                }
                                foreach (tblBIAInterdependencia reg in proceso.tblBIAInterdependencia)
                                {
                                    tblBIAInterdependencia newReg = new tblBIAInterdependencia
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Contacto = reg.Contacto,
                                        Organizacion = reg.Organizacion,
                                        Servicio = reg.Servicio
                                    };
                                    db.tblBIAInterdependencia.Add(newReg);
                                }
                                foreach (tblBIAMTD reg in proceso.tblBIAMTD)
                                {
                                    tblBIAMTD newReg = new tblBIAMTD
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        Observacion = reg.Observacion
                                    };
                                    db.tblBIAMTD.Add(newReg);
                                }
                                foreach (tblBIAPersonaRespaldoProceso reg in proceso.tblBIAPersonaRespaldoProceso)
                                {
                                    tblBIAPersonaRespaldoProceso newReg = new tblBIAPersonaRespaldoProceso
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdPersona = reg.IdPersona
                                    };
                                    db.tblBIAPersonaRespaldoProceso.Add(newReg);
                                }
                                foreach (tblBIAProcesoAlterno reg in proceso.tblBIAProcesoAlterno)
                                {
                                    tblBIAProcesoAlterno newReg = new tblBIAProcesoAlterno
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        ProcesoAlterno = reg.ProcesoAlterno
                                    };
                                    db.tblBIAProcesoAlterno.Add(newReg);
                                }
                                foreach (tblBIAProveedor reg in proceso.tblBIAProveedor)
                                {
                                    tblBIAProveedor newReg = new tblBIAProveedor
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Contacto = reg.Contacto,
                                        Organizacion = reg.Organizacion,
                                        Servicio = reg.Servicio
                                    };
                                    db.tblBIAProveedor.Add(newReg);
                                }
                                foreach (tblBIARespaldoPrimario reg in proceso.tblBIARespaldoPrimario)
                                {
                                    tblBIARespaldoPrimario newReg = new tblBIARespaldoPrimario
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Ubicacion = reg.Ubicacion
                                    };
                                    db.tblBIARespaldoPrimario.Add(newReg);
                                }
                                foreach (tblBIARespaldoSecundario reg in proceso.tblBIARespaldoSecundario)
                                {
                                    tblBIARespaldoSecundario newReg = new tblBIARespaldoSecundario
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        Ubicacion = reg.Ubicacion
                                    };
                                    db.tblBIARespaldoSecundario.Add(newReg);
                                }
                                foreach (tblBIARPO reg in proceso.tblBIARPO)
                                {
                                    tblBIARPO newReg = new tblBIARPO
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        Observacion = reg.Observacion
                                    };
                                    db.tblBIARPO.Add(newReg);
                                }
                                foreach (tblBIARTO reg in proceso.tblBIARTO)
                                {
                                    tblBIARTO newReg = new tblBIARTO
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        Observacion = reg.Observacion
                                    };
                                    db.tblBIARTO.Add(newReg);
                                }
                                foreach (tblBIAWRT reg in proceso.tblBIAWRT)
                                {
                                    tblBIAWRT newReg = new tblBIAWRT
                                    {
                                        IdDocumentoBIA = newBIA.IdDocumentoBIA,
                                        IdEmpresa = IdEmpresa,
                                        IdProceso = newProceso.IdProceso,
                                        IdEscala = reg.IdEscala,
                                        IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                        Observacion = reg.Observacion
                                    };
                                    db.tblBIAWRT.Add(newReg);
                                }
                            }
                            break;
                        case eSystemModules.PMT:
                            foreach (tblPMTFrecuencia reg in documentoActual.tblPMTFrecuencia)
                            {
                                tblPMTFrecuencia newFrec = new tblPMTFrecuencia
                                {
                                    IdDocumento = nuevoDocumento.IdDocumento,
                                    IdEmpresa = IdEmpresa,
                                    IdTipoDocumento = IdTipoDocumento,
                                    IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                    Segmento = reg.Segmento,
                                    TipoFrecuencia = reg.TipoFrecuencia
                                };
                                db.tblPMTFrecuencia.Add(newFrec);

                                foreach (tblPMTFrecuenciaParticipante frecPart in reg.tblPMTFrecuenciaParticipante)
                                {
                                    tblPMTFrecuenciaParticipante newFrecPart = new tblPMTFrecuenciaParticipante
                                    {
                                        IdCargo = frecPart.IdCargo,
                                        IdDocumento = nuevoDocumento.IdDocumento,
                                        IdEmpresa = IdEmpresa,
                                        IdFrecuencia = newFrec.IdFrecuencia,
                                        IdTipoDocumento = IdTipoDocumento
                                    };
                                    db.tblPMTFrecuenciaParticipante.Add(newFrecPart);
                                }
                                
                            }

                            foreach (tblPMTProgramacion regProg in documentoActual.tblPMTProgramacion)
                            {
                                tblPMTProgramacion newProg = new tblPMTProgramacion
                                {
                                    FechaFinal = regProg.FechaFinal,
                                    FechaInicio = regProg.FechaInicio,
                                    IdDocumento = nuevoDocumento.IdDocumento,
                                    IdEmpresa = IdEmpresa,
                                    IdEstado = regProg.IdEstado,
                                    IdModulo = IdTipoDocumento,
                                    Negocios = nuevoDocumento.Negocios
                                };
                                db.tblPMTProgramacion.Add(newProg);

                            }
                            break;
                        case eSystemModules.PPE:
                            foreach (tblPPEFrecuencia reg in documentoActual.tblPPEFrecuencia)
                            {
                                tblPPEFrecuencia newReg = new tblPPEFrecuencia
                                {
                                    IdDocumento = documentoActual.IdDocumento,
                                    IdEmpresa = IdEmpresa,
                                    IdTipoDocumento = IdTipoDocumento,
                                    IdTipoFrecuencia = reg.IdTipoFrecuencia,
                                    Participantes = reg.Participantes,
                                    Proposito = reg.Proposito,
                                    TipoPrueba = reg.TipoPrueba
                                };
                                db.tblPPEFrecuencia.Add(newReg);
                            }
                            break;
                    }

                    db.SaveChanges();
                    IdNuevoDocumento = nuevoDocumento.IdDocumento;
                }
            }

            return IdNuevoDocumento;
        }
    }
}