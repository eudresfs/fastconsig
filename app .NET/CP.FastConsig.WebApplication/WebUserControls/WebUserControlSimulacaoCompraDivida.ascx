<%@ Control ClassName="WebUserControlSimulacaoCompraDivida" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlSimulacaoCompraDivida.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlSimulacaoCompraDivida" %>
<%@ Register TagPrefix="uc3" TagName="WebUserControlChartBarra" Src="~/WebUserControls/WebUserControlChartBarra.ascx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<div class="compraDivida" style="height: 100%; clear: both; overflow: hidden;">
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel4" HeaderStyle-Font-Bold="true"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
        Paddings-PaddingBottom="10px" ContentPaddings-PaddingTop="10px" ContentPaddings-PaddingRight="0px"
        CssPostfix="Aqua" HeaderText="Simulação de Compra Dívida" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                <asp:Panel runat="server" ID="PanelObjetivo" CssClass="compraDivida">
                    <asp:Label runat="server" ID="LabelObjetivo" Text="Meu objetivo é: "></asp:Label>
                    <asp:DropDownList Enabled="false" CssClass="TextBoxDropDownEstilosCompraDivida" AutoPostBack="true"
                        OnSelectedIndexChanged="DropDownListOpcao_SelectedIndexChanged" runat="server"
                        ID="DropDownListOpcao">
                        <asp:ListItem Value="0">Obter Mais Dinheiro</asp:ListItem>
                        <asp:ListItem Value="1">Regularizar Minha Margem</asp:ListItem>
                        <asp:ListItem Value="2">Diminuir Valor Parcelas</asp:ListItem>
                        <asp:ListItem Value="3">Diminuir Quantidade de Parcelas</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <asp:UpdatePanel runat="server" ID="UpdatePanelAverbacaos">
        <ContentTemplate>
            <div class="DivFlutuaEsquerda">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelParametros" HeaderStyle-Font-Bold="true"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
                    ContentPaddings-PaddingBottom="95px" ContentPaddings-PaddingTop="30px" ContentPaddings-PaddingRight="0px"
                    CssPostfix="Aqua" HeaderText="Averbações Ativas" GroupBoxCaptionOffsetY="-28px"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentParametros" runat="server" Height="270px" Width="300px" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView EnableCallBacks="false"
                                runat="server" Font-Size="10px" ID="ASPxGridViewAverbacaosAtivos" OnRowCommand="ASPxGridViewAverbacaosAtivos_RowCommand" ClientInstanceName="ASPxGridViewAverbacaosAtivos"
                                Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                OnSelectionChanged="ASPxGridViewAverbacaosAtivos_SelectionChanged" KeyFieldName="IDAverbacao"
                                CssPostfix="Aqua">
                                <Columns>
<%--                                    <dx:GridViewDataColumn Settings-AllowSort="false" VisibleIndex="0">
                                        <DataItemTemplate>
                                            <dx:ASPxCheckBox ValueType="System.String" ValueChecked='<%#Container.VisibleIndex + ";" + Eval("ParcelaAtual") + ";" + Eval("SaldoRestante") + ";" + Eval("ValorParcela") %>'
                                                ID="ASPxCheckBoxSelecionar" OnCheckedChanged="ASPxCheckBoxSelecionar_CheckedChanged"
                                                runat="server" Text="" AutoPostBack="true" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
