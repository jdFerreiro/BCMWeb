using BCMWeb.Data.EF;
using BCMWeb.Models;
using BCMWeb.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace BCMWeb
{

    public static class Metodos
    {
        private static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        private static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
        private static string Culture = HttpContext.Current.Request.UserLanguages[0];
        private static Uri _ContextUrl = HttpContext.Current.Request.Url;
        private static string _AppUrl = _ContextUrl.AbsoluteUri.Replace(_ContextUrl.PathAndQuery, string.Empty);
        private static HttpServerUtility _Server = HttpContext.Current.Server;

        private class ValorEscalaSearch
        {
            private short _Valor;

            public ValorEscalaSearch(short valor)
            {
                _Valor = valor;
            }

            public bool Exists(short obj)
            {
                return obj == _Valor;
            }
        }
        private class ValorlongSearch
        {
            private long _Valor;

            public ValorlongSearch(long valor)
            {
                _Valor = valor;
            }

            public bool Exists(long obj)
            {
                return obj == _Valor;
            }
        }
        private class ValorStringSearch
        {
            private string _Valor;

            public ValorStringSearch(string valor)
            {
                _Valor = valor;
            }

            public bool Exists(string obj)
            {
                return obj == _Valor;
            }
        }

        public static PerfilModelView GetPerfilData()
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            PerfilModelView model = new PerfilModelView();

            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();

                Encriptador Encriptar = new Encriptador();
                string _password = Encriptar.Desencriptar(usuario.ClaveUsuario, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

                model = new PerfilModelView
                {
                    Email = usuario.Email,
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Password = _password,
                    PasswordCompare = _password
                };

                //model = (from x in db.tblUsuario
                //         let _password = Encriptar.Desencriptar(x.ClaveUsuario, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256)
                //         where x.IdUsuario == IdUser
                //         select new PerfilModelView
                //         {
                //             Email = x.Email,
                //             IdUsuario = x.IdUsuario,
                //             Nombre = x.Nombre,
                //             Password = _password,
                //             PasswordCompare = _password
                //         }).FirstOrDefault();

            }

            return model;
        }

        public static IList<long> GetRelatedUOIds(long filterIdUnidadOrganizativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<long> RelatedUOIds = new List<long>();

            using (Entities db = new Entities())
            {
                tblUnidadOrganizativa unidad = db.tblUnidadOrganizativa.FirstOrDefault(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == filterIdUnidadOrganizativa);
                RelatedUOIds.Add(unidad.IdUnidadOrganizativa);
                long IdUnidadPadre = unidad.IdUnidadOrganizativa;

                while (IdUnidadPadre > 0)
                {
                    List<tblUnidadOrganizativa> Unidades = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && RelatedUOIds.Contains(x.IdUnidadPadre) && !RelatedUOIds.Contains(x.IdUnidadOrganizativa)).ToList();
                    if (Unidades != null && Unidades.Count() > 0)
                    {
                        RelatedUOIds.AddRange(Unidades.Select(x => x.IdUnidadOrganizativa));
                    }
                    else
                    {
                        IdUnidadPadre = 0;
                    }
                }
            }
            return RelatedUOIds;
        }

        public static IList<EmpresaModel> GetEmpresasUsuario()
        {

            List<EmpresaModel> Data = new List<EmpresaModel>();
            string _AppUrl = _ContextUrl.AbsoluteUri.Replace(_ContextUrl.PathAndQuery, string.Empty);

            if (Session != null && Session["UserId"] != null)
            {
                long UserId = long.Parse(Session["UserId"].ToString());

                using (Entities db = new Entities())
                {
                    List<tblEmpresa> Empresas = (from eu in db.tblEmpresaUsuario
                                                 where eu.IdUsuario == UserId
                                                 select eu.tblEmpresa).ToList();

                    if (Empresas != null && Empresas.Count > 0)
                    {
                        foreach (tblEmpresa eu in Empresas)
                        {
                            Data.Add(new EmpresaModel
                            {
                                Direccion = eu.DireccionAdministrativa,
                                FechaInicio = eu.FechaInicioActividad,
                                FechaUltimoEstado = eu.FechaUltimoEstado,
                                IdEmpresa = eu.IdEmpresa,
                                IdEstatus = eu.IdEstado,
                                LogoUrl = eu.LogoURL, //String.Format("{0}/PDFDocs/{1}", _AppUrl, eu.LogoURL),
                                NombreComercial = eu.NombreComercial.Trim(),
                                NombreFiscal = eu.NombreFiscal.Trim(),
                                RegistroFiscal = eu.RegistroFiscal
                            });
                        }
                    }
                }

            }

            return Data;
        }
        public static string GetSeveridad(short? urgente)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            short IdPrioridad = (short)urgente;
            string _Prioridad = Resources.IniciativaResource.textoNormal;

            using (Entities db = new Entities())
            {
                _Prioridad = db.tblSeveridadRiesgo.Where(x => x.IdEmpresa == IdEmpresa && x.IdSeveridad == IdPrioridad).FirstOrDefault().Nombre;
            }

            return _Prioridad;
        }
        public static void SavePerfil(PerfilModelView model)
        {
            long IdUser = long.Parse(Session["UserId"].ToString());

            Encriptador Encriptar = new Encriptador();
            string _password = Encriptar.Encriptar(model.Password, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.Email = model.Email;
                    usuario.CodigoUsuario = model.Email;
                    usuario.ClaveUsuario = _password;
                    usuario.Nombre = model.Nombre;

                    db.SaveChanges();
                }
            }
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
        public static string GetNombreEmpresa()
        {
            string _NombreEmpresa = string.Empty;

            if (Session["IdEmpresa"] != null)
            {
                using (Entities db = new Entities())
                {
                    long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
                    _NombreEmpresa = db.tblEmpresa.Where(x => x.IdEmpresa == IdEmpresa).FirstOrDefault().NombreComercial;
                }
            }

            return _NombreEmpresa;
        }
        public static IList<ModuloModel> GetModulosPrincipalesEmpresa()
        {
            List<ModuloModel> Data = new List<ModuloModel>();

            if (Session != null && Session["IdEmpresa"] != null)
            {
                long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

                using (Entities db = new Entities())
                {
                    Data = (from m in db.tblModulo
                            where m.IdEmpresa == IdEmpresa && m.IdModuloPadre == 0
                                    && m.Activo
                            select new ModuloModel
                            {
                                Action = m.Accion,
                                Activo = m.Activo,
                                CodigoModulo = (int)m.IdCodigoModulo,
                                Controller = m.Controller,
                                Descripcion = m.Descripcion,
                                IdModulo = m.IdModulo,
                                IdModuloPadre = 0,
                                IdTipoElemento = m.IdTipoElemento,
                                ImageRoot = m.imageRoot,
                                Negocios = m.Negocios,
                                Nombre = m.Nombre,
                                Tecnologia = m.Tecnologia,
                                Titulo = m.Titulo
                            }).ToList();
                }
            }

            return Data;
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
        public static IList<ModuloModel> GetModulosPrincipalesEmpresaUsuario(long IdEmpresa)
        {
            List<ModuloModel> Data = new List<ModuloModel>();

            if (Session != null && Session["IdEmpresa"] != null && Session["UserId"] != null)
            {
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
        public static IQueryable<DocumentoModel> GetDocumentosModulo(int IdTipoDocumento, bool Negocios)
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

                List<long> Unidades = db.tblUsuarioUnidadOrganizativa
                                        .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUser)
                                        .Select(x => x.IdUnidadOrganizativa)
                                        .ToList();

                bool ProgramacionExist = db.tblPMTProgramacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.Negocios == Negocios
                             && x.IdModulo == (IdTipoDocumento * 1000000)).Count() > 0;

                tblPMTProgramacion Programacion = db.tblPMTProgramacion
                    .FirstOrDefault(x => x.IdEmpresa == IdEmpresa
                                      && x.IdModulo == (IdTipoDocumento * 1000000)
                                      && x.Negocios == Negocios
                                      && x.FechaInicio <= DateTime.UtcNow
                                      && x.FechaFinal >= DateTime.UtcNow);

                bool CanEdit = ((!ProgramacionExist && Programacion == null) || (ProgramacionExist && Programacion != null))
                               && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEditables > 0);

                Documentos = (from d in db.tblDocumento
                              where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento
                                                             && d.Negocios == Negocios
                                                             && (d.IdTipoDocumento == 4 && EmpresaUsuario.IdNivelUsuario != 6 && EmpresaUsuario.IdNivelUsuario != 4
                                                                    ? Unidades.Contains((long)d.tblBIADocumento
                                                                                         .Where(x => x.IdEmpresa == d.IdEmpresa && x.IdDocumento == d.IdDocumento && x.IdTipoDocumento == d.IdTipoDocumento)
                                                                                         .FirstOrDefault().IdUnidadOrganizativa)
                                                                    : true)
                              select d).AsEnumerable()
                                .Select(d => new DocumentoModel
                                {
                                    Editable = CanEdit,
                                    Eliminable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEliminables > 0),
                                    Estatus = (d.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion),
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
                                    HasVersion = db.tblDocumento
                                        .Where(x => x.IdEmpresa == IdEmpresa
                                                    && x.IdTipoDocumento == IdTipoDocumento
                                                    && x.NroDocumento == d.NroDocumento
                                                    && x.NroVersion > d.NroVersion).Count() > 0,
                                    VersionOriginal = (int)d.VersionOriginal,
                                    DatosBIA = (d.tblBIADocumento == null || d.tblBIADocumento.Count() == 0
                                                ? null
                                                : new DocumentoBIA
                                                {
                                                    CadenaServicio = string.Empty,
                                                    IdCadenaServicio = (long)(d.tblBIADocumento.FirstOrDefault().IdCadenaServicio ?? 0),
                                                    IdUnidadOrganizativa = (long)(d.tblBIADocumento.FirstOrDefault().IdUnidadOrganizativa ?? 0)
                                                }),
                                    DatosBCP = (d.tblBCPDocumento == null || d.tblBCPDocumento.Count() == 0
                                                ? null
                                                : new DocumentoBCP
                                                {
                                                    IdProceso = (long)(d.tblBCPDocumento.FirstOrDefault().IdProceso ?? 0),
                                                    IdUnidadOrganizativa = (long)(d.tblBCPDocumento.FirstOrDefault().IdUnidadOrganizativa ?? 0),
                                                    Proceso = d.tblBCPDocumento.FirstOrDefault().Proceso,
                                                    Responsable = d.tblBCPDocumento.FirstOrDefault().Responsable,
                                                    Subproceso = d.tblBCPDocumento.FirstOrDefault().SubProceso,
                                                }),
                                    Procesos = (d.tblBIADocumento == null || d.tblBIADocumento.Count() == 0 || d.tblBIADocumento.FirstOrDefault().tblBIAProceso == null || d.tblBIADocumento.FirstOrDefault().tblBIAProceso.Count() == 0
                                                ? null
                                                : d.tblBIADocumento.FirstOrDefault().tblBIAProceso.AsEnumerable()
                                                    .Select(x => new DocumentoProcesoModel
                                                    {
                                                        Critico = x.Critico ?? false,
                                                        Descripcion = x.Descripcion,
                                                        FechaCreacion = (DateTime)x.FechaCreacion,
                                                        FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                                                        IdEstatus = x.IdEstadoProceso ?? 0,
                                                        IdProceso = x.IdProceso,
                                                        Nombre = x.Nombre,
                                                        NroProceso = x.NroProceso ?? 0
                                                    }).ToList()),
                                }).ToList();
            }

            return Documentos.OrderBy(x => x.NroDocumento).ThenBy(x => x.Version).ThenBy(x => x.NroVersion).ToList().AsQueryable();
        }
        public static string getStringProcesos(object dataItem)
        {
            StringBuilder sb = new StringBuilder();
            long IdDocumento = (long)dataItem;

            using (Entities db = new Entities())
            {
                List<tblBIAProceso> _procesos = (from d in db.tblBIAProceso
                                                 where d.tblBIADocumento.IdDocumento == IdDocumento
                                                 select d).ToList();

                _procesos = _procesos.OrderBy(x => x.NroProceso).ToList();

                foreach (tblBIAProceso proceso in _procesos)
                {
                    string strItem = string.Format("<tr><td style=\"text-align: center;\">{0}</td><td>{1}</td><td style=\"margin: auto; text-align: center;\">{2}</td></tr>",
                                                    proceso.NroProceso.ToString(),
                                                    proceso.Nombre,
                                                    (!(bool)proceso.Critico ? "<img width=\"15\" src=\"../../Content/Images/boton_Verde.jpg\" />" : "<img width=\"15\" src=\"../../Content/Images/Boton_rojo.png\" />"));
                    sb.Append(strItem);
                }
            }

            return sb.ToString();
        }
        public static string ConvertirRuta(object dataItem)
        {
            StringBuilder sb = new StringBuilder();
            string Ruta = dataItem.ToString();
            string imgRuta = string.Empty;

            if (!string.IsNullOrEmpty(Ruta))
            {
                imgRuta = "<a href=\"" + Ruta + "\"><img src=\"../../Content/Images/Internal/icono-pdf.jpg\" /></a>";
                sb.Append(imgRuta);
            }


            return sb.ToString();
        }
        public static bool ValidarEmailUsuario(string email)
        {
            bool IsValid = false;

            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.Email == email).FirstOrDefault();
                if (usuario != null)
                    IsValid = true;
            }

            return IsValid;
        }
        public static void ChangePassword(string email, string password)
        {
            Encriptador _Encriptar = new Encriptador();

            string encriptPassword = _Encriptar.Encriptar(password, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);
            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.Email == email).FirstOrDefault();
                usuario.ClaveUsuario = encriptPassword;
                db.SaveChanges();
            }
        }
        public static DocumentoModel GetDocumento(long IdDocumento, int IdTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdUser = long.Parse(Session["UserId"].ToString());
            long minModulo = IdTipoDocumento * 1000000;
            long maxModulo = IdTipoDocumento * 1999999;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            DocumentoModel Documento = new DocumentoModel();
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

                Documento = (from d in db.tblDocumento
                             where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.IdDocumento == IdDocumento
                             select d).AsEnumerable()
                .Select(d => new DocumentoModel
                {
                    Editable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEditables > 0),
                    Eliminable = d.IdEstadoDocumento != 6 && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEliminables > 0),
                    Estatus = (d.IdEstadoDocumento == 6
                                ? db.tblCultura_EstadoDocumento.Where(x => x.IdEstadoDocumento == 1 && (x.Culture == Culture || x.Culture == "es-VE")).FirstOrDefault().Descripcion
                                : d.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion),
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
                    HasVersion = db.tblDocumento
                        .Where(x => x.IdEmpresa == IdEmpresa
                                    && x.IdTipoDocumento == IdTipoDocumento
                                    && x.NroDocumento == d.NroDocumento
                                    && x.NroVersion > d.NroVersion).Count() > 0,
                    VersionOriginal = (int)d.VersionOriginal,
                    DatosBIA = (d.tblBIADocumento == null || d.tblBIADocumento.Count() == 0
                                ? null
                                : new DocumentoBIA
                                {
                                    CadenaServicio = string.Empty,
                                    IdCadenaServicio = (long)(d.tblBIADocumento.FirstOrDefault().IdCadenaServicio ?? 0),
                                    IdUnidadOrganizativa = (long)(d.tblBIADocumento.FirstOrDefault().IdUnidadOrganizativa ?? 0)
                                }),
                    DatosBCP = (d.tblBCPDocumento == null || d.tblBCPDocumento.Count() == 0
                                ? null
                                : new DocumentoBCP
                                {
                                    IdProceso = (long)(d.tblBCPDocumento.FirstOrDefault().IdProceso ?? 0),
                                    IdUnidadOrganizativa = (long)(d.tblBCPDocumento.FirstOrDefault().IdUnidadOrganizativa ?? 0),
                                    Proceso = d.tblBCPDocumento.FirstOrDefault().Proceso,
                                    Responsable = d.tblBCPDocumento.FirstOrDefault().Responsable,
                                    Subproceso = d.tblBCPDocumento.FirstOrDefault().SubProceso,
                                }),
                }).FirstOrDefault();

                //tblDocumento data = (from d in db.tblDocumento
                //                     where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.IdDocumento == IdDocumento
                //                     select d).FirstOrDefault();
                //string _EstadoDocumento = string.Empty;

                //if (data == null || data.IdEstadoDocumento < 6)
                //{
                //    _EstadoDocumento = db.tblCultura_EstadoDocumento.Where(x => x.IdEstadoDocumento == 1 && (x.Culture == Culture || x.Culture == "es-VE")).FirstOrDefault().Descripcion;
                //}
                //else
                //{
                //    _EstadoDocumento = data.tblEstadoDocumento.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion;
                //}


                //if (data != null)
                //{
                //    Documento = new DocumentoModel
                //    {
                //        Estatus = _EstadoDocumento,
                //        FechaCreacion = (DateTime)data.FechaCreacion.AddMinutes(Minutos),
                //        FechaEstadoDocumento = (DateTime)data.FechaEstadoDocumento.AddMinutes(Minutos),
                //        FechaUltimaModificacion = (DateTime)((DateTime)data.FechaUltimaModificacion).AddMinutes(Minutos),
                //        IdDocumento = data.IdDocumento,
                //        IdEstatus = data.IdEstadoDocumento,
                //        IdPersonaResponsable = data.IdPersonaResponsable,
                //        IdTipoDocumento = data.IdTipoDocumento,
                //        Negocios = data.Negocios,
                //        NroDocumento = data.NroDocumento,
                //        NroVersion = data.NroVersion,
                //        RequiereCertificacion = data.RequiereCertificacion,
                //        Version = string.Format("{0}.{1}", data.VersionOriginal.ToString(), data.NroVersion.ToString()),
                //        VersionOriginal = (int)data.VersionOriginal
                //    };
                //}
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
                                    db.tblBIAAmenaza.RemoveRange(db.tblBIAAmenaza.Where(x => x.IdEmpresa == procesoBIA.IdEmpresa && x.IdProceso == procesoBIA.IdProceso).ToList());
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
                            .ToList();

                    if (ListPrevDoc != null && ListPrevDoc.Count > 0)
                    {
                        tblDocumento LastPrevDoc = ListPrevDoc.OrderBy(x => x.NroDocumento).ToList().LastOrDefault();
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
        public static long GetIdDocumentoByProceso(long idProceso)
        {
            long IdDocumento = 0;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                IdDocumento = (long)db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdProceso == idProceso).FirstOrDefault().tblBIADocumento.IdDocumento;
            }

            return IdDocumento;
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
            long IdDocumento = 0;

            if (Session["IdDocumento"] != null)
                IdDocumento = long.Parse(Session["IdDocumento"].ToString());

            using (Entities db = new Entities())
            {
                long IdNivelUsuario = db.tblEmpresaUsuario.Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario).FirstOrDefault().IdNivelUsuario;
                SubModulos = db.tblModulo_Usuario
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdModuloPadre == IdModuloPadre && x.tblModulo.Activo)
                    .Select(x => x.tblModulo).ToList();

                if (IdModuloPadre == 99010000)
                {
                    if (IdDocumento > 0)
                    {
                        eEstadoDocumento IdEstadoDocumento = (eEstadoDocumento)db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == IdDocumento).FirstOrDefault().IdEstadoDocumento;
                        switch (IdEstadoDocumento)
                        {
                            case eEstadoDocumento.Aprobando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Cargando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Certificado:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.Certificando:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                break;
                            case eEstadoDocumento.PorAprobar:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010500).FirstOrDefault());
                                break;
                            case eEstadoDocumento.PorCertificar:
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010100).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
                                SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                                break;
                        }
                    }
                    else
                    {
                        SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010400).FirstOrDefault());
                        SubModulos.Remove(SubModulos.Where(x => x.IdModulo == 99010300).FirstOrDefault());
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
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUsuario && x.tblModulo.IdCodigoModulo == _IdCodigoModulo && x.tblModulo.IdTipoElemento == 4 && x.tblModulo.Activo)
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
        public static PersonaModel GetShortPersonaById(long IdPersona)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            PersonaModel Persona = new PersonaModel();

            using (Entities db = new Entities())
            {
                Persona = (from p in db.tblPersona
                           where p.IdEmpresa == IdEmpresa && p.IdPersona == IdPersona
                           select new PersonaModel
                           {
                               IdCargoPersona = p.IdCargo ?? 0,
                               Identificacion = p.Identificacion,
                               IdPersona = p.IdPersona,
                               IdUnidadOrganizativaPersona = p.IdUnidadOrganizativa ?? 0,
                               IdUsuario = p.IdUsuario ?? 0,
                               Nombre = p.Nombre,
                               UnidadOrganizativa = (p.tblUnidadOrganizativa == null ? new UnidadOrganizativaModel { IdUnidad = 0, IdUnidadPadre = 0, NombreUnidadOrganizativa = "" } : new UnidadOrganizativaModel { IdUnidad = p.tblUnidadOrganizativa.IdUnidadOrganizativa, IdUnidadPadre = p.tblUnidadOrganizativa.IdUnidadPadre, NombreUnidadOrganizativa = p.tblUnidadOrganizativa.Nombre })
                           }).FirstOrDefault();
            }

            return Persona;
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
                               IdCiudad = x.IdCiudad,
                               IdEstado = x.IdEstado,
                               IdPais = x.IdPais,
                               IdPersonaDireccion = x.IdPersonaDireccion,
                               IdTipoDireccion = x.IdTipoDireccion,
                               TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion,
                               Ubicación = x.Ubicacion
                           }).ToList()
                           let TelefonosPersona = p.tblPersonaTelefono.Select(x => new PersonaTelefono
                           {
                               NroTelefono = x.Telefono,
                               IdPersonaTelefono = x.IdPersonaTelefono,
                               IdTipoTelefono = x.IdTipoTelefono,
                               TipoTelefono = x.tblTipoTelefono.tblCultura_TipoTelefono
                                    .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                    .FirstOrDefault().Descripcion
                           }).ToList()
                           where p.IdEmpresa == IdEmpresa && p.IdPersona == IdPersona
                           select new PersonaModel
                           {
                               Cargo = (p.tblCargo == null ? new CargoModel { IdCargo = 0, NombreCargo = "" } : new CargoModel { IdCargo = p.tblCargo.IdCargo, NombreCargo = p.tblCargo.Descripcion }),
                               CorreosElectronicos = CorreosPersona,
                               Direcciones = DireccionesPersona,
                               IdCargoPersona = (long)p.IdCargo,
                               Identificacion = p.Identificacion,
                               IdPersona = p.IdPersona,
                               IdUnidadOrganizativaPersona = (long)p.IdUnidadOrganizativa,
                               IdUsuario = (long)p.IdUsuario,
                               Nombre = p.Nombre,
                               Telefonos = TelefonosPersona,
                               UnidadOrganizativa = new UnidadOrganizativaModel { IdUnidad = p.tblUnidadOrganizativa.IdUnidadOrganizativa, IdUnidadPadre = p.tblUnidadOrganizativa.IdUnidadPadre, NombreUnidadOrganizativa = p.tblUnidadOrganizativa.Nombre }
                           }).FirstOrDefault();
            }

            return Persona;
        }
        public static string GetTecnologiaDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Result = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAAplicacion> Data = db.tblBIAAplicacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAAplicacion Reg in Data)
                {
                    Result += "<div class=\"internal\"><table>";
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.TecnologiaPataformaHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Nombre);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.TecnologiaUsuarioHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Usuario);
                    Result += "</table></div>";
                }
            }

            return Result;
        }
        public static string GetProveedoresDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Result = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAProveedor> Data = db.tblBIAProveedor
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAProveedor Reg in Data)
                {
                    Result += "<div class=\"internal\"><table>";
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ProveedorOrganizacionHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Organizacion);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ProveedorServicioHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Servicio);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ProveedorResponsableHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Contacto);
                    Result += "</table></div>";
                }
            }

            return Result;
        }
        public static string GetPersonasClaveDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Result = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAPersonaClave> Data = db.tblBIAPersonaClave
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAPersonaClave Reg in Data)
                {
                    Result += "<div class=\"internal\"><table>";
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderNombre);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.Nombre);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderCedula);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.Cedula);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderTelfOficina);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.TelefonoOficina);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderTelfHabitacion);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.TelefonoHabitacion);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderTelfMovil);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.TelefonoCelular);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderEmail);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.Correo);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.PersonaClaveHeaderDireccion);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.tblDocumentoPersonaClave.DireccionHabitacion);
                    Result += "</table></div>";
                }
            }

            return Result;
        }
        public static string GetEntradasDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Result = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAEntrada> Data = db.tblBIAEntrada
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAEntrada Reg in Data)
                {
                    Result += "<div class=\"internal\"><table>";
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.EntradaUnidadHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Unidad.Replace("\r\n", "<br />"));
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.EntradaProcesoHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Evento.Replace("\r\n", "<br />"));
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.EntradaResponsableHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Responsable.Replace("\r\n", "<br />"));
                    Result += "</table></div>";
                }
            }
            return Result;
        }
        public static string GetClientesProductosDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Result = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAClienteProceso> Data = db.tblBIAClienteProceso
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAClienteProceso Reg in Data)
                {
                    Result += "<div class=\"internal\"><table>";
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ClienteUnidadHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Unidad);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ClienteProcesoHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Proceso);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ClienteResponsableHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Responsable);
                    Result += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.ClienteProductoHeader);
                    Result += string.Format("<tr><td>{0}</td></tr>", Reg.Producto);
                    Result += "</table></div>";
                }
            }
            return Result;
        }
        public static string GetInterdependenciasDiagrama(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            string Data = string.Empty;

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                List<tblBIAInterdependencia> Interdependencias = db.tblBIAInterdependencia
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBIA == _idDocBIA && x.IdProceso == idProceso)
                    .ToList();
                foreach (tblBIAInterdependencia Interdependencia in Interdependencias)
                {
                    Data += "<div class=\"internal\"><table>";
                    //Data += string.Format("<tr><td style=\"width: 40%; padding: 2px;\">{0}</td><td style=\"width: 60%; padding: 2px;\">{1}</td></tr>", Resources.DiagramaResource.InterdependenciaUnidadHeader, Interdependencia.Organizacion);
                    //Data += string.Format("<tr><td style=\"width: 40%; padding: 2px;\">{0}</td><td style=\"width: 60%; padding: 2px;\">{1}</td></tr>", Resources.DiagramaResource.InterdependenciaProcesoHeader, Interdependencia.Servicio);
                    //Data += string.Format("<tr><td style=\"width: 40%; padding: 2px;\">{0}</td><td style=\"width: 60%; padding: 2px;\">{1}</td></tr>", Resources.DiagramaResource.InterdepdendenciaResponsableHeader, Interdependencia.Contacto);
                    Data += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.InterdependenciaUnidadHeader);
                    Data += string.Format("<tr><td>{0}</td></tr>", Interdependencia.Organizacion);
                    Data += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.InterdependenciaProcesoHeader);
                    Data += string.Format("<tr><td>{0}</td></tr>", Interdependencia.Servicio);
                    Data += string.Format("<tr><td class=\"subHeader\">{0}</td></tr>", Resources.DiagramaResource.InterdepdendenciaResponsableHeader);
                    Data += string.Format("<tr><td>{0}</td></tr>", Interdependencia.Contacto);

                    Data += "</table></div>";
                }
            }
            return Data;
        }
        public static DocumentoProcesoModel GetProceso(long idProceso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());

            DocumentoProcesoModel Proceso = new DocumentoProcesoModel();

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                tblBIAProceso _proceso = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa &&
                                                                     x.IdDocumentoBia == _idDocBIA &&
                                                                     x.IdProceso == idProceso).FirstOrDefault();

                if (_proceso != null)
                {
                    Proceso.Critico = (bool)_proceso.Critico;
                    Proceso.Descripcion = _proceso.Descripcion;
                    Proceso.FechaCreacion = (DateTime)_proceso.FechaCreacion;
                    Proceso.FechaEstatus = (DateTime)_proceso.FechaUltimoEstatus;
                    Proceso.IdEstatus = (long)_proceso.IdEstadoProceso;
                    Proceso.IdProceso = _proceso.IdProceso;
                    Proceso.Nombre = _proceso.Nombre;
                    Proceso.NroProceso = (int)_proceso.NroProceso;
                }
            }

            return Proceso;
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

                    while (unidad != null && unidad.IdUnidadPadre > 0 && unidad.IdUnidadPadre != unidad.IdUnidadOrganizativa)
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
                                IdCiudad = x.IdCiudad,
                                IdEstado = x.IdEstado,
                                IdPais = x.IdPais,
                                IdPersonaDireccion = x.IdPersonaDireccion,
                                IdTipoDireccion = x.IdTipoDireccion,
                                TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion
                                     .Where(c => c.Culture == Culture || c.Culture == "es-VE")
                                     .FirstOrDefault().Descripcion,
                                Ubicación = x.Ubicacion
                            }).ToList()
                            let TelefonosPersona = p.tblPersonaTelefono.Select(x => new PersonaTelefono
                            {
                                IdPersonaTelefono = x.IdPersonaTelefono,
                                IdTipoTelefono = x.IdTipoTelefono,
                                NroTelefono = x.Telefono,
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
                                IdCargoPersona = (long)p.IdCargo,
                                Identificacion = p.Identificacion,
                                IdPersona = p.IdPersona,
                                IdUnidadOrganizativaPersona = (long)p.IdUnidadOrganizativa,
                                IdUsuario = (long)p.IdUsuario,
                                Nombre = p.Nombre,
                                Telefonos = TelefonosPersona,
                                UnidadOrganizativa = new UnidadOrganizativaModel { IdUnidad = p.tblUnidadOrganizativa.IdUnidadOrganizativa, IdUnidadPadre = p.tblUnidadOrganizativa.IdUnidadPadre, NombreUnidadOrganizativa = p.tblUnidadOrganizativa.Nombre }
                            }).ToList();

            }

            Personas = Personas.OrderBy(x => x.Nombre).ToList();
            Personas.Insert(0, new PersonaModel { IdPersona = 0, Nombre = Resources.BCMWebPublic.itemSelectValue });

            return Personas;
        }
        public static IList<UnidadOrganizativaModel> GetUnidadesOrganizativas()
        {
            List<UnidadOrganizativaModel> Data = new List<UnidadOrganizativaModel>();

            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                Data = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new UnidadOrganizativaModel()
                    {
                        IdUnidad = x.IdUnidadOrganizativa,
                        IdUnidadPadre = x.IdUnidadPadre,
                        NombreUnidadOrganizativa = x.Nombre
                    }).ToList();
            }

            return Data;
        }
        public static DataTable GetDataTableUnidadesOrganizativas()
        {

            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            DataTable _UODataTable = new DataTable();

            _UODataTable.Columns.Add("IdUnidad", typeof(System.Int64));
            _UODataTable.Columns.Add("IdUnidadPadre", typeof(System.Int64));
            _UODataTable.Columns.Add("Nombre", typeof(System.String));
            _UODataTable.PrimaryKey = new DataColumn[] { _UODataTable.Columns["IdUnidad"] };

            using (Entities db = new Entities())
            {
                List<tblUnidadOrganizativa> Data = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa).ToList();
                foreach (tblUnidadOrganizativa dataUO in Data)
                {
                    _UODataTable.Rows.Add(dataUO.IdUnidadOrganizativa, dataUO.IdUnidadPadre, dataUO.Nombre);
                }
            }

            return _UODataTable;
        }
        public static object GetImpactoOperacional(long IdUnidadOrganizativa, long IdProceso = 0)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objImpactoOperacional> ImpactoOperacional = new List<objImpactoOperacional>();
            List<long> IdsUO = GetIdUnidadesOrganizativas(IdUnidadOrganizativa);
            List<long> IdsProcesos = new List<long>();
            IdsUO.Insert(0, IdUnidadOrganizativa);


            using (Entities db = new Entities())
            {
                if (IdProceso == 0)
                {
                    IdsProcesos = db.tblBIAProceso
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIADocumento.IdUnidadOrganizativa))
                       .Select(x => x.IdProceso).Distinct().ToList();
                }
                else
                {
                    IdsProcesos = db.tblBIAProceso
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIADocumento.IdUnidadOrganizativa) && x.IdProceso == IdProceso)
                       .Select(x => x.IdProceso).Distinct().ToList();
                }
                ImpactoOperacional = db.tblBIAImpactoOperacional
                    .Where(x => x.IdEmpresa == IdEmpresa && IdsProcesos.Contains(x.IdProceso))
                    .Select(x => new objImpactoOperacional
                    {
                        DescEscala = (x.tblEscala != null ? x.tblEscala.Descripcion : string.Empty),
                        Escala = (x.tblEscala != null ? x.tblEscala.Valor : 0),
                        IdProceso = x.IdProceso,
                        ImpactoOperacional = x.ImpactoOperacional,
                        Proceso = x.tblBIAProceso.Nombre
                    }).ToList();
            }

            return ImpactoOperacional;
        }
        public static List<objValorImpacto> GetValoresImpacto(long IdUnidadOrganizativa, long IdProceso = 0)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objValorImpacto> ValoresImpacto = new List<objValorImpacto>();
            List<long> IdsUO = GetIdUnidadesOrganizativas(IdUnidadOrganizativa);
            List<long> IdsProcesos = new List<long>();
            IdsUO.Insert(0, IdUnidadOrganizativa);


            using (Entities db = new Entities())
            {
                if (IdProceso == 0)
                {
                    ValoresImpacto = db.tblBIAProceso
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIADocumento.IdUnidadOrganizativa))
                       .Select(x => new objValorImpacto
                       {
                           IdProceso = x.IdProceso,
                           Proceso = x.Nombre
                       }).Distinct().ToList();
                }
                else
                {
                    ValoresImpacto = db.tblBIAProceso
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIADocumento.IdUnidadOrganizativa) && x.IdProceso == IdProceso)
                       .Select(x => new objValorImpacto
                       {
                           IdProceso = x.IdProceso,
                           Proceso = x.Nombre
                       }).Distinct().ToList();
                }

                foreach (objValorImpacto Impacto in ValoresImpacto)
                {
                    tblBIAMTD MTD = db.tblBIAMTD.Where(x => x.IdEmpresa == IdEmpresa && x.IdProceso == Impacto.IdProceso).FirstOrDefault();
                    tblBIARPO RPO = db.tblBIARPO.Where(x => x.IdEmpresa == IdEmpresa && x.IdProceso == Impacto.IdProceso).FirstOrDefault();
                    tblBIARTO RTO = db.tblBIARTO.Where(x => x.IdEmpresa == IdEmpresa && x.IdProceso == Impacto.IdProceso).FirstOrDefault();
                    tblBIAWRT WRT = db.tblBIAWRT.Where(x => x.IdEmpresa == IdEmpresa && x.IdProceso == Impacto.IdProceso).FirstOrDefault();

                    if (MTD != null)
                    {
                        if (MTD.tblEscala != null)
                        {
                            Impacto.EscalaMTD = (MTD.tblEscala != null ? MTD.tblEscala.Valor : 0);
                            Impacto.DescEscalaMTD = (MTD.tblEscala != null ? MTD.tblEscala.Descripcion : string.Empty);
                        }
                        else
                        {
                            Impacto.EscalaMTD = 0;
                            Impacto.DescEscalaMTD = string.Empty;
                        }
                    }
                    if (RPO != null)
                    {
                        if (RPO.tblEscala != null)
                        {
                            Impacto.EscalaRPO = (RPO.tblEscala != null ? RPO.tblEscala.Valor : 0);
                            Impacto.DescEscalaRPO = (RPO.tblEscala != null ? RPO.tblEscala.Descripcion : string.Empty);
                        }
                        else
                        {
                            Impacto.EscalaRPO = 0;
                            Impacto.DescEscalaRPO = string.Empty;
                        }
                    }
                    if (RTO != null)
                    {
                        if (RTO.tblEscala != null)
                        {
                            Impacto.EscalaRTO = (RTO.tblEscala != null ? RTO.tblEscala.Valor : 0);
                            Impacto.DescEscalaRTO = (RTO.tblEscala != null ? RTO.tblEscala.Descripcion : string.Empty);
                        }
                        else
                        {
                            Impacto.EscalaRTO = 0;
                            Impacto.DescEscalaRTO = string.Empty;
                        }
                    }
                    if (WRT != null)
                    {
                        if (WRT.tblEscala != null)
                        {
                            Impacto.EscalaWRT = (WRT.tblEscala != null ? WRT.tblEscala.Valor : 0);
                            Impacto.DescEscalaWRT = (WRT.tblEscala != null ? WRT.tblEscala.Descripcion : string.Empty);
                        }
                        else
                        {
                            Impacto.EscalaWRT = 0;
                            Impacto.DescEscalaWRT = string.Empty;
                        }
                    }
                }
            }

            return ValoresImpacto;
        }
        public static List<objProcesoMes> GetProcesoMes(long IdUnidadOrganizativa, long IdProceso = 0)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objProcesoMes> ProcesosMes = new List<objProcesoMes>();
            List<long> IdsUO = GetIdUnidadesOrganizativas(IdUnidadOrganizativa);
            List<string> PM01 = new List<string>();
            List<string> PM02 = new List<string>();
            List<string> PM03 = new List<string>();
            List<string> PM04 = new List<string>();
            List<string> PM05 = new List<string>();
            List<string> PM06 = new List<string>();
            List<string> PM07 = new List<string>();
            List<string> PM08 = new List<string>();
            List<string> PM09 = new List<string>();
            List<string> PM10 = new List<string>();
            List<string> PM11 = new List<string>();
            List<string> PM12 = new List<string>();
            List<tblBIAGranImpacto> GranImpacto;
            List<long> IdsProcesos = new List<long>();
            IdsUO.Insert(0, IdUnidadOrganizativa);


            using (Entities db = new Entities())
            {
                if (IdProceso == 0)
                {
                    GranImpacto = db.tblBIAGranImpacto
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa))
                       .Distinct().ToList();
                }
                else
                {
                    GranImpacto = db.tblBIAGranImpacto
                       .Where(x => x.IdEmpresa == IdEmpresa && IdsUO.Contains((long)x.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa) && x.IdProceso == IdProceso)
                       .Distinct().ToList();
                }

                foreach (tblBIAGranImpacto Impacto in GranImpacto.OrderBy(x => x.tblBIAProceso.Nombre).ToList())
                {
                    if (Impacto.IdMes == 1)
                    {
                        PM01.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 2)
                    {
                        PM02.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 3)
                    {
                        PM03.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 4)
                    {
                        PM04.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 5)
                    {
                        PM05.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 6)
                    {
                        PM06.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 7)
                    {
                        PM07.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 8)
                    {
                        PM08.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 9)
                    {
                        PM09.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 10)
                    {
                        PM10.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 11)
                    {
                        PM11.Add(Impacto.tblBIAProceso.Nombre);
                    }
                    if (Impacto.IdMes == 12)
                    {
                        PM12.Add(Impacto.tblBIAProceso.Nombre);
                    }
                }
            }

            int _TotalLength = 0;
            _TotalLength = (PM01.Count > _TotalLength ? PM01.Count : _TotalLength);
            _TotalLength = (PM02.Count > _TotalLength ? PM02.Count : _TotalLength);
            _TotalLength = (PM03.Count > _TotalLength ? PM03.Count : _TotalLength);
            _TotalLength = (PM04.Count > _TotalLength ? PM04.Count : _TotalLength);
            _TotalLength = (PM05.Count > _TotalLength ? PM05.Count : _TotalLength);
            _TotalLength = (PM06.Count > _TotalLength ? PM06.Count : _TotalLength);
            _TotalLength = (PM07.Count > _TotalLength ? PM07.Count : _TotalLength);
            _TotalLength = (PM08.Count > _TotalLength ? PM08.Count : _TotalLength);
            _TotalLength = (PM09.Count > _TotalLength ? PM09.Count : _TotalLength);
            _TotalLength = (PM10.Count > _TotalLength ? PM10.Count : _TotalLength);
            _TotalLength = (PM11.Count > _TotalLength ? PM11.Count : _TotalLength);
            _TotalLength = (PM12.Count > _TotalLength ? PM12.Count : _TotalLength);

            for (int index = 0; index < _TotalLength; index++)
            {
                objProcesoMes ProcesoMes = new objProcesoMes
                {
                    Proceso_M01 = string.Empty,
                    Proceso_M02 = string.Empty,
                    Proceso_M03 = string.Empty,
                    Proceso_M04 = string.Empty,
                    Proceso_M05 = string.Empty,
                    Proceso_M06 = string.Empty,
                    Proceso_M07 = string.Empty,
                    Proceso_M08 = string.Empty,
                    Proceso_M09 = string.Empty,
                    Proceso_M10 = string.Empty,
                    Proceso_M11 = string.Empty,
                    Proceso_M12 = string.Empty
                };
                if (index < PM01.Count)
                    ProcesoMes.Proceso_M01 = PM01[index];
                if (index < PM02.Count)
                    ProcesoMes.Proceso_M02 = PM02[index];
                if (index < PM03.Count)
                    ProcesoMes.Proceso_M03 = PM03[index];
                if (index < PM04.Count)
                    ProcesoMes.Proceso_M04 = PM04[index];
                if (index < PM05.Count)
                    ProcesoMes.Proceso_M05 = PM05[index];
                if (index < PM06.Count)
                    ProcesoMes.Proceso_M06 = PM06[index];
                if (index < PM07.Count)
                    ProcesoMes.Proceso_M07 = PM07[index];
                if (index < PM08.Count)
                    ProcesoMes.Proceso_M08 = PM08[index];
                if (index < PM09.Count)
                    ProcesoMes.Proceso_M09 = PM09[index];
                if (index < PM10.Count)
                    ProcesoMes.Proceso_M10 = PM10[index];
                if (index < PM11.Count)
                    ProcesoMes.Proceso_M11 = PM11[index];
                if (index < PM12.Count)
                    ProcesoMes.Proceso_M12 = PM12[index];

                ProcesosMes.Add(ProcesoMes);
            }

            return ProcesosMes;
        }
        public static List<long> GetIdUnidadesOrganizativas(long IdUnidadOrganizativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<long> data = new List<long>();
            List<long> idUnidades = new List<long>();

            using (Entities db = new Entities())
            {
                idUnidades = db.tblUnidadOrganizativa
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadPadre == IdUnidadOrganizativa)
                    .Select(x => x.IdUnidadOrganizativa)
                    .ToList();

            }
            data.AddRange(idUnidades);

            foreach (long IdUnidad in idUnidades)
            {
                data.AddRange(GetIdUnidadesOrganizativas(IdUnidad));
            }
            return data;
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
                          }).ToList();

            }

            Cargos = Cargos.OrderBy(x => x.NombreCargo).ToList();
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
                        IdPersonaTelefono = x.IdPersonaTelefono,
                        IdTipoTelefono = x.IdTipoTelefono,
                        NroTelefono = x.Telefono,
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
                        IdCiudad = x.IdCiudad,
                        IdEstado = x.IdEstado,
                        IdPais = x.IdPais,
                        IdPersonaDireccion = x.IdPersonaDireccion,
                        IdTipoDireccion = x.IdTipoDireccion,
                        TipoDireccion = x.tblTipoDireccion.tblCultura_TipoDireccion.Where(t => t.Culture == Culture || t.Culture == "es-VE").FirstOrDefault().Descripcion,
                        Ubicación = x.Ubicacion
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

                    if (IdTipoDocumento == 4)
                    {
                        tblBIADocumento docBia = db.tblBIADocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                                            && x.IdDocumento == IdDocumento
                                                                            && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();
                        if (docBia == null)
                        {
                            docBia = new tblBIADocumento
                            {
                                IdCadenaServicio = 0,
                                IdDocumento = IdDocumento,
                                IdEmpresa = IdEmpresa,
                                IdTipoDocumento = IdTipoDocumento,
                                IdUnidadOrganizativa = 0,
                            };

                            db.tblBIADocumento.Add(docBia);
                        }
                    }
                    if (IdTipoDocumento == 7)
                    {

                        tblBCPDocumento docBCP = db.tblBCPDocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                                            && x.IdDocumento == IdDocumento
                                                                            && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();
                        if (docBCP == null)
                        {
                            tblBIADocumento docuBIA = db.tblBIADocumento.Where(x => x.IdEmpresa == IdEmpresa
                                                                                 && x.IdDocumento == IdDocumento
                                                                                 && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();
                            docBCP = new tblBCPDocumento
                            {
                                IdDocumento = IdDocumento,
                                IdDocumentoBIA = 0,
                                IdEmpresa = IdEmpresa,
                                IdProceso = null,
                                IdTipoDocumento = IdTipoDocumento,
                                IdUnidadOrganizativa = 0,
                                Proceso = string.Empty,
                                Responsable = string.Empty,
                                SubProceso = string.Empty
                            };
                        }
                    }
                }
                else
                {
                    docContenido.ContenidoBin = Contenido;
                }

                dataDocumento.FechaUltimaModificacion = DateTime.UtcNow;

                db.SaveChanges();
                Updated = true;
            }

            if (Updated)
            {
                Auditoria.RegistarAccion(Accion);
                ProcesarDocumento.ProcesarContenidoDocumento(IdModulo, Contenido);
            }
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
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            List<AuditoriaModel> data = new List<AuditoriaModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblAuditoria
                        let fechaRegistro = SqlFunctions.DateAdd("mi", Minutos, d.FechaRegistro)
                        where d.IdEmpresa == IdEmpresa && d.IdDocumento == IdDocumento
                        select new AuditoriaModel
                        {
                            Accion = d.Accion,
                            DireccionIP = d.DireccionIP,
                            Empresa = d.tblEmpresa.NombreComercial,
                            FechaRegistro = (DateTime)fechaRegistro,
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
                            NroDocumento = (d.tblDocumento == null ? 0 : d.tblDocumento.NroDocumento),
                            TipoDocumento = string.Empty,
                        }).ToList();

            }
            return data.OrderByDescending(x => x.FechaRegistro).ToList();
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
                    IdPersona = (long)x.IdPersona,
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
                        case eSystemModules.RSG:
                            foreach (tblBIAAmenaza amenaza in documentoActual.tblBIAAmenaza)
                            {
                                tblBIAAmenaza newAmenaza = new tblBIAAmenaza
                                {
                                    Control = amenaza.Control,
                                    ControlesImplantar = amenaza.ControlesImplantar,
                                    Descripcion = amenaza.Descripcion,
                                    Estado = amenaza.Estado,
                                    Evento = amenaza.Evento,
                                    Fuente = amenaza.Fuente,
                                    IdDocumento = amenaza.IdDocumento,
                                    IdDocumentoBIA = amenaza.IdDocumentoBIA,
                                    IdEmpresa = IdEmpresa,
                                    IdTipoDocumento = amenaza.IdTipoDocumento,
                                    Impacto = amenaza.Impacto,
                                    IdProceso = amenaza.IdProceso,
                                    Probabilidad = amenaza.Probabilidad,
                                    Severidad = amenaza.Severidad,
                                    TipoControlImplantado = amenaza.TipoControlImplantado
                                };
                                db.tblBIAAmenaza.Add(newAmenaza);

                            }
                            break;
                        case eSystemModules.PMT:
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
        public static List<DocumentoProcesoModel> GetProcesosDocumento()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();

            using (Entities db = new Entities())
            {
                long _idDocBIA = db.tblBIADocumento.FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdDocumento == IdDocumento).IdDocumentoBIA;
                Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == _idDocBIA)
                    .Select(x => new DocumentoProcesoModel
                    {
                        Critico = (bool)x.Critico,
                        Descripcion = x.Descripcion,
                        FechaCreacion = (DateTime)x.FechaCreacion,
                        FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                        IdEstatus = (long)x.IdEstadoProceso,
                        IdProceso = x.IdProceso,
                        Nombre = x.Nombre,
                        NroProceso = (int)x.NroProceso
                    }).Distinct().ToList();
            }

            return Procesos.OrderBy(x => x.NroProceso).ToList();
        }
        public static List<DocumentoProcesoModel> GetProcesosDocumento(long IdDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();

            using (Entities db = new Entities())
            {
                Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == IdDocumento)
                    .Select(x => new DocumentoProcesoModel
                    {
                        Critico = (bool)x.Critico,
                        Descripcion = x.Descripcion,
                        FechaCreacion = (DateTime)x.FechaCreacion,
                        FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                        IdEstatus = (long)x.IdEstadoProceso,
                        IdProceso = x.IdProceso,
                        Nombre = x.Nombre,
                        NroProceso = (int)x.NroProceso
                    }).Distinct().ToList();
            }

            return Procesos.OrderBy(x => x.NroProceso).ToList();
        }
        public static List<DocumentoProcesoModel> GetProcesosEmpresa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();

            using (Entities db = new Entities())
            {
                Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new DocumentoProcesoModel
                    {
                        Critico = x.Critico ?? false,
                        Descripcion = x.Descripcion,
                        FechaCreacion = (DateTime)x.FechaCreacion,
                        FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                        IdEstatus = x.IdEstadoProceso ?? 0,
                        IdProceso = x.IdProceso,
                        Nombre = x.Nombre,
                        NroProceso = x.NroProceso ?? 0
                    }).Distinct().ToList();
            }

            Procesos.Add(new DocumentoProcesoModel
            {
                IdProceso = 0,
                Nombre = Resources.DocumentoResource.AllDataMale,
                NroProceso = 0
            });

            return Procesos.OrderBy(x => x.Nombre).ToList();
        }
        public static List<Int16> GetValoresEscala(eTipoEscala TipoEscala)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<short> Valores = new List<short>();

            using (Entities db = new Entities())
            {
                Valores = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)TipoEscala).Select(x => x.Valor).ToList();
            }

            return Valores.OrderBy(x => x).ToList();
        }
        public static List<string> GetDescripcionEscala(eTipoEscala TipoEscala)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<string> Valores = new List<string>();

            using (Entities db = new Entities())
            {
                Valores = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)TipoEscala).Select(x => x.Descripcion).ToList();
            }

            return Valores;
        }
        public static object GetNroProcesosByImpactoOperacional()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objCantidadIO> Resultado = new List<objCantidadIO>();

            using (Entities db = new Entities())
            {
                var objData = (from d in db.tblBIAImpactoOperacional
                               where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                               group d by new
                               {
                                   d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                   d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                   d.tblEscala.Valor
                               } into gcs
                               select new
                               {
                                   IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                   ValorEscala = gcs.Key.Valor,
                                   CantidadEscala = gcs.Count()
                               }).ToList();

                foreach (var objInfo in objData)
                {
                    objCantidadIO objRes;
                    objRes = Resultado.Find(delegate (objCantidadIO objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadIO();

                        objRes.ValorEscala = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.ImpactoOperacional).OrderBy(x => x.Valor)
                                             .Select(x => x.Valor).Distinct().ToList();
                        objRes.Escala = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.ImpactoOperacional).OrderBy(x => x.Valor)
                                             .Select(x => x.Descripcion).Distinct().ToList();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.CantidadEscala = (new Int32[objRes.ValorEscala.Count]).ToList();
                        Resultado.Add(objRes);
                    }
                    var search = new ValorEscalaSearch(objInfo.ValorEscala);
                    int pos = objRes.ValorEscala.FindIndex(search.Exists);
                    objRes.CantidadEscala[pos] += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetNroProcesosByValorImpacto()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objCantidadVI> Resultado = new List<objCantidadVI>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAMTD
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                var objRPOData = (from d in db.tblBIARPO
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                var objRTOData = (from d in db.tblBIARTO
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                var objWRTData = (from d in db.tblBIAWRT
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objCantidadVI objRes;
                    objRes = Resultado.Find(delegate (objCantidadVI objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadVI();

                        objRes.EscalaMTD = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.MTD)
                                             .Select(x => x.Descripcion).Distinct().ToList();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.ValoresMTD = (new Int32[objRes.EscalaMTD.Count]).ToList();
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        if (objRes.EscalaMTD == null)
                        {
                            objRes.EscalaMTD = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.MTD)
                                                 .Select(x => x.Descripcion).Distinct().ToList();
                            objRes.ValoresMTD = (new Int32[objRes.EscalaMTD.Count]).ToList();
                        }
                    }
                    var search = new ValorStringSearch(objInfo.ValorEscala);
                    int pos = objRes.EscalaMTD.FindIndex(search.Exists);
                    objRes.ValoresMTD[pos] += objInfo.CantidadEscala;
                }
                foreach (var objInfo in objRPOData)
                {
                    objCantidadVI objRes;
                    objRes = Resultado.Find(delegate (objCantidadVI objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadVI();

                        objRes.EscalaRPO = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.RPO)
                                             .Select(x => x.Descripcion).Distinct().ToList();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.ValoresRPO = (new Int32[objRes.EscalaRPO.Count]).ToList();
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        if (objRes.EscalaRPO == null)
                        {
                            objRes.EscalaRPO = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.RPO)
                                                 .Select(x => x.Descripcion).Distinct().ToList();
                            objRes.ValoresRPO = (new Int32[objRes.EscalaRPO.Count]).ToList();
                        }
                    }
                    var search = new ValorStringSearch(objInfo.ValorEscala);
                    int pos = objRes.EscalaRPO.FindIndex(search.Exists);
                    objRes.ValoresRPO[pos] += objInfo.CantidadEscala;
                }
                foreach (var objInfo in objRTOData)
                {
                    objCantidadVI objRes;
                    objRes = Resultado.Find(delegate (objCantidadVI objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadVI();

                        objRes.EscalaRTO = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.RTO)
                                             .Select(x => x.Descripcion).Distinct().ToList();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.ValoresRTO = (new Int32[objRes.EscalaRTO.Count]).ToList();
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        if (objRes.EscalaRTO == null)
                        {
                            objRes.EscalaRTO = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.RTO)
                                                 .Select(x => x.Descripcion).Distinct().ToList();
                            objRes.ValoresRTO = (new Int32[objRes.EscalaRTO.Count]).ToList();
                        }
                    }
                    var search = new ValorStringSearch(objInfo.ValorEscala);
                    int pos = objRes.EscalaRTO.FindIndex(search.Exists);
                    objRes.ValoresRTO[pos] += objInfo.CantidadEscala;
                }
                foreach (var objInfo in objWRTData)
                {
                    objCantidadVI objRes;
                    objRes = Resultado.Find(delegate (objCantidadVI objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadVI();

                        objRes.EscalaWRT = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.WRT)
                                             .Select(x => x.Descripcion).Distinct().ToList();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.ValoresWRT = (new Int32[objRes.EscalaWRT.Count]).ToList();
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        if (objRes.EscalaWRT == null)
                        {
                            objRes.EscalaWRT = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)eTipoEscala.WRT)
                                                 .Select(x => x.Descripcion).Distinct().ToList();
                            objRes.ValoresWRT = (new Int32[objRes.EscalaWRT.Count]).ToList();
                        }
                    }
                    var search = new ValorStringSearch(objInfo.ValorEscala);
                    int pos = objRes.EscalaWRT.FindIndex(search.Exists);
                    objRes.ValoresWRT[pos] += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetNroProcesosByGranImpacto()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objCantidadGI> Resultado = new List<objCantidadGI>();

            using (Entities db = new Entities())
            {
                var objGIData = (from d in db.tblBIAGranImpacto
                                 where d.IdEmpresa == IdEmpresa && d.IdMes > 0
                                 group d by new
                                 {
                                     d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                     d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                     d.IdMes
                                 } into gcs
                                 select new
                                 {
                                     IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                     ValorEscala = gcs.Key.IdMes,
                                     CantidadEscala = gcs.Count()
                                 }).ToList();

                foreach (var objInfo in objGIData)
                {
                    objCantidadGI objRes;
                    objRes = Resultado.Find(delegate (objCantidadGI objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objCantidadGI();
                        objRes.Mes = new List<long>();
                        objRes.nombreMes = new List<string>();

                        for (short Index = 1; Index <= 12; Index++)
                        {
                            objRes.Mes.Add(Index);
                            objRes.nombreMes.Add(db.tblCultura_Mes.Where(x => (x.Culture == Culture || x.Culture == "es-VE") && x.IdMes == Index).FirstOrDefault().Descripcion);
                        }
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.Valores = (new Int32[12]).ToList();
                        Resultado.Add(objRes);
                    }
                    var search = new ValorlongSearch(objInfo.ValorEscala);
                    int pos = objRes.Mes.FindIndex(search.Exists);
                    objRes.Valores[pos] += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoImpactoOperacional()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {

                var objData = (from d in db.tblBIAImpactoOperacional
                               where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                               group d by new
                               {
                                   d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                   d.tblEscala.Descripcion
                               } into gcs
                               select new
                               {
                                   IdUnidad = gcs.Key.IdUnidadOrganizativa,
                                   ValorEscala = gcs.Key.Descripcion,
                                   CantidadEscala = gcs.Count()
                               }).ToList();

                foreach (var objInfo in objData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(c => (c.IdUnidad == IdUnidadPrincipal) && (c.Escala == objInfo.ValorEscala));
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = objInfo.CantidadEscala;
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        objRes.Cantidad += objInfo.CantidadEscala;
                    }
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static long GetUnidadPrincipal(long? idUnidadOrganizativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdUnidadPrincipal = 0;

            if (idUnidadOrganizativa > 0)
            {
                long IdUnidadPadre = 0;
                using (Entities db = new Entities())
                {
                    tblUnidadOrganizativa dataUnidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == idUnidadOrganizativa).FirstOrDefault();
                    IdUnidadPrincipal = dataUnidad.IdUnidadOrganizativa;
                    IdUnidadPadre = dataUnidad.IdUnidadPadre;
                    while (IdUnidadPadre != 0)
                    {
                        dataUnidad = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadOrganizativa == IdUnidadPadre).FirstOrDefault();
                        IdUnidadPrincipal = dataUnidad.IdUnidadOrganizativa;
                        IdUnidadPadre = dataUnidad.IdUnidadPadre;
                    }
                }
            }

            return IdUnidadPrincipal;
        }
        public static object GetDataGraficoMTD()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAMTD
                                  let Escala = d.tblEscala.Valor.ToString() + " " + d.tblEscala.Descripcion
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRTO()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objRTOData = (from d in db.tblBIARTO
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                foreach (var objInfo in objRTOData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRPO()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objRPOData = (from d in db.tblBIARPO
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                foreach (var objInfo in objRPOData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoWRT()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objWRTData = (from d in db.tblBIAWRT
                                  where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                  group d by new
                                  {
                                      d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                      d.tblEscala.Descripcion
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Descripcion,
                                      CantidadEscala = gcs.Count()
                                  }).ToList();

                foreach (var objInfo in objWRTData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoGranImpacto()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objGIData = (from d in db.tblBIAGranImpacto
                                 let Mes = d.IdMes + " - " + db.tblCultura_Mes.Where(x => (x.Culture == Culture || x.Culture == "es-VE") && x.IdMes == d.IdMes).FirstOrDefault().Descripcion
                                 where d.IdEmpresa == IdEmpresa && d.IdMes > 0
                                 group d by new
                                 {
                                     d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                     d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                     Mes
                                 } into gcs
                                 select new
                                 {
                                     IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                     ValorEscala = gcs.Key.Mes,
                                     CantidadEscala = gcs.Count()
                                 }).ToList();

                foreach (var objInfo in objGIData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ThenBy(x => (x.Escala.Substring(0, 3).EndsWith("-") ? "0" + x.Escala : x.Escala)).ToList();
        }
        public static object GetDataAmenazasProbabilidad()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objData = (from d in db.tblBIAAmenaza
                               let IdUnidadOrganizativa = db.tblBIAProceso.Where(x => x.IdEmpresa == d.IdEmpresa && x.IdDocumentoBia == x.IdDocumentoBia && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa
                               where d.IdEmpresa == IdEmpresa
                               group d by new
                               {

                                   IdUnidadOrganizativa,
                                   d.Probabilidad
                               } into gcs
                               select new
                               {
                                   IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                   ValorEscala = gcs.Key.Probabilidad,
                                   CantidadEscala = gcs.Count()
                               }).ToList();

                foreach (var objInfo in objData)
                {
                    objGraphIO objRes;
                    objRes = Resultado.Find(delegate (objGraphIO objCIO) { return objCIO.IdUnidad == objInfo.IdUnidad; });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala.ToString();
                        objRes.IdUnidad = objInfo.IdUnidad;
                        objRes.Cantidad = objInfo.CantidadEscala;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static List<TipoEscalaGrafico> GetSemestres()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 1,
                TipoEscala = "Q1"
            });
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 2,
                TipoEscala = "Q2"
            });
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 3,
                TipoEscala = "Q3"
            });
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 4,
                TipoEscala = "Q4"
            });
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 5,
                TipoEscala = "S1"
            });
            EscalaGrafico.Add(new TipoEscalaGrafico
            {
                IdTipoEscala = 6,
                TipoEscala = "S2"
            });

            return EscalaGrafico;
        }
        public static List<TipoEscalaGrafico> GetEscalaGrafico()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblTipoEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala > 2 && x.IdTipoEscala < 7)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdTipoEscala,
                        TipoEscala = x.Descripcion
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static List<TipoEscalaGrafico> GetTipoEscalaGrafico(eTipoEscala TipoEscala)
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoEscala = (int)TipoEscala;

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == IdTipoEscala)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdEscala,
                        TipoEscala = x.Descripcion
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static object GetDataGraficoGranImpacto(long IdEscalaGrafico)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();
            List<int> Meses = new List<int>();
            switch (IdEscalaGrafico)
            {
                case 1: // Q1
                    Meses = new List<int>() { 1, 2, 3 };
                    break;
                case 2: // Q2
                    Meses = new List<int>() { 4, 5, 6 };
                    break;
                case 3: // Q3
                    Meses = new List<int>() { 7, 8, 9 };
                    break;
                case 4: // Q4
                    Meses = new List<int>() { 10, 11, 12 };
                    break;
                case 5: // S1
                    Meses = new List<int>() { 1, 2, 3, 4, 5, 6 };
                    break;
                case 6: // S2
                    Meses = new List<int>() { 7, 8, 9, 10, 11, 12 };
                    break;
            }

            using (Entities db = new Entities())
            {
                var objGIData = (from d in db.tblBIAGranImpacto
                                 let Mes = d.IdMes + " - " + db.tblCultura_Mes.Where(x => (x.Culture == Culture || x.Culture == "es-VE") && x.IdMes == d.IdMes).FirstOrDefault().Descripcion
                                 where d.IdEmpresa == IdEmpresa && Meses.Contains(d.IdMes)
                                 group d by new
                                 {
                                     d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                     d.tblBIAProceso.tblBIADocumento.tblUnidadOrganizativa.Nombre,
                                     Mes
                                 } into gcs
                                 select new
                                 {
                                     IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                     ValorEscala = gcs.Key.Mes,
                                     CantidadEscala = gcs.Count()
                                 }).ToList();

                foreach (var objInfo in objGIData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ThenBy(x => (x.Escala.Substring(0, 3).EndsWith("-") ? "0" + x.Escala : x.Escala)).ToList();
        }
        public static object GetDataGraficoValorImpacto(eTipoEscala idEscalaGrafico)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objData = new List<objDataGrafico>();

                switch (idEscalaGrafico)
                {
                    case eTipoEscala.MTD:
                        objData = (from d in db.tblBIAMTD
                                   let Escala = d.tblEscala.Descripcion
                                   where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                   group d by new
                                   {
                                       d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                       d.IdEscala,
                                       Escala
                                   } into gcs
                                   select new objDataGrafico
                                   {
                                       IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                       IdEscala = (long)gcs.Key.IdEscala,
                                       ValorEscala = gcs.Key.Escala,
                                       CantidadEscala = gcs.Count()
                                   }).OrderBy(x => x.IdEscala).ToList();
                        break;
                    case eTipoEscala.RPO:
                        objData = (from d in db.tblBIARPO
                                   let Escala = d.tblEscala.Descripcion
                                   where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                   group d by new
                                   {
                                       d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                       d.IdEscala,
                                       Escala
                                   } into gcs
                                   select new objDataGrafico
                                   {
                                       IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                       IdEscala = (long)gcs.Key.IdEscala,
                                       ValorEscala = gcs.Key.Escala,
                                       CantidadEscala = gcs.Count()
                                   }).OrderBy(x => x.IdEscala).ToList();
                        break;
                    case eTipoEscala.RTO:
                        objData = (from d in db.tblBIARTO
                                   let Escala = d.tblEscala.Descripcion
                                   where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                   group d by new
                                   {
                                       d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                       d.IdEscala,
                                       Escala
                                   } into gcs
                                   select new objDataGrafico
                                   {
                                       IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                       IdEscala = (long)gcs.Key.IdEscala,
                                       ValorEscala = gcs.Key.Escala,
                                       CantidadEscala = gcs.Count()
                                   }).OrderBy(x => x.IdEscala).ToList();
                        break;
                    case eTipoEscala.WRT:
                        objData = (from d in db.tblBIAWRT
                                   let Escala = d.tblEscala.Descripcion
                                   where d.IdEmpresa == IdEmpresa && d.IdEscala > 0
                                   group d by new
                                   {
                                       d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                       d.IdEscala,
                                       Escala
                                   } into gcs
                                   select new objDataGrafico
                                   {
                                       IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                       IdEscala = (long)gcs.Key.IdEscala,
                                       ValorEscala = gcs.Key.Escala,
                                       CantidadEscala = gcs.Count()
                                   }).OrderBy(x => x.IdEscala).ToList();
                        break;
                }

                foreach (var objInfo in objData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdEscala = objInfo.IdEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.ToList();
        }
        public static object GetDataGraficoImpactoOperacional(long idEscalaGrafico)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {

                var objData = (from d in db.tblBIAImpactoOperacional
                               where d.IdEmpresa == IdEmpresa && d.IdEscala == idEscalaGrafico
                               group d by new
                               {
                                   d.tblBIAProceso.tblBIADocumento.IdUnidadOrganizativa,
                                   d.tblEscala.Descripcion
                               } into gcs
                               select new
                               {
                                   IdUnidad = gcs.Key.IdUnidadOrganizativa,
                                   ValorEscala = gcs.Key.Descripcion,
                                   CantidadEscala = gcs.Count()
                               }).ToList();

                foreach (var objInfo in objData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(c => (c.IdUnidad == IdUnidadPrincipal) && (c.Escala == objInfo.ValorEscala));
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = objInfo.CantidadEscala;
                        Resultado.Add(objRes);
                    }
                    else
                    {
                        objRes.Cantidad += objInfo.CantidadEscala;
                    }
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetProbabilidadEmpresa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablasGeneralesModel> Resultado = new List<TablasGeneralesModel>();

            using (Entities db = new Entities())
            {
                Resultado = db.tblProbabilidadRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TablasGeneralesModel
                    {
                        Descripcion = x.Nombre,
                        Valoracion = x.IdProbabilidad.ToString()
                    }).ToList();
            }

            return Resultado;
        }
        public static object GetImpactoEmpresa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablasGeneralesModel> Resultado = new List<TablasGeneralesModel>();

            using (Entities db = new Entities())
            {
                Resultado = db.tblImpactoRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TablasGeneralesModel
                    {
                        Descripcion = x.Nombre,
                        Valoracion = x.IdImpacto.ToString()
                    }).ToList();
            }

            return Resultado;
        }
        public static object GetControlEmpresa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablasGeneralesModel> Resultado = new List<TablasGeneralesModel>();

            using (Entities db = new Entities())
            {
                Resultado = db.tblControlRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TablasGeneralesModel
                    {
                        Descripcion = x.Nombre,
                        Valoracion = x.IdControl.ToString()
                    }).ToList();
            }

            return Resultado;
        }
        public static object GetFuenteEmpresa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablasGeneralesModel> Resultado = new List<TablasGeneralesModel>();

            using (Entities db = new Entities())
            {
                Resultado = db.tblFuenteRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TablasGeneralesModel
                    {
                        Descripcion = x.Nombre,
                        Valoracion = x.CodigoFuente
                    }).ToList();
            }

            return Resultado;
        }
        public static List<DataRiesgoControl> GetRiesgoControles(long idUnidadOrganizativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DataRiesgoControl> Resultado = new List<DataRiesgoControl>();
            List<long> UnidadesOrganizativas = GetUnidadesDependientes(idUnidadOrganizativa);
            UnidadesOrganizativas.Add(idUnidadOrganizativa);

            using (Entities db = new Entities())
            {
                Resultado = db.tblBIAAmenaza.Where(x => x.IdEmpresa == IdEmpresa && UnidadesOrganizativas.Contains((long)db.tblBIAProceso.Where(y => y.IdEmpresa == IdEmpresa && y.IdDocumentoBia == x.IdDocumentoBIA && y.IdProceso == x.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa))
                    .Select(x => new DataRiesgoControl
                    {
                        Amenaza = x.Descripcion,
                        Estado = (int)x.Estado,
                        Evento = x.Evento,
                        Implantado = x.TipoControlImplantado,
                        Implantar = x.ControlesImplantar,
                        NroProceso = (int)db.tblBIAProceso.Where(y => y.IdEmpresa == x.IdEmpresa && y.IdDocumentoBia == x.IdDocumentoBIA && y.IdProceso == x.IdProceso).FirstOrDefault().NroProceso
                    }).ToList();
            }

            return Resultado;
        }
        private static List<long> GetUnidadesDependientes(long idUnidadOrganizativa)
        {
            List<long> IdUnidades = new List<long>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                List<tblUnidadOrganizativa> Unidades = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUnidadPadre == idUnidadOrganizativa).ToList();

                if (Unidades != null && Unidades.Count > 0)
                {
                    IdUnidades.AddRange(Unidades.Select(x => x.IdUnidadOrganizativa).ToList());
                    foreach (tblUnidadOrganizativa unidad in Unidades)
                    {
                        IdUnidades.AddRange(GetUnidadesDependientes(unidad.IdUnidadOrganizativa));
                    }
                }
            }

            return IdUnidades;
        }
        public static void ProcesarDocumentosModulo(long idEmpresaSelected, long idModuloActualiza)
        {
            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string _IdModulo = idModuloActualiza.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Length == 7 ? _IdModulo.Substring(0, 1) : _IdModulo.Substring(0, 2));

            Session["IdTipoDocumento"] = IdTipoDocumento;
            Session["IdEmpresa"] = idEmpresaSelected;

            using (Entities db = new Entities())
            {
                var data = db.tblDocumentoContenido.Where(x => x.IdEmpresa == idEmpresaSelected
                                                            && x.IdTipoDocumento == IdTipoDocumento)
                                .Select(x => new
                                {
                                    IdModulo = x.IdSubModulo,
                                    IdDocumento = x.IdDocumento,
                                    Contenido = x.ContenidoBin
                                }).OrderBy(x => x.IdModulo).ToList();

                foreach (var dato in data)
                {
                    Session["IdDocumento"] = dato.IdDocumento;
                    ProcesarDocumento.ProcesarContenidoDocumento(dato.IdModulo, dato.Contenido);
                }
            }

            Session["IdEmpresa"] = _IdEmpresa;
        }
        public static object GetDataGraficoRiesgoProbabilidad()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAAmenaza
                                  let Escala = d.tblProbabilidadRiesgo.EtiquetaGrafico
                                  where d.IdEmpresa == IdEmpresa && db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault() != null
                                  group d by new
                                  {
                                      db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRiesgoImpacto()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAAmenaza
                                  let Escala = d.tblImpactoRiesgo.Nombre
                                  where d.IdEmpresa == IdEmpresa && db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault() != null
                                  group d by new
                                  {
                                      db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRiesgoSeveridad()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAAmenaza
                                  let Escala = d.tblSeveridadRiesgo.Nombre
                                  where d.IdEmpresa == IdEmpresa && db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault() != null
                                  group d by new
                                  {
                                      db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRiesgoFuente()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAAmenaza
                                  let Escala = d.tblFuenteRiesgo.Nombre
                                  where d.IdEmpresa == IdEmpresa && db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault() != null
                                  group d by new
                                  {
                                      db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static object GetDataGraficoRiesgoControl()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<objGraphIO> Resultado = new List<objGraphIO>();

            using (Entities db = new Entities())
            {
                var objMTDData = (from d in db.tblBIAAmenaza
                                  let Escala = d.tblControlRiesgo.Nombre
                                  where d.IdEmpresa == IdEmpresa && db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault() != null
                                  group d by new
                                  {
                                      db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumentoBia == d.IdDocumentoBIA && x.IdProceso == d.IdProceso).FirstOrDefault().tblBIADocumento.IdUnidadOrganizativa,
                                      Escala
                                  } into gcs
                                  select new
                                  {
                                      IdUnidad = (long)gcs.Key.IdUnidadOrganizativa,
                                      ValorEscala = gcs.Key.Escala,
                                      CantidadEscala = gcs.Count()
                                  }).OrderBy(x => x.ValorEscala).ToList();

                foreach (var objInfo in objMTDData)
                {
                    objGraphIO objRes;
                    long IdUnidadPrincipal = GetUnidadPrincipal(objInfo.IdUnidad);
                    objRes = Resultado.Find(delegate (objGraphIO objCIO)
                    {
                        return (objCIO.IdUnidad == IdUnidadPrincipal)
                            && (objCIO.Escala == objInfo.ValorEscala);
                    });
                    if (objRes == null)
                    {
                        objRes = new objGraphIO();
                        objRes.Escala = objInfo.ValorEscala;
                        objRes.IdUnidad = IdUnidadPrincipal;
                        objRes.Unidad = GetNombreUnidadCompleto(IdUnidadPrincipal);
                        objRes.Cantidad = 0;
                        Resultado.Add(objRes);
                    }
                    objRes.Cantidad += objInfo.CantidadEscala;
                }
            }

            return Resultado.OrderBy(x => x.Unidad).ToList();
        }
        public static IQueryable<AuditoriaModel> GetAuditoria(DateTime? FechaDesde = null, DateTime? FechaHasta = null, long? IdUsuario = null)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            IList<AuditoriaModel> data = new List<AuditoriaModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblAuditoria
                        let fechaRegistro = SqlFunctions.DateAdd("mi", Minutos, d.FechaRegistro)
                        where d.IdEmpresa == IdEmpresa
                             && fechaRegistro >= FechaDesde
                             && fechaRegistro <= FechaHasta
                             && d.IdUsuario == (long)(IdUsuario == null ? d.IdUsuario : IdUsuario)
                        select new AuditoriaModel
                        {
                            Accion = d.Accion,
                            datosmodificados = d.DatosModificados,
                            DireccionIP = d.DireccionIP,
                            Empresa = d.tblEmpresa.NombreComercial,
                            FechaRegistro = (DateTime)fechaRegistro,
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
                            NroDocumento = (d.tblDocumento != null ? d.tblDocumento.NroDocumento : 0),
                            TipoDocumento = string.Empty,
                        }).ToList();
            }
            return data.OrderByDescending(x => x.FechaRegistro).AsQueryable();
        }
        public static List<TablaModel> GetUsuariosAuditoria()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            List<TablaModel> data;

            using (Entities db = new Entities())
            {
                data = (from d in db.tblAuditoria
                        where d.IdEmpresa == IdEmpresa
                        select new TablaModel
                        {
                            Descripcion = d.tblUsuario.Nombre,
                            Id = d.tblUsuario.IdUsuario
                        }).Distinct().ToList();
            }

            data = (data == null ? new List<TablaModel>() : data.OrderBy(x => x.Descripcion).ToList());
            data.Insert(0, new TablaModel
            {
                Descripcion = Resources.AdministracionResource.itemAllUsers,
                Id = -1
            });

            return data;

        }
        public static string GetNombreEscala(eTipoEscala TipoEscala)
        {
            string data = string.Empty;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                data = db.tblTipoEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == (int)TipoEscala)
                        .Select(x => x.Descripcion).FirstOrDefault();
            }

            return data;
        }
        public static List<ProcesoImpactoModel> GetProcesosAjustarImpacto(eTipoEscala TipoEscala)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            using (Entities db = new Entities())
            {
                // List<tblBIAProceso> _Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa).ToList();

                switch (TipoEscala)
                {
                    case eTipoEscala.ImpactoFinanciero:
                        List<ProcesoImpactoModel> _IF = new List<ProcesoImpactoModel>();

                        _IF = db.tblBIAImpactoFinanciero.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoImpactoModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         Impacto = x.Impacto,
                                         IdEscala = x.IdEscala ?? 0
                                     }).Distinct().ToList();
                        return _IF.OrderBy(x => x.Impacto).ToList();
                    case eTipoEscala.ImpactoOperacional:
                        List<ProcesoImpactoModel> _IO = new List<ProcesoImpactoModel>();
                        _IO = db.tblBIAImpactoOperacional.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoImpactoModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         Impacto = x.ImpactoOperacional,
                                         IdEscala = x.IdEscala ?? 0
                                     }).Distinct().ToList();
                        return _IO.OrderBy(x => x.Impacto).ToList();
                    default:
                        return null;
                }

            }

        }
        public static List<ProcesoValoresModel> GetProcesosAjustarValores(eTipoEscala TipoEscala)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());


            using (Entities db = new Entities())
            {
                // List<tblBIAProceso> _Procesos = db.tblBIAProceso.Where(x => x.IdEmpresa == IdEmpresa).ToList();

                switch (TipoEscala)
                {
                    case eTipoEscala.MTD:
                        List<ProcesoValoresModel> _MTD = new List<ProcesoValoresModel>();
                        _MTD = db.tblBIAMTD.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoValoresModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         IdDocumentoBIA = x.IdDocumentoBIA,
                                         IdImpacto = x.IdMTD,
                                         IdProceso = x.IdProceso,
                                         IdTipoEscala = x.IdEscala ?? 0,
                                         Impacto = x.Observacion,
                                         NombreProceso = x.tblBIAProceso.Nombre,
                                         NroProceso = x.tblBIAProceso.NroProceso ?? 0
                                     }).Distinct().ToList();
                        return _MTD.OrderBy(x => x.NroProceso).ToList();
                    case eTipoEscala.RPO:
                        List<ProcesoValoresModel> _RPO = new List<ProcesoValoresModel>();
                        _RPO = db.tblBIARPO.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoValoresModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         IdDocumentoBIA = x.IdDocumentoBIA,
                                         IdImpacto = x.IdRPO,
                                         IdProceso = x.IdProceso,
                                         IdTipoEscala = x.IdEscala,
                                         Impacto = x.Observacion,
                                         NombreProceso = x.tblBIAProceso.Nombre,
                                         NroProceso = x.tblBIAProceso.NroProceso ?? 0
                                     }).Distinct().ToList();
                        return _RPO.OrderBy(x => x.NroProceso).ToList();
                    case eTipoEscala.RTO:
                        List<ProcesoValoresModel> _RTO = new List<ProcesoValoresModel>();
                        _RTO = db.tblBIARTO.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoValoresModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         IdDocumentoBIA = x.IdDocumentoBIA,
                                         IdImpacto = x.IdRTO,
                                         IdProceso = x.IdProceso,
                                         IdTipoEscala = x.IdEscala,
                                         Impacto = x.Observacion,
                                         NombreProceso = x.tblBIAProceso.Nombre,
                                         NroProceso = x.tblBIAProceso.NroProceso ?? 0
                                     }).Distinct().ToList();
                        return _RTO.OrderBy(x => x.NroProceso).ToList();
                    case eTipoEscala.WRT:
                        List<ProcesoValoresModel> _WRT = new List<ProcesoValoresModel>();
                        _WRT = db.tblBIAWRT.Where(x => x.IdEmpresa == IdEmpresa)
                                     .Select(x => new ProcesoValoresModel
                                     {
                                         IdEmpresa = IdEmpresa,
                                         IdDocumentoBIA = x.IdDocumentoBIA,
                                         IdImpacto = x.IdWRT,
                                         IdProceso = x.IdProceso,
                                         IdTipoEscala = x.IdEscala ?? 0,
                                         Impacto = x.Observacion,
                                         NombreProceso = x.tblBIAProceso.Nombre,
                                         NroProceso = x.tblBIAProceso.NroProceso ?? 0
                                     }).Distinct().ToList();
                        return _WRT.OrderBy(x => x.NroProceso).ToList();
                    default:
                        return null;
                }

            }

        }
        public static List<TipoEscalaGrafico> GetDataComboEscala(eTipoEscala TipoEscala)
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdTipoEscala = (int)TipoEscala;

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblEscala.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoEscala == IdTipoEscala)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdEscala,
                        TipoEscala = x.Descripcion
                    }).ToList();
            }

            EscalaGrafico.Insert(0, new TipoEscalaGrafico
            {
                IdTipoEscala = 0,
                TipoEscala = Resources.BCMWebPublic.itemSelectValue
            });

            return EscalaGrafico;
        }
        public static List<DocumentoProcesoModel> GetProcesosEscala(List<long> ImpactoF, List<long> ImpactoO, List<long> MTD,
                                                                    List<long> RPO, List<long> RTO, List<long> WRT)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();

            using (Entities db = new Entities())
            {
                Procesos = db.tblBIAProceso
                    .Where(x => x.IdEmpresa == IdEmpresa
                            && ImpactoF.Contains((long)(x.tblBIAImpactoFinanciero == null ? x.tblBIAImpactoFinanciero.FirstOrDefault().IdEscala : 0))
                            && ImpactoO.Contains((long)(x.tblBIAImpactoOperacional == null ? x.tblBIAImpactoOperacional.FirstOrDefault().IdEscala : 0))
                            && MTD.Contains((long)(x.tblBIAMTD == null ? x.tblBIAMTD.FirstOrDefault().IdEscala : 0))
                            && RPO.Contains((long)(x.tblBIARPO == null ? x.tblBIARPO.FirstOrDefault().IdEscala : 0))
                            && RTO.Contains((long)(x.tblBIARTO == null ? x.tblBIARTO.FirstOrDefault().IdEscala : 0))
                            && WRT.Contains((long)(x.tblBIAWRT == null ? x.tblBIAWRT.FirstOrDefault().IdEscala : 0))
                            )
                    .Select(x => new DocumentoProcesoModel
                    {
                        Critico = (bool)x.Critico,
                        Descripcion = x.Descripcion,
                        FechaCreacion = (DateTime)x.FechaCreacion,
                        FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                        IdEstatus = (long)x.IdEstadoProceso,
                        IdProceso = x.IdProceso,
                        Nombre = x.Nombre,
                        NroProceso = (int)x.NroProceso,
                        ImpactoFinanciero = string.Empty
                    }).Distinct().ToList();
            }

            return Procesos.OrderBy(x => x.Nombre).ToList();
        }

        public static List<DocumentoProcesoModel> GetProcesosByImpacto()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();
            List<string> ValoresIF = (Session["ValoresIF"] != null ? Session["ValoresIF"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresIO = (Session["ValoresIO"] != null ? Session["ValoresIO"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresMTD = (Session["ValoresMTD"] != null ? Session["ValoresMTD"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresRTO = (Session["ValoresRTO"] != null ? Session["ValoresRTO"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresRPO = (Session["ValoresRPO"] != null ? Session["ValoresRPO"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresWRT = (Session["ValoresWRT"] != null ? Session["ValoresWRT"].ToString().Split(',').ToList() : new List<string>());
            bool _firstTime = (ValoresIF.Count() == 0 && ValoresIO.Count() == 0 && ValoresMTD.Count() == 0 && ValoresRTO.Count() == 0 && ValoresRPO.Count() == 0 && ValoresWRT.Count() == 0);

            using (Entities db = new Entities())
            {

                Procesos = (from x in db.tblBIAProceso
                            let ImpFin = (from d in db.tblBIAImpactoFinanciero where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let ImpOper = (from d in db.tblBIAImpactoOperacional where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let MTD = (from d in db.tblBIAMTD where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let RPO = (from d in db.tblBIARPO where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let RTO = (from d in db.tblBIARTO where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let WRT = (from d in db.tblBIAWRT where d.IdEmpresa == x.IdEmpresa && d.IdDocumentoBIA == x.IdDocumentoBia && d.IdProceso == x.IdProceso select d).FirstOrDefault()
                            let selected = (ValoresIF.Contains((ImpFin == null || ImpFin.IdEscala == null ? long.MaxValue.ToString() : ImpFin.IdEscala.ToString())) ||
                                            ValoresIO.Contains((ImpOper == null || ImpOper.IdEscala == null ? long.MaxValue.ToString() : ImpOper.IdEscala.ToString())) ||
                                            ValoresMTD.Contains((MTD == null || MTD.IdEscala == null ? long.MaxValue.ToString() : MTD.IdEscala.ToString())) ||
                                            ValoresRPO.Contains((RPO == null ? long.MaxValue.ToString() : RPO.IdEscala.ToString())) ||
                                            ValoresRTO.Contains((RTO == null ? long.MaxValue.ToString() : RTO.IdEscala.ToString())) ||
                                            ValoresWRT.Contains((WRT == null || WRT.IdEscala == null ? long.MaxValue.ToString() : WRT.IdEscala.ToString())) ||
                                            (_firstTime && (x.Critico ?? false)))
                            where x.IdEmpresa == IdEmpresa
                            select new DocumentoProcesoModel
                            {
                                Critico = x.Critico ?? false,
                                Descripcion = x.Descripcion,
                                FechaCreacion = (DateTime)x.FechaCreacion,
                                FechaEstatus = (DateTime)x.FechaUltimoEstatus,
                                IdEstatus = (long)x.IdEstadoProceso,
                                IdProceso = x.IdProceso,
                                ImpactoFinanciero = (ImpFin == null || ImpFin.tblEscala == null ? string.Empty : ImpFin.tblEscala.Descripcion),
                                ImpactoOperacional = (ImpOper == null || ImpOper.tblEscala == null ? string.Empty : ImpOper.tblEscala.Descripcion),
                                MTD = (MTD == null || MTD.tblEscala == null ? string.Empty : MTD.tblEscala.Descripcion),
                                Nombre = x.Nombre,
                                NroProceso = (int)x.NroProceso,
                                RPO = (RPO == null || RPO.tblEscala == null ? string.Empty : RPO.tblEscala.Descripcion),
                                RTO = (RTO == null || RTO.tblEscala == null ? string.Empty : RTO.tblEscala.Descripcion),
                                Selected = selected,
                                WRT = (WRT == null || WRT.tblEscala == null ? string.Empty : WRT.tblEscala.Descripcion),
                            }).Distinct().ToList();
            }

            return Procesos.OrderByDescending(x => x.Selected).ThenBy(x => x.NroProceso).ToList();
        }
        public static List<TipoEscalaGrafico> GetDataComboProbabilidad()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblProbabilidadRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdProbabilidad,
                        TipoEscala = x.Nombre
                    }).ToList();
            }
            return EscalaGrafico;
        }
        public static List<TipoEscalaGrafico> GetDataComboImpacto()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblImpactoRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdImpacto,
                        TipoEscala = x.Nombre
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static List<TipoEscalaGrafico> GetDataComboSeveridad()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblSeveridadRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdSeveridad,
                        TipoEscala = x.Nombre
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static List<TipoEscalaTexto> GetDataComboFuente()
        {
            List<TipoEscalaTexto> EscalaGrafico = new List<TipoEscalaTexto>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblFuenteRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TipoEscalaTexto
                    {
                        IdTipoEscala = x.CodigoFuente,
                        TipoEscala = x.Nombre
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static List<TipoEscalaGrafico> GetDataComboControl()
        {
            List<TipoEscalaGrafico> EscalaGrafico = new List<TipoEscalaGrafico>();
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                EscalaGrafico = db.tblControlRiesgo.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new TipoEscalaGrafico
                    {
                        IdTipoEscala = x.IdControl,
                        TipoEscala = x.Nombre
                    }).ToList();
            }

            return EscalaGrafico;
        }
        public static List<DocumentoProcesoModel> GetProcesosByRiesgo()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<DocumentoProcesoModel> Procesos = new List<DocumentoProcesoModel>();
            List<string> ValoresProbabilidad = (Session["ValoresProbabilidad"] != null ? Session["ValoresProbabilidad"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresImpacto = (Session["ValoresImpacto"] != null ? Session["ValoresImpacto"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresSeveridad = (Session["ValoresSeveridad"] != null ? Session["ValoresSeveridad"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresFuente = (Session["ValoresFuente"] != null ? Session["ValoresFuente"].ToString().Split(',').ToList() : new List<string>());
            List<string> ValoresControl = (Session["ValoresControl"] != null ? Session["ValoresControl"].ToString().Split(',').ToList() : new List<string>());

            using (Entities db = new Entities())
            {
                Procesos = (from x in db.tblBIAAmenaza
                            let selected = (ValoresProbabilidad.Contains(x.tblProbabilidadRiesgo.IdProbabilidad.ToString()) ||
                                           ValoresImpacto.Contains(x.tblImpactoRiesgo.IdImpacto.ToString()) ||
                                           ValoresSeveridad.Contains(x.tblSeveridadRiesgo.IdSeveridad.ToString()) ||
                                           ValoresFuente.Contains(x.tblFuenteRiesgo.CodigoFuente) ||
                                           ValoresControl.Contains(x.tblControlRiesgo.IdControl.ToString()))
                            let escalaProbabilidad = (x.tblProbabilidadRiesgo == null ? string.Empty : x.tblProbabilidadRiesgo.Nombre)
                            let escalaImpacto = (x.tblImpactoRiesgo == null ? string.Empty : x.tblImpactoRiesgo.Nombre)
                            let escalaSeveridad = (x.tblSeveridadRiesgo == null ? string.Empty : x.tblSeveridadRiesgo.Nombre)
                            let escalaFuente = (x.tblFuenteRiesgo == null ? string.Empty : x.tblFuenteRiesgo.Nombre)
                            let escalaControl = (x.tblControlRiesgo == null ? string.Empty : x.tblControlRiesgo.Nombre)
                            where x.IdEmpresa == IdEmpresa
                            select new DocumentoProcesoModel
                            {
                                Critico = (bool)x.tblBIAProceso.Critico,
                                Descripcion = x.tblBIAProceso.Descripcion,
                                FechaCreacion = (DateTime)x.tblBIAProceso.FechaCreacion,
                                FechaEstatus = (DateTime)x.tblBIAProceso.FechaUltimoEstatus,
                                IdEstatus = (long)x.tblBIAProceso.IdEstadoProceso,
                                IdProceso = x.tblBIAProceso.IdProceso,
                                ImpactoFinanciero = escalaProbabilidad,
                                ImpactoOperacional = escalaImpacto,
                                MTD = escalaSeveridad,
                                Nombre = x.tblBIAProceso.Nombre,
                                NroProceso = (int)x.tblBIAProceso.NroProceso,
                                RPO = escalaFuente,
                                RTO = escalaControl,
                                Selected = selected,
                                WRT = String.Empty,
                            }).Distinct().ToList();
            }

            return Procesos.OrderByDescending(x => x.Selected).ThenBy(x => x.NroProceso).ToList();
        }
        public static string GetEstatusIniciativa(long IdEstatus)
        {
            string _Estatus = string.Empty;

            using (Entities db = new Entities())
            {
                tblPlanTrabajoEstatus EstatusIniciativa = db.tblPlanTrabajoEstatus.Where(x => x.IdEstatusActividad == IdEstatus).FirstOrDefault();
                if (EstatusIniciativa != null)
                    _Estatus = EstatusIniciativa.tblCultura_PlanTrabajoEstatus.Where(x => x.Culture == Culture || x.Culture == "es-VE").FirstOrDefault().Descripcion;
            }

            return _Estatus;
        }
        public static List<IniciativaModel> GetIniciativas()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<IniciativaModel> Iniciativas = new List<IniciativaModel>();

            using (Entities db = new Entities())
            {
                List<tblIniciativas> data = db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa).ToList();

                foreach (tblIniciativas x in data)
                {
                    string _strPath = string.Format("{0}Content\\FileManager\\AnexosPlanTrabajo\\E{1}\\I{2}", Server.MapPath("/"), IdEmpresa.ToString("000"), x.IdIniciativa);
                    bool _hasFiles = false;

                    if (Directory.Exists(_strPath))
                        _hasFiles = Directory.GetFiles(_strPath, "*.*", SearchOption.AllDirectories).Count() > 0;

                    Iniciativas.Add(new IniciativaModel
                    {
                        Descripcion = (x.Descripcion != null ? x.Descripcion : string.Empty),
                        FechaCierreEstimada = x.FechaCierreEstimada,
                        FechaCierreReal = x.FechaCierreReal,
                        FechaInicioEstimada = x.FechaInicioEstimada,
                        FechaInicioReal = x.FechaInicioReal,
                        hasFiles = _hasFiles,
                        HorasEstimadas = x.HorasEstimadas,
                        HorasInvertidas = x.HorasConsumidas,
                        IdEmpresa = x.IdEmpresa,
                        IdEstatus = (short)x.IdEstatusIniciativa,
                        IdIniciativa = x.IdIniciativa,
                        MontoAbonado = x.MontoAbonado,
                        MontoPendiente = x.MontoPendiente,
                        Nombre = x.Nombre,
                        NroIniciativa = (int)x.NroIniciativa,
                        Observacion = (x.Observacion != null ? x.Observacion : string.Empty),
                        PorcentajeAvance = x.PorcentajeAvance,
                        PresupuestoEstimado = (decimal)(x.PresupuestoEstimado != null ? x.PresupuestoEstimado : 0),
                        PresupuestoReal = (decimal)(x.PresupuestoReal != null ? x.PresupuestoReal : 0),
                        Responsable = x.NombreResponsable,
                        UnidadOrganizativa = x.UnidadOrganizativa,
                        Urgente = (short)x.IdPrioridad,
                    });
                }
            }

            return Iniciativas;
        }
        public static IniciativaModel GetEditableIniciativa(long IdIniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            IniciativaModel Iniciativa = new IniciativaModel();

            using (Entities db = new Entities())
            {
                tblIniciativas x = db.tblIniciativas.Where(d => d.IdEmpresa == IdEmpresa && d.IdIniciativa == IdIniciativa).FirstOrDefault();

                string _strPath = string.Format("{0}Content\\FileManager\\AnexosPlanTrabajo\\E{1}\\I{2}", Server.MapPath("/"), IdEmpresa.ToString("000"), x.IdIniciativa);
                bool _hasFiles = false;

                if (Directory.Exists(_strPath))
                    _hasFiles = Directory.GetFiles(_strPath, "*.*", SearchOption.AllDirectories).Count() > 0;

                Iniciativa = new IniciativaModel
                {
                    Descripcion = (x.Descripcion != null ? x.Descripcion : string.Empty),
                    FechaCierreEstimada = x.FechaCierreEstimada,
                    FechaCierreReal = x.FechaCierreReal,
                    FechaInicioEstimada = x.FechaInicioEstimada,
                    FechaInicioReal = x.FechaInicioReal,
                    hasFiles = _hasFiles,
                    HorasEstimadas = x.HorasEstimadas,
                    HorasInvertidas = x.HorasConsumidas,
                    IdEmpresa = x.IdEmpresa,
                    IdEstatus = (short)x.IdEstatusIniciativa,
                    IdIniciativa = x.IdIniciativa,
                    MontoAbonado = x.MontoAbonado,
                    MontoPendiente = x.MontoPendiente,
                    Nombre = x.Nombre,
                    NroIniciativa = (int)x.NroIniciativa,
                    Observacion = (x.Observacion != null ? x.Observacion : string.Empty),
                    PorcentajeAvance = x.PorcentajeAvance,
                    PresupuestoEstimado = (decimal)(x.PresupuestoEstimado != null ? x.PresupuestoEstimado : 0),
                    PresupuestoReal = (decimal)(x.PresupuestoReal != null ? x.PresupuestoReal : 0),
                    Responsable = x.NombreResponsable,
                    UnidadOrganizativa = x.UnidadOrganizativa,
                    Urgente = (short)x.IdPrioridad,
                };
            }

            return Iniciativa;
        }
        public static void InsertIniciativa(IniciativaModel Iniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                int Numero = GetNextNroIniciativa();
                tblIniciativas reg = new tblIniciativas
                {
                    Descripcion = Iniciativa.Descripcion,
                    FechaCierreEstimada = Iniciativa.FechaCierreEstimada,
                    FechaCierreReal = Iniciativa.FechaCierreReal,
                    FechaInicioEstimada = Iniciativa.FechaInicioEstimada,
                    FechaInicioReal = Iniciativa.FechaInicioReal,
                    HorasConsumidas = Iniciativa.HorasInvertidas,
                    HorasEstimadas = Iniciativa.HorasInvertidas,
                    IdEmpresa = IdEmpresa,
                    IdEstatusIniciativa = Iniciativa.IdEstatus,
                    IdPrioridad = (short)Iniciativa.Urgente,
                    MontoAbonado = Iniciativa.MontoAbonado,
                    MontoPendiente = Iniciativa.MontoPendiente,
                    Nombre = Iniciativa.Nombre,
                    NombreResponsable = Iniciativa.Responsable,
                    NroIniciativa = Numero,
                    Observacion = Iniciativa.Observacion,
                    PorcentajeAvance = Iniciativa.PorcentajeAvance,
                    PresupuestoEstimado = Iniciativa.PresupuestoEstimado,
                    PresupuestoReal = Iniciativa.PresupuestoReal,
                    UnidadOrganizativa = Iniciativa.UnidadOrganizativa,
                };

                db.tblIniciativas.Add(reg);
                db.SaveChanges();

                Auditoria.RegistarIniciativa(eTipoAccion.AgregarIniciativa, reg.IdIniciativa, reg.Nombre, string.Empty);
            }
        }
        public static void UpdateIniciativa(IniciativaModel Iniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                tblIniciativas reg = db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa && x.IdIniciativa == Iniciativa.IdIniciativa).FirstOrDefault();

                reg.Descripcion = Iniciativa.Descripcion;
                reg.FechaCierreEstimada = Iniciativa.FechaCierreEstimada;
                reg.FechaCierreReal = Iniciativa.FechaCierreReal;
                reg.FechaInicioEstimada = Iniciativa.FechaInicioEstimada;
                reg.FechaInicioReal = Iniciativa.FechaInicioReal;
                reg.HorasConsumidas = Iniciativa.HorasInvertidas;
                reg.HorasEstimadas = Iniciativa.HorasEstimadas;
                reg.IdEstatusIniciativa = Iniciativa.IdEstatus;
                reg.IdPrioridad = Iniciativa.Urgente;
                reg.MontoAbonado = Iniciativa.MontoAbonado;
                reg.MontoPendiente = Iniciativa.MontoPendiente;
                reg.NombreResponsable = Iniciativa.Responsable;
                reg.Nombre = Iniciativa.Nombre;
                reg.NombreResponsable = (Iniciativa.Responsable == null ? string.Empty : Iniciativa.Responsable);
                reg.Observacion = Iniciativa.Observacion;
                reg.PorcentajeAvance = Iniciativa.PorcentajeAvance;
                reg.PresupuestoEstimado = Iniciativa.PresupuestoEstimado;
                reg.PresupuestoReal = Iniciativa.PresupuestoReal;
                reg.UnidadOrganizativa = Iniciativa.UnidadOrganizativa;

                db.SaveChanges();
            }
        }
        public static string GetDatosActualizados(IniciativaModel iniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            tblIniciativas reg = null;

            using (Entities db = new Entities())
            {
                reg = db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa && x.IdIniciativa == iniciativa.IdIniciativa).FirstOrDefault();
            }

            string Actualizaciones = string.Empty;

            if (reg != null)
            {
                if (reg.Descripcion != iniciativa.Descripcion)
                {
                    string _valorAnterior = (reg.Descripcion == null ? string.Empty : reg.Descripcion);
                    string _valorActual = (string)(iniciativa.Descripcion == null ? string.Empty : iniciativa.Descripcion);
                    Actualizaciones += string.Format("Descripción de la iniciativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.FechaInicioEstimada != iniciativa.FechaInicioEstimada)
                {
                    string _valorAnterior = (reg.FechaInicioEstimada == null ? string.Empty : ((DateTime)reg.FechaInicioEstimada).ToString("dd/MM/yyyy"));
                    string _valorActual = (iniciativa.FechaInicioEstimada == null ? string.Empty : ((DateTime)iniciativa.FechaInicioEstimada).ToString("dd/MM/yyyy"));
                    Actualizaciones += string.Format("Fecha de inicio estimada de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.FechaCierreEstimada != iniciativa.FechaCierreEstimada)
                {
                    string _valorAnterior = (reg.FechaCierreEstimada == null ? string.Empty : ((DateTime)reg.FechaCierreEstimada).ToString("dd/MM/yyyy"));
                    string _valorActual = (iniciativa.FechaCierreEstimada == null ? string.Empty : ((DateTime)iniciativa.FechaCierreEstimada).ToString("dd/MM/yyyy"));
                    Actualizaciones += string.Format("Fecha de cierre estimada de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.FechaInicioReal != iniciativa.FechaInicioReal)
                {
                    string _valorAnterior = (reg.FechaInicioReal == null ? string.Empty : ((DateTime)reg.FechaInicioReal).ToString("dd/MM/yyyy"));
                    string _valorActual = (iniciativa.FechaInicioReal == null ? string.Empty : ((DateTime)iniciativa.FechaInicioReal).ToString("dd/MM/yyyy"));
                    Actualizaciones += string.Format("Fecha de inicio real de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.FechaCierreReal != iniciativa.FechaCierreReal)
                {
                    string _valorAnterior = (reg.FechaCierreReal == null ? string.Empty : ((DateTime)reg.FechaCierreReal).ToString("dd/MM/yyyy"));
                    string _valorActual = (iniciativa.FechaCierreReal == null ? string.Empty : ((DateTime)iniciativa.FechaCierreReal).ToString("dd/MM/yyyy"));
                    Actualizaciones += string.Format("Fecha de cierre real de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.IdEstatusIniciativa != iniciativa.IdEstatus)
                {
                    string _valorAnterior = (reg.IdEstatusIniciativa == null ? string.Empty : GetEstatusIniciativa((short)reg.IdEstatusIniciativa));
                    string _valorActual = (iniciativa.IdEstatus == null ? string.Empty : GetEstatusIniciativa((short)iniciativa.IdEstatus));
                    Actualizaciones += string.Format("Estado de la iniciativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.Nombre != iniciativa.Nombre)
                {
                    string _valorAnterior = (reg.Nombre == null ? string.Empty : reg.Nombre);
                    string _valorActual = (iniciativa.Nombre == null ? string.Empty : iniciativa.Nombre);
                    Actualizaciones += string.Format("Nombre de la iniciativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if ((reg.PresupuestoEstimado == null ? 0 : reg.PresupuestoEstimado) != (iniciativa.PresupuestoEstimado == null ? 0 : iniciativa.PresupuestoEstimado))
                {
                    string _valorAnterior = (reg.PresupuestoEstimado == null ? string.Empty : ((decimal)reg.PresupuestoEstimado).ToString("#.##0.00"));
                    string _valorActual = (iniciativa.PresupuestoEstimado == null ? string.Empty : ((decimal)iniciativa.PresupuestoEstimado).ToString("#.##0.00"));
                    Actualizaciones += string.Format("Presupuesto estimado de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if ((reg.PresupuestoReal == null ? 0 : reg.PresupuestoReal) != (iniciativa.PresupuestoReal == null ? 0 : iniciativa.PresupuestoReal))
                {
                    string _valorAnterior = (reg.PresupuestoReal == null ? string.Empty : ((decimal)reg.PresupuestoReal).ToString("#.##0.00"));
                    string _valorActual = (iniciativa.PresupuestoReal == null ? string.Empty : ((decimal)iniciativa.PresupuestoReal).ToString("#.##0.00"));
                    Actualizaciones += string.Format("Presupuesto real de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.MontoAbonado != iniciativa.MontoAbonado)
                {
                    string _valorAnterior = (reg.MontoAbonado == null ? string.Empty : ((double)reg.MontoAbonado).ToString("#,##0.00"));
                    string _valorActual = (iniciativa.MontoAbonado == null ? string.Empty : ((double)iniciativa.MontoAbonado).ToString("#,##0.00"));
                    Actualizaciones += string.Format("Monto abonado de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.MontoPendiente != iniciativa.MontoPendiente)
                {
                    string _valorAnterior = (reg.MontoPendiente == null ? string.Empty : ((double)reg.MontoPendiente).ToString("#,##0.00"));
                    string _valorActual = (iniciativa.MontoPendiente == null ? string.Empty : ((double)iniciativa.MontoPendiente).ToString("#,##0.00"));
                    Actualizaciones += string.Format("Monto pendiente de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.NombreResponsable != iniciativa.Responsable)
                {
                    string _valorAnterior = (reg.NombreResponsable == null ? string.Empty : reg.NombreResponsable);
                    string _valorActual = (iniciativa.Responsable == null ? string.Empty : iniciativa.Responsable);
                    Actualizaciones += string.Format("Responsable de la iniciativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.IdPrioridad != iniciativa.Urgente)
                {
                    string _valorAnterior = (reg.IdPrioridad == null ? string.Empty : Metodos.GetPrioridadIniciativa(reg.IdPrioridad));
                    string _valorActual = (iniciativa.Urgente == null ? string.Empty : Metodos.GetPrioridadIniciativa(iniciativa.Urgente));
                    Actualizaciones += string.Format("Prioridad de la iniciativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.HorasEstimadas != iniciativa.HorasEstimadas)
                {
                    string _valorAnterior = (reg.HorasEstimadas == null ? string.Empty : ((int)reg.HorasEstimadas).ToString("#0"));
                    string _valorActual = (iniciativa.HorasEstimadas == null ? string.Empty : ((int)iniciativa.HorasEstimadas).ToString("#0"));
                    Actualizaciones += string.Format("Horas estimadas de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.HorasConsumidas != iniciativa.HorasInvertidas)
                {
                    string _valorAnterior = (reg.HorasConsumidas == null ? string.Empty : ((int)reg.HorasConsumidas).ToString("#0"));
                    string _valorActual = (iniciativa.HorasInvertidas == null ? string.Empty : ((int)iniciativa.HorasInvertidas).ToString("#0"));
                    Actualizaciones += string.Format("Horas invertidas de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.PorcentajeAvance != iniciativa.PorcentajeAvance)
                {
                    string _valorAnterior = (reg.PorcentajeAvance == null ? string.Empty : ((double)reg.PorcentajeAvance).ToString("##0.00"));
                    string _valorActual = (iniciativa.PorcentajeAvance == null ? string.Empty : ((double)iniciativa.PorcentajeAvance).ToString("##0.00"));
                    Actualizaciones += string.Format("Porcentaje de avance de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.Observacion != iniciativa.Observacion)
                {
                    string _valorAnterior = (reg.Observacion == null ? string.Empty : reg.Observacion);
                    string _valorActual = (iniciativa.Observacion == null ? string.Empty : iniciativa.Observacion);
                    Actualizaciones += string.Format("Observación de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
                if (reg.UnidadOrganizativa != iniciativa.UnidadOrganizativa)
                {
                    string _valorAnterior = (reg.UnidadOrganizativa == null ? string.Empty : reg.UnidadOrganizativa);
                    string _valorActual = (iniciativa.UnidadOrganizativa == null ? string.Empty : iniciativa.UnidadOrganizativa);
                    Actualizaciones += string.Format("Unidad Organizativa de \"{0}\" a \"{1}\"", _valorAnterior, _valorActual) + Environment.NewLine;
                }
            }

            if (string.IsNullOrEmpty(Actualizaciones))
                Actualizaciones = "Se editó pero sin modificar datos";
            else
                Actualizaciones = "Información Actualizada" + Environment.NewLine + Environment.NewLine + Actualizaciones;

            return Actualizaciones;
        }
        public static string GetPrioridadIniciativa(short? idPrioridad)
        {
            string _Prioridad = string.Empty;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                _Prioridad = db.tblIniciativaPrioridad.Where(x => x.IdEmpresa == IdEmpresa && x.IdPrioridad == idPrioridad).FirstOrDefault().ToString();
            }

            return _Prioridad;
        }
        public static void DeleteIniciativa(long IdIniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string NombreIniciativa = string.Empty;

            using (Entities db = new Entities())
            {
                tblIniciativas reg = db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa && x.IdIniciativa == IdIniciativa).FirstOrDefault();

                int NroIniciativa = (int)reg.NroIniciativa;
                IdIniciativa = reg.IdIniciativa;
                NombreIniciativa = reg.Nombre;

                db.tblIniciativas.Remove(reg);

                List<tblIniciativas> Iniciativas = db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa && x.NroIniciativa > NroIniciativa).ToList();

                foreach (tblIniciativas Iniciativa in Iniciativas.OrderBy(x => x.NroIniciativa).ToList())
                {
                    Iniciativa.NroIniciativa -= 1;
                }
                db.SaveChanges();

                Auditoria.RegistarIniciativa(eTipoAccion.EliminarIniciativa, IdIniciativa, NombreIniciativa, string.Empty);
            }
        }
        public static List<TablaModel> GetEstadosIniciativa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblPlanTrabajoEstatus.Select(x => new TablaModel
                {
                    Descripcion = x.tblCultura_PlanTrabajoEstatus.Where(y => y.Culture == Culture || y.Culture == "es-VE").FirstOrDefault().Descripcion,
                    Id = x.IdEstatusActividad
                }).ToList();
            }

            return data;
        }
        public static List<TablaModel> GetPrioridadesIniciativa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblIniciativaPrioridad.Where(x => x.IdEmpresa == IdEmpresa).Select(x => new TablaModel
                {
                    Descripcion = x.Nombre,
                    Id = x.IdPrioridad
                }).ToList();
            }

            return data;
        }
        public static int GetNextNroIniciativa()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int NextNroIniciativa = 0;

            using (Entities db = new Entities())
            {

                if (db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa).Count() > 0)
                {
                    int? _maxNroIniciativa = (int)db.tblIniciativas.Where(x => x.IdEmpresa == IdEmpresa).Max(x => x.NroIniciativa);
                    if (_maxNroIniciativa != null) NextNroIniciativa = (int)_maxNroIniciativa;
                }
            }

            NextNroIniciativa++;
            return NextNroIniciativa;
        }
        public static RegisterModel GetUsuario()
        {

            long IdUsuario = long.Parse(Session["UserId"].ToString());
            RegisterModel data = new RegisterModel();

            Encriptador Encriptar = new Encriptador();

            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUsuario).FirstOrDefault();

                data = new RegisterModel
                {
                    ConfirmPassword = Encriptar.Desencriptar(usuario.ClaveUsuario, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256),
                    Password = Encriptar.Desencriptar(usuario.ClaveUsuario, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256),
                    UserName = usuario.Nombre
                };
            }

            return data;
        }
        public static void UpdateUsuario(string Nombre, string Contraseña)
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            Encriptador Encriptar = new Encriptador();

            string _ecryptPassword = Encriptar.Encriptar(Contraseña, Encriptador.HasAlgorimt.SHA1, Encriptador.Keysize.KS256);

            using (Entities db = new Entities())
            {
                tblUsuario reg = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();
                reg.Nombre = Nombre;
                reg.PrimeraVez = false;
                reg.ClaveUsuario = _ecryptPassword;

                db.SaveChanges();
            }

        }
        public static List<AnexosIniciativaModel> GetAnexosIniciativas(long IdIniciativa)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<AnexosIniciativaModel> Anexos = new List<AnexosIniciativaModel>();

            using (Entities db = new Entities())
            {
                Anexos = db.tblIniciativas_Anexo.Where(x => x.IdEmpresa == IdEmpresa && x.IdIniciativa == IdIniciativa)
                    .Select(x => new AnexosIniciativaModel
                    {
                        fechaRegistro = x.fechaRegistro,
                        id = x.IdAnexo,
                        IdEmpresa = x.IdEmpresa,
                        IdIniciativa = x.IdIniciativa,
                        Nombre = x.Nombre,
                        Ruta = x.RutaArchivo,
                    }).ToList();
            }

            return Anexos;
        }
        public static List<TablaModel> GetIniciativaResponsables()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblIniciativaResponsable.Where(x => x.IdEmpresa == IdEmpresa).Select(x => new TablaModel
                {
                    Descripcion = x.Nombre,
                    Id = x.IdResponsable
                }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static string GetResponsableIniciativa(long IdResponsable)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string data;

            using (Entities db = new Entities())
            {
                data = db.tblIniciativaResponsable.Where(x => x.IdEmpresa == IdEmpresa && x.IdResponsable == IdResponsable).FirstOrDefault().Nombre;
            }

            return data;

        }
        public static List<TablaModel_short> GetPMT_Programacion_TipoActualizacion()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel_short> data = new List<TablaModel_short>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_PMTProgramacionTipoActualizacion.Where(x => x.Culture == Culture || x.Culture == "es-VE")
                         .Select(x => new TablaModel_short
                         {
                             Descripcion = x.Descripcion,
                             Id = x.IdTipoActualizacion
                         }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static List<TablaModel> GetPMT_Programacion_TipoNotificacion()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_PMTProgramacionTipoNotificacion.Where(x => x.Cultura == Culture || x.Cultura == "es-VE").Select(x => new TablaModel
                {
                    Descripcion = x.Descripcion,
                    Id = x.IdTipoNotificacion
                }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static List<ProgramacionModel> GetProgramaciones()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<ProgramacionModel> Programacion = new List<ProgramacionModel>();

            using (Entities db = new Entities())
            {
                Programacion = (from d in db.tblPMTProgramacion
                                let FechaHasta = SqlFunctions.DateAdd("d", -1, d.FechaFinal)
                                where d.IdEmpresa == IdEmpresa
                                select new ProgramacionModel
                                {
                                    FechaFinal = (DateTime)FechaHasta,
                                    FechaInicio = d.FechaInicio,
                                    IdClaseDocumento = (d.Negocios ? 1 : 2),
                                    IdEmpresa = d.IdEmpresa,
                                    IdEstadoProgramacion = d.IdEstado,
                                    IdModuloProgramacion = d.IdModulo,
                                    IdProgramacion = d.IdPMTProgramacion,
                                    IdTipoActualizacion = d.IdTipoActualizacion,
                                    IdTipoFrecuencia = d.IdTipoFrecuencia,
                                }).ToList();
            }

            return Programacion;
        }
        public static List<ProgramacionDocumentoModel> GetDocumentosProgramacion(long IdProgramacion, long IdTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<ProgramacionDocumentoModel> data = new List<ProgramacionDocumentoModel>();

            if (IdTipoDocumento == 4 || IdTipoDocumento == 7)
            {
                using (Entities db = new Entities())
                {

                    data = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdTipoDocumento == IdTipoDocumento)
                        .Select(x => new
                        {
                            x.IdEstadoDocumento,
                            x.FechaEstadoDocumento,
                            x.IdDocumento,
                            x.IdEmpresa,
                            x.IdTipoDocumento,
                            db.tblModulo.Where(m => m.IdEmpresa == IdEmpresa && m.IdCodigoModulo == x.IdTipoDocumento && m.IdModuloPadre == 0).FirstOrDefault().Nombre,
                            x.NroDocumento,
                            x.VersionOriginal,
                            x.NroVersion,
                            x.tblPMTProgramacionDocumentos
                        }).AsEnumerable()
                        .Select(dc => new ProgramacionDocumentoModel
                        {
                            Estado = (eEstadoDocumento)dc.IdEstadoDocumento,
                            FechaUltimoEstado = dc.FechaEstadoDocumento,
                            IdDocumento = dc.IdDocumento,
                            IdModulo = dc.IdTipoDocumento * 1000000,
                            IdTipoDocumento = dc.IdTipoDocumento,
                            NombreDocumento = dc.Nombre,
                            NroDocumento = dc.NroDocumento,
                            NroVersion = dc.NroVersion,
                            VersionOriginal = (int)dc.VersionOriginal,
                            Selected = (dc.tblPMTProgramacionDocumentos != null)
                        }).ToList();
                }
            }

            return data.OrderBy(x => x.NroDocumento).ThenBy(x => x.VersionOriginal).ThenBy(x => x.NroVersion).ToList();
        }
        public static List<ProgramacionUsuarioModel> GetUsuariosProgramacion(long IdProgramacion, long IdTipoDocumento)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<ProgramacionUsuarioModel> data = new List<ProgramacionUsuarioModel>();

            if (IdTipoDocumento != 4 || IdTipoDocumento != 7)
            {
                using (Entities db = new Entities())
                {
                    tblPMTProgramacion _programacion = db.tblPMTProgramacion.Where(x => x.IdPMTProgramacion == IdProgramacion).FirstOrDefault();

                    data = (from eu in db.tblEmpresaUsuario
                            let IdTipoNotificacion = eu.tblUsuario.tblPMTProgramacionUsuario.Where(x => x.IdPMTProgramacion == IdProgramacion).FirstOrDefault().IdTipoNotificacion
                            let TipoNotificacion = eu.tblUsuario.tblPMTProgramacionUsuario.Where(x => x.IdPMTProgramacion == IdProgramacion).FirstOrDefault().tblPMTProgramacionTipoNotificacion.tblCultura_PMTProgramacionTipoNotificacion.Where(clt => clt.Cultura == Culture || clt.Cultura == "es-VE").FirstOrDefault().Descripcion
                            where eu.IdEmpresa == IdEmpresa
                            select new ProgramacionUsuarioModel
                            {
                                FechaNotificacion = eu.tblUsuario.tblPMTProgramacionUsuario.Max(p => (DateTime)p.FechaUltimaNotificacion),
                                IdProgramacion = IdProgramacion,
                                IdTipoNotificacion = (short)(IdTipoNotificacion ?? 0),
                                IdUsuario = eu.IdUsuario,
                                Nombre = eu.tblUsuario.Nombre,
                                TipoNotificacion = (TipoNotificacion ?? string.Empty),
                            }).ToList();
                }
            }

            return data.OrderBy(x => x.Nombre).ToList();
        }
        public static ProgramacionModel GetEditableProgramacion(long IdProgramacion)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            ProgramacionModel Programacion = new ProgramacionModel();

            using (Entities db = new Entities())
            {
                Programacion = (from d in db.tblPMTProgramacion
                                let FechaHasta = SqlFunctions.DateAdd("d", -1, d.FechaFinal)
                                where d.IdEmpresa == IdEmpresa && d.IdPMTProgramacion == IdProgramacion
                                select new ProgramacionModel
                                {
                                    FechaFinal = (DateTime)FechaHasta,
                                    FechaInicio = d.FechaInicio,
                                    IdClaseDocumento = (d.Negocios ? 1 : 2),
                                    IdEmpresa = d.IdEmpresa,
                                    IdEstadoProgramacion = d.IdEstado,
                                    IdModuloProgramacion = d.IdModulo,
                                    IdProgramacion = d.IdPMTProgramacion,
                                    IdTipoActualizacion = d.IdTipoActualizacion,
                                    IdTipoFrecuencia = d.IdTipoFrecuencia,
                                }).FirstOrDefault();
            }

            return Programacion;
        }
        public static void InsertProgramacion(ProgramacionModel Programacion)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas > 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                tblPMTProgramacion reg = new tblPMTProgramacion
                {
                    FechaFinal = DateTime.Parse(Programacion.FechaFinal.AddDays(1).ToString("yyyy/MM/dd")),
                    FechaInicio = DateTime.Parse(Programacion.FechaInicio.ToString("yyyy/MM/dd")),
                    IdEmpresa = IdEmpresa,
                    IdEstado = Programacion.IdEstadoProgramacion,
                    IdModulo = Programacion.IdModuloProgramacion,
                    IdTipoActualizacion = 1, // Programacion.IdTipoActualizacion,
                    IdTipoFrecuencia = Programacion.IdTipoFrecuencia,
                    Negocios = Programacion.IdClaseDocumento == 1,
                };

                db.tblPMTProgramacion.Add(reg);

                db.SaveChanges();

                Auditoria.RegistarProgramacion(eTipoAccion.AgregarProgramacion, reg.IdPMTProgramacion, reg.IdModulo, string.Empty);
            }
        }
        public static void UpdateProgramacion(ProgramacionModel Programacion)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas > 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                tblPMTProgramacion reg = db.tblPMTProgramacion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPMTProgramacion == Programacion.IdProgramacion).FirstOrDefault();

                reg.FechaFinal = DateTime.Parse(Programacion.FechaFinal.AddDays(1).ToString("yyyy/MM/dd"));
                reg.FechaInicio = DateTime.Parse(Programacion.FechaInicio.ToString("yyyy/MM/dd"));
                reg.IdModulo = Programacion.IdModuloProgramacion;
                reg.IdTipoActualizacion = 1; // Programacion.IdTipoActualizacion;
                reg.IdTipoFrecuencia = Programacion.IdTipoFrecuencia;

                db.SaveChanges();
                Auditoria.RegistarProgramacion(eTipoAccion.ActualizarProgramacion, reg.IdPMTProgramacion, reg.IdModulo, string.Empty);
            }
        }
        public static void DeleteProgramacion(long IdProgramacion)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            string NombreProgramacion = string.Empty;

            using (Entities db = new Entities())
            {
                tblPMTProgramacion reg = db.tblPMTProgramacion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPMTProgramacion == IdProgramacion).FirstOrDefault();

                db.tblPMTProgramacionDocumentos.RemoveRange(reg.tblPMTProgramacionDocumentos);
                db.tblPMTProgramacionUsuario.RemoveRange(reg.tblPMTProgramacionUsuario);
                db.tblPMTProgramacion.Remove(reg);
                db.SaveChanges();
                Auditoria.RegistarProgramacion(eTipoAccion.EliminarProgramacion, reg.IdPMTProgramacion, reg.IdModulo, string.Empty);
            }
        }
        public static List<TablaModel> GetTipoFrecuencia()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_TipoFrecuencia.Where(x => x.Culture == Culture || x.Culture == "es-VE").Select(x => new TablaModel
                {
                    Descripcion = x.Descripcion,
                    Id = x.IdTipoFrecuencia
                }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static List<TablaModel> GetPMTTipoFrecuencia()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_TipoFrecuencia.Where(x => x.IdTipoFrecuencia > 6 && (x.Culture == Culture || x.Culture == "es-VE")).Select(x => new TablaModel
                {
                    Descripcion = x.Descripcion,
                    Id = x.IdTipoFrecuencia
                }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static List<PruebaModel> GetPruebas()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            List<PruebaModel> Pruebas = new List<PruebaModel>();

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                Pruebas = (from d in db.tblPBEPruebaPlanificacion
                           let FechaI = SqlFunctions.DateAdd("mi", Minutos, d.FechaInicio)
                           where d.IdEmpresa == IdEmpresa && d.Negocios == (IdTipoPruebas == 1)
                           select new PruebaModel
                           {
                               FechaInicio = (DateTime)FechaI,
                               IdClaseDocumento = IdTipoPruebas,
                               IdEmpresa = d.IdEmpresa,
                               IdEstatus = (short)d.IdEstatus,
                               IdPrueba = d.IdPlanificacion,
                               Nombre = d.Prueba,
                               Ubicacion = d.Lugar,
                               Proposito = d.Propósito,
                           }).ToList();
            }

            return Pruebas;
        }
        public static PruebaModel GetPrueba(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            PruebaModel Prueba = new PruebaModel();

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                Prueba = (from d in db.tblPBEPruebaPlanificacion
                          let FechaI = SqlFunctions.DateAdd("mi", Minutos, d.FechaInicio)
                          where d.IdEmpresa == IdEmpresa && d.Negocios == (IdTipoPruebas == 1) && d.IdPlanificacion == IdPrueba
                          select new PruebaModel
                          {
                              FechaInicio = (DateTime)FechaI,
                              IdClaseDocumento = IdTipoPruebas,
                              IdEmpresa = d.IdEmpresa,
                              IdEstatus = (short)d.IdEstatus,
                              IdPrueba = d.IdPlanificacion,
                              Nombre = d.Prueba,
                              Ubicacion = d.Lugar,
                              Proposito = d.Propósito,
                          }).FirstOrDefault();
            }

            return Prueba;
        }
        public static long AddPrueba(PruebaModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = -1;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas > 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacion reg = new tblPBEPruebaPlanificacion
                {
                    FechaInicio = data.FechaInicio.AddMinutes(Minutos),
                    IdEmpresa = IdEmpresa,
                    Lugar = data.Ubicacion,
                    Negocios = data.IdClaseDocumento == 1,
                    Propósito = data.Proposito,
                    Prueba = data.Nombre,
                    IdEstatus = -1,
                };

                db.tblPBEPruebaPlanificacion.Add(reg);
                db.SaveChanges();

                IdPrueba = reg.IdPlanificacion;
            }

            return IdPrueba;
        }
        public static void UpdatePrueba(PruebaModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = data.IdPrueba;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas > 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacion reg = db.tblPBEPruebaPlanificacion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba).FirstOrDefault();
                reg.FechaInicio = data.FechaInicio.AddMinutes(Minutos);
                reg.Lugar = data.Ubicacion;
                reg.Propósito = data.Proposito;
                reg.Prueba = data.Nombre;

                db.SaveChanges();
            }

        }
        public static List<TablaModel_short> GetEmpresasPruebaParticipantes(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            List<TablaModel_short> Empresas = new List<TablaModel_short>();

            using (Entities db = new Entities())
            {
                Empresas = (from d in db.tblPBEPruebaPlanificacionParticipante
                            where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                            orderby d.Empresa
                            select d.Empresa).Distinct().AsEnumerable()
                            .Select(x => new TablaModel_short
                            {
                                Descripcion = x
                            }).ToList();
            }

            return Empresas;
        }
        public static List<PruebaParticipanteModel> GetPruebaParticipantes(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaParticipanteModel> Participantes = new List<PruebaParticipanteModel>();

            using (Entities db = new Entities())
            {
                Participantes = (from d in db.tblPBEPruebaPlanificacionParticipante
                                 where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba
                                 select new PruebaParticipanteModel
                                 {
                                     CorreoElectronico = d.Correo,
                                     EmpresaParticipante = d.Empresa,
                                     IdClaseDocumento = IdTipoPruebas,
                                     IdEmpresa = IdEmpresa,
                                     IdParticipante = d.IdParticipante,
                                     IdPrueba = d.IdPlanificacion,
                                     NombreParticipante = d.Nombre,
                                     NroTelefono = d.Telefono,
                                     Planificado = true,
                                     Responsable = d.Responsable,
                                     Utilizado = db.tblPBEPruebaEjecucionEjercicioParticipante.Where(x => x.IdParticipante == d.IdParticipante).Count() > 0,
                                 }).ToList();
            }

            return Participantes;
        }
        public static PruebaParticipanteModel GetPruebaParticipante(long IdPrueba, long IdParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaParticipanteModel Participante = new PruebaParticipanteModel();

            using (Entities db = new Entities())
            {
                Participante = (from d in db.tblPBEPruebaPlanificacionParticipante
                                where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdParticipante == IdParticipante
                                select new PruebaParticipanteModel
                                {
                                    CorreoElectronico = d.Correo,
                                    EmpresaParticipante = d.Empresa,
                                    IdClaseDocumento = IdTipoPruebas,
                                    IdEmpresa = IdEmpresa,
                                    IdParticipante = d.IdParticipante,
                                    IdPrueba = d.IdPlanificacion,
                                    NombreParticipante = d.Nombre,
                                    NroTelefono = d.Telefono,
                                    Planificado = true,
                                    Responsable = d.Responsable,
                                    Utilizado = db.tblPBEPruebaEjecucionEjercicioParticipante.Where(x => x.IdParticipante == d.IdParticipante).Count() > 0,
                                }).FirstOrDefault();
            }

            return Participante;
        }
        public static long AddParticipante(PruebaParticipanteModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdParticipante = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionParticipante reg = new tblPBEPruebaPlanificacionParticipante
                {
                    Correo = data.CorreoElectronico,
                    Empresa = data.EmpresaParticipante,
                    IdEmpresa = IdEmpresa,
                    IdPlanificacion = IdPrueba,
                    Nombre = data.NombreParticipante,
                    Responsable = data.Responsable,
                    Telefono = data.NroTelefono,
                };

                db.tblPBEPruebaPlanificacionParticipante.Add(reg);
                db.SaveChanges();

                IdParticipante = reg.IdParticipante;
            }

            return IdParticipante;
        }
        public static void UpdateParticipante(PruebaParticipanteModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionParticipante reg = db.tblPBEPruebaPlanificacionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdParticipante == data.IdParticipante).FirstOrDefault();
                reg.Correo = data.CorreoElectronico;
                reg.Empresa = data.EmpresaParticipante;
                reg.Nombre = data.NombreParticipante;
                reg.Responsable = data.Responsable;
                reg.Telefono = data.NroTelefono;

                db.SaveChanges();
            }

        }
        public static void DeleteParticipante(long IdParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionParticipante reg = db.tblPBEPruebaPlanificacionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdParticipante == IdParticipante).FirstOrDefault();

                db.tblPBEPruebaPlanificacionEjercicioParticipante.RemoveRange(reg.tblPBEPruebaPlanificacionEjercicioParticipante);
                db.tblPBEPruebaPlanificacionParticipante.Remove(reg);

                db.SaveChanges();
            }

        }
        public static List<PruebaEjercicioModel> GetPruebaEjercicios(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaEjercicioModel> Ejercicios = new List<PruebaEjercicioModel>();

            using (Entities db = new Entities())
            {
                Ejercicios = (from d in db.tblPBEPruebaPlanificacionEjercicio
                              where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba
                              select new PruebaEjercicioModel
                              {
                                  Descripcion = d.Descripcion,
                                  DuracionHoras = d.Duracion,
                                  Ejecutado = db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba && x.IdEjercicio == d.IdEjercicio).Count() > 0,
                                  Inicio = d.FechaInicio,
                                  IdClaseDocumento = IdTipoPruebas,
                                  IdEmpresa = IdEmpresa,
                                  IdEjercicio = d.IdEjercicio,
                                  IdPrueba = d.IdPlanificacion,
                                  Nombre = d.Nombre,
                              }).ToList();
            }

            return Ejercicios;
        }
        public static PruebaEjercicioModel GetPruebaEjercicio(long IdPrueba, long IdEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaEjercicioModel Ejercicio = new PruebaEjercicioModel();

            using (Entities db = new Entities())
            {
                Ejercicio = (from d in db.tblPBEPruebaPlanificacionEjercicio
                             where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdEjercicio == IdEjercicio
                             select new PruebaEjercicioModel
                             {
                                 Descripcion = d.Descripcion,
                                 DuracionHoras = d.Duracion,
                                 Ejecutado = db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == d.IdEjercicio).Count() > 0,
                                 Inicio = d.FechaInicio,
                                 IdClaseDocumento = IdTipoPruebas,
                                 IdEmpresa = IdEmpresa,
                                 IdEjercicio = d.IdEjercicio,
                                 IdPrueba = d.IdPlanificacion,
                                 Nombre = d.Nombre,
                             }).FirstOrDefault();
            }

            return Ejercicio;
        }
        public static long AddEjercicio(PruebaEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdEjercicio = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicio reg = new tblPBEPruebaPlanificacionEjercicio
                {
                    Descripcion = data.Descripcion,
                    Duracion = data.DuracionHoras,
                    FechaInicio = data.Inicio,
                    IdEmpresa = IdEmpresa,
                    IdPlanificacion = IdPrueba,
                    Nombre = data.Nombre,
                };

                db.tblPBEPruebaPlanificacionEjercicio.Add(reg);
                db.SaveChanges();

                IdEjercicio = reg.IdEjercicio;
            }

            return IdEjercicio;
        }
        public static void UpdateEjercicio(PruebaEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicio reg = db.tblPBEPruebaPlanificacionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == data.IdEjercicio).FirstOrDefault();
                reg.Descripcion = data.Descripcion;
                reg.Duracion = data.DuracionHoras;
                reg.FechaInicio = data.Inicio;
                reg.Nombre = data.Nombre;

                db.SaveChanges();
            }

        }
        public static void DeleteEjercicio(long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicio reg = db.tblPBEPruebaPlanificacionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == idEjercicio).FirstOrDefault();

                if (reg.tblPBEPruebaPlanificacionEjercicioParticipante != null && reg.tblPBEPruebaPlanificacionEjercicioParticipante.Count() > 0)
                    db.tblPBEPruebaPlanificacionEjercicioParticipante.RemoveRange(reg.tblPBEPruebaPlanificacionEjercicioParticipante);
                if (reg.tblPBEPruebaPlanificacionEjercicioRecurso != null && reg.tblPBEPruebaPlanificacionEjercicioRecurso.Count() > 0)
                    db.tblPBEPruebaPlanificacionEjercicioRecurso.RemoveRange(reg.tblPBEPruebaPlanificacionEjercicioRecurso);

                db.tblPBEPruebaPlanificacionEjercicio.Remove(reg);
                db.SaveChanges();
            }

        }
        public static List<PruebaParticipanteEjercicioModel> GetPruebaParticipantesEjercicio(long idPrueba, long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaParticipanteEjercicioModel> data = new List<PruebaParticipanteEjercicioModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaPlanificacionEjercicioParticipante
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio
                        select new PruebaParticipanteEjercicioModel
                        {
                            IdEjercicio = idEjercicio,
                            IdParticipante = d.IdParticipante,
                            Responsable = d.Responsable
                        }).ToList();
            }

            return data;
        }
        public static PruebaParticipanteEjercicioModel GetPruebaParticipanteEjercicio(long IdPrueba, long IdEjercicio, long idParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaParticipanteEjercicioModel data = new PruebaParticipanteEjercicioModel();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaPlanificacionEjercicioParticipante
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdEjercicio == IdEjercicio && d.IdParticipante == idParticipante
                        select new PruebaParticipanteEjercicioModel
                        {
                            IdEjercicio = d.IdEjercicio,
                            IdParticipante = d.IdParticipante,
                            Responsable = d.Responsable
                        }).FirstOrDefault();
            }

            return data;
        }
        public static long AddParticipanteEjercicio(PruebaParticipanteEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdParticipante = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioParticipante reg = new tblPBEPruebaPlanificacionEjercicioParticipante
                {
                    IdEjercicio = data.IdEjercicio,
                    IdEmpresa = IdEmpresa,
                    IdParticipante = data.IdParticipante,
                    IdPlanificacion = IdPrueba,
                    Responsable = data.Responsable
                };

                db.tblPBEPruebaPlanificacionEjercicioParticipante.Add(reg);
                db.SaveChanges();

                IdParticipante = reg.IdParticipante;
            }

            return IdParticipante;
        }
        public static void UpdateParticipanteEjercicio(PruebaParticipanteEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioParticipante reg = db.tblPBEPruebaPlanificacionEjercicioParticipante
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == data.IdEjercicio && x.IdParticipante == data.IdParticipante).FirstOrDefault();

                reg.Responsable = data.Responsable;
                db.SaveChanges();
            }

        }
        public static void DeleteParticipanteEjercicio(long idEjercicio, long idParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioParticipante reg = db.tblPBEPruebaPlanificacionEjercicioParticipante
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == idEjercicio && x.IdParticipante == idParticipante).FirstOrDefault();

                db.tblPBEPruebaPlanificacionEjercicioParticipante.Remove(reg);
                db.SaveChanges();
            }

        }
        public static List<PruebaRecursoEjercicioModel> GetPruebaRecursosEjercicio(long idPrueba, long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaRecursoEjercicioModel> data = new List<PruebaRecursoEjercicioModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaPlanificacionEjercicioRecurso
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio
                        select new PruebaRecursoEjercicioModel
                        {
                            Cantidad = d.Cantidad,
                            Descripcion = d.Descripcion,
                            IdEjercicio = idEjercicio,
                            IdRecurso = d.IdRecurso,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable
                        }).ToList();
            }

            return data;
        }
        public static PruebaRecursoEjercicioModel GetPruebaRecursoEjercicio(long idPrueba, long idEjercicio, long idRecurso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaRecursoEjercicioModel data = new PruebaRecursoEjercicioModel();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaPlanificacionEjercicioRecurso
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio && d.IdRecurso == idRecurso
                        select new PruebaRecursoEjercicioModel
                        {
                            Cantidad = d.Cantidad,
                            Descripcion = d.Descripcion,
                            IdEjercicio = idEjercicio,
                            IdRecurso = d.IdRecurso,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable
                        }).FirstOrDefault();
            }

            return data;
        }
        public static long AddRecursoEjercicio(PruebaRecursoEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdRecurso = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioRecurso reg = new tblPBEPruebaPlanificacionEjercicioRecurso
                {
                    Cantidad = data.Cantidad,
                    Descripcion = data.Descripcion,
                    IdEjercicio = data.IdEjercicio,
                    IdEmpresa = IdEmpresa,
                    IdRecurso = data.IdRecurso,
                    IdPlanificacion = IdPrueba,
                    Nombre = data.Nombre,
                    Responsable = data.Responsable
                };

                db.tblPBEPruebaPlanificacionEjercicioRecurso.Add(reg);
                db.SaveChanges();

                IdRecurso = reg.IdRecurso;
            }

            return IdRecurso;
        }
        public static void UpdateRecursoEjercicio(PruebaRecursoEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioRecurso reg = db.tblPBEPruebaPlanificacionEjercicioRecurso
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == data.IdEjercicio && x.IdRecurso == data.IdRecurso).FirstOrDefault();

                reg.Cantidad = data.Cantidad;
                reg.Descripcion = data.Descripcion;
                reg.Nombre = data.Nombre;
                reg.Responsable = data.Responsable;
                db.SaveChanges();
            }

        }
        public static void DeleteRecursoEjercicio(long idEjercicio, long idRecurso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacionEjercicioRecurso reg = db.tblPBEPruebaPlanificacionEjercicioRecurso
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == idEjercicio && x.IdRecurso == idRecurso).FirstOrDefault();

                db.tblPBEPruebaPlanificacionEjercicioRecurso.Remove(reg);
                db.SaveChanges();
            }

        }
        public static void ActivarPrueba(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacion prueba = db.tblPBEPruebaPlanificacion
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == idPrueba);

                prueba.IdEstatus = 0;
                db.SaveChanges();
            }
        }
        public static void SuspenderPrueba(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacion prueba = db.tblPBEPruebaPlanificacion
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == idPrueba);

                prueba.IdEstatus = 3;
                db.SaveChanges();
            }
        }
        public static List<TablaModel_int> GetEstadoEjecucion()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel_int> data = new List<TablaModel_int>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_PBEPruebaEstatus
                         .Where(x => x.IdEstatus > 0 && (x.Cultura == Culture || x.Cultura == "es-VE"))
                         .Select(x => new TablaModel_int
                         {
                             Descripcion = x.Descripcion,
                             Id = x.IdEstatus
                         }).ToList();
            }

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static PruebaEjecucionModel GetEjecucion(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            PruebaEjecucionModel datos = new PruebaEjecucionModel();

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                if (db.tblPBEPruebaEjecucion.Count(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == IdPrueba) == 0)
                {
                    tblPBEPruebaPlanificacion data =
                        (from d in db.tblPBEPruebaPlanificacion
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select d).FirstOrDefault();

                    db.tblPBEPruebaEjecucion.Add(new tblPBEPruebaEjecucion
                    {
                        FechaInicio = data.FechaInicio,
                        IdPlanificacion = IdPrueba,
                        Lugar = data.Lugar,
                        IdEmpresa = data.IdEmpresa,
                    });

                    List<tblPBEPruebaEjecucionParticipante> dataP = new List<tblPBEPruebaEjecucionParticipante>();
                    List<tblPBEPruebaEjecucionEjercicio> dataE = new List<tblPBEPruebaEjecucionEjercicio>();

                    List<tblPBEPruebaPlanificacionParticipante> dataPP =
                        (from d in db.tblPBEPruebaPlanificacionParticipante
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select d).ToList();

                    List<tblPBEPruebaPlanificacionEjercicio> dataPE =
                        (from d in db.tblPBEPruebaPlanificacionEjercicio
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select d).ToList();

                    foreach (tblPBEPruebaPlanificacionParticipante d in dataPP)
                    {
                        dataP.Add(new tblPBEPruebaEjecucionParticipante
                        {
                            Correo = d.Correo,
                            Empresa = d.Empresa,
                            IdEmpresa = d.IdEmpresa,
                            IdPlanificacion = d.IdPlanificacion,
                            IdPlanificacionParticipante = d.IdParticipante,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable,
                            Telefono = d.Telefono,
                        });
                    }
                    db.tblPBEPruebaEjecucionParticipante.AddRange(dataP);

                    foreach (tblPBEPruebaPlanificacionEjercicio d in dataPE)
                    {
                        dataE.Add(new tblPBEPruebaEjecucionEjercicio
                        {
                            Descripcion = d.Descripcion,
                            Duracion = d.Duracion,
                            FechaInicio = d.FechaInicio,
                            IdEmpresa = d.IdEmpresa,
                            IdEstatus = 0,
                            IdEjercicioPlanificacion = d.IdEjercicio,
                            IdPlanificacion = d.IdPlanificacion,
                            Nombre = d.Nombre,
                        });
                    }
                    db.tblPBEPruebaEjecucionEjercicio.AddRange(dataE);
                    db.SaveChanges();

                    List<tblPBEPruebaEjecucionEjercicioParticipante> dataEP = new List<tblPBEPruebaEjecucionEjercicioParticipante>();
                    List<tblPBEPruebaEjecucionEjercicioRecurso> dataER = new List<tblPBEPruebaEjecucionEjercicioRecurso>();

                    List<tblPBEPruebaPlanificacionEjercicioParticipante> dataPEP =
                        (from d in db.tblPBEPruebaPlanificacionEjercicioParticipante
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select d).ToList();

                    List<tblPBEPruebaPlanificacionEjercicioRecurso> dataPER =
                        (from d in db.tblPBEPruebaPlanificacionEjercicioRecurso
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select d).ToList();

                    foreach (tblPBEPruebaPlanificacionEjercicioParticipante d in dataPEP)
                    {
                        dataEP.Add(new tblPBEPruebaEjecucionEjercicioParticipante
                        {
                            IdEmpresa = IdEmpresa,
                            IdEjercicio = db.tblPBEPruebaEjecucionEjercicio
                                            .FirstOrDefault(e => e.IdEjercicioPlanificacion == d.IdEjercicio).IdEjercicio,
                            IdPlanificacion = IdPrueba,
                            IdParticipante = db.tblPBEPruebaEjecucionParticipante
                                               .FirstOrDefault(e => e.IdEmpresa == IdEmpresa
                                                                   && e.IdPlanificacion == IdPrueba
                                                                   && e.IdPlanificacionParticipante == d.IdParticipante)
                                               .IdParticipante,
                            Responsable = d.Responsable,
                        });
                    }

                    foreach (tblPBEPruebaPlanificacionEjercicioRecurso d in dataPER)
                    {
                        dataER.Add(new tblPBEPruebaEjecucionEjercicioRecurso
                        {
                            Cantidad = d.Cantidad,
                            Descripcion = d.Descripcion,
                            IdEjercicio = db.tblPBEPruebaEjecucionEjercicio
                                             .FirstOrDefault(e => e.IdEjercicioPlanificacion == d.IdEjercicio).IdEjercicio,
                            IdEmpresa = IdEmpresa,
                            IdPlanificacion = IdPrueba,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable,
                        });
                    }

                    db.tblPBEPruebaEjecucionEjercicioParticipante.AddRange(dataEP);
                    db.tblPBEPruebaEjecucionEjercicioRecurso.AddRange(dataER);
                    db.SaveChanges();

                }
                datos = (from d in db.tblPBEPruebaEjecucion
                         let FechaI = SqlFunctions.DateAdd("mi", Minutos, d.FechaInicio)
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select new PruebaEjecucionModel
                         {
                             FechaInicio = (DateTime)FechaI,
                             IdClaseDocumento = IdTipoPruebas,
                             IdEmpresa = d.IdEmpresa,
                             IdPrueba = d.IdPlanificacion,
                             Nombre = d.tblPBEPruebaPlanificacion.Prueba,
                             Proposito = d.tblPBEPruebaPlanificacion.Propósito,
                             Ubicacion = d.Lugar,
                         }).FirstOrDefault();
            }

            return datos;
        }
        public static void UpdateEjecucion(PruebaEjecucionModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = data.IdPrueba;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas > 0) Minutos *= -1;

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucion reg = db.tblPBEPruebaEjecucion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba).FirstOrDefault();
                if (reg == null)
                {
                    reg = new tblPBEPruebaEjecucion
                    {
                        IdEmpresa = IdEmpresa,
                        IdPlanificacion = IdPrueba,
                        FechaInicio = data.FechaInicio.AddMinutes(Minutos),
                        Lugar = data.Ubicacion,
                    };

                    db.tblPBEPruebaEjecucion.Add(reg);
                }
                else
                {
                    reg.FechaInicio = data.FechaInicio.AddMinutes(Minutos);
                    reg.Lugar = data.Ubicacion;
                }

                db.SaveChanges();
            }

        }
        public static List<TablaModel_short> GetEmpresasPruebaEjecucionParticipantes(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            List<TablaModel_short> Empresas = new List<TablaModel_short>();

            using (Entities db = new Entities())
            {
                if (db.tblPBEPruebaEjecucionParticipante.Count(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == IdPrueba) == 0)
                {
                    List<tblPBEPruebaEjecucionParticipante> dataPrueba =
                        (from d in db.tblPBEPruebaPlanificacionParticipante
                         where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                         select new tblPBEPruebaEjecucionParticipante()
                         {
                             Correo = d.Correo,
                             Empresa = d.Empresa,
                             IdEmpresa = d.IdEmpresa,
                             IdPlanificacion = d.IdPlanificacion,
                             IdPlanificacionParticipante = d.IdParticipante,
                             Nombre = d.Nombre,
                             Responsable = d.Responsable,
                             Telefono = d.Telefono,
                         }).ToList();

                    db.tblPBEPruebaEjecucionParticipante.AddRange(dataPrueba);
                }
                Empresas = (from d in db.tblPBEPruebaEjecucionParticipante
                            where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba
                            orderby d.Empresa
                            select d.Empresa).Distinct().AsEnumerable()
                            .Select(x => new TablaModel_short
                            {
                                Descripcion = x
                            }).ToList();
            }

            return Empresas;
        }
        public static List<PruebaParticipanteModel> GetPruebaEjecucionParticipantes(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaParticipanteModel> Participantes = new List<PruebaParticipanteModel>();

            using (Entities db = new Entities())
            {
                Participantes = (from d in db.tblPBEPruebaEjecucionParticipante
                                 where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba
                                 select new PruebaParticipanteModel
                                 {
                                     CorreoElectronico = d.Correo,
                                     EmpresaParticipante = d.Empresa,
                                     IdClaseDocumento = IdTipoPruebas,
                                     IdEmpresa = IdEmpresa,
                                     IdParticipante = d.IdParticipante,
                                     IdPrueba = d.IdPlanificacion,
                                     NombreParticipante = d.Nombre,
                                     NroTelefono = d.Telefono,
                                     Planificado = true,
                                     Responsable = d.Responsable,
                                     Utilizado = db.tblPBEPruebaEjecucionEjercicioParticipante.Where(x => x.IdParticipante == d.IdParticipante).Count() > 0,
                                 }).ToList();
            }

            return Participantes;
        }
        public static PruebaParticipanteModel GetPruebaEjecucionParticipante(long IdPrueba, long IdParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaParticipanteModel Participante = new PruebaParticipanteModel();

            using (Entities db = new Entities())
            {
                Participante = (from d in db.tblPBEPruebaEjecucionParticipante
                                where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdParticipante == IdParticipante
                                select new PruebaParticipanteModel
                                {
                                    CorreoElectronico = d.Correo,
                                    EmpresaParticipante = d.Empresa,
                                    IdClaseDocumento = IdTipoPruebas,
                                    IdEmpresa = IdEmpresa,
                                    IdParticipante = d.IdParticipante,
                                    IdPrueba = d.IdPlanificacion,
                                    NombreParticipante = d.Nombre,
                                    NroTelefono = d.Telefono,
                                    Planificado = true,
                                    Responsable = d.Responsable,
                                    Utilizado = db.tblPBEPruebaEjecucionEjercicioParticipante.Where(x => x.IdParticipante == d.IdParticipante).Count() > 0,
                                }).FirstOrDefault();
            }

            return Participante;
        }
        public static long AddParticipanteEjecucion(PruebaParticipanteModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdParticipante = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionParticipante reg = new tblPBEPruebaEjecucionParticipante
                {
                    Correo = data.CorreoElectronico,
                    Empresa = data.EmpresaParticipante,
                    IdEmpresa = IdEmpresa,
                    IdPlanificacion = IdPrueba,
                    Nombre = data.NombreParticipante,
                    Responsable = data.Responsable,
                    Telefono = data.NroTelefono,
                };

                db.tblPBEPruebaEjecucionParticipante.Add(reg);
                db.SaveChanges();

                IdParticipante = reg.IdParticipante;
            }

            return IdParticipante;
        }
        public static void UpdateParticipanteEjecucion(PruebaParticipanteModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionParticipante reg = db.tblPBEPruebaEjecucionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdParticipante == data.IdParticipante).FirstOrDefault();
                reg.Correo = data.CorreoElectronico;
                reg.Empresa = data.EmpresaParticipante;
                reg.Nombre = data.NombreParticipante;
                reg.Responsable = data.Responsable;
                reg.Telefono = data.NroTelefono;

                db.SaveChanges();
            }

        }
        public static void DeleteParticipanteEjecucion(long IdParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionParticipante reg = db.tblPBEPruebaEjecucionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdParticipante == IdParticipante).FirstOrDefault();

                db.tblPBEPruebaEjecucionEjercicioParticipante.RemoveRange(reg.tblPBEPruebaEjecucionEjercicioParticipante);
                db.tblPBEPruebaEjecucionParticipante.Remove(reg);

                db.SaveChanges();
            }

        }
        public static List<PruebaEjercicioEjecucionModel> GetPruebaEjerciciosEjecucion(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaEjercicioEjecucionModel> Ejercicios = new List<PruebaEjercicioEjecucionModel>();

            using (Entities db = new Entities())
            {
                Ejercicios = (from d in db.tblPBEPruebaEjecucionEjercicio
                              where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba
                              select new PruebaEjercicioEjecucionModel
                              {
                                  Descripcion = d.Descripcion,
                                  DuracionHoras = d.Duracion,
                                  Ejecutado = d.IdEstatus > 0 && d.IdEstatus < 3,
                                  Inicio = d.FechaInicio,
                                  IdClaseDocumento = IdTipoPruebas,
                                  IdEmpresa = IdEmpresa,
                                  IdEjercicio = d.IdEjercicio,
                                  IdPrueba = d.IdPlanificacion,
                                  IdStatus = d.IdEstatus ?? 0,
                                  Nombre = d.Nombre,
                              }).ToList();
            }

            return Ejercicios;
        }
        public static PruebaEjercicioEjecucionModel GetPruebaEjercicioEjecucion(long IdPrueba, long IdEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaEjercicioEjecucionModel Ejercicio = new PruebaEjercicioEjecucionModel();

            using (Entities db = new Entities())
            {
                Ejercicio = (from d in db.tblPBEPruebaEjecucionEjercicio
                             where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdEjercicio == IdEjercicio
                             select new PruebaEjercicioEjecucionModel
                             {
                                 Descripcion = d.Descripcion,
                                 DuracionHoras = d.Duracion,
                                 Ejecutado = db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == d.IdEjercicio).Count() > 0,
                                 Inicio = d.FechaInicio,
                                 IdClaseDocumento = IdTipoPruebas,
                                 IdEmpresa = IdEmpresa,
                                 IdEjercicio = d.IdEjercicio,
                                 IdPrueba = d.IdPlanificacion,
                                 IdStatus = d.IdEstatus ?? 0,
                                 Nombre = d.Nombre,
                             }).FirstOrDefault();
            }

            return Ejercicio;
        }
        public static long AddEjercicioEjecucion(PruebaEjercicioEjecucionModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdEjercicio = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicio reg = new tblPBEPruebaEjecucionEjercicio
                {
                    Descripcion = data.Descripcion,
                    Duracion = data.DuracionHoras,
                    FechaInicio = data.Inicio,
                    IdEmpresa = IdEmpresa,
                    IdPlanificacion = IdPrueba,
                    IdEjercicio = IdEjercicio,
                    IdEstatus = data.IdStatus,
                    Nombre = data.Nombre,
                };

                db.tblPBEPruebaEjecucionEjercicio.Add(reg);
                db.SaveChanges();

                IdEjercicio = reg.IdEjercicio;
            }

            return IdEjercicio;
        }
        public static void UpdateEjercicioEjecucion(PruebaEjercicioEjecucionModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicio reg = db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == data.IdEjercicio).FirstOrDefault();
                reg.Descripcion = data.Descripcion;
                reg.Duracion = data.DuracionHoras;
                reg.FechaInicio = data.Inicio;
                reg.IdEstatus = data.IdStatus;
                reg.Nombre = data.Nombre;

                db.SaveChanges();
            }

        }
        public static void DeleteEjercicioEjecucion(long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicio reg = db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba && x.IdEjercicio == idEjercicio).FirstOrDefault();

                if (reg.tblPBEPruebaEjecucionEjercicioParticipante != null && reg.tblPBEPruebaEjecucionEjercicioParticipante.Count() > 0)
                    db.tblPBEPruebaEjecucionEjercicioParticipante.RemoveRange(reg.tblPBEPruebaEjecucionEjercicioParticipante);
                if (reg.tblPBEPruebaEjecucionEjercicioRecurso != null && reg.tblPBEPruebaEjecucionEjercicioRecurso.Count() > 0)
                    db.tblPBEPruebaEjecucionEjercicioRecurso.RemoveRange(reg.tblPBEPruebaEjecucionEjercicioRecurso);

                db.tblPBEPruebaEjecucionEjercicio.Remove(reg);
                db.SaveChanges();
            }

        }
        public static List<PruebaParticipanteEjercicioModel> GetPruebaParticipantesEjercicioEjecucion(long idPrueba, long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaParticipanteEjercicioModel> data = new List<PruebaParticipanteEjercicioModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaEjecucionEjercicioParticipante
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio
                        select new PruebaParticipanteEjercicioModel
                        {
                            IdEjercicio = idEjercicio,
                            IdParticipante = d.IdParticipante,
                            Responsable = d.Responsable
                        }).ToList();
            }

            return data;
        }
        public static PruebaParticipanteEjercicioModel GetPruebaParticipanteEjercicioEjecucion(long IdPrueba, long IdEjercicio, long idParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaParticipanteEjercicioModel data = new PruebaParticipanteEjercicioModel();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaEjecucionEjercicioParticipante
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == IdPrueba && d.IdEjercicio == IdEjercicio && d.IdParticipante == idParticipante
                        select new PruebaParticipanteEjercicioModel
                        {
                            IdEjercicio = d.IdEjercicio,
                            IdParticipante = d.IdParticipante,
                            Responsable = d.Responsable
                        }).FirstOrDefault();
            }

            return data;
        }
        public static long AddParticipanteEjercicioEjecucion(PruebaParticipanteEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdParticipante = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioParticipante reg = new tblPBEPruebaEjecucionEjercicioParticipante
                {
                    IdEjercicio = data.IdEjercicio,
                    IdEmpresa = IdEmpresa,
                    IdParticipante = data.IdParticipante,
                    IdPlanificacion = IdPrueba,
                    Responsable = data.Responsable
                };

                db.tblPBEPruebaEjecucionEjercicioParticipante.Add(reg);
                db.SaveChanges();

                IdParticipante = reg.IdParticipante;
            }

            return IdParticipante;
        }
        public static void UpdateParticipanteEjercicioEjecucion(PruebaParticipanteEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioParticipante reg = db.tblPBEPruebaEjecucionEjercicioParticipante
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == data.IdEjercicio && x.IdParticipante == data.IdParticipante).FirstOrDefault();

                reg.Responsable = data.Responsable;
                db.SaveChanges();
            }

        }
        public static void DeleteParticipanteEjercicioEjecucion(long idEjercicio, long idParticipante)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioParticipante reg = db.tblPBEPruebaEjecucionEjercicioParticipante
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == idEjercicio && x.IdParticipante == idParticipante).FirstOrDefault();

                db.tblPBEPruebaEjecucionEjercicioParticipante.Remove(reg);
                db.SaveChanges();
            }

        }
        public static List<PruebaRecursoEjercicioModel> GetPruebaRecursosEjecucionEjercicio(long idPrueba, long idEjercicio)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            List<PruebaRecursoEjercicioModel> data = new List<PruebaRecursoEjercicioModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaEjecucionEjercicioRecurso
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio
                        select new PruebaRecursoEjercicioModel
                        {
                            Cantidad = d.Cantidad,
                            Descripcion = d.Descripcion,
                            IdEjercicio = idEjercicio,
                            IdRecurso = d.IdRecurso,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable
                        }).ToList();
            }

            return data;
        }
        public static PruebaRecursoEjercicioModel GetPruebaRecursoEjecucionEjercicio(long idPrueba, long idEjercicio, long idRecurso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());

            PruebaRecursoEjercicioModel data = new PruebaRecursoEjercicioModel();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblPBEPruebaPlanificacionEjercicioRecurso
                        where d.IdEmpresa == IdEmpresa && d.IdPlanificacion == idPrueba && d.IdEjercicio == idEjercicio && d.IdRecurso == idRecurso
                        select new PruebaRecursoEjercicioModel
                        {
                            Cantidad = d.Cantidad,
                            Descripcion = d.Descripcion,
                            IdEjercicio = idEjercicio,
                            IdRecurso = d.IdRecurso,
                            Nombre = d.Nombre,
                            Responsable = d.Responsable
                        }).FirstOrDefault();
            }

            return data;
        }
        public static long AddRecursoEjecucionEjercicio(PruebaRecursoEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());
            long IdRecurso = 0;

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioRecurso reg = new tblPBEPruebaEjecucionEjercicioRecurso
                {
                    Cantidad = data.Cantidad,
                    Descripcion = data.Descripcion,
                    IdEjercicio = data.IdEjercicio,
                    IdEmpresa = IdEmpresa,
                    IdRecurso = data.IdRecurso,
                    IdPlanificacion = IdPrueba,
                    Nombre = data.Nombre,
                    Responsable = data.Responsable
                };

                db.tblPBEPruebaEjecucionEjercicioRecurso.Add(reg);
                db.SaveChanges();

                IdRecurso = reg.IdRecurso;
            }

            return IdRecurso;
        }
        public static void UpdateRecursoEjecucionEjercicio(PruebaRecursoEjercicioModel data)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioRecurso reg = db.tblPBEPruebaEjecucionEjercicioRecurso
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == data.IdEjercicio && x.IdRecurso == data.IdRecurso).FirstOrDefault();

                reg.Cantidad = data.Cantidad;
                reg.Descripcion = data.Descripcion;
                reg.Nombre = data.Nombre;
                reg.Responsable = data.Responsable;
                db.SaveChanges();
            }

        }
        public static void DeleteRecursoEjecucionEjercicio(long idEjercicio, long idRecurso)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            long IdPrueba = long.Parse(Session["IdPrueba"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionEjercicioRecurso reg = db.tblPBEPruebaEjecucionEjercicioRecurso
                    .Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == IdPrueba
                        && x.IdEjercicio == idEjercicio && x.IdRecurso == idRecurso).FirstOrDefault();

                db.tblPBEPruebaEjecucionEjercicioRecurso.Remove(reg);
                db.SaveChanges();
            }

        }
        public static byte[] GetHallazgoEjecucion(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            byte[] data = new byte[] { };

            using (Entities db = new Entities())
            {

                tblPBEPruebaEjecucionResultado datos = db.tblPBEPruebaEjecucionResultado
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == IdPrueba && e.IdContenido == 1);

                data = (datos != null ? datos.Contenido ?? null : null);

            }

            return data;
        }
        public static byte[] GetAccionesEjecucion(long IdPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoPruebas = int.Parse(Session["IdClaseDocumento"].ToString());
            byte[] data = new byte[] { };

            using (Entities db = new Entities())
            {

                tblPBEPruebaEjecucionResultado datos = db.tblPBEPruebaEjecucionResultado
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == IdPrueba && e.IdContenido == 2);

                data = (datos != null ? datos.Contenido ?? null : null);

            }

            return data;
        }
        public static bool UpdateResultadoEjecucion(long IdPrueba, long IdContenido, byte[] Contenido)
        {
            bool Updated = false;
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                tblPBEPruebaEjecucionResultado resultado = db.tblPBEPruebaEjecucionResultado
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == IdPrueba && e.IdContenido == IdContenido);

                if (resultado == null)
                {
                    resultado = new tblPBEPruebaEjecucionResultado
                    {
                        Contenido = Contenido,
                        IdContenido = IdContenido,
                        IdEmpresa = IdEmpresa,
                        IdPlanificacion = IdPrueba,
                    };

                    db.tblPBEPruebaEjecucionResultado.Add(resultado);
                }
                else
                {
                    resultado.Contenido = Contenido;
                }

                db.SaveChanges();
                Updated = true;
            }

            return Updated;
        }
        public static void FinalizarPrueba(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            using (Entities db = new Entities())
            {
                tblPBEPruebaPlanificacion prueba = db.tblPBEPruebaPlanificacion
                    .FirstOrDefault(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == idPrueba);

                int Satisfactorios = db.tblPBEPruebaEjecucionEjercicio
                    .Count(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == idPrueba && e.IdEstatus == 1);
                int Fallidos = db.tblPBEPruebaEjecucionEjercicio
                    .Count(e => e.IdEmpresa == IdEmpresa && e.IdPlanificacion == idPrueba && e.IdEstatus > 1);

                if (Fallidos > Satisfactorios)
                    prueba.IdEstatus = 2;
                else
                    prueba.IdEstatus = 1;

                db.SaveChanges();
            }
        }
        public static void EliminarPrueba(long idPrueba)
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            using (Entities db = new Entities())
            {
                db.tblPBEPruebaEjecucionResultado.RemoveRange(db.tblPBEPruebaEjecucionResultado.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaEjecucionEjercicioRecurso.RemoveRange(db.tblPBEPruebaEjecucionEjercicioRecurso.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaEjecucionEjercicioParticipante.RemoveRange(db.tblPBEPruebaEjecucionEjercicioParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaEjecucionEjercicio.RemoveRange(db.tblPBEPruebaEjecucionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaEjecucionParticipante.RemoveRange(db.tblPBEPruebaEjecucionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaEjecucion.RemoveRange(db.tblPBEPruebaEjecucion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaPlanificacionEjercicioRecurso.RemoveRange(db.tblPBEPruebaPlanificacionEjercicioRecurso.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaPlanificacionEjercicioParticipante.RemoveRange(db.tblPBEPruebaPlanificacionEjercicioParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaPlanificacionEjercicio.RemoveRange(db.tblPBEPruebaPlanificacionEjercicio.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaPlanificacionParticipante.RemoveRange(db.tblPBEPruebaPlanificacionParticipante.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.tblPBEPruebaPlanificacion.RemoveRange(db.tblPBEPruebaPlanificacion.Where(x => x.IdEmpresa == IdEmpresa && x.IdPlanificacion == idPrueba).ToList());
                db.SaveChanges();
            }
        }
        public static bool EditDocumentoActivo(int IdTipoDocumento, bool Negocios)
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long minModulo = IdTipoDocumento * 1000000;
            long maxModulo = IdTipoDocumento * 1999999;
            bool CanEdit = false;

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

                List<long> Unidades = db.tblUsuarioUnidadOrganizativa
                                        .Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUser)
                                        .Select(x => x.IdUnidadOrganizativa)
                                        .ToList();

                bool ProgramacionExist = db.tblPMTProgramacion
                    .Where(x => x.IdEmpresa == IdEmpresa && x.Negocios == Negocios
                             && x.IdModulo == (IdTipoDocumento * 1000000)).Count() > 0;

                tblPMTProgramacion Programacion = db.tblPMTProgramacion
                    .FirstOrDefault(x => x.IdEmpresa == IdEmpresa
                                      && x.IdModulo == (IdTipoDocumento * 1000000)
                                      && x.Negocios == Negocios
                                      && x.FechaInicio <= DateTime.UtcNow
                                      && x.FechaFinal >= DateTime.UtcNow);

                CanEdit = ((!ProgramacionExist && Programacion == null) || (ProgramacionExist && Programacion != null))
                               && (EmpresaUsuario.IdNivelUsuario == 6 || EmpresaUsuario.IdNivelUsuario == 4 || ModulosEditables > 0);
            }

            return CanEdit;
        }
        public static List<TablaModel> GetEstatusDocumento()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = db.tblCultura_EstadoDocumento.Where(x => x.Culture == Culture || x.Culture == "es-VE").Select(x => new TablaModel
                {
                    Descripcion = x.Descripcion,
                    Id = x.IdEstadoDocumento
                }).ToList();
            }

            data.Add(new TablaModel
            {
                Id = 0,
                Descripcion = Resources.DocumentoResource.AllDataMale,
            });

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static IList<UnidadOrganizativaModel> GetUnidadesOrganizativasByName()
        {
            List<UnidadOrganizativaModel> Data = new List<UnidadOrganizativaModel>();

            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());

            using (Entities db = new Entities())
            {
                Data = db.tblUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa)
                    .Select(x => new UnidadOrganizativaModel()
                    {
                        IdUnidad = x.IdUnidadOrganizativa,
                        IdUnidadPadre = x.IdUnidadPadre,
                        NombreUnidadOrganizativa = x.Nombre
                    }).ToList();
            }

            return Data.OrderBy(x => x.NombreUnidadOrganizativa).ToList();
        }
        public static List<TablaModel> GetResponsablesTipoDocumento()
        {
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            int IdTipoDocumento = int.Parse(Session["IdTipoDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            List<TablaModel> data = new List<TablaModel>();

            using (Entities db = new Entities())
            {
                data = (from d in db.tblDocumento
                        join p in db.tblPersona on d.IdPersonaResponsable equals p.IdPersona
                        where d.IdEmpresa == IdEmpresa && d.IdTipoDocumento == IdTipoDocumento && d.Negocios == (IdClaseDocumento == 1)
                        select p)
                        .AsEnumerable()
                        .Select(to => new TablaModel
                        {
                            Descripcion = string.IsNullOrEmpty(to.Nombre) ? string.Format("Responsable Id {0}", to.IdPersona.ToString()) : to.Nombre,
                            Id = to.IdPersona
                        }).Distinct().ToList();
            }

            data.Add(new TablaModel
            {
                Id = 0,
                Descripcion = Resources.DocumentoResource.AllDataMale,
            });

            return data.OrderBy(x => x.Descripcion).ToList();

        }
        public static IList<UnidadOrganizativaModel> GetUnidadesOrganizativasByUser(bool forFilter)
        {
            List<UnidadOrganizativaModel> Data = new List<UnidadOrganizativaModel>();

            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdUser = long.Parse(Session["UserId"].ToString());

            using (Entities db = new Entities())
            {
                Data = db.tblUsuarioUnidadOrganizativa.Where(x => x.IdEmpresa == IdEmpresa && x.IdUsuario == IdUser)
                    .Select(x => new UnidadOrganizativaModel()
                    {
                        IdUnidad = x.tblUnidadOrganizativa.IdUnidadOrganizativa,
                        IdUnidadPadre = x.tblUnidadOrganizativa.IdUnidadPadre,
                        NombreUnidadOrganizativa = x.tblUnidadOrganizativa.Nombre
                    }).ToList();
            }

            if (forFilter)
            {
                Data.Add(new UnidadOrganizativaModel
                {
                    IdUnidad = 0,
                    IdUnidadPadre = 0,
                    NombreUnidadOrganizativa = Resources.DocumentoResource.AllDataFemale,
                });
            }

            return Data.OrderBy(x => x.NombreUnidadOrganizativa).ToList();
        }


    }
}