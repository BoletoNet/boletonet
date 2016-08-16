<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Banestes.cs" Inherits="Bancos_Banestes" Title="Banestes" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc1:BoletoBancario id="boletoBancario" runat="server" CodigoBanco="021"></cc1:BoletoBancario>
</asp:Content>

