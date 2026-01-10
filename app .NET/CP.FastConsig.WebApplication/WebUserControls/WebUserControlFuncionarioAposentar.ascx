<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlFuncionarioAposentar.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFuncionarioAposentar" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<div>
<dx:ASPxGridView EnableTheming="True" ID="gridAposentar" ClientInstanceName="gridAposentar"
	EnableCallBacks="false" OnRowCommand="gridAposentar_RowCommand" runat="server"
	KeyFieldName="IDFuncionario" AutoGenerateColumns="False" Font-Size="11px" Width="100%"
	CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" >
	<Columns>
		<dx:GridViewDataColumn VisibleIndex="0">
			<DataItemTemplate>
				<asp:LinkButton runat="server" ID="select" CommandName="Select">
					<img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Selecionar para Importar" title="Selecionar para Importar"  />
				</asp:LinkButton>
			</DataItemTemplate>
		</dx:GridViewDataColumn>
		<dx:GridViewDataColumn FieldName="Data" VisibleIndex="0" />
		<dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="0" />
		<dx:GridViewDataColumn FieldName="Nome" VisibleIndex="1" />
		<dx:GridViewDataColumn FieldName="CPF" VisibleIndex="1" />
	</Columns>
	<ClientSideEvents SelectionChanged="function(s, e) {  gridAposentar.PerformCallback(); }" />
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

<asp:UpdatePanel runat="server" ID="upFunc" UpdateMode="Conditional">
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="gridAposentar" EventName="RowCommand" />
	</Triggers>
	<ContentTemplate>
        <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosFunc" Visible="false" Width="100%" HeaderText="Dados do Funcionário"
        HeaderStyle-Font-Bold="true" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaHum">
                            Matrícula:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Nome:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseHum">
                            CPF:
                        </td>
                        <td class="SemBordaBase">
                            <asp:Label runat="server" ID="LabelCpfFuncionario"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

		<dx:ASPxRoundPanel ID="panelFunc" HeaderStyle-Font-Bold="true" runat="server"
			Visible="false" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
			HeaderText="Averbações do Funcionário" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
			<ContentPaddings Padding="5px" />
			<HeaderStyle Font-Bold="True" />
			<PanelCollection>
				<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView EnableTheming="True" ID="gridAverbacoes" ClientInstanceName="gridAverbacoes"
	                EnableCallBacks="false" OnRowCommand="gridAverbacoes_RowCommand" runat="server"
	                KeyFieldName="IDFuncionarioAposenta" AutoGenerateColumns="False" Font-Size="11px" Width="100%"
	                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" >
	                <Columns>
		                <dx:GridViewDataColumn VisibleIndex="0">
			                <DataItemTemplate>
				                <asp:LinkButton runat="server" ID="select" CommandName="Select">
					                <img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Selecionar para Importar" title="Selecionar para Importar"  />
				                </asp:LinkButton>
			                </DataItemTemplate>
		                </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
                        <dx:GridViewDataColumn FieldName="Identificador" VisibleIndex="1" Caption="Identificador" />
		                <dx:GridViewDataColumn FieldName="NomeConsignataria" VisibleIndex="1" Caption="Consignatária" />
                        <dx:GridViewDataColumn FieldName="Verba" VisibleIndex="1" Caption="Verba" />
		                <dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="2" Caption="Parcela" />
		                <dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="3" Caption="Prazo" />
		                <dx:GridViewDataColumn FieldName="CompetenciaInicial" VisibleIndex="3" Caption="Início" />
	                </Columns>
	                <TotalSummary>
		                <dx:ASPxSummaryItem FieldName="ValorParcela" SummaryType="Sum" ShowInColumn="ValorParcela"
			                DisplayFormat="{0:n2}" />
	                </TotalSummary>
	                <ClientSideEvents SelectionChanged="function(s, e) {  gridAverbacoes.PerformCallback(); }" />
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


        <dx:ASPxRoundPanel runat="server" ID="panelAverbacao" Visible="false" Width="100%" HeaderText="Dados da Averbação"
            HeaderStyle-Font-Bold="true" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <ContentPaddings Padding="14px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="TituloNegrito" clientidmode="Static" id="Td1">
                                Número:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="LabelNumero"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                Consignatária:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="LabelConsignataria"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="cmbConsignataria" DataTextField="Fantasia" DataValueField="IDEmpresa" AutoPostBack="true" OnSelectedIndexChanged="SelecionouConsignataria"></asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td class="TituloNegrito" clientidmode="Static" id="Td2">
                                Verba:
                            </td>
                            <td class="SemBordaBase">
                                <asp:Label runat="server" ID="LabelVerba"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="cmbProduto" DataTextField="Nome" DataValueField="IDProduto"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito" clientidmode="Static" id="Td3">
                                Valor Parcela:
                            </td>
                            <td class="SemBordaBase">
                                <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito" clientidmode="Static" id="Td4">
                                Prazo:
                            </td>
                            <td class="SemBordaBase">
                                <asp:Label runat="server" ID="LabelPrazo"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvar" OnClick="ButtonSalvarClick"
                Text="Importar" runat="server" Visible="false" />&nbsp;&nbsp;<asp:Button ID="ButtonCancelar" CssClass="BotaoEstiloGlobal"
                    Text="Voltar" runat="server" OnClick="ButtonCancelarClick" Visible="false" />
	</ContentTemplate>

</asp:UpdatePanel>
</div>