namespace CP.FastConsig.Common
{

	public static class Enums
	{
		
		public enum ConsignanteTipoCenter
		{
			Estado,
			Cidade,
			Autarquia
		}

		public enum AcaoHistoricoUsuario
		{
			Alteracao,
			TrocaSenha,
			Remocao,
			Inclusao
		}

		public enum TipoBloqueioUsuario
		{
			A, // Ativo
			B // Bloqueado
		}

		public enum Modulos
		{
			Consignante = 1,
			Funcionario = 2,
			Consignataria = 3,
			Agente = 4,
			Sindicato = 6
		}

		public enum Perfil
		{
			MasterConsignataria = 3,
			Agente = 4
		}

		public enum EmpresaTipo
		{
			CasePartners = 1,
			Consignante = 2,
			Agente = 3,
			Banco = 4,
			Financeira = 5,
			Sindicato = 6,
			Associacao = 7,
			Convenio = 8,
		}

		public enum BloqueioTipo
		{
			Completo,
			TipoProduto,
			TipoEmpresa
		}

		public enum  BloqueioPeriodo
		{
			I,
			D
		}

		public enum Recursos
		{
			Funcionarios = 238,
			Averbacaos = 239,
			RetornoFolha = 240,
			Personaliado = 241,
			CentralImportacao = 17,
			ConsignatariaCentralSimulacao = 269,
			DashBoardConsignataria = 130
		}

		public enum ProdutoGrupo
		{
			Emprestimos = 1,
			Mensalidades = 7
		}

		public enum AverbacaoSituacao
		{
			Cancelado = 0,
			Ativo = 1,
			Averbado = 2,
			AguardandoAprovacao = 3,
			Reservado = 4,
			Desaprovado = 5,
			Suspenso_MargemLivre = 6,
			Bloqueado_MargemRetida = 7,
			EmProcessodeCompra = 8,
			Comprado = 9,
			Liquidado = 10,
			Concluido = 11,
			PreReserva = 12
		}

		public enum SolicitacaoSituacao
		{
			Cancelado = 0,
			Pendente = 1,
			Processada = 2,
			EmAnalise = 3,
			Redirecionada = 4,
			Rejeitada = 5
		}

		public enum SolicitacaoTipo
		{
			InformarSaldoDevedordeContratos = 1,
			InformarQuitacao = 2,
			ConfirmarRejeitarQuitacao = 3,
			ConcluirCompradeDivida = 4,
			AprovarAverbacoes = 5,
			DesliquidarAverbacoes = 6,
			CancelarAverbacoes = 7,
			InformarPessoasdeContato = 8,
			HomologarAverbacoes = 9,
			AcompanharClientes = 10,
			AprovarReservaporSimulacao = 11,
			RegularizarQuitaçãoRejeitada = 12,
			Ouvidoria = 13,
			AcompanharAverbacaoSemPrimeiroDesconto = 14,
			MinhasSolicitacoesdeCompraAguardandoSaldoDevedor = 15,
			MinhasSolicitacoesdeCompraAguardandoLiquidacao = 16,
			SolicitacaoDeInformacao = 17,
			SolicitacaoEmprestimo = 18
		}

		public enum AverbacaoParcelaSituacao
		{
			Cancelada = 0,
			Aberta = 1,
			LiquidadaFolha = 2,
			LiquidadaManual = 3,
			RejeitadaFolha = 4
		}

		public enum AverbacaoTipo
		{
			Normal = 1,
			Compra = 2,
			Renegociacao = 3,
			CompraERenegociacao = 4
		}

		public enum Parametros
		{
			DiaCorte = 1,
		}

		public enum EmpresaSituacao
		{
			Normal = 1,
			SuspensoAverbacoes = 2,
			SuspensoCompra = 3,
			Bloqueado = 4,
			BloqueioPersonalizado = 5
		}

		public enum ProdutoTipo
		{
			PrazoDeterminado_ParcelaFixa = 1,
			PrazoIndeterminado_ParcelaFixa = 2,
			PrazoIndeterminado_ParcelaVariavel = 3
		}

		public enum FuncionarioSituacao
		{
			NaoInformado = 0,
			AtivoNaFolha = 1,
			RetiradoDaFolha = 2,
			Exonerado = 3,
			Bloqueado = 4,
            Aposentado = 5
		}

		public enum Permissao
		{
			Acessar = 1,
			Incluir = 2,
			Alterar = 3,
			Excluir = 4,
			Consultar = 5,
			QuadroParametros = 6,
			QuadroOcorrencias = 7,
			QuadroSolicitacoes = 8,
			QuadroMensagens = 9,
			GraficoValoresDescontados = 10,
			RankingBancos = 11,
			ResumoOperacoes = 12,
			ResumoFuncionarios = 13,
			FluxoContratos = 14,
			GraficoUtilizacaoMargem = 15,
			ConfigurarConteudo = 16,
			AjustarMargem = 17,
			RemanejarMargem = 18,
			Rescindir = 19,
			Aposentar = 20,
			Bloquear = 21,
			GerarSenhaProvisoria = 22,
			VisualizarContratos = 23,
			VerHistoricoAlteracoes = 24,
			VerExtratoMargem = 25,
			RegistrarAutorizacoesEspeciais = 26,
			RemanejarContratosMatriculas = 27,
			DefinirServicosVerbas = 28,
			DefinirTarifacao = 29,
			SuspenderAtivar = 30,
			Liquidar = 31,
			AprovarDesaprovar = 32,
			Desliquidar = 33,
			ConsultarApenasRegistradosPeloUsuario = 35,
			InformarSaldoDevedor = 36,
			InformarQuitacao = 37,
			Exportar = 38,
			Cancelar = 39,
			QuadroIndiceDeNegocios = 40,
			IndiceNegocioApenasContratosRegistradosPeloUsuario = 41,
			PermitirMostrarLucroIndicesDeNegocio = 42,
			AverbacaoSimples = 44,
			Refinancimentos = 45,
			Compra = 46,
			CompraComRenegociacao = 47,
			AlterarPrimeiroMesDesconto = 48
		}

		public enum FuncionarioAutorizacaoTipo
		{
			Independentedequalquerrestricao = 0,
			IndependentedeMargem = 1,
			IndependentedeSituacao = 2,
			IndependentedeBloqueio = 3
		}

		public enum CancelamentoIndevido
		{
			ParticipandoCompra = 1,
			ExisteSolicitacoesProcessadas = 2,
			Ok = 3
		}

		public enum TipoPagamento
		{
			Boleto = 1,
			TED = 2
		}

		public enum TipoCadastradorCenter
		{
			B, // Banco
			C, // Consignante
			U, // Usuário
		}

		public enum TipoSimulacaoDivida 
		{
			ObterMaisDinheiro = 0,
			RegularizarMargem = 1,
			ReduzirValorPago = 2,
			DiminuirQuantidadeParcelas = 3
		}

	}

}