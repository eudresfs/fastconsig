<%@ Control Language="C#" ClassName="WebUserControlChartVolumeAverbacoes" AutoEventWireup="true"
    CodeBehind="WebUserControlChartVolumeAverbacoes.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlChartVolumeAverbacoes" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelVolumeAverbacoes" HeaderStyle-Font-Bold="true"
    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
    ContentPaddings-PaddingBottom="10px" ContentPaddings-PaddingTop="5px" ContentPaddings-PaddingRight="0px"
    CssPostfix="Aqua" HeaderText="Gráfico" Height="270px" GroupBoxCaptionOffsetY="-28px"
    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
    <PanelCollection>
        <dx:PanelContent ID="PanelContentVolumeAberbacoes" runat="server" Height="470px"
            SupportsDisabledAttribute="True">
            <div style="float: left; margin-bottom: 10px; padding-top: 5px">
                <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabelaChatVolumeAverbacoes">
                    <tr>
                        <td align="right">
                            <asp:Label ID="LabelMeses" runat="server" Text="Últimos:" Width="70" Style="text-align: right;
                                margin-right: 3px"></asp:Label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" class="WebUserControlTabelaChatVolumeAverbaocesSpinMeses">
                                <tr>
                                    <td>
                                        <dx:ASPxSpinEdit ID="ASPxSpinEditMeses" ButtonStyle-Paddings-Padding="0px" Height="25px"
                                            Border-BorderColor="#c0dfe8" Border-BorderStyle="Solid" BorderLeft-BorderWidth="1px"
                                            Paddings-Padding="0px" runat="server" Number="6" MaxValue="12" Width="40" Style="float: left" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=" Meses" Width="70" Style="text-align: left;
                                            float: left; margin-right: 3px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelTipo" runat="server" Text="Tipo do Volume:" Width="100" Style="text-align: right;
                                float: left; margin-right: 3px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListVolume" CssClass="TextBoxDropDownEstilos" EnableViewState="true"
                                runat="server" Width="200px" Style="float: left; height: 25px; padding: 3px">
                                <asp:ListItem Value="1" Selected="True" Text="Volume Total de Averbações"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Volume de Parcelas Mensais"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Produto:" Width="100" Style="text-align: right;
                                float: left; margin-right: 3px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListProduto" CssClass="TextBoxDropDownEstilos" EnableViewState="true"
                                runat="server" Width="200px" Style="float: left; height: 25px; padding: 3px" DataValueField="IDProdutoGrupo" DataTextField="Nome">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="ButtonAplicar" Text="Aplicar" runat="server" CssClass="BotaoEstiloGlobal"
                                OnClick="atualizaGrafico_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="containerBarra" style="height: 410px; margin: 0 2em; clear: both">
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
