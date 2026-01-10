<%@ Control ClassName="WebUserControlContatos" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlContatos.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlContatos" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <div>
        <div style="float: left;">
            <asp:TextBox class="TextBoxDropDownEstilos" Height="28px" Width="300px" runat="server"
                ID="TextBoxPesquisar" Visible="false" ></asp:TextBox>
        </div>
        <div style="float: left; margin-left: 5px;">
            <asp:Button ID="ButtonPesquisar" Visible="false" class="BotaoEstiloGlobal" OnClick="ButtonPesquisar_Click"
                Text="Buscar" runat="server" />
        </div>
    </div>
    <div style="width: 100%; height: 5px; clear: both; overflow: hidden;">
    </div>
    <div>
        <asp:Button ID="ButtonNovoContato" class="BotaoEstiloGlobal" OnClick="ButtonNovoContato_Click" 
            runat="server" Text="Novo" OnClientClick="window.scrollTo(0,0);" />
    </div>
    <asp:ObjectDataSource ID="ODS_EmpresaContatos" runat="server" TypeName="CP.FastConsig.BLL.ODS_EmpresaContato"
        DataObjectTypeName="CP.FastConsig.DAL.EmpresaContato" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_Contatos_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdEmpresa" />
            <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="gridContatos_Sorting" CssClass="EstilosGridView" PageSize="20"
        OnPageIndexChanging="grid_PageIndexChanging" AllowPaging="true" AllowSorting="true"
        EmptyDataText="Sem itens para exibição!" DataSourceID="ODS_EmpresaContatos" DataKeyNames="IDEmpresaContato"
        ID="gridContatos" runat="server" Width="100%" AutoGenerateColumns="false" 
        OnRowDataBound="gridContatos_RowDataBound">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                     <asp:LinkButton runat="server" ID="ImageButtonEditar" CommandArgument='<%# Eval("IDEmpresaContato") %>' OnClick="ImageButtonEditar_Click">
                    <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" /></asp:LinkButton>
                    <%--                    <asp:LinkButton runat="server" ID="ProdutosEditar" CommandArgument='<%# Eval("IDProduto") %>' OnClick="ProdutosEditar_Click">
                    </asp:LinkButton>
                    --%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?');"
                        runat="server" ID="ContatosRemover" CommandArgument='<%# Eval("IDEmpresaContato") %>'
                        OnClick="ContatosRemover_Click">
                        <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Nome" HeaderText="Nome" DataField="Nome" />
            <asp:BoundField SortExpression="Titulo" HeaderText="Título" DataField="Titulo" />
            <asp:TemplateField HeaderText="Tipo">
                <ItemTemplate>
                    <%# Eval("ContatoTipo.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Conteudo" HeaderText="Conteúdo" DataField="Conteudo" />
            <asp:CommandField runat="server" ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn"
                HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
