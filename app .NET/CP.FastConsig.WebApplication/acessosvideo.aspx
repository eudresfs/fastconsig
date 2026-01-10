<%@ Page Language="C#" AutoEventWireup="True" EnableViewState="true" CodeBehind="acessosvideo.aspx.cs" Inherits="CP.FastConsig.WebApplication.acessosvideo" %>

<%@ Register TagPrefix="Bewise" Namespace="Bewise.Web.UI.WebControls" Assembly="FlashControl, Version=2.4.2276.31815, Culture=neutral, PublicKeyToken=0066be59ecd613cb" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=11.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        
        .texto {

            font-family: Arial;
            border: 1px solid #a2a2a2;
            
        }
        
        .texto tr {
            
            height: 25px;
        }
        
        .texto td {
            
            font-size: 14px;
            color: #ffffff;
        }
        
        h1 {
            color: #EBCB17;
            font-size: 10px;
            display: inline; 
        }
        h2 {
            color: #EBCB17;
            font-size: 12px;
            display: inline; 
        }
    </style>
</head>
<body bgcolor="#efefef">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManagerPrincipal">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanelPrincipal">
        <ContentTemplate>
            <div id="DivDadosAcesso" runat="server" visible="true">
                <br /><br/><br/>
                <table style="background-image: url('/Imagens/bg_form.png'); height: 446px" border="0" cellspacing="0" cellpadding="0" class="texto" align="center" width="595px">
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td></td></tr>
                    <tr><td></td><td colspan="2"><h2>Dados de Acesso</h2></td></tr>
                    <tr>
                        <td style="width: 210px"></td>
                        <td><b>Nome:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxNome"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><b>Senha:</b></td>
                        <td><asp:TextBox runat="server" ID="TextBoxSenha" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" valign="top"><center><asp:Button runat="server" ID="ButtonAcessar" Text="Acessar" OnClick="ButtonAcessar_Click" />&nbsp;
                        <br/><br/><br/> <%--<h1>* Para solicitar o acesso não é necessário preencher o campo de senha.</h1>--%>
                <br /><br/><h1><asp:Label runat="server" ID="LabelPreencherCampo" Text="" Visible="false"></asp:Label></h1><br /><br/><h1><asp:Label runat="server" ID="Labelerro" Text="" Visible="false"></asp:Label></h1></center></td>
                    </tr>
                </table>
                
            </div>
            <div id="DivGrid" runat="server" visible="false">
<%--            		<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" GridViewID="grid" runat="server">
		</dx:ASPxGridViewExporter>--%>
		<dx:ASPxGridView SettingsLoadingPanel-Text="Carregando..." 
			SettingsText-GroupPanel="Arraste uma coluna até aqui para agrupar por ela."
			SettingsText-EmptyDataRow="Sem dados para exibição!" EnableTheming="True" ID="grid"
			ClientInstanceName="grid"
			runat="server" KeyFieldName="Id"
			AutoGenerateColumns="False" Font-Size="11px" Width="100%" CssFilePath="~/App_Themes/Aqua/GridView/styles.css"
			CssPostfix="Aqua">
			<Settings ShowGroupPanel="True" />
			<Columns>

				<dx:GridViewDataTextColumn FieldName="DataAcesso" Caption="Data" Width="100px"
					CellStyle-HorizontalAlign="Left">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataColumn FieldName="Nome" Caption="Nome" Width="70px">
				</dx:GridViewDataColumn>
				<dx:GridViewDataColumn FieldName="Empresa" Caption="Empresa/Órgão" Width="250px"
					CellStyle-Wrap="False" />
				<dx:GridViewDataColumn FieldName="Estado" Caption="Estado" Visible="true" />
				<dx:GridViewDataColumn FieldName="Telefone" />
				<dx:GridViewDataColumn FieldName="Email" Caption="Email"/>				
			</Columns>			
			<Settings ShowFilterRow="True" ShowFooter="true" />
			<SettingsLoadingPanel ImagePosition="Top" />
			<SettingsBehavior ColumnResizeMode="Control" />
			<Images SpriteCssFilePath="~/App_Themes/Aqua/GridView/sprite.css">
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
			<Styles CssFilePath="~/App_Themes/Aqua/GridView/styles.css" CssPostfix="Aqua">
				<LoadingPanel ImageSpacing="8px">
				</LoadingPanel>
			</Styles>
			<StylesEditors>
				<CalendarHeader Spacing="1px">
				</CalendarHeader>
				<ProgressBar Height="25px">
				</ProgressBar>
			</StylesEditors>
		</dx:ASPxGridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
