using System;
using System.Web.Routing;
using System.Web;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;

namespace CP.FastConsig.WebApplication
{

    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {

            routes.MapPageRoute("Principal", "Portal", "~/Default.aspx");
            routes.MapPageRoute("Login", "Login", "~/Login.aspx");
            routes.MapPageRoute("RotaBloqueada", "{controller}.aspx", "~/Default.aspx");

        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exc = Server.GetLastError();

        //    // Handle HTTP errors
        //    if (exc.GetType() == typeof(HttpException))
        //    {
        //        // The Complete Error Handling Example generates
        //        // some errors using URLs with "NoCatch" in them;
        //        // ignore these here to simulate what would happen
        //        // if a global.asax handler were not implemented.
        //        if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
        //            return;

        //    }

        //    FachadaMaster.RegistrarErro(DadosSessao.IdModulo, DadosSessao.IdBanco, DadosSessao.IdPerfil, DadosSessao.IdUsuario == null ? 0 : DadosSessao.IdUsuario, exc.Message);


        //    // Clear the error from the server
        //    Server.ClearError();
        //}
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

       

    }

}