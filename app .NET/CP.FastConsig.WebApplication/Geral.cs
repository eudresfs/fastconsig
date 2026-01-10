using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication
{
    public static class Geral
    {
        public static CustomPage ObterUserControl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Handler is CustomPage)
                return (CustomPage)HttpContext.Current.Handler;
            else
                return null;
        }
    }
}