using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class PerfilModelView
    {
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.FichaResource))]
        public long IdUsuario { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEmail", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        [RegularExpression("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessageResourceName = "InvalidoErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.Password)]
        [Display(Name = "captionPassword", ResourceType = typeof(Resources.LoginResource))]
        public string Password { get; set; }
        [Required(ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [DataType(DataType.Password)]
        [Display(Name = "captionPasswordConfirm", ResourceType = typeof(Resources.LoginResource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "CompareErrorFemalevsFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string PasswordCompare { get; set; }
        private bool _Changed = false;
        public bool Changed { get { return _Changed; } set { _Changed = value; } }

    }
}