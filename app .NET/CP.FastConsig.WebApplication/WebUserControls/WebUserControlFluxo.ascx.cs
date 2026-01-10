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

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlFluxo : CustomUserControl
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
            }
            RegistrarBlockScript(this, GetScript, true);
        }
        
        protected void Salvar_Fluxo(object sender, EventArgs e)
        {
            var paineis = DockManager.Panels.OrderBy(x => x.VisibleIndex);
            for (int i = 0; i < paineis.Count(); i++)
            {
                
            }
        }

        private void PopulaCombos()
        {
            cmbTipoProduto.DataSource = FachadaAverbacoes.ObtemProdutosGrupo().ToList();
            cmbTipoProduto.DataBind();

            cmbTipoFluxo.DataSource = FachadaFluxo.ListaFluxoTipo().ToList();
            cmbTipoFluxo.DataBind();

            cmbSolicitacaoTipo.DataSource = FachadaFluxo.ListaSolitacaoTipo().ToList();
            cmbSolicitacaoTipo.DataBind();            
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
           // DockManager.Controls.Add(new ASPxDockPanel() { HeaderText = "teste", OwnerZoneUID="zone", PanelUID="teste", ID="pnteste" });
           // PopularDados();
        }

        private void PopularDados()
        {
            //int idmodulo = Convert.ToInt32(DropDownListModulo.SelectedValue);
            //int idprodutogrupo = Convert.ToInt32(cmbTipoProduto.SelectedValue);
            //int idempresa = (idmodulo != (int)Enums.Modulos.Consignante ? Sessao.IdBanco : Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            //var fluxos = FachadaFluxoAprovacao.ListaFluxoAprovacao(idprodutogrupo, idempresa);

        }
    }
}