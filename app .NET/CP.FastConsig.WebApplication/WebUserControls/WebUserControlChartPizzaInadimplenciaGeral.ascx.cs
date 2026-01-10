using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlChartPizzaInadimplenciaGeral : CustomUserControl
    {

        #region Contantes

        private const string TagTitulo = "#titulo";
        private const string TagSubTitulo = "#subTitulo";
        private const string TagDados = "#dados";
        private const string FormatoDados = "{{name:'{0}', y:{1}, color: colors[{2}], drilldown: #drilldown}}";
        private const string FormatoDrillDown = "{{y:{0}, name: '{1}'}}";
        private const string SeparadorDados = ",";

        #endregion

        private string ReplaceFirst(string haystack, string needle, string replacement)
        {

            int pos = haystack.IndexOf(needle);
            if (pos < 0) return haystack;

            return haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);

        }

        public void ConfiguraGrafico(string nomeArquivoScriptChartPizza, string titulo, string subTitulo, Dictionary<string, decimal> dados, Dictionary<string, decimal>[] drilldown)
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();

            int j = 0;

            tagsValores.Add(TagTitulo, titulo);

            tagsValores.Add(TagSubTitulo, subTitulo);
            string tag = string.Join(SeparadorDados, dados.Select(x => string.Format(FormatoDados, x.Key.Trim(), x.Value.ToString().Replace(",","."), j++)).ToArray());

            int i = 0;

            while (tag.IndexOf("#drilldown") != -1)
            {
                tag = ReplaceFirst(tag, "#drilldown", "{data: ["+string.Join(SeparadorDados, drilldown[i].Select(x => string.Format(FormatoDrillDown, x.Value.ToString().Replace(",", "."), x.Key.Trim())).ToArray()) + "]}" );  //String.Format(FormatoDrillDown, drilldown[i].Values[0]., drilldown[i].Keys));
                i++;
            }

            tag = tag.Replace(", drilldown: {data: []}", "");

            tagsValores.Add(TagDados, tag);

            LimpaScripts();
            AdicionaArquivoScriptParaExecucao(nomeArquivoScriptChartPizza, tagsValores);

        }
        
    }

}