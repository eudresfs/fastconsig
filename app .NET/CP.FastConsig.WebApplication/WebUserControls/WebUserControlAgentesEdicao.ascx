<%@ Control ClassName="WebUserControlAgentesEdicao" Language="C#" AutoEventWireup="True"
    ViewStateMode="Enabled" EnableViewState="true" CodeBehind="WebUserControlAgentesEdicao.ascx.cs"
    Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlAgentesEdicao" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Src="WebUserControlUsuariosPermissoes.ascx" TagName="WebUserControlUsuariosPermissoes"
    TagPrefix="uc1" %>
<%@ Register Src="WebUserControlEmpresaSuspensoes.ascx" TagName="WebUserControlEmpresaSuspensoes"
    TagPrefix="uc4" %>
<%@ Register Src="WebUserControlContatos.ascx" TagName="WebUserControlContatos" TagPrefix="uc3" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <div>
        <table runat="server" width="100%" border="0" cellspacing="1" cellpadding="4" style="margin-top: 3px;">
            <tr>
                <td style="border-bottom: 2px solid #083772; padding: 7px 0px;">
                    <h1 style="font-size: 14px; font-weight: bold;">
                        Cadastro de Agentes</h1>
                </td>
            </tr>
            <tr>
                <td style="padding: 0;">
                    <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                        <tr>
                            <td class="TituloNegrito" style="width: 20%;">
                                Tipo:
                            </td>
                            <td>
                                <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="331" runat="server" DataTextField="Nome"
                                    DataValueField="IDEmpresaTipo" ID="DropDownListTipo" AutoPostBack="true" OnSelectedIndexChanged="DropDownListTipo_SelectedIndexChanged" />
                                &nbsp;&nbsp;<br />
                                <br />
                                <asp:DropDownList Enabled="false" CssClass="TextBoxDropDownEstilos" Width="331" runat="server" DataTextField="Nome"
                                    DataValueField="IDConsignataria" ID="DropDownListContribuinte" />
                                    &nbsp;&nbsp;<br />
                                <br />
                                <asp:DropDownList Visible="false" CssClass="TextBoxDropDownEstilos" Width="331" runat="server" DataTextField="Nome"
                                    DataValueField="IDEmpresa" ID="DropDownListConsignataria" />
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                CNPJ:
                            </td>
                            <td>
                                <dx:ASPxTextBox AutoPostBack="true" OnTextChanged="TextBoxCnpj_TextChanged" CssClass="TextBoxDropDownEstilos" Paddings-Padding="0" Width="331"
                                    Border-BorderColor="#c0dfe8" ID="TextBoxCnpj" runat="server">
                                    <MaskSettings Mask="99,999,999/9999-99" ErrorText="Preencha o campo CNPJ completammente." />
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                Razão Social:
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxRazao"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                Fantasia:
                            </td>
                            <td>
                                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxFantasia"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito">
                                Sigla:
                            </td>
                            <td>
                                <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxSigla"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TituloNegrito" style="border: none;">
                                Telefone:
                            </td>
                            <td style="border: none;">
                                <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="331" Border-BorderColor="#c0dfe8"
                                    ID="TextBoxTelefone" runat="server">
                                    <MaskSettings Mask="(99) 9999,9999" ErrorText="Preencha o campo telefone completammente." />
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <!-- Área que ficará mostrando / ocultando-->
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
                            <table class="WebUserControlTabela" width="100%" border="0" cellspacing="0">
                                <tr>
                                    <td class="TituloNegrito" style="width: 20%;">
                                        Fax:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="331" Border-BorderColor="#c0dfe8"
                                            ID="TextBoxFax" runat="server">
                                            <MaskSettings Mask="(99) 9999,9999" ErrorText="Preencha o campo telefone completammente." />
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxEmail"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Endereço:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxEndereco"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Bairro:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxBairro"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Complemento:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxComplemento"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Cidade:
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxDropDownEstilos" runat="server" ID="TextBoxCidade"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito">
                                        Estado:
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="TextBoxDropDownEstilos" Width="332"
                                            runat="server" DataTextField="Nome" DataValueField="SiglaEstado" ID="DropDownListEstado" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TituloNegrito" style="border: none;">
                                        Cep:
                                    </td>
                                    <td style="border: none;">
                                        <dx:ASPxTextBox CssClass="TextBoxDropDownEstilos" Width="331" Border-BorderColor="#c0dfe8"
                                            ID="TextBoxCep" runat="server">
                                            <MaskSettings Mask="99999-999" ErrorText="Preencha o campo telefone completammente." />
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div style="clear: both; overflow: hidden; width: 100%; height: 2px;">
            &nbsp;</div>
        <div style="text-align: left; width: 100%; padding-bottom: 5px; padding-top: 5px;">
            <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonSalvar" OnClick="ButtonSalvarClick"
                Text="Salvar" runat="server" />&nbsp;&nbsp;<asp:Button ID="ButtonCancelar" CssClass="BotaoEstiloGlobal"
                    Text="Voltar" runat="server" OnClick="ButtonCancelarClick" />&nbsp;&nbsp;
            <a runat="server" id="OkButton"></a>
        </div>
        <dx:ASPxPageControl ID="tabcontrol" Visible="False" Width="100%" runat="server" EnableCallBacks="true"
            ActiveTabIndex="0" EnableHierarchyRecreation="True" Height="100%" OnActiveTabChanged="tabcontrol_ActiveTabChanged"
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
            TabSpacing="3px">
            <TabPages>
                <dx:TabPage Text="Usuários" Name="aba_usuarios">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl3" runat="Server">
                            <uc1:WebUserControlUsuariosPermissoes ID="WebUserControlUsuarios1" ExibirTitulo="false"
                                IdRecursoWebUserControl="11" runat="server" />
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Contatos" Name="aba_contatos">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="Server">
                            <uc3:WebUserControlContatos ID="WebUserControlContatos1" ExibirTitulo="false" IdRecursoWebUserControl="52"
                                EnableViewState="true" runat="server" />
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Suspender/Ativar" Name="aba_suspensoes">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl5" runat="Server">
                            <uc4:WebUserControlEmpresaSuspensoes ID="WebUserControlEmpresaSuspensoes1" ExibirTitulo="false"
                                IdRecursoWebUserControl="52" runat="server" />
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
        <%--<h1 class="TextoAncora">
            <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
                Topo da Página</a>
        </h1>--%>
    </div>
</div>