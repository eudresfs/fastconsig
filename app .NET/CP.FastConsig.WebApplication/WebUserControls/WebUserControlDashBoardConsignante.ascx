<%@ Control Language="C#" ClassName="WebUserControlDashBoardConsignante" AutoEventWireup="True"
    CodeBehind="WebUserControlDashBoardConsignante.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlDashBoardConsignante" %>
<%@ Import Namespace="CP.FastConsig.Common" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="WebUserControlChartAreaEnviadosDescontados.ascx" TagName="WebUserControlChartAreaEnviadosDescontados"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<div class="DivFlutuaEsquerda" >
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelParametros" HeaderStyle-Font-Bold="true"
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="100px" ContentPaddings-PaddingTop="35px"
            ContentPaddings-PaddingRight="0px" CssPostfix="Aqua"
        HeaderText="Quadro de Principais Parâmetros" Height="270px" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentParametros" runat="server" Height="270px" SupportsDisabledAttribute="True">
                <%-- TABELA DE PARAMETROS--%>
                <dx:ASPxGridView CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua" EnableTheming="False" ID="gridParametrosGerais" ClientInstanceName="gridParametrosGerais"
                    Width="100%" runat="server" KeyFieldName="Nome" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="Nome" VisibleIndex="1" Caption="Parâmetro"
                            Width="300px" />
                        <dx:GridViewDataColumn FieldName="Valor" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"
                            Caption="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </dx:GridViewDataColumn>
                    </Columns>
                    <SettingsDetail ShowDetailRow="False" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowFilterRow="False" />
                    <SettingsCustomizationWindow Enabled="True" />
                </dx:ASPxGridView>
                <br />
                <dx:ASPxGridView CssFilePath="~/App_Themes/Aqua/GridView/styles.css" SettingsPager-PageSize="11"
                        CssPostfix="Aqua" EnableTheming="False" Width="100%" ID="gridParametrosExecucao" ClientInstanceName="gridParametrosExecucao"
                    runat="server" KeyFieldName="Nome" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="Nome" VisibleIndex="1" Caption="Solicitação"
                            Width="300px" />
                        <dx:GridViewDataColumn FieldName="Prazo" Caption="Prazo (dias)" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </dx:GridViewDataColumn>
                    </Columns>
                    <SettingsDetail ShowDetailRow="False" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowFilterRow="False" />
                    <SettingsCustomizationWindow Enabled="True" />
                    <SettingsBehavior AllowSort="False"></SettingsBehavior>
                </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div class="DivFlutuaDireita">
    <dx:ASPxRoundPanel GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
        HeaderStyle-Font-Bold="true" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="60px" ContentPaddings-PaddingTop="60px"
            ContentPaddings-PaddingRight="0px" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
        runat="server" ID="ASPxRoundPanelEnviadosDescontados" Width="100%" HeaderText="Gráfico - A Descontar versus Descontados">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentEnviadosDescontados" runat="server" SupportsDisabledAttribute="True">
                <%-- GRÁFICO - A DESCONTAR x DESCONTADOS  --%>
                <uc1:WebUserControlChartAreaEnviadosDescontados ID="WebUserControlChartAreaEnviadosDescontados"
                    runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>    
