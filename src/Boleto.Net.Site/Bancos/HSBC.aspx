<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HSBC.aspx.cs" Inherits="Bancos_HSBC" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="boletonet" %>
<boletonet:BoletoBancario ID="boletoBancario" runat="server" CodigoBanco="399">
        </boletonet:BoletoBancario>        
        <p>
* Código referente ao HSBC cedido e testado por Leonardo Cooper (leonardo@ecod.com.br)
</p>
</asp:Content>

