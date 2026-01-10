<%@ Control ClassName="WebUserControlContatosEdicao" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlContatosEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlContatosEdicao" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="BordaBase" colspan="2">
                    <h1 class="TituloTabela">
                       Novo Contato</h1>
                </td>      
        </tr>
        <tr>
            <td  class="TituloNegrito" style="width: 20%;">
                Nome:
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNome"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  class="TituloNegrito">
                Título:
            </td>
            <td>
                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxTitulo"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  class="TituloNegrito">
                Tipo:
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="332px" runat="server" DataTextField="Nome"
                    DataValueField="IDContatoTipo" ID="DropDownListTipo" />
            </td>
        </tr>
        <tr>
            <td  class="TituloNegrito">
                Tipo:
            </td>
            <td>
                <asp:DropDownList CssClass="TextBoxDropDownEstilos"  Width="332px" runat="server" DataTextField="Nome"
                    DataValueField="IDEmpresaContatoPerfil" ID="DropDownListPerfil" />
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito" style="border: none;">
                Conteúdo:
            </td>
            <td style="border: none;">
                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxConteudo"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="border: none; padding: 5px;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="border: none; padding-left: 4px;">
                            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvarContato" OnClick="ButtonSalvarContatoClick"
                                Text="Salvar" runat="server" />&nbsp;
                        </td>
                        <td style="border: none; padding: 0px;">
                            <asp:Button ID="ButtonNovoContato" class="BotaoEstiloGlobal" OnClick="ButtonNovoContato_Click"
                                runat="server" Text="Limpar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
