using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaReajustarContratos
    {

        public static IEnumerable<string> ListaProdutos()
        {
            return Produtos.ListaProdutos().Where(x => x.IDProdutoGrupo.Equals((int)Enums.ProdutoGrupo.Mensalidades) && !string.IsNullOrEmpty(x.Nome)).Select(x => x.Nome).Distinct().ToList();
        }

    }

}