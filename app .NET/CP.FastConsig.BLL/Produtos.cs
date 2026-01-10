using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Produtos
    {

        public static IQueryable<Produto> ListaProdutosPorGrupo(int idProdutoGrupo)
        {
            return new Repositorio<Produto>().Listar().Where(x => x.IDProdutoGrupo.Equals(idProdutoGrupo)).OrderBy(x => x.IDConsignataria);
        }

        public static IQueryable<Produto> ListaProdutos(List<int> idsEmpresas)
        {
            return new Repositorio<Produto>().Listar().Where(x => idsEmpresas.Contains(x.IDConsignataria));
        }

        public static IQueryable<Produto> ListaProdutos(int idEmpresa, int idProdutoGrupo)
        {
            return new Repositorio<Produto>().Listar().Where(x => x.IDConsignataria == idEmpresa && x.IDProdutoGrupo == idProdutoGrupo );
        }

        public static IQueryable<Produto> ListaProdutos()
        {
          return new Repositorio<Produto>().Listar();
        }

        public static IQueryable<ProdutoGrupo> ListaProdutosGrupo()
        {
            return new Repositorio<ProdutoGrupo>().Listar();
        }

        public static bool existeMaisDeUmProduto(int idconsignataria)
        {
            return new Repositorio<Produto>().Listar().Where(x => x.IDConsignataria == idconsignataria).Select(x => x.IDProdutoGrupo).Distinct().Count() > 1;
        }
    }

}