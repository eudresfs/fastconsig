<%@ Control ClassName="WebUserControlGerenciarAverbacao" Language="C#" AutoEventWireup="true"
	CodeBehind="WebUserControlGerenciarAverbacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlGerenciarAverbacao" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50731.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxyTeste" runat="server">
</asp:ScriptManagerProxy>
<div>
	<div id="DivBuscaAverbacao">
		<dx:ASPxRoundPanel HeaderStyle-Font-Bold="true" EnableTheming="true" ID="ASPxRoundPanelFiltro"
			HeaderText="Pesquisa de Averbações" runat="server" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
			CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" CssClass="RoundPanelEstilos"
			SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
			<ContentPaddings Padding="10px" />
			<HeaderStyle Font-Bold="True"></HeaderStyle>
			<PanelCollection>
				<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
					<div class="MargemTopoDiv">
						<asp:Panel DefaultButton="btnBuscar" ID="PanelPesquisarAverbacao" runat="server">
							<div>
								<table width="100%">
									<tr>
										<td style="text-align: right; width: 10%;">
											<asp:Label ID="Label1" runat="server" AssociatedControlID="TextBoxBusca" Font-Bold="true"
												ForeColor="#083772" Text="Buscar por:" />
										</td>
										<td style="text-align: left; width: 300px; padding-right: 3px;">
											<asp:TextBox class="TextBoxDropDownEstilos" Height="28" Width="99%" runat="server"
												ID="TextBoxBusca" />
											<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
												TargetControlID="TextBoxBusca" WatermarkText="Procurar por nome, CPF, matrícula, Averbação" />
										</td>
										<td style="width: 55px;">
											<dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
												EnableTheming="false" ID="btnBuscar" runat="server" Text="Buscar" OnClick="Buscar_Click">
											</dx:ASPxButton>
										</td>
										<td style="text-align: left;">
											<dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
												EnableTheming="false" ID="btnFiltros" runat="server" Text="Exibir Pesquisa Avançada"
												OnClick="Filtros_Click">
											</dx:ASPxButton>
										</td>
									</tr>
									<tr>
										<td style="text-align: right">
											<asp:Label ID="Label19" runat="server" AssociatedControlID="TextBoxBusca" Font-Bold="true"
												ForeColor="#083772" Text="Ou:" />
										</td>
										<td style="text-align: left; padding-top: 3px;">
											<asp:DropDownList DataTextField="Nome" Height="27px" CssClass="DropDownPesquisaAverbacao"
												DataValueField="IDEmpresaSolicitacaoTipo" runat="server" ID="DropDownListTiposSolicitacoes" />
										</td>
									</tr>
									<tr>
										<td style="text-align: right">
										</td>
										<td style="text-align: left; padding-top: 3px;">
											<asp:CheckBox runat="server" ID="CheckBoxBuscarEmMinhasSolicitacoes" Text="Buscar nas averbações que solicitei atendimento." />
										</td>
									</tr>
								</table>
							</div>
							<div runat="server" id="divFiltros" visible="false">
								<table class="WebUserControlTabela" cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td colspan="3" class="BordaBase">
											<h1 class="TituloTabela">
												Filtros para Dados de Averbação</h1>
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito" style="width: 20%;">
											<asp:Label ID="Label42" runat="server" AssociatedControlID="DateEditPeriodoInicio"
												Text="Periodo:" />
										</td>
										<td style="padding: 0;">
											<table border="0" cellpadding="0" cellspacing="0">
												<tr>
													<td class="UltimaCelulaSemBordaBase">
														<dx:ASPxDateEdit EnableDefaultAppearance="false" ID="DateEditPeriodoInicio" EditFormat="Custom"
															runat="server" ShowShadow="False" Width="100%" />
													</td>
													<td class="UltimaCelulaSemBordaBase">
														a
													</td>
													<td class="UltimaCelulaSemBordaBase">
														<dx:ASPxDateEdit ID="DateEditPeriodoFim" runat="server" ShowShadow="False" Width="100%" />
													</td>
													<td class="UltimaCelulaSemBordaBase">
														<asp:RadioButtonList CellPadding="0" CellSpacing="0" CssClass="EstilosRadioButtom"
															RepeatDirection="Horizontal" runat="server" ID="RadioButtonListFiltroTipoData">
															<asp:ListItem Selected="true" Text="Buscar pela data da averbação" Value="1"></asp:ListItem>
															<asp:ListItem Text="Buscar pela data da tramitação" Value="2"></asp:ListItem>
														</asp:RadioButtonList>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label18" runat="server" AssociatedControlID="ASPxTextAnoMes" Text="Mês/Ano da Parcela - Período:" />
										</td>
										<td>
											<dx:ASPxTextBox MaskSettings-ShowHints="False" ValidationSettings-RequiredField="false"
												ValidationSettings-ValidateOnLeave="False" ValidationSettings-EnableCustomValidation="False"
												ValidationSettings-CausesValidation="false" ValidationSettings-Display="None"
												CssClass="TextBoxDropDownEstilos" Height="30px" Width="75px" ID="ASPxTextAnoMes"
												MaskSettings-Mask="99/9999" EnableDefaultAppearance="True" EnableTheming="True"
												runat="server">
											</dx:ASPxTextBox>
											a
											<dx:ASPxTextBox MaskSettings-ShowHints="False" ValidationSettings-RequiredField="false"
												ValidationSettings-ValidateOnLeave="False" ValidationSettings-EnableCustomValidation="False"
												ValidationSettings-CausesValidation="false" ValidationSettings-Display="None"
												CssClass="TextBoxDropDownEstilos" Height="30px" Width="75px" ID="TextBoxAnoMesFinal" MaskSettings-Mask="99/9999"
												EnableDefaultAppearance="True" EnableTheming="True" runat="server">
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label2" runat="server" AssociatedControlID="DropDownListSituacao"
												Text="Situação:" />
										</td>
										<td>
											<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome"
												DataValueField="IDAverbacaoSituacao" runat="server" ID="DropDownListSituacao"
												CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label6" runat="server" AssociatedControlID="DropDownListConsignataria"
												Text="Consignataria:" />
										</td>
										<td>
											<asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="DropDownListConsignataria_SelectedIndexChanged"
												Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome" DataValueField="IDEmpresa"
												runat="server" ID="DropDownListConsignataria" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label3" runat="server" AssociatedControlID="DropDownListProdutoGrupo"
												Text="Grupo de Produto:" />
										</td>
										<td>
											<asp:UpdatePanel runat="server" ID="upProdutoGrupo" UpdateMode="Conditional">
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="DropDownListConsignataria" EventName="SelectedIndexChanged" />
												</Triggers>
												<ContentTemplate>
													<asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="DropDownListProdutoGrupo_SelectedIndexChanged"
														Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome" DataValueField="IDProdutoGrupo"
														runat="server" ID="DropDownListProdutoGrupo" CssClass="DropdownlistUserControl" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label4" runat="server" AssociatedControlID="DropDownListProduto" Text="Produto:" />
										</td>
										<td>
											<asp:UpdatePanel runat="server" ID="upProduto" UpdateMode="Conditional">
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="DropDownListConsignataria" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="DropDownListProdutoGrupo" EventName="SelectedIndexChanged" />
												</Triggers>
												<ContentTemplate>
													<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome"
														DataValueField="IDProduto" runat="server" ID="DropDownListProduto" CssClass="DropdownlistUserControl" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<%--									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label5" runat="server" AssociatedControlID="DropDownListSituacaoCompra"
												Text="Tipo de Averbação:" />
										</td>
										<td class="3">
											<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome"
												DataValueField="IDAverbacaoSituacao" runat="server" ID="DropDownListSituacaoCompra"
												CssClass="DropdownlistUserControl" />
										</td>
									</tr>--%>
									<%--<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label7" runat="server" AssociatedControlID="DropDownListDeduzMargem"
												Text="Deduz Margem:" />
										</td>
										<td>
											<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" runat="server" ID="DropDownListDeduzMargem"
												CssClass="DropdownlistUserControl">
												<asp:ListItem Text="-- Selecione --" Value="-1" />
												<asp:ListItem Text="Deduz Margem" Value="1" />
												<asp:ListItem Text="Não Deduz Margem" Value="2" />
											</asp:DropDownList>
										</td>
									</tr>--%>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label8" runat="server" AssociatedControlID="DropDownListAgente" Text="Correspondente:" />
										</td>
										<td>
											<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="DropDownListConsignataria" EventName="SelectedIndexChanged" />
												</Triggers>
												<ContentTemplate>
													<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome"
														DataValueField="IDEmpresa" runat="server" ID="DropDownListAgente" CssClass="DropdownlistUserControl" />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<%--<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label9" runat="server" AssociatedControlID="DropDownListAverbacaoTipo"
												Text="Tipo da Averbação:" />
										</td>
										<td>
											<asp:DropDownList Height="27px" class="TextBoxDropDownEstilos" DataTextField="Nome"
												DataValueField="IDAverbacaoTipo" runat="server" ID="DropDownListAverbacaoTipo"
												CssClass="DropdownlistUserControl" />
										</td>
									</tr>--%>
									<tr>
										<td class="TituloNegrito" style="border: none;">
											<asp:Label ID="Label10" runat="server" AssociatedControlID="TextBoxPrazo" Text="Prazo:" />
										</td>
										<td style="border: none;">
											<asp:TextBox class="TextBoxDropDownEstilos" Width="50px" runat="server" ID="TextBoxPrazo"
												Text="0" />
										</td>
									</tr>
								</table>
								<table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
									<tr>
										<td colspan="3" class="BordaBase">
											<h1 class="TituloTabela">
												Filtros para Dados Funcionais</h1>
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito" style="width: 20%;">
											<asp:Label ID="Label11" runat="server" AssociatedControlID="DropDownListLocal" Text="Local:" />
										</td>
										<td>
											<asp:DropDownList runat="server" ID="DropDownListLocal" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label12" runat="server" AssociatedControlID="DropDownListSetor" Text="Setor:" />
										</td>
										<td>
											<asp:DropDownList CssClass="DropdownlistUserControl" runat="server" ID="DropDownListSetor" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label13" runat="server" AssociatedControlID="DropDownListCargo" Text="Cargo:" />
										</td>
										<td>
											<asp:DropDownList runat="server" ID="DropDownListCargo" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label14" runat="server" AssociatedControlID="DropDownListRegime" Text="Regime:" />
										</td>
										<td>
											<asp:DropDownList runat="server" ID="DropDownListRegime" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label15" runat="server" AssociatedControlID="DropDownListCategoria"
												Text="Categoria:" />
										</td>
										<td class="TituloNegrito">
											<asp:DropDownList DataTextField="Nome" DataValueField="IDFuncionarioCategoria" runat="server"
												ID="DropDownListCategoria" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito">
											<asp:Label ID="Label20" runat="server" AssociatedControlID="DropDownListCategoria"
												Text="Situação Funcional:" />
										</td>
										<td class="TituloNegrito">
											<asp:DropDownList runat="server" ID="DropDownListSituacaoFuncional" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td class="TituloNegrito" style="border: none;">
											<asp:Label ID="Label16" runat="server" AssociatedControlID="DropDownListFuncionarioSituacao"
												Text="Situação Cadastral:" />
										</td>
										<td class="UltimaCelulaSemBordaBase">
											<asp:DropDownList DataTextField="Nome" DataValueField="IDFuncionarioSituacao" runat="server"
												ID="DropDownListFuncionarioSituacao" CssClass="DropdownlistUserControl" />
										</td>
									</tr>
									<tr>
										<td style="border: none;">
											<table class="TabelaWebUserControlGerenciarAverbacao" cellpadding="0" border="0"
												cellspacing="3">
												<tr>
													<td>
														<dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
															EnableTheming="false" ID="ASPxButton1" runat="server" Text="Buscar" OnClick="Buscar_Click">
														</dx:ASPxButton>
													</td>
													<td>
														<dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
															EnableTheming="false" ID="ASPxButtonLimparFiltros" runat="server" Text="Limpar"
															OnClick="ASPxButtonLimparFiltros_Click">
														</dx:ASPxButton>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
								<h1 class="TextoAncora">
									<a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
										Topo da Página</a>
								</h1>
							</div>
						</asp:Panel>
					</div>
				</dx:PanelContent>
			</PanelCollection>
		</dx:ASPxRoundPanel>
	</div>
	<table cellpadding="0" border="0" cellspacing="0">
		<tr>
			<td>
				<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonAprovar" Width="90" OnClick="ASPxButtonAprovar_Click"
					EnableDefaultAppearance="false" CssClass="BotaoEstiloGlobal" Visible="false"
					EnableTheming="false" runat="server" Text="Aprovar">
				</dx:ASPxButton>
			</td>
			<td>
				<dx:ASPxButton Cursor="Pointer" ID="ASPxButtonDesaprovar" Width="90" OnClick="ASPxButtonDesaprovar_Click"
					EnableDefaultAppearance="false" CssClass="BotaoEstiloGlobal" Visible="false"
					EnableTheming="false" runat="server" Text="Desaprovar">
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
	<div id="DivResultado" class="DivResultadoEstilos" runat="server" visible="false">
		<%--        <asp:Button ID="ButtonExportarPDF" OnClick="ButtonExportarPDF_Click"
					CssClass="BotaoEstiloGlobal" runat="server" Text="Exportar PDF">
		</asp:Button>
		<asp:Button ID="ButtonExportarExcel" OnClick="ButtonExportarExcel_Click"
					CssClass="BotaoEstiloGlobal" runat="server" Text="Exportar Excel">
		</asp:Button>
		<asp:Button ID="ButtonExportarTXT" OnClick="ButtonExportarTXT_Click"
					CssClass="BotaoEstiloGlobal" runat="server" Text="Exportar TXT">
		</asp:Button>--%>
		<div>
			<br />
			<asp:Label ID="LabelFiltroDescricao" runat="server"></asp:Label>
			<br />
			<br />
			<asp:Button ID="ButtonFiltro" OnClick="ButtonFiltro_Click" CssClass="BotaoEstiloGlobal"
				runat="server" Text="Mudar Filtro"></asp:Button>
			<br />
			<br />
		</div>
		<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" GridViewID="gridAverbacaos" runat="server">
		</dx:ASPxGridViewExporter>
                                <table border="0" cellspacing="0" cellpadding="0">
