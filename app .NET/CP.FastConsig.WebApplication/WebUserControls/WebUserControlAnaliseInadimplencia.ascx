<%@ Control Language="C#" ClassName="WebUserControlAnaliseInadimplencia" AutoEventWireup="true"
    CodeBehind="WebUserControlAnaliseInadimplencia.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAnaliseInadimplencia" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartAreaVolumeInadimplencia.ascx" TagName="WebUserControlChartAreaVolumeInadimplencia"
    TagPrefix="uc4" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartPizzaInadimplenciaGeral.ascx" TagName="WebUserControlChartPizzaInadimplenciaGeral"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<div>
    <table>
        <tr>
            <td><asp:Label ID="LabelPeriodo" runat="server" Text="Competência:"></asp:Label></td>
            <td><asp:Label ID="LabelCompetencia" runat="server" Visible="false"></asp:Label></td>        
            <td valign="middle">
                                <asp:Button ID="ButtonPeriodoAnterior" runat="server" Text="<<" CssClass="BotaoEstiloGlobal"
                                    OnClick="selecionaPeriodoAnterior_Click" />&nbsp;
                                <asp:Button ID="ButtonPeriodoProximo" runat="server" Text=">>" CssClass="BotaoEstiloGlobal"
                                    OnClick="selecionaPeriodoProximo_Click" />
            </td>

        </tr>
    </table>
    <table>
        <tr>
            <td><asp:Label ID="LabelConsignataria" runat="server" Text="Consignatária:"></asp:Label></td>
            <td colspan="2"><asp:DropDownList ID="DropDownListConsignataria" runat="server" CssClass="TextBoxDropDownEstilos" 
                                DataTextField="Nome" DataValueField="IDEmpresa" AutoPostBack="true" Width="350" OnSelectedIndexChanged="DropDownListConsignataria_SelectedIndex"></asp:DropDownList></td>
        </tr>
    </table>
</div>
<div style="float: left; width: 50%; margin-top: 1%; clear: both;">
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelGraficoInadimplenciaGeral" ShowHeader="true" 
        Width="100%" ContentPaddings-PaddingBottom="10px" ContentPaddings-PaddingLeft="0px"
        ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="5px" HeaderText="Gráfico de Inadimplência Geral"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentInadimplenciaGeral" runat="server" SupportsDisabledAttribute="True">
                <uc1:WebUserControlChartPizzaInadimplenciaGeral ID="WebUserControlChartPizzaInadimplenciaGeral"
                    runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div style="float: right; width: 49%; margin-left: 0%; margin-top: 1%;">
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelGraficoVolumeInadimplencia" ShowHeader="true"  HeaderStyle-Font-Bold="true"
        Width="100%" ContentPaddings-PaddingBottom="7px" ContentPaddings-PaddingLeft="0px"
        ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="12px" HeaderText="Histórico da Inadimplência"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentVolumeInadimplencia" runat="server" SupportsDisabledAttribute="True">
                <uc4:WebUserControlChartAreaVolumeInadimplencia ID="WebUserControlChartAreaVolumeInadimplencia"
                    runat="server" />
                <div>                  
                    <asp:RadioButtonList RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="5" CellPadding="2" ID="RadioButtonListTipoValor" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="RadioButtonListTipoValor_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1">Volume</asp:ListItem>
                        <asp:ListItem Value="2">Percentual</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div style="height:8px;clear:both;overflow:hidden;">&nbsp;</div>

