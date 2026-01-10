<%@ Control ClassName="WebUserControlFuncionarios_Consulta" EnableViewState="true"
    Language="C#" AutoEventWireup="True" CodeBehind="WebUserControlFuncionariosConsulta.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFuncionariosConsulta" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="~/WebUserControls/WebUserControlImprimirAposentadoria.ascx" TagName="WebUserControlImprimirAposentadoria"
	TagPrefix="uc1" %>

<div>
    <!-- Menu Principal -->
    <div style="margin: 0 auto; text-align: center; clear: both; display: block; height: 40px;
        margin-top: 10px;">
<%--        <div class="float-divider">
            <dx:ASPxButton Cursor="Pointer" ID="ASPxButton1" EnableDefaultAppearance="false"
                CssClass="BotaoEstiloGlobal" EnableTheming="false" runat="server" Text="Imprimir Tela">
            </dx:ASPxButton>
        </div>--%>
        <div class="float-divider">
            <dx:ASPxButton Cursor="Pointer" OnClick="ASPxButtonBloquearUsuario_Click" ID="ASPxButtonBloquearUsuario"
                CssClass="BotaoEstiloGlobal" EnableDefaultAppearance="false" EnableTheming="false"
                runat="server" Text="Bloquear">
            </dx:ASPxButton>
        </div>
        <div class="float-divider">
            <dx:ASPxButton Cursor="Pointer" ID="ASPxButtonHistoricos" CssClass="BotaoEstiloGlobal"
                runat="server" EnableDefaultAppearance="false" EnableTheming="false" OnClick="ASPxButtonHistoricos_Click"
                Text="Histórico">
            </dx:ASPxButton>
        </div>
        <div class="float-divider">
            <dx:ASPxButton ID="ASPxButtonAutorizacoes" runat="server" EnableDefaultAppearance="false"
                EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Autorizações Especiais"
                OnClick="ASPxButtonAutorizacoes_Click">
            </dx:ASPxButton>
        </div>
        <div class="float-divider">
            <dx:ASPxButton ID="ASPxButtonRescindir" runat="server" EnableDefaultAppearance="false"
                EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Rescindir"
                OnClick="ASPxButtonRescindir_Click">
            </dx:ASPxButton>
        </div>
        <div class="float-divider">
            <dx:ASPxButton ID="ASPxButtonAposentar" runat="server" EnableDefaultAppearance="false" ClientSideEvents-Click="function(s, e) { e.processOnServer = confirma(); }"
                EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Aposentar" 
                OnClick="ASPxButtonAposentar_Click">
            </dx:ASPxButton>
            <dx:ASPxPopupControl LoadContentViaCallback="OnPageLoad" OnLoad="ASPxPopupControlAposentadoria_Load" BackColor="#ffffff" DragElement="Header" LoadingPanelText="Carregando..." ID="ASPxPopupControlAposentadoria"
								runat="server" CloseAction="OuterMouseClick"  
								PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" AllowDragging="False"
								ForeColor="#000000" Font-Bold="true" Width="700px" HeaderText="Aposentadoria" ClientInstanceName="ASPxPopupControlAposentadoria">
								<HeaderStyle ForeColor="#000000" Font-Bold="true" CssClass="AlturaCabecalhoPopControl" />
								<ContentCollection>
									<dx:PopupControlContentControl ID="PopupControlAposentadoria" runat="server">
										<uc1:WebUserControlImprimirAposentadoria ID="WebUserControlImprimirAposentadoria" runat="server" />
									</dx:PopupControlContentControl>                                    
								</ContentCollection>
							</dx:ASPxPopupControl>
        </div>
    </div>
    <!-- Fim -->
    <div>
        <div id="DivRescindir" runat="server" visible="false">

            <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelBoasNoticias" ShowHeader="true" Width="100%"
                HeaderText="Escolha o tipo de Rescisão" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContentBoasNoticias" runat="server"  SupportsDisabledAttribute="True">
                        <asp:DropDownList ID="DropDownListRescindir" runat="server" CssClass="TextBoxDropDownEstilos">
                            <asp:ListItem Text="Não enviar mais descontos para este funcionário" Selected="true" Value="1" />
                            <asp:ListItem Text="Enviar apenas os descontos deste mês" Value="2" />
                        </asp:DropDownList>
                        <br />
                        <br />
                        <div class="float-divider">
                            <dx:ASPxButton ID="ASPxButtonAplicarRescisao" runat="server" EnableDefaultAppearance="false"
                                EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Aplicar"
                                OnClick="ASPxButtonAplicarRescisao_Click">
                            </dx:ASPxButton>
                        </div>
                        <div class="float-divider">
                            <dx:ASPxButton ID="ASPxButtonCancelarRescisao" runat="server" EnableDefaultAppearance="false"
                                EnableTheming="false" CssClass="BotaoEstiloGlobal" Text="Cancelar"
                                OnClick="ASPxButtonCancelarRescisao_Click">
                            </dx:ASPxButton>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
        </div>
        <!-- Bloco para Dados Pessoais -->
        <!-- Tabela populada com os dados pessoais -->
        <div style="width: 100%;">
            <table id="Table1" runat="server" width="100%" border="0" cellspacing="1" cellpadding="4"
                class="MargemTopoTabela">
                <tr>
                    <td class="BordaBase">
                        <h1 class="TituloTabela">
                            Dados Pessoais</h1>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td rowspan="4" class="LarguraBordaCelula">
                                    <img src="/Imagens/Perfil.png" alt="" width="65px" height="74px" />
                                </td>
                                <td class="TituloNegrito" style="width: 20%;">
                                    <asp:Label ID="Label1" runat="server" Text="Nome:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtNome" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TituloNegrito">
                                    <asp:Label ID="Label2" runat="server" Text="CPF:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtCPF" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TituloNegrito">
                                    <asp:Label ID="Label5" runat="server" Text="RG:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtRG" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TituloNegrito" style="border: none;">
                                    <asp:Label ID="Label6" runat="server" Text="Data Nasc:"></asp:Label>
                                </td>
                                <td class="UltimaCelulaSemBordaBase">
                                    <asp:Label ID="txtDataNasc" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h2 class="trigger">
                            <a id="aTitulo" onclick="return false;" href="#">Mais Detalhes</a>
                        </h2>
                        <div id="DivMaisDetalhes" class="toggle_container">
                            <div class="block">
                                <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td rowspan="9" class="LarguraBordaCelula">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito" style="width: 20%;">
                                            <asp:Label ID="Label8" runat="server" Text="Endereço:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtEndereco" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label9" runat="server" Text="Complemento:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtComplemento" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label10" runat="server" Text="Bairro:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtBairro" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label11" runat="server" Text="Cidade/UF:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtCidade" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label12" runat="server" Text="Cep:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtCep" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label13" runat="server" Text="Telefone:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtTelefone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito">
                                            <asp:Label ID="Label14" runat="server" Text="Email:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="txtEmail" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloNegrito" style="border: none;">
                                            <asp:Label ID="Label15" runat="server" Text="Fone Celular:"></asp:Label>
                                        </td>
                                        <td class="UltimaCelulaSemBordaBase">
                                            <asp:Label ID="txtCelular" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <!-- fim -->
        <table id="Table2" runat="server" width="100%" border="0" cellspacing="1" cellpadding="4"
            class="MargemTopoTabela">
            <tr>
                <td class="BordaBase">
                    <h1 class="TituloTabela">
                        Dados Funcionais</h1>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td rowspan="7" class="LarguraBordaCelula">
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito" style="width: 20%;">
                                <asp:Label ID="Label3" runat="server" Text="Matricula:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtMatricula" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                <asp:Label ID="Label4" runat="server" Text="Categoria:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtCategoria" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                <asp:Label ID="Label19" runat="server" Text="Data de Admissão:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LabelDataAdmissao" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                <asp:Label ID="Label16" runat="server" Text="Regime:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtRegime" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                <asp:Label ID="Label17" runat="server" Text="Situação:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtSituacao" runat="server"></asp:Label>&nbsp;
                                <asp:LinkButton ID="txtBloqueio" runat="server" Text="Existe(m) bloqueio(s)!" style="color: Red;" Visible="false" OnClick="ASPxButtonBloquearUsuario_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito" style="border: none;">
                                <asp:Label ID="Label7" runat="server" Text="Senha Provisória:"></asp:Label>
                            </td>
                            <td style="border: none;">
                                <asp:Label ID="txtSenhaProvisoria" runat="server"></asp:Label>&nbsp;&nbsp;
                                <asp:Button Cursor="Pointer" ID="ASPxButtonGerarSenha" runat="server" Text="Gerar Senha"
                                    CssClass="BotaoEstiloGlobal" OnClick="GerarSenha_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <%--                <td colspan="2" class="CelulaBotaoMaisDetalhes">
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Mais Detalhes..." OnClick="FuncionaisDetalhes_Click"></asp:LinkButton>
                </td>--%>
                <td>
                    <h2 class="trigger2">
                        <a id="aTitulo2" onclick="return false;" href="#">Mais Detalhes</a>
                    </h2>
                    <div id="DivMaisDetalhes2" class="toggle_container2">
                        <div class="block">
                            <table class="WebUserControlTabela" width="100%" border="0" cellspacing="1">
                                <tr>
                                    <td rowspan="6" class="LarguraBordaCelula">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito" style="width: 20%;">
                                        <asp:Label ID="Label18" runat="server" Text="Estabelecimento:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtEstabelecimento" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        <asp:Label ID="Label20" runat="server" Text="Local:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtLocal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        <asp:Label ID="Label22" runat="server" Text="Setor:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtSetor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        <asp:Label ID="Label24" runat="server" Text="Cargo:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtCargo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridViewMargens" DataKeyNames="Nome" EnableViewState="true" runat="server"
            CssClass="EstilosGridView" PageSize="20" AllowPaging="true" AllowSorting="true"
            EnablePersistedSelection="true" EmptyDataText="Sem itens para exibição!" Width="100%"
            AutoGenerateColumns="false">
            <HeaderStyle CssClass="CabecalhoGridView" />
            <RowStyle CssClass="LinhaListaGridView" />
            <AlternatingRowStyle CssClass="LinhaAlternadaListaGridView" />
            <PagerStyle CssClass="PaginadorEstilos" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField HeaderText="Tipos de Serviços" DataField="Nome" />
                <asp:BoundField HeaderText="Margem Folha" DataField="MargemFolha" />
                <asp:BoundField HeaderText="Margem Disponível" DataField="MargemDisponivel" />
            </Columns>
        </asp:GridView>
        <!-- Fim -->
        <div class="BoxBotaoExibirAverbacoesConsignadas">
            <asp:Button ID="Button3" class="BotaoEstiloGlobal" runat="server" Text="Exibir Averbações Consignadas"
                OnClick="MostrarAverbacaos_Click"></asp:Button>
        </div>
        <div id="fsAverbacaos" runat="server" visible="false">
            <%--        <asp:ObjectDataSource ID="ODS_Averbacaos" runat="server"  
            TypeName="CP.FastConsig.BLL.ODS_Averbacao" DataObjectTypeName="CP.FastConsig.DAL.Averbacao"  
            SelectMethod="SelectAverbacaoDoFunc" 
            SortParameterName="sortExpression"   
            SelectCountMethod="SelectAverbacaoDoFuncCount" EnablePaging="True" 
                onselecting="ODS_Averbacaos_Selecting" > 
            <SelectParameters>
                        <asp:Parameter DbType="Int32" Name="IdFuncionario" />                    
                </SelectParameters>
        </asp:ObjectDataSource>  --%>
            <dx:ASPxGridView SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
                SettingsText-EmptyDataRow="Sem dados para exibição!" EnableTheming="True" ID="gridAverbacaos"
                ClientInstanceName="gridAverbacaos" runat="server" KeyFieldName="IDAverbacao" OnRowCommand="gridAverbacoes_RowCommand"
                Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                CssPostfix="Aqua">
                <Columns>
                    <dx:GridViewDataColumn VisibleIndex="0">
                        <DataItemTemplate>
                            <asp:LinkButton runat="server" ID="select" CommandName="Select">
                                                            <img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Consultar Averbação" title="Selecione para Consultar"  />
                            </asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewCommandColumn VisibleIndex="1">
                        <ClearFilterButton Visible="True" Text="Limpar" />
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
                    <dx:GridViewDataColumn FieldName="Data" VisibleIndex="2" />
                    <dx:GridViewDataColumn FieldName="Empresa1.Nome" VisibleIndex="3" Caption="Consignatária" />
                    <dx:GridViewDataColumn FieldName="AverbacaoTipo.Nome" VisibleIndex="4" Caption="Tipo" />
                    <dx:GridViewDataColumn FieldName="Produto.Nome" VisibleIndex="5" Caption="Produto" />
                    <dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="5" Caption="Valor Parcela" />
                    <dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="5" />
                    <dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="5" Caption="Situação" />
                    <dx:GridViewDataColumn FieldName="UltimaTramitacao" VisibleIndex="5" Caption="Ult.Tramitação" />
                    <%--<dx:GridViewDataColumn FieldName="Produto.ProdutoGrupo.Nome" VisibleIndex="5" Caption="Tipo Consignação" />--%>
                </Columns>
                <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                    </LoadingPanelOnStatusBar>
                    <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                    </LoadingPanel>
                </Images>
                <ImagesEditors>
                    <DropDownEditDropDown>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                    </DropDownEditDropDown>
                    <SpinEditIncrement>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                            PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                    </SpinEditIncrement>
                    <SpinEditDecrement>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                            PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                    </SpinEditDecrement>
                    <SpinEditLargeIncrement>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                            PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                    </SpinEditLargeIncrement>
                    <SpinEditLargeDecrement>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                            PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                    </SpinEditLargeDecrement>
                </ImagesEditors>
                <ImagesFilterControl>
                    <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanel>
                </ImagesFilterControl>
                <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                    <LoadingPanel ImageSpacing="8px">
                    </LoadingPanel>
                </Styles>
                <StylesEditors>
                    <CalendarHeader Spacing="1px">
                    </CalendarHeader>
                    <ProgressBar Height="25px">
                    </ProgressBar>
                </StylesEditors>
                <Templates>
                    <DetailRow>
                        <div style="padding: 3px 3px 2px 3px">
                            <dx:ASPxPageControl EnableTheming="True" CssFilePath="~/App_Themes/Aqua/Web/styles.css"
                                runat="server" ID="pageControl" Width="100%" EnableCallBacks="true">
                                <TabPages>
                                    <dx:TabPage Text="Averbações Vinculadas (Compra/Renegociação)" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server" ID="ContentControl1">
                                                <dx:ASPxGridView CssFilePath="~/Estilos/DevExpress/GridView/styles.css" CssPostfix="Aqua"
                                                    ID="gridVinculos" runat="server" KeyFieldName="IDAverbacao" Width="100%" OnDataBinding="gridVinculos_DataSelect" OnRowCommand="gridVinculos_RowCommand">
                                                    <Columns>
                                                        <dx:GridViewDataColumn VisibleIndex="1" Width="25px">
					                                        <DataItemTemplate>
						                                        <asp:LinkButton runat="server" ID="select" CommandName="Select">
							                                        <img style="padding-top:4px;" src="/Imagens/BtnProcurar.png" width="16" height="16" alt="Detalhes da Averbação" title="Detalhes da Averbação"  />
						                                        </asp:LinkButton>
					                                        </DataItemTemplate>
				                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
                                                        <dx:GridViewDataColumn FieldName="Data" VisibleIndex="2" />
                                                        <dx:GridViewDataColumn FieldName="Empresa.Nome" VisibleIndex="3" Caption="Consignatária" />
                                                        <dx:GridViewDataColumn FieldName="Produto.Nome" VisibleIndex="4" Caption="Serviço" />
                                                        <dx:GridViewDataColumn FieldName="ValorParcela" VisibleIndex="5" Caption="Valor Parcela" />
                                                        <dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="5" />
                                                        <dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="5" Caption="Situação" />
                                                        <dx:GridViewDataColumn FieldName="UltimaTramitacao" VisibleIndex="5" Caption="Ult.Tramitação" />
                                                        <dx:GridViewDataColumn FieldName="Produto.ProdutoGrupo.Nome" VisibleIndex="5" Caption="Tipo Consignação" />
                                                    </Columns>
                                                    <%--                                <SettingsDetail ShowDetailRow="true" />
                                                    --%>
                                                    <Settings ShowFooter="True" />
                                                    <SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
                                                </dx:ASPxGridView>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Parcelas" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server" ID="ContentControl2">
                                                <dx:ASPxGridView CssFilePath="~/Estilos/DevExpress/GridView/styles.css" CssPostfix="Aqua"
                                                    ID="ASPxGridViewParcelas" runat="server" KeyFieldName="IDAverbacaoParcela" Width="100%"
                                                    OnDataBinding="gridParcelas_DataSelect">
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="Numero" VisibleIndex="1" Caption="Número" />
                                                        <dx:GridViewDataColumn FieldName="MesAno" VisibleIndex="2" Caption="Mês/Ano" />
                                                        <dx:GridViewDataColumn FieldName="Valor" VisibleIndex="3" Caption="Valor" />
                                                        <dx:GridViewDataColumn FieldName="ValorDescontado" VisibleIndex="4" Caption="Descontado" />
                                                        <dx:GridViewDataColumn FieldName="AverbacaoParcelaSituacao.Nome" VisibleIndex="5"
                                                            Caption="Situação" />
                                                        <dx:GridViewDataColumn FieldName="Observacao" VisibleIndex="6" Caption="Obs." />
                                                    </Columns>
                                                    <%--                                <SettingsDetail ShowDetailRow="true" />
                                                    --%>
                                                    <Settings ShowFooter="True" />
                                                    <SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
                                                </dx:ASPxGridView>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Tramitações" Visible="true">
                                        <ContentCollection>
                                            <dx:ContentControl runat="server" ID="ContentControl3">
                                                <dx:ASPxGridView CssFilePath="~/Estilos/DevExpress/GridView/styles.css" CssPostfix="Aqua"
                                                    ID="ASPxGridViewTramitacoes" runat="server" KeyFieldName="IDAverbacaoTramitacao"
                                                    Width="100%" OnDataBinding="gridTramitacoes_DataSelect">
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="AverbacaoSituacao.Nome" VisibleIndex="4" Caption="Situação" />
                                                        <dx:GridViewDataColumn FieldName="OBS" VisibleIndex="5" Caption="Obs." />
                                                        <dx:GridViewDataDateColumn FieldName="CreatedOn" VisibleIndex="5" Caption="Data/Hora">
                                                                                        <PropertiesDateEdit DisplayFormatString="{0:g}">
                                                                                        </PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn> 
                                                        <dx:GridViewDataColumn FieldName="Usuario.Nome" VisibleIndex="5" Caption="Usuário" />
                                                    </Columns>
                                                    <%--                                <SettingsDetail ShowDetailRow="true" />
                                                    --%>
                                                    <Settings ShowFooter="True" />
                                                    <SettingsText GroupPanel="Arraste uma coluna até aqui para agrupar por ela." EmptyDataRow="Sem dados para exibição!" />
                                                </dx:ASPxGridView>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>
                        </div>
                    </DetailRow>
                </Templates>
                <SettingsLoadingPanel ImagePosition="Top" />
                <SettingsDetail ShowDetailRow="true" />
                <Settings ShowGroupPanel="True" />
                <Settings ShowFilterRow="True" />
                <SettingsCustomizationWindow Enabled="True" />
                <%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
                --%>
            </dx:ASPxGridView>
        </div>
        <h1 class="TextoAncora">
            <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
                Topo da Página</a>
        </h1>
        <div style="height: 5px;">
            &nbsp;</div>
    </div>
    <div id="dlgconfirmaapos" title="Aposentar Funcionário" style="display: none;">
    Favor confirmar a matrícula deste funcionário que passará para a situação Aposentado. Isto significa que nenhum de seus descontos será mais enviado para folha de pagamento após a confirmação desta mensagem.
    </div>
</div>
