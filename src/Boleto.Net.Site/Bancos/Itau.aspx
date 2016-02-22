<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Itau.aspx.cs" Inherits="Bancos_Itau" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="boletonet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <boletonet:BoletoBancario ID="boletoBancario" runat="server" CodigoBanco="341" MostrarComprovanteEntrega="true">
        </boletonet:BoletoBancario>
</asp:Content>

