using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{
    public class FachadaImpactoAlteracoesFuncionarios
    {
        public static IEnumerable<MargemFuncionarioHistorico> ListaMargemFuncionarioHistorico(DateTime datai, DateTime dataf)
        {
            return Consignatarias.ListaMargensHistorico(datai, dataf);
        }

        public static IQueryable<TmpGrupoBoasNoticias> ListaGrupoBoasNoticias(string competencia)
        {
            return Consignatarias.ListaGrupoBoasNoticias(competencia);
        }

        public static IQueryable<TmpBoasNoticias> obtemBoasNoticias(int id, string competencia, int IDConsignataria)
        {
            return Consignatarias.obtemBoasNoticias( id, competencia, IDConsignataria );
        }

        public static IQueryable<TmpBoasNoticiasDetalhe> obtemBoasNoticiasDetalhe(int id)
        {
            return Consignatarias.obtemBoasNoticiasDetalhe(id);
        }

        public static IQueryable<TmpMasNoticias> listaMasNoticias(string competencia, int IDConsignataria)
        {
            return Consignatarias.obtemMasNoticias(competencia, IDConsignataria);
        }

        public static IQueryable<TmpMasNoticiasDetalhe> obtemMasNoticiasDetalhe(int id)
        {
            return Consignatarias.obtemMasNoticiasDetalhe(id);
        }

        public static IQueryable<TmpMasNoticiasInadiplentes> listaMasNoticiasInadiplentes(string competencia, int IDConsignataria)
        {
            return Consignatarias.obtemMasNoticiasInadiplentes(competencia, IDConsignataria);
        }

        public static IQueryable<TmpMasNoticiasInadiplentesDetalhe> obtemMasNoticiasInadiplentesDetalhe(int id)
        {
            return Consignatarias.obtemMasNoticiasInadiplentesDetalhe(id);
        }

        public static string ObtemUltimaConciliacao(int idmodulo, int idempresa)
        {
            return Averbacoes.ObtemUltimaConciliacao(idmodulo, idempresa);
        }
    }
}
