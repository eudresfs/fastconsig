using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Data.Objects;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq.Dynamic;
using System.Web;
using CP.FastConsig.Common;

namespace CP.FastConsig.DAL
{

    public class Repositorio<T> : IDisposable, IRepositorio<T> where T : class
    {

        private const string StringPesquisaTextual = "SELECT * FROM {0} WHERE CONTAINS (*, '\"*{1}*\"') OR CONTAINS (*, '\"{1}\"')";

        public readonly DbContext contexto;
        
        public static IEnumerable<EntitySetBase> entidades;
        private EntitySetBase entidade;

        private static IEnumerable<EntityType> campos;
        private ReadOnlyMetadataCollection<EdmMember> campodatabela;

        private ReadOnlyMetadataCollection<EdmMember> chavedatabela;

        private int IdUsuario;
        private int IdModulo;
        private int IdPerfil;
        private int IdRecurso;

        public Repositorio()
        {            
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UsuarioLogado"] != null)
                IdUsuario = ((Usuario)HttpContext.Current.Session["UsuarioLogado"]).IDUsuario;
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdModulo"] != null)
                IdModulo = ((int)HttpContext.Current.Session["IdModulo"]);
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdPerfil"] != null)
                IdPerfil = ((int)HttpContext.Current.Session["IdPerfil"]);
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["IdRecursoAtual"] != null)
                IdRecurso = ((int)HttpContext.Current.Session["IdRecursoAtual"]);

            this.contexto = new DbContext(HttpContext.Current.Session["NomeStringConexao"] as string);
            
            ObjectContext objectContext = ((IObjectContextAdapter)contexto).ObjectContext;
            if (entidades == null)
            {
                entidades = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                                                                               DataSpace.CSpace).BaseEntitySets.ToList();


