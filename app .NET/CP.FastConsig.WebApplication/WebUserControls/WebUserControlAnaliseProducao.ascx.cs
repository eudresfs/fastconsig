using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using System.Linq;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using System.IO;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAnaliseProducao : CustomUserControl
    {
        string fileName;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (DivResultado.Visible) PopularDados();

            PopularCombos();

            EhPostBack = true;

        }

        private void PopularCombos()
        {

            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();
                DropDownListConsignataria.Visible = false;
            }

        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            PopularDados();
            DivResultado.Visible = true;
        }

        private void PopularDados()
        {
            string mesinicio = Utilidades.ConverteAnoMes(ASPxTextAnoMesInicio.Text);
            string mesfim = Utilidades.ConverteAnoMes(ASPxTextBoxAnoMesFim.Text);

            var dados = FachadaAverbacoes.RelatorioAnaliseProducao(mesinicio, mesfim, Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            //ASPxPivotGridAnalise.OptionsData.AllowCrossGroupVariation = true;
            ASPxPivotGridAnalise.DataSource = dados;

        }

        protected void buttonSaveAs_Click(object sender, EventArgs e)
        {
            ExportarGrid(true);
        }

        protected void Filtros_Click(object sender, EventArgs e)
        {
            divFiltros.Visible = !divFiltros.Visible;
        }

        void ExportarGrid(bool saveAs)
        {

            //const string fileName = "Conciliação Detalhe";
            //string contentType = "application/ms-excel";

            switch (cmbTipoExportacao.SelectedIndex)
            {

                case 0:
                    fileName = "AnaliseProducao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".pdf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        ASPxPivotGridExporter1.DataBind();
                        ASPxPivotGridExporter1.ExportToPdf(s);
                    }
                    break;
                case 1:
                    fileName = "AnaliseProducao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        ASPxPivotGridExporter1.DataBind();
                        ASPxPivotGridExporter1.ExportToXls(s);
                    }
                    break;
                case 2:
                    fileName = "AnaliseProducao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".rtf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        ASPxPivotGridExporter1.DataBind();
                        ASPxPivotGridExporter1.ExportToRtf(s);
                    }
                    break;
                case 3:
                    fileName = "AnaliseProducao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".txt";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        ASPxPivotGridExporter1.DataBind();
                        ASPxPivotGridExporter1.ExportToCsv(s);
                    }
                    break;

            }
        }

        private bool bRegistrouScript
        {
            get
            {
                if (ViewState["RegistrouScript"] == null) ViewState["RegistrouScript"] = false;
                return (bool)ViewState["RegistrouScript"];
            }
            set
            {
                ViewState["RegistrouScript"] = value;
            }
        }

        private void ExecutaScript()
        {
            //RegistrarStartupScript(this, "		function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitializeRequest); } " +
            //"function InitializeRequest(sender, args) { "+
            //"if (sender._postBackSettings.sourceElement.id == 'DownloadFile') { "+
            //    "__doPostBack('Salvar', 'exportar_click'); " +
            //    "var iframe = document.createElement('iframe'); "+
            //    "iframe.src = 'DownloadFile.aspx';" +
            //    "iframe.style.display = 'none'; "+
            //    "document.body.appendChild(iframe); } } ");
            

            //RegistrarStartupScript(this, " var jabaixou = false; function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest); } " +
            //"function onEndRequest(sender, args) { " +
            //"if (sender._postBackSettings.sourceElement != null && !jabaixou && sender._postBackSettings.sourceElement.id == 'DownloadFile') { jabaixou = true; " +
            //    "if ($('iframe') != null) $('iframe').remove(); "+
            //    "var iframe = document.createElement('iframe'); " +
            //    "iframe.src = 'DownloadFile.aspx?arquivo=" + fileName + "'; " +
            //    "iframe.style.display = 'none'; " +
            //    "document.body.appendChild(iframe); jabaixou = false; } } ");

            //if (bRegistrouScript)
            //    return;

            RegistrarStartupScript(this, "function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest); } " +
            "function onEndRequest(sender, args) { " +
            "if (sender._postBackSettings.sourceElement != null && sender._postBackSettings.sourceElement.id == 'DownloadFile') { " +
                "if ($('iframe') != null) $('iframe').remove(); " +
                "var iframe = document.createElement('iframe'); " +
                "iframe.src = 'DownloadFile.aspx?arquivo=" + fileName + "'; " +
                "iframe.style.display = 'none'; " +
                "document.body.appendChild(iframe);} } ");
            bRegistrouScript = true;
        }
    }

}