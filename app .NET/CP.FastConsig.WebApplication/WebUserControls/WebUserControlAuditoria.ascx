<%@ Control ClassName="WebUserControlAuditoria" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlAuditoria.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAuditoria" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelAuditoria" ShowHeader="true" Width="100%"
        HeaderText="Auditoria" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentAuditoria" runat="server" SupportsDisabledAttribute="True">

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
                <asp:GridView CssClass="EstilosGridView" PageSize="20"
                            AllowPaging="true" AllowSorting="true"
                            EmptyDataText="Sem itens para exibição!" 
                            ID="gridAuditoria" runat="server" Width="100%" AutoGenerateColumns="false">
                <HeaderStyle CssClass="CabecalhoGridView" />
                <RowStyle CssClass="LinhaListaGridView" />
                <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
                <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                    <Columns>
<asp:TemplateField HeaderText="Usuário">
<ItemTemplate>
<%# Eval("Usuario.NomeCompleto") %>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Ação Realizada">
<ItemTemplate>
<%# Eval("AuditoriaOperacao.Nome") %>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Página Acessada">
<ItemTemplate>
<%# (Eval("IDRecurso") != null) ? Eval("Recurso.Nome") : "" %>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Recurdo Afetado" DataField="Descricao" />
                        <asp:BoundField HeaderText="Data/Hora" DataField="Data" />
<asp:TemplateField HeaderText="Perfil">
<ItemTemplate>
<%# Eval("Usuario.Perfil")%>
</ItemTemplate>
</asp:TemplateField>
                        <asp:BoundField HeaderText="IP" DataField="IP" />
                        <asp:BoundField HeaderText="Browse" DataField="Browser" />
                    </Columns>
                </asp:GridView>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="window.scrollTo(0,0); return false;">Topo da Página</a>
    </h1>
</div>



            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

