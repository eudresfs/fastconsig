using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{

    public static class FachadaPerfis
    {

        public static void RemovePerfil(int IdPerfil)
        {
            Empresas.RemovePerfil(IdPerfil);
        }

    }

}