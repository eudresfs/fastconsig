using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using System.Text;
using CP.FastConsig.BLL;

namespace CP.FastConsig.WebApplication.WebUserControls
{

	public partial class WebUserControlSimulacaoCompraDivida : CustomUserControl
	{

		#region Constantes
        private const string PathImagensLogos = "~/Imagens/Logos/{0}";
		private const string CampoParcelaParcela = "Parcela";
		private const string CampoSaldoBruto = "SaldoBruto";
		private const string CampoValor = "Valor";
		private const string IdControleAsPxCheckBoxSelecionar = "ASPxCheckBoxSelecionar";
		private const string CampoAverbacao = "Averbacao";
		private const string CampoTroco = "Troco";
		private const string NomeBancoDoBrasil = "Banco do Brasil";
		private const string NomeBancoItau = "Itaú";
		private const string NomeBancoBVFinanceira = "BV Financeira";
		private const string NomeBancoSantander = "Santander";
		private const string FormatoDinheiro = "{0:C}";
		private const string ParametroValorMargem = "ValorMargem";
        private const string ParametroValorMargemFolha = "ValorMargemFolha";
        private const string ParametroTipoSimulacao = "TipoSimulacao";
        private const string ParametroTotalSaldoDevedor = "TotalSaldoDevedor";
        private const string ParametroMargemParaRenegociacao = "MargemParaNegociacao";
        private const string ParametroEhViavel = "EhViavel";
		private const string IdControleImageViabilidade = "ImageViabilidade";
		private const string PathImagens = "~/Imagens/{0}";
		private const string ImagemNaoViavel = "cross.png";
		private const string ImagemViavel = "tick.png";
		private const string MascaraRanking = "{0}º";
		private const string ParametroCoeficientes = "Coeficientes";
		private const string ParametroValorDesejado = "ValorDesejado";
		private const char SeparadorValores = ';';
		private const string ValorMonetarioPadrao = "0";
		private const string ParametroAtualizaIndices = "AtualizaIndices";
        private const string ParametroIdFunc = "IdFuncDivida";

		private static readonly Dictionary<string, decimal> ValoresTotais = new Dictionary<string, decimal>();

		private static List<int> IndicesSelecionadosGridAverbacaosAtivos = new List<int>();

		#endregion

		private string ValueDropDownMarcado
		{
			get
			{
				return (string)ParametrosConfiguracao[1];
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

            if (EhPostBack || ControleCarregado) return;

            ConfiguraTela();
            PageMaster.Titulo = "Simulação de Compra de Dívida";

            EhPostBack = true;

		}

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

		public override void ConfiguraTopo()
		{
			base.ConfiguraTopo();
			PageMaster.Titulo = "Simulação de Compra de Dívida";
		}

		private void ConfiguraTela()
		{

			ValoresTotais.Clear();

			ValoresTotais.Add(CampoParcelaParcela, 0);
			ValoresTotais.Add(CampoSaldoBruto, 0);
			ValoresTotais.Add(CampoValor, 0);

			ConfiguraDropDown();

			IndicesSelecionadosGridAverbacaosAtivos = new List<int>();

            const int indiceTipoSimulacao = 1;
			const int indiceValorMargem = 2;
            const int indiceValorMargemFolha = 3;
            const int indiceValorDesejado = 4;
            const int indiceFuncionario = 5;

            TipoSimulacao = Convert.ToInt32(ParametrosConfiguracao[indiceTipoSimulacao]); // (int) TipoMarcado.ObterMaisDinheiro;
			ValorMargem = Convert.ToDecimal(ParametrosConfiguracao[indiceValorMargem]);
            ValorMargemFolha = Convert.ToDecimal(ParametrosConfiguracao[indiceValorMargemFolha]);
            ValorDesejado = Convert.ToDecimal(ParametrosConfiguracao[indiceValorDesejado]);
            IdFunc = Convert.ToInt32(ParametrosConfiguracao[indiceFuncionario]);

			ASPxTextBoxMargemDisponivelAtual.Text = ValorMargem.ToString();

            ConfiguraGridAverbacoes();

			MarcaAverbacoes();


		}

		private void MarcaAverbacoes()
		{
            int cont = 0;
            while (!EhViavel && cont <= ASPxGridViewAverbacaosAtivos.VisibleRowCount - 1)
            {
                ASPxGridViewAverbacaosAtivos.Selection.SelectRow(cont);
                Calcula();
                SimulaCompraDivida();
                cont++;
            }

            //switch (TipoSimulacao)
            //{

            //    case ((int)Enums.TipoSimulacaoDivida.ObterMaisDinheiro):

					
            //        int cont = 0;
            //        while (!EhViavel && cont <= ASPxGridViewAverbacaosAtivos.VisibleRowCount-1)
            //        {
            //            ASPxGridViewAverbacaosAtivos.Selection.SelectRow(cont);
            //            cont++;
            //        }
            //        break;
					
            //    default :

					
            //        break;


            //}

			//SimulaCompraDivida();

		}

		private void ConfiguraDropDown()
		{

			foreach (ListItem item in DropDownListOpcao.Items)
			{

				if (item.Value == ValueDropDownMarcado)
				{
					item.Selected = true;
					return;
				}

			}

		}

		private void ConfiguraGridAverbacoes()
		{

			var averbacoes = RetornaListaAverbacaos();

			ASPxGridViewAverbacaosAtivos.DataSource = averbacoes;
			ASPxGridViewAverbacaosAtivos.DataBind();

		}

		class AverbacaoCompraDivida
		{

			public string Banco { get; set; }
			public string Dia { get; set; }

			public int Prazo { get; set; }
			public int ParcelasPagas { get; set; }
			public int ParcelasRestantes { get; set; }

			public decimal Parcela { get; set; }
			public decimal Valor { get; set; }
			public decimal SaldoBruto { get; set; }

		}
		
		private IEnumerable<Averbacao> RetornaListaAverbacaos()
		{
            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);
            IEnumerable<Averbacao> averbacoes = func.Averbacao.Where(x => x.Ativo == 1);

			return averbacoes;
		}

