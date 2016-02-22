<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EnvioEmail.aspx.cs" Inherits="EnvioEmail" Title="Untitled Page" %>

<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="boletonet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <p>
        Para o envio do boleto por email é necessário configurar &lt;system.net&gt; no web.config.
    </p>
    <div class="boxinfo">
        <span class="info">&lt;system.net&gt;<br />
            &nbsp;&nbsp;&lt;mailSettings&gt;<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&lt;smtp from=&quot;mymail@mymail.com&quot;&gt;<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;network host=&quot;smtp.myhost.com&quot;
            port=&quot;25&quot; defaultCredentials=&quot;true&quot; /&gt;<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&lt;/smtp&gt;<br />
            &nbsp;&nbsp;&lt;/mailSettings&gt;<br />
            &lt;/system.net&gt; </span>
    </div>
    <p>
        Se você enviar um boleto &quot;On-Line&quot;, só é possível abri-lo quando o seu site 
        estiver no ar. Assim sendo, é recomendado que se envie apenas boletos Off-Line, 
        que sempre irão abrir. Por outro lado, o email do Boleto Off-Line é um pouco 
        maior.</p>
        <p>
        Você pode enviar apenas um boleto no email ou enviar mais de um boleto no mesmo email. 
        A segunda opção é interessante quando o cliente comprou algo a prazo.
        
        </p>
    <p>
        <asp:RadioButton ID="RadioButton1" runat="server" GroupName="tipo" Text="On-Line" /><asp:RadioButton ID="RadioButton2" GroupName="tipo"
            runat="server" Text="Off-Line" Checked="True" />
        <br />
    <br />
        Email para envio:<br />
        <asp:TextBox ID="TextBox1" runat="server" Width="300px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Enviar apenas um boleto" OnClick="Button1_Click" /> 
        <asp:Button ID="Button2" runat="server" Text="Enviar vários boletos (sempre Off-Line)" OnClick="Button1_Click2" /><br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
    </p>
</asp:Content>
