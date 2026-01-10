using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using DevExpress.Web.ASPxGridView;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public enum TipoValor
    {
        Volume = 1,
        Percentual = 2
    }

    public partial class WebUserControlAnaliseInadimplencia : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            Parametro param = FachadaDashBoardConsignante.obterParametro("AtivaAnaliseInadimplencia");
            if ((param == null) || (param.Valor.ToUpper() != "S"))
            {
                PageMaster.ExibeMensagem("Ainda não houve importação do arquivo de retorno da folha de pagamento. Esta atualização deve ocorrer logo após o fechamento da folha.");
                PageMaster.Voltar();
                return;
            }
            PopulaGridInadimplenciaTempo();

            if (ControleCarregado) return;            
            
            if (EhPostBack)
            {
                return;
            }

            EhPostBack = true;

            List<Empresa> consignatarias = FachadaConsignatarias.ListaConsignatarias().ToList();
            Empresa emp = new Empresa();
            emp.IDEmpresa = -1;            

            consignatarias.Add(emp);
            DropDownListConsignataria.DataSource = consignatarias.OrderBy(x => x.Nome);
            DropDownListConsignataria.DataBind();

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();
                DropDownListConsignataria.Visible = false;
                LabelConsignataria.Visible = false;
                ASPxRoundPanelInadimplenciaDetalhada.Visible = false;
                mostraGraficoVolumeInadimplencia(TipoValor.Volume, Convert.ToInt32(DropDownListConsignataria.SelectedValue));

                populaDados(false);
            }
            else
            {
                DropDownListConsignataria.SelectedValue = "-1";

                DateTime dtData = DateTime.Now.AddMonths(-1);

                string competenciaatual = String.Format("{0:00}/{1}", dtData.Month, dtData.Year );

                LabelCompetencia.Text = String.Format("{0}", competenciaatual);

                LabelPeriodo.Text = String.Format("Período de Análise: {0}", competenciaatual);

                
                ASPxRoundPanelPadraoInadimplencia.Visible = false;
                ASPxRoundPanelInadimplenciaDetalhada.Visible = false;
                ASPxRoundPanelRecuperavel.Visible = false;
                ASPxRoundPanelNaoRecuperavel.Visible = false;
                ASPxRoundPanelGraficoInadimplenciaGeral.Visible = false;
                ASPxRoundPanelGraficoVolumeInadimplencia.Visible = false;
                btRecuperavel.Visible = false;
                btNaoRecuperavel.Visible = false;

            }

        }

        public void populaDados(bool navegarPeriodo = false, string competencia = "")
        {
            string competenciaatual;

            if (!navegarPeriodo)
            {
                if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                {
                    calculaPeriodoIndiceNegocios(out competenciaatual, Sessao.IdBanco);
                }
                else
                {
                    calculaPeriodoIndiceNegocios(out competenciaatual, Convert.ToInt32(DropDownListConsignataria.SelectedValue));
                }
            }
            else
            {
                competenciaatual = competencia;
            }

            LabelCompetencia.Text = String.Format("{0}", competenciaatual);

            LabelPeriodo.Text = String.Format("Período de Análise: {0}", competenciaatual);

            int idempresa;

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                idempresa = Sessao.IdBanco;
            }
            else
            {
                idempresa = Convert.ToInt32(DropDownListConsignataria.SelectedValue);
            }

            mostraGraficoInadimplenciaGeral(competenciaatual, idempresa);
            mostraGraficoVolumeInadimplencia(TipoValor.Volume, Convert.ToInt32(DropDownListConsignataria.SelectedValue));
            PopulaGridPadraoTrabalho(competenciaatual, idempresa);
            PopulaGridPadraoMargem(competenciaatual, idempresa);

            ASPxRoundPanelPadraoInadimplencia.Visible = true;
            btRecuperavel.Visible = true;
            btNaoRecuperavel.Visible = true;
        }

        public void calculaPeriodoIndiceNegocios(out string competencia, int idempresa)
        {
            competencia = FachadaConciliacao.ultimaCompetenciaConciliada(idempresa);//FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, idempresa);
            if (competencia != "")
            {
                competencia = Utilidades.ConverteMesAno(competencia);
            }
            else
            {
                competencia = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, idempresa);
                competencia = Utilidades.CompetenciaDiminui(competencia);
                competencia = Utilidades.ConverteMesAno(competencia);
            }

        }

        public void mostraGraficoInadimplenciaGeral(string competencia, int idempresa)
        {
            List<TmpInadimplenciaGeral> dados = FachadaAnaliseInadimplencia.listaInadimplenciaGeral(Utilidades.ConverteAnoMes(competencia), idempresa).ToList();

            Dictionary<string, decimal> serieValores = new Dictionary<string, decimal>();

            List<Dictionary<string, decimal>> drillDown = new List<Dictionary<string, decimal>>();

            int i = 0;

            foreach (var item in dados)
            {

                serieValores.Add(item.Descricao, (decimal)item.Percentual);

                List<TmpInadimplenciaGeralDetalhe> detalhe = FachadaAnaliseInadimplencia.listaInadimplenciaGeralDetalhe(item.Id).ToList();

                Dictionary<string, decimal> dicionario = new Dictionary<string, decimal>();
                
                foreach (var x in detalhe) 
                    if (x.IDInadimplenciaGeral != 1)
                       dicionario.Add(x.Descricao.Trim(), (decimal)x.Percentual);

                drillDown.Add(dicionario);

            }            

            WebUserControlChartPizzaInadimplenciaGeral.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartPizzaInadimplenciaGeral, "Analisando " + competencia, string.Empty, serieValores, drillDown.ToArray());
            ASPxRoundPanelGraficoInadimplenciaGeral.Visible = true;
        }

        protected void gridPadraoTrabalho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }


        private void PopulaGridPadraoTrabalho(string competencia, int idempresa)
        {
            gridPadraoTrabalho.DataSource = FachadaAnaliseInadimplencia.listaInadimplenciaPadraoTrabalho(Utilidades.ConverteAnoMes(competencia), idempresa).OrderByDescending( x => x.Percentual ).ToList();
            gridPadraoTrabalho.DataBind();
        }

        private void PopulaGridPadraoMargem(string competencia, int idempresa)
        {
            gridPadraoMargem.DataSource = FachadaAnaliseInadimplencia.listaInadimplenciaPadraoMargem(Utilidades.ConverteAnoMes(competencia), idempresa).ToList();
            gridPadraoMargem.DataBind();
        }

        private void PopulaGridInadimplenciaTempo()
        {
            //List<TmpInadimplenciaTempoDetalhe> grupo = FachadaAnaliseInadimplencia.listaInadimplenciaTempoDetalhe().ToList();

            //gridInadimplenciaDetalhada.DataSource = grupo;
            //gridInadimplenciaDetalhada.DataBind();
        }

        public void mostraGraficoVolumeInadimplencia(TipoValor tipo, int idempresa)        
        {

            ASPxRoundPanelGraficoVolumeInadimplencia.Visible = true;
            
            List<TmpVolumeInadimplencia> dados = FachadaAnaliseInadimplencia.listaVolumeInadimplencia(idempresa).ToList();

            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosTotais = new List<decimal>();

            List<string> categorias = new List<string>();

            if (tipo == TipoValor.Volume)
            {
                foreach (var item in dados)
                {
                    categorias.Add(Utilidades.ConverteMesAno(item.Mes));
                    dadosTotais.Add((decimal)item.SomaParcelas);
                }

                serieValores.Add("Volume", dadosTotais.ToArray());
            }
            else
            {
                int meses = 0;
                decimal? soma = 0; decimal? somaadic = 0;
                //foreach (var item in dados)
                //{
                //    meses++;
                //    soma += (decimal)item.SomaParcelas;
                //}

                foreach (var item in dados)
                {
                    categorias.Add(Utilidades.ConverteMesAno(item.Mes));
                    decimal perc = FachadaAnaliseInadimplencia.ValorInadimplencia(item.Mes, idempresa, "Inadimplente");
                    //FachadaAnaliseInadimplencia.VolumeValorAverbacoes(item.Mes, idempresa, (int)Enums.ProdutoGrupo.Emprestimos, out soma, out somaadic);                     

                    dadosTotais.Add(perc);
                }
                serieValores.Add("Percentual", dadosTotais.ToArray());
            }
                                                                    
            if (tipo == TipoValor.Percentual)
                WebUserControlChartAreaVolumeInadimplencia.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartAreaVolumeInadimplenciaPercentual, (tipo == TipoValor.Volume ? "Volume de Inadimplência" : "Percentual de Inadimplência"), string.Empty, (tipo == TipoValor.Volume ? "Total" : "Percentual"), categorias.ToArray(), serieValores);
            else
                WebUserControlChartAreaVolumeInadimplencia.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartAreaVolumeInadimplencia, (tipo == TipoValor.Volume ? "Volume de Inadimplência" : "Percentual de Inadimplência"), string.Empty, (tipo == TipoValor.Volume ? "Total" : "Percentual"), categorias.ToArray(), serieValores);            

        }

        protected void RadioButtonListTipoValor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonListTipoValor.SelectedValue == "1")
            {
                mostraGraficoVolumeInadimplencia(TipoValor.Volume, Convert.ToInt32(DropDownListConsignataria.SelectedValue));
            }
            else
            {
                mostraGraficoVolumeInadimplencia(TipoValor.Percentual, Convert.ToInt32(DropDownListConsignataria.SelectedValue));
            }
        }

        protected void DropDownListConsignataria_SelectedIndex(object sender, EventArgs e)
        {
            populaDados(true, LabelCompetencia.Text);
        }

        protected void Recuperavel_Click(object sender, EventArgs e)
        {
            List<TmpRecuperavelPorFolha> rec = FachadaAnaliseInadimplencia.listaRecuperavelPelaFolha(Utilidades.ConverteAnoMes(LabelCompetencia.Text), Convert.ToInt32(DropDownListConsignataria.SelectedValue)).ToList();
            ASPxGridViewRecuperavel.DataSource = rec;
            ASPxGridViewRecuperavel.DataBind();

            ASPxRoundPanelRecuperavel.Visible = true;
            ASPxRoundPanelNaoRecuperavel.Visible = false;
        }

        protected void NaoRecuperavel_Click(object sender, EventArgs e)
        {
            List<TmpNaoRecuperavel> naorec = FachadaAnaliseInadimplencia.listaNaoRecuperavel(Utilidades.ConverteAnoMes(LabelCompetencia.Text), Convert.ToInt32(DropDownListConsignataria.SelectedValue)).ToList();
            ASPxGridViewNaoRecuperavel.DataSource = naorec;
            ASPxGridViewNaoRecuperavel.DataBind();

            ASPxRoundPanelRecuperavel.Visible = false;
            ASPxRoundPanelNaoRecuperavel.Visible = true;
        }

        protected void selecionaPeriodoAnterior_Click(object sender, EventArgs e)
        {
            string competencia = Utilidades.ConverteMesAno(Utilidades.CompetenciaDiminui(Utilidades.ConverteAnoMes(LabelCompetencia.Text)));

            LabelCompetencia.Text = competencia;

            populaDados(true, competencia);

        }

        protected void selecionaPeriodoProximo_Click(object sender, EventArgs e)
        {
            string competencia = Utilidades.ConverteMesAno(Utilidades.CompetenciaAumenta(Utilidades.ConverteAnoMes(LabelCompetencia.Text), 1));

            LabelCompetencia.Text = competencia;

            populaDados(true, competencia);
        }
        
        /*
         protected void gridInadimplenciaTempoDetalhe_DataSelect(object sender, EventArgs e)
         {

             ASPxGridView gridMaster = (sender as ASPxGridView);

             int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
             var dados = FachadaAnaliseInadimplencia.listaInadimplenciaTempoDetalhe(id);

             gridMaster.DataSource = dados.ToList();

         }
          */
    
    }

}