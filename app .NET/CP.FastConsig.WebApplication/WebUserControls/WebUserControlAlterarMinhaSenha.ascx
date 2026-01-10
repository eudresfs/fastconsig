<%@ Control ClassName="WebUserControlAlterarMinhaSenha" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlAlterarMinhaSenha.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAlterarMinhaSenha" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<dx:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderText="Alteração de Senha" runat="server"
    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" He CssPostfix="Aqua"
    GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
    <ContentPaddings Padding="5px" />
    <HeaderStyle Font-Bold="True"></HeaderStyle>
    <PanelCollection>
        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
            <div class="BoxPerfilAlterarSenha">
                <asp:Image BorderStyle="Ridge" BorderWidth="1px" ID="ImagemFloat" ClientIDMode="Static"
                    runat="server" ImageUrl="~/Imagens/Perfil.png" Width="65px" Height="74px" /><br />
                <h3 id="TextoNome">
                    <asp:Label runat="server" ID="LabelPerfil"></asp:Label><br />
                </h3>
                <h3 id="TextoCargo">
                    <asp:Label runat="server" ID="LabelNomePerfil"></asp:Label><br />
                    <br />
                </h3>
            </div>
            <div class="BoxConfiguraSenha">
                <table border="0" cellpadding="0" width="100%" cellspacing="0" class="WebUserControlTabela">
                    
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            <asp:Label runat="server" ID="LabelSenhaAntiga">Informe a Senha Antiga:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" TextMode="Password" Width="100px"
                                Height="27" runat="server" ID="TextBoxSenhaAntiga"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" ID="Label1">Informe a Senha Nova:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" TextMode="Password" Width="100px"
                                Height="27" runat="server" ID="TextBoxSenhaNova"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="border: none;">
                            <asp:Label runat="server" ID="Label2">Repita a Senha Nova:</asp:Label>
                        </td>
                        <td class="UltimaCelulaSemBordaBase">
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" TextMode="Password" Width="100px"
                                Height="27" runat="server" ID="TextBoxSenhaNova2"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="UltimaCelulaSemBordaBase" colspan="2">
                            <asp:Button ID="Button1" class="BotaoEstiloGlobal" runat="server" Text="Confirma a Alteração da Senha"
                                OnClick="ButtonAlterarSenha_Click" />
                        </td>
                    </tr>
                </table>
                
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
