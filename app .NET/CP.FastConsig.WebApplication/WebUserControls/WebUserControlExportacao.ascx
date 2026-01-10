<%@ Control ClassName="WebUserControlExportacao" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlExportacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlExportacao" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelParametros" HeaderStyle-Font-Bold="true"
    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
    ContentPaddings-PaddingBottom="10px" ContentPaddings-PaddingTop="5px" ContentPaddings-PaddingRight="0px"
    CssPostfix="Aqua" HeaderText="Quadro de Principais Parâmetros" Height="270px"
    GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
    <panelcollection>
            <dx:PanelContent ID="PanelContentParametros" runat="server" Height="270px" SupportsDisabledAttribute="True">



<asp:Panel runat="server" ID="PanelBotoes" CssClass="tela">
    <dx:ASPxButton ForeColor="#004d63" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png"
        Border-BorderColor="#6ccfe9" ImagePosition="Top" Image-Url="~/Imagens/MenuImportarRetornoFolha.png"
        BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png" EnableTheming="True"
        EnableDefaultAppearance="True" AutoPostBack="false" OnClick="ASPxButtonMenuDescontoFolha_Click"
        Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonMenuDescontoFolha"
        runat="server" Text="Desconto Folha" Height="140px">
    </dx:ASPxButton>
</asp:Panel>
<div style="height: 15px; clear: both; overflow: hidden;">
    &nbsp;</div>

<div>


    <div>
        <table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
            font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
            cellspacing="0">
            <tr>
                <td colspan="2" style="border-bottom: 1px solid #000000;">
                    <h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                        display: inline;">
                       Obs: Baixe o arquivo de descontos para Folha de Pagamento</h1>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Seu Nome:
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNome" />
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Seu Telefone:
                </td>
                <td style="padding-left: 5px;">
                    <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="150" Border-BorderColor="#c0dfe8"
                        ID="TextBoxTelefone" runat="server">
                        <MaskSettings Mask="(99) 9999,9999" ErrorText="Preencha o campo telefone completammente." />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Mês/Ano:
                </td>
                <td>
                    <dx:ASPxTextBox MaskSettings-ShowHints="False" ValidationSettings-RequiredField="false"
                        ValidationSettings-ValidateOnLeave="False" ValidationSettings-EnableCustomValidation="False"
                        ValidationSettings-CausesValidation="false" ValidationSettings-Display="None"
                        CssClass="TextBoxDropDownEstilos" Height="30px" Width="75px" ID="ASPxTextAnoMes"
                        MaskSettings-Mask="99/9999" EnableDefaultAppearance="True" EnableTheming="True"
                        runat="server">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Layout:
                </td>
                <td>
                    <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" ID="DropDownListLayout">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <%-- 
        <br/>
        <div>

           <asp:Label CssClass="label" runat="server" ID="LabelArquivo" Text="Selecione um arquivo: "></asp:Label>
        <dx:ASPxUploadControl FileUploadMode="OnPageLoad" ID="ASPxUploadControlFoto" runat="server"
            ClientInstanceName="uploader" ShowProgressPanel="false" OnFileUploadComplete="ASPxUploadControlFoto_FileUploadComplete">
            <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
                FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
            <ValidationSettings MaxFileSize="102400000" AllowedFileExtensions=".xls,.csv,.txt,.xlsx"
                GeneralErrorText="Escolha um arquivo nos formatos: xls, csv, txt ou xlsx." NotAllowedFileExtensionErrorText="Escolha um arquivo nos formatos: xls, csv, txt ou xlsx.">
            </ValidationSettings>
        </dx:ASPxUploadControl>

        </div>
        <br />
    --%>
    <div style="text-align: left; width: 0px; margin-left: 112px" runat="server" id="DivCamposImportacaoPersonalizada"
        visible="false">
        <table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
            font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
            cellspacing="0">
            <tr>
                <td class="TituloNegrito">
                    Tabela:
                </td>
                <td>
                    <asp:TextBox CCssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxNomeTabela"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Campo:
                </td>
                <td>
                    <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxCampoPersonalizado"></asp:TextBox>
                </td>
                <td>
                    <asp:Button CssClass="BotaoEstiloGlobal"  runat="server" ID="ButtonAdicionarCampoPersonalizado"
                        OnClick="ButtonAdicionarCampoPersonalizado_Click" Text="Adicionar" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox CssClass="TextBoxDropDownEstilos" runat="server" ID="ListBoxCamposPersonalizado" Rows="5" />
                </td>
                <td>
                    <asp:Button CssClass="BotaoEstiloGlobal"  runat="server" ID="ButtonRemoverCampoPersonalizado"
                        OnClick="ButtonRemoverCampoPersonalizado_Click" Text="Remover" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%" border="0" cellpadding="0"
            cellspacing="0">
            <tr>
                <td class="TituloNegrito" style="width:20%;">
                    
                        Observação:
                </td>
                <td>
                    <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxObservacao" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    <div style="float: left; clear: left">
        <asp:Button ID="ButtonExportar" CssClass="BotaoEstiloGlobal"  runat="server" Text="Baixar" OnClick="ButtonExportar_Click" />
        <asp:Button CssClass="BotaoEstiloGlobal"  runat="server" ID="ButtonVoltar" OnClick="ButtonVoltar_Click"
            Text="Voltar" />
        <asp:Button CssClass="BotaoEstiloGlobal"  runat="server" ID="ButtonConfigurar" OnClick="ButtonConfigurar_Click"
            Text="Configurar" />
    </div>
</div>
<div>
    &nbsp;</div>
<div id="DivConfiguracaoImportacao" runat="server" visible="false">
    <div>
        <fieldset>
            <legend>Configuração da Importação</legend>
            <div>
                <asp:GridView EmptyDataText="Sem itens para exibição!" runat="server" ID="GridViewPreviewImportacao"
                    CssClass="EstilosGridViewDashBoardConsiliacao" Width="100%" Height="100%" AutoGenerateColumns="true">
                    <HeaderStyle Height="16px" CssClass="CabecalhoGridView" HorizontalAlign="Left" />
                    <RowStyle Height="16px" CssClass="LinhaListaGridView" HorizontalAlign="Left" />
                    <AlternatingRowStyle Height="17px" CssClass="LinhaAlternadaListaGridView" HorizontalAlign="Left" />
                    <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <div>
        &nbsp;</div>
    <div>
        <table>
            <asp:Repeater OnItemDataBound="RepeaterConfiguracaoImportacao_ItemDataBound" ID="RepeaterConfiguracaoImportacao"
                runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            Coluna
                            <%# (Container.ItemIndex + 1) %>:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="DropdownlistUserControl" runat="server" ID="DropDownListOpcaoColuna" />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background-color: #efefef">
                        <td>
                            Coluna
                            <%# (Container.ItemIndex + 1) %>:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="DropdownlistUserControl" runat="server" ID="DropDownListOpcaoColuna" />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <br />
        <div style="text-align: center">
            <asp:Button CssClass="BotaoEstiloGlobal"  runat="server" ID="ButtonImportar" Text="Importar" />
        </div>
    </div>
</div>


 </dx:PanelContent>
        </panelcollection>
</dx:ASPxRoundPanel>
