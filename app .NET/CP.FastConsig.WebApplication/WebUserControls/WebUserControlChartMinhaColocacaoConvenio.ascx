<%@ Control ClassName="WebUserControlChartMinhaColocacaoConvenio" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlChartMinhaColocacaoConvenio.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlChartMinhaColocacaoConvenio" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v11.1.Web, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register Src="WebUserControlChartBarra.ascx" TagName="WebUserControlChartBarra"
    TagPrefix="uc3" %>
<%@ Register Src="WebUserControlChartLinha.ascx" TagName="WebUserControlChartLinha"
    TagPrefix="uc4" %>
<br />

<div style="float:left; margin-bottom:10px; padding-top:5px">
    <asp:Label ID="LabelProdutoGrupo" runat="server" Text="Tipo Produto:" Width="100" style="text-align: right; float:left; margin-right:3px"></asp:Label>
    <asp:DropDownList ID="DropDownListProdutoGrupo" CssClass="TextBoxDropDownEstilos" DataTextField="Nome" EnableViewState="true" 
                      DataValueField="IDProdutoGrupo" runat="server" Height="16px" Width="185px" style="float:left; height:25px; padding:3px">
    </asp:DropDownList>
    <asp:Label ID="Label1" runat="server" Text="Dados:" Width="100" style="text-align: right; float:left; margin-right:3px"></asp:Label>
    <asp:DropDownList ID="DropDownListDados" CssClass="TextBoxDropDownEstilos" EnableViewState="true" 
                       runat="server" Height="16px" Width="85px" style="float:left; height:25px; padding:3px">
                       <asp:ListItem Text="Carteira" Value="Carteira" Selected></asp:ListItem>
                       <asp:ListItem Text="Produção" Value="Produzido"></asp:ListItem>
    </asp:DropDownList>

    <asp:Label ID="LabelTop" runat="server" Text="Top:" Width="50" style="text-align: right; float:left; margin-right:3px"></asp:Label> 
    <dx:ASPxSpinEdit ID="ASPxSpinEditTop" runat="server" Height="21px" Number="5" Width="60"  style="float:left"/>

    <asp:Label ID="LabelMeses" runat="server" Text="Meses:" Width="70" style="text-align: right; float:left; margin-right:3px"></asp:Label>
    <dx:ASPxSpinEdit ID="ASPxSpinEditMeses" runat="server" Height="21px" Number="6" MaxValue="12" Width="60"  style="float:left"/>
</div>
<div style="float: right; margin: 0pt 30px 30px;">
    <asp:Button ID="ButtonBarra" class="BotaoEstiloGlobal" runat="server" Text="Barra" 
            onclick="ButtonBarra_Click" />&nbsp;&nbsp;&nbsp;
    <asp:Button ID="ButtonLinha" class="BotaoEstiloGlobal" runat="server" Text="Linha" 
            onclick="ButtonLinha_Click" />&nbsp;&nbsp;&nbsp;
    <asp:Button ID="ButtonBolha" class="BotaoEstiloGlobal" runat="server" Text="Bolha" 
            onclick="ButtonBolha_Click" />&nbsp;&nbsp;&nbsp;
    <asp:Button ID="ButtonRanking" class="BotaoEstiloGlobal" runat="server" 
            Text="Ranking" onclick="ButtonRanking_Click" />
</div>

<dxchartsui:WebChartControl ID="WebChartControlBolha" runat="server" ClientInstanceName="chartBolha" Height="520px" Width="900px">

<EmptyChartText Text="N&#227;o h&#225; informa&#231;&#245;es para mostrar o gr&#225;fico"></EmptyChartText>

<SmallChartText Text="Aumente o tamanho do gr&#225;fico,\r\n para visualizar este layout."></SmallChartText>

<BorderOptions Color="White"></BorderOptions>
    <DiagramSerializable>
        <cc1:XYDiagram>
            <axisx visibleinpanesserializable="-1">
                <range sidemarginsenabled="True" />
            </axisx>
            <axisy visibleinpanesserializable="-1">
                <range sidemarginsenabled="True" />
            </axisy>
            <defaultpane>
                <fillstyle fillmode="Solid">
                    <optionsserializable>
                        <cc1:SolidFillOptions />
                    </optionsserializable>
                </fillstyle>
                <shadow visible="True" />
            </defaultpane>
        </cc1:XYDiagram>
    </DiagramSerializable>
    <fillstyle fillmode="Gradient">
        <optionsserializable>
            <cc1:RectangleGradientFillOptions Color2="255, 255, 255" />
        </optionsserializable>
    </fillstyle>
    <annotationrepository>
        <cc1:TextAnnotation Name="Text Annotation 1" Text="Tamanho da Bolha =&gt;  Quantidade de Averbações
Altura da Bolha  =&gt;  Valor das Averbações" Visible="False">
            <anchorpointserializable>
                <cc1:ChartAnchorPoint X="0" Y="43" />
            </anchorpointserializable>
            <shapepositionserializable>
                <cc1:FreePosition>
                    <innerindents left="0" top="0" />
                </cc1:FreePosition>
            </shapepositionserializable>
        </cc1:TextAnnotation>
    </annotationrepository>
    
    <seriesserializable>
        <cc1:Series LegendText="Legenda:" Name="Series 1">
            <points>
                <cc1:SeriesPoint ArgumentSerializable="Jan" Values="10;60">
                </cc1:SeriesPoint>
                <cc1:SeriesPoint ArgumentSerializable="Fev" Values="25;120">
                </cc1:SeriesPoint>
            </points>
            <viewserializable>
                <cc1:BubbleSeriesView>
                </cc1:BubbleSeriesView>
            </viewserializable>
            <labelserializable>
                <cc1:BubbleSeriesLabel LineVisible="False">
                    <fillstyle>
                        <optionsserializable>
                            <cc1:SolidFillOptions />
                        </optionsserializable>
                    </fillstyle>
                </cc1:BubbleSeriesLabel>
            </labelserializable>
            <pointoptionsserializable>
                <cc1:PointOptions>
                </cc1:PointOptions>
            </pointoptionsserializable>
            <legendpointoptionsserializable>
                <cc1:PointOptions>
                </cc1:PointOptions>
            </legendpointoptionsserializable>
        </cc1:Series>
    </seriesserializable>

    <seriestemplate>
        <viewserializable>
            <cc1:BubbleSeriesView>
            </cc1:BubbleSeriesView>
        </viewserializable>
        <labelserializable>
            <cc1:BubbleSeriesLabel LineVisible="True">
                <fillstyle>
                    <optionsserializable>
                        <cc1:SolidFillOptions />
                    </optionsserializable>
                </fillstyle>
            </cc1:BubbleSeriesLabel>
        </labelserializable>
        <pointoptionsserializable>
            <cc1:PointOptions>
            </cc1:PointOptions>
        </pointoptionsserializable>
        <legendpointoptionsserializable>
            <cc1:PointOptions>
            </cc1:PointOptions>
        </legendpointoptionsserializable>
    </seriestemplate>

</dxchartsui:WebChartControl>

<uc3:WebUserControlChartBarra ID="WebUserControlChartBarraColocacaoConvenio" runat="server" />
<uc4:WebUserControlChartLinha ID="WebUserControlChartLinhaColocacaoConvenio" runat="server" />