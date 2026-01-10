<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoPlay.aspx.cs" Inherits="CP.FastConsig.WebApplication.VideoPlay" %>
<%@ Register TagPrefix="Bewise" Namespace="Bewise.Web.UI.WebControls" Assembly="FlashControl, Version=2.4.2276.31815, Culture=neutral, PublicKeyToken=0066be59ecd613cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.: FastConsig - Visibilidade e Controle na Gestão do Crédito Consignado :.
	</title>
</head>
<body bgcolor="#efefef">
    <form id="form1" runat="server">
    <div>
        <center>
            <bewise:flashcontrol id="FlashControlVideo" runat="server" movieurl="~/Video.swf"
                browserdetection="False" scale="Showall" width="1024" height="810" />
        </center>
    </div>
    </form>
</body>
</html>
