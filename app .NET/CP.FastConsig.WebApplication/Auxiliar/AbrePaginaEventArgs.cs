using System;

namespace CP.FastConsig.WebApplication.Auxiliar
{

    public class AbrePaginaEventArgs : EventArgs
    {

        public string NomeWebUserControl { get; set; }

        public object[] Parametros { get; set; }

    }

}