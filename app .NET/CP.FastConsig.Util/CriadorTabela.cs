using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CP.FastConsig.Util
{

    public static class CriadorTabela
    {

        public static void CriaTabela(string nomeTabela, IEnumerable<string> colunas)
        {

            string comandoCriacao = string.Format("if exists (select * from sysobjects where name='{0}' and xtype='U') drop table {0}; create table {0} ({1})", nomeTabela, string.Join(",", colunas.Select(x => string.Format("{0} varchar(255)", x)).ToArray()));

            SqlConnection conexao = new SqlConnection(ConfigurationManager.ConnectionStrings[HttpContext.Current.Session["NomeStringConexaoSemEntity"].ToString()].ToString());
            SqlCommand comando = new SqlCommand(comandoCriacao, conexao);

            using (conexao)
            {

                conexao.Open();
                comando.ExecuteNonQuery();
                conexao.Close();

            }

        }

    }

}