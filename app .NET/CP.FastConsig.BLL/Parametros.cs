using System;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Parametros
    {

        public static bool AtivaAbasSistema()
        {
            Parametro parametro = new Repositorio<Parametro>().Listar().SingleOrDefault(x => x.Nome.Equals("AtivaAbasSistema"));
            return parametro == null ? false : Convert.ToBoolean(parametro.Valor);
        }

    }

}