                campos = (from meta in objectContext.MetadataWorkspace.GetItems(DataSpace.CSpace).Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                         let m = (meta as EntityType)
                         select m);
            }
            entidade = entidades.Where(x => x.Name == typeof(T).Name).FirstOrDefault();
            campodatabela = campos.Where(x => x.Name == typeof (T).Name).FirstOrDefault().Members;
            chavedatabela = campos.Where(x => x.Name == typeof(T).Name).FirstOrDefault().KeyMembers;
        }

        public Repositorio(DbContext contexto)
        {           
            this.contexto = contexto;

            ObjectContext objectContext = ((IObjectContextAdapter)contexto).ObjectContext;
            if (entidades == null)
            {
                entidades = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                                                                               DataSpace.CSpace).BaseEntitySets.ToList();


                campos = (from meta in objectContext.MetadataWorkspace.GetItems(DataSpace.CSpace).Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                          let m = (meta as EntityType)
                          select m);
            }
            entidade = entidades.Where(x => x.Name == typeof(T).Name).FirstOrDefault();
            campodatabela = campos.Where(x => x.Name == typeof(T).Name).FirstOrDefault().Members;
            chavedatabela = campos.Where(x => x.Name == typeof(T).Name).FirstOrDefault().KeyMembers;
        }

        public virtual string NomeTabela()
        {
            return entidade.Name;
        }

        public virtual List<string> Campos()
        {
            //return campo.Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EdmProperty).Select(w => w.Name).ToList<string>();
            List<string> listacampos = new List<string>();
            foreach (EdmProperty entityType in campodatabela.Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EdmProperty))
            {
                listacampos.Add(entityType.Name);
            }
            return listacampos;
        }

        public virtual bool ExisteDelecaoLogica()
        {
            return Campos().Exists(delegate(string s) { return s == "Ativo"; });
        }

        public virtual bool ExisteCampo(string campo)
        {
            return Campos().Exists(delegate(string s) { return s == campo; });
        }

        public virtual string ChavePrimaria()
        {
            return chavedatabela.Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EdmProperty).ToList()[0].Name;
        }

        public virtual string IdChavePrimaria(T objeto)
        {
            return contexto.Entry(objeto).Property(ChavePrimaria()).CurrentValue.ToString();
        }

        public virtual List<string> Dados(T objeto)
        {
            List<string> lista = new List<string>();
            foreach (string dado in Campos())
            {
                lista.Add(dado + " = " + contexto.Entry(objeto).Property(dado).CurrentValue.ToString());
            }
            return lista;
        }

        public virtual string DadosString(T objeto)
        {
            List<string> lista = new List<string>();
            foreach (string dado in Campos())
            {
                if (contexto.Entry(objeto).Property(dado).CurrentValue != null)
                    lista.Add(dado + " : " + contexto.Entry(objeto).Property(dado).CurrentValue.ToString());
            }
            return string.Join(",", lista);
        }

        public virtual int Incluir(T objeto)
        {                 
            if (ExisteDelecaoLogica())
                contexto.Entry(objeto).Property("Ativo").CurrentValue = 1;
            if (ExisteCampo("CreatedBy"))
                contexto.Entry(objeto).Property("CreatedBy").CurrentValue = IdUsuario;
            if (ExisteCampo("CreatedOn"))
                contexto.Entry(objeto).Property("CreatedOn").CurrentValue = DateTime.Now;
            contexto.Set<T>().Add(objeto);
            contexto.SaveChanges();

            RegistraAuditoria(objeto, "I");
            return Convert.ToInt32(contexto.Entry(objeto).Property(ChavePrimaria()).CurrentValue);           
        }

        public virtual void Excluir(T objeto)
        {
            if (!ExisteDelecaoLogica())
                contexto.Set<T>().Remove(objeto);
            else
            {
                contexto.Entry(objeto).State = EntityState.Modified;
                contexto.Entry(objeto).Property("Ativo").CurrentValue = 0;
            }

            contexto.SaveChanges();
            RegistraAuditoria(objeto, "D");
        }

        public virtual void Excluir(int id)
        {
            T item = contexto.Set<T>().Find(id);

            if (!ExisteDelecaoLogica())
                contexto.Set<T>().Remove(item);
            else
            {
                contexto.Entry(item).State = EntityState.Modified;
                contexto.Entry(item).Property("Ativo").CurrentValue = 0;
            }

            contexto.SaveChanges();
            RegistraAuditoria(item, "D");
        }

        public virtual void Alterar(T objeto)
        {
            contexto.Entry(objeto).State = EntityState.Modified;
            if (ExisteDelecaoLogica())
                contexto.Entry(objeto).Property("Ativo").CurrentValue = 1;
            if (ExisteCampo("ModifiedOn"))
                contexto.Entry(objeto).Property("ModifiedOn").CurrentValue = DateTime.Now;

            contexto.SaveChanges();
            RegistraAuditoria(objeto, "U");
        }

        public virtual T ObterPorId(int id)
        {
            return contexto.Set<T>().Find(id);
        }

        public virtual IQueryable<T> Listar()
        {
            if (ExisteDelecaoLogica())
                return contexto.Set<T>().Where("ativo = 1");
            //return contexto.Set<T>().SqlQuery("SELECT * FROM " + NomeTabela() + " where ativo = 1").AsQueryable();
            else
                return contexto.Set<T>();
        }

        public virtual IQueryable<T> Excluir(string condicao)
        {
            string comando = "";
            if (ExisteDelecaoLogica())
                comando = "update "+NomeTabela()+" set ativo = 0 where "+condicao;
            else
                comando = "delete from " + NomeTabela() + " where "+condicao;
             return contexto.Set<T>().SqlQuery(comando).AsQueryable();
        }

        public virtual IQueryable<T> ListarTudo()
        {
            return contexto.Set<T>();
        }

        public IQueryable<T> ListarDaPagina(int pagina, int qtdeporpagina)
        {
            var query = contexto.Set<T>().AsQueryable();
            if (!(contexto.Set<T>() is IOrderedQueryable<T>))
               query = query.OrderBy(ChavePrimaria());
            query = query.Skip(pagina).Take(qtdeporpagina);
            return query;
        }

        public virtual IQueryable<T> PesquisaTextual(string texto, string ordem)
        {
            if (string.IsNullOrWhiteSpace(ordem))
                ordem = ChavePrimaria();
            if (string.IsNullOrEmpty(texto))
                return Listar();
            else
                return contexto.Set<T>().SqlQuery(string.Format(StringPesquisaTextual, NomeTabela(), texto)+" order by "+ordem).AsQueryable();
        }

        public virtual IQueryable<T> ExecutarSQL(string texto)
        {
            return contexto.Set<T>().SqlQuery(texto).AsQueryable();
        }

        public virtual void Atachar(T objeto)
        {
            ObjectContext objContext = ((IObjectContextAdapter)contexto).ObjectContext;
            objContext.Attach((IEntityWithKey) objeto);
        }

        public ObjectContext ObterObjectContext()
        {
            return ((IObjectContextAdapter)contexto).ObjectContext;
        }

        public void Dispose()
        {
            contexto.Dispose();
        }

        public void RegistraAuditoria(T objeto, string tipo)
        {
            string tabela = NomeTabela();
            if (!tabela.StartsWith("Auditoria"))
                RegistraAuditoria(tabela, Convert.ToInt32(IdChavePrimaria(objeto)), DadosString(objeto), IdUsuario, IdRecurso, IdModulo, IdPerfil, tipo);
        }

        public void RegistraAuditoria(string tabela, int chave, string registro, int idusuario, int idrecurso, int idmodulo, int idperfil, string tipo)
        {
            string browser = ObtemBrowser(HttpContext.Current.Request);
            string ip = ObtemIP(HttpContext.Current.Request);

            Auditoria a = new Auditoria();
            a.Data = DateTime.Now;
            if (idusuario > 0)
                a.IDUsuario = idusuario;
            if (idperfil > 0)
                a.IDPerfil = idperfil;
            if (idrecurso > 0)
                a.IDRecurso = idrecurso;
            if (idmodulo > 0)
                a.IDModulo = idmodulo;
            a.TipoOperacao = tipo;
            a.Tabela = tabela;
            a.Chave = chave;
            a.Registro = registro;
            a.Browser = browser;
            a.IP = ip;

            new Repositorio<Auditoria>().Incluir(a);

        }

        public static string ObtemIP(HttpRequest request)
        {
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip != "")
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string ObtemBrowser(HttpRequest request)
        {
            System.Web.HttpBrowserCapabilities browser = request.Browser;
            string s = browser.Type + "\n"
                + " " + browser.Browser + "\n"
                + " " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " + (browser.EcmaScriptVersion.ToString().CompareTo("1") >= 0 ? "Sim " : "Nao ") + browser.EcmaScriptVersion.ToString() + "\n"
                + "Supports JavaScript Version = " + browser["JavaScriptVersion"] + "\n";
            return s;
        }

    }
}
