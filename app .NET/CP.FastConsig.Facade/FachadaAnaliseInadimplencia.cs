using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{
    public class FachadaAnaliseInadimplencia
    {

        public static IQueryable<TmpInadimplenciaGeral> listaInadimplenciaGeral(string competencia, int idempresa)
        {
            return Consignatarias.listaInadimplenciaGeral(competencia,idempresa);
        }

        public static decimal ValorInadimplencia(string competencia, int idempresa, string descricao)
        {
            return Consignatarias.ValorInadimplencia(competencia, idempresa, descricao);
        }

        public static IQueryable<TmpVolumeInadimplencia> listaVolumeInadimplencia(int idempresa)
        {
            return Consignatarias.listaVolumeInadimplencia(idempresa);
        }

        public static IQueryable<TmpInadimplenciaPadraoTrabalho> listaInadimplenciaPadraoTrabalho(string competencia, int idempresa)
        {
            return Consignatarias.listaInadimplenciaPadraoTrabalho(competencia, idempresa);
        }

        public static IQueryable<TmpInadimplenciaPadraoMargem> listaInadimplenciaPadraoMargem(string competencia, int idempresa)
        {
            return Consignatarias.listaInadimplenciaPadraoMargem(competencia, idempresa);
        }

        public static IQueryable<TmpInadimplenciaTempo> listaInadimplenciaTempo()
        {
            return Consignatarias.listaInadimplenciaTempo();
        }

        public static IQueryable<TmpInadimplenciaTempoDetalhe> listaInadimplenciaTempoDetalhe()
        {
            return Consignatarias.listaInadimplenciaTempoDetalhe();
        }

        public static IQueryable<TmpInadimplenciaGeralDetalhe> listaInadimplenciaGeralDetalhe(int id)
        {
            return Consignatarias.listaInadimplenciaGeralDetalhe(id);
        }

        public static IQueryable<TmpRecuperavelPorFolha> listaRecuperavelPelaFolha(string competencia, int idempresa)
        {
            return Consignatarias.listaRecuperavelPelaFolha(competencia, idempresa);
        }

        public static IQueryable<TmpNaoRecuperavel> listaNaoRecuperavel(string competencia, int idempresa)
        {
            return Consignatarias.listaNaoRecuperavel(competencia, idempresa);
        }
        
        public static void VolumeValorAverbacoes( string competencia, int idempresa, int idprodutogrupo, out decimal? valorbruto, out decimal? valoradicionado )
        {
            Consignatarias.VolumeValorAverbacoes(competencia, competencia, idempresa, idprodutogrupo, out valorbruto, out valoradicionado);
        }
    }
}
