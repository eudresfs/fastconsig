using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.BLL;
using CP.FastConsig.Util;
using System.Text;
using System.Web.Script.Serialization;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlMinhasAverbacoes : CustomUserControl
    {
        private const string ParametroIdFunc = "IDFuncDash";

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

        private int IdAverbacao
        {
            get
            {
                if (ViewState["IDMinhaAverbacao"] == null) ViewState["IDMinhaAverbacao"] = 0;
                return (int)ViewState["IDMinhaAverbacao"];
            }
            set
            {
                ViewState["IDMinhaAverbacao"] = value;
            }
        }

        public string chamaDialogo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("function chamaDialogo() {");
            sb.Append("$('#dlgconfirmacao').dialog({");
            sb.Append(" draggable: true,");
            sb.Append(" autoOpen: true,");
            sb.Append(" position: 'center',");
            sb.Append(" show: 'blind',");
            sb.Append(" width: 500,");
            sb.Append(" height: 180,");
            sb.Append(" modal: true,");
            sb.Append(" bgiframe: true,");
            sb.Append(" buttons: {");
            sb.Append(" 'Ok': function() {");
            sb.Append(" $( this ).dialog( 'close' );");
            sb.Append("__doPostBack('link', 'link_click:');");
            sb.Append(" return true;");
            sb.Append(" },");
            sb.Append(" 'Cancelar': function() {");
            sb.Append(" $( this ).dialog( 'close' );");
            sb.Append(" return false;");
            sb.Append(" }");
            sb.Append(" }");
            sb.Append(" });");
            
            sb.Append(" }");
            return sb.ToString();

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RegistrarBlockScript(this, chamaDialogo(), true);

            string eventArgs = Request["__EVENTARGUMENT"];

            if (!string.IsNullOrEmpty(eventArgs))
            {
                if (eventArgs.Substring(0,10) == "link_click")
                {
                    EhPostBack = false;
                    
                    char c = ':';

                    int averbacao = Convert.ToInt32(eventArgs.Substring(eventArgs.IndexOf(c)+1,20));

                    registraSolicitacao(averbacao);
                    
                }
            }

            
            if (EhPostBack || ControleCarregado)
                return;

            EhPostBack = true;

            if (Sessao.IdModulo != (int)Enums.Modulos.Funcionario)
            {
                PageMaster.ExibeMensagem("Esta página só pode ser exibida através do módulo de Funcionários.");
                PageMaster.Voltar();
                return;
            }

            Pessoa p = Sessao.UsuarioLogado.Pessoa.FirstOrDefault();
            if (p != null)
            {
                if (p.Funcionario.FirstOrDefault() != null)
                    IdFunc = p.Funcionario.FirstOrDefault().IDFuncionario;
            }
            
            popularDados(IdFunc);
         
        }

        private void popularDados(int idfunc)
        {
            List<Averbacao> a;
            if (ckbApenasEmFolha.Checked)
            {
                a = Averbacoes.listaAverbacaoFuncionario(idfunc).Where(x => x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos && x.AverbacaoSituacao.ParaDescontoFolha == true).ToList();
            }
            else
            {
                a = Averbacoes.listaAverbacaoFuncionario(idfunc).Where(x => x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos).ToList();
            }
            GridViewAverbacoes.DataSource = a;
            GridViewAverbacoes.DataBind();

            List<Averbacao> outros;//
            if (ckbApenasEmFolha.Checked)
            {
                outros = Averbacoes.listaAverbacaoFuncionario(idfunc).Where(x => x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && x.Produto.IDProdutoGrupo != (int)Enums.ProdutoGrupo.Emprestimos && x.AverbacaoSituacao.ParaDescontoFolha == true).ToList();
            }
            else
            {
                outros = Averbacoes.listaAverbacaoFuncionario(idfunc).Where(x => x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && x.Produto.IDProdutoGrupo != (int)Enums.ProdutoGrupo.Emprestimos).ToList();
            }
            GridViewOutrosProdutos.DataSource = outros;
            GridViewOutrosProdutos.DataBind();
        }

        protected void selecionaApenasDesc_Click(object sender, EventArgs e)
        {
            popularDados(IdFunc);
        }

        protected void Seleciona_Averbacao_Click(object sender, EventArgs e)
        {
            int idAverbacao = Convert.ToInt32(GridViewAverbacoes.SelectedDataKey.Value);
        
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, Sessao.IdModulo), 1, idAverbacao);
        }

        protected void Seleciona_AverbacaoOutro_Click(object sender, EventArgs e)
        {
            int idAverbacao = Convert.ToInt32(GridViewOutrosProdutos.SelectedDataKey.Value);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, FachadaGerenciarAverbacao.ObtemIdRecursoPorNomeModulo(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, Sessao.IdModulo), 1, idAverbacao);
        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;            

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            Averbacao a = Averbacoes.ObtemAverbacao( id );

            int idempresa = a.IDConsignataria;

            int idconsignante = Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante());

            Solicitacoes.AdicionaSolicitacao(idconsignante, (int)Enums.SolicitacaoTipo.InformarSaldoDevedordeContratos, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, id, IdFunc, Sessao.IdUsuario, Sessao.IdUsuario, "Solicitação informação de Saldo Devedor", "");

            Parametro param = FachadaDashBoardConsignante.obterParametro("SaldoDevedor");

            string dias = param.Valor;

            string msg = "A solicitação foi enviada e o banco tem {0} dias úteis para responder.";

            PageMaster.ExibeMensagem( String.Format( msg, dias ) );


            //PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConsignatariasEdicao, IdRecurso, 1, id);
        }

        public void registraSolicitacao( int id )
        {
            string obs = TextBoxObs.Text;            

            Averbacao a = Averbacoes.ObtemAverbacao(id);

            int idempresa = a.IDConsignataria;

            int idconsignante = Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante());

            Solicitacoes.AdicionaSolicitacao(idconsignante, (int)Enums.SolicitacaoTipo.SolicitacaoDeInformacao, (int)Enums.SolicitacaoSituacao.Pendente, idempresa, id, IdFunc, Sessao.IdUsuario, Sessao.IdUsuario, "Solicitação informação de Saldo Devedor", obs);

            string msg = "A solicitação foi enviada.";

            PageMaster.ExibeMensagem(msg);
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Averbacao a = (Averbacao) e.Row.DataItem;

                int parcrestante = FachadaAverbacoes.ObtemParcelasRestantes(a.IDAverbacao);
                int prazo = (int)a.Prazo;
                int parcpagas = prazo - parcrestante;

                e.Row.Cells[7].Text = parcpagas.ToString();

                EmpresaSolicitacaoSaldoDevedor solicSaldoDevedor = FachadaSolicitacao.ObtemSolicitacaoSaldoDevedor(a.IDAverbacao);

                if (solicSaldoDevedor != null)
                {
                    e.Row.Cells[8].Text = String.Format("{0:#,0.00}", solicSaldoDevedor.Valor) + " em " + String.Format("0:d",solicSaldoDevedor.Data);
                }
                   

            }
        }

        protected void gridOutros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Averbacao a = (Averbacao)e.Row.DataItem;

                int parcrestante = FachadaAverbacoes.ObtemParcelasRestantes(a.IDAverbacao);
                int prazo = (int)a.Prazo;
                int parcpagas = prazo - parcrestante;

                e.Row.Cells[7].Text = parcpagas.ToString();

            }
        }        


    }
}