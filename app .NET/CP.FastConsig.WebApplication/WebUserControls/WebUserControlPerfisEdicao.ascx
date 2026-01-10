<%@ Control ClassName="WebUserControlPerfisEdicao" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlPerfisEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlPerfisEdicao" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="TituloNegrito" style="width: 20%;">
                Nome:
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNome"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito" runat="server" id="LabelModulo">
                Módulo:
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="332px" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="DropDownListModulo_SelectedIndexChanged" DataTextField="Nome"
                    DataValueField="IDModulo" ID="DropDownListModulo" />
            </td>
        </tr>
        <tr runat="server" id="tr_consignataria" visible="false">
            <td class="TituloNegrito">
                Consignatária:
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Nome"
                    DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito">
                Copiar Perfil de:
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="332px" runat="server" DataTextField="Nome"
                    DataValueField="IDPerfil" ID="DropDownListCopiarPerfil" />
                &nbsp;&nbsp;
                <asp:Button ID="ButtonCopiar" Visible="false" class="BotaoEstiloGlobal" OnClick="ButtonCopiar_Click"
                    runat="server" Text="Copiar do Perfil" />
            </td>
        </tr>
        <tr>
            <td style="border: none; padding:5px;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="border: none;padding:0px;">
                            <asp:Button ID="ButtonNovo" class="BotaoEstiloGlobal" OnClick="ButtonNovo_Click"
                                runat="server" Text="Novo" />
                        </td>
                        <td style="border: none;padding-left:4px;">
                            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvar" OnClick="ButtonSalvarClick"
                                Text="Salvar" runat="server" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
