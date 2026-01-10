<%@ Control ClassName="WebUserControlFuncionarios_Autorizacoes_Edicao" Language="C#"
    AutoEventWireup="True" EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlFuncionariosAutorizacoesEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFuncionariosAutorizacoesEdicao" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
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
<br />
<div class="GlobalUserControl">
    <div style="margin: 5px 0px;">
        <asp:Button ID="ButtonNovo" CssClass="BotaoEstiloGlobal" class="BotaoNovoConsignatario"
            OnClick="ButtonNovo_Click" runat="server" Text="Novo" /></div>
    <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td class="TituloNegrito" style="width: 20%;">
                Data:
            </td>
            <td>
                <dx:ASPxDateEdit SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="30px" ID="ASPxDateEditData" runat="server">
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito">
                Tipo
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Height="30px" runat="server" DataTextField="Nome"
                    DataValueField="IDFuncionarioAutorizacaoTipo" ID="DropDownListTipo" />
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito">
                Dias de Validade:
            </td>
            <td>
                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxValidade" ClientIDMode="Static" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito" style="border: none;">
                Motivo:
            </td>
            <td style="border: none;">
                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxMotivo"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="text-align: left; width: 100%; margin: 5px 0px;">
        <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvar" OnClick="ButtonSalvarClick"
            Text="Salvar" runat="server" />&nbsp;
    </div>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
