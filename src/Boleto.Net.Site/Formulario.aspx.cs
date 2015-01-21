using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using BoletoNet;

public partial class Formulario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (!string.IsNullOrEmpty(Request["Carteira"]))
            {
                txtCarteira.Text = Request["Carteira"];
            }
            else
            {
                //Carteira de teste do Santander.
                txtCarteira.Text = "18";
            }

            //Response.Write("Banco:" & Request["NumBanco"])
            if (!string.IsNullOrEmpty(Request["NumBanco"]))
            {
                SelectedItemByID(txtBanco, Request["NumBanco"]);
            }
            else
            {
                //txtBanco.SelectedItem.Value = Request["NumBanco"]
                //Banco de teste Santander.
                txtBanco.SelectedItem.Value = "001";
            }

            if (!string.IsNullOrEmpty(Request["Vencimento"]))
            {
                txtVencimento.Text = Request["Vencimento"];
            }
            else
            {
                txtVencimento.Text = new DateTime(2009, 3, 6).ToString("dd/MM/yyyy");
            }

            if (!string.IsNullOrEmpty(Request["ValorBoleto"]))
            {
                txtValorBoleto.Text = Request["ValorBoleto"];
            }
            else
            {
                txtValorBoleto.Text = "3,0";
            }

            if (!string.IsNullOrEmpty(Request["ValorCobrado"]))
            {
                txtValorCobrado.Text = Request["ValorCobrado"];
            }
            else
            {
                txtValorCobrado.Text = "10,0";
            }

            if (!string.IsNullOrEmpty(Request["NumeroDocumentoBoleto"]))
            {
                txtNumeroDocumentoBoleto.Text = Request["NumeroDocumentoBoleto"];
            }
            else
            {
                txtNumeroDocumentoBoleto.Text = "B20005446";
            }


            //Cedente 
            if (!string.IsNullOrEmpty(Request["CodigoCedente"]))
            {
                txtCodigoCedente.Text = Request["CodigoCedente"];
            }
            else
            {
                txtCodigoCedente.Text = "1535547";
            }
            if (!string.IsNullOrEmpty(Request["NossoNumeroBoleto"]))
            {
                txtNossoNumeroBoleto.Text = Request["NossoNumeroBoleto"];
            }
            else
            {
                txtNossoNumeroBoleto.Text = "00000333200710000";
            }
            if (!string.IsNullOrEmpty(Request["CPFCNPJ"]))
            {
                txtCPFCNPJ.Text = Request["CPFCNPJ"];
            }
            else
            {
                txtCPFCNPJ.Text = "59.323.998/0001-08";
            }
            if (!string.IsNullOrEmpty(Request["NomeCedente"]))
            {
                txtNomeCedente.Text = Request["NomeCedente"];
            }
            else
            {
                txtNomeCedente.Text = "Uniabc";
            }
            if (!string.IsNullOrEmpty(Request["AgenciaCedente"]))
            {
                txtAgenciaCedente.Text = Request["AgenciaCedente"];
            }
            else
            {
                txtAgenciaCedente.Text = "1250-5";
            }
            if (!string.IsNullOrEmpty(Request["ContaCedente"]))
            {
                txtContaCedente.Text = Request["ContaCedente"];
            }
            else
            {
                txtContaCedente.Text = "26398-2";
            }
            if (!string.IsNullOrEmpty(Request["Instrucoes"]))
            {
                txtInstrucoes.Text = HttpUtility.HtmlDecode(Request["Instrucoes"]);
            }
            else
            {
                txtInstrucoes.Text = HttpUtility.HtmlDecode("Não receber após o vencimento");
            }


            //Sacado 
            if (!string.IsNullOrEmpty(Request["CPFCNPJSacado"]))
            {
                txtCPFCNPJSacado.Text = Request["CPFCNPJSacado"];
            }
            else
            {
                txtCPFCNPJSacado.Text = "000.000.000-00";
            }
            if (!string.IsNullOrEmpty(Request["NomeSacado"]))
            {
                txtNomeSacado.Text = Request["NomeSacado"];
            }
            else
            {
                txtNomeSacado.Text = "Fulano de Silva";
            }
            if (!string.IsNullOrEmpty(Request["EnderecoSacado"]))
            {
                txtEnderecoSacado.Text = Request["EnderecoSacado"];
            }
            else
            {
                txtEnderecoSacado.Text = "SSS 154 Bloco J Casa 23";
            }
            if (!string.IsNullOrEmpty(Request["BairroSacado"]))
            {
                txtBairroSacado.Text = Request["BairroSacado"];
            }
            else
            {
                txtBairroSacado.Text = "Testando";
            }
            if (!string.IsNullOrEmpty(Request["CidadeSacado"]))
            {
                txtCidadeSacado.Text = Request["CidadeSacado"];
            }
            else
            {
                txtCidadeSacado.Text = "Testelândia";
            }
            if (!string.IsNullOrEmpty(Request["CEPSacado"]))
            {
                txtCEPSacado.Text = Request["CEPSacado"];
            }
            else
            {
                txtCEPSacado.Text = "70000000";
            }
            if (!string.IsNullOrEmpty(Request["UFSacado"]))
            {
                txtUFSacado.Text = Request["UFSacado"];
            }
            else
            {
                txtUFSacado.Text = "DF";
            }



            //alterei so esses
            // Vencimento: 6/3/2009 
            //Valor: 656,40
            //Nosso Número: 0000033320071 
            // Número(Documento) : B20005446()


            //Dados do Cedente
            //CPF/CNPJ: 59.323.998/0001-08 
            //Código: 0806498
            //Nome: Uniabc()
            //Agência: 432 
            //Conta: 0806498

            if (Request["Acao"] == "visualizar")
            {
                Button1_Click(sender, e);

            }
        }
    }

    private void SelectedItemByID(DropDownList oComboBox, string intID)
    {
        int oItem = 0;
        for (oItem = 0; oItem <= oComboBox.Items.Count - 1; oItem++)
            if (oComboBox.Items[oItem].Value == intID.ToString())
                oComboBox.SelectedIndex = oItem;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        try
        {

            //Remove dígito da Agência.
            int DigAgencia = 0;
            int Agencia = 0;
            if (txtAgenciaCedente.Text.IndexOf("-") > -1)
            {
                int s = txtAgenciaCedente.Text.IndexOf("-") + 1;
                int tam = Strings.Len(txtAgenciaCedente.Text);
                DigAgencia = Convert.ToInt32(Strings.Right(txtAgenciaCedente.Text, tam - s));
                int dif = tam - (s - 1);
                //incluindo o traço.
                Agencia = Convert.ToInt32(Strings.Left(txtAgenciaCedente.Text, tam - dif));
            }
            //txtAgenciaCedente.Text

            //Remove dígito da Conta.
            int DigConta = 0;
            int Conta = 0;
            if (txtContaCedente.Text.IndexOf("-") > -1)
            {
                int s2 = txtContaCedente.Text.IndexOf("-") + 1;
                int tam2 = Strings.Len(txtContaCedente.Text);
                DigConta = Convert.ToInt32(Strings.Right(txtContaCedente.Text, tam2 - s2));
                int dif2 = tam2 - (s2 - 1);
                //incluindo o traço.
                Conta = Convert.ToInt32(Strings.Left(txtContaCedente.Text, tam2 - dif2));
            }
            //txtContaCedente.Text

            //Remove dígito do Cedente.
            if (txtCodigoCedente.Text.IndexOf("-") > -1)
            {
                int s3 = txtCodigoCedente.Text.IndexOf("-") + 1;
                int tam3 = Strings.Len(txtCodigoCedente.Text);
                int dif3 = tam3 - (s3 - 1);
                //incluindo o traço.
                txtCodigoCedente.Text = Strings.Left(txtCodigoCedente.Text, tam3 - dif3);
            }


            //Validação.
            switch (txtBanco.Text)
            {
                case "001":
                    //Banco do Brasil.

                    //Carteira com 2 caracteres.
                    //If Len(txtCarteira.Text) <> 2 Then
                    //Response.Write("A Carteira deve conter 2 dígitos."]
                    //Exit Sub
                    //End If

                    //Agência com 4 caracteres.
                    if (Strings.Len(Agencia) > 4)
                    {
                        Response.Write("A Agência deve conter até 4 dígitos.");
                        return;
                    }


                    //Conta com 8 caracteres.
                    if (Strings.Len(Conta) > 8)
                    {
                        Response.Write("A Conta deve conter até 8 dígitos.");
                        return;
                    }


                    //Cedente com 8 caracteres.
                    if (Strings.Len(txtCodigoCedente.Text) > 8)
                    {
                        Response.Write("O Código do Cedente deve conter até 8 dígitos.");
                        return;
                    }


                    //Nosso Número deve ser 11 ou 17 dígitos.
                    if (Strings.Len(txtNossoNumeroBoleto.Text) != 11 & Strings.Len(txtNossoNumeroBoleto.Text) != 17)
                    {
                        Response.Write("O Nosso Número deve ter 11 ou 17 dígitos dependendo da Carteira.");
                        return;
                    }


                    break;
                //Se Carteira 18 então NossoNumero são 17 dígitos.
                //If txtCarteira.Text = "18" Then
                // If Len(txtNossoNumeroBoleto.Text) <> 17 Then
                // Response.Write("O Nosso Número deve ter 17 dígitos para Carteira 18."]
                // Exit Sub
                // End If
                //Else
                // 'Senão, então NossoNumero 11 dígitos.
                // If Len(txtNossoNumeroBoleto.Text) <> 11 Then
                // Response.Write("O Nosso Número deve ter 11 dígitos para Carteira diferente que 18."]
                // Exit Sub
                // End If
                //End If

                case "033":
                    //Santander.
                    break;

                case "070":
                    //Banco BRB.
                    break;

                case "104":
                    //Caixa Econômica Federal.
                    break;

                case "237":
                    //Banco Bradesco.
                    break;

                case "275":
                    //Banco Real.

                    //Cedente 
                    if (!string.IsNullOrEmpty(Request["CodigoCedente"]))
                    {

                    }

                    //Cobrança registrada 7 dígitos.
                    //Cobrança sem registro até 13 dígitos.
                    if (Strings.Len(txtNossoNumeroBoleto.Text) < 7 & Strings.Len(txtNossoNumeroBoleto.Text) < 13)
                    {
                        Response.Write("O Nosso Número deve ser entre 7 e 13 caracteres.");
                        return;
                    }


                    //Carteira
                    if (txtCarteira.Text != "00" & txtCarteira.Text != "20" & txtCarteira.Text != "31" & txtCarteira.Text != "42" & txtCarteira.Text != "47" & txtCarteira.Text != "85" & txtCarteira.Text != "57")
                    {
                        Response.Write("A Carteira deve ser 00,20,31,42,47,57 ou 85.");
                        return;
                    }

                    //00'- Carteira do convênio
                    //20' - Cobrança Simples
                    //31' - Cobrança Câmbio
                    //42' - Cobrança Caucionada
                    //47' - Cobr. Caucionada Crédito Imobiliário
                    //85' - Cobrança Partilhada
                    //57 - última implementação ?

                    //Agência 4 dígitos.
                    if (Strings.Len(Agencia) > 4)
                    {
                        Response.Write("A Agência deve conter até 4 dígitos.");
                        return;
                    }

                    //Número da conta 6 dígitos.
                    if (Strings.Len(Conta) > 6)
                    {
                        Response.Write("A Conta Corrente deve conter até 6 dígitos.");
                        return;
                    }


                    break;
                case "291":
                    //Banco BCN.
                    break;

                case "341":
                    //Banco Itaú.
                    break;

                case "347":
                    //Banco Sudameris.
                    break;

                case "356":
                    //Banco Real.

                    //Cedente 
                    if (!string.IsNullOrEmpty(Request["CodigoCedente"]))
                    {
                    }
                    //?


                    //Cobrança registrada 7 dígitos.
                    //Cobrança sem registro até 13 dígitos.
                    if (Strings.Len(txtNossoNumeroBoleto.Text) < 7 & Strings.Len(txtNossoNumeroBoleto.Text) < 13)
                    {
                        Response.Write("O Nosso Número deve ser entre 7 e 13 caracteres.");
                        return;
                    }


                    //Carteira
                    if (txtCarteira.Text != "00" & txtCarteira.Text != "20" & txtCarteira.Text != "31" & txtCarteira.Text != "42" & txtCarteira.Text != "47" & txtCarteira.Text != "85" & txtCarteira.Text != "57")
                    {
                        Response.Write("A Carteira deve ser 00,20,31,42,47,57 ou 85.");
                        return;
                    }

                    //00'- Carteira do convênio
                    //20' - Cobrança Simples
                    //31' - Cobrança Câmbio
                    //42' - Cobrança Caucionada
                    //47' - Cobr. Caucionada Crédito Imobiliário
                    //85' - Cobrança Partilhada

                    //Agência 4 dígitos.
                    if (Strings.Len(Agencia) > 4)
                    {
                        Response.Write("A Agência deve conter até 4 dígitos.");
                        return;
                    }

                    //Número da conta 6 dígitos.
                    if (Strings.Len(Conta) > 6)
                    {
                        Response.Write("A Conta Corrente deve conter até 6 dígitos.");
                        return;
                    }


                    break;
                case "409":
                    //Banco Unibanco.
                    break;

                case "422":
                    //Banco Safra.
                    break;

                default:

                    break;
            }


            //Informa os dados do cedente
            Cedente c = new Cedente(txtCPFCNPJ.Text, txtNomeCedente.Text, Agencia.ToString(), DigAgencia.ToString(), Conta.ToString(), DigConta.ToString());

            //Dependendo da carteira, é necessário informar o código do cedente (o banco que fornece)
            c.Codigo = txtCodigoCedente.Text.Trim();

            //Dados para preenchimento do boleto (data de vencimento, valor, carteira e nosso número)
            Boleto b = new Boleto(Convert.ToDateTime(txtVencimento.Text), Convert.ToDecimal(txtValorBoleto.Text), txtCarteira.Text, txtNossoNumeroBoleto.Text, c);
            //"12345678901"

            //b.Carteira = "1"
            //b.Banco.Codigo = "18-019"

            //Dependendo da carteira, é necessário o número do documento
            b.NumeroDocumento = txtNumeroDocumentoBoleto.Text;
            b.ValorCobrado = Convert.ToDecimal(txtValorCobrado.Text);
            //"12345678901"

            //Informa os dados do sacado
            b.Sacado = new Sacado(txtCPFCNPJSacado.Text, txtNomeSacado.Text);
            b.Sacado.Endereco.End = txtEnderecoSacado.Text;
            b.Sacado.Endereco.Bairro = txtBairroSacado.Text;
            b.Sacado.Endereco.Cidade = txtCidadeSacado.Text;
            b.Sacado.Endereco.CEP = txtCEPSacado.Text;
            b.Sacado.Endereco.UF = txtUFSacado.Text;

            //Instrução.
            switch (txtBanco.Text)
            {
                case "001":
                    //Banco do Brasil.
                    Instrucao_BancoBrasil i1 = new Instrucao_BancoBrasil(Convert.ToInt32(txtBanco.Text));
                    i1.Descricao = txtInstrucoes.Text;
                    // "Não Receber após o vencimento"
                    b.Instrucoes.Add(i1);
                    break;
                case "033":
                    //Santander.
                    Instrucao_Santander i2 = new Instrucao_Santander(Convert.ToInt32(txtBanco.Text));
                    i2.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i2);
                    break;
                case "070":
                    //Banco BRB.
                    Instrucao i3 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i3.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i3);
                    break;
                case "104":
                    //Caixa Econômica Federal.
                    Instrucao_Caixa i4 = new Instrucao_Caixa(Convert.ToInt32(txtBanco.Text));
                    i4.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i4);
                    break;
                case "237":
                    //Banco Bradesco.
                    Instrucao_Bradesco i5 = new Instrucao_Bradesco(Convert.ToInt32(txtBanco.Text));
                    i5.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i5);
                    break;
                case "275":
                    //Banco Real.
                    Instrucao i6 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i6.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i6);
                    break;
                case "291":
                    //Banco BCN.
                    Instrucao i7 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i7.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i7);
                    break;
                case "341":
                    //Banco Itaú.
                    Instrucao_Itau i8 = new Instrucao_Itau(Convert.ToInt32(txtBanco.Text));
                    i8.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i8);
                    break;
                case "347":
                    //Banco Sudameris.
                    Instrucao i9 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i9.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i9);
                    break;
                case "356":
                    //Banco Real.
                    //Dim i10 As New Instrucao(CInt(txtBanco.Text))
                    Instrucao i10 = new Instrucao(1);
                    i10.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i10);
                    break;
                case "409":
                    //Banco Unibanco.
                    Instrucao i11 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i11.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i11);
                    break;
                case "422":
                    //Banco Safra.
                    Instrucao i12 = new Instrucao(Convert.ToInt32(txtBanco.Text));
                    i12.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i12);
                    break;
                default:
                    //Instrução de teste Santander.
                    Instrucao_Santander i13 = new Instrucao_Santander(Convert.ToInt32(txtBanco.Text));
                    i13.Descricao = txtInstrucoes.Text;
                    //"Não Receber após o vencimento"
                    b.Instrucoes.Add(i13);
                    break;
            }


            //Espécie do Documento - [R] Recibo
            switch (txtBanco.Text)
            {
                case "001":
                    //Banco do Brasil.
                    b.EspecieDocumento = new EspecieDocumento_BancoBrasil("2");
                    break;
                //Espécie.
                //Cheque = 1, //CH – CHEQUE
                //DuplicataMercantil = 2, //DM – DUPLICATA MERCANTIL
                //DuplicataMercantilIndicacao = 3, //DMI – DUPLICATA MERCANTIL P/ INDICAÇÃO
                //DuplicataServico = 4, //DS – DUPLICATA DE SERVIÇO
                //DuplicataServicoIndicacao = 5, //DSI – DUPLICATA DE SERVIÇO P/ INDICAÇÃO
                //DuplicataRural = 6, //DR – DUPLICATA RURAL
                //LetraCambio = 7, //LC – LETRA DE CAMBIO
                //NotaCreditoComercial = 8, //NCC – NOTA DE CRÉDITO COMERCIAL
                //NotaCreditoExportacao = 9, //NCE – NOTA DE CRÉDITO A EXPORTAÇÃO
                //NotaCreditoIndustrial = 10, //NCI – NOTA DE CRÉDITO INDUSTRIAL
                //NotaCreditoRural = 11, //NCR – NOTA DE CRÉDITO RURAL
                //NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
                //NotaPromissoriaRural = 13, //NPR –NOTA PROMISSÓRIA RURAL
                //TriplicataMercantil = 14, //TM – TRIPLICATA MERCANTIL
                //TriplicataServico = 15, //TS – TRIPLICATA DE SERVIÇO
                //NotaSeguro = 16, //NS – NOTA DE SEGURO
                //Recibo = 17, //RC – RECIBO
                //Fatura = 18, //FAT – FATURA
                //NotaDebito = 19, //ND – NOTA DE DÉBITO
                //ApoliceSeguro = 20, //AP – APÓLICE DE SEGURO
                //MensalidadeEscolar = 21, //ME – MENSALIDADE ESCOLAR
                //ParcelaConsorcio = 22, //PC – PARCELA DE CONSÓRCIO
                //Outros = 23 //OUTROS

                case "033":
                    //Santander.
                    b.EspecieDocumento = new EspecieDocumento_Santander("17");
                    break;
                case "070":
                    //Banco BRB.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                case "104":
                    //Caixa Econômica Federal.
                    b.EspecieDocumento = new EspecieDocumento_Caixa("17");
                    break;
                case "237":
                    //Banco Bradesco.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                case "275":
                    //Banco Real.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                case "291":
                    //Banco BCN.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                case "341":
                    //Banco Itaú.
                    b.EspecieDocumento = new EspecieDocumento_Itau("99");
                    break;
                case "347":
                    //Banco Sudameris.
                    b.EspecieDocumento = new EspecieDocumento_Sudameris("17");
                    break;
                case "356":
                    //Banco Real.
                    break;
                //Não funciona com isso.
                //b.EspecieDocumento = New EspecieDocumento_BancoBrasil(17)
                //b.EspecieDocumento = New EspecieDocumento_Itau(99)
                case "409":
                    //Banco Unibanco.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                case "422":
                    //Banco Safra.
                    b.EspecieDocumento = new EspecieDocumento(17);
                    break;
                default:
                    //Banco de teste Santander.
                    b.EspecieDocumento = new EspecieDocumento_Santander("17");
                    break;
            }


            BoletoBancario bb = new BoletoBancario();
            bb.CodigoBanco = Convert.ToInt16(txtBanco.Text);
            //33 '-> Referente ao código do Santander
            bb.Boleto = b;
            //bb.MostrarCodigoCarteira = True
            bb.Boleto.Valida();

            //true -> Mostra o compravante de entrega
            //false -> Oculta o comprovante de entrega
            bb.MostrarComprovanteEntrega = false;

            panelDados.Visible = false;
            panelBoleto.Controls.Clear();
            if (panelBoleto.Controls.Count == 0)
            {
                panelBoleto.Controls.Add(bb);
            }

            //03399.08063 49800.000330 32007.101028 8 41680000065640 -> correta
            //03399.08063 49800.000330 32007.101028 8 41680000065640
            //03399.08063 49800.000330 32007.101028 1 41680000065640
            //03399.08063 49800.003334 20071.301012 6 41680000065640
            //03399.08063 49800.000330 32007.101028 1 41680000065640

            //03399.08063 49800.003334 20071.301020 4 41680000065640
            //03399.08063 49800.003334 20071.301020 4 41680000065640

            //Gerar remessa.
            //Dim rdr As System.IO.Stream
            //Dim arquivo As New ArquivoRemessa(TipoArquivo.CNAB400)
            //arquivo.GerarArquivoRemessa(txtCodigoCedente.Text, b.Banco, _
            // b.Cedente, b, rdr, 1)
            //Response.Write(rdr.ToString())



            return;
        }
        catch (Exception ex)
        {

            Response.Write(ex);

        }
    }
}
