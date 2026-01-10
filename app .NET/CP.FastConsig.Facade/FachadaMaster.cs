using System;
using System.Collections.Generic;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using System.Linq;

namespace CP.FastConsig.Facade
{

    public static class FachadaMaster
    {

        public static Perfil ObtemPerfil(int idPerfil)
        {
            return Perfis.ObtemPerfil(idPerfil);
        }

        public static Recurso ObtemRecurso(int idrecurso)
        {
            return Geral.ObtemRecurso(idrecurso);
        }

        public static IQueryable<Recurso> ListaMenuOpcoes(int idpai,int idmodulo)
        {
            return Geral.ListaMenuOpcoes(idpai,idmodulo);
        }

        public static List<object> ObtemSolicitacoes(int idModulo, int idBanco, int idUsuario)
        {

            List<object> notificacoes = new List<object>();

            IQueryable<EmpresaSolicitacao> solicitacoesAux;            

            List<EmpresaSolicitacao> solic = new List<EmpresaSolicitacao>();

            if (idModulo.Equals((int)Enums.Modulos.Funcionario))
                solicitacoesAux = Solicitacoes.ObtemSolicitacoesPendentes(idUsuario, "", true, idBanco);
            else if (idModulo.Equals((int)Enums.Modulos.Consignante))
                solicitacoesAux = Solicitacoes.ObtemSolicitacoesPendentes(Convert.ToInt32(Geral.IdEmpresaConsignante()), "1", false, idBanco);
            else if ((idModulo.Equals((int)Enums.Modulos.Consignataria)) && (idBanco == 0))
                solicitacoesAux = Solicitacoes.ObtemSolicitacoesPendentes(idBanco, ((int)Enums.Modulos.Consignataria).ToString(), false, idBanco, idUsuario);
            else
                solicitacoesAux = Solicitacoes.ObtemSolicitacoesPendentes(idBanco, ((int)Enums.Modulos.Consignataria).ToString(), false, idBanco, idUsuario);

            IQueryable<IGrouping<int, EmpresaSolicitacao>> solicitacoes = solicitacoesAux.GroupBy(x => x.IDEmpresaSolicitacaoTipo);

            foreach (IGrouping<int, EmpresaSolicitacao> grupoSolicitacao in solicitacoes) notificacoes.Add(new { IDEmpresaSolicitacaoTipo = grupoSolicitacao.Key, Descricao = Solicitacoes.ObtemDescricao(grupoSolicitacao.Key), ValorPro = grupoSolicitacao.Where(x => DateTime.Today < x.DataSolicitacao).Count(), ValorNeutro = grupoSolicitacao.Where(x => DateTime.Today == x.DataSolicitacao).Count(), ValorContra = grupoSolicitacao.Where(x => DateTime.Today > x.DataSolicitacao).Count() });

            if ((idUsuario > 0) && (notificacoes.Count == 0))
            {
                Solicitacoes.ObtemSolicitacoesPendentes(idBanco, ((int)Enums.Modulos.Consignataria).ToString(), false);

                solicitacoes = solicitacoesAux.GroupBy(x => x.IDEmpresaSolicitacaoTipo);

                foreach (IGrouping<int, EmpresaSolicitacao> grupoSolicitacao in solicitacoes) notificacoes.Add(new { IDEmpresaSolicitacaoTipo = grupoSolicitacao.Key, Descricao = Solicitacoes.ObtemDescricao(grupoSolicitacao.Key), ValorPro = grupoSolicitacao.Where(x => DateTime.Today < x.DataSolicitacao).Count(), ValorNeutro = grupoSolicitacao.Where(x => DateTime.Today == x.DataSolicitacao).Count(), ValorContra = grupoSolicitacao.Where(x => DateTime.Today > x.DataSolicitacao).Count() });
            }
            
            
            if (idUsuario == 0)
            {

                List<EmpresaSolicitacaoTipo> solicitacoesTipo = Solicitacoes.ListaSolicitacaoTipo(idModulo).ToList();

                foreach (var s in solicitacoesTipo)
                {
                    if ((s.Prazo > 0) && (!(notificacoes.Any(x => ((dynamic)x).IDEmpresaSolicitacaoTipo == s.IDEmpresaSolicitacaoTipo))))
                        notificacoes.Add(new { IDEmpresaSolicitacaoTipo = s.IDEmpresaSolicitacaoTipo, Descricao = s.Nome, ValorPro = 0, ValorNeutro = 0, ValorContra = 0 });
                }
            }
                        
            return notificacoes;

        }



