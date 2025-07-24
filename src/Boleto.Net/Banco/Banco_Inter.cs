using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.077.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Lucas Reis
    /// </author>    
    internal class Banco_Inter : AbstractBanco, IBanco
    {
        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Inter.
        /// </summary>
        internal Banco_Inter()
        {
            this.Codigo = 77;
            this.Digito = "9";
            this.Nome = "Inter";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public int CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod10(
                boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0') +
                boleto.Carteira.PadLeft(3, '0') +
                Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true)
                );
        }

        #region IBanco Members

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, as cinco primeiras posições do campo 
        ///          livre e o dígito verificador deste campo;
        ///      2º campo
        ///          composto pelas posições 6ª a 15ª do campo livre e o dígito verificador deste campo;
        ///      3º campo
        ///          composto pelas posições 16ª a 25ª do campo livre e o dígito verificador deste campo;
        ///      4º campo
        ///          composto pelo dígito verificador do código de barras, ou seja, a 5ª posição do código de 
        ///          barras;
        ///      5º campo
        ///          Composto pelo fator de vencimento com 4(quatro) caracteres e o valor do documento com 10(dez) caracteres, sem separadores e sem edição.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV

            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
            string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
            string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
            string D1 = Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);

            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            string D2 = Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            string D3 = Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = boleto.DigitoNossoNumero.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            string FFFF = FatorVencimento(boleto).ToString();

            string VVVVVVVVVV = boleto.ValorCodBarra.ToString("N2").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05 – 1 - Dígito verificador do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 – 25 - Campo Livre
        /// 
        ///   *******
        /// 
        /// </summary>
        /// 
        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = boleto.ValorCodBarra.ToString("N2").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            var codigo = string.Format(
                "{0}{1}{2}{3}{4}", 
                Codigo.ToString(), 
                boleto.Moeda, 
                FatorVencimento(boleto), 
                valorBoleto, 
                FormataCampoLivre(boleto));

            boleto.CodigoBarra.Codigo = 
                Strings.Left(boleto.CodigoBarra.Codigo, 4) +
                Mod11Peso2a9(boleto.CodigoBarra.Codigo) + 
                Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 26 -  3 - Carteira
        ///    27 a 33	- 7 - Número da Operação     
        ///    34 a 44 - 11 - Número do Nosso Número(Com o digito verificador: Carteira 112: disponível no arquivo retorno | Carteira 110: enviado no arquivo remessa)
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {
            string FormataCampoLivre = string.Format(
                "{0}{1}{2}{3}{4}", 
                boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0'), 
                boleto.Carteira.PadLeft(3, '0'),
                boleto.Cedente.Convenio.ToString().PadLeft(7, '0'),
                boleto.NossoNumero.PadLeft(10, '0'), 
                boleto.DigitoNossoNumero);

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", Utils.FormatCode(boleto.Carteira, 3), boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira != "110" && boleto.Carteira != "112")
                throw new NotImplementedException("Carteira não implementada. Carteiras implementadas 110 e 112.");
    
            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 11);           
            else if (boleto.NossoNumero.Length < 11)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.Cedente.ContaBancaria.Agencia + ", são de 4 números.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 7)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", são de 07 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            // Atribui o nome do banco ao local de pagamento
            if (string.IsNullOrEmpty(boleto.LocalPagamento))
                boleto.LocalPagamento = "PAGAVEL EM QUALQUER BANCO OU LOTERICA";

            // Calcula o DAC do Nosso Número
            boleto.DigitoNossoNumero = CalcularDigitoNossoNumero(boleto).ToString();

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }
        #endregion IBanco Members

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação normal";
                case "07":
                    return "07-Baixado Automaticamente via Arquivo";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Valor Alterado";
                case "16":
                    return "16-Valor e Vencimento Alterado";
                default:
                    return "";
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                // Identificação do Registro ==> 1 a 1
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));

                //Tipo de Inscrição Empresa ==> 2 a 3
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));

                //Nº Inscrição da Empresa ==> 4 a 17
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Zeros "000" ==> 18 a 20

                //Carteira ==> 21 a 23
                detalhe.Carteira = registro.Substring(20, 3);

                //Agência do Cedente ==> 24 a 27 + Conta Cedente ==> 28 a 37
                detalhe.Agencia = Utils.ToInt32(registro.Substring(23, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(27, 10));

                //Nº Controle do Participante ==> 38 a 62
                detalhe.NumeroControle = registro.Substring(37, 25);

                // Zeros "00000000" ==> 63 a 70

                //Identificação do Título no Banco ==> 71 a 81
                detalhe.NossoNumero = registro.Substring(70, 11);

                //Branco ==> 82 a 86

                //Carteira ==> 87 a 89 | Já lido acima

                //Identificação de Ocorrência ==> 90 a 91
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(89, 2));

                //Descrição da ocorrência ==> 90 a 91
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(89, 2));               

                //Data Ocorrência no Banco ==> 92 a 97
                int dataOcorrencia = Utils.ToInt32(registro.Substring(91, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Número do Documento ==> 98 a 107
                detalhe.NumeroDocumento = registro.Substring(97, 10);

                //Identificação do Título no Banco ==> 108 a 118
                detalhe.IdentificacaoTitulo = registro.Substring(107, 11);

                //Data Vencimento do Título ==> 119 a 124
                int dataVencimento = Utils.ToInt32(registro.Substring(118, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                //Valor do Título ==> 125 a 137
                decimal valorTitulo = Convert.ToInt64(registro.Substring(124, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador ==> 138 a 140
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(137, 3));

                //Agência Cobradora ==> 141 a 144
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(140, 4));

                //Espécie do Título ==> 145 a 146
                detalhe.Especie = Utils.ToInt32(registro.Substring(144, 2));

                //Branco ==> 147 a 159

                //Valor Pago ==> 160 a 172
                decimal valorPago = Convert.ToUInt64(registro.Substring(159, 13));
                detalhe.ValorPago = valorPago / 100;

                // Data do Crédito ==> 173 a 178
                int dataCredito = Utils.ToInt32(registro.Substring(172, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Branco ==> 179 a 181

                //Nome do Pagador ==> 182 a 221
                detalhe.NomeSacado = registro.Substring(181, 40);

                //Branco ==> 222 a 226

                //CPF/CNPJ do Pagador ==> 227 a 240
                detalhe.CgcCpf = registro.Substring(226, 14);

                //Motivo da rejeição ==> 241 a 380
                detalhe.MotivosRejeicao = registro.Substring(240, 140);

                //Núm. da Operação ==> 381 a 394 No Inter, é o código do Convenio, vem nos detalhes
                //detalhe.NumeroControle = registro.Substring(380, 14); // ???

                //Núm. Sequencial do Registro ==> 395 a 400
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheSegmentoYRetornoCNAB240 LerDetalheSegmentoYRetornoCNAB240(string registro)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                return new HeaderRetorno(registro)
                {
                    TipoRegistro = Utils.ToInt32(registro.Substring(0, 1)),
                    CodigoRetorno = Utils.ToInt32(registro.Substring(1, 1)),
                    LiteralRetorno = registro.Substring(2, 7),
                    CodigoServico = Utils.ToInt32(registro.Substring(9, 2)),
                    LiteralServico = registro.Substring(11, 15),
                    ComplementoRegistro1 = Utils.ToInt32(registro.Substring(26, 10)),
                    Conta = Utils.ToInt32(registro.Substring(36, 9)),
                    DACConta = Utils.ToInt32(registro.Substring(45, 1)),
                    NomeEmpresa = registro.Substring(46, 30),
                    CodigoBanco = Utils.ToInt32(registro.Substring(76, 3)),
                    NomeBanco = registro.Substring(79, 15),
                    DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(94, 6)).ToString("##-##-##")),
                    ComplementoRegistro2 = registro.Substring(100, 294),
                    NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6))
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #region Seygi gerando remessa
        #region HEADER
        /// <summary>
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _header;

                _header = "0";
                _header += "1";
                _header += "REMESSA".PadRight(7, ' ');
                _header += "01";
                _header += "COBRANCA".PadRight(15, ' ');
                _header += "".PadRight(20, ' ');
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "077";
                _header += "INTER".PadRight(15,' ');
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "".PadRight(10, ' ');
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);
                _header += "".PadRight(277, ' '); ;
                _header += "000001"; // Número Sequencial do Registro Header (6 posições numéricas) 

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        # region DETALHE

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string _detalhe;

                _detalhe = "1"; //Identificação do Registro (1,1) 
                _detalhe += "".PadRight(19, ' '); //Branco (2,19) 
                _detalhe += boleto.Carteira.PadLeft(3,'0'); //Carteira (21,3) “112” ou “110” Para utilizar a carteira 110, solicite uma análise de reserva de faixa bancária com seu advisor.
                _detalhe += "0001"; //Agência (24,4)  - FIXO

                _detalhe += boleto.Cedente.ContaBancaria.Conta.PadLeft(9, '0'); //Conta Corrente (28,9) 
                _detalhe += boleto.Cedente.ContaBancaria.DigitoConta; //Dig da Conta Corrente (37, 1) 
                _detalhe += boleto.NumeroDocumento.PadRight(25, ' '); //Núm. Controle do Participante (38, 25) 
                _detalhe += "".PadRight(3, ' '); //Branco (63, 3)

                /********************************
                 * "0" = Valor sem multa        *
                 * "1" = Valor fixo de multa    *
                 * "2" = Percentual de multa    *
                 ********************************/
                if (boleto.ValorMulta > 0)
                {
                    _detalhe += "1"; //Campo da Multa (66, 1) 
                    _detalhe += boleto.ValorMulta.ApenasNumeros().ToString().PadLeft(13, '0'); //Valor da Multa (67, 13)
                    _detalhe += "".PadLeft(4, '0'); //Percentual Multa (80,4)
                    _detalhe += boleto.DataMulta.ToString("ddMMyy"); //Data da Multa (84, 6) DDMMAA 
                }
                else if (boleto.PercMulta > 0)
                {
                    _detalhe += "2"; //Campo da Multa (66, 1) 
                    _detalhe += "".PadLeft(13, '0'); //Valor da Multa (67, 13)
                    _detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 4, 4, '0', 0, true, true, true); //Percentual Multa (80,4)
                    _detalhe += boleto.DataMulta.ToString("ddMMyy"); //Data da Multa (84, 6) DDMMAA 
                }
                else
                {
                    _detalhe += "0"; //Campo da Multa (66, 1) 
                    _detalhe += "".PadLeft(13, '0'); //Valor da Multa (67, 13)
                    _detalhe += "".PadLeft(4, '0'); //Percentual Multa (80,4)
                    _detalhe += "".PadLeft(6, '0'); //Data da Multa (84, 6) DDMMAA 
                }
                
                if (boleto.Carteira == "110")
                {
                    _detalhe += Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true) + CalcularDigitoNossoNumero(boleto).ToString(); // Identificação do Título no Banco - Nosso Numero + Dígito Verificador (90,11)
                }
                else
                {
                    _detalhe += "".PadLeft(11, '0');
                }

                _detalhe += "".PadRight(8, ' '); //Branco (101, 8)

                /********************************************
                * 01..Remessa                               *
                * 06..Edição da data de vencimento          *
                * 07..Solicitação de baixa                  *
                * 20..Edição de valor                       *
                * 26..Edição de data de vencimento e valor  *
                *********************************************/
                _detalhe += "01"; //Identificação da Ocorrência (109, 2) - "01" - Remessa

                _detalhe += Utils.Right(boleto.NumeroDocumento, 10, '0', true); //Nº do Documento (111, 10) 
                _detalhe += boleto.DataVencimento.ToString("ddMMyy"); //Data do Vencimento do Título (121, 6) DDMMAA                
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor do Título (127, 13)

                if (boleto.DataLimitePagamento != DateTime.MinValue)
                    _detalhe += Utils.DateDiff(DateInterval.Day, boleto.DataVencimento, boleto.DataLimitePagamento);  //Data Limite para Pagamento (140, 2) 
                else
                    _detalhe += "60"; //Data Limite para Pagamento (140, 2) 

                _detalhe += "".PadRight(6, ' '); //Branco (142, 6)
                _detalhe += "01"; //Espécie do Título (148, 2) - FIXO
                _detalhe += "N"; //Identificaçao (150, 1) - FIXO

                _detalhe += "".PadRight(6, ' '); //Data da Emissão do Título (151, 6)
                _detalhe += "".PadRight(3, ' '); //Branco (157, 3)

                /****************************************************
                * "0" = Sem juros/mora                              *
                * "1" = Valor fixo de juros/mora por dia            *
                * "2" = Taxa mensal de juros/mora (cobrado prorata) *
                *****************************************************/
                if (boleto.JurosMora > 0)
                {
                    _detalhe += "1"; //Campo de juros/mora (160, 1)
                    _detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor a ser cobrado por Dia de Atraso (161, 13)
                    _detalhe += "".PadLeft(4, '0'); //Taxa mensal de juros/mora (174, 4) 
                    _detalhe += boleto.DataJurosMora.ToString("ddMMyy"); //Data da mora (178, 6) 
                }
                else if (boleto.PercJurosMora > 0)
                {
                    _detalhe += "2"; //Campo de juros/mora (160, 1)
                    _detalhe += "".PadLeft(13, '0'); //Valor a ser cobrado por Dia de Atraso (161, 13)
                    _detalhe += Utils.FitStringLength(boleto.PercJurosMora.ApenasNumeros(), 4, 4, '0', 0, true, true, true); //Taxa mensal de juros/mora (174, 4) 
                    _detalhe += boleto.DataJurosMora.ToString("ddMMyy"); //Data da mora (178, 6) 
                }
                else
                {
                    _detalhe += "0"; //Campo de juros/mora (160, 1)
                    _detalhe += "".PadLeft(13, '0'); //Valor a ser cobrado por Dia de Atraso (161, 13)
                    _detalhe += "".PadLeft(4, '0'); //Taxa mensal de juros/mora (174, 4) 
                    _detalhe += "".PadLeft(6, '0'); //Data da mora (178, 6) 
                }

                /************************************************
                * "0" = Sem desconto                            *
                * "1" = Valor fixo até a data informada         *
                * "4" = Percentual do valor nominal até a data  *
                *************************************************/
                if (boleto.ValorDesconto > 0)
                {
                    _detalhe += "1"; //Campo de descontos (184, 1)
                    _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor desconto (185, 13)
                    _detalhe += "".PadLeft(4, '0'); //Percentual de desconto (198, 4) 
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy"); //Data limite para concessão do desconto (202, 6) 
                }
                else
                {
                    _detalhe += "0"; //Campo de descontos (184, 1)
                    _detalhe += "".PadLeft(13, '0'); //Valor desconto (185, 13)
                    _detalhe += "".PadLeft(4, '0'); //Percentual de desconto (198, 4) 
                    _detalhe += "".PadLeft(6, '0'); //Data limite para concessão do desconto (202, 6) 
                }

                _detalhe += "".PadLeft(13, '0'); //Branco (208, 13)

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF - Identificação do tipo de inscrição do pagador (221, 2)
                else
                    _detalhe += "02"; // CNPJ - Identificação do tipo de inscrição do pagador (221, 2)

                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpf_Cnpj, 14, 14, '0', 0, true, true, true); //CPF/CNPJ do pagador (223, 14)

                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper(); //Nome do pagador (237, 40)            
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumero.TrimStart(' '), 38, 38, ' ', 0, true, true, false).ToUpper(); //Endereço Completo do pagador (277, 38)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF.TrimStart(' '), 2, 2, ' ', 0, true, true, false).ToUpper(); //Endereço Completo do pagador (315, 2)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true); //UF do pagador (317, 8)

                /*******************************************************************************
                *  Mensagem livre impressa no boleto (não será confirmada no arquivo retorno)  *
                ********************************************************************************/
                _detalhe += Utils.FitStringLength("", 70, 70, ' ', 0, true, true, false); //1ª Mensagem (325, 70)                
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //Nº Seqüencial do Registro (395, 6)

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        # endregion DETALHE

        # region TRAILER

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                string _trailer;

                _trailer = "9";
                _trailer += (numeroRegistro - 2).ToString().PadLeft(6, '0');
                _trailer += "".PadRight(387, ' '); 
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        # endregion
        #endregion Seygi Gerando Remessa


        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }
    }
}
