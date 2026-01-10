<%@ Control ClassName="WebUserControlImportarExportar" Language="C#" AutoEventWireup="true" CodeBehind="WebUserControlImportarExportar.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlImportarExportar" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%--<div class="tela">
    
        <asp:Repeater ID="RepeaterSubMenuImpExp" OnItemDataBound="RepeaterSubMenuImpExp_ItemDataBound" runat="server">
            <ItemTemplate>
              
                    <dx:ASPxButton ForeColor="#004d63"  HoverStyle-BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesOverBtn.png" Border-BorderColor="#6ccfe9" 
                      ImagePosition="Top" BackgroundImage-ImageUrl="~/Imagens/BgMenuConfiguracoesBtn.png"  EnableTheming="True" EnableDefaultAppearance="True" 
                      AutoPostBack="false" OnClick="LinkSubMenu_Click" Width="200px" CssClass="MenuConfiguracoesBtn" ID="ASPxButtonSubMenu" runat="server" 
                      Text='<%# Eval("Nome") %>' Height="140px" CommandArgument='<%# Eval("IDRecurso") %>'>
                    </dx:ASPxButton> 
                
            </ItemTemplate>
        </asp:Repeater>
    
</div>--%>

<div>

    

    <center><br/><br/><br/><br/>
<a runat="server" id="LinkImportacao" OnServerClick="LinkImportacao_Click">
    <img id="Img1" runat="server" src="~/Imagens/ImportacaoExportacao.png" />
</a>
</center>
</div>