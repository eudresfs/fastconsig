using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlReajustarContratos : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (EhPostBack) return;

            ConfiguraComponentes();

            EhPostBack = true;

            if (!FachadaConsignatarias.existeVerbaEmpresa((int)Enums.ProdutoGrupo.Mensalidades, Sessao.IdBanco))
            {
                PageMaster.ExibeMensagem("Esta Empresa não tem o produto mensalidade definido. Operação Abortada.");
                PageMaster.FechaControleVoltaChamada();
            }

        }

        private void ConfiguraComponentes()
        {

            LabelDataAtual.Text = DateTime.Today.ToString("dd/MM/yyyy");
            DropDownListServico.DataSource = FachadaReajustarContratos.ListaProdutos();

            DropDownListServico.DataBind();
        }

        protected void ButtonAplicarReajuste_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxDescricao.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemCamposVazios);
                return;
            }

            System.Threading.Thread.Sleep(3000);

            ASPxRoundPanelAnaliseReajuste.Visible = true;
            LabelProduto.Text = DropDownListServico.SelectedValue;

            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", ResourceAuxiliar.ScriptRodapePagina, true);

        }

        protected void ButtonConfirmar_Click(object sender, EventArgs e)
        {
            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {

            ASPxRoundPanelAnaliseReajuste.Visible = false;
            TrResultado.Visible = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", ResourceAuxiliar.ScriptTopoPagina, true);

        }

        protected void ButtonDownloadCriticas_Click(object sender, EventArgs e)
        {
            TrResultado.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", ResourceAuxiliar.ScriptRodapePagina, true);
        }

    }

}