using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxUploadControl;
using CP.FastConsig.Util;
using SpreadsheetGear.Data;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlImportacao : CustomUserControl
    {

        #region Constantes

        private const string IdControleAsPxButtonMenu = "ASPxButtonMenu";
        private const string ExtensoesPermitidas = ".xls, .csv, .txt e .xlsx";
        private const string PathImagens = "~/Imagens/{0}";
        private const string PathPastaArquivoImportacao = "{0}Arquivos\\Importacao\\{1}\\{2}";
        private const string FlagSelecao = "Selecionado";
        private const string ImgPng = "{0}{1}.png";
        private const string MascaraTelefone = "(  )     .";
        private const string LabelFuncionariosAtivos = "Funcionários Ativos";
        private const string LabelFuncionariosCancelados = "Funcionários Cancelados";
        private const string LabelAtivos = "Ativos";
        private const string LabelCancelados = "Cancelados";
        private const string ParametroModoImportacao = "ModoImportacao";
        private const string LabelAverbacoesAtivas = "Averbações Ativas";
        private const string LabelAverbacoesCanceladas = "Averbações Canceladas";
        private const string LabelDescontadosFolha = "Descontados em Folha";
        private const string LabelNaoDescontadosFolha = "Não Descontados em Folha";
        private const string LabelDescontados = "Descontados";
        private const string LabelNaoDescontados = "NaoDescontados";
        private const string RegexData = @"\d{1,2}\/\d{1,2}\/\d{4}";
        private const string ParametroArquivoExcel = "ArquivoExcel";
        private const string ParametroNumeroCampos = "NumeroCampos";
        private const string IdControleDropDownListOpcaoColuna = "DropDownListOpcaoColuna";
        private const string IdControleLinkButtonFiltros = "LinkButtonFiltros";
        private const string IdControleWebUserControlImportacaoFiltrosLinha = "WebUserControlImportacaoFiltrosLinha";
        private const string IdControleTdFiltroLinha = "TdFiltroLinha";
        private const string ScriptJQueryExibirUmFiltro = "$('#{0}').dialog('open'); return false;";
        private const string LabelDadosColuna = "DadosColuna";
        private const string LabelNaoImportar = "-- Não Importar --";
        private const string RegraImportacaoTrocar = "Trocar";
        private const string RegraImportacaoDireita = "Direita";
        private const string RegraImportacaoEsquerda = "Esquerda";
        private const string RegraImportacaoMascara = "Mascara";
        private const string RegraImportacaoMaior = "Maior";
        private const string RegraImportacaoMenor = "Menor";
        private const string IdControleFiltroModal = "FiltroModal";
        private const string TagFimFiltro = "#FIM";
        private const string InicioColunaId = "Id";
        private const string ColunaMatricula = "MATRICULA";
        private const string ColunaNome = "NOME";
        private const string ColunaCpf = "CPF";
        private const string ColunaImportacao = "IdImportacao";
        private const string SufixoTabelaPersonalizada = "Personalizada";
        private const string ParametroNomeTabela = "NomeTabela";
        private const string ParametroNomeArquivo = "NomeArquivo";
        private const string ScriptLimpaSelecaoColunasImportacao = "$('td.TdColuna select').each(function(i){$(this).val('')});";
        private const string ScriptLimpaFiltros = "$('div[id*=DivFiltroColuna]').children().remove();";
        private const string ScriptCriacaoArray = "var scripts = new Array();";
        private const string ScriptSelecaoFiltros = "scripts[{0}]=\"{1}\";";
        private const string ScriptAplicacaoFiltros = "eval(scripts[this.value]);";
        private const string ScriptExibeMensagem = "ExibeMensagem('{0}');";
        private const string ScriptInibePostBack = "return false;";
        private const string AtributoOnChange = "onchange";
        private const string ScriptQuebraLabelsColunas = "var colunasArray = '{0}'.split(',');";
        private const string ScriptSelecaoColunas = "$('td.TdColuna select').each(function(i){$(this).val(colunasArray[i])});";
        private const string ScriptClonagemFormulario = "var formParaClonar{0} = $('p[id*=pFiltros]').first();";
        private const string ScriptAdicaoFormulario = "$('div[id*=DivFiltroColuna]:eq({0})').append(formParaClonar{0}.clone());";
        private const string ScriptSelecaoRegra = "$('div[id*=DivFiltroColuna]:eq({0})').first().children().last().find('select').val('{1}');";
        private const string ScriptConfiguracaoColuna = "$('div[id*=DivFiltroColuna]:eq({0})').first().children().last().find('input[id*=InputColunaFiltro]').val('{0}');";
        private const string ScriptConfiguracaoValorRegraA = "$('div[id*=DivFiltroColuna]:eq({0})').first().children().last().find('input[id*=InputFiltroTexto]').val('{1}');";
        private const string ScriptConfiguracaoValorRegraB = "$('div[id*=DivFiltroColuna]:eq({0})').first().children().last().find('input[id*=InputFiltroTroca]').val('{1}');";
        private const string AuxiliarFormatacaoData = "{0}/{1}/{2}";

        #endregion

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

        private bool ArquivoValido
        {

            get
            {
                return ExtensoesPermitidas.Contains(Path.GetExtension(NomeArquivo));
            }

        }

        private void SelecionaBotao(ASPxButton botao)
        {

            if (botao.Image.Url.Contains(FlagSelecao)) return;

            string imagemSelecao = string.Format(ImgPng, Path.GetFileNameWithoutExtension(botao.Image.Url), FlagSelecao);
            string bgImagemSelecao = string.Format(ImgPng, Path.GetFileNameWithoutExtension(botao.BackgroundImage.ImageUrl), FlagSelecao);

            botao.Image.Url = string.Format(PathImagens, imagemSelecao);
            botao.BackgroundImage.ImageUrl = string.Format(PathImagens, bgImagemSelecao);

            foreach (Control outroBotao in PanelBotoes.Controls)
            {
                if (outroBotao.ID == null || !outroBotao.ID.Contains(IdControleAsPxButtonMenu)) continue;

                ASPxButton outroBotaoAux = (ASPxButton)outroBotao;

                if (outroBotao == botao) continue;

                outroBotaoAux.Image.Url = string.Format(PathImagens, Path.GetFileName(outroBotaoAux.Image.Url).Replace(FlagSelecao, string.Empty));
                outroBotaoAux.BackgroundImage.ImageUrl = string.Format(PathImagens, Path.GetFileName(outroBotaoAux.BackgroundImage.ImageUrl).Replace(FlagSelecao, string.Empty));

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

                if ((string.IsNullOrEmpty(TextBoxNome.Text.Trim())) && (TextBoxTelefone.Text.Trim() == MascaraTelefone)) e.ErrorText = ResourceMensagens.MensagemFaltamCamposNomeTelefone;
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

            string nomeArquivo = string.Format("{0}{1}", string.Format("{0}_{1}_{2}{3}{4}_{5}_{6}_{7}", Sessao.UsuarioLogado.IDUsuario, Sessao.IdModulo, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), Path.GetExtension(uploadedFile.FileName));

            NomeArquivo = string.Format(PathPastaArquivoImportacao, Request.PhysicalApplicationPath, Sessao.PastaUpload, nomeArquivo);

            uploadedFile.SaveAs(NomeArquivo);

            ASPxUploadControlFoto.Enabled = true;
            ASPxButtonUploadFoto.Enabled = true;

            return NomeArquivo;

        }

        protected void ASPxButtonMenuFuncionarios_Click(object sender, EventArgs e)
        {

            LimpaCampos();

            DivCamposImportacaoPersonalizada.Visible = false;

            ModoImportacao = (int)TipoImportacao.Funcionarios;

            FieldsetImportacao.Visible = true;

            ASPxButton botao = (ASPxButton)sender;

            DivNomeLayout.Visible = true;

            SelecionaBotao(botao);

            DropDownListLayout.Visible = true;
            LabelLayout.Visible = true;

            LabelExplicacaoOpcao.Text = ResourceMensagens.ExplicacaoOpcaoFuncionario;

            DropDownListLayout.Items.Clear();

            DropDownListLayout.Items.Add(new ListItem(LabelFuncionariosAtivos, LabelAtivos));
            DropDownListLayout.Items.Add(new ListItem(LabelFuncionariosCancelados, LabelCancelados));

        }

        private int ModoImportacao
        {
            get
            {
                if (ViewState[ParametroModoImportacao] == null) ViewState[ParametroModoImportacao] = (int)TipoImportacao.Funcionarios;
                return (int)ViewState[ParametroModoImportacao];
            }
            set
            {
                ViewState[ParametroModoImportacao] = value;
            }
        }

        private string NomeTabela
        {
            get
            {
                if (ViewState[ParametroNomeTabela] == null) ViewState[ParametroNomeTabela] = string.Empty;
                return (string)ViewState[ParametroNomeTabela];
            }
            set
            {
                ViewState[ParametroNomeTabela] = value;
            }
        }

        private enum TipoImportacao
        {
            Contratos,
            Funcionarios,
            Personalizado,
            RetornoFolha
        }

        protected void ASPxButtonMenuContratos_Click(object sender, EventArgs e)
        {

            LimpaCampos();

            DivCamposImportacaoPersonalizada.Visible = false;

            ModoImportacao = (int)TipoImportacao.Contratos;

            FieldsetImportacao.Visible = true;

            ASPxButton botao = (ASPxButton)sender;

            DivNomeLayout.Visible = true;

            SelecionaBotao(botao);

            DropDownListLayout.Visible = true;
            LabelLayout.Visible = true;

            LabelExplicacaoOpcao.Text = ResourceMensagens.ExplicacaoOpcaoAverbacoes;

            DropDownListLayout.Items.Clear();

            DropDownListLayout.Items.Add(new ListItem(LabelAverbacoesAtivas, LabelAtivos));
            DropDownListLayout.Items.Add(new ListItem(LabelAverbacoesCanceladas, LabelCancelados));

        }

        protected void ASPxButtonMenuRetornoFolha_Click(object sender, EventArgs e)
        {

            LimpaCampos();

            DivCamposImportacaoPersonalizada.Visible = false;

            ModoImportacao = (int)TipoImportacao.RetornoFolha;

            FieldsetImportacao.Visible = true;

            ASPxButton botao = (ASPxButton)sender;

            DivNomeLayout.Visible = true;

            SelecionaBotao(botao);

            DropDownListLayout.Visible = true;
            LabelLayout.Visible = true;

            LabelExplicacaoOpcao.Text = ResourceMensagens.ExplicacaoOpcaoRetornoFolha;

            DropDownListLayout.Items.Clear();

            DropDownListLayout.Items.Add(new ListItem(LabelDescontadosFolha, LabelDescontados));
            DropDownListLayout.Items.Add(new ListItem(LabelNaoDescontadosFolha, LabelNaoDescontados));

        }

        protected void ASPxButtonMenuPersonalizado_Click(object sender, EventArgs e)
        {

            LimpaCampos();

            DivCamposImportacaoPersonalizada.Visible = true;

            TextBoxNomeLayout.Text = SufixoTabelaPersonalizada;

            DivNomeLayout.Visible = false;

            ModoImportacao = (int)TipoImportacao.Personalizado;

            FieldsetImportacao.Visible = true;

            ASPxButton botao = (ASPxButton)sender;

            SelecionaBotao(botao);

            DropDownListLayout.Visible = false;
            LabelLayout.Visible = false;

            LabelExplicacaoOpcao.Text = ResourceMensagens.ExplicacaoOpcaoPersonalizado;

        }

        protected void ButtonConfigurar_Click(object sender, EventArgs e)
        {

            LimpaScripts();

            if (string.IsNullOrEmpty(TextBoxNomeLayout.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemDigitarNomeLayout);
                return;
            }

            if (string.IsNullOrEmpty(NomeArquivo))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarArquivoUpload);
                return;
            }

            if (ModoImportacao == (int)TipoImportacao.Personalizado && (ListBoxCamposPersonalizado.Items.Count.Equals(0) || string.IsNullOrEmpty(TextBoxNomeTabela.Text)))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemIncluirUmCampoPersonalizadoComTituloTabela);
                return;
            }

            NomeTabela = ObtemTabelaImportacao();

            ButtonSalvarImportar.Visible = DivLayouts.Visible = !NomeTabela.EndsWith(SufixoTabelaPersonalizada);

            List<ImportacaoLayout> layoutsSalvos = FachadaImportacao.ObtemLayoutsSalvos(NomeTabela, Sessao.IdBanco);

            Dictionary<int, string> scriptsFiltrosParaExecutar = ConfiguraScriptsFiltrosExecutar(layoutsSalvos);

            DropDownListLayoutsSalvos.DataSource = layoutsSalvos;
            DropDownListLayoutsSalvos.DataBind();

            DropDownListLayoutsSalvos.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, null));

            StringBuilder scriptParaExecutar = new StringBuilder();

            scriptParaExecutar.Append(ScriptLimpaSelecaoColunasImportacao);
            scriptParaExecutar.Append(ScriptLimpaFiltros);
            scriptParaExecutar.Append(ScriptCriacaoArray);

            foreach (KeyValuePair<int, string> filtroScript in scriptsFiltrosParaExecutar) scriptParaExecutar.Append(string.Format(ScriptSelecaoFiltros, filtroScript.Key, filtroScript.Value));

            scriptParaExecutar.Append(ScriptAplicacaoFiltros);

            scriptParaExecutar.Append(string.Format(ScriptExibeMensagem, ResourceMensagens.MensagemFiltrosCarregadosSucesso));

            scriptParaExecutar.Append(ScriptInibePostBack);

            DropDownListLayoutsSalvos.Attributes[AtributoOnChange] = scriptParaExecutar.ToString();

            DropDownListLayoutsSalvos.SelectedIndex = 0;

            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(NomeArquivo);

            ArquivoExcel = CheckBoxImportarPrimeiraLinha.Checked ? workbook.GetDataSet(GetDataFlags.NoColumnHeaders | GetDataFlags.FormattedText) : workbook.GetDataSet(GetDataFlags.FormattedText);

            DataTable dataTable = ArquivoExcel.Tables[0];

            dataTable = ConsertaDatas(dataTable);

            DataTable dataTablePreview = SelectTopDataRow(dataTable, 10);

            GridViewPreviewImportacao.DataSource = dataTablePreview;
            GridViewPreviewImportacao.DataBind();

            var colunas = (from DataColumn coluna in dataTablePreview.Columns select new { NomeColuna = coluna.ColumnName }).ToList();

            NumeroCampos = colunas.Count;

            RepeaterConfiguracaoImportacao.DataSource = colunas;
            RepeaterConfiguracaoImportacao.DataBind();

            DivConfiguracaoImportacao.Visible = true;
            FieldsetImportacao.Visible = false;

        }

        private Dictionary<int, string> ConfiguraScriptsFiltrosExecutar(IEnumerable<ImportacaoLayout> layoutsSalvos)
        {

            Dictionary<int, string> scriptsFiltrosParaExecutar = new Dictionary<int, string>();

            foreach (ImportacaoLayout importacaoLayout in layoutsSalvos)
            {

                StringBuilder scriptParaExecutar = new StringBuilder();

                scriptParaExecutar.Append(string.Format(ScriptQuebraLabelsColunas, importacaoLayout.Colunas));
                scriptParaExecutar.Append(ScriptSelecaoColunas);

                List<FiltroImportacao> filtrosImportacao = ObtemFiltrosImportacao(importacaoLayout.Filtros);

                foreach (FiltroImportacao filtroImportacao in filtrosImportacao)
                {

                    scriptParaExecutar.Append(string.Format(ScriptClonagemFormulario, filtroImportacao.IndiceColuna));

                    scriptParaExecutar.Append(string.Format(ScriptAdicaoFormulario, filtroImportacao.IndiceColuna));
                    scriptParaExecutar.Append(string.Format(ScriptSelecaoRegra, filtroImportacao.IndiceColuna, filtroImportacao.Regra.NomeRegra));
                    scriptParaExecutar.Append(string.Format(ScriptConfiguracaoColuna, filtroImportacao.IndiceColuna));
                    scriptParaExecutar.Append(string.Format(ScriptConfiguracaoValorRegraA, filtroImportacao.IndiceColuna, filtroImportacao.Regra.ValorA));
                    scriptParaExecutar.Append(string.Format(ScriptConfiguracaoValorRegraB, filtroImportacao.IndiceColuna, filtroImportacao.Regra.ValorB));

                }

                scriptsFiltrosParaExecutar.Add(importacaoLayout.IdImportacaoLayout, scriptParaExecutar.ToString());

            }

            return scriptsFiltrosParaExecutar;

        }

        private DataTable ConsertaDatas(DataTable dt)
        {

            Regex regex = new Regex(RegexData);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                object[] dadosLinha = dt.Rows[i].ItemArray;

                bool mudaLinha = false;

                for (int j = 0; j < dadosLinha.Length; j++)
                {

                    string dado = dadosLinha[j].ToString();

                    DateTime data;

                    if (regex.IsMatch(dado) && DateTime.TryParse(dado, out data))
                    {

                        mudaLinha = true;

                        string[] partesData = dado.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        string dia = partesData[1];
                        string mes = partesData[0];
                        string ano = partesData[2];

                        dado = string.Format(AuxiliarFormatacaoData, dia, mes, ano);

                        dadosLinha[j] = dado;

                    }

                }

                if (mudaLinha) dt.Rows[i].ItemArray = dadosLinha;

            }

            return dt;

        }

        private DataTable SelectTopDataRow(DataTable dt, int count)
        {

            DataTable dtn = dt.Clone();

            if (dt.Rows.Count < count) count = dt.Rows.Count;

            for (int i = 0; i < count; i++) dtn.ImportRow(dt.Rows[i]);

            return dtn;

        }

        private DataSet ArquivoExcel
        {
            get
            {
                if (Session[ParametroArquivoExcel] == null) return null;
                return (DataSet)Session[ParametroArquivoExcel];
            }
            set
            {
                Session[ParametroArquivoExcel] = value;
            }
        }

        protected void ButtonImportar_Click(object sender, EventArgs e)
        {
            Importar();
        }

        private void Importar()
        {

            DataTable dataTable = ArquivoExcel.Tables[0];

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings[Sessao.NomeStringConexaoSemEntity].ToString());

            bool importar = false;

            foreach (RepeaterItem configuracaoColuna in RepeaterConfiguracaoImportacao.Items)
            {

                foreach (Control controle in configuracaoColuna.Controls)
                {

                    if (controle is DropDownList)
                    {

                        DropDownList dropDownListOpcaoColuna = (DropDownList)controle;

                        if (dropDownListOpcaoColuna.SelectedIndex.Equals(0)) continue;

                        DadosColuna dadosColuna = ObtemDadosColuna(dropDownListOpcaoColuna);

                        sqlBulkCopy.ColumnMappings.Add(dadosColuna.Indice, dropDownListOpcaoColuna.SelectedValue);

                        importar = true;

                    }

                }

            }

            if (importar)
            {

                int idImportacao = FachadaImportacao.SalvaDadosImportacao(Sessao.IdUsuario, NomeArquivo, CheckBoxImportarPrimeiraLinha.Checked, DropDownListLayout.SelectedValue, TextBoxNomeLayout.Text, TextBoxObservacao.Text, TextBoxTelefone.Text, Sessao.IdBanco);

                string filtros = HiddenFieldFiltros.Value;

                AplicaFiltros(dataTable, filtros);
                AdicionaIdImportacao(dataTable, idImportacao);

                sqlBulkCopy.ColumnMappings.Add(dataTable.Columns.Count - 1, ColunaImportacao);

                if (ModoImportacao == (int)TipoImportacao.Personalizado)
                {
                    List<string> colunas = (from SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings select columnMapping.DestinationColumn).ToList();
                    CriadorTabela.CriaTabela(NomeTabela, colunas);
                }
                else if (ModoImportacao == (int)TipoImportacao.Funcionarios)
                {
                    RemoveLinhasInvalidas(dataTable, sqlBulkCopy.ColumnMappings);
                }

                sqlBulkCopy.DestinationTableName = NomeTabela;
                sqlBulkCopy.BulkCopyTimeout = 960;

                sqlBulkCopy.WriteToServer(dataTable);

                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

                ArquivoExcel = null;

                DivConfiguracaoImportacao.Visible = false;
                FieldsetImportacao.Visible = true;

                NomeArquivo = string.Empty;

                LimpaCampos();

            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSemDadosParaImportacao);
            }

        }

        private void RemoveLinhasInvalidas(DataTable dataTable, SqlBulkCopyColumnMappingCollection mapaIndices)
        {

            List<DataRow> linhasParaRemover = new List<DataRow>();

            for (int posicaoColuna = 0; posicaoColuna < dataTable.Columns.Count; posicaoColuna++ )
            {

                if (mapaIndices.Count < posicaoColuna + 1) break;

                if (mapaIndices[posicaoColuna].DestinationColumn.ToUpper().Equals(ColunaNome) || mapaIndices[posicaoColuna].DestinationColumn.ToUpper().Equals(ColunaMatricula) || mapaIndices[posicaoColuna].DestinationColumn.ToUpper().Equals(ColunaCpf))
                {
                    foreach (DataRow linha in dataTable.Rows) if (string.IsNullOrEmpty(linha.ItemArray[posicaoColuna].ToString().Trim())) linhasParaRemover.Add(linha);
                }

                posicaoColuna++;

            }

            foreach (var linha in linhasParaRemover) linha.Delete();
            
        }

        private void AdicionaIdImportacao(DataTable dataTable, int idImportacao)
        {

            dataTable.Columns.Add(ColunaImportacao);

            foreach (DataRow linha in dataTable.Rows)
            {

                object[] dados = linha.ItemArray;

                dados[dataTable.Columns.Count - 1] = idImportacao;

                linha.ItemArray = dados;

            }

        }

        private void AplicaFiltros(DataTable dataTable, string filtros)
        {
            List<FiltroImportacao> filtrosImportacao = ObtemFiltrosImportacao(filtros);
            for (int i = 0; i < NumeroCampos; i++) foreach (DataRow linha in dataTable.Rows) linha.ItemArray = AplicaFiltro(linha.ItemArray.ToArray(), filtrosImportacao);
        }

        private List<FiltroImportacao> ObtemFiltrosImportacao(string filtros)
        {
            List<string> filtrosPorColuna = filtros.Split(new[] { TagFimFiltro }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return filtrosPorColuna.Select(t => new FiltroImportacao(t)).Where(x => !string.IsNullOrEmpty(x.Regra.ValorA)).ToList();
        }

        private object[] AplicaFiltro(object[] dadosLinha, List<FiltroImportacao> filtrosImportacao)
        {
            for (int i = 0; i < dadosLinha.Length; i++) dadosLinha[i] = AplicaRegra(dadosLinha[i], filtrosImportacao.Where(x => x.IndiceColuna.Equals(i)).ToList());
            return dadosLinha;
        }

        private object AplicaRegra(object valor, IEnumerable<FiltroImportacao> regras)
        {

            string novoValor = valor.ToString().ToUpper();

            try
            {

                foreach (RegraFiltroImportacao regraFiltroImportacao in regras.Select(x => x.Regra))
                {

                    if (string.IsNullOrEmpty(regraFiltroImportacao.ValorA)) return novoValor;

                    switch (regraFiltroImportacao.NomeRegra)
                    {

                        case (RegraImportacaoTrocar):

                            novoValor = novoValor.Replace(regraFiltroImportacao.ValorA, regraFiltroImportacao.ValorB);
                            break;

                        case (RegraImportacaoDireita):

                            novoValor = novoValor + regraFiltroImportacao.ValorA;
                            break;

                        case (RegraImportacaoEsquerda):

                            novoValor = regraFiltroImportacao.ValorA + novoValor;
                            break;

                        case (RegraImportacaoMascara):

                            novoValor = StringHelper.MaskString(novoValor, regraFiltroImportacao.ValorA);
                            break;

                        case (RegraImportacaoMaior):

                            novoValor = String.CompareOrdinal(novoValor, regraFiltroImportacao.ValorA) > 0 ? regraFiltroImportacao.ValorB : novoValor;
                            break;

                        case (RegraImportacaoMenor):

                            novoValor = String.CompareOrdinal(novoValor, regraFiltroImportacao.ValorA) < 0 ? regraFiltroImportacao.ValorB : novoValor;
                            break;

                    }

                }

            }
            catch (Exception ex)
            {
                return novoValor;
            }

            return novoValor;

        }

        private string ObtemTabelaImportacao()
        {

            switch (ModoImportacao)
            {

                case ((int)TipoImportacao.Funcionarios): return ResourceAuxiliar.TabelaImportacaoFuncionario;
                case ((int)TipoImportacao.Contratos): return ResourceAuxiliar.TabelaImportacaoContrato;
                case ((int)TipoImportacao.RetornoFolha): return ResourceAuxiliar.TabelaImportacaoRetornoFolha;

                case ((int)TipoImportacao.Personalizado): return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", Utilidades.RemoveCaracteresEspeciais(TextBoxNomeTabela.Text), Sessao.IdUsuario, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, SufixoTabelaPersonalizada);

                default: return ResourceAuxiliar.TabelaImportacaoFuncionario;

            }

        }

        private void LimpaCampos()
        {

            TextBoxNome.Text = string.Empty;
            TextBoxTelefone.Text = string.Empty;
            TextBoxObservacao.Text = string.Empty;
            TextBoxNomeTabela.Text = string.Empty;
            TextBoxCampoPersonalizado.Text = string.Empty;
            TextBoxNomeLayout.Text = string.Empty;

            CheckBoxImportarPrimeiraLinha.Checked = true;

            DivConfiguracaoImportacao.Visible = false;

            ListBoxCamposPersonalizado.Items.Clear();

            RepeaterConfiguracaoImportacao.DataSource = null;
            RepeaterConfiguracaoImportacao.DataBind();

            LimpaFiltros();

            if (DropDownListLayout.Items.Count > 0) DropDownListLayout.SelectedIndex = 0;

        }

        private int NumeroCampos
        {
            get
            {
                if (ViewState[ParametroNumeroCampos] == null) ViewState[ParametroNumeroCampos] = 0;
                return (int)ViewState[ParametroNumeroCampos];
            }
            set
            {
                ViewState[ParametroNumeroCampos] = value;
            }
        }

        protected void RepeaterConfiguracaoImportacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            RepeaterItem linha = e.Item;

            DropDownList dropDownListOpcaoColuna = (DropDownList)linha.FindControl(IdControleDropDownListOpcaoColuna);

            LinkButton linkButton = (LinkButton)linha.FindControl(IdControleLinkButtonFiltros);
            WebUserControlImportacaoFiltros webUserControlImportacaoFiltrosLinha = (WebUserControlImportacaoFiltros)linha.FindControl(IdControleWebUserControlImportacaoFiltrosLinha);
            HtmlTableCell tdFiltroLinha = (HtmlTableCell)linha.FindControl(IdControleTdFiltroLinha);
            Control divFiltroLinha = linha.FindControl(IdControleFiltroModal);

            if (NumeroCampos > 0) tdFiltroLinha.RowSpan = NumeroCampos;

            StringBuilder scriptParaExecutar = new StringBuilder();

            scriptParaExecutar.Append(string.Format(ScriptJQueryExibirUmFiltro, divFiltroLinha.ClientID));

            linkButton.OnClientClick = scriptParaExecutar.ToString();

            dynamic coluna = linha.DataItem;

            webUserControlImportacaoFiltrosLinha.ConfiguraCampoFiltro(linha.ItemIndex.ToString());

            dropDownListOpcaoColuna.Attributes.Add(LabelDadosColuna, string.Format("{0};{1}", coluna.NomeColuna, linha.ItemIndex));

            dropDownListOpcaoColuna.DataSource = ObtemCamposImportacao();
            dropDownListOpcaoColuna.DataBind();

        }

        private string[] ObtemCamposImportacao()
        {

            switch (ModoImportacao)
            {

                case ((int)TipoImportacao.Funcionarios): return ObtemCamposImportacao(typeof(FuncionarioImportacao));
                case ((int)TipoImportacao.Contratos): return ObtemCamposImportacao(typeof(AverbacaoImportacao));
                case ((int)TipoImportacao.RetornoFolha): return ObtemCamposImportacao(typeof(RetornoFolhaImportacao));

                case ((int)TipoImportacao.Personalizado):

                    List<string> opcoes = new List<string> { LabelNaoImportar };

                    opcoes.AddRange(from ListItem item in ListBoxCamposPersonalizado.Items select item.Value);

                    return opcoes.ToArray();

                default: return ObtemCamposImportacao(typeof(FuncionarioImportacao));

            }

        }

        private string[] ObtemCamposImportacao(Type objType)
        {

            List<string> propriedades = objType.GetProperties().Where(x => !x.Name.StartsWith(InicioColunaId)).OrderBy(x => x.Name).Select(x => x.Name).ToList();

            propriedades.Insert(0, LabelNaoImportar);

            return propriedades.ToArray();

        }

        private DadosColuna ObtemDadosColuna(DropDownList dropDownList)
        {
            string[] dados = dropDownList.Attributes[LabelDadosColuna].Split(new[] { ';' });
            return new DadosColuna { Indice = Convert.ToInt32(dados[1]), NomeColuna = dados[0] };
        }

        class DadosColuna
        {

            public int Indice { get; set; }

            public string NomeColuna { get; set; }

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

        protected void ButtonSalvarImportar_Click(object sender, EventArgs e)
        {
            SalvaLayout();
            Importar();
        }

        private void SalvaLayout()
        {

            string filtros = HiddenFieldFiltros.Value;
            string colunas = HiddenFieldColunas.Value;

            string nomeLayout = string.Format("{0} [{1}] [{2}]", TextBoxNomeLayout.Text, NomeTabela, DateTime.Now.ToString());

            int idUsuario = Sessao.IdUsuario;
            int idBanco = Sessao.IdBanco;

            FachadaImportacao.SalvaLayout(filtros, NomeTabela, nomeLayout, idUsuario, idBanco, colunas);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void ButtonRemoverLayout_Click(object sender, EventArgs e)
        {

            if (DropDownListLayoutsSalvos.SelectedIndex.Equals(0))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarItemRemocao);
                return;
            }

            FachadaImportacao.RemoveImportacaoLayout(Convert.ToInt32(DropDownListLayoutsSalvos.SelectedValue));

            List<ImportacaoLayout> layoutsSalvos = FachadaImportacao.ObtemLayoutsSalvos(NomeTabela, Sessao.IdBanco);

            DropDownListLayoutsSalvos.DataSource = layoutsSalvos;
            DropDownListLayoutsSalvos.DataBind();

            DropDownListLayoutsSalvos.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, null));

            DropDownListLayoutsSalvos.SelectedIndex = 0;

            LimpaFiltros();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        private void LimpaFiltros()
        {

            StringBuilder scriptParaExecutar = new StringBuilder();

            scriptParaExecutar.Append(ScriptLimpaSelecaoColunasImportacao);
            scriptParaExecutar.Append(ScriptLimpaFiltros);

            AdicionaScriptParaExecucao(scriptParaExecutar.ToString());

        }

    }

}