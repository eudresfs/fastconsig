using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using DevExpress.Web.ASPxGridView;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlDashBoardConsignante : CustomUserControl
    {
       
       private const string PathImagensLogos = "{0}"; //~/Imagens/Logos/Bancos/{0}";
       private const string CaminhoImagemCross = "~/Imagens/cross.png";
       private const string CaminhoImagemTick = "~/Imagens/tick.png";
 
       public enum Ocorrencias
       {
            Quitacao = 1,
            Liquidacao = 2,
            Aprovacao = 3,
            Cancelamento = 4
       }
       
        private const string AtributoValorPro = "ValorPro";
        private const string AtributoValorNeutro = "ValorNeutro";
        private const string AtributoValorContra = "ValorContra";

        private List<string> parametrosFechamento = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            AtualizarSuspensoes();

            if (ControleCarregado) return;

            parametrosFechamento.Add("DiaCorte");
            parametrosFechamento.Add("AverbadosMes");
            parametrosFechamento.Add("PrazoMaximo");

            carregaParametrosFechamento();
            carregaParametrosExecucao();

            if (EhPostBack) return;

            EhPostBack = true;

            carregaSolicitacoesTipo();

            mostraGraficoEnviadosDescontados(6);

            AtualizaSolicitacoesPendencias();

        }

        public void AtualizarSuspensoes()
        {
            carregaSolicitacoesTipo();
            AtualizaOcorrencias(Convert.ToInt32(DropDownListSolicitacaoTipo.SelectedValue));
        }

        public void carregaSolicitacoesTipo()
        {
            DropDownListSolicitacaoTipo.DataSource = Solicitacoes;
            DropDownListSolicitacaoTipo.DataBind();
        }

        private List<EmpresaSolicitacaoTipo> Solicitacoes
        {
            get
            {
                if (Session["Solicitacoes"] == null) Session["Solicitacoes"] = FachadaDashBoardConsignante.listaSolicitacoesTipo().Where(x => x.Prazo > 0).ToList();
                return (List<EmpresaSolicitacaoTipo>)Session["Solicitacoes"];
            }
        }

        public void carregaParametrosFechamento()
        {
            List<Tabela> dados = new List<Tabela>();            

            foreach (var item in parametrosFechamento)
            {                                
                
                Parametro param = FachadaDashBoardConsignante.obterParametro( (string)item );

                Tabela tabela = new Tabela();
                tabela.Nome = param.Descricao;
                if ((string)item == "AverbadosMes")
                {
                    tabela.Valor = Utilidades.ConverteMesAno(FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante())));

                    string mes = tabela.Valor.Substring(0, 2);

                    DateTime dt = new DateTime(2011, Convert.ToInt32(mes), 1);

                    tabela.Valor = String.Format("{0:MMMM}", dt) + "/" + tabela.Valor.Substring(3, 4);
                }
                else
                    tabela.Valor = param.Valor;
                
                dados.Add(tabela);
            }

            gridParametrosGerais.DataSource = dados.ToList();
            gridParametrosGerais.DataBind();

        }

        public void carregaParametrosExecucao()
        {

            //List<EmpresaSolicitacaoTipo> dados = FachadaDashBoardConsignante.listaPrazosExecucao(Sessao.IdModulo.ToString());
            List<EmpresaSolicitacaoTipo> dados = FachadaDashBoardConsignante.listaSolicitacoesTipo().Where(x => x.Prazo > 0).ToList();

            gridParametrosExecucao.DataSource = dados.ToList();
            gridParametrosExecucao.DataBind();

        }
    
        public void mostraGraficoEnviadosDescontados(int nMeses)
        {

            List<ADescontar_E_Descontados> dados = FachadaDashBoardConsignante.listaADescontarVDescontados(nMeses);

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

        protected void selecionaOpcaoOcorrencia_Click(object sender, EventArgs e)
        {
            AtualizaSolicitacoesPendencias();
        }

        protected void selecionaAverbacoes_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            if (DropDonwListOpcao.SelectedValue == "1")
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, id);
            }
            else
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, id, true);
            }
        }

        private void AtualizaSolicitacoesPendencias()
        {            
            List<object> dados;

            dados = FachadaMaster.ObtemSolicitacoes(Convert.ToInt32(DropDonwListOpcao.SelectedValue), 0, 0);

            gridSolicitacoesPendencias.DataSource = dados;
            gridSolicitacoesPendencias.DataBind();

        }

        private void AtualizaOcorrencias(int tipo)
        {

            List<EmpresaSolicitacao> solic = FachadaDashBoardConsignante.listaSolicitacoes().Where( x => x.IDEmpresa != null).ToList();

            List<Empresa> bancos = FachadaDashBoardConsignante.listaConsignatarias();            

            var solicPorEmpresa = from s in solic
                                  where (s.IDEmpresaSolicitacaoTipo == tipo) && (s.Empresa.IDEmpresaTipo == 4)
                                  group s by s.Empresa into g
                                  select new { ID = g.Key.IDEmpresa ,Logo = ObtemCaminhoLogoConsignataria((int)g.Key.IDContribuinteFastConsig), Banco = g.Key.Nome, Contratos = g.Count() };
            
            gridOcorrencias.DataSource = solicPorEmpresa;
            gridOcorrencias.DataBind();

        }

        private string ObtemCaminhoLogoConsignataria(int idConsignataria)
        {
            using (ServicoUsuarioClient ServicoUsuario = new ServicoUsuarioClient())
            {
                var empresa = ServicoUsuario.ObtemConsignataria(idConsignataria);
                if (empresa != null)
                    return string.Format(PathImagensLogos, empresa.Logo);
                else
                    return string.Empty;
            }
        }


        protected void selecionaTipoSolicitacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizaOcorrencias( Convert.ToInt32(DropDownListSolicitacaoTipo.SelectedValue) );
        }

        protected void AdicionarSolicitacao_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            int tipo = Convert.ToInt32(DropDownListSolicitacaoTipo.SelectedValue);
            string nome = FachadaDashBoardConsignante.obtemEmpresa(id).Nome;

            EmpresaSolicitacaoTipo s = FachadaDashBoardConsignante.obtemSolicitacaoTipo(tipo);

            FachadaDashBoardConsignante.AdicionaSolicitacao_Alerta(Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante()), id, tipo, Sessao.UsuarioLogado.IDUsuario);
           
            PageMaster.ExibeMensagem( String.Format( ResourceMensagens.MensagemAlerta, nome) );
        }

        protected void SuspenderConsignataria_Click(object sender, EventArgs e)
        {

            int idEmpresa = Convert.ToInt32(((ImageButton) sender).CommandArgument);

            EmpresaSuspensao empresaSuspensao = FachadaDashBoardConsignante.ObtemEmpresaSuspensao(idEmpresa);

            int idEmpresaSuspensao = empresaSuspensao == null ? 0 : empresaSuspensao.IDEmpresaSuspensao;

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSuspensoesEdicao, IdRecurso, 1, idEmpresaSuspensao, idEmpresa);

        }

        protected void gridOcorrencias_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {

                ImageButton imgSuspender = (ImageButton)gridOcorrencias.FindRowCellTemplateControl(e.VisibleIndex, null, "imgSuspender");

                int idEmpresa = Convert.ToInt32(gridOcorrencias.GetRowValues(e.VisibleIndex, "ID"));

                bool empresaBloqueada = FachadaLogin.EmpresaBloqueada(idEmpresa);

                imgSuspender.ImageUrl = empresaBloqueada ? CaminhoImagemTick : CaminhoImagemCross;

            }

        }

    }
}