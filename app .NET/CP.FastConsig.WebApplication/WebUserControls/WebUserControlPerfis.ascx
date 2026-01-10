<%@ Control ClassName="WebUserControlPerfis" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlPerfis.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlPerfis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
<div style="height: 100px; margin-bottom: 5px;">
    <div style="margin-top: 5px;">
        <asp:Panel ID="panel" runat="server">
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
                    </tr>
                    <tr runat="server" id="tr_consignataria" visible="false">
                        <td style="text-align: right; padding: 5px 3px 5px 0px;">
                            <span style="font-weight: bold;">Consignatária: </span>
                        </td>
                        <td style="text-align: left; width: 32%; padding: 5px 3px 5px 0px;">
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="DropDownListConsignataria_SelectedIndexChanged"
                                DataTextField="Nome" Visible="false" DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
</div>
    <div>
        <asp:Button ID="ButtonNovo" class="BotaoEstiloGlobal" OnClick="ButtonNovo_Click"
            runat="server" Text="Novo" />
    </div>
    <asp:ObjectDataSource ID="ODS_Perfis" runat="server" TypeName="CP.FastConsig.BLL.ODS_Perfil"
        DataObjectTypeName="CP.FastConsig.DAL.Perfil" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_Perfis_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdEmpresa" />
            <asp:Parameter DbType="Int32" Name="IdModulo" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="grid_Sorting" CssClass="EstilosGridView" PageSize="20"
        OnPageIndexChanging="grid_PageIndexChanging" AllowPaging="true" AllowSorting="true"
        EmptyDataText="Sem itens para exibição!" DataSourceID="ODS_Perfis" DataKeyNames="IDPerfil"
        ID="grid" runat="server" Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="grid_SelectedIndexChanged"
        OnRowDataBound="grid_RowDataBound">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                     <asp:LinkButton runat="server" ID="ImageButtonEditar" CommandArgument='<%# Eval("IDPerfil") %>' OnClick="ImageButtonEditar_Click">
                    <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" /></asp:LinkButton>
                    <%--                    <asp:LinkButton runat="server" ID="ProdutosEditar" CommandArgument='<%# Eval("IDProduto") %>' OnClick="ProdutosEditar_Click">
                    </asp:LinkButton>
                    --%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?')"
                        runat="server" ID="PerfisRemover" CommandArgument='<%# Eval("IDPerfil") %>'
                        OnClick="PerfisRemover_Click">
                        <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Nome" HeaderText="Nome" DataField="Nome" />           
            <asp:CommandField runat="server" ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn"
                HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
