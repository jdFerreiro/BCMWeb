using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class PruebaModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaPlanificacion", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [FechaMenorIgualAHoy(ErrorMessageResourceName = "FechaMenorIgualQueHoy", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime FechaInicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionUbicacion", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Ubicacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionProposito", ResourceType = typeof(Resources.PPEResource))]
        public string Proposito { get; set; }
        [Display(Name = "captionFechaEjecucion", ResourceType = typeof(Resources.PPEResource))]
        public DateTime FechaEjecucion { get; set; }
        public bool Ejecucion { get; set; }
        public short IdEstatus { get; set; }
    }
    public class PruebaEjercicioModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        public long IdEjercicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDescripcionEjercicio", ResourceType = typeof(Resources.PPEResource))]
        public string Descripcion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionInicioPlanEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Inicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDuracionPlanEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int DuracionHoras { get; set; }
        [Display(Name = "captionEjecutado", ResourceType = typeof(Resources.PPEResource))]
        public bool Ejecutado { get; set; }
    }
    public class PruebaParticipanteModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        public long IdParticipante { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreEmpresa", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string EmpresaParticipante { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreParticipante", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string NombreParticipante { get; set; }
        [Display(Name = "captionResponsable", ResourceType = typeof(Resources.PPEResource))]
        public bool Responsable { get; set; }
        [Display(Name = "captionPlanificado", ResourceType = typeof(Resources.PPEResource))]
        public bool Planificado { get; set; }
        [Display(Name = "captionUtilizado", ResourceType = typeof(Resources.PPEResource))]
        public bool Utilizado { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEmail", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        [RegularExpression("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessageResourceName = "InvalidoErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string CorreoElectronico { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTelefono", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string NroTelefono { get; set; }
        public bool Ejecucion { get; set; }
    }
    public class PruebaParticipanteEjercicioModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreEjercicio", ResourceType = typeof(Resources.PPEResource))]
        public long IdEjercicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreParticipante", ResourceType = typeof(Resources.PPEResource))]
        public long IdParticipante { get; set; }
        [Display(Name = "captionResponsable", ResourceType = typeof(Resources.PPEResource))]
        public bool Responsable { get; set; }
    }
    public class PruebaRecursoEjercicioModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreEjercicio", ResourceType = typeof(Resources.PPEResource))]
        public long IdEjercicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreRecurso", ResourceType = typeof(Resources.PPEResource))]
        public long IdRecurso { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreRecurso", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDescripcionRecurso", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Descripcion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCantidadRecurso", ResourceType = typeof(Resources.PPEResource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int Cantidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionResponsableRecurso", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Responsable { get; set; }
    }
    public class PruebaResultadoModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        public long IdEjercicio { get; set; }
        public long IdResultado { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionResponsableRecurso", ResourceType = typeof(Resources.PPEResource))]
        public long IdResponsable { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaInicio", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Inicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaFinal", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Final { get; set; }
    }
    public class PruebaEjecucionModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.PPEResource))]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaEjecucion", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [FechaMayorIgualAHoy(ErrorMessageResourceName = "FechaMayorIgualAHoy", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime FechaInicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionUbicacion", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Ubicacion { get; set; }
        [Display(Name = "captionProposito", ResourceType = typeof(Resources.PPEResource))]
        public string Proposito { get; set; }
    }
    public class PruebaEjercicioEjecucionModel : ModulosUserModel
    {
        public long IdPrueba { get; set; }
        public long IdEjercicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [StringLength(500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDescripcionEjercicio", ResourceType = typeof(Resources.PPEResource))]
        public string Descripcion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionInicioPlanEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Inicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDuracionPlanEjercicio", ResourceType = typeof(Resources.PPEResource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public int DuracionHoras { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEstadoEjecucion", ResourceType = typeof(Resources.PPEResource))]
        [Range(1, short.MaxValue, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public short IdStatus { get; set; }
        [Display(Name = "captionEjecutado", ResourceType = typeof(Resources.PPEResource))]
        public bool Ejecutado { get; set; }
    }

}