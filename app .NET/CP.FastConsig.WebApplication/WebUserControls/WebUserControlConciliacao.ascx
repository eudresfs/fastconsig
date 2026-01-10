<%@ Control ClassName="WebUserControlConciliacao" EnableViewState="true" Language="C#"
    AutoEventWireup="True" CodeBehind="WebUserControlConciliacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlConciliacao" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartPizza.ascx" TagName="WebUserControlChartPizza"
    TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<div>
    <div id="DivBusca" style="height: auto;">
        <asp:Panel DefaultButton="btnBuscar" ID="PanelConciliacao" runat="server" CssClass="MargemTop">
            <div style="float: left; height: 31px; line-height: 31px; font-weight: bold;">
                <asp:Label ID="Label1" runat="server" Text="Mês/Ano:"></asp:Label>
            </div>
            <div style="float: left; margin-left: 5px; width: 80px;">
                <dx:ASPxTextBox MaskSettings-ShowHints="False" ValidationSettings-RequiredField="false" ValidationSettings-ValidateOnLeave="False" ValidationSettings-EnableCustomValidation="False" ValidationSettings-CausesValidation="false" ValidationSettings-Display="None"
                    CssClass="TextBoxDropDownEstilos" Height="30px" Width="75px" ID="ASPxTextAnoMes"
                    MaskSettings-Mask="99/9999" EnableDefaultAppearance="True" EnableTheming="True"
                    runat="server">
                </dx:ASPxTextBox>
            </div>
            <div style="float: left; background-color: Red;">
                <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
                    EnableTheming="false" ID="btnBuscar" runat="server" Text="Buscar" OnClick="Buscar_Click">
                </dx:ASPxButton>
            </div>
            <div style="float: left; margin-left: 5px;">
                <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
                    EnableTheming="false" ID="btnFiltros" runat="server" Text="Pesquisa Avançada"
                    OnClick="Filtros_Click">
                </dx:ASPxButton>
            </div>
            <div style="clear: both; height: 5px;">
                &nbsp;</div>
            <div style="margin-bottom: 15px;" runat="server" id="divFiltros" visible="false">
                <table class="WebUserControlTabela" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2" style="border-bottom: 2px solid #083772; padding: 7px 0px;">
                            <h1 style="font-size: 14px; font-weight: bold; color: #083772;">
                                Filtros para Pesquisa de Conciliação Mensal</h1>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="width: 19%;">
                            Consignatária:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Nome"
                                DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            Situação:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Nome"
                                DataValueField="IDConciliacaoTipo" ID="DropDownListSituacao" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    <!--  Resultados da Busca -->
    <div id="DivResultado" runat="server" visible="false">
        <div style="background-color: Red;">
            <div style="float: left; width: 50%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderStyle-Font-Bold="true" runat="server"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    HeaderText="Resumo do Retorno da Folha" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <ContentPaddings Padding="5px" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                            <asp:GridView runat="server" ID="GridViewResumoFolha" CssClass="EstilosGridViewDashBoardConsiliacao"
                                Width="100%" Height="100%" AutoGenerateColumns="false">
                                <HeaderStyle Height="30px" CssClass="CabecalhoGridViewDashBoardConsiliacao " />
                                <RowStyle Height="30px" CssClass="LinhaListaGridViewDashBoardConsiliacao " />
                                <AlternatingRowStyle Height="30px" CssClass="LinhaAlternadaListaGridViewDashBoardConsiliacao " />
                                <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                                <Columns>
                                    <asp:BoundField HeaderText="Descrição" DataField="Descricao" />
                                    <asp:BoundField HeaderText="Valor" DataField="Valor" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Diferença" DataField="Diferenca" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                </Columns>
                            </asp:GridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <div style="float: left; width: 49.5%; margin-left: 0.5%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" HeaderStyle-Font-Bold="true" runat="server"
                    Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    HeaderText="Repasses Registrados" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <ContentPaddings Padding="5px" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                            <asp:GridView runat="server" ID="GridViewRepasses" CssClass="EstilosGridViewDashBoardConsiliacao"
                                Width="100%" Height="100%" AutoGenerateColumns="false">
                                <HeaderStyle Height="30px" CssClass="CabecalhoGridViewDashBoardConsiliacao " />
                                <RowStyle Height="30px" CssClass="LinhaListaGridViewDashBoardConsiliacao " />
                                <AlternatingRowStyle Height="30px" CssClass="LinhaAlternadaListaGridView" />
                                <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Fonte Recurso">
                                        <ItemTemplate>
                                            <%# Eval("FonteRecurso.Nome")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Data" DataField="Data" />
                                    <asp:BoundField HeaderText="Valor" DataField="Valor" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
        </div>
        <div style="clear: both; padding: 8px 0px 8px 0px; width: 100%; overflow: hidden;">
        </div>
        <div style="float: left; width: 50%;">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" HeaderStyle-Font-Bold="true" runat="server"
                Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                HeaderText="Resumo da Conciliação" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <ContentPaddings Padding="5px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                        <asp:GridView runat="server" ID="GridViewResumoConciliacao" CssClass="EstilosGridViewDashBoardConsiliacao "
                            Width="100%" Height="100%" AutoGenerateColumns="false">
                            <HeaderStyle Height="30px" CssClass="CabecalhoGridViewDashBoardConsiliacao" />
                            <RowStyle Height="30px" CssClass="LinhaListaGridViewDashBoardConsiliacao" />
                            <AlternatingRowStyle Height="30px" CssClass="LinhaAlternadaListaGridViewDashBoardConsiliacao" />
                            <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                            <Columns>
                                <asp:BoundField HeaderText="Descrição" DataField="Descricao" />
                                <asp:BoundField HeaderText="Valor" DataField="Valor" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Descontado" DataField="Descontado" DataFormatString="{0:n}"
                                    ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
        <div style="float: left; width: 49.5%; margin-left: 0.5%;">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel4" HeaderStyle-Font-Bold="true" runat="server"
                Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                HeaderText="Gráfico de Conciliação" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <ContentPaddings Padding="5px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                        <uc2:WebUserControlChartPizza ID="WebUserControlChartPizzaConciliacao" runat="server" />
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
        <div style="height: 3px; clear: both;">
           
        </div>

        <table border="0" cellpadding="0" cellspacing="5">
          <tr>
                <td>
                 <asp:Button ID="Label7" CssClass="BotaoEstiloGlobal" runat="server" Text="Detalhes da Conciliação" OnClick="ConciliacaoDetalhes_Click"></asp:Button>
             </td>

             <td>
                 <asp:Button ID="LinkButton1" CssClass="BotaoEstiloGlobal" runat="server" Text="Análise dos últimos 6 meses"
                OnClick="Analise6meses_Click"></asp:Button>
             </td>

          
          </tr>
        </table>
          <div style="height: 3px;">
           
        </div>
        <!-- 6 meses -->
        <div runat="server" id="divAnalise6meses" visible="false" style=" width: 100%;">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel5" HeaderStyle-Font-Bold="true" runat="server"
                Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                HeaderText="Análise dos últimos 6 meses" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <ContentPaddings Padding="5px" />
                <HeaderStyle Font-Bold="True"></HeaderStyle>
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="4" style="padding: 0px 5px 5px 0px;">
                                    <dx:ASPxButton EnableDefaultAppearance="false" EnableTheming="false" ID="ASPxButtonColunas"
                                        CssClass="BotaoEstiloGlobal" Height="29px" runat="server" AutoPostBack="False"
                                        EnableClientSideAPI="True" Text="Adicionar/Remover Colunas" ClientInstanceName="ASPxButtonColunas">
                                        <ClientSideEvents Click="function(s, e) { ASPxPivotGridAnaliseUltimosMeses.ChangeCustomizationFieldsVisibility(); }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 0px; width: 12%;">
                                    Exportar para:
                                </td>
                                <td style="padding: 5px 0px; width: 62px; text-align: center;">
                                    <dx:ASPxComboBox EnableDefaultAppearance="false" EnableTheming="false" ItemStyle-Paddings-Padding="2px"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" CssClass="TextBoxDropDownEstilos" ID="listExportFormat" runat="server"
                                        Style="vertical-align: middle" SelectedIndex="0" ValueType="System.String" Width="75px">
                                        <Items>
                                            <dx:ListEditItem Text="Pdf" Value="0" />
                                            <dx:ListEditItem Text="Excel" Value="1" />
                                            <dx:ListEditItem Text="Rtf" Value="2" />
                                            <dx:ListEditItem Text="Texto/CSV" Value="3" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                                <td style="text-align: left; padding-left: 5px;width: 62px;">
                                    <dx:ASPxButton ID="DownloadFilex"  EnableDefaultAppearance="false" EnableTheming="false"
                                        CssClass="BotaoEstiloGlobal" Width="60px" Height="29px"  runat="server"
                                        ToolTip="Exportar e salvar" Style="vertical-align: middle;" OnClick="buttonSaveAs_Click"
                                        Text="Salvar" />
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxPivotGrid ID="ASPxPivotGridAnaliseUltimosMeses" ClientInstanceName="ASPxPivotGridAnaliseUltimosMeses"
                            runat="server" Width="100%" Height="116px" MenuStyle-GutterWidth="0px">
                            <Fields>
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Descrição" FieldName="Descricao"
                                    ID="colDescricao" UnboundFieldName="" />
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Consignatária" FieldName="NomeConsignataria"
                                    ID="colNomeConsignataria" UnboundFieldName="" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" Caption="Ano/Mês" FieldName="Competencia"
                                    ID="ColunaCompetencia" UnboundFieldName="UnboundColumn1" SortOrder="Descending" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Valor" FieldName="Valor"
                                    ID="ColunaValor" UnboundFieldName="" />
                            </Fields>
                            <OptionsView ShowHorizontalScrollBar="True" ShowColumnGrandTotals="True" ShowRowGrandTotals="False" />
                            <OptionsLoadingPanel ImagePosition="Top">
                                <Image Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
                                </Image>
                            </OptionsLoadingPanel>
                            <OptionsView ShowHorizontalScrollBar="True" ShowRowGrandTotals="False"></OptionsView>
                            <OptionsLoadingPanel Text="Carregando&amp;hellip;">
                            </OptionsLoadingPanel>
                            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                <CustomizationFieldsBackground Url="~/App_Themes/Aqua/PivotGrid/pcHeaderBack.gif">
                                </CustomizationFieldsBackground>
                                <LoadingPanel Url="~/App_Themes/Aqua/PivotGrid/Loading.gif">
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
                                <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                </LoadingPanel>
                            </ImagesEditors>
                            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                <MenuStyle GutterWidth="0px" />
                            </Styles>
                        </dx:ASPxPivotGrid>
                        <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" ASPxPivotGridID="ASPxPivotGridAnaliseUltimosMeses"
                            runat="server">
                        </dx:ASPxPivotGridExporter>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
      <br />
        <!-- fim -->
        <!-- Detalhes da Conciliação -->
              <dx:ASPxRoundPanel ID="ASPxRoundPanel6" HeaderStyle-Font-Bold="true" runat="server"
            Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            HeaderText="Detalhes da Conciliação" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <ContentPaddings Padding="5px" />
            <HeaderStyle Font-Bold="True"></HeaderStyle>
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <div runat="server" id="divConciliacaoDetalhe">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="4" style="padding: 0px 5px 5px 0px;">
                                    <dx:ASPxButton ID="ASPxButton1" EnableDefaultAppearance="false" EnableTheming="false"
                                        CssClass="BotaoEstiloGlobal" Height="29px" runat="server" AutoPostBack="False"
                                        EnableClientSideAPI="True" Text="Adicionar/Remover Colunas" ClientInstanceName="ASPxButtonColunas">
                                        <ClientSideEvents Click="function(s, e) { ASPxPivotGridAnaliseUltimosMeses.ChangeCustomizationFieldsVisibility(); }" />
                                    </dx:ASPxButton>
                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gridAverbacaos">
                                    </dx:ASPxGridViewExporter>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 0px; width: 12%;">
                                    Exportar para:
                                </td>
                                <td style="padding: 5px 0px; width: 62px; text-align: center;">
                                    <dx:ASPxComboBox EnableDefaultAppearance="false" EnableTheming="false" ItemStyle-Paddings-Padding="2px"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" CssClass="TextBoxDropDownEstilos"
                                        ID="cmbTipoExportacao" runat="server" Style="vertical-align: middle" SelectedIndex="0"
                                        ValueType="System.String" Width="75px">
                                        <Items>
                                            <dx:ListEditItem Text="Pdf" Value="0" />
                                            <dx:ListEditItem Text="Excel" Value="1" />
                                            <dx:ListEditItem Text="Rtf" Value="2" />
                                            <dx:ListEditItem Text="Texto/CSV" Value="3" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                                <td style="text-align: left; padding-left: 5px;">
                                    <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="DownloadFile" Text="Exportar"
                                        ClientIDMode="Static" OnClick="buttonSaveAs_Click" />
                                    <%--                                    <dx:ASPxButton ID="DownloadFile" CssClass="BotaoEstiloGlobal" ClientIDMode="Static" Height="29px" runat="server" 
                                        ToolTip="Exportar e salvar" Style="vertical-align: middle;" 
                                        Text="Salvar" />
                                    --%>
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxGridView EnableTheming="True" ID="gridAverbacaos" ClientInstanceName="gridAverbacaos"
                            runat="server" KeyFieldName="IDAverbacaoConciliacao" Width="100%" AutoGenerateColumns="False"
                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0">
                                    <ClearFilterButton Visible="True" Text="Limpar" />
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataColumn FieldName="Empresa.Nome" VisibleIndex="1" Caption="Consignatária" />
                                <dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="1" Caption="Matrícula" />
                                <dx:GridViewDataColumn FieldName="Funcionario.Pessoa.Nome" VisibleIndex="1" Caption="Nome" />
                                <dx:GridViewDataColumn FieldName="Verba" VisibleIndex="2" Caption="Verba" />
                                <dx:GridViewDataColumn FieldName="Valor" VisibleIndex="5">
                                    <%--            <PropertiesEdit DisplayFormatString="c" />--%>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="ValorDescontado" VisibleIndex="5" Caption="Descontado">
                                    <%--            <PropertiesEdit DisplayFormatString="c" />--%>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Funcionario.NomeSituacao" VisibleIndex="5" Caption="Situação Func." />
                                <dx:GridViewDataColumn FieldName="ConciliacaoTipo.Nome" VisibleIndex="5" Caption="Situação" />
                                <dx:GridViewDataColumn FieldName="Produto.ProdutoGrupo.Nome" VisibleIndex="5" Caption="Tipo Consignação" />
                                <dx:GridViewDataColumn FieldName="Motivo" VisibleIndex="5" Caption="Motivo" />
                            </Columns>
                            <GroupSummary>
                                <dx:ASPxSummaryItem FieldName="Verba" SummaryType="Count" ShowInGroupFooterColumn="Verba"
                                    ValueDisplayFormat="#,0" DisplayFormat="{0:n0}" />
                                <dx:ASPxSummaryItem FieldName="Valor" SummaryType="Sum" ShowInGroupFooterColumn="Valor"
                                    DisplayFormat="{0:n2}" />
                                <dx:ASPxSummaryItem FieldName="ValorDescontado" SummaryType="Sum" ShowInGroupFooterColumn="ValorDescontado"
                                    DisplayFormat="{0:n2}" />
                            </GroupSummary>
                            <TotalSummary>
                                <dx:ASPxSummaryItem FieldName="Verba" SummaryType="Count" ShowInColumn="Verba" ValueDisplayFormat="#,0"
                                    DisplayFormat="{0:n0}" />
                                <dx:ASPxSummaryItem FieldName="Valor" SummaryType="Sum" ShowInColumn="Valor" DisplayFormat="{0:n2}" />
                                <dx:ASPxSummaryItem FieldName="ValorDescontado" ShowInColumn="ValorDescontado" DisplayFormat="{0:n2}"
                                    SummaryType="Sum" />
                            </TotalSummary>
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
                            <Settings ShowGroupPanel="True" ShowFilterRow="True" ShowFooter="true" ShowGroupFooter="VisibleAlways" />
                            <SettingsCustomizationWindow Enabled="True" />
                            <%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
                            --%>
                        </dx:ASPxGridView>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <!-- fim -->
        <h1 class="TextoAncora">
            <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
                Topo da Página</a>
        </h1>
    </div>
    <!-- fim -->
</div>
