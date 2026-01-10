<%@ Control ClassName="WebUserControlInformarQuitacao" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlInformarQuitacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlInformarQuitacao" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxUploadControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" HeaderStyle-Font-Bold="true"
        Width="100%" HeaderText="Dados do Funcionário" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaHum">
                            Matrícula:
                        </td>
                        <td class="TituloNegrito">
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
</div>
<div style="height: 5px;">
    &nbsp;</div>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosAverbacao" Width="100%"
        HeaderText="Dados da Averbação" HeaderStyle-Font-Bold="true" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaDois">
                            Número:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNumeroAverbacao"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Consignatária:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelConsignataria"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Prazo:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelPrazo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Valor Parcela:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseDois">
                            Situação:
                        </td>
                        <td class="SemBordaBase">
                            <asp:Label runat="server" ID="LabelSituacaoAtual"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div style="height: 5px;">
    &nbsp;</div>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel1" HeaderStyle-Font-Bold="true"
        Width="100%" HeaderText="Saldo Devedor" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaTres">
                            Informado em:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelInformadoEm"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Data de Validade:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelDataValidade"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Forma de Pagamento:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelFormaPagamento"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Banco:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelBancoCredito"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Agência:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelAgenciaCredito"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Conta:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelContaCredito"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Favorecido:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelFavorecidoCredito"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Observação:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelObservacao"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Saldo em Aberto:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSaldoAberto"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Desconto:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelDesconto"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ececec">
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseTres">
                            Saldo Devedor:
                        </td>
                        <td class="SemBordaBase">
                            <asp:Label runat="server" ID="LabelSaldoDevedor"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div style="height: 5px;">
    &nbsp;</div>
<div>
    <dx:ASPxRoundPanel HeaderStyle-Font-Bold="true" runat="server" ID="ASPxRoundPanel2"
        Width="100%" HeaderText="Informe a Quitação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaQuatro">
                            Data da Quitação:
                        </td>
                        <td>
                            <dx:ASPxDateEdit AllowNull="false" ID="ASPxDateEditDataQuitacao" runat="server" Width="200" />
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Valor:
                        </td>
                        <td>
                            <dx:ASPxTextBox Width="332px" CssClass="TextBoxDropDownEstilos" ClientInstanceName="aSPxTextBoxValor" ID="ASPxTextBoxValor"
                                runat="server">
                                <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Forma de Pagamento:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="DropDownListFormaPagamento" Enabled="false"
                                DataTextField="Nome" DataValueField="IDTipoPagamento" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Observação:
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" ClientIDMode="Static" runat="server" ID="TextBoxObservacao"
                                TextMode="MultiLine" Height="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseQuatro">
                            Comprovante de Quitação:
                        </td>
                        <td class="SemBordaBase">
                            <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabelaInformarQuitacaoUploadComprovante">
                                <tr>
                                    <td>
                                        <dx:ASPxUploadControl BrowseButton-Text="Procurar" BrowseButtonStyle-CssClass="BotaoEstiloGlobal"
                                            Height="29" BrowseButtonStyle-Border-BorderColor="#a3c0e8" BrowseButtonStyle-Border-BorderStyle="Solid"
                                            BrowseButtonStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderStyle="Solid"
                                            TextBoxStyle-BackColor="#ffffff" TextBoxStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderColor="#a3c0e8"
                                            EnableDefaultAppearance="True" EnableTheming="False" BrowseButtonStyle-Font-Size="11px"
                                            FileUploadMode="OnPageLoad" ID="ASPxUploadComprovante" runat="server" ClientInstanceName="uploader"
                                            ShowProgressPanel="True" Size="35" OnFileUploadComplete="ASPxUploadComprovante_FileUploadComplete">
                                            <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); document.getElementById('TextBoxObservacao').value = ''; aSPxTextBoxValor.SetText(''); }"
                                                FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"></ClientSideEvents>
                                            <ValidationSettings GeneralErrorText="Verifique as informações!" NotAllowedFileExtensionErrorText="Arquivo inválido! Tipos suportados: .jpg, .jpeg, .pdf, .doc e .png." MaxFileSize="4194304"
                                                AllowedFileExtensions=".jpg,.jpeg,.pdf,.doc,.png">
                                            </ValidationSettings>
                                        </dx:ASPxUploadControl>
                                        <span class="EstiloCampoObrigatorio">*</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="SemBordaBase">
                            <table class="WebUserControlTabelaInformarQuitacao" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxButton EnableDefaultAppearance="false" Paddings-Padding="0" EnableTheming="false"
                                            CssClass="BotaoEstiloGlobal" ID="ASPxButtonInformarQuitacao" runat="server" AutoPostBack="False"
                                            Text="Informar Quitação" ClientInstanceName="btnUpload">
                                            <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="SemBordaBase">
                            <span class="TituloCamposObrigatorios">*Campo Obrigatório</span>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
            Topo da Página</a>
    </h1>
    <div style="height:8px;">
             &nbsp;    
    </div>
</div>
