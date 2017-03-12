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

            List<DocumentoModel> Documentos = new List<DocumentoModel>();
            using (Entities db = new Entities())
            {
                Documentos = (from d in db.tblDocumento
                              let docAnexos = d.tblDocumentoAnexo.Select(x => new DocumentoAnexoModel
                              {
                                  IdAnexo = x.IdAnexo,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  NombreArchivo = x.Nombre,
                                  RutaArchivo = x.Ruta
                              }).ToList()
                              let docAprobaciones = d.tblDocumentoAprobacion.Select(x => new DocumentoAprobacionModel
                              {
                                  Aprobado = (bool)x.Aprobado,
                                  FechaAprobacion = (DateTime)x.Fecha,
                                  IdAprobacion = x.IdAprobacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  Procesado = x.Procesado
                              }).ToList()
                              let docAuditoria = d.tblDocumentoAuditoria.Select(x => new DocumentoAuditoriaModel
                              {
                                  Accion = x.Accion,
                                  DireccionIP = x.DireccionIP,
                                  FechaRegistro = x.FechaRegistro,
                                  IdAuditoria = x.IdAuditoria,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  IdUsuario = x.IdUsuario,
                                  Mensaje = x.Mensaje,
                              }).ToList()
                              let docCertificaciones = d.tblDocumentoCertificacion.Select(x => new DocumentoCertificacionModel
                              {
                                  Certificado = (bool)x.Certificado,
                                  FechaCertificacion = (DateTime)x.Fecha,
                                  IdCertificacion = x.IdCertificacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  Procesado = x.Procesado
                              }).ToList()
                              let docContenido = d.tblDocumentoContenido.Select(x => new DocumentoContenidoModel
                              {
                                  Contenido = x.ContenidoBin,
                                  FechaCreacion = (DateTime)x.FechaCreacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdModulo = x.IdSubModulo,
                                  IdTipoDocumento = x.IdTipoDocumento
                              }).ToList()
                              let docEntrevistas = (from e in d.tblDocumentoEntrevista
                                                    let _Personas = e.tblDocumentoEntrevistaPersona
                                                        .Select(x => new DocumentoEntrevistaPersonaModel
                                                        {
                                                            Empresa = x.Empresa,
                                                            Entrevistador = x.EsEntrevistador,
                                                            IdDocumento = x.IdDocumento,
                                                            IdEmpresa = x.IdEmpresa,
                                                            IdEntrevista = x.IdEntrevista,
                                                            IdPersona = x.IdPersonaEntrevista,
                                                            IdTipoDocumento = x.IdTipoDocumento,
                                                            Nombre = x.Nombre
                                                        }).ToList()
                                                    select new DocumentoEntrevistaModel
                                                    {
                                                        Final = e.FechaFinal,
                                                        IdDocumento = e.IdDocumento,
                                                        IdEmpresa = e.IdEmpresa,
                                                        IdEntrevista = e.IdEntrevista,
                                                        IdTipoDocumento = e.IdTipoDocumento,
                                                        Inicio = e.FechaInicio,
                                                        Personas = _Personas
                                                    }).ToList()
                              let docPersonasClave = d.tblDocumentoPersonaClave.Select(x => new DocumentoPersonaClaveModel
                              {
                                  Cédula = x.Cedula,
                                  DireccionHabitacion = x.DireccionHabitacion,
                                  Email = x.Correo,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,  
                                  Nombre = x.Nombre,
                                  Principal = (bool)(x.Principal),
                                  TelefonoCelular = x.TelefonoCelular,
                                  TelefonoHabitacion = x.TelefonoHabitacion,
                                  TelefonoOficina = x.TelefonoOficina
                              }).ToList()
                              where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.Negocios == Negocios
                              select new DocumentoModel
                              {
                                  Anexos = docAnexos,
                                  Aprobaciones = docAprobaciones,
                                  Auditoria = docAuditoria,
                                  Certificaciones = docCertificaciones,
                                  Contenido = docContenido,
                                  Entrevistas = docEntrevistas,
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
                                  PersonasClave = docPersonasClave,
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
                              let docAnexos = d.tblDocumentoAnexo.Select(x => new DocumentoAnexoModel
                              {
                                  IdAnexo = x.IdAnexo,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  NombreArchivo = x.Nombre,
                                  RutaArchivo = x.Ruta
                              }).ToList()
                              let docAprobaciones = d.tblDocumentoAprobacion.Select(x => new DocumentoAprobacionModel
                              {
                                  Aprobado = (bool)x.Aprobado,
                                  FechaAprobacion = (DateTime)x.Fecha,
                                  IdAprobacion = x.IdAprobacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  Procesado = x.Procesado
                              }).ToList()
                              let docAuditoria = d.tblDocumentoAuditoria.Select(x => new DocumentoAuditoriaModel
                              {
                                  Accion = x.Accion,
                                  DireccionIP = x.DireccionIP,
                                  FechaRegistro = x.FechaRegistro,
                                  IdAuditoria = x.IdAuditoria,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  IdUsuario = x.IdUsuario,
                                  Mensaje = x.Mensaje,
                              }).ToList()
                              let docCertificaciones = d.tblDocumentoCertificacion.Select(x => new DocumentoCertificacionModel
                              {
                                  Certificado = (bool)x.Certificado,
                                  FechaCertificacion = (DateTime)x.Fecha,
                                  IdCertificacion = x.IdCertificacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  Procesado = x.Procesado
                              }).ToList()
                              let docContenido = d.tblDocumentoContenido.Select(x => new DocumentoContenidoModel
                              {
                                  Contenido = x.ContenidoBin,
                                  FechaCreacion = (DateTime)x.FechaCreacion,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdModulo = x.IdSubModulo,
                                  IdTipoDocumento = x.IdTipoDocumento
                              }).ToList()
                              let docEntrevistas = (from e in d.tblDocumentoEntrevista
                                                    let _Personas = e.tblDocumentoEntrevistaPersona
                                                        .Select(x => new DocumentoEntrevistaPersonaModel
                                                        {
                                                            Empresa = x.Empresa,
                                                            Entrevistador = x.EsEntrevistador,
                                                            IdDocumento = x.IdDocumento,
                                                            IdEmpresa = x.IdEmpresa,
                                                            IdEntrevista = x.IdEntrevista,
                                                            IdPersona = x.IdPersonaEntrevista,
                                                            IdTipoDocumento = x.IdTipoDocumento,
                                                            Nombre = x.Nombre
                                                        }).ToList()
                                                    select new DocumentoEntrevistaModel
                                                    {
                                                        Final = e.FechaFinal,
                                                        IdDocumento = e.IdDocumento,
                                                        IdEmpresa = e.IdEmpresa,
                                                        IdEntrevista = e.IdEntrevista,
                                                        IdTipoDocumento = e.IdTipoDocumento,
                                                        Inicio = e.FechaInicio,
                                                        Personas = _Personas
                                                    }).ToList()
                              let docPersonasClave = d.tblDocumentoPersonaClave.Select(x => new DocumentoPersonaClaveModel
                              {
                                  Cédula = x.Cedula,
                                  DireccionHabitacion = x.DireccionHabitacion,
                                  Email = x.Correo,
                                  IdDocumento = x.IdDocumento,
                                  IdEmpresa = x.IdEmpresa,
                                  IdPersona = x.IdPersona,
                                  IdTipoDocumento = x.IdTipoDocumento,
                                  Nombre = x.Nombre,
                                  Principal = (bool)(x.Principal),
                                  TelefonoCelular = x.TelefonoCelular,
                                  TelefonoHabitacion = x.TelefonoHabitacion,
                                  TelefonoOficina = x.TelefonoOficina
                              }).ToList()
                              where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento && d.IdTipoDocumento == IdTipoDocumento
                             select new DocumentoModel
                              {
                                  Anexos = docAnexos,
                                  Aprobaciones = docAprobaciones,
                                  Auditoria = docAuditoria,
                                  Certificaciones = docCertificaciones,
                                  Contenido = docContenido,
                                  Entrevistas = docEntrevistas,
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
                                  PersonasClave = docPersonasClave,
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

            try
            {
                using (Entities db = new Entities())
                {

                    tblDocumento documento = (from d in db.tblDocumento
                                              where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento && d.IdTipoDocumento == IdTipoDocumento
                                              select d).FirstOrDefault();

                    IEnumerable<tblDocumentoEntrevistaPersona> PersonasEntrevista = (documento.tblDocumentoEntrevista.Select(x => db.tblDocumentoEntrevistaPersona).ToList() as IEnumerable<tblDocumentoEntrevistaPersona>);

                    switch (IdTipoDocumento)
                    {
                        case 4:
                            IEnumerable<tblBIAAmenaza> tblBIAAmenaza = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAAmenaza).ToList() as IEnumerable<tblBIAAmenaza>);
                            IEnumerable<tblBIAAmenazaEvento> tblBIAAmenazaEvento = (tblBIAAmenaza.Select(x => x.tblBIAAmenazaEvento).ToList() as IEnumerable<tblBIAAmenazaEvento>);
                            IEnumerable<tblBIAEventoControl> tblBIAEventoControl = (tblBIAAmenazaEvento.Select(x => x.tblBIAEventoControl).ToList() as IEnumerable<tblBIAEventoControl>);
                            IEnumerable<tblBIAAplicacion> tblBIAAplicacion = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAAplicacion).ToList() as IEnumerable<tblBIAAplicacion>);
                            IEnumerable<tblBIAClienteProducto> tblBIAClienteProducto = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAClienteProducto).ToList() as IEnumerable<tblBIAClienteProducto>);
                            IEnumerable<tblBIADocumentacion> tblBIADocumentacion = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIADocumentacion).ToList() as IEnumerable<tblBIADocumentacion>);
                            IEnumerable<tblBIAEntrada> tblBIAEntrada = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAEntrada).ToList() as IEnumerable<tblBIAEntrada>);
                            IEnumerable<tblBIAGranImpacto> tblBIAGranImpacto = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAGranImpacto).ToList() as IEnumerable<tblBIAGranImpacto>);
                            IEnumerable<tblBIAImpactoFinanciero> tblBIAImpactoFinanciero = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAImpactoFinanciero).ToList() as IEnumerable<tblBIAImpactoFinanciero>);
                            IEnumerable<tblBIAImpactoOperacional> tblBIAImpactoOperacional = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAImpactoOperacional).ToList() as IEnumerable<tblBIAImpactoOperacional>);
                            IEnumerable<tblBIAInterdependencia> tblBIAInterdependencia = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAInterdependencia).ToList() as IEnumerable<tblBIAInterdependencia>);
                            IEnumerable<tblBIAMTD> tblBIAMTD = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAMTD).ToList() as IEnumerable<tblBIAMTD>);
                            IEnumerable<tblBIAPersonaRespaldoProceso> tblBIAPersonaRespaldoProceso = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAPersonaRespaldoProceso).ToList() as IEnumerable<tblBIAPersonaRespaldoProceso>);
                            IEnumerable<tblBIAProcesoAlterno> tblBIAProcesoAlterno = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAProcesoAlterno).ToList() as IEnumerable<tblBIAProcesoAlterno>);
                            IEnumerable<tblBIAProveedor> tblBIAProveedor = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAProveedor).ToList() as IEnumerable<tblBIAProveedor>);
                            IEnumerable<tblBIARespaldoPrimario> tblBIARespaldoPrimario = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIARespaldoPrimario).ToList() as IEnumerable<tblBIARespaldoPrimario>);
                            IEnumerable<tblBIARespaldoSecundario> tblBIARespaldoSecundario = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIARespaldoSecundario).ToList() as IEnumerable<tblBIARespaldoSecundario>);
                            IEnumerable<tblBIARPO> tblBIARPO = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIARPO).ToList() as IEnumerable<tblBIARPO>);
                            IEnumerable<tblBIARTO> tblBIARTO = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIARTO).ToList() as IEnumerable<tblBIARTO>);
                            IEnumerable<tblBIAWRT> tblBIAWRT = (documento.tblBIADocumento.tblBIAProceso.Select(x => x.tblBIAWRT).ToList() as IEnumerable<tblBIAWRT>);

                            db.tblBIAAmenaza.RemoveRange(tblBIAAmenaza);
                            db.tblBIAAmenazaEvento.RemoveRange(tblBIAAmenazaEvento);
                            db.tblBIAEventoControl.RemoveRange(tblBIAEventoControl);
                            db.tblBIAAplicacion.RemoveRange(tblBIAAplicacion);
                            db.tblBIAClienteProducto.RemoveRange(tblBIAClienteProducto);
                            db.tblBIADocumentacion.RemoveRange(tblBIADocumentacion);
                            db.tblBIAEntrada.RemoveRange(tblBIAEntrada);
                            db.tblBIAGranImpacto.RemoveRange(tblBIAGranImpacto);
                            db.tblBIAImpactoFinanciero.RemoveRange(tblBIAImpactoFinanciero);
                            db.tblBIAImpactoOperacional.RemoveRange(tblBIAImpactoOperacional);
                            db.tblBIAInterdependencia.RemoveRange(tblBIAInterdependencia);
                            db.tblBIAMTD.RemoveRange(tblBIAMTD);
                            db.tblBIAPersonaRespaldoProceso.RemoveRange(tblBIAPersonaRespaldoProceso);
                            db.tblBIAProcesoAlterno.RemoveRange(tblBIAProcesoAlterno);
                            db.tblBIAProveedor.RemoveRange(tblBIAProveedor);
                            db.tblBIARespaldoPrimario.RemoveRange(tblBIARespaldoPrimario);
                            db.tblBIARespaldoSecundario.RemoveRange(tblBIARespaldoSecundario);
                            db.tblBIARPO.RemoveRange(tblBIARPO);
                            db.tblBIARTO.RemoveRange(tblBIARTO);
                            db.tblBIAWRT.RemoveRange(tblBIAWRT);
                            db.tblBIAComentario.RemoveRange(documento.tblBIADocumento.tblBIAComentario);
                            db.tblBIAProceso.RemoveRange(documento.tblBIADocumento.tblBIAProceso);
                            db.tblBIADocumento.Remove(documento.tblBIADocumento);
                            break;
                        case 7:
                            IEnumerable<tblBCPReanudacionTareaActividad> TareaActividad = (documento.tblBCPDocumento.tblBCPReanudacionTarea.Select(x => db.tblBCPReanudacionTareaActividad).ToList() as IEnumerable<tblBCPReanudacionTareaActividad>);

                            db.tblBCPReanudacionPersonaClave.RemoveRange(documento.tblBCPDocumento.tblBCPReanudacionPersonaClave);
                            db.tblBCPReanudacionTareaActividad.RemoveRange(TareaActividad);
                            db.tblBCPReanudacionTarea.RemoveRange(documento.tblBCPDocumento.tblBCPReanudacionTarea);
                            db.tblBCPRecuperacionPersonaClave.RemoveRange(documento.tblBCPDocumento.tblBCPRecuperacionPersonaClave);
                            db.tblBCPRecuperacionRecurso.RemoveRange(documento.tblBCPDocumento.tblBCPRecuperacionRecurso);
                            db.tblBCPRespuestaAccion.RemoveRange(documento.tblBCPDocumento.tblBCPRespuestaAccion);
                            db.tblBCPRespuestaRecurso.RemoveRange(documento.tblBCPDocumento.tblBCPRespuestaRecurso);
                            db.tblBCPRestauracionEquipo.RemoveRange(documento.tblBCPDocumento.tblBCPRestauracionEquipo);
                            db.tblBCPRestauracionInfraestructura.RemoveRange(documento.tblBCPDocumento.tblBCPRestauracionInfraestructura);
                            db.tblBCPRestauracionMobiliario.RemoveRange(documento.tblBCPDocumento.tblBCPRestauracionMobiliario);
                            db.tblBCPRestauracionOtro.RemoveRange(documento.tblBCPDocumento.tblBCPRestauracionOtro);
                            db.tblBCPDocumento.Remove(documento.tblBCPDocumento);
                            break;
                    }

                    db.tblDocumentoAnexo.RemoveRange(documento.tblDocumentoAnexo);
                    db.tblDocumentoAprobacion.RemoveRange(documento.tblDocumentoAprobacion);
                    db.tblDocumentoAuditoria.RemoveRange(documento.tblDocumentoAuditoria);
                    db.tblDocumentoCertificacion.RemoveRange(documento.tblDocumentoCertificacion);
                    db.tblDocumentoContenido.RemoveRange(documento.tblDocumentoContenido);
                    db.tblDocumentoEntrevistaPersona.RemoveRange(PersonasEntrevista);
                    db.tblDocumentoEntrevista.RemoveRange(documento.tblDocumentoEntrevista);
                    db.tblDocumentoPersonaClave.RemoveRange(documento.tblDocumentoPersonaClave);
                    db.tblDocumento.Remove(documento);

                    db.SaveChanges();
                }

                Eliminado = true;
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                Eliminado = false;
            }
            return Eliminado;
        }
        public static long GetNextNroDocumento(int IdClaseDocumento, int IdTipoDocumento)
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
        public static int GetNextVersion(int idClaseDocumento, int idTipoDocumento, int VersionOriginal)
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

            using (Entities db = new Entities())
            {
                long IdNivelUsuario = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario).FirstOrDefault().IdNivelUsuario;
                SubModulos = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdModuloPadre == IdModuloPadre)
                    .Select(x => x.tblModulo).ToList();
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

            return NombreCompleto;
        }

        public static IList<PersonaModel> GetPersonasConUsuario()
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
                           where p.IdEmpresa == IdEmpresa && p.IdUsuario != 0
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

    }
}