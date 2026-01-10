using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{

    public static class FachadaContatos
    {

        public static void RemoveContato(int IdContato)
        {
            Empresas.RemoveContato(IdContato);
        }

    }

}