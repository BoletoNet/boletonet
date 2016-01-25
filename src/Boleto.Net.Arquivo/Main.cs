using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using BoletoNet;

namespace BoletoNet.Arquivo
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void remessaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region Remessa
        public void GeraArquivoCNAB400(IBanco banco, Cedente cedente, Boletos boletos)
        {
            try
            {
                saveFileDialog.Filter = "Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ArquivoRemessa arquivo = new ArquivoRemessa(TipoArquivo.CNAB400);

                    //Valida a Remessa Correspondentes antes de Gerar a mesma...
                    string vMsgRetorno = string.Empty;
                    bool vValouOK = arquivo.ValidarArquivoRemessa(cedente.Convenio.ToString(), banco, cedente, boletos, 1, out vMsgRetorno);
                    if (!vValouOK)
                    {
                        MessageBox.Show(String.Concat("Foram localizados inconsist�ncias na valida��o da remessa!", Environment.NewLine, vMsgRetorno),
                                        "Teste",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                    else
                    {
                        arquivo.GerarArquivoRemessa("0", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                        MessageBox.Show("Arquivo gerado com sucesso!", "Teste",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GeraArquivoCNAB240(IBanco banco, Cedente cedente, Boletos boletos)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ArquivoRemessa arquivo = new ArquivoRemessa(TipoArquivo.CNAB240);
                arquivo.GerarArquivoRemessa("1200303001417053", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                MessageBox.Show("Arquivo gerado com sucesso!", "Teste",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }
        //
        public void GeraDadosItau()
        {
            DateTime vencimento = new DateTime(2007, 9, 10);

            Instrucao_Itau item1 = new Instrucao_Itau(9, 5);
            Instrucao_Itau item2 = new Instrucao_Itau(81, 10);
            Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
            //Na carteira 198 o c�digo do Cedente � a conta banc�ria
            c.Codigo = "13000";

            Boleto b = new Boleto(vencimento, 1642, "198", "92082835", c);
            b.NumeroDocumento = "1008073";

            b.DataVencimento = Convert.ToDateTime("12-12-12");

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testel�ndia";
            b.Sacado.Endereco.CEP = "70000000";
            b.Sacado.Endereco.UF = "DF";

            item2.Descricao += item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
            b.Instrucoes.Add(item1);
            b.Instrucoes.Add(item2);
            b.Cedente.ContaBancaria.DigitoAgencia = "1";
            b.Cedente.ContaBancaria.DigitoAgencia = "2";

            b.Banco = new Banco(341);

            Boletos boletos = new Boletos();
            boletos.Add(b);

            Boleto b2 = new Boleto(vencimento, 1642, "198", "92082835", c);
            b2.NumeroDocumento = "1008073";

            b2.DataVencimento = Convert.ToDateTime("12-12-12");

            b2.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b2.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b2.Sacado.Endereco.Bairro = "Testando";
            b2.Sacado.Endereco.Cidade = "Testel�ndia";
            b2.Sacado.Endereco.CEP = "70000000";
            b2.Sacado.Endereco.UF = "DF";

            item2.Descricao += item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
            b2.Instrucoes.Add(item1);
            b2.Instrucoes.Add(item2);
            b2.Cedente.ContaBancaria.DigitoAgencia = "1";
            b2.Cedente.ContaBancaria.DigitoAgencia = "2";

            b2.Banco = new Banco(341);

            boletos.Add(b2);

            GeraArquivoCNAB400(b2.Banco, c, boletos);
        }
        public void GeraDadosBanrisul()
        {
            ContaBancaria conta = new ContaBancaria();
            conta.Agencia = "051";
            conta.DigitoAgencia = "2";
            conta.Conta = "13000";
            conta.DigitoConta = "3";
            //
            Cedente c = new Cedente();
            c.ContaBancaria = conta;
            c.CPFCNPJ = "00.000.000/0000-00";
            c.Nome = "Empresa de Atacado";
            //Na carteira 198 o c�digo do Cedente � a conta banc�ria
            c.Codigo = "513035600299";//No Banrisul, esse c�digo est� no manual como 12 caracteres, por eu(sidneiklein) isso tive que alterar o tipo de int para string;
            c.Convenio = 124522;
            //
            Boleto b = new Boleto();
            b.Cedente = c;
            //
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "1";
            b.VariacaoCarteira = "02";
            b.NossoNumero = string.Empty; //"92082835"; //** Para o "Remessa.TipoDocumento = "06", n�o poder� ter NossoN�mero Gerado!
            b.NumeroDocumento = "1008073";
            //
            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testel�ndia";
            b.Sacado.Endereco.CEP = "70000000";
            b.Sacado.Endereco.UF = "RS";

            Instrucao_Banrisul item1 = new Instrucao_Banrisul(9, 5, 0);
            b.Instrucoes.Add(item1);
            //b.Instrucoes.Add(item2);
            b.Banco = new Banco(041);

            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "06"; //06 - COBRAN�A ESCRITURAL
            #endregion

            //
            Boletos boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }
        public void GeraDadosSicredi()
        {
            ContaBancaria conta = new ContaBancaria();
            conta.Agencia = "051";
            conta.DigitoAgencia = "2";
            conta.Conta = "13000";
            conta.DigitoConta = "3";
            //
            Cedente c = new Cedente();
            c.ContaBancaria = conta;
            c.CPFCNPJ = "00000000000000";
            c.Nome = "Empresa de Atacado";
            //Na carteira 198 o c�digo do Cedente � a conta banc�ria
            c.Codigo = "12345";//No Banrisul, esse c�digo est� no manual como 12 caracteres, por eu(sidneiklein) isso tive que alterar o tipo de int para string;
            c.Convenio = 124522;
            //
            Boleto b = new Boleto();
            b.Cedente = c;
            //
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "1";
            b.VariacaoCarteira = "02";
            b.NossoNumero = string.Empty; //"92082835"; //** Para o "Remessa.TipoDocumento = "06", n�o poder� ter NossoN�mero Gerado!
            b.NumeroDocumento = "1008073";
            //
            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testel�ndia";
            b.Sacado.Endereco.CEP = "70000000";
            b.Sacado.Endereco.UF = "RS";

            Instrucao_Sicredi item1 = new Instrucao_Sicredi(9, 5);
            b.Instrucoes.Add(item1);
            //b.Instrucoes.Add(item2);
            b.Banco = new Banco(748);

            //
            EspecieDocumento especiedocumento = new EspecieDocumento(748, "A");//(341, 1);
            b.EspecieDocumento = especiedocumento;


            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "A"; //A = 'A' - SICREDI com Registro
            #endregion

            //
            Boletos boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }
        public void GeraDadosSantander()
        {
            Boletos boletos = new Boletos();

            DateTime vencimento = new DateTime(2003, 5, 15);

            Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "2269", "130000946");
            c.Codigo = "1795082";

            Boleto b = new Boleto(vencimento, 0.20m, "101", "566612457800", c);

            //NOSSO N�MERO
            //############################################################################################################################
            //N�mero adotado e controlado pelo Cliente, para identificar o t�tulo de cobran�a.
            //Informa��o utilizada pelos Bancos para referenciar a identifica��o do documento objeto de cobran�a.
            //Poder� conter n�mero da duplicata, no caso de cobran�a de duplicatas, n�mero de ap�lice, no caso de cobran�a de seguros, etc.
            //Esse campo � devolvido no arquivo retorno.
            b.NumeroDocumento = "0282033";

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testel�ndia";
            b.Sacado.Endereco.CEP = "70000000";
            b.Sacado.Endereco.UF = "DF";

            //b.Instrucoes.Add("N�o Receber ap�s o vencimento");
            //b.Instrucoes.Add("Ap�s o Vencimento pague somente no Bradesco");
            //b.Instrucoes.Add("Instru��o 2");
            //b.Instrucoes.Add("Instru��o 3");

            //Esp�cie Documento - [R] Recibo
            b.EspecieDocumento = new EspecieDocumento_Santander("17");

            boletos.Add(b);

            GeraArquivoCNAB240(new Banco(33), c, boletos);
        }
        public void GeraDadosCaixa()
        {
            ContaBancaria conta = new ContaBancaria();
            conta.OperacaConta = "OPE";
            conta.Agencia = "345";
            conta.DigitoAgencia = "6";
            conta.Conta = "87654321";
            conta.DigitoConta = "0";
            //
            Cedente c = new Cedente();
            c.ContaBancaria = conta;
            c.CPFCNPJ = "00.000.000/0000-00";
            c.Nome = "Empresa de Atacado";
            //Na carteira 198 o c�digo do Cedente � a conta banc�ria
            c.Codigo = String.Concat(conta.Agencia, conta.DigitoAgencia, conta.OperacaConta, conta.Conta, conta.DigitoConta); //Na Caixa, esse c�digo est� no manual como 16 caracteres AAAAOOOCCCCCCCCD;
            //
            Boleto b = new Boleto();
            b.Cedente = c;
            //
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "SR";
            b.NossoNumero = "92082835";
            b.NumeroDocumento = "1008073";
            EspecieDocumento ED = new EspecieDocumento(104);
            b.EspecieDocumento = ED;

            //
            b.Sacado = new Sacado("Fulano de Silva");
            b.Sacado.CPFCNPJ = "000.000.000-00";
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testel�ndia";
            b.Sacado.Endereco.CEP = "70000000";
            b.Sacado.Endereco.UF = "RS";

            Instrucao_Caixa item1 = new Instrucao_Caixa(9, 5);
            b.Instrucoes.Add(item1);
            //b.Instrucoes.Add(item2);
            b.Banco = new Banco(104);

            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "2"; // SIGCB - SEM REGISTRO
            b.Remessa.CodigoOcorrencia = string.Empty;
            #endregion

            //
            Boletos boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB240(b.Banco, c, boletos);
        }
        #endregion Remessa

        #region Retorno
        private void LerRetorno(int codigo)
        {
            try
            {
                Banco bco = new Banco(codigo);

                openFileDialog.FileName = "";
                openFileDialog.Title = "Selecione um arquivo de retorno";
                openFileDialog.Filter = "Arquivos de Retorno (*.ret;*.crt)|*.ret;*.crt|Todos Arquivos (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    if (radioButtonCNAB400.Checked)
                    {
                        ArquivoRetornoCNAB400 cnab400 = null;
                        if (openFileDialog.CheckFileExists == true)
                        {
                            cnab400 = new ArquivoRetornoCNAB400();
                            cnab400.LinhaDeArquivoLida += new EventHandler<LinhaDeArquivoLidaArgs>(cnab400_LinhaDeArquivoLida);
                            cnab400.LerArquivoRetorno(bco, openFileDialog.OpenFile());
                        }

                        if (cnab400 == null)
                        {
                            MessageBox.Show("Arquivo n�o processado!");
                            return;
                        }

                        lstReturnFields.Items.Clear();

                        foreach (DetalheRetorno detalhe in cnab400.ListaDetalhe)
                        {
                            ListViewItem li = new ListViewItem(detalhe.NomeSacado.ToString().Trim());
                            li.Tag = detalhe;

                            li.SubItems.Add(detalhe.DataVencimento.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.DataCredito.ToString("dd/MM/yy"));

                            li.SubItems.Add(detalhe.ValorTitulo.ToString("###,###.00"));

                            li.SubItems.Add(detalhe.ValorPago.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.CodigoOcorrencia.ToString());
                            li.SubItems.Add("");
                            li.SubItems.Add(detalhe.NossoNumeroComDV); // = detalhe.NossoNumero.ToString() + "-" + detalhe.DACNossoNumero.ToString());
                            li.SubItems.Add(detalhe.NumeroDocumento);
                            lstReturnFields.Items.Add(li);
                        }
                    }
                    else if (radioButtonCNAB240.Checked)
                    {
                        ArquivoRetornoCNAB240 cnab240 = null;
                        if (openFileDialog.CheckFileExists == true)
                        {
                            cnab240 = new ArquivoRetornoCNAB240();
                            cnab240.LinhaDeArquivoLida += new EventHandler<LinhaDeArquivoLidaArgs>(cnab240_LinhaDeArquivoLida);
                            cnab240.LerArquivoRetorno(bco, openFileDialog.OpenFile());
                        }

                        if (cnab240 == null)
                        {
                            MessageBox.Show("Arquivo n�o processado!");
                            return;
                        }


                        lstReturnFields.Items.Clear();

                        foreach (DetalheRetornoCNAB240 detalhe in cnab240.ListaDetalhes)
                        {
                            ListViewItem li = new ListViewItem(detalhe.SegmentoT.NomeSacado.Trim());
                            li.Tag = detalhe;

                            li.SubItems.Add(detalhe.SegmentoT.DataVencimento.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.SegmentoU.DataCredito.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.SegmentoT.ValorTitulo.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.SegmentoU.ValorPagoPeloSacado.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.SegmentoU.CodigoOcorrenciaSacado.ToString());
                            li.SubItems.Add("");
                            li.SubItems.Add(detalhe.SegmentoT.NossoNumero);
                            lstReturnFields.Items.Add(li);
                        }
                    }
                    MessageBox.Show("Arquivo aberto com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao abrir arquivo de retorno.");
            }
        }

        void cnab240_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            MessageBox.Show(e.Linha);
        }

        void cnab400_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            MessageBox.Show(e.Linha);
        }
        #endregion Retorno

        #region Exemplos de arquivos de retorno
        public void GeraArquivoCNAB400Itau(Stream arquivo)
        {
            try
            {
                StreamWriter gravaLinha = new StreamWriter(arquivo);

                #region Vari�veis

                string _header;
                string _detalhe1;
                string _detalhe2;
                string _detalhe3;
                string _trailer;

                string n275 = new string(' ', 275);
                string n025 = new string(' ', 25);
                string n023 = new string(' ', 23);
                string n039 = new string('0', 39);
                string n026 = new string('0', 26);
                string n090 = new string(' ', 90);
                string n160 = new string(' ', 160);

                #endregion

                #region HEADER

                _header = "02RETORNO01COBRANCA       347700232610        ALLMATECH TECNOLOGIA DA INFORM341BANCO ITAU SA  ";
                _header += "08010800000BPI00000201207";
                _header += n275;
                _header += "000001";

                gravaLinha.WriteLine(_header);

                #endregion

                #region DETALHE

                _detalhe1 = "10201645738000250097700152310        " + n025 + "00000001            112000000000             ";
                _detalhe1 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                _detalhe1 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                _detalhe1 += "AA000002";

                gravaLinha.WriteLine(_detalhe1);

                _detalhe2 = "10201645738000250097700152310        " + n025 + "00000002            112000000000             ";
                _detalhe2 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                _detalhe2 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                _detalhe2 += "AA000003";

                gravaLinha.WriteLine(_detalhe2);

                _detalhe3 = "10201645738000250097700152310        " + n025 + "00000003            112000000000             ";
                _detalhe3 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                _detalhe3 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                _detalhe3 += "AA000004";

                gravaLinha.WriteLine(_detalhe3);

                #endregion

                #region TRAILER

                _trailer = "9201341          0000000300000000060000                  0000000000000000000000        ";
                _trailer += n090 + "0000000000000000000000        000010000000300000000060000" + n160 + "000005";
                ;

                gravaLinha.WriteLine(_trailer);

                #endregion

                gravaLinha.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar arquivo.", ex);
            }
        }
        #endregion Exemplos de arquivos de retorno

        private void impress�oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NBoleto form = new NBoleto();

            if (radioButtonItau.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonItau.Tag);
            else if (radioButtonUnibanco.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonUnibanco.Tag);
            else if (radioButtonSudameris.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonSudameris.Tag);
            else if (radioButtonSafra.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonSafra.Tag);
            else if (radioButtonReal.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonReal.Tag);
            else if (radioButtonHsbc.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonHsbc.Tag);
            else if (radioButtonBancoBrasil.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBancoBrasil.Tag);
            else if (radioButtonBradesco.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBradesco.Tag);
            else if (radioButtonCaixa.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonCaixa.Tag);


            form.ShowDialog();
        }

        private void cNABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (radioButtonCNAB400.Checked)
            {
                if (radioButtonItau.Checked)
                    GeraDadosItau();
                else if (radioButtonBanrisul.Checked)
                    GeraDadosBanrisul();
                else if (radioButtonCaixa.Checked)
                    GeraDadosCaixa();
                else if (radioButtonSicredi.Checked)
                    GeraDadosSicredi();
            }
            else if (radioButtonCNAB240.Checked)
            {
                if (radioButtonSantander.Checked)
                    GeraDadosSantander();
                else if (radioButtonBanrisul.Checked)
                    MessageBox.Show("N�o Implementado!");
                else if (radioButtonCaixa.Checked)
                    GeraDadosCaixa();
            }
        }

        private void lerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (radioButtonItau.Checked)
                LerRetorno(341);
            else if (radioButtonSudameris.Checked)
                LerRetorno(347);
            else if (radioButtonSantander.Checked)
                LerRetorno(33);
            else if (radioButtonReal.Checked)
                LerRetorno(356);
            else if (radioButtonCaixa.Checked)
                LerRetorno(104);
            else if (radioButtonBradesco.Checked)
                LerRetorno(237);
            else if (radioButtonSicredi.Checked)
                LerRetorno(748);
            else if (radioButtonBanrisul.Checked)
                LerRetorno(041);            
        }

        private void gerarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (radioButtonCNAB400.Checked)
                {
                    if (radioButtonItau.Checked)
                        GeraArquivoCNAB400Itau(saveFileDialog.OpenFile());
                }
                else
                {
                    //if (radioButtonSantander.Checked)
                    //    GeraArquivoCNAB240Santander(saveFileDialog.OpenFile());
                }

                MessageBox.Show("Arquivo gerado com sucesso!", "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}