using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaAgentesEdicao
    {

        public static Empresa ObtemEmpresaPorCnpj(string text)
        {
            return (Empresa) Empresas.ObtemEmpresaPorCnpj(text);
        }

        public static int ObtemIdConsignatariaVinculadaComAgente(int idAgente)
        {
            Empresa empresa = Empresas.ListaVinculadas().ToList().Where(x => x.IDEmpresaVinculada.Equals(idAgente)).Select(x => x.Consignataria).FirstOrDefault();
            return empresa == null ? 0 : empresa.IDEmpresa;
        }

    }

}