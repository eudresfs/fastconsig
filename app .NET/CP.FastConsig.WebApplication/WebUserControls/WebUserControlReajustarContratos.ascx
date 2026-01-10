<%@ Control ClassName="WebUserControlReajustarContratos" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlReajustarContratos.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlReajustarContratos" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelSimulacao" ShowHeader="true"
            Width="100%" HeaderStyle-Font-Bold="true" HeaderText="Central de Simulação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <PanelCollection>
                <dx:PanelContent ID="PanelContentSimulacao" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td style="width: 120px"><b>Data:</b></td>
                            <td><asp:Label runat="server" ID="LabelDataAtual"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Descrição:</b></td>
                            <td><asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxDescricao"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><b>Serviço:</b></td>
                            <td><asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" ID="DropDownListServico"/></td>
                        </tr>
                        <tr>
                            <td><b>Arquivo:</b></td>
                            <td><asp:FileUpload runat="server" ID="FileUploadArquivoReajuste"/>&nbsp;<a href="#">(baixe o layout aqui)</a></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonAplicarReajuste" Text="Aplicar Reajuste" runat="server" OnClick="ButtonAplicarReajuste_Click" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
</div>
<div>&nbsp;</div>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelAnaliseReajuste" Visible="false" ShowHeader="true"
            Width="100%" HeaderStyle-Font-Bold="true" HeaderText="Central de Simulação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table>
                        <tr>
                            <td style="text-align:left">
                                <b>Confirma aplicação do Reajuste?</b><br/><br/>
                                Contratos do Serviço: <b><asp:Label ID="LabelProduto" runat="server" Text=""></asp:Label></b><br/><br/>

                                10 contratos se enquadram no filtro<br/>
                                08 contratos serão reajustados<br/>
                                02 contratos não serão reajustados por falta de margem<br/><br/>

                                R$ 3.221,25 somam os contratos hoje<br/>
                                R$ 3.780,30 passará a somar após o reajuste<br/>
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td style="text-align:left">
                                <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonConfirmar" OnClick="ButtonConfirmar_Click" Text="Confirmar" runat="server" />&nbsp;<asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonCancelar" OnClick="ButtonCancelar_Click" Text="Cancelar" runat="server"/><br/>
                                <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonDownloadCriticas" OnClick="ButtonDownloadCriticas_Click" Text="Ver Críticas" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr runat="server" id="TrResultado" Visible="false">
                            <td style="text-align: left">
                                13414;3011;117.24<br/>
                                56705;2222;247.23<br/>
                                63338;1159;567.14 - Insuficiência de Margem para o Funcionário<br/>
                                83269;9870;197.88<br/>
                                32571;8448;067.90<br/>
                                21692;5687;060.00<br/>
                                20120;7701;098.34<br/>
                                90041;7913;087.53<br/>
                                59974;2025;160.76<br/>
                                47886;3296;264.92 - Insuficiência de Margem para o Funcionário<br/>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
</div>