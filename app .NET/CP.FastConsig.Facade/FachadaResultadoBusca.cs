using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;

namespace CP.FastConsig.Facade
{

    public static class FachadaResultadoBusca
    {

        public static List<Usuario> ObtemUsuariosPesquisa(string prefixText, int count)
        {
            return Usuarios.ObtemUsuariosPesquisa(prefixText).Take(count).ToList();
        }

        public static List<ResultadoBusca> ObtemResultadosPesquisa(string prefixText, int count)
        {

            List<ResultadoBusca> resultado = new List<ResultadoBusca>();

            List<Usuario> usuarios =  Usuarios.ObtemUsuariosPesquisa(prefixText).Take(count).ToList();
            List<Funcionario> funcionarios = Funcionarios.ObtemFuncionariosPesquisa(prefixText).Take(count).ToList();

            foreach (Usuario usuario in usuarios) resultado.Add(new ResultadoBusca { IDUsuario = usuario.IDUsuario, NomeCompleto = usuario.NomeCompleto, Perfil = usuario.Perfil, Email = usuario.Email });
            foreach (Funcionario funcionario in funcionarios) resultado.Add(new ResultadoBusca { IDUsuario = funcionario.Pessoa.IDUsuario.Value, NomeCompleto = funcionario.Pessoa.Nome, Perfil = funcionario.Pessoa.Usuario.Perfil, Email = funcionario.Pessoa.Usuario.Email });
            
            return resultado;

        }

        public static List<Averbacao> ObtemAverbacaosPesquisa(string prefixText, int count)
        {
            return Averbacoes.ObtemAverbacaosPesquisa(prefixText).Take(count).ToList();
        }

        public static List<Funcionario> ObtemFuncionariosPesquisa(string prefixText, int count)
        {
            return Funcionarios.ObtemFuncionariosPesquisa(prefixText).Take(count).ToList();
        }

        public static List<string> PesquisaIncremental(string prefixText, int count)
        {

            prefixText = prefixText.ToUpper();

            List<Usuario> usuariosPesquisa = ObtemUsuariosPesquisa(prefixText, count);
            List<Averbacao> AverbacaosPesquisa = ObtemAverbacaosPesquisa(prefixText, count);
            List<Funcionario> funcionariosPesquisa = ObtemFuncionariosPesquisa(prefixText, count);

            List<string> nomes = usuariosPesquisa.Where(x => x.NomeCompleto.ToUpper().Contains(prefixText)).Select(x => x.NomeCompleto).ToList();
            List<string> emails = usuariosPesquisa.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToUpper().Contains(prefixText)).Select(x => x.Email).ToList();
            List<string> cpfs = usuariosPesquisa.Where(x => x.CPF.ToUpper().Contains(prefixText)).Select(x => x.CPF).ToList();
            List<string> numerosAverbacaos = AverbacaosPesquisa.Where(x => x.Numero.ToUpper().Contains(prefixText)).Select(x => x.Numero).ToList();
            List<string> matriculas = funcionariosPesquisa.Where(x => x.Matricula.ToUpper().Contains(prefixText)).Select(x => x.Matricula).ToList();

            if (nomes.Count > 0) return nomes;
            if (matriculas.Count > 0) return matriculas;
            if (cpfs.Count > 0) return cpfs;
            if (numerosAverbacaos.Count > 0) return numerosAverbacaos;
            if (emails.Count > 0) return emails;

            return new List<string>();

        }

        public static List<int> ObtemIdsFuncionarios(int idUsuario)
        {
            return Funcionarios.ObtemIdsFuncionarios(idUsuario);
        }

    }

}