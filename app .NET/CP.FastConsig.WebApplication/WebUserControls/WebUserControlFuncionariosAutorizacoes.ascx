<%@ Control ClassName="WebUserControlFuncionarios_Autorizacoes" Language="C#" AutoEventWireup="True"
    EnableViewState="true" ViewStateMode="Enabled" CodeBehind="WebUserControlFuncionariosAutorizacoes.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFuncionariosAutorizacoes" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div>
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosUsuario" Width="100%" HeaderText="Dados do Funcionário"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
                            <b>Matrícula:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelMatriculaFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito">
                            <b>Nome:</b>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TituloNegrito" style="width: 20%;">
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
<br />
<div class="GlobalUserControl">
    <a name="topo" runat="server" href="#topo"></a>
    <div>
        <asp:TextBox class="TextBoxDropDownEstilos" Width="300px" runat="server" ID="TextBoxPesquisar"></asp:TextBox>
        <asp:Button ID="ButtonPesquisar" CssClass="BotaoEstiloGlobal" OnClick="ButtonPesquisar_Click"
            Text="Buscar" runat="server" />
    </div>
    <div style="margin: 5px 0px;">
        <asp:Button ID="ButtonNovo" CssClass="BotaoEstiloGlobal" OnClick="ButtonNovoProduto_Click"
            runat="server" Text="Novo" />
    </div>
    <asp:ObjectDataSource ID="ODS_FuncAutorizacoes" runat="server" TypeName="CP.FastConsig.BLL.ODS_FuncAutorizacoes"
        DataObjectTypeName="CP.FastConsig.DAL.FuncionarioAutorizacao" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="IdFuncionario" />
            <asp:ControlParameter ControlID="TextBoxPesquisar" Name="nameSearchString" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView OnSorting="Grid_Sorting" CssClass="EstilosGridView" PageSize="20" OnPageIndexChanging="grid_PageIndexChanging"
        AllowPaging="true" AllowSorting="true" EmptyDataText="Sem itens para exibição!"
        DataSourceID="ODS_FuncAutorizacoes" DataKeyNames="IDFuncionarioAutorizacao" ID="grid"
        runat="server" Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="grid_SelectedIndexChanged"
        OnRowDataBound="grid_RowDataBound">
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
                        <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="65" 
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
      <PagerStyle CssClass="PaginadorGridView" HorizontalAlign="Center" />
      <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <Columns>
            <asp:BoundField HeaderText="Data" DataField="AutorizacaoData" />
            <asp:BoundField HeaderText="Dias de Validade" DataField="AutorizacaoValidade" />
            <%--<asp:BoundField HeaderText="Tipo" DataField="FuncionarioAutorizacaoTipo.Nome" />--%>
            <asp:TemplateField HeaderText="Tipo">
                <ItemTemplate>
                    <%# Eval("FuncionarioAutorizacaoTipo.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Motivo" DataField="Motivo" />
            <asp:CommandField runat="server" ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn"
                HeaderStyle-CssClass="HiddenColumn" />
            <asp:TemplateField>
                <ItemTemplate>
                    <img alt="Editar" title="Editar" id="ImgEditar" runat="server" src="~/Imagens/gtk-edit.png" />
                    <%--                    <asp:LinkButton runat="server" ID="ProdutosEditar" CommandArgument='<%# Eval("IDProduto") %>' OnClick="ProdutosEditar_Click">
                    </asp:LinkButton>
                    --%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton OnClientClick="return confirm('Tem certeza que deseja remover o item clicado?')"
                        runat="server" ID="Remover" CommandArgument='<%# Eval("IDFuncionarioAutorizacao") %>'
                        OnClick="Remover_Click">
                        <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
