using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlDashBoard : CustomUserControl
    {

        #region Constantes

        private const string FormatoDataHoraGrafico = "dd/MM/yyyy hh:mm:ss";
        private const string ValorZerado = "0,00";
        private const string ObterMaisDinheiro = "ObterMaisDinheiro";
        private const string DiminuirValorParcelas = "DiminuirValorParcelas";

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

        private bool bCarregaControle = false;

        private bool RadioButtonMarcado
        {
            get
            {
                foreach (ListItem item in RadioButtonListOpcao.Items)
                {
                    if (item.Selected) return true;
                }

                return false;
            }
        }

        private string ValueRadioButtonMarcado
        {
            get
            {
                foreach (ListItem item in RadioButtonListOpcao.Items) if (item.Selected) return item.Value;
                return string.Empty;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (bCarregaControle)
            {
                PageMaster.CarregaControle("WebUserControlMinhasAverbacoes", 216, 1, IdFunc);
            }
        }

        private bool bAcessoUtilizacaoMargem = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += Page_LoadComplete;

            string eventArgs = Request["__EVENTARGUMENT"];

            bAcessoUtilizacaoMargem = FachadaPermissoesAcesso.CheckPermissao(215, IdFunc, Sessao.IdPerfil, (int)Enums.Permissao.GraficoUtilizacaoMargem);

            if (!string.IsNullOrEmpty(eventArgs))
            {
                if (eventArgs.Equals("link_click"))
                {
                    EhPostBack = false;                    

                    Pessoa p = Sessao.UsuarioLogado.Pessoa.FirstOrDefault();
                    if (p != null)
                    {
                        if (p.Funcionario.FirstOrDefault() != null)
                            IdFunc = p.Funcionario.FirstOrDefault().IDFuncionario;
                    }

                    bCarregaControle = true;
                }
            }

            if (EhPostBack) return;

            Pessoa pp = Sessao.UsuarioLogado.Pessoa.FirstOrDefault();

            if (pp != null)
            {
                if (pp.Funcionario.FirstOrDefault() != null)
                    IdFunc = pp.Funcionario.FirstOrDefault().IDFuncionario;
            }

            List<GrupoMargem> gruposMargem = FachadaDashBoard.ListarMargens(IdFunc).ToList();
            ConfiguraDadosMargem();

            //if (bAcessoUtilizacaoMargem)
            //{
                ConfiguraGraficoUtilizacaoMargem(gruposMargem);
                ConfiguraGraficoMargemPorProduto(gruposMargem);
                populaAverbacoesReservadas();
            //}
            //else
            //{
            //    ASPxRoundPanel3.Visible = false;
            //    ASPxRoundPanel4.Visible = false;
            //}

            EhPostBack = true;

        }

        private void ConfiguraDadosMargem()
        {
            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            GridViewUtilizacaoMargem.DataSource = FachadaFuncionariosConsulta.ProcessaMargens(func.FuncionarioMargem).ToList(); //gruposMargem.Select(x => new { Produto = x.Nome, MargemTotal = x.MargemFolha, MargemDisponivel = x.MargemDisponivel }).ToList();
            GridViewUtilizacaoMargem.DataBind();

            string margem = string.Format("{0:N}",func.MargemDisponivel1.ToString()); // gruposMargem.Where(x => x.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos).Sum(x => x.MargemDisponivel).ToString();
            ASPxTextBoxMargemDisponivel.Text = margem;
            ASPxTextBoxMargemNegociacao.Text = margem;
            hfMargemFolha.Value = string.Format("{0:N}", func.MargemFolha(1).ToString());
        }

        private void ConfiguraGraficoMargemPorProduto(List<GrupoMargem> gruposMargem)
        {
            
            decimal valorTotal = gruposMargem.Sum(x => x.MargemUtilizada ?? 0);

            if (valorTotal.Equals(0)) return;

            Dictionary<string, decimal> dados = gruposMargem.Where(x => x.MargemUtilizada != null && !x.MargemUtilizada.Equals(0)).ToDictionary(x => x.Nome, x => ((decimal)(x.MargemUtilizada / valorTotal)) * 100);

            WebUserControlChartPizzaUtilizacaoMargemPorProduto.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartPizzaMargemPorProduto, string.Empty, DateTime.Now.ToString(FormatoDataHoraGrafico), dados);
            
        }

        private void populaAverbacoesReservadas()
        {
            List<Averbacao> reservas = FachadaAverbacoes.listaAverbacoesFuncionario(IdFunc, (int)Enums.AverbacaoSituacao.PreReserva).ToList();
            GridViewReservas.DataSource = reservas;            
            GridViewReservas.DataBind();
        }

        private void ConfiguraGraficoUtilizacaoMargem(List<GrupoMargem> gruposMargem)
        {
            
            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosTotais = new List<decimal>();
            List<decimal> dadosDisponiveis = new List<decimal>();

            List<string> categorias = new List<string>();

            foreach (GrupoMargem grupoMargem in gruposMargem)
            {

                categorias.Add(grupoMargem.Nome);
                dadosTotais.Add(grupoMargem.MargemFolha);
                dadosDisponiveis.Add(grupoMargem.MargemDisponivel ?? 0);

            }

            serieValores.Add(ResourceMensagens.LabelGraficoMargemTotal, dadosTotais.ToArray());
            serieValores.Add(ResourceMensagens.LabelGraficoMargemDisponivel, dadosDisponiveis.ToArray());

            WebUserControlChartBarraMargens.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartBarraUtilizacaoMargem, string.Empty, DateTime.Now.ToString(FormatoDataHoraGrafico), string.Empty, categorias.ToArray(), serieValores);

        }

        protected void Imprimir_Reservas_Click(object sender, EventArgs e)
        {            
            //
        }

        protected void ButtonLimpar_Click(object sender, EventArgs e)
        {

            ASPxTextBoxValorLiberado.Text = string.Empty;
            ASPxTextBoxValorParcela.Text = string.Empty;
            ASPxTextBoxQuantidadeParcelas.Text = string.Empty;

        }

        protected void ButtonSimular_Click(object sender, EventArgs e)
        {

            if ((ASPxTextBoxValorParcela.Text.Equals(ValorZerado) || ASPxTextBoxValorLiberado.Text.Equals(ValorZerado)) && string.IsNullOrEmpty(ASPxTextBoxQuantidadeParcelas.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemPreencherValorParcelaOuValorLiberado);
                return;
            }

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSimulacaoEmprestimo, 0, 1, ASPxTextBoxQuantidadeParcelas.Text, ASPxTextBoxValorParcela.Text, ASPxTextBoxValorLiberado.Text, ASPxTextBoxMargemDisponivel.Text);

        }

        protected void ButtonSimularNegociacaoDivida_Click(object sender, EventArgs e)
        {

            if (!RadioButtonMarcado)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemMarcarPeloMenosUmaOpcao);
                return;
            }

            if (string.IsNullOrEmpty(ASPxTextBoxInformacao.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemDigitarInformacaoSolicitada);
                return;
            }

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSimulacaoCompraDivida, 0, RadioButtonListOpcao.SelectedValue, ASPxTextBoxMargemNegociacao.Text, hfMargemFolha.Value, ASPxTextBoxInformacao.Text, IdFunc);
        }

        protected void ButtonSimularNegociacaoDivida_SelectedIndexChanged(object sender, EventArgs e)
        {

            RadioButtonList radioButton = (RadioButtonList) sender;

            string value = radioButton.SelectedValue;

            if(!string.IsNullOrEmpty(value))
            {

                LabelInformacao.Visible = true;

                ASPxTextBoxInformacao.Visible = true;
                ASPxTextBoxInformacao.Text = string.Empty;
                ASPxTextBoxInformacao.MaskSettings.Mask = "$ <0..99999g>.<00..99>";

                if (Convert.ToInt32(value) == (int)Enums.TipoSimulacaoDivida.ObterMaisDinheiro)
                {
                    LabelInformacao.Text = ResourceMensagens.LabelValorDesejado;
                }
                else if (Convert.ToInt32(value) == (int)Enums.TipoSimulacaoDivida.ReduzirValorPago)
                {
                    LabelInformacao.Text = ResourceMensagens.LabelQuantoGostariaPagar;
                }
                else
                {
                    LabelInformacao.Text = ResourceMensagens.LabelPrazoDesejado;
                    ASPxTextBoxInformacao.MaskSettings.Mask = "999";
                }

            }

        }

        protected void CancelarReserva_Click(object sender, EventArgs e)
        {
            LinkButton bt = (LinkButton)sender;
            string conteudo = bt.CommandArgument;

            Averbacoes.CancelarAverbacaoReservada( Convert.ToInt32(conteudo) );

            PageMaster.ExibeMensagem("Reserva cancelada com Sucesso!");

            populaAverbacoesReservadas();

        }

        protected void GridViewReservado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow linha = e.Row;

            if (linha.RowType != DataControlRowType.DataRow) return;

            Averbacao a = (Averbacao)linha.DataItem;

            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(a.IDFuncionario);

            Label margem = (Label)linha.FindControl("LabelMargemDisponivel");

            if (margem != null)
            {
                decimal margemdisponivel = func.MargemDisponivel(a.Produto.IDProdutoGrupo);
                margem.Text = String.Format("{0:N}", margemdisponivel);
            }
        }

    }

}