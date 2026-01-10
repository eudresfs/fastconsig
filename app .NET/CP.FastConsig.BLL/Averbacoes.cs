using System.Collections.Generic;
using System;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Transactions;
using CP.FastConsig.Util;
using Microsoft.Data.Extensions;

namespace CP.FastConsig.BLL
{
    public static class Averbacoes
    {
        public static IQueryable<Averbacao> ListaAverbacao()
        {
            return new Repositorio<Averbacao>().Listar();
        }

        public static IQueryable<Averbacao> ListarAverbacaoSimples(string pesq)
        {
            return new Repositorio<Averbacao>().Listar().Where(x => x.Funcionario.Pessoa.Nome.Contains(pesq));
        }

        public static IQueryable<Averbacao> ObtemAverbacaosPesquisa(string prefixText)
        {
            return new Repositorio<Averbacao>().PesquisaTextual(prefixText, "");
        }

        public static List<Averbacao> ObtemAverbacaoVinculos(int id)
        {
            return new Repositorio<Averbacao>().ObterPorId(id).AverbacaoVinculo1.Select(x => x.Averbacao).ToList();
        }

        public static Averbacao ObtemAverbacao(int id)
        {
            return new Repositorio<Averbacao>().ObterPorId(id);
        }

        public static IQueryable<AverbacaoSituacao> ListaAverbacaoSituacao()
        {
            return new Repositorio<AverbacaoSituacao>().Listar();
        }

        public static IQueryable<AverbacaoTipo> ListaAverbacaoTipo()
        {
            return new Repositorio<AverbacaoTipo>().Listar();
        }

        public static string ObtemDescricaoAverbacaoTipo(int averbacaoTipo)
        {
            return new Repositorio<AverbacaoTipo>().Listar().Where(x => x.IDAverbacaoTipo == averbacaoTipo).FirstOrDefault().Nome;
        }

        public static IQueryable<AverbacaoSituacao> ListaAverbacaoSituacaoDeCompra()
        {
            return new Repositorio<AverbacaoSituacao>().Listar().Where(x => x.Compra == true);
        }

        public static IQueryable<ProdutoGrupo> ListaProdutoGrupo()
        {
            return new Repositorio<ProdutoGrupo>().Listar();
        }

        public static IQueryable<Produto> ListaProduto()
        {
            return new Repositorio<Produto>().Listar();
        }

        public static IQueryable<Averbacao> ListarAverbacaoTextual(string texto)
        {
            Repositorio<Funcionario> func = new Repositorio<Funcionario>();

            IQueryable<Pessoa> dadospessoas = Funcionarios.PesquisarPessoas(texto);

            IQueryable<Funcionario> funcDePessoas = from p in dadospessoas
                                                    from f in p.Funcionario
                                                    select f;

            var dados = func.PesquisaTextual(texto, "");
            dados = dados.Concat(funcDePessoas);
            //dados = dados.Union(funcDePessoas);

            Repositorio<Averbacao> repcon = new Repositorio<Averbacao>();

            var Averbacaos = repcon.PesquisaTextual(texto, "");

            IQueryable<Averbacao> AverbacaosDosFunc = from d in dados
                                                      from c in d.Averbacao
                                                      select c;

            //Averbacaos = Averbacaos.Concat(AverbacaosDosFunc);
            Averbacaos = Averbacaos.Union(AverbacaosDosFunc);

            return AverbacaosDosFunc;
        }

        public static IQueryable<ConciliacaoTipo> ListaConciliacaoTipo()
        {
            return new Repositorio<ConciliacaoTipo>().Listar();
        }

        public static IQueryable<Conciliacao> ListarConciliacao(string anomes, int idconsignataria, int idsituacao)
        {
            var dados = new Repositorio<Conciliacao>().Listar().Where(x => x.Competencia == anomes);
            if (idconsignataria > 0)
                dados = dados.Where(x => x.Produto.IDConsignataria == idconsignataria);
            if (idsituacao > 0)
                dados = dados.Where(x => x.ConciliacaoTipo.IDConciliacaoTipo == idsituacao);
            return dados;
        }

        public static void Aprovar(int IDAverbacao, int IDEmpresa, int IDResponsavel)
        {
            EmpresaSolicitacao es = Solicitacoes.ObtemUltimaSolicitacaoPendente(IDAverbacao, IDEmpresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);
            if (es != null)
                Solicitacoes.AtualizaSolicitacao(es.IDEmpresaSolicitacao, (int)Enums.SolicitacaoSituacao.Processada, IDResponsavel, es.Motivo);

            ProcessarAprovacao(ObtemAverbacao(IDAverbacao));
        }

        public static void Desaprovar(int IDAverbacao, int IDEmpresa, int IDResponsavel)
        {
            EmpresaSolicitacao es = Solicitacoes.ObtemUltimaSolicitacaoPendente(IDAverbacao, IDEmpresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);
            if (es != null)
                Solicitacoes.AtualizaSolicitacao(es.IDEmpresaSolicitacao, (int)Enums.SolicitacaoSituacao.Rejeitada, IDResponsavel, es.Motivo);

            Tramitar(IDAverbacao, (int)Enums.AverbacaoSituacao.Desaprovado, IDEmpresa, "");
        }

        public static void AprovarDesaprovar(List<object> listaselecionados, int IDEmpresa, bool aprovar, int IDResponsavel)
        {
            foreach (var item in listaselecionados)
            {
                if (aprovar)
                {
                    Aprovar(Convert.ToInt32(item), IDEmpresa, IDResponsavel);
                }
                else
                {
                    Desaprovar(Convert.ToInt32(item), IDEmpresa, IDResponsavel);
                }
            }
        }

        public static void Tramitar(int IDAverbacao, int IDAverbacaoSituacao, int IDEmpresa, string obs)
        {
            Repositorio<AverbacaoTramitacao> rep = new Repositorio<AverbacaoTramitacao>();

            AverbacaoTramitacao tram = new AverbacaoTramitacao();
            tram.IDAverbacao = IDAverbacao;
            tram.IDAverbacaoSituacao = IDAverbacaoSituacao;
            if (IDEmpresa > 0)
                tram.IDEmpresa = IDEmpresa;
            tram.OBS = obs;
            tram.CreatedOn = DateTime.Now;

            rep.Incluir(tram);

            //SolicitacaoAtualizar(IDAverbacao, (int)Enums.SolicitacaoSituacao.Processada);
        }

        private static bool ValidaAutorizacaoEspecial(int idautorizacaotipo, Funcionario func)
        {
            return func.FuncionarioAutorizacao.Any(x => x.IDFuncionarioAutorizacaoTipo == idautorizacaotipo && x.AutorizacaoData.AddDays(x.AutorizacaoValidade) >= DateTime.Today);
        }

        public static bool FuncionarioBloqueado(int idFuncionario)
        {
            Funcionario func = Funcionarios.ObtemFuncionario(idFuncionario);

            if (ValidaAutorizacaoEspecial((int)Enums.FuncionarioAutorizacaoTipo.IndependentedeBloqueio, func)) return true;

            return func.FuncionarioBloqueio.Any(x => x.TipoBloqueio.Equals("0"));
        }

