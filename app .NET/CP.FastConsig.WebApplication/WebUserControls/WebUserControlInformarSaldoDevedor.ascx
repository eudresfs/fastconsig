<%@ Control Language="C#" ClassName="WebUserControlInformarSaldoDevedor" AutoEventWireup="true"
    CodeBehind="WebUserControlInformarSaldoDevedor.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlInformarSaldoDevedor" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        HeaderStyle-Font-Bold="true" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
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
</div>
<div style="height: 5px;">
    &nbsp;</div>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosAverbacao" HeaderStyle-Font-Bold="true"
        Width="100%" HeaderText="Dados da Averbação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" width="100%">
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
                            <b>Consignatária:</b>
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
                        <td class="TituloNegrito">
                            Saldo Devedor Atual:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSaldo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseDois">
                            Situacao:
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
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelInformarSaldoDevedor" ShowHeader="true"
        Width="100%" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="0"
        ContentPaddings-PaddingTop="5px" ContentPaddings-PaddingRight="0px" HeaderText="Informações do Saldo Devedor"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" HeaderStyle-Font-Bold="true">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentInformarSaldoDevedor" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabela" width="100%">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="TamanhoCelulaTres">
                            <asp:Label runat="server" Text="Validade:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="DateEditValidade" runat="server" CssClass="CalendarioEstilos"
                                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="30px"
                                ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                <CalendarProperties>
                                    <HeaderStyle Spacing="1px" />
                                    <FooterStyle Spacing="17px" />
                                </CalendarProperties>
                                <DropDownButton>
                                    <Image>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                    </Image>
                                </DropDownButton>
                                <ValidationSettings>
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Valor:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" EnableTheming="false" CssClass="TextBoxDropDownEstilos"
                                ID="ASPxTextBoxValor" runat="server">
                                <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Forma de Pagamento:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" ID="DropDownListFormaPagamento"
                                runat="server" DataTextField="Nome" DataValueField="IDTipoPagamento" AutoPostBack="true"
                                OnSelectedIndexChanged="DropDownListFormaPagamento_SelectIndexChanged">
                            </asp:DropDownList>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="DadosTED" visible="false" border="0" cellpadding="0" cellspacing="0"
                    class="WebUserControlTabela" width="100%">
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Identificador:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" MaxLength="50" EnableTheming="false"
                                CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxIdentificado" runat="server">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Banco:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" MaxLength="50" EnableTheming="false"
                                CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxBanco" runat="server">
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Agência:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" MaxLength="50" EnableTheming="false"
                                CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxAgencia" runat="server">
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Conta de Crédito:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" MaxLength="50" EnableTheming="false"
                                CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxContaCredito" runat="server">
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" Text="Nome do Favorecido:"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox EnableDefaultAppearance="false" MaxLength="80" EnableTheming="false"
                                CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxNomeFavorecido" runat="server">
                            </dx:ASPxTextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabela" width="100%">
                    <tr>
                        <td class="TituloNegrito" clientidmode="Static" id="SemBordaBaseTres">
                            <asp:Label ID="Label1" runat="server" Text="Observação:"></asp:Label>
                        </td>
                        <td class="SemBordaBase">
                            <dx:ASPxMemo EnableDefaultAppearance="false" EnableTheming="false" CssClass="TextBoxDropDownEstilos"
                                Height="120px" ID="ASPxTextBoxObs" runat="server" MaxLength="255">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td class="SemBordaBase">
                            <span class="TituloCamposObrigatorios">*Campo Obrigatório</span>
                        </td>
                    </tr>
                </table>
                <div class="float-divider">
                    <dx:ASPxButton Cursor="Pointer" ID="ASPxButtonSalvar" EnableDefaultAppearance="false"
                        CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Salvar"
                        OnClick="SalvarSaldoDevedor_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
            Topo da Página</a>
    </h1>
</div>