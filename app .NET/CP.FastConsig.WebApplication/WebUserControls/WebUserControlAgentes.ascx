<%@ Control ClassName="WebUserControlAgentes" Language="C#" AutoEventWireup="True"
    CodeBehind="WebUserControlAgentes.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAgentes" %>
<div class="GlobalUserControl">
    <div id="DivBusca">
        <h1 id="TituloPesquisa">
            Faça aqui sua pesquisa:</h1>
        <asp:Panel CssClass="MargemTopoDivBusca" runat="server" DefaultButton="ButtonPesquisar"
            ID="PanelPesquisaConsignatariasCta">
            <div style="float: left;">
                <asp:TextBox class="TextBoxDropDownEstilos" Height="27" Width="300px" runat="server"
                    ID="TextBoxPesquisar"></asp:TextBox>
            </div>
            <div style="float: left; margin-left: 5px;">
                <asp:Button ID="ButtonPesquisar" class="BotaoEstiloGlobal"
                    Text="Buscar" runat="server" /></div>
        </asp:Panel>
    </div>
    <div style="clear: both;">
        <div style="height: 5px;">
            &nbsp;</div>
        <asp:Button ID="ButtonNovo" OnClick="ButtonNovo_Click" class="BotaoEstiloGlobal"
            runat="server" Text="Nova" />
    </div>
    <div>
        <asp:ObjectDataSource OnSelecting="ODS_Consignatarias_Selecting" ID="ODS_Consignatarias" runat="server" TypeName="CP.FastConsig.BLL.ODS_Empresa"
            DataObjectTypeName="CP.FastConsig.DAL.Empresa" SelectMethod="SelectGridAgentes" SortParameterName="sortExpression"
            SelectCountMethod="SelectGridCountAgentes" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text" Type="String" />
                <asp:Parameter Name="idConsignataria" DefaultValue="0" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView OnSorting="grid_Sorting" CssClass="EstilosGridView" OnPageIndexChanging="grid_PageIndexChanging"
            AllowPaging="True" AllowSorting="True" OnDataBound="grid_DataBound" PageSize="20"
            EmptyDataText="Sem itens para exibição!" DataKeyNames="IDEmpresa" ID="grid" runat="server"
            Width="100%" AutoGenerateColumns="False" DataSourceID="ODS_Consignatarias">
            <HeaderStyle CssClass="CabecalhoGridView" />
            <PagerTemplate>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:ImageButton runat="server" Width="19" Height="19" ID="BtnFirst" ImageUrl="~/Imagens/first.png"
                                CommandArgument="First" CommandName="Page" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" Width="19" Height="19" ID="BtnPrevious" ImageUrl="~/Imagens/prev.png"
                                CommandArgument="Prev" CommandName="Page" />
                        </td>
                        <td>
                            Página
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="65" OnSelectedIndexChanged="dropwdown_SelectedIndexChangend"
                                ID="DropDownPagina" runat="server" AutoPostBack="true" />
                            de
                            <asp:Label ID="LabelPaginas" runat="server" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" Width="19" Height="19" ID="BtnNext" ImageUrl="~/Imagens/next.png"
                                CommandArgument="Next" CommandName="Page" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" Width="19" Height="19" ID="BtnLast" ImageUrl="~/Imagens/last.png"
                                CommandArgument="Last" CommandName="Page" />
                        </td>
                    </tr>
                </table>
            </PagerTemplate>
            <RowStyle CssClass="LinhaListaGridView" />
            <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
            <PagerStyle CssClass="PaginadorGridView" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="3%">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="EditarEmpresa" OnClick="ImageButtonEditar_Click"
                            CommandArgument='<%# Eval("IDEmpresa") %>'>
                            <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  ItemStyle-Width="3%">
                    <ItemTemplate>
                        <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?')"
                            runat="server" ID="ImageButtonRemover" CommandArgument='<%# Eval("IDEmpresa") %>'
                            OnClick="ImageButtonRemover_Click">
                            <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField SortExpression="Fantasia" HeaderText="Nome" DataField="Nome" />
                <asp:BoundField SortExpression="Sigla" HeaderText="Sigla" DataField="Sigla" />
                <asp:BoundField SortExpression="Fone1" HeaderText="Telefone" DataField="Fone1" />
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <%# Eval("EmpresaTipo.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Situação">
                    <ItemTemplate>
                        <%# Eval("EmpresaSituacao.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <h1 class="TextoAncora">
            <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
                Topo da Página</a>
        </h1>
    </div>
</div>