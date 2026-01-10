<%@ Control Language="C#" ClassName="WebUserControlDashBoardConsignataria" AutoEventWireup="true"
    CodeBehind="WebUserControlDashBoardConsignataria.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlDashBoardConsignataria" %>
<%@ Import Namespace="CP.FastConsig.Common" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="WebUserControlChartAreaEnviadosDescontados.ascx" TagName="WebUserControlChartAreaEnviadosDescontados"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="LimpaFloatDiv" >
    <%--
    <div style="float: left; width: 100%; margin-top: 1%; clear: both; margin-bottom: 1%">
        <dx:ASPxRoundPanel DefaultButton="ButtonSimular" runat="server" ID="ASPxRoundPanelSimulacao" ShowHeader="true"
            Width="100%" HeaderStyle-Font-Bold="true" HeaderText="Central de Simulação" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            <PanelCollection>
                <dx:PanelContent ID="PanelContentSimulacao" runat="server" SupportsDisabledAttribute="True">
                    <asp:Label ForeColor="#083772" Font-Size="13px" runat="server" ID="LabelInformarMatrículaCpf"
                        Font-Bold="true" Text="Informe Matrícula/CPF: "></asp:Label>
                    &nbsp;
                    <asp:TextBox runat="server" ID="TextBoxMatriculaCpf" CssClass="TextBoxDropDownEstilos"></asp:TextBox>
                    <asp:Button CssClass="BotaoEstiloGlobal" runat="server" ID="ButtonSimular" Text="Simular"
                        OnClick="ButtonSimular_Click" />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    --%>
    <div class="DivFlutuaEsquerda">
        <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanelParametros" ShowHeader="true"
            Width="100%" ContentPaddings-PaddingBottom="8px" ContentPaddings-PaddingLeft="0px"
            ContentPaddings-PaddingRight="0px" HeaderText="Parâmetros Gerais" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
            HeaderStyle-Font-Bold="true">
            <PanelCollection>
                <dx:PanelContent ID="PanelContentParametros" runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewPeriodoFechamento" ClientInstanceName="gridParametrosGerais"
                        runat="server" KeyFieldName="Nome" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Parâmetro" FieldName="Nome" Width="60%">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Valor" FieldName="Valor" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowSort="False"></SettingsBehavior>
                    </dx:ASPxGridView>
                    <div style="height: 7px;">
                        &nbsp;</div>
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewPrazosExecucao" ClientInstanceName="gridParametrosGerais"
                        runat="server" KeyFieldName="Nome" Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Solicitação" FieldName="Nome" Width="60%">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Prazo (Dias)" FieldName="Prazo" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowSort="False"></SettingsBehavior>
                        <SettingsPager AlwaysShowPager="false" Visible="false" Mode="ShowAllRecords">
                        </SettingsPager>
                    </dx:ASPxGridView>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    <div class="DivFlutuaDireita" >
        <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel1" ShowHeader="true" Width="100%"
            ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingBottom="100px" ContentPaddings-PaddingTop="99px"
            ContentPaddings-PaddingRight="0px" HeaderText="Gráfico A Descontar x Descontados"
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" HeaderStyle-Font-Bold="true">
            <PanelCollection>
                <dx:PanelContent ID="PanelContentEnviadosDescontados" runat="server" SupportsDisabledAttribute="True">
                    <uc1:WebUserControlChartAreaEnviadosDescontados ID="WebUserControlChartAreaEnviadosDescontados"
                        runat="server" />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    <div class="AlturaLimpaFloatDiv"  >
        &nbsp;</div>
    <div class="TamanhoAlturaMargemLimpaDiv" >
        <dx:ASPxRoundPanel runat="server" ID="ASPxRoundPanel2" ShowHeader="true" Width="100%"
            ContentPaddings-PaddingLeft="0px" ContentPaddings-PaddingRight="0px" HeaderText="Solicitações e Pendências"
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px"
            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" HeaderStyle-Font-Bold="true">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <div>
                        <asp:DropDownList ID="DropDownListTipoPendencia" runat="server" CssClass="TextBoxDropDownEstilos"
                            EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="selecionaTipoPendencia_Click">
                            <asp:ListItem Text="Minhas Solicitações e Pendências" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Tudo que Solicitei Atendimento" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <br />
                    <dx:ASPxGridView EnableTheming="True" ID="ASPxGridViewPendencias" ClientInstanceName="ASPxGridViewPendencias"
                        SettingsText-EmptyDataRow="Sem solicitações e pendências!" runat="server" KeyFieldName="IDEmpresaSolicitacaoTipo"
                        Width="100%" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
                        CssPostfix="Aqua">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="Descricao">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="selecionaAverbacoes_Click"
                                        CommandArgument='<%# Container.KeyValue %>' Text='<%# Eval("Descricao") %>'></asp:LinkButton>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorContra" CellStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                        <dx:ASPxImage runat="server" ID="ASPxImageRedCircle" Width="20" ImageUrl="~/Imagens/RedBall.png"
                                            ToolTip="Prazo Expirado" />
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorNeutro" CellStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                        <dx:ASPxImage runat="server" ID="ASPxImageGreenCircle" Width="20" ImageUrl="~/Imagens/YellowBall.png"
                                            ToolTip="Prazo Vence Hoje" />
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption=" " FieldName="ValorPro" CellStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <center>
                                        <dx:ASPxImage runat="server" ID="ASPxImageGreenCircle" Width="20" ImageUrl="~/Imagens/GreenBall.png"
                                            ToolTip="Dentro do Prazo" />
                                    </center>
                                </HeaderTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowSort="False"></SettingsBehavior>
                        <SettingsPager AlwaysShowPager="false" Visible="false" Mode="ShowAllRecords">
                        </SettingsPager>
                    </dx:ASPxGridView>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    
