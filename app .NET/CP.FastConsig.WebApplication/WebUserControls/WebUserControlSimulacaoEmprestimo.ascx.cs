using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using CP.FastConsig.BLL;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlSimulacaoEmprestimo : CustomUserControl
    {

        #region Constantes

        private const string IdControleImageViabilidade = "ImageViabilidade";
        private const string PathImagensLogos = "~/Imagens/Logos/{0}";
        private const string PathImagens = "~/Imagens/{0}";
        private const string ImagemNaoViavel = "cross.png";
        private const string ImagemViavel = "tick.png";
        private const string IdControleImageButtonPreReservar = "ImageButtonPreReservar";
        private const string IdControleImageButtonSolicitarContato = "ImageButtonSolicitarContato";

        #endregion

        private const string ParametroIdFunc = "IDFuncDash";

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFunc] == null) ViewState[ParametroIdFunc] = 0;
                return (int)ViewState[ParametroIdFunc];
            }
            set
            {
                ViewState[ParametroIdFunc] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            ConfiguraComponentes();

            Pessoa p = Sessao.UsuarioLogado.Pessoa.FirstOrDefault();
            if (p != null)
            {
                if (p.Funcionario.FirstOrDefault() != null)
                    IdFunc = p.Funcionario.FirstOrDefault().IDFuncionario;
            }


            EhPostBack = true;

        }

        private int QuantidadeParcelas
        {
            get { return Convert.ToInt32(ParametrosConfiguracao[(int) PosicaoParametro.QuantidadeParcelas]); }
        }

        private decimal ValorParcela
        {
            get { return Convert.ToDecimal(ParametrosConfiguracao[(int) PosicaoParametro.ValorParcela]); }
        }

        private decimal ValorLiberado
        {
            get { return Convert.ToDecimal(ParametrosConfiguracao[(int) PosicaoParametro.ValorLiberado]); }
        }

        private decimal MargemDisponivel
        {
            get { return Convert.ToDecimal(ParametrosConfiguracao[(int)PosicaoParametro.MargemDisponivel]); }
        }

        private enum PosicaoParametro
        {

            QuantidadeParcelas,
            ValorParcela,
            ValorLiberado,
            MargemDisponivel

        }

        private void ConfiguraComponentes()
        {
            ConfiguraTopo();
            ConfiguraResultadoGridSimulacao();
        }

        private void ConfiguraResultadoGridSimulacao()
        {

            List<EmpresaCoeficienteDetalhe> coeficientes = FachadaSimulacaoEmprestimo.ObtemCoeficientes(QuantidadeParcelas).OrderBy(x => x.Coeficiente).ToList();

            if (coeficientes == null || coeficientes.Count == 0)
            {
                PageMaster.ExibeMensagem("Não existem coeficientes cadastrado para esta condição!");
                return;
            }

            int ranking = 1;

            GridViewSimulacaoEmprestimo.DataSource = ValorParcela != 0 ? coeficientes.Select(x => 
                new Rank { Competencia = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco) , 
                    Prazo = x.Prazo, 
                    Logo = ObtemCaminhoLogoConsignataria(FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDContribuinteFastConsig ?? 0), 
                    Ranking = string.Format("{0}º", ranking++), 
                    ValorParcela = ValorParcela, 
                    Banco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).Nome, 
                    ValorAverbacao = ValorParcela / x.Coeficiente, 
                    Taxa = x.CET ?? 0,
                    IDBanco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDEmpresa
                }).ToList() 
                : coeficientes.Select(x => new Rank {
                    Competencia = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco), 
                    Prazo = x.Prazo,
                    Logo = ObtemCaminhoLogoConsignataria(FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDContribuinteFastConsig ?? 0), 
                    Ranking = string.Format("{0}º", ranking++), 
                    ValorParcela = ValorLiberado * x.Coeficiente, 
                    Banco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).Nome, 
                    ValorAverbacao = ValorLiberado, 
                    Taxa = x.CET ?? 0,
                    IDBanco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDEmpresa
                }).ToList(); 
            GridViewSimulacaoEmprestimo.DataBind();

        }

        private string ObtemCaminhoLogoConsignataria(int idConsignante)
        {
            if (idConsignante == null)
                return null;

            try
            {
                using (ServicoUsuarioClient ServicoUsuario = new ServicoUsuarioClient()) return string.Format(PathImagensLogos, ServicoUsuario.ObtemConsignante(idConsignante).Logo);
            }
            catch
            {
                return null;
            }
        }

        class Rank
        {

            public string Ranking { get; set; }
            public string Banco { get; set; }
            public string Logo { get; set; }
            public string Competencia { get; set; }

            public int Prazo { get; set; }

            public decimal ValorParcela { get; set; }
            public decimal ValorAverbacao { get; set; }
            public decimal Taxa { get; set; }
            public int IDBanco { get; set; }
        }

        protected void ButtonNovaSimulacao_Click(object sender, EventArgs e)
        {
            PageMaster.Voltar();
        }

        protected void ButtonImprimir_Click(object sender, EventArgs e)
        {
            return;
        }

        protected void ImageButtonPreReservar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            string conteudo = bt.CommandArgument;

            String[] str = conteudo.Split(';');

            bool viavel = Convert.ToBoolean(str[0]);
            int idbanco = Convert.ToInt32(str[1]);
            int prazo = Convert.ToInt32(str[2]);
            decimal valorAverbacao = Convert.ToDecimal(str[3]);
            decimal valorParcela = Convert.ToDecimal(str[4]);

            if (viavel)
            {
                Funcionario func = Funcionarios.ObtemFuncionario(IdFunc);
                                
                Repositorio<Pessoa> reppessoa = new Repositorio<Pessoa>();
                Pessoa pessoa = reppessoa.ObterPorId(func.IDPessoa);

                List<Produto> produto = Produtos.ListaProdutos(idbanco, (int)Enums.ProdutoGrupo.Emprestimos).ToList();

                //EmpresaSolicitacaoTipo solicTipo = Solicitacoes.ObtemSolicitacaoTipo( (int)Enums.SolicitacaoTipo.AprovarReservaporSimulacao ); 

                if (produto.Count > 0)
                {
                    DateTime dataCancelar = new DateTime();
                    dataCancelar = DateTime.Today;
                    dataCancelar.AddDays(Convert.ToInt32(produto[0].PrazoMaximo)); 

                    Averbacao dado = new Averbacao();
                    dado.IDConsignataria = idbanco;
                    dado.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.Normal;
                    dado.Ativo = 1;
                    dado.Obs = "Funcionário Solicitou para Reservar.";
                    dado.Data = DateTime.Today;
                    dado.IDFuncionario = IdFunc;
                    dado.IDProduto = produto[0].IDProduto;
                    dado.IDUsuario = Sessao.IdUsuario;
                    dado.Prazo = prazo;
                    dado.ValorContratado = valorAverbacao;
                    dado.ValorDevidoTotal = valorAverbacao;
                    dado.ValorTroco = 0;
                    dado.ValorParcela = valorParcela;
                    dado.ValorDeducaoMargem = valorParcela;
                    dado.IDAverbacaoSituacao = (int)Enums.AverbacaoSituacao.PreReserva;
                    dado.PrazoAprovacao = dataCancelar;

                    int idaverbacao = Averbacoes.SalvarAverbacao(dado, pessoa, (int)Enums.ProdutoGrupo.Emprestimos, false, new List<int>(), new List<int>());                    

                    Solicitacoes.AdicionaSolicitacao(dado.IDConsignataria, (int)Enums.SolicitacaoTipo.AprovarReservaporSimulacao, (int)Enums.SolicitacaoSituacao.Pendente, null, idaverbacao, dado.IDFuncionario, dado.IDUsuario, null, "Número de Averbação (Reservado): " + dado.Numero,"Reservado");

                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemSimulacaoEmprestimoReserva);
                }
                else
                {
                    PageMaster.ExibeMensagem("Este banco não tem produto do tipo Empréstimo cadastrado.");
                }
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSimulacaoEmprestimoSemMargemSuficiente);
            }
        }

        protected void ImageButtonSolicitarContato_Click(object sender, EventArgs e)
        {
            LinkButton bt = (LinkButton)sender;
            string conteudo = bt.CommandArgument;

            String[] str = conteudo.Split(';');

            bool viavel = Convert.ToBoolean(str[0]);
            int idbanco = Convert.ToInt32(str[1]);
            int prazo = Convert.ToInt32(str[2]);
            decimal valorAverbacao = Convert.ToDecimal(str[3]);
            decimal valorParcela = Convert.ToDecimal(str[4]);

            string descricaoEmprestimo = string.Format("Valor: {0} - Prazo: {1} - Valor da Parcela: {2}", valorAverbacao, prazo, valorParcela);

            if (viavel)
            {
                Solicitacoes.AdicionaSolicitacao(IdFunc, (int)Enums.SolicitacaoTipo.SolicitacaoEmprestimo, (int)Enums.SolicitacaoSituacao.Pendente, idbanco, 0, IdFunc, Sessao.IdUsuario, Sessao.IdUsuario, descricaoEmprestimo, "");
            }
            
            PageMaster.ExibeMensagem(viavel ? ResourceMensagens.MensagemSimulacaoEmprestimoSolicitarContato : ResourceMensagens.MensagemSimulacaoEmprestimoSemMargemSuficiente);
        }

        protected void GridViewSimulacaoEmprestimo_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            GridViewRow linha = e.Row;

            if (linha.RowType != DataControlRowType.DataRow) return;

            Rank rank = (Rank) linha.DataItem;

            Image viabilidade = (Image) linha.FindControl(IdControleImageViabilidade);

            ImageButton imageButtonPreReservar = (ImageButton) linha.FindControl(IdControleImageButtonPreReservar);
            LinkButton imageButtonSolicitarContato = (LinkButton) linha.FindControl(IdControleImageButtonSolicitarContato);

            bool viavel = rank.ValorParcela <= MargemDisponivel;

            imageButtonPreReservar.CommandArgument = imageButtonSolicitarContato.CommandArgument = String.Format("{0};{1};{2};{3};{4}", viavel.ToString(), rank.IDBanco, rank.Prazo, rank.ValorAverbacao, rank.ValorParcela);

            viabilidade.ImageUrl = string.Format(PathImagens, viavel ? ImagemViavel : ImagemNaoViavel);

        }

    }

}