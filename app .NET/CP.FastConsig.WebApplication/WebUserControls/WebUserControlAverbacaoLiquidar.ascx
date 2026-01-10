<%@ Control ClassName="WebUserControlAverbacaoLiquidar" Language="C#" AutoEventWireup="True"
    CodeBehind="WebUserControlAverbacaoLiquidar.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAverbacaoLiquidar" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
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
                        <td class="TituloNegrito" style="border: none;">
                            CPF:
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
<br />
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosAverbacao" Width="100%"
        HeaderText="Dados da Averbação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
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
                        <td class="TituloNegrito" style="border: none;">
                            Situacao:
                        </td>
                        <td style="border: none;">
                            <asp:Label runat="server" ID="LabelSituacaoAtual"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div style="margin: 5px 0px;">
    <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabela">
        <tr>
            <td class="TituloNegrito" style="border: none;">
                <asp:Label ID="Label1" Text="Motivo:" runat="server"></asp:Label>
            </td>
            <td style="border: none;">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtMotivo" CssClass="TextBoxDropDownEstilos" Height="150" Width="330" Rows="5" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border: none;" colspan="2">
                <table class="TabelaWebUserControlGerenciarAverbacao" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td>
                            <dx:ASPxButton Cursor="Pointer" ID="ASPxButtonSalvar" EnableDefaultAppearance="false"
                                CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Liquidar"
                                OnClick="SalvarClick">
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
