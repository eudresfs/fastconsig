using System;
using System.Collections.Generic;
using System.Configuration;
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
using DevExpress.Web.ASPxUploadControl;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlCoeficientesEmprestimo : CustomUserControl
    {


        #region Constantes

        private const string ParametroNomeArquivo = "NomeArquivo";
        private const string ParametroIdEmpresaCoeficiente = "IdEmpresaCoeficiente";
        private const string NomeTabelaEmpresaCoeficienteDetalhe = "EmpresaCoeficienteDetalhe";
        private const string CaminhoDiretorioImportacaoCoeficientes = "{0}Arquivos\\Coeficientes\\{1}\\{2}";
        private const string CaminhoDiretorioDownloadCoeficientes = "{0}Arquivos\\Coeficientes\\Temp\\{1}.xlsx";
        private const string ScriptDownloadArquivo = "window.location.href = 'http://{0}/Arquivos/Coeficientes/Temp/{1}.xlsx'; return false;";
        private const string IdControleImageButtonExportar = "ImageButtonExportar";
        private const string TituloPlanilhaCoeficiente = "Coeficientes - {0}";
        private const string NomePlanilhaDefault = "Sheet1";
        private const string FormatoData = "dd-MM-yyyy";
        private const string FlagHttpHost = "HTTP_HOST";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack) return;

            EhPostBack = true;

            ConfiguraComponentes();

        }

        private void ConfiguraComponentes()
        {
            LimpaCampos();
            ConfiguraCoeficientes();
        }

        private void ConfiguraCoeficientes()
        {
            GridViewCoeficientes.DataSource = FachadaCoeficientesEmprestimo.ObtemCoeficientesEmpresa(Sessao.IdBanco).ToList();
            GridViewCoeficientes.DataBind();
        }

        private void LimpaCampos()
        {

            DateTime amanha = DateTime.Today.AddDays(1);

            ASPxDateEditInicioVigencia.MinDate = amanha;
            ASPxDateEditInicioVigencia.Date = amanha;

            TrUploadArquivoCoeficiente.Visible = false;

            ASPxSpinEditCarencia.Value = 0;
            ASPxSpinEditValidade.Value = 0;

            NomeArquivo = string.Empty;
            IdEmpresaCoeficiente = 0;

        }

        protected void ButtonSalvarCoeficiente_Click(object sender, EventArgs e)
        {

            bool existeCoeficienteValido = FachadaCoeficientesEmprestimo.ExisteCoeficienteValido(Sessao.IdBanco, ASPxDateEditInicioVigencia.Date, Convert.ToInt32(ASPxSpinEditValidade.Value));

            if (existeCoeficienteValido) PageMaster.ExibeMensagem(ResourceMensagens.MensagemCoeficienteJaCadastrado);

            IdEmpresaCoeficiente = FachadaCoeficientesEmprestimo.Salva(Convert.ToInt32(ASPxSpinEditCarencia.Value), Convert.ToInt32(ASPxSpinEditValidade.Value), Sessao.IdBanco, ASPxDateEditInicioVigencia.Date);

            TrUploadArquivoCoeficiente.Visible = true;

            ConfiguraCoeficientes();

        }
        protected void ASPxUploadControlArquivoCoeficiente_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            e.CallbackData = SavePostedFile(e.UploadedFile);
        }

        private string SavePostedFile(UploadedFile uploadedFile)
        {

            if (!uploadedFile.IsValid) return string.Empty;

            string nomeArquivo = string.Format("{0}{1}", string.Format("{0}_{1}_{2}{3}{4}_{5}_{6}_{7}", Sessao.UsuarioLogado.IDUsuario, Sessao.IdModulo, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), Path.GetExtension(uploadedFile.FileName));

            NomeArquivo = string.Format(CaminhoDiretorioImportacaoCoeficientes, Request.PhysicalApplicationPath, Sessao.PastaUpload, nomeArquivo);

            uploadedFile.SaveAs(NomeArquivo);

            ASPxUploadControlArquivoCoeficiente.Enabled = true;
            ASPxButtonUploadArquivoCoeficiente.Enabled = true;

            return NomeArquivo;

        }

        private string NomeArquivo
        {

            get
            {
                if (Session[ParametroNomeArquivo] == null) Session[ParametroNomeArquivo] = string.Empty;
                return (string)Session[ParametroNomeArquivo];
            }
            set
            {
                Session[ParametroNomeArquivo] = value;
            }

        }

        private int IdEmpresaCoeficiente
        {

            get
            {
                if (ViewState[ParametroIdEmpresaCoeficiente] == null) ViewState[ParametroIdEmpresaCoeficiente] = string.Empty;
                return (int)ViewState[ParametroIdEmpresaCoeficiente];
            }
            set
            {
                ViewState[ParametroIdEmpresaCoeficiente] = value;
            }

        }

        protected void ButtonProcessarArquivoCoeficiente_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(NomeArquivo))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarArquivoUpload);
                return;
            }

            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(NomeArquivo);

            DataTable dataTable = workbook.GetDataSet(SpreadsheetGear.Data.GetDataFlags.NoColumnHeaders).Tables[0];

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings[Sessao.NomeStringConexaoSemEntity].ToString());

            sqlBulkCopy.DestinationTableName = NomeTabelaEmpresaCoeficienteDetalhe;
            sqlBulkCopy.BulkCopyTimeout = 960;

            DataTable dataTableModificada = new DataTable();

            for (int i = 0; i <= dataTable.Columns.Count + 1; i++) dataTableModificada.Columns.Add();
            foreach (DataRow linha in dataTable.Rows) dataTableModificada.Rows.Add(null, IdEmpresaCoeficiente, Convert.ToInt32(linha.ItemArray[(int)PosicaoDadosArquivoCoeficiente.Prazo]), Convert.ToDecimal(linha.ItemArray[(int)PosicaoDadosArquivoCoeficiente.Coeficiente].ToString().Replace(".", ",")), Convert.ToDecimal(linha.ItemArray[(int)PosicaoDadosArquivoCoeficiente.Cet].ToString().Replace(".", ",")), Convert.ToInt32(linha.ItemArray[(int)PosicaoDadosArquivoCoeficiente.Dia]));

            using (sqlBulkCopy) sqlBulkCopy.WriteToServer(dataTableModificada);

            ConfiguraComponentes();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        private enum PosicaoDadosArquivoCoeficiente
        {

            Dia,
            Prazo,
            Coeficiente,
            Cet

        }

        protected void ImageButtonRemover_Click(object sender, EventArgs e)
        {

            if (IdEmpresaCoeficiente > 0)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFinalizarCadastroParaProsseguir);
                return;
            }

            FachadaCoeficientesEmprestimo.RemoveCoeficiente(Convert.ToInt32(((ImageButton)sender).CommandArgument));

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            ConfiguraCoeficientes();

        }

        protected void ImageButtonVer_Click(object sender, ImageClickEventArgs e)
        {

            GridViewDetalhesCoeficiente.DataSource = FachadaCoeficientesEmprestimo.ObtemCoeficientesDetalhesCoeficiente(Convert.ToInt32(((ImageButton)sender).CommandArgument)).ToList();
            GridViewDetalhesCoeficiente.DataBind();

            PopupControlDetalhesCoeficiente.ShowOnPageLoad = true;

        }

        private enum ColunasExportacao
        {
            Dia,
            Prazo,
            Coeficiente,
            CET
        }

        private string GeraArquivoParaDownload(EmpresaCoeficiente empresaCoeficiente)
        {

            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[NomePlanilhaDefault];
            SpreadsheetGear.IRange cells = worksheet.Cells;

            worksheet.Name = string.Format(TituloPlanilhaCoeficiente, empresaCoeficiente.Data.ToString(FormatoData));

            List<EmpresaCoeficienteDetalhe> detalhes = empresaCoeficiente.Detalhes.Where(x => x.Ativo.Equals(1)).OrderBy(x => x.Prazo).ToList();

            for (int linha = 0; linha < detalhes.Count; linha++)
            {

                cells[linha, (int)ColunasExportacao.Dia].Value = detalhes[linha].Dia;
                cells[linha, (int)ColunasExportacao.Prazo].Value = detalhes[linha].Prazo;
                cells[linha, (int)ColunasExportacao.Coeficiente].Value = detalhes[linha].Coeficiente;
                cells[linha, (int)ColunasExportacao.CET].Value = detalhes[linha].CET;

            }

            string nomeArquivo = Path.GetRandomFileName();

            workbook.SaveAs(string.Format(CaminhoDiretorioDownloadCoeficientes, Request.PhysicalApplicationPath, nomeArquivo), SpreadsheetGear.FileFormat.OpenXMLWorkbook);

            return nomeArquivo;

        }

        protected void GridViewCoeficientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            GridViewRow linha = e.Row;

            if (linha.RowType != DataControlRowType.DataRow) return;

            EmpresaCoeficiente empresaCoeficiente = (EmpresaCoeficiente) linha.DataItem;

            ImageButton imageButtonExportar = (ImageButton) linha.FindControl(IdControleImageButtonExportar);

            imageButtonExportar.OnClientClick = string.Format(ScriptDownloadArquivo, Request[FlagHttpHost], GeraArquivoParaDownload(empresaCoeficiente));

        }

    }

}