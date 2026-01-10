using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Modulos
    {

        public static List<Modulo> ObtemModulos()
        {
            return new Repositorio<Modulo>().Listar().OrderBy(x => x.Nome).ToList();
        } 

    }

}