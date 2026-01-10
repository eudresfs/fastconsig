using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{
    public static class Gerencial
    {
        public static IQueryable<TmpColocacaoConvenio> ListarColocacaoConvenio()
        {
            return new Repositorio<TmpColocacaoConvenio>().Listar();
        }
    }
}
