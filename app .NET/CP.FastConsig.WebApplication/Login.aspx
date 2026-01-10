<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CP.FastConsig.WebApplication.Login" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50731.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxDataView" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html id="BG" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Estilos/EstilosMasterPage.css" type="text/css" rel="Stylesheet" />
    <link href="Estilos/EstilosFormularios.css" type="text/css" rel="Stylesheet" />
    <link href="Estilos/EstilosGerais.css" type="text/css" rel="Stylesheet" />
    <link href="Estilos/EstilosLogin.css" type="text/css" rel="Stylesheet" />
    <link href="Estilos/EstilosWebUserControls.css" type="text/css" rel="Stylesheet" />
    <link href="Estilos/redmond/jquery-ui-1.8.16.custom.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.3.min.js"></script>
    <script type="text/javascript" src="jquery-ui-1.8.16.custom.min.js"></script>
    <script type="text/javascript" src="Scripts/reflection.js"></script>
    <title>.: FastConsig - Visibilidade e Controle na Gestão do Crédito Consignado :.
    </title>
    <script type="text/javascript" language="javascript">

        function AtivaScroll() {
            document.getElementById('BodyLogin').setAttribute('style', 'overflow:auto');
        }

        function pageLoad() {

            if (screen.width > 800)
                trResolucao.setAttribute('style', 'display: none;');

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);

        }

        $(document).ready(function () {
            AtivaScroll();
        });

        function onEndRequest(sender, args) {

            AtivaScroll();

            var error = args.get_error();

            if (error != undefined && error.message != "") {

                if (error.message.toUpperCase().indexOf("THE STATUS CODE RETURNED FROM THE SERVER WAS") >= 0) window.location = "http://" + document.location.host + "/Login";
                else if (error.message.toUpperCase().indexOf("COULD NOT LOAD FILE OR ASSEMBLY") >= 0) window.location = "http://www.fastconsig.com.br";

                var diverro = document.getElementById("DivMensagemErro");

                diverro.innerHTML = error.message;
                diverro.setAttribute("style", "height: 200px; overflow-y:auto; display: none;");

                var mensag = document.getElementById("DivMensagemUsuario");

                mensag.setAttribute("style", "display:;");

                $("#dialog").dialog({
                    autoOpen: false,
                    show: "blind",
                    hide: "puff",
                    modal: true
                });

                $("#dialog").dialog("open");

                args.set_errorHandled(true);

            }

        }

        function DetalheDoErro(sender, args) {

            var diverro = document.getElementById("DivMensagemErro");

            if (diverro.getAttribute("style") == "height: 200px; overflow-y:auto; display: none;") {
                diverro.setAttribute("style", "height: 200px; overflow-y:auto; display: block;");
            }
            else {
                diverro.setAttribute("style", "height: 200px; overflow-y:auto; display: none;");
            }

        }

        jQuery(function ($) {
            $("img.reflect").reflect();
        });

    </script>
