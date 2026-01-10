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

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlChartVolumeAverbacoes : CustomUserControl
    {

        #region Contantes

        private const string TagTitulo = "#titulo";
        private const string TagSubTitulo = "#subTitulo";
        private const string TagDados = "#dados";
        private const string FormatoDados = "{{name:'{0}', data:[{1}]}}";
        private const string SeparadorDados = ",";
        private const string TagTituloEixoY = "#eixoY";
        private const string TagCategorias = "#categorias";

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((EhPostBack) || (ControleCarregado)) return;

            ConfiguraTopo();
           
            DropDownListProduto.DataSource = FachadaVolumeAverbacoes.ListaProdutosGrupo(Sessao.IdBanco).ToList();
            DropDownListProduto.DataBind();

            mostraGrafico(Convert.ToInt32(DropDownListVolume.SelectedValue), (int)ASPxSpinEditMeses.Number, Convert.ToInt32(DropDownListProduto.SelectedValue));

            EhPostBack = true;

        }

        public void mostraGrafico(int tipo, int qtdemeses, int idprodutogrupo)
        {

            List<VolumeAverbacoes> dados = FachadaVolumeAverbacoes.listaVolumeAverbacoes( tipo, qtdemeses, Sessao.IdBanco, idprodutogrupo);

            var meses = dados.Select(x => x.Mes).Distinct();

            List<string> aMeses = meses.ToList();
            
            Dictionary<string, decimal[]> serieValores = new Dictionary<string, decimal[]>();

            List<decimal> dadosValorBruto = new List<decimal>();
            List<decimal> dadosValorAdicionado = new List<decimal>();

            List<string> categorias = new List<string>();

            foreach (var item in aMeses)
            {
                categorias.Add((string)item.Trim());
            }

            if (tipo == 1)
            {
                foreach (var item in dados)
                {
                    dadosValorBruto.Add(item.ValorBruto ?? 0);
                    dadosValorAdicionado.Add(item.ValorAdicionado ?? 0);
                }
                serieValores.Add("Valor Bruto", dadosValorBruto.ToArray());
                serieValores.Add("Valor Adicionado", dadosValorAdicionado.ToArray());
                ConfiguraGrafico("ChartBarraVolumeAverbacoes", "Gráfico - Volume Total de Averbações", string.Empty, "Volume Total", categorias.ToArray(), serieValores);
            }
            else
            {
                foreach (var item in dados)
                {
                    dadosValorBruto.Add(item.ValorBruto ?? 0);
                }
                serieValores.Add("Parcelas", dadosValorBruto.ToArray());
                ConfiguraGrafico("ChartBarraVolumeAverbacoes", "Gráfico - Volume de Parcelas", string.Empty, "Valores de Parcelas", categorias.ToArray(), serieValores);
            }

        }

        protected void atualizaGrafico_Click(object sender, EventArgs e)
        {
            mostraGrafico(Convert.ToInt32(DropDownListVolume.SelectedValue), (int)ASPxSpinEditMeses.Number, Convert.ToInt32( DropDownListProduto.SelectedValue ));
        }

        
        private void ConfiguraTopo()
        {

        }

        public void ConfiguraGrafico(string nomeArquivoScriptChartBarra, string titulo, string subTitulo, string tituloEixoY, string[] categorias, Dictionary<string, decimal[]> dados)
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();

            tagsValores.Add(TagTitulo, titulo);
            tagsValores.Add(TagSubTitulo, subTitulo);
            tagsValores.Add(TagTituloEixoY, tituloEixoY);
            tagsValores.Add(TagCategorias, string.Join(SeparadorDados, categorias.Select(x => string.Format("'{0}'", x)).ToArray()));
            tagsValores.Add(TagDados, string.Join(SeparadorDados, string.Join(SeparadorDados, dados.Select(x => string.Format(FormatoDados, x.Key, string.Join(SeparadorDados, x.Value.Select(y => y.ToString().Replace(",", "."))))).ToArray())));

            LimpaScripts();
            AdicionaArquivoScriptParaExecucao(nomeArquivoScriptChartBarra, tagsValores);

        }
    }
}