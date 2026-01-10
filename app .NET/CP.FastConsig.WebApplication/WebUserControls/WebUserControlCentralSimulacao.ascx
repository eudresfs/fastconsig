<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlCentralSimulacao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlCentralSimulacao2" %>
<%@ Import Namespace="CP.FastConsig.Common" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div style="clear: both; overflow: hidden;">
    <asp:MultiView runat="server" ID="MultiViewCentralSimulacao">
        <asp:View runat="server" ID="ViewInformacoes">
            <div style="float: left; width: 100%; margin-bottom: 1%; clear: both;">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelDadosPessoais" ShowHeader="true"
                    Width="100%" HeaderText="Central de Simulação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    HeaderStyle-Font-Bold="true">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentDadosPessoais" runat="server" SupportsDisabledAttribute="True">
                            <table  border="0" cellpadding="0" cellspacing="1" width="100%">
                                <tr>
                                    <td valign="middle" style="width:10%;border:1px solid #a3c0e8;background-color:#e0edff;">
                                        <asp:Image runat="server" ID="ImageFuncionario" Width="55" />
                                    </td>
                                    <td valign="top" style="width: 30%;border:1px solid #a3c0e8;background-color:#e0edff;padding-right:5px;">
                                        <table border="0" cellpadding="0" cellspacing="4" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelNome"></asp:Label><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelCpf"></asp:Label><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelRg"></asp:Label><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelMatricula"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelDataAdmissao"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" style="width: 30%;border:1px solid #a3c0e8;background-color:#e0edff;padding-right:5px;">
                                        <table border="0" cellpadding="0" cellspacing="4" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelDataNascimento"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelRegime"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelCategoria"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="LabelSituacao"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" style="width: 30%;border:1px solid #a3c0e8;background-color:#e0edff;">
                                        <table border="0" cellpadding="0" cellspacing="4" >
                                            <tr>
                                                <td valign="top" style="width: 50%;">
                                                    <asp:Label runat="server" ID="LabelMargemDisponivel" Text="Margem Disponível: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox  Width="80" runat="server" ID="TextBoxMargemDisponivel" CssClass="TextBoxDropDownEstilos"
                                                        Enabled="false" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label runat="server" ID="ValorContrato" Text="Valor da Averbação: "></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" 
                                                        ClientInstanceName="aSPxTextBoxValorContrato" ID="ASPxTextBoxValorContrato" runat="server"
                                                        Width="84">
                                                        <ClientSideEvents GotFocus="function(s, e) { aSPxTextBoxValorParcela.SetText(''); }" />
                                                        <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label runat="server" ID="Label1" Text="Valor da Parcela: "></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos"
                                                        ClientInstanceName="aSPxTextBoxValorParcela" ID="ASPxTextBoxValorParcela" runat="server"
                                                        Width="84">
                                                        <ClientSideEvents GotFocus="function(s, e) { aSPxTextBoxValorContrato.SetText(''); }" />
                                                        <MaskSettings Mask="$ <-99999..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 20%;text-align:left;">
                                                    <asp:Button runat="server" ID="ButtonSimular" CssClass="BotaoEstiloGlobal" OnClick="ButtonSimular_Click"
                                                        Text="Simular" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%-- <div style="width: 49%; margin-right: 1%">
                                <div style="width: 20%; float: left; margin-right: 10px">
                                    <div class="formulario2" style="width: 38%">
                                    </div>
                                    <div class="formulario2" style="width: 38%">
                                    </div>
                                </div>
                                <div style="width: 50%; float: left">
                                    <div class="formulario3" style="width: 100%; padding: 0px 0 53px 0;">
                                        <div style="clear: none; float: left">
                                        </div>
                                    </div>
                                </div>--%>
                            <div style="width: 100%;margin-top:8px;">
                                <dx:ASPxGridView Style="margin-right: 1%; float: left" Width="49%" KeyFieldName="Matricula"
                                    runat="server" Font-Size="10px" ID="ASPxGridViewMatriculas" ClientInstanceName="aSPxGridViewMatriculas"
                                    AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                                    OnHtmlRowCreated="ASPxGridViewMatriculas_HtmlRowCreated">
                                    <Columns>
                                        <dx:GridViewDataColumn Settings-AllowSort="false" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <%--<input runat="server" type="radio" id="RadioButtonSelecionarMatricula" name="RadioButtonMatriculas" value='<% %>'/>--%>
                                                <dx:ASPxRadioButton Variaveis="" GroupName="RadioButtonMatriculas" onclientclick="alert('blabla');"
                                                    ID="ASPxRadioButtonSelecionar" OnCheckedChanged="ASPxRadioButtonSelecionar_CheckedChanged"
                                                    runat="server" Text="" AutoPostBack="true" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Matrícula" FieldName="Matricula"
                                            VisibleIndex="1" />
                                        <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Margem para Empréstimo"
                                            VisibleIndex="1">
                                            <DataItemTemplate>
                                                <asp:Label runat="server" ID="LabelMargem"></asp:Label>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                                <dx:ASPxGridView runat="server" Font-Size="10px" ID="ASPxGridViewCentralSimulacaoContratosRefinanciar"
                                    ClientInstanceName="aSPxGridViewMatriculas" AutoGenerateColumns="False" KeyFieldName="IDAverbacao"
                                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" OnHtmlRowCreated="ASPxGridViewCentralSimulacaoContratosRefinanciar_HtmlRowCreated">
                                    <Columns>
                                        <dx:GridViewBandColumn Caption="Averbações para Refinanciar">
                                            <Columns>
                                                <dx:GridViewDataColumn Settings-AllowSort="false" VisibleIndex="0">
                                                    <DataItemTemplate>
                                                        <dx:ASPxCheckBox ValueChecked='<%# Eval("ValorParcela") %>' ValueType="System.String"
                                                            ID="ASPxCheckBoxSelecionar" CssClass="checksCetralSimulacao" runat="server" Text=""
                                                            AutoPostBack="true" OnCheckedChanged="ASPxCheckBoxSelecionar_CheckedChanged" />
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Parcela" VisibleIndex="1">
                                                    <DataItemTemplate>
                                                        <asp:Label runat="server" ID="LabelParcela"></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Settings-AllowSort="false" Caption="Parcelas Restantes" VisibleIndex="2">
                                                    <DataItemTemplate>
                                                        <asp:Label runat="server" ID="LabelParcelasRestantes"></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Settings-AllowSort="false" VisibleIndex="3" Caption="Saldo em Aberto">
                                                    <DataItemTemplate>
                                                        <asp:Label runat="server" ID="LabelSaldoAberto"></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>
                                    </Columns>
                                    <SettingsPager Mode="ShowAllRecords">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <div style="float: left; width: 100%; clear: both;">
                <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelResultado" ShowHeader="true"
                    Width="100%" HeaderText="Resultado da Simulação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Visible="false" HeaderStyle-Font-Bold="true">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentResultado" runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView runat="server" Font-Size="10px" ID="ASPxGridViewResultado" ClientInstanceName="aSPxGridViewResultado"
                                Width="100%" AutoGenerateColumns="False" KeyFieldName="Ranking" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua" OnHtmlRowCreated="ASPxGridViewResultado_HtmlRowCreated">
                                <Columns>
                                    <dx:GridViewBandColumn Caption="Averbações para Refinanciar">
                                        <HeaderTemplate>
                                            <asp:Label Style="float: left" runat="server" ID="LabelData" Text='<%# string.Format("Data: {0}",DateTime.Now.ToString("dd/MM/yyyy HH:mm")) %>'></asp:Label>
                                            <asp:Label Style="float: right; margin: 2px" runat="server" ID="LabelLegenda" Text="Averbar Simulação"></asp:Label>
                                            <asp:Image Style="float: right; margin: 2px" runat="server" ID="ImageLegenda" ImageUrl="~/Imagens/iconeAverbar.png"
                                                Width="25" />
                                        </HeaderTemplate>
                                        <Columns>
                                            <dx:GridViewDataColumn VisibleIndex="0">
                                                <DataItemTemplate>
                                                    <asp:LinkButton runat="server" ID="LinkButtonAverbar" CommandArgument='<%# Eval("ValorAverbacao") + ";" + Eval("Prazo") + ";" + Eval("ValorParcela") %>'
                                                        OnClick="LinkButtonAverbar_Click">
                                                        <asp:Image runat="server" ID="ImageAverbar" ImageUrl="~/Imagens/iconeAverbar.png"
                                                            Width="25" />
                                                    </asp:LinkButton>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn VisibleIndex="1">
                                                <DataItemTemplate>
                                                    <dx:ASPxCheckBox ValueChecked='<%# Eval("Prazo") %>' ValueType="System.String" ID="ASPxCheckBoxSelecionar"
                                                        runat="server" Text="" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn VisibleIndex="2" Caption="Ranking" FieldName="Ranking" />
                                            <dx:GridViewDataColumn VisibleIndex="3" Caption="Valor da Averbacao">
                                                <DataItemTemplate>
                                                    <asp:Label runat="server" ID="LabelValorAverbacao"></asp:Label>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn VisibleIndex="4" Caption="Valor da Parcela">
                                                <DataItemTemplate>
                                                    <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn VisibleIndex="5" Caption="Prazo" FieldName="Prazo" />
                                            <dx:GridViewDataColumn VisibleIndex="6" Caption="Coeficiente" FieldName="Coeficiente">
                                            </dx:GridViewDataColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                </Columns>
                                <SettingsBehavior AllowSort="false"></SettingsBehavior>
                            </dx:ASPxGridView>
                            <asp:Button Style="margin-top: 5px" runat="server" ID="ButtonImprimirOrcamento" Text="Ver Orçamento"
                                CssClass="BotaoEstiloGlobal" OnClick="ButtonImprimirOrcamento_Click" />
                            <asp:Button Style="margin-top: 5px" runat="server" ID="ButtonCancelar" Text="Cancelar"
                                CssClass="BotaoEstiloGlobal" OnClientClick="return confirm('Deseja cancelar?')"
                                OnClick="ButtonCancelar_Click" />
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
        </asp:View>
        <asp:View runat="server" ID="ViewResultado">
            <div id="divImpressao">
                <fieldset>
                    <legend>Dados do Funcionário </legend>
                    <div style="width: 35%; padding-bottom: 10px; float: left; width: 350px">
                        <asp:Image runat="server" ID="ImageFuncionarioResutado" Width="110" Style="float: left;
                            clear: left; margin-bottom: 30px" />
                        <asp:Label runat="server" ID="LabelNomeResultado" Style="width: 230px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelCpfResultado" Style="width: 230px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelMatriculaResultado" Style="width: 230px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelTelefoneResultado" Style="width: 230px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelLocalTrabalhoResultado" Style="width: 230px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                    </div>
                    <div style="float: left; width: 190px">
                        <asp:Label runat="server" ID="LabelDataAdmissaoResultado" Style="width: 190px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelDataNascimentoResultado" Style="width: 190px;
                            float: left; text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelRegimeResultado" Style="width: 190px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelCategoriaResultado" Style="width: 190px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                        <asp:Label runat="server" ID="LabelSituacaoResultado" Style="width: 190px; float: left;
                            text-align: left; margin: 4px"></asp:Label>
                    </div>
                </fieldset>
                <fieldset>
                    <div>
                        <dx:ASPxGridView runat="server" Font-Size="10px" ID="ASPxGridViewResultadoOrcamento"
                            ClientInstanceName="aSPxGridViewResultado" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="Ranking" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                            OnHtmlRowCreated="ASPxGridViewResultadoOrcamento_HtmlRowCreated">
                            <Columns>
                                <dx:GridViewBandColumn>
                                    <Columns>
                                        <dx:GridViewDataColumn VisibleIndex="0" Caption="Simulação" FieldName="Ranking" />
                                        <dx:GridViewDataColumn VisibleIndex="1" Caption="Valor da Averbacao">
                                            <DataItemTemplate>
                                                <asp:Label runat="server" ID="LabelValorAverbacao"></asp:Label>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn VisibleIndex="2" Caption="Valor da Parcela">
                                            <DataItemTemplate>
                                                <asp:Label runat="server" ID="LabelValorParcela"></asp:Label>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn VisibleIndex="3" Caption="Prazo" FieldName="Prazo" />
                                        <dx:GridViewDataColumn VisibleIndex="4" Caption="Coeficiente" FieldName="Coeficiente">
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn VisibleIndex="5" Caption="Data e Hora">
                                            <DataItemTemplate>
                                                <asp:Label runat="server" ID="LabelDataHora" Text='<%# DateTime.Now %>'></asp:Label>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                    </Columns>
                                </dx:GridViewBandColumn>
                            </Columns>
                            <SettingsBehavior AllowSort="false"></SettingsBehavior>
                        </dx:ASPxGridView>
                    </div>
                </fieldset>
                <fieldset>
                    <asp:Label Style="margin: 20px 10% 60px 10%; float: left; text-align: justify; line-height: 20px"
                        runat="server" ID="LabelTextoOrcamento" Text='Este documento tem por finalidade exibir as simulações realizadas pelo usuário acima mensionado, onde a mesma tem validade de _____ dias úteis. Deste modo declaramos abaixo que estamos cientes que esta simulação poderá ser contratada até _____ dias úteis por esta empresa.'></asp:Label>
                    <div style="width: 250px; text-align: center; padding-top: 5px; border-top: solid 1px;
                        margin-left: 10%; float: left; margin-bottom: 30px">
                        <asp:Label runat="server" ID="LabelFuncionarioInstituicao" Text="Funcionário da Instituição Financeira"></asp:Label>
                    </div>
                    <div style="width: 250px; text-align: center; padding-top: 5px; border-top: solid 1px;
                        margin-right: 10%; float: right">
                        <asp:Label runat="server" ID="LabelNomeFuncionario"></asp:Label>
                    </div>
                    <asp:Label runat="server" ID="LabelLugarData" Style="float: right; clear: right;
                        margin-top: 30px"></asp:Label>
                </fieldset>
            </div>
            <asp:Button runat="server" ID="ButtonImprmir" Text="Imprimir Orçamento" OnClientClick=""
                OnClick="ButtonImprimir_Click" CssClass="BotaoEstiloGlobal" Style="margin-top: 5px" />
            <asp:Button runat="server" ID="ButtonCancelar2" Text="Cancelar" OnClick="ButtonCancelar2_Click"
                CssClass="BotaoEstiloGlobal" Style="margin-top: 5px" />
        </asp:View>
    </asp:MultiView>
</div>
