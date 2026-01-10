using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.FastConsig.DAL
{
    public partial class TmpEnviadosVDescontados
    {

        public TmpEnviadosVDescontados(string tipo, string mes , decimal? valor)
        {
            Tipo = tipo;
            Mes = mes;
            Valor = valor;
        }

        public TmpEnviadosVDescontados(){}

    }
}
