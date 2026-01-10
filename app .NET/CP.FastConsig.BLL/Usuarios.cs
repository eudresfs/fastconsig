using System;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Util;

namespace CP.FastConsig.BLL
{

    public static class Usuarios
    {

        public static IQueryable<Usuario> ObtemUsuarios(int quantidadeUsuarios = 0)
        {
            return quantidadeUsuarios.Equals(0) ? new Repositorio<Usuario>().Listar().OrderBy(x => x.NomeCompleto) : new Repositorio<Usuario>().Listar().OrderBy(x => x.NomeCompleto).Take(quantidadeUsuarios);
        }

        private static int SalvaUsuario(int? idUsuario, string nome, string cpf, string login, string email, string telefone, string senhaProvisoria, int idPerfil, int idConsignataria, int idmodulo, string senhaCadastradaNoCenter)
        {

            Usuario usuario;

            using (Repositorio<Usuario> repositorioUsuario = new Repositorio<Usuario>())
            {

                bool inclusao = (idUsuario == null);
                bool moduloFuncionario = Perfis.ObtemPerfis((int)Enums.Modulos.Funcionario).Select(x => x.IDPerfil).Contains(idPerfil);

                usuario = inclusao ? new Usuario() : repositorioUsuario.ObterPorId(idUsuario.Value);

                if (usuario == null) return 0;

                UsuarioPerfil usuarioPerfil = usuario.UsuarioPerfil.FirstOrDefault(x => x.IDEmpresa.Equals(idConsignataria)) ?? new UsuarioPerfil();

                usuario.NomeCompleto = nome;
                usuario.CPF = cpf;
                usuario.Email = email;
                usuario.Celular = telefone;
                usuario.ApelidoLogin = login;
                usuario.Ativo = 1;
                usuario.Situacao = Enums.TipoBloqueioUsuario.A.ToString();
                
                if(usuarioPerfil.IDUsuario.Equals(0)) usuario.UsuarioPerfil.Add(usuarioPerfil);
                //else using (Repositorio<UsuarioPerfil> repositorioUsuarioPerfil = new Repositorio<UsuarioPerfil>()) repositorioUsuarioPerfil.Alterar(usuarioPerfil);

                usuarioPerfil.IDPerfil = idPerfil;
                usuarioPerfil.Usuario = usuario;
                usuarioPerfil.IDEmpresa = idConsignataria;

                string senhaCript = string.IsNullOrEmpty(senhaCadastradaNoCenter) ? Seguranca.getMd5Hash(senhaProvisoria) : senhaCadastradaNoCenter;

                if (inclusao)
                {

                    if(string.IsNullOrEmpty(senhaCadastradaNoCenter)) usuario.SenhaProvisoria = senhaProvisoria;

                    usuario.Senha = senhaCript;

                    repositorioUsuario.Incluir(usuario);

                    if (moduloFuncionario) Pessoas.AdicionaPessoa(nome, cpf, email, telefone, idConsignataria);

                }
                else
                {

                    if (!string.IsNullOrEmpty(senhaProvisoria))
                    {
                        if (!usuario.Senha.Equals(senhaProvisoria)) usuario.SenhaProvisoria = senhaProvisoria;
                        usuario.Senha = senhaCript;
                    }

                    repositorioUsuario.Alterar(usuario);

                    if (moduloFuncionario)
                    {
                        if (Pessoas.ObtemPessoa(cpf) != null) Pessoas.AtualizaPessoa(Pessoas.ObtemPessoa(cpf).IDPessoa, nome, cpf, email, telefone, idConsignataria);
                        else Pessoas.AdicionaPessoa(nome, cpf, email, telefone, idConsignataria);
                    }

                }

            }

            return usuario.IDUsuario;

        }

        public static void RemoveUsuario(int idUsuario)
        {

            Repositorio<Usuario> repositorio = new Repositorio<Usuario>();

            Usuario usuario = repositorio.ObterPorId(idUsuario);

            if (usuario == null) return;

            repositorio.Excluir(usuario);

        }

        public static Usuario ObtemUsuario(int idUsuario)
        {
            return new Repositorio<Usuario>().ObterPorId(idUsuario);
        }

