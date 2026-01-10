using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaTermoAverbacao
    {

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }

    }

}