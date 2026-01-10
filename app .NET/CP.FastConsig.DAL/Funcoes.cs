using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.Util;

namespace CP.FastConsig.DAL
{
    public static class Funcoes
    {
        public static string ConverteMesAno(string mesano)
        {
            if (string.IsNullOrEmpty(mesano))
                return "";
            return mesano.Substring(5, 2) + "/" + mesano.Substring(0, 4);
        }

        public static string ObtemAnoMesCorte(int idmodulo, int idempresa)
        {
            int somames = 0;
            int dia = DateTime.Today.Day;
            int ano = DateTime.Today.Year;
            int mes = DateTime.Today.Month;

            string anomes = ano.ToString() + "/" + mes.ToString().PadLeft(2, '0');

            int diacorte = Convert.ToInt32(new Repositorio<Parametro>().Listar().FirstOrDefault(x => x.Nome == "DiaCorte").Valor);

            CorteHistorico ch = new Repositorio<CorteHistorico>().Listar().FirstOrDefault(x => x.Competencia == anomes);
            if (ch != null)
                diacorte = Convert.ToInt32(ch.DiaCorte);

            if (dia > diacorte)
                return Utilidades.CompetenciaAumenta(anomes, somames + 1);
            else
                return Utilidades.CompetenciaAumenta(anomes, somames);
        }
    }
}
