using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaSimulacaoCompraDivida
    {

        public static IQueryable<TmpSimulacaoCompraDivida> ObtemSimulacoesCompraDivida()
        {
            return Fakes.ObtemSimulacoesCompraDivida();
        } 

        public static IQueryable<EmpresaCoeficienteDetalhe> ObtemCoeficientes()
        {

            var empresasCoeficientes = Empresas.ObtemCoeficientesDetalhes();

            var dados = from c in empresasCoeficientes
                        group c by c.IDEmpresaCoeficiente
                        into g
                        select new {IDEmpresaCoeficiente = g.Key, Prazo = g.Max(x => x.Prazo)};

            var consulta = from c in empresasCoeficientes
                           from d in dados
                           where c.IDEmpresaCoeficiente == d.IDEmpresaCoeficiente && c.Prazo == d.Prazo
                           select c;

            return consulta;

        }

    }

}