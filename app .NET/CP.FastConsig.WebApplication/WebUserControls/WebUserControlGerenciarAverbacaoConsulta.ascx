<%@ Control ClassName="WebUserControlGerenciarAverbacaoConsulta" EnableViewState="true"
	Language="C#" AutoEventWireup="True" CodeBehind="WebUserControlGerenciarAverbacaoConsulta.ascx.cs"
	Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlGerenciarAverbacaoConsulta" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="~/WebUserControls/WebUserControlTermoAverbacao.ascx" TagName="WebUserControlTermoAverbacao"
	TagPrefix="uc1" %>
<div class="PreencheBaseTela">
	<!-- Menu Gerenciar Averbacaos Detalhes -->
	<div>
		<table border="0" cellpadding="0" cellspacing="5" width="100%">
			<tr>
				<td style="padding: 5px 0px;background:transparent url(/Imagens/BgMenuAverbacoes.png) repeat-x top left;">
<%--                    <div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonImprimirTela" EnableDefaultAppearance="false"
							CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Imprimir Tela">
						</dx:ASPxButton>
					</div>--%>
<%--                    <div class="float-divider" style="border: none;">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonImprimirTermo" EnableDefaultAppearance="false"
							CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Imprimir Termo">
						</dx:ASPxButton>
					</div>--%>
					<div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonSuspenderBloquear" CssClass="BotaoEstiloGlobal"
							runat="server" EnableDefaultAppearance="false" EnableTheming="false" Text="Suspender/Bloquear"
							OnClick="ASPxButtonSuspender_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonReimprimirTermo" CssClass="BotaoEstiloGlobal"
							runat="server" EnableDefaultAppearance="false" EnableTheming="false" Text="Reimprimir Termo">
						</dx:ASPxButton>
						<dx:ASPxPopupControl LoadContentViaCallback="OnPageLoad" OnLoad="ASPxPopupControlTermo_Load" BackColor="#ffffff" DragElement="Header" LoadingPanelText="Carregando..." ID="ASPxPopupControlTermo"
								runat="server" CloseAction="OuterMouseClick" PopupElementID="ASPxButtonReimprimirTermo"
								PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" AllowDragging="False"
								ForeColor="#000000" Font-Bold="true" Width="700px" HeaderText="Comprovante" ClientInstanceName="aSPxPopupControlTermo">
								<HeaderStyle ForeColor="#000000" Font-Bold="true" CssClass="AlturaCabecalhoPopControl" />
								<ContentCollection>
									<dx:PopupControlContentControl ID="PopupControlReimpressaoTermo" runat="server">
										<uc1:WebUserControlTermoAverbacao ID="WebUserControlTermoAverbacaoImpressao" runat="server" />
									</dx:PopupControlContentControl>                                    
								</ContentCollection>
							</dx:ASPxPopupControl>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonAtivar" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Ativar">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonLiquidar" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Liquidar" OnClick="ASPxButtonLiquidar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonAprovar" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Aprovar" OnClick="ASPxButtonAprovar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonDesaprovar" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Desaprovar" OnClick="ASPxButtonDesaprovar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonSolicitarSaldoDevedor" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Solicitar Saldo Devedor">
						</dx:ASPxButton>
					</div>                    
					<div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonCancelar" runat="server" Text="Cancelar" CssClass="BotaoEstiloGlobal"
							EnableDefaultAppearance="false" EnableTheming="false" OnClick="ASPxButtonCancelar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonInformarSaldoDevedor" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Informar Saldo Devedor" OnClick="ASPxButtonInformarSaldoDevedor_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonInformarQuitacao" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Confirmar/Rejeitar Quitação" OnClick="ASPxButtonConfirmarQuitacao_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider"  style="border: none;">
						<dx:ASPxButton ID="ASPxButtonConcluirCompra" runat="server" EnableDefaultAppearance="false" EnableTheming="false"
							CssClass="BotaoEstiloGlobal" Text="Concluir Compra">
						</dx:ASPxButton>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<div style="height: 1px; clear: both; overflow: hidden; width: 100%;">
		&nbsp;</div>
	<!-- Fim -->
	<!-- Bloco com os dados do Averbacao -->
	<div>
		<!-- Tabela populada com os dados do Funcionario -->
		<table id="Table1" runat="server" width="100%" border="0" cellspacing="1" cellpadding="4"
			class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Dados do Funcionário</h1>
				</td>
			</tr>
			<tr>
				<td>
					<table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
						<tr>
							<td rowspan="9" class="LarguraBordaCelula">
								<img src="/Imagens/Perfil.png" alt="" width="65px" height="74px" />
							</td>
							<td class="TituloNegrito" style="width: 20%;">
								<asp:Label ID="Label3" runat="server" Text="Matricula:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtMatricula" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label1" runat="server" Text="Nome:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtNome" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label2" runat="server" Text="CPF:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtCPF" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label5" runat="server" Text="RG:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtRG" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label6" runat="server" Text="Data Nasc:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtDataNasc" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label17" runat="server" Text="Situação:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtSituacaoFunc" runat="server"></asp:Label>
							</td>
						</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label16" runat="server" Text="Regime:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtRegime" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label20" runat="server" Text="Local:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtLocal" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito" id="UltimaCelulaSemBordaBaseSituacao">
										<asp:Label ID="Label26" runat="server" Text="Data Adm:"></asp:Label>
									</td>
									<td class="UltimaCelulaSemBordaBase">
										<asp:Label ID="txtDataAdm" runat="server"></asp:Label>
									</td>
								</tr>

					</table>
				</td>
			</tr>
			<tr>
				<%--                <td colspan="2" class="CelulaBotaoMaisDetalhes" >
					<asp:LinkButton ID="Label7" CssClass="TituloBotaoMaisDetalhes" runat="server" Text="Mais Detalhes..." OnClick="PessoaisDetalhes_Click"></asp:LinkButton>
				</td>--%>
				<td>
					<h2 class="trigger">
						<a id="aTitulo" onclick="return false;" href="#">Mais Detalhes</a>
					</h2>
					<div id="DivMaisDetalhes" class="toggle_container">
						<div class="block">
							<table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
								<tr>
									<td rowspan="15" class="LarguraBordaCelula">
									</td>
								</tr>
								<tr>
									<td style="width: 20%;" class="TituloNegrito">
										<asp:Label ID="Label4" runat="server" Text="Categoria:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtCategoria" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label22" runat="server" Text="Setor:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtSetor" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label24" runat="server" Text="Cargo:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtCargo" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label8" runat="server" Text="Endereço:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtEndereco" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label9" runat="server" Text="Complemento:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtComplemento" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label10" runat="server" Text="Bairro:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtBairro" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label11" runat="server" Text="Cidade/UF:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtCidade" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label12" runat="server" Text="Cep:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtCep" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label13" runat="server" Text="Telefone:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtTelefone" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label14" runat="server" Text="Email:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtEmail" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito" id="UltimaCelulaSemBordaBaseCelular">
										<asp:Label ID="Label15" runat="server" Text="Celular:"></asp:Label>
									</td>
									<td class="UltimaCelulaSemBordaBase">
										<asp:Label ID="txtCelular" runat="server"></asp:Label>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<!-- fim tabela-->
		<!-- Tabela populada com os dados da consignatária -->
		<table runat="server" width="100%" border="0" cellspacing="1" cellpadding="4" class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Dados da Consignatária</h1>
				</td>
			</tr>
			<tr>
				<td>
					<table class="WebUserControlTabela" id="Table2" runat="server" width="100%" border="0"
						cellspacing="0" cellpadding="0">
						<tr>
							<td rowspan="4" class="LarguraBordaCelula">
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="width: 20%;">
								<asp:Label ID="Label18" runat="server" Text="Consignatária:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtConsignataria" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label19" runat="server" Text="Correspondente:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtAgente" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="border: none !important;">
								<asp:Label ID="Label21" runat="server" Text="Usuário:"></asp:Label>
							</td>
							<td class="UltimaCelulaSemBordaBase">
								<asp:Label ID="txtUsuario" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<!-- fim tabela -->
		<!-- Tabela populada com os dados do Averbacao -->
		<table id="Table3" runat="server" width="100%" border="0" cellspacing="1" cellpadding="4"
			class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Dados da Averbação</h1>
				</td>
			</tr>
			<tr>
				<td>
					<table class="WebUserControlTabela" runat="server" width="100%" border="0" cellspacing="0"
						cellpadding="0">
						<tr>
							<td rowspan="12" class="LarguraBordaCelula">
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="width: 20%;">
								<asp:Label ID="Label23" runat="server" Text="Número:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtNumero" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="width: 20%;">
								<asp:Label ID="Label7" runat="server" Text="Identificador:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="LabelIdentificador" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label27" runat="server" Text="Data:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtData" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label29" runat="server" Text="Grupo de Produto:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtTipoProduto" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label31" runat="server" Text="Operação:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtOperacao" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label33" runat="server" Text="Situação:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtSituacaoAverbacao" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label25" runat="server" Text="Prazo:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtPrazo" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label30" runat="server" Text="Valor Parcela:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtValorParcela" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label34" runat="server" Text="Mês Início:"></asp:Label>
							</td>
							<td>
								<asp:Label ID="txtMesInicio" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								<asp:Label ID="Label36" runat="server" Text="Mês Final:"></asp:Label>
							</td>
							<td style="text-align: left; background-color: #ffffff;">
								<asp:Label ID="txtMesFim" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="border: none !important;">
								<asp:Label ID="Label28" runat="server" Text="Produto:"></asp:Label>
							</td>
							<td class="UltimaCelulaSemBordaBase">
								<asp:Label ID="txtProduto" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<%--                <td colspan="2" class="CelulaBotaoMaisDetalhes">
					<asp:LinkButton ID="LinkButton1" CssClass="TituloBotaoMaisDetalhes" runat="server"
						Text="Mais Detalhes..." OnClick="AverbacaoDetalhes_Click"></asp:LinkButton>
				</td>
				--%>
				<td>
					<h2 class="trigger2">
						<a id="aTitulo2" onclick="return false;" href="#">Mais Detalhes</a>
					</h2>
					<div id="DivMaisDetalhes2" class="toggle_container2">
						<div class="block">
							<table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
								<tr>
									<td rowspan="6" class="LarguraBordaCelula">
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito" style="width: 20%;">
										<asp:Label ID="Label37" runat="server" Text="Valor Contrato:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtValorAverbacao" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label39" runat="server" Text="Valor Consignado:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtValorConsignado" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label41" runat="server" Text="CET:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtJuros" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito">
										<asp:Label ID="Label43" runat="server" Text="Coeficiente:"></asp:Label>
									</td>
									<td>
										<asp:Label ID="txtCoeficiente" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="TituloNegrito" style="border: none !important;">
										<asp:Label ID="Label45" runat="server" Text="Obs.:"></asp:Label>
									</td>
									<td class="UltimaCelulaSemBordaBase">
										<asp:Label ID="txtObs" runat="server"></asp:Label>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<!-- fim tabela -->
		<!-- GridView populado com os dados das parcelas do Averbação -->
		<table width="100%" border="0" cellspacing="1" cellpadding="4" class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Parcelas da Averbação</h1>
				</td>
			</tr>
			<tr>
				<td class="PreencherGridView">
					<dx:ASPxGridView EnableTheming="True" ID="gridParcelas" ClientInstanceName="gridParcelas"
						runat="server" KeyFieldName="IDAverbacaoParcela" Width="100%" AutoGenerateColumns="False"
						CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SettingsPager-PageSize="12">
						<Columns>
							<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Parcela" />
							<dx:GridViewDataColumn FieldName="Competencia" VisibleIndex="2" Caption="Mês/Ano" />
							<dx:GridViewDataColumn FieldName="Valor" VisibleIndex="3" Caption="Valor" />
							<dx:GridViewDataColumn FieldName="ValorDescontado" VisibleIndex="5" Caption="Descontado" />
							<dx:GridViewDataColumn FieldName="AverbacaoParcelaSituacao.Nome" VisibleIndex="4"
								Caption="Situação" />                            
							<dx:GridViewDataColumn FieldName="Observacao" VisibleIndex="5" Caption="Obs." />
						</Columns>
						<Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
							<LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
							</LoadingPanelOnStatusBar>
							<LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
							</LoadingPanel>
						</Images>
						<ImagesEditors>
							<DropDownEditDropDown>
								<SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
							</DropDownEditDropDown>
							<SpinEditIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
							</SpinEditIncrement>
							<SpinEditDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
							</SpinEditDecrement>
							<SpinEditLargeIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
							</SpinEditLargeIncrement>
							<SpinEditLargeDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
							</SpinEditLargeDecrement>
						</ImagesEditors>
						<ImagesFilterControl>
							<LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
							</LoadingPanel>
						</ImagesFilterControl>
						<Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
							<LoadingPanel ImageSpacing="8px">
							</LoadingPanel>
						</Styles>
						<StylesEditors>
							<CalendarHeader Spacing="1px">
							</CalendarHeader>
							<ProgressBar Height="25px">
							</ProgressBar>
						</StylesEditors>
						<SettingsLoadingPanel ImagePosition="Top" />
						<SettingsCustomizationWindow Enabled="True" />
						<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />

					</dx:ASPxGridView>
				</td>
			</tr>
		</table>
		<!-- fim tabela -->
		<!-- GridView populado com os dados da tramitação do Averbação -->
		<table width="100%" border="0" cellspacing="1" cellpadding="4" class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Tramitação da Averbação</h1>
				</td>
			</tr>
			<tr>
				<td class="PreencherGridView">
					<dx:ASPxGridView EnableTheming="True" ID="gridTramitacao" ClientInstanceName="gridTramitacao"
						runat="server" KeyFieldName="IDAverbacaoTramitacao" Width="100%" AutoGenerateColumns="False"
						CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
						<Columns>
							<dx:GridViewDataDateColumn Width="250px" FieldName="CreatedOn" VisibleIndex="1" Caption="Data/Hora">
								<PropertiesDateEdit DisplayFormatString="{0:g}">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="2" Caption="Situação" />
							<dx:GridViewDataColumn FieldName="Empresa.Nome" VisibleIndex="3" Caption="Empresa" />
							<dx:GridViewDataColumn FieldName="Usuario.Nome" VisibleIndex="4" Caption="Usuário" />
							<dx:GridViewDataColumn FieldName="OBS" VisibleIndex="5" Caption="Obs." />
						</Columns>
						<Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
							<LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
							</LoadingPanelOnStatusBar>
							<LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
							</LoadingPanel>
						</Images>
						<ImagesEditors>
							<DropDownEditDropDown>
								<SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
							</DropDownEditDropDown>
							<SpinEditIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
							</SpinEditIncrement>
							<SpinEditDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
							</SpinEditDecrement>
							<SpinEditLargeIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
							</SpinEditLargeIncrement>
							<SpinEditLargeDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
							</SpinEditLargeDecrement>
						</ImagesEditors>
						<ImagesFilterControl>
							<LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
							</LoadingPanel>
						</ImagesFilterControl>
						<Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
							<LoadingPanel ImageSpacing="8px">
							</LoadingPanel>
						</Styles>
						<StylesEditors>
							<CalendarHeader Spacing="1px">
							</CalendarHeader>
							<ProgressBar Height="25px">
							</ProgressBar>
						</StylesEditors>
						<SettingsLoadingPanel ImagePosition="Top" />
						<SettingsCustomizationWindow Enabled="True" />
						<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
					</dx:ASPxGridView>
				</td>
			</tr>
		</table>


