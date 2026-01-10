using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaBloqueioUsuario
    {

        public static Funcionario PesquisarFuncionarioPorId(int id)
        {
            return Funcionarios.ObtemFuncionario(id);
        }

        public static IQueryable<Empresa> ListaConsignatarias()
        {
            List<int> idEmpresasComProduto = Produtos.ListaProdutos().Select(x => x.IDConsignataria).Distinct().ToList();
            return Empresas.ListaConsignatarias().Where(x => idEmpresasComProduto.Contains(x.IDEmpresa));
        }

        public static IQueryable<Produto> ListaProdutos(int id)
        {
            return Empresas.ListaProdutos(id);
        }

        public static IQueryable<FuncionarioBloqueio> ObtemBloqueios(int idFuncionario)
        {
            return Funcionarios.ObtemBloqueios(idFuncionario);
        }

        public static IQueryable<Produto> ListaProdutos(List<int> idsEmpresas)
        {
            return Produtos.ListaProdutos(idsEmpresas);
        }

        public static void SalvaBloqueios(int idFuncionario, int tipoBloqueio, List<int> idsProdutosBloqueio, string motivo, int idAutor)
        {
            Funcionarios.SalvaBloqueios(idFuncionario, tipoBloqueio, idsProdutosBloqueio, motivo, idAutor);
        }

        public static void RemoveBloqueio(int idFuncionarioBloqueio, int idAutor)
        {
            Funcionarios.RemoveBloqueio(idFuncionarioBloqueio, idAutor);
        }

        public static void RemoverBloqueios(int idFuncionario)
        {
            Funcionarios.RemoveBloqueios(idFuncionario);
        }

        public static Usuario ObtemUsuario(int idUsuario)
        {
            return Usuarios.ObtemUsuario(idUsuario);
        }

        public static Produto ObtemProduto(int idProduto)
        {
            return Empresas.ObtemProduto(idProduto);
        }

    }

}