        public static int SolicitacaoAtualizar(int idAverbacao, int idEmpresa, int idSolicitacaoTipo, int idResponsavel, bool rejeita = false, string obs = "")
        {
            EmpresaSolicitacao es = Solicitacoes.ObtemUltimaSolicitacaoPendente(idAverbacao, idEmpresa, idSolicitacaoTipo);

            if (es != null)
            {
                Solicitacoes.AtualizaSolicitacao(es.IDEmpresaSolicitacao, rejeita ? (int)Enums.SolicitacaoSituacao.Rejeitada : (int)Enums.SolicitacaoSituacao.Processada, idResponsavel, obs);

                ProcessarFluxoCompra(ObtemAverbacao(idAverbacao));
            }

            return es == null ? 0 : es.IDEmpresaSolicitacao;
        }

        public static void Suspender(int idAverbacao, string motivo, int idempresa)
        {
            Tramitar(idAverbacao, (int)Enums.AverbacaoSituacao.Suspenso_MargemLivre, idempresa, motivo);
        }

        public static void Bloquear(int idAverbacao, string motivo, int idempresa)
        {
            Tramitar(idAverbacao, (int)Enums.AverbacaoSituacao.Bloqueado_MargemRetida, idempresa, motivo);
        }

        public static void Ativar(int idAverbacao, string motivo, int idempresa)
        {
            Tramitar(idAverbacao, (int)Enums.AverbacaoSituacao.Ativo, idempresa, motivo);
        }

        public static List<ConciliacaoResumoFolha> ListarConciliacaoResumoFolha(string competencia, int idempresa)
        {
            List<ConciliacaoResumoFolha> lista = new List<ConciliacaoResumoFolha>();

            ConciliacaoResumoFolha previsao = new ConciliacaoResumoFolha();
            previsao.Descricao = "Enviado para Descontar na Folha";
            previsao.Valor = Averbacoes.ObtemTotalPrevisao(competencia, idempresa);
            previsao.Diferenca = "--------";
            lista.Add(previsao);

            ConciliacaoResumoFolha retorno = new ConciliacaoResumoFolha();
            retorno.Descricao = "Retornado pela Folha";
            retorno.Valor = Averbacoes.ObtemTotalRetorno(competencia, idempresa);
            retorno.Diferenca = String.Format("{0:C2}", retorno.Valor - previsao.Valor);
            lista.Add(retorno);

            ConciliacaoResumoFolha repassado = new ConciliacaoResumoFolha();
            repassado.Descricao = "Previsão de Repasse";
            repassado.Valor = Averbacoes.ObtemTotalRepassado(competencia, idempresa);
            repassado.Diferenca = String.Format("{0:C2}", retorno.Valor - repassado.Valor);
            lista.Add(repassado);

            ConciliacaoResumoFolha conciliado = new ConciliacaoResumoFolha();
            conciliado.Descricao = "Conciliado";
            conciliado.Valor = Averbacoes.ObtemTotalConciliado(competencia, idempresa);
            conciliado.Diferenca = String.Format("{0:C2}", retorno.Valor - conciliado.Valor);
            lista.Add(conciliado);

            return lista;
        }

        private static decimal? ObtemTotalConciliado(string competencia, int idempresa)
        {
            var dados = new Repositorio<Conciliacao>().Listar().Where(x => x.Competencia == competencia && (idempresa == 0 || (idempresa > 0 && x.Produto.IDConsignataria == idempresa)) && x.ConciliacaoTipo.IDConciliacaoGrupo == 1);
            if (dados.Count() > 0)
                return dados.Sum(x => x.Valor);
            else
                return 0;
        }

        private static decimal? ObtemTotalRepassado(string competencia, int idempresa)
        {
            var dados = ListarConciliacaoRepasses(competencia, idempresa);
            if (dados.Count() > 0)
                return dados.Sum(x => x.Valor);
            else
                return 0;
        }

        private static decimal? ObtemTotalRetorno(string competencia, int idempresa)
        {
            var rep_cr = new Repositorio<ConciliacaoRetorno>();
            var dados_cr = rep_cr.Listar();

            DbContext ctx = rep_cr.contexto;

            var rep_p = new Repositorio<Produto>(ctx);
            var dados_p = rep_p.Listar();

            var consulta = from d in dados_cr
                           from s in dados_p
                           where d.Verba == s.VerbaFolha && s.Ativo == 1 &&
                           d.Competencia == competencia && (idempresa == 0 || (idempresa > 0 && s.IDConsignataria == idempresa))
                           select d.ValorDescontado;

            if (consulta.Count() > 0)
                return consulta.Sum();
            else
                return 0;
        }

        private static decimal? ObtemTotalPrevisao(string competencia, int idempresa)
        {
            var dados = new Repositorio<ConciliacaoMovimento>().Listar().Where(x => x.Competencia == competencia && (idempresa == 0 || (idempresa > 0 && x.IDConsignataria == idempresa)));
            if (dados.Count() > 0)
                return dados.Sum(x => x.ValorMovimento);
            else
                return 0;
        }

        public static List<ConciliacaoResumoConciliacao> ListarConciliacaoResumoConciliacao(string competencia, int idempresa)
        {
            var dados = new Repositorio<Conciliacao>().Listar();

            var consulta = from c in dados
                           where c.Competencia == competencia && (idempresa == 0 || (c.IDConsignataria == idempresa))
                           group c by c.ConciliacaoTipo into g
                           select new ConciliacaoResumoConciliacao() { Descricao = g.Key.Nome, Valor = g.Sum(x => x.Valor), Descontado = g.Sum(x => x.ValorDescontado) };

            return consulta.ToList();
        }

        public static IQueryable<ConciliacaoRepasse> ListarConciliacaoRepasses(string competencia, int idempresa)
        {
            return new Repositorio<ConciliacaoRepasse>().Listar().Where(x => x.Competencia == competencia && (idempresa == 0 || (idempresa > 0 && x.IDEmpresa == idempresa)));
        }

        public static IQueryable<ConciliacaoResultadoConciliacao> ListarConciliacaoAnalise(string competencia, int idempresa, int meses)
        {
            string primeiracompetencia = Utilidades.CompetenciaDiminui(competencia, meses);
            var conciliacao = new Repositorio<Conciliacao>().Listar();

            var dados = from c in conciliacao
                        where c.Competencia.CompareTo(primeiracompetencia) >= 0 && c.Competencia.CompareTo(competencia) <= 0 && (idempresa == 0 || (c.IDConsignataria == idempresa))
                        orderby c.Competencia ascending
                        group c by new { c.Competencia, Consignataria = c.Empresa.Fantasia, c.ConciliacaoTipo.ConciliacaoGrupo.Nome } into g
                        select new ConciliacaoResultadoConciliacao { Descricao = g.Key.Nome, Competencia = g.Key.Competencia, NomeConsignataria = g.Key.Consignataria, Valor = g.Sum(x => x.ValorDescontado > 0 ? x.ValorDescontado : x.Valor) };

            return dados;
        }

