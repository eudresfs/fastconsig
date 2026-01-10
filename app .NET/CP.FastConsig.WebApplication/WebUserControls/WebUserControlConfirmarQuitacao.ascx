<%@ Control ClassName="WebUserControlConfirmarQuitacao" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlConfirmarQuitacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlConfirmarQuitacao" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="cc1" Namespace="PdfViewer" Assembly="PdfViewer" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
                    <tr>
                        <td style="width: 150px">
                            <b>Matrícula:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Nome:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>CPF:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelCpfFuncionario"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<br />
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosAverbacao" Width="100%"
        HeaderText="Dados da Averbação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
                    <tr>
                        <td style="width: 150px">
                            <b>Número:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNumeroAverbacao"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Consignatária:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelConsignataria"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Prazo:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelPrazo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Valor Parcela:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Situação:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSituacaoAtual"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button OnClientClick="return false;" ClientIDMode="Static" CssClass="BotaoEstiloGlobal"
                                runat="server" ID="ButtonVisualizarComprovanteQuitacao" Text="Visualizar Comprovante de Quitação" />
                            <dx:ASPxPopupControl BackColor="#e7f6fa" LoadingPanelText="Carregando..." ID="PopupControlComprovante"
                                runat="server" CloseAction="OuterMouseClick" PopupElementID="ButtonVisualizarComprovanteQuitacao"
                                PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" AllowDragging="False"
                                ForeColor="#ffffff" Font-Bold="true" Width="400px" HeaderText="Comprovante" ClientInstanceName="popupControlNotificacoes">
                                <HeaderStyle ForeColor="#ffffff" Font-Bold="true" CssClass="AlturaCabecalhoPopControl" />
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControlComprovante" runat="server">
                                        <asp:Image runat="server" ID="ImageComprovante" Visible="false" />
                                        <cc1:ShowPdf Visible="false" ID="ShowPdfComprovante" runat="server" BorderStyle="Inset"
                                            BorderWidth="2px" Height="250px" Width="650px" />
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<br />
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel2" Width="100%" HeaderText="Saldo Devedor"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            Informado em:
                        </td>
                        <td class="TituloNegrito">
                            <asp:Label runat="server" ID="LabelInformadoEm"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Data de Validade:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelDataValidade"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Forma de Pagamento:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelFormaPagamento"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Telefone:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelTelefone"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Observação:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelObservacao"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Saldo em Aberto:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSaldoAberto"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Desconto:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelDesconto"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ececec">
                        <td class="TituloNegrito">
                            Saldo Devedor:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelSaldoDevedor"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<br />
<br />
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel1" Width="100%" HeaderText="Liquidação"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            Motivo:
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxMotivo"
                                TextMode="MultiLine" Rows="5" Width="300px" Height="120px"></asp:TextBox>
                            <span class="EstiloCampoObrigatorio">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="TituloCamposObrigatorios">*Campo Obrigatório</span>
                        </td>
                    </tr>
                </table>
                <div style="margin: 5px 0px;">
                    <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="ButtonConfirmarQuitacao"
                        OnClick="ButtonConfirmarQuitacao_Click" Text="Confirmar Quitação" />
                    <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="ButtonRejeitarQuitacao"
                        OnClick="ButtonRejeitarQuitacao_Click" Text="Rejeitar Quitação" />
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
