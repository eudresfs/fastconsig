<%@ Control ClassName="WebUserControlUsuariosPermissoes" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlUsuariosPermissoes.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlUsuariosPermissoes" %>
<%@ Import Namespace="CP.FastConsig.Util" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <asp:Panel DefaultButton="ButtonNovoUsuario" runat="server" ID="PanelPesquisaUsuario">
        <asp:Panel DefaultButton="ButtonPesquisarUsuarios" runat="server" ID="PanelPesquisaUsuariosPerm">
            <table border="0" cellpadding="0" cellspacing="4" >
                <tr>
                    <td>
                        <asp:TextBox class="TextBoxDropDownEstilos" Width="300px" Height="28px" runat="server"
                            ID="TextBoxPesquisarUsuarios"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="ButtonPesquisarUsuarios" class="BotaoEstiloGlobal" OnClick="ButtonPesquisarUsuarios_Click"
                            Text="Buscar" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div>
            <table border="0" cellpadding="0" cellspacing="2" >
                <tr>
                    <td>
                        <dx:ASPxComboBox Width="305" Height="28" ID="ASPxComboBoxModulos" AutoPostBack="True"
                            OnSelectedIndexChanged="ASPxComboBoxModulos_SelectedIndexChanged" Border-BorderColor="#c0dfe8"
                            runat="server" TextField="Nome" ValueField="IDModulo" ValueType="System.Int32"
                            LoadingPanelImagePosition="Top" ShowShadow="False">
                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                            </LoadingPanelImage>
                            <DropDownButton>
                                <Image>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                </Image>
                            </DropDownButton>
                            <ValidationSettings>
                                <ErrorFrameStyle ImageSpacing="4px">
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                    <td>
                        <dx:ASPxComboBox Width="304" Height="28" Visible="False" Border-BorderColor="#c0dfe8"
                            ID="ASPxComboBoxBancos" AutoPostBack="True" OnSelectedIndexChanged="ASPxComboBoxBancos_SelectedIndexChanged"
                            runat="server" TextField="Nome" ValueField="IDEmpresa" ValueType="System.Int32"
                            LoadingPanelImagePosition="Top">
                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                            </LoadingPanelImage>
                            <DropDownButton>
                                <Image>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                </Image>
                            </DropDownButton>
                            <ValidationSettings>
                                <ErrorFrameStyle ImageSpacing="4px">
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding:3px 0px 2px 1px;">
                        <asp:Button class="BotaoEstiloGlobal" ID="ButtonNovoUsuario" OnClick="ButtonNovoUsuario_Click"
                            runat="server" Text="Novo" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div>
        <asp:ObjectDataSource ID="ODS_Usuarios" runat="server" TypeName="CP.FastConsig.BLL.ODS_Usuario"
            DataObjectTypeName="CP.FastConsig.DAL.Usuario" OnSelected="ODS_Usuarios_Selected"
            SelectMethod="SelectGrid" SortParameterName="sortExpression" SelectCountMethod="SelectGridCount"
            EnablePaging="True" OnSelecting="ODS_Usuarios_Selecting">
            <SelectParameters>
                <asp:Parameter DbType="Int32" Name="IdEmpresa" />
                <asp:Parameter DbType="Int32" Name="idModulo" />
                <asp:ControlParameter ControlID="TextBoxPesquisarUsuarios" Name="nameSearchString"
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView OnSorting="GridViewUsuarios_Sorting" ShowFooter="true" OnRowDataBound="GridViewUsuarios_RowDataBound"
            OnDataBound="grid_DataBound" PageSize="20" OnPageIndexChanging="GridViewUsuarios_PageIndexChanging"
            AllowPaging="true" AllowSorting="true" CssClass="EstilosGridView" EmptyDataText="Sem itens para exibição!"
            DataKeyNames="IDUsuario" DataSourceID="ODS_Usuarios" ID="GridViewUsuarios" runat="server"
            Width="100%" AutoGenerateColumns="false">
            <HeaderStyle CssClass="CabecalhoGridView" />
            <FooterStyle BackColor="#2ba5c9" ForeColor="#ffffff" BorderStyle="None" />
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
            <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:ImageButton AlternateText="Editar" ToolTip="Editar" ImageUrl="~/Imagens/gtk-edit.png" OnClick="ImageButtonEditar_Click" runat="server" ID="ImageButtonEditar"
                            CommandArgument='<%# Eval("IDUsuario") %>'></asp:ImageButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label Font-Bold="true" ID="LabelTotal" runat="server"></asp:Label></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton OnClientClick='<%# "return confirm(\"Tem certeza de que deseja remover o usuário " + Eval("NomeCompleto") + "?\");"  %>'
                            runat="server" ID="ImageButtonRemover" CommandArgument='<%# Eval("IDUsuario") %>'
                            OnClick="ImageButtonRemover_Click">
                            <img alt="Remover" title="Remover" id="ImgRemover" runat="server" src="~/Imagens/trash_16x16.gif" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField SortExpression="NomeCompleto" HeaderText="Nome" DataField="NomeCompleto" />
                <asp:TemplateField HeaderText="CPF">
                    <ItemTemplate>
                        <%# StringHelper.MaskString(Eval("CPF").ToString(), "###.###.###-##") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField SortExpression="Email" HeaderText="E-Mail" DataField="Email" />
                <asp:BoundField SortExpression="ApelidoLogin" HeaderText="Login" DataField="ApelidoLogin" />
                <asp:BoundField SortExpression="Celular" HeaderText="Telefone" DataField="Celular" />
                <asp:TemplateField HeaderText="Perfil">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="LabelPerfil" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
            Topo da Página</a>
    </h1>
</div>
