<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sudameris.aspx.cs" Inherits="Bancos_Sudameris" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
            <cc1:BoletoBancario ID="boletoBancario" runat="server" CodigoBanco="347">
            </cc1:BoletoBancario>
</asp:Content>

