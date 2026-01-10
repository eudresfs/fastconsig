using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Importacoes
    {

        public static void SalvaLayout(string filtros, string tabela, string nomeLayout, int idUsuario, int idBanco, string colunas)
        {
            
            Repositorio<ImportacaoLayout> repositorio = new Repositorio<ImportacaoLayout>();

            ImportacaoLayout importacaoLayout = new ImportacaoLayout();

            importacaoLayout.Ativo = 1;
            importacaoLayout.Filtros = filtros;
            importacaoLayout.NomeTabela = tabela;
            importacaoLayout.NomeLayout = nomeLayout;
            importacaoLayout.IdUsuario = idUsuario;
            importacaoLayout.IdBanco = idBanco;
            importacaoLayout.Colunas = colunas;

            repositorio.Incluir(importacaoLayout);
            
        }

        public static List<ImportacaoLayout> ObtemLayoutsSalvos(string nomeTabela, int idBanco)
        {
            return new Repositorio<ImportacaoLayout>().Listar().Where(x => x.NomeTabela.Equals(nomeTabela) && x.IdBanco.Equals(idBanco)).ToList();
        }

        public static ImportacaoLayout ObtemImportacaoLayout(int idImportacaoLayout)
        {
            return new Repositorio<ImportacaoLayout>().ObterPorId(idImportacaoLayout);
        }

        public static void RemoveImportacaoLayout(int idImportacaoLayout)
        {
            new Repositorio<ImportacaoLayout>().Excluir(idImportacaoLayout);
        }

        public static int SalvaDadosImportacao(int idUsuario, string nomeArquivo, bool incluirPrimeiraLinha, string layout, string nomeLayout, string observacao, string telefone, int idBanco)
        {

            Repositorio<Importacao> repositorioImportacao = new Repositorio<Importacao>();

            Importacao importacao = new Importacao();

            importacao.Data = DateTime.Now;
            importacao.NomeArquivo = Path.GetFileName(nomeArquivo);

            importacao.Ativo = 1;
            importacao.IdUsuario = idUsuario;
            importacao.IncluirPrimeiraLinha = incluirPrimeiraLinha;
            importacao.Layout = layout;
            importacao.NomeLayout = nomeLayout;
            importacao.Observacao = observacao;
            importacao.Telefone = telefone;
            importacao.IdBanco = idBanco;

            repositorioImportacao.Incluir(importacao);

            return importacao.IdImportacao;

        }

    }

}