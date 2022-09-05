<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Chart.ascx.cs" Inherits="SGI.GestionTramite.Controls.Charts.Chart" %>

<%@ Register TagPrefix="ej" Namespace="Syncfusion.JavaScript.DataVisualization.Models" Assembly="Syncfusion.EJ" %>

<ej:Chart ID="Chart_estadoECA" runat="server" ClientIDMode="Static" Palette="green,#f0d400,red" CanResize="true">
    <Title Text="" TextAlignment="Center"></Title>
    <Legend Visible="false">
    </Legend>
    <Series>
        <ej:Series Type="Column" LabelPosition="Inside" XName="ValorTexto" YName="ValorPorcentaje" ColumnWidth="0.2">

            <Marker Visible="true">
                <DataLabel Visible="true" Shape="Rectangle" TextMappingName="ValorDetalle">
                    <Margin Left="10" Right="10" />
                    <Font Color="White" FontFamily="Segoe UI" FontWeight="Bold" FontSize="16px"></Font>
                </DataLabel>
            </Marker>
            <Border Color="#ffffff" Width="2" />
            <SelectionSettings Enable="true" Opacity="0.4" />
        </ej:Series>
    </Series>
</ej:Chart>

