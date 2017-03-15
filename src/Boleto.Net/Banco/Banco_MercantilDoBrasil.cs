using System;
using System.Globalization;
using System.Web.UI;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.389.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Banco_Mercantil do Brasil
    /// Autor: Rafael Paiva
    /// E-mail: desenvolvimento@rcrinfo.com.br
    /// FBook: facebook.com/kauhuha
    /// Data: 14/03/2017
    /// </summary>
    internal class Banco_MercantilDoBrasil : AbstractBanco, IBanco
    {
        private string _dacBoleto = string.Empty;      

        internal Banco_MercantilDoBrasil()
        {
            this.Codigo = 389;
            this.Digito = "1";
            this.Nome = "Mercantil do Brasil";
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            // Posição 01 - 03   | IDENTIFICAÇÃO DO BANCO [03 BYTES]
            var codigoBanco = Codigo.ToString();

            // Posição 04        | CÓDIGO DA MOEDA 01 BYTES
            var codigoMoeda = "9";

            // Posição 05        | DIGITO VERIFICADOR DO CÓDIGO DE BARRAS (DAC) [01 BYTES]
            var dvCodigoBarras = "";

            // Posição 06 - 09   | FATOR DE VENCIMENTO [04 BYTES]
            var fatorVencimento = FatorVencimento(boleto);

            // Posição 10 - 19   | VALOR NOMINAL DO TITULO [10 BYTES]
            var valorNominalTitulo = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorNominalTitulo = Utils.FormatCode(valorNominalTitulo, 10);

            // Posição 20 - 23   | AGÊNCIA BENEFICIÁRIO [04 BYTES]
            var agenciaBeneficiario = boleto.Cedente.ContaBancaria.Agencia;
             
            // Posição 24 - 34   | NOSSO NÚMERO [11 BYTES]
            var nossoNumero = boleto.NossoNumero;

            // Posição 35 - 43   | CÓDIGO DO CONTRATO [09 BYTES]
            var codigoContrato = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;

            // Posição 44        | INDICADOR DE DESCONTO = 2 (SEM DESCONTO) [01 BYTE]
            var indicadorDesconto = "2";

            //DAC - DIGITO DE AUTO CONFERENCIA (CALCULADO MOD.11) 01 BYTE
            var dac = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", codigoBanco, codigoMoeda, fatorVencimento, valorNominalTitulo,
                                                       agenciaBeneficiario, nossoNumero, codigoContrato, indicadorDesconto);

            dvCodigoBarras = MOD11_Mercantil(dac, false, true);
            _dacBoleto = dvCodigoBarras;

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", codigoBanco, codigoMoeda, dvCodigoBarras, fatorVencimento,
                valorNominalTitulo, agenciaBeneficiario, nossoNumero, codigoContrato, indicadorDesconto);

        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            var grupo1 = "";
            var grupo2 = "";
            var grupo3 = "";
            var grupo4 = "";
            var grupo5 = "";
                     
            var codigoBanco = Codigo.ToString();
            var codigoMoeda = "9";
            var agenciaBeneficiario = boleto.Cedente.ContaBancaria.Agencia;
            var primeiroByteNossoNumero = boleto.NossoNumero.Substring(0, 1);
            var digitoVerificador = Mod10(codigoBanco + codigoMoeda + agenciaBeneficiario + primeiroByteNossoNumero).ToString();

            grupo1 = string.Format("{0}{1}{2}{3}{4}", codigoBanco, codigoMoeda, agenciaBeneficiario, 
                primeiroByteNossoNumero, digitoVerificador);

            var nossoNumero = boleto.NossoNumero.Substring(1, 10);
            digitoVerificador = Mod10(nossoNumero).ToString();

            grupo2 = string.Format("{0}{1}", nossoNumero, digitoVerificador);

            var numeroContrato = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
            var indicadorDesconto = "2";
            digitoVerificador = Mod10(numeroContrato + indicadorDesconto).ToString();

            grupo3 = string.Format("{0}{1}{2}", numeroContrato, indicadorDesconto, digitoVerificador);

            grupo4 = _dacBoleto;

            var fatorVencimento = FatorVencimento(boleto);
            var valorNominalTitulo = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorNominalTitulo = Utils.FormatCode(valorNominalTitulo, 10);

            grupo5 = string.Format("{0}{1}", fatorVencimento, valorNominalTitulo);

            // Concatena os dados da linha digitavel
            boleto.CodigoBarra.LinhaDigitavel = LinhaDigitavelFormatada(grupo1 + grupo2 + grupo3 + grupo4 + grupo5);
        }

        private string LinhaDigitavelFormatada(string linhaDigitavel)
        {
            var Resultado = linhaDigitavel.Substring(0, 5) + ".";
            Resultado += linhaDigitavel.Substring(5, 5) + " ";
            Resultado += linhaDigitavel.Substring(10, 5) + ".";
            Resultado += linhaDigitavel.Substring(15, 6) + " ";
            Resultado += linhaDigitavel.Substring(21, 5) + ".";
            Resultado += linhaDigitavel.Substring(26, 6) + " ";
            Resultado += linhaDigitavel.Substring(32, 1) + " ";
            Resultado += linhaDigitavel.Substring(33, 14);

            return Resultado;
        }

        private string MOD11_Mercantil(string _String, bool _NossoNumero = false, bool _DAC = false) // COMPOSIÇÃO DOS CAMPOS NECESSÁRIOS AO CALCULO DO DIGITO VERIFICADOR
        {
            var Retorno = "";
            var Peso = ""; // SEQÜÊNCIA DE “98765432”

            var Sequencia = "98765432";
            var ContadorPeso = 1;

            var Soma = 0;
            var Resto = 0;

            // CRIAR CAMPO PESO COM O MESMO TAMANHO DO CAMPO STRING E DEVERÁ SER COMPOSTO POR UMA SEQÜÊNCIA DE “98765432” DA DIREITA PARA A ESQUERDA.
            // EX.: PESO = 12 BYTES = “543298765432” PESO = 5 BYTES = “65432”
            for (int i = 0; i < _String.Length; i++) {
                Peso = Sequencia.Substring(Sequencia.Length - ContadorPeso, 1) + Peso;

                ContadorPeso++;

                if (ContadorPeso > 8) {
                    ContadorPeso = 1;
                }
            }

            // MULTIPLICAR CADA BYTE DO CAMPO PESO PELO SEU CORRESPONDENTE NO CAMPO STRING E ACUMULAR O RESULTADO.
            for (int i = 0; i < _String.Length; i++) {
                Soma += Convert.ToInt16(_String.Substring(i, 1)) * Convert.ToInt16(Peso.Substring(i, 1));
            }

            // DIVIDIR O SOMATÓRIO POR 11(ONZE) E SALVAR O RESTO
            Resto = Soma % 11;

            // QUANDO FOR O CÁLCULO DO NOSSO NÚMERO USAR  SE RESTO FOR IGUAL A 0(ZERO) OU 1(UM) O DIGITO DEVERÁ SER 0(ZERO), CASO CONTRÁRIO, O DIGITO SERÁ A DIFERENÇA ENTRE 11 E O RESTO.
            if (_NossoNumero) {
                if (Resto == 1 || Resto == 0) {
                    Retorno = "0";
                }
                else {
                    Retorno = Convert.ToString(11 - Resto);
                }
            }

            // QUANDO FOR O CÁLCULO DO DAC USAR  SE RESTO FOR IGUAL A 0(ZERO) OU 1(UM) O DIGITO DEVERÁ SER 1(UM), CASO CONTRÁRIO, O DIGITO SERÁ A DIFERENÇA ENTRE 11 E O RESTO.
            if (_DAC) {
                if (Resto == 1 || Resto == 0) {
                    Retorno = "1";
                }
                else {
                    Retorno = Convert.ToString(11 - Resto);
                }
            }

            return Retorno;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            var Agencia = boleto.Cedente.ContaBancaria.Agencia;
            var Modalidade = boleto.TipoModalidade;
            var Carteira = boleto.Carteira;
            var NossoNumero = Convert.ToInt16(boleto.NossoNumero).ToString().PadLeft(6, '0');

            // 230XXXXXXXD
            // 230 = Fixo - Número da Faixa cadastrada para o contrato da empresa 
            // XXXXXXX - Livre para preenchimento da empresa, variando de 0000001 até 9999999
            // D - Digito Verificador, conforme fórmula de cálculo disponível no lay out.

            // Obs: agencia utilizada apenas para o calculo do digito verificador [NAO EXIBE NO BOLETO]

            boleto.NossoNumero = Modalidade + Carteira + NossoNumero + 
                                 MOD11_Mercantil(Agencia + Modalidade + Carteira + NossoNumero, true, false);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {

        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (Convert.ToInt64(boleto.NossoNumero).ToString().Length < 11)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO";

            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);            
        }

        #region Gera Arquivo Remessa

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool retorno = true;
            string msg = string.Empty;

            switch (tipoArquivo) {
                case TipoArquivo.CNAB400:
                    retorno = ValidarRemessaCNAB400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out msg);

                    break;
                default:
                    throw new Exception($"O CNAB {tipoArquivo} não foi implementado.");
            }
            
            mensagem = msg;
            return retorno;
        }
                
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            var _header = string.Empty;

            base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

            switch (tipoArquivo) {   
                case TipoArquivo.CNAB400:
                    _header = GerarHeaderRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                    break;                  
                default:
                    throw new Exception($"O CNAB {tipoArquivo} não foi implementado.");
            }

            return _header;
        }

        public new string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            var _detalhe = string.Empty;

            base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB400:
                    _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                    break;
                default:
                    throw new Exception($"O CNAB {tipoArquivo} não foi implementado.");
            }

            return _detalhe;
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            var _trailer = string.Empty;

            base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB400:
                    _trailer = GerarTrailerRemessa400(numeroRegistro, 0);
                    break;
                default:
                    throw new Exception($"O CNAB {tipoArquivo} não foi implementado.");
            }

            return _trailer;
        }

        #region CNAB 400

        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool retorno = true;
            string msg = string.Empty;

            #region Pré Validações
            if (banco == null)
            {
                msg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                retorno = false;
            }
            if (cedente == null)
            {
                msg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                retorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                msg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                retorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Sacado == null)
                {
                    msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Sacado: Informe os dados do sacado!", Environment.NewLine);
                    retorno = false;
                }
                else
                {
                    if (boleto.Sacado.Nome == null)
                    {
                        msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Nome: Informe o nome do sacado!", Environment.NewLine);
                        retorno = false;
                    }

                    if (boleto.Sacado.CPFCNPJ == null || boleto.Sacado.CPFCNPJ == "")
                    {
                        msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; CPF/CNPJ: Informe o CPF ou CNPJ do sacado!", Environment.NewLine);
                        retorno = false;
                    }

                    if (boleto.Sacado.Endereco == null)
                    {
                        msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o endereço do sacado!", Environment.NewLine);
                        retorno = false;
                    }
                    else
                    {
                        if (boleto.Sacado.Endereco.End == null || boleto.Sacado.Endereco.End == "")
                        {
                            msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o Endereço do sacado!", Environment.NewLine);
                            retorno = false;
                        }
                        if (boleto.Sacado.Endereco.Bairro == null || boleto.Sacado.Endereco.Bairro == "")
                        {
                            msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o Bairro do sacado!", Environment.NewLine);
                            retorno = false;
                        }
                        if (boleto.Sacado.Endereco.CEP == null || boleto.Sacado.Endereco.CEP == "" || boleto.Sacado.Endereco.CEP == "00000000")
                        {
                            msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o CEP do sacado!", Environment.NewLine);
                            retorno = false;
                        }
                        if (boleto.Sacado.Endereco.Cidade == null || boleto.Sacado.Endereco.Cidade == "")
                        {
                            msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe a cidade do sacado!", Environment.NewLine);
                            retorno = false;
                        }
                        if (boleto.Sacado.Endereco.UF == null || boleto.Sacado.Endereco.UF == "")
                        {
                            msg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe a UF do sacado!", Environment.NewLine);
                            retorno = false;
                        }
                    }
                }

                #endregion
            }
            //
            mensagem = msg;
            return retorno;
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {

            var reg = new TRegistroEDI();

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                           //001-001
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                           //002-002
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                     //003-009
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                          //010-011
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                    //012-026
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 004, 0, cedente.ContaBancaria.Agencia, ' ')); //027-030
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0031, 015, 0, cedente.CPFCNPJ, '0'));               //031-045
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0046, 001, 0, ' ', ' '));                           //046-046
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));        //047-076
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 003, 0, "389", ' '));                         //077-079
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, "BANCO MERCANTIL", ' '));             //080-094
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                  //095-100
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 281, 0, "", ' '));                            //101-381
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0382, 008, 0, "01600   ", ' '));                    //382-389
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0390, 005, 0, numeroArquivoRemessa, '0'));          //390-394
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0395, 006, 0, "000001", ' '));                      //395-400
            //
            reg.CodificarLinha();
            //
            string vLinha = reg.LinhaRegistro;
            string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
            //
            return _header;
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {                                  
            var reg = new TRegistroEDI();

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));                           //001-001

            if (boleto.PercMulta > 0) {
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, "09", '0'));                      //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 001, 0, "2", '0'));                       //004-004
            }
            else {
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, "00", '0'));                      //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 001, 0, "0", '0'));                       //004-004
            }

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0005, 013, 2, 5, '0'));                             //005-017

            #region Data da Multa        
            // Seta a data da multa para um dia apos o vencimento do boleto
            if (boleto.PercMulta > 0) {                
                if (boleto.DataMulta == DateTime.MinValue) {
                    boleto.DataMulta = boleto.DataVencimento.AddDays(1);
                }
            }
            #endregion

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0018, 006, 0, boleto.DataMulta, ' '));                     //018-023
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0024, 005, 0, string.Empty, ' '));                  //024-028

            #region Codigo Contrato
            var codigoContrato = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
            #endregion

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0029, 009, 0, codigoContrato, ' '));                //029-037
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0038, 025, 0, boleto.NumeroDocumento, ' '));        //038-062
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));  //063-066
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0067, 011, 0, boleto.NossoNumero, '0'));            //067-077
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0078, 005, 0, string.Empty, ' '));                  //078-082
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0083, 015, 0, boleto.Cedente.CPFCNPJ, '0'));        //083-097
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0098, 010, 2, boleto.ValorBoleto, '0'));            //083-097
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 001, 0, "1", '0'));                           //108-108
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, "01", '0'));                          //109-110
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));        //111-120
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));         //121-126
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));            //127-139
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "389", '0'));                         //140-142                  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0143, 005, 0, "00000", '0'));                       //143-147  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0148, 002, 0, "01", '0'));                          //148-149  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite.Substring(0,1).ToUpper(), '0')); //150-151
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));          //151-156
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0157, 002, 0, "00", '0'));                          //157-158
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0159, 002, 0, "00", '0'));                          //159-160
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));              //161-173
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0174, 006, 0, boleto.DataDesconto, ' '));           //174-179
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 0, boleto.ValorDesconto, '0'));          //180-192
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 0, boleto.IOF, '0'));                    //193-205
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 0, boleto.Abatimento, '0'));             //206-218

            #region Tipo de Sacado
            var tipoSacado = string.Empty;

            if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) tipoSacado = "01";
            else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) tipoSacado = "02";
            else tipoSacado = "99";
            #endregion

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0219, 002, 0, tipoSacado, '0'));                    //219-220
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));         //221-234
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));  //235-274

            #region Endereco + Numero + Complemento
            var enderecoNumeroCompl = ((string.IsNullOrEmpty(boleto.Sacado.Endereco.End)) ? string.Empty : boleto.Sacado.Endereco.End);
            enderecoNumeroCompl += ((string.IsNullOrEmpty(boleto.Sacado.Endereco.Numero)) ? string.Empty : boleto.Sacado.Endereco.Numero);
            enderecoNumeroCompl += ((string.IsNullOrEmpty(boleto.Sacado.Endereco.Complemento)) ? string.Empty : boleto.Sacado.Endereco.Complemento);
            #endregion

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, enderecoNumeroCompl.ToUpper(), ' ')); //275-314
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' ')); //315-326
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 008, 0, boleto.Sacado.Endereco.CEP, ' '));    //327-334
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' ')); //335-349
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));     //350-351
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 030, 0, boleto.Avalista, ' '));               //352-381
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0382, 012, 0, string.Empty, ' '));                  //382-393
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, "1", ' '));                           //394-394
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                //395-400
            
            //
            reg.CodificarLinha();
            //
            string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            //
            return _detalhe;
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            var reg = new TRegistroEDI();

            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-394
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400

            //
            reg.CodificarLinha();
            //
            string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            //
            return _detalhe;
        }
        #endregion


        #endregion
    }
}
