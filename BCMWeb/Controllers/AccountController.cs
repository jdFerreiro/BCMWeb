using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using BCMWeb.Models;
using BCMWeb.Security;
using System.Security.Principal;

namespace BCMWeb.Controllers {
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller {

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login() {
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, Resources.LoginResource.accountHeader);
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User _User = Repository.GetUserDetails(model.UserName, model.Password);
                if (_User != null)
                {

                    eEstadoUsuario EstatusActual = (eEstadoUsuario)_User.Estatus;

                    switch (EstatusActual)
                    {
                        case eEstadoUsuario.Activo:
                            FormsAuthentication.SetAuthCookie(_User.Email, true);
                            string UserId = _User.Id.ToString();
                            var authTicket = new FormsAuthenticationTicket(1, _User.Name, DateTime.Now, DateTime.Now.AddMinutes(20), false, UserId);
                            string encriptedTicket = FormsAuthentication.Encrypt(authTicket);
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encriptedTicket);
                            HttpContext.Response.Cookies.Add(authCookie);
                            Session["UserId"] = UserId;
                            Session["IdEmpresa"] = Metodos.GetEmpresasUsuario().FirstOrDefault().IdEmpresa;
                            Session["UserTimeZone"] = model.UserTimezone;
                            Metodos.LoginUsuario(long.Parse(UserId));
                            return RedirectToAction("Index", "Menu");
                        case eEstadoUsuario.Bloqueado:
                            ViewBag.ErrorMessage = Resources.ErrorResource.BloqueadoErrorMessage;
                            return View(model);
                        case eEstadoUsuario.Conectado:
                            ViewBag.ErrorMessage = Resources.ErrorResource.ConectadoErrorMessage;
                            return View(model);
                        case eEstadoUsuario.Eliminado:
                            ViewBag.ErrorMessage = Resources.ErrorResource.EliminadoErrorMessage;
                            return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = Resources.ErrorResource.LoginFailError;
                }
            }
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, Resources.LoginResource.accountHeader);
            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult LogOff() {
            long UserId = 0;

            if (Session["UserId"] != null)
            {
                UserId = long.Parse(Session["UserId"].ToString());
            }
            else
            {
                IPrincipal user = HttpContext.User;
                IIdentity userIdentity = user.Identity;

                FormsIdentity id = (FormsIdentity)user.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                if (ticket != null)
                {
                    UserId = long.Parse(ticket.UserData);
                }
            }

            Metodos.Logout(UserId);
            Auditoria.RegistrarLogout();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model) {
            if(ModelState.IsValid) {
                // Attempt to register the user
                try {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return Redirect("/");
                }
                catch(MembershipCreateUserException e) {
                    ViewBag.ErrorMessage = ErrorCodeToString(e.StatusCode);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [SessionExpire]
        [HandleError]
        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult ChangePassword(ChangePasswordModel model) {
            if(ModelState.IsValid) {
                bool changePasswordSucceeded;
                try {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch(Exception) {
                    changePasswordSucceeded = false;
                }
                if(changePasswordSucceeded) {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else {
                    ViewBag.ErrorMessage = Resources.ErrorResource.ChangePasswordFailError;
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        [HandleError]
        public ActionResult ChangePasswordSuccess() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPasword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [HandleError]
        public ActionResult ForgotPasword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (Metodos.ValidarEmailUsuario(model.Email))
                {
                    string _esquema = Request.Url.Scheme;
                    string _hostName = Request.Url.Host;

                    EmailManager.EnviarEmailConfirmPassword(model.Email, this.ControllerContext, RouteTable.Routes, _esquema, _hostName);
                    return RedirectToAction("EmailSent");
                }
                else
                {
                    ModelState.AddModelError("Email", Resources.ErrorResource.EmailError);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        [HandleError]
        public ActionResult RecoveryPage(string eui)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [HandleError]
        public ActionResult RecoveryPage(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (Metodos.ValidarEmailUsuario(model.Email))
                {
                    Metodos.ChangePassword(model.Email, model.Password);
                    return RedirectToAction("ConfirmPaswordChange");
                }
                else
                {
                    ModelState.AddModelError("Email", Resources.ErrorResource.EmailError);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        [HandleError]
        public ActionResult EmailSent()
        {
            return View();
        }
        [AllowAnonymous]
        [HandleError]
        public ActionResult ConfirmPaswordChange()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch(createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.ErrorResource.DuplicateUserNameError;

                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.ErrorResource.DuplicateEmailError;

                case MembershipCreateStatus.InvalidPassword:
                    return Resources.ErrorResource.InvalidPasswordError;

                case MembershipCreateStatus.InvalidEmail:
                    return Resources.ErrorResource.InvalidEmailError;

                case MembershipCreateStatus.InvalidAnswer:
                    return Resources.ErrorResource.InvalidAnswerError;

                case MembershipCreateStatus.InvalidQuestion:
                    return Resources.ErrorResource.InvalidQuestionError;

                case MembershipCreateStatus.InvalidUserName:
                    return Resources.ErrorResource.InvalidUserNameError;

                case MembershipCreateStatus.ProviderError:
                    return Resources.ErrorResource.ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return Resources.ErrorResource.UserRejectedError;

                default:
                    return Resources.ErrorResource.defaultError;
            }
        }
        #endregion
    }
}