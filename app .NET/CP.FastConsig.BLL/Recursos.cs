using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Recursos
    {

        public static int ObtemIdRecursoPorNomeModulo(string nome, int modulo)
        {
            Recurso recurso = new Repositorio<Recurso>().Listar().FirstOrDefault(x => x.IDModulo != null && x.IDModulo.Value.Equals(modulo) && x.Arquivo.Equals(nome) && (x.Visivel == null || x.Visivel.Value));
            return recurso == null ? 0 : recurso.IDRecurso;
        }

    }

}