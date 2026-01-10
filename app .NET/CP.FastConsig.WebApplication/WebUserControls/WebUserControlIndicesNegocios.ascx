<%@ Control Language="C#" AutoEventWireup="true" ClassName="WebUserControlIndicesNegocio" CodeBehind="WebUserControlIndicesNegocios.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlIndicesNegocios" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="LarguraMargem" >
        <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelIndicesNegocio" ShowHeader="true"
            Width="100%" HeaderText="Índice de Negócios" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingRight="0px"
            GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
            HeaderStyle-Font-Bold="true">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellpadding="0" cellspacing="0" width="570px">
                        <tr>
                            <td valign="middle" style="padding-left: 5px; width: 48%;">
                                <asp:Label runat="server" ID="LabelPeriodo" Text=""></asp:Label>
                                <asp:Label runat="server" ID="LabelCompetencia" Visible="false" Text=""></asp:Label>                                
                            </td>
                            <td valign="middle">
                                <asp:Button ID="ButtonPeriodoAnterior" runat="server" Text="<<" CssClass="BotaoEstiloGlobal"
                                    OnClick="selecionaPeriodoAnterior_Click" />&nbsp;
                                <asp:Button ID="ButtonPeriodoProximo" runat="server" Text=">>" CssClass="BotaoEstiloGlobal"
                                    OnClick="selecionaPeriodoProximo_Click" />
                            </td>
                        </tr>
                    </table>
                    <br /> 
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewAConquistar" ClientInstanceName="gridParametrosGerais"
                        runat="server" KeyFieldName="Nome" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua">
                        <Columns>
                            <dx:GridViewBandColumn>
                                <HeaderTemplate>
                                    <dx:ASPxImage runat="server" ID="ASPxImageAConquistar" ImageUrl="~/Imagens/Interrogacao.png"
                                        ToolTip="Ajuda" ClientIDMode="Static" Width="15" Style="float: left; margin-top: 1px" />
                                    <dx:ASPxLabel runat="server" ID="ASPxLabelAConquistar" Style="float: left; margin-left: 5px"
                                        Text="Correr para Conquistar" />
                                </HeaderTemplate>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Clientes seus com Margem Hoje" FieldName="Quantidade"
                                        Width="20%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Margem Disponível" ToolTip="Soma da Margem Disponível de cada cliente neste momento."
                                        FieldName="MargemDisponivel" Width="20%" CellStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <HeaderTemplate>
                                            <div style="text-align: center;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabel54" ToolTip="Soma da Margem Disponível de cada cliente neste momento."
                                                    Text="Margem Disponível" />
                                            </div>
                                        </HeaderTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Margem Utilizada" ToolTip="A soma de todas as parcelas dos contratos ativos para descontar em folha."
                                        FieldName="MargemUtilizada" Width="20%" CellStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <HeaderTemplate>
                                            <div style="text-align: center;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabel54" ToolTip="Soma da Margem Disponível de cada cliente neste momento."
                                                    Text="Margem Utilizada" />
                                            </div>
                                        </HeaderTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Perspectiva de Negócios" FieldName="VolumeNegocio" Width="20%"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <HeaderTemplate>
                                            <div style="text-align: center;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabel32" ToolTip="Margem Disponível x Seu Prazo Máximo"
                                                    Text="Perspectiva de Negócios" />
                                            </div>
                                        </HeaderTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Valor que Adiciona" FieldName="VolumeAdicionado" Width="20%"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <HeaderTemplate>
                                            <div style="text-align: center;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabel12" ToolTip="Diferença entre Valor Consignado Total e o Valor Líquido dos Contratos."
                                                    Text="Valor que Adiciona" />
                                            </div>
                                        </HeaderTemplate>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                        <SettingsText EmptyDataRow="Sem dados para exibição!" />
                    </dx:ASPxGridView>
                    
                    <div style="height: 7px;">
                        &nbsp;</div>
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewConquistado" ClientInstanceName="gridParametrosGerais"
                        runat="server" KeyFieldName="Nome" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua" >
                        <Columns>
                            <dx:GridViewBandColumn>
                                <HeaderTemplate>
                                    <dx:ASPxLabel runat="server" ID="ASPxLabelConquistado" Text="Conquistas Realizadas"
                                        Style="float: left; margin-left: 5px" />
                                </HeaderTemplate>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Tipo" FieldName="AverbacaoTipo.Nome" Width="20%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Qtde.Averbações" FieldName="Quantidade" Width="20%" 
                                        HeaderStyle-HorizontalAlign="Center">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Valor Bruto" FieldName="ValorBruto" Width="20%"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <DataItemTemplate>
                                            <div style="text-align: right;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabelValorBruto" Text='<%# string.Format("{0:#,0.00}", Eval("ValorBruto")) %>'>
                                                </dx:ASPxLabel>
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Valor Adicionado" FieldName="ValorAdicionado"
                                        Width="20%" HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
                                        <DataItemTemplate>
                                            <div style="text-align: right;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabelewq" Text='<%# string.Format("{0:#,0.00}", Eval("ValorAdicionado")) %>'>
                                                </dx:ASPxLabel>
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Lucro Bruto" FieldName="LucroBruto" Width="20%"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="#,0.00" />
                                        <HeaderTemplate>
                                            <div style="text-align: center;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabel65" ToolTip="Diferença entre Valor Consignado Total e o Valor Líquido dos Contratos."
                                                    Text="Lucro Bruto" />
                                            </div>
                                        </HeaderTemplate>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                        <Settings ShowFooter="True" />
                        <SettingsBehavior AllowSort="False"></SettingsBehavior>
                        <SettingsText EmptyDataRow="Sem dados para exibição!" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="Quantidade" SummaryType="Sum" ValueDisplayFormat="#,0" DisplayFormat="{0:n0}"  />
                            <dx:ASPxSummaryItem FieldName="ValorBruto" SummaryType="Sum" ValueDisplayFormat="#,0.00" DisplayFormat="{0:c}"  />
                            <dx:ASPxSummaryItem FieldName="ValorAdicionado" SummaryType="Sum" ValueDisplayFormat="#,0.00" DisplayFormat="{0:c}"  />
                            <dx:ASPxSummaryItem FieldName="LucroBruto" SummaryType="Sum" ValueDisplayFormat="#,0.00" DisplayFormat="{0:c}" />
                        </TotalSummary>
                    </dx:ASPxGridView>
                    <div style="height: 7px;">
                        &nbsp;</div>
                    
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewPerdas" ClientInstanceName="gridParametrosGerais"
                        runat="server" KeyFieldName="Nome" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua" >
                        <Columns>
                            <dx:GridViewBandColumn>
                                <HeaderTemplate>
                                    <dx:ASPxImage runat="server" ID="ASPxImagePerdas" ImageUrl="~/Imagens/Interrogacao.png"
                                        ClientIDMode="Static" Width="15" Style="float: left; margin-top: 1px" />
                                    <dx:ASPxLabel runat="server" ID="ASPxLabelPerdas" Text="Perdas Evitáveis"
                                        Style="float: left; margin-left: 5px" />
                                </HeaderTemplate>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Tipo" FieldName="Descricao" Width="25%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Qtde.Averbações" FieldName="Quantidade" Width="25%"
                                        HeaderStyle-HorizontalAlign="Center">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Valor" FieldName="Valor" Width="25%" HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
