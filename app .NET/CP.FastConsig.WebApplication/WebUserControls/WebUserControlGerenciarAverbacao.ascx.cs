using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.DAL;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Common;
using DevExpress.Web.ASPxGridView;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using System.IO;

namespace CP.FastConsig.WebApplication.WebUserControls
{        
    public partial class WebUserControlGerenciarAverbacao : CustomUserControl
    {
        string fileName;

        #region Constantes

        private const string ParametroTela = "parametrotela";
        private const string ParametroDadosPopulados = "DadosPopulados";
        private const string ParametroVariasConsignatarias = "DadosVariasConsignatarias";
        private string filtroDescricao;
        private string separador = ", ";
        private string formatoFiltro = "'<b>{0}</b>'";

        #endregion

        private bool bMaisDeUmProduto;

        private enum TipoFiltroData
        {
            SemData,
            DataAverbacao,
            DataTramitacao
        }

        private bool DadosPopulados
        {
            get
            {
                if (ViewState[ParametroDadosPopulados] == null) ViewState[ParametroDadosPopulados] = false;
                return (bool)ViewState[ParametroDadosPopulados];
            }
            set
            {
                ViewState[ParametroDadosPopulados] = value;
            }
        }

        private bool bSolicVariasConsignarias
        {
            get
            {
                try
                {
                    if (ViewState[ParametroVariasConsignatarias] == null) ViewState[ParametroVariasConsignatarias] = Convert.ToBoolean(ParametrosConfiguracao[1]);
                    return (bool)ViewState[ParametroVariasConsignatarias];
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //PostBackTrigger pt = new PostBackTrigger();
            //pt.ControlID = "ButtonExportarPDF";
            //upPrincipal.Triggers.Add(pt);

            if ((DivResultado.Visible) && (Averbacoes.Any()))
            {
                gridAverbacaos.DataSource = Averbacoes;
                gridAverbacaos.DataBind();
            }

            if (ControleCarregado) return;

            if (!EhPostBack)
            {
                EhPostBack = true;

                ConfigurarTela();                

                if (Id != null && Id > 0)
                {
                    DivResultado.Visible = true;                    
                    ASPxRoundPanelFiltro.Visible = false;
                    PopularDados(Id.Value, false);
                }

                bMaisDeUmProduto = FachadaProdutos.existeMaisDeUmProduto(Sessao.IdBanco);

            }
            else
            {
                if (DadosPopulados)
                {
                    ASPxRoundPanelFiltro.Visible = false;
                }
                else
                    ASPxRoundPanelFiltro.Visible = true;
            }

        }

        private void ConfigurarTela()
        {

            CheckBoxBuscarEmMinhasSolicitacoes.Visible = !Sessao.IdModulo.Equals((int) Enums.Modulos.Consignante);

            DropDownListLocal.DataSource = FachadaGerenciarAverbacao.ListaLocais();
            DropDownListLocal.DataBind();

            DropDownListLocal.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            DropDownListCargo.DataSource = FachadaGerenciarAverbacao.ListaCargos();
            DropDownListCargo.DataBind();

            DropDownListCargo.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            DropDownListSetor.DataSource = FachadaGerenciarAverbacao.ListaSetores();
            DropDownListSetor.DataBind();

            DropDownListSetor.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            List<EmpresaSolicitacaoTipo> listaSolicitacoesTipo;
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignante)
            {
                listaSolicitacoesTipo =  FachadaSolicitacao.listaSolicitacaoTipo().ToList();
            }
            else
            {
                listaSolicitacoesTipo = FachadaGerenciarAverbacao.ObtemSolicitacoesTipo(Sessao.IdBanco).ToList();
                listaSolicitacoesTipo.AddRange(FachadaGerenciarAverbacao.ListaSolicitacaoTipo().Where(x => x.Prazo == 0).ToList());
            }

            DropDownListTiposSolicitacoes.DataSource = listaSolicitacoesTipo.OrderBy(x => x.Nome).ToList();
            DropDownListTiposSolicitacoes.DataBind();

            DropDownListTiposSolicitacoes.Items.Insert(0, DropDownListTiposSolicitacoes.Items.Equals(0) ? new ListItem(ResourceMensagens.LabelSemItens, "-1") : new ListItem(ResourceMensagens.LabelSolicitacao, "-1"));

            DropDownListSituacao.DataSource = FachadaGerenciarAverbacao.ListaAverbacaoSituacao().ToList();
            DropDownListSituacao.DataBind();
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.TodasQueDeduzemMargem, "100"));
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.TodasQueNaoDeduzemMargem, "102"));
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.TodasSuspensas, "103"));
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.TodasDescontaveisEmFolha, "104"));
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                DropDownListProdutoGrupo.DataSource = FachadaGerenciarAverbacao.ListaProdutoGrupo().Where(x => x.Produto.Any(y => y.IDConsignataria == Sessao.IdBanco)).ToList();
            else
                DropDownListProdutoGrupo.DataSource = FachadaGerenciarAverbacao.ListaProdutoGrupo().ToList();
            DropDownListProdutoGrupo.DataBind();
            DropDownListProdutoGrupo.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                DropDownListProduto.DataSource = FachadaGerenciarAverbacao.ListaProduto().Where(x => x.IDConsignataria == Sessao.IdBanco).ToList();
            else
                DropDownListProduto.DataSource = FachadaGerenciarAverbacao.ListaProduto().ToList();
            DropDownListProduto.DataBind();
            DropDownListProduto.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            //DropDownListSituacaoCompra.DataSource = FachadaGerenciarAverbacao.ListaAverbacaoSituacaoDeCompra().ToList();
            //DropDownListSituacaoCompra.DataBind();
            //DropDownListSituacaoCompra.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            //DropDownListAverbacaoTipo.DataSource = FachadaGerenciarAverbacao.ListaAverbacaoTipo().ToList();
            //DropDownListAverbacaoTipo.DataBind();
            //DropDownListAverbacaoTipo.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListConsignataria.DataSource = FachadaGerenciarAverbacao.ListaConsignataria().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListAgente.DataSource = FachadaGerenciarAverbacao.ListaAgente().ToList();
            DropDownListAgente.DataBind();
            DropDownListAgente.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListFuncionarioSituacao.DataSource = FachadaGerenciarAverbacao.ListaFuncionarioSituacao().ToList();
            DropDownListFuncionarioSituacao.DataBind();
            DropDownListFuncionarioSituacao.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListSituacaoFuncional.DataSource = FachadaGerenciarAverbacao.ListaFuncionarioSituacaoFolha().ToList();
            DropDownListSituacaoFuncional.DataBind();
            DropDownListSituacaoFuncional.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            DropDownListRegime.DataSource = FachadaGerenciarAverbacao.ListaFuncionarioRegime().ToList();
            DropDownListRegime.DataBind();
            DropDownListRegime.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListCategoria.DataSource = FachadaGerenciarAverbacao.ListaFuncionarioCategoria().ToList();
            DropDownListCategoria.DataBind();
            DropDownListCategoria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();
                DropDownListConsignataria.Enabled = false;                
            }

        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            DadosPopulados = PopularDados(true);

            ASPxRoundPanelFiltro.Visible = !DadosPopulados;                        

            //if (DadosPopulados)
            //    ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", ResourceAuxiliar.ScriptRodapePagina, true);

        }

        private bool PopularDados(bool mostrarmsg)
        {
            bool retorno = PopularDados(0, mostrarmsg);
            ASPxRoundPanelFiltro.Visible = !retorno;
            return retorno;
        }

        public List<Averbacao> Averbacoes
        {
            get
            {
                if (Session["Averbacoes"] == null) Session["Averbacoes"] = new List<Averbacao>();
                return (List<Averbacao>) Session["Averbacoes"];
            }
            set
            {
                Session["Averbacoes"] = value;
            }
        } 

        private bool PopularDados(int idsolicitacaotipo, bool mostrarmsg)
        {

            filtroDescricao = "Busca por: ";

            if ((!string.IsNullOrEmpty(TextBoxBusca.Text)) && (idsolicitacaotipo == 0) && (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue)==-1))
                filtroDescricao += string.Format(formatoFiltro, TextBoxBusca.Text) + separador;

            if (idsolicitacaotipo > 0)
            {
                DropDownListTiposSolicitacoes.SelectedValue = idsolicitacaotipo.ToString();

                filtroDescricao += "Tipo de Solicitação = " + string.Format(formatoFiltro, DropDownListTiposSolicitacoes.SelectedItem.Text ) + separador;

                if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == -1)
                    return false;
            }
            else
            {
                if ((Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue)>0))
                    filtroDescricao += "Tipo de Solicitação = " + string.Format(formatoFiltro, DropDownListTiposSolicitacoes.SelectedItem.Text) + separador;
            }

            bool filtrarPorDataTramitacao = Convert.ToInt32(RadioButtonListFiltroTipoData.SelectedValue).Equals((int)TipoFiltroData.DataTramitacao);
            if ((filtrarPorDataTramitacao) && (DateEditPeriodoInicio.Date.Year > 1) && (DateEditPeriodoFim.Date.Year > 1))
                filtroDescricao += "Período de tramitação " + string.Format(formatoFiltro, String.Format("{0:dd/MM/yy}", DateEditPeriodoInicio.Date) + " a " +
                    String.Format("{0:dd/MM/yy}", DateEditPeriodoFim.Date)) + separador;
            
            bool filtrarPorDataAverbacao = Convert.ToInt32(RadioButtonListFiltroTipoData.SelectedValue).Equals((int)TipoFiltroData.DataAverbacao);
            if ((filtrarPorDataAverbacao) && (DateEditPeriodoInicio.Date.Year > 1) && (DateEditPeriodoFim.Date.Year > 1))
                filtroDescricao += "Período de averbação " + string.Format(formatoFiltro, String.Format("{0:dd/MM/yy}", DateEditPeriodoInicio.Date) + " a " +
                    String.Format("{0:dd/MM/yy}", DateEditPeriodoFim.Date)) + separador;

            bool buscarEmAverbacoesSolicitacao = CheckBoxBuscarEmMinhasSolicitacoes.Checked;
            if (buscarEmAverbacoesSolicitacao)
                filtroDescricao += string.Format(formatoFiltro, "Solicitadas por mim") + separador;

            int idSituacao = Convert.ToInt32(DropDownListSituacao.SelectedValue);
            if (idSituacao > 0)
            {
                filtroDescricao += "Situação = " + string.Format(formatoFiltro, DropDownListSituacao.SelectedItem.Text) + separador;
            }

            if ((!string.IsNullOrEmpty(ASPxTextAnoMes.Text)) && (!string.IsNullOrEmpty(TextBoxAnoMesFinal.Text)) && (ASPxTextAnoMes.Text != "  /") && (TextBoxAnoMesFinal.Text != "  /"))
                filtroDescricao += "Mês/Ano = " + string.Format(formatoFiltro, ASPxTextAnoMes.Text + " a " + TextBoxAnoMesFinal.Text) + separador;

            if (Convert.ToInt32(DropDownListProdutoGrupo.SelectedValue) > 0)
                filtroDescricao += "Grupo Produto = " + string.Format(formatoFiltro, DropDownListProdutoGrupo.SelectedItem.Text) + separador;

            if (Convert.ToInt32(DropDownListProduto.SelectedValue) > 0)
                filtroDescricao += "Produto = " + string.Format(formatoFiltro, DropDownListProduto.SelectedItem.Text) + separador;

            if (Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0)
                filtroDescricao += "Consignatária = " + string.Format(formatoFiltro, DropDownListConsignataria.SelectedItem.Text) + separador;

            if (Convert.ToInt32(DropDownListAgente.SelectedValue) > 0)
                filtroDescricao += "Correspondente = " + string.Format(formatoFiltro, DropDownListAgente.SelectedItem.Text) + separador;

            if ((!string.IsNullOrEmpty(TextBoxPrazo.Text)) && (Convert.ToInt32(TextBoxPrazo.Text)>0))
                filtroDescricao += "Prazo = " + string.Format(formatoFiltro, TextBoxPrazo.Text) + separador;

            if (!string.IsNullOrEmpty(DropDownListLocal.SelectedValue))
                filtroDescricao += "Local = " + string.Format(formatoFiltro, DropDownListLocal.SelectedItem.Text) + separador;

            if (!string.IsNullOrEmpty(DropDownListSetor.SelectedValue))
                filtroDescricao += "Setor = " + string.Format(formatoFiltro, DropDownListSetor.SelectedItem.Text) + separador;

            if (!string.IsNullOrEmpty(DropDownListCargo.SelectedValue))
                filtroDescricao += "Cargo = " + string.Format(formatoFiltro, DropDownListCargo.SelectedItem.Text) + separador;

            if ((!string.IsNullOrEmpty(DropDownListRegime.SelectedValue)) && (Convert.ToInt32(DropDownListRegime.SelectedValue) > 0))
                filtroDescricao += "Regime = " + string.Format(formatoFiltro, DropDownListRegime.SelectedItem.Text) + separador;

            if (Convert.ToInt32(DropDownListCategoria.SelectedValue) > 0)
                filtroDescricao += "Categoria = " + string.Format(formatoFiltro, DropDownListCategoria.SelectedItem.Text) + separador;

            if (!string.IsNullOrEmpty(DropDownListSituacaoFuncional.SelectedValue))
                filtroDescricao += "Situação Funcional = " + string.Format(formatoFiltro, DropDownListSituacaoFuncional.SelectedItem.Text) + separador;

            if ((!string.IsNullOrEmpty(DropDownListFuncionarioSituacao.SelectedValue)) && (Convert.ToInt32(DropDownListFuncionarioSituacao.SelectedValue) > 0))
                filtroDescricao += "Situação Cadastral = " + string.Format(formatoFiltro, DropDownListFuncionarioSituacao.SelectedItem.Text) + separador;          
            
            var dados = FachadaGerenciarAverbacao.PesquisarContratos(Sessao.IdModulo, Sessao.IdBanco, TextBoxBusca.Text, DateEditPeriodoInicio.Date, DateEditPeriodoFim.Date, filtrarPorDataTramitacao, filtrarPorDataAverbacao, ASPxTextAnoMes.Text, TextBoxAnoMesFinal.Text, idSituacao,
                Convert.ToInt32(DropDownListProdutoGrupo.SelectedValue), Convert.ToInt32(DropDownListProduto.SelectedValue), 
                Convert.ToInt32(DropDownListConsignataria.SelectedValue), Convert.ToInt32(DropDownListAgente.SelectedValue),
                0, Convert.ToInt32(string.IsNullOrEmpty(TextBoxPrazo.Text) ? "0" : TextBoxPrazo.Text), DropDownListLocal.SelectedValue, DropDownListSetor.SelectedValue, DropDownListCargo.SelectedValue,
                DropDownListRegime.SelectedItem.Text, Convert.ToInt32(DropDownListCategoria.SelectedValue), Convert.ToInt32(DropDownListFuncionarioSituacao.SelectedValue),
                Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue), DropDownListSituacaoFuncional.SelectedValue, buscarEmAverbacoesSolicitacao, bSolicVariasConsignarias).ToList();

            LabelFiltroDescricao.Text = "<b>Filtrado por:</b> " + filtroDescricao;

            Averbacoes = dados.ToList();

            gridAverbacaos.DataSource = Averbacoes;
            gridAverbacaos.DataBind();

            if (string.IsNullOrEmpty(TextBoxBusca.Text))
            {
                MostraColunasPorSolicitacaoTipo(Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue));
            }
            else
            {
                MostraColunasPadrao();
            }

            if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.AprovarAverbacoes)
            {
                gridAverbacaos.Columns[0].Visible = true;
                ASPxButtonAprovar.Visible = true;
                ASPxButtonDesaprovar.Visible = true;
            }
            else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) > 0)
            {
                gridAverbacaos.Columns[0].Visible = false;
                ASPxButtonAprovar.Visible = false;
                ASPxButtonDesaprovar.Visible = false;
                gridAverbacaos.Columns[1].Visible = true;                

            }
            else
            {
                gridAverbacaos.Columns[0].Visible = false;
                ASPxButtonAprovar.Visible = false;
                ASPxButtonDesaprovar.Visible = false;
            }

            if (dados.Count() > 0)
            {
                DivResultado.Visible = true;
                return true;
            }
            else
            {
                if (mostrarmsg)
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemNaoExiste);
                ASPxRoundPanelFiltro.Visible = true;
                return false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (bSolicVariasConsignarias) gridAverbacaos.GroupBy(gridAverbacaos.Columns["Empresa1.Nome"]);
        }

        protected void MostraColunasPorSolicitacaoTipo(int solicitacaoTipo)
        {
            gridAverbacaos.Columns["Numero"].Visible = false;
            gridAverbacaos.Columns["Data"].Visible = false;
            gridAverbacaos.Columns["Prazo"].Visible = false;
            gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = false;
            gridAverbacaos.Columns["Expiracao"].Visible = false;
            gridAverbacaos.Columns["SaldoBruto"].Visible = false;
            gridAverbacaos.Columns["Solicitante"].Visible = false;
            gridAverbacaos.Columns["PrazoRestante"].Visible = false;
            gridAverbacaos.Columns["SaldoDevedor"].Visible = false;
            gridAverbacaos.Columns["FormaPagamento"].Visible = false;
            gridAverbacaos.Columns["Obs"].Visible = false;
            gridAverbacaos.Columns["InformadoPor"].Visible = false;
            gridAverbacaos.Columns["CompetenciaInicial"].Visible = false;
            gridAverbacaos.Columns["CompetenciaFinal"].Visible = false;
            gridAverbacaos.Columns["AverbacaoSituacao.Nome"].Visible = false;
            gridAverbacaos.Columns["Empresa1.Nome"].Visible = bSolicVariasConsignarias;

            if (bSolicVariasConsignarias)
            {
                //((GridViewDataColumn)gridAverbacaos.Columns["Empresa1.Nome"]).GroupBy(); 
                //gridAverbacaos.GroupBy( gridAverbacaos.Columns[4]);
            }

            if ((solicitacaoTipo == (int)Enums.SolicitacaoTipo.AprovarAverbacoes) || (solicitacaoTipo == (int)Enums.SolicitacaoTipo.AprovarReservaporSimulacao))
            {
                gridAverbacaos.Columns["Data"].Visible = true;
                gridAverbacaos.Columns["Prazo"].Visible = true;
                gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = bMaisDeUmProduto;
                gridAverbacaos.Columns["Expiracao"].Visible = true;
            }
            else if ((solicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos))
            {
                gridAverbacaos.Columns["SaldoBruto"].Visible = true;
                gridAverbacaos.Columns["Solicitante"].Visible = true;
                gridAverbacaos.Columns["PrazoRestante"].Visible = true;
                gridAverbacaos.Columns["Expiracao"].Visible = true;
            }
            else if ((solicitacaoTipo == (int)Enums.SolicitacaoTipo.InformarQuitacao) || (solicitacaoTipo == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada))
            {
                gridAverbacaos.Columns["SaldoBruto"].Visible = true;
                gridAverbacaos.Columns["SaldoDevedor"].Visible = true;
                gridAverbacaos.Columns["FormaPagamento"].Visible = true;
                gridAverbacaos.Columns["InformadoPor"].Visible = true;
                gridAverbacaos.Columns["Obs"].Visible = true;
                gridAverbacaos.Columns["Expiracao"].Visible = true;
            }
            else if (solicitacaoTipo == (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao)
            {
                gridAverbacaos.Columns["SaldoBruto"].Visible = true;
                gridAverbacaos.Columns["SaldoDevedor"].Visible = true;
                gridAverbacaos.Columns["FormaPagamento"].Visible = true;
                gridAverbacaos.Columns["InformadoPor"].Visible = true;
                gridAverbacaos.Columns["Expiracao"].Visible = true;
            }
            else if (solicitacaoTipo == (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida)
            {
                gridAverbacaos.Columns["Data"].Visible = true;
                gridAverbacaos.Columns["Prazo"].Visible = true;
                gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = bMaisDeUmProduto;
                gridAverbacaos.Columns["SaldoDevedor"].Visible = true;
                gridAverbacaos.Columns["FormaPagamento"].Visible = true;
                gridAverbacaos.Columns["InformadoPor"].Visible = true;
                gridAverbacaos.Columns["Expiracao"].Visible = true;
            }
            else if (solicitacaoTipo == (int)Enums.SolicitacaoTipo.AcompanharAverbacaoSemPrimeiroDesconto)
            {
                gridAverbacaos.Columns["Data"].Visible = true;
                gridAverbacaos.Columns["Prazo"].Visible = true;
                gridAverbacaos.Columns["CompetenciaInicial"].Visible = true;
                gridAverbacaos.Columns["CompetenciaFinal"].Visible = true;
            }
            else if (Sessao.IdModulo == (int)Enums.Modulos.Consignante)
            {
                gridAverbacaos.Columns["Data"].Visible = true;
                gridAverbacaos.Columns["Prazo"].Visible = true;
                gridAverbacaos.Columns["Numero"].Visible = true;
                gridAverbacaos.Columns["Empresa1.Nome"].Visible = true;
                gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = true;
                gridAverbacaos.Columns["AverbacaoSituacao.Nome"].Visible = true;
                gridAverbacaos.Columns["PrazoRestante"].Visible = true;
            }
            else
            {
                gridAverbacaos.Columns["Data"].Visible = true;
                gridAverbacaos.Columns["Prazo"].Visible = true;
                gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = bMaisDeUmProduto;
                gridAverbacaos.Columns["PrazoRestante"].Visible = true;
                gridAverbacaos.Columns["AverbacaoSituacao.Nome"].Visible = true;
            }
        }

        protected void MostraColunasPadrao()
        {
            gridAverbacaos.Columns["Numero"].Visible = false;
            gridAverbacaos.Columns["Data"].Visible = false;
            gridAverbacaos.Columns["Prazo"].Visible = false;
            gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = false;
            gridAverbacaos.Columns["Expiracao"].Visible = false;
            gridAverbacaos.Columns["SaldoBruto"].Visible = false;
            gridAverbacaos.Columns["Solicitante"].Visible = false;
            gridAverbacaos.Columns["PrazoRestante"].Visible = false;
            gridAverbacaos.Columns["SaldoDevedor"].Visible = false;
            gridAverbacaos.Columns["FormaPagamento"].Visible = false;
            gridAverbacaos.Columns["Obs"].Visible = false;
            gridAverbacaos.Columns["InformadoPor"].Visible = false;
            gridAverbacaos.Columns["CompetenciaInicial"].Visible = false;
            gridAverbacaos.Columns["CompetenciaFinal"].Visible = false;
            gridAverbacaos.Columns["AverbacaoSituacao.Nome"].Visible = false;
            gridAverbacaos.Columns["Empresa1.Nome"].Visible = bSolicVariasConsignarias; 
            
            gridAverbacaos.Columns["Data"].Visible = true;
            gridAverbacaos.Columns["Prazo"].Visible = true;
            gridAverbacaos.Columns["Produto.ProdutoGrupo.Nome"].Visible = bMaisDeUmProduto;
            gridAverbacaos.Columns["PrazoRestante"].Visible = true;
            gridAverbacaos.Columns["AverbacaoSituacao.Nome"].Visible = true;
        }

        protected void Filtros_Click(object sender, EventArgs e)
        {
            divFiltros.Visible = !divFiltros.Visible;
            btnFiltros.Text = divFiltros.Visible ? "Esconder Pesquisa Avançada" : "Exibir Pesquisa Avançada";
        }

        protected void ButtonFiltro_Click(object sender, EventArgs e)
        {
            ASPxRoundPanelFiltro.Visible = true;
            DivResultado.Visible = false;            
        }

        protected void ButtonExportarPDF_Click(object sender, EventArgs e)
        {            
            ASPxGridViewExporter1.FileName = "Averbacoes";
            ASPxGridViewExporter1.WritePdfToResponse( "Averbacoes", true);
        }

        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.FileName = "Averbacoes";
            ASPxGridViewExporter1.WriteXlsToResponse(ASPxGridViewExporter1.FileName, true);
        }

        protected void ButtonExportarTXT_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.FileName = "Averbacoes";
            ASPxGridViewExporter1.WriteCsvToResponse(ASPxGridViewExporter1.FileName, true);
        }

        protected void gridVinculos_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacaoVinculos(id);

            gridMaster.DataSource = dados;

        }

        protected void gridParcelas_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacao(id).AverbacaoParcela;

            gridMaster.DataSource = dados;

        }

        protected void gridTramitacoes_DataSelect(object sender, EventArgs e)
        {
            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacao(id).AverbacaoTramitacao.OrderByDescending(x => x.CreatedOn);

            gridMaster.DataSource = dados;

        }

        protected void gridAverbacaos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            int idAverbacao = Convert.ToInt32(e.KeyValue);
            int idTipoSolicitacao = Id.Value.Equals(0) ? Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) : Id.Value;

            Averbacao averbacao = FachadaGerenciarAverbacao.ObtemAverbacao(idAverbacao);

            if (Sessao.IdModulo != (int)Enums.Modulos.Consignante && averbacao.IDConsignataria != Sessao.IdBanco && !FachadaGerenciarAverbacao.ContratoEmProcessoDeQuitacaoOuCompra(idAverbacao))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemProibidoAcessoContratoOutrosBancos);
                return;
            }

            if (e.CommandArgs.CommandName == "Edit")
            {

                switch (idTipoSolicitacao)
                {
                    
                    case (int)Enums.SolicitacaoTipo.InformarQuitacao:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, Sessao.IdModulo), 1, idAverbacao);
                        break;

                    case (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConfirmarQuitacao, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlConfirmarQuitacao, Sessao.IdModulo), 1, idAverbacao);
                        break;

                    case (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, Sessao.IdModulo), 1, idAverbacao);
                        break;

                    case (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAverbacao, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlAverbacao, Sessao.IdModulo), 1, idAverbacao);
                        break;

                    case (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarSaldoDevedor, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlInformarSaldoDevedor, Sessao.IdModulo), 1, idAverbacao);
                        break;
                    case (int)Enums.SolicitacaoTipo.MinhasSolicitacoesdeCompraAguardandoSaldoDevedor:

                        PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlInformarQuitacao, Sessao.IdModulo), 1, idAverbacao);
                        break;

                }

            }
            else
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, this.IdRecurso, 1, idAverbacao);
            }

        }

        protected void gridVinculos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            int idAverbacao = Convert.ToInt32(e.KeyValue);

            Averbacao averbacao = FachadaGerenciarAverbacao.ObtemAverbacao(idAverbacao);

            if (Sessao.IdModulo != (int)Enums.Modulos.Consignante && averbacao.IDConsignataria != Sessao.IdBanco && !FachadaGerenciarAverbacao.ContratoEmProcessoDeQuitacaoOuCompra(idAverbacao))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemProibidoAcessoContratoOutrosBancos);
                return;
            }

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, this.IdRecurso, 1, idAverbacao);
        }

        protected void ASPxButtonAprovar_Click(object sender, EventArgs e)
        {
            List<object> listaselecionados = gridAverbacaos.GetSelectedFieldValues("IDAverbacao");

            FachadaGerenciarAverbacao.AprovarDesaprovar(listaselecionados, Sessao.IdBanco, true, Sessao.UsuarioLogado.IDUsuario);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemAprovado);

            PopularDados(false);
        }

        protected void ASPxButtonDesaprovar_Click(object sender, EventArgs e)
        {
            List<object> listaselecionados = gridAverbacaos.GetSelectedFieldValues("IDAverbacao");

            FachadaGerenciarAverbacao.AprovarDesaprovar(listaselecionados, Sessao.IdBanco, false, Sessao.UsuarioLogado.IDUsuario);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemDesaprovado);

            PopularDados(false);
        }

        protected void ASPxButtonLimparFiltros_Click(object sender, EventArgs e)
        {
            DateEditPeriodoInicio.Text = string.Empty;
            DateEditPeriodoFim.Text = string.Empty;

            RadioButtonListFiltroTipoData.SelectedIndex = (int) TipoFiltroData.SemData;

            ASPxTextAnoMes.Text = string.Empty;
            TextBoxPrazo.Text = string.Empty;
            TextBoxAnoMesFinal.Text = string.Empty;

            DropDownListSituacao.SelectedIndex = 0;
            DropDownListProdutoGrupo.SelectedIndex = 0;
            DropDownListProduto.SelectedIndex = 0;
            DropDownListConsignataria.SelectedIndex = 0;
            DropDownListAgente.SelectedIndex = 0;
            //DropDownListAverbacaoTipo.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListSituacao.SelectedIndex = 0;
            DropDownListLocal.SelectedIndex = 0;
            DropDownListSetor.SelectedIndex = 0;
            DropDownListCargo.SelectedIndex = 0;
            DropDownListRegime.SelectedIndex = 0;
            DropDownListCategoria.SelectedIndex = 0;
            DropDownListFuncionarioSituacao.SelectedIndex = 0;
            DropDownListFuncionarioSituacao.SelectedIndex = 0;
            DropDownListSituacaoFuncional.SelectedIndex = 0;
        }

        protected void DropDownListConsignataria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfiguraProdutos();
        }

        private void ConfiguraProdutos()
        {
            int idConsignataria = DropDownListConsignataria.SelectedIndex > 0 ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : 0;
            int idProdutoGrupo = DropDownListProdutoGrupo.SelectedIndex > 0 ? Convert.ToInt32(DropDownListProdutoGrupo.SelectedValue) : 0;

            DropDownListProduto.DataSource = FachadaGerenciarAverbacao.ListaProduto(idConsignataria, idProdutoGrupo).ToList();

            DropDownListProduto.DataBind();

            DropDownListProduto.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListProdutoGrupo.DataSource = FachadaGerenciarAverbacao.ListaProdutoGrupo().Where(x => x.Produto.Any(y => y.IDConsignataria == idConsignataria)).ToList();
            DropDownListProdutoGrupo.DataBind();
            DropDownListProdutoGrupo.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
        }

        protected void DropDownListProdutoGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfiguraProdutos();
        }

        protected void gridAverbacaos_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (DivResultado.Visible)
            {
                DadosPopulados = PopularDados(false);
            }
        }

        protected void gridAverbacoes_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                
                Label labelPrazoRestante = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelPrazoRestante") as Label;

                Label labelSaldoBruto = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoBruto") as Label;

                int idaverbacao = (int)e.KeyValue;

                int parcrestante = FachadaAverbacoes.ObtemParcelasRestantes(idaverbacao);

                if (labelPrazoRestante != null)
                    labelPrazoRestante.Text = parcrestante.ToString();

                decimal valorParcela = (decimal)e.GetValue("ValorParcela");

                if (labelSaldoBruto != null)
                    labelSaldoBruto.Text = String.Format("{0:#,0.00}", valorParcela * parcrestante);

                string cpf = Utilidades.RetornaStringCpf( e.GetValue("Funcionario.Pessoa.CPF" ).ToString() );

                Label labelCpf = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelCpf") as Label;
                
                if (labelCpf != null)
                    labelCpf.Text = cpf;

                if (DropDownListTiposSolicitacoes.Items.Count.Equals(0)) ConfigurarTela();

                if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.AprovarAverbacoes)                   
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);

                    if ((solic != null) && (img != null))
                    {
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }
                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.AprovarReservaporSimulacao)
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.AprovarReservaporSimulacao);

                    if ((solic != null) && (img != null))
                    {
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }
                }
                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.InformarQuitacao)
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    Label saldoDevedor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoDevedor") as Label;

                    Label formaPagamento = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelFormaPagamento") as Label;

                    Label obs = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelObs") as Label;

                    Label informadoPor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelInformadoPor") as Label;

                    EmpresaSolicitacaoSaldoDevedor solicSaldoDevedor = FachadaSolicitacao.ObtemSolicitacaoSaldoDevedor(idaverbacao);

                    if (solicSaldoDevedor != null)
                    {
                        if (saldoDevedor != null)
                            saldoDevedor.Text = String.Format("{0:#,0.00}", solicSaldoDevedor.Valor);
                        if (formaPagamento != null)
                            formaPagamento.Text = solicSaldoDevedor.TipoPagamento.Nome;
                        if (obs != null)
                            obs.Text = solicSaldoDevedor.Observacao;

                        if ((solicSaldoDevedor.EmpresaSolicitacao.Usuario1 != null) && (informadoPor != null))
                        {
                            informadoPor.Text = solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Nome + "/" + solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Celular;
                        }
                    }

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.InformarQuitacao);

                    if ((solic != null) && (img != null))
                    {
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }
                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao)
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    Label saldoDevedor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoDevedor") as Label;

                    Label formaPagamento = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelFormaPagamento") as Label;

                    Label obs = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelObs") as Label;

                    Label informadoPor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelInformadoPor") as Label;

                    EmpresaSolicitacaoSaldoDevedor solicSaldoDevedor = FachadaSolicitacao.ObtemSolicitacaoSaldoDevedor(idaverbacao);

                    if (solicSaldoDevedor != null)
                    {
                        if (saldoDevedor != null)
                            saldoDevedor.Text = String.Format("{0:#,0.00}", solicSaldoDevedor.Valor);
                        if (formaPagamento != null)
                            formaPagamento.Text = solicSaldoDevedor.TipoPagamento.Nome;
                        if (obs != null)
                            obs.Text = solicSaldoDevedor.Observacao;

                        if ((solicSaldoDevedor.EmpresaSolicitacao.Usuario1 != null) && (informadoPor != null))
                        {
                            informadoPor.Text = solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Nome + "/" + solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Celular;
                        }
                    }

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao);

                    if ((solic != null) && (img != null))
                    {
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }
                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada)
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    Label saldoDevedor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoDevedor") as Label;

                    Label formaPagamento = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelFormaPagamento") as Label;

                    Label obs = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelObs") as Label;

                    Label informadoPor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelInformadoPor") as Label;

                    EmpresaSolicitacaoSaldoDevedor solicSaldoDevedor = FachadaSolicitacao.ObtemSolicitacaoSaldoDevedor(idaverbacao);                 

                    if (solicSaldoDevedor != null)
                    {
                        if (saldoDevedor != null)
                            saldoDevedor.Text = String.Format("{0:#,0.00}", solicSaldoDevedor.Valor);
                        if (formaPagamento != null)
                            formaPagamento.Text = solicSaldoDevedor.TipoPagamento.Nome;
                        if (obs != null)
                            obs.Text = solicSaldoDevedor.Observacao;

                        if ((solicSaldoDevedor.EmpresaSolicitacao.Usuario1 != null) && (informadoPor != null))
                        {
                            informadoPor.Text = solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Nome + "/" + solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Celular;
                        }
                    }

                    EmpresaSolicitacaoQuitacao solicQuitacao = FachadaSolicitacao.ObtemSolicitacaoQuitacao(idaverbacao);

                    if ((solicQuitacao != null) && (obs != null))
                        obs.Text = solicQuitacao.Observacao;

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.RegularizarQuitaçãoRejeitada);

                    if ((solic != null) && (img != null))
                    {                      
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }
                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos)
                {

                    Label labelSolicitante = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSolicitante") as Label;

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos);

                    if (solic == null) return;

                    if ((solic.IDFuncionario != null) && (labelSolicitante != null))
                    {
                        labelSolicitante.Text = FachadaFuncionariosConsulta.ObtemFuncionario((int)solic.IDFuncionario).Pessoa.Nome;
                    }
                    else if ((solic.IDEmpresaSolicitacao != null) && (labelSolicitante != null))
                    {
                        labelSolicitante.Text = solic.Empresa.Nome;
                    }

                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    if ((solic != null) && (img != null))
                    {
                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }

                else if (Convert.ToInt32(DropDownListTiposSolicitacoes.SelectedValue) == (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida)
                {
                    Image img = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "ImagemExpiracao") as Image;

                    Label saldoDevedor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoDevedor") as Label;

                    Label formaPagamento = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelFormaPagamento") as Label;

                    Label obs = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelObs") as Label;

                    Label informadoPor = gridAverbacaos.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelInformadoPor") as Label;

                    EmpresaSolicitacaoSaldoDevedor solicSaldoDevedor = FachadaSolicitacao.ObtemSolicitacaoSaldoDevedor(idaverbacao);

                    if (solicSaldoDevedor != null)
                    {
                        if (saldoDevedor != null)
                            saldoDevedor.Text = String.Format("{0:#,0.00}", solicSaldoDevedor.Valor);
                        if (formaPagamento != null)
                            formaPagamento.Text = solicSaldoDevedor.TipoPagamento.Nome;
                        if (obs != null)
                            obs.Text = solicSaldoDevedor.Observacao;

                        if ((solicSaldoDevedor.EmpresaSolicitacao.Usuario1 != null) && (informadoPor != null))
                        {
                            informadoPor.Text = solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Nome + "/" + solicSaldoDevedor.EmpresaSolicitacao.Usuario1.Celular;
                        }
                    }

                    EmpresaSolicitacao solic = FachadaSolicitacao.ObtemSolicitacaoPendente(idaverbacao, (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida);

                    if ((solic != null) && (img != null))
                    {

                        if (solic.DataSolicitacao < DateTime.Today)
                            img.ImageUrl = "~/Imagens/RedBall.png";
                        else if (solic.DataSolicitacao > DateTime.Today)
                            img.ImageUrl = "~/Imagens/GreenBall.png";
                        else
                            img.ImageUrl = "~/Imagens/YellowBall.png";
                    }

                }

            }
        }

        protected void gridAverbacaos_PageIndexChanged(object sender, EventArgs e)
        {
            gridAverbacaos.DataSource = Averbacoes;
            gridAverbacaos.DataBind();
        }

        void ExportarGrid(bool saveAs)
        {

            //const string fileName = "Conciliação Detalhe";
            //string contentType = "application/ms-excel";

            switch (cmbTipoExportacao.SelectedIndex)
            {

                case 0:
                    fileName = "Consulta_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".pdf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopularDados(false);
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WritePdf(s);

                    }
                    break;
                case 1:
                    fileName = "Consulta_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopularDados(false);
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WriteXls(s);
                    }
                    break;
                case 2:
                    fileName = "Consulta_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".rtf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopularDados(false);
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WriteRtf(s);
                    }
                    break;
                case 3:
                    fileName = "Consulta_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".txt";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopularDados(false);
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WriteCsv(s);
                    }
                    break;

            }

        }

        protected void buttonSaveAs_Click(object sender, EventArgs e)
        {
            ExportarGrid(true);
        }


        private void ExecutaScript()
        {
            RegistrarStartupScript(this, "function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest); } " +
            "function onEndRequest(sender, args) { " +
            "if (sender._postBackSettings.sourceElement != null && sender._postBackSettings.sourceElement.id == 'DownloadFile') { " +
                "if ($('iframe') != null) $('iframe').remove(); " +
                "var iframe = document.createElement('iframe'); " +
                "iframe.src = 'DownloadFile.aspx?arquivo=" + fileName + "'; " +
                "iframe.style.display = 'none'; " +
                "document.body.appendChild(iframe);} } ");
        }
    }

}