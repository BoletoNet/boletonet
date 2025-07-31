using BoletoNet.EDI.Banco;
using BoletoNet.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.748.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <Author>
    /// Samuel Schmidt - Sicredi Nordeste RS / Felipe Eduardo - RS
    /// </Author>
    internal class Banco_Sicredi : AbstractBanco, IBanco
    {
        private static readonly Dictionary<int, string> carteirasDisponiveis = new Dictionary<int, string>() {
            { 1, "Com Registro" },
            { 3, "Sem Registro" }
        };

        private HeaderRetorno header;

        /// <author>
        /// Classe responsavel em criar os campos do Banco Sicredi.
        /// </author>
        internal Banco_Sicredi()
        {
            this.Codigo = 748;
            this.Digito = "X";
            this.Nome = "Banco Sicredi";
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //Formata o tamanho do Número da Agência
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Formata o tamanho do Número da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 5)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Atã o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;
            else boleto.LocalPagamento = "PAGãVEL PREFERENCIALMENTE NAS COOPERATIVAS DE Crédito DO SICREDI";

            //Verifica se data do processamento ã valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento ã valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            string infoFormatoCodigoCedente = "formato AAAAPPCCCCC, onde: AAAA = Número da Agência, PP = Posto do beneficiãrio, CCCCC = Código do beneficiãrio";

            var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 11);

            if (string.IsNullOrEmpty(codigoCedente))
                throw new BoletoNetException("Código do cedente deve ser informado, " + infoFormatoCodigoCedente);

            var conta = boleto.Cedente.ContaBancaria.Conta;
            if (boleto.Cedente.ContaBancaria != null &&
                (!codigoCedente.StartsWith(boleto.Cedente.ContaBancaria.Agencia) ||
                 !(codigoCedente.EndsWith(conta) || codigoCedente.EndsWith(conta.Substring(0, conta.Length - 1)))))
                //throw new BoletoNetException("Código do cedente deve estar no " + infoFormatoCodigoCedente);
                boleto.Cedente.Codigo = string.Format("{0}{1}{2}", boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, (boleto.Cedente.Codigo.Length >= 5 ? Utils.Right(boleto.Cedente.Codigo, 5, '0', true) : Utils.Right((boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente), 5, '0', true)));

            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new BoletoNetException("Tipo de carteira ã obrigatãrio. " + ObterInformacoesCarteirasDisponiveis());

            if (!CarteiraValida(boleto.Carteira))
                throw new BoletoNetException("Carteira informada ã invãlida. Informe " + ObterInformacoesCarteirasDisponiveis());

            //Verifica se o nosso Número ã vãlido
            var Length_NN = boleto.NossoNumero.Length;
            switch (Length_NN)
            {
                case 9:
                    boleto.NossoNumero = boleto.NossoNumero.Substring(0, Length_NN - 1);
                    boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                    boleto.NossoNumero += boleto.DigitoNossoNumero;
                    break;
                case 8:
                    boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                    boleto.NossoNumero += boleto.DigitoNossoNumero;
                    break;
                case 6:
                    boleto.NossoNumero = DateTime.Now.ToString("yy") + boleto.NossoNumero;
                    boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                    boleto.NossoNumero += boleto.DigitoNossoNumero;
                    break;
                default:
                    throw new NotImplementedException("Nosso Número invãlido");
            }

            FormataCodigoBarra(boleto);
            if (boleto.CodigoBarra.Codigo.Length != 44)
                throw new BoletoNetException("Código de barras ã invãlido");

            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        private string ObterInformacoesCarteirasDisponiveis()
        {
            return string.Join(", ", carteirasDisponiveis.Select(o => string.Format("ã{0}ã ã {1}", o.Key, o.Value)));
        }

        private bool CarteiraValida(string carteira)
        {
            int tipoCarteira;
            if (int.TryParse(carteira, out tipoCarteira))
            {
                return carteirasDisponiveis.ContainsKey(tipoCarteira);
            }
            return false;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            string nossoNumero = boleto.NossoNumero;

            if (nossoNumero == null || nossoNumero.Length != 9)
            {
                throw new Exception("Erro ao tentar formatar nosso Número, verifique o tamanho do campo");
            }

            try
            {
                boleto.NossoNumero = string.Format("{0}/{1}-{2}", nossoNumero.Substring(0, 2), nossoNumero.Substring(2, 6), nossoNumero.Substring(8));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso Número", ex);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função do fomata Número do documento não implementada.");
        }
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1  CCCCC.CCNNNd2  NNNNN.041XXd3  V FFFF9999999999

            string campo1 = "7489" + boleto.CodigoBarra.Codigo.Substring(19, 5);
            int d1 = Mod10Sicredi(campo1);
            campo1 = FormataCampoLD(campo1) + d1.ToString();

            string campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            int d2 = Mod10Sicredi(campo2);
            campo2 = FormataCampoLD(campo2) + d2.ToString();

            string campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            int d3 = Mod10Sicredi(campo3);
            campo3 = FormataCampoLD(campo3) + d3.ToString();

            string campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            string campo5 = boleto.CodigoBarra.Codigo.Substring(5, 14);

            boleto.CodigoBarra.LinhaDigitavel = campo1 + "  " + campo2 + "  " + campo3 + "  " + campo4 + "  " + campo5;
        }
        private string FormataCampoLD(string campo)
        {
            return string.Format("{0}.{1}", campo.Substring(0, 5), campo.Substring(5));
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            var codigoCobranca = 1; //Código de Cobrança com registro
            string cmp_livre =
                codigoCobranca +
                boleto.Carteira +
                Utils.FormatCode(boleto.NossoNumero, 9) +
                Utils.FormatCode(boleto.Cedente.Codigo, 11) + "10";

            string dv_cmpLivre = digSicredi(cmp_livre).ToString();

            var codigoTemp = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, dv_cmpLivre);

            boleto.CodigoBarra.CampoLivre = cmp_livre;
            boleto.CodigoBarra.FatorVencimento = FatorVencimento(boleto);
            boleto.CodigoBarra.Moeda = 9;
            boleto.CodigoBarra.ValorDocumento = valorBoleto;

            int _dacBoleto = digSicredi(codigoTemp);

            if (_dacBoleto == 0 || _dacBoleto > 9)
                _dacBoleto = 1;

            boleto.CodigoBarra.Codigo = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, dv_cmpLivre, _dacBoleto);
        }

        private string GerarCodigoDeBarras(Boleto boleto, string valorBoleto, string cmp_livre, string dv_cmpLivre, int? dv_geral = null)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}",
                Utils.FormatCode(Codigo.ToString(), 3),
                boleto.Moeda,
                dv_geral.HasValue ? dv_geral.Value.ToString() : string.Empty,
                FatorVencimento(boleto),
                valorBoleto,
                cmp_livre,
                dv_cmpLivre);
        }

        //public bool RegistroByCarteira(Boleto boleto)
        //{
        //    bool valida = false;
        //    if (boleto.Carteira == "112"
        //        || boleto.Carteira == "115"
        //        || boleto.Carteira == "104"
        //        || boleto.Carteira == "147"
        //        || boleto.Carteira == "188"
        //        || boleto.Carteira == "108"
        //        || boleto.Carteira == "109"
        //        || boleto.Carteira == "150"
        //        || boleto.Carteira == "121")
        //        valida = true;
        //    return valida;
        //}

        #region Mãtodos de Geração do Arquivo de Remessa
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                //base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
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

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                detalhe += Utils.FormatCode("", "0", 4, true);
                detalhe += "3";
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), 5);
                detalhe += "P 01";
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 5);
                detalhe += "0";
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 12);
                detalhe += boleto.Cedente.ContaBancaria.DigitoConta;
                detalhe += " ";
                detalhe += Utils.FormatCode(boleto.NossoNumero.Replace("/", "").Replace("-", ""), 20);
                detalhe += "1";
                detalhe += (Convert.ToInt16(boleto.Carteira) == 1 ? "1" : "2");
                detalhe += "122";
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, 15);
                detalhe += boleto.DataVencimento.ToString("ddMMyyyy");
                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += "00000 99A";
                detalhe += boleto.DataDocumento.ToString("ddMMyyyy");
                detalhe += "200000000";
                valorBoleto = boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += "1";
                detalhe += boleto.DataDesconto.ToString("ddMMyyyy");
                valorBoleto = boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += Utils.FormatCode("", 26);
                detalhe += Utils.FormatCode("", " ", 25);
                detalhe += "0001060090000000000 ";

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB240.", e);
            }
        }

        /// <summary>
        /// Função que gera nosso numero a ser colocado na remessa sicoob CNAB240, segundo layout para troca de informaçães
        /// </summary>
        /// <param name="boleto"></param>
        /// <returns></returns>
        private string NossoNumeroFormatado(Boleto boleto)
        {
            /*
             * Identificação do título no banco
             * Número adotado pelo banco cedente para identificar o título.
             * Para Código de movimento igual a '01' (entrada de títulos), caso esteja preenchido com zeros, a numerçãão serã feita pelo banco.
             * A identificação do título no SICREDI/nosso Número composta por nove Dígitos, conforme descrição do item 3.6, pãgina 39.
             * 
             * 3.6.2 Geração pelo cedente
             * 1. A parte sequencial do nosso Número ã controlada pelo cedente
             * 2. o sequencial do nosso Número não poderã ser repetido para que não haja títulos com o mesmo nosso Número
             * 3. o cedente deverã enviar o nosso Número calculado de acordo com a descrição na prãxima pãgina, abaixo o leiauto de como ficarã o nosso Número nos bloquetos:
             * YY = Ano da geração do título
             * B = Geração no nosso Número: 1 - Cooperativa de Crédito / Agência cedente. 2 a 9 - Cedente.
             * nnnn = Número sequencial por cedente
             * d = Dígito verificador, calculado atravãs do mãdulo 11.
             * 
             * OBS: A cada ãnicio de ano, o Número sequencial deve ser reinicializado.
             * 
             *  -> Ano: relacionado ao ano atual que este nosso Número esta sendo gerado. Exemplo: 2021
             *  -> byte: relacionado ao controle de quem emite, estão disponãveis os valores 2 a 9 onde 2 o cedente que emite e 1 somente a cooperativa pode utilizar( para boletos prã-impressos )
             *  -> Sequencial: relacionado ao nosso Número de controle ãnico do boleto
             *  -> Dv do sequencial: esta relacionado ao digito verificador do nosso Número
             *  -> Ao realizar a junção de todas estas informaçães, temos o seguinte valor para o banco

             *  -> AABXXXXD( 21/20004-1 ) onde as letras
             *  -> "A" representam o ano, 
             *  -> "B" byte, 
             *  -> "X" Nosso Número,
             *  -> "D" digito verificador
             *    
             */
            
            string vAuxNossoNumeroComDV = boleto.NossoNumero;
            if (string.IsNullOrEmpty(boleto.DigitoNossoNumero) || boleto.NossoNumero.Length < 9)
            {
                boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                vAuxNossoNumeroComDV = boleto.NossoNumero + boleto.DigitoNossoNumero;
            }

            string retorno = vAuxNossoNumeroComDV;
            retorno = Utils.FormatCode(retorno, " ", 20, false);
            return retorno;
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3);                                // Posição 001 a 003    Código do Sicoob na Compensação: "756"
                detalhe += "0001";                                                                      // Posição 004 a 007    Lote de serviço
                detalhe += "3";                                                                         // Posição 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                   // Posição 009 a 013    Número Sequencial
                detalhe += "P";                                                                         // Posição 014          Cód. Segmento do Registro Detalhe: "P"
                detalhe += " ";                                                                         // Posição 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa.CodigoOcorrencia ?? "1", "0", 2, true);      // Posição 016 a 017    '01' = Entrada de títulos, '02' = Solicitação de Baixa, '06' = Prorrogação de Vencimento, '09' = Protestar, '10' = Desistãncia do Protesto e Baixar título, '11' = Desistãncia do Protesto e manter em carteira, '31' = Alteraçães de outros dados
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 5);                   // Posição 018 a 022    Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoAgencia, " ", 1, true);  // Posição 023          Dígito Verificador do Prefixo: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 12);                    // Posição 024 a 035    Conta Corrente: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 1);               // Posição 036          Dígito Verificador da Conta: vide planilha "Capa" deste arquivo
                detalhe += " ";                                                                         // Posição 037          Dígito Verificador da Ag/Conta: Brancos
                detalhe += Utils.FormatCode(NossoNumeroFormatado(boleto), 20);                          // Posição 038 a 057    Nosso Número
                detalhe += (Convert.ToInt16(boleto.Carteira) == 1 ? "1" : "2");                         // Posição 058          Código da Carteira: vide planilha "Capa" deste arquivo
                detalhe += "1";                                                                         // Posição 059          Forma de Cadastr. do título no Banco: "1" - Cobrança com registro
                detalhe += "1";                                                                         // Posição 060          Tipo de Documento: Deve ser informado "1" ou "2" 1 - Tradicional 2 - Escritural Obs.: O Sicredi não realizarã diferenciação entre os domãnios
                detalhe += "2";                                                                         // Posição 061          Identificação da Emissão do Boleto: 1=SICREDI emite 2=CEDENTE emite
                detalhe += "2";                                                                         // Posição 062          Identificação da distribuição do Boleto: 1=SICREDI distribui 2=CEDENTE distribui
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 15);                           // Posição 063 a 077    Número do documento de cobrança.
                detalhe += Utils.FormatCode(boleto.DataVencimento.ToString("ddMMyyyy"), 8);             // Posição 078 a 085    Número do documento de cobrança.

                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                valorBoleto = Utils.FormatCode(valorBoleto, 15);
                detalhe += valorBoleto;                                                                 // Posição 86 a 100     Valor Nominal do título
                detalhe += "00000";                                                                     // Posição 101 a 105    Coop/Agência Encarregada da Cobrança: "00000"
                detalhe += new string(' ', 1);                                                          // Posição 106          Dígito Verificador da Agência: Brancos
                detalhe += Utils.FormatCode(boleto.EspecieDocumento.Codigo, 2);                         // Posição 107 a 108    Espãcie do título
                detalhe += Utils.FormatCode(boleto.Aceite, 1);                                          // Posição 109          Identificação do título Aceito/Não Aceito
                detalhe += Utils.FormatCode(boleto.DataProcessamento.ToString("ddMMyyyy"), 8);          // Posição 110 a 117    Data Emissão do título
                detalhe += Utils.FormatCode(boleto.CodJurosMora, "1", 1);                               // Posição 118          Código do juros mora. 1 = Valor monetãrio, 2 = Taxa mensal, 3 = Isento
                detalhe += new string('0', 8);                                                          // Posição 119 a 126    Data do Juros de Mora: O SICREDI não utiliarã esse campo.
                detalhe += Utils.FormatCode(boleto.CodJurosMora == "0" || boleto.CodJurosMora == "3" ? "".PadLeft(15, '0') : (boleto.CodJurosMora == "1" ? boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", "") : boleto.PercJurosMora.ToString("f").Replace(",", "").Replace(".", "")), 15);   // Posição 127 a 141  - Juro de mora por dia/taxa, valor dsobre o titulo a ser cobrado de juros de mora.

                if (boleto.DataDesconto > DateTime.MinValue)
                {
                    detalhe += "1";                                                                     // Posição 142          Código do desconto
                    detalhe += Utils.FormatCode(boleto.DataDesconto.ToString("ddMMyyyy"), 8);           // Posição 143 a 150    Data do Desconto 1
                    detalhe += Utils.FormatCode(boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", ""), 15);
                }
                else
                {
                    detalhe += "0";                                                                     // Posição 142          Código do desconto - Sem Desconto
                    detalhe += Utils.FormatCode("", "0", 8, true); ;                                    // Posição 143 a 150    Data do Desconto
                    detalhe += Utils.FormatCode("", "0", 15, true);
                }

                //detalhe += Utils.FormatCode(boleto.IOF.ToString(), 15);                                 // Posição 166 a 180    Valor do IOF a ser Recolhido -> O Sicredi não utiliza esse campo, preencher com zeros
                detalhe += Utils.FormatCode("", "0", 15, true);                                         // Posição 166 a 180    Valor do IOF a ser Recolhido -> O Sicredi não utiliza esse campo, preencher com zeros
                detalhe += Utils.FormatCode(boleto.Abatimento.ToString(), 15);                          // Posição 181 a 195    Valor do Abatimento
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 25);                           // Posição 196 a 220    Identificação do título
                detalhe += "3";                                                                         // Posição 221          Código do protesto: 1 = Protestar dias corridos, 3 = Nao Protestar, 9 = Cancelamento protesto Automático.

                #region Instruçães

                string vInstrucao1 = "00"; //2º instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicoob)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicoob.CobrarJuros:
                            vInstrucao1 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }

                #endregion

                detalhe += Utils.FormatCode(vInstrucao1, 2);                                            // Posição 222 a 223    Código do protesto
                detalhe += Utils.FormatCode("1", 1);                                                    // Posição 224          Código para Baixa/Devolução: "1" = Baixar/devolver
                //detalhe += Utils.FormatCode("60", " ", 3);                                              // Posição 225 A 227    Número de Dias para Baixa/Devolução: Utilizar sempre, nesse campo, 60 dias para baixa/devolução.
                detalhe += "000";                                                                       // Posição 225 A 227    Número de Dias para Baixa/Devolução: Utilizar sempre, nesse campo, 60 dias para baixa/devolução.
                detalhe += Utils.FormatCode(boleto.Moeda.ToString(), "0", 2, true);                     // Posição 228 A 229    Código da Moeda - 09
                detalhe += Utils.FormatCode("", "0", 10, true);                                         // Posição 230 A 239    Nã do Contrato da Operação de Crãd.: "0000000000"
                detalhe += " ";
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);             // Posição 001 a 003    Código do Sicredi na Compensação: "748"
                detalhe += "0001";                                                              // Posição 004 a 007    Lote
                detalhe += "3";                                                                 // Posição 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);           // Posição 009 a 013    Número Sequencial
                detalhe += "Q";                                                                 // Posição 014          Cód. Segmento do Registro Detalhe: "P"
                detalhe += " ";                                                                 // Posição 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa.CodigoOcorrencia ?? "1", "0", 2, true); // Posição 016 a 017 '01' = Entrada de títulos, '02' = Solicitação de Baixa, '06' = Prorrogação de Vencimento, '09' = Protestar, '10' = Desistãncia do Protesto e Baixar título, '11' = Desistãncia do Protesto e manter em carteira, '31' = Alteraçães de outros dados
                detalhe += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");                    // Posição 018          1=CPF    2=CGC/CNPJ
                detalhe += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15, true);              // Posição 019 a 033    Número de Inscrição da Empresa
                detalhe += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);                       // Posição 034 a 73     Nome
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);               // Posição 074 a 113    Endereão
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);            // Posição 114 a 128    Bairro 
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.CEP, 8);                     // Posição 129 a 136    CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);            // Posição 137 a 151    Cidade 
                detalhe += boleto.Sacado.Endereco.UF;                                           // Posição 152 a 153    Unidade da Federação
                //detalhe += (boleto.Cedente.CPFCNPJ.Length == 11 ? "1" : "2");                   // Posição 154          Tipo de Inscrição Sacador avalista
                //detalhe += Utils.FormatCode(boleto.Cedente.CPFCNPJ, "0", 15, true);             // Posição 155 a 169    Número de Inscrição / Sacador avalista
                //detalhe += Utils.FormatCode(boleto.Cedente.Nome, " ", 40);                      // Posição 170 a 209    Nome / Sacador avalista
                detalhe += "0";                                                                 // Posição 154          Beneficiario final / Tipo pessoa -> 0 - Sem Beneficiãrio Final, 1 = CPF, 2 = CNPJ
                detalhe += Utils.FormatCode("", "0", 15, true);                                 // Posição 155 a 169    Beneficiario final / CPF/CNPJ   
                detalhe += Utils.FormatCode("", " ", 40);                                       // Posição 170 a 209    Nome do Beneficiario Final
                detalhe += "000";                                                               // Posição 210 a 212    Código Bco. Corresp. na Compensação
                detalhe += Utils.FormatCode("", " ", 20);                                       // Posição 213 a 232    Nosso Nã no Banco Correspondente "1323739"
                detalhe += Utils.FormatCode("", " ", 8);                                        // Posição 233 a 240    Uso Exclusivo FEBRABAN/CNAB
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe).ToUpper();
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3);                                                        // Posição 001 a 003    Código do Sicoob na Compensação: "748"
                detalhe += "0001";                                                                                              // Posição 004 a 007    Lote de serviço
                detalhe += "3";                                                                                                 // Posição 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                                           // Posição 009 a 013    Número Sequencial do registro no lote
                detalhe += "R";                                                                                                 // Posição 014          Cód. Segmento do Registro Detalhe: "R"
                detalhe += " ";                                                                                                 // Posição 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa.CodigoOcorrencia ?? "1", "0", 2, true);                              // Posição 016 a 017    '01' = Entrada de títulos, '02' = Solicitação de Baixa, '06' = Prorrogação de Vencimento, '09' = Protestar, '10' = Desistãncia do Protesto e Baixar título, '11' = Desistãncia do Protesto e manter em carteira, '31' = Alteraçães de outros dados

                if (boleto.DataOutrosDescontos > DateTime.MinValue)
                {
                    detalhe += "1";                                                                                             // Posição 18           Código do desconto 2
                    detalhe += Utils.FormatCode(boleto.DataOutrosDescontos.ToString("ddMMyyyy"), 8);                            // Posição 19 a 26      Data do Desconto 2
                    detalhe += Utils.FormatCode(boleto.OutrosDescontos.ToString("f").Replace(",", "").Replace(".", ""), 15);    // Posição 27 a 41      Valor do Desconto 2
                }
                else
                {
                    detalhe += "0";                                                                                             // Posição 18           Código do desconto 2
                    detalhe += Utils.FormatCode("", "0", 8, true);                                                              // Posição 19 a 26      Data do Desconto 2
                    detalhe += Utils.FormatCode("", "0", 15, true);                                                             // Posição 27 a 41      Valor/percentual a ser concedido
                }

                detalhe += "0";                                                                                                 // Posição 42           Código da desconto 3
                detalhe += Utils.FormatCode("", "0", 8, true);                                                                  // Posição 43 a 50      Data do Desconto 3                
                detalhe += Utils.FormatCode("", "0", 15, true);                                                                 // Posição 51 a 65      Valor/percentual a ser concedido

                if (boleto.PercMulta > 0)
                {
                    // Código da multa 2 - percentual
                    detalhe += "2";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);                                      // Posição 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);       // Posição 75 a 89      Valor/percentual a ser aplilcado
                }
                else if (boleto.ValorMulta > 0)
                {
                    // Código da multa 1 - valor fixo
                    detalhe += "1";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);                                      // Posição 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);      // Posição 75 a 89      Valor/percentual a ser aplilcado
                }
                else
                {
                    // Código da multa 0 - sem multa
                    detalhe += "0";
                    detalhe += Utils.FormatCode("", "0", 8);                                                                    // Posição 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);                                    // Posição 75 a 89      Valor/percentual a ser aplilcado
                }

                detalhe += Utils.FormatCode("", " ", 10);       // Posição 90 a 99 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 40);       // Posição 100 a 139 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 40);       // Posição 140 a 179 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 20);       // Posição 180 a 199 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode("", "0", 8, true);  // Posição 200 a 207  Cód. Ocor. do Pagador: "00000000"
                detalhe += Utils.FormatCode("", "0", 3, true);  // Posição 208 a 210  Cód. do Banco na Conta do Débito: "000"
                detalhe += Utils.FormatCode("", "0", 5, true);  // Posição 211 a 215  Código da Agência do Débito: "00000"
                detalhe += " ";                                 // Posição 216 Dígito Verificador da Agência: Brancos
                detalhe += Utils.FormatCode("", "0", 12, true); // Posição 217 a 228  Conta Corrente para Débito: "000000000000"
                detalhe += " ";                                 // Posição 229  Verificador da Conta: Brancos
                detalhe += " ";                                 // Posição 230  Verificador Ag/Conta: Brancos
                detalhe += "0";                                 // Posição 231  Aviso para Débito Automático: "0"
                detalhe += Utils.FormatCode("", " ", 9);        // Posição Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);     // Posição 001 a 003    Código do banco
                trailer += "0001";                                                      // Posição 004 a 007    Lote de serviço
                trailer += "5";                                                         // Posição 008          Tipo de registro = 5    
                trailer += Utils.FormatCode("", " ", 9);                                // Posição 009 a 017    Exclusivo FEBRABAN/CNAB: Brancos    - Brancos
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);   // Posição 018 a 023    Quantidade de registros do lote
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posição 024 a 029    Quantidade de títulos em Cobrança    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posição 030 a 046    Valor total dos títulos em carteira     
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posição 047 a 052    Quantidade de títulos em Cobrança    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posição 053 a 069    Valor total dos títulos em carteira    
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posição 070 a 075    Quantidade de títulos em Cobrança    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posição 076 a 092    Quantidade de títulos em carteiras
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posição 093 a 098    Quantidade de títulos em Cobrança
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posição 099 a 115    Valor total dos títulos em carteira
                trailer += Utils.FormatCode("", " ", 8, true);                          // Posição 116 a 123    Número do aviso de lanãamento       - Brancos
                trailer += Utils.FormatCode("", " ", 117);                              // Posição 124 a 240    Uso Exclusivo FEBRABAN/CNAB         - Brancos
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);     // Posição 001 a 003    Código do banco na Compensação
                trailer += "9999";                                                      // Posição 004 a 007    Lote de serviço
                trailer += "9";                                                         // Posição 008          Lote de serviço
                trailer += Utils.FormatCode("", " ", 9);                                // Posição 009 a 017    Uso exclusivo FEBRABAN/CNAB
                trailer += Utils.FormatCode("1", "0", 6, true);                         // Posição 018 a 023    Quantidade de lotes do arquivo
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);   // Posição 024 a 029    Quantidade de registros do arquivo
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posição 030 a 035    Quantidade de contas / conc. (lotes)
                trailer += Utils.FormatCode("", " ", 205);                              // Posição 036 a 240    Uso exclusivo FEBRABAN/CNAB

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = "748";                                                          // Posição 001 a 003    Código do Sicredi na Compensação: "748"
                header += "0001";                                                               // Posição 004 a 007    Lote de serviço      
                header += "1";                                                                  // Posição 008          Tipo de Registro: "1"
                header += "R";                                                                  // Posição 009          Tipo de operação    
                header += "01";                                                                 // Posição 010 a 011    Tipo de serviço: "01"
                header += new string(' ', 2);                                                   // Posição 012 a 013    Nº da Versão do Layout do Lote: "040"
                header += "040";                                                                // Posição 014 a 016    Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += new string(' ', 1);                                                   // Posição 017          Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                           // Posição 018          1=CPF    2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                     // Posição 019 a 033    Número de Inscrição da Empresa
                header += Utils.FormatCode(cedente.Convenio.ToString(), " ", 20);               // Posição 034 a 053    Código do Convenio no Sicredi: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);        // Posição 054 a 058    Agência mantenedora da conta
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, " ", 1);        // Posição 059 a 059    Digito verfiricador da agencia
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);         // Posição 060 a 071    Número da conta corrente
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);    // Posição 072 a 72     Dígito verificador da conta
                header += new string(' ', 1);                                                   // Posição 073          Dígito Verificador da coop/Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);                              // Posição 074 a 103    Nome da empresa
                header += Utils.FormatCode("", " ", 40);                                        // Posição 104 a 143    Informação 1: Brancos			
                header += Utils.FormatCode("", " ", 40);                                        // Posição 144 a 183    Informação 2: Brancos
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8, true);      // Posição 184 a 191    Número da remessa
                header += DateTime.Now.ToString("ddMMyyyy");                                    // Posição 192 a 199    Data de Gravação Remessa/Retorno
                header += Utils.FormatCode("", "0", 8, true);                                   // Posição 200 a 207    Data do Crédito: "00000000": Zeros
                header += new string(' ', 33);                                                  // Posição 208 a 240    Uso Exclusivo FEBRABAN/CNAB: Brancos
                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        // não tem no CNAB 400 header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            //Variaveis
            try
            {
                //Montagem do header
                string header = "748";                                                          //Posição 001 a 003  Código do Sicredi na Compensação: "756"
                header += "0000";                                                               //Posição 004 a 007  Lote de serviço: "0000"
                header += "0";                                                                  //Posição 008        Tipo de Registro: "0"
                header += Utils.FormatCode("", " ", 9);                                         //Posição 09 a 017   Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                           //Posição 018  1=CPF 2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true);                     //Posição 019 a 032  Número de Inscrição da Empresa
                header += Utils.FormatCode(cedente.Convenio.ToString(), " ", 20);               //Posição 033 a 052  Código do Convenio no Sicoob: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);        //Posição 053 a 057  Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                header += " ";                                                                  //Posição 058 a 058  Digito Agência
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);         //Posição 059 a 070
                header += cedente.ContaBancaria.DigitoConta;                                    //Posição 071 a 71
                header += " ";                                                                  //Posição 072 a 72   Dígito Verificador da Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);                              //Posição 073 a 102  Nome do Banco: SICOOB
                header += Utils.FormatCode("SICREDI", " ", 30);                                 //Posição 103 a 132  Nome da Empresa
                header += Utils.FormatCode("", " ", 10);                                        //Posição 133 a 142  Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += "1";                                                                  //Posição 143 a 143  Código Remessa / Retorno: "1"
                header += DateTime.Now.ToString("ddMMyyyyHHmmss");                              //Posição 144 a 151  Data de Geração do Arquivo  -   Posição 152 a 157  Hora de Geração do Arquivo
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);      //Posição 158 a 163  Número sequencial do arquivo
                header += "081";                                                                //Posição 164 a 166  No da Versão do Layout do Arquivo: "081"
                header += "01600";                                                              //Posição 167 a 171  Densidade de Gravação do Arquivo: "00000"
                header += Utils.FormatCode("", " ", 69);
                header = Utils.SubstituiCaracteresEspeciais(header);
                //Retorno
                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240(numeroRegistro);
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro, cedente);
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

        public string GerarTrailerRemessa240(int numeroRegistro)
        {
            try
            {
                string complemento = new string(' ', 205);
                string _trailer;

                _trailer = "74899999";
                _trailer += Utils.FormatCode("", " ", 9);
                _trailer += Utils.FormatCode("", 6);
                _trailer += Utils.FormatCode(numeroRegistro.ToString(), 6);
                _trailer += Utils.FormatCode("", 6);
                _trailer += complemento;

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region Mãtodos de Leitura do Arquivo de Retorno
        /*
         * Substituãdo Mãtodo de Leitura do Retorno pelo Interpretador de EDI;
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                int idRegistro = Utils.ToInt32(registro.Substring(0, 1));
                detalhe.IdentificacaoDoRegistro = idRegistro;

                detalhe.NossoNumero = registro.Substring(47, 15);

                int codigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                detalhe.CodigoOcorrencia = codigoOcorrencia;

                //Data Ocorrãncia no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                detalhe.SeuNumero = registro.Substring(116, 10);

                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                decimal valorTitulo = Convert.ToUInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                detalhe.EspecieTitulo = registro.Substring(174, 1);

                decimal despeasaDeCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.DespeasaDeCobranca = despeasaDeCobranca / 100;

                decimal outrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.OutrasDespesas = outrasDespesas / 100;

                decimal abatimentoConcedido = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.Abatimentos = abatimentoConcedido / 100;

                decimal descontoConcedido = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = descontoConcedido / 100;

                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

                int dataCredito = Utils.ToInt32(registro.Substring(328, 8));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("####-##-##"));

                detalhe.MotivosRejeicao = registro.Substring(318, 10);

                detalhe.NomeSacado = registro.Substring(19, 5);
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }
        */
        #endregion Mãtodos de Leitura do Arquivo de Retorno

        public int Mod10Sicredi(string seq)
        {
            /* Variãveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 2, r;

            for (int i = seq.Length - 1; i >= 0; i--)
            {

                r = (Convert.ToInt32(seq.Substring(i, 1)) * p);
                if (r > 9)
                    r = SomaDezena(r);
                s = s + r;
                if (p < b)
                    p++;
                else
                    p--;
            }

            d = Multiplo10(s);
            return d;
        }

        public int SomaDezena(int dezena)
        {
            string d = dezena.ToString();
            int d1 = Convert.ToInt32(d.Substring(0, 1));
            int d2 = Convert.ToInt32(d.Substring(1));
            return d1 + d2;
        }

        public int digSicredi(string seq)
        {
            /* Variãveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 9;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                s = s + (Convert.ToInt32(seq.Substring(i, 1)) * p);
                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            d = 11 - (s % 11);
            if (d > 9)
                d = 0;
            return d;
        }

        public string DigNossoNumeroSicredi(Boleto boleto, bool arquivoRemessa = false)
        {
            //Adicionado por diego.dariolli pois ao gerar remessa o Dígito saãa errado pois faltava Agência e posto no Código do cedente
            string codigoCedente = ""; //Código do beneficiãrio aaaappccccc
            if (arquivoRemessa)
            {
                if (string.IsNullOrEmpty(boleto.Cedente.ContaBancaria.OperacaConta))
                    throw new Exception("O Código do posto beneficiãrio não foi informado.");

                codigoCedente = string.Concat(boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, boleto.Cedente.Codigo);
            }
            else
                codigoCedente = boleto.Cedente.Codigo;

            string nossoNumero = boleto.NossoNumero; //ano atual (yy), indicador de geração do nosso Número (b) e o Número seqãencial do beneficiãrio (nnnnn);

            string seq = string.Concat(codigoCedente, nossoNumero); // = aaaappcccccyybnnnnn
            /* Variãveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 9;
            //Atribui os pesos de {2..9}
            for (int i = seq.Length - 1; i >= 0; i--)
            {
                s = s + (Convert.ToInt32(seq.Substring(i, 1)) * p);
                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }
            d = 11 - (s % 11);//Calcula o Mãdulo 11;
            if (d > 9)
                d = 0;
            return d.ToString();
        }


        /// <summary>
        /// Efetua as Validaçães dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB240:
                    //vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
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


        #region CNAB 400 - sidneiklein
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Prã Validaçães
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco ã Obrigatãrio!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiãrio ã Obrigatãrio!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Deverã existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    #region Validaçães da Remessa que deverão estar preenchidas quando SICREDI
                    //Comentado porque ainda estã fixado em 01
                    //if (String.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código de Ocorrãncia!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    if (String.IsNullOrEmpty(boleto.NumeroDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe um Número de Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        // Para o Sicredi, defini o Tipo de Documento sendo: 
                        //       A = 'A' - SICREDI com Registro
                        //      C1 = 'C' - SICREDI sem Registro Impressão Completa pelo Sicredi
                        //      C2 = 'C' - SICREDI sem Registro Pedido de bloquetos prã-impressos
                        // ** Isso porque são tratados 3 leiautes de escrita diferentes para o Detail da remessa;

                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.Remessa.TipoDocumento.Equals("A") && !boleto.Remessa.TipoDocumento.Equals("C1") && !boleto.Remessa.TipoDocumento.Equals("C2"))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Tipo de Documento Invãlido! Deverão ser: A = SICREDI com Registro; C1 = SICREDI sem Registro Impressão Completa pelo Sicredi;  C2 = SICREDI sem Registro Pedido de bloquetos prã-impressos", Environment.NewLine);
                        vRetorno = false;
                    }
                    //else if (boleto.Remessa.TipoDocumento.Equals("06") && !String.IsNullOrEmpty(boleto.NossoNumero))
                    //{
                    //    //Para o "Remessa.TipoDocumento = "06", não poderã ter NossoNumero Gerado!
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Não pode existir NossoNumero para o Tipo Documento '06 - Cobrança escritural'!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    else if (!boleto.EspecieDocumento.Codigo.Equals("A") && //A - Duplicata Mercantil por Indicação
                             !boleto.EspecieDocumento.Codigo.Equals("B") && //B - Duplicata Rural;
                             !boleto.EspecieDocumento.Codigo.Equals("C") && //C - Nota Promissãria;
                             !boleto.EspecieDocumento.Codigo.Equals("D") && //D - Nota Promissãria Rural;
                             !boleto.EspecieDocumento.Codigo.Equals("E") && //E - Nota de Seguros;
                             !boleto.EspecieDocumento.Codigo.Equals("F") && //G ã Recibo;

                             !boleto.EspecieDocumento.Codigo.Equals("H") && //H - Letra de Cãmbio;
                             !boleto.EspecieDocumento.Codigo.Equals("I") && //I - Nota de Débito;
                             !boleto.EspecieDocumento.Codigo.Equals("J") && //J - Duplicata de serviço por Indicação;
                             !boleto.EspecieDocumento.Codigo.Equals("O") && //O ã Boleto Proposta
                             !boleto.EspecieDocumento.Codigo.Equals("K") //K ã Outros.
                            )
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código da EspãcieDocumento! São Aceitas:{A,B,C,D,E,F,H,I,J,O,K}", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.Sacado.CPFCNPJ.Length.Equals(11) && !boleto.Sacado.CPFCNPJ.Length.Equals(14))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Cpf/Cnpj diferente de 11/14 caracteres!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.NossoNumero.Length.Equals(8))
                    {
                        //sidnei.klein: Segundo definição recebida pelo Sicredi-RS, o Nosso Número sempre terã somente 8 caracteres sem o DV que estã no boleto.DigitoNossoNumero
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: O Nosso Número diferente de 8 caracteres!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.TipoImpressao.Equals("A") && !boleto.TipoImpressao.Equals("B"))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Tipo de Impressão deve conter A - Normal ou B - Carnã", Environment.NewLine);
                        vRetorno = false;
                    }
                    #endregion
                }
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                             //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                             //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                       //003-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                            //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                      //012-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 005, 0, cedente.ContaBancaria.Conta, ' '));     //027-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 014, 0, cedente.CPFCNPJ, ' '));                 //032-045
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0046, 031, 0, "", ' '));                              //046-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 003, 0, "748", ' '));                           //077-079
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, "SICREDI", ' '));                       //080-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataAAAAMMDD_________, 0095, 008, 0, DateTime.Now, ' '));                    //095-102
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0103, 008, 0, "", ' '));                              //103-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0111, 007, 0, numeroArquivoRemessa.ToString(), '0')); //111-117
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0118, 273, 0, "", ' '));                              //118-390
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0391, 004, 0, "2.00", ' '));                          //391-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                        //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
            return GerarDetalheRemessaCNAB400_A(boleto, numeroRegistro, tipoArquivo);
        }
        public string GerarDetalheRemessaCNAB400_A(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "1", ' '));                                       //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "A", ' '));                                       //002-002  'A' - SICREDI com Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 001, 0, "A", ' '));                                       //003-003  'A' - Simples
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 001, 0, boleto.TipoImpressao, ' '));                                       //004-004  'A' ã Normal
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0005, 012, 0, string.Empty, ' '));                              //005-016
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0017, 001, 0, "A", ' '));                                       //017-017  Tipo de moeda: 'A' - REAL
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 001, 0, "A", ' '));                                       //018-018  Tipo de desconto: 'A' - VALOR
                #region Código de Juros
                string CodJuros = "A";
                decimal ValorOuPercJuros = 0;
                if (boleto.JurosMora > 0)
                {
                    CodJuros = "A";
                    ValorOuPercJuros = boleto.JurosMora;
                }
                else if (boleto.PercJurosMora > 0)
                {
                    CodJuros = "B";
                    ValorOuPercJuros = boleto.PercJurosMora;
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0019, 001, 0, CodJuros, ' '));                                  //019-019  Tipo de juros: 'A' - VALOR / 'B' PERCENTUAL
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 028, 0, string.Empty, ' '));                              //020-047
                #region Nosso Número + DV
                string NossoNumero = boleto.NossoNumero.Replace("/", "").Replace("-", ""); // AA/BXXXXX-D
                string vAuxNossoNumeroComDV = NossoNumero;
                if (string.IsNullOrEmpty(boleto.DigitoNossoNumero) || NossoNumero.Length < 9)
                {
                    boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto, true);
                    vAuxNossoNumeroComDV = NossoNumero + boleto.DigitoNossoNumero;
                }
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0048, 009, 0, vAuxNossoNumeroComDV, '0'));                      //048-056
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0057, 006, 0, string.Empty, ' '));                              //057-062
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataAAAAMMDD_________, 0063, 008, 0, boleto.DataProcessamento, ' '));                  //063-070
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0071, 001, 0, string.Empty, ' '));                              //071-071
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0072, 001, 0, "N", ' '));                                       //072-072 'N' - Não Postar e remeter para o beneficiãrio
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 001, 0, string.Empty, ' '));                              //073-073
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 001, 0, "B", ' '));                                       //074-074 'B' ã Impressão ã feita pelo Beneficiãrio
                if (boleto.TipoImpressao.Equals("A"))
                {
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0075, 002, 0, 0, '0'));                                      //075-076
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 002, 0, 0, '0'));                                      //077-078
                }
                else if (boleto.TipoImpressao.Equals("B"))
                {
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0075, 002, 0, boleto.NumeroParcela, '0'));                   //075-076
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 002, 0, boleto.TotalParcela, '0'));                    //077-078
                }
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0079, 004, 0, string.Empty, ' '));                              //079-082
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0083, 010, 2, boleto.ValorDescontoAntecipacao, '0'));           //083-092
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0093, 004, 2, boleto.PercMulta, '0'));                          //093-096
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0097, 012, 0, string.Empty, ' '));                              //097-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, ObterCodigoDaOcorrencia(boleto), ' '));           //109-110 01 - Cadastro de título;
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 009, 0, string.Empty, ' '));                              //140-148
                #region Espãcie de documento
                //Adota Duplicata Mercantil p/ Indicação como padrão.
                var especieDoc = boleto.EspecieDocumento ?? new EspecieDocumento_Sicredi("A");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0149, 001, 0, especieDoc.Codigo, ' '));                         //149-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                #region Instruçães
                string vInstrucao1 = "00"; //1ã instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                string vInstrucao2 = "00"; //2ã instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicredi)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_CancelamentoProtestoAutomatico:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Sicredi.PedidoProtesto:
                            vInstrucao1 = "06"; //Indicar o Código ã06ã - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0159, 002, 0, vInstrucao2, '0'));                               //159-160
                #endregion               
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, ValorOuPercJuros, '0'));                          //161-173 Valor/% de juros por dia de atraso
                #region DataDesconto
                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 0, 0, '0'));                                         //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "0";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "1"; //Cpf ã sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "2"; //Cnpj ã sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 001, 0, vCpfCnpjSac, '0'));                               //219-219
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0220, 001, 0, "0", '0'));                                       //220-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.EndComNumeroEComplemento.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0315, 005, 0, 0, '0'));                                         //315-319
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0320, 006, 0, 0, '0'));                                         //320-325
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0326, 001, 0, string.Empty, ' '));                              //326-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0335, 005, 1, 0, '0'));                                         //335-339
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0340, 014, 0, string.Empty, ' '));                              //340-353
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0354, 041, 0, string.Empty, ' '));                              //354-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400
                //
                reg.CodificarLinha();
                //
                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
                //
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessa400(int numeroRegistro, Cedente cedente)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));                         //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                         //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 003, 0, "748", ' '));                       //003-006
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0006, 005, 0, cedente.ContaBancaria.Conta, ' ')); //006-010
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0011, 384, 0, string.Empty, ' '));                //011-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));              //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        private string LerMotivoRejeicao(string codigorejeicao)
        {
            var rejeicao = String.Empty;

            if (codigorejeicao.Length >= 2)
            {
                #region LISTA DE MOTIVOS
                List<String> ocorrencias = new List<string>();

                ocorrencias.Add("01-Código do banco invãlido");
                ocorrencias.Add("02-Código do registro detalhe invãlido");
                ocorrencias.Add("03-Código da ocorrãncia invãlido");
                ocorrencias.Add("04-Código de ocorrãncia não permitida para a carteira");
                ocorrencias.Add("05-Código de ocorrãncia não numãrico");
                ocorrencias.Add("07-Cooperativa/Agência/conta/Dígito invãlidos");
                ocorrencias.Add("08-Nosso Número invãlido");
                ocorrencias.Add("09-Nosso Número duplicado");
                ocorrencias.Add("10-Carteira invãlida");
                ocorrencias.Add("14-título protestado");
                ocorrencias.Add("15-Cooperativa/carteira/Agência/conta/nosso Número invãlidos");
                ocorrencias.Add("16-Data de vencimento invãlida");
                ocorrencias.Add("17-Data de vencimento anterior ã data de emissão");
                ocorrencias.Add("18-Vencimento fora do prazo de operação");
                ocorrencias.Add("20-Valor do título invãlido");
                ocorrencias.Add("21-Espãcie do título invãlida");
                ocorrencias.Add("22-Espãcie não permitida para a carteira");
                ocorrencias.Add("24-Data de emissão invãlida");
                ocorrencias.Add("29-Valor do desconto maior/igual ao valor do título");
                ocorrencias.Add("31-Concessão de desconto - existe desconto anterior");
                ocorrencias.Add("33-Valor do abatimento invãlido");
                ocorrencias.Add("34-Valor do abatimento maior/igual ao valor do título");
                ocorrencias.Add("36-Concessão de abatimento - existe abatimento anterior");
                ocorrencias.Add("38-Prazo para protesto invãlido");
                ocorrencias.Add("39-Pedido para protesto não permitido para o título");
                ocorrencias.Add("40-título com ordem de protesto emitida");
                ocorrencias.Add("41-Pedido cancelamento/sustação sem instrução de protesto");
                ocorrencias.Add("44-Cooperativa de Crédito/Agência beneficiãria não prevista");
                ocorrencias.Add("45-Nome do pagador invãlido");
                ocorrencias.Add("46-Tipo/Número de inscrição do pagador invãlidos");
                ocorrencias.Add("47-Endereão do pagador não informado");
                ocorrencias.Add("48-CEP irregular");
                ocorrencias.Add("49-Número de Inscrição do pagador/avalista invãlido");
                ocorrencias.Add("50-Pagador/avalista não informado");
                ocorrencias.Add("60-Movimento para título não cadastrado");
                ocorrencias.Add("63-Entrada para título jã cadastrado");
                ocorrencias.Add("A -Aceito");
                ocorrencias.Add("A1-Praãa do pagador não cadastrada.");
                ocorrencias.Add("A2-Tipo de Cobrança do título divergente com a praãa do pagador.");
                ocorrencias.Add("A3-Cooperativa/Agência depositãria divergente: atualiza o cadastro de praãas da Coop./Agência beneficiãria");
                ocorrencias.Add("A4-Beneficiãrio não cadastrado ou possui CGC/CIC invãlido");
                ocorrencias.Add("A5-Pagador não cadastrado");
                ocorrencias.Add("A6-Data da instrução/ocorrãncia invãlida");
                ocorrencias.Add("A7-Ocorrãncia não pode ser comandada");
                ocorrencias.Add("A8-Recebimento da liquidação fora da rede Sicredi - via Compensação eletrãnica");
                ocorrencias.Add("B4-Tipo de moeda invãlido");
                ocorrencias.Add("B5-Tipo de desconto/juros invãlido");
                ocorrencias.Add("B6-Mensagem padrão não cadastrada");
                ocorrencias.Add("B7-Seu Número invãlido");
                ocorrencias.Add("B8-Percentual de multa invãlido");
                ocorrencias.Add("B9-Valor ou percentual de juros invãlido");
                ocorrencias.Add("C1-Data limite para concessão de desconto invãlida");
                ocorrencias.Add("C2-Aceite do título invãlido");
                ocorrencias.Add("C3-Campo alterado na instrução ã31 ã alteração de outros dadosã invãlido");
                ocorrencias.Add("C4-título ainda não foi confirmado pela centralizadora");
                ocorrencias.Add("C5-título rejeitado pela centralizadora");
                ocorrencias.Add("C6-título jã liquidado");
                ocorrencias.Add("C7-título jã baixado");
                ocorrencias.Add("C8-Existe mesma instrução pendente de confirmação para este título");
                ocorrencias.Add("C9-Instrução prãvia de concessão de abatimento não existe ou não confirmada");
                ocorrencias.Add("D -Desprezado");
                ocorrencias.Add("D1-título dentro do prazo de vencimento (em dia);");
                ocorrencias.Add("D2-Espãcie de documento não permite protesto de título");
                ocorrencias.Add("D3-título possui instrução de baixa pendente de confirmação");
                ocorrencias.Add("D4-Quantidade de mensagens padrão excede o limite permitido");
                ocorrencias.Add("D5-Quantidade invãlida no pedido de boletos prã-impressos da Cobrança sem registro");
                ocorrencias.Add("D6-Tipo de impressão invãlida para Cobrança sem registro");
                ocorrencias.Add("D7-Cidade ou Estado do pagador não informado");
                ocorrencias.Add("D8-Seqçãncia para comPosição do nosso Número do ano atual esgotada");
                ocorrencias.Add("D9-Registro mensagem para título não cadastrado");
                ocorrencias.Add("E2-Registro complementar ao cadastro do título da Cobrança com e sem registro não cadastrado");
                ocorrencias.Add("E3-Tipo de postagem invãlido, diferente de S, N e branco");
                ocorrencias.Add("E4-Pedido de boletos prã-impressos");
                ocorrencias.Add("E5-Confirmação/rejeição para pedidos de boletos não cadastrado");
                ocorrencias.Add("E6-Pagador/avalista não cadastrado");
                ocorrencias.Add("E7-Informação para atualização do valor do título para protesto invãlido");
                ocorrencias.Add("E8-Tipo de impressão invãlido, diferente de A, B e branco");
                ocorrencias.Add("E9-Código do pagador do título divergente com o Código da cooperativa de Crédito");
                ocorrencias.Add("F1-Liquidado no sistema do cliente");
                ocorrencias.Add("F2-Baixado no sistema do cliente");
                ocorrencias.Add("F3-Instrução invãlida, este título estã caucionado/descontado");
                ocorrencias.Add("F4-Instrução fixa com caracteres invãlidos");
                ocorrencias.Add("F6-Nosso Número / Número da parcela fora de seqçãncia ã total de parcelas invãlido");
                ocorrencias.Add("F7-Falta de comprovante de prestação de serviço");
                ocorrencias.Add("F8-Nome do beneficiãrio incompleto / incorreto.");
                ocorrencias.Add("F9-CNPJ / CPF incompatãvel com o nome do pagador / Sacador Avalista");
                ocorrencias.Add("G1-CNPJ / CPF do pagador Incompatãvel com a espãcie");
                ocorrencias.Add("G2-título aceito: sem a assinatura do pagador");
                ocorrencias.Add("G3-título aceito: rasurado ou rasgado");
                ocorrencias.Add("G4-título aceito: falta título (cooperativa/ag. beneficiãria deverã enviã-lo);");
                ocorrencias.Add("G5-Praãa de pagamento incompatãvel com o endereão");
                ocorrencias.Add("G6-título aceito: sem endosso ou beneficiãrio irregular");
                ocorrencias.Add("G7-título aceito: valor por extenso diferente do valor numãrico");
                ocorrencias.Add("G8-Saldo maior que o valor do título");
                ocorrencias.Add("G9-Tipo de endosso invãlido");
                ocorrencias.Add("H1-Nome do pagador incompleto / Incorreto");
                ocorrencias.Add("H2-Sustação judicial");
                ocorrencias.Add("H3-Pagador não encontrado");
                ocorrencias.Add("H4-Alteração de carteira");
                ocorrencias.Add("H5-Recebimento de liquidação fora da rede Sicredi ã VLB Inferior ã Via Compensação");
                ocorrencias.Add("H6-Recebimento de liquidação fora da rede Sicredi ã VLB Superior ã Via Compensação");
                ocorrencias.Add("H7-Espãcie de documento necessita beneficiãrio ou avalista PJ");
                ocorrencias.Add("H8-Recebimento de liquidação fora da rede Sicredi ã Contingãncia Via Compe");
                ocorrencias.Add("H9-Dados do título não conferem com disquete");
                ocorrencias.Add("I1-Pagador e Sacador Avalista são a mesma pessoa");
                ocorrencias.Add("I2-Aguardar um dia ãtil apãs o vencimento para protestar");
                ocorrencias.Add("I3-Data do vencimento rasurada");
                ocorrencias.Add("I4-Vencimento ã extenso não confere com Número");
                ocorrencias.Add("I5-Falta data de vencimento no título");
                ocorrencias.Add("I6-DM/DMI sem comprovante autenticado ou declaração");
                ocorrencias.Add("I7-Comprovante ilegãvel para conferãncia e microfilmagem");
                ocorrencias.Add("I8-Nome solicitado não confere com emitente ou pagador");
                ocorrencias.Add("I9-Confirmar se são 2 emitentes. Se sim, indicar os dados dos 2");
                ocorrencias.Add("J1-Endereão do pagador igual ao do pagador ou do portador");
                ocorrencias.Add("J2-Endereão do apresentante incompleto ou não informado");
                ocorrencias.Add("J3-Rua/Número inexistente no endereão");
                ocorrencias.Add("J4-Falta endosso do favorecido para o apresentante");
                ocorrencias.Add("J5-Data da emissão rasurada");
                ocorrencias.Add("J6-Falta assinatura do pagador no título");
                ocorrencias.Add("J7-Nome do apresentante não informado/incompleto/incorreto");
                ocorrencias.Add("J8-Erro de preenchimento do titulo");
                ocorrencias.Add("J9-Titulo com direito de regresso vencido");
                ocorrencias.Add("K1-Titulo apresentado em duplicidade");
                ocorrencias.Add("K2-Titulo jã protestado");
                ocorrencias.Add("K3-Letra de cambio vencida ã falta aceite do pagador");
                ocorrencias.Add("K4-Falta declaração de saldo assinada no título");
                ocorrencias.Add("K5-Contrato de cambio ã Falta conta grãfica");
                ocorrencias.Add("K6-Ausãncia do documento fãsico");
                ocorrencias.Add("K7-Pagador falecido");
                ocorrencias.Add("K8-Pagador apresentou quitação do título");
                ocorrencias.Add("K9-título de outra jurisdição territorial");
                ocorrencias.Add("L1-título com emissão anterior a concordata do pagador");
                ocorrencias.Add("L2-Pagador consta na lista de falãncia");
                ocorrencias.Add("L3-Apresentante não aceita publicação de edital");
                ocorrencias.Add("L4-Dados do Pagador em Branco ou invãlido");
                ocorrencias.Add("L5-Código do Pagador na Agência beneficiãria estã duplicado");
                ocorrencias.Add("M1-Reconhecimento da dãvida pelo pagador");
                ocorrencias.Add("M2-Não reconhecimento da dãvida pelo pagador");
                ocorrencias.Add("M3-Inclusão de desconto 2 e desconto 3 invãlida");
                ocorrencias.Add("X0-Pago com cheque");
                ocorrencias.Add("X1-Regularização centralizadora ã Rede Sicredi");
                ocorrencias.Add("X2-Regularização centralizadora ã Compensação");
                ocorrencias.Add("X3-Regularização centralizadora ã Banco correspondente");
                ocorrencias.Add("X4-Regularização centralizadora - VLB Inferior - via Compensação");
                ocorrencias.Add("X5-Regularização centralizadora - VLB Superior - via Compensação");
                ocorrencias.Add("X6-Pago com cheque ã bloqueado 24 horas");
                ocorrencias.Add("X7-Pago com cheque ã bloqueado 48 horas");
                ocorrencias.Add("X8-Pago com cheque ã bloqueado 72 horas");
                ocorrencias.Add("X9-Pago com cheque ã bloqueado 96 horas");
                ocorrencias.Add("XA-Pago com cheque ã bloqueado 120 horas");
                ocorrencias.Add("XB-Pago com cheque ã bloqueado 144 horas");
                #endregion

                var ocorrencia = (from s in ocorrencias where s.Substring(0, 2) == codigorejeicao.Substring(0, 2) select s).FirstOrDefault();

                if (ocorrencia != null)
                    rejeicao = ocorrencia;
            }

            return rejeicao;
        }

        // 7.3 Tabela de Motivos da Ocorrãncia ã28 ã Tarifasã Maio 2020 v1.6
        private string LerMotivoRejeicaoTarifas(string codigorejeicao)
        {
            var rejeicao = String.Empty;

            if (codigorejeicao.Length >= 2)
            {
                #region LISTA DE MOTIVOS
                List<String> ocorrencias = new List<string>();

                ocorrencias.Add("03-Tarifa de sustação");
                ocorrencias.Add("04-Tarifa de protesto");
                ocorrencias.Add("08-Tarifa de custas de protesto");
                ocorrencias.Add("A9-Tarifa de manutenção de título vencido");
                ocorrencias.Add("B1-Tarifa de baixa da carteira");
                ocorrencias.Add("B3-Tarifa de registro de entrada do título");
                ocorrencias.Add("F5-Tarifa de entrada na rede Sicredi");
                ocorrencias.Add("S4-Tarifa de Inclusão Negativação");
                ocorrencias.Add("S5-Tarifa de Exclusão Negativação");
                #endregion

                var ocorrencia = (from s in ocorrencias where s.Substring(0, 2) == codigorejeicao.Substring(0, 2) select s).FirstOrDefault();

                if (ocorrencia != null)
                    rejeicao = ocorrencia;
            }

            return rejeicao;
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                TRegistroEDI_Sicredi_Retorno reg = new TRegistroEDI_Sicredi_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(reg.IdentificacaoRegDetalhe);
                //Filler1
                //TipoCobranca
                //CodigoPagadorAgenciaBeneficiario
                detalhe.NomeSacado = reg.CodigoPagadorJuntoAssociado;
                //BoletoDDA
                //Filler2
                #region NossoNumeroSicredi
                detalhe.NossoNumeroComDV = reg.NossoNumeroSicredi;
                detalhe.NossoNumero = reg.NossoNumeroSicredi.Substring(0, reg.NossoNumeroSicredi.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumeroSicredi.Substring(reg.NossoNumeroSicredi.Length - 1); //DV do Nosso Numero
                #endregion
                //Filler3
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.Ocorrencia);
                int dataOcorrencia = Utils.ToInt32(reg.DataOcorrencia);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Descrição da ocorrãncia
                detalhe.DescricaoOcorrencia = new CodigoMovimento(748, detalhe.CodigoOcorrencia).Descricao;

                detalhe.NumeroDocumento = reg.SeuNumero;
                //Filler4
                if (!String.IsNullOrEmpty(reg.DataVencimento))
                {
                    int dataVencimento = Utils.ToInt32(reg.DataVencimento);
                    detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                }
                decimal valorTitulo = Convert.ToInt64(reg.ValorTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;
                //Filler5
                //Despesas de Cobrança para os Códigos de Ocorrãncia (Valor Despesa)
                if (!String.IsNullOrEmpty(reg.DespesasCobranca))
                {
                    decimal valorDespesa = Convert.ToUInt64(reg.DespesasCobranca);
                    detalhe.ValorDespesa = valorDespesa / 100;
                }
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                if (!String.IsNullOrEmpty(reg.DespesasCustasProtesto))
                {
                    decimal valorOutrasDespesas = Convert.ToUInt64(reg.DespesasCustasProtesto);
                    detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                }
                //Filler6
                //Abatimento Concedido sobre o título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.AbatimentoConcedido);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.DescontoConcedido);
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorEfetivamentePago);
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.JurosMora);
                detalhe.JurosMora = jurosMora / 100;
                //Filler7
                //SomenteOcorrencia19
                //Filler8
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                int dataCredito = Utils.ToInt32(reg.DataPrevistaLancamentoContaCorrente);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("####-##-##"));
                //Filler9
                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequencialRegistro);
                //
                #region NAO RETORNADOS PELO SICREDI
                //detalhe.Especie = reg.TipoDocumento; //Verificar Espãcie de Documentos...
                detalhe.OutrosCreditos = 0;
                detalhe.OrigemPagamento = String.Empty;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //
                detalhe.IOF = 0;
                //Motivos das Rejeiçães para os Códigos de Ocorrãncia
                if (detalhe.CodigoOcorrencia == 28)
                {
                    detalhe.MotivosRejeicao = LerMotivoRejeicaoTarifas(detalhe.MotivoCodigoOcorrencia);
                }
                else
                {
                    detalhe.MotivosRejeicao = LerMotivoRejeicao(detalhe.MotivoCodigoOcorrencia);
                }

                //Número do Cartãrio
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;

                detalhe.CodigoInscricao = 0;
                detalhe.NumeroInscricao = string.Empty;
                detalhe.Agencia = 0;
                detalhe.Conta = header.Conta;
                detalhe.DACConta = header.DACConta;

                detalhe.NumeroControle = string.Empty;
                detalhe.IdentificacaoTitulo = string.Empty;
                //Banco Cobrador
                detalhe.CodigoBanco = 0;
                //Agência Cobradora
                detalhe.AgenciaCobradora = 0;
                #endregion
                //
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                header = new HeaderRetorno(registro);
                header.TipoRegistro = Utils.ToInt32(registro.Substring(000, 1));
                header.CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1));
                header.LiteralRetorno = registro.Substring(002, 7);
                header.CodigoServico = Utils.ToInt32(registro.Substring(009, 2));
                header.LiteralServico = registro.Substring(011, 15);
                string _conta = registro.Substring(026, 5);
                header.Conta = Utils.ToInt32(_conta.Substring(0, _conta.Length - 1));
                header.DACConta = Utils.ToInt32(_conta.Substring(_conta.Length - 1));
                header.ComplementoRegistro2 = registro.Substring(031, 14);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 8)).ToString("##-##-##"));
                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(110, 7));
                header.Versao = registro.Substring(390, 5);
                header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));



                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            long num;
            if (nossoNumero.Length >= 8 && long.TryParse(nossoNumero.Substring(0, 8), out num))
            {
                return num;
            }
            throw new BoletoNetException("Nosso Número ã invãlido!");
        }
    }
}