<table width="100%" border="0" cellspacing="1" cellpadding="4" class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Solicitações da Averbação</h1>
				</td>
			</tr>
			<tr>
				<td class="PreencherGridView">
					<dx:ASPxGridView EnableTheming="True" ID="gridSolicitacoes" ClientInstanceName="gridSolicitacoes"
						runat="server" KeyFieldName="IDEmpresaSolicitacao" Width="100%" AutoGenerateColumns="False"
						CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
						<Columns>
							<dx:GridViewDataDateColumn Width="250px" FieldName="DataSolicitacao" VisibleIndex="1" Caption="Data/Hora">
								<PropertiesDateEdit DisplayFormatString="{0:g}">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataColumn FieldName="EmpresaSolicitacaoTipo.Nome" VisibleIndex="2" Caption="Tipo" />
							<%--<dx:GridViewDataColumn FieldName="Empresa.Nome" VisibleIndex="4" Caption="Empresa" />--%>
							<dx:GridViewDataColumn FieldName="Usuario.Nome" VisibleIndex="3" Caption="Solicitante" />                            
							<dx:GridViewDataColumn FieldName="EmpresaSolicitacaoSituacao.Nome" VisibleIndex="4" Caption="Situação" />
							<dx:GridViewDataColumn FieldName="Usuario1.Nome" VisibleIndex="5" Caption="Atendido por" />
							<dx:GridViewDataColumn FieldName="Motivo" VisibleIndex="6" Caption="Obs" />
						</Columns>
						<Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
							<LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
							</LoadingPanelOnStatusBar>
							<LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
							</LoadingPanel>
						</Images>
						<ImagesEditors>
							<DropDownEditDropDown>
								<SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
							</DropDownEditDropDown>
							<SpinEditIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
							</SpinEditIncrement>
							<SpinEditDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
							</SpinEditDecrement>
							<SpinEditLargeIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
							</SpinEditLargeIncrement>
							<SpinEditLargeDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
							</SpinEditLargeDecrement>
						</ImagesEditors>
						<ImagesFilterControl>
							<LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
							</LoadingPanel>
						</ImagesFilterControl>
						<Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
							<LoadingPanel ImageSpacing="8px">
							</LoadingPanel>
						</Styles>
						<StylesEditors>
							<CalendarHeader Spacing="1px">
							</CalendarHeader>
							<ProgressBar Height="25px">
							</ProgressBar>
						</StylesEditors>
						<SettingsLoadingPanel ImagePosition="Top" />
						<SettingsCustomizationWindow Enabled="True" />
						<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
					</dx:ASPxGridView>
				</td>
			</tr>
		</table>        <!-- Fim GridView -->
		<!-- GridView populado com os dados dos Averbaçoes vinculadas -->
		<table id="TableAverbacoesVinculadas" runat="server" width="100%" border="0" cellspacing="1" cellpadding="4" class="MargemTopoTabela">
			<tr>
				<td class="BordaBase">
					<h1 class="TituloTabela">
						Averbações Vinculadas</h1>
				</td>
			</tr>
			<tr>
				<td class="PreencherGridView">
					<dx:ASPxGridView EnableTheming="True" ID="gridAverbacaosVinc" ClientInstanceName="gridAverbacaosVinc"
						runat="server" KeyFieldName="IDAverbacao" Width="100%" AutoGenerateColumns="False"
						CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" OnRowCommand="gridVinculos_RowCommand">
						<Columns>
							<dx:GridViewCommandColumn VisibleIndex="0">
								<ClearFilterButton Visible="False" Text="Limpar" />
							</dx:GridViewCommandColumn>
							<dx:GridViewDataColumn VisibleIndex="1" Width="25px">
								<DataItemTemplate>
									<asp:LinkButton runat="server" ID="select" CommandName="Select">
										<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Detalhes da Averbação" title="Detalhes da Averbação"  />
									</asp:LinkButton>
								</DataItemTemplate>
							</dx:GridViewDataColumn>

							<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
							<dx:GridViewDataColumn FieldName="Data" VisibleIndex="2" />
							<dx:GridViewDataColumn FieldName="Empresa1.Nome" VisibleIndex="3" Caption="Consignatária" />
							<%--<dx:GridViewDataColumn FieldName="Produto.Nome" VisibleIndex="4" Caption="Produto" />--%>
							<dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="5" Caption="Valor Parcela" />
							<dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="5" />
							<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="5" Caption="Situação" />
							<dx:GridViewDataColumn FieldName="UltimaSolicitacao" VisibleIndex="6" Caption="Ult.Solicitação" />
						</Columns>
						<Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
							<LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
							</LoadingPanelOnStatusBar>
							<LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
							</LoadingPanel>
						</Images>
						<ImagesEditors>
							<DropDownEditDropDown>
								<SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
							</DropDownEditDropDown>
							<SpinEditIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
							</SpinEditIncrement>
							<SpinEditDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
							</SpinEditDecrement>
							<SpinEditLargeIncrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
							</SpinEditLargeIncrement>
							<SpinEditLargeDecrement>
								<SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
									PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
							</SpinEditLargeDecrement>
						</ImagesEditors>
						<ImagesFilterControl>
							<LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
							</LoadingPanel>
						</ImagesFilterControl>
						<Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
							<LoadingPanel ImageSpacing="8px">
							</LoadingPanel>
						</Styles>
						<StylesEditors>
							<CalendarHeader Spacing="1px">
							</CalendarHeader>
							<ProgressBar Height="25px">
							</ProgressBar>
						</StylesEditors>
						<Templates>
							<DetailRow>
								<div style="padding: 3px 3px 2px 3px">
									<dx:ASPxPageControl EnableTheming="True" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
										runat="server" ID="pageControl" Width="100%" EnableCallBacks="true">
										<TabPages>
											<dx:TabPage Text="Averbacaos Vinculados (Compra/Renegociação)" Visible="true">
												<ContentCollection>
													<dx:ContentControl runat="server" ID="ContentControl1">
														<dx:ASPxGridView ID="gridVinculos" runat="server" KeyFieldName="IDAverbacao" Width="100%"
															OnDataBinding="gridVinculos_DataSelect">
															<Columns>
																<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
																<dx:GridViewDataColumn FieldName="Data" VisibleIndex="2" />
																<dx:GridViewDataColumn FieldName="Empresa.Nome" VisibleIndex="3" Caption="Consignatária" />
																<dx:GridViewDataColumn FieldName="Produto.Nome" VisibleIndex="4" Caption="Produto" />
																<dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="5" Caption="Valor Parcela" />
																<dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="5" />
																<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="5" Caption="Situação" />
																<dx:GridViewDataColumn FieldName="UltimaTramitacao" VisibleIndex="5" Caption="Ult.Tramitação" />
															</Columns>
															<%--                                <SettingsDetail ShowDetailRow="true" />
															--%>
															<Settings ShowFooter="True" />
															<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
														</dx:ASPxGridView>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
											<dx:TabPage Text="Parcelas" Visible="true">
												<ContentCollection>
													<dx:ContentControl runat="server" ID="ContentControl2">
														<dx:ASPxGridView ID="ASPxGridViewParcelas" runat="server" KeyFieldName="IDAverbacaoParcela"
															Width="100%" OnDataBinding="gridParcelas_DataSelect">
															<Columns>
																<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
																<dx:GridViewDataColumn FieldName="Competencia" VisibleIndex="2" Caption="Mês/Ano" />
																<dx:GridViewDataColumn FieldName="Valor" VisibleIndex="3" Caption="Valor" />
																<dx:GridViewDataColumn FieldName="AverbacaoParcelaSituacao.Nome" VisibleIndex="4"
																	Caption="Situação" />
																<dx:GridViewDataColumn FieldName="ValorDescontado" VisibleIndex="5" Caption="Descontado" />
																<dx:GridViewDataColumn FieldName="Observacao" VisibleIndex="5" Caption="Obs." />
															</Columns>
															<%--                                <SettingsDetail ShowDetailRow="true" />
															--%>
															<Settings ShowFooter="True" />
														   <SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />

														</dx:ASPxGridView>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
											<dx:TabPage Text="Tramitações" Visible="true">
												<ContentCollection>
													<dx:ContentControl runat="server" ID="ContentControl3">
														<dx:ASPxGridView ID="ASPxGridViewTramitacoes" runat="server" KeyFieldName="IDAverbacaoTramitacao"
															Width="100%" OnDataBinding="gridTramitacoes_DataSelect">
															<Columns>
																<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="4" Caption="Situação" />
																<dx:GridViewDataColumn FieldName="OBS" VisibleIndex="5" Caption="Obs." />
																<dx:GridViewDataColumn FieldName="CreatedOn" VisibleIndex="5" Caption="Data" />
																<dx:GridViewDataColumn FieldName="Usuario.Nome" VisibleIndex="5" Caption="Usuário" />
															</Columns>
															<%--                                <SettingsDetail ShowDetailRow="true" />
															--%>
															<Settings ShowFooter="True" />
															<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />

														</dx:ASPxGridView>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
										</TabPages>
									</dx:ASPxPageControl>
								</div>
							</DetailRow>
						</Templates>
						<SettingsLoadingPanel ImagePosition="Top" />
						<SettingsDetail ShowDetailRow="true" />
						<Settings ShowGroupPanel="True" />
						<Settings ShowFilterRow="False" />
						<SettingsCustomizationWindow Enabled="True" />
						<SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />

						<%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
						--%>
					</dx:ASPxGridView>
				</td>
			</tr>
		</table>
		<!-- fim tabela -->        
	<!-- Menu Gerenciar Averbacaos Detalhes -->
	<div>
		<table border="0" cellpadding="0" cellspacing="5" width="100%">
			<tr>
				<td style="padding: 5px 0px;background:transparent url(/Imagens/BgMenuAverbacoes.png) repeat-x top left;">