		private decimal ValorMargem
		{

			get
			{
				if (ViewState[ParametroValorMargem] == null) ViewState[ParametroValorMargem] = 0;
				return Convert.ToDecimal(ViewState[ParametroValorMargem]);
			}
			set
			{
				ViewState[ParametroValorMargem] = value;
			}

		}

        private decimal ValorMargemFolha
        {

            get
            {
                if (ViewState[ParametroValorMargemFolha] == null) ViewState[ParametroValorMargemFolha] = 0;
                return Convert.ToDecimal(ViewState[ParametroValorMargemFolha]);
            }
            set
            {
                ViewState[ParametroValorMargemFolha] = value;
            }

        }

		private int TipoSimulacao
		{

			get
			{
				if (ViewState[ParametroTipoSimulacao] == null) ViewState[ParametroTipoSimulacao] = 0;
				return (int) ViewState[ParametroTipoSimulacao];
			}
			set
			{
				ViewState[ParametroTipoSimulacao] = value;
			}

		}

		private decimal ValorDesejado
		{

			get
			{
				if (ViewState[ParametroValorDesejado] == null) ViewState[ParametroValorDesejado] = 0;
				return Convert.ToDecimal(ViewState[ParametroValorDesejado]);
			}
			set
			{
				ViewState[ParametroValorDesejado] = value;
			}

		}

        private decimal TotalSaldoDevedor
		{

			get
			{
                if (ViewState[ParametroTotalSaldoDevedor] == null) ViewState[ParametroTotalSaldoDevedor] = 0;
                return Convert.ToDecimal(ViewState[ParametroTotalSaldoDevedor]);
			}
			set
			{
                ViewState[ParametroTotalSaldoDevedor] = value;
			}

		}

        private decimal MargemParaNegociacao
        {

            get
            {
                if (ViewState[ParametroMargemParaRenegociacao] == null) ViewState[ParametroMargemParaRenegociacao] = 0;
                return Convert.ToDecimal(ViewState[ParametroMargemParaRenegociacao]);
            }
            set
            {
                ViewState[ParametroMargemParaRenegociacao] = value;
            }

        }

        private bool EhViavel
        {

            get
            {
                if (ViewState[ParametroEhViavel] == null) ViewState[ParametroEhViavel] = false;
                return Convert.ToBoolean(ViewState[ParametroEhViavel]);
            }
            set
            {
                ViewState[ParametroEhViavel] = value;
            }

        }

		private void ConfiguraSeriesChart(List<Rank> rankings)
		{

			string[] categorias = rankings.Select(x => x.Banco).ToArray();

			List<decimal> dadosAverbacao = new List<decimal>();
			List<decimal> dadosTroco = new List<decimal>();

			Dictionary<string, decimal[]> series = new Dictionary<string, decimal[]>();

			foreach (Rank rank in rankings)
			{
				dadosAverbacao.Add(rank.ValorAverbacao);
				dadosTroco.Add(rank.Troco);
			}

			series.Add(CampoAverbacao, dadosAverbacao.ToArray());
			series.Add(CampoTroco, dadosTroco.ToArray());

			WebUserControlChartBarraSimulacaoEmprestimo.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartBarraSimulacaoEmprestimo, string.Empty, string.Empty, string.Empty, categorias, series);

		}

