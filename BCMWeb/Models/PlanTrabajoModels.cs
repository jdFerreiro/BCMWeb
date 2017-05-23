using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class IniciativaModel : ModulosUserModel
    {
        [Display(Name = "IdIniciativa", ResourceType = typeof(Resources.IniciativaResource))]
        public long IdIniciativa { get; set; }
        [Display(Name = "NroIniciativa", ResourceType = typeof(Resources.IniciativaResource))]
        public int NroIniciativa { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "Nombre", ResourceType = typeof(Resources.IniciativaResource))]
        [StringLength(1500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Display(Name = "Descripcion", ResourceType = typeof(Resources.IniciativaResource))]
        public string Descripcion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "UnidadOrganizativa", ResourceType = typeof(Resources.IniciativaResource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public long? IdUnidadOrganizativa { get; set; }
        [Display(Name = "UnidadOrganizativa", ResourceType = typeof(Resources.IniciativaResource))]
        public string UnidadOrganizativa
        {
            get
            {
                return IdUnidadOrganizativa == null ? string.Empty : Metodos.GetNombreUnidadCompleto((long)IdUnidadOrganizativa);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "Responsable", ResourceType = typeof(Resources.IniciativaResource))]
        [StringLength(1500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Responsable { get; set; }
        [Display(Name = "FechaInicioEstimada", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<DateTime> FechaInicioEstimada { get; set; }
        [Display(Name = "FechaCierreEstimada", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<DateTime> FechaCierreEstimada { get; set; }
        [Display(Name = "FechaInicioReal", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<DateTime> FechaInicioReal { get; set; }
        [Display(Name = "FechaCierreReal", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<DateTime> FechaCierreReal { get; set; }
        [Display(Name = "PresupuestoEstimado", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<decimal> PresupuestoEstimado { get; set; }
        [Display(Name = "PresupuestoReal", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<decimal> PresupuestoReal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "Estatus", ResourceType = typeof(Resources.IniciativaResource))]
        [Range(1, short.MaxValue, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public short IdEstatus { get; set; }
        [Display(Name = "Estatus", ResourceType = typeof(Resources.IniciativaResource))]
        public string Estatus
        {
            get
            {
                return Metodos.GetEstatusIniciativa(IdEstatus);
            }
        }
        [Display(Name = "Urgente", ResourceType = typeof(Resources.IniciativaResource))]
        public Nullable<bool> Urgente { get; set; }
        [Display(Name = "Urgente", ResourceType = typeof(Resources.IniciativaResource))]
        public string strUrgente
        {
            get
            {
                if (Urgente != null && (bool)Urgente)
                {
                    return Resources.IniciativaResource.textoUrgente;
                }
                else
                {
                    return Resources.IniciativaResource.textoNormal;
                }
            }
        }
        [Display(Name = "Observacion", ResourceType = typeof(Resources.IniciativaResource))]
        public string Observacion { get; set; }
    }
}