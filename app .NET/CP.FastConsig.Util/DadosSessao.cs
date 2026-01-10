using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CP.FastConsig.Util
{
    public static class DadosSessao
    {
        public static int IdModulo
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdModulo"] != null)
                    return ((int)HttpContext.Current.Session["IdModulo"]);
                return 0;
            }
        }

        public static int IdBanco
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdBanco"] != null)
                    return ((int)HttpContext.Current.Session["IdBanco"]);
                return 0;
            }
        }

        public static int IdPerfil
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdPerfil"] != null)
                    return ((int)HttpContext.Current.Session["IdPerfil"]);
                return 0;
            }
        }

        public static int IdUsuario
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdUsuario"] != null)
                    return ((int)HttpContext.Current.Session["IdUsuario"]);
                return 0;
            }
        }

        public static int IdRecurso
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdRecursoAtual"] != null)
                    return ((int)HttpContext.Current.Session["IdRecursoAtual"]);
                return 0;
            }
        }

        public static string NomeRecurso
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["NomeRecursoAtual"] != null)
                    return ((string)HttpContext.Current.Session["NomeRecursoAtual"]);
                return "";
            }
        } 

    }
}
