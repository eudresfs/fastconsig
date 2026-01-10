<%@ Control Language="C#" ClassName="WebUserControlMinhasAverbacoes" AutoEventWireup="true" CodeBehind="WebUserControlMinhasAverbacoes.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlMinhasAverbacoes" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div style="margin:10px;">
    <asp:CheckBox ID="ckbApenasEmFolha" runat="server" Text="Apenas as Averbações que são Descontáveis em Folha" AutoPostBack="true" OnCheckedChanged="selecionaApenasDesc_Click" />
</div>
      <dx:ASPxRoundPanel ID="ASPxRoundPanelEmprestimos" HeaderStyle-Font-Bold="true" runat="server"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"  CssPostfix="Aqua"
        HeaderText="Empréstimos" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="0px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">

    <asp:GridView ID="GridViewAverbacoes" runat="server" 
        CssClass="EstilosGridView" PageSize="15" 
        AllowPaging="true" AllowSorting="true" EnablePersistedSelection="true" 
        DataKeyNames="IDAverbacao" Width="100%" EmptyDataText="Este funcionário não possui contratos ativos!" OnRowDataBound="grid_RowDataBound"
        AutoGenerateColumns="false" OnSelectedIndexChanged="Seleciona_Averbacao_Click">
        <HeaderStyle CssClass="CabecalhoGridView" />
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
            <asp:BoundField SortExpression="Numero" ItemStyle-HorizontalAlign="Left" HeaderText="Número" DataField="Numero" />
            <asp:BoundField SortExpression="Data" ItemStyle-HorizontalAlign="Left" HeaderText="Data" DataField="Data" DataFormatString="{0:d}" />
            <asp:BoundField SortExpression="ValorContratado" HeaderText="Valor" DataField="ValorContratado"  ItemStyle-HorizontalAlign="Left" DataFormatString="{0:N}"/>
            <asp:BoundField SortExpression="Prazo" HeaderText="Prazo" DataField="Prazo" />
            <asp:BoundField SortExpression="ValorParcela" HeaderText="Valor Parcela" DataField="ValorParcela" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:N}"/>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Situação">
                <ItemTemplate>
                    <%# Eval("AverbacaoSituacao.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>                                                
		    <asp:TemplateField HeaderText="Parcelas Pagas">
				<ItemTemplate>                    
                    <asp:Label ID="Label1" runat="server"></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>            
		    <asp:TemplateField HeaderText="Últ. Saldo Devedor">
				<ItemTemplate>                        
                   <asp:Label ID="Label2" runat="server"></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>            
            <asp:TemplateField>
                <HeaderTemplate><center>Solicitar Saldo</center></HeaderTemplate>
                <ItemTemplate>
                    <%--<asp:ImageButton ToolTip="Solicitar Saldo" ID="ImageButtonSolicitarSaldo" runat="server" CommandName="select"
                        ImageUrl="~/imagens/tick.png" />--%>
                        <center>
						<asp:LinkButton runat="server" ID="SolicitarSaldo" OnClick="ImageButtonEditar_Click" CommandArgument='<%# Eval("IDAverbacao") %>' >
							<img alt="Solicitar Saldo" title="Solicitar Saldo" id="ImgSolicitarSaldo" runat="server" src="~/Imagens/tick.png" /></asp:LinkButton>
                        </center>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxRoundPanel>

    <br />

    <dx:ASPxRoundPanel ID="ASPxRoundPanelOutrosProdutos" HeaderStyle-Font-Bold="true" runat="server"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"  CssPostfix="Aqua"
        HeaderText="Outros Produtos" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="0px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

    <asp:GridView ID="GridViewOutrosProdutos" runat="server" 
        CssClass="EstilosGridView" PageSize="15" 
        AllowPaging="true" AllowSorting="true" EnablePersistedSelection="true" 
        DataKeyNames="IDAverbacao" Width="100%" EmptyDataText="Este funcionário não possui contratos ativos!" OnRowDataBound="gridOutros_RowDataBound"
        AutoGenerateColumns="false" OnSelectedIndexChanged="Seleciona_AverbacaoOutro_Click">
        <HeaderStyle CssClass="CabecalhoGridView" />
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
            <asp:BoundField SortExpression="Numero" ItemStyle-HorizontalAlign="Left" HeaderText="Número" DataField="Numero" />
            <asp:BoundField SortExpression="Data" ItemStyle-HorizontalAlign="Left" HeaderText="Data" DataField="Data" DataFormatString="{0:d}" />
            <asp:BoundField SortExpression="ValorContratado" HeaderText="Valor" DataField="ValorContratado"  ItemStyle-HorizontalAlign="Left" DataFormatString="{0:N}"/>
            <asp:BoundField SortExpression="Prazo" HeaderText="Prazo" DataField="Prazo" />
            <asp:BoundField SortExpression="ValorParcela" HeaderText="Valor Parcela" DataField="ValorParcela" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:N}"/>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Situação">
                <ItemTemplate>
                    <%# Eval("AverbacaoSituacao.Nome")%>
                </ItemTemplate>
            </asp:TemplateField>                                                
		    <asp:TemplateField HeaderText="Parcelas Pagas">
				<ItemTemplate>                    
                    <asp:Label ID="Label1" runat="server"></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>            
            <asp:TemplateField>
                <HeaderTemplate><center>Solicitar Informação</center></HeaderTemplate>
                <ItemTemplate>
                        <center>                                                                                    <%--'<%# Eval('IDAverbacao') %>'--%>
						<asp:LinkButton runat="server" ID="SolicitarInformacao" OnClientClick="javascript:chamaDialogo(); return false;">
							<img alt="Solicitar Informação" title="Solicitar Informação" id="ImgSolicitarInfo" runat="server" src="~/Imagens/tick.png" /></asp:LinkButton>
                        </center>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxRoundPanel>

<div id="dlgconfirmacao" title="Solicitação de Informação" style="display: none;">
<table border="0" cellpadding="0" cellspacing="1" width="100%" style="background-color: #8FD8D8;">
<tr>
<td valign="top" style="border-bottom: none; width: 39%; padding: 2px; background-color: #ffffff;">
<table class="WebUserControlTabelaConfirmaAverbacoes" border="0" cellpadding="0"
cellspacing="0" width="100%">
<tr>
<td>
    <asp:TextBox CssClass="TextBoxSemBg" Rows="5" TextMode="MultiLine" runat="server" ID="TextBoxObs" Enabled="true" ClientIDMode="Static"></asp:TextBox>
</td>
</tr>

</table>
</td>
</tr>
</table>
</div>
