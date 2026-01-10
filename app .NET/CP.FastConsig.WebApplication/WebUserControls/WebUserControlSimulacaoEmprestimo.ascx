<%@ Control ClassName="WebUserControlSimulacaoEmprestimo" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlSimulacaoEmprestimo.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlSimulacaoEmprestimo" %>
<div style="text-align: left;">
    <asp:Button class="BotaoEstiloGlobal"  ID="ButtonNovaSimulacao" OnClick="ButtonNovaSimulacao_Click"
        runat="server" Text="Nova Simulação" />
    &nbsp;
    <asp:Button class="BotaoEstiloGlobal"  ID="ButtonImprimir" OnClick="ButtonImprimir_Click"
        runat="server" Text="Imprimir" />
</div>
<div>
    <asp:GridView OnRowDataBound="GridViewSimulacaoEmprestimo_RowDataBound" PageSize="20" AllowPaging="true" AllowSorting="true" CssClass="EstilosGridView"
        EmptyDataText="Sem itens para exibição!" ID="GridViewSimulacaoEmprestimo"
        runat="server" Width="100%" AutoGenerateColumns="false">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
        <Columns>
            <asp:TemplateField HeaderText="Solicitar Contato">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("Ranking") %>' ID="ImageButtonSolicitarContato" OnClick="ImageButtonSolicitarContato_Click">
                        <img alt="Solicitar Contato" title="Solicitar Contato" id="ImgSolicContato" runat="server" src="~/Imagens/business-contact.png" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reservar">
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ImageButtonPreReservar" OnClick="ImageButtonPreReservar_Click" ImageUrl="~/Imagens/Money32x32.png" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Ranking" DataField="Ranking"/>
            <asp:ImageField HeaderText="" DataImageUrlField="Logo"/>
            <asp:BoundField HeaderText="Banco" DataField="Banco" />
            <asp:BoundField DataFormatString="{0:c}" HeaderText="Valor da Parcela" DataField="ValorParcela" />
            <asp:BoundField DataFormatString="{0:c}" HeaderText="Valor da Averbacao" DataField="ValorAverbacao" />
            <asp:BoundField HeaderText="CET" DataField="Taxa" />
            <asp:BoundField HeaderText="1º Desconto" DataField="Competencia" />
            <asp:BoundField HeaderText="Prazo" DataField="Prazo" />            
            <asp:TemplateField HeaderText="Viabilidade">
                <ItemTemplate>
                    <asp:Image runat="server" ID="ImageViabilidade" />
                </ItemTemplate>
            </asp:TemplateField>        
        </Columns>
    </asp:GridView>
</div>
