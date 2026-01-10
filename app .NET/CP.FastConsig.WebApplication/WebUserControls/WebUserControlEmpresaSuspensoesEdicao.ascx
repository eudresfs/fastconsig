<%@ Control ClassName="WebUserControlEmpresaSuspensoesEdicao" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlEmpresaSuspensoesEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlEmpresaSuspensoesEdicao" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<div class="GlobalUserControl">
    <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
        <tr>
            <td class="TituloNegrito" style="width: 20%;">
                Consignatária:
            </td>
            <td colspan="2">
                <asp:Label runat="server" ID="NomeConsignataria" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito">
                Motivo*:
            </td>
            <td colspan="2">
                <asp:TextBox CssClass="TextBoxDropDownEstilos" Height="80px" TextMode="MultiLine" Rows="8" runat="server"
                    ID="dfMotivo"></asp:TextBox><br/><asp:Label runat="server" Text="*  Obrigatório" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TituloNegrito">
                Situação:
            </td>
            <td colspan="2">
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" AutoPostBack="True" OnSelectedIndexChanged="cmbSituacao_SelectedIndexChanged" Width="332px" runat="server" DataTextField="Nome"
                    DataValueField="IDEmpresaSituacao" ID="cmbSituacao" />
            </td>
        </tr>
        <tr runat="server" id="TrTipoPeriodo">
            <td class="TituloNegrito">
                Tipo Período:
            </td>
            <td colspan="2">
                <asp:DropDownList OnSelectedIndexChanged="cmbTipoPeriodo_SelectedIndexChanged" AutoPostBack="True" CssClass="TextBoxDropDownEstilos" Width="332px" runat="server" ID="cmbTipoPeriodo">
                    <asp:ListItem Text="Indeterminado" Value="I"></asp:ListItem>
                    <asp:ListItem Text="Determinado" Value="D" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="TrPeriodo">
            <td class="TituloNegrito" style="border: none;">
                Período:
            </td>
            <td style="padding:0;border: none;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="border: none;">
                            <dx:ASPxDateEdit AllowNull="False" AllowUserInput="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua" ID="dfDataInicio" runat="server">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: 10px;border: none;">
                            à
                        </td>
                        <td style="border: none;">
                            <dx:ASPxDateEdit AllowNull="False" AllowUserInput="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua" ID="dfDataFim" runat="server">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="border: none; padding: 5px;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="border: none; padding: 0px;">
                            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvarContato" OnClick="ButtonSalvarClick"
                                Text="Salvar" runat="server" />
                        </td>
                        <td style="border: none; padding-left: 4px;">
                            <asp:Button ID="ButtonNovoContato" class="BotaoEstiloGlobal" OnClick="ButtonNovo_Click"
                                runat="server" Text="Novo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
