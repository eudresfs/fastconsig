using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using System.Linq;
using CP.FastConsig.Facade;
using System.Collections.Generic;
using CP.FastConsig.Util;
using CP.FastConsig.DAL;
using System.Web;
using System.IO;
using System.Net;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConciliacao : CustomUserControl
    {
        string fileName;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (ControleCarregado || EhPostBack)
            {
                PopulaGrids(false);
                return;
            }

            PopularCombos();

            if (DivResultado.Visible && !string.IsNullOrEmpty(ASPxTextAnoMes.Text))
            {
                PopularDados();
                PopulaGrids();
            }

            //string eventArgs = Request["__EVENTARGUMENT"];

            //if (!string.IsNullOrEmpty(eventArgs))
            //{
            //    if (eventArgs.Equals("exportar_click"))
            //    {
            //        EhPostBack = false;
            //        ExportarGrid(false);
            //    }
            //}

            //if (!EhPostBack)
            //  ExecutaScript();

            EhPostBack = true;
        }

        private void PopularCombos()
        {

            DropDownListSituacao.DataSource = FachadaConciliacao.ListarConciliacaoTipo().ToList();
            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();

            DropDownListConsignataria.DataBind();
            DropDownListSituacao.DataBind();

            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            DropDownListSituacao.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();
                //DropDownListConsignataria.Items.FindByValue(Sessao.IdBanco.ToString()).Selected = true;
                DropDownListConsignataria.Enabled = false;            
                if (!DivResultado.Visible)
                    gridAverbacaos.GroupBy(gridAverbacaos.Columns["ConciliacaoTipo.Nome"]);
            }
            else if (!DivResultado.Visible)
            {
                gridAverbacaos.GroupBy(gridAverbacaos.Columns["Empresa.Nome"]);
                gridAverbacaos.GroupBy(gridAverbacaos.Columns["ConciliacaoTipo.Nome"]);
            }

        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            PopularDados();
            PopulaGrids();            
        }

        private void PopularDados()
        {
            int idempresa = (Sessao.IdModulo == (int)Enums.Modulos.Consignataria || Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0  ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : 0);

            string anomes = Utilidades.ConverteAnoMes(ASPxTextAnoMes.Text);
            // Resumo Retorno Folha
            GridViewResumoFolha.DataSource = FachadaConciliacao.ListarConciliacaoResumoFolha(anomes,idempresa);
            GridViewResumoFolha.DataBind();

            // Resumo Conciliação
            GridViewResumoConciliacao.DataSource = FachadaConciliacao.ListarConciliacaoResumoConciliacao(anomes, idempresa);
            GridViewResumoConciliacao.DataBind();

            // Repasses
            GridViewRepasses.DataSource = FachadaConciliacao.ListarConciliacaoRepasses(anomes, idempresa).ToList();
            GridViewRepasses.DataBind();

            // Gráfico Conciliação
            PopulaGraficoConciliacao();
        }

        private void PopulaGrids(bool exibeMensagem = true)
        {
            int idempresa = (Sessao.IdModulo == (int)Enums.Modulos.Consignataria || Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0 ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : 0);

            string anomes = Utilidades.ConverteAnoMes(ASPxTextAnoMes.Text);

            // Resultado últimos 6 meses de conciliação
            List<ConciliacaoResultadoConciliacao> dados = FachadaConciliacao.ListarConciliacaoAnalise(anomes, idempresa, 6).ToList();

            bool bDados = dados.Count > 0;

            DivResultado.Visible = bDados;

            if (bDados)
            {
                ASPxPivotGridAnaliseUltimosMeses.DataSource = dados; 
                ASPxPivotGridAnaliseUltimosMeses.DataBind();

                // Detalhe Conciliacao                                  
                //PageMaster.getExportarGrid("gridAverbacaos");        
                gridAverbacaos.DataSource = FachadaConciliacao.ListarConciliacao(anomes, Convert.ToInt32(DropDownListConsignataria.SelectedValue), Convert.ToInt32(DropDownListSituacao.SelectedValue)).ToList();
                gridAverbacaos.DataBind();
            }
            else if(exibeMensagem)
            {
                PageMaster.ExibeMensagem("Não existem dados gerados para o mês solicitado. Verifique novamente em outro momento.");
                ASPxTextAnoMes.Focus();

            }

            
        }

        void ExportarGrid(bool saveAs)
        {

            //const string fileName = "Conciliação Detalhe";
            //string contentType = "application/ms-excel";

            switch (cmbTipoExportacao.SelectedIndex)
            {

                case 0:
                    fileName = "Conciliacao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".pdf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopulaGrids();
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WritePdf(s);

                    }
                    break;
                case 1:
                    fileName = "Conciliacao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopulaGrids();
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WriteXls(s);
                    }
                    break;
                case 2:
                    fileName = "Conciliacao_"+DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss")+".rtf";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopulaGrids();
                        ASPxGridViewExporter1.Landscape = true;
                        ASPxGridViewExporter1.DataBind();
                        ASPxGridViewExporter1.WriteRtf(s);
                    }
                    break;
                case 3:
                    fileName = "Conciliacao_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".txt";
                    ExecutaScript();
                    using (FileStream s = new FileStream(Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), fileName), FileMode.Create))
                    {
                        PopulaGrids();
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

        protected void Filtros_Click(object sender, EventArgs e)
        {
            divFiltros.Visible = !divFiltros.Visible;
        }

        protected void Analise6meses_Click(object sender, EventArgs e)
        {
            divAnalise6meses.Visible = !divAnalise6meses.Visible;
        }

        protected void ConciliacaoDetalhes_Click(object sender, EventArgs e)
        {
            ASPxRoundPanel6.Visible = !ASPxRoundPanel6.Visible;
        }

        private void PopulaGraficoConciliacao()
        {

            int idempresa = (Sessao.IdModulo == (int)Enums.Modulos.Consignataria ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : 0);

            string anomes = Utilidades.ConverteAnoMes(ASPxTextAnoMes.Text);

            var consulta = FachadaConciliacao.ListarConciliacaoResumoConciliacao(anomes, idempresa); // FachadaConciliacao.ListarConciliacaoAnalise

            var grupo = from c in consulta
                        group c by c.Descricao into g
                        select new { Descricao = g.Key, Valor = g.Sum(x => x.Valor) };

            decimal valorTotal = 0;
            
            if (consulta.Count() > 0)
                valorTotal = consulta.Sum(x => x.Valor ?? 0);

            if (valorTotal.Equals(0)) return;

            Dictionary<string, decimal> dados = grupo.ToDictionary(x => x.Descricao, x => (decimal)(Math.Round((x.Valor ?? 0 / valorTotal) * 100, 2)));

            WebUserControlChartPizzaConciliacao.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartPizzaConciliacao, ResourceMensagens.TituloGraficoConciliacao, ASPxTextAnoMes.Text, dados);       
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