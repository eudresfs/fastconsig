using System;
using System.IO;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Common;
using ImageResizer;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConfirmarQuitacao : CustomUserControl
    {

        #region Constantes

        private const string PathComprovantesVirtual = "~/Arquivos/Comprovantes/{0}/";
        private const string PathComprovantes = "Arquivos\\Comprovantes\\{0}\\";
        private const string ConfiguracaoImagens = "width=650&quality=90";
        private const string FormatoDataHora = "dd/MM/yyyy HH:mm";
        private const string TituloPagina = "Confirmar/Rejeitar Quitação";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            PopularDados();

            EhPostBack = true;

            ConfiguraTopo();

        }

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
        }

        private void PopularDados()
        {

            Averbacao con = FachadaConciliacao.ObtemAverbacao(Id ?? 0);

            LabelMatriculaFuncionario.Text = con.Funcionario.Matricula;
            LabelNomeFuncionario.Text = con.Funcionario.Pessoa.Nome;
            LabelCpfFuncionario.Text = con.Funcionario.Pessoa.CPFMascara;

            LabelNumeroAverbacao.Text = con.Numero;
            LabelConsignataria.Text = con.Empresa1.Nome;
            LabelPrazo.Text = con.Prazo == null ? "---" : con.Prazo.Value.ToString();
            LabelSituacaoAtual.Text = con.AverbacaoSituacao.Nome;
            LabelValorParcela.Text = string.Format("{0:N}",con.ValorParcela);

            EmpresaSolicitacaoQuitacao empresaSolicitacaoQuitacao = FachadaConfirmarQuitacao.ObtemSolicitacaoQuitacao(Id.Value);

            if (empresaSolicitacaoQuitacao == null) return;

            EmpresaSolicitacao es = FachadaInformarSaldoDevedor.ObtemSolicitacaoProcessadaOrigem(con.IDAverbacao);

            if (es == null) return;

            EmpresaSolicitacaoSaldoDevedor ess = FachadaInformarSaldoDevedor.ObtemSaldoDevedor(es.IDEmpresaSolicitacao);

            LabelInformadoEm.Text = es.DataAtendimento.HasValue ? es.DataAtendimento.Value.ToString(FormatoDataHora) : es.DataSolicitacao.ToString();
            LabelDataValidade.Text = ess == null ? string.Empty : ess.Validade.ToString();
            LabelFormaPagamento.Text = ess == null ? string.Empty : ess.TipoPagamento.Nome;
            LabelObservacao.Text = ess == null ? string.Empty : ess.Observacao;
            LabelSaldoAberto.Text = string.Format("{0:N}", FachadaInformarQuitacao.CalculaSaldoRestante(con.IDAverbacao));
            LabelSaldoDevedor.Text = ess == null ? string.Empty : (ess.Valor.HasValue ? ess.Valor.Value.ToString() : "0");

            string pathCompletoUpload = string.Format("{0}{1}{2}", Request.PhysicalApplicationPath, string.Format(PathComprovantes, Sessao.PastaUpload), empresaSolicitacaoQuitacao.Comprovante);
            string pathCompletoUploadVirtual = string.Format("{0}{1}", string.Format(PathComprovantesVirtual, Sessao.PastaUpload), empresaSolicitacaoQuitacao.Comprovante);

            if (File.Exists(pathCompletoUpload))
            {

                ButtonVisualizarComprovanteQuitacao.Visible = true;
                
                if (pathCompletoUpload.ToUpper().Contains(".PDF"))
                {

                    ShowPdfComprovante.Visible = true;
                    ImageComprovante.Visible = false;

                    ShowPdfComprovante.FilePath = pathCompletoUploadVirtual;

                }
                else
                {

                    ImageBuilder.Current.Build(pathCompletoUpload, pathCompletoUpload, new ResizeSettings(ConfiguracaoImagens));

                    ShowPdfComprovante.Visible = false;
                    ImageComprovante.Visible = true;

                    ImageComprovante.ImageUrl = pathCompletoUploadVirtual;

                }
            
            }

        }

        protected void ButtonConfirmarQuitacao_Click(object sender, EventArgs e)
        {

            Averbacao con = FachadaConciliacao.ObtemAverbacao(Id.Value);

            FachadaConfirmarQuitacao.ConfirmarQuitacao(Id.Value, con.IDConsignataria, TextBoxMotivo.Text, Sessao.UsuarioLogado.IDUsuario);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            PageMaster.Voltar();

        }

        protected void ButtonRejeitarQuitacao_Click(object sender, EventArgs e)
        {

            FachadaConfirmarQuitacao.RejeitarQuitacao(Id.Value, Sessao.IdBanco, Sessao.UsuarioLogado.IDUsuario);
            
            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            PageMaster.Voltar();

        }

    }

}