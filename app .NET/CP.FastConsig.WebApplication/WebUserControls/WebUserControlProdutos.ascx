<%@ Control ClassName="WebUserControlProdutos" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlProdutos.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlProdutos" %>
<div class="GlobalUserControl">
    <a name="topo" runat="server" href="#topo"></a>
    <div style="width: 100%;">
        <div style="float: left; margin-right: 5px;">
            <asp:TextBox class="TextBoxDropDownEstilos" Width="300px" Height="28" runat="server"
                ID="TextBoxPesquisar" Visible="false"></asp:TextBox>
        </div>
        <div style="float: left;">
            <asp:Button ID="ButtonPesquisar" class="BotaoEstiloGlobal" OnClick="ButtonPesquisar_Click"
                Text="Buscar" runat="server" Visible="false" />
        </div>
    </div>
    <div style="width: 100%; height: 8px; clear: both; overflow: hidden;">
    </div>
    <div>
        <asp:Button ID="ButtonNovoProduto" class="BotaoEstiloGlobal" OnClick="ButtonNovoProduto_Click"
            runat="server" Text="Nova Verba/Evento" />
    </div>
    <asp:ObjectDataSource ID="ODS_Produtos" runat="server" TypeName="CP.FastConsig.BLL.ODS_Produto"
        DataObjectTypeName="CP.FastConsig.DAL.Produto" SelectMethod="SelectGrid" SortParameterName="sortExpression"
        SelectCountMethod="SelectGridCount" EnablePaging="True" OnSelecting="ODS_Produtos_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdEmpresa" />
            <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="gridProdutos_Sorting" CssClass="EstilosGridView" PageSize="20"
        OnPageIndexChanging="grid_PageIndexChanging" AllowPaging="true" AllowSorting="true"
        EmptyDataText="Sem itens para exibição!" DataSourceID="ODS_Produtos" DataKeyNames="IDProduto"
        ID="gridProdutos" runat="server" Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="gridProdutos_SelectedIndexChanged"
        OnRowDataBound="gridProdutos_RowDataBound">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                <asp:LinkButton runat="server" ID="ImageButtonEditar" CommandArgument='<%# Eval("IDProduto") %>' OnClick="ImageButtonEditar_Click">
                    <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" /></asp:LinkButton>
                    <%--                    <asp:LinkButton runat="server" ID="ProdutosEditar" CommandArgument='<%# Eval("IDProduto") %>' OnClick="ProdutosEditar_Click">
                    </asp:LinkButton>
                    --%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?')"
                        runat="server" ID="ProdutosRemover" CommandArgument='<%# Eval("IDProduto") %>'
                        OnClick="ProdutosRemover_Click">
                        <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Nome" HeaderText="Nome Produto" DataField="Nome" />
            <asp:BoundField SortExpression="Verba" HeaderText="Verba" DataField="Verba" />
            <asp:TemplateField HeaderText="Consignante">
                <ItemTemplate>
                    <%# Eval("Empresa1.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField runat="server" ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn"
                HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
