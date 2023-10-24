using BoletoNet.Util;
using System;
using System.Web.UI;
using BoletoNet.Enums;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.530.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Carlos dos Santos
    /// </author>    
    internal class Banco_SerFinance : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_SerFinance.
        /// </summary>
        internal Banco_SerFinance()
        {
            this.Codigo = 530;
            this.Nome = "Ser Finance";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod10SerFinance(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true), boleto.Carteira, Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true));
        }

        public static long FatorVencimento2000(Boleto boleto)
        {
            var dateBase = new DateTime(2000, 7, 3, 0, 0, 0);
            var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            long rangeUtilizavel = Utils.DateDiff(DateInterval.Day, dataAtual, boleto.DataVencimento);
            while (boleto.DataVencimento > dateBase.AddDays(9999))
                dateBase = boleto.DataVencimento.AddDays(-(((Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) - 9999) - 1) + 1000));
            return Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) + 1000;
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

            string D4 = _dacBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            string FFFF = FatorVencimento2000(boleto).ToString();

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

            if (boleto.Carteira == "110" || boleto.Carteira == "180")
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString("000"), boleto.Moeda,
                FatorVencimento2000(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else
            {
                throw new NotImplementedException("Carteira ainda não implementada.");
            }
            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);
            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }


        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - Número do Nosso Número(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {
            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.Cedente.ContaBancaria.Agencia, boleto.Carteira,
                                            boleto.NossoNumero, boleto.Cedente.ContaBancaria.Conta, "0");
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
            if (boleto.Carteira != "110" && boleto.Carteira != "180")
                throw new NotImplementedException("Carteira não implementada.");

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
            {
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 11);
            }

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
            boleto.LocalPagamento = "Pagável em qualquer banco até o vencimento";
            // Atribui o nome do banco ao local de pagamento
            if (string.IsNullOrEmpty(boleto.LocalPagamento))
                boleto.LocalPagamento = "Pagável em qualquer banco até o vencimento";


            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _dacNossoNumero;

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
                case "01":
                    return "01-Entrada Confirmadana CIP";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "04":
                    return "04-Transferencia de Carteira/Entrada";
                case "05":
                    return "05-Transferencia de Carteira/Baixa";
                case "06":
                    return "06-Liquidação";
                case "07":
                    return "07-Confirmacao de Recebimento da Instrucao de Desconto";
                case "08":
                    return "08-Confirmacao de Recebimento do Cancelamento do Desconto";
                case "09":
                    return "09-Baixa";
                case "11":
                    return "11-Em Ser - Arquivo de Títulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "17":
                    return "17-Liquidação após baixa ou Título não registrado";
                case "19":
                    return "19-Recebimento de Instrucao de Protesto";
                case "20":
                    return "20-Recebimento de Cancelamento de Protesto";
                case "23":
                    return "23-Remessa a Cartorio";
                case "24":
                    return "24-Retirada de Cartorio";
                case "25":
                    return "25-Protestado e Baixado";
                case "26":
                    return "26-Instrucao Rejeitada";
                case "27":
                    return "27-Pedido de Alteracao de Outros Dados";
                case "28":
                    return "28-Debito de Tarifas";
                case "29":
                    return "29-Ocorrencias do Sacado";
                case "30":
                    return "30-Alteracao de Dados Rejeitada";
                case "53":
                    return "53-Titulo recusado na CIP";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Verifica o código do motivo da rejeição informada pelo banco
        /// </summary>
        public string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Código do registro detalhe inválido";
                case "03":
                    return "03-Código da ocorrência inválida";
                case "04":
                    return "04-Código de ocorrência não permitida para a carteira";
                case "05":
                    return "05-Código de ocorrência não numérico";
                case "07":
                    return "07-Agência/conta/Digito - Inválido";
                case "08":
                    return "08-Nosso número inválido";
                case "09":
                    return "09-Nosso número duplicado";
                case "10":
                    return "10-Carteira inválida";
                case "13":
                    return "13-Identificação da emissão do bloqueio inválida";
                case "16":
                    return "16-Data de vencimento inválida";
                case "18":
                    return "18-Vencimento fora do prazo de operação";
                case "20":
                    return "20-Valor do Título inválido";
                case "21":
                    return "21-Espécie do Título inválida";
                case "22":
                    return "22-Espécie não permitida para a carteira";
                case "24":
                    return "24-Data de emissão inválida";
                case "28":
                    return "28-Código de desconto inválido";
                case "44":
                    return "44-Agência Cedente não prevista";
                case "45":
                    return "45-Nome do pagador não informado";
                case "46":
                    return "46-Tipo/número de inscrição do pagador inválidos";
                case "47":
                    return "47-Endereço do pagador não informado";
                case "48":
                    return "48-CEP inválido";
                case "63":
                    return "63-Entrada para Título já cadastrado";
                default:
                    return "";
            }
        }

        private string Mod10SerFinance(string agencia,string carteira, string num)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */
            var seq = agencia+carteira+num;

            int d, s = 0, p = 2, r;

            for (int i = seq.Length; i > 0; i--)
            {
                r = (Convert.ToInt32(seq.Mid(i, 1)) * p);

                if (r > 9)
                    r = (r / 10) + (r % 10);

                s += r;

                if (p == 2)
                    p = 1;
                else
                    p = p + 1;
            }
            d = ((10 - (s % 10)) % 10);
            return d.ToString();
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro)
                {
                    // Identificação do Registro ==> 001 a 001
                    IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1)),

                    //Tipo de Inscrição Empresa ==> 002 a 003
                    CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2)),

                    //Nº Inscrição da Empresa ==> 004 a 017
                    NumeroInscricao = registro.Substring(3, 14),

                    //Identificação da Empresa Cedente no Banco ==> 021 a 037 = 17 (Igual remessa)
                    // 0 + Carteira 3 + Agência 5 + Conta 7 + Digito 1 = 17
                    // ex: 00090750315206870
                    Agencia = Utils.ToInt32(registro.Substring(24, 5)),
                    Conta = Utils.ToInt32(registro.Substring(29, 7)),
                    DACConta = Utils.ToInt32(registro.Substring(36, 1)),

                    //Nº Controle do Participante ==> 038 a 062
                    NumeroControle = registro.Substring(37, 25),

                    //Identificação do Título no Banco ==> 071 a 081
                    NossoNumero = registro.Substring(70, 11),//Sem o DV

                    //Identificação do Título no Banco ==> 082 a 082
                    DACNossoNumero = registro.Substring(81, 1), //DV

                    //Carteira ==> 108 a 108
                    Carteira = registro.Substring(107, 1),

                    //Identificação de Ocorrência ==> 109 a 110
                    CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2)),

                    //Descrição da ocorrência
                    DescricaoOcorrencia = this.Ocorrencia(registro.Substring(110, 2)),
                };

                //Data Ocorrência no Banco ==> 111 a 116
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Número do Documento ==> 117 a 126
                detalhe.NumeroDocumento = registro.Substring(116, 10);

                //Identificação do Título no Banco ==> 127 a 146
                detalhe.IdentificacaoTitulo = registro.Substring(126, 20);

                //Data Vencimento do Título ==> 147 a 152
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                //Valor do Título ==> 153 a 165
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador ==> 166 a 168
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                //Agência Cobradora ==> 169 a 173
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));

                //Espécie do Título ==> 174 a 175
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa) ==> 176 a 188
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.ValorDespesa = valorDespesa / 100;

                //Outras despesas Custas de Protesto (Valor Outras Despesas) ==> 189 a 201
                decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;

                //Juros Operação em Atraso ==> 202 a 214
                decimal OutrosCreditos = Convert.ToUInt64(registro.Substring(201, 13));
                detalhe.OutrosCreditos = OutrosCreditos / 100;

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido) ==> 228 a 240
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                //Desconto Concedido (Valor Desconto Concedido) ==> 241 a 253
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;

                //Valor Pago ==> 254 a 266
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                //Juros Mora ==> 267 a 279
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

                // Data do Crédito ==> 296 a 301
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Motivos das Rejeições para os Códigos de Ocorrência ==> 319 a 328
                detalhe.MotivosRejeicao = registro.Substring(318, 10);

                //Nome do Sacado
                detalhe.NomeSacado = "";

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 240.", ex);
            }
        }

        #region gerando remessa
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
                        _header = GerarHeaderRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        throw new Exception("Tipo de arquivo inexistente.");
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

        public string GerarHeaderRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _header;
                string cpf_Cnpj = cedente.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");

                _header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _header += "0000";
                _header += "0";
                _header += Utils.FormatCode("", 9);
                _header += cedente.CPFCNPJ.Length <= 11 ? "1" : "2";
                _header += Utils.FormatCode(cpf_Cnpj, "0", 14);
                _header += Utils.FormatCode(cedente.Codigo.ToString(), " ", 20);
                _header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);
                _header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 1, true);
                _header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true); 
                _header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);
                _header += "0"; // Digito verificado AG/Conta
                _header += Utils.FormatCode(cedente.Nome.ToString(), " ", 30);
                _header += Utils.FormatCode(Nome," ",30);
                _header += Utils.FormatCode("", " ",10);
                _header += "1";
                _header += DateTime.Now.ToString("ddMMyyyy");
                _header += DateTime.Now.ToString("hhmmss");
                _header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);
                _header += "101";
                _header += "01600";
                _header += Utils.FormatCode("", " ", 20);
                _header += Utils.FormatCode("", " ", 20);
                _header += Utils.FormatCode("", " ", 29);
                _header = Utils.SubstituiCaracteresEspeciais(_header);
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        #endregion

        #region HEADER LOTE

        /// <summary>
        /// HEADER DE LOTE do arquivo CNAB
        /// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        throw new Exception("Tipo de arquivo inexistente.");
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string _headerLote;
                string cpf_Cnpj = cedente.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                string mensagem1 = new string(' ', 40);
                string mensagem2 = new string(' ', 40);

                _headerLote = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _headerLote += "0001";
                _headerLote += "1";
                _headerLote += " ";
                _headerLote += "01";
                _headerLote += "  ";
                _headerLote += "030";
                _headerLote += " ";
                _headerLote += cedente.CPFCNPJ.Length <= 11 ? "1" : "2";
                _headerLote += Utils.FormatCode(cpf_Cnpj, "0", 15);
                _headerLote += Utils.FormatCode(cedente.Codigo.ToString(), " ", 20);
                _headerLote += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);
                _headerLote += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 1, true);
                _headerLote += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);
                _headerLote += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);
                _headerLote += "0"; // Digito verificado AG/Conta
                _headerLote += Utils.FormatCode(cedente.Nome.ToString(), " ", 30);
                _headerLote += mensagem1;
                _headerLote += mensagem2;
                _headerLote += "00000000"; // Numero Remessa/Retorno
                _headerLote += "00000000"; // DAta gravacao Remessa/Retorno
                _headerLote += "00000000";
                _headerLote += Utils.FormatCode("", " ", 33);
                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);
                return _headerLote;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region DETALHE

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.CNAB400:
                        throw new Exception("Tipo de arquivo inexistente.");
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

        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            return "";
            
        }
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string nrDeControle = Utils.FitStringLength(boleto.NumeroDocumento.TrimStart(' '), 25, 25, ' ', 0, true, true, false);

                string _detalhe;

                _detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _detalhe += "0001";
                _detalhe += "3";
                _detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);
                _detalhe += "P";
                _detalhe += " ";
                _detalhe += "01";// Entrada de títulos
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, "0", 5, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoAgencia, "0", 1, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, "0", 12, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, "0", 1, true);
                _detalhe += "0"; // Digito verificado AG/Conta
                _detalhe += "5"; // Direcionamento da Cobranca
                _detalhe += "000"; // Modalidade Banco Correspondente
                _detalhe += "00";
                _detalhe += "110"; // Modalidade Banco Cedente
                _detalhe += Utils.FormatCode(boleto.NossoNumero, "0", 11, true);
                _detalhe += "1"; // Codigo Carteira 
                _detalhe += "1";
                _detalhe += "2";
                _detalhe += "1";
                _detalhe += "1";
                _detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 15);
                _detalhe += boleto.DataVencimento.ToString("ddMMyyyy");
                _detalhe += Utils.FormatCode(boleto.ValorBoleto.ApenasNumeros(), "0", 15, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, "0", 5, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoAgencia, "0", 1, true);
                _detalhe += Utils.FormatCode(boleto.EspecieDocumento.Codigo.ToString(), "0", 2, true);
                _detalhe += "A";
                _detalhe += boleto.DataProcessamento.ToString("ddMMyyyy");
                _detalhe += "1"; // Juros por dia
                _detalhe += boleto.DataJurosMora.ToString("ddMMyyyy");
                _detalhe += Utils.FormatCode(boleto.JurosMora.ApenasNumeros(), "0", 15, true);
                _detalhe += "1"; // Valor Desconto 
                if (boleto.DataDesconto == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                {
                    _detalhe += "00000000";
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyyyy");
                }
                _detalhe += Utils.FormatCode(boleto.ValorDesconto.ApenasNumeros(), "0", 15, true);
                _detalhe += Utils.FormatCode("0", "0", 15); // IOF
                _detalhe += Utils.FormatCode("0", "0", 15); // Abatimento
                _detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 25);
                if (boleto.Instrucoes.FirstOrDefault()?.QuantidadeDias > 0) // Protesto
                {
                    _detalhe += "1";
                    _detalhe += boleto.Instrucoes.FirstOrDefault()?.QuantidadeDias.ToString("00");
                }
                else
                {
                    _detalhe += "3";
                    _detalhe += "00";
                }
                _detalhe += "2"; // Nao baixar / nao devolver
                _detalhe += "000";
                _detalhe += "09"; // Moeda: Real
                _detalhe += "0000000000";
                _detalhe += " ";
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Segmento P do arquivo CNAB400.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                string cpf_CnpjA = boleto.Avalista?.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "") ?? "0";

                string _detalhe;

                _detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _detalhe += "0001";
                _detalhe += "3";
                _detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);
                _detalhe += "Q";
                _detalhe += " ";
                _detalhe += "01"; // Entrada Titulos
                _detalhe += boleto.Sacado.CPFCNPJ.Length <= 11 ? "1" : "2";
                _detalhe += Utils.FormatCode(cpf_Cnpj, "0", 15, true);
                _detalhe += Utils.FormatCode(boleto.Sacado.Nome.TrimStart(' '), " ", 40).ToUpper();
                _detalhe += Utils.FormatCode(boleto.Sacado.Endereco.EndComNumero.TrimStart(' '), " ", 40).ToUpper();
                _detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);
                _detalhe += Utils.FormatCode(boleto.Sacado.Endereco.CEP.Replace("-", ""), "0", 8);
                _detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);
                _detalhe += Utils.FormatCode(boleto.Sacado.Endereco.UF, " ", 2);
                _detalhe += cpf_CnpjA;
                if(!string.IsNullOrEmpty(boleto.Avalista?.CPFCNPJ))
                {
                    _detalhe += Utils.FormatCode(cpf_CnpjA, "0", 15);
                    _detalhe += Utils.FormatCode(boleto.Avalista.Nome, " ", 40);
                }
                else
                {
                    _detalhe += Utils.FormatCode("0", 15);
                    _detalhe += Utils.FormatCode(" ", 40);
                }

                _detalhe += "000"; // Banco Correspondente Compensasao
                _detalhe += Utils.FormatCode(" ", " ", 20);
                _detalhe += Utils.FormatCode(" ", " ", 8);
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Segmento Q do arquivo CNAB240.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe;

                _detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _detalhe += "0001";
                _detalhe += "3";
                _detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);
                _detalhe += "R";
                _detalhe += " ";
                _detalhe += "01"; // Entrada Titulos
                if (boleto.DataDescontoAntecipacao2.HasValue && boleto.ValorDescontoAntecipacao2.HasValue)
                {
                    _detalhe += "1";
                    _detalhe += Utils.FormatCode(boleto.DataDescontoAntecipacao2.Value.ToString("ddMMyyyy"), "0", 8);
                    _detalhe += Utils.FormatCode(boleto.ValorDescontoAntecipacao2.ApenasNumeros(), "0", 15, true);
                }
                else
                {
                    _detalhe += "1";
                    _detalhe += Utils.FormatCode("0", "0", 8);
                    _detalhe += Utils.FormatCode("0", "0", 15, true);
                }

                if (boleto.DataDescontoAntecipacao3.HasValue && boleto.ValorDescontoAntecipacao3.HasValue)
                {
                    _detalhe += "1";
                    _detalhe += Utils.FormatCode(boleto.DataDescontoAntecipacao3.Value.ToString("ddMMyyyy"), "0", 8);
                    _detalhe += Utils.FormatCode(boleto.ValorDescontoAntecipacao3.ApenasNumeros(), "0", 15, true);
                }
                else
                {
                    _detalhe += "1";
                    _detalhe += Utils.FormatCode("0", "0", 8);
                    _detalhe += Utils.FormatCode("0", "0", 15, true);
                }

                _detalhe += "1";
                _detalhe += boleto.DataMulta.ToString("ddMMyyyy");                                        
                _detalhe += Utils.FormatCode(boleto.ValorMulta.ApenasNumeros(), "0", 15, true);
                _detalhe += Utils.FormatCode("", " ", 10);                                                
                _detalhe += Utils.FormatCode("", " ", 40);                                                
                _detalhe += Utils.FormatCode("", " ", 40);                                                
                _detalhe += Utils.FormatCode("", " ", 20);
                _detalhe += Utils.FormatCode("", " ", 8); // Codigo Ocorrencia Sacado
                _detalhe += Utils.FormatCode("", " ", 23);
                _detalhe += "0";
                _detalhe += Utils.FormatCode("", " ", 9);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Segmento R do arquivo CNAB240.", ex);
            }
        }

        #endregion DETALHE

        #region TRAILER

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      
                header += "0001";                                                                       
                header += "5";                                                                          
                header += Utils.FormatCode("", " ", 9);                                                
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);                          
                header += Utils.FormatCode("", "0", 6);                                                 
                header += Utils.FormatCode("", "0", 17);                                                
                header += Utils.FormatCode("", "0", 6);                                                 
                header += Utils.FormatCode("", "0", 17);                                                
                header += Utils.FormatCode("", "0", 6);                                                 
                header += Utils.FormatCode("", "0", 17);                                                
                header += Utils.FormatCode("", "0", 6);                                                 
                header += Utils.FormatCode("", "0", 17);                                                
                header += Utils.FormatCode("", " ", 8);                                                 
                header += Utils.FormatCode("", " ", 117);                                               
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                return GerarTrailerRemessa240(numeroRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public string GerarTrailerRemessa240(int numeroRegistro)
        {
            try
            {
                string _trailer;

                _trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _trailer += "9999";
                _trailer += "9";
                _trailer += Utils.FormatCode("", " ", 9);
                _trailer += "000001";
                _trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6,true);
                _trailer += Utils.FormatCode("0", "0", 6);
                _trailer += Utils.FormatCode("", "0", 205);
                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);
                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        # endregion
        #endregion 


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
