using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.DAL;
using System.Web.Script.Serialization;
using System.Text;
using CP.FastConsig.Facade;

namespace CP.FastConsig.WebApplication
{
    public class AverbacaoVinc
    {
        public string Numero { get; set; }
        public string Consignataria { get; set; }
        public string Prazo { get; set; }
        public string ValorParcela { get; set; }
    }

    public partial class ListaAverbacoesVinc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];

            Averbacao a = FachadaAverbacoes.ObtemAverbacao(Convert.ToInt32(id));

            List<AverbacaoVinc> lista = new List<AverbacaoVinc>();
            foreach (var item in a.AverbacaoVinculo1)
            {
                lista.Add(new AverbacaoVinc() { Numero = item.Averbacao.Numero, Consignataria = item.Averbacao.Empresa1.Nome, Prazo = item.Averbacao.Prazo.HasValue ? item.Averbacao.Prazo.Value.ToString() : "", ValorParcela = string.Format("{0:N}",item.Averbacao.ValorParcela) });
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sbRes = new StringBuilder();
            jss.Serialize(lista, sbRes);

            Response.Clear();
            Response.Write(sbRes.ToString());
            Response.End();
        }
    }
}