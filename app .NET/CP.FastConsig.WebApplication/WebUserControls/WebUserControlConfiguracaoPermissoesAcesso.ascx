<%@ Control ClassName="WebUserControlConfiguracaoPermissoesAcesso" EnableTheming="false"
    Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlConfiguracaoPermissoesAcesso.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlConfiguracaoPermissoesAcesso" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<div style="height: 100px; margin-bottom: 5px;">
    <div style="margin-top: 5px;">
        <asp:Panel DefaultButton="btnBuscar" ID="panel" runat="server">
            <div>
                <table border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td runat="server" id="LabelModulo" style="width: 2%; text-align: right; padding: 5px 3px 5px 0px;">
                            <span style="font-weight: bold;">Módulo:</span>
                        </td>
                        <td style="text-align: left; width: 32%; padding: 5px 3px 5px 0px;">
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" AutoPostBack="true"
                                DataTextField="Nome" Visible="false" DataValueField="IDModulo" ID="DropDownListModulo"
                                OnSelectedIndexChanged="DropDownListModulo_SelectedIndexChanged" />
                        </td>
                        <td style="padding: 5px 0px">
                            <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Width="80" Font-Bold="true"
                                EnableDefaultAppearance="false" EnableTheming="True" ID="btnBuscar" runat="server"
                                Text="Buscar" OnClick="Buscar_Click">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr runat="server" id="tr_consignataria" visible="false">
                        <td style="text-align: right; padding: 5px 3px 5px 0px;">
                            <span style="font-weight: bold;">Consignatária: </span>
                        </td>
                        <td style="text-align: left; width: 32%; padding: 5px 3px 5px 0px;">
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" AutoPostBack="true"
                                DataTextField="Nome" Visible="false" DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
</div>
<div class="GlobalUserControl">
    <asp:Button ID="ButtonPerfis" class="BotaoEstiloGlobal" OnClick="ButtonPerfis_Click"
        runat="server" Text="Perfis" />
    <br />
    <br />
    <dx:ASPxTreeList ID="menu" EnableTheming="True" runat="server" Width="100%" Styles-Cell-HorizontalAlign="Left"
        AutoGenerateColumns="False" ClientInstanceName="menu" KeyFieldName="IDRecurso" 
        ParentFieldName="IDRecursoPai" OnCustomDataCallback="treeList_CustomDataCallback"
        CssPostfix="Aqua">
        <Columns>
            <dx:TreeListDataColumn FieldName="Nome" VisibleIndex="0" Caption="Opção/Recurso" />
            <dx:TreeListCheckColumn FieldName="coluna_1" VisibleIndex="1" Name="coluna_1">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__1' meuid='<%# Container.NodeKey %>' Enabled="<%#GetCheckboxVisibility(Container)%>"
                        runat="server" ValueType="System.Int32" Value='<%# Container.GetValue("coluna_1") %>'
                        ValueChecked="1" ValueUnchecked="0">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('1|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_2" VisibleIndex="1" Name="coluna_2">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__2' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_2") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('2|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_3" VisibleIndex="1" Name="coluna_3">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__3' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_3") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('3|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_4" VisibleIndex="1" Name="coluna_4">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__4' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_4") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('4|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_5" VisibleIndex="1" Name="coluna_5">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__5' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_5") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('5|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_6" VisibleIndex="1" Name="coluna_6">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__6' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_6") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('6|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_7" VisibleIndex="1" Name="coluna_7">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__7' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_7") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('7|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_8" VisibleIndex="1" Name="coluna_8">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__8' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_8") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('8|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_9" VisibleIndex="1" Name="coluna_9">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__9' meuid='<%# Container.NodeKey %>' runat="server" ValueType="System.Int32"
                        Value='<%# Container.GetValue("coluna_9") %>' ValueChecked="1" ValueUnchecked="0"
                        CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('9|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
            <dx:TreeListCheckColumn FieldName="coluna_10" VisibleIndex="1" Name="coluna_10">
                <DataCellTemplate>
                    <dx:ASPxCheckBox ID='coluna__10' meuid='<%# Container.NodeKey %>' runat="server"
                        ValueType="System.Int32" Value='<%# Container.GetValue("coluna_10") %>' ValueChecked="1"
                        ValueUnchecked="0" CssPostfix="CustomImage">
                        <ClientSideEvents ValueChanged="function OnValueChanged(s, e) {                            
                            menu.PerformCustomDataCallback('10|'+s.name+'|'+document.getElementById(s.name).getAttribute('meuid')+'|'+s.GetChecked()); 
                            }" />
                        <CheckedImage Url="~/Imagens/tick.png" ToolTip="Permitir" Width="16px" Height="16px" />
                        <UncheckedImage Url="~/Imagens/unchecked.png" ToolTip="Proibir" Width="16px" Height="16px" />
                    </dx:ASPxCheckBox>
                </DataCellTemplate>
            </dx:TreeListCheckColumn>
        </Columns>
        <SettingsLoadingPanel ImagePosition="Top" />
        <Images SpriteCssFilePath="~/App_Themes/Aqua/TreeList/sprite.css">
            <LoadingPanel Url="~/App_Themes/Aqua/TreeList/Loading.gif">
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
        <Styles CssFilePath="~/App_Themes/Aqua/TreeList/styles.css" CssPostfix="Aqua">
            <AlternatingNode Enabled="true" />
            <CustomizationWindowContent VerticalAlign="Top">
            </CustomizationWindowContent>
        </Styles>
        <Settings GridLines="Both" />
        <Settings ShowTreeLines="true" />
        <SettingsBehavior AllowFocusedNode="true" />
        <Settings ShowTreeLines="False"></Settings>
        <SettingsBehavior AllowFocusedNode="True"></SettingsBehavior>
        <Styles>
            <CustomizationWindowContent VerticalAlign="Top">
            </CustomizationWindowContent>
        </Styles>
    </dx:ASPxTreeList>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
            Topo da Página</a>
    </h1>
    <div style="height: 5px;">
        &nbsp;</div>
</div>
