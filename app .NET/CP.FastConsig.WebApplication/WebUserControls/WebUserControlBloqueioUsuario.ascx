<%@ Control ClassName="WebUserControlBloqueioUsuario" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlBloqueioUsuario.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlBloqueioUsuario" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            <b>Matrícula:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <b>Nome:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="border: none;">
                            <b>CPF:</b>
                        </td>
                        <td style="border: none;">
                            <asp:Label runat="server" ID="LabelCpfFuncionario"></asp:Label>
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
    <asp:Button CssClass="BotaoEstiloGlobal" Width="80" runat="server" ID="ButtonNovo"
        Text="Novo" OnClick="ButtonNovo_Click" />
    &nbsp;
    <asp:Button CssClass="BotaoEstiloGlobal" Width="120" runat="server" ID="ButtonRemoverTodos"
        OnClientClick="return confirm('Tem certeza de que deseja remover todos os itens?')"
        Text="Remover Todos" OnClick="ButtonRemoverTodos_Click" />
</div>
<div>
    &nbsp;</div>
<div runat="server" id="DivGridConfiguracao">
    <dx:ASPxGridView SettingsLoadingPanel-Text="Carregando..." SettingsText-EmptyDataRow="Sem itens para exibição!"
        ID="ASPxGridViewBloqueio" ClientInstanceName="aSPxGridViewBloqueio" runat="server"
        KeyFieldName="IDFuncionarioBloqueio" Width="100%" AutoGenerateColumns="False"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
        <Columns>
            <dx:GridViewDataDateColumn PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy hh:mm:ss"
                Caption="Data do Bloqueio" FieldName="DataBloqueio" VisibleIndex="1">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataColumn Caption="Tipo do Bloqueio" FieldName="TipoBloqueio" VisibleIndex="2" />
            <dx:GridViewDataColumn Caption="Autor" FieldName="Autor" VisibleIndex="3" />
            <dx:GridViewDataColumn Caption="Serviço" FieldName="Produto" VisibleIndex="4" />
            <dx:GridViewDataColumn FieldName="Motivo" VisibleIndex="5" />
            <dx:GridViewDataColumn Caption="Remover" VisibleIndex="6" Width="15%">
                <DataItemTemplate>
                    <dx:ASPxButton OnClick="ASPxButtonRemoverBloqueio_Click" CommandArgument='<%# Container.KeyValue %>'
                        BackgroundImage-Repeat="NoRepeat" BackgroundImage-VerticalPosition="Center" BackgroundImage-HorizontalPosition="Center"
                        runat="server" ID="ASPxButtonRemoverBloqueio" BackgroundImage-ImageUrl="~/Imagens/trash_16x16.gif"
                        Width="16px" Height="16px">
                        <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Tem certeza que deseja remover o item selecionado?'); }" />
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataColumn>
        </Columns>
        <SettingsText EmptyDataRow="Sem itens para exibi&#231;&#227;o!"></SettingsText>
        <SettingsLoadingPanel ImagePosition="Top"></SettingsLoadingPanel>
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
    </dx:ASPxGridView>
</div>
<div runat="server" visible="false" id="DivBloqueio">
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelBloqueio" ShowHeader="true" Width="100%"
        HeaderText="Bloqueio" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td class="TituloNegrito" style="width: 17%; border: none;">
                            Tipo de Bloqueio:
                        </td>
                        <td style="border: none; text-align: left;">
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" BackColor="#efefef" AutoPostBack="true"
                                OnSelectedIndexChanged="DropDownListTipoBloqueio_SelectedIndexChanged" runat="server"
                                ID="DropDownListTipoBloqueio">
                                <asp:ListItem Value="0" Text="Completo"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Por Serviço"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Por Empresa"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left; border: none;">
                            <asp:MultiView Visible="false" runat="server" ID="MultiViewBloqueio">
                                <Views>
                                    <asp:View runat="server" ID="ViewBloqueioProduto">
                                        <dx:ASPxGridView SettingsLoadingPanel-Text="Carregando..." SettingsText-EmptyDataRow="Sem itens para exibição!"
                                            ID="ASPxGridViewProdutos" ClientInstanceName="aSPxGridViewProdutos" runat="server"
                                            KeyFieldName="IdProduto" Width="100%">
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" />
                                                <dx:GridViewDataColumn FieldName="ProdutoGrupo" VisibleIndex="1" />
                                                <dx:GridViewDataColumn FieldName="Produto" VisibleIndex="2" />
                                                <dx:GridViewDataColumn FieldName="Verba" VisibleIndex="3" />
                                                <dx:GridViewDataColumn FieldName="Banco" VisibleIndex="4" />
                                            </Columns>
                                            <ClientSideEvents SelectionChanged="lerItensSelecionados" />
                                            <SettingsLoadingPanel ImagePosition="Top"></SettingsLoadingPanel>
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
                                        </dx:ASPxGridView>
                                    </asp:View>
                                    <asp:View runat="server" ID="ViewBloqueioBanco">
                                        <dx:ASPxListBox ID="ASPxListBoxEmpresasBloqueio" runat="server" SelectionMode="CheckColumn"
                                            Width="250" Height="210" ValueField="IDEmpresa" ValueType="System.Int32" TextField="Nome"
                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" LoadingPanelImagePosition="Top"
                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                            </LoadingPanelImage>
                                            <ValidationSettings>
                                                <ErrorFrameStyle ImageSpacing="4px">
                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                </ErrorFrameStyle>
                                            </ValidationSettings>
                                        </dx:ASPxListBox>
                                    </asp:View>
                                </Views>
                            </asp:MultiView>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="border: none;">
                            Motivo:
                        </td>
                        <td style="border: none; text-align: left;">
                            <asp:TextBox BorderWidth="1px" BorderColor="#6ccfe9" runat="server" ID="TextBoxMotivo"
                                TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left; border: none;">
                            <asp:HiddenField runat="server" ID="HiddenFieldItensSelecionados" ClientIDMode="Static" />
                            <asp:Button OnClick="ButtonSalvarBloqueio_Click" CssClass="BotaoEstiloGlobal" ID="ButtonSalvarBloqueio"
                                Text="Salvar" runat="server" />&nbsp;
                            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonVoltar" OnClick="ButtonVoltar_Click"
                                Text="Cancelar" runat="server" />
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
