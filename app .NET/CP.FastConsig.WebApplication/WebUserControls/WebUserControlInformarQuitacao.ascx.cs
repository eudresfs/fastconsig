using System;
using System.IO;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxUploadControl;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlInformarQuitacao : CustomUserControl
    {

        #region Constantes

        private const string PathComprovantes = "Arquivos\\Comprovantes\\{0}\\";
        private const string TituloPagina = "Informar Quitação";
        private const string ConteudoItemSemDado = "---";
        private const string FormatoDataHora = "dd/MM/yyyy";
        private const string ParametroProcessouArquivoInformacaoQuitacao = "ProcessouArquivoInformacaoQuitacao";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;
            
            EhPostBack = true;

            PopularDados();
            ConfiguraTopo();

        }
        
        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
        }
        
        private void PopularDados()
        {

            Averbacao con = FachadaConciliacao.ObtemAverbacao(Id ?? 0);

            if (con == null) return;

            LabelMatriculaFuncionario.Text = con.Funcionario.Matricula;
            LabelNomeFuncionario.Text = con.Funcionario.Pessoa.Nome;
            LabelCpfFuncionario.Text = Utilidades.MascaraCPF(con.Funcionario.Pessoa.CPF);

            LabelNumeroAverbacao.Text = con.Numero;
            LabelConsignataria.Text = con.Empresa1.Nome;
            LabelPrazo.Text = con.Prazo == null ? ConteudoItemSemDado : con.Prazo.Value.ToString();
            LabelSituacaoAtual.Text = con.AverbacaoSituacao.Nome;
            LabelValorParcela.Text = string.Format("{0:N}",con.ValorParcela);

            ASPxDateEditDataQuitacao.Date = DateTime.Today;

            DropDownListFormaPagamento.DataSource = FachadaInformarQuitacao.ObtemFormasPagamento().ToList();
            DropDownListFormaPagamento.DataBind();

            EmpresaSolicitacao es = FachadaInformarSaldoDevedor.ObtemSolicitacaoProcessadaOrigem(con.IDAverbacao);

            if (es == null) return;

            EmpresaSolicitacaoSaldoDevedor ess = FachadaInformarSaldoDevedor.ObtemSaldoDevedor(es.IDEmpresaSolicitacao);

            if (ess == null) return;
           
            LabelInformadoEm.Text = es.DataSolicitacao.ToString();
            LabelDataValidade.Text = ess.Validade == null ? ConteudoItemSemDado : ess.Validade.Value.ToString(FormatoDataHora);
            LabelFormaPagamento.Text = ess.TipoPagamento.Nome;
            LabelObservacao.Text = ess.Observacao;
            LabelSaldoAberto.Text = string.Format("{0:N}",FachadaInformarQuitacao.CalculaSaldoRestante(con.IDAverbacao));
            LabelSaldoDevedor.Text = ess.Valor.HasValue ? string.Format("{0:N}",ess.Valor.Value) : "0";
            LabelFavorecidoCredito.Text = ess.NomeFavorecido;
            LabelContaCredito.Text = ess.ContaCredito;
            LabelAgenciaCredito.Text = ess.Agencia;
            LabelBancoCredito.Text = ess.Banco;

            if(ess.IDTipoPagamento != null) DropDownListFormaPagamento.SelectedValue = ess.IDTipoPagamento.Value.ToString();

        }

        private void ValidaEntrada(FileUploadCompleteEventArgs e)
        {

            if (Utilidades.ExisteItemVazio(ASPxDateEditDataQuitacao.Text, ASPxTextBoxValor.Text))
            {

                e.IsValid = false;
                e.ErrorText = ResourceMensagens.MensagemCamposVazios;

                return;

            }

            if (Convert.ToDecimal(ASPxTextBoxValor.Text).Equals(0))
            {

                e.IsValid = false;
                e.ErrorText = ResourceMensagens.MensagemDigitarValorValido;

                return;

            }

            if (!e.IsValid)
            {

                e.IsValid = false;
                e.ErrorText = ResourceMensagens.MensagemTiposValidosParaComprovante;

                return;

            }

            if (string.IsNullOrEmpty(e.UploadedFile.FileName))
            {

                e.IsValid = false;
                e.ErrorText = ResourceMensagens.MensagemAnexarComprovante;

                return;

            }

        }

        protected void ASPxUploadComprovante_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            ValidaEntrada(e);
            if(e.IsValid) SavePostedFile(e.UploadedFile);
        }

        private void SavePostedFile(UploadedFile uploadedFile)
        {

            string pathCompletoUpload = string.Format("{0}{1}{2}{3}", Request.PhysicalApplicationPath, string.Format(PathComprovantes, Sessao.PastaUpload), Id ?? 0, Path.GetExtension(uploadedFile.FileName));

            uploadedFile.SaveAs(pathCompletoUpload);

            FachadaInformarQuitacao.SalvaInformacaoQuitacao(ASPxDateEditDataQuitacao.Date, Convert.ToDecimal(ASPxTextBoxValor.Text), Convert.ToInt32(DropDownListFormaPagamento.SelectedValue), TextBoxObservacao.Text, Id.Value, Path.GetFileName(pathCompletoUpload));
            
        }
        
    }

}