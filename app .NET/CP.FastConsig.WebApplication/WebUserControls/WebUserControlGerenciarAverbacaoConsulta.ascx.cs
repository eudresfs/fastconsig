using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxPopupControl;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlGerenciarAverbacaoConsulta : CustomUserControl
    {
        private const string ParametroIdAverbacao = "IDAverbacao";
        private const string TituloPagina = "Gerenciar Averbações";
        private const string ParametroIdTermoParaImpressao = "IdTermoParaImpressao";
        private const string ParametroProcessouTermo = "ProcessouTermo";

        private int IdAverbacao
        {
            get
            {
                if (ViewState[ParametroIdAverbacao] == null) ViewState[ParametroIdAverbacao] = 0;
                return (int)ViewState[ParametroIdAverbacao];
            }
            set
            {
                ViewState[ParametroIdAverbacao] = value;
            }
        }

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
            PageMaster.SubTitulo = "Consulta";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((ControleCarregado) && (EhPostBack) && (IdAverbacao > 0))
            {
                Averbacao averbacao = FachadaConciliacao.ObtemAverbacao(IdAverbacao);
                if (averbacao != null)
                {
                    txtSituacaoAverbacao.Text = averbacao.AverbacaoSituacao.Nome;
                    //HabilitaBotoes(averbacao);
                    PopulaDados(IdAverbacao);
                    //PopularGrids(averbacao);
                }
            } 
            
            if (ControleCarregado) return;

            if (!EhPostBack)
            {
                EhPostBack = true;
                IdAverbacao = Id.Value;

                PopulaDados(IdAverbacao);

                AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapseGA);
                AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapseGA2);
            }
            else
            {
                if (IdAverbacao > 0)
                {
                    Averbacao averbacao = FachadaConciliacao.ObtemAverbacao(IdAverbacao);
                    if (averbacao != null)
                    {
                        txtSituacaoAverbacao.Text = averbacao.AverbacaoSituacao.Nome;
                        //HabilitaBotoes(averbacao);
                        PopulaDados(IdAverbacao);
                        //PopularGrids(averbacao);
                    }
                }
            }
        }

        private void HabilitaBotoes(Averbacao a)
        {
            bool bConsignatariaOUAgente = (Sessao.IdModulo == (int)Enums.Modulos.Consignataria) || (Sessao.IdModulo == (int)Enums.Modulos.Agente);

            ASPxButtonSuspenderBloquear.Visible = !bConsignatariaOUAgente && (bool)a.AverbacaoSituacao.ParaDescontoFolha;

            ASPxButtonAtivar.Visible = (a.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Suspenso_MargemLivre) || (a.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Bloqueado_MargemRetida);

            ASPxButtonLiquidar.Visible = bConsignatariaOUAgente && (a.AverbacaoSituacao.Liquidavel == true);

            ASPxButtonCancelar.Visible = bConsignatariaOUAgente && (a.AverbacaoSituacao.Cancelavel == true);

            ASPxButtonReimprimirTermo.Visible = a.AverbacaoSituacao.DeduzMargem;

            ASPxButtonSolicitarSaldoDevedor.Visible = (Sessao.IdModulo == (int)Enums.Modulos.Funcionario) && (a.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);

            ASPxButtonAprovar.Visible = FachadaGerenciarAverbacao.ExisteSolicitacaoPendente(a.IDAverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.AprovarAverbacoes);
            ASPxButtonDesaprovar.Visible = ASPxButtonAprovar.Visible;

            ASPxButtonInformarSaldoDevedor.Visible = bConsignatariaOUAgente && (FachadaGerenciarAverbacao.ExisteSolicitacaoPendente(a.IDAverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos)); // (a.Empresa1.IDEmpresaTipo == (int)Enums.EmpresaTipo.Banco) && (a.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);

            ASPxButtonInformarQuitacao.Visible = bConsignatariaOUAgente && (FachadaGerenciarAverbacao.ExisteSolicitacaoPendente(a.IDAverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.ConfirmarRejeitarQuitacao));

            ASPxButtonConcluirCompra.Visible = bConsignatariaOUAgente && (FachadaGerenciarAverbacao.ExisteSolicitacaoPendente(a.IDAverbacao, a.IDConsignataria, (int)Enums.SolicitacaoTipo.ConcluirCompradeDivida));

            ASPxButtonSuspenderBloquearEmbaixo.Visible = ASPxButtonSuspenderBloquear.Visible;
            ASPxButtonAtivarEmbaixo.Visible = ASPxButtonAtivar.Visible;
            ASPxButtonLiquidarEmbaixo.Visible = ASPxButtonLiquidar.Visible;
            ASPxButtonCancelarEmbaixo.Visible = ASPxButtonCancelar.Visible;
            ASPxButtonSolicitarSaldoDevedorEmbaixo.Visible = ASPxButtonSolicitarSaldoDevedor.Visible;
            ASPxButtonAprovarEmbaixo.Visible = ASPxButtonAprovar.Visible;
            ASPxButtonDesaprovarEmbaixo.Visible = ASPxButtonDesaprovar.Visible;
            ASPxButtonInformarSaldoDevedorEmbaixo.Visible = ASPxButtonInformarSaldoDevedor.Visible;
            ASPxButtonInformarQuitacaoEmbaixo.Visible = ASPxButtonInformarQuitacao.Visible;
            ASPxButtonConcluirCompraEmbaixo.Visible = ASPxButtonConcluirCompra.Visible;
        }

        private void PopulaDados(int id)
        {
            if (id == 0) return;

            Averbacao con = FachadaConciliacao.ObtemAverbacao(id);

            HabilitaBotoes(con);

            // Dados Pessoais
            txtNome.Text = con.Funcionario.Pessoa.Nome;
            txtCPF.Text = Utilidades.RetornaStringCpf(con.Funcionario.Pessoa.CPF);
            txtRG.Text = con.Funcionario.Pessoa.RG.Replace(',', '.');
            txtEndereco.Text = con.Funcionario.Pessoa.Endereco;
            txtDataNasc.Text = con.Funcionario.Pessoa.DataNascimento == null ? "---" : con.Funcionario.Pessoa.DataNascimento.Value.ToString("dd/MM/yyyy");
            txtComplemento.Text = con.Funcionario.Pessoa.Complemento;
            txtBairro.Text = con.Funcionario.Pessoa.Bairro;
            txtCidade.Text = con.Funcionario.Pessoa.Cidade + "/" + con.Funcionario.Pessoa.Estado;
            txtCep.Text = con.Funcionario.Pessoa.CEP;
            txtTelefone.Text = con.Funcionario.Pessoa.Fone;
            txtCelular.Text = con.Funcionario.Pessoa.Celular;
            txtEmail.Text = con.Funcionario.Pessoa.Email;

            // Dados con.Funcionarioionais
            txtMatricula.Text = con.Funcionario.Matricula;
            txtCategoria.Text = con.Funcionario.FuncionarioCategoria.Nome;
            txtRegime.Text = con.Funcionario.NomeRegimeFolha;
            txtSituacaoFunc.Text = con.Funcionario.NomeSituacao;
            txtLocal.Text = con.Funcionario.NomeLocalFolha;
            txtSetor.Text = con.Funcionario.NomeSetorFolha;
            txtCargo.Text = con.Funcionario.NomeCargoFolha;
            txtDataAdm.Text = con.Funcionario.DataAdmissao == null ? "---" : con.Funcionario.DataAdmissao.Value.ToString("dd/MM/yyyy"); //con.Funcionario.DataAdmissao.ToString();

            // Dados do Averbacao
            txtConsignataria.Text = con.Empresa1.Nome;
            txtNumero.Text = con.Numero;
            LabelIdentificador.Text = con.Identificador;
            txtData.Text = con.Data.ToString();
            txtUsuario.Text = con.Usuario.NomeCompleto;
            txtPrazo.Text = con.Prazo.ToString() + string.Format(" ({0} restante)", FachadaAverbacoes.ObtemParcelasRestantes(con.IDAverbacao));
            txtValorParcela.Text = con.ValorParcela.ToString();
            txtValorConsignado.Text = (con.Prazo * con.ValorParcela).ToString();
            txtValorAverbacao.Text = con.ValorContratado.ToString();
            txtJuros.Text = con.CET.ToString();
            txtCoeficiente.Text = con.Coeficiente.ToString();
            txtMesInicio.Text = con.CompetenciaInicial;
            txtMesFim.Text = con.CompetenciaFinal;
            txtProduto.Text = con.Produto.Nome;
            txtTipoProduto.Text = con.Produto.ProdutoGrupo.Nome;
            txtOperacao.Text = con.AverbacaoTipo.Nome;
            txtSituacaoAverbacao.Text = con.AverbacaoSituacao.Nome;

            AtualizaButtonSuspenderAverbacao(con);

            PopularGrids(con);
        }

        private void AtualizaButtonSuspenderAverbacao(Averbacao con)
        {
            if (con.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Suspenso_MargemLivre || con.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Suspenso_MargemLivre)
                ASPxButtonSuspenderBloquear.Text = "Ativar";
            else
                ASPxButtonSuspenderBloquear.Text = "Suspender/Bloquear";
        }

        private void PopularGrids(Averbacao con)
        {
            List<Averbacao> averbacoes = FachadaFuncionariosConsulta.ObtemAverbacaoVinculos(IdAverbacao);

            TableAverbacoesVinculadas.Visible = averbacoes.Count > 0;

            gridAverbacaosVinc.DataSource = averbacoes;
            gridAverbacaosVinc.DataBind();

            gridTramitacao.DataSource = con.AverbacaoTramitacao.OrderByDescending(x => x.CreatedOn);
            gridTramitacao.DataBind();

            gridSolicitacoes.DataSource = con.EmpresaSolicitacao.OrderByDescending(x => x.DataSolicitacao);
            gridSolicitacoes.DataBind();

            gridParcelas.DataSource = FormataCompetencia(con.AverbacaoParcela.OrderBy(x => x.Competencia).ToList());
            gridParcelas.DataBind();
        }

        private IEnumerable<AverbacaoParcela> FormataCompetencia(ICollection<AverbacaoParcela> parcelas)
        {
            foreach (AverbacaoParcela parcela in parcelas)
            {
                string[] partesCompetencia = parcela.Competencia.Split(new[] { '/' });
                parcela.Competencia = string.Join("/", partesCompetencia.Reverse().ToArray());
            }

            return parcelas;
        }

        protected void gridVinculos_DataSelect(object sender, EventArgs e)
        {
            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacaoVinculos(id);

            gridMaster.DataSource = dados;
        }

        protected void gridParcelas_DataSelect(object sender, EventArgs e)
        {
            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacao(id).AverbacaoParcela;

            gridMaster.DataSource = dados;
        }

        protected void gridTramitacoes_DataSelect(object sender, EventArgs e)
        {
            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaFuncionariosConsulta.ObtemAverbacao(id).AverbacaoTramitacao;

            gridMaster.DataSource = dados;
        }

        protected void PessoaisDetalhes_Click(object sender, EventArgs e)
        {
            //pessoaisDetalhes.Visible = !pessoaisDetalhes.Visible;
        }

        protected void AverbacaoDetalhes_Click(object sender, EventArgs e)
        {
            //averbacaoDetalhes.Visible = !averbacaoDetalhes.Visible;
        }

        protected void ODS_Averbacaos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdAverbacao;
        }

        protected void ASPxButtonSuspender_Click(object sender, EventArgs e)
        {
            int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo);

            if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.SuspenderAtivar))
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAverbacaoSuspenderBloquear, idrecurso, (int)Enums.Permissao.SuspenderAtivar, IdAverbacao, ASPxButtonSuspenderBloquear.Text != "Ativar");
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
            }
        }

        protected void ASPxButtonLiquidar_Click(object sender, EventArgs e)
        {
            int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo);

            if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.Liquidar))
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAverbacaoLiquidar, idrecurso, (int)Enums.Permissao.Liquidar, IdAverbacao);
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
            }
        }
        
        protected void ASPxButtonConfirmarQuitacao_Click(object sender, EventArgs e)
        {
            int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlConfirmarQuitacao, Sessao.IdModulo);

            if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.Acessar))
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConfirmarQuitacao, idrecurso, (int)Enums.Permissao.Acessar, IdAverbacao);
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
            }
        }

        protected void ASPxButtonCancelar_Click(object sender, EventArgs e)
        {
            int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo);

            if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.Cancelar))
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAverbacaoCancelar, idrecurso, (int)Enums.Permissao.Cancelar, IdAverbacao);
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
            }
        }

        protected void ASPxButtonInformarSaldoDevedor_Click(object sender, EventArgs e)
        {
            int idrecurso = FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo);

            if (FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.InformarSaldoDevedor))
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlInformarSaldoDevedor, idrecurso, (int)Enums.Permissao.InformarSaldoDevedor, IdAverbacao);
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemAcessoNegado);
            }
        }

        protected void ASPxButtonAprovar_Click(object sender, EventArgs e)
        {
            FachadaGerenciarAverbacao.AprovarDesaprovar(IdAverbacao, Sessao.IdBanco, true, Sessao.UsuarioLogado.IDUsuario);
        }

        protected void ASPxButtonDesaprovar_Click(object sender, EventArgs e)
        {
            FachadaGerenciarAverbacao.AprovarDesaprovar(IdAverbacao, Sessao.IdBanco, false, Sessao.UsuarioLogado.IDUsuario);
        }

        protected void ASPxPopupControlTermo_Load(object sender, EventArgs e)
        {
            if (IdAverbacao.Equals(0)) return;

            Averbacao con = FachadaConciliacao.ObtemAverbacao(IdAverbacao);

            if (con == null) return;

            WebUserControlTermoAverbacaoImpressao.Visible = con.AverbacaoSituacao.DeduzMargem;

            if (WebUserControlTermoAverbacaoImpressao.Visible) WebUserControlTermoAverbacaoImpressao.ConfiguraTermo(con);
        }

        protected void gridVinculos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int idAverbacao = Convert.ToInt32(e.KeyValue);

            Averbacao averbacao = FachadaGerenciarAverbacao.ObtemAverbacao(idAverbacao);

            if (Sessao.IdModulo != (int)Enums.Modulos.Consignante && averbacao.IDConsignataria != Sessao.IdBanco && !FachadaGerenciarAverbacao.ContratoEmProcessoDeQuitacaoOuCompra(idAverbacao))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemProibidoAcessoContratoOutrosBancos);
                return;
            }

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, this.IdRecurso, 1, idAverbacao);
        }

        public void AtualizaGrid()
        {
            PopulaDados(IdAverbacao);
        }

    }
}