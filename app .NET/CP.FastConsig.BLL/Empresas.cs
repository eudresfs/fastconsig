using System.Collections.Generic;
using System;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Transactions;
using CP.FastConsig.Util;
using Microsoft.Data.Extensions;

namespace CP.FastConsig.BLL
{

    public static class Empresas
    {

        public static IQueryable<Empresa> ListaEmpresas(int quantidadeConsignatarias = 10)
        {
            var consulta = new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresaTipo >= 4).OrderBy(x => x.Fantasia); // .Include("EmpresaTipo").Include("EmpresaSituacao")

            return consulta;
        }

        public static IQueryable<Empresa> ListaConsignantes()
        {
            var consulta = new Repositorio<Empresa>().Listar().OrderBy(x => x.Fantasia);
            //if (consulta.Count() > quantidadeConsignatarias)
            //    return consulta.Take(quantidadeConsignatarias).ToList();
            //els
            //    

            return consulta;
        }

        public static List<EmpresaTipo> ObtemEmpresaTipos()
        {
            var consulta = new Repositorio<EmpresaTipo>().Listar().Where(x => x.IDEmpresaTipo >= 4);
            return consulta.ToList();
        }

        public static List<EmpresaTipo> ObtemTodasEmpresaTipos()
        {
            var consulta = new Repositorio<EmpresaTipo>().Listar();
            return consulta.ToList();
        }

        public static void SalvarEmpresa(Empresa empresa)
        {

            Repositorio<Empresa> repositorio = new Repositorio<Empresa>();
            if (empresa.IDEmpresa == 0) repositorio.Incluir(empresa);
            else
            {
                //repositorio.Atachar(empresa);
                repositorio.Alterar(empresa);
            }

        }

        public static void RemoveEmpresa(int idEmpresa)
        {

            Repositorio<Empresa> repositorio = new Repositorio<Empresa>();

            Empresa empresa = repositorio.ObterPorId(idEmpresa);

            if (empresa == null) return;

            repositorio.Excluir(empresa);

        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return new Repositorio<Empresa>().ObterPorId(idEmpresa);
        }

        public static void AtualizaEmpresa(Empresa empresa)
        {
            new Repositorio<Empresa>().Alterar(empresa);
        }


        public static IQueryable<Produto> ListaProdutos(int id)
        {
            var lista = new Repositorio<Produto>().Listar().Where(x => x.IDConsignataria == id).OrderBy(x => x.Nome);
            if (id == 0)
                lista = new Repositorio<Produto>().Listar().OrderBy(x => x.Nome);
            return lista;
        }

        public static IQueryable<EmpresaContato> ListaContatos(int id)
        {
            var lista = new Repositorio<EmpresaContato>().Listar().Where(x => x.IDEmpresa == id);
            if (id == 0)
                lista = new Repositorio<EmpresaContato>().Listar();
            return lista;
        }

        public static void SalvarProduto(Produto Produto)
        {
            
            Repositorio<Produto> repositorio = new Repositorio<Produto>();
            if (Produto.IDProduto == 0) repositorio.Incluir(Produto);
            else
            {
                //repositorio.Atachar(empresa);
                repositorio.Alterar(Produto);
            }

        }

        public static void SalvarContato(EmpresaContato contato)
        {

            Repositorio<EmpresaContato> repositorio = new Repositorio<EmpresaContato>();
            if (contato.IDEmpresaContato == 0) repositorio.Incluir(contato);
            else
            {
                //repositorio.Atachar(empresa);
                repositorio.Alterar(contato);
            }
        }

        public static void SalvarPerfil(Perfil perfil)
        {

            Repositorio<Perfil> repositorio = new Repositorio<Perfil>();
            if (perfil.IDPerfil == 0) repositorio.Incluir(perfil);
            else
            {
                repositorio.Alterar(perfil);
            }
        }

        public static Perfil ObtemPerfil(int IdPerfil)
        {
            return new Repositorio<Perfil>().ObterPorId(IdPerfil);
        }

        public static Produto ObtemProduto(int idProduto)
        {
            return new Repositorio<Produto>().ObterPorId(idProduto);
        }

        public static EmpresaContato ObtemContato(int IdContato)
        {
            return new Repositorio<EmpresaContato>().ObterPorId(IdContato);
        }

