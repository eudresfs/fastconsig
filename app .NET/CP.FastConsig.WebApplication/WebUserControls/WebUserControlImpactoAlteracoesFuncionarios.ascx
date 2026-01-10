<%@ Control Language="C#" ClassName="WebUserControlImpactoAlteracoesFuncionarios" AutoEventWireup="true" CodeBehind="WebUserControlImpactoAlteracoesFuncionarios.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlImpactoAlteracoesFuncionarios" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Src="WebUserControlChartBarraImpactoAlteracoesFuncionarios.ascx" TagName="WebUserControlChartBarraImpactoAlteracoesFuncionarios"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50731.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<div class="GlobalUserControl">
    
<%--    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelBloqueio" ShowHeader="true" Width="100%"
        HeaderText="Informações sobre Atualização" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentInfo" runat="server" SupportsDisabledAttribute="True">
            <label ID="LabelMsgData">Última Atualização em </label>
            <label ID="LabelData" style="font-weight:bold;">11/11/2010</label><br /><br />
            <label ID="LabelPeriodo">Período de Análise (MM/AAAA): </label>
            <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="70px" Border-BorderColor="#c0dfe8"
                                            ID="TextBoxCompetenciaInicial" runat="server">
                                            <MaskSettings Mask="99/9999" ErrorText="Preencha o campo competência completammente." />
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                        </dx:ASPxTextBox>  a  
            <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="70px" Border-BorderColor="#c0dfe8"
                                            ID="TextBoxCompetenciaFinal" runat="server">
                                            <MaskSettings Mask="99/9999" ErrorText="Preencha o campo competência completammente." />
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                        </dx:ASPxTextBox>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ButtonAplicar" Text="Aplicar" class="BotaoEstiloGlobal" runat="server" onclick="ButtonAplicar_Click" />                                                    
            </dx:PanelContent>
        </PanelCollection> 
    </dx:ASPxRoundPanel>
--%>    

<%--    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelGrafico" ShowHeader="true" Width="100%"
        HeaderText="Movimentação de Margens" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="GraficoMargem" runat="server"  SupportsDisabledAttribute="True">
                <uc3:WebUserControlChartBarraImpactoAlteracoesFuncionarios ID="WebUserControlChartBarraImpactoAlteracoesFuncionarios" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>

                <table border="0">
                    <tr>
                        <td>Competência:</td>
                        <td>
                            <asp:Label ID="LabelCompetencia" runat="server"></asp:Label>
                                <asp:Button ID="ButtonPeriodoAnterior" runat="server" Text="<<" CssClass="BotaoEstiloGlobal"
                                    OnClick="Anterior_Click" />&nbsp;
                                <asp:Button ID="ButtonPeriodoProximo" runat="server" Text=">>" CssClass="BotaoEstiloGlobal"
                                    OnClick="Proximo_Click" />

                        </td>
                    </tr>
                    <%--<tr><td><asp:Label runat="server" ID="IDLabelConsignataria" Text="Consignatária:"></asp:Label></td><td><asp:DropDownList ID="DropDownListConsignataria" CssClass="TextBoxDropDownEstilos" DataTextField="Nome" DataValueField="IDEmpresa" runat="server" Width="350"></asp:DropDownList></td></tr>--%>
                </table>

    <br />
    
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelBoasNoticias" ShowHeader="true" Width="100%"
        HeaderText="Boas Notícias" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentBoasNoticias" runat="server"  SupportsDisabledAttribute="True">

        <dx:ASPxGridView EnableTheming="True" ID="gridAumentoMargens" 
            ClientInstanceName="gridAumentoMargens" 
            runat="server" KeyFieldName="ID" Width="100%" 
            AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"  
            CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
            <Columns>
                <dx:GridViewDataColumn FieldName="Grupo" VisibleIndex="1" />
