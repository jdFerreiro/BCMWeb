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

namespace BCMWeb.Controllers {
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller {

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login() {
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
            return View(model);
        }
        public ActionResult LogOff() {
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

        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public ActionResult ChangePasswordSuccess() {
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