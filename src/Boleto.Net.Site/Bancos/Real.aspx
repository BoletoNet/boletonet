<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Real.aspx.cs" Inherits="Bancos_Real" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
        <cc1:BoletoBancario ID="real" runat="server" CodigoBanco="356">
        </cc1:BoletoBancario>
</asp:Content>