        public static IQueryable<AverbacaoParcela> ListarAnaliseAverbacoes(string anomesInicio, string anomesFim, int idempresa)
        {
            anomesInicio = anomesInicio.Substring(3, 4) + "/" + anomesInicio.Substring(0, 2);
            anomesFim = anomesFim.Substring(3, 4) + "/" + anomesFim.Substring(0, 2);

            Repositorio<AverbacaoParcela> rep = new Repositorio<AverbacaoParcela>();

            var parcelas = rep.Listar();

            var dados = from p in parcelas
                        where p.Competencia.CompareTo(anomesInicio) >= 0 && p.Competencia.CompareTo(anomesFim) <= 0
                        && (idempresa == 0 || (p.Averbacao.IDConsignataria == idempresa))
                        orderby p.Competencia descending
                        select p;

            return dados;
        }

        public static IQueryable<Funcionario> LocalizarFuncPorMatriculaOuCpf(string texto)
        {
            Repositorio<Funcionario> func = new Repositorio<Funcionario>();

            IQueryable<Funcionario> funcs = func.Listar().Where(x => x.Matricula == texto);

            if (funcs.Count() == 0)
            {
                Repositorio<Pessoa> pessoa = new Repositorio<Pessoa>();
                var pessoas = pessoa.Listar().Where(x => x.CPF == texto);
                IQueryable<Funcionario> funcDePessoas = from p in pessoas
                                                        from f in p.Funcionario
                                                        select f;
                funcs = funcDePessoas;
            }

            return funcs;
        }

        public static IQueryable<Funcionario> LocalizarFunc(string texto)
        {
            Repositorio<Funcionario> func = new Repositorio<Funcionario>();

            IQueryable<Pessoa> dadospessoas = Funcionarios.PesquisarPessoas(texto);

            IQueryable<Funcionario> funcDePessoas = from p in dadospessoas
                                                    from f in p.Funcionario
                                                    select f;

            var dados = func.PesquisaTextual(texto, "");

            dados = dados.Concat(funcDePessoas);

            return dados;
        }

        public static int SalvarAverbacao(Averbacao dado, Pessoa pessoa, int idprodutogrupo, bool bAprovacaoFuncionario, List<int> listaRefinancia, List<int> listaCompra)
        {
            int idaverbacao = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Atualizar dados pessoais
                Repositorio<Pessoa> reppessoa = new Repositorio<Pessoa>();
                Pessoa p = reppessoa.ObterPorId(pessoa.IDPessoa);
                p.Endereco = pessoa.Endereco;
                p.Bairro = pessoa.Bairro;
                p.Cidade = pessoa.Cidade;
                p.Estado = pessoa.Estado;
                p.CEP = pessoa.CEP;
                p.Fone = pessoa.Fone;
                p.Celular = pessoa.Celular;
                p.Email = pessoa.Email;
                p.DataNascimento = pessoa.DataNascimento;

                reppessoa.Alterar(p);

                // Gerar Numero
                Repositorio<ProxNumero> repprox = new Repositorio<ProxNumero>();
                int ID = repprox.Incluir(new ProxNumero());

                // calcula valor refinanciado
                decimal valorrefin = 0;

                for (int i = 0; i < listaRefinancia.Count; i++)
                {
                    valorrefin += CalculaSaldoRestante(listaRefinancia[i]);
                }

                // salvar vinculos da compra
                for (int i = 0; i < listaCompra.Count; i++)
                {
                    valorrefin += CalculaSaldoRestante(listaCompra[i]);
                }

                // salvar averbacao
                Repositorio<Averbacao> averb = new Repositorio<Averbacao>();
                dado.Numero = ID.ToString().PadLeft(8, '0');
                dado.ValorRefinanciado = valorrefin;
                idaverbacao = averb.Incluir(dado);

                //dado = ObtemAverbacao(idaverbacao);

                // salvar vinculos do refinancimento
                for (int i = 0; i < listaRefinancia.Count; i++)
                {
                    IncluiAverbacaoVinculo(dado.IDAverbacao, listaRefinancia[i]);
                }

                // salvar vinculos da compra
                for (int i = 0; i < listaCompra.Count; i++)
                {
                    IncluiAverbacaoVinculo(dado.IDAverbacao, listaCompra[i]);
                }

                if (bAprovacaoFuncionario)
                {
                    Solicitacoes.AdicionaSolicitacao(dado.IDConsignataria, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Processada, null, idaverbacao, dado.IDFuncionario, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "Aprovação realizada no mesmo momento do registro da averbação.");
                }

                // tramitações de fluxo e solicitações
                bool bGerarParcelas = ProcessarTramitacoes(ObtemAverbacao(idaverbacao), idprodutogrupo);

                //Funcionario func = Funcionarios.ObtemFuncionario(dado.IDFuncionario);

                //if (func.MargemDisponivel(idprodutogrupo) < 0)
                //{
                //    idaverbacao = 0;
                //    scope.Dispose();
                //}
                //else
                //{
                //    scope.Complete();
                //}
                scope.Complete();
            }
            return idaverbacao;
        }

        public static decimal CalculaSaldoRestante(int idaverbacao)
        {
            Averbacao a = ObtemAverbacao(idaverbacao);
            string anomes = ObtemAnoMesCorte((int)Enums.Modulos.Consignataria, a.IDConsignataria);

            int prazorestante = 0;
            if (a.AverbacaoParcela.Count() > 0)
                prazorestante = a.AverbacaoParcela.Count(x => x.Competencia.CompareTo(anomes) >= 0);
            else
                prazorestante = a.Prazo.HasValue ? a.Prazo.Value : 0;

            return prazorestante * a.ValorParcela;
        }

        public static int CalculaPrazoRestante(int idaverbacao)
        {
            Averbacao a = ObtemAverbacao(idaverbacao);
            string anomes = ObtemAnoMesCorte((int)Enums.Modulos.Consignataria, a.IDConsignataria);

            int prazorestante = 0;
            if (a.AverbacaoParcela.Count() > 0)
                prazorestante = a.AverbacaoParcela.Count(x => x.Competencia.CompareTo(anomes) >= 0);
            else
                prazorestante = a.Prazo.HasValue ? a.Prazo.Value : 0;

            return prazorestante;
        }

        public static void SalvarCompra(Averbacao dado, Pessoa pessoa, int idprodutogrupo, List<int> listaRefinancia, List<int> listaCompra)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Atualizar dados pessoais
                Repositorio<Pessoa> reppessoa = new Repositorio<Pessoa>();
                Pessoa p = reppessoa.ObterPorId(pessoa.IDPessoa);
                p.Endereco = pessoa.Endereco;
                p.Bairro = pessoa.Bairro;
                p.Cidade = pessoa.Cidade;
                p.Estado = pessoa.Estado;
                p.CEP = pessoa.CEP;
                p.Fone = pessoa.Fone;
                p.Celular = pessoa.Celular;
                p.Email = pessoa.Email;
                p.DataNascimento = pessoa.DataNascimento;

                reppessoa.Alterar(p);

                // salvar averbacao
                Repositorio<Averbacao> averb = new Repositorio<Averbacao>();
                dado.ValorDeducaoMargem = dado.ValorParcela;
                averb.Alterar(dado);

