<%@ Control Language="C#" AutoEventWireup="True" ClassName="WebUserControlFluxoAprovacao"
    EnableViewState="true" CodeBehind="WebUserControlFluxoAprovacao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFluxoAprovacao" %>
<%--<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxDocking" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%--<dx:ASPxButton runat="server" ID="btnShowHiddenPanels" ClientInstanceName="btnShowHiddenPanels"
    AutoPostBack="false" Text="Mostrar todos os níveis">
    <ClientSideEvents Click="function(s, e) { ShowHiddenDockPanels(); }" />
</dx:ASPxButton>
<br />--%>
<div>
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" HeaderStyle-Font-Bold="true"
        HeaderText="Fluxo de Aprovação" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
        <ContentPaddings Padding="14px" />
        <PanelCollection>
            <dx:PanelContent runat="server" ID="PanelFluxoAprovacao" SupportsDisabledAttribute="True">
                <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TituloNegrito" style="width: 10%;">
                            Tipo de Produto:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="170" runat="server" DataTextField="Nome"
                                DataValueField="IDProdutoGrupo" ID="cmbTipoProduto" AutoPostBack="true" OnSelectedIndexChanged="cmbTipoProduto_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="tr_modulo">
                        <td class="TituloNegrito">
                            Módulo:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" AutoPostBack="true"
                                DataTextField="Nome" DataValueField="IDModulo" ID="DropDownListModulo"
                                OnSelectedIndexChanged="DropDownListModulo_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="tr_consignataria" visible="false">
                        <td class="TituloNegrito">
                            Consignatária:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="TextBoxDropDownEstilos" runat="server" AutoPostBack="true"
                                DataTextField="Nome" Visible="false" DataValueField="IDEmpresa" ID="DropDownListConsignataria"
                                OnSelectedIndexChanged="DropDownListConsignataria_SelectedIndexChanged" />
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxCheckBox ID="cbFuncionario" runat="server" Text="Requer Aprovação do Funcionário">
                </dx:ASPxCheckBox>
                <br />
                <dx:ASPxCheckBox ID="cbConsignataria" runat="server" Text="Requer Aprovação da Consignatária">
                </dx:ASPxCheckBox>
                <br />
                <dx:ASPxCheckBox ID="cbConsignante" runat="server" Text="Requer Aprovação da Consignante">
                </dx:ASPxCheckBox>
                <br />
                <dx:ASPxButton CssClass="BotaoEstiloGlobal"  EnableDefaultAppearance="false" EnableTheming="false" runat="server" ID="btnSalvar_Fluxo" AutoPostBack="true" Text="Salvar"
                    OnClick="Salvar_Fluxo">
                </dx:ASPxButton>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</div>
<%--
<dx:ASPxButton ID="btnBuscar" runat="server"  AutoPostBack="true"    Text="Buscar" OnClick="Buscar_Click">
</dx:ASPxButton>
<dx:ASPxButton runat="server" ID="btnSalvar_Fluxo" AutoPostBack="true" Text="Salvar"
    OnClick="Salvar_Fluxo">
</dx:ASPxButton>
<br />
<div style="padding-bottom: 20px;">
    <dx:ASPxDockManager runat="server" ID="DockManager" ClientInstanceName="manager">
    </dx:ASPxDockManager>
    <dx:ASPxDockPanel runat="server" ID="pnConsignante" ClientInstanceName="pnConsignante"
        PanelUID="pnConsignante" OwnerZoneUID="zone" HeaderText="Consignante" VisibleIndex="0"
        AllowedDockState="DockedOnly">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxImage runat="server" ID="panel1Image" ClientInstanceName="panel1Image" ImageUrl="~/Imagens/Perfil.png">
                </dx:ASPxImage>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxDockPanel>
    <dx:ASPxDockPanel runat="server" ID="pnFuncionario" ClientInstanceName="pnFuncionario"
        PanelUID="pnFuncionario" OwnerZoneUID="zone" HeaderText="Funcionário" VisibleIndex="2"
        AllowedDockState="DockedOnly">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxImage runat="server" ID="ASPxImage1" ClientInstanceName="panel2Image" ImageUrl="~/Imagens/Perfil.png">
                </dx:ASPxImage>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxDockPanel>
    <dx:ASPxDockPanel runat="server" ID="pnConsignataria" ClientInstanceName="pnConsignataria"
        PanelUID="pnConsignataria" OwnerZoneUID="zone" HeaderText="Consignatária" VisibleIndex="3"
        AllowedDockState="DockedOnly">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxImage runat="server" ID="ASPxImage2" ClientInstanceName="panel3Image" ImageUrl="~/Imagens/Perfil.png">
                </dx:ASPxImage>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxDockPanel>
    <dx:ASPxDockZone runat="server" ID="ASPxDockZone" CssClass="zone" Orientation="Vertical"
        ZoneUID="zone" PanelSpacing="3px" Width="250px" Height="160px" >
    </dx:ASPxDockZone>--%>