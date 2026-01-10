<%@ Control ClassName="WebUserControlCoeficientesEmprestimo" Language="C#" AutoEventWireup="true"
	CodeBehind="WebUserControlCoeficientesEmprestimo.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlCoeficientesEmprestimo" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxUploadControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div>
	<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderStyle-Font-Bold="true"
		Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
		HeaderText="Importação" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
		<ContentPaddings Padding="0px" />
		<PanelCollection>
			<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
				<table class="WebUserControlTabelaCoeficienteEmprestimo" border="0" cellpadding="2"
					cellspacing="4" width="100%">
					<tr>
						<td  ClientIDMode="Static" id="TamanhoCelulaHum" class="TituloNegrito">
							Início da vigência:
						</td>
						<td>
							<dx:ASPxDateEdit AllowUserInput="false" AllowNull="false" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
								CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ID="ASPxDateEditInicioVigencia"
								runat="server">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td class="TituloNegrito">
							Carência (dias):
						</td>
						<td>
							<dx:ASPxSpinEdit SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Paddings-Padding="0px"
								CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ID="ASPxSpinEditCarencia"
								AllowNull="false" MaxLength="3" runat="server" Number="0" NumberType="Integer"
								MinValue="0" MaxValue="999">
							</dx:ASPxSpinEdit>
						</td>
					</tr>
					<tr>
						<td class="TituloNegrito" ClientIDMode="Static"  id="SemBordaBaseHum">
							Validade (dias):
						</td>
						<td class="SemBordaBase">
							<dx:ASPxSpinEdit SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Paddings-Padding="0px"
								CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ID="ASPxSpinEditValidade"
								AllowNull="false" MaxLength="3" runat="server" Number="0" NumberType="Integer"
								MinValue="0" MaxValue="999">
							</dx:ASPxSpinEdit>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<asp:Button class="BotaoEstiloGlobal" ID="ButtonSalvarCoeficiente" OnClick="ButtonSalvarCoeficiente_Click"
								runat="server" Text="Criar" />
						</td>
					</tr>
					<tr id="TrUploadArquivoCoeficiente" runat="server" visible="false">
						<td class="TituloNegrito">
							Arquivo:
						</td>
						<td>
							<table class="WebUserControTabelaUpload" width="40%" border="0" cellpadding="0" cellspacing="4">
								<tr>
									<td>
										Exemplo:<br />
										<br />
										<asp:Image runat="server" ID="ImageExemploCoeficiente" ImageUrl="~/Arquivos/ExemploCoeficiente.png" /><br />
										<br />
										<a runat="server" id="LinkArquivo" href="~/Arquivos/ExemploCoeficiente.xlsx">Donwnload
											do arquivo de exemplo</a><br />
										<br />
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxUploadControl BrowseButtonStyle-CssClass="BotaoEstiloGlobal" Height="29"
											BrowseButtonStyle-Border-BorderColor="#a3c0e8" BrowseButtonStyle-Border-BorderStyle="Solid"
											BrowseButtonStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderStyle="Solid"
											TextBoxStyle-BackColor="#ffffff" TextBoxStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderColor="#a3c0e8"
											EnableDefaultAppearance="false" EnableTheming="False" BrowseButtonStyle-Font-Size="11px"
											FileUploadMode="OnPageLoad" ID="ASPxUploadControlArquivoCoeficiente" runat="server"
											ClientInstanceName="uploader" ShowProgressPanel="false" OnFileUploadComplete="ASPxUploadControlArquivoCoeficiente_FileUploadComplete">
											<ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
												FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
												TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
											<ValidationSettings MaxFileSize="102400000" AllowedFileExtensions=".xls,.csv,.txt,.xlsx"
												GeneralErrorText="Escolha um arquivo nos formatos: xls, ou xlsx." NotAllowedFileExtensionErrorText="Escolha um arquivo nos formatos: xls, ou xlsx.">
											</ValidationSettings>
										</dx:ASPxUploadControl>
									</td>
									<td style="text-align: left;">
										<dx:ASPxButton ID="ASPxButtonUploadArquivoCoeficiente" EnableDefaultAppearance="false"
											EnableTheming="false" CssClass="BotaoEstiloGlobal" runat="server" AutoPostBack="False"
											Text="Enviar" ClientInstanceName="btnUpload" ClientEnabled="False">
											<ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Button class="BotaoEstiloGlobal" ID="ButtonProcessarArquivoCoeficiente" OnClick="ButtonProcessarArquivoCoeficiente_Click"
											runat="server" Text="Processar" />
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
</div>
<div>
	&nbsp;</div>
