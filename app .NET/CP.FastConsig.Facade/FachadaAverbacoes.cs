using CP.FastConsig.BLL;
using System.Linq;
using CP.FastConsig.DAL;
using System.Collections.Generic;
using CP.FastConsig.Common;
using System;


namespace CP.FastConsig.Facade
{
    public static class FachadaAverbacoes
    {
        public static IQueryable<Funcionario> LocalizarFuncPorMatriculaOuCpf(string texto)
        {
            return Averbacoes.LocalizarFuncPorMatriculaOuCpf(texto);
        }

        public static Funcionario ObtemFuncionario(int id)
        {
            return Funcionarios.ObtemFuncionario(id);
        }

        public static IQueryable<ProdutoGrupo> ObtemProdutosGrupo()
        {
            return Produtos.ListaProdutosGrupo();
        }

        public static IQueryable<Produto> ObtemProdutosDoGrupo(int idempresa, int idprodutogrupo)
        {
            return Produtos.ListaProdutosPorGrupo(idprodutogrupo).Where(x => x.IDConsignataria == idempresa && !x.DesativadoConsignante).OrderBy(x => x.Nome);
        }

        public static int SalvarAverbacao(Averbacao dado, Pessoa pessoa, int idprodutogrupo, bool bAprovacaoFuncionario, List<int> listaRefinancia, List<int> listaCompra)
        {
            return Averbacoes.SalvarAverbacao(dado, pessoa, idprodutogrupo, bAprovacaoFuncionario, listaRefinancia, listaCompra);
        }

        public static void SalvarCompra(Averbacao dado, Pessoa pessoa, int idprodutogrupo, List<int> listaRefinancia, List<int> listaCompra)
        {
            Averbacoes.SalvarCompra(dado, pessoa, idprodutogrupo, listaRefinancia, listaCompra);
        }

        public static string ObtemAnoMesCorte(int idmodulo, int idempresa)
        {
            return Averbacoes.ObtemAnoMesCorte(idmodulo, idempresa);
        }

        public static List<Estado> ObtemEstados()
        {
            return Geral.ObtemEstados();
        }

        public static IQueryable<Produto> ObtemProdutosDoEmpresa(int idempresa)
        {
            return Produtos.ListaProdutos().Where(x => x.IDConsignataria == idempresa && !x.DesativadoConsignante).OrderBy(x => x.Nome).OrderBy(x => x.IDProdutoGrupo);
        }

        public static Produto ObtemProduto(int idproduto)
        {
            return Averbacoes.ObtemProduto(idproduto);
        }

        public static Empresa ObtemEmpresa(int idempresa)
        {
            return new Repositorio<Empresa>().ObterPorId(idempresa);
        }

        public static bool RequerAprovacao(int idmodulo, int IdProdutoGrupo, int idempresa)
        {
            return Averbacoes.RequerAprovacao(idmodulo, IdProdutoGrupo, idempresa);
        }

        public static void salvarSaldoDevedor(int idsolicitacao, int averbacao, int idempresa, int idresponsavel, DateTime data, DateTime validade, decimal valor, int idTipoPagamento, string identificador, string banco, string agencia, string contacredito, string nomefavorecido, string observacao)
        {
            Averbacoes.SalvaInformacaoSaldoDevedor(idsolicitacao, averbacao, idempresa, idresponsavel, data, validade, valor, idTipoPagamento, identificador, banco, agencia, contacredito, nomefavorecido, observacao);
        }

        public static IQueryable<Averbacao> AverbacoesParaComprar(IQueryable<Averbacao> listaAverbacoesDoFunc, int idempresa)
        {
            return Averbacoes.AverbacoesParaComprar(listaAverbacoesDoFunc, idempresa);
        }

        public static IQueryable<Averbacao> AverbacoesParaRefinanciar(IQueryable<Averbacao> listaAverbacoesDoFunc, int idempresa)
        {
            return Averbacoes.AverbacoesParaRefinanciar(listaAverbacoesDoFunc, idempresa);
        }



        public static Averbacao ObtemAverbacao(int ID)
        {
            return Averbacoes.ObtemAverbacao(ID);
        }

        public static IQueryable<Averbacao> AverbacoesCompradas(int IdAverbacao)
        {
            return Averbacoes.AverbacoesCompradas(IdAverbacao);
        }

        public static IQueryable<Averbacao> AverbacoesRefinanciadas(int IdAverbacao)
        {
            return Averbacoes.AverbacoesRefinanciadas(IdAverbacao);
        }

        public static int? ObtemProdutoGrupoDeProduto(string idproduto)
        {
            if (string.IsNullOrEmpty(idproduto))
                return null;

            var produto = FachadaAverbacoes.ObtemProduto(Convert.ToInt32(idproduto));
            if (produto != null)
            {
                return produto.IDProdutoGrupo;
            }
            else
            {
                return null;
            }
        }

        public static string ObtemDescricaoAverbacaoTipo(int averbacaoTipo)
        {
            return Averbacoes.ObtemDescricaoAverbacaoTipo(averbacaoTipo);
        }

        public static int ObtemParcelasRestantes(int idaverbacao)
        {
            return Averbacoes.CalculaPrazoRestante(idaverbacao);
            //return Averbacoes.ObtemParcelasRestantes(idaverbacao);
        }


        public static decimal CalculaValorDeducaoMargem(int IdAverbacao)
        {
            return Averbacoes.CalculaValorDeducaoMargem(IdAverbacao);
        }

        public static decimal CalculaRefinanciaQueDeduzMargem(List<int> lista)
        {
            return Averbacoes.CalculaRefinanciaQueDeduzMargem(lista);
        }

        public static decimal CalculaCompraQueDeduzMargem(List<int> lista)
        {
            return Averbacoes.CalculaCompraQueDeduzMargem(lista);
        }

        public static ProdutoGrupo ObtemProdutoGrupo(int IdProdutoGrupo)
        {
            return Averbacoes.ObtemProdutoGrupo(IdProdutoGrupo);
        }

        public static string ObtemPrazoMaximo(int idproduto, int idprodutogrupo)
        {
            return Averbacoes.ObtemPrazoMaximo(idproduto, idprodutogrupo);
        }

        public static bool VerificaExistePrevenirDuplicacao(Averbacao averb)
        {
            return Averbacoes.VerificaExistePrevenirDuplicacao(averb);
        }

        public static IEnumerable<AnaliseProducao> RelatorioAnaliseProducao(string mesinicio, string mesfim, int idempresa)
        {
            return Averbacoes.RelatorioAnaliseProducao(idempresa, mesinicio, mesfim);
        }

        public static IQueryable<Averbacao> listaAverbacoesFuncionario(int idfuncionario, int situacao)
        {
            return Averbacoes.listaAverbacoesFuncionario(idfuncionario, situacao);
        }

        public static void IncluirAverbacao(Averbacao a)
        {
            Averbacoes.IncluirAverbacao(a);
        }
    }

}