        public static void RemoveProduto(int idProduto)
        {

            Repositorio<Produto> repositorio = new Repositorio<Produto>();

            Produto serv = repositorio.ObterPorId(idProduto);

            if (serv == null) return;

            repositorio.Excluir(serv);

        }

        public static void RemoveContato(int IdContato)
        {

            Repositorio<EmpresaContato> repositorio = new Repositorio<EmpresaContato>();

            EmpresaContato contato = repositorio.ObterPorId(IdContato);

            if (contato == null) return;

            repositorio.Excluir(contato);
        }

        public static void RemovePerfil(int IdPerfil)
        {

            Repositorio<Perfil> repositorio = new Repositorio<Perfil>();

            Perfil perfil = repositorio.ObterPorId(IdPerfil);

            if (perfil == null) return;

            repositorio.Excluir(perfil);
        }

        public static IQueryable<Empresa> ListaConsignatarias()
        {
            return new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresaTipo.Equals((int)Enums.EmpresaTipo.Banco)).OrderBy(x => x.Fantasia);
        }

        public static IQueryable<Empresa> ListaAgentes()
        {
            return new Repositorio<Empresa>().Listar().Where(x => x.IDEmpresaTipo.Equals((int) Enums.EmpresaTipo.Agente)).OrderBy(x => x.Fantasia);
        }

        public static object ObtemEmpresaPorCnpj(string cnpj)
        {
            return new Repositorio<Empresa>().Listar().FirstOrDefault(x => x.CNPJ.Equals(cnpj));
        }

        public static IQueryable<EmpresaCoeficienteDetalhe> ObtemCoeficientesDetalhes(int prazo)
        {
            if (prazo > 0)
            {
                return new Repositorio<EmpresaCoeficienteDetalhe>().Listar().Where(x => x.Prazo.Equals(prazo));
            }
            else
            {
                return new Repositorio<EmpresaCoeficienteDetalhe>().Listar();
            }
        }

        public static IQueryable<EmpresaCoeficienteDetalhe> ObtemCoeficientesDetalhesCoeficiente(int idCoeficiente)
        {
            return new Repositorio<EmpresaCoeficienteDetalhe>().Listar().Where(x => x.IDEmpresaCoeficiente.Equals(idCoeficiente));
        }

        public static IQueryable<EmpresaCoeficiente> ObtemCoeficientes(int idEmpresa)
        {
            return new Repositorio<EmpresaCoeficiente>().Listar().Where(x => x.IDEmpresa.Equals(idEmpresa));
        }

        public static IQueryable<EmpresaCoeficienteDetalhe> ObtemCoeficientesDetalhes()
        {
            return new Repositorio<EmpresaCoeficienteDetalhe>().Listar();
        }

        public static EmpresaCoeficiente ObtemCoeficiente(int idEmpresaCoeficiente)
        {
            return new Repositorio<EmpresaCoeficiente>().ObterPorId(idEmpresaCoeficiente);
        }

        public static IQueryable<EmpresaVinculo> ListaVinculadas(int idBanco)
        {
            return new Repositorio<EmpresaVinculo>().Listar().Where(x => x.IDEmpresa.Equals(idBanco));
        }

        public static IQueryable<EmpresaVinculo> ListaVinculadas()
        {
            return new Repositorio<EmpresaVinculo>().Listar();
        }

        public static List<EmpresaCoeficienteDetalhe> ObtemCoeficientesBanco(int idBanco)
        {
            EmpresaCoeficiente empresaCoeficiente =
                new Repositorio<EmpresaCoeficiente>().Listar().Where(x => x.IDEmpresa == idBanco).FirstOrDefault();
            return Empresas.ObtemCoeficientesDetalhes().Where(x => x.IDEmpresaCoeficiente == empresaCoeficiente.IDEmpresaCoeficiente).ToList();
        }

        public static IQueryable<EmpresaTarifada> ListaTipoTarifas()
        {
            return new Repositorio<EmpresaTarifada>().Listar();
        }

        public static void RemoveSuspensao(int idEmpresaSuspensao)
        {

            Repositorio<EmpresaSuspensao> repositorioEmpresaSuspensao = new Repositorio<EmpresaSuspensao>();
            Repositorio<Empresa> repositorioEmpresa = new Repositorio<Empresa>();

            EmpresaSuspensao suspensaoParaRemover = repositorioEmpresaSuspensao.ObterPorId(idEmpresaSuspensao);
            EmpresaSuspensao ultimaSuspensao = repositorioEmpresaSuspensao.Listar().Where(x => x.IDEmpresa.Equals(suspensaoParaRemover.IDEmpresa)).ToList().LastOrDefault();

            if (suspensaoParaRemover == null || ultimaSuspensao == null) return;

            if (suspensaoParaRemover.IDEmpresaSuspensao.Equals(ultimaSuspensao.IDEmpresaSuspensao))
            {

                Empresa empresa = repositorioEmpresa.ObterPorId(suspensaoParaRemover.IDEmpresa);

                empresa.IDEmpresaSituacao = suspensaoParaRemover.IDEmpresaSituacaoAnterior;

                repositorioEmpresa.Alterar(empresa);

            }

            repositorioEmpresaSuspensao.Excluir(suspensaoParaRemover);

        }

        public static EmpresaSuspensao ObtemSuspensao(int IdSuspensaoEdicao)
        {
            return new Repositorio<EmpresaSuspensao>().ObterPorId(IdSuspensaoEdicao);
        }

        public static void SalvarSuspensao(EmpresaSuspensao suspensao)
        {

            Repositorio<EmpresaSuspensao> repositorio = new Repositorio<EmpresaSuspensao>();
            Repositorio<Empresa> repositorioEmpresa = new Repositorio<Empresa>();

            Empresa empresa = repositorioEmpresa.ObterPorId(suspensao.IDEmpresa);
            empresa.IDEmpresaSituacao = suspensao.IDEmpresaSituacaoSuspensao;

            repositorioEmpresa.Alterar(empresa);

            if (suspensao.IDEmpresaSituacaoSuspensao.Equals((int)Enums.EmpresaSituacao.Normal)) foreach (EmpresaSuspensao empresaSuspensaoAux in repositorio.Listar().Where(x => x.IDEmpresa.Equals(suspensao.IDEmpresa)).ToList()) repositorio.Excluir(empresaSuspensaoAux.IDEmpresaSuspensao);
            else if (suspensao.IDEmpresaSuspensao == 0) repositorio.Incluir(suspensao);
            else repositorio.Alterar(suspensao);
            
        }

        public static void AplicaSuspensoes()
        {
            Repositorio<EmpresaSuspensao> rep = new Repositorio<EmpresaSuspensao>();
            Repositorio<Empresa> repemp = new Repositorio<Empresa>();

            DateTime dataatual = DateTime.Today;

            int situacaoativa = (int)Enums.EmpresaSituacao.Normal;

            // aplicar suspensão das empresas ainda não aplicadas
            IList<EmpresaSuspensao> lista = rep.Listar().Where(x => x.TipoPeriodo == "I" && x.Empresa.IDEmpresaSituacao == (int)Enums.EmpresaSituacao.Normal || (x.TipoPeriodo != "I" && x.Empresa.IDEmpresaSituacao == (int)Enums.EmpresaSituacao.Normal && x.DataInicial.Value >= dataatual && x.DataFinal.Value <= dataatual)).OrderBy(x => x.IDEmpresaSuspensao).ToList();

            // aplicar revogação de suspensão
            IList<Empresa> listaemp = repemp.Listar().Where(x => x.IDEmpresaTipo == (int)Enums.EmpresaTipo.Banco && x.IDEmpresaSituacao != (int)Enums.EmpresaSituacao.Normal).ToList();

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            //{
                foreach (var item in lista)
                {
                        Empresa emp = repemp.ObterPorId(item.IDEmpresa);
                        emp.IDEmpresaSituacao = item.IDEmpresaSituacaoSuspensao;

                        repemp.Alterar(emp);

                }

                foreach (var item in listaemp)
                {
                    if (!lista.Any(y => y.IDEmpresa == item.IDEmpresa))
                    {
                        Empresa emp = repemp.ObterPorId(item.IDEmpresa);
                        emp.IDEmpresaSituacao = situacaoativa;

                        repemp.Alterar(emp);
                    }
                }
            //    scope.Complete();
            //}
        }

        public static void RestaurarPermissoes(int IdEmpresaEdicao)
        {
            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_RestauraPermissoes", CommandType.StoredProcedure, new SqlParameter("IDEmpresa", IdEmpresaEdicao));

            int result;
            using (ctx.Connection.CreateConnectionScope())
            {
                result = comando.ExecuteNonQuery();
            }

            //Repositorio<PermissaoSistema> repsistema = new Repositorio<PermissaoSistema>();
            //IQueryable<PermissaoSistema> permissoespadrao = repsistema.Listar();

            //// excluindo definição atual
            //Repositorio<PermissaoUsuario> repusuario = new Repositorio<PermissaoUsuario>();
            //repusuario.Excluir("IDEmpresa = " + IdEmpresaEdicao.ToString());

            //// incluindo novos
            //foreach (var item in permissoespadrao)
            //{
            //    PermissaoUsuario pu = new PermissaoUsuario();
            //    pu.IDEmpresa = IdEmpresaEdicao;
            //    pu.IDPerfil = item.IDPerfil;
            //    pu.IDPermissao = item.IDPermissao;
            //    pu.IDRecurso = item.IDRecurso;

            //    repusuario.Incluir(pu);
            //}
        }

        public static void RemoveCoeficiente(int idCoeficiente)
        {

            Repositorio<EmpresaCoeficiente> repositorioEmpresaCoeficiente = new Repositorio<EmpresaCoeficiente>();
            Repositorio<EmpresaCoeficienteDetalhe> repositorioEmpresaCoeficienteDetalhe = new Repositorio<EmpresaCoeficienteDetalhe>();

            EmpresaCoeficiente empresaCoeficiente = repositorioEmpresaCoeficiente.ObterPorId(idCoeficiente);

            if (empresaCoeficiente == null) return;

            List<EmpresaCoeficienteDetalhe> detalhes = repositorioEmpresaCoeficienteDetalhe.Listar().Where(x => x.IDEmpresaCoeficiente.Equals(idCoeficiente)).ToList();

            foreach (EmpresaCoeficienteDetalhe detalhe in detalhes) repositorioEmpresaCoeficienteDetalhe.Excluir(detalhe);

            repositorioEmpresaCoeficiente.Excluir(empresaCoeficiente);

        }

        public static void SalvarVinculo(int idConsignataria, int idAgente)
        {

            Repositorio<EmpresaVinculo> repositorio = new Repositorio<EmpresaVinculo>();
            
            EmpresaVinculo empresaVinculo = repositorio.Listar().SingleOrDefault(x => x.IDEmpresa.Equals(idConsignataria) && x.IDEmpresaVinculada.Equals(idAgente));

            if (empresaVinculo != null) return;

            empresaVinculo = new EmpresaVinculo {IDEmpresa = idConsignataria, IDEmpresaVinculada = idAgente, Ativo = 1};

            repositorio.Incluir(empresaVinculo);

        }

        public static IEnumerable<Empresa> ListaAgentesEmpresa(int idBanco)
        {
            return new Repositorio<EmpresaVinculo>().Listar().Where(x => x.IDEmpresa.Equals(idBanco)).Select(x => x.Agente);
        }

        public static IEnumerable<Empresa> ObtemConsignatariasAgente(int idAgente)
        {
            return new Repositorio<EmpresaVinculo>().Listar().Where(x => x.IDEmpresaVinculada.Equals(idAgente)).Select(x => x.Consignataria);
        }

        public static EmpresaSuspensao ObtemUltimaSuspensao(int idEmpresa)
        {
            Repositorio<EmpresaSuspensao> repositorioEmpresa = new Repositorio<EmpresaSuspensao>();
            return repositorioEmpresa.Listar().Where(x => x.IDEmpresa.Equals(idEmpresa)).ToList().LastOrDefault();
        }

        public static List<EmpresaSuspensao> ObtemSuspensoes(int idEmpresa)
        {
            Repositorio<EmpresaSuspensao> repositorioEmpresa = new Repositorio<EmpresaSuspensao>();
            return repositorioEmpresa.Listar().Where(x => x.IDEmpresa.Equals(idEmpresa)).ToList();
        }

    }

}