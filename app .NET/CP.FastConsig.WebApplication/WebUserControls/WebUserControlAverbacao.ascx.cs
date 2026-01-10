using System;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Util;
using System.Linq;
using DevExpress.Web.ASPxGridView;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAverbacao : CustomUserControl
    {

        private const string ParametroIdFunc = "IDFunc";
        private const string ParametroIdProdGrupo = "IDProdGrupo";
        private const string ParametroMargemDispFunc = "MargemDispFunc";
        private const string ParametroValida = "Valida";
        private const string ParametroJaSalvou = "JaSalvou";
        private const string ParametroIdAverbacao = "IdAverbacao";

        public string GetScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(GetAjax);
                sb.Append(DlgConfirmacao);
                sb.Append(ScriptFormatacaoCampos);

                sb.Append("function calculaconsig() {");
                sb.Append("  if ($('#hfValidaMargem').val() != 'S' && $('#txtValorParcela').val() != '' && parseFloat($('#txtValorParcela').convertevalor()) > parseFloat($('#txtMargemDisp').convertevalor()) )");
                sb.Append("  {");
                sb.Append("$(function() { ");
                sb.Append("  $('#dlgMargem').dialog({ ");
                sb.Append("     modal: true,");
                sb.Append("     show: 'blind',");
                sb.Append("     hide: 'explode',");
                sb.Append("     buttons: {");
                sb.Append("         Ok: function() {");
                sb.Append("          $(this).dialog('close');");
                sb.Append("         }");
                sb.Append("     }");
                sb.Append("  });");
                sb.Append("});");
                sb.Append("  }");
                sb.Append("var calculo = $('#txtPrazo').val() * $('#txtValorParcela').convertevalor();");
                sb.Append("var valor = mascaravalor(calculo.toFixed(2),2);");
                sb.Append("$('#txtValorConsignado').val(valor);");
                sb.Append("if ($('#hfPrazoMaximo').val() != '' && parseInt($('#txtPrazo').val()) > parseInt($('#hfPrazoMaximo').val()) )");
                sb.Append("  {");
                sb.Append("  $(function() { ");
                sb.Append("    $('#dlgPrazoMaximo').dialog({ ");
                sb.Append("       modal: true,");
                sb.Append("       show: 'blind',");
                sb.Append("       hide: 'explode',");
                sb.Append("       buttons: {");
                sb.Append("         Ok: function() {");
                sb.Append("          $(this).dialog('close');");
                sb.Append("         }");
                sb.Append("       }");
                sb.Append("    });");
                sb.Append("  });");
                sb.Append("  }");

                sb.Append("}");
                sb.Append(" ");
                sb.Append("function calculafator() {");
                sb.Append("var calculo = ($('#txtValorParcela').convertevalor()) / ($('#txtValorContrato').convertevalor());");
                sb.Append("if(!isFinite(calculo)) calculo = 0;");
                sb.Append("var valor = calculo.toFixed(6);");
                sb.Append("$('#txtCoeficiente').val(valor);");
                sb.Append("  if (parseFloat($('#txtValorContrato').convertevalor()) > parseFloat($('#txtValorConsignado').convertevalor()) )");
                sb.Append("  {");
                sb.Append("  $(function() { ");
                sb.Append("    $('#dlgValorContrato').dialog({ ");
                sb.Append("       modal: true,");
                sb.Append("       show: 'blind',");
                sb.Append("       hide: 'explode',");
                sb.Append("       buttons: {");
                sb.Append("         Ok: function() {");
                sb.Append("          $(this).dialog('close');");
                sb.Append("         }");
                sb.Append("       }");
                sb.Append("    });");
                sb.Append("  });");
                sb.Append("  }");


                sb.Append("}");
                sb.Append(" ");
                sb.Append("function calculacontrato() {");
                sb.Append("var calculo = $('#txtCoeficiente').convertevalor() * $('#txtValorParcela').convertevalor();");
                sb.Append("var valor = calculo.toFixed(2);");
                sb.Append("$('#txtValorContrato').val(valor);");


                sb.Append("}");
                sb.Append(" ");
                sb.Append("function calculamesfinal() {");
                sb.Append("var mes = $('#txtMesInicio').val();");
                sb.Append("mes = mes.substring(0,2);");
                sb.Append("var ano = $('#txtMesInicio').val();");
                sb.Append("ano = ano.substring(3,4);");
                sb.Append("if (parseInt(mes) > 12) {mes = 12;}");
                sb.Append("if (parseInt(mes) < 1) {mes = 1;}");
                //sb.Append("if (parseInt(ano) > 2200) {ano = (new Date).getFullYear();}");
                //sb.Append("if (parseInt(ano) < 2000) {ano = (new Date).getFullYear();}");
                //sb.Append("$('#txtMesInicio').val(mes + '/' + ano);");
                sb.Append("var valor = calculamesfim($('#txtMesInicio').val(),$('#txtPrazo').val());"); //$('#txtMesInicio').val()
                sb.Append("$('#txtMesFim').val(valor);");
                sb.Append("}");
                sb.Append(" ");
                return sb.ToString();
            }
        }

        public string ScriptFormatacaoCampos
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("$(document).ready(function(){");
                sb.Append("  $('#txtPrazo').priceFormat({ prefix: '', centsSeparator: '', thousandsSeparator: '', limit : 2, centsLimit: 0 });");
                sb.Append("  $('#txtCoeficiente').priceFormat({ prefix: '', thousandsSeparator: '', limit : 8, centsLimit: 6 });");
                sb.Append("  $('#txtCET').priceFormat({ prefix: '', thousandsSeparator: '', limit : 5, centsLimit: 2 });");
                sb.Append("  $('#txtValorParcela').priceFormat();");
                sb.Append("  $('#txtValorContrato').priceFormat();");
                sb.Append("  $('#txtValorConsignado').priceFormat();");
                sb.Append("  $('#txtMesInicio').mask('99/9999');");
                sb.Append("  $('#txtMesFim').mask('99/9999');");
                sb.Append("  $('#dfTelefone').mask('(99)9999-9999');");
                sb.Append("  $('#dfCelular').mask('(99)9999-9999');");
                
                sb.Append("});");
                return sb.ToString();
            }
        }

        public string GridSelecao
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("function gridRefinanciar_SelectionChanged(s,e) {");
                sb.Append("   s.GetSelectedFieldValues('ValorParcela',gridRefinanciar_GetSelectedFieldValuesCallback);");
                sb.Append("}");
                sb.Append(" ");
                sb.Append("function gridComprar_SelectionChanged(s,e) {");
                sb.Append("   s.GetSelectedFieldValues('ValorParcela',gridComprar_GetSelectedFieldValuesCallback);");
                sb.Append("}");
                sb.Append(" ");
                sb.Append("function gridRefinanciar_GetSelectedFieldValuesCallback(values) {");
                sb.Append("   document.getElementById('selCount').value=grid.GetSelectedRowCount();");
                sb.Append("}");
                sb.Append(" ");
                sb.Append("function gridComprar_GetSelectedFieldValuesCallback(values) {");
                sb.Append("   document.getElementById('selCount').innerHTML=grid.GetSelectedRowCount();");
                sb.Append("}");
                return sb.ToString();
            }
        }

        public string DlgConfirmacao
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("function textoItem(select) {");
                sb.Append("var texto;");
                sb.Append("  $('#'+select+' option:selected').each(function() {");
                sb.Append("    texto = $(this).text();");
                sb.Append("  });");
                sb.Append("  return texto;");
                sb.Append("}");
                sb.Append("function isValidEmailAddress(emailAddress) {");
                sb.Append(@"var pattern = new RegExp(/^(('[\w-+\s]+')|([\w-+]+(?:\.[\w-+]+)*)|('[\w-+\s]+')([\w-+]+(?:\.[\w-+]+)*))(@((?:[\w-+]+\.)*\w[\w-+]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][\d]\.|1[\d]{2}\.|[\d]{1,2}\.))((25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\.){2}(25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\]?$)/i);");
                sb.Append("return pattern.test(emailAddress);");
                sb.Append("}");
                sb.Append("function preenchedados() {");
                sb.Append("  $('#dfMat').val($('#txtMatricula').text());");
                sb.Append("  $('#dfNome').val($('#txtNome').text());");
                sb.Append("  $('#dfEndereco').val($('#txtEndereco').val());");
                sb.Append("  $('#dfBairro').val($('#txtBairro').val());");
                sb.Append("  $('#dfCidade').val($('#txtCidade').val());");
                //sb.Append("  $('#cmbEstado').val($('#txtEstado').val());");
                sb.Append("  $('#dfCep').val($('#txtCep').val());");
                sb.Append("  $('#dfEmail').val($('#txtEmail').val());");
                sb.Append("  $('#dfCPF').val($('#txtCPF').text());");
                sb.Append("  $('#dfRG').val($('#txtRG').text());");
                sb.Append("  $('#dfDataNasc').val($('#txtDataNasc').text());");
                sb.Append("  $('#dfTelefone').val($('#txtTelefone').val());");
                sb.Append("  $('#dfCelular').val($('#txtCelular').val());");

                sb.Append("  $('#txtCProduto').text(textoItem('cmbProduto'));");
                sb.Append("  $('#txtCValorContrato').text($('#txtValorContrato').val());");
                sb.Append("  $('#txtCPrazo').text($('#txtPrazo').val());");
                sb.Append("  $('#txtCValorParcela').text($('#txtValorParcela').val());");
                sb.Append("  $('#txtCValorConsig').text($('#txtValorConsignado').val());");
                sb.Append("  $('#txtCObs').text($('#txtObs').val());");
                sb.Append("  $('#txtCMesInicio').text($('#txtMesInicio').val());");
                sb.Append("  $('#txtCMesFim').text($('#txtMesFim').val());");
                sb.Append("  $('#txtCFator').text($('#txtCoeficiente').val());");
                sb.Append("  $('#txtCCET').text($('#txtCET').val());");
                sb.Append("  if ($('#hfAprovaFunc').val() == 'N') { ");
                sb.Append("         document.getElementById('DivAprovaFunc').setAttribute('style', 'display: none;');");
                sb.Append("  }");
                sb.Append("}");
                sb.Append("");
                sb.Append("function atribuidados() {");
                sb.Append("  $('#txtEndereco').val($('#dfEndereco').val());");
                sb.Append("  $('#txtBairro').val($('#dfBairro').val());");
                sb.Append("  $('#txtCidade').val($('#dfCidade').val());");
                sb.Append("  $('#txtEstado').val($('#cmbEstado').val());");
                sb.Append("  $('#txtCep').val($('#dfCep').val());");
                sb.Append("  $('#txtEmail').val($('#dfEmail').val());");
                sb.Append("  $('#txtTelefone').val($('#dfTelefone').val());");
                sb.Append("  $('#txtCelular').val($('#dfCelular').val());");
                sb.Append("}");
                sb.Append("");
                sb.Append("function validacoes() {");
                sb.Append("  if ($('#dfEmail').val() != '' && !isValidEmailAddress($('#dfEmail').val())) {");
                sb.Append("     alert('Email Inválido!');");
                sb.Append("     return false;");
                sb.Append("  }");
                sb.Append("  if ($('#hfAprovaFunc').val() != 'N') { ");
                sb.Append("    if (document.getElementById('cbAprovaFunc').checked == true) { return true; }");
                sb.Append("    else if (MD5($('#dfSenhaFunc').val().trim()) != $('#hfSenhaFunc').val().trim() && $('#dfSenhaFunc').val().trim() != $('#hfSenhaFunc').val().trim()) {alert('Senha Inválida!'); return false; }");
                sb.Append("  }");
                sb.Append("  return true;");
                sb.Append("}");
                sb.Append("");
                sb.Append("function confirmacao() {");
                sb.Append("  if (parseInt($('#txtPrazo').val()) <= 0) { ");
                sb.Append("     messagebox('Por favor, informe o Prazo.','Atenção','Ok');");
                sb.Append("     return false;");               
                sb.Append("  }");
                sb.Append("");
                sb.Append("");
                sb.Append("");
                sb.Append("  $('#dlgconfirmacao').dialog({");
                sb.Append("     draggable: true,");
                sb.Append("     autoOpen: false,");
                sb.Append("     position: 'center',");
                sb.Append(" 	show: 'blind',");
                //sb.Append("	    hide: 'puff',");
                sb.Append("	    width: 770,");
                sb.Append("	    height: 585,");
                sb.Append("	    modal: true,");
                sb.Append("     bgiframe: true,");
                //sb.Append("     open: function(type,data) {");
                //sb.Append("         $(this).parent().appendTo('form');");
                //sb.Append("         },");
                sb.Append("     buttons: {");
                sb.Append("         'Confirmar': function() {");
                //sb.Append("             alert('1');");
                //sb.Append("             $(\":button:contains('Confirmar')\").attr('disabled','disabled'); ");
                sb.Append("             jQuery('.ui-dialog button:nth-child(1)').button('disable');");
                //sb.Append("             alert('2');");
                sb.Append("             if (validacoes()) {");
                //sb.Append("                alert('passou validacao');");
                //sb.Append("                $(\":button:contains('Confirmar')\").attr('disabled','disabled'); ");
                sb.Append("                atribuidados();");
                sb.Append("                $( this ).dialog( 'close' );");
                sb.Append("                __doPostBack('Salvar', 'salvar_click');");
                sb.Append("                return true;");
                sb.Append("             } else { jQuery('.ui-dialog button:nth-child(1)').button('enable'); return false; } "); //$(\".ui-dialog-buttonpane button:contains('Confirmar')\").button('enable');
                sb.Append("         },");
                sb.Append("         'Desistir': function() {");
                sb.Append("             $( this ).dialog( 'close' );");
                sb.Append("             return false;");
                sb.Append("         }");
                sb.Append("     }");
                sb.Append("  });");
                //sb.Append("  $('#dlgconfirmacao').parent().appendTo($('form:first'));");
                //sb.Append("  preenchedados();");

                sb.Append("  if ($('#hfAprovaFunc').val() == 'N') { ");
                sb.Append("         document.getElementById('DivAprovaFunc').setAttribute('style', 'display: none;'); }");
                sb.Append("  $('#dlgconfirmacao').dialog( 'open' );");
                sb.Append("  preenchedados();");
                sb.Append("  return false;");
                sb.Append("}");
                sb.Append("");
                sb.Append("function confirmar() {");
                sb.Append("   $( '#dlgconfirmacao' ).dialog( 'close' );");
                sb.Append("   return true;");
                sb.Append("}");
                sb.Append("");
                sb.Append("function desistir() {");
                sb.Append("   $( '#dlgconfirmacao' ).dialog( 'close' );");
                sb.Append("   return false;");
                sb.Append("}");
                sb.Append(" ");
                sb.Append("function imprimedados() {");
                sb.Append("  $('#impNumero').text($('#txtNumeroAverbacao').text());");
                sb.Append("  $('#impBanco').text($('#txtBanco').val());");
                sb.Append("  $('#impData').text($('#txtData').val());");
                sb.Append("  $('#impCNPJ').text($('#txtCNPJ').val());");
                sb.Append("  $('#impUF').text($('#txtEstado').val());");
                sb.Append("  $('#impMatricula').text($('#txtMatricula').text());");
                sb.Append("  $('#impLocal').text($('#txtLocal').val());");
                sb.Append("  $('#impSetor').text($('#txtSetor').val());");
                sb.Append("  $('#impCargo').text($('#txtCargo').val());");
                sb.Append("  $('#impCategoria').text($('#txtCategoria').text());");
                sb.Append("  $('#impRegime').text($('#txtRegime').text());");
                sb.Append("  $('#impDataAdm').text($('#txtDataAdm').text());");
                sb.Append("  $('#impSituacao').text($('#txtSituacaoFunc').text());");
                sb.Append("  $('#impNome').text($('#txtNome').text());");
                sb.Append("  $('#impNome2').text($('#txtNome').text());");
                sb.Append("  $('#impEndereco').text($('#txtEndereco').val());");
                sb.Append("  $('#impBairro').text($('#txtBairro').val());");
                sb.Append("  $('#impCidade').text($('#txtCidade').val());");
                sb.Append("  $('#impCep').text($('#txtCep').val());");
                sb.Append("  $('#impEmail').text($('#txtEmail').val());");
                sb.Append("  $('#impCPF').text($('#txtCPF').text());");
                sb.Append("  $('#impRG').text($('#txtRG').text());");
                sb.Append("  $('#impDataNasc').text($('#txtDataNasc').text());");
                sb.Append("  $('#impTelefone').text($('#txtTelefone').val());");
                sb.Append("  $('#impCelular').text($('#txtCelular').val());");
                sb.Append("  $('#impProduto').text(textoItem('cmbProduto'));");
                sb.Append("  $('#impValorContrato').text($('#txtValorContrato').val());");
                sb.Append("  $('#impPrazo').text($('#txtPrazo').val());");
                sb.Append("  $('#impValorParcela').text($('#txtValorParcela').val());");
                sb.Append("  $('#impValorConsignado').text($('#txtValorConsignado').val());");
                sb.Append("  $('#impObs').text($('#txtObs').val());");
                sb.Append("  $('#impMesInicio').text($('#txtMesInicio').val());");
                sb.Append("  $('#impMesFim').text($('#txtMesFim').val());");
                sb.Append("  $('#impFator').text($('#txtCoeficiente').val());");
                sb.Append("  $('#impCET').text($('#txtCET').val());");
                sb.Append("}");
                sb.Append("");
                sb.Append("function imprimir() {");
                sb.Append("  $(function() { ");
                sb.Append("     $('#DivTermo').dialog({ ");
                sb.Append("        modal: true,");
                sb.Append(" 	   show: 'blind',");
                sb.Append("        width:770, ");
                sb.Append("        height:585,");
                sb.Append("	       hide: 'puff',");
                sb.Append("        buttons: {");
                sb.Append("           'Imprimir': function() {");
                sb.Append("               $('.printable').print(); ");
                sb.Append("            },");
                sb.Append("            'Fechar': function() {");
                sb.Append("               $( this ).dialog('close');");
                sb.Append("            }");
                sb.Append("       }");
                sb.Append("     });");
                sb.Append("  });");
                sb.Append("  $('#DivTermo').dialog('open');");
                sb.Append("  imprimedados();");
                sb.Append("  return false;");
                sb.Append("} ");

                return sb.ToString();
            }
        }

        public string GetAjax
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("var request = null;");
                //sb.Append("try {");
                //sb.Append("    request = new XMLHttpRequest();");
                //sb.Append("} catch (trymicrosoft) {");
                //sb.Append("    try {");
                //sb.Append("        request = new ActiveXObject('Msxml2.XMLHTTP');");
                //sb.Append("    } catch (othermicrosoft) {");
                //sb.Append("        try {");
                //sb.Append("            request = new ActiveXObject('Microsoft.XMLHTTP');");
                //sb.Append("        } catch (failed) {");
                //sb.Append("            request = null;");
                //sb.Append("        }");
                //sb.Append("    }");
                //sb.Append("}");

                sb.Append("function criarAjax() {");
                sb.Append("    try {");
                sb.Append("        request = new XMLHttpRequest();");
                sb.Append("    } catch (trymicrosoft) {");
                sb.Append("        try {");
                sb.Append("            request = new ActiveXObject('Msxml2.XMLHTTP');");
                sb.Append("        } catch (othermicrosoft) {");
                sb.Append("            try {");
                sb.Append("                request = new ActiveXObject('Microsoft.XMLHTTP');");
                sb.Append("            } catch (failed) {");
                sb.Append("                request = null;");
                sb.Append("            }");
                sb.Append("        }");
                sb.Append("    }");

                sb.Append("    if (request == null) { ");
                sb.Append("        alert('Não foi possível criar o objeto ajax!'); } ");
                sb.Append("}");

                sb.Append("function getDados() {");
                sb.Append("    if (request == null) criarAjax();");
                sb.Append("    if (request == null) return;");
                sb.Append("    var url = 'ListaAverbacoesVinc.aspx?id='+$('#hfAverbacao').val();");
                sb.Append("    url = url  + '&cache='+   new Date().getTime();");
                sb.Append("    request.open('GET', url, true);");
                sb.Append("    request.onreadystatechange = getDados_Response;");
                sb.Append("    request.send(null);");
                sb.Append("}");

                sb.Append("function getDados_Response() {");
                sb.Append("    if (request.readyState == 4) { ");
                sb.Append("        if (request.status == 200) { ");
                sb.Append("            var jsonData = eval('(' +   request.responseText +  ')');");
                sb.Append("            var tabela = '<table width=\"99%\"    border=\"0\" cellspacing=\"1\" cellpadding=\"0\" style=\"border: 1px solid #000000;font-family:Helvetica Neue ,Helvetica,Arial,Sans-serif;font-size: 8.5pt;\">';");
                sb.Append("            tabela = tabela + '<td>Número</td><td>Consignatária</td><td>Prazo</td><td>Valor Parcela</td>';");
                sb.Append("            for (i = 0; i < jsonData.length; i++  ) {");
                sb.Append("                tabela = tabela + '<tr>';");
                sb.Append("                tabela = tabela  + '<td> '+ jsonData[i].Numero+ '</td><td>' +jsonData[i].Consignataria+ '</td><td>' + jsonData[i].Prazo + '</td><td>' +jsonData[i].ValorParcela+ '</td>';");
                sb.Append("                tabela = tabela + '</tr>';");
                sb.Append("            }");
                sb.Append("            tabela = tabela + '</table><h1 style=\"font-size: 8pt; font-weight: bold;font-family:Helvetica,Arial,Sans-serif;display:inline;\"> Para que esta nova averbação seja válida para desconto em folha, depende da liquidação de todas estas averbações participantes. </h1>';");
                sb.Append("            document.getElementById('DivAverbacoes').innerHTML = tabela;");
                sb.Append("            request = null;");
                sb.Append("        }");
                sb.Append("    }");
                sb.Append("}");

                return sb.ToString();
            }
        }

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFunc] == null) ViewState[ParametroIdFunc] = 0;
                return (int)ViewState[ParametroIdFunc];
            }
            set
            {
                ViewState[ParametroIdFunc] = value;
            }
        }

        private bool Valida
        {
            get
            {
                if (ViewState[ParametroValida] == null) ViewState[ParametroValida] = true;
                return (bool)ViewState[ParametroValida];
            }
            set
            {
                ViewState[ParametroValida] = value;
            }
        }

        private bool JaSalvou
        {
            get
            {
                if (ViewState[ParametroJaSalvou] == null) ViewState[ParametroJaSalvou] = false;
                return (bool)ViewState[ParametroJaSalvou];
            }
            set
            {
                ViewState[ParametroJaSalvou] = value;
            }
        }
        private decimal MargemDispFunc
        {
            get
            {
                if (ViewState[ParametroMargemDispFunc] == null) ViewState[ParametroMargemDispFunc] = 0;
                return Convert.ToDecimal(ViewState[ParametroMargemDispFunc]);
            }
            set
            {
                ViewState[ParametroMargemDispFunc] = value;
            }
        }

        private int IdProdutoGrupo
        {
            get
            {
                if (ViewState[ParametroIdProdGrupo] == null) ViewState[ParametroIdProdGrupo] = 0;
                return (int)ViewState[ParametroIdProdGrupo];
            }
            set
            {
                ViewState[ParametroIdProdGrupo] = value;
            }
        }

        private int IdAverbacao
        {
            get
            {
                if (ViewState[ParametroIdAverbacao] == null) ViewState[ParametroIdAverbacao] = 0;
                return (int)ViewState[ParametroIdAverbacao];
            }
            set
            {
                ViewState[ParametroIdAverbacao] = value;
            }
        }

        private bool bSair = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((ControleCarregado && string.IsNullOrEmpty(Request["__EVENTARGUMENT"])) || bSair) return;

            Page.LoadComplete += Page_LoadComplete;
            //Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.Salvar));
            if (!EhPostBack)
            {
                if (!ValidaSuspensaoEmpresa((int)Enums.AverbacaoTipo.Normal))
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemEmpresaSuspensa);
                    return;
                }



                int idconsignante = Convert.ToInt32(FachadaGeral.IdEmpresaConsignante());
                impDataAverbacao.Text = string.Format("{0}, {1} de {2} de {3} {4}", FachadaConsignatarias.ObtemEmpresa(idconsignante).Cidade, DateTime.Today.ToString("dd"), DateTime.Today.ToString("MMMM"), DateTime.Today.ToString("yyyy"), DateTime.Now.ToString("HH:mm"));

                StringBuilder sb = new StringBuilder();
                //sb.Append("function SalvarConcluido() {");
                sb.Append("$(function() { ");
                sb.Append("  $('#DivSalvarConcluido').dialog({ ");
                sb.Append("     modal: true,");
                sb.Append("     show: 'blind',");
                sb.Append("     hide: 'puff',");
                sb.Append("     buttons: {");
                sb.Append("         Ok: function() {");
                sb.Append("          $( this ).dialog('close');");
                sb.Append("                __doPostBack('Salvarxx', 'sair');");
                sb.Append("         },");
                sb.Append("         'Imprimir Termo': function() {");
                sb.Append("           if ($('#hfCompra').val() == 'S') {");
                sb.Append("              getDados();");
                sb.Append("           }");
                sb.Append("           imprimir();");
                sb.Append("         }");
                sb.Append("     }");
                sb.Append("  });");
                sb.Append("});");
                //sb.Append("}");

                RegistrarStartupScript(this, "		function pageLoad() { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest); } " +
                "function onEndRequest(sender, args) { " + //

                
                //$(document).ready(function(){
                //// cria a tabela dinamica
                //var dv = $('#dv');
                //var table = $('<table></table>')
                //var rows = 10;
                //var cols = 10;                
                //for (var i = 0; i < rows; i++) {
                //    var tr = $('<tr><tr>');
                //    for (var j = 0; j < cols; j++) {
                //        $('<td></td>')
                //            .text("[x: " + i + ", y: " + j + "]")
                //            .appendTo(tr);
                //    }
                //    tr.appendTo(table);
                //}
                //dv.append(table);
                //$(table).find('tr:odd').css("background-color", "#cccccc");
                //});
                
                //"$(\".ui-dialog-buttonpane button:contains('Confirmar')\").button('enable'); " + 
                "if (sender._postBackSettings.sourceElement != null && sender._postBackSettings.sourceElement.id == 'Salvar' && $('#txtNumeroAverbacao').text() != '') {  $('#dlgconfirmacao').dialog('close'); " + sb.ToString() + "  } " + GetScript + " }");

                RegistrarBlockScript(this, GetScript, true);

                if (Valida)
                    LimpaCampos();

                EhPostBack = true;

                ConfiguraTopo();

                if (Id.HasValue && Id.Value > 0)
                    ConcluirCompra(Id.Value);
            }

            string eventArgs = Request["__EVENTARGUMENT"];

            if (!string.IsNullOrEmpty(eventArgs))
            {
                if (eventArgs.Equals("salvar_click"))
                {
                    EhPostBack = false;
                    //JaSalvou = true;
                    int idaverb = SalvarAverbacao();
                    //JaSalvou = false;
                    Valida = idaverb > 0;

                    //if (!Valida)
                    //    PageMaster.ExibeMensagem("Lançamento de Averbação Indevido! Favor entrar em contato com suporte técnico.");
                    
                    //Averbacao averb = FachadaAverbacoes.ObtemAverbacao(idaverb);

                    //GridViewAverbacoesVinc.DataSource = averb.AverbacaoVinculo1.Select(x => x.Averbacao);
                    //GridViewAverbacoesVinc.DataBind();

                    //Table t = new Table();

                    //TableRow r = new TableRow();

                    //TableCell c = new TableCell();
                    //c.Text = "TESTETESTESTESTESTE";
                    //r.Controls.Add(c);

                    //t.Controls.Add(r);

                    //DivAverbacoes.Controls.Add(t);

                }
                else if (eventArgs.Equals("sair"))
                {
                    //EhPostBack = false;
                    bSair = true;
                }
            }

            //if (cbRefinancia.Checked)
            //    PopulaGridRefinanciar();
            //if (cbCompra.Checked)
            //    PopulaGridComprar();

        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            if (bSair && PageMaster != null)
                PageMaster.F5(IdRecurso); //PageMaster.Voltar();
            else if (bSair)
                FachadaMaster.RegistrarErro(Request, "Prevenção de Erro --> Ocorreu alguma falha, pois o PageMaster estava nulo");
        }

        private void ConcluirCompra(int idaverbacao)
        {
            IdAverbacao = idaverbacao;
            LimpaCampos();

            Averbacao averb = FachadaAverbacoes.ObtemAverbacao(idaverbacao);
            IdFunc = averb.IDFuncionario;
            IdProdutoGrupo = averb.Produto.IDProdutoGrupo;

            PopulaDadosFunc();

            DadosAverbacao(averb, IdProdutoGrupo);

            //panelRefinancia.Visible = true;
            //panelCompra.Visible = true;
            //panelDadosFunc.Visible = true;
            //divEscolhaCompra.Visible = false;
            //DivDadosAverbacao.Visible = true;
            //DivSalvar.Visible = true;
            //upRefinancia.Update();
            //upCompra.Update();
            //upDadosAverbacao.Update();
            //upSalvar.Update();

        }

        private void Reiniciar()
        {
            PageMaster.F5(IdRecurso); //PageMaster.Voltar();
            //IdFunc = 0;
            //LimpaCampos();
            //gridRefinanciar.Enabled = true;
            //gridComprar.Enabled = true;
            //panelRefinancia.Visible = false;
            //panelCompra.Visible = false;
            //panelDadosFunc.Visible = false;
            //divEscolhaCompra.Visible = false;
            //DivDadosAverbacao.Visible = false;
            //DivSalvar.Visible = false;
            //upRefinancia.Update();
            //upCompra.Update();
            //upEscolhaCompra.Update();
            //upDadosAverbacao.Update();
            //upSalvar.Update();
        }

        private void PopulaCombos()
        {

            //cmbTipoProduto.DataSource = FachadaAverbacoes.ObtemProdutosGrupo().ToList();
            //cmbTipoProduto.DataBind();

            cmbProduto.DataSource = FachadaAverbacoes.ObtemProdutosDoEmpresa(Sessao.IdBanco).ToList();
            cmbProduto.DataBind();
            cmbProduto.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            cmbEstado.DataSource = FachadaAverbacoes.ObtemEstados();
            cmbEstado.DataBind();

            Empresa consignante = Empresas.ObtemEmpresa(Convert.ToInt32(FachadaGeral.IdEmpresaConsignante()));

            cmbProduto.SelectedIndex = 0;
            cmbEstado.SelectedIndex = cmbEstado.Items.IndexOf(cmbEstado.Items.FindByValue(consignante.Estado));
            
        }

        protected void cmbProduto_Select(object sender, EventArgs e)
        {
            var produtogrupo = FachadaAverbacoes.ObtemProdutoGrupoDeProduto(cmbProduto.SelectedValue);
            if (produtogrupo.HasValue)
                DadosAverbacao(null, produtogrupo.Value);
        }

        protected void gridRefinanciar_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.KeyValue);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, 0, 1, id);
        }

        protected void gridComprar_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.KeyValue);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, Sessao.IdModulo), 1, id);
        }

        protected void Desistir_Click(object sender, EventArgs e)
        {
            Reiniciar();
        }

        protected void Limpar_Click(object sender, EventArgs e)
        {
            Reiniciar();
        }

        //protected void Salvar_Click(object sender, EventArgs e)
        //{
        //    SalvarAverbacao();
        //}

        private int SalvarAverbacao()
        {
            List<object> listaRefinancia = gridRefinanciar.GetSelectedFieldValues("IDAverbacao");
            List<object> listaCompra = gridComprar.GetSelectedFieldValues("IDAverbacao");

            Averbacao dado = new Averbacao();

            if (listaRefinancia.Count > 0 && listaCompra.Count > 0)
                dado.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.CompraERenegociacao;
            else if (listaRefinancia.Count > 0)
                dado.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.Renegociacao;
            else if (listaCompra.Count > 0)
                dado.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.Compra;
            else
                dado.IDAverbacaoTipo = (int)Enums.AverbacaoTipo.Normal;

            dado = PopulaObjeto(dado);

            if (!ValidaInformacoes(dado))
            {
                return 0;
            }


            Pessoa pessoa = FachadaAverbacoes.ObtemFuncionario(IdFunc).Pessoa;

            pessoa = PopulaObjetoPessoa(pessoa);
            int ID = 0;

            if (IdAverbacao > 0)
            {
                ID = IdAverbacao;
                dado.IDAverbacao = IdAverbacao;
                FachadaAverbacoes.SalvarCompra(dado, pessoa, IdProdutoGrupo, listaRefinancia.Select(x => Convert.ToInt32(x)).ToList(), listaCompra.Select(x => Convert.ToInt32(x)).ToList());
            }
            else
            {
                ID = FachadaAverbacoes.SalvarAverbacao(dado, pessoa, IdProdutoGrupo, hfAprovaFunc.Value == "S" && !cbAprovaFunc.Checked, listaRefinancia.Select(x => Convert.ToInt32(x)).ToList(), listaCompra.Select(x => Convert.ToInt32(x)).ToList());
            }

            if (ID > 0)
            {
                var averb = FachadaAverbacoes.ObtemAverbacao(ID);
                txtNumeroAverbacao.Text = averb.Numero;
                hfAverbacao.Value = ID.ToString();
                hfCompra.Value = dado.IDAverbacaoTipo != (int)Enums.AverbacaoTipo.Normal ? "S" : "N";
                txtData.Value = averb.Data.ToString("dd/MM/yyyy");
            }
//            Averbacao averb = FachadaAverbacoes.ObtemAverbacao(idaverb);

            //if (averb.IDAverbacaoTipo != (int)Enums.AverbacaoTipo.Normal)
            //{
            
                //GridViewAverbacoesVinc.DataSource = averb.AverbacaoVinculo1.Select(x => x.Averbacao);
                //GridViewAverbacoesVinc.DataBind();

                //Table t = new Table();

                //TableRow r = new TableRow();

                //TableCell c = new TableCell();
                //c.Text = "TESTETESTESTESTESTE";
                //r.Controls.Add(c);

                //t.Controls.Add(r);

                //DivAverbacoes.Controls.Add(t);

            //}

            //PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
            return ID;
        }

        private Averbacao PopulaObjeto(Averbacao averb)
        {
            int idsituacao = (int)Enums.AverbacaoSituacao.Reservado;

            if (IdAverbacao > 0)
            {
                Averbacao averbacao = FachadaAverbacoes.ObtemAverbacao(IdAverbacao);
                averb.Numero = averbacao.Numero;
            }

            averb.IDFuncionario = IdFunc;
            averb.IDProduto = Convert.ToInt32(cmbProduto.SelectedValue);
            averb.IDConsignataria = Sessao.IdBanco;
            if (Sessao.IdAgente > 0)
                averb.IDAgente = Sessao.IdAgente;
            averb.IDUsuario = Sessao.UsuarioLogado.IDUsuario;
            averb.Data = DateTime.Today.Date;
            averb.Identificador = txtIdentificador.Text;
            averb.IDAverbacaoSituacao = idsituacao;
            averb.Obs = txtObs.Text;
            averb.Prazo = string.IsNullOrEmpty(txtPrazo.Text) ? 0 : Convert.ToInt32(txtPrazo.Text);
            averb.CompetenciaInicial = Utilidades.ConverteAnoMes(txtMesInicio.Text);
            averb.CompetenciaFinal = Utilidades.CalculaCompetenciaFinal(Utilidades.ConverteAnoMes(txtMesInicio.Text), averb.Prazo.HasValue ? averb.Prazo.Value : 0);
            averb.ValorParcela = string.IsNullOrEmpty(txtValorParcela.Text) ? 0 : Convert.ToDecimal(txtValorParcela.Text.Replace("R$ ", ""));
            averb.ValorContratado = string.IsNullOrEmpty(txtValorContrato.Text) ? 0 : Convert.ToDecimal(txtValorContrato.Text.Replace("R$ ", ""));
            averb.ValorDevidoTotal = string.IsNullOrEmpty(txtValorConsignado.Text) ? 0 : Convert.ToDecimal(txtValorConsignado.Text.Replace("R$ ", ""));
            averb.ValorRefinanciado = 0;
            if (averb.IDAverbacaoTipo != (int)Enums.AverbacaoTipo.Normal)
            {
                averb.ValorDeducaoMargem = averb.ValorParcela - (CalculaMargemRefinancia() + CalculaMargemCompra());
                //if (averb.ValorDeducaoMargem < 0)
                //    averb.ValorDeducaoMargem = 0;
            }
            else
                averb.ValorDeducaoMargem = averb.ValorParcela;
            averb.Coeficiente = Convert.ToDecimal(txtCoeficiente.Text.Replace(".",","));
            if (!string.IsNullOrEmpty(txtCET.Text))
                averb.CET = Convert.ToDecimal(txtCET.Text);
            averb.Obs = txtObs.Text;

            return averb;
        }

        private Pessoa PopulaObjetoPessoa(Pessoa pessoa)
        {
            pessoa.Endereco = txtEndereco.Value;
            pessoa.Bairro = txtBairro.Value;
            pessoa.Cidade = txtCidade.Value;
            pessoa.Estado = cmbEstado.SelectedValue;
            pessoa.CEP = txtCep.Value;
            pessoa.Fone = txtTelefone.Value;
            pessoa.Celular = txtCelular.Value;
            pessoa.Email = txtEmail.Value;
            //pessoa.DataNascimento = string.IsNullOrEmpty(dfDataNasc.Text) ? pessoa.DataNascimento : Convert.ToDateTime(dfDataNasc.Text);

            return pessoa;
        }

        private void LimpaCampos()
        {
            dfMatriculaCPF.Text = string.Empty;
            IdFunc = 0;
            txtMesInicio.Text = Utilidades.ConverteMesAno(FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco));
            txtMesInicio.Enabled = FachadaPermissoesAcesso.CheckPermissao(this.IdRecurso, Sessao.IdBanco, Sessao.IdPerfil, (int)Enums.Permissao.AlterarPrimeiroMesDesconto);
            txtMesFim.Text = string.Empty;
            txtPrazo.Text = "0";
            txtValorParcela.Text = string.Empty;
            txtValorConsignado.Text = "0";
            txtValorContrato.Text = "0";
            txtIdentificador.Text = string.Empty;
            txtCoeficiente.Text = "0";
            txtCET.Text = "0";

            txtNome.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtRG.Text = string.Empty;
            txtDataNasc.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtRegime.Text = string.Empty;
            txtSituacaoFunc.Text = string.Empty;
            txtDataAdm.Text = string.Empty;

            cbRefinancia.Checked = false;
            cbCompra.Checked = false;
            PopulaCombos();
        }

        private bool ValidaMargem()
        {
            if (ValidaAutorizacaoEspecial((int)Enums.FuncionarioAutorizacaoTipo.IndependentedeMargem))
                return true;

            decimal MargemDispFunc = CalcularMargemDisponivel();
            return !(Convert.ToDecimal(txtValorParcela.Text.Replace("R$ ", "")) > MargemDispFunc);
        }

        private bool ValidaFuncSituacao()
        {
            if (ValidaAutorizacaoEspecial((int)Enums.FuncionarioAutorizacaoTipo.IndependentedeSituacao))
                return true;

            if (IdFunc > 0)
            {
                Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);
                return (func != null && func.IDFuncionarioSituacao == (int)Enums.FuncionarioSituacao.AtivoNaFolha);
            }
            else
                return false;
        }

        private bool ValidaFuncBloqueio()
        {
            if (ValidaAutorizacaoEspecial((int)Enums.FuncionarioAutorizacaoTipo.IndependentedeBloqueio))
                return true;

            Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);

            bool bloqueado = false;
            bloqueado = func.FuncionarioBloqueio.Any(x => x.Ativo == 1 && x.TipoBloqueio == "0");

            if (!bloqueado) // servico
                bloqueado = func.FuncionarioBloqueio.Any(x => x.Ativo == 1 && x.TipoBloqueio == "1" && x.Chaves == cmbProduto.SelectedValue);

            if (!bloqueado) // empresa
                bloqueado = func.FuncionarioBloqueio.Any(x => x.Ativo == 1 && x.TipoBloqueio == "2" && x.Chaves == Sessao.IdBanco.ToString());

            return !bloqueado;
        }

        private bool FuncPossuiAutorizacaoEspecial()
        {
            bool autorizacao = false;
            if (IdFunc > 0)
            {
                Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);

                if (func != null && func.FuncionarioAutorizacao.Count() > 0)
                    autorizacao = func.FuncionarioAutorizacao.Any(x => x.IDFuncionarioAutorizacaoTipo == (int)Enums.FuncionarioAutorizacaoTipo.Independentedequalquerrestricao && x.AutorizacaoData.AddDays(x.AutorizacaoValidade) >= DateTime.Today);
            }
            return autorizacao;
        }

        private bool ValidaAutorizacaoEspecial(int idautorizacaotipo)
        {
            bool autorizacao = false;
            if (IdFunc > 0)
            {
                Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);

                if (func != null && func.FuncionarioAutorizacao.Count() > 0)
                    autorizacao = func.FuncionarioAutorizacao.Any(x => x.IDFuncionarioAutorizacaoTipo == idautorizacaotipo && x.AutorizacaoData.AddDays(x.AutorizacaoValidade) >= DateTime.Today);
            }
            return autorizacao;
        }

        private bool ValidaSuspensaoEmpresa(int idaverbacaotipo)
        {
            int situacao = 0;
            situacao = FachadaAverbacoes.ObtemEmpresa(Sessao.IdBanco).IDEmpresaSituacao;

            bool naopermitido = (idaverbacaotipo == (int)Enums.AverbacaoTipo.Normal && (situacao == (int)Enums.EmpresaSituacao.SuspensoAverbacoes || situacao == (int)Enums.EmpresaSituacao.Bloqueado));

            if (!naopermitido)
                naopermitido = ((idaverbacaotipo != (int)Enums.AverbacaoTipo.Normal && idaverbacaotipo != (int)Enums.AverbacaoTipo.Renegociacao) && (situacao == (int)Enums.EmpresaSituacao.SuspensoCompra || situacao == (int)Enums.EmpresaSituacao.Bloqueado));

            return !naopermitido;
        }

        private bool ValidaMesInicio(Averbacao averb)
        {
            bool valida = true;

            string mesinicio = Utilidades.ConverteMesAno(averb.CompetenciaInicial);
            if (string.IsNullOrEmpty(mesinicio))
                valida = false;

            if (valida && Convert.ToInt32(mesinicio.Substring(0, 2)) > 12)
                valida = false;

            if (valida && Convert.ToInt32(mesinicio.Substring(3, 4)) > 2200)
                valida = false;
            if (valida && Convert.ToInt32(mesinicio.Substring(3, 4)) <= 2010)
                valida = false;

            if (valida && Utilidades.ConverteAnoMes(mesinicio).CompareTo(FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco)) < 0)
                valida = false;

            if (!valida)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemMesInicioInvalido);
            }
            else if (Utilidades.ConverteAnoMes(mesinicio).CompareTo(FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco)) != 0)
            {
                int idpermissao = (int)Enums.Permissao.AlterarPrimeiroMesDesconto;
                valida = FachadaPermissoesAcesso.CheckPermissao(this.IdRecurso, Sessao.IdBanco, Sessao.IdPerfil, idpermissao);

                if (!valida)
                {
                    PageMaster.ExibeMensagem("Usuário não possui permissão para alterar o primeiro mês de desconto da averbação!");
                };
            }

            return valida;
        }

        private bool ValidaValorConsignado(Averbacao averb)
        {
            bool valida = true;

            valida = !(averb.ValorContratado > averb.ValorDevidoTotal);
            if (!valida)
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemValorConsignadoMaiorValorContrato);

            return valida;
        }

        private bool ValidaDados(Averbacao averb)
        {
            bool valida = ValidaMesInicio(averb);

            if (valida)
                valida = ValidaValorConsignado(averb);

            if (valida)
                valida = ValidaPrazo(averb);

            if (valida)
                valida = ValidaValorParcela(averb);

            if (valida)
                valida = ValidaAverbacaoTipo(averb.IDAverbacaoTipo);

            if (valida)
                valida = ValidaAverbacaoDuplicada(averb);
            
            return valida;
        }

        private bool ValidaAverbacaoTipo(int idaverbacaotipo)
        {
            int idpermissao = 0;

            if (idaverbacaotipo == (int)Enums.AverbacaoTipo.Normal)
                idpermissao = (int)Enums.Permissao.AverbacaoSimples;
            else if (idaverbacaotipo == (int)Enums.AverbacaoTipo.Renegociacao)
                idpermissao = (int)Enums.Permissao.Refinancimentos;
            else if (idaverbacaotipo == (int)Enums.AverbacaoTipo.Compra)
                idpermissao = (int)Enums.Permissao.Compra;
            else if (idaverbacaotipo == (int)Enums.AverbacaoTipo.CompraERenegociacao)
                idpermissao = (int)Enums.Permissao.CompraComRenegociacao;
 
            bool valida = FachadaPermissoesAcesso.CheckPermissao(this.IdRecurso, Sessao.IdBanco, Sessao.IdPerfil, idpermissao);

            if (!valida)
            {
                PageMaster.ExibeMensagem("Usuário não possui permissão para este tipo de averbação! --> "+FachadaMaster.ObtemPermissao(idpermissao).Nome);
                return valida;
            };

            return valida;
        }

        private bool ValidaAverbacaoDuplicada(Averbacao averb)
        {
            bool valida = true;

            valida = !FachadaAverbacoes.VerificaExistePrevenirDuplicacao(averb);

            if (!valida)
                PageMaster.ExibeMensagem("Esta Averbação já foi salva com esses mesmos dados hoje!");

            return valida;
        }

        private bool ValidaPrazo(Averbacao averb)
        {
            bool valida = true;

            if (!string.IsNullOrEmpty(hfPrazoMaximo.Value) && averb.Prazo > Convert.ToInt32(hfPrazoMaximo.Value))
                valida = false;

            if (!valida)
                PageMaster.ExibeMensagem("Prazo limite foi excedido!");

            return valida;
        }

        private bool ValidaValorParcela(Averbacao averb)
        {
            bool valida = true;

            if (averb.ValorParcela <= 0)
                valida = false;

            if (!valida && ControleCarregado)
                PageMaster.ExibeMensagem("Valor parcela não foi informado!");

            return valida;
        }
        private bool ValidaInformacoes(Averbacao averb)
        {
            if (!ValidaDados(averb))
            {
                return false;
            }

            if (FuncPossuiAutorizacaoEspecial())
            {
                return true;
            }
            else
            {
                if (!ValidaSuspensaoEmpresa(averb.IDAverbacaoTipo))
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemEmpresaSuspensa);
                    return false;
                }
                else if (!ValidaFuncSituacao())
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncSituacaoInvalida);
                    return false;
                }
                else if (!ValidaFuncBloqueio())
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncBloqueado);
                    return false;
                }
                else if (!ValidaMargem())
                {
                    //Salvar.Attributes.Remove("Validacao");
                    //Salvar.Attributes.Add("Validacao","Nao");
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemMargemInsuficiente);
                    return false;
                }
            }
            //Salvar.Attributes.Remove("Validacao");
            //Salvar.Attributes.Add("Validacao", "Sim");
            return true;

        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();

            PageMaster.SubTitulo = ResourceMensagens.TituloNovo;
        }

        protected void dfMatriculaCPF_TextChanged(object sender, EventArgs e)
        {
            IQueryable<Funcionario> funcs = FachadaAverbacoes.LocalizarFuncPorMatriculaOuCpf(dfMatriculaCPF.Text);
            int qtdefuncs = funcs.Count();
            if (qtdefuncs != 0)
            {
                IdFunc = funcs.First().IDFuncionario;

                if (!ValidaFuncSituacao())
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncSituacaoInvalida);
                    return;
                }
                else if (!ValidaFuncBloqueio())
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncBloqueado);
                    return;
                }

                PopulaDadosFunc();
                if (cmbProduto.SelectedValue != "0" && !string.IsNullOrEmpty(cmbProduto.SelectedValue))
                {
                    var produto = FachadaAverbacoes.ObtemProduto(Convert.ToInt32(cmbProduto.SelectedValue));
                    if (produto != null)
                    {
                        IdProdutoGrupo = produto.IDProdutoGrupo;
                        DadosAverbacao(null, IdProdutoGrupo);
                    }
                }
            }
            else
            {
                panelDadosFunc.Visible = false;
                divEscolhaCompra.Visible = false;
                DivDadosAverbacao.Visible = false;
                DivSalvar.Visible = false;
                upEscolhaCompra.Update();
                upDadosAverbacao.Update();
                upSalvar.Update();

                if (!string.IsNullOrEmpty(dfMatriculaCPF.Text))
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncNaoExiste);
                }
            }
        }

        private void PopulaDadosFunc()
        {
            Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);

            // Dados Pessoais
            dfMatriculaCPF.Text = func.Matricula;
            txtMatricula.Text = func.Matricula;
            
            // Dados Pessoais
            txtNome.Text = func.Pessoa.Nome;
            txtCPF.Text = func.Pessoa.CPFMascara;
            txtRG.Text = func.Pessoa.RG;
            txtDataNasc.Text = func.Pessoa.DataNascimento == null ? "---" : func.Pessoa.DataNascimento.Value.ToString("dd/MM/yyyy");
            txtMatricula.Text = func.Matricula;
            txtCategoria.Text = func.FuncionarioCategoria.Nome;
            txtRegime.Text = func.NomeRegimeFolha;
            txtSituacaoFunc.Text = func.NomeSituacao;
            txtDataAdm.Text = func.DataAdmissao != null ? func.DataAdmissao.Value.ToString("dd/MM/yyyy") : "";
            txtEndereco.Value = func.Pessoa.Endereco;
            dfEndereco.Text = txtEndereco.Value;
            txtBairro.Value = func.Pessoa.Bairro;
            txtCidade.Value = func.Pessoa.Cidade;
            txtEstado.Value = func.Pessoa.Estado;
            txtCep.Value = func.Pessoa.CEP;
            txtEmail.Value = func.Pessoa.Email;
            txtTelefone.Value = func.Pessoa.Fone;
            txtCelular.Value = func.Pessoa.Celular;
            hfSenhaFunc.Value = string.IsNullOrEmpty(func.Pessoa.Usuario.SenhaProvisoria) ? func.Pessoa.Usuario.Senha : func.Pessoa.Usuario.SenhaProvisoria;
            txtBanco.Value = FachadaAverbacoes.ObtemEmpresa(Sessao.IdBanco).Nome;
            txtCNPJ.Value = FachadaAverbacoes.ObtemEmpresa(Sessao.IdBanco).CNPJ;
            txtLocal.Value = func.NomeLocalFolha;
            txtSetor.Value = func.NomeSetorFolha;
            txtCargo.Value = func.NomeCargoFolha;
            hfValidaMargem.Value = ValidaAutorizacaoEspecial((int)Enums.FuncionarioAutorizacaoTipo.IndependentedeMargem) ? "S" : "N";

            if (IdAverbacao == 0 && func.Pessoa.Funcionario.Count() > 1)
            {
                GridViewListaFunc.DataSource = func.Pessoa.Funcionario.OrderBy(x => x.Matricula);
                GridViewListaFunc.DataBind();
                GridViewListaFunc.Visible = true;
            }

            ChecksRenegociacaoCompra.Visible = true;
            panelDadosFunc.Visible = (func.IDFuncionarioSituacao != (int)Enums.FuncionarioSituacao.RetiradoDaFolha);

            if (!panelDadosFunc.Visible)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFuncSituacaoInvalida);
            }

            if ((panelDadosFunc.Visible && divEscolhaCompra.Visible) || cmbProduto.SelectedValue != "0")
            {
                var produtogrupo = FachadaAverbacoes.ObtemProdutoGrupoDeProduto(cmbProduto.SelectedValue);
                if (produtogrupo.HasValue)
                    DadosAverbacao(null, produtogrupo.Value);
            }
        }

        private void DadosAverbacao(Averbacao averb, int idprodutogrupo)
        {
            if (IdFunc == 0)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecioneFuncionario);
                return;
            }

            IdProdutoGrupo = idprodutogrupo;
            if (averb != null)
            {
                PopulaDadosAverbacao(averb);

                PopulaGridRefinanciar();
                PopulaGridComprar();
            }

            // pegar prazo maximo
            hfPrazoMaximo.Value = FachadaAverbacoes.ObtemPrazoMaximo(Convert.ToInt32(cmbProduto.SelectedValue), IdProdutoGrupo);

            if (!FachadaAverbacoes.RequerAprovacao((int)Enums.Modulos.Funcionario, idprodutogrupo, 0))
                hfAprovaFunc.Value = "N";
            else
                hfAprovaFunc.Value = "S";

            Funcionario func = FachadaAverbacoes.ObtemFuncionario(IdFunc);
            MargemDispFunc = func.MargemDisponivelReal(IdProdutoGrupo);

            txtMargemDisp.Text = String.Format("{0:N}", MargemDispFunc);
            txtMargemDisponivel.Text = String.Format("{0:N}", MargemDispFunc);

            dfMatriculaCPF.Enabled = false;
            cmbProduto.Enabled = false;
            btLimpar.Visible = true;
            upInicio.Update();

            upMargem.Update();
            upDadosFunc.Update();

            if (IdAverbacao == 0)
            {               
                divEscolhaCompra.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
                int qtdeproprio = func.Averbacao.Where(x => x.IDConsignataria == Sessao.IdBanco && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos && x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && !x.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao)).Count();
                int qtdeterceiro = func.Averbacao.Where(x => x.IDConsignataria != Sessao.IdBanco && x.Produto.IDProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos && x.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.Ativo && !x.AverbacaoVinculo.Any(y => y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.EmProcessodeCompra || y.Averbacao1.IDAverbacaoSituacao == (int)Enums.AverbacaoSituacao.AguardandoAprovacao)).Count();
                divEscolhaCompra.Visible = divEscolhaCompra.Visible && (qtdeproprio > 0 || qtdeterceiro > 0);
                if (divEscolhaCompra.Visible)
                {
                    txtQtdeAverbacoesProprio.Text = qtdeproprio.ToString() + " averbações com " + FachadaAverbacoes.ObtemEmpresa(Sessao.IdBanco).Nome;
                    txtQtdeAverbacoesTerceiros.Text = qtdeterceiro.ToString() + " averbações com outras consignatárias";

                    cbRefinancia.Checked = false;
                    cbCompra.Checked = false;

                    cbRefinancia.Enabled = qtdeproprio > 0;
                    cbCompra.Enabled = qtdeterceiro > 0;

                    upEscolhaCompra.Update();
                }
            }

            DivDadosAverbacao.Visible = true;
            DivEmprestimo.Visible = true; // (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaPrazo.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaValorContrato.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaValorConsig.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaUltDesconto.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaCET.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            linhaFator.Visible = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos);
            PrimMesDesconto.Text = (IdProdutoGrupo == (int)Enums.ProdutoGrupo.Emprestimos) ? "1º Desconto Folha:" : "Mês/Ano Desconto:";

            DivSalvar.Visible = true;
            upDadosAverbacao.Update();
            upSalvar.Update();
        }

        private void PopulaDadosAverbacao(Averbacao averb)
        {

            dfMatriculaCPF.Text = averb.Funcionario.Matricula;
            cmbProduto.SelectedValue = averb.IDProduto.ToString();
            txtIdentificador.Text = averb.Identificador;
            txtObs.Text = averb.Obs;

            string mescorte = FachadaAverbacoes.ObtemAnoMesCorte(Sessao.IdModulo, Sessao.IdBanco);
            if (averb.CompetenciaInicial.CompareTo(mescorte) < 0)
            {
                txtMesInicio.Text = Utilidades.ConverteMesAno(mescorte);
                txtMesFim.Text = Utilidades.ConverteMesAno(Utilidades.CalculaCompetenciaFinal(mescorte, averb.Prazo.Value));
            }
            else
            {
                txtMesInicio.Text = Utilidades.ConverteMesAno(averb.CompetenciaInicial);
                txtMesFim.Text = Utilidades.ConverteMesAno(averb.CompetenciaFinal);
            }
            txtPrazo.Text = averb.Prazo.ToString();
            txtValorParcela.Text = String.Format("{0:N}",averb.ValorParcela);
            txtValorContrato.Text = String.Format("{0:N}", averb.ValorContratado);
            txtValorConsignado.Text = String.Format("{0:N}", averb.ValorDevidoTotal);

            txtCoeficiente.Text = averb.ValorContratado.Equals(0) ? "---" : (averb.ValorParcela / averb.ValorContratado).ToString();

            txtCET.Text = averb.CET.ToString();
            txtObs.Text = averb.Obs;
            hfNumero.Value = averb.Numero;
        }

        protected void cbCompra_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCompra.Checked && !ValidaAverbacaoTipo((int)Enums.AverbacaoTipo.Compra))
            {
                cbCompra.Checked = false;
                return;
            }

            panelCompra.Visible = cbCompra.Checked;

            if (IdFunc > 0 && panelCompra.Visible)
            {
                PopulaGridComprar();
            }
            else
            {
                gridComprar.Selection.UnselectAll();
            }
        }

        private void PopulaGridComprar()
        {
            panelCompra.Visible = true;
            if (IdAverbacao > 0)
            {
                var dados = FachadaAverbacoes.AverbacoesCompradas(IdAverbacao);
                if (dados.Count() == 0)
                    panelCompra.Visible = false;

                gridComprar.DataSource = dados;
            }
            else
            {
                Funcionario func = new Repositorio<Funcionario>().ObterPorId(IdFunc);
                gridComprar.DataSource = FachadaAverbacoes.AverbacoesParaComprar(func.Averbacao.AsQueryable(), Sessao.IdBanco);
            }
            gridComprar.DataBind();

            if (IdAverbacao > 0)
            {
                gridComprar.Selection.SelectAll();
                gridComprar.Enabled = false;
            }
        }

        protected void cbRefinancia_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRefinancia.Checked && !ValidaAverbacaoTipo((int)Enums.AverbacaoTipo.Renegociacao))
            {
                cbRefinancia.Checked = false;
                return;
            }

            panelRefinancia.Visible = cbRefinancia.Checked;

            if (IdFunc > 0 && panelRefinancia.Visible)
            {
                PopulaGridRefinanciar();
            }
            else
            {
                gridRefinanciar.Selection.UnselectAll();
            }
        }

        private void PopulaGridRefinanciar()
        {
            panelRefinancia.Visible = true;
            if (IdFunc > 0)
            {
                if (IdAverbacao > 0)
                {
                    var dados = FachadaAverbacoes.AverbacoesRefinanciadas(IdAverbacao);
                    if (dados.Count() == 0)
                        panelRefinancia.Visible = false;

                    gridRefinanciar.DataSource = dados;
                }
                else
                {
                    Funcionario func = new Repositorio<Funcionario>().ObterPorId(IdFunc);
                    gridRefinanciar.DataSource = FachadaAverbacoes.AverbacoesParaRefinanciar(func.Averbacao.AsQueryable(), Sessao.IdBanco);
                }
                gridRefinanciar.DataBind();
                if (IdAverbacao > 0)
                {
                    gridRefinanciar.Selection.SelectAll();
                    gridRefinanciar.Enabled = false;
                }
            }
        }

        protected void gridRefinanciar_SelectionChanged(object sender, EventArgs e)
        {
            CalcularMargemDisponivel();
        }

        protected void gridComprar_SelectionChanged(object sender, EventArgs e)
        {
            CalcularMargemDisponivel();
        }


        private decimal CalculaMargemRefinancia()
        {
            List<object> listaRefinancia = gridRefinanciar.GetSelectedFieldValues("ValorParcela");

            decimal somarefinancia = 0;
            if (listaRefinancia.Count() > 0)
            {
                if (IdAverbacao == 0)
                    somarefinancia = listaRefinancia.Sum(x => Convert.ToDecimal(x));
                else
                {
                    listaRefinancia = gridRefinanciar.GetSelectedFieldValues("IDAverbacao");

                    somarefinancia = FachadaAverbacoes.CalculaRefinanciaQueDeduzMargem(listaRefinancia.Select(x => Convert.ToInt32(x)).ToList());
                }
            }

            return somarefinancia;
        }


        private decimal CalculaMargemCompra()
        {
            List<object> listaCompra = gridComprar.GetSelectedFieldValues("ValorParcela");

            decimal somarcompra = 0;
            if (listaCompra.Count() > 0)
            {
                if (IdAverbacao == 0)
                    somarcompra = listaCompra.Sum(x => Convert.ToDecimal(x));
                else
                {
                    listaCompra = gridComprar.GetSelectedFieldValues("IDAverbacao");

                    somarcompra = FachadaAverbacoes.CalculaCompraQueDeduzMargem(listaCompra.Select(x => Convert.ToInt32(x)).ToList());
                }
            }
            return somarcompra;
        }


        private decimal CalcularMargemDisponivel()
        {
            decimal margemdisp = MargemDispFunc;

            decimal margemrefinancia = CalculaMargemRefinancia();

            decimal margemcompra = CalculaMargemCompra();

            decimal valordeducaomargem = 0;

            if (IdAverbacao > 0)
                valordeducaomargem = FachadaAverbacoes.CalculaValorDeducaoMargem(IdAverbacao);

            //if ((margemdisp + margemcompra) < 0) 
            //    txtMargemDisp.Text = String.Format("{0:N}",0);
            //else
            decimal margem = margemdisp + margemcompra + margemrefinancia + valordeducaomargem;

            txtMargemDisp.Text = String.Format("{0:N}",margem);
            // Gerando tabela com as averbações vinculadas
            //AdicionaVinculoTermo();          
            upMargem.Update();

            return margem;
        }

        //private void AdicionaVinculoTermo()
        //{
        //    List<object> listaRefinancia = gridRefinanciar.GetSelectedFieldValues("IDAverbacao");
        //    List<object> listaCompra = gridComprar.GetSelectedFieldValues("IDAverbacao");

        //    var refinancia = listaRefinancia.Select(x => Convert.ToInt32(x)).ToList();
        //    var compra = listaCompra.Select(x => Convert.ToInt32(x)).ToList();

        //    var dados_refinancia = FachadaAverbacoes.o


        //        var dados = averb.AverbacaoVinculo1.ToList(); // .Select(x => x.Averbacao). id


        //        HtmlTable tabela = new HtmlTable();

        //        foreach (var item in dados)
        //        {

        //            HtmlTableRow linha = new HtmlTableRow();

        //            HtmlTableCell coluna1 = new HtmlTableCell();
        //            coluna1.InnerHtml = "xxxxxxxxx";
        //            linha.Cells.Add(coluna1);

        //            HtmlTableCell coluna2 = new HtmlTableCell();
        //            coluna2.InnerHtml = "wwwwwwww";
        //            linha.Cells.Add(coluna2);

        //            tabela.Rows.Add(linha);
        //        }

        //        DivAverbacoes.Controls.Add(tabela);
        //    //}
        //}

        protected void GridViewListaFuncSelect_Click(object sender, EventArgs e)
        {
//            int id = Convert.ToInt32(GridViewListaFunc.SelectedDataKey.Value);
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            IdFunc = id;
            PopulaDadosFunc();
            //PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncionariosConsulta, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 1, id);
        }
    }

}