<div class="DivFlutuaEsquerda">
    <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelSolicitacoesPendencias" Width="100%" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="115px" ContentPaddings-PaddingTop="0px"
            ContentPaddings-PaddingRight="0px"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" HeaderStyle-Font-Bold="true"
        HeaderText="Quadro de Solicitações e Pendências">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentSolicitacoesPendencias" runat="server" SupportsDisabledAttribute="True">                
               <br />
               <div>
                <asp:DropDownList id="DropDonwListOpcao" Runat="server" AutoPostBack="true" CssClass="TextBoxDropDownEstilos"  OnSelectedIndexChanged="selecionaOpcaoOcorrencia_Click">
                    <asp:ListItem Text="Minhas Solicitações" Value="1" Selected="true"/>
                    <asp:ListItem Text="Solicitações das Consignatárias" Value="3"/>
                </asp:DropDownList>
               </div>
               <br />
                    <dx:ASPxGridView EnableTheming="True" ID="gridSolicitacoesPendencias" ClientInstanceName="gridSolicitacoesPendencias"
                        runat="server" KeyFieldName="IDEmpresaSolicitacaoTipo" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua" >                    
                    <Columns>                        
                            <dx:GridViewDataHyperLinkColumn Caption=" " FieldName="Descricao" >
                                <DataItemTemplate>
                                <asp:LinkButton runat="server" OnClick="selecionaAverbacoes_Click" CommandArgument='<%# Container.KeyValue %>' Text='<%# Eval("Descricao") %>'></asp:LinkButton>
                                </DataItemTemplate>                            
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorContra" CellStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                    <dx:ASPxImage runat="server" ID="ASPxImageRedCircle" Width="20" ImageUrl="~/Imagens/RedBall.png" ToolTip="Prazo Expirado" />
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorNeutro" CellStyle-HorizontalAlign="Center" >
                                <HeaderTemplate>
                                    <center>
                                    <dx:ASPxImage runat="server" ID="ASPxImageGreenCircle" Width="20" ImageUrl="~/Imagens/YellowBall.png" ToolTip="Prazo Vence Hoje"/>
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorPro" CellStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                    <dx:ASPxImage runat="server" ID="ASPxImageGreenCircle" Width="20" ImageUrl="~/Imagens/GreenBall.png" ToolTip="Dentro do Prazo" />
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSort="False" ></SettingsBehavior>
                    <SettingsPager AlwaysShowPager="false" Visible="false" Mode="ShowAllRecords" ></SettingsPager>            
                    </dx:ASPxGridView>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<div class="DivFlutuaDireita" >
  
    <dx:ASPxRoundPanel CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="40px" ContentPaddings-PaddingTop="15px"
            ContentPaddings-PaddingRight="0px"
        GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
        HeaderStyle-Font-Bold="true" runat="server" ID="ASPxRoundPanelOcorrencias" Width="100%"
        HeaderText="Quadro de Ocorrências">
        <PanelCollection>
            <dx:PanelContent ID="PanelContentOcorrencias" runat="server" SupportsDisabledAttribute="True">
                <div>
                    <asp:DropDownList ID="DropDownListSolicitacaoTipo" runat="server" CssClass="TextBoxDropDownEstilos"
                        DataTextField="Nome" DataValueField="IDEmpresaSolicitacaoTipo" AutoPostBack="true"
                        Width="250px" OnSelectedIndexChanged="selecionaTipoSolicitacao_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <br />
                <asp:UpdatePanel runat="server" ID="upQuadroOcorrencias" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DropDownListSolicitacaoTipo" EventName="SelectedIndexChanged" />
                    </Triggers>
                    <ContentTemplate>                
                    <dx:ASPxGridView OnHtmlRowCreated="gridOcorrencias_HtmlRowCreated" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua" EnableTheming="False" ID="gridOcorrencias" ClientInstanceName="gridOcorrencias"
                        runat="server" KeyFieldName="ID" Width="100%" AutoGenerateColumns="False" SettingsText-EmptyDataRow="Sem dados para exibição!">
                    <Columns>
                        <dx:GridViewDataImageColumn FieldName="Logo" Caption=" " Width="50px" >
                            <DataItemTemplate>
                                <dx:ASPxImage ID="imgLogo" runat="server" ImageAlign="Middle" ImageUrl='<%#  string.Format("{0}", Eval("Logo"))  %>' />
                            </DataItemTemplate>
                        </dx:GridViewDataImageColumn>
                        <dx:GridViewDataColumn FieldName="Banco" VisibleIndex="1" Caption="Consignatária" Width="200px" />
                        <dx:GridViewDataColumn FieldName="Contratos" VisibleIndex="2" HeaderStyle-HorizontalAlign="Center"
                            Caption="Averbações">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="4" Caption="Alertar" HeaderStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <div style="text-align: center;">
                                    <asp:ImageButton ID="imgSolicitarContatos" OnClick="AdicionarSolicitacao_Click" runat="server"
                                        ImageUrl="~/Imagens/gtk-edit.png" CommandArgument='<%# Container.KeyValue %>'
                                        Width="16px" Height="16px"></asp:ImageButton>
                                </div>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="5" Caption="Suspender" HeaderStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <div style="text-align: center;">
                                    <asp:ImageButton ID="imgSuspender" OnClick="SuspenderConsignataria_Click" runat="server"
                                        CommandArgument='<%# Container.KeyValue %>' Width="16px"
                                        Height="16px"></asp:ImageButton>
                                </div>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDetail ShowDetailRow="False" />
                    <Settings ShowGroupPanel="False" />
                    <Settings ShowFilterRow="False" />
                    <SettingsBehavior AllowSort="False"></SettingsBehavior>
                    <SettingsCustomizationWindow Enabled="True" />
                </dx:ASPxGridView>
                </ContentTemplate>
                </asp:UpdatePanel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>  
</div>
<div class="LimpaFlutuaDiv" >
    &nbsp;</div>
<h1 class="TextoAncora">
    <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">Topo da Página</a>
</h1>
<div style="height: 5px;">
    &nbsp;
</div>
