<%@ Control ClassName="WebUserControlAnaliseAverbacaos" EnableViewState="true" Language="C#"
    AutoEventWireup="true" CodeBehind="WebUserControlAnaliseAverbacaos.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAnaliseAverbacaos" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartPizza.ascx" TagName="WebUserControlChartPizza"
    TagPrefix="uc2" %>
<div>
    <div id="DivBusca" style="height: 100%;">
        <div style="margin-top: 5px;">
            <asp:Panel DefaultButton="btnBuscar" ID="PanelConciliacao" runat="server">
                <table width="100%" border="0" cellpadding="4" cellspacing="4">
                    <tr>
                        <td class="BordaBase" style="border: none; padding: 5px 0px;">
                            <h1 style="font-weight: bold; font-size: 14px;">
                                Período:</h1>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="4" cellspacing="4" class="WebUserControlTabelaAnaliseAverbacao">
                    <tr>
                        <td style="border: none;">
                            <table border="0" cellpadding="0" cellspacing="0" style="background-color: #d9ecf4;
                                border: 1px solid #c0dfe8;">
                                <tr>
                                    <td style="width: 105px; text-align: right; border: none; padding: 4px;">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Inicial (Mês/Ano):"></asp:Label>
                                    </td>
                                    <td style="text-align: left; border: none; width: 75px; padding: 4px;">
                                        <dx:ASPxTextBox Width="75px" ID="ASPxTextAnoMesInicio" BackColor="#ffffff" CssClass="TextBoxDropDownEstilos"
                                            MaskSettings-Mask="99/9999" ValidationSettings-EnableCustomValidation="false"
                                            ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-CausesValidation="false"
                                            EnableDefaultAppearance="false" EnableTheming="false" runat="server">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding: 4px;">
                            <asp:Label ID="LabelPeriod" runat="server" Font-Bold="true" Text="a"></asp:Label>
                        </td>
                        <td style="border: none;">
                            <table border="0" cellpadding="0" cellspacing="0" style="background-color: #d9ecf4;
                                border: 1px solid #c0dfe8;">
                                <tr>
                                    <td style="width: 105px; text-align: right; border: none; padding: 4px;">
                                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Inicial (Mês/Ano):"></asp:Label>
                                    </td>
                                    <td style="text-align: left; border: none; width: 75px; padding: 4px;">
                                        <dx:ASPxTextBox Width="75px" ID="ASPxTextBoxAnoMesFim" BackColor="#ffffff" CssClass="TextBoxDropDownEstilos"
                                            MaskSettings-Mask="99/9999" ValidationSettings-EnableCustomValidation="false"
                                            ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-CausesValidation="false"
                                            EnableDefaultAppearance="false" EnableTheming="false" runat="server">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="border: none; text-align: right; padding: 4px;">
                            <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
                                EnableTheming="false" ID="btnBuscar" runat="server" Text="Buscar" OnClick="Buscar_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
                                EnableTheming="false" ID="btnFiltros" runat="server" Text="Filtros" OnClick="Filtros_Click">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="divFiltros" visible="false" style="padding: 5px 5px 5px 5px;
                    clear: both;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="text-align: right; width: 2%; padding-right: 5px;">
                                Consignatária:
                            </td>
                            <td style="text-align: left; width: 32%;">
                                <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" DataTextField="Nome"
                                    DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div style="clear: both; overflow: hidden; height: 3px;">
            </div>
        </div>
    </div>
    <div style="clear: both; overflow: hidden; height: 3px;">
        &nbsp;
    </div>
    <div id="DivResultado" runat="server" visible="false" style="padding: 5px 0px;">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="4" style="padding: 0px 5px 5px 0px;">
                    <dx:ASPxButton EnableDefaultAppearance="false" EnableTheming="false" ID="ASPxButtonColunas"
                        CssClass="BotaoEstiloGlobal" runat="server" AutoPostBack="False" EnableClientSideAPI="True"
                        Text="Adicionar/Remover Colunas" ClientInstanceName="ASPxButtonColunas">
                        <ClientSideEvents Click="function(s, e) { ASPxPivotGridAnaliseUltimosMeses.ChangeCustomizationFieldsVisibility(); }" />
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <td style="padding: 5px 5px 5px 0px; width: 12%;">
                    <span>Exportar para: </span>
                </td>
                <td style="padding: 5px 0px; width: 62px; text-align: center;">
                    <dx:ASPxComboBox EnableDefaultAppearance="false" EnableTheming="false" ItemStyle-Paddings-Padding="2px" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" CssClass="TextBoxDropDownEstilos"
                        ID="cmbTipoExportacao" runat="server" Style="vertical-align: middle" SelectedIndex="0"
                        ValueType="System.String" Width="62px">
                        <Items>
                                            <dx:ListEditItem Text="Pdf" Value="0" />
                                            <dx:ListEditItem Text="Excel" Value="1" />
                                            <dx:ListEditItem Text="Rtf" Value="2" />
                                            <dx:ListEditItem Text="Texto/CSV" Value="3" />
                        </Items>
                    </dx:ASPxComboBox>
                </td>
                <td style="text-align: left; padding-left: 5px;">
                                <asp:Button  CssClass="BotaoEstiloGlobal"  runat="server" ID="DownloadFile" Text="Exportar" ClientIDMode="Static" OnClick="buttonSaveAs_Click" />
                </td>
            </tr>
        </table>
        <dx:ASPxPivotGrid ID="ASPxPivotGridAnalise" ClientInstanceName="ASPxPivotGridAnalise"
            runat="server" Width="100%" Height="116px" ClientIDMode="AutoID">
            <Fields>
                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Consignatária" FieldName="Fantasia"
                    ID="colNomeConsignataria" UnboundFieldName="" />
                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Tipo Consignação" FieldName="TipoConsignacao"
                    ID="PivotGridField5" UnboundFieldName="" />
                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Tipo" FieldName="Tipo" ID="colTipo"
                    UnboundFieldName="" />
                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Situação" FieldName="Situacao"
                    ID="PivotGridField1" UnboundFieldName="" />
                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" Caption="Ano/Mês" FieldName="Competencia"
                    ID="ColunaCompetencia" UnboundFieldName="UnboundColumn1" />
                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Qtde." FieldName="Competencia"
                    SummaryType="Count" ID="PivotGridField4" UnboundFieldName="" />
                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Valor Averbação" FieldName="Valor"
                    SummaryType="Sum" ID="ColunaValor" UnboundFieldName="" />
<%--                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Valor Averbação" FieldName="ValorContratado"
                    SummaryType="Sum" ID="PivotGridField2" UnboundFieldName="" />
--%>                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Valor Consignado" FieldName="ValorDevidoTotal"
                    SummaryType="Sum" ID="PivotGridField3" UnboundFieldName="" />
            </Fields>
            <OptionsView ShowHorizontalScrollBar="True" ShowColumnGrandTotals="True" ShowRowGrandTotals="False" />
            <OptionsPager RowsPerPage="40" />
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
        <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" ASPxPivotGridID="ASPxPivotGridAnalise"
            runat="server">
        </dx:ASPxPivotGridExporter>
    </div>
</div>
