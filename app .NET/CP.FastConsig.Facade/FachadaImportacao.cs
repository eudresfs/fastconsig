using System.Collections.Generic;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaImportacao
    {

        public static void SalvaLayout(string filtros, string tabela, string nomeLayout, int idUsuario, int idBanco, string colunas)
        {
            Importacoes.SalvaLayout(filtros, tabela, nomeLayout, idUsuario, idBanco, colunas);
        }

        public static List<ImportacaoLayout> ObtemLayoutsSalvos(string nomeTabela, int idBanco)
        {
            return Importacoes.ObtemLayoutsSalvos(nomeTabela, idBanco);
        }

        public static ImportacaoLayout ObtemImportacaoLayout(int idImportacaoLayout)
        {
            return Importacoes.ObtemImportacaoLayout(idImportacaoLayout);
        }

        public static void RemoveImportacaoLayout(int idImportacaoLayout)
        {
            Importacoes.RemoveImportacaoLayout(idImportacaoLayout);
        }

        public static int SalvaDadosImportacao(int idUsuario, string nomeArquivo, bool incluirPrimeiraLinha, string layout, string nomeLayout, string observacao, string telefone, int idBanco)
        {
            return Importacoes.SalvaDadosImportacao(idUsuario, nomeArquivo, incluirPrimeiraLinha, layout, nomeLayout, observacao, telefone, idBanco);
        }


        public static void ImportaFuncionarios(int id)
        {
            Funcionarios.Importacao(id);
        }
    }

}