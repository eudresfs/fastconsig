<%@ Control ClassName="WebUserControlImportacao" ClientIDMode="AutoID" Language="C#"
    AutoEventWireup="true" CodeBehind="WebUserControlImportacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlImportacao" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxUploadControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="WebUserControlImportacaoFiltros.ascx" TagName="WebUserControlImportacaoFiltros"
    TagPrefix="uc1" %>
<asp:Panel runat="server" ID="PanelBotoes" CssClass="tela">
    <dx:ASPxButton ForeColor="#004d63" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png"
        Border-BorderColor="#6ccfe9" ImagePosition="Top" Image-Url="~/Imagens/MenuImportarFuncionarios.png"
        BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png" EnableTheming="True"
        EnableDefaultAppearance="True" AutoPostBack="false" OnClick="ASPxButtonMenuFuncionarios_Click"
        Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonMenuFuncionarios"
        runat="server" Text="Funcionários" Height="140px">
    </dx:ASPxButton>
    <dx:ASPxButton ForeColor="#004d63" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png"
        Border-BorderColor="#6ccfe9" ImagePosition="Top" Image-Url="~/Imagens/MenuImportarContratos.png"
        BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png" EnableTheming="True"
        EnableDefaultAppearance="True" AutoPostBack="false" OnClick="ASPxButtonMenuContratos_Click"
        Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonMenuContratos" runat="server"
        Text="Averbações" Height="140px">
    </dx:ASPxButton>
    <dx:ASPxButton ForeColor="#004d63" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png"
        Border-BorderColor="#6ccfe9" ImagePosition="Top" Image-Url="~/Imagens/MenuImportarRetornoFolha.png"
        BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png" EnableTheming="True"
        EnableDefaultAppearance="True" AutoPostBack="false" OnClick="ASPxButtonMenuRetornoFolha_Click"
        Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonMenuRetornoFolha"
        runat="server" Text="Retorno Folha" Height="140px">
    </dx:ASPxButton>
    <dx:ASPxButton ForeColor="#004d63" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png"
        Border-BorderColor="#6ccfe9" ImagePosition="Top" Image-Url="~/Imagens/MenuImportarPersonalizado.png"
        BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png" EnableTheming="True"
        EnableDefaultAppearance="True" AutoPostBack="false" OnClick="ASPxButtonMenuPersonalizado_Click"
        Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonMenuPersonalizado"
        runat="server" Text="Personalizada" Height="140px">
    </dx:ASPxButton>
</asp:Panel>
<div style="height: 1px; clear: both; overflow: hidden;">
    &nbsp;</div>