<%--                                    <DataItemTemplate>
                                            <div style="text-align: right;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabelConquistado" ToolTip='<%# Eval("DicaValor") %>'
                                                    Text='<%# string.Format("{0:c}", Eval("Valor")) %>'>
                                                </dx:ASPxLabel>
                                            </div>
                                        </DataItemTemplate>--%>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Deixou de Lucrar" FieldName="NaoLucrou" Width="25%"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <PropertiesTextEdit DisplayFormatString="c" />
<%--                                        <DataItemTemplate>
                                            <div style="text-align: right;">
                                                <dx:ASPxLabel runat="server" ID="ASPxLabelConquistado" ToolTip='<%# Eval("DicaLucro") %>'
                                                    Text='<%# string.Format("{0:c}", Eval("Lucro")) %>'>
                                                </dx:ASPxLabel>
                                            </div>
                                        </DataItemTemplate>--%>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:GridViewBandColumn>
                        </Columns>
                        <Settings ShowFooter="True" />
                        <SettingsText EmptyDataRow="Sem dados para exibição!" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="Quantidade" SummaryType="Sum" ValueDisplayFormat="#,0" DisplayFormat="{0:n0}"/>
                            <dx:ASPxSummaryItem FieldName="Valor" SummaryType="Sum" DisplayFormat="{0:c}" />
                            <dx:ASPxSummaryItem FieldName="NaoLucrou" SummaryType="Sum" DisplayFormat="{0:c}" />
                        </TotalSummary>
                    </dx:ASPxGridView>
                    
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
