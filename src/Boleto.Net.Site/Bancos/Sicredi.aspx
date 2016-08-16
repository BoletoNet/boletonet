<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sicredi.aspx.cs" Inherits="Bancos_Sicredi" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc1:BoletoBancario id="boletoBancario" runat="server" CodigoBanco="748" MostrarComprovanteEntrega="true"></cc1:BoletoBancario>
</asp:Content>

