<%@ Control ClassName="WebUserControlUsuario" Language="C#" AutoEventWireup="True"
    CodeBehind="WebUserControlUsuario.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlUsuario" %>
<div id="BoxPerfil">
    <asp:Image BorderStyle="Ridge" BorderWidth="1px" ID="ImagemFloat" runat="server"
        ImageUrl="~/Imagens/Perfil.png" Width="65px" Height="74px" />
    <h3 id="TextoNome">
        <asp:Label runat="server" ID="LabelNomePerfil"></asp:Label><br />
    </h3>
    <h3 id="TextoCargo">
        <asp:Label runat="server" ID="LabelPerfil"></asp:Label><br />
        <br />
    </h3>
</div>