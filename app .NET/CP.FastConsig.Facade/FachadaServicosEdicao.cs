using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaProdutosEdicao
    {

        public static Produto ObtemProduto(int idProduto)
        {
            return Empresas.ObtemProduto(idProduto);
        }

        public static void SalvarProduto(Produto serv)
        {
            Empresas.SalvarProduto(serv);
        }

        public static IQueryable<Empresa> ListaConsignantes()
        {
            return Empresas.ListaConsignantes();
        }

        public static IQueryable<ProdutoGrupo> ListaProdutoGrupos()
        {
            return Consignatarias.ListaProdutoGrupos();
        }

    }

}