<%--                <dx:GridViewDataTextColumn FieldName="Total" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" >
                    <PropertiesTextEdit DisplayFormatString="c" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Possibilidade" Caption="Possibilidade de Negócios" VisibleIndex="3" HeaderStyle-HorizontalAlign="Center" >
                    <PropertiesTextEdit DisplayFormatString="c" />
                </dx:GridViewDataTextColumn>--%>
            </Columns>
            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                </LoadingPanelOnStatusBar>
                <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                </LoadingPanel>
            </Images>
            <ImagesEditors>
                <DropDownEditDropDown>
                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                        PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
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
                                            <dx:ASPxGridView ID="gridBoasNoticias" runat="server" KeyFieldName="Id"  Width="100%"
                                                AutoGenerateColumns="False" ClientInstanceName="gridBoasNoticias" OnDataBinding="gridBoasNoticias_DataSelect"
                                                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Tipo" VisibleIndex="1" />
                                                    <dx:GridViewDataColumn FieldName="Qtde" VisibleIndex="2" Caption="Quantidade" HeaderStyle-HorizontalAlign="Center" />
                                                    <dx:GridViewDataTextColumn FieldName="ValorMargem" VisibleIndex="3" Caption="Valor Margem" HeaderStyle-HorizontalAlign="Center">
                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Possibilidade" VisibleIndex="4" Caption="Possibilidade de Negócios" HeaderStyle-HorizontalAlign="Center">
                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>

                                                <Templates>
                                                    <DetailRow>
                                                        <div style="padding: 3px 3px 2px 3px">
                                                            <dx:ASPxGridView ID="gridBoasNoticiasDetalhe" runat="server" KeyFieldName="Id"  Width="100%"
                                                            OnDataBinding="gridBoasNoticiasDetalhe_DataSelect" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                                                                <Columns>
                                                                    <dx:GridViewDataColumn FieldName="Cpf" VisibleIndex="1" />
                                                                    <dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="2" Caption="Matrícula" />
                                                                    <dx:GridViewDataColumn FieldName="Nome" VisibleIndex="3" Caption="Nome" />
                                                                    <dx:GridViewDataColumn FieldName="Telefone" VisibleIndex="3" Caption="Telefone" />
                                                                        <dx:GridViewDataTextColumn FieldName="MargemAnterior" VisibleIndex="4" Caption="Margem Anterior" HeaderStyle-HorizontalAlign="Center">
                                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="MargemAtual" VisibleIndex="4" Caption="Margem Atual" HeaderStyle-HorizontalAlign="Center">
                                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="SaldoRestante" VisibleIndex="4" Caption="Saldo Restante" HeaderStyle-HorizontalAlign="Center">
                                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Possibilidade" VisibleIndex="4" Caption="Possibilidade de Negócios" HeaderStyle-HorizontalAlign="Center">
                                                                        <PropertiesTextEdit DisplayFormatString="c" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>

                                                                <Settings ShowFooter="False" />
                                                                <SettingsText EmptyDataRow="Sem dados para exibição!" />
                                                            </dx:ASPxGridView>
                                                        </div>
                                                    </DetailRow>
                                                </Templates>

                                                <Settings ShowFooter="False" />
                                                <SettingsDetail ShowDetailRow="true" />
                                                <SettingsCustomizationWindow Enabled="True" />
                                                <SettingsLoadingPanel ImagePosition="Top" />
                                                <Settings ShowGroupPanel="False" />
                                                <Settings ShowFilterRow="False" />
                                                <SettingsText EmptyDataRow="Sem dados para exibição!" />
                                            </dx:ASPxGridView>
                    </div>
                </DetailRow>
            </Templates>
            
            <SettingsLoadingPanel ImagePosition="Top" />
            <SettingsDetail ShowDetailRow="true" />
            <Settings ShowGroupPanel="False" />
            <Settings ShowFilterRow="False" />
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsText EmptyDataRow="Sem dados para exibição!" />
           
        </dx:ASPxGridView>


            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    
    <br />

    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelMaNoticias" ShowHeader="true" Width="100%"
        HeaderText="Notícias Críticas" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentMasNoticias" runat="server"  SupportsDisabledAttribute="True">

        <dx:ASPxGridView EnableTheming="True" ID="gridMasNoticias" 
            ClientInstanceName="gridMasNoticias" 
            runat="server" KeyFieldName="Id" Width="100%" 
            AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
            <Columns>
                <dx:GridViewDataColumn FieldName="DescricaoGrupo" VisibleIndex="1" Caption="Grupo" Width="34%" />
                <dx:GridViewDataColumn FieldName="Quantidade" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" Width="22%" />    
                <dx:GridViewDataColumn FieldName="QuantidadeContratos" VisibleIndex="3" HeaderStyle-HorizontalAlign="Center" Caption="Qtd. Contratos" Width="22%" />             
                <dx:GridViewDataTextColumn FieldName="SaldoDevedor" VisibleIndex="4" HeaderStyle-HorizontalAlign="Center" Caption="Saldo Devedor em Aberto" Width="22%" >
                    <PropertiesTextEdit DisplayFormatString="c" />
                </dx:GridViewDataTextColumn>
            </Columns>
            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                </LoadingPanelOnStatusBar>
                <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                </LoadingPanel>
            </Images>
            <ImagesEditors>
                <DropDownEditDropDown>
                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                        PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
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
                       <dx:ASPxGridView ID="gridMasNoticiasDetalhe" runat="server" KeyFieldName="Id"  Width="100%"
                                        OnDataBinding="gridMasNoticias_DataSelect" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                           <Columns>
                                <dx:GridViewDataColumn FieldName="Cpf" VisibleIndex="1" />
                                <dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="2" Caption="Matrícula" />
                                <dx:GridViewDataColumn FieldName="Nome" VisibleIndex="3" />
                                <dx:GridViewDataColumn FieldName="Motivo" VisibleIndex="4" />
                                <dx:GridViewDataColumn FieldName="Parcela" VisibleIndex="5" HeaderStyle-HorizontalAlign="Center" />
                                <dx:GridViewDataTextColumn FieldName="ValorParcela" Caption="Valor Parcela" HeaderStyle-HorizontalAlign="Center" VisibleIndex="6">
                                    <PropertiesTextEdit DisplayFormatString="c" />
                                </dx:GridViewDataTextColumn>                                
                                <dx:GridViewDataColumn FieldName="Prazo" VisibleIndex="7" HeaderStyle-HorizontalAlign="Center" />
                                <dx:GridViewDataTextColumn FieldName="ValorTotal" Caption="Valor Total" HeaderStyle-HorizontalAlign="Center" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="c" />
                                </dx:GridViewDataTextColumn>                                                                
                                <%--<dx:GridViewDataColumn FieldName="QtdeContratos" VisibleIndex="5" Caption="Qtde. Contratos" HeaderStyle-HorizontalAlign="Center" />--%>
                                <dx:GridViewDataTextColumn FieldName="SaldoDevedor" VisibleIndex="9" HeaderStyle-HorizontalAlign="Center" Caption="Saldo Devedor" >
                                    <PropertiesTextEdit DisplayFormatString="c" />
                                </dx:GridViewDataTextColumn>
                           </Columns>
                           <Settings ShowFooter="False" />
                        </dx:ASPxGridView>
                   </div>
                </DetailRow>

            </Templates>
            
            <SettingsLoadingPanel ImagePosition="Top" />
            <SettingsDetail ShowDetailRow="true" />
            <Settings ShowGroupPanel="False" />
            <Settings ShowFilterRow="False" />
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsText EmptyDataRow="Sem dados para exibição!" />
            <%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
            --%>
        </dx:ASPxGridView>

        <br />
        <%--  MAS NOTICIAS - Inadimplentes          --%>

        <dx:ASPxGridView EnableTheming="True" ID="gridMasNoticiasInadiplentes" 
            ClientInstanceName="gridMasNoticiasInadiplentes" 
            runat="server" KeyFieldName="Id" Width="100%" 
            AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua" SettingsBehavior-AllowSort="false" >
            <Columns>
                <dx:GridViewDataColumn FieldName="DescricaoGrupo" VisibleIndex="1" Caption="Grupo" Width="34%" />
                <dx:GridViewDataColumn FieldName="Quantidade" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" Width="22%" />
                <dx:GridViewDataColumn FieldName="Parcelas" VisibleIndex="3" HeaderStyle-HorizontalAlign="Center" Width="22%" />
                <dx:GridViewDataTextColumn FieldName="NaoConciliado" VisibleIndex="4" HeaderStyle-HorizontalAlign="Center" Width="22%" Caption="Não Conciliado" >
                    <PropertiesTextEdit DisplayFormatString="c" />
                </dx:GridViewDataTextColumn>
            </Columns>
            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                </LoadingPanelOnStatusBar>
                <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                </LoadingPanel>
            </Images>
            <ImagesEditors>
                <DropDownEditDropDown>
                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                        PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
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
                       <dx:ASPxGridView ID="gridMasNoticiasInadiplentesDetalhe" runat="server" KeyFieldName="Id"  Width="100%"
                                        OnDataBinding="gridMasNoticiasInadiplentes_DataSelect" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SettingsBehavior-AllowSort="false">
                           <Columns>
                                <dx:GridViewDataColumn FieldName="Cpf" VisibleIndex="1" />
                                <dx:GridViewDataColumn FieldName="Matricula" VisibleIndex="2" Caption="Matrícula" />
                                <dx:GridViewDataColumn FieldName="Nome" VisibleIndex="3" />
                                <dx:GridViewDataColumn FieldName="QtdeParcelas" VisibleIndex="4" Caption="Qtde. Parcelas" HeaderStyle-HorizontalAlign="Center" />
                                <dx:GridViewDataTextColumn FieldName="Valor" VisibleIndex="5" HeaderStyle-HorizontalAlign="Center" Caption="Valor" >
                                    <PropertiesTextEdit DisplayFormatString="c" /> 
                                </dx:GridViewDataTextColumn>
                           </Columns>
                           <Settings ShowFooter="False" />
                           <SettingsText EmptyDataRow="Sem dados para exibição!" />
                        </dx:ASPxGridView>
                   </div>
                </DetailRow>

            </Templates>
            
            <SettingsLoadingPanel ImagePosition="Top" />
            <SettingsDetail ShowDetailRow="true" />
            <Settings ShowGroupPanel="False" />
            <Settings ShowFilterRow="False" />
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsText EmptyDataRow="Sem dados para exibição!" />
            <%--        <SettingsPager AlwaysShowPager="true" Mode="ShowPager" PageSize="20" Visible="true" />
            --%>
        </dx:ASPxGridView>


                
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>


</div>
