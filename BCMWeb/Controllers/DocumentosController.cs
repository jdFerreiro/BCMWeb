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

            DocumentoModel model = new DocumentoModel();
            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            long IdResponsable = (Session["IdResponsable"] != null ? long.Parse(Session["IdResponsable"].ToString()) : 0);

            if (IdDocumento == -1)
            {
                model = new DocumentoModel
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
                    IdClaseDocumento = IdClaseDocumento,
                    IdDocumento = IdDocumento,
                    IdEmpresa = long.Parse(Session["IdEmpresa"].ToString()),
                    IdEstatus = 1,
                    IdModulo = IdModulo,
                    IdPersonaResponsable = IdResponsable,
                    NroDocumento = Metodos.GetNextNroDocumento(IdClaseDocumento, IdTipoDocumento),
                    NroVersion = Metodos.GetNextVersion(IdClaseDocumento, IdTipoDocumento, IdVersion),
                    PersonasClave = new List<DocumentoPersonaClaveModel>(),
                    RequiereCertificacion = true,
                    returnPage = Url.Action("Index", "Documento", new { IdModulo }),
                    
                    VersionOriginal = IdVersion
                };

            }
            else
            {
                model = Metodos.GetDocumento(IdDocumento, IdTipoDocumento);
            }

            model.Negocios = (IdClaseDocumento == 1);
            model.IdTipoDocumento = IdTipoDocumento;

            return View(model);
        }
        [HttpPost]
        public ActionResult Ficha(DocumentoModel model)
        {
            if (model.IdPersonaResponsable > 0)
            {
                PersonaModel persona = Metodos.GetPersonaById(model.IdPersonaResponsable);

                List<DocumentoAprobacionModel> Aprobaciones = model.Aprobaciones.ToList();
                List<DocumentoCertificacionModel> Certificaciones = model.Certificaciones.ToList();
                List<DocumentoPersonaClaveModel> PersonasClave = model.PersonasClave.ToList();

                Aprobaciones.Add(new DocumentoAprobacionModel()
                {
                    Aprobado = false,
                    IdEmpresa = model.IdEmpresa,
                    IdPersona = model.IdPersonaResponsable,
                    Persona = persona,
                    Procesado = false,
                    Responsable = true
                });

                Certificaciones.Add(new DocumentoCertificacionModel()
                {
                    Certificado = false,
                    IdEmpresa = model.IdEmpresa,
                    IdPersona = model.IdPersonaResponsable,
                    Persona = persona,
                    Procesado = false,
                    Responsable = true
                });

                string Email = (persona.CorreosElectronicos.Where(x => x.IdTipoEmail == 1).FirstOrDefault() != null ? persona.CorreosElectronicos.Where(x => x.IdTipoEmail == 1).FirstOrDefault().Email : string.Empty);
                string Direccion = (persona.Direcciones.Where(x => x.IdTipoDireccion == 1).FirstOrDefault() != null ? persona.Direcciones.Where(x => x.IdTipoDireccion == 1).FirstOrDefault().Urbanizacion : string.Empty);
                string Oficina = (persona.Telefonos.Where(x => x.IdTipoTelefono == 1).FirstOrDefault() != null ? persona.Telefonos.Where(x => x.IdTipoTelefono == 1).FirstOrDefault().TelefonoCompleto : string.Empty);
                string Habitacion = (persona.Telefonos.Where(x => x.IdTipoTelefono == 2).FirstOrDefault() != null ? persona.Telefonos.Where(x => x.IdTipoTelefono == 2).FirstOrDefault().TelefonoCompleto : string.Empty);
                string Movil = (persona.Telefonos.Where(x => x.IdTipoTelefono == 3).FirstOrDefault() != null ? persona.Telefonos.Where(x => x.IdTipoTelefono == 3).FirstOrDefault().TelefonoCompleto : string.Empty);

                PersonasClave.Add(new DocumentoPersonaClaveModel()
                {
                    Cédula = persona.Identificacion,
                    DireccionHabitacion = Direccion,
                    Email = Email,
                    IdEmpresa= model.IdEmpresa,
                    IdPersona = persona.IdPersona,
                    IdTipoDocumento = model.IdTipoDocumento,
                    Nombre = persona.Nombre,
                    Principal = false,
                    Responsable = true,
                    TelefonoCelular = Movil,
                    TelefonoHabitacion = Habitacion,
                    TelefonoOficina = Oficina 
                });

                model.PersonasClave = PersonasClave;
                model.Certificaciones = Certificaciones;
                model.Aprobaciones = Aprobaciones;
            }

            if (model.Aprobaciones == null || model.Aprobaciones.Count() == 0)
                ModelState.AddModelError("Aprobaciones", Resources.ErrorResource.RequiredErrorFemale);
            if (model.RequiereCertificacion && (model.Certificaciones == null || model.Certificaciones.Count() == 0))
                ModelState.AddModelError("Certificaciones", Resources.ErrorResource.RequiredErrorFemale);
            if (model.IdClaseDocumento == 4 && (model.Procesos == null || model.Procesos.Count() == 0))
                ModelState.AddModelError("Procesos", Resources.ErrorResource.RequiredErrorMale);
            if (model.Entrevistas == null || model.Entrevistas.Count() == 0)
                ModelState.AddModelError("Entrevistas", Resources.ErrorResource.RequiredErrorFemale);
            if (model.PersonasClave == null || model.PersonasClave.Count() == 0)
                ModelState.AddModelError("PersonasClave", Resources.ErrorResource.RequiredErrorFemale);

            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
        public ActionResult PersonaPartialView(long modId)
        {
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;
            Session["IdTipoDocumento"] = IdTipoDocumento;

            long IdDocumento = long.Parse(Session["IdDocumento"].ToString());
            int IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion = int.Parse(Session["IdVersion"].ToString());

            PersonaModel model = new PersonaModel
            {
                Cargo = new CargoModel(),
                CorreosElectronicos = new List<PersonaEmail>(),
                Direcciones = new List<PersonaDireccion>(),
                EditDocumento= true,
                IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString()),
                IdCargoPersona = 0,
                IdEmpresa = long.Parse(Session["IdEmpresa"].ToString()),
                Identificacion = string.Empty,
                IdModulo = IdModulo,
                IdPersona = 0,
                IdUnidadOrganizativaPersona = 0,
                IdUsuario = 0,
                Nombre = string.Empty,
                returnPage = Url.Action("Ficha", "Documentos", new { modId }),
                Telefonos = new List<PersonaTelefono>(),
                UnidadOrganizativa = new UnidadOrganizativaModel()
            };
            //return PartialView(model);
            return View(model);
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
        public JsonResult CheckEmails(IList<PersonaEmail> data)
        {
            bool isValid = data.Count() > 0;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckTelefonos(IList<PersonaTelefono> data)
        {
            bool isValid = data.Count() > 0;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosPersonaSelected(long IdPersona)
        {
            bool success = false;
            string Cargo = string.Empty;
            string UnidadOrganizativa = string.Empty;

            PersonaModel persona = Metodos.GetPersonaById(IdPersona);
            if (persona != null)
            {
                Cargo = persona.Cargo.NombreCargo;
                UnidadOrganizativa = persona.UnidadOrganizativa.NombreCompleto;
            }
            return Json(new { success, Cargo, UnidadOrganizativa });
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