<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelPadraoInadimplencia" ShowHeader="true" 
        Width="100%" HeaderText="Padrão de Inadimplência" ContentPaddings-PaddingBottom="0px"
        ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="0px"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentPadraoInadimplencia" runat="server" SupportsDisabledAttribute="True">
                <div style="float: left; width: 50%; margin-top: 1%; clear: both;">
                    <asp:GridView CssClass="EstilosGridView" EmptyDataText="Sem itens para exibição!" AllowPaging="true" PagerSettings-Visible="true" 
                        DataKeyNames="Id" ID="gridPadraoTrabalho" runat="server" Width="100%" AutoGenerateColumns="false" OnPageIndexChanging="gridPadraoTrabalho_PageIndexChanging" >
                        <HeaderStyle CssClass="CabecalhoGridView" />
                        <RowStyle CssClass="LinhaListaGridView" />
                        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
                        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                        <Columns>
                            <asp:BoundField HeaderText="Local de Trabalho" DataField="Descricao" />
                            <asp:BoundField HeaderText="Qtd.Func." DataField="QtdeFunc" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Inadimp. Total %" DataField="PercentualGeral" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Minha Inadimp. %" DataField="Percentual" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="float: right; width: 49%; margin-left: 0%; margin-top: 1%; margin-bottom: 1%;">
                    <asp:GridView CssClass="EstilosGridView" EmptyDataText="Sem itens para exibição!"
                        DataKeyNames="Id" ID="gridPadraoMargem" runat="server" Width="100%" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="CabecalhoGridView" />
                        <RowStyle CssClass="LinhaListaGridView" />
                        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
                        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                        <Columns>
                            <asp:BoundField HeaderText="Faixa de Margem" DataField="Descricao" />
                            <asp:BoundField HeaderText="Qtd.Func." DataField="QtdeFunc" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField HeaderText="Volume de Inadimplência %" DataField="Percentual" DataFormatString="{0:N}"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div>
    <dx:ASPxRoundPanel ContentPaddings-PaddingBottom="0px" ContentPaddings-PaddingLeft="0px" Visible="false"
        ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="5px" runat="server"
        ID="ASPxRoundPanelInadimplenciaDetalhada" ShowHeader="true" Width="100%" HeaderText="Inadimplência Detalhada"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentInadimplenciaDetalhada" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxGridView EnableTheming="True" ID="gridInadimplenciaDetalhada" ClientInstanceName="gridInadimplenciaDetalhada"
                    runat="server" KeyFieldName="Id" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="TmpInadimplenciaTempo.Descricao" Caption="Tempo de Inadimplência"
                            GroupIndex="0" SortIndex="0" Visible="false" SortOrder="Descending" />
                        <%--<dx:GridViewDataColumn FieldName="CPF"   VisibleIndex="1" />--%>
                        <dx:GridViewDataColumn FieldName="Nome"    VisibleIndex="2" />
                        <dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="3"    Caption="Matrícula" />
                        <dx:GridViewDataColumn FieldName="Situacao"  VisibleIndex="4"  Caption="Situação" />
                        <dx:GridViewDataTextColumn FieldName="ValorParcela" VisibleIndex="5"  HeaderStyle-HorizontalAlign="Center"
                            Caption="Valor da Parcela">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn  FieldName="ParcelasRestante" VisibleIndex="6" Caption="Parcelas Restante" HeaderStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn  FieldName="MargemNegativa" VisibleIndex="7"  HeaderStyle-HorizontalAlign="Center"
                            Caption="Margem Negativa">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn   FieldName="ValorAtrasado" VisibleIndex="8" HeaderStyle-HorizontalAlign="Center"
                            Caption="Valor em Atraso">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn FieldName="Proposta" VisibleIndex="9"  Caption="Proposta de Solução" />
                    </Columns>
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
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsDetail ShowDetailRow="false" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowGroupedColumns="true" />
                    <Settings ShowFilterRow="False" />
                    <SettingsCustomizationWindow Enabled="True" />
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
<div style="height:8px;clear:both;overflow:hidden;">&nbsp;</div>
<asp:Button CssClass="BotaoEstiloGlobal" ID="btRecuperavel" ClientIDMode="Static" Text="Recuperável pela Folha" runat="server" OnClick="Recuperavel_Click" />
<asp:Button CssClass="BotaoEstiloGlobal" ID="btNaoRecuperavel" ClientIDMode="Static" Text="Não Recuperável" runat="server" OnClick="NaoRecuperavel_Click" />
<br />
<br />
    <dx:ASPxRoundPanel ContentPaddings-PaddingBottom="0px" ContentPaddings-PaddingLeft="0px" Visible="false"
        ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="5px" runat="server"
        ID="ASPxRoundPanelRecuperavel" ShowHeader="true" Width="100%" HeaderText="Recuperável pela Folha"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewRecuperavel" ClientInstanceName="ASPxGridViewRecuperavel"
                    runat="server" KeyFieldName="ID" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="Nome" />
                        <dx:GridViewDataColumn FieldName="Matricula" Caption="Matrícula" />
                        <dx:GridViewDataColumn FieldName="CPF" Caption="CPF" />
                        <dx:GridViewDataTextColumn FieldName="Parcela" HeaderStyle-HorizontalAlign="Center" Caption="Parcela">                            
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn  FieldName="Pagas_Prazo" Caption="Pagas/Prazo" HeaderStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn  FieldName="Margem" HeaderStyle-HorizontalAlign="Center" Caption="Margem">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn   FieldName="EmAtraso" HeaderStyle-HorizontalAlign="Center"
                            Caption="Valor em Atraso">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <%--<dx:GridViewDataColumn FieldName="Proposta" VisibleIndex="9"  Caption="Proposta de Solução" />--%>
                    </Columns>
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
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsDetail ShowDetailRow="false" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowGroupedColumns="true" />
                    <Settings ShowFilterRow="False" />
                    <SettingsCustomizationWindow Enabled="True" />
                    <SettingsText EmptyDataRow="Sem dados para exibição!" />
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>


    <dx:ASPxRoundPanel ContentPaddings-PaddingBottom="0px" ContentPaddings-PaddingLeft="0px" Visible="false"
        ContentPaddings-PaddingRight="0px" ContentPaddings-PaddingTop="5px" runat="server"
        ID="ASPxRoundPanelNaoRecuperavel" ShowHeader="true" Width="100%" HeaderText="Não Recuperável pela Folha"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">                
                <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewNaoRecuperavel" ClientInstanceName="ASPxGridViewNaoRecuperavel"
                    runat="server" KeyFieldName="ID" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="Nome" />
                        <dx:GridViewDataColumn FieldName="Matricula" Caption="Matrícula" />
                        <dx:GridViewDataColumn FieldName="CPF" Caption="CPF" />
                        <dx:GridViewDataTextColumn FieldName="Parcela" VisibleIndex="5"  HeaderStyle-HorizontalAlign="Center"
                            Caption="Valor da Parcela">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataColumn  FieldName="Pagas_Prazo" Caption="Pagas/Prazo" HeaderStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn  FieldName="SaldoRestante" HeaderStyle-HorizontalAlign="Center"
                            Caption="Saldo Restante">
                            <PropertiesTextEdit DisplayFormatString="N" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
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
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsDetail ShowDetailRow="false" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowGroupedColumns="true" />
                    <Settings ShowFilterRow="False" />
                    <SettingsCustomizationWindow Enabled="True" />
                    <SettingsText EmptyDataRow="Sem dados para exibição!" />
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
     </dx:ASPxRoundPanel>


     <h1 class="TextoAncora">
            <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
        </h1>
        <div style="height: 5px;">
            &nbsp;</div>
</div>
