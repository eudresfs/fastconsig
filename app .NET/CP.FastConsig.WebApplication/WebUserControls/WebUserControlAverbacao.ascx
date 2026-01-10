<%@ Control ClassName="WebUserControlAverbacao" Language="C#" AutoEventWireup="true"
	ViewStateMode="Enabled" EnableViewState="true" CodeBehind="WebUserControlAverbacao.ascx.cs"
	Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAverbacao" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="WebUserControlChartAreaEnviadosDescontados.ascx" TagName="WebUserControlChartAreaEnviadosDescontados"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<div class="GlobalUserControl">
	<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
		CssPostfix="Aqua" ShowHeader="True" HeaderStyle-Font-Bold="true" HeaderText="Exportação"
		GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
		<ContentPaddings PaddingLeft="0px" PaddingTop="5px" PaddingRight="0px" />
		<HeaderStyle Font-Bold="True"></HeaderStyle>
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<div class="BoxMatriculaProdutos">
					<center>
						<asp:UpdatePanel runat="server" ID="upInicio" UpdateMode="Conditional">
							<ContentTemplate>
								<table class="WebUserControlTabelaDadosFuncionariosAverbacoes" width="580px" border="0"
									cellspacing="0">
									<tr>
										<td class="TituloNegritoAverbacoes" style="width: 20%;">
											Matrícula ou CPF:
										</td>
										<td style="text-align: left;">
											<dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Paddings-Padding="0" Width="326px"
												Border-BorderColor="#c0dfe8" ID="dfMatriculaCPF" runat="server" AutoPostBack="true"
												ClientIDMode="Static" OnTextChanged="dfMatriculaCPF_TextChanged">
												<MaskSettings ErrorText="Preencha o campo com a Matricula ou CPF." />
												<Paddings Padding="0px">
                                                </Paddings>
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
												<Border BorderColor="#C0DFE8"></Border>
											</dx:ASPxTextBox>
										</td>
										<td class="TituloNegritoAverbacoes">
											Produto:
										</td>
										<td style="text-align: left;">
											<asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="170" runat="server" DataTextField="Nome"
												ClientIDMode="Static" DataValueField="IDProduto" ID="cmbProduto" AutoPostBack="true"
												OnSelectedIndexChanged="cmbProduto_Select" />
										</td>
										<td>
											<asp:Button CssClass="BotaoEstiloGlobal" ID="btLimpar" ClientIDMode="Static" Visible="false"
												Text="Limpar" runat="server" OnClick="Limpar_Click" />
										</td>
									</tr>
								</table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</center>
				</div>
				<div class="BoxDadosFuncionario" id="DadosFuncionario">
					<asp:UpdatePanel runat="server" ID="upDadosFunc" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="dfMatriculaCPF" EventName="TextChanged" />
						</Triggers>
						<ContentTemplate>
							<div id="panelDadosFunc" runat="server" visible="false">
								<table id="Table1" runat="server" width="100%" border="0" cellspacing="3" cellpadding="0"
									style="padding: 5px;">
									<tr>
										<td>
											<h1 id="TituloDadosFuncionario" runat="server" clientidmode="Static">
												Dados do Funcionário</h1>
										</td>
									</tr>
									<tr>
										<td style="background-color: #aecaf0;">
											<asp:HiddenField ID="txtEndereco" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtBairro" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtCidade" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtEstado" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtCep" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtEmail" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtTelefone" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtCelular" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtBanco" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtCNPJ" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtLocal" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtSetor" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="txtCargo" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="hfSenhaFunc" ClientIDMode="Static" runat="server" />
											<asp:HiddenField ID="hfValidaMargem" ClientIDMode="Static" runat="server" Value="N" />
											<table border="0" cellpadding="0" cellspacing="1" width="100%">
												<tr>
													<td style="width: 45%; background-color: #f3f9ff; padding: 3px;" valign="top">
														<table class="WebUserControlTabelaDadosFuncionariosAverbacoes" width="100%" border="0"
															cellspacing="0">
															<tr style="background-color:#8FD8D8;">
																<td rowspan="6" valign="top" style="width: 45px; border: none; background-color: #f3f9ff;">
																	<img src="/Imagens/Perfil.png" alt="" width="45px" height="45px" />
																</td>
																<td class="TituloNegrito" style="width: 20%;">
																	Matricula:
																</td>
																<td>
																	<asp:Label ID="txtMatricula" Style="font-weight: bold;" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Nome:
																</td>
																<td>
																	<asp:Label ID="txtNome" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	CPF:
																</td>
																<td>
																	<asp:Label ID="txtCPF" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	RG:
																</td>
																<td>
																	<asp:Label ID="txtRG" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Data Nasc:
																</td>
																<td>
																	<asp:Label ID="txtDataNasc" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
														</table>
													</td>
													<td style="width: 22%; background-color: #f3f9ff; padding: 3px;" valign="top">
														<table class="WebUserControlTabelaDadosFuncionariosAverbacoes" width="100%" border="0"
															cellspacing="0">
															<tr>
																<td style="width: 20%;">
																	Categoria:
																</td>
																<td>
																	<asp:Label ID="txtCategoria" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Regime:
																</td>
																<td>
																	<asp:Label ID="txtRegime" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Data Adm:
																</td>
																<td>
																	<asp:Label ID="txtDataAdm" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Situação:
																</td>
																<td>
																	<asp:Label ID="txtSituacaoFunc" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	Margem Disponível:
																</td>
																<td>
																	<asp:Label ID="txtMargemDisponivel" runat="server" ClientIDMode="Static"></asp:Label>
																</td>
															</tr>
														</table>
													</td>
													<td style="width: 33%; background-color: #f3f9ff; padding: 0px 6px;" valign="top">
														<asp:GridView ID="GridViewListaFunc" runat="server" CssClass="EstilosGridViewAverbacoesDadosFuncionario"
															DataKeyNames="IDFuncionario" Width="100%" AutoGenerateColumns="false" Visible="false"
															AllowSorting="false">
															<HeaderStyle CssClass="CabecalhoGridViewAverbacoesDadosFuncionario" />
															<RowStyle CssClass="LinhaListaGridViewAverbacoesDadosFuncionario" />
															<AlternatingRowStyle CssClass="LinhaAlternadaListaGridViewAverbacoesDadosFuncionario" />
															<PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridViewAverbacoesDadosFuncionario" />
															<Columns>
																<asp:TemplateField>
																	<ItemTemplate>
																		<asp:LinkButton runat="server" ID="EditarEmpresa" OnClick="GridViewListaFuncSelect_Click"
																			CommandArgument='<%# Eval("IDFuncionario") %>'>
																			<img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/BtnProcurar.png" /></asp:LinkButton>
																	</ItemTemplate>
																	<%--                                                                    <ItemTemplate>
																		<asp:ImageButton ToolTip="Visualizar" ID="ImageButtonSelecionar" runat="server" CommandName="select"
																			ImageUrl="~/imagens/BtnProcurar.png" />
																	</ItemTemplate>
																	--%>
																</asp:TemplateField>
																<asp:BoundField HeaderText="Escolha a Matricula" DataField="Matricula" />
																<asp:BoundField HeaderText="Margem Disponível" DataField="MargemDisponivel1" />
															</Columns>
														</asp:GridView>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<asp:UpdatePanel runat="server" ID="upEscolhaCompra" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="cmbProduto" EventName="SelectedIndexChanged" />
						</Triggers>
						<ContentTemplate>
							<div style="margin: 0px 8px 8px 8px; background-color: #f3f9ff; padding: 5px; border: 1px solid #aecaf0;"
								runat="server" id="divEscolhaCompra" visible="false">
								<span style="font-weight: bold;">Esta Matrícula já possui:</span>
								<table border="0" cellspacing="3" id="ChecksRenegociacaoCompra" visible="true" runat="server">
									<tr>
										<td>
											<div id="MenuAverbacao" runat="server" clientidmode="Static">
												<ul>
													<li>
														<asp:Label runat="server" ID="txtQtdeAverbacoesProprio"></asp:Label>
													</li>
													<li>
														<asp:Label runat="server" ID="txtQtdeAverbacoesTerceiros"></asp:Label>
													</li>
												</ul>
											</div>
										</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxCheckBox ID="cbRefinancia" Text="Marque aqui para Refinanciar" AutoPostBack="true"
												runat="server" OnCheckedChanged="cbRefinancia_CheckedChanged" />
										</td>
										<td style="padding: 5px;">
											<dx:ASPxCheckBox ID="cbCompra" Text="Marque aqui para Comprar" AutoPostBack="true"
												runat="server" OnCheckedChanged="cbCompra_CheckedChanged" />
										</td>
									</tr>
								</table>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div style="float: left; width: 49%;">
					<asp:UpdatePanel runat="server" ID="upRefinancia" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="cbRefinancia" EventName="CheckedChanged" />
						</Triggers>
						<ContentTemplate>
							<dx:ASPxRoundPanel ID="panelRefinancia" HeaderStyle-Font-Bold="true" runat="server"
								Visible="false" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
								HeaderText="Renegociação de Dívida" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
								<ContentPaddings Padding="5px" />
								<HeaderStyle Font-Bold="True" />
								<PanelCollection>
									<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
										<dx:ASPxGridView EnableTheming="True" ID="gridRefinanciar" ClientInstanceName="gridRefinanciar"
											EnableCallBacks="false" OnRowCommand="gridRefinanciar_RowCommand" runat="server"
											KeyFieldName="IDAverbacao" AutoGenerateColumns="False" Font-Size="11px" Width="100%"
											CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" OnSelectionChanged="gridRefinanciar_SelectionChanged">
											<Columns>
												<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Name="gridSelecionar">
													<HeaderTemplate>
														<dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ClientSideEvents-CheckedChanged="function(s, e) { gridRefinanciar.SelectAllRowsOnPage(s.GetChecked()); }" />
													</HeaderTemplate>
													<HeaderStyle HorizontalAlign="Center" />
												</dx:GridViewCommandColumn>
												<dx:GridViewDataColumn VisibleIndex="0">
													<DataItemTemplate>
														<asp:LinkButton runat="server" ID="select" CommandName="Select">
															<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Selecionar para Refinanciamento" title="Selecionar para Refinanciamento"  />
														</asp:LinkButton>
													</DataItemTemplate>
												</dx:GridViewDataColumn>
												<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="0" />
												<dx:GridViewDataColumn FieldName="Empresa1.Fantasia" VisibleIndex="1" Caption="Banco" />
												<dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="2" Caption="Parcela" />
												<dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="3" Caption="Prazo" />
												<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="3" Caption="Situação" />
											</Columns>
											<TotalSummary>
												<dx:ASPxSummaryItem FieldName="ValorParcela" SummaryType="Sum" ShowInColumn="ValorParcela"
													DisplayFormat="{0:n2}" />
											</TotalSummary>
											<ClientSideEvents SelectionChanged="function(s, e) {  gridRefinanciar.PerformCallback(); }" />
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
											<SettingsPager PageSize="5" />
											<Settings ShowFooter="true" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxRoundPanel>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div style="float: left; width: 50%; margin-left: 1%;">
					<asp:UpdatePanel runat="server" ID="upCompra" UpdateMode="Conditional">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="cbCompra" EventName="CheckedChanged" />
						</Triggers>
						<ContentTemplate>
							<dx:ASPxRoundPanel ID="panelCompra" HeaderStyle-Font-Bold="true" runat="server" Visible="false"
								Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
								HeaderText="Compra de Dívida" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
								<ContentPaddings Padding="5px" />
								<PanelCollection>
									<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
										<dx:ASPxGridView EnableTheming="True" ID="gridComprar" ClientInstanceName="gridComprar"
											EnableCallBacks="false" OnRowCommand="gridComprar_RowCommand" runat="server"
											KeyFieldName="IDAverbacao" AutoGenerateColumns="False" Font-Size="11px" Width="100%"
											CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" OnSelectionChanged="gridComprar_SelectionChanged">
											<Columns>
												<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Name="gridSelecionar">
													<HeaderTemplate>
														<dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ClientSideEvents-CheckedChanged="function(s, e) { gridComprar.SelectAllRowsOnPage(s.GetChecked()); }" />
													</HeaderTemplate>
													<HeaderStyle HorizontalAlign="Center" />
												</dx:GridViewCommandColumn>
												<dx:GridViewDataColumn VisibleIndex="0">
													<DataItemTemplate>
														<asp:LinkButton runat="server" ID="select" CommandName="Select">
															<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Selecionar para Compra" title="Selecionar para Compra"  />
														</asp:LinkButton>
													</DataItemTemplate>
												</dx:GridViewDataColumn>
												<dx:GridViewDataColumn FieldName="Numero" VisibleIndex="0" />
												<dx:GridViewDataColumn FieldName="Empresa1.Fantasia" VisibleIndex="1" Caption="Banco" />
												<dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="2" Caption="Parcela" />
												<dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="3" Caption="Prazo" />
												<dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="3" Caption="Situação" />
											</Columns>
											<TotalSummary>
												<dx:ASPxSummaryItem FieldName="ValorParcela" SummaryType="Sum" ShowInColumn="ValorParcela"
													DisplayFormat="{0:n2}" />
											</TotalSummary>
											<ClientSideEvents SelectionChanged="function(s, e) {  gridComprar.PerformCallback(); }" />
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
											<SettingsPager PageSize="5" />
											<Settings ShowFooter="true" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxRoundPanel>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div style="clear: both; overflow: hidden; width: 100%; height: 5px;">
					&nbsp;</div>
				<asp:UpdatePanel runat="server" ID="upDadosAverbacao" UpdateMode="Conditional">
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="cmbProduto" EventName="SelectedIndexChanged" />
					</Triggers>
					<ContentTemplate>
						<asp:HiddenField ID="hfAprovaFunc" ClientIDMode="Static" runat="server" />
						<div class="block" id="DivDadosAverbacao" runat="server" visible="false">
							<dx:ASPxRoundPanel DefaultButton="Salvar" ID="panelDados" HeaderStyle-Font-Bold="true" runat="server" Width="100%"
								CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" HeaderText="Dados da Averbação"
								GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
								<ContentPaddings Padding="5px" />
								<HeaderStyle Font-Bold="True"></HeaderStyle>
								<PanelCollection>
									<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
										<table width="100%" border="0" cellspacing="1" cellpadding="4" style="margin-top: 3px;">
											<!-- Linha que mostra/oculta a talela com as outras linhas -->
											<tr>
												<td style="padding: 0;">
													<table class="WebUserControlTabelaDadosAverbacoes" width="100%" border="0" cellspacing="0">
														<tr>
															<td class="TituloNegrito" style="width: 20%;">
																Margem Disponível para Operação:
															</td>
															<td>
																<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMargem">
																	<ContentTemplate>
																		<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="130" ID="txtMargemDisp" runat="server"
																			Enabled="false" ClientIDMode="Static">
																		</asp:TextBox>
																		<span class="EstiloCampoObrigatorio">*</span>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
													<div id="DivEmprestimo" runat="server" visible="false">
														<table class="WebUserControlTabelaDadosAverbacoes" width="100%" border="0" cellspacing="0">
															<tr>
																<td class="TituloNegrito" style="width: 20%;">
																	Identificador:
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="120" runat="server" ID="txtIdentificador"
																		ClientIDMode="Static"></asp:TextBox>&nbsp;(número do contrato ou proposta)
																</td>
															</tr>
															<tr runat="server" id="linhaPrazo">
																<td class="TituloNegrito">
																	Prazo (meses):
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="40" runat="server" ID="txtPrazo"
																		ClientIDMode="Static" onblur="javascript:calculamesfinal(); javascript:calculaconsig();"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr>
																<td class="TituloNegrito">
																	Valor Parcela:
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="130" runat="server" ID="txtValorParcela"
																		ClientIDMode="Static" onblur="javascript:calculaconsig(); javascript:calculafator();"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr runat="server" id="linhaValorContrato">
																<td class="TituloNegrito">
																	Valor Contrato:
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="130" runat="server" ID="txtValorContrato"
																		ClientIDMode="Static" onblur="javascript:calculafator();"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr runat="server" id="linhaValorConsig">
																<td class="TituloNegrito">
																	Valor Consignado:
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="130" runat="server" ID="txtValorConsignado"
																		ClientIDMode="Static" Enabled="false"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr runat="server" id="linhaFator">
																<td class="TituloNegrito">
																	Coeficiente:
																</td>
																<td style="text-align: left;">
																	<asp:TextBox MaxLength="8" CssClass="TextBoxDropDownEstilos" Width="130" runat="server"
																		ID="txtCoeficiente" ClientIDMode="Static"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr runat="server" id="linhaCET">
																<td class="TituloNegrito">
																	CET (%):
																</td>
																<td style="text-align: left;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="130" runat="server" ID="txtCET"
																		ClientIDMode="Static"></asp:TextBox>
																</td>
															</tr>
															<tr>
																<td class="TituloNegrito">
																	<asp:Label runat="server" ID="PrimMesDesconto" Text="1º Desconto Folha:" />
																</td>
																<td>
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="100" ID="txtMesInicio" runat="server"
																		ClientIDMode="Static" onblur="javascript:calculamesfinal();"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr runat="server" id="linhaUltDesconto">
																<td class="TituloNegrito">
																	Últ. Desconto Folha:
																</td>
																<td>
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="100" ID="txtMesFim" runat="server"
																		Enabled="false" ClientIDMode="Static"></asp:TextBox>
																	<span class="EstiloCampoObrigatorio">*</span>
																</td>
															</tr>
															<tr>
																<td class="TituloNegrito" style="border: none;">
																	Observações:
																</td>
																<td style="border: none;">
																	<asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="txtObs" ClientIDMode="Static"
																		Height="60px" TextMode="MultiLine" Rows="4"></asp:TextBox>
																</td>
															</tr>
															<tr>
																<td>
																	<span class="TituloCamposObrigatorios">*Campo Obrigatório</span>
																</td>
															</tr>
															<asp:HiddenField runat="server" ID="hfNumero" ></asp:HiddenField>
														</table>
													</div>
												</td>
											</tr>
										</table>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxRoundPanel>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
				<asp:UpdatePanel runat="server" ID="upSalvar" UpdateMode="Conditional">
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="cmbProduto" EventName="SelectedIndexChanged" />
					</Triggers>
					<ContentTemplate>
						<div style="clear: both; overflow: hidden; width: 100%; height: 2px;">
							&nbsp;</div>
						<div runat="server" id="DivSalvar" visible="false" style="text-align: left; width: 100%;
							padding-bottom: 5px; padding-top: 5px;">
							<asp:Button CssClass="BotaoEstiloGlobal" ID="Salvar" ClientIDMode="Static" OnClientClick="javascript:confirmacao(); return false;"
								Text="Salvar" runat="server" />&nbsp;&nbsp;
							<asp:Button ID="ButtonCancelar" CssClass="BotaoEstiloGlobal" Text="Desistir" runat="server"
								OnClick="Desistir_Click" />
							<h1 class="TextoAncora">
								<a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
									Topo da Página</a>
							</h1>
						</div>
						<div id="DivSalvarConcluido" title="Averbação Concluída" style="display: none;">
							<p>
								Número da Averbação:</p>
							<asp:Label ID="txtNumeroAverbacao" runat="server" ClientIDMode="Static"></asp:Label>
							<asp:HiddenField ID="txtData" ClientIDMode="Static" runat="server" />
							<asp:HiddenField ID="hfAverbacao" ClientIDMode="Static" runat="server" />
							<asp:HiddenField ID="hfCompra" ClientIDMode="Static" runat="server" />
							<asp:HiddenField ID="hfPrazoMaximo" ClientIDMode="Static" runat="server" />
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
	<div id="dlgconfirmacao" title="Confirmação da Averbação" style="display: none;">
		<table border="0" cellpadding="0" cellspacing="1" width="100%" style="background-color: #8FD8D8;">
			<tr>
				<td valign="top" style="border-bottom: none; width: 39%; padding: 2px; background-color: #ffffff;">
					<table class="WebUserControlTabelaConfirmaAverbacoes" border="0" cellpadding="0"
						cellspacing="0" width="100%">
						<tr>
							<td colspan="2" style="background-color: #add8e6;">
								<h1 class="TituloTabela">
									Dados Pessoais</h1>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Matrícula:
							</td>
							<td>
								<asp:TextBox CssClass="TextBoxSemBg" runat="server" ID="dfMat" Enabled="false" ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Nome:
							</td>
							<td>
								<asp:TextBox CssClass="TextBoxSemBg" runat="server" ID="dfNome" Enabled="false" ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								CPF:
							</td>
							<td>
								<asp:TextBox CssClass="TextBoxSemBg" runat="server" ID="dfCPF" Enabled="false" ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								RG:
							</td>
							<td>
								<asp:TextBox CssClass="TextBoxSemBg" runat="server" ID="dfRG" Enabled="false" ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Data Nasc.:
							</td>
							<td>
								<asp:TextBox CssClass="TextBoxSemBg" runat="server" Enabled="false" ID="dfDataNasc"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Endereço:
							</td>
							<td>
								<asp:TextBox Width="300" CssClass="TextBoxDropDownEstilos" runat="server" ID="dfEndereco"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Bairro:
							</td>
							<td>
								<asp:TextBox runat="server" Width="300" CssClass="TextBoxDropDownEstilos" ID="dfBairro"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Cidade:
							</td>
							<td>
								<asp:TextBox Width="300" CssClass="TextBoxDropDownEstilos" runat="server" ID="dfCidade"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								UF:
							</td>
							<td>
								<asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="150" runat="server" DataTextField="Nome"
									DataValueField="SiglaEstado" ID="cmbEstado" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Cep:
							</td>
							<td>
								<asp:TextBox Width="300" CssClass="TextBoxDropDownEstilos" runat="server" ID="dfCep"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Email:
							</td>
							<td>
								<asp:TextBox Width="300" CssClass="TextBoxDropDownEstilos" runat="server" ID="dfEmail"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Telefone:
							</td>
							<td>
								<asp:TextBox Width="300" CssClass="TextBoxDropDownEstilos" runat="server" ID="dfTelefone"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="border-bottom: none;">
								Celular:
							</td>
							<td style="border-bottom: none;">
								<asp:TextBox CssClass="TextBoxDropDownEstilos" Width="300" runat="server" ID="dfCelular"
									ClientIDMode="Static"></asp:TextBox>
							</td>
						</tr>
					</table>
				</td>
				<td valign="top" style="border-bottom: none; width: 60%; padding: 3px; background-color: #ffffff;">
					<table class="WebUserControlTabelaConfirmaAverbacoes" border="0" cellpadding="0"
						cellspacing="0" width="100%">
						<tr>
							<td colspan="2" style="background-color: #add8e6;">
								<h1 class="TituloTabela">
									Dados da Averbação</h1>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="width: 45%;">
								Produto:
							</td>
							<td class="TituloNegrito">
								<asp:Label runat="server" ID="txtCProduto" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Valor Contrato:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCValorContrato" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Prazo:
							</td>
							<td>
								<asp:Label runat="server" ID="txtCPrazo" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Valor Parcela:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCValorParcela" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Valor Consignado:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCValorConsig" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Obs.:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCObs" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								1ª Parcela:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCMesInicio" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Última Parcela:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="txtCMesFim" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito">
								Fator (coeficiente):
							</td>
							<td class="TituloNegrito">
								<asp:Label runat="server" ID="txtCFator" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="TituloNegrito" style="border-bottom: none;">
								CET (%):
							</td>
							<td style="border-bottom: none; text-align: left;">
								<asp:Label runat="server" ID="txtCCET" ClientIDMode="Static"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<div id="DivAprovaFunc" style="padding: 5px 0px;">
			<fieldset>
				<legend style="margin-bottom: 5px;">Requer Aprovação do Funcionário</legend>Senha:
				<asp:TextBox TextMode="Password" CssClass="TextBoxDropDownEstilos" runat="server"
					ID="dfSenhaFunc" ClientIDMode="Static" Width="120"></asp:TextBox>
				<br />
				<br />
				<asp:CheckBox runat="server" ClientIDMode="Static" ID="cbAprovaFunc" Text="Não quero aprovar agora. Enviar solitação para aprovação do funcionário." />
			</fieldset>
		</div>
	</div>
	<div id="dlgMargem" title="Atenção" style="display: none;">
		<p>
			Margem disponível insuficiente!</p>
	</div>
	<div id="dlgValorContrato" title="Atenção" style="display: none;">
		<p>
			Valor do contrato maior que o valor consignado!</p>
	</div>
	<div id="dlgPrazoMaximo" title="Atenção" style="display: none;">
		<p>
			Prazo limite foi excedido!</p>
	</div>
	<div id="DivTermo" class="printable" title="Termo de Averbação em Folha de Pagamento"
		style="display: none; overflow: scroll;">
		<div style="text-align: center;">
			<table border="0" cellpadding="0" cellspacing="0" width="99%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
				<tr>
					<td style="width: 160px; text-align: center;">
						<%--<img runat="server" id="ImgLogoBanco" src="~/Imagens/Logos/Bancos/BIG_BV.png" alt="" />--%>
					</td>
					<td>
						<h1 style="font-size: 10pt; font-weight: bold;">
							Termo de Averbação em Folha de Pagamento</h1>
					</td>
					<td style="width: 160px; text-align: center;">
						<%-- <img runat="server" id="ImgLogoConsignante" src="~/Imagens/LogoItaqua.png" alt="" />--%>
					</td>
				</tr>
			</table>
		</div>
		<table border="0" cellpadding="2" cellspacing="1" width="99%" class="TabelaTermoDeImpressao"
			style="border: 1px solid #000000; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
			font-size: 8.5pt;">
			<tr>
				<td>
					<table width="100%" border="0" cellpadding="0" cellspacing="0" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
						font-size: 8pt;">
						<tr>
							<td style="width: 15%; padding: 1px;">
								Número:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impNumero" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Data:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impData" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Consignatária:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impBanco" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								CNPJ:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCNPJ" ClientIDMode="Static" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
						font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
						cellspacing="0">
						<tr>
							<td colspan="2" style="border-bottom: 1px solid #000000;">
								<h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
									display: inline;">
									Dados Pessoais:</h1>
							</td>
						</tr>
						<tr>
							<td style="width: 15%; padding: 1px;">
								Nome:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impNome" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								CPF:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCPF" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								RG:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impRG" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Data Nasc.:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impDataNasc" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Endereço:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impEndereco" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Bairro:
							</td>
							<td>
								<asp:Label runat="server" ID="impBairro" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Cidade:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCidade" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								UF:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impUF" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Cep:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCep" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Email:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impEmail" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Telefone:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impTelefone" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Celular:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCelular" ClientIDMode="Static" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
						font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
						cellspacing="0">
						<tr>
							<td colspan="2" style="border-bottom: 1px solid #000000;">
								<h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
									display: inline;">
									Dados Funcionais:</h1>
							</td>
						</tr>
						<tr>
							<td style="padding: 1px; width: 15%;">
								Matrícula:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impMatricula" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Local:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impLocal" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Setor:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impSetor" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Cargo:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCargo" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Categoria:
							</td>
							<td style="padding: 1px;">
								<asp:Label ID="impCategoria" runat="server" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Regime:
							</td>
							<td style="padding: 1px;">
								<asp:Label ID="impRegime" runat="server" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Data Adm:
							</td>
							<td style="padding: 1px;">
								<asp:Label ID="impDataAdm" runat="server" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Situação:
							</td>
							<td style="padding: 1px;">
								<asp:Label ID="impSituacao" runat="server" ClientIDMode="Static" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
						font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
						cellspacing="0">
						<tr>
							<td colspan="2" style="border-bottom: 1px solid #000000;">
								<h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
									display: inline;">
									Dados da Averbação:</h1>
							</td>
						</tr>
						<tr>
							<td style="padding: 1px; width: 15%;">
								Produto:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impProduto" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Valor Contrato:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impValorContrato" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Prazo:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impPrazo" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Valor Parcela:
							</td>
							<td style="text-align: left;">
								<asp:Label runat="server" ID="impValorParcela" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Valor Consignado:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impValorConsignado" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Obs.:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impObs" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								1ª Parcela:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impMesInicio" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Última Parcela:
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impMesFim" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								Fator (coeficiente):
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impFator" ClientIDMode="Static" />
							</td>
						</tr>
						<tr>
							<td style="padding: 1px;">
								CET (%):
							</td>
							<td style="padding: 1px;">
								<asp:Label runat="server" ID="impCET" ClientIDMode="Static" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<div id="DivAverbacoes" style="padding: 5px 0px;">
		</div>
		<div class="Div" style="width: 99%;">
			<p style="text-align: justify; font-size: 7pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
				Eu
				<asp:Label runat="server" ID="impNome2" ClientIDMode="Static" />, acima especificado,
				autorizo a averbação e o desconto em folha de pagamento conforme instruído neste
				documento, atestando serem verídicos todos os dados aqui contidos, incluindo minhas
				informações sobre endereço e telefones, ciente de que posso responder, inclusive
				judicialmente, se questionado em futuro.
			</p>
			<br />
			<p style="text-align: justify; font-size: 9pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
				<asp:Label runat="server" ID="impDataAverbacao" ClientIDMode="Static" />
			</p>
			<br />
			<br />
			<p style="text-align: center; font-size: 9pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
				_____________________________________________________________<br />
				Assinatura do Funcionário
			</p>
		</div>
	</div>
</div>
