<%@ Control ClassName="WebUserControlImprimirAposentadoria" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlImprimirAposentadoria.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlImprimirAposentadoria" %>
<div id="DivTermo" class="printable" title="Aposentadoria de Funcionário">
    <div style="text-align: center;">
        <table border="0" cellpadding="0" cellspacing="0" width="99%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
            <tr>
                <td style="width: 160px; text-align: center;">
                    <%--<img runat="server" id="ImgLogoBanco" src="~/Imagens/Logos/Bancos/BIG_BV.png" alt="" />--%>
                </td>
                <td>
                    <h1 style="font-size: 10pt; font-weight: bold;">
                        Aposentadoria de Funcionário</h1>
                </td>
                <td style="width: 160px; text-align: center;">
                    <%-- <img runat="server" id="ImgLogoConsignante" src="~/Imagens/LogoItaqua.png" alt="" />--%>
                </td>
            </tr>
        </table>
    </div>
    <table border="0" cellpadding="2" cellspacing="1" width="99%" class="TabelaTermoDeImpressao"
        style="border: 1px solid #000000; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
        font-size: 8.5pt;">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                    font-size: 8pt;">
                    <tr>
                        <td style="width: 15%; padding: 1px; text-align: left;">
                            Data:
                        </td>
                        <td style="padding: 1px; text-align: left;">
                            <asp:Label runat="server" ID="impData" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                    font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td colspan="2" style="border-bottom: 1px solid #000000;">
                            <h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                                display: inline;">
                                Dados Pessoais:</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding: 1px; text-align: left;">
                            Nome:
                        </td>
                        <td style="padding: 1px; text-align: left;">
                            <asp:Label runat="server" ID="impNome" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px; text-align: left;">
                            CPF:
                        </td>
                        <td style="padding: 1px; text-align: left;">
                            <asp:Label runat="server" ID="impCPF" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            RG:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impRG" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Data Nasc.:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impDataNasc" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Endereço:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impEndereco" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Bairro:
                        </td>
                        <td style="text-align: left;">
                            <asp:Label runat="server" ID="impBairro" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Cidade:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impCidade" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            UF:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impUF" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Cep:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impCep" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Email:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impEmail" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Telefone:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impTelefone" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Celular:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impCelular" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                    font-size: 8.5pt;" class="TabelaTermoDeImpressaoPrimeirosDados" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td colspan="2" style="border-bottom: 1px solid #000000;">
                            <h1 style="font-size: 9pt; font-weight: bold; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;
                                display: inline;">
                                Dados Funcionais:</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px; width: 15%;text-align: left;">
                            Matrícula:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impMatricula" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Local:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impLocal" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Setor:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impSetor" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Cargo:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label runat="server" ID="impCargo" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Categoria:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label ID="impCategoria" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Regime:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label ID="impRegime" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Data Adm:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label ID="impDataAdm" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;text-align: left;">
                            Situação:
                        </td>
                        <td style="padding: 1px;text-align: left;">
                            <asp:Label ID="impSituacao" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="DivAverbacoes" style="padding: 5px 0px;"  title="Averbações de Empréstimo">
    <asp:GridView ID="GridViewAverbacoes" runat="server" 
        CssClass="EstilosGridView"  DataKeyNames="IDAverbacao" Width="100%" 
        AutoGenerateColumns="false">
        <HeaderStyle CssClass="CabecalhoGridView" />
        <RowStyle CssClass="LinhaListaGridView" />
        <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
        <Columns>
            <asp:BoundField HeaderText="Numero" DataField="Numero" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Consignatária" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%# Eval("Empresa1.Fantasia") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Valor Parcela" DataField="ValorParcela" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField HeaderText="Prazo" DataField="Prazo" ItemStyle-HorizontalAlign="Left"/>
        </Columns>
    </asp:GridView>

    </div>
</div>
<div>
    <br />
    <br />
</div>
<div>
    <center>
        <asp:Button class="BotaoEstiloGlobal" ID="ButtonSimular" OnClientClick="$('.printable').print(); return false;"
            runat="server" Text="Imprimir" /></center>
</div>