		private IEnumerable<EmpresaCoeficienteDetalhe> Coeficientes
		{
			get
			{
				if (Session[ParametroCoeficientes] == null) Session[ParametroCoeficientes] = FachadaSimulacaoCompraDivida.ObtemCoeficientes().OrderBy(x => x.Coeficiente).ToList();
				return (List<EmpresaCoeficienteDetalhe>) Session[ParametroCoeficientes];
			}
		}

        protected void ASPxGridViewAverbacaosAtivos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            int id = Convert.ToInt32(e.KeyValue);

            Averbacao a = FachadaGerenciarAverbacao.ObtemAverbacao(id);
            if (e.CommandArgs.CommandName == "Select")
            {
                int idempresa = a.IDConsignataria;

                int idconsignante = Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante());

                Solicitacoes.AdicionaSolicitacao(idconsignante, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, id, IdFunc, Sessao.IdUsuario, Sessao.IdUsuario, "Solicitação informação de Saldo Devedor", "");

                Parametro param = FachadaDashBoardConsignante.obterParametro("SaldoDevedor");

                string dias = param.Valor;

                string msg = "A solicitação foi enviada e o banco tem {0} dias úteis para responder.";

                PageMaster.ExibeMensagem(String.Format(msg, dias));
            }
            else if (e.CommandArgs.CommandName == "Edit")
            {
                int idempresa = a.IDConsignataria;

                int idconsignante = Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante());

                Solicitacoes.AdicionaSolicitacao(idconsignante, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, (int)Enums.SolicitacaoSituacao.Pendente, null, id, IdFunc, Sessao.IdUsuario, Sessao.IdUsuario, "Solicitação informação de Saldo Devedor", "");

                int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo);

                //if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.InformarSaldoDevedor))
                //{
                    PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarSaldoDevedor, idrecurso, (int)Enums.Permissao.InformarSaldoDevedor, id);
                //}
                //else
                //{
                //    PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
                //}
            }
        }

        private void SimulaCompraDivida()
		{
            EhViavel = false;
            //IEnumerable<EmpresaCoeficienteDetalhe> coeficientes = Coeficientes;

            //int ranking = 1;

            //var coeficientesAux = coeficientes.Select(x => ObtemRank(x, ranking++)).Where(x => x != null).ToList();

            //if (ValoresTotais.ContainsKey(CampoValor)) foreach (Rank rank in coeficientesAux) rank.Troco = Decimal.Round(rank.ValorAverbacao - ValoresTotais[CampoValor], 2);

            List<EmpresaCoeficienteDetalhe> coeficientes = FachadaSimulacaoEmprestimo.ObtemCoeficientes(0).OrderBy(x => x.Coeficiente).ToList();

            if (coeficientes == null || coeficientes.Count == 0)
            {
                PageMaster.ExibeMensagem("Não existem coeficientes cadastrado para esta condição!");
                return;
            }
            int ranking = 1;

            decimal ValorParcela = 0;
            decimal ValorLiberado = 0;

            if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.ObterMaisDinheiro)
                ValorLiberado = ValorDesejado + TotalSaldoDevedor;
            else if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.ReduzirValorPago || TipoSimulacao == (int)Enums.TipoSimulacaoDivida.RegularizarMargem)
                ValorParcela = ValorDesejado;
            else if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.DiminuirQuantidadeParcelas)
                ValorParcela = ValorMargemFolha;

            var dadossimulacao = ValorParcela != 0 ? coeficientes.Select(x =>
                            new Rank
                            {
                                Competencia = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco),
                                Prazo = x.Prazo,
                                Logo = ObtemCaminhoLogoConsignataria(FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDContribuinteFastConsig ?? 0),
                                Ranking = string.Format("{0}º", ranking++),
                                ValorParcela = ValorParcela,
                                Banco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).Nome,
                                ValorAverbacao = ValorParcela / x.Coeficiente,
                                Taxa = x.CET ?? 0,
                                Troco = (ValorParcela / x.Coeficiente) - TotalSaldoDevedor
                            }).ToList()
                            : coeficientes.Select(x => new Rank
                            {
                                Competencia = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco),
                                Prazo = x.Prazo,
                                Logo = ObtemCaminhoLogoConsignataria(FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).IDContribuinteFastConsig ?? 0),
                                Ranking = string.Format("{0}º", ranking++),
                                ValorParcela = ValorLiberado * x.Coeficiente,
                                Banco = FachadaSimulacaoEmprestimo.ObtemEmpresa(FachadaSimulacaoEmprestimo.ObtemCoeficiente(x.IDEmpresaCoeficiente).IDEmpresa).Nome,
                                ValorAverbacao = ValorLiberado,
                                Taxa = x.CET ?? 0,
                                Troco = ValorLiberado - TotalSaldoDevedor
                            }).ToList();

            ASPxGridViewSimulacaoNegociacao.DataSource = dadossimulacao;
			ASPxGridViewSimulacaoNegociacao.DataBind();

            ConfiguraSeriesChart(dadossimulacao.Take(5).ToList());

		}

        private string ObtemCaminhoLogoConsignataria(int idConsignante)
        {
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
            public decimal Troco { get; set; }
        }

        //protected override void OnPreRender(EventArgs e)
        //{

        //    base.OnPreRender(e);

        //    if (!AtualizaIndices) return;

        //    AtualizaIndices = false;

        //    for (int i = 0; i < ASPxGridViewAverbacaosAtivos.VisibleRowCount; i++)
        //    {
        //        ASPxCheckBox aSPxCheckBoxSelecionar = (ASPxCheckBox)ASPxGridViewAverbacaosAtivos.FindRowCellTemplateControl(i, null, IdControleAsPxCheckBoxSelecionar);
        //        if (aSPxCheckBoxSelecionar != null)
        //            aSPxCheckBoxSelecionar.Checked = IndicesSelecionadosGridAverbacaosAtivos.Contains(i);
        //    }

        //}

		protected void ASPxTextBoxMargemParaNegociacao_TextChanged(object sender, EventArgs e)
		{
			SimulaCompraDivida();
		}

		protected void DropDownListOpcao_SelectedIndexChanged(object sender, EventArgs e)
		{

			//TipoSimulacao = DropDownListOpcao.SelectedIndex;
			
			//DesmarcaTodosContratos();
			//MarcaAverbacoes();

		}
		
		protected void ASPxGridViewSimulacaoNegociacao_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{

			if (e.RowType == GridViewRowType.Data)
			{
				
				Image viabilidade = ASPxGridViewSimulacaoNegociacao.FindRowCellTemplateControl(e.VisibleIndex, null, IdControleImageViabilidade) as Image;

                bool viavel = false;
                if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.ObterMaisDinheiro)
                    viavel = Convert.ToDecimal(e.GetValue("ValorParcela")) <= MargemParaNegociacao;
                else if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.ReduzirValorPago || TipoSimulacao == (int)Enums.TipoSimulacaoDivida.RegularizarMargem)
                    viavel = (Convert.ToDecimal(e.GetValue("ValorParcela")) <= MargemParaNegociacao && Convert.ToDecimal(e.GetValue("ValorParcela")) <= ValorDesejado && Convert.ToDecimal(e.GetValue("Troco")) >= 0);
                else if (TipoSimulacao == (int)Enums.TipoSimulacaoDivida.DiminuirQuantidadeParcelas)
                    viavel = (Convert.ToDecimal(e.GetValue("ValorParcela")) <= MargemParaNegociacao && Convert.ToDecimal(e.GetValue("Troco")) >= 0 && Convert.ToDecimal(e.GetValue("Prazo")) <= ValorDesejado);
                if (!EhViavel)
                    EhViavel = viavel;
				viabilidade.ImageUrl = string.Format(PathImagens, viavel ? ImagemViavel : ImagemNaoViavel);

			}

		}

        protected void ASPxGridViewAverbacaosAtivos_SelectionChanged(object sender, EventArgs e)
        {
            Calcula();
            SimulaCompraDivida();
        }

        private decimal Calcula()
        {
            List<object> lista = ASPxGridViewAverbacaosAtivos.GetSelectedFieldValues("ValorParcela");
            TotalSaldoDevedor = ASPxGridViewAverbacaosAtivos.GetSelectedFieldValues("SaldoDevedorValor").Sum(x => Convert.ToDecimal(x));
            decimal soma = 0;
            if (lista.Count() > 0)
            {
                soma = lista.Sum(x => Convert.ToDecimal(x));                
            }
            MargemParaNegociacao = soma + ValorMargem;

            ASPxTextBoxParcelasParaNegociar.Text = soma.ToString();
            ASPxTextBoxMargemParaNegociacao.Text = MargemParaNegociacao.ToString();

            return soma;
        }

	}
	
}