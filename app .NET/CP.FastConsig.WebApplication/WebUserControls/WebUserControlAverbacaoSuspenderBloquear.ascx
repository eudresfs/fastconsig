<%@ Control ClassName="WebUserControlAverbacaoSuspenderBloquear" Language="C#" AutoEventWireup="True" CodeBehind="WebUserControlAverbacaoSuspenderBloquear.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAverbacaoSuspenderBloquear" %>
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
                <table width="100%">                    
                <tr>
                        <td>
                            <b>Matrícula:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <b>Nome:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <b>CPF:</b>
                        </td>
                        <td>
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
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosAverbacao" Width="100%" HeaderText="Dados da Averbação"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">                    
                <tr>
                        <td>
                            <b>Número:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNumeroAverbacao"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <b>Consignatária:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelConsignataria"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Prazo:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelPrazo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Valor Parcela:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Situacao:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSituacaoAtual"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>

<div>
    <fieldset title="Tipo" id="fsSuspender" runat="server">
        <legend>Tipo</legend>
    <dx:ASPxRadioButton ID="ASPxRadioButtonSuspender" Checked="true" GroupName="suspender" Text="Suspender (Margem Liberada)" runat="server">
    </dx:ASPxRadioButton>
    <dx:ASPxRadioButton ID="ASPxRadioButtonBloquear" GroupName="suspender" Text="Bloquear (Margem Retida)" runat="server">
    </dx:ASPxRadioButton>
    </fieldset>
    <asp:Label Text="Motivo:" runat="server"></asp:Label>
    <br />
    <asp:TextBox TextMode="MultiLine" runat="server" ID="txtMotivo" Rows="5" Width="100%"></asp:TextBox> 
    <br />
    <div class="float-divider">
    <dx:ASPxButton Cursor="Pointer" ID="ASPxButtonSalvar" Width="90" EnableDefaultAppearance="false"
        CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Salvar" OnClick="SalvarSuspensaoClick">
    </dx:ASPxButton>
    <br />
    </div>
    <br />
</div>