        public static void AlteraUsuario(int idUsuario, string nome, string cpf, string login, string email, string telefone, string senhaProvisoria, int idPerfil, int idConsignataria, int idmodulo)
        {
            SalvaUsuario(idUsuario, nome, cpf, login, email, telefone, senhaProvisoria, idPerfil, idConsignataria, idmodulo, string.Empty);
        }

        public static int AdicionaUsuario(string nome, string cpf, string login, string email, string telefone, string senhaProvisoria, int idPerfil, int idConsignataria, int idmodulo, string senhaCadastradaNoCenter)
        {
            return SalvaUsuario(null, nome, cpf, login, email, telefone, senhaProvisoria, idPerfil, idConsignataria, idmodulo, senhaCadastradaNoCenter);
        }

        public static Usuario ObtemUsuario(string cpf)
        {
            return new Repositorio<Usuario>().ListarTudo().FirstOrDefault(x => x.CPF.Equals(cpf));
        }

        public static UsuarioPerfil ObtemUsuarioPerfil(int idUsuarioPerfil)
        {
            return new Repositorio<UsuarioPerfil>().ObterPorId(idUsuarioPerfil);
        }

        public static Usuario ObtemUsuarioPorLogin(string login)
        {

            try
            {
                return new Repositorio<Usuario>().Listar().FirstOrDefault(x => x.ApelidoLogin.Equals(login) || x.CPF.Equals(login)) ?? Funcionarios.ObtemFuncionario(login).Pessoa.Usuario;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static IQueryable<Usuario> ObtemUsuariosPesquisa(string prefixText)
        {
            return new Repositorio<Usuario>().PesquisaTextual(prefixText,"");
        }

        public static IQueryable<Usuario> ListaUsuarios(int id)
        {
            var lista = new Repositorio<UsuarioPerfil>().Listar().Where(x => x.IDEmpresa == id).Select(y => y.Usuario).Distinct();
            return lista;
        }

        public static void IncrementaQuantidadeAcessos(int idUsuario)
        {

            using (Repositorio<Usuario> repositorioUsuarios = new Repositorio<Usuario>())
            {

                Usuario usuario = repositorioUsuarios.ObterPorId(idUsuario);

                usuario.QtdAcessos++;
                usuario.UltimoAcesso = DateTime.Now;

                repositorioUsuarios.Alterar(usuario);

            }

        }

        public static void AtualizaUsuarioPrimeiroAcesso(int idUsuario, string apelidoLogin, string senha)
        {

            using (Repositorio<Usuario> repositorioUsuarios = new Repositorio<Usuario>())
            {

                Usuario usuario = repositorioUsuarios.ObterPorId(idUsuario);

                if (!string.IsNullOrEmpty(apelidoLogin)) usuario.ApelidoLogin = apelidoLogin;
                if (!string.IsNullOrEmpty(senha)) usuario.Senha = Seguranca.getMd5Hash(senha);

                usuario.SenhaProvisoria = null;

                repositorioUsuarios.Alterar(usuario);

            }

        }

        public static void AtualizaSenha(int idUsuario, string senha)
        {

            Repositorio<Usuario> repositorioUsuarios = new Repositorio<Usuario>();

            Usuario usuario = repositorioUsuarios.ObterPorId(idUsuario);

            using (repositorioUsuarios)
            {

                if (!string.IsNullOrEmpty(senha)) usuario.Senha = Seguranca.getMd5Hash(senha);

                repositorioUsuarios.Alterar(usuario);

            }


        }


        public static void BloqueiaUsuario(int idUsuario)
        {

            Repositorio<Usuario> repositorio = new Repositorio<Usuario>();

            Usuario usuario = repositorio.ObterPorId(idUsuario);

            string novaSituacao = usuario.Situacao.Equals(Enums.TipoBloqueioUsuario.A.ToString()) ? Enums.TipoBloqueioUsuario.B.ToString() : Enums.TipoBloqueioUsuario.A.ToString();

            usuario.Situacao = novaSituacao;

            repositorio.Alterar(usuario);

        }

    }

}