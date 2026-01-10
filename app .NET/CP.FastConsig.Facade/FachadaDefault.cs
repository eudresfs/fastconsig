using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaDefault
    {

        public static List<string> PesquisaIncremental(string prefixText, int count)
        {

            prefixText = prefixText.ToUpper();

            List<Usuario> usuariosPesquisa = ObtemUsuariosPesquisa(prefixText, count);
            List<Averbacao> averbacoes = ObtemAverbacaosPesquisa(prefixText, count);
            List<Funcionario> funcionariosPesquisa = ObtemFuncionariosPesquisa(prefixText, count);

            List<string> resultado = new List<string>();

            List<string> nomes = usuariosPesquisa.Where(x => x.NomeCompleto.ToUpper().Contains(prefixText)).Select(x => x.NomeCompleto).ToList();
            List<string> emails = usuariosPesquisa.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToUpper().Contains(prefixText)).Select(x => x.Email).ToList();
            List<string> cpfs = usuariosPesquisa.Where(x => x.CPF.ToUpper().Contains(prefixText)).Select(x => x.CPF).ToList();
            List<string> numerosAverbacaos = averbacoes.Where(x => x.Numero.ToUpper().Contains(prefixText)).Select(x => x.Numero).ToList();
            List<string> matriculas = funcionariosPesquisa.Where(x => x.Matricula.ToUpper().Contains(prefixText)).Select(x => x.Matricula).ToList();

            resultado.AddRange(nomes);
            resultado.AddRange(emails);
            resultado.AddRange(cpfs);
            resultado.AddRange(numerosAverbacaos);
            resultado.AddRange(matriculas);

            return resultado.Distinct().ToList();

        }

        public static List<Usuario> ObtemUsuariosPesquisa(string prefixText, int count)
        {
            return Usuarios.ObtemUsuariosPesquisa(prefixText).Take(count).ToList();
        }

        public static List<Averbacao> ObtemAverbacaosPesquisa(string prefixText, int count)
        {
            return Averbacoes.ObtemAverbacaosPesquisa(prefixText).Take(count).ToList();
        }

        public static List<Funcionario> ObtemFuncionariosPesquisa(string prefixText, int count)
        {
            return Funcionarios.ObtemFuncionariosPesquisa(prefixText).Take(count).ToList();
        }

    }

}