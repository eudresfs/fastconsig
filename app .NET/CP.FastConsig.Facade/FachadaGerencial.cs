using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{
    public class FachadaGerencial
    {
        public static IQueryable<TmpColocacaoConvenio> ListarColocacaoConvenio()
        {
            return Gerencial.ListarColocacaoConvenio();
        }
        
        public static Parametro obterParametro(string nome)
        {
            return Geral.ObtemParametro(nome);
        }
    }
}
