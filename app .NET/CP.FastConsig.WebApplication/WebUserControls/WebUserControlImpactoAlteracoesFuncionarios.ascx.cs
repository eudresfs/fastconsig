using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;
using DevExpress.Web.ASPxGridView;


namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlImpactoAlteracoesFuncionarios : CustomUserControl
    {

        private string[] aMeses = { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado)
            {
                return;
            }

            EhPostBack = true;

            //List<Empresa> consignatarias = FachadaConsignatarias.ListaConsignatarias().ToList();
            //DropDownListConsignataria.DataSource = consignatarias.OrderBy( x => x.Nome );
            //DropDownListConsignataria.DataBind();

            //if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            //{
            //    DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();
            //    DropDownListConsignataria.Visible = false;
            //    IDLabelConsignataria.Visible = false;
            // }
            //else
            //{
                //DropDownListConsignataria.SelectedValue = "0";
            //}

            ASPxRoundPanelBoasNoticias.Visible = false;
            ASPxRoundPanelMaNoticias.Visible = false;

            popularDados();

        }

        private void popularDados(bool bNavega = false, string competencia = "")
        {
            int IDConsignataria;

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                IDConsignataria = Sessao.IdBanco;
                if (!bNavega)
                {
                    calculaCompetencia(out competencia, IDConsignataria);
                }                
            }
            else
            {
                IDConsignataria = 0;
                if (!bNavega)
                {
                    calculaCompetencia(out competencia, Convert.ToInt32(FachadaDashBoardConsignante.IDEmpresaConsignante()));
                }
            }

        
            List<TmpGrupoBoasNoticias> grupo = FachadaImpactoAlteracoesFuncionarios.ListaGrupoBoasNoticias(Utilidades.ConverteAnoMes(competencia)).ToList();

            gridAumentoMargens.DataSource = grupo;
            gridAumentoMargens.DataBind();
            gridAumentoMargens.DetailRows.ExpandRow(0);

            List<TmpMasNoticias> masnoticias = FachadaImpactoAlteracoesFuncionarios.listaMasNoticias(Utilidades.ConverteAnoMes(competencia), IDConsignataria).ToList();

            gridMasNoticias.DataSource = masnoticias.ToList();
            gridMasNoticias.DataBind();

            List<TmpMasNoticiasInadiplentes> inadimplentes = FachadaImpactoAlteracoesFuncionarios.listaMasNoticiasInadiplentes(Utilidades.ConverteAnoMes(competencia), IDConsignataria).ToList();

            gridMasNoticiasInadiplentes.DataSource = inadimplentes.ToList();
            gridMasNoticiasInadiplentes.DataBind();

            ASPxRoundPanelBoasNoticias.Visible = true;
            ASPxRoundPanelMaNoticias.Visible = true;

        }

        public void calculaCompetencia(out string competencia, int idempresa)
        {
            competencia = FachadaImpactoAlteracoesFuncionarios.ObtemUltimaConciliacao((int)Enums.Modulos.Consignataria, idempresa); // FachadaAverbacoes.ObtemAnoMesCorte((int)Enums.Modulos.Consignataria, idempresa); //FachadaDashBoardConsignante.obterParametro("AverbadosMes").Valor;
            competencia = Utilidades.CompetenciaDiminui(competencia);
            competencia = Utilidades.ConverteMesAno(competencia);
            LabelCompetencia.Text = competencia;
        }


        protected void ButtonAplicar_Click(object sender, EventArgs e)
        {

            DateTime datai = new DateTime(2011, 1, 1);
            DateTime dataf = new DateTime(2011, 12, 1);

            List<MargemFuncionarioHistorico> margens = FachadaImpactoAlteracoesFuncionarios.ListaMargemFuncionarioHistorico(datai, dataf).ToList();

            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosTotais = new List<decimal>();

            List<string> categorias = new List<string>();

            foreach (var item in margens)
            {
                categorias.Add(aMeses[(int)item.mes-1]+"/"+item.ano.ToString().Substring(2,2));
                dadosTotais.Add((decimal)item.soma);
            }

            serieValores.Add(ResourceMensagens.LabelGraficoMargemTotal, dadosTotais.ToArray());

            //WebUserControlChartBarraImpactoAlteracoesFuncionarios.ConfiguraGrafico(ResourceAuxiliar.NomeArquivoScriptChartBarraImpactoAlteracoesFuncionarios, "Movimentação de Margens", string.Empty, "Total", categorias.ToArray(), serieValores);
        }

        protected void gridBoasNoticias_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            
            IQueryable<TmpBoasNoticias> dados;

            dados = FachadaImpactoAlteracoesFuncionarios.obtemBoasNoticias(id, Utilidades.ConverteAnoMes(LabelCompetencia.Text), Sessao.IdModulo == (int)Enums.Modulos.Consignante ? 0 : Sessao.IdBanco); //Convert.ToInt32(DropDownListConsignataria.SelectedValue)

            gridMaster.DataSource = dados.ToList();

        }

        protected void gridBoasNoticiasDetalhe_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());

            var dados = FachadaImpactoAlteracoesFuncionarios.obtemBoasNoticiasDetalhe(id);

            gridMaster.DataSource = dados.ToList();

        }

        protected void gridMasNoticias_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());

            var dados = FachadaImpactoAlteracoesFuncionarios.obtemMasNoticiasDetalhe(id);

            gridMaster.DataSource = dados.ToList();

        }

        protected void gridMasNoticiasInadiplentes_DataSelect(object sender, EventArgs e)
        {

            ASPxGridView gridMaster = (sender as ASPxGridView);

            int id = Convert.ToInt32(gridMaster.GetMasterRowKeyValue());
            var dados = FachadaImpactoAlteracoesFuncionarios.obtemMasNoticiasInadiplentesDetalhe( id );

            gridMaster.DataSource = dados.ToList();

        }

        protected void ODS_TmpGrupoBoasNoticias_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = 1;
        }

        protected void ODS_TmpGrupoBoasNoticiasDetalhe_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = 1;
        }

        protected void Proximo_Click(object sender, EventArgs e)
        {
            string competencia = Utilidades.ConverteMesAno(Utilidades.CompetenciaAumenta(Utilidades.ConverteAnoMes(LabelCompetencia.Text), 1));
            LabelCompetencia.Text = competencia; 
            popularDados(true, competencia);
        }

        protected void Anterior_Click(object sender, EventArgs e)
        {
            string competencia = Utilidades.ConverteMesAno(Utilidades.CompetenciaDiminui(Utilidades.ConverteAnoMes(LabelCompetencia.Text)));
            LabelCompetencia.Text = competencia;
            popularDados(true, competencia);
        }

    }
}