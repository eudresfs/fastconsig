using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web;

namespace CP.FastConsig.WebApplication
{

    public partial class Default : CustomPage
    {

        private const string PathPaginaLogin = "~/Login";

        override protected void OnInit(EventArgs e)
        {

            base.OnInit(e);

            if (Context.Session != null)
            {

                if (Session.IsNewSession)
                {
                    string szCookieHeader = Request.Headers["Cookie"];
                    if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0)) Response.Redirect(PathPaginaLogin);
                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Sessao.Logado) Response.Redirect(PathPaginaLogin);
        }

        [WebMethod]
        public static List<string> PesquisaIncremental(string prefixText, int count)
        {
            return FachadaDefault.PesquisaIncremental(prefixText, count);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SalvaFiltros(string parametros)
        {
            return parametros;
        }

    }

}