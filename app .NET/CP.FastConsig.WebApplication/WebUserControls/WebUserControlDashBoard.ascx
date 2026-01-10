<%@ Control ClassName="WebUserControlDashBoard" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlDashBoard.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlDashBoard" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartBarra.ascx" TagName="WebUserControlChartBarra"
    TagPrefix="uc3" %>
<%@ Register Src="WebUserControlChartPizza.ascx" TagName="WebUserControlChartPizza"
    TagPrefix="uc2" %>

        <dx:ASPxRoundPanel ID="ASPxRoundPanelReservas" runat="server" HeaderStyle-Font-Bold="true"
            Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            HeaderText="Suas Averbações Reservadas" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <ContentPaddings Padding="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                    <asp:GridView EmptyDataText="Sem itens para exibição!" runat="server" ID="GridViewReservas" CssClass="EstilosGridViewDashBoardConsiliacao"
                        Width="100%" Height="100%" AutoGenerateColumns="false" OnRowDataBound="GridViewReservado_RowDataBound">
                        <HeaderStyle Height="16px" CssClass="CabecalhoGridView" HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField HeaderText="Número" DataField="Numero" />
                            <asp:BoundField HeaderText="Data/Hora Simulação" DataField="CreatedOn" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Banco">
                                <ItemTemplate>
                                    <%# Eval("Empresa1.Nome")%>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Valor">
                                <ItemTemplate>
                                    <%# String.Format("{0:N}",Eval("ValorContratado")) %>
                                </ItemTemplate>
                            </asp:TemplateField>                                                      
                            <asp:BoundField HeaderText="Prazo" DataField="Prazo" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Valor Parcela">
                                <ItemTemplate>
                                    <%# String.Format("{0:N}", Eval("ValorParcela"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Reservado até">
                                <ItemTemplate>
                                    <%# String.Format("{0:d}", Eval("PrazoAprovacao"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Margem Disponível" ItemStyle-HorizontalAlign="Right">
				                <ItemTemplate>                    
                                    <asp:Label ID="LabelMargemDisponivel" runat="server"></asp:Label>
				                </ItemTemplate>
			                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cancelar" ItemStyle-HorizontalAlign="Center">                            
                                <ItemTemplate>
                                    <center>
						                <asp:LinkButton runat="server" ID="CancelarReserva" OnClick="CancelarReserva_Click" CommandArgument='<%# Eval("IDAverbacao") %>' >
							            <img alt="Cancelar Reserva" title="Cancelar Reserva" id="ImgCancelarReserva" runat="server" src="~/Imagens/cross.png" /></asp:LinkButton>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>   
                    <br />
                    <asp:Button ID="ImprimirAverbacoesReservadas" runat="server" CssClass="BotaoEstiloGlobal" Text="Imprimir" ToolTip="Imprimir" OnClick="Imprimir_Reservas_Click" />                 
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

    <div class="DivFlutuaEsquerda" >
      <dx:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderStyle-Font-Bold="true" runat="server"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingBottom="18px"  CssPostfix="Aqua"
        HeaderText="Serviços" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="0px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
              
                    <asp:GridView EmptyDataText="Sem itens para exibição!" runat="server" ID="GridViewUtilizacaoMargem" CssClass="EstilosGridViewDashBoardConsiliacao"
                        Width="100%" Height="100%" AutoGenerateColumns="false">
                        <HeaderStyle Height="16px" CssClass="CabecalhoGridView" HorizontalAlign="Left" />
                        <RowStyle Height="16px" CssClass="LinhaListaGridView" HorizontalAlign="Left" />
                        <AlternatingRowStyle Height="17px" CssClass="LinhaAlternadaListaGridView" HorizontalAlign="Left" />
                        <PagerStyle HorizontalAlign="Center" CssClass="PaginadorGridView" />
                        <Columns>
                            <asp:BoundField HeaderText="Serviço" DataField="Nome" />
                            <asp:BoundField HeaderText="Margem Total" DataField="MargemFolha" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField HeaderText="Margem Disponível" DataField="MargemDisponivel" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                        </Columns>
                    </asp:GridView>
             
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    
    </div>
    <asp:HiddenField runat="server" ID="hfMargemFolha" />
    <div  class="DivFlutuaDireita">
      <dx:ASPxRoundPanel ID="ASPxRoundPanel2" HeaderStyle-Font-Bold="true" runat="server"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ContentPaddings-PaddingTop="20px" ContentPaddings-PaddingBottom="23px"
        HeaderText="Orientação Financeira" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="0px" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
              
                    <dx:ASPxPageControl ID="ASPxPageControl2" ContentStyle-BackgroundImage-ImageUrl="~/Imagens/Familia1.png" ContentStyle-BackgroundImage-HorizontalPosition="right"   ContentStyle-BackgroundImage-Repeat="NoRepeat"  Width="100%" ForeColor="#083772" runat="server" EnableCallBacks="True"
                        ActiveTabIndex="0" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                        CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TabSpacing="3px">
                        <TabPages>
                            <dx:TabPage TabStyle-Font-Size="9px" TabStyle-Font-Bold="true" ActiveTabStyle-Font-Size="9px"
                                ActiveTabStyle-Font-Bold="true" Text="Simulação de Empréstimos" Name="AbaEmprestimos">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl1" runat="Server">
                                        <asp:Panel ID="PanelSimularEmprestimo" runat="server" DefaultButton="ButtonSimular">
                                            <table class="WebUserControlTabelaDashboard " width="100%" cellpadding="0" cellspacing="0"
                                                border="0">
                                                <tr>
                                                    <td class="TituloNegrito">
                                                        Margem Disponível:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox Width="150" CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxMargemDisponivel"
                                                            Enabled="false" runat="server">
                                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TituloNegrito">
                                                        Quantidade de Parcelas:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox Width="150" CssClass="TextBoxDropDownEstilos" Height="18px" ID="ASPxTextBoxQuantidadeParcelas"
                                                            runat="server">
                                                            <MaskSettings Mask="99" />
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TituloNegrito">
                                                        Valor da Parcela:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" ClientInstanceName="aSPxTextBoxValorParcela"
                                                            ID="ASPxTextBoxValorParcela" runat="server" Width="150">
                                                            <ClientSideEvents GotFocus="function(s, e) { aSPxTextBoxValorLiberado.SetText(''); }" />
                                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TituloNegrito">
                                                        Valor Liberado:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" ClientInstanceName="aSPxTextBoxValorLiberado"
                                                            ID="ASPxTextBoxValorLiberado" runat="server" Width="150">
                                                            <ClientSideEvents GotFocus="function(s, e) { aSPxTextBoxValorParcela.SetText(''); }" />
                                                            <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Button class="BotaoEstiloGlobal" ID="ButtonSimular" OnClick="ButtonSimular_Click"
                                                            runat="server" Text="Simular" />
                                                        &nbsp;
                                                        <asp:Button class="BotaoEstiloGlobal" ID="ButtonLimpar" OnClick="ButtonLimpar_Click"
                                                            runat="server" Text="Limpar" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage TabStyle-Font-Size="9px" TabStyle-Font-Bold="true" ActiveTabStyle-Font-Size="9px"
                                ActiveTabStyle-Font-Bold="true" Text="Simulação de Compra de Dívidas" Name="AbaCompraDividas">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl2" runat="Server">
                                        <asp:Panel   ID="PanelSimulacaoCompraDivida" runat="server" DefaultButton="ButtonSimularNegociacaoDivida"
                                            CssClass="compraDivida">
                                            <div class="DashBoardTabSimulacaoCompra" style="width:71%;float:left;">
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label runat="server" ID="LabelMargemNegociacao" Text="Margem Disponível Atual: "></asp:Label>
                                                                                    <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" ID="ASPxTextBoxMargemNegociacao"
                                                                                        Enabled="false" runat="server" Width="130">
                                                                                        <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="LabelObjetivo" Text="Qual seu objetivo? "></asp:Label>
                                                                        <asp:RadioButtonList RepeatLayout="Table" RepeatDirection="Vertical" runat="server"
                                                                            ID="RadioButtonListOpcao" CssClass="clearLeft" AutoPostBack="true" OnSelectedIndexChanged="ButtonSimularNegociacaoDivida_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0" style="margin-left: 3px">Obter Mais Dinheiro</asp:ListItem>
                                                                            <asp:ListItem Value="1" Enabled="false">Regularizar Minha Margem</asp:ListItem>
                                                                            <asp:ListItem Value="2">Reduzir o Valor que Pago Mensalmente</asp:ListItem>
                                                                            <asp:ListItem Value="3">Diminuir a Quantidade de Parcelas</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="ButtonSimularNegociacaoDivida"
                                                                OnClick="ButtonSimularNegociacaoDivida_Click" Text="Simular Negociação de Dívida" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left; width: 25%; padding-top: 30px;">
                                                <asp:Label runat="server" CssClass="TituloPrazoDesejado" ID="LabelInformacao" Visible="false"></asp:Label>
                                                <dx:ASPxTextBox ID="ASPxTextBoxInformacao" Font-Size="12px" CssClass="TextBoxDropDownEstilos"
                                                    Height="60px" runat="server" Width="120" Visible="false">
                                                    <MaskSettings Mask="$ <0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </asp:Panel>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                        <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                        </LoadingPanelImage>
                        <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px" />
                        <ContentStyle>
                            <Border BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                    </dx:ASPxPageControl>
             
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    </div>

    <div class="DivFlutuaEsquerda" >
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderStyle-Font-Bold="true"
            Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            HeaderText="Utilização de Margens" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <ContentPaddings Padding="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <uc3:WebUserControlChartBarra ID="WebUserControlChartBarraMargens" runat="server" />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    
    </div>

    <div class="DivFlutuaDireita">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderStyle-Font-Bold="true"
            Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
            HeaderText="Utilização de Margens por Serviços" GroupBoxCaptionOffsetY="-28px"
            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <ContentPaddings Padding="0px" />
            <ContentPaddings Padding="0px"></ContentPaddings>
            <HeaderStyle Font-Bold="True" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <uc2:WebUserControlChartPizza ID="WebUserControlChartPizzaUtilizacaoMargemPorProduto"
                        runat="server" />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>

    



   

