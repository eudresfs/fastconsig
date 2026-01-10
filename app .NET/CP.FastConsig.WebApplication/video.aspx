<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="video.aspx.cs" Inherits="CP.FastConsig.WebApplication.video" %>

<%@ Register TagPrefix="Bewise" Namespace="Bewise.Web.UI.WebControls" Assembly="FlashControl, Version=2.4.2276.31815, Culture=neutral, PublicKeyToken=0066be59ecd613cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        
        .texto {

            font-family: Arial;
            border: 1px solid #a2a2a2;
            
        }
        
        .texto tr {
            
            height: 25px;
        }
        
        .texto td {
            
            font-size: 14px;
            color: #ffffff;
        }
        
        h1 {
            color: #EBCB17;
            font-size: 10px;
            display: inline; 
        }
        h2 {
            color: #EBCB17;
            font-size: 12px;
            display: inline; 
        }
    </style>
</head>
<body bgcolor="#efefef">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerPrincipal">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanelPrincipal">
        <ContentTemplate>
            <div id="DivDados" runat="server">
                <br /><br/><br/>
                <asp:UpdateProgress ID="UpdateProgressPrincipal" AssociatedUpdatePanelID="UpdatePanelPrincipal" runat="server">
                    <ProgressTemplate>
                        <center><b><font face="Arial">PROCESSANDO...</font></b></center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table style="background-image: url('/Imagens/bg_form.png'); height: 446px" border="0" cellspacing="0" cellpadding="0" class="texto" align="center" width="595px">
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td colspan="2"><h2>Preencha o formulário abaixo para assistir ao vídeo do FastConsig.</h2></td></tr>
                    <tr>
                        <td style="width: 210px"></td>
                        <td><b>Nome:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxNome"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><b>E-mail:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxEmail"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><b>Empresa/Órgão:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxEmpresa"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><b>Telefone:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxTelefone"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><b>Estado:</b></td>
                        <td><asp:DropDownList EnableViewState="true" Width="158px" runat="server" ID="cmbEstado" DataTextField="Nome" DataValueField="SiglaEstado"></asp:DropDownList></td>
                    </tr>
<%--                    <tr>
                        <td></td>
                        <td><b>Senha:</b></td>
                        <td><asp:TextBox TextMode="Password" runat="server" ID="TextBoxSenha"></asp:TextBox></td>
                    </tr>
--%>                    <tr>
                        <td></td>
                        <td colspan="2" valign="top"><center><asp:Button runat="server" ID="ButtonVerVideo" Text="Acessar" OnClick="ButtonVerVideo_Click" />&nbsp;
                        <%--<asp:Button runat="server" ID="ButtonSolicitarSenha" Text="Solicitar Senha" OnClick="ButtonSolicitarSenha_Click" />--%><br/><br/> <h1>* Todos os campos são obrigatórios.</h1><br/> <%--<h1>* Para solicitar o acesso não é necessário preencher o campo de senha.</h1>--%>
                <br /><br/><h1><asp:Label runat="server" ID="LabelPreencherCampo" Text="" Visible="false"></asp:Label></h1><br /><br/><h1><asp:Label runat="server" ID="Labelerro" Text="" Visible="false"></asp:Label></h1></center></td>
                    </tr>
                </table>
                
            </div>
            <div id="DivVideo" runat="server" visible="False">
                <center>
                    <Bewise:FlashControl ID="FlashControlVideo" runat="server" MovieUrl="~/Video.swf" BrowserDetection="False"
                        Scale="Exactfit" Width="500" Height="395" />
                        <br/>
                        <asp:Button runat="server" Text="Exibir Grande" ID="ButtonTamanhoOriginal" OnClick="ButtonTamanhoOriginal_Click"/>
                        <asp:Button runat="server" Text="Exibir Pequeno" ID="ButtonTamanhoPequeno" OnClick="ButtonTamanhoPequeno_Click"/>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
