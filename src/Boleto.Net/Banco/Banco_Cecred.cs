using System;
using System.Web.UI;
using BoletoNet;
using System.Text;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.085.jpg", "image/jpg")]

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
            boleto.NossoNumero = GetFormatedNossoNumero(boleto);
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

            boleto.LocalPagamento = "Pagar preferencialmente nas cooperativas do Sistema Ailos.";

            //if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
            //    boleto.EspecieDocumento = new EspecieDocumento_Santander("2");

            boleto.FormataCampos();
        }


        #region Métodos internos
        int Calcula11(string parte1) {
            int sum = 0;
            for (int i = parte1.Length - 1, multiplier = 2; i >= 0; i--) {
                sum += (int)char.GetNumericValue(parte1[i]) * multiplier;
                if (++multiplier > 9) multiplier = 2;
            }

            int mod = (sum % 11);

            // Conta data de vencimento 04/04/2016 estava retornando 0 e deveria ser 1
            // 08590.10126 00777.296302 00000.006015 5 67550000013990 VCTO 05/04/2016
            // 08590.10126 00777.296302 00000.006015 0 67540000013990 VCTO 04/04/2016 erro
            if (mod == 0 || mod == 1) return 1;

            return (11 - mod);
        }
        internal static string Mod11Cecred(string value) {
            #region Trecho do manual DVMD11.doc
            /* 
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 9 a 2.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 9, o segundo por 8 e assim sucessivamente.
            O resultados das multiplicações devem ser somados:
            72+35+24+27+4+9+8=179
            O total da soma deverá ser dividido por 11:
            179 / 11=16
            RESTO=3

            Se o resto da divisão for igual a 10 o D.V. será igual a X. 
            Se o resto da divisão for igual a 0 o D.V. será igual a 0.
            Se o resto for menor que 10, o D.V.  será igual ao resto.

            No exemplo acima, o dígito verificador será igual a 3
            */
            #endregion

            /* d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            string d;
            int s = 0, p = 9, b = 2;

            for (int i = value.Length - 1; i >= 0; i--) {
                s += (int.Parse(value[i].ToString()) * p);
                if (p == b)
                    p = 9;
                else
                    p--;
            }

            int r = (s % 11);
            if (r == 10)
                d = "X";
            else if (r == 0)
                d = "0";
            else
                d = r.ToString();

            return d;
        }

        internal static string GetFormatedNossoNumero(Boleto boleto) {
            string _nossoNumero = string.Format("{0}{1}",
                Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 8),
                Utils.FormatCode(boleto.NossoNumero, 9));
            return _nossoNumero;
        }
        #endregion


        #region Métodos de geração do arquivo remessa - Genéricos
        /// <summary>
        /// HEADER DE LOTE do arquivo CNAB
        /// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo) {
            try {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
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
        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {
            try {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
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
        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo) {
                case TipoArquivo.CNAB240:
                    vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.CNAB400:
                    vRetorno = ValidarRemessaCNAB400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                        break;
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

        // Específico 240 (P, Q, R, S)
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio) {
            return GerarDetalheSegmentoPRemessaCNAB240(boleto, numeroRegistro);
        }
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            return GerarDetalheSegmentoQRemessaCNAB240(boleto, numeroRegistro);
        }
        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            return GerarDetalheSegmentoRRemessaCNAB240(boleto, numeroRegistro);
        }
        //

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal) {
            try {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessaCNAB240(numeroRegistro);
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro, 0);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do TRAILER do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarTrailerLoteRemessa(int numeroRegistro) {
            return GerarTrailerLoteRemessaCNAB240(numeroRegistro);
        }
        public override string GerarTrailerArquivoRemessa(int numeroRegistro) {
            return GerarTrailerRemessaCNAB240(numeroRegistro);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos) {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion


        #region Arquivo Remessa 240
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            string vMsg = string.Empty;
            mensagem = vMsg;
            return true;
            //throw new NotImplementedException("Função não implementada.");
        }

        /*
         * Tipo de registro
         * '0'  =  Header de Arquivo
         * '1'  =  Header de Lote
         * '2'  =  Registros Iniciais do Lote
         * '3'  =  Detalhe
         * '4'  =  Registros Finais do Lote
         * '5'  =  Trailer de Lote
         * '9'  =  Trailer de Arquivo
         */

        /// <summary>
        /// Header de Arquivo (Tipo = 0)
        /// </summary>
        /// <param name="cedente"></param>
        /// <param name="numeroArquivoRemessa"></param>
        /// <returns></returns>
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa) {
            try {
                string _header;

                // Controle
                _header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);           // 001-003 Código do banco na Compensação
                _header += "0000";                                                     // 004-007 Lote de Serviço
                _header += "0";                                                        // 000-008 Tipo de Registro

                // CNAB
                _header += new string(' ', 9); // 009-017 Febraban Brancos

                // Empresa (Inscrição)
                _header += "2";                                                                                         // 018-018 Tipo de inscrição da empresa 1=CPF 2=CNPJ
                _header += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);                    // 019-032 Número de inscrição da empresa
                // Empresa (Convênio)
                _header += Utils.FitStringLength(cedente.Convenio.ToString(), 20, 20, ' ', 0, true, true, false);                    // 033-052 Código do convêncio no banco
                // Empresa (Conta Corrente)
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);        // 053-057 Agência da cede da cooperativa
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);  // 058-058 Digito agência
                _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);        // 059-070 Número da conta Corrente
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);    // 071-071 Digito da Conta
                _header += " ";                                                                                         // 072-072 Digito Ag/Conta
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                      // 073-102 Nome da Empresa


                // Nome da Cooperativa
                _header += Utils.FitStringLength(Nome, 30, 30, ' ', 0, true, true, false);                          // 103-132 Nome da cooperativa

                // CNAB
                _header += new string(' ', 10); // 133-142 Febraban Brancos

                // Arquivo
                _header += "1";                                                                                     // 143-143 Código Remessa / Retorno
                _header += DateTime.Now.ToString("ddMMyyyy");                                                       // 144-151 Data de Geração do Arquivo
                _header += DateTime.Now.ToString("hhMMss");                                                         // 152-157 Hora de Geração do Arquivo
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 6, '0', 0, true, true, true);  // 158-163 Número Sequencial do Arquivo
                _header += "087";                                                                                   // 164-166 No da Versão do Layout do Arquivo
                _header += new string('0', 5);                                                                      // 167-171 Densidade de Gravação do Arquivo

                // Reservados
                _header += new string(' ', 20); // 172-191 Para Uso Reservado da Coop.
                _header += new string(' ', 20); // 192-211 Para Uso Reservado da Empresa
                _header += new string(' ', 29); // 212-240 Uso Exclusivo FEBRABAN / CNAB

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do HEADER DE ARQUIVO do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// Header de Lote (tipo = 1)
        /// </summary>
        /// <param name="numeroConvenio"></param>
        /// <param name="cedente"></param>
        /// <param name="numeroArquivoRemessa"></param>
        /// <returns></returns>
        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa) {
            try {
                string _headerLote;

                // Controle
                _headerLote = Utils.FormatCode(Codigo.ToString(), "0", 3, true);           // 01 001-003 Código do banco na Compensação
                _headerLote += "0001";                                                     // 02 004-007 Lote de Serviço
                _headerLote += "1";                                                        // 03 000-008 Tipo de Registro

                // Serviço
                _headerLote += new string('R', 1);  // 04 009-009 Tipo de Operação
                _headerLote += "01";                // 05 010-011 Tipo de Serviço
                _headerLote += new string(' ', 2);  // 06 012-013 Uso Exclusivo FEBRABAN/CNAB
                _headerLote += "045";               // 07 014-016 Layout do Lote


                _headerLote += new string(' ', 1); // 08 017-017 Febraban Brancos

                // Empresa (Inscrição)
                _headerLote += "2";                                                                                         // 09 018-018 Tipo de inscrição da empresa 1=CPF 2=CNPJ
                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true);                    // 10 019-033 Número de inscrição da empresa
                // Empresa (Convênio)
                _headerLote += Utils.FitStringLength(numeroConvenio, 20, 20, ' ', 0, true, true, false);                    // 11 034-053 Código do convêncio no banco
                // Empresa (Conta Corrente)
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);        // 12 054-058 Agência da cede da cooperativa
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);  // 13 059-059 Digito agência
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);        // 14 060-071 Número da conta Corrente
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);    // 15 072-072 Digito da Conta
                _headerLote += " ";                                                                                         // 16 073-073 Digot Ag/Conta
                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                      // 17 074-103 Nome da Empresa

                _headerLote += new string(' ', 40); // 18 104-143 Mensagem 1
                _headerLote += new string(' ', 40); // 19 144-183 Mensagem 2

                // Controle da Cobrança
                _headerLote += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 8, 8, '0', 0, true, true, true);  // 20 184-191 Número Remessa/Retorno
                _headerLote += DateTime.Now.ToString("ddMMyyyy");                                                       // 21 192-199 Data de Gravação Remessa/Retorno

                _headerLote += new string('0', 8); // 22 200-207 Data do Crédito

                // CNAB
                _headerLote += new string(' ', 33); // 23 208-240 Febraban Brancos

                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);

                return _headerLote;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// Detalhe (tipo = 3)
        /// </summary>
        /// <param name="boleto"></param>
        /// <param name="numeroRegistro"></param>
        /// <param name="tipoArquivo"></param>
        /// <returns></returns>
        private string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            throw new NotImplementedException("Função não implementada.");
        }

        //
        #region P, Q, R, S
        private string GerarDetalheSegmentoPRemessaCNAB240(Boleto boleto, int numeroRegistro) {
            try {
                #region Segmento P
                //validaInstrucoes240(boleto);
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 001-003 (003) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                           // posição 004-007 (004) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                           // posição 008-008 (001) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                                // posição 009-013 (005) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "P", '0'));                                           // posição 014-014 (001) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                  // posição 015-015 (001) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, "01", '0'));                                          // posição 016-017 (002) - Código de Movimento Remessa ('01'  =  Entrada de Títulos)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 005, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));          // posição 018-022 (005) - Agência Mantenedora da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0023, 001, 0, boleto.Cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posição 023-023 (001) - Dígito Verificador da Agência
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0024, 012, 0, boleto.Cedente.ContaBancaria.Conta, '0'));            // posição 024-035 (012) - Número da Conta Corrente
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0036, 001, 0, boleto.Cedente.ContaBancaria.DigitoConta, '0'));      // posição 036-036 (001) - Dígito Verificador da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0037, 001, 0, string.Empty, ' '));                                  // posição 037-037 (001) - Dígito Verificador da Ag/Conta

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 020, 0, GetFormatedNossoNumero(boleto), ' '));                // posição 038-057 (020) - Identificação do Título no Banco

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0058, 001, 0, "1", '0'));                                           // posição 058-058 (001) - Código Carteira '1'  =  Cobrança Simples
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0059, 001, 0, "1", '0'));                                           // posição 059-059 (001) - Forma de Cadastr. do Título no Banco '1'  =  Com Cadastramento (Cobrança Registrada)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0060, 001, 0, "1", '0'));                                           // posição 060-060 (001) - Tipo de Documento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0061, 001, 0, "2", '0'));                                           // posição 061-061 (001) - Identificação da Emissão do Bloqueto (1=Banco Emite, 2=Cliente Emite)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0062, 001, 0, "2", '0'));                                           // posição 062-062 (001) - Identificação da Distribuição (1=Banco Distribui, '2' = Cliente Distribui, 3=Banco envia e-mail, 4=Banco envia SMS)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 015, 0, boleto.NumeroDocumento, ' '));                        // posição 063-073 (015) - Número do Documento de Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0078, 008, 0, boleto.DataVencimento, ' '));                         // posição 078-085 (008) - Data de Vencimento do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0086, 015, 2, boleto.ValorBoleto, '0'));                            // posição 086-100 (015)- Valor Nominal do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 005, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));          // posição 101-105 (005) - AEC = Agência Encarregada da Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0106, 001, 0, boleto.Cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posição 106-106 (001) - Dígito Verificador da Agência
                string EspDoc = boleto.EspecieDocumento.Sigla.Equals("DM") ? "02" : boleto.EspecieDocumento.Codigo;
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, EspDoc, '0'));                                        // posição 107-108 (002) - Espécie do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 001, 0, boleto.Aceite, ' '));                                 // posição 109-109 (001) - Identific. de Título Aceito/Não Aceito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0110, 008, 0, boleto.DataDocumento, '0'));                          // posição 110-117 (008) - Data da Emissão do Título

                #region Código de juros
                string CodJurosMora;
                if (boleto.JurosMora == 0 && boleto.PercJurosMora == 0)
                    CodJurosMora = "3"; //  Isento
                else
                    CodJurosMora = "1"; // Valor por Dia
                #endregion

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0118, 001, 0, CodJurosMora, '0'));                                  // posição 118-118 (001) - Código do Juros de Mora
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0119, 008, 0, boleto.DataJurosMora, '0'));                          // posição 119-126 (008) - Data do Juros de Mora
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 015, 2, boleto.JurosMora, '0'));                              // posição 127-141 (015)- Juros de Mora por Dia/Taxa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0142, 001, 0, "0", '0'));                                           // posição 142-142 (001) -  Código do Desconto 1 - "0" = Sem desconto. "1"= Valor Fixo-a data informada "2" = Percentual-a data informada
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0143, 008, 0, boleto.DataDesconto, '0'));                           // posição 143-150 (008) - Data do Desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0151, 015, 2, boleto.ValorDesconto, '0'));                          // posição 151-165 (015)- Valor/Percentual a ser Concedido
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0166, 015, 2, boleto.IOF, '0'));                                    // posição 166-180 (015)- Valor do IOF a ser concedido
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0181, 015, 2, boleto.Abatimento, '0'));                             // posição 181-195 (015)- Valor do Abatimento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0196, 025, 0, boleto.NumeroDocumento, ' '));                        // posição 196-220 (025)- Identificação do Título na Empresa. Informar o Número do Documento - Seu Número (mesmo das posições 63-73 do Segmento P)                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 001, 0, "3", '0'));                                           // posição 221-221 (001) -  Código para protesto  - ‘1’ = Protestar. "3" = Não Protestar. "9" = Cancelamento Protesto Automático
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0222, 002, 0, "00", '0'));                                          // posição 222-223 (002) -  Número de Dias para Protesto                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0224, 001, 0, "2", '0'));                                           // posição 224-224 (001) -  Código para Baixa/Devolução ‘1’ = Baixar / Devolver. "2" = Não Baixar / Não Devolver
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0225, 003, 0, "   ", ' '));                                         // posição 225-227 (003) - Número de Dias para Baixa/Devolução
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0228, 002, 0, "09", '0'));                                          // posição 228-229 (002) - Código da Moeda. Informar fixo: ‘09’ = REAL
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0230, 010, 0, "0", '0'));                                           // posição 230-239 (010)- Nº do Contrato da Operação de Créd.
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0240, 001, 0, string.Empty, ' '));                                  // posição 240-240 (001) - Uso Exclusivo FEBRABAN/CNAB
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoP = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoP;
                #endregion
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do Segmento P no arquivo de remessa do CNAB240.", ex);
            }

        }
        private string GerarDetalheSegmentoQRemessaCNAB240(Boleto boleto, int numeroRegistro) {
            try {
                #region Segmento Q
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 001-003 (003) - código do banco na compensação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                           // posição 004-007 (004) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                           // posição 008-008 (001) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                                // posição 009-013 (005) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "Q", '0'));                                           // posição 014-014 (001) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                  // posição 015-015 (001) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, "01", '0'));                                          // posição 016-017 (002) - Código de Movimento Remessa
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "0";//não informado
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                   // posição 018-018 (001) - Tipo de Inscrição 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 015, 0, boleto.Sacado.CPFCNPJ, '0'));                         // posição 019-033 (015) - Número de Inscrição da empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0034, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));                  // posição 034-073 (040) - Nome
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));          // posição 74-0113 (040) - Endereço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0114, 015, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));       // posição 114-128 (015) - Bairro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0129, 008, 0, boleto.Sacado.Endereco.CEP, ' '));                    // posição 114-128 (008) - CEP                
                // posição 134-136 (3) - sufixo cep** já incluso em CEP                                                                                                                                                                   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0137, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));       // posição 137-151 (015) - Cidade
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0152, 002, 0, boleto.Sacado.Endereco.UF, ' '));                     // posição 152-153 (015) - UF
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0154, 001, 0, "0", '0'));                                           // posição 154-154 (001) - Tipo de Inscrição Sacador Avalialista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0155, 015, 0, "0", '0'));                                           // posição 155-169 (015) - Inscrição Sacador Avalialista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0170, 040, 0, string.Empty, ' '));                                  // posição 170-209 (040) - Nome do Sacador/Avalista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0210, 003, 0, "000", '0'));                                         // posição 210-212 (003) - Cód. Bco. Corresp. na Compensação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0213, 020, 0, string.Empty, ' '));                                  // posição 213-232 (020) - Nosso Nº no Banco Correspondente
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0233, 008, 0, string.Empty, ' '));                                  // posição 213-232 (008) - Uso Exclusivo FEBRABAN/CNAB
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoQ = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoQ;
                #endregion
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do Segmento Q no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }
        private string GerarDetalheSegmentoRRemessaCNAB240(Boleto boleto, int numeroRegistro) {
            try {
                #region Segmento R
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 001-003 (003) - código do banco na compensação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                           // posição 004-007 (004) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                           // posição 008-008 (001) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                                // posição 009-013 (005) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "R", '0'));                                           // posição 014-014 (001) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                  // posição 015-015 (001) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, "01", '0'));                                          // posição 016-017 (002) - Código de Movimento Remessa

                // Desconto 1
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, "0", '0'));                                           // posição 018-018 (001) - Código do desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 008, 0, "0", '0'));                                           // posição 019-026 (008) - Data do desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 015, 0, "0", '0'));                                           // posição 027-041 (015) - Valor do desconto

                // Desconto 2
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0042, 001, 0, "0", '0'));                                           // posição 042-042 (001) - Código do desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0043, 008, 0, "0", '0'));                                           // posição 043-050 (008) - Data do desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0051, 015, 0, "0", '0'));                                           // posição 051-065 (015) - Valor do desconto

                // Multa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0066, 001, 0, "2", ' '));                                           // posição 066-066 (001) - Código
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0067, 008, 0, boleto.DataMulta.ToString("ddMMyyyy"), '0'));                                           // posição 067-074 (008) - Data
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0075, 015, 0, Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 100).ToString(), 15, 15, '0', 1, true, true, true), '0'));                                           // posição 075-089 (015) - Valor

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0090, 010, 0, string.Empty, ' '));          // posição 090-099 (010) - Informação ao sacado
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0100, 040, 0, string.Empty, ' '));          // posição 100-139 (040) - Mensagem 3
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 040, 0, string.Empty, ' '));          // posição 140-179 (040) - Mensagem 4

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0180, 020, 0, string.Empty, ' '));          // posição 180-199 (020) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0200, 008, 0, "0", '0'));                   // posição 200-207 (008) - Cód. Ocor. do Sacado

                // Dados para débito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0208, 003, 0, "0", '0'));                   // posição 208-210 (003) - Cód. Banco conta debito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0211, 005, 0, "0", '0'));                   // posição 211-215 (005) - Cód. ag conta debito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0216, 001, 0, string.Empty, ' '));          // posição 216-216 (001) - Dig. ag conta debito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0217, 012, 0, "0", '0'));                   // posição 217-228 (012) - 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0229, 001, 0, string.Empty, ' '));          // posição 229-229 (001) - 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0230, 001, 0, string.Empty, ' '));          // posição 230-230 (001) - Dígito Verificador Ag/Conta

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0231, 001, 0, "0", '0'));                   // posição 231-231 (001) - Aviso para Débito Automático 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0232, 009, 0, string.Empty, ' '));          // posição 232-240 (009) - Uso Exclusivo FEBRABAN/CNAB 
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoR = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoR;
                #endregion
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        private string GerarDetalheSegmentoSRemessaCNAB240() {
            return string.Empty;
        }
        #endregion
        //

        /// <summary>
        /// Trailer do lote (tipo = 5)
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        private string GerarTrailerLoteRemessaCNAB240(int numeroRegistro) {
            try {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                // posição 001-003 (003) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                        // posição 004-007 (004) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "5", '0'));                        // posição 008-008 (001) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));               // posição 009-017 (009) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 006, 0, numeroRegistro, '0'));             // posição 018-023 (006) - Quantidade de Registros no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0024, 006, 0, "0", '0'));                        // posição 024-029 (006) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0030, 017, 2, "0", '0'));                        // posição 030-046 (017) - Valor Total dos Títulos em Carteiras
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0047, 006, 0, "0", '0'));                        // posição 047-052 (006) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0053, 017, 2, "0", '0'));                        // posição 053-069 (017) - Valor Total dos Títulos em Carteiras                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0070, 006, 0, "0", '0'));                        // posição 070-075 (006) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0076, 017, 2, "0", '0'));                        // posição 076-092 (017) - Quantidade de Títulos em Carteiras 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0093, 006, 0, "0", '0'));                        // posição 093-098 (006) - Quantidade de Títulos em Cobrança Descontadas
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0099, 017, 2, "0", '0'));                        // posição 099-115 (017) - Valor Total dosTítulos em Carteiras Descontadas
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0116, 008, 0, string.Empty, ' '));               // posição 116-123 (008) - Número do Aviso de Lançamento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0124, 117, 0, string.Empty, ' '));               // posição 124-240 (117) - Uso Exclusivo FEBRABAN/CNAB                
                reg.CodificarLinha();

                string vLinha = reg.LinhaRegistro;
                string _trailerLote = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _trailerLote;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar TRAILER do lote no arquivo de remessa do CNAB240.", ex);
            }
        }

        /// <summary>
        /// Trailer arquivo (tipo = 9)
        /// </summary>
        /// <param name="numeroRegistro"></param>
        /// <returns></returns>
        private string GerarTrailerRemessaCNAB240(int numeroRegistro) {
            try {
                string _trailer;

                // Controle
                _trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);           // 001-003 Código do banco na Compensação
                _trailer += "9999";                                                     // 004-007 Lote de Serviço
                _trailer += "9";                                                        // 000-008 Tipo de Registro

                _trailer += Utils.FormatCode("", " ", 9);                               // Uso Exclusivo FEBRABAN/CNAB

                // Totais
                _trailer += "000001";                                                   // 018-023 Quantidade de Lotes do Arquivo
                _trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);  // 024-029 Quantidade de Registros do Arquivo
                _trailer += "000000";                                                   // 030-035 Qtde de Contas p/ Conc. (Lotes)
                _trailer += Utils.FormatCode("", " ", 205);                             // 036-240 Uso Exclusivo FEBRABAN/CNAB

                return _trailer;
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do TRAILER DE ARQUIVO do arquivo de REMESSA.", ex);
            }

        }
        #endregion


        #region Arquivo Remessa 400      
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0064, 017, 0, GetFormatedNossoNumero(boleto), '0'));                              //064-080
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120
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
                string numeroDeDiasParaProtesto = string.Empty;

                if (boleto.Instrucoes.Count > 0) {
                    var instrucao = boleto.Instrucoes[0];
                    vInstrucao1 = instrucao.Codigo.ToString();

                    if (instrucao.QuantidadeDias > 0)
                    {
                        numeroDeDiasParaProtesto = instrucao.QuantidadeDias.ToString();
                    }
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 002, 0, numeroDeDiasParaProtesto, ' '));                  //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, string.Empty, ' '));                              //394-394                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400

                reg.CodificarLinha();

                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);

                return _detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null) {
                vMsg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null) {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0)) {
                vMsg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos) {
                #region Validação de cada boleto
                if (boleto.Remessa == null) {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                //else {
                //    #region Validações da Remessa que deverão estar preenchidas quando BANCO DO BRASIL
                //    if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento)) {
                //        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                //        vRetorno = false;
                //    }
                //    #endregion
                //}
                #endregion
            }
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
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal) {
            try {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _trailer;
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        #endregion



        #region Arquivo Retorno 240
        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        //public string Movimento(string codigo) {
        //    switch (codigo) {
        //        case "02":
        //            return "02-Entrada Confirmada";
        //        case "03":
        //            return "03-Entrada Rejeitada";
        //        case "04":
        //            return "04-Transferência de Carteira/Entrada";
        //        case "05":
        //            return "05-Transferência de Carteira/Baixa";
        //        case "06":
        //            return "06-Liquidação";
        //        case "07":
        //            return "07-Confirmação do Recebimento da Instrução de Desconto";
        //        case "08":
        //            return "08-Confirmação do Recebimento do Cancelamento do Desconto";
        //        case "09":
        //            return "09-Baixa";
        //        case "11":
        //            return "11-Títulos em Carteira";
        //        case "12":
        //            return "12-Confirmação Recebimento Instrução de Abatimento";
        //        case "13":
        //            return "13-Confirmação Recebimento Instrução de Cancelamento Abatimento";
        //        case "14":
        //            return "14-Confirmação Recebimento Instrução Alteração de Vencimento";
        //        case "17":
        //            return "17-Liquidação Após Baixa ou Liquidação Título Não Registrado";
        //        case "19":
        //            return "19-Confirmação Recebimento Instrução de Protesto";
        //        case "20":
        //            return "20-Confirmação Recebimento Instrução de Sustação/Cancelamento de Protesto";
        //        case "23":
        //            return "23-Remessa a Cartório (Aponte em Cartório)";
        //        case "24":
        //            return "24-Retirada de Cartório e Manutenção em Carteira";
        //        case "25":
        //            return "25-Protestado e Baixado (Baixa por Ter Sido Protestado)";
        //        case "26":
        //            return "26-Instrução Rejeitada";
        //        case "27":
        //            return "27-Confirmação do Pedido de Alteração de Outros Dados";
        //        case "28":
        //            return "28-Débito de Tarifas/Custas";
        //        case "30":
        //            return "30-Alteração de Dados Rejeitada";
        //        case "42":
        //            return "42-Confirmação da alteração dos dados do Sacado";
        //        case "46":
        //            return "46-Instrução para cancelar protesto confirmada";
        //        default:
        //            return "";
        //    }
        //}
        DetalheSegmentoTRetornoCNAB240 IBanco.LerDetalheSegmentoTRetornoCNAB240(string registro) {
            string _Controle_numBanco = registro.Substring(0, 3); //01
            string _Controle_lote = registro.Substring(3, 7); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            // C044 Código de Movimento Retorno
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _ccAgenciaCodigo = registro.Substring(17, 5); //08
            string _ccAgenciaDv = registro.Substring(22, 1); //09
            string _ccContaNumero = registro.Substring(23, 12); //10
            string _ccContaDv = registro.Substring(35, 1); //11
            string _ccDv = registro.Substring(36, 1); //12
            //string _outUsoExclusivo = registro.Substring(37, 9);
            string _outNossoNumero = registro.Substring(37, 20); //13
            string _outCarteira = registro.Substring(57, 1); //14
            string _outNumeroDocumento = registro.Substring(58, 15); //15
            string _outVencimento = registro.Substring(73, 8); //16
            string _outValor = registro.Substring(81, 15); //17
            //string _outUf = registro.Substring(96, 2);
            string _outBanco = registro.Substring(96, 3); //18
            string _outCodCedente = registro.Substring(99, 5); //19
            string _outDvCedente = registro.Substring(104, 1); //20
            string _outNomeCedente = registro.Substring(105, 25); //21
            string _outCodMoeda = registro.Substring(130, 2); //22
            string _sacadoInscricaoTipo = registro.Substring(132, 1); //23
            string _sacadoInscricaoNumero = registro.Substring(133, 15); //24
            string _sacadoNome = registro.Substring(148, 40); //25
            string _cnab28 = registro.Substring(188, 10); //26
            string _valorTarifasCustas = registro.Substring(198, 15); //27
            string _motivoCobraca = registro.Substring(213, 10); //28
            string _cnab31 = registro.Substring(223, 17); //29

            // pag. 44
            try {
                /* 05 */
                if (!_Servico_segmento.Equals(@"T")) {
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");
                }
                DetalheSegmentoTRetornoCNAB240 segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);
                segmentoT.CodigoBanco = Utils.ToInt32(_Controle_numBanco); //01
                segmentoT.idCodigoMovimento = Utils.ToInt32(_Servico_codMov); //07
                segmentoT.Agencia = Utils.ToInt32(_ccAgenciaCodigo); //08
                segmentoT.DigitoAgencia = _ccAgenciaDv; //09
                segmentoT.Conta = Utils.ToInt64(_ccContaNumero); //10
                segmentoT.DigitoConta = _ccContaDv; //11
                segmentoT.DACAgenciaConta = (String.IsNullOrEmpty(_ccDv.Trim())) ? 0 : Utils.ToInt32(_ccDv); //12
                segmentoT.NossoNumero = _outNossoNumero; //13
                segmentoT.CodigoCarteira = Utils.ToInt32(_outCarteira); //14
                segmentoT.NumeroDocumento = _outNumeroDocumento; //15
                segmentoT.DataVencimento = _outVencimento.ToString() == "00000000" ? DateTime.Now : DateTime.ParseExact(_outVencimento, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture); //16
                segmentoT.ValorTitulo = Utils.ToDecimal(_outValor) / 100; //17
                segmentoT.IdentificacaoTituloEmpresa = _outNomeCedente; //21
                segmentoT.TipoInscricao = Utils.ToInt32(_sacadoInscricaoTipo); //25
                segmentoT.NumeroInscricao = _sacadoInscricaoNumero; //24
                segmentoT.NomeSacado = _sacadoNome; //25
                segmentoT.ValorTarifas = Utils.ToDecimal(_valorTarifasCustas) / 100; //27
                // C047
                segmentoT.CodigoRejeicao = registro.Substring(213, 1) == "A" ? registro.Substring(214, 9) : registro.Substring(213, 10); //30
                segmentoT.UsoFebraban = _cnab31;

                return segmentoT;
            } catch (Exception ex) {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        DetalheSegmentoURetornoCNAB240 IBanco.LerDetalheSegmentoURetornoCNAB240(string registro) {
            string _Controle_numBanco = registro.Substring(0, 3); //01
            string _Controle_lote = registro.Substring(3, 4); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _dadosTituloAcrescimo = registro.Substring(17, 15); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 15); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 15); //10
            string _dadosTituloValorIof = registro.Substring(62, 15); //11
            string _dadosTituloValorPago = registro.Substring(77, 15); //12
            string _dadosTituloValorLiquido = registro.Substring(92, 15); //13
            string _outDespesas = registro.Substring(107, 15); //14
            //string _outPerCredEntRecb = registro.Substring(122, 5); //15
            string _outCreditos = registro.Substring(122, 15); //15
            //string _outBanco = registro.Substring(127, 10); //16
            string _outDataOcorrencia = registro.Substring(137, 8); //16
            string _outDataCredito = registro.Substring(145, 8); //17
            string _codOcorrencia = registro.Substring(153, 4); //18
            //string _cnab19 = registro.Substring(153, 87); //18


            try {

                if (!_Servico_segmento.Equals(@"U")) {
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.Servico_Codigo_Movimento_Retorno = Utils.ToDecimal(_Servico_codMov); //07.3U|Serviço|Cód. Mov.|Código de Movimento Retorno
                segmentoU.JurosMultaEncargos = Utils.ToDecimal(_dadosTituloAcrescimo) / 100;
                segmentoU.ValorDescontoConcedido = Utils.ToDecimal(_dadosTituloValorDesconto) / 100;
                segmentoU.ValorAbatimentoConcedido = Utils.ToDecimal(_dadosTituloValorAbatimento) / 100;
                segmentoU.ValorIOFRecolhido = Utils.ToDecimal(_dadosTituloValorIof) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Utils.ToDecimal(_dadosTituloValorPago) / 100;
                segmentoU.ValorLiquidoASerCreditado = Utils.ToDecimal(_dadosTituloValorLiquido) / 100;
                segmentoU.ValorOutrasDespesas = Utils.ToDecimal(_outDespesas) / 100;
                segmentoU.ValorOutrosCreditos = Utils.ToDecimal(_outCreditos) / 100;
                segmentoU.DataOcorrencia = DateTime.ParseExact(_outDataOcorrencia, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                segmentoU.DataCredito = _outDataCredito.ToString() == "00000000" ? segmentoU.DataOcorrencia : DateTime.ParseExact(_outDataCredito, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                // Código de movimento C044
                segmentoU.CodigoOcorrenciaSacado = _codOcorrencia;

                return segmentoU;
            } catch (Exception ex) {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        DetalheSegmentoWRetornoCNAB240 IBanco.LerDetalheSegmentoWRetornoCNAB240(string registro) {
            throw new NotImplementedException();
        }
        #endregion

        #region Arquivo Retorno 400
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

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = new CodigoMovimento(85, detalhe.CodigoOcorrencia).Descricao;

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

                detalhe.ValorTitulo = Utils.ToDecimal(reg.ValorTitulo) / 100;
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoRecebedor);
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.PrefixoAgenciaRecebedora);
                //detalhe. = reg.DVPrefixoRecebedora;
                detalhe.Especie = Utils.ToInt32(reg.EspecieTitulo);
                //
                int dataCredito = Utils.ToInt32(reg.DataCredito);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //
                detalhe.TarifaCobranca = Utils.ToDecimal(reg.ValorTarifa) / 100;
                detalhe.OutrasDespesas = Utils.ToDecimal(reg.OutrasDespesas) / 100;
                detalhe.ValorOutrasDespesas = Utils.ToDecimal(reg.JurosDesconto) / 100;
                detalhe.IOF = Utils.ToDecimal(reg.IOFDesconto) / 100;
                detalhe.Abatimentos = Utils.ToDecimal(reg.ValorAbatimento) / 100;
                detalhe.Descontos = Utils.ToDecimal(reg.DescontoConcedido) / 100;
                detalhe.ValorPrincipal = Utils.ToDecimal(reg.ValorRecebido) / 100;
                detalhe.JurosMora = Utils.ToDecimal(reg.JurosMora) / 100;
                detalhe.OutrosCreditos = Utils.ToDecimal(reg.OutrosRecebimentos) / 100;
                //detalhe. = reg.AbatimentoNaoAproveitado;
                detalhe.ValorPago = Utils.ToDecimal(reg.ValorLancamento) / 100;
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

    }
}
