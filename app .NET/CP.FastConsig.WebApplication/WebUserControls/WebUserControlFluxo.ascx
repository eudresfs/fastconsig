<%@ Control Language="C#" AutoEventWireup="True" ClassName="WebUserControlFluxo" EnableViewState="true" EnableTheming="false"
    CodeBehind="WebUserControlFluxo.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlFluxo" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxDocking" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<dx:ASPxButton runat="server" ID="btnShowHiddenPanels" ClientInstanceName="btnShowHiddenPanels"
    AutoPostBack="false" Text="Mostrar todos os níveis">
    <ClientSideEvents Click="function(s, e) { ShowHiddenDockPanels(); }" />
</dx:ASPxButton>
<br />
<table>
 <tr>
        <td>
            Tipo de Fluxo:
        </td>
        <td>
            <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="170" runat="server" DataTextField="Nome"
                DataValueField="IDFluxoTipo" ID="cmbTipoFluxo" />
        </td>
    </tr>
    <tr>
        <td>
            Tipo de Produto:
        </td>
        <td>
            <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="170" runat="server" DataTextField="Nome"
                DataValueField="IDProdutoGrupo" ID="cmbTipoProduto" />
        </td>
    </tr>
</table>
<dx:ASPxButton ID="btnBuscar" runat="server" AutoPostBack="true"    Text="Buscar" OnClick="Buscar_Click">
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
                Enviar Solicitação:
                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="170" runat="server" DataTextField="Nome"
                DataValueField="IDEmpresaSolicitacaoTipo" ID="cmbSolicitacaoTipo" />
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
    <dx:ASPxDockZone runat="server" ID="znComprador" CssClass="zone" Orientation="Vertical"
        ZoneUID="zncomprador" PanelSpacing="3px" Width="250px" Height="160px" >
    </dx:ASPxDockZone>
    <dx:ASPxDockZone runat="server" ID="znComprado" CssClass="zone" Orientation="Vertical"
        ZoneUID="zncomprado" PanelSpacing="3px" Width="250px" Height="160px" >
    </dx:ASPxDockZone>
</div>