<tr>
                                <td style="padding: 5px 5px 5px 0px; width: 12%;">
                                    Exportar para:
                                </td>
                                <td style="padding: 5px 0px; width: 62px; text-align: center;">
                                    <dx:ASPxComboBox EnableDefaultAppearance="false" EnableTheming="false" ItemStyle-Paddings-Padding="2px"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" CssClass="TextBoxDropDownEstilos"
                                        ID="cmbTipoExportacao" runat="server" Style="vertical-align: middle" SelectedIndex="0"
                                        ValueType="System.String" Width="75px">
                                        <Items>
                                            <dx:ListEditItem Text="Pdf" Value="0" />
                                            <dx:ListEditItem Text="Excel" Value="1" />
                                            <dx:ListEditItem Text="Rtf" Value="2" />
                                            <dx:ListEditItem Text="Texto/CSV" Value="3" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                                <td style="text-align: left; padding-left: 5px;">
                                    <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="DownloadFile" Text="Exportar"
                                        ClientIDMode="Static" OnClick="buttonSaveAs_Click" />

                                </td>
                            </tr>
                        </table>
		<dx:ASPxGridView SettingsLoadingPanel-Text="Carregando..." OnPageIndexChanged="gridAverbacaos_PageIndexChanged"
			OnBeforeColumnSortingGrouping="gridAverbacaos_BeforeColumnSortingGrouping" SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
			SettingsText-EmptyDataRow="Sem dados para exibição!" EnableTheming="True" ID="gridAverbacaos"
			ClientInstanceName="gridAverbacaos" OnRowCommand="gridAverbacaos_RowCommand"
			OnHtmlRowCreated="gridAverbacoes_HtmlRowCreated" runat="server" KeyFieldName="IDAverbacao"
			AutoGenerateColumns="False" Font-Size="11px" Width="100%" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
			CssPostfix="Aqua">
			<%--                <ClientSideEvents SelectionChanged="grid_SelectionChanged" />--%>
			<%--            <ClientSideEvents SelectionChanged="function(s, e) { e.processOnServer = true; }" />
			--%>
			<%--            <SettingsBehavior ProcessSelectionChangedOnServer="true" />--%>
			<%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
			--%>
			<Settings ShowGroupPanel="True" UseFixedTableLayout="true" />
			<Columns>
				<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Name="gridSelecionar"
					Visible="false">
					<HeaderTemplate>
						<dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ClientSideEvents-CheckedChanged="function(s, e) { gridAverbacaos.SelectAllRowsOnPage(s.GetChecked()); }" />
					</HeaderTemplate>
					<HeaderStyle HorizontalAlign="Center" />
					<ClearFilterButton Visible="True" Text="Limpar">
					</ClearFilterButton>
				</dx:GridViewCommandColumn>
				<dx:GridViewDataColumn VisibleIndex="0" Visible="false" Width="25px">
					<DataItemTemplate>
						<asp:LinkButton runat="server" ID="select" CommandName="Edit">
							<img style="padding-top: 4px;" src="/Imagens/Tick.png" width="16" height="16" runat="server"
								id="imgAcao" alt="Ação" title="Ação" />
						</asp:LinkButton>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn VisibleIndex="1" Width="25px">
					<DataItemTemplate>
						<asp:LinkButton runat="server" ID="select" CommandName="Select">
							<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Detalhes da Averbação" title="Detalhes da Averbação"  />
						</asp:LinkButton>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<%--                <dx:GridViewCommandColumn  ButtonType="Image" >                    
					<SelectButton Visible="true" Text="Selecionar" >
						<Image ToolTip="Selecionar" Url="~/imagens/gtk-edit.png"></Image>
					</SelectButton>
				</dx:GridViewCommandColumn>--%>
				<dx:GridViewDataTextColumn FieldName="Funcionario.Pessoa.CPF" Caption="CPF" Width="100px"
					CellStyle-HorizontalAlign="Right">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelCpf"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataColumn FieldName="Funcionario.Matricula" Caption="Matrícula" Width="70px">
					<EditCellStyle>
						<Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" />
					</EditCellStyle>
					<HeaderStyle>
						<Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px"
							PaddingTop="0px" />
					</HeaderStyle>
					<CellStyle Font-Size="10pt">
						<Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px"
							PaddingTop="0px" />
					</CellStyle>
					<FooterCellStyle>
						<Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px"
							PaddingTop="0px" />
					</FooterCellStyle>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn FieldName="Funcionario.Pessoa.Nome" Caption="Nome" Width="250px"
					CellStyle-Wrap="False" />
				<dx:GridViewDataColumn FieldName="Numero" Caption="Número" Visible="true" />
				<dx:GridViewDataColumn FieldName="Data" Visible="false" />
				<dx:GridViewDataColumn FieldName="Empresa1.Nome" Caption="Consignatária" Visible="true" />
				<dx:GridViewDataColumn FieldName="Produto.ProdutoGrupo.Nome" Caption="Produto" Visible="false" />
				<dx:GridViewDataColumn FieldName="ValorParcela" Caption="Valor Parcela" />
				<dx:GridViewDataColumn FieldName="Prazo" Visible="false" />
				<dx:GridViewDataColumn Caption="Prz.Restante" FieldName="PrazoRestante" Visible="false">
					<DataItemTemplate>
						<div style="text-align: right;">
							<asp:Label runat="server" ID="LabelPrazoRestante"></asp:Label>
						</div>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" Caption="Situação" Visible="false" />
				<dx:GridViewDataColumn FieldName="CompetenciaInicial" Caption="Mês Inicial" Visible="false" />
				<dx:GridViewDataColumn FieldName="CompetenciaFinal" Caption="Mês Final" Visible="false" />
				<dx:GridViewDataColumn Caption="Expiração" FieldName="Expiracao" Visible="false"
					Width="120px">
					<DataItemTemplate>
						<div style="text-align: center">
							<asp:Image runat="server" ID="ImagemExpiracao"></asp:Image>
						</div>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Saldo Bruto" FieldName="SaldoBruto" Visible="false">
					<DataItemTemplate>
						<div style="text-align: right;">
							<asp:Label runat="server" ID="LabelSaldoBruto"></asp:Label>
						</div>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Saldo Devedor" FieldName="SaldoDevedor" Visible="false">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelSaldoDevedor"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Forma" FieldName="FormaPagamento" Visible="false">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelFormaPagamento"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Informado Por" FieldName="InformadoPor" Visible="false">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelInformadoPor"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Solicitante" FieldName="Solicitante" Visible="false">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelSolicitante"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn Caption="Obs" FieldName="Obs" Visible="false">
					<DataItemTemplate>
						<asp:Label runat="server" ID="LabelObs"></asp:Label>
					</DataItemTemplate>
				</dx:GridViewDataColumn>
			</Columns>
			<GroupSummary>
				<dx:ASPxSummaryItem FieldName="Numero" SummaryType="Count" ShowInGroupFooterColumn="Numero"
					ValueDisplayFormat="#,0" DisplayFormat="{0:n0}" />
				<dx:ASPxSummaryItem FieldName="ValorParcela" SummaryType="Sum" ShowInGroupFooterColumn="ValorParcela"
					DisplayFormat="{0:n2}" />
			</GroupSummary>
			<TotalSummary>
				<dx:ASPxSummaryItem FieldName="Numero" SummaryType="Count" ShowInColumn="Numero"
					ValueDisplayFormat="#,0" DisplayFormat="{0:n0}" />
				<dx:ASPxSummaryItem FieldName="ValorParcela" SummaryType="Sum" ShowInColumn="ValorParcela"
					DisplayFormat="{0:n2}" />
			</TotalSummary>
			<Settings ShowFilterRow="True" ShowFooter="true" />
			<SettingsCustomizationWindow Enabled="True" />
			<SettingsLoadingPanel ImagePosition="Top" />
			<SettingsDetail ShowDetailRow="true" />
			<SettingsBehavior ColumnResizeMode="Control" />
			<Images SpriteCssFilePath="~/App_Themes/Aqua/GridView/sprite.css">
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
			<Styles CssFilePath="~/App_Themes/Aqua/GridView/styles.css" CssPostfix="Aqua">
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
					<div style="padding: 0">
						<dx:ASPxPageControl CssPostfix="Aqua" EnableTheming="True" EnableDefaultAppearance="true"
							CssFilePath="~/Estilos/DevExpress/TabControl/styles.css" runat="server" ID="pageControl"
							Width="100%" EnableCallBacks="true">
							<TabPages>
								<dx:TabPage Text="Parcelas" Visible="true">
									<ContentCollection>
										<dx:ContentControl runat="server" ID="ContentControl2">
											<dx:ASPxGridView SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
												SettingsText-EmptyDataRow="Sem dados para exibição!" CssPostfix="Aqua" EnableTheming="true"
												ID="ASPxGridViewParcelas" runat="server" KeyFieldName="IDAverbacaoParcela" Width="100%"
												CssFilePath="~/Estilos/DevExpress/GridView/styles.css" OnDataBinding="gridParcelas_DataSelect">
												<Columns>
													<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
													<dx:GridViewDataColumn FieldName="Competencia" VisibleIndex="2" Caption="Mês/Ano" />
													<dx:GridViewDataColumn FieldName="Valor" VisibleIndex="3" Caption="Valor" />
													<dx:GridViewDataColumn FieldName="AverbacaoParcelaSituacao.Nome" VisibleIndex="4"
														Caption="Situação" />
													<dx:GridViewDataColumn FieldName="ValorDescontado" VisibleIndex="5" Caption="Descontado" />
													<dx:GridViewDataColumn FieldName="Observacao" VisibleIndex="5" Caption="Obs." />
												</Columns>
												<Settings ShowFooter="True" />
											</dx:ASPxGridView>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Tramitações" Visible="true">
									<ContentCollection>
										<dx:ContentControl runat="server" ID="ContentControl3">
											<dx:ASPxGridView SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
												SettingsText-EmptyDataRow="Sem dados para exibição!" EnableTheming="true" CssPostfix="Aqua"
												CssFilePath="~/Estilos/DevExpress/GridView/styles.css" ID="ASPxGridViewTramitacoes"
												runat="server" KeyFieldName="IDAverbacaoTramitacao" Width="100%" OnDataBinding="gridTramitacoes_DataSelect">
												<Columns>
													<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="4" Caption="Situação" />
													<dx:GridViewDataColumn FieldName="OBS" VisibleIndex="5" Caption="Obs." />
													<dx:GridViewDataColumn FieldName="CreatedOn" VisibleIndex="5" Caption="Data" />
													<dx:GridViewDataColumn FieldName="Usuario.Nome" VisibleIndex="5" Caption="Usuário" />
												</Columns>
												<Settings ShowFooter="True" />
											</dx:ASPxGridView>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Averbações Vinculadas (Compra/Renegociação)" Visible="true">
									<ContentCollection>
										<dx:ContentControl runat="server" ID="ContentControl1">
											<dx:ASPxGridView SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
												SettingsText-EmptyDataRow="Sem dados para exibição!" EnableTheming="True" CssPostfix="Aqua"
												ID="gridVinculos" CssFilePath="~/Estilos/DevExpress/GridView/styles.css" runat="server"
												KeyFieldName="IDAverbacao" Width="100%" OnDataBinding="gridVinculos_DataSelect"
												OnRowCommand="gridVinculos_RowCommand">
												<Columns>
													<dx:GridViewDataColumn VisibleIndex="1" Width="25px">
														<DataItemTemplate>
															<asp:LinkButton runat="server" ID="select" CommandName="Select">
																<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Detalhes da Averbação" title="Detalhes da Averbação"  />
															</asp:LinkButton>
														</DataItemTemplate>
													</dx:GridViewDataColumn>
													<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
													<dx:GridViewDataColumn FieldName="Data" VisibleIndex="2" />
													<dx:GridViewDataColumn FieldName="Produto.ProdutoGrupo.Nome" VisibleIndex="4" Caption="Produto" />
													<dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="5" Caption="Valor Parcela" />
													<dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="5" />
													<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="5" Caption="Situação" />
													<dx:GridViewDataColumn FieldName="UltimaSolicitacao" VisibleIndex="6" Caption="Últ. Solicitação" />
												</Columns>
												<Settings ShowFooter="True" />
											</dx:ASPxGridView>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
							</TabPages>
						</dx:ASPxPageControl>
					</div>
				</DetailRow>
			</Templates>
		</dx:ASPxGridView>
		<h1 class="TextoAncora">
			<a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
				Topo da Página</a>
		</h1>
		<a name="pageBottom"></a>
	</div>
</div>
