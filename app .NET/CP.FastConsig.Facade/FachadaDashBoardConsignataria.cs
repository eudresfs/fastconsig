using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;

namespace CP.FastConsig.Facade
{
    public static class FachadaDashBoardConsignataria
    {
        public static List<TmpEnviadosVDescontados> listaEnviadosVDescontados()
        {
            return Consignatarias.listaEnviadosVDescontados().ToList();
        }

        public static Pessoa obtemPessoa(string matriculaCpf)
        {
            return Funcionarios.ObtemPessoa(matriculaCpf);
        }

        public static List<TmpIndicesNegocios> obtemIndicesNegocios(int tipo)
        {
            return Consignatarias.obtemIndicesNegocios( tipo ).ToList();
        }

        public static List<ADescontar_E_Descontados> listaADescontarVDescontados(int nMeses, int idempresa)
        {
            List<ADescontar_E_Descontados> dados = new List<ADescontar_E_Descontados>();

            DateTime dtDataAtual = DateTime.Now.Date;

            for (int i = nMeses; i >= 1; i--)
            {
                DateTime dtData = dtDataAtual.AddMonths(i * (-1));

                string competencia = dtData.Year.ToString() + "/" + dtData.Month.ToString().PadLeft(2, '0');

                decimal? valorADescontar = Consignantes.ADescontar(competencia, idempresa);

                decimal? valorDescontados = Consignantes.Descontados(competencia, idempresa);

                ADescontar_E_Descontados dadoNaoDescontado = new ADescontar_E_Descontados();
                dadoNaoDescontado.Mes = Utilidades.ConverteMesAno(competencia);
                dadoNaoDescontado.Tipo = "A Descontar";
                dadoNaoDescontado.Valor = (decimal)valorADescontar;

                dados.Add(dadoNaoDescontado);

                ADescontar_E_Descontados dadoDescontado = new ADescontar_E_Descontados();
                dadoDescontado.Mes = Utilidades.ConverteMesAno(competencia);
                dadoDescontado.Tipo = "Descontados";
                dadoDescontado.Valor = (decimal)valorDescontados;

                dados.Add(dadoDescontado);

            }

            return dados;
        }


        public static IQueryable<Averbacao> listaAverbacoesPorTipo(DateTime datai, DateTime dataf, int idusuario = 0)
        {
            if (idusuario == 0)
            {
                return new Repositorio<Averbacao>().Listar().Where(x => x.Data >= datai && x.Data <= dataf);
            }
            else
            {
                return new Repositorio<Averbacao>().Listar().Where(x => x.Data >= datai && x.Data <= dataf && x.IDUsuario == idusuario);
            }
        }
    }
}