--%>                                
									<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Name="gridSelecionar">
										<HeaderTemplate>
											<dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" ClientSideEvents-CheckedChanged="function(s, e) { ASPxGridViewAverbacaosAtivos.SelectAllRowsOnPage(s.GetChecked()); }" />
										</HeaderTemplate>
										<HeaderStyle HorizontalAlign="Center" />
									</dx:GridViewCommandColumn>
									<dx:GridViewDataColumn VisibleIndex="0">
										<DataItemTemplate>
											<asp:LinkButton runat="server" ID="select" CommandName="Select">
												<img style="padding-top:4px;" src="/Imagens/SolicitarSaldoDevedor.png" width="16" height="16" alt="Solicitar Saldo Devedor" title="Solicitar Saldo Devedor"  />
											</asp:LinkButton>
										</DataItemTemplate>
									</dx:GridViewDataColumn>
									<dx:GridViewDataColumn VisibleIndex="0">
										<DataItemTemplate>
											<asp:LinkButton runat="server" ID="select" CommandName="Edit">
												<img style="padding-top:4px;" src="/Imagens/InformarSaldoDevedor.png" width="16" height="16" alt="Informar Saldo Devedor Provisório" title="Informar Saldo Devedor Provisório"  />
											</asp:LinkButton>
										</DataItemTemplate>
									</dx:GridViewDataColumn>    
                                    <dx:GridViewDataColumn Settings-AllowSort="false" FieldName="Empresa1.Nome" VisibleIndex="1" Caption="Consignatária" />
                                    <dx:GridViewDataTextColumn Settings-AllowSort="false" FieldName="ValorParcela" VisibleIndex="2" Caption="Valor Parcela">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn Width="10%" Settings-AllowSort="false" Caption="Parc. Restantes"
                                        FieldName="PrazoRestante" VisibleIndex="3" />
                                    <dx:GridViewDataTextColumn Settings-AllowSort="false" Caption="Saldo Bruto" FieldName="SaldoRestante"
                                        VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewBandColumn Caption="Saldo Devedor">
                                        <Columns>
                                            <dx:GridViewDataColumn Settings-AllowSort="false" FieldName="SaldoDevedorData" Caption="Data" VisibleIndex="5" />
                                            <dx:GridViewDataTextColumn Settings-AllowSort="false" FieldName="SaldoDevedorValor" Caption="Valor" VisibleIndex="6">
                                                <PropertiesTextEdit DisplayFormatString="c" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                </Columns>
                                <Settings ShowFooter="True" />
                                <TotalSummary>
                                    <dx:ASPxSummaryItem FieldName="Parcela" SummaryType="Sum" />
                                    <dx:ASPxSummaryItem FieldName="SaldoBruto" SummaryType="Sum" />
                                    <dx:ASPxSummaryItem FieldName="Valor" SummaryType="Sum" />
                                </TotalSummary>
                                <ClientSideEvents SelectionChanged="function(s, e) {  ASPxGridViewAverbacaosAtivos.PerformCallback(); }" />

                                <SettingsLoadingPanel ImagePosition="Top" />
                                <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                                    </LoadingPanelOnStatusBar>
                                    <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                                    </LoadingPanel>
                                </Images>
                                <ImagesEditors>
                                    <DropDownEditDropDown>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                    </DropDownEditDropDown>
                                    <SpinEditIncrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                    </SpinEditIncrement>
                                    <SpinEditDecrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                    </SpinEditDecrement>
                                    <SpinEditLargeIncrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                    </SpinEditLargeIncrement>
                                    <SpinEditLargeDecrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                    </SpinEditLargeDecrement>
                                </ImagesEditors>
                                <ImagesFilterControl>
                                    <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                    </LoadingPanel>
                                </ImagesFilterControl>
                                <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                    <LoadingPanel ImageSpacing="8px">
                                    </LoadingPanel>
                                </Styles>
                                <StylesEditors>
                                    <CalendarHeader Spacing="1px">
                                    </CalendarHeader>
                                    <ProgressBar Height="25px">
                                    </ProgressBar>
                                </StylesEditors>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <div class="DivFlutuaDireita">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel1" HeaderStyle-Font-Bold="true"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
                    ContentPaddings-PaddingBottom="20px" ContentPaddings-PaddingTop="20px" ContentPaddings-PaddingRight="0px"
                    CssPostfix="Aqua" HeaderText="Gráfico de Análise de Negociação" GroupBoxCaptionOffsetY="-28px"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" Height="270px" SupportsDisabledAttribute="True">
                            <uc3:WebUserControlChartBarra ID="WebUserControlChartBarraSimulacaoEmprestimo" runat="server" />
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <div style="clear: both; overflow: hidden; height: 5px;">
                &nbsp;</div>
            <div class="DivFlutuaEsquerda">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel2" HeaderStyle-Font-Bold="true"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
                    ContentPaddings-PaddingBottom="20px" ContentPaddings-PaddingTop="35px" ContentPaddings-PaddingRight="0px"
                    CssPostfix="Aqua" HeaderText="Resumo de Simulação de Negociação" GroupBoxCaptionOffsetY="-28px"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" Height="270px" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView OnHtmlRowCreated="ASPxGridViewSimulacaoNegociacao_HtmlRowCreated"
                                runat="server" Font-Size="10px" ID="ASPxGridViewSimulacaoNegociacao" ClientInstanceName="aSPxGridViewSimulacaoNegociacao"
                                Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua">
                                <Columns>
                                    <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Ranking" FieldName="Ranking"
                                        VisibleIndex="0" />
                                    <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Banco" FieldName="Banco"
                                        VisibleIndex="1" />
                                    <dx:GridViewDataTextColumn Settings-AllowSort="false" Caption="Valor da Parcela" FieldName="ValorParcela"
                                        VisibleIndex="2">
                                        <PropertiesTextEdit DisplayFormatString="N" />
                                        </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Settings-AllowSort="false" Caption="Valor da Averbação" FieldName="ValorAverbacao"
                                        VisibleIndex="3" >
                                        <PropertiesTextEdit DisplayFormatString="N" />
                                        </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Settings-AllowSort="false" Caption="Troco" FieldName="Troco" 
                                        VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="N" />                                       
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Prazo" FieldName="Prazo"
                                        VisibleIndex="5" />
                                    <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Viabilidade" VisibleIndex="6">
                                        <DataItemTemplate>
                                            <asp:Image runat="server" ID="ImageViabilidade" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                </Columns>
                                <SettingsLoadingPanel ImagePosition="Top" />
                                <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                                    </LoadingPanelOnStatusBar>
                                    <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                                    </LoadingPanel>
                                </Images>
                                <ImagesEditors>
                                    <DropDownEditDropDown>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                    </DropDownEditDropDown>
                                    <SpinEditIncrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                    </SpinEditIncrement>
                                    <SpinEditDecrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                    </SpinEditDecrement>
                                    <SpinEditLargeIncrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                    </SpinEditLargeIncrement>
                                    <SpinEditLargeDecrement>
                                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                            PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                    </SpinEditLargeDecrement>
                                </ImagesEditors>
                                <ImagesFilterControl>
                                    <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                    </LoadingPanel>
                                </ImagesFilterControl>
                                <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                    <LoadingPanel ImageSpacing="8px">
                                    </LoadingPanel>
                                </Styles>
                                <StylesEditors>
                                    <CalendarHeader Spacing="1px">
                                    </CalendarHeader>
                                    <ProgressBar Height="25px">
                                    </ProgressBar>
                                </StylesEditors>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <div class="DivFlutuaDireita">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel3" HeaderStyle-Font-Bold="true"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px"
                    ContentPaddings-PaddingBottom="98px" ContentPaddings-PaddingTop="20px" ContentPaddings-PaddingRight="0px"
                    CssPostfix="Aqua" HeaderText="Margens" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%" border="0" cellpadding="3" cellspacing="3">
                                <tr>
                                    <td style="width: 20%;">
                                        Margem Disponível Atual:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxMargemDisponivelAtual"
                                            Enabled="false" runat="server" Width="150">
                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Parcelas Para Negociar:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxParcelasParaNegociar"
                                            Enabled="false" runat="server" Width="150">
                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Margem Para Negociação:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" AutoPostBack="true" OnTextChanged="ASPxTextBoxMargemParaNegociacao_TextChanged"
                                            ID="ASPxTextBoxMargemParaNegociacao" runat="server" Width="150">
                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<h1 class="TextoAncora">
    <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
        Topo da Página</a>
</h1>
<div style="height: 5px;">
    &nbsp;
</div>
