using CP.FastConsig.BLL;

namespace CP.FastConsig.Facade
{

    public static class FachadaProdutos
    {

        public static void RemoveProduto(int idProduto)
        {
            Empresas.RemoveProduto(idProduto);
        }

        public static bool existeMaisDeUmProduto(int idconsignataria)
        {
            return Produtos.existeMaisDeUmProduto(idconsignataria);
        }

    
    }

}