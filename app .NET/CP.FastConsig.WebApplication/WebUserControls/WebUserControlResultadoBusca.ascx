<%@ Control ClassName="WebUserControlResultadoBusca" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlResultadoBusca.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlResultadoBusca" %>
<div>
    <asp:GridView OnRowDataBound="GridViewResultadoBuscaFuncionarios_RowDataBound" DataKeyNames="IDUsuario"
        Caption="Funcionários" ShowHeader="false" CssClass="EstilosGridView"  EmptyDataText="Sem resultados para exibição!"
        Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewResultadoBuscaFuncionarios_SelectedIndexChanged"
        runat="server" ID="GridViewResultadoBuscaFuncionarios">
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="45">
                <ItemTemplate>
                    <asp:Image BorderWidth="0" ID="ImagemFloat" runat="server" ImageUrl="~/Imagens/Perfil.png"
                        Width="45px" Height="45px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label CssClass="TituloResultadoBusca" runat="server" ID="LabelNome" Text='<%# Eval("NomeCompleto") %>'></asp:Label><br />
                    <asp:Label runat="server" ID="LabelPerfil" Text='<%# Eval("Perfil") %>'></asp:Label><br />
                    <asp:Label runat="server" ID="LabelEmail" Text='<%# Eval("Email") %>'></asp:Label><br />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn" HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
</div>
<div>
    <asp:GridView OnSelectedIndexChanged="GridViewResultadoBuscaAverbacaos_SelectedIndexChanged" OnRowDataBound="GridViewResultadoBuscaAverbacaos_RowDataBound"
        DataKeyNames="IDAverbacao" Caption="Averbações" ShowHeader="true" CssClass="EstilosGridViewAverbacoesDadosFuncionario"
        EmptyDataText="Sem resultados para exibição!" Width="100%" AutoGenerateColumns="false"
        runat="server" ID="GridViewResultadoBuscaAverbacaos">
        <RowStyle CssClass="LinhaListaGridViewAverbacoesDadosFuncionario" />
        <HeaderStyle CssClass="CabecalhoGridViewAverbacoesDadosFuncionario" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridViewAverbacoesDadosFuncionario" />
        <Columns>
            <asp:BoundField DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data" DataField="Data" />
            <asp:BoundField HeaderText="Número" DataField="Numero" />
            <asp:BoundField HeaderText="Prazo" DataField="Prazo" />
            <asp:BoundField HeaderText="Parcela" DataField="ValorParcela" />
            <asp:CommandField ShowSelectButton="true" ItemStyle-CssClass="HiddenColumn" HeaderStyle-CssClass="HiddenColumn" />
        </Columns>
    </asp:GridView>
</div>
