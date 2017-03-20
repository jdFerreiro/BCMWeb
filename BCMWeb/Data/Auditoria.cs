using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BCMWeb
{
    public static class Auditoria
    {
        internal static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        internal static HttpRequest Request { get { return HttpContext.Current.Request;  } }

        public static void RegistrarLogin()
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();

                tblAuditoria regAuditoria = new tblAuditoria
                {
                    Accion = string.Format(Resources.AuditoriaResource.LoginAccionMessage, usuario.Nombre),
                    DireccionIP = Request.UserHostAddress,
                    FechaRegistro = DateTime.UtcNow,
                    IdUsuario = IdUser,
                    Mensaje = string.Empty,
                };

                db.tblAuditoria.Add(regAuditoria);
                usuario.FechaUltimaConexion = DateTime.UtcNow;
                db.SaveChanges();
            }
        }
        public static void RegistrarLogout()
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();

                tblAuditoria regAuditoria = new tblAuditoria
                {
                    Accion = string.Format(Resources.AuditoriaResource.LogOutAccionMessage, usuario.Nombre),
                    DireccionIP = Request.UserHostAddress,
                    FechaRegistro = DateTime.UtcNow,
                    IdUsuario = IdUser,
                    Mensaje = string.Empty,
                };

                db.tblAuditoria.Add(regAuditoria);
                usuario.FechaUltimaConexion = DateTime.UtcNow;
                db.SaveChanges();
            }
        }
        public static void RegistarAccion(eTipoAccion Accion)
        {
            long IdUser = long.Parse(Session["UserId"].ToString());
            long IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            long IdDocumento = (Session["IdDocumento"] != null ? long.Parse(Session["IdDocumento"].ToString()) : 0);
            long IdTipoDocumento = (Session["IdTipoDocumento"] != null ? long.Parse(Session["IdTipoDocumento"].ToString()) : 0);
            long IdModuloActivo = (Session["modId"] != null ? long.Parse(Session["modId"].ToString()) : 0);
            long IdModuloPrincipal = IdTipoDocumento * 1000000;
            int IdClaseDocumento = (Session["IdClaseDocumento"] != null ? int.Parse(Session["IdClaseDocumento"].ToString()) : 0);
            string AccionMessage = string.Empty;

            using (Entities db = new Entities())
            {
                tblUsuario usuario = db.tblUsuario.Where(x => x.IdUsuario == IdUser).FirstOrDefault();
                tblModulo moduloActivo = db.tblModulo.Where(x => x.IdEmpresa == IdEmpresa && x.IdModulo == IdModuloActivo).FirstOrDefault();
                tblModulo moduloPrincipal = db.tblModulo.Where(x => x.IdEmpresa == IdEmpresa && x.IdModulo == IdModuloPrincipal).FirstOrDefault();
                tblDocumento Documento = db.tblDocumento.Where(x => x.IdEmpresa == IdEmpresa && x.IdDocumento == IdDocumento && x.IdTipoDocumento == IdTipoDocumento).FirstOrDefault();

                string docVersion = string.Empty;
                string NroDocumento = string.Empty;
                string NombreModulo = string.Empty;

                switch (Accion)
                {
                    case eTipoAccion.AbrirPDFMovil:
                        AccionMessage = Resources.AuditoriaResource.AbrirPDFMovilMessage;
                        NombreModulo = moduloPrincipal.Nombre;
                        break;
                    case eTipoAccion.AbrirPDFWeb:
                        AccionMessage = Resources.AuditoriaResource.AbrirPDFWebMessage;
                        NombreModulo = moduloPrincipal.Nombre;
                        break;
                    case eTipoAccion.AccesoModuloMovil:
                        AccionMessage = Resources.AuditoriaResource.AccesoModuloMovilMessage;
                        NombreModulo = moduloActivo.Nombre;
                        break;
                    case eTipoAccion.AccesoModuloWeb:
                        AccionMessage = Resources.AuditoriaResource.AccesoModuloWebMessage;
                        NombreModulo = moduloActivo.Nombre;
                        break;
                    case eTipoAccion.Actualizar:
                        AccionMessage = Resources.AuditoriaResource.ActualizarAccionMessage;
                        NombreModulo = moduloActivo.Nombre;
                        break;
                    case eTipoAccion.Eliminar:
                        AccionMessage = Resources.AuditoriaResource.EliminarAccionMessage;
                        NombreModulo = moduloPrincipal.Nombre;
                        break;
                    case eTipoAccion.GenerarPDF:
                        AccionMessage = Resources.AuditoriaResource.GenerarPDFMessage;
                        NombreModulo = moduloPrincipal.Nombre;
                        break;
                    case eTipoAccion.GenerarVersion:
                        AccionMessage = Resources.AuditoriaResource.GenerarVersionMessage;
                        NombreModulo = moduloPrincipal.Nombre;
                        break;
                    case eTipoAccion.Mostrar:
                        AccionMessage = Resources.AuditoriaResource.MostrarAccionMessage;
                        NombreModulo = moduloActivo.Nombre;
                        break;
                }

                if (Documento != null)
                {
                    docVersion = (Documento.VersionOriginal > 0 ? string.Format("{0}.{1}", Documento.VersionOriginal, Documento.NroVersion) : Documento.NroVersion.ToString());
                    NroDocumento = Documento.NroDocumento.ToString();
                }

                string _Accion = string.Format(AccionMessage, NombreModulo, NroDocumento, docVersion);

                tblAuditoria regAuditoria = new tblAuditoria
                {
                    Accion = _Accion,
                    DireccionIP = Request.UserHostAddress,
                    FechaRegistro = DateTime.UtcNow,
                    IdDocumento = IdDocumento,
                    IdEmpresa = IdEmpresa,
                    IdTipoDocumento = IdTipoDocumento,
                    IdUsuario = IdUser,
                    Mensaje = string.Empty,
                    Negocios = (IdClaseDocumento == 1),
                };

                db.tblAuditoria.Add(regAuditoria);
                usuario.FechaUltimaConexion = DateTime.UtcNow;
                db.SaveChanges();
            }
        }


    }
}