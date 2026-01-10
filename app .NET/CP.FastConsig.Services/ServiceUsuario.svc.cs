using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using System.Transactions;

namespace CP.FastConsig.Services
{
    
    public class ServiceUsuario : IServicoUsuario
    {

        #region Constantes
        
        private const string ArgumentComposite = "composite";
        private const string LabelYouEntered = "You entered: {0}";
        private const string AuxSuffix = "Suffix";

        #endregion

        public string GetData(int value)
        {
            return string.Format(LabelYouEntered, value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {

            if (composite == null) throw new ArgumentNullException(ArgumentComposite);
            if (composite.BoolValue) composite.StringValue += AuxSuffix;
            
            return composite;

        }

        public bool ExcluirUsuarioPorCpf(string cpf, int? idCriador, string tipoCriador)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {

                if (!ExisteUsuario(cpf)) return false;

                FastConsigCenterEntities ctx = new FastConsigCenterEntities();

                Usuario usu = ctx.Usuarios.FirstOrDefault(x => x.CPF == cpf);

                if (usu == null) return false;

                SalvaHistorico(usu.IDUsuario, idCriador, tipoCriador, Enums.AcaoHistoricoUsuario.Remocao.ToString());

                List<UsuarioVinculo> vinculos = usu.UsuarioVinculos.ToList();
                List<UsuarioHistorico> historicos = usu.UsuarioHistoricos.ToList();

                foreach (UsuarioVinculo usuarioVinculo in vinculos)
                {
                    ctx.DeleteObject(ctx.Vinculos.Single(x => x.IDVinculo.Equals(usuarioVinculo.IDVinculo)));
                    ctx.DeleteObject(usuarioVinculo);
                }

                foreach (UsuarioHistorico usuarioHistorico in historicos) ctx.DeleteObject(usuarioHistorico);

                ctx.DeleteObject(usu);
                ctx.SaveChanges();

                scope.Complete();

                return true;

            }

        }

        private void SalvaHistorico(int idUsuario, int? idCriador, string tipoCriador, string tipoHistorico)
        {

            FastConsigCenterEntities ctx = new FastConsigCenterEntities();

            UsuarioHistorico historico = new UsuarioHistorico();

            historico.IDUsuario = idUsuario;
            historico.Acao = tipoHistorico;
            historico.Ativo = true;
            historico.ModifiedOn = DateTime.Now;
            historico.ModifiedBy = idCriador;
            historico.ModifiedByType = tipoCriador;

            string modifiedByName = string.Empty;

            try
            {

                switch (tipoCriador)
                {

                    case "U":

                        modifiedByName = ctx.Usuarios.Single(x => x.IDUsuario.Equals(idCriador.Value)).Nome;
                        break;

                    case "B":

                        modifiedByName = ctx.Consignatarias.Single(x => x.IDConsignataria.Equals(idCriador.Value)).Nome;
                        break;

                    case "C":

                        modifiedByName = ctx.Consignantes.Single(x => x.IDConsignante.Equals(idCriador.Value)).Nome;
                        break;

                }

            }
            catch(Exception ex)
            {
                modifiedByName = string.Empty;
            }

            historico.ModifiedByName = modifiedByName;

            ctx.AddToUsuarioHistoricos(historico);

            ctx.SaveChanges();

        }

        public List<Consignataria> ListarConsignatarias()
        {
            return new FastConsigCenterEntities().Consignatarias.ToList();
        }

        public List<Consignante> ListarConsignantes()
        {
            return new FastConsigCenterEntities().Consignantes.ToList();
        }

        public bool ExisteUsuario(string CPF)
        {
            return new FastConsigCenterEntities().Usuarios.Any(x => x.CPF == CPF && x.Ativo == 1);
        }

        public Usuario ObtemUsuarioPorLogin(string login)
        {
            return new FastConsigCenterEntities().Usuarios.FirstOrDefault(x => x.Login.ToUpper().Equals(login.ToUpper()));
        }

        public Usuario ObtemUsuarioPorCpf(string cpf)
        {
            return new FastConsigCenterEntities().Usuarios.FirstOrDefault(x => x.CPF.Equals(cpf));
        }

        public void AtualizaUsuario(string cpf, string nome, string email, string telefone, string login, string senha, int idConsignataria, string novoCpf, int idConsignante, int? idCriador, string tipoCriador)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {

                if (AlterarUsuario(cpf, nome, email, telefone, login, senha, novoCpf, idCriador, tipoCriador))
                {

                    string cpfParaUsuar = !cpf.Equals(novoCpf) && !string.IsNullOrEmpty(novoCpf) ? novoCpf : cpf;

                    FastConsigCenterEntities ctx = new FastConsigCenterEntities();
                    
                    Usuario usuario = ctx.Usuarios.FirstOrDefault(x => x.CPF.Equals(cpfParaUsuar));

                    if (usuario == null) return;

                    UsuarioVinculo usuarioVinculo = usuario.UsuarioVinculos.FirstOrDefault(x => x.Vinculo.IDConsignataria.Equals(idConsignataria) && x.Vinculo.IDConsignante.Equals(idConsignante));

                    if (usuarioVinculo != null && !usuario.IDConsignataria.Equals(idConsignataria))
                    {
                        usuario.IDConsignataria = idConsignataria;
                    }
                    else if (usuarioVinculo == null)
                    {

                        usuarioVinculo = new UsuarioVinculo();

                        usuarioVinculo.Ativo = true;
                        usuarioVinculo.Usuario = usuario;
                        usuarioVinculo.Vinculo = new Vinculo {Ativo = true, IDConsignante = idConsignante, IDConsignataria = idConsignataria};

                        ctx.AddToUsuarioVinculos(usuarioVinculo);

                    }
                    
                    ctx.SaveChanges();

                }

                scope.Complete();
                
            }

        }

