<%@ Control ClassName="WebUserControlSubMenu" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlSubMenu.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlSubMenu" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxDataView" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="tela">
    <center>
        <dx:ASPxDataView EnableCallBacks="false" ItemSpacing="8px"   LoadingPanelText="Carregando..." ClientInstanceName="aSPxDataViewMenu"
            OnCustomCallback="ASPxDataViewMenu_CustomCallback" Cursor="Pointer" EmptyDataText="Não existem opções configuradas neste módulo!"
            EnableDefaultAppearance="false" HideEmptyRows="true" ID="ASPxDataViewMenu" runat="server"
            ColumnCount="3" RowPerPage="4" Width="40%">
            <ItemTemplate>
                <div onclick="aSPxDataViewMenu.PerformCallback(<%# Eval("IDRecurso") %>)" id="RepeaterSubMenuEstilos">
                    <table border="0" cellpadding="0" cellspacing="0" class="TabelaRepeaterMenuBotao">
                        <tr>
                            <td valign="middle" rowspan="2" id="LarguraCelulaBotao">
                                <asp:Image ImageUrl='<%# string.Format("~/Imagens/{0}", Eval ("Imagem")) %>' runat="server"
                                    ID="ImageIcone" />
                            </td>
                            <td id="AlturaLinha">
                                <asp:Label CssClass="TituloBotao" ID="Label1" runat="server" Text='<%# Eval("Nome") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td id="AlinhamentoDescricaoObjetivo" valign="top">
                                <h1 class="DescricaoObjetivo">
                                    <%#Eval ("Objetivo") %></h1>
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
            <PagerSettings>
                <AllButton Visible="True">
                </AllButton>
            </PagerSettings>
        </dx:ASPxDataView>
    </center>
</div>
