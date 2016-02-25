<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Formulario.aspx.cs" Inherits="Formulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="panelDados" runat="server">
        <h3>
            Dados do Boleto</h3>
        <table border="0">
            <tr>
                <td>
                    Banco:
                </td>
                <td>
                    <asp:DropDownList ID="txtBanco" runat="Server">
                        <asp:ListItem Text="Banco do Brasil" Value="001" />
                        <asp:ListItem Text="Banco Santander" Value="033" />
                        <asp:ListItem Text="Banco BRB" Value="070" />
                        <asp:ListItem Text="Caixa Econômica Federal" Value="104" />
                        <asp:ListItem Text="Banco Bradesco" Value="237" />
                        <asp:ListItem Text="Banco Real-275" Value="275" />
                        <asp:ListItem Text="Banco BCN" Value="291" />
                        <asp:ListItem Text="Banco Itaú" Value="341" />
                        <asp:ListItem Text="Banco Sudameris" Value="347" />
                        <asp:ListItem Text="Banco Real-356" Value="356" />
                        <asp:ListItem Text="Banco Unibanco" Value="409" />
                        <asp:ListItem Text="Banco Safra" Value="422" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Vencimento:
                </td>
                <td>
                    <asp:TextBox ID="txtVencimento" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Valor:
                </td>
                <td>
                    <asp:TextBox ID="txtValorBoleto" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Valor Cobrado:
                </td>
                <td>
                    <asp:TextBox ID="txtValorCobrado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nosso Número:
                </td>
                <td>
                    <asp:TextBox ID="txtNossoNumeroBoleto" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Número Documento:
                </td>
                <td>
                    <asp:TextBox ID="txtNumeroDocumentoBoleto" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Carteira:
                </td>
                <td>
                    <asp:TextBox ID="txtCarteira" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h3>
            Dados do Cedente</h3>
        <table border="0">
            <tr>
                <td>
                    CPF/CNPJ:
                </td>
                <td>
                    <asp:TextBox ID="txtCPFCNPJ" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Código:
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoCedente" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nome:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeCedente" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Agência:
                </td>
                <td>
                    <asp:TextBox ID="txtAgenciaCedente" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Conta:
                </td>
                <td>
                    <asp:TextBox ID="txtContaCedente" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Instruções:
                </td>
                <td>
                    <asp:TextBox ID="txtInstrucoes" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h3>
            Dados do Sacado</h3>
        <table border="0">
            <tr>
                <td>
                    CPF/CNPJ:
                </td>
                <td>
                    <asp:TextBox ID="txtCPFCNPJSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Nome:
                </td>
                <td>
                    <asp:TextBox ID="txtNomeSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Endereço:
                </td>
                <td>
                    <asp:TextBox ID="txtEnderecoSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Bairro:
                </td>
                <td>
                    <asp:TextBox ID="txtBairroSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Cidade:
                </td>
                <td>
                    <asp:TextBox ID="txtCidadeSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    CEP:
                </td>
                <td>
                    <asp:TextBox ID="txtCEPSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    UF:
                </td>
                <td>
                    <asp:TextBox ID="txtUFSacado" runat="server"></asp:TextBox>
                </td>
            </tr>
            
        </table>
    </asp:Panel>
    <asp:Panel ID="panelBoleto" runat="server">
    </asp:Panel>
    <asp:Button ID="Button1" runat="server" Text=" Imprimir " OnClick="Button1_Click" />
    </form>
</asp:Content>
