<%@ Control ClassName="WebUserControlAuditoriaAcesso" EnableViewState="true" Language="C#"
    AutoEventWireup="true" CodeBehind="WebUserControlAuditoriaAcesso.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAuditoriaAcesso" %>
<%@ Import Namespace="CP.FastConsig.Util" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
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
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" AutoPostBack="true"
                                DataTextField="Nome" Visible="false" DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
</div>
<div class="DivResultadoEstilos">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" HeaderStyle-Font-Bold="true" HeaderText="Auditoria de Acesso"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent runat="server" ID="PanelPesquisar" SupportsDisabledAttribute="True">
                <div id="DivBusca">
                    <h1 id="TituloPesquisa">
                        Faça aqui sua pesquisa:</h1>
                    <div class="MargemTopo">
                        <asp:Panel ID="PanelPesquisaFuncionarios" DefaultButton="btnBuscar" runat="server">
                            <div class="FlutuaEsquerdaMargemDireita">
                                <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Paddings-PaddingLeft="5px" ID="ASPxTextBoxBusca"
                                    EnableDefaultAppearance="false" EnableTheming="false" runat="server" Width="300px"
                                    Height="30px">
                                </dx:ASPxTextBox>
                            </div>
                            <div class="FlutuaEsquerda">
                                <dx:ASPxButton Cursor="pointer" CssClass="BotaoEstiloGlobal" Font-Bold="true" EnableDefaultAppearance="false"
                                    EnableTheming="false" ID="btnBuscar" runat="server" Text="Buscar" OnClick="Buscar_Click">
                                </dx:ASPxButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <asp:ObjectDataSource ID="ODS_AuditoriaAcesso" runat="server" TypeName="CP.FastConsig.BLL.ODS_AuditoriaAcesso"
        DataObjectTypeName="CP.FastConsig.DAL.AuditoriaAcesso" SelectMethod="SelectGrid"
        SortParameterName="sortExpression" SelectCountMethod="SelectGridCount" EnablePaging="True"
        OnSelecting="ODS_AuditoriaAcesso_Selecting" OnSelected="ODS_AuditoriaAcesso_Selected">
        <SelectParameters>
            <asp:Parameter Name="nameSearchString" Type="String" />
            <asp:Parameter Name="IdEmpresa" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView ID="GridViewLista" runat="server" OnDataBound="grid_DataBound"
        CssClass="EstilosGridView" PageSize="15" OnPageIndexChanging="grid_PageIndexChanging"
        AllowPaging="true" AllowSorting="true" EnablePersistedSelection="true" DataSourceID="ODS_AuditoriaAcesso"
        DataKeyNames="IDAuditoriaAcesso" Width="100%" 
        AutoGenerateColumns="false">
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
            <%-- <asp:Button ID="btnFirst" runat="server" Text="Primeiro" CommandArgument="First" CommandName="Page" />
                <asp:Button ID="btnPrevious" runat="server" Text="Anterior" CommandArgument="Prev"
                    CommandName="Page" />
                <asp:Button ID="btnNext" runat="server" Text="Próximo" CommandArgument="Next" CommandName="Page" />
                <asp:Button ID="btnLast" runat="server" Text="Último" CommandArgument="Last" CommandName="Page" />--%>
        </PagerTemplate>
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <PagerSettings Visible="true" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ToolTip="Visualizar" ID="ImageButtonSelecionar" runat="server" CommandName="select"
                        ImageUrl="~/imagens/BtnProcurar.png" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Data" />
            <asp:TemplateField HeaderText="Módulo">
                <ItemTemplate>
                    <%# Eval("Modulo.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Opção">
                <ItemTemplate>
                    <%# Eval("Recurso.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ação">
                <ItemTemplate>
                    <%# Eval("Descricao") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Navegador">
                <ItemTemplate>
                    <%# Eval("Navegador") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Empresa">
                <ItemTemplate>
                    <%# Eval("Empresa.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Perfil">
                <ItemTemplate>
                    <%# Eval("Perfil.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Usuário">
                <ItemTemplate>
                    <%# Eval("Usuario.Nome") %>
                </ItemTemplate>
            </asp:TemplateField>
<%--            <asp:BoundField SortExpression="NomeRegimeFolha" HeaderText="Regime" DataField="NomeRegimeFolha" />
            <asp:BoundField SortExpression="NomeSituacao" HeaderText="Situação" DataField="NomeSituacao" />
--%>        </Columns>
    </asp:GridView>
    <h1 class="TextoAncora" id="TextoAncora" runat="server" visible="false">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
    </h1>
</div>
