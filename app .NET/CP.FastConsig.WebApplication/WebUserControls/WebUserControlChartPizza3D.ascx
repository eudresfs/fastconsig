<%@ Control ClassName="WebUserControlChartPizza3D" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlChartPizza3D.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlChartPizza3D" %>
<%@ Register TagPrefix="dxcharts" Namespace="DevExpress.XtraCharts" Assembly="DevExpress.XtraCharts.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxchartsui" Namespace="DevExpress.XtraCharts.Web" Assembly="DevExpress.XtraCharts.v11.1.Web, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<dxchartsui:WebChartControl BackColor="#E7F6FB" BorderOptions-Thickness="1" BorderOptions-Visible="true"
    BorderOptions-Color="#40CFFF" ID="WebChartControlGraficoPorProduto" runat="server"
    Height="280px" Width="200px" ClientInstanceName="chart" 
    PaletteName="Urban">
    <titles>
        <dxcharts:ChartTitle Text=""></dxcharts:ChartTitle>
        <dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="" Font="Tahoma, 8pt" TextColor="Gray"></dxcharts:ChartTitle>
    </titles>
    <seriesserializable>
        <dxcharts:Series>
            <LegendPointOptionsSerializable>
                <dxcharts:PiePointOptions PointView="ArgumentAndValues">
                    <ValueNumericOptions Format="Percent" Precision="0"></ValueNumericOptions>
                </dxcharts:PiePointOptions>
            </LegendPointOptionsSerializable>
            <PointOptionsSerializable>
                <dxcharts:PiePointOptions PointView="ArgumentAndValues">
                    <ValueNumericOptions Format="Percent" Precision="0"></ValueNumericOptions>
                </dxcharts:PiePointOptions>
        </PointOptionsSerializable>
        </dxcharts:Series>
    </seriesserializable>
    <diagramserializable>
        <dxcharts:SimpleDiagram3D ZoomPercent="130" RotationType="UseAngles" RotationOrder="ZXY" RotationAngleX="-35" RotationAngleZ="15" LabelsResolveOverlappingMinIndent="3"></dxcharts:SimpleDiagram3D>
    </diagramserializable>
</dxchartsui:WebChartControl>
