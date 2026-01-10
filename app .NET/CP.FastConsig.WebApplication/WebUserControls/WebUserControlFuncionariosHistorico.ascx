<%@ Control ClassName="WebUserControlFuncionarios_Historico" Language="C#" AutoEventWireup="True"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlFuncionariosHistorico.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFuncionariosHistorico" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
                    <tr>
                        <td>
                            <b>Matrícula:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <b>Nome:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>CPF:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelCpfFuncionario"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div class="GlobalUserControl">
    <a name="topo" runat="server" href="#topo"></a>
    <br />
    <div>
        <asp:TextBox class="TextBoxDropDownEstilos" Height="29px" Width="300px" runat="server"
            ID="TextBoxPesquisar"></asp:TextBox>
        <asp:Button ID="ButtonPesquisar" CssClass="BotaoEstiloGlobal" Height="30px" OnClick="ButtonPesquisar_Click"
            Text="Buscar" runat="server" />
    </div>
    <asp:ObjectDataSource ID="ODS_FuncHistoricos" runat="server" TypeName="CP.FastConsig.BLL.ODS_FuncHistoricos"
        DataObjectTypeName="CP.FastConsig.DAL.FuncionarioHistorico" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdFuncionario" />
            <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="grid_Sorting" CssClass="EstilosGridView" PageSize="20" OnPageIndexChanging="grid_PageIndexChanging"
        AllowPaging="true" AllowSorting="true" EmptyDataText="Sem itens para exibição!"
        DataSourceID="ODS_FuncHistoricos" DataKeyNames="IDFuncionarioHistorico" ID="grid"
        runat="server" Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="grid_SelectedIndexChanged"
        OnRowDataBound="grid_RowDataBound">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle CssClass="PaginadorEstilos" HorizontalAlign="Center" />
        <Columns>
            <asp:BoundField HeaderText="Data" DataField="CreatedOn" />
            <asp:BoundField HeaderText="Situação" DataField="NomeSituacao" />
            <asp:BoundField HeaderText="Regime" DataField="NomeRegimeFolha" />
            <asp:BoundField HeaderText="Local" DataField="NomeLocalFolha" />
            <asp:BoundField HeaderText="Setor" DataField="NomeSetorFolha" />
            <asp:BoundField HeaderText="Cargo" DataField="NomeCargoFolha" />
            <asp:BoundField HeaderText="Margem Bruta" DataField="MargemFolhaBruta" />
            <asp:BoundField HeaderText="Margem Disponível" DataField="MargemFolhaDisponivel" />
        </Columns>
    </asp:GridView>
</div>
