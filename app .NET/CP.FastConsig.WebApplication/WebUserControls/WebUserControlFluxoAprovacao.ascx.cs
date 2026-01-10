using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Text;
using DevExpress.Web.ASPxDocking;
using CP.FastConsig.Facade;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlFluxoAprovacao : CustomUserControl
    {

        public string GetScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("function ShowHiddenDockPanels() {");
                sb.Append("var panels = manager.GetPanels();");
                sb.Append("for(var i = 0; i < panels.length; i++)");
                sb.Append("   panels[i].Show();");
                //sb.Append("SetShowHiddenPanelsButtonEnable(false);");
                sb.Append("}");
                sb.Append("");
                sb.Append("function SetShowHiddenPanelsButtonEnable(enable) {");
                sb.Append("   btnShowHiddenPanels.SetEnabled(enable);");
                sb.Append("}");
                return sb.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!EhPostBack)
            {
                PopulaCombos();
                EhPostBack = true;
                PopularDados();
            }
            //RegistrarBlockScript(this, GetScript, true);
        }
        
        protected void Salvar_Fluxo(object sender, EventArgs e)
        {
            int idmodulo = Convert.ToInt32(DropDownListModulo.SelectedValue);
            int idprodutogrupo = Convert.ToInt32(cmbTipoProduto.SelectedValue);
            int idempresa = (Sessao.IdModulo != (int)Enums.Modulos.Consignante ? Sessao.IdBanco : Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            if (idmodulo == (int)Enums.Modulos.Consignante)
            {
                FachadaFluxoAprovacao.SalvarFluxoAprovacao(Convert.ToInt32(cmbTipoProduto.SelectedValue), cbConsignante.Checked, cbFuncionario.Checked, cbConsignataria.Checked);
            }
            else
            {
                FachadaFluxoAprovacao.SalvarFluxoAprovacaoEmpresa(Convert.ToInt32(cmbTipoProduto.SelectedValue), idempresa, cbConsignataria.Checked);
            }
            
            //PageMaster.ExibeAlerta(ResourceMensagens.MensagemSucessoOperacao);
            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            //var paineis = DockManager.Panels.OrderBy(x => x.VisibleIndex);
            //for (int i = 0; i < paineis.Count(); i++)
            //{
                
            //}
        }

        private void PopulaCombos()
        {
            cmbTipoProduto.DataSource = FachadaAverbacoes.ObtemProdutosGrupo().ToList();
            cmbTipoProduto.DataBind();


            DropDownListModulo.DataSource = FachadaPermissoesAcesso.ListarModulos().Where(x => x.IDModulo != (int)Enums.Modulos.Funcionario).ToList();
            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListModulo.DataBind();

            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();

            DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();
            tr_modulo.Visible = Sessao.IdModulo == (int)Enums.Modulos.Consignante;
        }

        protected void cmbTipoProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopularDados();
        }

        protected void DropDownListConsignataria_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopularDados();
        }

        protected void DropDownListModulo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListModulo.SelectedValue == ((int)Enums.Modulos.Consignataria).ToString())
            {
                tr_consignataria.Visible = true;
                DropDownListConsignataria.Visible = true;
                cbConsignante.Checked = false;
                cbFuncionario.Checked = false;
                cbConsignante.Enabled = false;
                cbFuncionario.Enabled = false;
            }
            else
            {
                DropDownListConsignataria.Visible = false;
                tr_consignataria.Visible = false;
                cbConsignante.Enabled = true;
                cbFuncionario.Enabled = true;
            }

        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
           // DockManager.Controls.Add(new ASPxDockPanel() { HeaderText = "teste", OwnerZoneUID="zone", PanelUID="teste", ID="pnteste" });
           // PopularDados();
        }

        private void PopularDados()
        {
            int idmodulo = Convert.ToInt32(DropDownListModulo.SelectedValue);
            int idprodutogrupo = Convert.ToInt32(cmbTipoProduto.SelectedValue);
            int idempresa = (Sessao.IdModulo != (int)Enums.Modulos.Consignante ? Sessao.IdBanco : Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            FluxoAprovacao fluxo = FachadaFluxoAprovacao.ObtemFluxoAprovacao(idprodutogrupo);
            if (fluxo != null)
            {
                cbConsignante.Checked = fluxo.RequerAprovacaoConsignante;
                cbConsignataria.Checked = fluxo.RequerAprovacaoConsignataria;
                cbFuncionario.Checked = fluxo.RequerAprovacaoFuncionario;
            }
            else
            {
                cbConsignante.Checked = false;
                cbConsignataria.Checked = false;
                cbFuncionario.Checked = false;
            }

            if (idmodulo == (int)Enums.Modulos.Consignataria)
            {
                cbConsignante.Enabled = false;
                cbFuncionario.Enabled = false;

                FluxoAprovacaoEmpresa fluxoe = FachadaFluxoAprovacao.ObtemFluxoAprovacaoEmpresa(idprodutogrupo, idempresa);
                if (fluxoe != null)
                    cbConsignataria.Checked = fluxoe.RequerAprovacao;
                else
                    cbConsignataria.Checked = false;
            }
            //var fluxos = FachadaFluxoAprovacao.ListaFluxoAprovacao(idprodutogrupo, idempresa);

        }


    }
}