<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Unicredi.aspx.cs" Inherits="Bancos_Unicred" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc1:BoletoBancario id="boletoBancario" runat="server" CodigoBanco="136" MostrarComprovanteEntrega="true"></cc1:BoletoBancario>
</asp:Content>

