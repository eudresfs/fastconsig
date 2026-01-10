using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaSimulacaoEmprestimo
    {

        public static IQueryable<EmpresaCoeficienteDetalhe> ObtemCoeficientes(int prazo)
        {
            return Empresas.ObtemCoeficientesDetalhes(prazo);
        }

        public static EmpresaCoeficiente ObtemCoeficiente(int idEmpresaCoeficiente)
        {
            return Empresas.ObtemCoeficiente(idEmpresaCoeficiente);
        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }

    }

}