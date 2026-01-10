using System;
using System.Collections.Generic;
using System.Web.UI;
using CP.FastConsig.DAL;

namespace CP.FastConsig.WebApplication.Auxiliar
{

	public sealed class GerenciadorSessao : Page
	{

		#region Constantes

		private const string ParametroIdSessao = "IdSessao";
		private const string ParametroPathWebCamImagemTemp = "PathWebCamImagemTemp";
		private const string ParametroUsuarioLogado = "UsuarioLogado";
		private const string ParametroIdModulo = "IdModulo";
		private const string ParammetroMatricula = "Matricula";
		private const string ParametroIdPerfil = "IdPerfil";
		private const string ParametroIdBanco = "IdBanco";
		private const string ParametroIdAgente = "IdAgente";
		private const string ParametroIdUsuario = "IdUsuario";
		private const string ParametroIdRecurso = "IdRecursoAtual";
		private const string ParametroNomeRecurso = "NomeRecursoAtual";
		private const string ParametroNomeStringConexao = "NomeStringConexao";
		private const string ParametroNomeStringConexaoSemEntity = "NomeStringConexaoSemEntity";
		private const string ParametroHistoricoNavegacaoRecursos = "HistoricoNavegacaoRecursos";
		private const string ParametroPastaUpload = "PastaUpload";
		private const string ParametroVoltou = "Voltou";
		
		#endregion

		public GerenciadorSessao(Page pagina)
		{
			Page = pagina;
		}

		public string IdSessao
		{
			get
			{
				if (Session[ParametroIdSessao] == null) Session[ParametroIdSessao] = Guid.NewGuid().ToString();
				return (string)Session[ParametroIdSessao];
			}
		}

		public string NomeStringConexao
		{

			get
			{
				return (string)Session[ParametroNomeStringConexao];
			}
			set
			{
				Session[ParametroNomeStringConexao] = value;
			}

		}

		public string PastaUpload
		{

			get
			{
				return (string)Session[ParametroPastaUpload];
			}
			set
			{
				Session[ParametroPastaUpload] = value;
			}

		}

		public string NomeStringConexaoSemEntity
		{

			get
			{
				return (string)Session[ParametroNomeStringConexaoSemEntity];
			}
			set
			{
				Session[ParametroNomeStringConexaoSemEntity] = value;
			}

		}

		public string PathWebCamImagemTemp
		{
			get
			{
				if (Session[ParametroPathWebCamImagemTemp] == null) Session[ParametroPathWebCamImagemTemp] = string.Empty;
				return (string)Session[ParametroPathWebCamImagemTemp];
			}
			set
			{
				Session[ParametroPathWebCamImagemTemp] = value;
			}
		}

		public bool Voltou
		{
			get
			{
				
				if (Session[ParametroVoltou] == null) Session[ParametroVoltou] = false;
				
				bool retorno = (bool) Session[ParametroVoltou];
				
				Session[ParametroVoltou] = false;

				return retorno;

			}
			set
			{
				Session[ParametroVoltou] = value;
			}
		}

		public Stack<int> HistoricoNavegacaoRecursos
		{
			get
			{
				if (Session[ParametroHistoricoNavegacaoRecursos] == null) Session[ParametroHistoricoNavegacaoRecursos] = new Stack<int>();
				return (Stack<int>)Session[ParametroHistoricoNavegacaoRecursos];
			}
			set
			{
				Session[ParametroHistoricoNavegacaoRecursos] = value;
			}
		}  

		public Usuario UsuarioLogado
		{
			get
			{
				if (Session[ParametroUsuarioLogado] == null) Session[ParametroUsuarioLogado] = null;
				return (Usuario) Session[ParametroUsuarioLogado];
			}
			set
			{
				Session[ParametroUsuarioLogado] = value;
			}
		}

		public bool Logado
		{
			get { return UsuarioLogado != null; }
		}

		public int IdModulo
		{
			get
			{
				if (Session[ParametroIdModulo] == null) Session[ParametroIdModulo] = 0;
				return (int) Session[ParametroIdModulo];
			}
			set
			{
				Session[ParametroIdModulo] = value;
			}
		}

		public int IdUsuario
		{
			get
			{
				if (Session[ParametroIdUsuario] == null) Session[ParametroIdUsuario] = 0;
				return (int)Session[ParametroIdUsuario];
			}
			set
			{
				Session[ParametroIdUsuario] = value;
			}
		}

		public int IdRecurso
		{
			get
			{
				if (Session[ParametroIdRecurso] == null) Session[ParametroIdRecurso] = 0;
				return (int)Session[ParametroIdRecurso];
			}
			set
			{
				Session[ParametroIdRecurso] = value;
			}
		}

		public string NomeRecurso
		{
			get
			{
				if (Session[ParametroNomeRecurso] == null) Session[ParametroNomeRecurso] = "";
				return (string)Session[ParametroNomeRecurso];
			}
			set
			{
				Session[ParametroNomeRecurso] = value;
			}
		}

		public string Matricula
		{
			get
			{
				if (Session[ParammetroMatricula] == null) Session["Matricula"] = 0;
				return (string)Session["Matricula"];
			}
			set
			{
				Session["Matricula"] = value;
			}
		}

		public int IdPerfil
		{
			get
			{
				if (Session[ParametroIdPerfil] == null) Session[ParametroIdPerfil] = 0;
				return (int)Session[ParametroIdPerfil];
			}
			set
			{
				Session[ParametroIdPerfil] = value;
			}
		}

		public int IdBanco
		{
			get
			{
				if (Session[ParametroIdBanco] == null) Session[ParametroIdBanco] = 0;
				return (int)Session[ParametroIdBanco];
			}
			set
			{
				Session[ParametroIdBanco] = value;
			}
		}

		public int IdAgente
		{
			get
			{
				if (Session[ParametroIdAgente] == null) Session[ParametroIdAgente] = 0;
				return (int)Session[ParametroIdAgente];
			}
			set
			{
				Session[ParametroIdAgente] = value;
			}
		}

		public void Finaliza()
		{
			try
			{
				Session.Abandon();
			}
			catch (Exception ex) { }
		}

	}

}