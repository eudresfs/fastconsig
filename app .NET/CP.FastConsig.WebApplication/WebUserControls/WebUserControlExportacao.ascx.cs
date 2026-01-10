using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxUploadControl;
using CP.FastConsig.Util;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlExportacao : CustomUserControl
    {

        #region Constantes

        private const string IdControleAsPxButtonMenu = "ASPxButtonMenu";
        private const string PathImagens = "~/Imagens/{0}";
        private const string PathExportacao = "~/Arquivos/Exportacao/{0}";

        #endregion

        private static string NomeArquivo;

        private bool ArquivoValido
        {

            get
            {

                const string extensoes = ".txt";

                string extensao = Path.GetExtension(NomeArquivo);

                return extensoes.Contains(extensao);

            }

        }

        private void SelecionaBotao(ASPxButton botao)
        {

            const string flagSelecao = "Selecionado";

            if (botao.Image.Url.Contains(flagSelecao)) return;

            string imagemSelecao = string.Format("{0}{1}.png", Path.GetFileNameWithoutExtension(botao.Image.Url), flagSelecao);
            string bgImagemSelecao = string.Format("{0}{1}.png", Path.GetFileNameWithoutExtension(botao.BackgroundImage.ImageUrl), flagSelecao);

            botao.Image.Url = string.Format(PathImagens, imagemSelecao);
            botao.BackgroundImage.ImageUrl = string.Format(PathImagens, bgImagemSelecao);

            foreach (Control outroBotao in PanelBotoes.Controls)
            {
                if (outroBotao.ID == null || !outroBotao.ID.Contains(IdControleAsPxButtonMenu)) continue;

                ASPxButton outroBotaoAux = (ASPxButton)outroBotao;

                if (outroBotao == botao) continue;

                outroBotaoAux.Image.Url = string.Format(PathImagens, Path.GetFileName(outroBotaoAux.Image.Url).Replace(flagSelecao, string.Empty));
                outroBotaoAux.BackgroundImage.ImageUrl = string.Format(PathImagens, Path.GetFileName(outroBotaoAux.BackgroundImage.ImageUrl).Replace(flagSelecao, string.Empty));

            }

        }

        protected void ButtonVoltar_Click(object sender, EventArgs e)
        {
            PageMaster.Voltar();
        }

        protected void ASPxUploadControlFoto_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            if (Utilidades.ExisteItemVazio(TextBoxNome.Text, TextBoxTelefone.Text))
            {
                
                e.IsValid = false;

                if ((string.IsNullOrEmpty(TextBoxNome.Text.Trim())) && (TextBoxTelefone.Text.Trim() == "(  )     .")) e.ErrorText = ResourceMensagens.MensagemFaltamCamposNomeTelefone;
                else if (string.IsNullOrEmpty(TextBoxNome.Text.Trim())) e.ErrorText = ResourceMensagens.MensagemFaltaCampoNome;
                else if (string.IsNullOrEmpty(TextBoxTelefone.Text.Trim())) e.ErrorText = ResourceMensagens.MensagemFaltaCampoTelefone;

            }

            e.CallbackData = SavePostedFile(e.UploadedFile);

        }

        private string SavePostedFile(UploadedFile uploadedFile)
        {

            if (!uploadedFile.IsValid) return string.Empty;

            NomeArquivo = uploadedFile.FileName;

            if (!ArquivoValido) return string.Empty;

            string nomeArquivo = string.Format("{0}_{1}_{2}{3}{4}_{5}_{6}_{7}", Sessao.UsuarioLogado.IDUsuario, Sessao.IdModulo,
                                            DateTime.Now.Day,
                                            DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour,
                                            DateTime.Now.Minute, DateTime.Now.Second) +
                              Path.GetExtension(uploadedFile.FileName);

            NomeArquivo = string.Format("{0}Arquivos\\Exportacao\\{1}", Request.PhysicalApplicationPath, nomeArquivo);

            uploadedFile.SaveAs(NomeArquivo);

            ButtonExportar.Enabled = true;
            //ASPxButtonUploadFoto.Enabled = true;

            return NomeArquivo;

        }

        private int ModoExportacao
        {
            get
            {
                if (ViewState["ModoExportacao"] == null) ViewState["ModoExportacao"] = (int)TipoExportacao.DescontoFolha;
                return (int) ViewState["ModoExportacao"];
            }
            set
            {
                ViewState["ModoExportacao"] = value;
            }
        }

        private enum TipoExportacao
        {
            DescontoFolha
        }


        protected void ASPxButtonMenuDescontoFolha_Click(object sender, EventArgs e)
        {

            DivCamposImportacaoPersonalizada.Visible = false;

            ModoExportacao = (int)TipoExportacao.DescontoFolha;

            //FieldsetImportacao.Visible = true;

            ASPxButton botao = (ASPxButton)sender;

            SelecionaBotao(botao);

            DropDownListLayout.Visible = false;
            //LabelLayout.Visible = false;
            //ASPxButtonUploadFoto.Enabled = true;

            //LabelExplicacaoOpcao.Text = ResourceMensagens.ExplicacaoOpcaoDescontoFolha;

            //DropDownListLayout.Items.Clear();
            //DropDownListLayout.Items.Add(new ListItem("Descontados em Folha", "Descontados"));
            //DropDownListLayout.Items.Add(new ListItem("Não Descontados em Folha", "NaoDescontados"));

        }

        protected void ButtonConfigurar_Click(object sender, EventArgs e)
        {

            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(NomeArquivo);

            ArquivoExcel = workbook.GetDataSet(SpreadsheetGear.Data.GetDataFlags.FormattedText);
            DataTable dataTable = ArquivoExcel.Tables[0];

            DataTable dataTablePreview = SelectTopDataRow(dataTable, 10);

            GridViewPreviewImportacao.DataSource = dataTablePreview;
            GridViewPreviewImportacao.DataBind();

            var colunas = (from DataColumn coluna in dataTablePreview.Columns select new { NomeColuna = coluna.ColumnName }).ToList();

            RepeaterConfiguracaoImportacao.DataSource = colunas;
            RepeaterConfiguracaoImportacao.DataBind();

            DivConfiguracaoImportacao.Visible = true;
            //FieldsetImportacao.Visible = false;

        }

        private DataTable SelectTopDataRow(DataTable dt, int count)
        {

            DataTable dtn = dt.Clone();

            for (int i = 0; i < count; i++)  dtn.ImportRow(dt.Rows[i]);

            return dtn;

        }

        private DataSet ArquivoExcel
        {
            get
            {
                if (Session["ArquivoExcel"] == null) return null;
                return (DataSet) Session["ArquivoExcel"];
            }
            set
            {
                Session["ArquivoExcel"] = value;
            }
        }


        private void DownloadArquivo(string caminhoArquivo)
        {

            ////Response.ContentType = "text/plain";

            //string FilePath = MapPath(caminhoArquivo);

            ////Response.WriteFile(FilePath);

            ////Response.End();
            Page.Response.Redirect(caminhoArquivo, true);

        }

        protected void ButtonExportar_Click(object sender, EventArgs e)
        {

            if ((string.IsNullOrEmpty(TextBoxNome.Text.Trim())) && (TextBoxTelefone.Text.Trim() == "(  )     ."))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFaltamCamposNomeTelefone);
                return;
            }
            else if (string.IsNullOrEmpty(TextBoxNome.Text.Trim()))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFaltaCampoNome);
                return;
            }
            else if (string.IsNullOrEmpty(TextBoxTelefone.Text.Trim()))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFaltaCampoTelefone);
                return;
            }

            FachadaExportacao.ArquivoParaDescontoFolha(ASPxTextAnoMes.Text);

            NomeArquivo = string.Format("{0}Arquivos\\Exportacao\\{1}", Request.PhysicalApplicationPath, "exportacaoDescontoFolha.txt");

            List<Averbacao> averbacoes = FachadaExportacao.ExportaAberbacao().ToList();
            
            TextWriter tw = new StreamWriter(NomeArquivo);
            tw.WriteLine("MATRICULA;VERBA;COMPETENCIA;VALOR");

            foreach (var r in averbacoes)
            {
                tw.WriteLine( String.Format( "{0};{1};{2};{3}", r.Funcionario.Matricula, r.Produto.VerbaFolha, r.CompetenciaInicial, r.ValorParcela  ) );
            }

            tw.Close();

            DownloadArquivo("~/Arquivos/Exportacao/exportacaoDescontoFolha.txt");                     

        }

        private void LimpaCampos()
        {

            TextBoxNome.Text = string.Empty;
            TextBoxTelefone.Text = string.Empty;
            TextBoxObservacao.Text = string.Empty;
            TextBoxNomeTabela.Text = string.Empty;
            TextBoxCampoPersonalizado.Text = string.Empty;

            ListBoxCamposPersonalizado.Items.Clear();

            if(DropDownListLayout.Items.Count > 0) DropDownListLayout.SelectedIndex = 0;

        }

        protected void RepeaterConfiguracaoImportacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void ButtonAdicionarCampoPersonalizado_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxCampoPersonalizado.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemDigitarTituloCampoPersonalizado);
                return;
            }

            ListBoxCamposPersonalizado.Items.Add(Utilidades.RemoveCaracteresEspeciais(TextBoxCampoPersonalizado.Text));
            TextBoxCampoPersonalizado.Text = string.Empty;

        }

        protected void ButtonRemoverCampoPersonalizado_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(ListBoxCamposPersonalizado.SelectedValue))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarItemRemocao);
                return;
            }

            ListBoxCamposPersonalizado.Items.RemoveAt(ListBoxCamposPersonalizado.SelectedIndex);

        }

        protected void ButtonImportar_Click(object sender, EventArgs e)
        {
            return;
        }

    }

}