<%--                    <div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonImprimirTelaEmbaixo" EnableDefaultAppearance="false"
							CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Imprimir Tela">
						</dx:ASPxButton>
					</div>
					<div class="float-divider" style="border: none;">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonImprimirTermoEmbaixo" EnableDefaultAppearance="false"
							CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Imprimir Termo">
						</dx:ASPxButton>
					</div>--%>
					<div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonSuspenderBloquearEmbaixo" CssClass="BotaoEstiloGlobal"
							runat="server" EnableDefaultAppearance="false" EnableTheming="false" Text="Suspender/Bloquear"
							OnClick="ASPxButtonSuspender_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonAtivarEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Ativar">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonLiquidarEmbaixo" OnClick="ASPxButtonLiquidar_Click" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Liquidar">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonAprovarEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Aprovar" OnClick="ASPxButtonAprovar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonDesaprovarEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Desaprovar" OnClick="ASPxButtonDesaprovar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonSolicitarSaldoDevedorEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Solicitar Saldo Devedor" OnClick="ASPxButtonInformarSaldoDevedor_Click">
						</dx:ASPxButton>
					</div>                    
					<div class="float-divider">
						<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonCancelarEmbaixo" runat="server" Text="Cancelar" CssClass="BotaoEstiloGlobal"
							EnableDefaultAppearance="false" EnableTheming="false" OnClick="ASPxButtonCancelar_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonInformarSaldoDevedorEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Informar Saldo Devedor" OnClick="ASPxButtonInformarSaldoDevedor_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider">
						<dx:ASPxButton ID="ASPxButtonInformarQuitacaoEmbaixo" runat="server" EnableDefaultAppearance="false"
							EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Informar Quitação" OnClick="ASPxButtonConfirmarQuitacao_Click">
						</dx:ASPxButton>
					</div>
					<div class="float-divider"  style="border: none;">
						<dx:ASPxButton ID="ASPxButtonConcluirCompraEmbaixo" runat="server" EnableDefaultAppearance="false" EnableTheming="false"
							CssClass="BotaoEstiloGlobal" Text="Concluir Compra">
						</dx:ASPxButton>
					</div>
				</td>
			</tr>
		</table>
	</div>
		<h1 class="TextoAncora">
			<a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
		</h1>
	</div>
</div>
