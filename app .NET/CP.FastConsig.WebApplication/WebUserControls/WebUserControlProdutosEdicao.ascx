<%@ Control ClassName="WebUserControlProdutos_Edicao" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlProdutosEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlProdutosEdicao" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
        
        <tr>
            <td class="BordaBase">
                    <h1 class="TituloTabela">
                       Nova Verba/Evento</h1>
                </td>      
        </tr>
        
        
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            Nome:
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxProdutoNome"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Verba:
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxVerba"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Verba Folha:
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxVerbaFolha"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Carência Máxima (meses):
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxCarenciaMaxima"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Grupo:
                        </td>
                        <td>
                            <asp:DropDownList Width="332" CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Nome"
                                DataValueField="IDProdutoGrupo" ID="DropDownListGrupo" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="border: none;">
                            Consignante:
                        </td>
                        <td style="border: none;">
                            <asp:DropDownList Width="332" CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Fantasia"
                                DataValueField="IDEmpresa" ID="DropDownListConsiganante" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="border: none;">
                <table border="0" cellpadding="0" cellspacing="4">
                    <tr>
                        <td style="border: none;padding:0;">
                            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvarProduto" OnClick="ButtonSalvarProdutoClick"
                                Text="Salvar" runat="server" />&nbsp;
                        </td>
                        <td style="border: none;padding:0;">
                            <asp:Button ID="ButtonNovoProduto" class="BotaoEstiloGlobal" OnClick="ButtonNovoProduto_Click"
                                runat="server" Text="Novo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