<div>
	<dx:ASPxRoundPanel ID="ASPxRoundPanelCoeficientesCadastrados" runat="server" HeaderStyle-Font-Bold="true"
		Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
		HeaderText="Coeficientes Cadastrados" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
		<ContentPaddings Padding="0px" />
		<PanelCollection>
			<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
				<asp:GridView OnRowDataBound="GridViewCoeficientes_RowDataBound" ShowFooter="false" CssClass="EstilosGridView" EmptyDataText="Sem coeficientes para exibição!"
					DataKeyNames="IDEmpresaCoeficiente" ID="GridViewCoeficientes" runat="server"
					Width="100%" AutoGenerateColumns="false">
					<HeaderStyle CssClass="CabecalhoGridView" />
					<RowStyle CssClass="LinhaListaGridView" />
					<AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
					<PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
					<Columns>
						<asp:TemplateField HeaderText="">
							<ItemTemplate>
								<asp:ImageButton runat="server" ID="ImageButtonVer" AlternateText="Ver" ImageUrl="~/Imagens/BtnProcurar.png"
									CommandArgument='<%# Eval("IDEmpresaCoeficiente") %>' OnClick="ImageButtonVer_Click">
								</asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="">
							<ItemTemplate>
								<asp:ImageButton runat="server" ID="ImageButtonExportar" AlternateText="Ver" ImageUrl="~/Imagens/BtnExportar.png">
								</asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="">
							<ItemTemplate>
								<asp:ImageButton OnClientClick="return confirm('Tem certeza de que deseja remover o item?');"
									runat="server" ID="ImageButtonRemover" AlternateText="Remover" ImageUrl="~/Imagens/trash_16x16.gif"
									CommandArgument='<%# Eval("IDEmpresaCoeficiente") %>' OnClick="ImageButtonRemover_Click">
								</asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField HeaderText="Data do Cadastro" DataField="Data" DataFormatString="{0:dd/MM/yyyy}" />
						<asp:BoundField HeaderText="Início da Vigência" DataField="InicioVigencia" DataFormatString="{0:dd/MM/yyyy}" />
						<asp:BoundField HeaderText="Carência" DataField="Carencia" />
						<asp:BoundField HeaderText="Validade" DataField="ValidadeDias" />
					</Columns>
				</asp:GridView>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
	<dx:ASPxPopupControl Width="400px" BackColor="#e7f6fa" ID="PopupControlDetalhesCoeficiente"
		runat="server" CloseAction="CloseButton" Modal="True" CloseButtonImage-Url="~/Imagens/CloseBtnPopUp.png"
		PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupControlAviso"
		HeaderText="Detalhes" Font-Bold="true" AllowDragging="True" EnableAnimation="True"
		EnableViewState="True">
		<HeaderStyle CssClass="AlturaCabecalhoPopControl" />
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControlDetalhesCoeficiente"
				runat="server">
				<div style="vertical-align: middle">
					<asp:GridView DataKeyNames="IDEmpresaCoeficienteDetalhe" EmptyDataText="Sem detalhes para exibição!"
						runat="server" ID="GridViewDetalhesCoeficiente" CssClass="EstilosGridViewDashBoardConsiliacao"
						Width="100%" Height="100%" AutoGenerateColumns="false">
						<HeaderStyle Height="16px" CssClass="CabecalhoGridView" />
						<RowStyle Height="16px" CssClass="LinhaListaGridView" />
						<AlternatingRowStyle Height="17px" CssClass="LinhaAlternadaListaGridView" />
						<PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
						<Columns>
							<asp:BoundField HeaderText="Dia" DataField="Dia" />
							<asp:BoundField HeaderText="Prazo" DataField="Prazo" />
							<asp:BoundField HeaderText="Fator / Coeficiente" DataField="Coeficiente" />
							<asp:BoundField HeaderText="CET" DataField="CET" />
						</Columns>
					</asp:GridView>
				</div>
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
</div>