                Averbacoes.SolicitacaoAtualizar(dado.IDAverbacao, dado.IDConsignataria, (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida, dado.IDUsuario);

                scope.Complete();
            }
        }

        private static void GerarParcelas(Averbacao dado)
        {
            if (dado.AverbacaoParcela.Count > 0)
                return;

            int prazo = dado.Prazo.Value;
            string mes = dado.CompetenciaInicial;
            Repositorio<AverbacaoParcela> repparcela = new Repositorio<AverbacaoParcela>();

            for (int i = 1; i <= prazo; i++)
            {
                AverbacaoParcela parcela = new AverbacaoParcela();
                parcela.IDAverbacao = dado.IDAverbacao;
                parcela.IDAverbacaoParcelaSituacao = (int)Enums.AverbacaoParcelaSituacao.Aberta;
                parcela.Numero = i;
                parcela.Valor = dado.ValorParcela;
                parcela.Competencia = mes;

                repparcela.Incluir(parcela);
                mes = Utilidades.CompetenciaAumenta(mes, 1);
            }
        }

        private static void IncluiAverbacaoVinculo(int idaverbacaopai, int idaverbacao)
        {
            Repositorio<AverbacaoVinculo> repv = new Repositorio<AverbacaoVinculo>();

            AverbacaoVinculo av = new AverbacaoVinculo();
            av.IDAverbacaoPai = idaverbacaopai;
            av.IDAverbacao = idaverbacao;

            repv.Incluir(av);
        }

        private static bool ProcessarAprovacao(Averbacao dado)
        {
            int idprodutogrupo = dado.Produto.IDProdutoGrupo;
            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;

            // verificacao fluxos
            bool existefluxo = false;

            // verificação se requer aprovacao da consignataria
            if (RequerAprovacao((int)Enums.Modulos.Consignataria, idprodutogrupo, idempresa) && Solicitacoes.ObtemUltimaSolicitacaoProcessadaEmpresa(idaverbacao, idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes) == null && Solicitacoes.ObtemUltimaSolicitacaoProcessadaFuncionario(idaverbacao, dado.IDFuncionario, (int)Enums.SolicitacaoTipo.AprovarAverbacoes) != null)
            {
                int idrecurso = Geral.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlAverbacao, DadosSessao.IdModulo);

                if (!Permissoes.CheckPermissao(idrecurso, DadosSessao.IdBanco, DadosSessao.IdPerfil, 43))
                {
                    Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "");
                    existefluxo = true;
                }
                else
                {
                    Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Processada, idempresa, idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "Gerado automático.");
                }
            }

            // verificação se requer aprovacao da consignante
            if (!existefluxo && RequerAprovacao((int)Enums.Modulos.Consignante, idprodutogrupo, Convert.ToInt32(Geral.IdEmpresaConsignante())) && Solicitacoes.ObtemUltimaSolicitacaoProcessadaEmpresa(idaverbacao, Convert.ToInt32(Geral.IdEmpresaConsignante()), (int)Enums.SolicitacaoTipo.AprovarAverbacoes) == null)
            {
                Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Pendente, Convert.ToInt32(Geral.IdEmpresaConsignante()), idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "");
                existefluxo = true;
            }

            //if (!existefluxo && dado.IDAverbacaoTipo != (int)Enums.AverbacaoTipo.Normal && idprodutogrupo == (int)Enums.ProdutoGrupo.Emprestimos)
            //    ProcessarFluxoCompra(dado);
            //else
            ProcessarFluxoSituacao(dado, existefluxo);

            return false;
        }

        public static bool RequerAprovacao(int idmodulo, int idprodutogrupo, int idempresa)
        {
            FluxoAprovacao fa = FluxoAprovacoes.ObtemFluxoAprovacao(idprodutogrupo);
            FluxoAprovacaoEmpresa fae = FluxoAprovacoes.ObtemFluxoAprovacaoEmpresa(idprodutogrupo, idempresa);

            if (idmodulo == (int)Enums.Modulos.Consignataria)
                return ((fa != null && fa.RequerAprovacaoConsignataria) || (fae != null && fae.RequerAprovacao));
            else if (idmodulo == (int)Enums.Modulos.Consignante)
                return (fa != null && fa.RequerAprovacaoConsignante);
            else if (idmodulo == (int)Enums.Modulos.Funcionario)
                return fa != null && fa.RequerAprovacaoFuncionario;
            else
                return false;
        }

        public static bool ProcessarFluxoCompra(Averbacao dado)
        {
            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;

            EmpresaSolicitacao es = Solicitacoes.ObtemUltimaSolicitacaoProcessada(idaverbacao);
            int idresponsavel = DadosSessao.IdUsuario;
            if (es != null)
                idresponsavel = es.IDResponsavel.HasValue ? es.IDResponsavel.Value : DadosSessao.IdUsuario;

            Averbacao pai = dado.AverbacaoVinculo.OrderByDescending(x => x.IDAverbacaoVinculo).Select(x => x.Averbacao1).FirstOrDefault();

            if (pai != null)
            {
                if (pai.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.EmProcessodeCompra)
                    Tramitar(pai.IDAverbacao, (int)Enums.AverbacaoSituacao.EmProcessodeCompra, idempresa, "");
            }

            if (es != null)
            {
                if (es.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos)
                {
                    if (pai != null)
                        Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.InformarQuitacao, (int)Enums.SolicitacaoSituacao.Pendente, pai.IDConsignataria, idaverbacao, null, idresponsavel, null, "", "");
                }
                else if (es.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarQuitacao || es.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada)
                {
                    Solicitacoes.AdicionaSolicitacao(pai.IDConsignataria, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, idaverbacao, null, idresponsavel, null, "", "");
                }
                else if (es.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao)
                {
                    if (pai != null)
                    {
                        if (es.IDEmpresaSolicitacaoSituacao == (int)Enums.SolicitacaoSituacao.Processada)
                        {
                            if (!ExistemAverbacoesParaQuitar(pai, idaverbacao))
                                Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida, (int)Enums.SolicitacaoSituacao.Pendente, pai.IDConsignataria, pai.IDAverbacao, null, idresponsavel, null, "", "");
                            Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Comprado, idempresa, es.Motivo);
                        }
                        else
                        {
                            Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada, (int)Enums.SolicitacaoSituacao.Pendente, pai.IDConsignataria, idaverbacao, null, idresponsavel, null, es.Motivo, "");
                        }
                    }
                }
                else if (es.IDEmpresaSolicitacaoTipo == (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida)
                {
                    var vinc_para_renegociacao = dado.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria == idempresa);
                    foreach (var item in vinc_para_renegociacao)
                    {
                        if (item.Averbacao1.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.Liquidado)
                        {
                            Tramitar(item.IDAverbacao, (int)Enums.AverbacaoSituacao.Liquidado, idempresa, "Liquidado por Conclusão de Compra de Dívida");
                            ParcelasAlterarSituacao(item.IDAverbacao, (int)Enums.AverbacaoParcelaSituacao.LiquidadaManual);
                        }
                    }

                    var vinc_para_compra = dado.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria != dado.IDConsignataria);

                    foreach (var item in vinc_para_compra)
                    {
                        if (item.Averbacao1.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.Liquidado)
                        {
                            Tramitar(item.IDAverbacao, (int)Enums.AverbacaoSituacao.Liquidado, idempresa, "Liquidado por Conclusão de Compra de Dívida");
                            ParcelasAlterarSituacao(item.IDAverbacao, (int)Enums.AverbacaoParcelaSituacao.LiquidadaManual);
                        }
                    }

                    // Atualizar valor da dedução da margem para o valor da parcela real do contrato após sua conclusão
                    Repositorio<Averbacao> ra = new Repositorio<Averbacao>();
                    Averbacao a = ra.ObterPorId(idaverbacao);
                    a.ValorDeducaoMargem = a.ValorParcela;
                    ra.Alterar(a);

                    Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Averbado, idempresa, "Conclusão de Compra de Dívida");

                    GerarParcelas(dado);
                }
            }
            return true;
        }

        private static bool ExistemAverbacoesParaQuitar(Averbacao pai, int idaverbacao)
        {
            var vinc_para_compra = pai.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria != pai.IDConsignataria && x.IDAverbacao != idaverbacao);

            bool bParaQuitar = false;
            foreach (var item in vinc_para_compra)
            {
                if (item.Averbacao.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo)
                    bParaQuitar = true;
            }
            return bParaQuitar;
        }

        private static bool ProcessarTramitacoes(Averbacao dado, int idprodutogrupo)
        {
            bool bexistefluxopendente = ProcessarFluxoAprovacao(dado, idprodutogrupo);

            return ProcessarFluxoSituacao(dado, bexistefluxopendente);
        }

        private static bool ProcessarFluxoAprovacao(Averbacao dado, int idprodutogrupo)
        {
            if (dado.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.PreReserva)
                return true;

            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;

            // verificacao fluxos
            FluxoAprovacao fa = FluxoAprovacoes.ObtemFluxoAprovacao(idprodutogrupo);
            FluxoAprovacaoEmpresa fae = FluxoAprovacoes.ObtemFluxoAprovacaoEmpresa(idprodutogrupo, idempresa);

            bool terminoufluxo = false;
            // verificação se requer aprovacao do funcionario
            if (fa != null && fa.RequerAprovacaoFuncionario)
            {
                // Verifica se já foi atendida solicitação de aprovação
                EmpresaSolicitacao solic = Solicitacoes.ObtemUltimaSolicitacaoProcessadaFuncionario(idaverbacao, dado.IDFuncionario, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);

                if (solic == null)
                {
                    Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Pendente, null, idaverbacao, dado.IDFuncionario, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "");
                    terminoufluxo = true;
                }
            }

            // verificação se requer aprovacao da consignataria
            if (!terminoufluxo && ((fa != null && fa.RequerAprovacaoConsignataria) || (fae != null && fae.RequerAprovacao)))
            {
                // Verifica se já foi atendida solicitação de aprovação
                EmpresaSolicitacao solic = Solicitacoes.ObtemUltimaSolicitacaoProcessadaEmpresa(idaverbacao, dado.IDConsignataria, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);

                int idrecurso = Geral.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlAverbacao, DadosSessao.IdModulo);

                if (!Permissoes.CheckPermissao(idrecurso, DadosSessao.IdBanco, DadosSessao.IdPerfil, 43))
                {
                    Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "");
                    terminoufluxo = true;
                }
                else
                {
                    Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Processada, idempresa, idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "Gerado automático.");
                }
            }

            // verificação se requer aprovacao da consignante
            if (!terminoufluxo && fa != null && fa.RequerAprovacaoConsignante)
            {
                Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.AprovarAverbacoes, (int)Enums.SolicitacaoSituacao.Pendente, Convert.ToInt32(Geral.IdEmpresaConsignante()), idaverbacao, null, dado.IDUsuario, null, "Número de Contrato: " + dado.Numero, "");
                terminoufluxo = true;
            }
            return terminoufluxo;
        }

        private static bool ProcessarFluxoSituacao(Averbacao dado, bool bexistefluxopendente)
        {
            if (dado.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.PreReserva)
                return true;
            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;
            bool bGerarParcelas = false;

            if (bexistefluxopendente)
            {
                if (dado.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.AguardandoAprovacao)
                    Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.AguardandoAprovacao, idempresa, "");
            }

            if (!bexistefluxopendente)
            {
                if (dado.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.Normal)
                {
                    Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Averbado, idempresa, "");
                    if (dado.Produto.ProdutoGrupo.IDProdutoTipo != (int)Enums.ProdutoTipo.PrazoIndeterminado_ParcelaFixa)
                        bGerarParcelas = true;
                }
                else if (dado.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.Renegociacao)
                {
                    ProcessaRenegociacao(dado);
                    bGerarParcelas = true;
                }
                else if (dado.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.Compra)
                {
                    bGerarParcelas = ProcessaCompraInicio(dado);
                }
                else if (dado.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.CompraERenegociacao)
                {
                    bGerarParcelas = ProcessaCompraInicio(dado);
                }
            }
            if (bGerarParcelas)
                GerarParcelas(dado);

            return bGerarParcelas;
        }

        private static void ProcessaRenegociacao(Averbacao dado)
        {
            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;

            Repositorio<Averbacao> rep_a = new Repositorio<Averbacao>();
            Averbacao a = rep_a.ObterPorId(idaverbacao);
            a.ValorDeducaoMargem = a.ValorParcela;
            rep_a.Alterar(a);

            Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Averbado, idempresa, "");

            var vinc_para_renegociacao = dado.AverbacaoVinculo1.Where(x => x.Averbacao1.IDConsignataria == idempresa);
            foreach (var item in vinc_para_renegociacao)
            {
                Tramitar(item.IDAverbacao, (int)Enums.AverbacaoSituacao.Liquidado, idempresa, "");
                ParcelasAlterarSituacao(item.IDAverbacao, (int)Enums.AverbacaoParcelaSituacao.LiquidadaManual);
            }
        }

        private static bool ProcessaCompraInicio(Averbacao dado)
        {
            int idempresa = dado.IDConsignataria;
            int idaverbacao = dado.IDAverbacao;

            if (dado.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.EmProcessodeCompra)
                Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.EmProcessodeCompra, idempresa, "");

            //var vinc_para_renegociacao = dado.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria == idempresa);
            //foreach (var item in vinc_para_renegociacao)
            //{
            //    if (item.Averbacao1.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.Liquidado)
            //        Tramitar(item.IDAverbacao, (int)Enums.AverbacaoSituacao.Liquidado, idempresa, "");
            //}

            var vinc_para_compra = dado.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria != idempresa);
            foreach (var item in vinc_para_compra)
            {
                Solicitacoes.AdicionaSolicitacao(idempresa, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, (int)Enums.SolicitacaoSituacao.Pendente, item.Averbacao.IDConsignataria, item.IDAverbacao, null, dado.IDUsuario, null, "", "");
            }
            return false;
        }

        public static string ObtemUltimaConciliacao(int idmodulo, int idempresa)
        {
            string competencia = "";
            
            if (idmodulo == 1)
            {
                var teste = new Repositorio<TmpGrupoBoasNoticias>().Listar().Where(x => x.Competencia != null && x != null).ToList().Max(x => x.Competencia).ToString();
                if (teste== null) 
                    teste = string.Empty;
                competencia = teste.ToString();
            }
            else
            {
                var teste = new Repositorio<TmpBoasNoticias>().Listar().Where( x => x.IDConsignataria == idempresa && x.Competencia != null && x != null ).ToList().Max( x => x.Competencia );
                if (teste== null) 
                    teste = string.Empty;
                competencia = teste.ToString();
            }

            if (string.IsNullOrEmpty(competencia))
            {
                competencia = ObtemAnoMesCorte(idmodulo, idempresa);
            }

            return competencia;
        }

        public static string ObtemAnoMesCorte(int idmodulo, int idempresa)
        {
            int somames = 0;
            int dia = DateTime.Today.Day;
            int ano = DateTime.Today.Year;
            int mes = DateTime.Today.Month;

            string anomes = ano.ToString() + "/" + mes.ToString().PadLeft(2, '0');

            int diacorte = Convert.ToInt32(new Repositorio<Parametro>().Listar().FirstOrDefault(x => x.Nome == "DiaCorte").Valor);

            CorteHistorico ch = new Repositorio<CorteHistorico>().Listar().FirstOrDefault(x => x.Competencia == anomes);
            if (ch != null)
                diacorte = Convert.ToInt32(ch.DiaCorte);

            if (idmodulo == (int)Enums.Modulos.Consignataria && idempresa > 0)
            {
                Empresa emp = Consignatarias.ObtemConsignataria(idempresa);
                if (emp.DiaCorte.HasValue && emp.DiaCorte.Value > 0 && diacorte < emp.DiaCorte.Value)
                {
                    diacorte = emp.DiaCorte.Value;
                    somames++;
                }
                else if (emp.DiaCorte.HasValue && emp.DiaCorte.Value > 0 && diacorte > emp.DiaCorte.Value)
                {
                    diacorte = emp.DiaCorte.Value;
                }

                if (diacorte == 30)
                    diacorte = DateTime.DaysInMonth(ano, mes);
            }

            if (dia > diacorte)
                return Utilidades.CompetenciaAumenta(anomes, somames + 1);
            else
                return Utilidades.CompetenciaAumenta(anomes, somames);
        }

        public static int ObtemParcelaAtual(int idAverbacao)
        {
            string competenciaAtual = ObtemAnoMesCorte(DadosSessao.IdModulo,DadosSessao.IdBanco); //string.Format("{0}/{1}", DateTime.Now.Year, DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString());
            return new Repositorio<AverbacaoParcela>().Listar().Where(
                x => x.IDAverbacao == idAverbacao && x.Competencia == competenciaAtual).Select(x => x.Numero).
                FirstOrDefault();
        }

        public static int ObtemParcelasRestantes(int idAverbacao)
        {
            int parcelaAtual = ObtemParcelaAtual(idAverbacao);
            string diaCorte =
                new Repositorio<Parametro>().Listar().Where(x => x.IDParametro == (int)Enums.Parametros.DiaCorte).
                    Select(x => x.Valor).FirstOrDefault();
            Averbacao averbacao = ObtemAverbacao(idAverbacao);
            if (DateTime.Now.Day > Convert.ToInt32(diaCorte)) return Convert.ToInt32(averbacao.Prazo - parcelaAtual);
            return Convert.ToInt32(averbacao.Prazo - (parcelaAtual - 1));
        }

        public static decimal obtemSaldoAberto(int idAverbacao)
        {
            Averbacao averbacao = ObtemAverbacao(idAverbacao);
            List<AverbacaoParcela> parcelas =
                new Repositorio<AverbacaoParcela>().Listar().Where(x => x.IDAverbacao == idAverbacao).ToList();
            string diaCorte =
                new Repositorio<Parametro>().Listar().Where(x => x.IDParametro == (int)Enums.Parametros.DiaCorte).
                    Select(x => x.Valor).FirstOrDefault();
            decimal valorTotalPago = 0;

            foreach (AverbacaoParcela parcela in parcelas)
            {
                string[] competencia = parcela.Competencia.Split('/');
                if (Convert.ToInt32(competencia[1]) > DateTime.Now.Month || (Convert.ToInt32(competencia[1]) == DateTime.Now.Month && DateTime.Now.Day <= Convert.ToInt32(diaCorte))) continue;

                decimal valorPago = parcela.ValorDescontado ?? parcela.Valor;
                valorTotalPago += valorPago;
            }

            return averbacao.ValorContratado - valorTotalPago;
        }

        public static IQueryable<Averbacao> AverbacoesParaComprar(IQueryable<Averbacao> listaAverbacoesDoFunc, int idempresa)
        {
            return listaAverbacoesDoFunc.Where(x => x.IDConsignataria != idempresa && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos && x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && !x.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao));
        }

        public static IQueryable<Averbacao> AverbacoesParaRefinanciar(IQueryable<Averbacao> listaAverbacoesDoFunc, int idempresa)
        {
            return listaAverbacoesDoFunc.Where(x => x.IDConsignataria == idempresa && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos && x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && !x.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao));
        }

        public static void SalvaInformacaoQuitacao(DateTime dataQuitacao, decimal valor, int idTipoFormaPagamento, string observacao, int idAverbacao, string comprovante)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                Averbacao a = ObtemAverbacao(idAverbacao);

                int idSolicitacao = Averbacoes.SolicitacaoAtualizar(idAverbacao, DadosSessao.IdBanco, (int)Enums.SolicitacaoTipo.InformarQuitacao, DadosSessao.IdUsuario);
                if (idSolicitacao == 0)
                {
                    idSolicitacao = Averbacoes.SolicitacaoAtualizar(idAverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, DadosSessao.IdUsuario);
                    idSolicitacao = Averbacoes.SolicitacaoAtualizar(idAverbacao, DadosSessao.IdBanco, (int)Enums.SolicitacaoTipo.InformarQuitacao, DadosSessao.IdUsuario);
                }
                EmpresaSolicitacaoQuitacao empresaSolicitacaoQuitacao = new EmpresaSolicitacaoQuitacao();

                empresaSolicitacaoQuitacao.DataQuitacao = dataQuitacao;
                empresaSolicitacaoQuitacao.Valor = valor;
                empresaSolicitacaoQuitacao.IDAverbacaoTipoQuitacao = idTipoFormaPagamento;
                empresaSolicitacaoQuitacao.IDEmpresaSolicitacao = idSolicitacao;
                empresaSolicitacaoQuitacao.IDAverbacao = idAverbacao;
                empresaSolicitacaoQuitacao.Comprovante = comprovante;

                if (!string.IsNullOrEmpty(observacao)) empresaSolicitacaoQuitacao.Observacao = observacao;

                new Repositorio<EmpresaSolicitacaoQuitacao>().Incluir(empresaSolicitacaoQuitacao);
                scope.Complete();
            }
        }

        public static void Liquidar(int idaverbacao, string motivo, int IDResponsavel)
        {
            if (!AverbacaoParticipaCompra(idaverbacao))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Liquidado, 0, motivo);

                    ParcelasAlterarSituacao(idaverbacao, (int)Enums.AverbacaoParcelaSituacao.LiquidadaManual);
                    scope.Complete();
                }
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    Averbacao a = ObtemAverbacao(idaverbacao);

                    EmpresaSolicitacao es = Solicitacoes.ObtemUltimaSolicitacaoPendente(idaverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao);
                    if (es != null)
                    {
                        Solicitacoes.AtualizaSolicitacao(es.IDEmpresaSolicitacao, (int)Enums.SolicitacaoSituacao.Processada, IDResponsavel, es.Motivo);
                        ProcessarFluxoCompra(a);
                    }
                    else
                    {
                        Averbacao pai = ObtemAverbacao(idaverbacao).AverbacaoVinculo.OrderByDescending(x => x.IDAverbacaoVinculo).Select(x => x.Averbacao1).FirstOrDefault();
                        int idresponsavel = DadosSessao.IdUsuario;

                        if (!ExistemAverbacoesParaQuitar(pai, idaverbacao))
                            Solicitacoes.AdicionaSolicitacao(pai.IDConsignataria, (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida, (int)Enums.SolicitacaoSituacao.Pendente, pai.IDConsignataria, pai.IDAverbacao, null, idresponsavel, null, "", "");

                        Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Comprado, 0, motivo);
                    }

                    //Solicitacoes.AdicionaSolicitacao(a.IDConsignataria, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao, (int)Enums.SolicitacaoSituacao.Processada, null, idaverbacao, a.IDFuncionario, a.IDUsuario, null, "Número de Contrato: " + a.Numero, "Confirmação de quitação realizada através de operação de liquidação.");


                    scope.Complete();
                }
            }
        }

        private static void ParcelasAlterarSituacao(int idaverbacao, int situacao)
        {
            //Repositorio<AverbacaoParcela> repap = new Repositorio<AverbacaoParcela>();

            ObjectContext ctx = new Repositorio<AverbacaoParcela>().ObterObjectContext();
            int result;

            string anomes = ObtemAnoMesCorte(DadosSessao.IdModulo, DadosSessao.IdBanco);

            var comando2 = ctx.CreateStoreCommand("SP_AlterarParcelaSituacao", CommandType.StoredProcedure, new SqlParameter("AnoMes", anomes), new SqlParameter("idaverbacao", idaverbacao), new SqlParameter("idsituacao", situacao));

            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando2.ExecuteNonQuery();
            }

            //string comando = "update averbacaoparcela set idaverbacaoparcelasituacao = " + situacao.ToString() + " where idaverbacao = " + idaverbacao.ToString();
            //repap.ExecutarSQL(comando); // + " and competencia >= '" + anomes + "'");

            //IQueryable<AverbacaoParcela> parcelas = repap.Listar().Where(x => x.IDAverbacao == idaverbacao);

            //foreach (var item in parcelas)
            //{
            //    if (item.Competencia.CompareTo(anomes) >= 0)
            //    {
            //        item.IDAverbacaoParcelaSituacao = situacao;
            //        repap.Alterar(item);
            //    }
            //}
        }

        private static bool AverbacaoParticipaCompra(int idaverbacao)
        {
            Repositorio<Averbacao> rep = new Repositorio<Averbacao>();
            Averbacao a = ObtemAverbacao(idaverbacao);
            return a.AverbacaoVinculo.Any(x => x.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || x.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao);
        }

        public static void SalvaInformacaoSaldoDevedor(int idsolicitacao, int idaverbacao, int idempresa, int idresponsavel, DateTime data, DateTime validade, decimal valor, int idTipoPagamento, string identificador, string banco, string agencia, string contacredito, string nomefavorecido, string observacao)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                EmpresaSolicitacaoSaldoDevedor empresaSolicitacaoSaldoDevedor = new EmpresaSolicitacaoSaldoDevedor();

                empresaSolicitacaoSaldoDevedor.IDAverbacao = idaverbacao;
                empresaSolicitacaoSaldoDevedor.IDEmpresaSolicitacao = idsolicitacao;
                empresaSolicitacaoSaldoDevedor.Data = data;
                empresaSolicitacaoSaldoDevedor.Validade = validade;
                empresaSolicitacaoSaldoDevedor.Valor = valor;
                empresaSolicitacaoSaldoDevedor.IDTipoPagamento = idTipoPagamento;
                empresaSolicitacaoSaldoDevedor.Identificador = identificador;
                empresaSolicitacaoSaldoDevedor.Banco = banco;
                empresaSolicitacaoSaldoDevedor.Agencia = agencia;
                empresaSolicitacaoSaldoDevedor.ContaCredito = contacredito;
                empresaSolicitacaoSaldoDevedor.NomeFavorecido = nomefavorecido;

                if (!string.IsNullOrEmpty(observacao)) empresaSolicitacaoSaldoDevedor.Observacao = observacao;

                new Repositorio<EmpresaSolicitacaoSaldoDevedor>().Incluir(empresaSolicitacaoSaldoDevedor);

                Averbacoes.SolicitacaoAtualizar(idaverbacao, idempresa, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, idresponsavel, false, observacao);
                scope.Complete();
            }
        }

        public static IQueryable<Averbacao> ExportaAverbacao()
        {
            return new Repositorio<Averbacao>().Listar().Where(x => x.CompetenciaInicial == "2011/12").Take(200);
        }

        public static bool Cancelar(int idaverbacao, string motivo, int IDResponsavel, out Enums.CancelamentoIndevido tipo)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                Averbacao averb = ObtemAverbacao(idaverbacao);

                bool bParticipandoCompra = averb.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra);
                if (bParticipandoCompra)
                {
                    tipo = Enums.CancelamentoIndevido.ParticipandoCompra;
                    return false;
                }

                if (averb.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra && (averb.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.CompraERenegociacao || averb.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.Compra))
                {
                    bool bExisteSolicitacoesProcessadas = false;
                    foreach (var item in averb.AverbacaoVinculo1)
                    {
                        if (item.Averbacao.IDConsignataria != averb.IDConsignataria)
                        {
                            if (Solicitacoes.VerificaSolicitacoesCompraProcessadas(averb.IDAverbacao, item.IDAverbacao))
                                bExisteSolicitacoesProcessadas = true;
                        }
                    }
                    if (bExisteSolicitacoesProcessadas)
                    {
                        tipo = Enums.CancelamentoIndevido.ExisteSolicitacoesProcessadas;
                        return false;
                    }
                }

                if (averb.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.CompraERenegociacao || averb.IDAverbacaoTipo == (int)Enums.AverbacaoTipo.Renegociacao)
                {
                    foreach (var item in averb.AverbacaoVinculo1)
                    {
                        if (item.Averbacao.IDConsignataria == averb.IDConsignataria)
                            Tramitar(item.IDAverbacao, (int)Enums.AverbacaoSituacao.Ativo, 0, motivo);
                    }
                }
                Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Cancelado, 0, motivo);

                ParcelasAlterarSituacao(idaverbacao, (int)Enums.AverbacaoParcelaSituacao.Cancelada);

                scope.Complete();
            }

            tipo = Enums.CancelamentoIndevido.Ok;
            return true;
        }

        public static void ArquivoParaDescontoFolha(string mesano)
        {
            var perfis = new Repositorio<Perfil>().Listar();

            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_GerarConciliacaoCorteFolha", CommandType.StoredProcedure, new SqlParameter("AnoMes", Utilidades.ConverteAnoMes(mesano)));

            int result;
            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando.ExecuteNonQuery();
            }

            var comando2 = ctx.CreateStoreCommand("SP_GerarConciliacaoMovimento", CommandType.StoredProcedure, new SqlParameter("AnoMes", Utilidades.ConverteAnoMes(mesano)));

            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando2.ExecuteNonQuery();
            }
            //var dados2 = comando.Materialize<MenuPermissaoAcesso>();

            //return dados;
        }

        public static decimal CalculaValorDeducaoMargem(int IdAverbacao)
        {
            Averbacao a = ObtemAverbacao(IdAverbacao);
            decimal valorparcela = a.ValorParcela;

            decimal valoresvinc = a.AverbacaoVinculo1.Where(x => x.Averbacao.AverbacaoSituacao.DeduzMargem).Sum(w => w.Averbacao.ValorParcela);

            decimal valordeducao = valorparcela - valoresvinc;
            //if (valordeducao < 0)
            //    valordeducao = 0;

            return valordeducao;
        }

        public static IQueryable<Averbacao> AverbacoesRefinanciadas(int IdAverbacao)
        {
            Averbacao averb = ObtemAverbacao(IdAverbacao);
            if (averb == null) return new List<Averbacao>().AsQueryable();
            return averb.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria == averb.IDConsignataria).Select(y => y.Averbacao).AsQueryable();
        }

        public static IQueryable<Averbacao> AverbacoesCompradas(int IdAverbacao)
        {
            Averbacao averb = ObtemAverbacao(IdAverbacao);
            if (averb == null) return new List<Averbacao>().AsQueryable();
            return averb.AverbacaoVinculo1.Where(x => x.Averbacao.IDConsignataria != averb.IDConsignataria).Select(y => y.Averbacao).AsQueryable();
        }

        public static decimal CalculaRefinanciaQueDeduzMargem(List<int> lista)
        {
            decimal soma = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                Averbacao a = ObtemAverbacao(lista[i]);
                if (a.AverbacaoSituacao.DeduzMargem)
                    soma += a.ValorParcela;
            }
            return soma;
        }

        public static decimal CalculaCompraQueDeduzMargem(List<int> lista)
        {
            decimal soma = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                Averbacao a = ObtemAverbacao(lista[i]);
                if (a.AverbacaoSituacao.DeduzMargem)
                    soma += a.ValorParcela;
            }
            return soma;
        }

        public static ProdutoGrupo ObtemProdutoGrupo(int IdProdutoGrupo)
        {
            return new Repositorio<ProdutoGrupo>().ObterPorId(IdProdutoGrupo);
        }

        public static IEnumerable<Averbacao> ListaContratosParcelas(int idempresa, int modulo, string competenciaInicial, string competenciaFinal)
        {
            if (modulo == (int)Enums.Modulos.Consignante)
                return new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia.CompareTo(competenciaInicial) >= 0 && x.Competencia.CompareTo(competenciaFinal) <= 0 && x.IDAverbacaoParcelaSituacao > (int)Enums.AverbacaoParcelaSituacao.Cancelada && x.IDAverbacao != null).ToList().Select(x => Averbacoes.ObtemAverbacao(x.IDAverbacao)).ToList();
            else
                return new Repositorio<AverbacaoParcela>().Listar().Where(x => x.Competencia.CompareTo(competenciaInicial) >= 0 && x.Competencia.CompareTo(competenciaFinal) <= 0 && x.IDAverbacaoParcelaSituacao > (int)Enums.AverbacaoParcelaSituacao.Cancelada && x.IDAverbacao != null && x.Averbacao.IDConsignataria == idempresa).ToList().Select(x => Averbacoes.ObtemAverbacao(x.IDAverbacao)).ToList();
        }

        public static string ObtemPrazoMaximo(int idproduto, int idprodutogrupo)
        {
            Parametro parametro = Geral.ObtemParametro("PrazoMaximo");
            string prazomaximo = parametro.Valor;

            Produto p = ObtemProduto(idproduto);
            if ((p != null) && (p.PrazoMaximo.HasValue) && (p.PrazoMaximo.Value > 0))
                prazomaximo = p.PrazoMaximo.Value.ToString();

            return prazomaximo;
        }

        public static Produto ObtemProduto(int idproduto)
        {
            return new Repositorio<Produto>().ObterPorId(idproduto);
        }

        public static void ConfirmarQuitacao(int averbacao, int idBanco, string observacao, int idResponsavel)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                SolicitacaoAtualizar(averbacao, idBanco, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao, idResponsavel);
                Tramitar(averbacao, (int)Enums.AverbacaoSituacao.Comprado, idBanco, observacao);

                scope.Complete();
            }
        }

        public static void AplicaLiquidacoes()
        {
            IQueryable<EmpresaSolicitacao> solicParaLiquidar = Solicitacoes.SolicitacoesPrazoExpiradoParaLiquidacao();

            foreach (var item in solicParaLiquidar)
            {
                if (item.IDAverbacao.HasValue)
                    Liquidar(item.IDAverbacao.Value, "Liquidação Automática por Expiração do Prazo de Confirmação da Quitação.", item.IDSolicitante.HasValue ? item.IDSolicitante.Value : DadosSessao.IdUsuario);
            }
        }

        public static bool VerificaExistePrevenirDuplicacao(Averbacao averb)
        {
            Repositorio<Averbacao> rep = new Repositorio<Averbacao>();
            return rep.Listar().ToList().Any(x => x.IDFuncionario == averb.IDFuncionario && x.Data == DateTime.Today && (averb.AverbacaoSituacao != null && averb.AverbacaoSituacao.DeduzMargem) && x.IDConsignataria == averb.IDConsignataria && x.IDProduto == averb.IDProduto && x.ValorParcela == averb.ValorParcela);
        }

        public static IEnumerable<AnaliseProducao> RelatorioAnaliseProducao(int idempresa, string mesinicio, string mesfim)
        {
            var perfis = new Repositorio<Perfil>().Listar();

            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_AnaliseProducaoConsignados", CommandType.StoredProcedure, new SqlParameter("IdEmpresa", idempresa), new SqlParameter("MesInicio", mesinicio), new SqlParameter("MesFim", mesfim));
            var dados = comando.Materialize<AnaliseProducao>();

            return dados;
        }

        public static IQueryable<Averbacao> listaAverbacaoFuncionario(int p)
        {
            return new Repositorio<Averbacao>().Listar().Where(x => x.IDFuncionario == p && x.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.Liquidado && x.IDAverbacaoSituacao != (int)Enums.AverbacaoSituacao.Concluido).OrderByDescending(x => x.Data);
        }

        public static IQueryable<Averbacao> listaAverbacoesFuncionario(int idfuncionario, int situacao)
        {
            DateTime hoje = new DateTime();
            hoje = DateTime.Today;
            IQueryable<Averbacao> a = new Repositorio<Averbacao>().Listar().Where(x => x.IDFuncionario == idfuncionario && x.IDAverbacaoSituacao == situacao && x.Ativo == 1);
            if (situacao == (int)Enums.AverbacaoSituacao.PreReserva)
                a = a.Where(x => x.PrazoAprovacao >= hoje);
            return a;
        }

        public static void CancelarAverbacaoReservada(int idaverbacao)
        {            
            Repositorio<Averbacao> averb = new Repositorio<Averbacao>();
            Averbacao a = averb.ObterPorId(idaverbacao);
            using (averb)
            {
                averb.Excluir(a);                
            }

        }

        public static void IncluirAverbacao(Averbacao a)
        {
            int idaverbacao = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                Repositorio<Averbacao> averb = new Repositorio<Averbacao>();
                idaverbacao = averb.Incluir(a);

                Tramitar(idaverbacao, (int)Enums.AverbacaoSituacao.Averbado, a.IDConsignataria, "");

                GerarParcelas(a);

                scope.Complete();
            }

        }

    }
}