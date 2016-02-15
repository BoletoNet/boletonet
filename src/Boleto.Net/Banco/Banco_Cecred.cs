using System;
using System.Web.UI;
using BoletoNet;
using System.Text;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Images.085.jpg", "image/jpg")]

namespace BoletoNet {

    /// <summary>
    /// Classe referente a CECRED
    /// VIACRED, ACREDI, CREDIFIESC, CECRISACRED, CREDELESC
    /// TRANSPOCRED, CREDIFOZ, CREDCREA, SCRCRED, RODOCREDITO
    /// CREDICOMIN, CREVISC, VIACREDI(ALTOVALE), TRANSULCRED
    /// </summary>
    internal class Banco_Cecred : AbstractBanco, IBanco {

        internal Banco_Cecred() {
            this.Codigo = 085;
            this.Digito = "1";  // TODO verificar digito banco cecred
            this.Nome = "CECRED";
        }

        /// <summary>
        /// 
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$)
        ///    05 a 05 -  1 - Dígito verificador do Código de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 25 -  6 - Nº convenio da cooperativa
        ///    26 a 33 -  8 - Nº Conta corrente
        ///    34 - 42 - 9 -  Número boleto
        ///    43 - 44 - 2 - Código da carteira
        ///    
        /// </summary>
        /// <param name="boleto"></param>
        void IBanco.FormataCodigoBarra(Boleto boleto) {

            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);
            string codigoMoeda = "9";
            string fatorVencimento = FatorVencimento(boleto).ToString();
            string valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            string numeroConvenio = Utils.FormatCode(boleto.Cedente.Codigo, 6);
            string numeroConta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 8);
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 9);
            string codigoCarteira = Utils.FormatCode(boleto.Carteira, 2);

            string parte1 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                codigoBanco, codigoMoeda, fatorVencimento.ToString(), valorNominal, numeroConvenio, numeroConta, nossoNumero, codigoCarteira);

            var digCodigoBarras = Calcula11(parte1).ToString();

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                codigoBanco, codigoMoeda, digCodigoBarras, fatorVencimento.ToString(), valorNominal, numeroConvenio, numeroConta, nossoNumero, codigoCarteira);

        }

        int Calcula11(string parte1) {
            int sum = 0;
            for (int i = parte1.Length - 1, multiplier = 2; i >= 0; i--) {
                sum += (int)char.GetNumericValue(parte1[i]) * multiplier;
                if (++multiplier > 9) multiplier = 2;
            }

            int mod = (sum % 11);
            if (mod == 0 || mod == 1) return 0;
            return (11 - mod);
        }

        void IBanco.FormataLinhaDigitavel(Boleto boleto) {

            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);
            string codigoMoeda = "9";
            string fatorVencimento = FatorVencimento(boleto).ToString();
            string valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            string numeroConvenio = Utils.FormatCode(boleto.Cedente.Codigo, 6);
            string numeroConta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 8);
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 9);
            string codigoCarteira = Utils.FormatCode(boleto.Carteira, 2);

            string parte1 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
               codigoBanco, codigoMoeda, fatorVencimento.ToString(), valorNominal, numeroConvenio, numeroConta, nossoNumero, codigoCarteira);

            var digCodigoBarras = Calcula11(parte1).ToString();

            string campo1, campo2, campo3, campo4, campo5;

            campo1 = codigoBanco + codigoMoeda + numeroConvenio.Substring(0, 5);
            campo1 = campo1 + Mod10(campo1).ToString();

            campo2 = numeroConvenio.Substring(numeroConvenio.Length - 1, 1) + numeroConta + nossoNumero.Substring(0, 1);
            campo2 = campo2 + Mod10(campo2).ToString();

            campo3 = nossoNumero.Substring(1, nossoNumero.Length - 1) + codigoCarteira;
            campo3 = campo3 + Mod10(campo3).ToString();

            campo4 = digCodigoBarras;

            campo5 = fatorVencimento + valorNominal;

            string format = "XXXXX.XXXXX XXXXX.XXXXXX XXXXX.XXXXXX X XXXXXXXXXXXXXX";
            string linhaDig = string.Format("{0}{1}{2}{3}{4}", campo1, campo2, campo3, campo4, campo5);
            boleto.CodigoBarra.LinhaDigitavel = Utils.Transform(linhaDig, format);

        }

        void IBanco.FormataNossoNumero(Boleto boleto) {
            string numeroConta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 8);
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 9);

            boleto.NossoNumero = string.Format("{0}{1}", numeroConta, nossoNumero);
        }

        void IBanco.FormataNumeroDocumento(Boleto boleto) {
            throw new NotImplementedException("Função não implementada.");
        }

        void IBanco.ValidaBoleto(Boleto boleto) {
            //throw new NotImplementedException("Função não implementada.");
            if ((boleto.Carteira != "01"))
                throw new NotImplementedException("Carteira não implementada.");

            if (boleto.NossoNumero.Length != 9)
                throw new NotSupportedException("Nosso Número deve ter 9 posições para o banco 085.");

            boleto.LocalPagamento = "PAGAVEL PREFERENCIALMENTE NAS COOPERATIVAS DO SISTEMA CECRED. APOS VENCIMENTO PAGAR SOMENTE NA COOPERATIVA";

            //if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
            //    boleto.EspecieDocumento = new EspecieDocumento_Santander("2");

            boleto.FormataCampos();
        }

        #region Arquivo Remessa

        #region HeaderRemessaCNAB400
        string IBanco.GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {
            try {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo) {
                    //case TipoArquivo.CNAB240:
                    //    _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                    //    break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }
        //string IBanco.GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos) {
        //    throw new NotImplementedException();
        //}
        //string IBanco.GerarHeaderRemessa(Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {
        //    throw new NotImplementedException();
        //}

        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa) {
            try {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));                                   //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));                                   //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                             //003-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                  //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 008, 0, "COBRANCA", ' '));                            //012-019
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 007, 0, string.Empty, ' '));                          //020-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 004, 0, cedente.ContaBancaria.Agencia, '0'));         //027-030
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, cedente.ContaBancaria.DigitoAgencia, ' '));   //031-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 008, 0, cedente.ContaBancaria.Conta, '0'));           //032-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 001, 0, cedente.ContaBancaria.DigitoConta, ' '));     //040-040
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0041, 006, 0, "000000", '0'));                              //041-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));                //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 018, 0, "085CECRED", ' '));                           //077-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                          //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 007, 0, numeroArquivoRemessa, '0'));                  //101-107
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 022, 0, string.Empty, ' '));                          //108-129
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0130, 007, 0, cedente.Convenio.ToString(), '0'));           //130-136
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0137, 258, 0, string.Empty, ' '));                          //137-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                              //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _header;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion HeaderRemessaCNAB400

        #region DetalheRemessaCNAB400
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo) {
                    //case TipoArquivo.CNAB240:
                    //    _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                    //    break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {

                /*
                 * Código de Movimento Remessa 
                 * 01 - Registro de títulos;  
                 * 02 - Solicitação de baixa; 
                 * 04 - Concessão de abatimento;  
                 * 05 - Cancelamento de abatimento;  
                 * 06 - Alteração de vencimento de título;  
                 * 09 - Instruções para protestar (Nota 09);   
                 * 10 - Instrução para sustar protesto;  
                 * 12 - Alteração de nome e endereço do Pagador;  
                 * 17 – Liquidação de título não registro ou pagamento em duplicidade; 
                 * 31 - Conceder desconto; 
                 * 32 - Não conceder desconto. 
                 */

                //if (string.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia)) {
                boleto.Remessa.CodigoOcorrencia = "01";
                //}

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string tipoInscricaoEmitente = "02";                                        // Padrão CNPJ
                string tipoInscricaoSacado = "02";                                          // Padrão CNPJ
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11))
                    tipoInscricaoEmitente = "01"; // CPF

                if (boleto.Sacado.CPFCNPJ.Length.Equals(11))
                    tipoInscricaoSacado = "01"; // CPF
                else if (string.IsNullOrEmpty(boleto.Sacado.CPFCNPJ))
                    tipoInscricaoSacado = "00"; // ISENTO

                string _nossoNumero = string.Format("{0}{1}",
                        Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 8),
                        Utils.FormatCode(boleto.NossoNumero, 9));

                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "7", '0'));                                       //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, tipoInscricaoEmitente, '0'));                     //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    //004-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //018-021
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 001, 0, boleto.Cedente.ContaBancaria.DigitoAgencia, ' '));//022-022
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0023, 008, 0, boleto.Cedente.ContaBancaria.Conta, '0'));        //023-030
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, boleto.Cedente.ContaBancaria.DigitoConta, ' '));  //031-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 007, 0, boleto.Cedente.Convenio, '0'));                   //032-038
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0039, 025, 0, boleto.NumeroDocumento, ' '));                    //039-063
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0064, 017, 0, _nossoNumero, '0'));                              //064-080
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0081, 002, 0, "00", '0'));                                      //081-082
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0083, 002, 0, "00", '0'));                                      //083-084
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0085, 003, 0, string.Empty, ' '));                              //085-087
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0088, 001, 0, string.Empty, ' '));                              //088-088
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0089, 003, 0, string.Empty, ' '));                              //089-091
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0092, 003, 0, boleto.VariacaoCarteira, '0'));                   //092-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0095, 001, 0, "0", '0'));                                       //095-095
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0096, 006, 0, "0", '0'));                                       //096-101
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0102, 005, 0, string.Empty, ' '));                              //102-106
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, boleto.Carteira, '0'));                           //107-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0111, 010, 0, boleto.NumeroDocumento, '0'));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "085", '0'));                                     //140-142   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 004, 0, "0000", '0'));                                    //143-146
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 001, 0, string.Empty, ' '));                              //147-147 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0148, 002, 0, boleto.Especie, '0'));                            //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156

                #region Instruções
                string vInstrucao1 = "00";
                if (boleto.Instrucoes.Count > 0) {
                    vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, "00", '0'));                                      //159-160

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));                          //161-173
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, "000000", '0'));                                  //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 0, "000000", '0'));                                  //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, tipoInscricaoSacado, '0'));                       //219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 037, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-271
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0272, 003, 0, string.Empty, ' '));                              //272-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //315-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 040, 0, string.Empty, ' '));                              //352-391
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 002, 0, string.Empty, ' '));                              //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, string.Empty, ' '));                              //394-394                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400

                reg.CodificarLinha();

                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);

                return _detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        #endregion

        string IBanco.GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal) {
            StringBuilder _trailer = new StringBuilder();
            try {
                //Montagem trailer
                _trailer.Append("9"); //Posição 001
                _trailer.Append(new string(' ', 393)); //Posição 002 a 394
                _trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 395 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_trailer.ToString());
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar TRAILER do arquivo de remessa do CNAB400.", ex);
            }
        }

        string IBanco.GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo) {
            try {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cendente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo) {
                    //case TipoArquivo.CNAB240:
                    //    header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cendente, numeroArquivoRemessa);
                    //    break;
                    case TipoArquivo.CNAB400:
                        header = "";
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        //string IBanco.GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente, Boleto boletos) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarTrailerArquivoRemessa(int numeroRegistro) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarTrailerLoteRemessa(int numeroRegistro) {
        //    throw new NotImplementedException();
        //}

        //string IBanco.GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos) {
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Arquivo Retorno
        //DetalheSegmentoTRetornoCNAB240 IBanco.LerDetalheSegmentoTRetornoCNAB240(string registro) {
        //    throw new NotImplementedException();
        //}

        //DetalheSegmentoURetornoCNAB240 IBanco.LerDetalheSegmentoURetornoCNAB240(string registro) {
        //    throw new NotImplementedException();
        //}

        //DetalheSegmentoWRetornoCNAB240 IBanco.LerDetalheSegmentoWRetornoCNAB240(string registro) {
        //    throw new NotImplementedException();
        //}

        DetalheRetorno IBanco.LerDetalheRetornoCNAB400(string registro) {
            try {

                TRegistroEDI_Cecred_Retorno reg = new TRegistroEDI_Cecred_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                //detalhe. = reg.Identificacao;
                //detalhe. = reg.Zeros1;
                //detalhe. = reg.Zeros2;
                detalhe.Agencia = Utils.ToInt32(String.Concat(reg.PrefixoAgencia, reg.DVPrefixoAgencia));
                detalhe.Conta = Utils.ToInt32(reg.ContaCorrente);
                detalhe.DACConta = Utils.ToInt32(reg.DVContaCorrente);
                //detalhe. = reg.NumeroConvenioCobranca;
                //detalhe. = reg.NumeroControleParticipante;
                //
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV
                //
                //detalhe. = reg.TipoCobranca;
                //detalhe. = reg.TipoCobrancaEspecifico;
                //detalhe. = reg.DiasCalculo;
                //detalhe. = reg.NaturezaRecebimento;
                //detalhe. = reg.PrefixoTitulo;
                //detalhe. = reg.VariacaoCarteira;
                //detalhe. = reg.ContaCaucao;
                //detalhe. = reg.TaxaDesconto;
                //detalhe. = reg.TaxaIOF;
                //detalhe. = reg.Brancos1;
                detalhe.Carteira = reg.Carteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.Comando);
                //
                int dataLiquidacao = Utils.ToInt32(reg.DataLiquidacao);
                detalhe.DataLiquidacao = Utils.ToDateTime(dataLiquidacao.ToString("##-##-##"));
                //
                detalhe.NumeroDocumento = reg.NumeroTituloCedente;
                //detalhe. = reg.Brancos2;
                //
                int dataVencimento = Utils.ToInt32(reg.DataVencimento);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                //
                detalhe.ValorTitulo = (Convert.ToInt64(reg.ValorTitulo) / 100);
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoRecebedor);
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.PrefixoAgenciaRecebedora);
                //detalhe. = reg.DVPrefixoRecebedora;
                detalhe.Especie = Utils.ToInt32(reg.EspecieTitulo);
                //
                int dataCredito = Utils.ToInt32(reg.DataCredito);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //
                detalhe.TarifaCobranca = (Convert.ToInt64(reg.ValorTarifa) / 100);
                detalhe.OutrasDespesas = (Convert.ToInt64(reg.OutrasDespesas) / 100);
                detalhe.ValorOutrasDespesas = (Convert.ToInt64(reg.JurosDesconto) / 100);
                detalhe.IOF = (Convert.ToInt64(reg.IOFDesconto) / 100);
                detalhe.Abatimentos = (Convert.ToInt64(reg.ValorAbatimento) / 100);
                detalhe.Descontos = (Convert.ToInt64(reg.DescontoConcedido) / 100);
                detalhe.ValorPrincipal = (Convert.ToInt64(reg.ValorRecebido) / 100);
                detalhe.JurosMora = (Convert.ToInt64(reg.JurosMora) / 100);
                detalhe.OutrosCreditos = (Convert.ToInt64(reg.OutrosRecebimentos) / 100);
                //detalhe. = reg.AbatimentoNaoAproveitado;
                detalhe.ValorPago = (Convert.ToInt64(reg.ValorLancamento) / 100);
                //detalhe. = reg.IndicativoDebitoCredito;
                //detalhe. = reg.IndicadorValor;
                //detalhe. = reg.ValorAjuste;
                //detalhe. = reg.Brancos3;
                //detalhe. = reg.Brancos4;
                //detalhe. = reg.Zeros3;
                //detalhe. = reg.Zeros4;
                //detalhe. = reg.Zeros5;
                //detalhe. = reg.Zeros6;
                //detalhe. = reg.Zeros7;
                //detalhe. = reg.Zeros8;
                //detalhe. = reg.Brancos5;
                //detalhe. = reg.CanalPagamento;
                //detalhe. = reg.NumeroSequenciaRegistro;
                #region NAO RETORNADOS PELO BANCO
                detalhe.MotivoCodigoOcorrencia = string.Empty;
                detalhe.MotivosRejeicao = string.Empty;
                detalhe.NumeroCartorio = 0;
                detalhe.NumeroProtocolo = string.Empty;
                detalhe.NomeSacado = string.Empty;
                #endregion

                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarRegistroDetalhe5(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            StringBuilder detalhe = new StringBuilder();
            switch (tipoArquivo) {
                case TipoArquivo.CNAB400:

                    detalhe.Append("5");                                        // 001
                    detalhe.Append("99");                                       // 002-003
                    detalhe.Append("2");                                        // 004 (Percentual)
                    detalhe.Append(boleto.DataMulta.ToString("ddMMyy"));        // 005-010
                    detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 100).ToString(), 12, 12, '0', 1, true, true, true)); // 011-022
                    detalhe.Append(new string(' ', 372));                       // 023 a 394
                    detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); // 395 a 400

                    //Retorno
                    return Utils.SubstituiCaracteresEspeciais(detalhe.ToString());
                default:
                    throw new Exception("Tipo de arquivo não suportado.");
            }
        }
    }
}
