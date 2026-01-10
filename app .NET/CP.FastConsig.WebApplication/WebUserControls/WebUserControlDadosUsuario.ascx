<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlDadosUsuario.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlDadosUsuario" %>
<fieldset>
    <legend><asp:Label ID="LabelTitulo" runat=""></asp:Label></legend>
    <table width="100%">
        <tr>
            <td><asp:Image runat="server" ID="ImageFotoUsuario"/></td>
            <td style="text-align: left">
                <table width="50%">
                    <tr>
                        <td><b>Nome:</b></td>
                        <td><asp:Label runat="server" ID="LabelNome"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>Matrícula:</b></td>
                        <td><asp:Label runat="server" ID="LabelMatricula"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>CPF:</b></td>
                        <td><asp:Label runat="server" ID="LabelCpf"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>Data de Admissão:</b></td>
                        <td><asp:Label runat="server" ID="LabelDataAdmissao"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>Regime:</b></td>
                        <td><asp:Label runat="server" ID="LabelRegime"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>Categoria:</b></td>
                        <td><asp:Label runat="server" ID="LabelCategoria"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b>Local:</b></td>
                        <td><asp:Label runat="server" ID="LabelLocal"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>