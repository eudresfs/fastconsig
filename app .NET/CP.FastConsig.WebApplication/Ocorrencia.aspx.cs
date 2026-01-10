using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using System.Security.Cryptography;
using System.Text;

namespace CP.FastConsig.WebApplication
{
    public partial class Ocorrencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var dados = FachadaConsignatarias.ListaConsignatarias();
            //string teste = Seguranca.getMd5Hash("1234");
            //string u = GetMD5Hash("1234xcaseconsigx");
            //for (int i = 0; i < 35; i++)
           // {
            //    Response.Write("Data=" + DateTime.Today.AddDays(Convert.ToDouble(i)).ToString() + "    corte=" + FachadaAverbacoes.ObtemAnoMesCorte(DateTime.Today.AddDays(Convert.ToDouble(i)), 3, 1026)+ "<br />");               
           // }
            Response.Write("Resolucao="+ Request.ServerVariables["HTTP_UA_PIXELS"]);
        }

        public static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}