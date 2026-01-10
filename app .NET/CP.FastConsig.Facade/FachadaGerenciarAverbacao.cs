using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;

namespace CP.FastConsig.Facade
{
    public static class FachadaGerenciarAverbacao
    {
        public static IQueryable<Averbacao> PesquisarContratos(int IDModulo, int IDEmpresa, string pesquisa,
            DateTime periodoInicio, DateTime periodoFim, bool considerarDataTramitacao, bool considerarDataAverbacao, 
            string compInicio, string compFim, 
            int idSituacaoAverbacao, int idGruopoProduto, int idProduto, 
            int consignataria, int idAgente, int idAverbacaoTipo,
            int prazo, string local, string setor, string cargo, string idFuncionarioRegime,
            int idFuncionarioCategoria, int idFuncionarioSituacao, int idTipoSolicitacao, string nomeSituacaoFolha, bool buscarEmAverbacoesSolicitacao, bool bSolicVariasConsignatarias)
        {

            if (pesquisa.Equals("Procurar por nome, CPF, matrícula, Averbação"))
                pesquisa = string.Empty;

            IQueryable<Averbacao> averbacoes = string.IsNullOrEmpty(pesquisa) ? Averbacoes.ListaAverbacao() : Averbacoes.ListaAverbacao().Where(x => x.Numero == pesquisa || x.Identificador == pesquisa || x.Funcionario.Matricula == pesquisa || x.Funcionario.Pessoa.Nome.Contains(pesquisa));

            if ((!idTipoSolicitacao.Equals(-1)))
            {
                IQueryable<EmpresaSolicitacao> solicitacoes;
                if (idTipoSolicitacao == (int)Enums.SolicitacaoTipo.MinhasSolicitacoesdeCompraAguardandoSaldoDevedor)
                {
                    solicitacoes = Solicitacoes.ObtemSolicitacoesDoSolicitantePendentes(IDEmpresa, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos);
                    averbacoes = solicitacoes.Select(x => x.Averbacao);
                }
                else if (idTipoSolicitacao == (int)Enums.SolicitacaoTipo.MinhasSolicitacoesdeCompraAguardandoLiquidacao)
                {
                    solicitacoes = Solicitacoes.ObtemSolicitacoesDoSolicitantePendentes(IDEmpresa, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao);
                    averbacoes = solicitacoes.Select(x => x.Averbacao);
                }
                else
                {

                    if (IDModulo == (int)Enums.Modulos.Consignataria)
                    {
                        solicitacoes = Solicitacoes.ObtemSolicitacoesPendentes(IDEmpresa);
                    }
                    else
                    {
                        if (bSolicVariasConsignatarias)
                        {
                            solicitacoes = Solicitacoes.ObtemSolicitacoesPendentes(0, "3", false, 0);
                        }
                        else
                        {
                            solicitacoes = Solicitacoes.ObtemSolicitacoesPendentes(Convert.ToInt32(Geral.IdEmpresaConsignante()), "1");
                        }
                    }
                    solicitacoes = solicitacoes.Where(x => x.IDEmpresaSolicitacaoTipo == idTipoSolicitacao);

                    averbacoes = solicitacoes.Select(x => x.Averbacao);

                }

            }
            else
            {

                if (compInicio.Equals("__/____")) compInicio = string.Empty;
                if (compFim.Equals("__/____")) compFim = string.Empty;

                if ((IDModulo == (int)Enums.Modulos.Consignataria) && (!buscarEmAverbacoesSolicitacao))
                    averbacoes = averbacoes.Where(x => x.IDConsignataria == IDEmpresa);
                if (periodoInicio.Year > 1 && considerarDataTramitacao) averbacoes = averbacoes.AsEnumerable().Where(x => x.UltimaTramitacao >= periodoInicio).AsQueryable();
                if (periodoFim.Year > 1 && considerarDataTramitacao) averbacoes = averbacoes.AsEnumerable().Where(x => x.UltimaTramitacao <= periodoFim).AsQueryable();
                if (periodoInicio.Year > 1 && considerarDataAverbacao) averbacoes = averbacoes.Where(x => x.Data >= periodoInicio);
                if (periodoFim.Year > 1 && considerarDataAverbacao) averbacoes = averbacoes.Where(x => x.Data <= periodoFim);
                //if (!string.IsNullOrEmpty(compInicio)) averbacoes = averbacoes.Where(x => x.CompetenciaInicial.CompareTo(compInicio) >= 0);
                //if (!string.IsNullOrEmpty(compFim)) averbacoes = averbacoes.Where(x => x.CompetenciaFinal.CompareTo(compFim) <= 0);
                if (!string.IsNullOrEmpty(nomeSituacaoFolha)) averbacoes = averbacoes.Where(x => !string.IsNullOrEmpty(x.Funcionario.NomeSituacaoFolha) && x.Funcionario.NomeSituacaoFolha.Equals(nomeSituacaoFolha));
                if (idSituacaoAverbacao > 0) 
                {
                    if (idSituacaoAverbacao == 100)
                        averbacoes = averbacoes.Where(x => x.AverbacaoSituacao.DeduzMargem);
                    else if (idSituacaoAverbacao == 102)
                        averbacoes = averbacoes.Where(x => !x.AverbacaoSituacao.DeduzMargem);
                    else if (idSituacaoAverbacao == 103)
                        averbacoes = averbacoes.Where(x => x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Suspenso_MargemLivre || x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Bloqueado_MargemRetida);
                    else if (idSituacaoAverbacao == 104)
                        averbacoes = averbacoes.Where(x => x.AverbacaoSituacao.ParaDescontoFolha.HasValue && x.AverbacaoSituacao.ParaDescontoFolha.Value);
                    else
                        averbacoes = averbacoes.Where(x => x.IDAverbacaoSituacao == idSituacaoAverbacao);
                }
                if (idGruopoProduto > 0) averbacoes = averbacoes.Where(x => x.Produto.IDProdutoGrupo == idSituacaoAverbacao);
                if (idProduto > 0)averbacoes = averbacoes.Where(x => x.IDProduto == idProduto);
                if ((consignataria > 0) && (!buscarEmAverbacoesSolicitacao)) 
                    averbacoes = averbacoes.Where(x => x.IDConsignataria == consignataria);
                if (idAgente > 0) averbacoes = averbacoes.Where(x => x.IDAgente == idAgente);
                if (idAverbacaoTipo > 0) averbacoes = averbacoes.Where(x => x.IDAverbacaoTipo == idAverbacaoTipo);
                if (prazo > 0) averbacoes = averbacoes.Where(x => x.Prazo == prazo);
                if (!string.IsNullOrEmpty(local)) averbacoes = averbacoes.Where(x => x.Funcionario.NomeLocalFolha.Equals(local));
                if (!string.IsNullOrEmpty(setor)) averbacoes = averbacoes.Where(x => x.Funcionario.NomeSetorFolha.Equals(setor));
                if (!string.IsNullOrEmpty(cargo)) averbacoes = averbacoes.Where(x => x.Funcionario.NomeCargoFolha.Equals(cargo));
                if (idFuncionarioRegime != ResourceMensagens.LabelSelecione) averbacoes = averbacoes.Where(x => x.Funcionario.NomeRegimeFolha == idFuncionarioRegime);
                if (idFuncionarioCategoria > 0) averbacoes = averbacoes.Where(x => x.Funcionario.IDFuncionarioCategoria == idFuncionarioCategoria);
                if (idFuncionarioSituacao > 0) averbacoes = averbacoes.Where(x => x.Funcionario.IDFuncionarioSituacao == idFuncionarioSituacao);                
                
            }

            List<Averbacao> retorno = averbacoes.ToList();

            if (buscarEmAverbacoesSolicitacao)
            {
                IEnumerable<Averbacao> averbacoesComSolicitacao = Solicitacoes.ListaContratosSolicitacoes(IDEmpresa);
                List<Averbacao> averbacoesSolic = averbacoesComSolicitacao.ToList();
                //IEnumerable

                var consulta = from s in averbacoesSolic
                               from a in averbacoes
                               where s.IDAverbacao == a.IDAverbacao
                               select s;
                retorno.Clear();                
                retorno.AddRange(consulta); //averbacoesComSolicitacao

            }

            if ((!string.IsNullOrEmpty(compInicio)) && (!string.IsNullOrEmpty(compFim)))
            {
                IEnumerable<Averbacao> averbacoesParcelas = Averbacoes.ListaContratosParcelas(IDEmpresa, IDModulo, compInicio, compFim);
                retorno.AddRange(averbacoesParcelas);
            }

            try
            {
                averbacoes = retorno.Where(x => x != null && x.Funcionario.Pessoa != null).OrderBy(x => x.Funcionario.Pessoa.Nome).AsQueryable();
            }
            catch (Exception ex)
            {
                averbacoes = retorno.AsQueryable();
            }
            
            return averbacoes;

        }

