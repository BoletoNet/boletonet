using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using BoletoNet;

namespace BoletoNet.Arquivo
{
    public partial class NBoleto : Form
    {
        private short _codigoBanco = 0;
        private Progresso _progresso;
        string _arquivo = string.Empty;
        private ImpressaoBoleto _impressaoBoleto = new ImpressaoBoleto();

        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        public NBoleto()
        {
            InitializeComponent();

            _impressaoBoleto.FormClosed += new FormClosedEventHandler(_impressaoBoleto_FormClosed);
        }

        void _impressaoBoleto_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
            Dispose();
        }

        #region GERA LAYOUT DO BOLETO
        private void GeraLayout(List<BoletoBancario> boletos)
        {
            StringBuilder html = new StringBuilder();
            foreach (BoletoBancario o in boletos)
            {
                string logoPath = Application.StartupPath + @"\logoBoleto.jpg";
                if (!File.Exists(logoPath))
                    logoPath = "";
                html.Append(o.MontaHtml(null,logoPath));
                html.Append("</br></br></br></br></br></br></br></br></br></br>");
            }

            _arquivo = System.IO.Path.GetTempFileName();

            using (FileStream f = new FileStream(_arquivo, FileMode.Create))
            {
                StreamWriter w = new StreamWriter(f, System.Text.Encoding.UTF8);
                w.Write(html.ToString());
                w.Close();
                f.Close();
            }
        }
        #endregion