</head>
<body id="BodyLogin">
    <div id="dialogMensagem" title="Atenção" style="display: none;">
    </div>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hfWidth" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hfHeight" ClientIDMode="Static" />
    <asp:ScriptManager runat="server" ID="ScriptManagerLogin" OnAsyncPostBackError="ScriptManagerLogin_AsyncPostBackError">
    </asp:ScriptManager>
    <div>
        <div id="dialog" title="ERRO!">
            <div id="DivMensagemUsuario" style="display: none;">
                <h3>
                    Ocorreu uma interrupção inesperada no sistema. Um email foi enviado para o administrador.</h3>
                <br />
                <br />
                <a href="javascript:DetalheDoErro()">Clique aqui para mais detalhes</a><br />
                <br />
            </div>
            <div id="DivMensagemErro" style="height: 200px; overflow-y: auto; display: none;">
            </div>
        </div>
        <div id="TopoLogin">
            <div id="LogoMarcaControles">
                <div id="BoxLogomarca">
                    <a id="LogomarcaLogin"></a>
                </div>
                <div id="Controles">
                    <div id="DivLogin" runat="server">
                        <div id="BoxLogin">
                            <span class="TextoLoginSenha">Login: </span>
                            <br />
                            <div id="TextBoxDivLogin">
                                <asp:TextBox runat="server" MaxLength="15" ID="TextBoxLogin" name="textfield" class="TextBoxLogin"
                                    type="text" value="" size="15"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="TextBoxLogin"
                                    WatermarkText="Digite seu Login" runat="server" />
                            </div>
                        </div>
                        <div id="BoxSenha">
                            <span class="TextoLoginSenha">Senha: </span>
                            <br />
                            <div id="TextBoxDivSenha">
                                <asp:TextBox runat="server" MaxLength="25" ID="TextBoxSenha" name="textfield" TextMode="Password"
                                    class="TextBoxSenha" type="text" size="30"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="TextBoxSenha"
                                    WatermarkText="Digite sua Senha" runat="server" />
                            </div>
                        </div>
                        <div id="BoxEntrar">
                            <dx:ASPxButton ID="ButtonEntrar" OnClick="ButtonEntrar_Click" HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BotaoHumOver.png"
                                BackgroundImage-ImageUrl="~/Imagens/BotaoHum.png" Width="60" ForeColor="#ffffff"
                                Font-Size="12px" Height="30" EnableDefaultAppearance="false" Cursor="pointer"
                                Font-Bold="true" EnableTheming="false" runat="server" Text="Entrar">
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <div id="DivLogado" runat="server" visible="false" style="color: #ffffff;">
                        <div style="float: right;">
                            <div style="float: left; padding-right: 7px; height: 70px; margin-top: 41px;">
                                <asp:Image ID="ImageFotoUsuario" BorderStyle="Ridge" BorderWidth="1px" runat="server"
                                    ImageUrl="~/Imagens/PerfilLogin.png" Width="32px" Height="32px" /></div>
                            <div style="line-height: 111px; float: left; height: 111px; padding-right: 7px; font-size: 12px;
                                font-weight: bold;">
                                <asp:Label ID="LabelNomeUsuarioLogado" runat="server"></asp:Label></div>
                            <div style="height: 50px; float: left; margin-top: 43px;">
                                <dx:ASPxButton Cursor="Pointer" ID="ButtonTrocarUsuario" OnClick="ButtonTrocarUsuario_Click"
                                    HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BotaoHumOver.png" BackgroundImage-ImageUrl="~/Imagens/BotaoHum.png"
                                    Width="60" ForeColor="#ffffff" Font-Size="11px" Height="30" EnableDefaultAppearance="false"
                                    Font-Bold="true" EnableTheming="false" runat="server" Text="Trocar">
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                    <dx:ASPxPopupControl Width="400px" BackColor="#e7f6fa" ID="ASPxPopupControlPrimeiroAcesso"
                        runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" ClientInstanceName="aSPxPopupControlPrimeiroAcesso"
                        HeaderText="Primeiro Acesso" Font-Bold="false" AllowDragging="True" EnableAnimation="True"
                        EnableViewState="True">
                        <HeaderStyle ForeColor="#ffffff" Font-Bold="true" CssClass="AlturaCabecalhoPopControl" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControlPrimeiroAcesso" runat="server">
                                <asp:Panel runat="server" ID="PanelPrimeiroLogin" DefaultButton="ButtonEntrarPrimeiroAcesso">
                                    <table width="100%" cellpadding="10" cellspacing="2">
                                        <tr>
                                            <td colspan="2">
                                                Este é o seu primeiro acesso. Você deve obrigatoriamente alterar a senha provisória,
                                                e opcionalmente alterar seu apelido:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                               <span style="color:Red;">*</span> Senha:
                                            </td>
                                            <td>
                                                <asp:TextBox TabIndex="2" Width="200" CssClass="TextBoxDropDownEstilos" TextMode="Password"
                                                    runat="server" ID="TextBoxSenhaPrimeiroAcesso"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <span style="color:Red;">*</span> Repetir:
                                            </td>
                                            <td>
                                                <asp:TextBox TabIndex="3" Width="200" CssClass="TextBoxDropDownEstilos" TextMode="Password"
                                                    runat="server" ID="TextBoxSenhaPrimeiroAcessoaRepeticao"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Apelido/Login:
                                            </td>
                                            <td>
                                                <asp:TextBox TabIndex="1" Width="200" CssClass="TextBoxDropDownEstilos" runat="server"
                                                    ID="TextBoxApelidoLoginPrimeiroAcesso"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:center;">
                                            <span style="color:Red;font-size:10px;">*Campos Obrigatórios</span>
                                            </td>
                                            <td style="text-align: right; padding-right: 64px;">
                                                <asp:Button TabIndex="4" class="BotaoEstiloGlobal" Width="80" ID="ButtonEntrarPrimeiroAcesso"
                                                    OnClick="ButtonEntrarPrimeiroAcesso_Click" runat="server" Text="Entrar" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <dx:ASPxPopupControl Width="400px" BackColor="#e7f6fa" ID="ASPxPopupControlLoginFastConsig"
                        runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" ClientInstanceName="aSPxPopupControlLoginFastConsig"
                        HeaderText="FastConsig" Font-Bold="false" AllowDragging="True" EnableAnimation="True"
                        EnableViewState="True">
                        <HeaderStyle ForeColor="#ffffff" Font-Bold="true" CssClass="AlturaCabecalhoPopControl" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="ButtonLoginFastConsig">
                                    <table width="100%" cellpadding="10" cellspacing="2">
                                        <tr>
                                            <td colspan="2">
                                                Indique com qual matrícula você deseja acessar caso o perfil seja de funcionário
                                                e com qual banco você deseja acessar caso o perfil seja de consignatária / correspondente:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Matrícula:
                                            </td>
                                            <td>
                                                <asp:TextBox TabIndex="1" Width="200" CssClass="TextBoxDropDownEstilos" runat="server"
                                                    ID="TextBoxMatriculaLoginFastConsig"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Banco:
                                            </td>
                                            <td>
                                                <asp:DropDownList TabIndex="2" DataTextField="Fantasia" DataValueField="IDEmpresa"
                                                    CssClass="TextBoxDropDownEstilos" Width="205" runat="server" ID="DropDownListBancosLoginFastConsig">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button TabIndex="4" class="BotaoEstiloGlobal" Width="80" ID="ButtonLoginFastConsig"
                                                    OnClick="ButtonLoginFastConsig_Click" runat="server" Text="Exibir Perfis" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </div>
            </div>
        </div>
        <div id="BoxSplashLogin">
            <dx:ASPxRoundPanel Visible="true" Width="98%" CssClass="RoundPanelConteudoLogin"
                EnableTheming="True" ShowHeader="true" HeaderStyle-Font-Bold="true" HeaderText="Escolha seu perfil de acesso clicando em uma das opções abaixo:"
                ID="DivLoginConteudo2" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <ContentPaddings Padding="14px" />
                <PanelCollection>
                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                        <div id="DivLoginNormal" runat="server" visible="false">
                            <asp:GridView OnRowDataBound="GridViewPerfis_RowDataBound" OnSelectedIndexChanged="GridViewPerfis_SelectedIndexChanged"
                                CssClass="EstilosGridView" Width="100%" AutoGenerateColumns="false" DataKeyNames="IDUsuarioPerfil, Matricula, IDEmpresa, IDPerfil"
                                runat="server" ID="GridViewPerfis">
                                <HeaderStyle CssClass="CabecalhoGridView" />
                                <RowStyle CssClass="LinhaListaGridView" />
                                <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
                                <PagerStyle CssClass="PaginadorGridView" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Instituição" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <%# Eval("Empresa") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Matrícula" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <%# Eval("Matricula") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perfil de Acesso" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <%# Eval("Nome") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Módulo" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <%# Eval("Modulo") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn" HeaderStyle-CssClass="HiddenColumn" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <dx:ASPxRoundPanel Visible="false" Width="98%" CssClass="RoundPanelConteudoLogin"
                EnableTheming="True" ShowHeader="true" HeaderStyle-Font-Bold="true" HeaderText="Painel de Acesso"
                ID="DivLoginBanco" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <ContentPaddings Padding="14px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxDataView Cursor="Pointer" EmptyDataText="Sem clientes para exibição!" HideEmptyRows="false"
                            ID="ASPxDataViewEstados" runat="server" ColumnCount="5" RowPerPage="2" Width="100%">
                            <ItemTemplate>
                                <div style="width: 100%">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding: 5px 0px;">
                                                <img id="ImageBanco" src="" visible="false" runat="server" alt="BANCO" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton CommandArgument='<%# Eval("URL") %>' ID="ImageButtonCliente" OnClick="ImageButtonCliente_Click"
                                                                runat="server" ImageUrl='<%# "~/Imagens/"+Eval("Logo") %>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                            <PagerSettings>
                                <AllButton Visible="True">
                                </AllButton>
                            </PagerSettings>
                        </dx:ASPxDataView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <div id="BoxFamiliaSlogan" style="padding-top: 35px;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="middle" style="width: 50%; text-align: center;">
                            <img id="ImageBanco" src="" visible="false" runat="server" alt="BANCO" />
                        </td>
                        <td valign="middle" style="width: 50%; text-align: center;">
                            <img id="ImageSlogan" src="~/Imagens/TextoSlogan.png" visible="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="RodapeLogin">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="middle" style="width: 50%; text-align: center;">
                            <img id="ImageFamiliaLogin" src="~/Imagens/FamiliaLogin.png" visible="true" runat="server"
                                alt="BANCO" />
                        </td>
                        <td valign="middle" style="width: 50%;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <td style="width: 270px;">
                                    <asp:Label ForeColor="#ffffff" runat="server" ID="LabelErro" Text=""></asp:Label>
                                </td>
                                <td style="width: 232px; padding-top: 15px;">
                                    <a href="http://www.casepartners.com.br" id="LogoCasePartnersRodape"></a>
                                </td>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="trResolucao">
                        <td colspan="2" style="text-align: center; padding: 20px 0px;">
                            <h1 style="color: White; font-size: 11pt; font-weight: normal;">
                                Este sistema é melhor visualizado na resolução 1024x768</h1>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="RodapeChatSuporte">
            <div id="IconeSuporteRodapeLogin">
                <a href="http://www.milldesk.com/flex/milldesk/index.html#empresa=case&cor=E49800"
                    target="_blank">
                    <img src="Imagens/IconeSuporte.png" alt="Suporte FastConsig" title="Suporte FastConsig"
                        width="24" height="24" />
                </a>
            </div>
            <div id="DivZopimChat" runat="server">
                <script type="text/javascript">
                    window.$zopim || (function (d, s) {
                        var z = $zopim = function (c) { z._.push(c) }, $ = z.s =
d.createElement(s), e = d.getElementsByTagName(s)[0]; z.set = function (o) {
    z.set.
_.push(o)
}; z._ = []; z.set._ = []; $.async = !0; $.setAttribute('charset', 'utf-8');
                        $.src = '//cdn.zopim.com/?fXMbYpMrFNDD1fF4FRFz4kvTaiHsER3p'; z.t = +new Date; $.
type = 'text/javascript'; e.parentNode.insertBefore($, e)
                    })(document, 'script');
                </script>
                <!--End of Zopim Live Chat Script-->
                <script type="text/javascript">                    $zopim(function () {
                        $zopim.livechat.setLanguage('pt_BR');
                    });</script>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
