<%@ Control ClassName="WebUserControlChartBarra3D" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlChartBarra3D.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlChartBarra3D" %>
<%@ Register TagPrefix="dxchartsui" Namespace="DevExpress.XtraCharts.Web" Assembly="DevExpress.XtraCharts.v11.1.Web, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxcharts" Namespace="DevExpress.XtraCharts" Assembly="DevExpress.XtraCharts.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<dxchartsui:WebChartControl BackColor="#E7F6FB" BorderOptions-Thickness="1" BorderOptions-Visible="true"
    BorderOptions-Color="#40CFFF" ID="WebChartControlGrafico" runat="server" Height="280px"
    Width="200px" ClientInstanceName="chart" PaletteName="Urban">
    <titles>
        <dxcharts:ChartTitle Text=""></dxcharts:ChartTitle>
        <dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="" Font="Tahoma, 8pt" TextColor="Gray"></dxcharts:ChartTitle>
    </titles>
    <DiagramSerializable>
        <dxcharts:XYDiagram3D PerspectiveAngle="0" ZoomPercent="130" VerticalScrollPercent="4">
            <AxisX>
                <Range SideMarginsEnabled="True"></Range>
            </AxisX>
            <AxisY>
                <Range SideMarginsEnabled="True"></Range>
            </AxisY>
        </dxcharts:XYDiagram3D>
    </DiagramSerializable>
</dxchartsui:WebChartControl>