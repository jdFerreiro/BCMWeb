using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BCMWeb
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpContext ctx = HttpContext.Current;

            if (ctx.Session["UserId"] == null)
            {

                IPrincipal user = ctx.User;
                IIdentity userIdentity = user.Identity;

                FormsIdentity id = (FormsIdentity)user.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                if (ticket != null)
                {
                    long IdUser = long.Parse(ticket.UserData);
                    if (IdUser > 0)
                    {
                        Metodos.Logout(IdUser);

                        filterContext.Result = new RedirectResult("~/Account/Login");
                        FormsAuthentication.SignOut();
                        return;
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
}