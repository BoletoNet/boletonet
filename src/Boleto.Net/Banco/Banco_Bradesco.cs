using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.237.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// </author>    
    internal class Banco_Bradesco : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Bradesco.
        /// </summary>
        internal Banco_Bradesco()
        {
            this.Codigo = 237;
            this.Digito = "2";
            this.Nome = "Bradesco";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7);
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

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            string FFFF = FatorVencimento(boleto).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            string VVVVVVVVVV = valor.ToString("N2").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

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
            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            var valorBoleto = valor.ToString("N2").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            if (boleto.Carteira == "02" || boleto.Carteira == "03" || boleto.Carteira == "04" || boleto.Carteira == "09" || boleto.Carteira == "19" || boleto.Carteira == "26") // Com registro
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.Carteira == "06" || boleto.Carteira == "16" || boleto.Carteira == "25") // Sem Registro
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}0000{2}{3}", Codigo.ToString(), boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
                }

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
            if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "04" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "16" && boleto.Carteira != "19" && boleto.Carteira != "25" && boleto.Carteira != "26")
                throw new NotImplementedException("Carteira não implementada. Carteiras implementadas 02, 03, 04, 06, 09, 16, 19, 25, 26.");

            //O valor é obrigatório para a carteira 03
            if (boleto.Carteira == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Para a carteira 03, o valor do boleto não pode ser igual a zero");
            }

            //O valor é obrigatório para a carteira 09
            if (boleto.Carteira == "09")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            }
            //else if (boleto.Carteira == "06")
            //{
            //    boleto.ValorBoleto = 0;
            //}

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
            {
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 11);
            }
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
                boleto.LocalPagamento = "PAGÁVEL PREFERENCIALMENTE NAS AGÊNCIAS DO BRADESCO";
            else if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

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
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação normal";
                case "09":
                    return "09-Baixado Automaticamente via Arquivo";
                case "10":
                    return "10-Baixado conforme instruções da Agência";
                case "11":
                    return "11-Em Ser - Arquivo de Títulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Liquidação em Cartório";
                case "17":
                    return "17-Liquidação após baixa ou Título não registrado";
                case "18":
                    return "18-Acerto de Depositária";
                case "19":
                    return "19-Confirmação Recebimento Instrução de Protesto";
                case "20":
                    return "20-Confirmação Recebimento Instrução Sustação de Protesto";
                case "21":
                    return "21-Acerto do Controle do Participante";
                case "23":
                    return "23-Entrada do Título em Cartório";
                case "24":
                    return "24-Entrada rejeitada por CEP Irregular";
                case "27":
                    return "27-Baixa Rejeitada";
                case "28":
                    return "28-Débito de tarifas/custas";
                case "30":
                    return "30-Alteração de Outros Dados Rejeitados";
                case "32":
                    return "32-Instrução Rejeitada";
                case "33":
                    return "33-Confirmação Pedido Alteração Outros Dados";
                case "34":
                    return "34-Retirado de Cartório e Manutenção Carteira";
                case "35":
                    return "35-Desagendamento ) débito automático";
                case "68":
                    return "68-Acerto dos dados ) rateio de Crédito";
                case "69":
                    return "69-Cancelamento dos dados ) rateio";
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
                case "38":
                    return "38-Prazo para protesto inválido";
                case "44":
                    return "44-Agência Cedente não prevista";
                case "50":
                    return "50-CEP irregular - Banco Correspondente";
                case "63":
                    return "63-Entrada para Título já cadastrado";
                case "68":
                    return "68-Débito não agendado - erro nos dados de remessa";
                case "69":
                    return "69-Débito não agendado - Sacado não consta no cadastro de autorizante";
                case "70":
                    return "70-Débito não agendado - Cedente não autorizado pelo Sacado";
                case "71":
                    return "71-Débito não agendado - Cedente não participa da modalidade de débito automático";
                case "72":
                    return "72-Débito não agendado - Código de moeda diferente de R$";
                case "73":
                    return "73-Débito não agendado - Data de vencimento inválida";
                case "74":
                    return "74-Débito não agendado - Conforme seu pedido, Título não registrado";
                case "75":
                    return "75-Débito não agendado - Tipo de número de inscrição do debitado inválido";
                default:
                    return "";
            }
        }

        private string Mod11Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o cálculo do dígito, será necessário acrescentar o número da carteira à esquerda antes do Nosso Número, 
            e aplicar o módulo 11, com base 7.
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma deverá ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferença entre o divisor e o resto, será o dígito de autoconferência: 11 - 3 = 8 (dígito de auto-conferência)
            
            Se o resto da divisão for “1”, desprezar o cálculo de subtração e considerar o dígito como “P”. 
            Se o resto da divisão for “0”, desprezar o cálculo de subtração e considerar o dígito como “0”.
            */
            #endregion

            /* Variáveis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(seq.Mid(i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
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

                    //Identificação do Título no Banco ==> 071 a 082
                    NossoNumeroComDV = registro.Substring(70, 12),

                    //Identificação do Título no Banco ==> 071 a 081
                    NossoNumero = registro.Substring(70, 11),//Sem o DV

                    //Identificação do Título no Banco ==> 082 a 082
                    DACNossoNumero = registro.Substring(81, 1), //DV

                    //Carteira ==> 108 a 108
                    Carteira = registro.Substring(107, 1),

                    //Identificação de Ocorrência ==> 109 a 110
                    CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2)),

                    //Descrição da ocorrência
                    DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2))
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

                // IOF ==> 215 a 227
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;

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

                //Outros Créditos ==> 280 a 292
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;

                //Motivo do Código de Ocorrência 19 (Confirmação de Instrução de Protesto) ==> 295 a 295
                detalhe.MotivoCodigoOcorrencia = registro.Substring(294, 1);

                // Data do Crédito ==> 296 a 301
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Origem Pagamento ==> 302 a 304
                detalhe.OrigemPagamento = registro.Substring(301, 3);

                //Motivos das Rejeições para os Códigos de Ocorrência ==> 319 a 328
                detalhe.MotivosRejeicao = registro.Substring(318, 10);

                //Número do Cartório ==> 369 a 370
                detalhe.NumeroCartorio = Utils.ToInt32(registro.Substring(365, 2));

                //Número do Protocolo ==> 371 a 380
                detalhe.NumeroProtocolo = registro.Substring(365, 2);

                //Nome do Sacado
                detalhe.NomeSacado = "";

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
            try
            {
                DetalheSegmentoYRetornoCNAB240 detalhe = new DetalheSegmentoYRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "Y")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento Y.");

                detalhe.CodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.IdentificacaoRegistro = Convert.ToInt32(registro.Substring(17, 2));
                detalhe.TipoInscricao = registro.Substring(19, 1);
                detalhe.NumeroInscricao = registro.Substring(20, 15);

                detalhe.NomeSacador = registro.Substring(35, 40);
                detalhe.EnderecoSacador = registro.Substring(75, 40);
                detalhe.BairroSacador = registro.Substring(115, 15);
                detalhe.CEPSacador = registro.Substring(130, 8);
                detalhe.CidadeSacador = registro.Substring(138, 15);
                detalhe.UFSacador = registro.Substring(153, 2);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO Y.", ex);
            }


        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoTRetornoCNAB240 detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                detalhe.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                detalhe.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                detalhe.DigitoAgencia = registro.Substring(22, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(23, 12));
                detalhe.DigitoConta = registro.Substring(35, 1);

                detalhe.NossoNumero = registro.Substring(37, 20);
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                detalhe.NumeroDocumento = registro.Substring(58, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                detalhe.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(91, 15));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                detalhe.NumeroInscricao = registro.Substring(133, 15);
                detalhe.NomeSacado = registro.Substring(148, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(198, 15));
                detalhe.ValorTarifas = valorTarifas / 100;
                detalhe.UsoFebraban = registro.Substring(218, 22);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }


        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoURetornoCNAB240 detalhe = new DetalheSegmentoURetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");

                detalhe.CodigoOcorrenciaSacado = registro.Substring(15, 2);
                int DataCredito = Convert.ToInt32(registro.Substring(145, 8));
                detalhe.DataCredito = Convert.ToDateTime(DataCredito.ToString("##-##-####"));
                int DataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                detalhe.DataOcorrencia = Convert.ToDateTime(DataOcorrencia.ToString("##-##-####"));
                int DataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));
                if (DataOcorrenciaSacado > 0)
                    detalhe.DataOcorrenciaSacado = Convert.ToDateTime(DataOcorrenciaSacado.ToString("##-##-####"));
                else
                    detalhe.DataOcorrenciaSacado = DateTime.Now;

                decimal JurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15));
                detalhe.JurosMultaEncargos = JurosMultaEncargos / 100;
                decimal ValorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15));
                detalhe.ValorDescontoConcedido = ValorDescontoConcedido / 100;
                decimal ValorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15));
                detalhe.ValorAbatimentoConcedido = ValorAbatimentoConcedido / 100;
                decimal ValorIOFRecolhido = Convert.ToUInt64(registro.Substring(62, 15));
                detalhe.ValorIOFRecolhido = ValorIOFRecolhido / 100;
                decimal ValorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15));
                detalhe.ValorPagoPeloSacado = ValorPagoPeloSacado / 100;
                decimal ValorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15));
                detalhe.ValorLiquidoASerCreditado = ValorLiquidoASerCreditado / 100;
                decimal ValorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15));
                detalhe.ValorOutrasDespesas = ValorOutrasDespesas / 100;

                decimal ValorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15));
                detalhe.ValorOutrosCreditos = ValorOutrosCreditos / 100;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                return new HeaderRetorno(registro)
                {
                    TipoRegistro = Utils.ToInt32(registro.Substring(000, 1)),
                    CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1)),
                    LiteralRetorno = registro.Substring(002, 7),
                    CodigoServico = Utils.ToInt32(registro.Substring(009, 2)),
                    LiteralServico = registro.Substring(011, 15),
                    CodigoEmpresa = registro.Substring(026, 20),
                    NomeEmpresa = registro.Substring(046, 30),
                    CodigoBanco = Utils.ToInt32(registro.Substring(076, 3)),
                    NomeBanco = registro.Substring(079, 15),
                    DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##")),
                    Densidade = Utils.ToInt32(registro.Substring(100, 8)),
                    NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(108, 5)),
                    ComplementoRegistro2 = registro.Substring(113, 266),
                    DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(379, 6)).ToString("##-##-##")),
                    ComplementoRegistro3 = registro.Substring(385, 9),
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
                string complemento = new string(' ', 277);
                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += Utils.FitStringLength(cedente.Codigo.ToString(), 20, 20, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "237";
                _header += "BRADESCO       ";
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "        ";
                _header += "MX";
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);
                _header += complemento;
                _header += "000001";

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

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>

        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        throw new Exception("Mensagem Variavel nao existe para o tipo CNAB 240.");
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarMensagemVariavelRemessaCNAB400(boleto, ref numeroRegistro, tipoArquivo);
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

                // USO DO BANCO - Identificação da operação no Banco (posição 87 a 107)
                string identificaOperacaoBanco = new string(' ', 10);
                string nrDeControle = Utils.FitStringLength(boleto.NumeroDocumento.TrimStart(' '), 25, 25, ' ', 0, true, true, false);
                string mensagem = new string(' ', 12);
                string mensagem2 = new string(' ', 60);

                string usoBanco = new string(' ', 10);
                string _detalhe;
                //detalhe                           (tamanho,tipo) A= Alfanumerico, N= Numerico
                _detalhe = "1"; //Identificação do Registro         (1, N)

                //Parte Não Necessaria - Parte de dados do Sacado
                _detalhe += "00000"; //Agencia de Debito            (5, N) Não Usado
                _detalhe += " "; //Dig da Agencia                   (1, A) Não Usado
                _detalhe += "00000"; //Razao da Conta Corrente      (5, N) Não Usado
                _detalhe += "0000000"; //Conta Corrente             (7, N) Não Usado
                _detalhe += " "; //Dig da Conta Corrente            (1, A) Não Usado

                //Identificação da Empresa Cedente no Banco (17, A)
                _detalhe += "0";
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); // Codigo da carteira (3)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); //N da agencia(5)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); //Conta Corrente(7)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//D da conta(1)
                //Nº de Controle do Participante - uso livre da empresa (25, A)  //  brancos
                _detalhe += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                //Código do Banco, só deve ser preenchido quando cliente cedente optar por "Débito Automático".
                _detalhe += "000";
                //0=sem multa, 2=com multa (1, N)
                if (boleto.PercMulta > 0)
                {
                    _detalhe += "2";
                    _detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 4, 4, '0', 0, true, true, true); //Percentual Multa 9(2)V99 - (04)
                }
                else
                {
                    _detalhe += "0";
                    _detalhe += "0000";
                }

                //Identificação do Título no Banco (12, A)
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Numero (11)

                // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012
                _detalhe += Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // Digito de Auto Conferencia do Nosso Número (01)
                //Desconto Bonificação por dia (10, N)
                _detalhe += "0000000000";

                // 1 = Banco emite e Processa o registro
                // 2 = Cliente emite e o Banco somente processa
                //Condição para Emissão da Papeleta de Cobrança(1, N)
                _detalhe += boleto.ApenasRegistrar ? "2" : "1";
                //Ident. se emite papeleta para Débito Automático (1, A)
                _detalhe += "N";
                //Identificação da Operação do Banco (10, A) Em Branco
                _detalhe += identificaOperacaoBanco;

                //Indicador de Rateio de Crédito (1, A)
                //Somente deverá ser preenchido com a Letra “R”, se a Empresa participa da rotina 
                // de rateio de crédito, caso não participe, informar Branco.
                _detalhe += " ";

                //Endereçamento para Aviso do Débito Automático em Conta Corrente (1, N)
                //1 = emite aviso, e assume o endereço do Sacado constante do Arquivo-Remessa;
                //2 = não emite aviso;
                //diferente de 1 ou 2 = emite e assume o endereço do cliente debitado, constante do nosso cadastro.
                _detalhe += "2";

                _detalhe += "  "; //Branco (2, A)

                //Identificação ocorrência(2, N)
                /*
                01..Remessa
                02..Pedido de baixa
                04..Concessão de abatimento
                05..Cancelamento de abatimento concedido
                06..Alteração de vencimento
                07..Alteração do controle do participante
                08..Alteração de seu número
                09..Pedido de protesto
                18..Sustar protesto e baixar Título
                19..Sustar protesto e manter em carteira
                31..Alteração de outros dados
                35..Desagendamento do débito automático
                68..Acerto nos dados do rateio de Crédito
                69..Cancelamento do rateio de crédito.
                */
                if (boleto.Remessa == null || string.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia.Trim()))
                {
                    _detalhe += "01";
                }
                else
                {
                    _detalhe += boleto.Remessa.CodigoOcorrencia.PadLeft(2, '0');
                }


                _detalhe += Utils.Right(boleto.NumeroDocumento, 10, '0', true); //Nº do Documento (10, A)
                _detalhe += boleto.DataVencimento.ToString("ddMMyy"); //Data do Vencimento do Título (10, N) DDMMAA

                //Valor do Título (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                _detalhe += "000"; //Banco Encarregado da Cobrança (3, N)
                _detalhe += "00000"; //Agência Depositária (5, N)

                /*Espécie de Título (2,N)
                * 01-Duplicata
                02-Nota Promissória
                03-Nota de Seguro
                04-Cobrança Seriada
                05-Recibo
                10-Letras de Câmbio
                11-Nota de Débito
                12-Duplicata de Serv.
                99-Outros
                */
                //_detalhe += "99";
                _detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);

                _detalhe += "N"; //Identificação (1, A) A – aceito; N - não aceito
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy"); //Data da emissão do Título (6, N) DDMMAA

                //Valida se tem instrução no list de instruções, repassa ao arquivo de remessa
                string vInstrucao1 = "00"; //1ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                string vInstrucao2 = "00"; //2ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Bradesco.Protestar:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.NaoProtestar:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.DevolverAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                    }
                }
                _detalhe += vInstrucao1; //posições: 157 a 158 do leiaute
                _detalhe += vInstrucao2; //posições: 159 a 160 do leiaute
                //

                // Valor a ser cobrado por Dia de Atraso (13, N)
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Data Limite P/Concessão de Desconto (06, N)
                //if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDesconto == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                {
                    _detalhe += "000000"; //Caso nao tenha data de vencimento
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //Valor do Desconto (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Valor do IOF (13, N)
                _detalhe += Utils.FitStringLength(boleto.IOF.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Valor do Abatimento a ser concedido ou cancelado (13, N)
                _detalhe += Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                /*Identificação do Tipo de Inscrição do Sacado (02, N)
                *01-CPF
                02-CNPJ
                03-PIS/PASEP
                98-Não tem
                99-Outros 
                00-Outros 
                */
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //Nº Inscrição do Sacado (14, N)
                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpf_Cnpj, 14, 14, '0', 0, true, true, true);

                //Nome do Sacado (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço Completo (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumero.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //1ª Mensagem (12, A)
                /*Campo livre para uso da Empresa. A mensagem enviada nesse campo será impressa
                somente no boleto e não será confirmada no Arquivo Retorno.
                */
                _detalhe += Utils.FitStringLength(mensagem, 12, 12, ' ', 0, true, true, false);

                //CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true);

                //Sacador|Avalista ou 2ª Mensagem (60, A)
                _detalhe += Utils.FitStringLength(mensagem2, 60, 60, ' ', 0, true, true, false);

                //Nº Seqüencial do Registro (06, N)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

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
                string complemento = new string(' ', 393);
                string _trailer;

                _trailer = "9";
                _trailer += complemento;
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

        public string GerarMensagemVariavelRemessaCNAB400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = "";
                if (!string.IsNullOrEmpty(_detalhe))
                    _detalhe += Environment.NewLine;

                //Código do registro = 2 (Recibo do Sacado) 3, 4, 5, 6 e 7 (Ficha de Compensação) ==> 001 - 001
                _detalhe += "2";
                //Mensagem 1 ==> 002 a 081
                //Mensagem 2 ==> 082 a 161
                //Mensagem 3 ==> 162 a 241
                //Mensagem 4 ==> 242 a 321
                for (int i = 0; i < 4; i++)
                {
                    if (boleto.Instrucoes.Count > i)
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[i].Descricao, 80, 80, ' ', 0, true, true, false);
                    else
                        _detalhe += new string(' ', 80);
                }

                //Data limite para concessão de Desconto 2 ==> 322 a 327
                _detalhe += new string('0', 6);

                //Valor do Desconto - 328 a 340
                _detalhe += new string('0', 13);

                //Data limite para concessão de Desconto 3 - 341 a 346
                _detalhe += new string('0', 6);

                //Valor do Desconto - 347 a 359
                _detalhe += new string('0', 13);

                //Reserva - 360 a 366
                _detalhe += new string(' ', 7);

                //Carteira - 367 a 369
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);
                //Carteira - 370 a 374
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                //Carteira - 375 a 381
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true);
                //Carteira - 382 a 382
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//D da conta(1)

                //Nosso Número - 383 a 393
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Numero (11)

                // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012 - 394 a 394
                _detalhe += Utils.FitStringLength(CalcularDigitoNossoNumero(boleto), 1, 1, '0', 0, true, true, true); //DAC Nosso Número (1, A)

                //Número sequêncial do registro no arquivo ==> 395 a 400

                //Nº Seqüencial do Registro (06, N) - 395 a 400
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarRegistroDetalhe2(Boleto boleto, int numeroRegistro)
        {
            string _detalhe = "";
            _detalhe += "2";                                        // 001 a 001 Tipo Registro
            _detalhe += new string(' ', 320);                       // 002 a 321 Mensagens 1,2,3,4
            if (boleto.DataOutrosDescontos == DateTime.MinValue)    // 322 a 327 Data limite para concessão de Desconto 2
            {
                _detalhe += "000000"; //Caso nao tenha data de vencimento
            }
            else
            {
                _detalhe += boleto.DataOutrosDescontos.ToString("ddMMyy");
            }

            // 328 a 340 Valor do Desconto 2
            _detalhe += Utils.FitStringLength(boleto.OutrosDescontos.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
            _detalhe += "000000"; // 341 a 346 ata limite para concessão de Desconto 3
            // 347 a 359 Valor do Desconto 3
            _detalhe += Utils.FitStringLength("", 13, 13, '0', 0, true, true, true);
            _detalhe += new string(' ', 7);          // 360 a 366 Filler 
            _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);  // 367 a 369  Nº da Carteira 
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); // 370 a 374 N da agencia(5)
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); // 375 a 381 Conta Corrente(7)
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);// 382 a 382 D da conta(1)
            _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); // 383 a 393 Nosso Número (11)
            // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012
            _detalhe += Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // 394 a 394 Digito de Auto Conferencia do Nosso Número (01)
            //Desconto Bonificação por dia (10, N)
            _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // 395 a 400
            //Retorno
            return Utils.SubstituiCaracteresEspeciais(_detalhe);
        }

    }
}
