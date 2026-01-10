<%@ Control ClassName="WebUserControlImportacaoFiltros" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlImportacaoFiltros.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlImportacaoFiltros" %>
<div style="display: none">
    <p runat="server" class="classeFiltros" id='pFiltros'>
        <input type="hidden" id="InputColunaFiltro" runat="server" />
        <select id="SelectTipoFiltro" runat="server" class="DropdownlistUserControlDyn">
            <option value="Trocar">Trocar</option>
            <option value="Direita">Acrescentar à direita</option>
            <option value="Esquerda">Acrescentar à esquerda</option>
            <option value="Mascara">Aplicar máscara</option>
            <option value="Maior">Trocar valores maiores que</option>
            <option value="Menor">Trocar valores menores que</option>
        </select>
        <input type="text" class="textarea" id="InputFiltroTexto" runat="server" />
        por
        <input type="text" class="textarea" id="InputFiltroTroca" runat="server" />
        <input type="hidden" value="#FIM" runat="server" />
        <br />
        <br />
    </p>
</div>
<div id="DivFiltroColuna" runat="server">
</div>
<asp:LinkButton runat="server" ID="LinkButtonAdicionar" Text="Adicionar"></asp:LinkButton>&nbsp;<asp:LinkButton
    runat="server" ID="LinkButtonRemover" Text="Remover"></asp:LinkButton>
<input type="hidden" value="#" />