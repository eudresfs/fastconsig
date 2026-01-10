<%@ Control ClassName="WebUserControlTermoAverbacao" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlTermoAverbacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlTermoAverbacao" %>
<div id="DivTermo" class="printable" title="Termo de Averbação em Folha de Pagamento">
    <div style="text-align: center;">
        <table border="0" cellpadding="0" cellspacing="0" width="99%" style="font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
            <tr>
                <td style="width: 160px; text-align: center;">
                    <%--<img runat="server" id="ImgLogoBanco" src="~/Imagens/Logos/Bancos/BIG_BV.png" alt="" />--%>
                </td>
                <td>
                    <h1 style="font-size: 10pt; font-weight: bold;">
                        Termo de Averbação em Folha de Pagamento</h1>
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
                        <td style="width: 15%; padding: 1px;">
                            Número:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impNumero" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Data:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impData" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Consignatária:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impBanco" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            CNPJ:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCNPJ" ClientIDMode="Static" />
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
                        <td style="width: 15%; padding: 1px;">
                            Nome:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impNome" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            CPF:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCPF" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            RG:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impRG" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Data Nasc.:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impDataNasc" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Endereço:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impEndereco" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Bairro:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="impBairro" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Cidade:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCidade" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            UF:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impUF" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Cep:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCep" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Email:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impEmail" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Telefone:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impTelefone" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Celular:
                        </td>
                        <td style="padding: 1px;">
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
                        <td style="padding: 1px; width: 15%;">
                            Matrícula:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impMatricula" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Local:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impLocal" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Setor:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impSetor" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Cargo:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCargo" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Categoria:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label ID="impCategoria" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Regime:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label ID="impRegime" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Data Adm:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label ID="impDataAdm" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Situação:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label ID="impSituacao" runat="server" ClientIDMode="Static" />
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
                                Dados da Averbação:</h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px; width: 15%;">
                            Produto:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impProduto" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Valor Contrato:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impValorContrato" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Prazo:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impPrazo" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Valor Parcela:
                        </td>
                        <td style="text-align: left;">
                            <asp:Label runat="server" ID="impValorParcela" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Valor Consignado:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impValorConsignado" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Obs.:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impObs" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            1ª Parcela:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impMesInicio" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Última Parcela:
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impMesFim" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            Fator (coeficiente):
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impFator" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px;">
                            CET (%):
                        </td>
                        <td style="padding: 1px;">
                            <asp:Label runat="server" ID="impCET" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="DivAverbacoes" style="padding: 5px 0px;">
    </div>
    <div class="Div" style="width: 99%;">
        <p style="text-align: justify; font-size: 7pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
            Eu
            <asp:Label runat="server" ID="impNome2" ClientIDMode="Static" />, acima especificado,
            autorizo a averbação e o desconto em folha de pagamento conforme instruído neste
            documento, atestando serem verídicos todos os dados aqui contidos, incluindo minhas
            informações sobre endereço e telefones, ciente de que posso responder, inclusive
            judicialmente, se questionado em futuro.
        </p>
        <br />
        <p style="text-align: justify; font-size: 7pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
            * Primeira Impressão em:
            <asp:Label runat="server" ID="impDataAverbacao" ClientIDMode="Static" /><br />
            * Reimpressão em:
            <asp:Label runat="server" ID="impDataAverbacao2" ClientIDMode="Static" />
        </p>
        <br />
        <br />
        <p style="text-align: center; font-size: 9pt; font-family: 'Helvetica Neue',Helvetica,Arial,Sans-serif;">
            _____________________________________________________________<br />
            Assinatura do Funcionário
        </p>
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