        private static DateTime ObtemDataDeCompetencia(string competencia)
        {

            try
            {


                string[] mesAno = competencia.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                int mes = 1;
                int ano = 1;

                if (mesAno.Count() > 0) mes = Convert.ToInt32(mesAno[0]);
                if (mesAno.Count() > 1) ano = Convert.ToInt32(mesAno[1]);

                return new DateTime(ano, mes, 1);
                
            }
            catch
            {
                return DateTime.MaxValue;
            }

        }

        public static IQueryable<Empresa> ListaConsignataria()
        {
            return Empresas.ListaConsignatarias();
        }

        public static IQueryable<Empresa> ListaAgente()
        {
            return Empresas.ListaAgentes();
        }

        public static IQueryable<AverbacaoSituacao> ListaAverbacaoSituacao()
        {
            return Averbacoes.ListaAverbacaoSituacao().OrderBy(x => x.Nome);
        }

        public static IQueryable<AverbacaoTipo> ListaAverbacaoTipo()
        {
            return Averbacoes.ListaAverbacaoTipo().OrderBy(x => x.Nome);
        }

        public static IQueryable<AverbacaoSituacao> ListaAverbacaoSituacaoDeCompra()
        {
            return Averbacoes.ListaAverbacaoSituacaoDeCompra().OrderBy(x => x.Nome);
        }

