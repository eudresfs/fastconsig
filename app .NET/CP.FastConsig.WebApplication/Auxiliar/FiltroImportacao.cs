using System;

namespace CP.FastConsig.WebApplication.Auxiliar
{

    public class FiltroImportacao
    {

        public int IndiceColuna;

        public RegraFiltroImportacao Regra;

        public FiltroImportacao(string filtroString)
        {

            const int posicaoIndice = 0;
            const int posicaoNomeRegra = 1;
            const int posicaoValorA = 2;
            const int posicaoValorB = 3;

            Regra = new RegraFiltroImportacao();

            if (filtroString.StartsWith(",")) filtroString = filtroString.Remove(0, 1);
            if (filtroString.EndsWith(",")) filtroString = filtroString.Remove(filtroString.Length - 1, 1);

            string[] dados = filtroString.Split(new[] {','});

            IndiceColuna = Convert.ToInt32(dados[posicaoIndice]);

            Regra.NomeRegra = dados[posicaoNomeRegra];
            Regra.ValorA = dados[posicaoValorA];
            Regra.ValorB = dados[posicaoValorB];

        }

    }

}