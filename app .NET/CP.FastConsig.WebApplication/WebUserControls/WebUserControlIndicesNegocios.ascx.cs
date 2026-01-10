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
    public partial class WebUserControlIndicesNegocios : CustomUserControl
    {
        bool bApenasDoUsuario;
        bool bMostrarLucro;
        private const string FormatoDinheiro = "{0:C}";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if ((ControleCarregado) || EhPostBack)
                return;

            EhPostBack = true;            
                      
            bApenasDoUsuario = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.IndiceNegocioApenasContratosRegistradosPeloUsuario);
            bMostrarLucro = FachadaPermissoesAcesso.CheckPermissao((int)Enums.Recursos.DashBoardConsignataria, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.PermitirMostrarLucroIndicesDeNegocio);

            populaDados(false);
        }

        private void populaDados(bool navegarPeriodo = false, string competencia = "")
        {

            string competenciaatual;

            if (!navegarPeriodo)
                calculaPeriodoIndiceNegocios(out competenciaatual, Sessao.IdBanco);
            else
            {
                competenciaatual = competencia;
            }

            LabelCompetencia.Text = String.Format("{0}", competenciaatual);            

            LabelPeriodo.Text = String.Format("Período de Análise: {0}", competenciaatual);
            
            int idempresa = Sessao.IdBanco;
            
            populaIndicesConquistado(Utilidades.ConverteAnoMes(competenciaatual), idempresa);
            populaIndicesPerdas(Utilidades.ConverteAnoMes(competenciaatual), idempresa);
            populaIndicesAConquistar(Utilidades.ConverteAnoMes(competenciaatual), idempresa);
        }

        private void populaIndicesAConquistar(string competencia, int idempresa)
        {
            ASPxGridViewAConquistar.DataSource = RetornaIndicesAConquistar(competencia, idempresa);
            ASPxGridViewAConquistar.DataBind();
        }

        private void populaIndicesPerdas(string competencia, int idempresa)
        {
            ASPxGridViewPerdas.DataSource = RetornaIndicesPerdas(competencia, idempresa);
            ASPxGridViewPerdas.DataBind();
        }

        private string ObtemDescricaoAverbacaoTipo(int tipo)
        {
            if (tipo == 0)
                return "Sem Tipo";
            else
                return FachadaAverbacoes.ObtemDescricaoAverbacaoTipo(tipo);
        }

        private void populaIndicesConquistado(string competencia, int idempresa)
        {
            //List<object> dados = new List<object>();

            //IQueryable<Averbacao> averbacoes;

            //if (!bApenasDoUsuario)
            //{
            //    averbacoes = FachadaDashBoardConsignataria.listaAverbacoesPorTipo(datai, dataf, Sessao.UsuarioLogado.IDUsuario);
            //}
            //else
            //{
            //    averbacoes = FachadaDashBoardConsignataria.listaAverbacoesPorTipo(datai, dataf);
            //}

            //IQueryable<IGrouping<int, Averbacao>> agrupado = averbacoes.GroupBy(x => x.IDAverbacaoTipo);

            //foreach (IGrouping<int, Averbacao> grupo in agrupado)
            //    dados.Add(new { IDAverbacaoTipo = grupo.Key, Descricao = ObtemDescricaoAverbacaoTipo(grupo.Key), Qtde = grupo.Count(), ValorBruto = grupo.Select(x => x.ValorDevidoTotal).Sum(), ValorAdicionado = grupo.Select(x => x.ValorDevidoTotal - x.ValorRefinanciado).Sum(), ValorLucro = grupo.Select(x => x.ValorDevidoTotal - x.ValorContratado).Sum() });

            ASPxGridViewConquistado.Columns["LucroBruto"].Visible = bMostrarLucro;

            List<IndiceNegocioRealizado> dados = FachadaIndicesNegocios.indicesConquistado(competencia, idempresa).ToList();
            ASPxGridViewConquistado.DataSource = dados;
            ASPxGridViewConquistado.DataBind();
        }


        private List<IndiceNegocioRealizado> RetornaIndicesConquistado(string competencia, int idempresa)
        {
            //List<IndiceNegocio> indices = new List<IndiceNegocio>();

            //IndiceNegocio indice = new IndiceNegocio("Novos Contratos", 20, 216873.22, 96403.22, "Valor Consignado Total", "");
            //IndiceNegocio indice2 = new IndiceNegocio("Refinanciamentos", 12, 35501.99, 12305.59, "Valor Consignado Total - Valor Consignado Antigo", "");
            //IndiceNegocio indice3 = new IndiceNegocio("Compras de Dívida", 7, 352443.05, 152993.88, "Valor Consignado Total", "");
            //indices.Add(indice);
            //indices.Add(indice2);
            //indices.Add(indice3);

            //return indices;

            return FachadaIndicesNegocios.indicesConquistado(competencia, idempresa).ToList();
        }

        private List<IndiceNegocioRealizar> RetornaIndicesAConquistar(string competencia, int idempresa)
        {
            //List<IndiceNegocio> indices = new List<IndiceNegocio>();

            //IndiceNegocio indice = new IndiceNegocio(12, "R$ 2.700,00", 194000, 48000);
            //indices.Add(indice);

            //return indices;
            return FachadaIndicesNegocios.indicesAConquistar(competencia, idempresa).ToList();
        }

        private List<IndiceNegocioAntecipar> RetornaIndicesPerdas(string competencia, int idempresa)
        {
            //List<IndiceNegocio> indices = new List<IndiceNegocio>();

            //IndiceNegocio indice = new IndiceNegocio("Perdeu por Compra de Dívida", 10, 46123.22, 96403.22, "Valor Consignado Total", "");
            //IndiceNegocio indice2 = new IndiceNegocio("Perdeu por não Acompanhar", 22, 335501.99, 192305.59, "Valor Consignado Total - Valor Consignado Antigo", "");
            //indices.Add(indice);
            //indices.Add(indice2);

            //return indices;
            return FachadaIndicesNegocios.indicesAntecipar(competencia, idempresa).ToList();
        }

        public void calculaPeriodoIndiceNegocios(out string competencia, int idempresa)
        {
            competencia = FachadaAverbacoes.ObtemAnoMesCorte((int)Enums.Modulos.Consignataria, idempresa); //FachadaDashBoardConsignante.obterParametro("AverbadosMes").Valor;
            competencia = Utilidades.CompetenciaDiminui(competencia);
            competencia = Utilidades.ConverteMesAno(competencia);
        }

        protected void ASPxGridViewPerdas_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            if (e.Item.FieldName == "Qtde")
                e.Text = string.Format("{0}", e.Value);
            else
                e.Text = string.Format(FormatoDinheiro, e.Value);

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

    }
}