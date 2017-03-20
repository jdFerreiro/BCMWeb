using BCMWeb.Data.EF;
using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace BCMWeb
{

    public static class Metodos
    {
        internal static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        internal static string Culture = HttpContext.Current.Request.UserLanguages[0];

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
                                where m.IdEmpresa == IdEmpresa && m.tblModulo.IdModuloPadre == 0 && m.IdUsuario == UserId && m.IdModulo < 99000000
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
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdUser = long.Parse(Session["UserId"].ToString());
            long minModulo = IdTipoDocumento * 1000000;
            long maxModulo = IdTipoDocumento * 1999999;
            
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

                Documentos = (from d in db.tblDocumento
                              where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.Negocios == Negocios
                              select new DocumentoModel
                              {
                                  Editable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEditables > 0),
                                  Eliminable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEliminables > 0),
                                  Estatus = d.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion,
                                  FechaCreacion = d.FechaCreacion,
                                  FechaEstadoDocumento = d.FechaEstadoDocumento,
                                  FechaUltimaModificacion = (DateTime)d.FechaUltimaModificacion,
                                  IdDocumento = d.IdDocumento,
                                  IdEstatus = d.IdEstadoDocumento,
                                  IdPersonaResponsable = d.IdPersonaResponsable,
                                  IdTipoDocumento = d.IdTipoDocumento,
                                  Negocios = d.Negocios,
                                  NroDocumento = d.NroDocumento,
                                  NroVersion = d.NroVersion,
                                  RequiereCertificacion = d.RequiereCertificacion,
                                  VersionOriginal = (int)d.VersionOriginal
                              }).ToList();
            }
            return Documentos;
        }
        public static DocumentoModel GetDocumento(long IdDocumento, int IdTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            DocumentoModel Documento = new DocumentoModel();
            using (Entities db = new Entities())
            {
                Documento = (from d in db.tblDocumento
                              where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.IdDocumento == IdDocumento
                              select new DocumentoModel
                              {
                                  Estatus = d.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion,
                                  FechaCreacion = d.FechaCreacion,
                                  FechaEstadoDocumento = d.FechaEstadoDocumento,
                                  FechaUltimaModificacion = (DateTime)d.FechaUltimaModificacion,
                                  IdDocumento = d.IdDocumento,
                                  IdEstatus = d.IdEstadoDocumento,
                                  IdPersonaResponsable = d.IdPersonaResponsable,
                                  IdTipoDocumento = d.IdTipoDocumento,
                                  Negocios = d.Negocios,
                                  NroDocumento = d.NroDocumento,
                                  NroVersion = d.NroVersion,
                                  RequiereCertificacion = d.RequiereCertificacion,
                                  VersionOriginal = (int)d.VersionOriginal
                              }).FirstOrDefault();
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

                                    db.tblBCPDocumento.RemoveRange(procesoBIA.tblBCPDocumento.ToList());
                                    db.tblBIAAmenaza.RemoveRange(procesoBIA.tblBIAAmenaza.ToList());
                                    db.tblBIAAplicacion.RemoveRange(procesoBIA.tblBIAAplicacion.ToList());
                                    db.tblBIAClienteProducto.RemoveRange(procesoBIA.tblBIAClienteProducto.ToList());
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

                    if (ListPrevDoc != null)
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
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdModuloPadre == IdModuloPadre)
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
        public static bool UpdateContenidoDocumento(long IdModulo, byte[] Contenido)
        {
            bool Updated = false;
            string _IdModulo = IdModulo.ToString();
            long IdTipoDocumento = long.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            long IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());
            eTipoAccion Accion = eTipoAccion.Actualizar;

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
                        FechaUltimaModificacion = DateTime.UtcNow,
                        IdDocumento = 0,
                        IdEmpresa = IdEmpresa,
                        IdEstadoDocumento = 1,
                        IdTipoDocumento = IdTipoDocumento,
                        IdPersonaResponsable = 0,
                        RequiereCertificacion = true,
                        Negocios = (IdClaseDocumento == 1),
                        NroVersion = Metodos.GetNextVersion(IdClaseDocumento, IdTipoDocumento, IdVersion),
                        NroDocumento = Metodos.GetNextNroDocumento(IdClaseDocumento, IdTipoDocumento),
                        VersionOriginal = IdVersion
                    };
                    db.tblDocumento.Add(dataDocumento);
                    db.SaveChanges();

                    Accion = eTipoAccion.GenerarVersion;

                    IdDocumento = dataDocumento.IdDocumento;
                }

                Session["IdDocumento"] = IdDocumento;

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
                        IdTipoDocumento = IdClaseDocumento
                    };

                    db.tblDocumentoContenido.Add(docContenido);
                }
                else
                {
                    docContenido.ContenidoBin = Contenido;
                }

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
                db.SaveChanges();
            }

            Auditoria.RegistrarLogout();
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

            List<AuditoriaModel> data = new List<AuditoriaModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblAuditoria
                        where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento
                        select new AuditoriaModel
                        {
                            Accion = d.Accion,
                            DireccionIP = d.DireccionIP,
                            Empresa = d.tblEmpresa.NombreComercial,
                            FechaRegistro = d.FechaRegistro,
                            Id = d.IdAuditoria,
                            EditDocumento = false,
                            IdClaseDocumento = (d.tblDocumento.Negocios ? 1 : 2),
                            IdDocumento = (long)d.IdDocumento,
                            IdEmpresa = (long)d.IdEmpresa,
                            IdModulo = 0,
                            IdModuloActual = 0,
                            IdTipoDocumento = (long)d.IdTipoDocumento,
                            IdUsuario = (long)d.IdUsuario,
                            Mensaje = d.Mensaje,
                            NombreUsuario = d.tblUsuario.Nombre,
                            NroDocumento = d.tblDocumento.NroDocumento,
                            TipoDocumento = string.Empty,
                        }).ToList();
            }

            return data;
        }
    }
}