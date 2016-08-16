<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Safra.aspx.cs" Inherits="Bancos_Safra" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<cc1:BoletoBancario id="boletoBancario" runat="server" CodigoBanco="422"></cc1:BoletoBancario>
</asp:Content>

