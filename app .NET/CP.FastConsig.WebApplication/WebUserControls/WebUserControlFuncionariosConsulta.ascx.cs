using System;
using CP.FastConsig.Common;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlFuncionariosConsulta : CustomUserControl
    {

        private const string ParametroIdFunc = "IDFunc";

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFunc] == null) ViewState[ParametroIdFunc] = 0;
                return (int)ViewState[ParametroIdFunc];
            }
            set
            {
                ViewState[ParametroIdFunc] = value;
            }
        }

        protected string Script()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("function confirma() {");
            sb.Append("  $(function() { ");
            sb.Append("    $('#dlgconfirmaapos').dialog({ ");
            sb.Append("       autoOpen: false,");
            sb.Append("       modal: true,");
            sb.Append("       show: 'blind',");
            sb.Append("       hide: 'explode',");
            sb.Append("       buttons: {");
            sb.Append("         Confirmar: function() {");
            sb.Append("           $(this).dialog('close');");
            sb.Append("           __doPostBack('Aposentar', 'aposenta_click');");
            //sb.Append("           ASPxPopupControlAposentadoria.SetHeaderText('ASPxPopupControlAposentadoria.Show()');");
            //sb.Append("           ASPxPopupControlAposentadoria.Show();");
            sb.Append("         },");
            sb.Append("         Desistir: function() {");
            sb.Append("           $(this).dialog('close');");
            sb.Append("           return false;");
            sb.Append("         }");
            sb.Append("       }");
            sb.Append("    });");
            sb.Append("  });");
            sb.Append("  $('#dlgconfirmaapos').dialog( 'open' );");
            sb.Append("  return false;");
            sb.Append("}");

            return sb.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (DadosAverbacoes.Any())
            {
                gridAverbacaos.DataSource = DadosAverbacoes;
                gridAverbacaos.DataBind();
            }

            if (ControleCarregado && string.IsNullOrEmpty(Request["__EVENTARGUMENT"])) return;

            RegistrarStartupScript(this, "		function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest); } " +
            "function onEndRequest(sender, args) { " +
            //"if (sender._postBackSettings.sourceElement != null && sender._postBackSettings.sourceElement.id == 'Aposentar' ) {  $('#dlgconfirmaapos').dialog('close'); " + Script() + "  } " +
            Script() + " }");

            RegistrarBlockScript(this, Script(), true);

            string eventArgs = Request["__EVENTARGUMENT"];

            if (!string.IsNullOrEmpty(eventArgs))
            {
                if (eventArgs.Equals("aposenta_click"))
                {
                    EhPostBack = false;
                    ASPxButtonAposentar_Click(this, new EventArgs());
                }
            }
            if (!EhPostBack)
            {

                ConfiguraComponentes();

                EhPostBack = true;
                IdFunc = Id.Value;

                PopulaDados(IdFunc);

                AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapse);
                AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapse2);
            }
            else
            {

                try
                {

                    ConfiguraGridAverbacoes();

                }
                catch { }

            }
            
        }

        private void ConfiguraGridAverbacoes()
        {

            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            DadosAverbacoes = func.Averbacao.AsQueryable();

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria) DadosAverbacoes = DadosAverbacoes.Where(x => x.IDConsignataria == Sessao.IdBanco);

            DadosAverbacoes = DadosAverbacoes.Where(x => x.AverbacaoSituacao.DeduzMargem && !x.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao));

            gridAverbacaos.DataSource = DadosAverbacoes;
            gridAverbacaos.DataBind();
            
        }

        private IQueryable<Averbacao> DadosAverbacoes
        {
            get
            {
                if (Session["DadosAverbacoes"] == null) Session["DadosAverbacoes"] = new List<Averbacao>().AsQueryable();
                return (IQueryable<Averbacao>)Session["DadosAverbacoes"];
            }
            set
            {
                Session["DadosAverbacoes"] = value;
            }
        } 

        private void ConfiguraComponentes()
        {

            if ((int)Enums.Modulos.Consignataria == Sessao.IdModulo)
            {
                ASPxButtonBloquearUsuario.Visible = false;
                ASPxButtonGerarSenha.Visible = false;
                ASPxButtonHistoricos.Visible = false;
                ASPxButtonAutorizacoes.Visible = false;
                txtSenhaProvisoria.Visible = false;
            }
            else
            {
                ASPxButtonBloquearUsuario.Visible = true;
                ASPxButtonGerarSenha.Visible = true;
                ASPxButtonHistoricos.Visible = true;
                ASPxButtonAutorizacoes.Visible = true;
                txtSenhaProvisoria.Visible = true;
            }

        }

        private void PopulaDados(int id)
        {

            if (id == 0) return;

            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(id);

            ASPxButtonRescindir.Visible = (Sessao.IdModulo == (int)Enums.Modulos.Consignante);
            ASPxButtonAposentar.Visible = (Sessao.IdModulo == (int)Enums.Modulos.Consignante && func.IDFuncionarioSituacao == (int)Enums.FuncionarioSituacao.AtivoNaFolha);
            // Dados Pessoais
            txtNome.Text = func.Pessoa.Nome;
            txtCPF.Text = StringHelper.MaskString(func.Pessoa.CPF, ResourceAuxiliar.MascaraCPF);
            txtRG.Text = func.Pessoa.RG;
            txtEndereco.Text = func.Pessoa.Endereco;
            txtDataNasc.Text = func.Pessoa.DataNascimento == null ? "---" : func.Pessoa.DataNascimento.Value.ToString("dd/MM/yyyy");
            txtComplemento.Text = func.Pessoa.Complemento;
            txtBairro.Text = func.Pessoa.Bairro;
            txtCidade.Text = func.Pessoa.Cidade+"/"+func.Pessoa.Estado;
            txtCep.Text = func.Pessoa.CEP;
            txtTelefone.Text = func.Pessoa.Fone;
            txtCelular.Text = func.Pessoa.Celular;
            txtEmail.Text = func.Pessoa.Email;

            // Dados Funcionais
            txtMatricula.Text = func.Matricula;
            txtCategoria.Text = func.FuncionarioCategoria == null ? "" : func.FuncionarioCategoria.Nome;
            txtRegime.Text = func.NomeRegimeFolha;
            txtSituacao.Text = func.NomeSituacao;
            txtLocal.Text = func.NomeLocalFolha;
            txtSetor.Text = func.NomeSetorFolha;
            txtCargo.Text = func.NomeCargoFolha;

            txtBloqueio.Visible = func.FuncionarioBloqueio.Any(x => x.Ativo == 1);

            if(func.Pessoa.Usuario != null) txtSenhaProvisoria.Text = func.Pessoa.Usuario.SenhaProvisoria ?? "";

            LabelDataAdmissao.Text = func.DataAdmissao == null ? "---" : func.DataAdmissao.Value.ToString("dd/MM/yyyy");

            if (func.FuncionarioMargem.Count == 0)
            {
                FachadaFuncionariosConsulta.GerarMargens(IdFunc);
                func = FachadaFuncionariosConsulta.ObtemFuncionario(id);
            }

            GridViewMargens.DataSource = ProcessaMargens(func.FuncionarioMargem).ToList();
            GridViewMargens.DataBind();

           

        }

        private IEnumerable<GrupoMargem> ProcessaMargens(ICollection<FuncionarioMargem> lista)
        {
            return FachadaFuncionariosConsulta.ProcessaMargens(lista);
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
            var dados = FachadaFuncionariosConsulta.ObtemAverbacao(id).AverbacaoTramitacao.OrderByDescending(x => x.CreatedOn);

            gridMaster.DataSource = dados;
        }

        protected void PessoaisDetalhes_Click(object sender, EventArgs e)
        {
            //pessoaisDetalhes.Visible = !pessoaisDetalhes.Visible;
        }

        protected void FuncionaisDetalhes_Click(object sender, EventArgs e)
        {
            //funcionaisDetalhes.Visible = !funcionaisDetalhes.Visible;
        }

        protected void MostrarAverbacaos_Click(object sender, EventArgs e)
        {
            fsAverbacaos.Visible = !fsAverbacaos.Visible;

            if (fsAverbacaos.Visible)
            {
                Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);
                IEnumerable<Averbacao> averbacoes = func.Averbacao.Where(x => x.Ativo == 1);
                if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                {
                    averbacoes = averbacoes.Where(x => x.IDConsignataria == Sessao.IdBanco);
                    if (FachadaPermissoesAcesso.CheckPermissao(Id.Value, Sessao.IdBanco, Sessao.IdPerfil, 35))
                        averbacoes = averbacoes.Where(x => x.IDUsuario == Sessao.UsuarioLogado.IDUsuario);
                }

                DadosAverbacoes = averbacoes.AsQueryable();

                gridAverbacaos.DataSource = DadosAverbacoes;
                gridAverbacaos.DataBind();
            }
        }

        protected void ODS_Averbacaos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdFunc;
        }


        protected void ButtonBloquearUsuario_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlBloqueioUsuario, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 21, IdFunc);
        }

        protected void ASPxButtonBloquearUsuario_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlBloqueioUsuario, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 21, IdFunc);
        }

        protected void ASPxButtonAutorizacoes_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncAutorizacoes, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 26, IdFunc);
        }

        private string getMesFolha()
        {
            DateTime dataAtual = DateTime.Now;

            Parametro param = FachadaDashBoardConsignante.obterParametro("DiaCorte");
            string diaCorte = param.Valor;
            string mesfolha;

            if (dataAtual.Day <= Convert.ToInt32(diaCorte))
            {
                mesfolha = String.Format("{0}/{1}", dataAtual.Month, dataAtual.Year);
            }
            else
            {
                int ano = dataAtual.Year;
                int mes = dataAtual.Month + 1;
                if (mes > 12)
                {
                    mes = 1;
                    ano = ano + 1;
                }
                mesfolha = String.Format("{0}/{1}", mes, ano);
            }
            return mesfolha;
        }

        protected void ASPxButtonRescindir_Click(object sender, EventArgs e)
        {
            Funcionario f = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            if (f.FuncionarioSituacao.IDFuncionarioSituacao == (int)Enums.FuncionarioSituacao.AtivoNaFolha)
            {
                string mesfolha = getMesFolha();
                DropDownListRescindir.Items.Clear();

                ListItem a = new ListItem("Não enviar mais descontos para este funcionário", "1");
                DropDownListRescindir.Items.Add(a);
                ListItem b = new ListItem("Enviar apenas os descontos deste mês: " + mesfolha, "2");
                DropDownListRescindir.Items.Add(b);
                
                
                DivRescindir.Visible = !DivRescindir.Visible;
            }
            else            
            {
                PageMaster.ExibeMensagem("Só é possivel apenas para funcionário na situação Ativo na Folha.");
            }
        }

        protected void ASPxButtonAposentar_Click(object sender, EventArgs e)
        {
            Funcionario f = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            if (f.FuncionarioSituacao.IDFuncionarioSituacao == (int)Enums.FuncionarioSituacao.AtivoNaFolha)
            {
                FachadaFuncionariosConsulta.Aposentar(IdFunc);
                PopulaDados(IdFunc);

                ASPxPopupControlAposentadoria.ShowOnPageLoad = true;
                //RegistrarStartupScript(this,"ASPxPopupControlAposentadoria.Show();");

                //PageMaster.ExibeMensagem("Este Funcionário foi aposentado com sucesso!");
            }
            else
            {
                ASPxPopupControlAposentadoria.ShowOnPageLoad = true;
//                PageMaster.ExibeMensagem("Só é possivel apenas para funcionário na situação Ativo na Folha.");
            }
        }

        protected void ASPxButtonAplicarRescisao_Click(object sender, EventArgs e)
        {
            string opcao = DropDownListRescindir.SelectedValue;

            string exonerar = getMesFolha();            

            int situacao = (opcao == "1" ? (int)Enums.FuncionarioSituacao.Exonerado : (int)Enums.FuncionarioSituacao.Bloqueado);

            FachadaFuncionariosConsulta.AtualizaSituacao(IdFunc, situacao, exonerar);            

            DivRescindir.Visible = !DivRescindir.Visible;

            PopulaDados(IdFunc);

            PageMaster.ExibeMensagem("Rescisão efetuada com Sucesso!");
            
        }

        protected void ASPxButtonCancelarRescisao_Click(object sender, EventArgs e)
        {
            DivRescindir.Visible = false;
        }

        protected void ASPxButtonHistoricos_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncHistorico, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 24, IdFunc);
        }

        protected void GerarSenha_Click(object sender, EventArgs e)
        {
            if (!FachadaPermissoesAcesso.CheckPermissao(this.IdRecurso,Sessao.IdBanco,Sessao.IdPerfil,22))
            {
                PageMaster.ExibeMensagem("Acesso Negado!", "Permissão de Acesso");
                return;
            }
            string senha = FachadaFuncionariosConsulta.GerarSenha(IdFunc);
            txtSenhaProvisoria.Text = senha;
        }

        protected void gridAverbacoes_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.KeyValue);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, 0, 1, id);
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

        protected void ASPxPopupControlAposentadoria_Load(object sender, EventArgs e)
        {
            if (IdFunc.Equals(0)) return;

            Funcionario fun = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            if (fun == null) return;

            //WebUserControlImprimirAposentadoria.Visible = fun.IDFuncionarioSituacao == (int)Enums.FuncionarioSituacao.Aposentado;

            WebUserControlImprimirAposentadoria.Configura(fun);
        }

    }

}