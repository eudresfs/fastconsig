using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;
using CP.FastConsig.BLL;
using System.Data;

namespace CP.FastConsig.Facade
{
    public class FachadaVolumeAverbacoes
    {
        public static List<VolumeAverbacoes> listaVolumeAverbacoes(int tipo, int meses, int idempresa, int idprodutogrupo)
        {
            List<VolumeAverbacoes> dados = new List<VolumeAverbacoes>();

            DateTime dtDataAtual = DateTime.Now.Date;

            string competencia = String.Format("{0}/{1}", dtDataAtual.Year, dtDataAtual.Month.ToString("00"));

            string primeiracompetencia = Utilidades.CompetenciaDiminui(competencia, meses);

            for (int i = meses; i >= 1; i--)
            {
                DateTime dtData = dtDataAtual.AddMonths((i-1) * (-1));

                competencia = String.Format("{0}/{1}", dtData.Year, dtData.Month.ToString("00"));  

                decimal? valorbruto = 0;
                decimal? valoradicionado = 0;

                if (tipo == 1)
                {
                    Consignatarias.VolumeValorAverbacoes(competencia, competencia, idempresa, idprodutogrupo, out valorbruto, out valoradicionado);
                }
                else
                {
                    valorbruto = Consignatarias.QtdeParcelasAverbacoes(competencia, idempresa, idprodutogrupo);
                }

                VolumeAverbacoes dado = new VolumeAverbacoes();
                
                dado.Mes = Utilidades.RetornaStringMes( dtData.Month );

                if (valorbruto == null)
                    dado.ValorBruto = 0;
                else
                    dado.ValorBruto = (decimal)valorbruto;

                if (valoradicionado == null)
                    dado.ValorAdicionado = 0;
                else
                    dado.ValorAdicionado = (decimal)valoradicionado;

                dados.Add(dado);

            }

            return dados;
        }

        public static List<ProdutoGrupo> ListaProdutosGrupo(int id)
        {
            List<ProdutoGrupo> grupos = new List<ProdutoGrupo>();
            
            var lista = new Repositorio<Produto>().Listar().Where(x => x.IDConsignataria == id);

            var prodGrupo = (from l in lista
                                      select new {IDProdutoGrupo=l.IDProdutoGrupo, Nome=l.ProdutoGrupo.Nome }).Distinct();

            foreach (var i in prodGrupo)
            {
                ProdutoGrupo grupo = new ProdutoGrupo();
                grupo.IDProdutoGrupo = i.IDProdutoGrupo;
                grupo.Nome = i.Nome;

                grupos.Add(grupo);
            }

            return grupos;
        }


    }
}
