<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Caixa.aspx.cs" Inherits="Bancos_Caixa" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <cc1:BoletoBancario id="boletoBancario" runat="server" CodigoBanco="104"></cc1:BoletoBancario>
        <asp:Label ID="lblCodigoBarras" runat="server"></asp:Label>
</asp:Content>

