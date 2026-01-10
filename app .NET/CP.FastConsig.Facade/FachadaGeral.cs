using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;
using System.Web;

namespace CP.FastConsig.Facade
{
    public static class FachadaGeral
    {
        public static Parametro obtemParametro(string parametro)
        {
            return Geral.ObtemParametro(parametro);
        }

        public static void atualizaParametro(string parametro, string valor)
        {
            Geral.atualizaParametro(parametro, valor);
        }

        public static string IdEmpresaConsignante()
        {
            return Geral.IdEmpresaConsignante();
        }

        public static string ObtemIP(HttpRequest request)
        {
            return Geral.ObtemIP(request);
        }

        public static string ObtemBrowser(HttpRequest request)
        {
            return Geral.ObtemBrowser(request);
        }

        public static void ProcessaInicializacao()
        {
            Geral.ProcessaInicializacao();
        }

    }
}
