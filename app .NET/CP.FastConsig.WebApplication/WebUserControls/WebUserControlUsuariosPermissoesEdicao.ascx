<%@ Control ClassName="WebUserControlUsuariosPermissoesEdicao" Language="C#" AutoEventWireup="true"
    CodeBehind="WebUserControlUsuariosPermissoesEdicao.ascx.cs" Inherits="CP.FastConsig.WebApplication.WebUserControls.WebUserControlUsuariosPermissoesEdicao" %>
<%@ Register Assembly="FlashControl" Namespace="Bewise.Web.UI.WebControls" TagPrefix="Bewise" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxUploadControl" Assembly="DevExpress.Web.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<div class="GlobalUserControl">
    <div>
        <table class="WebUserControlTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3" class="BordaBase">
                    <h1 class="TituloTabela">
                        Dados do Funcionário</h1>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    CPF:
                </td>
                <td>
                    <dx:ASPxTextBox TabIndex="1" AutoPostBack="true" OnTextChanged="TextBoxCpf_TextChanged"
                        CssClass="TextBoxDropDownEstilos" Border-BorderColor="#c0dfe8" Width="330px"
                        Height="30px" ID="TextBoxCpf" runat="server">
                        <MaskSettings Mask="999,999,999-99" ErrorText="Preencha o campo CPF completammente." />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="width: 20%;">
                    Nome:
                </td>
                <td>
                    <asp:TextBox TabIndex="2" runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxNome"></asp:TextBox>
                </td>
                <td valign="bottom" style="border: none; padding: 5px;" class="BoxCapturaFoto" rowspan="5">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border: none;">
                                <div id="DivOpcaoFoto" runat="server">
                                    <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonExibirFotoFlash" OnClick="ButtonExibirFotoFlash_Click"
                                        Text="Iniciar Captura" runat="server" />&nbsp;
                                    <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonExibirFotoUpload" OnClick="ButtonExibirFotoUpload_Click"
                                        Text="Carregar Arquivo" runat="server" />
                                </div>
                                <div id="DivFotoFlash" runat="server">
                                    <Bewise:FlashControl WMode="Opaque" BackColor="Aqua" ID="FlashControlCam" Width="260px"
                                        Height="190px" MovieUrl="~/Flash/webcam.swf" runat="server" />
                                </div>
                                <div id="DivFotoUpload" runat="server">
                                    <table class="DivFotoUploadTabela" width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <span class="TituloUpload">Selecione um arquivo:</span>
                                                <dx:ASPxUploadControl BrowseButtonStyle-CssClass="BotaoEstiloGlobal" Height="29"
                                                    BrowseButtonStyle-Border-BorderColor="#a3c0e8" BrowseButtonStyle-Border-BorderStyle="Solid"
                                                    BrowseButtonStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderStyle="Solid"
                                                    TextBoxStyle-BackColor="#ffffff" TextBoxStyle-Border-BorderWidth="1px" TextBoxStyle-Border-BorderColor="#a3c0e8"
                                                    EnableDefaultAppearance="false" EnableTheming="False" BrowseButtonStyle-Font-Size="11px"
                                                    BrowseButton-Text="Procurar" ID="ASPxUploadControlFoto" runat="server" ClientInstanceName="uploader"
                                                    ShowProgressPanel="false" Size="35" OnFileUploadComplete="ASPxUploadControlFoto_FileUploadComplete">
                                                    <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
                                                        FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                                                        TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
                                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg,.jpeg">
                                                    </ValidationSettings>
                                                </dx:ASPxUploadControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton Cursor="pointer" Paddings-Padding="0px" CssClass="BotaoEstiloGlobal"
                                                    EnableDefaultAppearance="false" EnableTheming="false" ID="ASPxButtonUploadFoto"
                                                    runat="server" AutoPostBack="False" Text="Enviar" ClientInstanceName="btnUpload"
                                                    ClientEnabled="False">
                                                    <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LabelUpload" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="text-align: center" id="DivFotoAtual" runat="server">
                                    <asp:Image runat="server" ID="ImageFotoAtual" /><br />
                                    <br />
                                    <asp:Button CssClass="BotaoEstiloGlobal" Width="120" ID="ButtonTrocarFoto" OnClick="ButtonTrocarFoto_Click"
                                        Text="Trocar Foto" runat="server" /><br />
                                    <br />
                                </div>
                                <div id="DivCancelarOpcaoFoto" style="text-align: right; border: none; padding: 4px 0px;"
                                    runat="server">
                                    <asp:Button CssClass="BotaoEstiloGlobal" ID="ButtonCancelarOpcaoFoto" OnClick="ButtonCancelarOpcaoFoto_Click"
                                        Text="Voltar" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    E-mail:
                </td>
                <td>
                    <asp:TextBox TabIndex="3" runat="server" CssClass="TextBoxDropDownEstilos" ID="TextBoxEmail"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="border: none;">
                    Telefone:
                </td>
                <td style="border: none;">
                    <dx:ASPxTextBox TabIndex="4" CssClass="TextBoxDropDownEstilos" Border-BorderColor="#c0dfe8"
                        Width="330px" Height="30px" ID="TextBoxTelefone" runat="server">
                        <MaskSettings Mask="(99) 9999,9999" ErrorText="Preencha o campo telefone completammente." />
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                    </dx:ASPxTextBox>
                </td>
            </tr>
        </table>
        <table class="WebUserControlTabela" width="100%" border="0" cellpadding="4" cellspacing="1">
            <tr>
                <td class="BordaBase" colspan="2">
                    <h1 class="TituloTabela">
                        Dados de Acesso
                    </h1>
                </td>
            </tr>
            <tr>
                <td class="AplicaPaddingMensagem" colspan="2">
                    O <b>CPF</b>, o <b>Login/Apelido</b> ou a <b>Matrícula</b> (para funcionários) podem
                    ser utilizados para, juntamente com a senha, permitir o acesso ao sistema.
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="width: 20%;">
                    Tipo de Usuário:
                </td>
                <td>
                    <asp:DropDownList TabIndex="5" AutoPostBack="true" Height="30px" CssClass="TextBoxDropDownEstilos"
                        runat="server" OnSelectedIndexChanged="DropDownListModulo_SelectedIndexChanged"
                        DataTextField="Nome" DataValueField="IDModulo" ID="DropDownListModulo" />
                    <br />
                </td>
            </tr>
            <tr runat="server" id="TrConsignataria" visible="false">
                <td class="TituloNegrito">
                    <asp:Label ID="LabelAgenteConsignataria" runat="server" Text="Entidade:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="DropDownListConsignataria_SelectedIndexChanged"
                        DataTextField="Fantasia" Height="30px" CssClass="TextBoxDropDownEstilos" DataValueField="IDEmpresa"
                        runat="server" ID="DropDownListConsignataria" />
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Perfil:
                </td>
                <td>
                    <div>
                        <asp:DropDownList TabIndex="7" DataTextField="Nome" Height="30px" CssClass="TextBoxDropDownEstilos"
                            DataValueField="IDPerfil" runat="server" ID="DropDownListPerfil" />
                    </div>
                    <div id="DivPerfisCadastrados" runat="server">
                        <h2 class="trigger">
                            <a id="aTitulo" onclick="return false;" href="#">Perfis Cadastrados - Mais Detalhes</a>
                        </h2>
                    </div>
                </td>
            </tr>
            <tr runat="server" id="TrPerfisCadastrados" class="toggle_container">
                <td>
                </td>
                <td>
                    <asp:ListBox Rows="5" CssClass="ListBoxEstilos" runat="server" ID="ListBoxPerfisCadastrados"
                        DataTextField="Descricao" DataValueField="IDUsuarioPerfil" />
                    <br />
                    <br />
                    <asp:Button runat="server" CssClass="BotaoEstiloGlobal" ID="ButtonRemoverPerfil"
                        OnClick="ButtonRemoverPerfil_Click" Text="Remover Perfil" />
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito">
                    Login/Apelido:
                </td>
                <td>
                    <asp:TextBox TabIndex="8" ID="TextBoxLogin" CssClass="TextBoxDropDownEstilos" Width="320px"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="border: none;">
                    Senha Provisória:
                </td>
                <td valign="middle" style="border: none; padding: 0;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border: none; width: 100px;">
                                <dx:ASPxTextBox TabIndex="9" Border-BorderColor="#c0dfe8" Width="100px" Height="30px"
                                    MaxLength="6" CssClass="TextBoxDropDownEstilos" ValidationSettings-ErrorDisplayMode="ImageWithTooltip"
                                    ID="TextBoxSenhaProvisoria" runat="server">
                                    <MaskSettings Mask="<999999>" IncludeLiterals="DecimalSymbol" />
                                </dx:ASPxTextBox>
                                <asp:Label ForeColor="#FF3300" runat="server" ID="LabelSituacaoSenhaProvisoria"></asp:Label>
                            </td>
                            <td style="border: none;">
                                <asp:Button runat="server" CssClass="BotaoEstiloGlobal" ID="ButtonGerarSenhaProvisoria"
                                    OnClick="ButtonGerarSenhaProvisoria_Click" Text="Gerar Senha" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="TableResponsabilidades" runat="server" visible="false" class="WebUserControlTabela"
            width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3" class="BordaBase">
                    <h1 class="TituloTabela">
                        Responsabilidades</h1>
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="width: 20%; border: none">
                    Tipo de Solicitação:
                </td>
                <td style="border: none;">
                    <asp:DropDownList TabIndex="7" DataTextField="Nome" Height="30px" CssClass="TextBoxDropDownEstilos"
                        DataValueField="IDEmpresaSolicitacaoTipo" runat="server" ID="DropDownListTipoSolicitacoes" />
                    <asp:Button CssClass="BotaoEstiloGlobal" Width="80" ID="ButtonAdicionarSolicitacaoTipo"
                        OnClick="ButtonAdicionarSolicitacaoTipo_Click" Text="Adicionar" runat="server" />
                    &nbsp;<asp:Button CssClass="BotaoEstiloGlobal" Width="80" ID="ButtonRemoverSolicitacaoTipo"
                        OnClick="ButtonRemoverSolicitacaoTipo_Click" Text="Remover" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="TituloNegrito" style="border: none;">
                </td>
                <td style="border: none;">
                    <asp:ListBox Rows="5" CssClass="ListBoxEstilos" runat="server" ID="ListBoxTipoSolicitacoes"
                        DataTextField="Nome" DataValueField="IDEmpresaSolicitacaoTipo" />
                </td>
            </tr>
        </table>
        <div style="text-align: left; width: 100%; padding-top: 5px; padding-bottom: 5px;">
            <asp:Button CssClass="BotaoEstiloGlobal" Width="80" ID="ButtonSalvar" OnClick="ButtonSalvarClick"
                Text="Salvar" runat="server" />&nbsp;
            <asp:Button CssClass="BotaoEstiloGlobal" OnClientClick="return confirm('Tem certeza de que deseja remover?');"
                Width="80" ID="ButtonExcluir" OnClick="ButtonExcluir_Click" Text="Excluir" runat="server" />&nbsp;
            <asp:Button CssClass="BotaoEstiloGlobal" OnClientClick="return confirm('Tem certeza de que deseja bloquear?');"
                Width="80" ID="ButtonBloquear" OnClick="ButtonBloquear_Click" Text="Bloquear" runat="server" />
        </div>
    </div>
    <h1 class="TextoAncora">
        <a style="cursor: pointer" onclick="$('html, body').animate({scrollTop: 0}, 800); return false;">
            Topo da Página</a>
    </h1>
</div>
