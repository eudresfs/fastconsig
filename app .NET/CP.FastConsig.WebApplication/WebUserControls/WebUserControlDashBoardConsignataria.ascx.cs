using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Utils;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlDashBoardConsignataria : CustomUserControl
    {

        #region Constantes
            private const string FormatoDinheiro = "{0:C}";
            private static readonly Dictionary<string, decimal> ValoresTotais = new Dictionary<string, decimal>();

            private const string CampoValor = "Valor";
            private const string CampoLucro = "Lucro";
        #endregion

        private List<string> parametrosFechamento = new List<string>();

        bool bMostrarParametros;
        bool bMostrarGraficoValoresDescontados;
        bool bMostrarQuadroSolicitacao;
        bool bMostrarIndicesNegocio;
        bool bApenasDoUsuario;
        bool bMostrarLucro;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if ((ControleCarregado) || EhPostBack)
                //return;

            bMostrarParametros = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.QuadroParametros);
            bMostrarGraficoValoresDescontados = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.GraficoValoresDescontados);
            bMostrarQuadroSolicitacao = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.QuadroSolicitacoes);
            //bMostrarIndicesNegocio = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.QuadroIndiceDeNegocios);
            bApenasDoUsuario = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.IndiceNegocioApenasContratosRegistradosPeloUsuario);
            bMostrarLucro = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.PermitirMostrarLucroIndicesDeNegocio);
            
            if (bMostrarParametros)
            {
                parametrosFechamento.Add("DiaCorte");
                parametrosFechamento.Add("DiaCorteConsignataria");
                parametrosFechamento.Add("AverbadosMes");
                parametrosFechamento.Add("PrazoMaximo");

                carregaParametrosFechamento();
                carregaParametrosExecucao();
            }
            else
            {
                ASPxRoundPanelParametros.Visible = false;
            }
           
            ConfiguraTopo();
            ConfiguraTela();

            EhPostBack = true;

        }

        private void ConfiguraTela()
        {

            ValoresTotais.Clear();
            ValoresTotais.Add(CampoValor, 0);
            ValoresTotais.Add(CampoLucro, 0);
            
            ConfiguraGrids();          

            if (bMostrarGraficoValoresDescontados)
            {
                mostraGraficoEnviadosDescontados(6);
            }
            else
            {
                WebUserControlChartAreaEnviadosDescontados.Visible = false;
            }
            
        }

        public void mostraGraficoEnviadosDescontados(int nMeses)
        {

            List<ADescontar_E_Descontados> dados = FachadaDashBoardConsignataria.listaADescontarVDescontados(nMeses, Sessao.IdBanco);

            var meses = dados.Select(x => x.Mes).Distinct();

            List<string> aMeses = meses.ToList();

            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosEnviados = new List<decimal>();
            List<decimal> dadosDescontados = new List<decimal>();

            List<string> categorias = new List<string>();

            foreach (var item in aMeses)
            {
                categorias.Add((string)item.Trim());
            }

            foreach (var item in dados)
            {
                if (item.Tipo.Trim() == "A Descontar")
                {
                    dadosEnviados.Add((decimal)item.Valor);
                }
                else
                {
                    dadosDescontados.Add((decimal)item.Valor);
                }
            }

            serieValores.Add("A Descontar", dadosEnviados.ToArray());
            serieValores.Add("Descontados", dadosDescontados.ToArray());

            WebUserControlChartAreaEnviadosDescontados.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartAreaEnviadosDescontados, "A Descontar x Descontados", string.Empty, "Total", categorias.ToArray(), serieValores);

        }

        private void ConfiguraGrids()
        {

            ToolTipControllerShowEventArgs dica = new ToolTipControllerShowEventArgs();
            dica.AllowHtmlText = DefaultBoolean.True;

            if (bMostrarQuadroSolicitacao)
            {

                if (DropDownListTipoPendencia.SelectedValue == "1")
                    ASPxGridViewPendencias.DataSource = FachadaMaster.ObtemSolicitacoes(Sessao.IdModulo, Sessao.IdBanco, Sessao.UsuarioLogado.IDUsuario);
                else
                    ASPxGridViewPendencias.DataSource = FachadaMaster.ObtemSolicitacoesSolicitadasPelaEmpresa(Sessao.IdModulo, Sessao.IdBanco);

                ASPxGridViewPendencias.DataBind();
            }
            else
                ASPxGridViewPendencias.Visible = false;
                        
        }


        public void carregaParametrosFechamento()
        {
            List<Tabela> dados = new List<Tabela>();

            foreach (var item in parametrosFechamento)
            {

                string sDiaCorte = "";
                
                Tabela tabela = new Tabela();
                
                if ((string)item == "AverbadosMes")
                {
                    Parametro param = FachadaDashBoardConsignante.obterParametro((string)item);
                    tabela.Nome = param.Descricao;
                    tabela.Valor = Utilidades.ConverteMesAno(FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco));

                    string mes = tabela.Valor.Substring(0, 2);
                    
                    DateTime dt = new DateTime(DateTime.Now.Year, Convert.ToInt32(mes), 1 );

                    tabela.Valor = String.Format("{0:MMMM}", dt) + "/" + tabela.Valor.Substring(3, 4);
                }
                else if ((string)item == "DiaCorte")
                {
                    Parametro param = FachadaDashBoardConsignante.obterParametro((string)item);
                    tabela.Nome = param.Descricao + " (Folha Pgto.)";
                    tabela.Valor = param.Valor;
                    sDiaCorte = param.Valor;
                }
                else if ((string)item == "DiaCorteConsignataria")
                {
                    Empresa e = FachadaConsignatarias.ObtemEmpresa(Sessao.IdBanco);

                    tabela.Nome = String.Format("Dia de Corte ({0})", e.Sigla);
                    if ((e.DiaCorte.Equals(null)) || (e.DiaCorte.Equals(0)))
                        tabela.Valor = sDiaCorte;
                    else
                        tabela.Valor = e.DiaCorte.ToString();
                }
                else if ((string)item == "PrazoMaximo")
                {
                    Parametro param = FachadaDashBoardConsignante.obterParametro((string)item);
                    tabela.Nome = param.Descricao;
                    tabela.Valor = param.Valor;

                    string prazo = FachadaConsignatarias.obtemPrazoMaximo(Sessao.IdBanco);

                    if (!prazo.Equals(string.Empty))
                        tabela.Valor = prazo;

                }
                else
                {
                    Parametro param = FachadaDashBoardConsignante.obterParametro((string)item);
                    tabela.Nome = param.Descricao;
                    tabela.Valor = param.Valor;
                }

                dados.Add(tabela);
            }

            ASPxGridViewPeriodoFechamento.DataSource = dados.ToList();
            ASPxGridViewPeriodoFechamento.DataBind();

        }


        public void carregaParametrosExecucao()
        {

            List<EmpresaSolicitacaoTipo> dados = FachadaDashBoardConsignante.listaPrazosExecucao(Sessao.IdModulo.ToString());

            ASPxGridViewPrazosExecucao.DataSource = dados.ToList();
            ASPxGridViewPrazosExecucao.DataBind();

        }

        protected void ButtonSimular_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(TextBoxMatriculaCpf.Text))
            //{
            //    PageMaster.ExibeMensagem(ResourceMensagens.MensagemPreencherCampoMatriculaCPF); return;
            //}
            //if (MatriculaCpfNaoEncontrado)
            //{
            //    PageMaster.ExibeMensagem(ResourceMensagens.MensagemMatriculaCPFNaoEncontrado); return;
            //}

            //int idRecurso = (int) Enums.Recursos.ConsignatariaCentralSimulacao;
            //PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlCentralSimulacao, (int)Enums.Recursos.ConsignatariaCentralSimulacao, TextBoxMatriculaCpf.Text);
        }

        protected void selecionaAverbacoes_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, id);
        }

        protected void selecionaTipoPendencia_Click(object sender, EventArgs e)
        {
            if (DropDownListTipoPendencia.SelectedValue == "1")
                ASPxGridViewPendencias.DataSource = FachadaMaster.ObtemSolicitacoes(Sessao.IdModulo, Sessao.IdBanco, Sessao.UsuarioLogado.IDUsuario);
            else
                ASPxGridViewPendencias.DataSource = FachadaMaster.ObtemSolicitacoesSolicitadasPelaEmpresa(Sessao.IdModulo, Sessao.IdBanco);
            ASPxGridViewPendencias.DataBind();
        }


    }

    public class ParametroDashBoard
    {
        public string Parametro { get; set; }
        public string Valor { get; set; }

        public ParametroDashBoard(string parametro, string valor)
        {
            Parametro = parametro;
            Valor = valor;
        }
    }

    
}