using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;
using System.Data.Objects;
using Microsoft.Data.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace CP.FastConsig.BLL
{

    public static class Funcionarios
    {

        public static IQueryable<Funcionario> ObtemFuncionariosPesquisa(string prefixText)
        {
            return new Repositorio<Funcionario>().PesquisaTextual(prefixText,"");
        }

        public static IQueryable<Funcionario> PesquisarFuncionarios(string texto)
        {
            return new Repositorio<Funcionario>().Listar().Where(x => x.Matricula == texto || x.Pessoa.Nome.Contains(texto) || (texto.Length == 11 && x.Pessoa.CPF.Contains(texto)) || x.NomeSituacaoFolha.Contains(texto) || x.NomeLocalFolha.Contains(texto) || x.NomeRegimeFolha.Contains(texto)); //PesquisaTextual(texto,"");
        }

        public static IQueryable<Pessoa> PesquisarPessoas(string texto)
        {
            return new Repositorio<Pessoa>().Listar().Where(x => x.Nome.Contains(texto) || x.CPF.Contains(texto) || x.RG.Contains(texto)); //PesquisaTextual(texto,"");
        }

        public static Funcionario ObtemFuncionario(int id)
        {
            return new Repositorio<Funcionario>().ObterPorId(id);
        }
        
        public static Funcionario ObtemFuncionario(string matricula)
        {
            return new Repositorio<Funcionario>().Listar().Where(x => x.Matricula == matricula).FirstOrDefault();
        }
        
        public static Pessoa ObtemPessoa(string matriculaCpf)
        {
            Funcionario func =
                new Repositorio<Funcionario>().Listar().Where(x => x.Matricula == matriculaCpf).FirstOrDefault();
            if (func == null) return new Repositorio<Pessoa>().Listar().Where(x => x.CPF == matriculaCpf).FirstOrDefault();
            return func.Pessoa;
        }

        public static void SalvaBloqueio(FuncionarioBloqueio funcionarioBloqueio)
        {

            Repositorio<FuncionarioBloqueio> repositorio = new Repositorio<FuncionarioBloqueio>();

            List<FuncionarioBloqueio> bloqueiosIguais = repositorio.Listar().Where(x => x.IDFuncionario.Equals(funcionarioBloqueio.IDFuncionario) && x.Chaves.Equals(funcionarioBloqueio.Chaves)).ToList();

            if(!bloqueiosIguais.Any()) repositorio.Incluir(funcionarioBloqueio);

        }

        public static IQueryable<FuncionarioBloqueio> ObtemBloqueios(int idFuncionario)
        {
            return new Repositorio<FuncionarioBloqueio>().Listar().Where(x => x.IDFuncionario.Equals(idFuncionario));
        }

        public static void SalvaBloqueios(int idFuncionario, int tipoBloqueio, IEnumerable<int> idsProdutosBloqueio, string motivo, int idAutor)
        {

            if (idsProdutosBloqueio == null)
            {

                FuncionarioBloqueio funcionarioBloqueio = CriaFuncionarioBloqueio(motivo, 0, tipoBloqueio.ToString(), idFuncionario, idAutor);

                RemoveBloqueios(idFuncionario);
                SalvaBloqueio(funcionarioBloqueio);

            }
            else
            {

                List<FuncionarioBloqueio> bloqueiosAnteriores = ObtemBloqueios(idFuncionario).ToList();

                if (bloqueiosAnteriores.Any(x => x.Chaves.Equals("0"))) return;

                foreach (int idProduto in idsProdutosBloqueio)
                {

                    FuncionarioBloqueio funcionarioBloqueio = CriaFuncionarioBloqueio(motivo, idProduto, tipoBloqueio.ToString(), idFuncionario, idAutor);
                    SalvaBloqueio(funcionarioBloqueio);

                }

            }

        }

        public static void RemoveBloqueios(int idFuncionario)
        {
            Repositorio<FuncionarioBloqueio> repositorio = new Repositorio<FuncionarioBloqueio>();
            repositorio.Listar().Where(x => x.IDFuncionario.Equals(idFuncionario)).ToList().ForEach(x => repositorio.Excluir(x));
        }

        private static FuncionarioBloqueio CriaFuncionarioBloqueio(string motivo, int idProduto, string tipoBloqueio, int idFuncionario, int idAutor)
        {

            FuncionarioBloqueio funcionarioBloqueio = new FuncionarioBloqueio();

            funcionarioBloqueio.Motivo = motivo;
            funcionarioBloqueio.DataBloqueio = DateTime.Now;
            funcionarioBloqueio.Chaves = idProduto.ToString();
            funcionarioBloqueio.Ativo = 1;
            funcionarioBloqueio.TipoBloqueio = tipoBloqueio;
            funcionarioBloqueio.IDFuncionario = idFuncionario;
            funcionarioBloqueio.CreatedBy = idAutor;

            return funcionarioBloqueio;

        }

        public static void RemoveBloqueio(int idFuncionarioBloqueio, int idAutor)
        {

            Repositorio<FuncionarioBloqueio> repositorio = new Repositorio<FuncionarioBloqueio>();

            FuncionarioBloqueio funcionarioBloqueio = repositorio.ObterPorId(idFuncionarioBloqueio);

            funcionarioBloqueio.DataDesbloqueio = DateTime.Now;
            funcionarioBloqueio.ModifiedBy = idAutor;
            funcionarioBloqueio.Ativo = 0;

            repositorio.Alterar(funcionarioBloqueio);

        }

        private static FuncionarioBloqueio ObtemBloqueio(int idFuncionarioBloqueio)
        {
            return new Repositorio<FuncionarioBloqueio>().ObterPorId(idFuncionarioBloqueio);
        }

        public static List<int> ObtemIdsFuncionarios(int idUsuario)
        {
            List<int> idsPessoas = new Repositorio<Pessoa>().Listar().Where(x => x.IDUsuario != null & x.IDUsuario.Value.Equals(idUsuario)).Select(x => x.IDPessoa).ToList();
            return new Repositorio<Funcionario>().Listar().Where(x => idsPessoas.Contains(x.IDPessoa)).Select(x => x.IDFuncionario).ToList();
        }


        public static void RemoveAutorizacao(int id)
        {
            Repositorio<FuncionarioAutorizacao> repositorio = new Repositorio<FuncionarioAutorizacao>();

            FuncionarioAutorizacao func = repositorio.ObterPorId(id);

            repositorio.Excluir(func);
        }

        public static FuncionarioAutorizacao ObtemAutorizacao(int IdAutorizacaoEdicao)
        {
            return new Repositorio<FuncionarioAutorizacao>().ObterPorId(IdAutorizacaoEdicao);
        }

        public static IQueryable<FuncionarioAutorizacaoTipo> ListaAutorizacoesTipo()
        {
            return new Repositorio<FuncionarioAutorizacaoTipo>().Listar();
        }

        public static void SalvarAutorizacao(FuncionarioAutorizacao dado)
        {

            Repositorio<FuncionarioAutorizacao> repositorio = new Repositorio<FuncionarioAutorizacao>();
            if (dado.IDFuncionarioAutorizacao == 0) repositorio.Incluir(dado);
            else
            {//repositorio.Atachar(empresa);
                repositorio.Alterar(dado);
            }
        }

        public static IQueryable<FuncionarioCategoria> ListarFuncionarioCategoria()
        {
            return new Repositorio<FuncionarioCategoria>().Listar();
        }

        public static IQueryable<string> ListarFuncionarioRegime()
        {
            return new Repositorio<Funcionario>().Listar().Select(x => x.NomeRegimeFolha).Distinct().OrderBy(x => x);
        }

        public static IQueryable<FuncionarioSituacao> ListarFuncionarioSituacao()
        {
            return new Repositorio<FuncionarioSituacao>().Listar();
        }

        public static IEnumerable<GrupoMargem> ListarMargens(int idFuncionario)
        {

            Funcionario funcionario = new Repositorio<Funcionario>().ObterPorId(idFuncionario);

            if(funcionario == null) return new List<GrupoMargem>();

            ICollection<FuncionarioMargem> lista = funcionario.FuncionarioMargem;

            try
            {
                IEnumerable<GrupoMargem> grupo = from x in lista
                                                 group x by x.ProdutoGrupo
                                                     into g
                                                     select
                                                         new GrupoMargem
                                                         {
                                                             IDProdutoGrupo = g.Key.IDProdutoGrupo,
                                                             Nome = g.Key.Nome,
                                                             MargemFolha = g.Sum(x => x.MargemFolha),
                                                             MargemUtilizada = g.Select(w => w.Funcionario.Averbacao.Where(y => y.Produto.IDProdutoGrupo == g.Key.IDProdutoGrupoCompartilha && y.AverbacaoSituacao.DeduzMargem).Sum(k => k.ValorParcela)).Sum() //g.Key.FuncionarioMargem.Sum(x => x.Funcionario.Averbacao.Where(w => w.Produto.IDProdutoGrupo == g.Key.IDProdutoGrupoCompartilha).Sum(y => y.ValorParcela))
                                                         };

                return grupo;

            }
            catch (Exception ex)
            {
                return new List<GrupoMargem>();
                // TODO - Magnum - Retirar este tratamento. Fazer tratamento na consulta linq.
            }

            

        }



        public static IQueryable<FuncionarioAutorizacao> ListaAutorizacoesDoFunc(int idfunc)
        {
            return new Repositorio<FuncionarioAutorizacao>().Listar().Where(x => x.IDFuncionario == idfunc);
        }

        public static List<Funcionario> ObtemFuncionariosPorPessoa(int idPessoa)
        {
            return new Repositorio<Funcionario>().Listar().Where(x => x.IDPessoa == idPessoa).ToList();
        }

        public static List<Averbacao> ObtemAverbacoesAtivas(int idFuncionario)
        {
            return new Repositorio<Averbacao>().Listar().Where(x => x.IDFuncionario == idFuncionario && x.AverbacaoSituacao.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo).ToList();
        }

        public static List<string> ListaLocais()
        {
            return new Repositorio<Funcionario>().Listar().Where(x => !string.IsNullOrEmpty(x.NomeLocalFolha)).Select(x => x.NomeLocalFolha).Distinct().ToList();
        }

        public static List<string> ListaCargos()
        {
            return new Repositorio<Funcionario>().Listar().Where(x => !string.IsNullOrEmpty(x.NomeCargoFolha)).Select(x => x.NomeCargoFolha).Distinct().ToList();
        }

        public static IEnumerable<string> ListaSituacoes()
        {
            return new Repositorio<Funcionario>().Listar().Where(x => !string.IsNullOrEmpty(x.NomeSituacaoFolha)).Select(x => x.NomeSituacaoFolha).Distinct().ToList();
        }

        public static List<string> ListaSetores()
        {
            return new Repositorio<Funcionario>().Listar().Where(x => !string.IsNullOrEmpty(x.NomeSetorFolha)).Select(x => x.NomeSetorFolha).Distinct().ToList();
        }

        public static string GerarSenha(int IdFunc)
        {
            Random numero = new Random();
            string senha = numero.Next(100000,999999).ToString();

            Repositorio<Funcionario> rep = new Repositorio<Funcionario>();
            Funcionario func = rep.ObterPorId(IdFunc);
            func.Pessoa.Usuario.SenhaProvisoria = senha;
            rep.Alterar(func);

            return senha;
        }

        public static void GerarMargens(int IdFunc)
        {
            Funcionario func = ObtemFuncionario(IdFunc);

            Repositorio<FuncionarioMargem> rep_fm = new Repositorio<FuncionarioMargem>();

            Repositorio<ProdutoGrupo> rep_pd = new Repositorio<ProdutoGrupo>();
            var produtosgrupos = rep_pd.Listar().Where(x => x.PercentualMargemBruta > 0);
            foreach (var item in produtosgrupos)
            {
                FuncionarioMargem fm = new FuncionarioMargem();
                fm.IDFuncionario = IdFunc;
                fm.IDProdutoGrupo = item.IDProdutoGrupo;
                fm.MargemFolha = ((func.MargemBruta ?? 0) * (item.PercentualMargemBruta ?? 0))/100;

                rep_fm.Incluir(fm);
            }
        }

        public static void Aposentar(int IdFunc)
        {
            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_FuncAposentar", CommandType.StoredProcedure, new SqlParameter("IDFunc", IdFunc));

            int result;
            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando.ExecuteNonQuery();
            }

        }

        public static void Importacao(int IdImportacao)
        {
            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_ImportacaoFuncionario", CommandType.StoredProcedure, new SqlParameter("IDImportacao", IdImportacao));

            int result;
            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando.ExecuteNonQuery();
            }

        }
    }

}