        public Consignante ObtemConsignante(int idConsignante)
        {
            return new FastConsigCenterEntities().Consignantes.FirstOrDefault(x => x.IDConsignante.Equals(idConsignante));
        }

        public Consignataria ObtemConsignataria(int idConsignataria)
        {
            return new FastConsigCenterEntities().Consignatarias.FirstOrDefault(x => x.IDConsignataria.Equals(idConsignataria));
        }

        public bool IncluirUsuario(int idConsignataria, int idConsignante, string cpf, string nome, string email, string fone, string login, string senha, int? idCriador, string tipoCriador)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {

                if (ExisteUsuario(cpf)) return false;
                if (idConsignataria.Equals(0)) return false;

                FastConsigCenterEntities ctx = new FastConsigCenterEntities();

                Usuario usuario = new Usuario();

                usuario.Ativo = 1;
                usuario.CPF = cpf;
                usuario.Nome = nome;
                usuario.Email = email;
                usuario.Telefone = fone;
                usuario.Login = login;
                usuario.Senha = senha;
                usuario.IDConsignataria = idConsignataria;
                usuario.CreatedOn = DateTime.Now;
                usuario.CreatedBy = idCriador;
                usuario.CreatedByType = tipoCriador;

                try
                {
                    usuario.CreatedByName = tipoCriador.Equals(Enums.TipoCadastradorCenter.B.ToString()) ? ctx.Consignatarias.Single(x => x.IDConsignataria.Equals(idCriador.Value)).Nome : ctx.Consignantes.Single(x => x.IDConsignante.Equals(idCriador.Value)).Nome;
                }
                catch (Exception ex)
                {
                    usuario.CreatedByName = string.Empty;
                }

                ctx.AddToUsuarios(usuario);

                UsuarioVinculo usuarioVinculo = new UsuarioVinculo();

                usuarioVinculo.Ativo = true;
                usuarioVinculo.Usuario = usuario;
                usuarioVinculo.Vinculo = new Vinculo {Ativo = true, IDConsignante = idConsignante, IDConsignataria = idConsignataria};

                ctx.AddToUsuarioVinculos(usuarioVinculo);
                ctx.SaveChanges();

                SalvaHistorico(usuario.IDUsuario, idCriador, tipoCriador, Enums.AcaoHistoricoUsuario.Inclusao.ToString());

                scope.Complete();

                return true;

            }

        }

        public bool AlterarUsuario(string cpf, string nome, string email, string fone, string login, string senha, string novoCpf, int? idCriador, string tipoCriador)
        {

            if (!ExisteUsuario(cpf)) return false;

            FastConsigCenterEntities ctx = new FastConsigCenterEntities();

            Usuario usuario = ctx.Usuarios.FirstOrDefault(x => x.CPF.Equals(cpf));

            if (usuario == null) return false;
            if (!cpf.Equals(novoCpf) && !string.IsNullOrEmpty(novoCpf)) usuario.CPF = novoCpf;

            SalvaHistorico(usuario.IDUsuario, idCriador, tipoCriador, Enums.AcaoHistoricoUsuario.Alteracao.ToString());

            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Telefone = fone;
            usuario.Login = login;
            usuario.Senha = senha;

            ctx.SaveChanges();

            return true;

        }

        public bool AlterarSenhaUsuario(string cpf, string novaSenha, int? idCriador, string tipoCriador)
        {

            if (!ExisteUsuario(cpf)) return false;

            FastConsigCenterEntities ctx = new FastConsigCenterEntities();

            Usuario usuario = ctx.Usuarios.FirstOrDefault(x => x.CPF.Equals(cpf));

            if (usuario == null) return false;

            SalvaHistorico(usuario.IDUsuario, idCriador, tipoCriador, Enums.AcaoHistoricoUsuario.TrocaSenha.ToString());

            usuario.Senha = novaSenha;

            ctx.SaveChanges();

            return true;

        }
        
        public List<Consignante> ConsignantesDoUsuario(string cpf)
        {
            return new FastConsigCenterEntities().Consignantes.Where(x => x.Vinculos.Any(y => y.UsuarioVinculos.Any(z => z.Usuario.CPF.Equals(cpf)))).ToList();
        }

        public Consignataria ConsignatariaDoUsuario(string cpf)
        {

            Usuario usuario = new FastConsigCenterEntities().Usuarios.SingleOrDefault(x => x.CPF.Equals(cpf));
            
            if(usuario == null) return null;
            
            return usuario.Consignataria;

        }

        public List<Consignante> ConsignantesDaConsignataria(int idConsignataria)
        {
            return new FastConsigCenterEntities().Consignantes.Where(x => x.Vinculos.Any(y => y.IDConsignataria.Equals(idConsignataria))).ToList();
        }

    }

}