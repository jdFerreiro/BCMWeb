using BCMWeb.Data.EF;
using BCMWeb.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {
        public ActionResult Ficha(long modId)
        {

            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;
            Session["IdTipoDocumento"] = IdTipoDocumento;
            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            DocumentoModel Documento = new DocumentoModel();
            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            if (IdDocumento == -1)
            {
                Documento = new DocumentoModel
                {
                    Anexos = new List<DocumentoAnexoModel>(),
                    Aprobaciones = new List<DocumentoAprobacionModel>(),
                    Auditoria = new List<DocumentoAuditoriaModel>(),
                    Certificaciones = new List<DocumentoCertificacionModel>(),
                    Contenido = new List<DocumentoContenidoModel>(),
                    Entrevistas = new List<DocumentoEntrevistaModel>(),
                    Estatus = "Cargando",
                    FechaCreacion = DateTime.UtcNow,
                    FechaEstadoDocumento = DateTime.UtcNow,
                    FechaUltimaModificacion = DateTime.UtcNow,
                    IdDocumento = IdDocumento,
                    IdEstatus = 1,
                    IdPersonaResponsable = 0,
                    NroDocumento = Metodos.GetNextNroDocumento(IdClaseDocumento, IdTipoDocumento),
                    NroVersion = Metodos.GetNextVersion(IdClaseDocumento, IdTipoDocumento, IdVersion),
                    PersonasClave = new List<DocumentoPersonaClaveModel>(),
                    RequiereCertificacion = true,
                    VersionOriginal = IdVersion
                };

            }
            else
            {
                Documento = Metodos.GetDocumento(IdDocumento, IdTipoDocumento);
            }

            Documento.Negocios = (IdClaseDocumento == 1);
            Documento.IdTipoDocumento = IdTipoDocumento;

            DocumentosModel model = new DocumentosModel
            {
                Documentos = (new List<DocumentoModel>()),
                EditDocumento = true,
                IdClaseDocumento = IdClaseDocumento,
                IdDocumentoSelected = 0,
                IdEmpresa = long.Parse(Session["IdEmpresa"].ToString()),
                IdModulo = IdModulo,
            };

            model.Documentos.Add(Documento);
            return View(model);
        }
        public ActionResult PersonaPartialView()
        {
            PersonaModel model = new PersonaModel();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult PersonaPartialView(PersonaModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return PartialView(model);
        }

        public ActionResult DireccionesPartialView()
        {
            return PartialView();
        }
        public ActionResult CorreosPartialView()
        {
            return PartialView();
        }
        public ActionResult TelefonosPartialView()
        {
            return PartialView();
        }
        public ActionResult NuevoCargoPartialView()
        {
            return PartialView();
        }
        public ActionResult NuevaUnidadOrganizativaPartialView()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult NuevoCargo(string Texto)
        {
            string IdCargo = string.Empty;

            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            bool success = false;

            if (Texto != null)
            {
                using (Entities db = new Entities())
                {
                    tblCargo newCargo = new tblCargo
                    {
                        IdEmpresa = _IdEmpresa,
                        Descripcion = Texto
                    };

                    db.tblCargo.Add(newCargo);
                    db.SaveChanges();
                    IdCargo = newCargo.IdCargo.ToString();
                    success = true;
                }
            }
            return Json(new { success, IdCargo });
        }
        [HttpPost]
        public JsonResult NuevaUnidad(string Texto, long idUnidadPadre)
        {
            string IdUnidad = string.Empty;

            long _IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            bool success = false;

            if (Texto != null)
            {
                using (Entities db = new Entities())
                {
                    tblUnidadOrganizativa newUnidad = new tblUnidadOrganizativa
                    {
                        IdEmpresa = _IdEmpresa,
                        IdUnidadPadre = idUnidadPadre,
                        Nombre = Texto
                    };

                    db.tblUnidadOrganizativa.Add(newUnidad);
                    db.SaveChanges();
                    IdUnidad = newUnidad.IdUnidadOrganizativa.ToString();
                    success = true;
                }
            }
            return Json(new { success, IdUnidad });
        }
        //[ValidateInput(false)]
        //public ActionResult BatchEditingUpdateCorreo(MVCxGridViewBatchUpdateValues<PersonaEmail, long> updateValues)
        //{
        //foreach (var product in updateValues.Insert)
        //{
        //    if (updateValues.IsValid(product))
        //        InsertProduct(product, updateValues);
        //}
        //foreach (var product in updateValues.Update)
        //{
        //    if (updateValues.IsValid(product))
        //        UpdateProduct(product, updateValues);
        //}
        //foreach (var productID in updateValues.DeleteKeys)
        //{
        //    DeleteProduct(productID, updateValues);
        //}
        //return PartialView("CorreosPartialView", NorthwindDataProvider.GetEditableProducts());
        //}
        //[ValidateInput(false)]
        //public ActionResult BatchEditingUpdateTelefono(MVCxGridViewBatchUpdateValues<PersonaTelefono, long> updateValues)
        //{
        //return PartialView("BatchEditingPartial", NorthwindDataProvider.GetEditableProducts());
        //}
    }
}