</div>
<dx:ASPxPopupControl LoadingPanelText="Carregando..." ID="PopupControlAConquistar"
    runat="server" CloseAction="OuterMouseClick" LoadContentViaCallback="OnFirstShow"
    PopupElementID="ASPxImageAConquistar" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides"
    AllowDragging="False" Width="350px" Height="130px" HeaderText="Correr para Conquistar"
    ClientInstanceName="popupControlMensagens">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControlAConquistar" runat="server">
            <asp:Label runat="server" ID="LabelExplicacaoOpcaoAConquistar" Text='Aqui você obtém a quantidade de clientes que compõem a sua carteira, bem como o total de margem livre que os mesmos possuem e por fim, a prospectiva de negócio relacionado a esse volume de margem.'></asp:Label>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl LoadingPanelText="Carregando..." ID="PopupControlConquistado"
    runat="server" CloseAction="OuterMouseClick" LoadContentViaCallback="OnFirstShow"
    PopupElementID="ASPxImageConquistado" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides"
    AllowDragging="False" Width="350px" Height="130px" HeaderText="Conquistas Realizadas"
    ClientInstanceName="popupControlMensagens">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControlConquistado" runat="server">
            <asp:Label runat="server" ID="LabelExplicacaoOpcaoConquistado" Text='Aqui você tem a informação de quantos contratos foram fechados por tipo de averbação, bem como o volume de negócio agregado à sua carteira de contratos.'></asp:Label>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl LoadingPanelText="Carregando..." ID="PopupControlPerdas" runat="server"
    CloseAction="OuterMouseClick" LoadContentViaCallback="OnFirstShow" PopupElementID="ASPxImagePerdas"
    PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" AllowDragging="False"
    Width="350px" Height="130px" HeaderText="Vacilos - Trabalhar para se Antecipar"
    ClientInstanceName="popupControlMensagens">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControlPerdas" runat="server">
            <asp:Label runat="server" ID="LabelExplicacaoOpcaoPerdas" Style="float: left; width: 100%;
                clear: left; margin-bottom: 5px" Text='Aqui você obtém duas principais informações:'></asp:Label>
            <asp:Label runat="server" ID="LabelExplicacaoOpcaoPerdas2" Style="float: left; width: 100%;
                clear: left; margin-bottom: 5px" Text='1 - Perdas por Compra de Dívida, exibindo a quantidade de contratos perdidos para a concorrência e o valor subtraído da carteira.'></asp:Label>
            <asp:Label runat="server" ID="LabelExplicacaoOpcaoPerdas3" Style="float: left; width: 100%;
                clear: left; margin-bottom: 15px" Text='2 – Perdas de Negócios, exibindo os funcionários que estão na minha carteira e que tinham margem livre, porém preferiram a concorrência para realizar novas averbações. Esses tipos de informações são de alto poder gerencial, tendo em vista que a partir deles podem ser tomadas medidas preventivas para o enriquecimento da carteira.'></asp:Label>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<h1 class="TextoAncora">
    <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
        Topo da Página</a>
</h1>
<div class="AlturaDiv" style="height: 5px;">
    &nbsp;
</div>