        public static List<object> ObtemSolicitacoesSolicitadasPelaEmpresa(int idModulo, int idBanco)
        {

            List<object> notificacoes = new List<object>();

            IQueryable<EmpresaSolicitacao> solicitacoesAux;

            List<EmpresaSolicitacao> solic = new List<EmpresaSolicitacao>();

            solicitacoesAux = Solicitacoes.ObtemSolicitacoesPendentesSolicitadasPelaEmpresa(idBanco);

            IQueryable<IGrouping<int, EmpresaSolicitacao>> solicitacoes = solicitacoesAux.GroupBy(x => x.IDEmpresaSolicitacaoTipo);

            foreach (IGrouping<int, EmpresaSolicitacao> grupoSolicitacao in solicitacoes) notificacoes.Add(new { IDEmpresaSolicitacaoTipo = grupoSolicitacao.Key, Descricao = Solicitacoes.ObtemDescricao(grupoSolicitacao.Key), ValorPro = grupoSolicitacao.Where(x => DateTime.Today < x.DataSolicitacao).Count(), ValorNeutro = grupoSolicitacao.Where(x => DateTime.Today == x.DataSolicitacao).Count(), ValorContra = grupoSolicitacao.Where(x => DateTime.Today > x.DataSolicitacao).Count() });

            //List<EmpresaSolicitacaoTipo> solicitacoesTipo = Solicitacoes.ListaSolicitacaoTipo(idModulo).ToList();

            //foreach (var s in solicitacoesTipo)
            //{
            //    if ((s.Prazo > 0) && (!(notificacoes.Any(x => ((dynamic)x).IDEmpresaSolicitacaoTipo == s.IDEmpresaSolicitacaoTipo))))
            //            notificacoes.Add(new { IDEmpresaSolicitacaoTipo = s.IDEmpresaSolicitacaoTipo, Descricao = s.Nome, ValorPro = 0, ValorNeutro = 0, ValorContra = 0 });
            //}

            return notificacoes;
        }


        public static int ObtemRecursoPorNome(string nomerecurso, int idmodulo)
        {
            return Geral.ObtemRecursoPorNome(nomerecurso, idmodulo);
        }

        public static Empresa ObtemEmpresa(int idBanco)
        {
            return Empresas.ObtemEmpresa(idBanco);
        }

        public static bool AtivaAbasSistema()
        {
            return Parametros.AtivaAbasSistema();
        }


        public static void RegistrarErro(string browser, string ip, int idmodulo, int idempresa, int idperfil, int idusuario, int idrecurso, string nomerecurso, string erro)
        {
            Geral.RegistrarErro(browser, ip, idmodulo, idempresa, idperfil, idusuario, idrecurso, nomerecurso, erro);
        }

        public static void RegistrarAcesso(string browser, string ip, int idmodulo, int idempresa, int idperfil, int idusuario, int idrecurso, string nomerecurso, string descricao)
        {
            Geral.RegistrarAcesso(browser, ip, idmodulo, idempresa, idperfil, idusuario, idrecurso, nomerecurso, descricao);
        }

        public static Permissao ObtemPermissao(int id)
        {
            return Geral.ObtemPermissao(id);
        }

        public static void RegistrarErro(System.Web.HttpRequest request, string erro)
        {
            Geral.RegistrarErro(request, erro);
        }
    }

}