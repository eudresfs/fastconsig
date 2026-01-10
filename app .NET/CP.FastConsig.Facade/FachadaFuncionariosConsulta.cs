using System.Collections.Generic;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using System.Linq;

namespace CP.FastConsig.Facade
{

    public static class FachadaFuncionariosConsulta
    {

        public static Funcionario ObtemFuncionario(int id)
        {
            return Funcionarios.ObtemFuncionario(id);
        }

        public static Funcionario ObtemFuncionario(string cpf)
        {
            var pessoa = Funcionarios.ObtemPessoa(cpf);
            if (pessoa != null)
                return Funcionarios.ObtemFuncionariosPorPessoa(pessoa.IDPessoa).FirstOrDefault();
            else
                return null;

        }

        public static List<Averbacao> ObtemAverbacaoVinculos(int id)
        {
            return Averbacoes.ObtemAverbacaoVinculos(id);
        }

        public static Averbacao ObtemAverbacao(int id)
        {
            return Averbacoes.ObtemAverbacao(id);
        }

        public static string GerarSenha(int IdFunc)
        {
            return Funcionarios.GerarSenha(IdFunc);
        }

        public static void GerarMargens(int IdFunc)
        {
            Funcionarios.GerarMargens(IdFunc);
        }

        public static void AtualizaSituacao(int id, int situacao, string mesexonerar)
        {

            Repositorio<Funcionario> func = new Repositorio<Funcionario>();

            Funcionario f = func.ObterPorId(id);

            using (func)
            {
                f.IDFuncionarioSituacao = situacao;
                if (! mesexonerar.Equals(string.Empty) )
                {
                    f.MESEXONERAR = mesexonerar;
                }
                
                func.Alterar(f);

            }


        }

        public static IEnumerable<GrupoMargem> ProcessaMargens(ICollection<FuncionarioMargem> lista)
        {
            IEnumerable<GrupoMargem> grupo = from x in lista
                                             group x by x.ProdutoGrupo.IDProdutoGrupoCompartilha
                                                 into g
                                                 select
                                                     new GrupoMargem
                                                     {
                                                         Nome = g.Select(c => c.ProdutoGrupo.Nome).Aggregate((i, j) => i + "," + j),
                                                         MargemFolha = g.Sum(x => x.MargemFolha),
                                                         MargemUtilizada = g.Select(w => w.Funcionario.Averbacao.Where(y => y.Produto.ProdutoGrupo.IDProdutoGrupoCompartilha == g.Key && y.AverbacaoSituacao.DeduzMargem).Sum(k => k.ValorDeducaoMargem)).Max() // g.Key.FuncionarioMargem.Sum(x => x.Funcionario.Averbacao.Where(w => w.Produto.IDProdutoGrupo == g.Key.IDProdutoGrupoCompartilha).Sum(y => y.ValorParcela))
                                                     };

            return grupo;
        }

        public static void Aposentar(int IdFunc)
        {
            Funcionarios.Aposentar(IdFunc);
        }
    }

}