        #region BOLETO Caixa
        private void GeraBoletoCaixa(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;
                bb.MostrarEnderecoCedente = true;
                DateTime vencimento = DateTime.Now.AddDays(10);

                Instrucao_Caixa item1 = new Instrucao_Caixa(9, 5);
                Instrucao_Caixa item2 = new Instrucao_Caixa(81, 10);
                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0132", "00542");

                Boleto b = new Boleto(vencimento, 460, "SR", "00000000010001", c);
                Endereco endCed = new Endereco();

                endCed.End = "Rua Testando o Boleto";
                endCed.Bairro = "BairroTest";
                endCed.Cidade = "CidadeTes";
                endCed.CEP = "70000000";
                endCed.UF = "MG";
                b.Cedente.Endereco = endCed;

                b.NumeroDocumento = Convert.ToString(1000 + i);

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23ddddddddddddddddddddddddddd";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                item2.Descricao += " " + item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
                b.Instrucoes.Add(item1);
                b.Instrucoes.Add(item2);

                // juros/descontos

                if (b.ValorDesconto == 0)
                {
                    Instrucao_Caixa item3 = new Instrucao_Caixa(999, 1);
                    item3.Descricao += ("1,00 por dia de antecipação.");
                    b.Instrucoes.Add(item3);
                }

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion

        #region BOLETO Caixa
        private void GeraBoletoSicredi(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;
                bb.MostrarEnderecoCedente = true;                
                bb.FormatoCarne = true;
                bb.OcultarInstrucoes = true;
                DateTime vencimento = DateTime.Now.AddDays(10);

                Instrucao_Sicredi item1 = new Instrucao_Sicredi((int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados_Desconto, (Double)50.90, AbstractInstrucao.EnumTipoValor.Reais);
              
             
                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0132", "00542");
                c.Codigo = "01321000542";
                c.ContaBancaria.Agencia = "0132";               
                c.ContaBancaria.Conta = "00542";
                c.ContaBancaria.OperacaConta = "10";

                Boleto b = new Boleto(vencimento, Convert.ToDecimal(1460), "1", "17200001" , c, new EspecieDocumento(_codigoBanco, "A"));
               
                Endereco endCed = new Endereco();

                b.NumeroParcela = 1;
                b.TotalParcela = 10;
                b.TipoImpressao = "B";


                endCed.End = "Rua Testando o Boleto";
                endCed.Bairro = "BairroTest";
                endCed.Cidade = "CidadeTes";
                endCed.CEP = "70000000";
                endCed.UF = "MG";
                b.Cedente.Endereco = endCed;

                b.NumeroDocumento = "1001";

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";
                b.Instrucoes.Add(item1);
              

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion

        #region BOLETO ITAÚ
        private void GeraBoletoItau(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);

                Instrucao_Itau item1 = new Instrucao_Itau(9, 5);
                Instrucao_Itau item2 = new Instrucao_Itau(81, 10);
                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
                //Na carteira 198 o código do Cedente é a conta bancária
                c.Codigo = "13000";

                Boleto b = new Boleto(vencimento, 1642, "198", "92082835", c, new EspecieDocumento(341, "1"));
                b.NumeroDocumento = Convert.ToString(10000 + i);

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                item2.Descricao += " " + item2.QuantidadeDias.ToString() + " dias corridos do vencimento.";
                b.Instrucoes.Add(item1);
                b.Instrucoes.Add(item2);

                // juros/descontos

                if (b.ValorDesconto == 0)
                {
                    Instrucao_Itau item3 = new Instrucao_Itau(999, 1);
                    item3.Descricao += ("1,00 por dia de antecipação.");
                    b.Instrucoes.Add(item3);
                }

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion

        #region BOLETO UNIBANCO
        public void GeraBoletoUnibanco(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);

                Instrucao instr = new Instrucao(001);
                Cedente c = new Cedente("00.000.000/0000-00", "Next Consultoria Ltda.", "0123", "100618", "9");
                c.Codigo = "203167";

                Boleto b = new Boleto(vencimento, 2952.95m, "20", "1803029901", c);
                b.NumeroDocumento = b.NossoNumero;

                b.Sacado = new Sacado("000.000.000-00", "Marlon Oliveira");
                b.Sacado.Endereco.End = "Rua Dr. Henrique Portugal, XX";
                b.Sacado.Endereco.Bairro = "São Francisco";
                b.Sacado.Endereco.Cidade = "Niterói";
                b.Sacado.Endereco.CEP = "24360080";
                b.Sacado.Endereco.UF = "RJ";
                b.Sacado.Endereco.Numero = "XX";
                b.Sacado.Endereco.Complemento = "Casa";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO SUDAMERIS
        public void GeraBoletoSudameris(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);
                Instrucao instr = new Instrucao(001);

                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0501", "6703255");
                c.Codigo = "13000";

                //Nosso número com 7 dígitos
                string nn = "0003020";
                //Nosso número com 13 dígitos
                //nn = "0000000003025";

                Boleto b = new Boleto(vencimento, 1642, "198", nn, c);// EnumEspecieDocumento_Sudameris.DuplicataMercantil);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO SAFRA
        public void GeraBoletoSafra(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);
                Instrucao instr = new Instrucao(001);

                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "5413000");
                c.Codigo = "13000";

                Boleto b = new Boleto(vencimento, 1642, "198", "02592082835", c);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();
                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO REAL
        public void GeraBoletoReal(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);
                Instrucao instr = new Instrucao(001);
                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
                c.Codigo = "13000";

                Boleto b = new Boleto(vencimento, 1642, "57", "92082835", c);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO HSBC
        public void GeraBoletoHsbc(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);
                Instrucao instr = new Instrucao(001);
                Cedente c = new Cedente("00.000.000/0000-00", "Minha empresa", "0000", "", "00000", "00");
                // Código fornecido pela agencia, NÃO é o numero da conta
                c.Codigo = "0000000"; // 7 posicoes

                Boleto b = new Boleto(vencimento, 2, "CNR", "888888888", c); //cod documento
                b.NumeroDocumento = "9999999999999"; // nr documento

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Tal");
                b.Sacado.Endereco.End = "lala";
                b.Sacado.Endereco.Bairro = "lala";
                b.Sacado.Endereco.Cidade = "Curitiba";
                b.Sacado.Endereco.CEP = "82000000";
                b.Sacado.Endereco.UF = "PR";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO BANCO DO BRASIL
        public void GeraBoletoBB(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;
                bb.MostrarEnderecoCedente = true;            
                //bb.ExibirDemonstrativo = true;
           
                DateTime vencimento = DateTime.Now.AddDays(10);
                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "12345678", "9");
                c.Endereco = new Endereco();
                c.Endereco.End = "Rua teste";
                c.Endereco.Bairro = "Jd. teste";
                c.Endereco.CEP = "13211-478";
                c.Endereco.Cidade = "Cidade Teste";
                c.Endereco.UF = "SP";

                c.Codigo = "00000000504";
                Boleto b = new Boleto(vencimento, 45.50m, "11", "12345678901", c);                
                
                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "DF";

                //Adiciona as instruções ao boleto
                //Protestar
                Instrucao_BancoBrasil item = new Instrucao_BancoBrasil(9, 5);
                b.Instrucoes.Add(item);
                //ImportanciaporDiaDesconto
                item = new Instrucao_BancoBrasil(30, 0);
                b.Instrucoes.Add(item);
                //ProtestarAposNDiasCorridos
                item = new Instrucao_BancoBrasil(81, 15);
                b.Instrucoes.Add(item);

                ////DEMONSTRATIVO
                //DemonstrativoValoresBoleto.GrupoDemonstrativo d = new DemonstrativoValoresBoleto.GrupoDemonstrativo();
                //d.Descricao = "Demonstrativo teste";
                //b.Demonstrativos.Add(d);

                b.NumeroDocumento = "12345678901";
                b.LocalPagamento = "Pagável em qualquer banco.";

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion
        
        #region BOLETO BRADESCO
        public void GeraBoletoBradesco(int qtde)
        {

            // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {

                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                DateTime vencimento = DateTime.Now.AddDays(10);
                Instrucao_Bradesco item = new Instrucao_Bradesco(9, 5);

                Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "123456", "7");
                c.Codigo = "13000";

                Endereco end = new Endereco();
                end.Bairro = "Lago Sul";
                end.CEP = "71666660";
                end.Cidade = "Brasília- DF";
                end.Complemento = "Quadra XX Conjunto XX Casa XX";
                end.End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX";
                end.Numero = "55";
                end.UF = "DF";

                Boleto b = new Boleto(vencimento, 1.01m, "02", "01000000001", c);
                b.NumeroDocumento = "01000000001";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = end;

                item.Descricao += " após " + item.QuantidadeDias.ToString() + " dias corridos do vencimento.";
                b.Instrucoes.Add(item); //"Não Receber após o vencimento");

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        #endregion

        #region BOLETO NORDESTE
        public void GeraBoletoBNB(int qtde)
        {
                        // Cria o boleto, e passa os parâmetros usuais
            BoletoBancario bb;

            List<BoletoBancario> boletos = new List<BoletoBancario>();
            for (int i = 0; i < qtde; i++)
            {
                bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                ContaBancaria conta = new ContaBancaria();
                conta.Agencia = "21";
                conta.DigitoAgencia = "0";
                conta.Conta = "12717";
                conta.DigitoConta = "8";

                Cedente c = new Cedente();
                c.ContaBancaria = conta;
                c.CPFCNPJ = "00.000.000/0000-00";
                c.Nome = "Empresa de Atacado";

                Boleto b = new Boleto();
                b.Cedente = c;
                //
                b.DataProcessamento = DateTime.Now;
                b.DataVencimento = DateTime.Now.AddDays(15);
                b.ValorBoleto = Convert.ToDecimal(1);
                b.Carteira = "4";
                b.NossoNumero = "7777777";
                b.NumeroDocumento = "2525";
                //
                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.CEP = "70000000";
                b.Sacado.Endereco.UF = "RS";

                b.Banco = new Banco(004);

                EspecieDocumento especiedocumento = new EspecieDocumento(004, "1");//Duplicata Mercantil
                b.EspecieDocumento = especiedocumento;

                bb.Boleto = b;
                bb.Boleto.Valida();
                boletos.Add(bb);
            }
            GeraLayout(boletos);


        }
        #endregion 

        #region Eventos do BackgroundWorker
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            switch (CodigoBanco)
            {
                case 1: // Banco do Brasil
                    GeraBoletoBB((int)numericUpDown.Value);
                    break;

                case 409: // Unibanco
                    GeraBoletoUnibanco((int)numericUpDown.Value);
                    break;

                case 347: // Sudameris
                    GeraBoletoSudameris((int)numericUpDown.Value);
                    break;

                case 422: // Safra
                    GeraBoletoSafra((int)numericUpDown.Value);
                    break;

                case 341: // Itau
                    GeraBoletoItau((int)numericUpDown.Value);
                    break;

                case 356: // Real
                    GeraBoletoReal((int)numericUpDown.Value);
                    break;

                case 399: // Hsbc
                    GeraBoletoItau((int)numericUpDown.Value);
                    break;

                case 237: // Bradesco
                    GeraBoletoBradesco((int)numericUpDown.Value);
                    break;

                case 104: // Caixa
                    GeraBoletoCaixa((int)numericUpDown.Value);
                    break;
                case 4: //BNB
                    GeraBoletoBNB((int)numericUpDown.Value);
                    break;

                case 748:
                    GeraBoletoSicredi((int)numericUpDown.Value);
                    break;
            }

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _progresso.Close();

            // Cria um formulário com um componente WebBrowser dentro
            _impressaoBoleto.webBrowser.Navigate(_arquivo);
            _impressaoBoleto.ShowDialog();

        }
        #endregion Eventos do BackgroundWorker



        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown.Value > 1)
                MessageBox.Show("O exemplo de envio do boleto bancário como imagem só está implementado para somente um boleto por vez.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.RunWorkerAsync();
            _progresso = new Progresso();
            _progresso.ShowDialog();
        }
    }
}
