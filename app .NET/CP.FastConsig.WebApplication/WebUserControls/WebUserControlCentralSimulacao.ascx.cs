using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlCentralSimulacao2 : CustomUserControl
    {
        #region Constantes e Variáveis

        private const int indiceIdRecurso = 0;
        private const int indiceMatriculaCpf = 1;
        private int PrimeiroRadioButton = 0;
        private Funcionario funcionario;
        private static double margemDisponivel;
        private static double valorParcela;
        private static List<object> parametrosPagina;
        private static string matriculaCpf;
        private const string ValorZero = "0,00";
        private static List<Rank> ListaRank;
        #endregion

        protected List<object> ParametrosPagina
        {
            get
            {
                if (parametrosPagina == null) parametrosPagina = ParametrosConfiguracao;
                return parametrosPagina;
            }
            set { parametrosPagina = value; }
        }

        protected bool ValorParcelaDigitado
        {
            get { return ASPxTextBoxValorParcela.Text != ValorZero; }
        }

        protected int IdRecurso
        {
            get { return Convert.ToInt32(ParametrosPagina[indiceIdRecurso]); }
        }

        protected double ValorMargemDisponivel
        {
            get { return margemDisponivel; }
            set { margemDisponivel = value; }
        }

        protected double ValorParcela
        {
            get { return valorParcela; }
            set { valorParcela = value; }
        }

        protected Pessoa PessoaEncontrada
        {
            get
            {
                string matriculaCpf = MatriculaCpf;
                return FachadaCentralSimulacao.obtemPessoa(matriculaCpf);
            }
        }

        protected Funcionario FuncionarioEncontrado
        {
            get { return funcionario ?? PessoaEncontrada.Funcionario.First(); }
            set { funcionario = value; }
        }

        protected string MatriculaCpf
        {
            get
            {
                if (matriculaCpf == null) matriculaCpf = ParametrosPagina[indiceMatriculaCpf].ToString();
                return matriculaCpf;
            }
            set { matriculaCpf = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EhPostBack) return;

            ConfiguraPagina();

            EhPostBack = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.NomeArquivoScriptEnableCheckBoxes);
        }

        private void ConfiguraPagina()
        {
            MultiViewCentralSimulacao.ActiveViewIndex = 0;
            ParametrosPagina = ParametrosConfiguracao;
            MatriculaCpf = ParametrosPagina[indiceMatriculaCpf].ToString();
            ConfiguraDadosPessoais();
            ConfiguraGrids();
        }

        private void ConfiguraGrids()
        {
            List<Funcionario> funcionarios = FachadaCentralSimulacao.obtemFuncionariosPorPessoa(PessoaEncontrada.IDPessoa);
            if (PessoaEncontrada.CPF != MatriculaCpf || funcionarios.Count <= 1)
            {
                ASPxGridViewMatriculas.Visible = false;
                ASPxGridViewCentralSimulacaoContratosRefinanciar.Attributes.Add("style", "float:left; width:100%");
                string[] dados = { "", MatriculaCpf, ValorMargemDisponivel.ToString() };
                ConfiguraCamposVariaveis(dados);
            }
            else
            {
                ASPxGridViewMatriculas.Visible = true;
                ASPxGridViewCentralSimulacaoContratosRefinanciar.Attributes.Add("style", "float:left; width:50%");
                ASPxGridViewMatriculas.DataSource = funcionarios;
                ASPxGridViewMatriculas.DataBind();

                MarcaRadioButton(PrimeiroRadioButton);
            }

            ConfiguraGridContratosRefinanciar();
        }

        private void ConfiguraGridContratosRefinanciar()
        {
            ASPxGridViewCentralSimulacaoContratosRefinanciar.DataSource = FachadaCentralSimulacao.obtemAverbacoesAtivas(FuncionarioEncontrado.IDFuncionario);
            ASPxGridViewCentralSimulacaoContratosRefinanciar.DataBind();
        }

        private void MarcaRadioButton(int indice)
        {
            ASPxRadioButton radioButton = (ASPxRadioButton)ASPxGridViewMatriculas.FindRowCellTemplateControl(indice, null, "ASPxRadioButtonSelecionar");
            radioButton.Checked = true;
            string[] dados = radioButton.Attributes["Variaveis"].Split(';');
            ConfiguraCamposVariaveis(dados);
        }

        private void ConfiguraCamposVariaveis(string[] dados)
        {
            MatriculaCpf = dados[1];
            LabelMatricula.Text = string.Format("Matrícula: {0}", MatriculaCpf);
            ValorParcela = Convert.ToDouble(dados[2]);
            ValorMargemDisponivel = Convert.ToDouble(dados[2]);
            TextBoxMargemDisponivel.Text = Utilidades.RetornaStringValor(ValorMargemDisponivel);
            ASPxTextBoxValorParcela.Text = Utilidades.RetornaStringValor(ValorParcela);
        }

        private void ConfiguraDadosPessoais()
        {
            Funcionario func = FuncionarioEncontrado;
            List<GrupoMargem> gruposMargem = FachadaCentralSimulacao.ListarMargens(func.IDFuncionario).ToList();
            string margem = gruposMargem.Where(x => x.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos).Sum(x => x.MargemDisponivel).ToString();
            LabelNome.Text = string.Format("Nome: {0}", func.Pessoa.Nome);
            LabelCpf.Text = string.Format("Cpf: {0}", Utilidades.RetornaStringCpf(func.Pessoa.CPF));
            LabelRg.Text = string.Format("RG: {0}", func.Pessoa.RG);
            LabelMatricula.Text = string.Format("Matrícula: {0}", func.Matricula);
            LabelDataAdmissao.Text = string.Format("Data Admissão: {0}",Convert.ToDateTime(func.DataAdmissao).ToString("dd/MM/yyyy"));
            LabelDataNascimento.Text = string.Format("Data Nascimento: {0}", Convert.ToDateTime(func.Pessoa.DataNascimento).ToString("dd/MM/yyyy"));
            LabelRegime.Text = string.Format("Regime: {0}",func.FuncionarioRegime != null ? func.FuncionarioRegime.Nome : "---");
            LabelCategoria.Text = string.Format("Categoria: {0}",func.FuncionarioCategoria.Nome);
            LabelSituacao.Text = string.Format("Situação: {0}",func.NomeSituacao);
            string imagem = string.Format("{0}.jpg", func.IDFuncionario);
            DirectoryInfo diretorio = new DirectoryInfo(string.Format("{0}Imagens/WebCam", Request.PhysicalApplicationPath));
            FileInfo[] arquivos = diretorio.GetFiles("*");
            bool existe = false;
            foreach (var arquivo in arquivos)
            {
                if (arquivo.Name.ToUpper() == imagem.ToUpper()) existe = true;
            }
            if (!existe) imagem = "photo_person.png";
            ImageFuncionario.ImageUrl = string.Format("~/Imagens/WebCam/{0}", imagem);
            ValorParcela = Convert.ToDouble(margem);
            ValorMargemDisponivel = Convert.ToDouble(margem);
            TextBoxMargemDisponivel.Text = Utilidades.RetornaStringValor(ValorMargemDisponivel);
        }

        protected void ButtonSimular_Click(object sender, EventArgs e)
        {
            ASPxRoundPanelResultado.Visible = true;
            ConfiguraGridResultado();
        }

        private void ConfiguraGridResultado()
        {
            List<EmpresaCoeficienteDetalhe> coeficientes = FachadaCentralSimulacao.ObtemCoeficientesBanco(Sessao.IdBanco).OrderBy(x => x.Coeficiente).ToList();
            int ranking = 1;
            if (ValorParcelaDigitado)
            {
                decimal valorParcela = Convert.ToDecimal(ASPxTextBoxValorParcela.Text);
                ListaRank = coeficientes.Select(
                    x =>
                    new Rank
                    {
                        Prazo = x.Prazo,
                        ValorParcela = valorParcela,
                        Coeficiente = x.Coeficiente,
                        ValorAverbacao = valorParcela / x.Coeficiente,
                        Ranking = string.Format("{0}º", ranking++)
                    }).ToList();
            }
            else
            {
                decimal valorAverbacao = Convert.ToDecimal(ASPxTextBoxValorContrato.Text);
                ListaRank =
                    coeficientes.Select(
                        x =>
                        new Rank
                        {
                            Prazo = x.Prazo,
                            ValorParcela = valorAverbacao * x.Coeficiente,
                            ValorAverbacao = valorAverbacao,
                            Coeficiente = x.Coeficiente,
                            Ranking = string.Format("{0}º", ranking++)
                        }).ToList();
            }

            ASPxGridViewResultado.DataSource = ListaRank;
            ASPxGridViewResultado.DataBind();
        }

        protected void ASPxRadioButtonSelecionar_CheckedChanged(object sender, EventArgs e)
        {
            ASPxRadioButton radioButton = (ASPxRadioButton)sender;
            if (radioButton.Checked)
            {
                string[] dados = radioButton.Attributes["Variaveis"].Split(';');
                int index = Convert.ToInt32(dados[0]);

                dados[0] = (string)ASPxGridViewMatriculas.GetRowValues(index, "LabelMargem");
                ConfiguraCamposVariaveis(dados);

                for (int i = 0; i < ASPxGridViewMatriculas.VisibleRowCount; i++)
                {
                    ASPxRadioButton radioButton2 = ASPxGridViewMatriculas.FindRowCellTemplateControl(i, null, "ASPxRadioButtonSelecionar") as ASPxRadioButton;
                    if (radioButton == radioButton2) continue;

                    radioButton2.Checked = false;
                }

                ConfiguraGridContratosRefinanciar();
            }
        }

        protected void ASPxGridViewMatriculas_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                Label margemEmprestimo = ASPxGridViewMatriculas.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelMargem") as Label;
                ASPxRadioButton radioButton = ASPxGridViewMatriculas.FindRowCellTemplateControl(e.VisibleIndex, null, "ASPxRadioButtonSelecionar") as ASPxRadioButton;

                string matricula = (string)ASPxGridViewMatriculas.GetRowValues(e.VisibleIndex, "Matricula");

                Funcionario func = FachadaCentralSimulacao.obtemFuncionario(matricula);
                List<GrupoMargem> gruposMargem = FachadaCentralSimulacao.ListarMargens(func.IDFuncionario).ToList();
                string margem = gruposMargem.Where(x => x.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos).Sum(x => x.MargemDisponivel).ToString();

                margemEmprestimo.Text = Utilidades.RetornaStringValor(Convert.ToDouble(margem));
                radioButton.Attributes["Variaveis"] = string.Format("{0};{1};{2}", e.VisibleIndex, matricula, margem);
            }
        }

        protected void ASPxGridViewCentralSimulacaoContratosRefinanciar_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                Label parcela = ASPxGridViewCentralSimulacaoContratosRefinanciar.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelParcela") as Label;
                Label parcelasRestantes = ASPxGridViewCentralSimulacaoContratosRefinanciar.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelParcelasRestantes") as Label;
                Label saldoAberto = ASPxGridViewCentralSimulacaoContratosRefinanciar.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelSaldoAberto") as Label;

                int idAverbacao = Convert.ToInt32(e.KeyValue);

                Averbacao averbacao = FachadaCentralSimulacao.obtemAverbacao(idAverbacao);
                parcela.Text = Utilidades.RetornaStringValor(Convert.ToDouble(averbacao.ValorParcela));
                parcelasRestantes.Text = FachadaCentralSimulacao.obtemParcelasRestantes(idAverbacao).ToString();
                saldoAberto.Text =
                    Utilidades.RetornaStringValor(Convert.ToDouble(FachadaCentralSimulacao.obtemSaldoAberto(idAverbacao)));
            }
        }

        protected void ASPxGridViewResultado_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string ranking = e.KeyValue.ToString();
                if (ListaRank == null) return;
                Rank rank = ListaRank.Where(x => x.Ranking == ranking).FirstOrDefault();

                Label valorContrato = ASPxGridViewResultado.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelValorAverbacao") as Label;
                Label valorParcela = ASPxGridViewResultado.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelValorParcela") as Label;

                valorContrato.Text = Utilidades.RetornaStringValor(Convert.ToDouble(rank.ValorAverbacao));
                valorParcela.Text = Utilidades.RetornaStringValor(Convert.ToDouble(rank.ValorParcela));
            }
        }

        protected void ASPxCheckBoxSelecionar_CheckedChanged(object sender, EventArgs e)
        {
            ValorParcela = Convert.ToDouble(ASPxTextBoxValorParcela.Text);
            ASPxCheckBox checkBox = (ASPxCheckBox)sender;
            double valor = Convert.ToDouble(checkBox.ValueChecked);
            if (checkBox.Checked) ValorParcela += valor;
            else ValorParcela -= valor;

            ASPxTextBoxValorParcela.Text = Utilidades.RetornaStringValor(ValorParcela);
            ASPxTextBoxValorContrato.Text = ValorZero;

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Enable", "disableChecksCetralSimulacao()", true);

        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlDashBoardConsignataria);
        }

        protected void ButtonImprimirOrcamento_Click(object sender, EventArgs e)
        {
            MultiViewCentralSimulacao.ActiveViewIndex = 1;
            ConfiguraResultadoDadosFuncionario();
        }

        private void ConfiguraResultadoDadosFuncionario()
        {
            Funcionario func = FuncionarioEncontrado;
            ImageFuncionarioResutado.ImageUrl = ImageFuncionario.ImageUrl;
            LabelNomeResultado.Text = string.Format("Nome: {0}", func.Pessoa.Nome);
            LabelCpfResultado.Text = string.Format("Cpf: {0}", Utilidades.RetornaStringCpf(func.Pessoa.CPF));
            LabelMatriculaResultado.Text = string.Format("Matrícula: {0}", func.Matricula);
            LabelTelefoneResultado.Text = string.Format("Telefone: {0}", func.Pessoa.Celular);
            LabelLocalTrabalhoResultado.Text = string.Format("Local de Trabalho: {0}", func.NomeLocalFolha);
            LabelRegimeResultado.Text = string.Format("Regime: {0}", func.FuncionarioRegime == null ? "---" : func.FuncionarioRegime.Nome);
            LabelNomeFuncionario.Text = func.Pessoa.Nome;
            LabelDataAdmissaoResultado.Text = string.Format("Data Admissão: {0}", Convert.ToDateTime(func.DataAdmissao).ToString("dd/MM/yyyy"));
            LabelDataNascimentoResultado.Text = string.Format("Data Nascimento: {0}", Convert.ToDateTime(func.Pessoa.DataNascimento).ToString("dd/MM/yyyy"));
            LabelCategoriaResultado.Text = string.Format("Categoria: {0}", func.FuncionarioCategoria.Nome);

            LabelLugarData.Text = string.Format("São Paulo, {0} de {1} de {2}", DateTime.Now.Day,
                                                Utilidades.RetornaStringMes(DateTime.Now.Month), DateTime.Now.Year);

            List<string> Prazos = new List<string>();

            for (int i = 0; i < ASPxGridViewResultado.VisibleRowCount; i++)
            {
                ASPxCheckBox checkBox = ASPxGridViewResultado.FindRowCellTemplateControl(i, null, "ASPxCheckBoxSelecionar") as ASPxCheckBox;
                string prazo = checkBox.ValueChecked.ToString();
                if (checkBox.Checked) Prazos.Add(prazo);
            }

            List<Rank> source = new List<Rank>();
            foreach (Rank rank in ListaRank)
            {
                if (Prazos.Contains(rank.Prazo.ToString())) source.Add(rank);
            }

            ASPxGridViewResultadoOrcamento.DataSource = source;
            ASPxGridViewResultadoOrcamento.DataBind();

            if (ValorParcela > ValorMargemDisponivel)
                ASPxGridViewResultadoOrcamento.Columns[0].Caption = string.Format("O extrato se refere ao Refinanciamento da parcela {0}",
                                                     Utilidades.RetornaStringValor(ValorParcela - ValorMargemDisponivel));
            else ASPxGridViewResultadoOrcamento.Columns[0].Caption = string.Empty;
        }

        protected void LinkButtonAverbar_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string[] dados = button.CommandArgument.Split(';');
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAverbacao, 0, dados[0], dados[1], dados[2]);
        }

        protected void ASPxGridViewResultadoOrcamento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string ranking = e.KeyValue.ToString();
                if (ListaRank == null) return;
                Rank rank = ListaRank.Where(x => x.Ranking == ranking).FirstOrDefault();
                Label valorContrato = ASPxGridViewResultadoOrcamento.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelValorAverbacao") as Label;

                Label valorParcela = ASPxGridViewResultadoOrcamento.FindRowCellTemplateControl(e.VisibleIndex, null, "LabelValorParcela") as Label;

                valorContrato.Text = Utilidades.RetornaStringValor(Convert.ToDouble(rank.ValorAverbacao));
                valorParcela.Text = Utilidades.RetornaStringValor(Convert.ToDouble(rank.ValorParcela));

            }
        }

        protected void ButtonCancelar2_Click(object sender, EventArgs e)
        {
            MultiViewCentralSimulacao.ActiveViewIndex = 0;
        }

        protected void ButtonImprimir_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dicionario = new Dictionary<string, string>();
            dicionario.Add("#id", "divImpressao");
            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.NomeArquivoScriptImpressao, dicionario);
        }

        protected void ASPxCheckBoxSelecionarPrazo_Click(object sender, EventArgs e)
        {
            //ASPxCheckBox checkBox = (ASPxCheckBox)sender;
            //string prazo = checkBox.ValueChecked.ToString();
            //if (checkBox.Checked) Prazos.Add(prazo);
            //else Prazos.Remove(prazo);
        }
    }

    class Rank
    {

        public string Ranking { get; set; }
        public string Banco { get; set; }
        public string Logo { get; set; }
        public string Competencia { get; set; }

        public int Prazo { get; set; }

        public decimal ValorParcela { get; set; }
        public decimal ValorAverbacao { get; set; }
        public decimal Coeficiente { get; set; }

    }
}