<div>
    <fieldset runat="server" id="FieldsetImportacao" visible="false" class="formulario">
        <legend>Importação </legend>
        <div class="divMensagemExplicacao">
            <asp:Label runat="server" CssClass="mensagem" ID="LabelExplicacaoOpcao"></asp:Label>
        </div>
        <div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label CssClass="label" runat="server" ID="LabelNome" Text="Seu Nome: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNome" />
                    </td>
                    <td>
                        <asp:Label CssClass="label" runat="server" ID="LabelTelefone" Text="Seu Telefone: "></asp:Label>
                    </td>
                    <td style="padding-left: 5px;">
                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="150" Border-BorderColor="#c0dfe8"
                            ID="TextBoxTelefone" runat="server">
                            <MaskSettings Mask="(99) 9999,9999" ErrorText="Preencha o campo telefone completammente." />
                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div style="text-align: left; width: 0px; margin-left: 112px" runat="server" id="DivCamposImportacaoPersonalizada"
            visible="false">
            <table align="left">
                <tr>
                    <td>
                        Tabela:
                    </td>
                    <td>
                        <asp:TextBox CssClass="textarea" runat="server" ID="TextBoxNomeTabela"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Campo:
                    </td>
                    <td>
                        <asp:TextBox CssClass="textarea" runat="server" ID="TextBoxCampoPersonalizado"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button CssClass="botao" runat="server" ID="ButtonAdicionarCampoPersonalizado"
                            OnClick="ButtonAdicionarCampoPersonalizado_Click" Text="Adicionar" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:ListBox CssClass="textarea" runat="server" ID="ListBoxCamposPersonalizado" Rows="5" />
                    </td>
                    <td>
                        <asp:Button CssClass="botao" runat="server" ID="ButtonRemoverCampoPersonalizado"
                            OnClick="ButtonRemoverCampoPersonalizado_Click" Text="Remover" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
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
        <div style="clear: both;">
            <asp:Label CssClass="label" runat="server" ID="LabelImportarPrimeiraLinha" Text="Importar primeira linha: "></asp:Label>
            &nbsp;&nbsp;<asp:CheckBox runat="server" ID="CheckBoxImportarPrimeiraLinha" Checked="True" />
        </div>
        <div id="DivNomeLayout" runat="server">
            <asp:Label CssClass="label" runat="server" ID="LabelNomeLayout" Text="Nome para Layout: "></asp:Label>
            <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNomeLayout"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:Label CssClass="label" runat="server" ID="LabelLayout" Text="Layout: "></asp:Label>
            <asp:DropDownList CssClass="select" runat="server" ID="DropDownListLayout">
            </asp:DropDownList>
        </div>
        <br />
        <div>
            <asp:Label CssClass="label" runat="server" ID="LabelObservacao" Text="Observação: "></asp:Label>
            <asp:TextBox CssClass="textarea" runat="server" ID="TextBoxObservacao" TextMode="MultiLine"></asp:TextBox>
        </div>
        <br />
        <div style="float: left; clear: left">
            <dx:ASPxButton ID="ASPxButtonUploadFoto" CssClass="primeiroBotao devBtn" runat="server"
                AutoPostBack="False" Text="Enviar" ClientInstanceName="btnUpload" ClientEnabled="False">
                <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
            </dx:ASPxButton>
        </div>
        <br />
        <div>
            <asp:Button CssClass="botao" runat="server" ID="ButtonVoltar" OnClick="ButtonVoltar_Click"
                Text="Voltar" />
            <asp:Button CssClass="botao" runat="server" ID="ButtonConfigurar" OnClick="ButtonConfigurar_Click"
                Text="Configurar" />
        </div>
    </fieldset>
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
            <tr>
                <td>
                    <table>
                        <asp:Repeater OnItemDataBound="RepeaterConfiguracaoImportacao_ItemDataBound" ID="RepeaterConfiguracaoImportacao"
                            runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top">
                                        Coluna
                                        <%# (Container.ItemIndex + 1) %>:
                                    </td>
                                    <td class="TdColuna">
                                        <asp:DropDownList CssClass="DropdownlistUserControl" runat="server" ID="DropDownListOpcaoColuna" />
                                    </td>
                                    <td valign="top">
                                        <asp:LinkButton runat="server" ID="LinkButtonFiltros" Text="Filtros"></asp:LinkButton>
                                    </td>
                                    <td style="width: 10px; background-color: #fff">
                                    </td>
                                    <td valign="top" runat="server" id="TdFiltroLinha" class="colunaFiltro" style="display: none">
                                        <div id="FiltroModal" title="Filtros" runat="server" class="classeFiltros">
                                            <uc1:WebUserControlImportacaoFiltros ID="WebUserControlImportacaoFiltrosLinha" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr style="background-color: #efefef">
                                    <td valign="top">
                                        Coluna
                                        <%# (Container.ItemIndex + 1) %>:
                                    </td>
                                    <td class="TdColuna" valign="top">
                                        <asp:DropDownList CssClass="DropdownlistUserControl" runat="server" ID="DropDownListOpcaoColuna" />
                                    </td>
                                    <td valign="top">
                                        <asp:LinkButton runat="server" ID="LinkButtonFiltros" Text="Filtros"></asp:LinkButton>
                                    </td>
                                    <td style="width: 10px; background-color: #fff">
                                    </td>
                                    <td valign="top" runat="server" id="TdFiltroLinha" class="colunaFiltro" style="display: none">
                                        <div id="FiltroModal" title="Filtros" runat="server" class="classeFiltros">
                                            <uc1:WebUserControlImportacaoFiltros ID="WebUserControlImportacaoFiltrosLinha" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
                <td valign="top">
                    <div id="DivLayouts" runat="server">
                        <b>Carregar Layout:</b><br />
                        <asp:DropDownList CssClass="TextBoxDropDownEstilos" DataTextField="NomeLayout" DataValueField="IdImportacaoLayout"
                            runat="server" ID="DropDownListLayoutsSalvos">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Button runat="server" ID="ButtonRemoverLayout" CssClass="botao" Text="Remover Layout"
                            OnClick="ButtonRemoverLayout_Click" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div style="text-align: center">
            <asp:Button CssClass="botao" OnClientClick="ObtemDadosFiltros();" runat="server"
                ID="ButtonImportar" OnClick="ButtonImportar_Click" Text="Importar" />&nbsp;
            <asp:Button CssClass="botao" OnClientClick="ObtemDadosFiltros();" runat="server"
                ID="ButtonSalvarImportar" OnClick="ButtonSalvarImportar_Click" Text="Salvar Layout e Importar" />
            <asp:HiddenField runat="server" ID="HiddenFieldFiltros" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="HiddenFieldColunas" ClientIDMode="Static" />
        </div>
    </div>
</div>
