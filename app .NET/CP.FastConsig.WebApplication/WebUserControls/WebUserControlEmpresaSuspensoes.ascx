<%@ Control ClassName="WebUserControlEmpresaSuspensoes" Language="C#" AutoEventWireup="true"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlEmpresaSuspensoes.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlEmpresaSuspensoes" %>
<div class="GlobalUserControl">
    <div>
        <div style="float: left;">
            <asp:TextBox class="TextBoxDropDownEstilos" Height="28px" Width="300px" runat="server"
                ID="TextBoxPesquisar"></asp:TextBox>
        </div>
        <div style="float: left; margin-left: 5px;">
            <asp:Button ID="ButtonPesquisar" class="BotaoEstiloGlobal" OnClick="ButtonPesquisar_Click"
                Text="Buscar" runat="server" />
        </div>
    </div>
    <div style="width: 100%; height: 5px; clear: both; overflow: hidden;">
    </div>
    <div>
        <asp:Button ID="ButtonNovo" class="BotaoEstiloGlobal" OnClick="ButtonNovo_Click"
            runat="server" Text="Novo Bloqueio/Suspensão" />
    </div>
    <asp:ObjectDataSource ID="ODS_EmpresaSuspensoes" runat="server" TypeName="CP.FastConsig.BLL.ODS_EmpresaSuspensao"
        DataObjectTypeName="CP.FastConsig.DAL.EmpresaSuspensao" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_Suspensoes_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdEmpresa" />
            <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="gridSuspensoes_Sorting" CssClass="EstilosGridView" PageSize="20"
        ClientIDMode="Static" OnPageIndexChanging="grid_PageIndexChanging" AllowPaging="true"
        AllowSorting="true" EmptyDataText="Sem itens para exibição!" DataSourceID="ODS_EmpresaSuspensoes"
        DataKeyNames="IDEmpresaSuspensao" ID="gridSuspensoes" runat="server" Width="100%"
        AutoGenerateColumns="false" OnSelectedIndexChanged="gridSuspensoes_SelectedIndexChanged"
        OnRowDataBound="gridSuspensoes_RowDataBound">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="ImageButtonEditar" CommandArgument='<%# Eval("IDEmpresaSuspensao") %>'
                        OnClick="ImageButtonEditar_Click">
                        <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="3%">
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?')"
                        runat="server" ID="SuspensoesRemover" CommandArgument='<%# Eval("IDEmpresaSuspensao") %>'
                        OnClick="SuspensoesRemover_Click">
                        <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="Data" HeaderText="Data" DataField="Data" />
            <asp:BoundField SortExpression="Motivo" HeaderText="Motivo" DataField="Motivo" />
            <asp:TemplateField HeaderText="Situação">
                <ItemTemplate>
                    <%# Eval("EmpresaSituacao.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo Período">
                <ItemTemplate>
                    <%# Eval("TipoPeriodo").ToString() == "I" ? "Indeterminado" : "Determinado" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField runat="server" ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn"
                HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="window.scrollTo(0,0); return false;">Topo da Página</a>
    </h1>
</div>