        public static IQueryable<ProdutoGrupo> ListaProdutoGrupo()
        {
            return Averbacoes.ListaProdutoGrupo().OrderBy(x => x.Nome);
        }
        
        public static IEnumerable<Produto> ListaProduto()
        {
            return Averbacoes.ListaProduto().OrderBy(x => x.Nome);
        }

        public static IEnumerable<Produto> ListaProduto(int idConsignataria, int idProdutoGrupo)
        {

            IQueryable<Produto> listaProdutos = Averbacoes.ListaProduto().OrderBy(x => x.Nome);

            if (idConsignataria.Equals(0) && idProdutoGrupo.Equals(0)) return listaProdutos;

            if (!idConsignataria.Equals(0)) listaProdutos = listaProdutos.Where(x => x.IDConsignataria.Equals(idConsignataria));
            if (!idProdutoGrupo.Equals(0)) listaProdutos = listaProdutos.Where(x => x.IDProdutoGrupo.Equals(idProdutoGrupo));

            return listaProdutos;

        }

        public static IQueryable<FuncionarioCategoria> ListaFuncionarioCategoria()
        {
            return Funcionarios.ListarFuncionarioCategoria().OrderBy(x => x.Nome);
        }

        public static IQueryable<string> ListaFuncionarioRegime()
        {
            return Funcionarios.ListarFuncionarioRegime();
        }

        public static IQueryable<FuncionarioSituacao> ListaFuncionarioSituacao()
        {
            return Funcionarios.ListarFuncionarioSituacao().OrderBy(x => x.Nome);
        }

        public static IEnumerable<string> ListaFuncionarioSituacaoFolha()
        {
            return Funcionarios.ListaSituacoes();
        }

        public static IQueryable<EmpresaSolicitacaoTipo> ObtemSolicitacoesTipo(int idEmpresa)
        {
            return Solicitacoes.ObtemSolicitacoesPendentes(idEmpresa).Select(x => x.EmpresaSolicitacaoTipo).Distinct();
        }

        public static IQueryable<EmpresaSolicitacaoTipo> ListaSolicitacaoTipo()
        {
            return Solicitacoes.ListaSolicitacaoTipo();
        }

        public static List<string> ListaLocais()
        {
            return Funcionarios.ListaLocais();
        }

        public static List<string> ListaCargos()
        {
            return Funcionarios.ListaCargos();
        }

        public static List<string> ListaSetores()
        {
            return Funcionarios.ListaSetores();
        }


        public static void Liquidar(int idaverbacao, string motivo, int idresponsavel)
        {
            Averbacoes.Liquidar(idaverbacao, motivo, idresponsavel);
        }

        public static Boolean ExisteSolicitacaoPendente(int idaverbacao, int idempresa, int idsolicitacaotipo)
        {
            EmpresaSolicitacao s = Solicitacoes.ObtemUltimaSolicitacaoPendente(idaverbacao, idempresa, idsolicitacaotipo);
            return (s != null);
        }

        public static bool Cancelar(int idaverbacao, string motivo, int idresponsavel, out Enums.CancelamentoIndevido tipo)
        {
            return Averbacoes.Cancelar(idaverbacao, motivo, idresponsavel, out tipo);
        }

        public static void AprovarDesaprovar(int IDAverbacao, int IDEmpresa, bool aprovar, int IDResponsavel)
        {
            if (aprovar)
                Averbacoes.Aprovar(IDAverbacao, IDEmpresa, IDResponsavel);
            else
                Averbacoes.Desaprovar(IDAverbacao, IDEmpresa, IDResponsavel);
        }

        public static void AprovarDesaprovar(List<object> listaselecionados, int IDEmpresa, bool aprovar, int IDResponsavel)
        {
            Averbacoes.AprovarDesaprovar(listaselecionados, IDEmpresa, aprovar, IDResponsavel);
        }

        public static void SuspenderBloquear(int idAverbacao, bool suspender, string motivo, int idempresa)
        {
            if (suspender)
                Averbacoes.Suspender(idAverbacao, motivo, idempresa);
            else
                Averbacoes.Bloquear(idAverbacao, motivo, idempresa);
        }

        public static void AtivarAverbacaoSuspenso(int idAverbacao, string motivo, int idempresa)
        {
            Averbacoes.Ativar(idAverbacao, motivo, idempresa);
        }


        public static Averbacao ObtemAverbacao(int id)
        {
            return Averbacoes.ObtemAverbacao(id);
        }

        public static int ObtemIdRecursoPorNomeModulo(string nome, int idModulo)
        {
            return Recursos.ObtemIdRecursoPorNomeModulo(nome, idModulo);
        }

        public static bool ContratoEmProcessoDeQuitacaoOuCompra(int idAverbacao)
        {
            return Solicitacoes.ContratoEmProcessoDeQuitacaoOuCompra(idAverbacao);
        }

    }

}