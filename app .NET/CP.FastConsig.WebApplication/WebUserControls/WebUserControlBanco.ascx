<%@ Control ClassName="WebUserControlBanco" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlBanco.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlBanco" %>
<div style="text-align: center">
    <asp:Button Text="Estados" runat="server" ID="ButtonEstados" OnClick="ButtonEstados_Click" />&nbsp;
    <asp:Button Text="Cidades" runat="server" ID="ButtonCidades" OnClick="ButtonCidades_Click" />&nbsp;
    <asp:Button Text="Autarquias" runat="server" ID="ButtonAutarquias" OnClick="ButtonAutarquias_Click" />&nbsp;
    <asp:Button Text="Favoritos" runat="server" ID="ButtonFavoritos" OnClick="ButtonFavoritos_Click" />&nbsp;
</div>
<div>&nbsp;</div>
<div>
    <asp:MultiView runat="server" ID="MultiViewOpcoesBancos">
        <asp:View runat="server" ID="ViewEstados">Estados</asp:View>
        <asp:View runat="server" ID="ViewCidades">Cidades</asp:View>
        <asp:View runat="server" ID="ViewAutarquias">Autarquias</asp:View>
        <asp:View runat="server" ID="ViewFavorites">Favoritos</asp:View>
    </asp:MultiView>
</div>