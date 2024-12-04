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
            //Formata o tamanho do n�mero da ag�ncia
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Formata o tamanho do n�mero da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 5)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "At� o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;
            else boleto.LocalPagamento = "PAG�VEL PREFERENCIALMENTE NAS COOPERATIVAS DE CR�DITO DO SICREDI";

            //Verifica se data do processamento � valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento � valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            string infoFormatoCodigoCedente = "formato AAAAPPCCCCC, onde: AAAA = N�mero da ag�ncia, PP = Posto do benefici�rio, CCCCC = C�digo do benefici�rio";

            var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 11);

            if (string.IsNullOrEmpty(codigoCedente))
                throw new BoletoNetException("C�digo do cedente deve ser informado, " + infoFormatoCodigoCedente);

            var conta = boleto.Cedente.ContaBancaria.Conta;
            if (boleto.Cedente.ContaBancaria != null &&
                (!codigoCedente.StartsWith(boleto.Cedente.ContaBancaria.Agencia) ||
                 !(codigoCedente.EndsWith(conta) || codigoCedente.EndsWith(conta.Substring(0, conta.Length - 1)))))
                //throw new BoletoNetException("C�digo do cedente deve estar no " + infoFormatoCodigoCedente);
                boleto.Cedente.Codigo = string.Format("{0}{1}{2}", boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, (boleto.Cedente.Codigo.Length >= 5 ? Utils.Right(boleto.Cedente.Codigo, 5, '0', true) : Utils.Right((boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente), 5, '0', true)));

            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new BoletoNetException("Tipo de carteira � obrigat�rio. " + ObterInformacoesCarteirasDisponiveis());

            if (!CarteiraValida(boleto.Carteira))
                throw new BoletoNetException("Carteira informada � inv�lida. Informe " + ObterInformacoesCarteirasDisponiveis());

            //Verifica se o nosso n�mero � v�lido
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
                    throw new NotImplementedException("Nosso n�mero inv�lido");
            }

            FormataCodigoBarra(boleto);
            if (boleto.CodigoBarra.Codigo.Length != 44)
                throw new BoletoNetException("C�digo de barras � inv�lido");

            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        private string ObterInformacoesCarteirasDisponiveis()
        {
            return string.Join(", ", carteirasDisponiveis.Select(o => string.Format("�{0}� � {1}", o.Key, o.Value)));
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
                throw new Exception("Erro ao tentar formatar nosso n�mero, verifique o tamanho do campo");
            }

            try
            {
                boleto.NossoNumero = string.Format("{0}/{1}-{2}", nossoNumero.Substring(0, 2), nossoNumero.Substring(2, 6), nossoNumero.Substring(8));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso n�mero", ex);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o do fomata n�mero do documento n�o implementada.");
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

            var codigoCobranca = 1; //C�digo de cobran�a com registro
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

        #region M�todos de Gera��o do Arquivo de Remessa
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
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
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
        /// Fun��o que gera nosso numero a ser colocado na remessa sicoob CNAB240, segundo layout para troca de informa��es
        /// </summary>
        /// <param name="boleto"></param>
        /// <returns></returns>
        private string NossoNumeroFormatado(Boleto boleto)
        {
            /*
             * Identifica��o do t�tulo no banco
             * N�mero adotado pelo banco cedente para identificar o t�tulo.
             * Para c�digo de movimento igual a '01' (entrada de t�tulos), caso esteja preenchido com zeros, a numer���o ser� feita pelo banco.
             * A identifica��o do t�tulo no SICREDI/nosso n�mero � composta por nove d�gitos, conforme descri��o do item 3.6, p�gina 39.
             * 
             * 3.6.2 Gera��o pelo cedente
             * 1. A parte sequencial do nosso n�mero � controlada pelo cedente
             * 2. o sequencial do nosso n�mero n�o poder� ser repetido para que n�o haja t�tulos com o mesmo nosso n�mero
             * 3. o cedente dever� enviar o nosso n�mero calculado de acordo com a descri��o na pr�xima p�gina, abaixo o leiauto de como ficar� o nosso n�mero nos bloquetos:
             * YY = Ano da gera��o do t�tulo
             * B = Gera��o no nosso n�mero: 1 - Cooperativa de cr�dito / Ag�ncia cedente. 2 a 9 - Cedente.
             * nnnn = N�mero sequencial por cedente
             * d = D�gito verificador, calculado atrav�s do m�dulo 11.
             * 
             * OBS: A cada �nicio de ano, o n�mero sequencial deve ser reinicializado.
             * 
             *  -> Ano: relacionado ao ano atual que este nosso n�mero esta sendo gerado. Exemplo: 2021
             *  -> byte: relacionado ao controle de quem emite, est�o dispon�veis os valores 2 a 9 onde 2 o cedente que emite e 1 somente a cooperativa pode utilizar( para boletos pr�-impressos )
             *  -> Sequencial: relacionado ao nosso n�mero de controle �nico do boleto
             *  -> Dv do sequencial: esta relacionado ao digito verificador do nosso n�mero
             *  -> Ao realizar a jun��o de todas estas informa��es, temos o seguinte valor para o banco

             *  -> AABXXXXD( 21/20004-1 ) onde as letras
             *  -> "A" representam o ano, 
             *  -> "B" byte, 
             *  -> "X" Nosso n�mero,
             *  -> "D" digito verificador
             *    
             */
            // FormataNossoNumero(boleto);

            string vAuxNossoNumeroComDV = boleto.NossoNumero;
            if (string.IsNullOrEmpty(boleto.DigitoNossoNumero) || boleto.NossoNumero.Length < 9)
            {
                boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                vAuxNossoNumeroComDV = boleto.NossoNumero + boleto.DigitoNossoNumero;
            }

            string retorno = vAuxNossoNumeroComDV;
            //retorno += DigNossoNumeroSicredi(boleto, true);
            retorno = Utils.FormatCode(retorno, " ", 20, false);
            return retorno;
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3);                                // Posi��o 001 a 003    C�digo do Sicoob na Compensa��o: "756"
                detalhe += "0001";                                                                      // Posi��o 004 a 007    Lote de servi�o
                detalhe += "3";                                                                         // Posi��o 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                   // Posi��o 009 a 013    N�mero Sequencial
                detalhe += "P";                                                                         // Posi��o 014          C�d. Segmento do Registro Detalhe: "P"
                detalhe += " ";                                                                         // Posi��o 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa?.CodigoOcorrencia ?? "1", "0", 2, true);     // Posi��o 016 a 017� �'01' = Entrada de T�tulos, '02' = Solicita��o de Baixa, '06' = Prorroga��o de Vencimento, '09' = Protestar, '10' = Desist�ncia do Protesto e Baixar T�tulo, '11' = Desist�ncia do Protesto e manter em carteira, '31' = Altera��es de outros dados
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 5);                   // Posi��o 018 a 022    Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoAgencia, " ", 1, true);  // Posi��o 023          D�gito Verificador do Prefixo: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 12);                    // Posi��o 024 a 035    Conta Corrente: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 1);               // Posi��o 036          D�gito Verificador da Conta: vide planilha "Capa" deste arquivo
                detalhe += " ";                                                                         // Posi��o 037          D�gito Verificador da Ag/Conta: Brancos
                detalhe += Utils.FormatCode(NossoNumeroFormatado(boleto), 20);                          // Posi��o 038 a 057    Nosso N�mero
                detalhe += (Convert.ToInt16(boleto.Carteira) == 1 ? "1" : "2");                         // Posi��o 058          C�digo da Carteira: vide planilha "Capa" deste arquivo
                detalhe += "1";                                                                         // Posi��o 059          Forma de Cadastr. do T�tulo no Banco: "1" - Cobran�a com registro
                detalhe += "1";                                                                         // Posi��o 060          Tipo de Documento: Deve ser informado "1" ou "2" 1 - Tradicional 2 - Escritural Obs.: O Sicredi n�o realizar� diferencia��o entre os dom�nios
                detalhe += "2";                                                                         // Posi��o 061          Identifica��o da Emiss�o do Boleto: 1=SICREDI emite 2=CEDENTE emite
                detalhe += "2";                                                                         // Posi��o 062          Identifica��o da distribui��o do Boleto: 1=SICREDI distribui 2=CEDENTE distribui
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 15);                           // Posi��o 063 a 077    N�mero do documento de cobran�a.
                detalhe += Utils.FormatCode(boleto.DataVencimento.ToString("ddMMyyyy"), 8);             // Posi��o 078 a 085    N�mero do documento de cobran�a.

                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                valorBoleto = Utils.FormatCode(valorBoleto, 15);
                detalhe += valorBoleto;                                                                 // Posi��o 86 a 100     Valor Nominal do T�tulo
                detalhe += "00000";                                                                     // Posi��o 101 a 105    Coop/Ag�ncia Encarregada da Cobran�a: "00000"
                detalhe += new string(' ', 1);                                                          // Posi��o 106          D�gito Verificador da Ag�ncia: Brancos
                detalhe += Utils.FormatCode(boleto.EspecieDocumento.Codigo, 2);                         // Posi��o 107 a 108    Esp�cie do t�tulo
                detalhe += Utils.FormatCode(boleto.Aceite, 1);                                          // Posi��o 109          Identifica��o do t�tulo Aceito/N�o Aceito
                detalhe += Utils.FormatCode(boleto.DataProcessamento.ToString("ddMMyyyy"), 8);          // Posi��o 110 a 117    Data Emiss�o do T�tulo
                detalhe += Utils.FormatCode(boleto.CodJurosMora, "1", 1);                               // Posi��o 118          C�digo do juros mora. 1 = Valor monet�rio, 2 = Taxa mensal, 3 = Isento
                detalhe += new string('0', 8);                                                          // Posi��o 119 a 126    Data do Juros de Mora: O SICREDI n�o utiliar� esse campo.
                detalhe += Utils.FormatCode(boleto.CodJurosMora == "0" || boleto.CodJurosMora == "3" ? "".PadLeft(15, '0') : (boleto.CodJurosMora == "1" ? boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", "") : boleto.PercJurosMora.ToString("f").Replace(",", "").Replace(".", "")), 15);   // Posi��o 127 a 141  - Juro de mora por dia/taxa, valor dsobre o titulo a ser cobrado de juros de mora.

                if (boleto.DataDesconto > DateTime.MinValue)
                {
                    detalhe += "1";                                                                     // Posi��o 142          C�digo do desconto
                    detalhe += Utils.FormatCode(boleto.DataDesconto.ToString("ddMMyyyy"), 8);           // Posi��o 143 a 150    Data do Desconto 1
                    detalhe += Utils.FormatCode(boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", ""), 15);
                }
                else
                {
                    detalhe += "0";                                                                     // Posi��o 142          C�digo do desconto - Sem Desconto
                    detalhe += Utils.FormatCode("", "0", 8, true); ;                                    // Posi��o 143 a 150    Data do Desconto
                    detalhe += Utils.FormatCode("", "0", 15, true);
                }

                //detalhe += Utils.FormatCode(boleto.IOF.ToString(), 15);                                 // Posi��o 166 a 180    Valor do IOF a ser Recolhido -> O Sicredi n�o utiliza esse campo, preencher com zeros
                detalhe += Utils.FormatCode("", "0", 15, true);                                         // Posi��o 166 a 180    Valor do IOF a ser Recolhido -> O Sicredi n�o utiliza esse campo, preencher com zeros
                detalhe += Utils.FormatCode(boleto.Abatimento.ToString(), 15);                          // Posi��o 181 a 195    Valor do Abatimento
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 25);                           // Posi��o 196 a 220    Identifica��o do t�tulo
                detalhe += "3";                                                                         // Posi��o 221          C�digo do protesto: 1 = Protestar dias corridos, 3 = Nao Protestar, 9 = Cancelamento protesto autom�tico.

                #region Instru��es

                string vInstrucao1 = "00"; //2� instru��o (2, N) Caso Queira colocar um cod de uma instru��o. ver no Manual caso nao coloca 00
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

                detalhe += Utils.FormatCode(vInstrucao1, 2);                                            // Posi��o 222 a 223    C�digo do protesto
                detalhe += Utils.FormatCode("1", 1);                                                    // Posi��o 224          C�digo para Baixa/Devolu��o: "1" = Baixar/devolver
                //detalhe += Utils.FormatCode("60", " ", 3);                                              // Posi��o 225 A 227    N�mero de Dias para Baixa/Devolu��o: Utilizar sempre, nesse campo, 60 dias para baixa/devolu��o.
                detalhe += "000";                                                                       // Posi��o 225 A 227    N�mero de Dias para Baixa/Devolu��o: Utilizar sempre, nesse campo, 60 dias para baixa/devolu��o.
                detalhe += Utils.FormatCode(boleto.Moeda.ToString(), "0", 2, true);                     // Posi��o 228 A 229    C�digo da Moeda - 09
                detalhe += Utils.FormatCode("", "0", 10, true);                                         // Posi��o 230 A 239    N� do Contrato da Opera��o de Cr�d.: "0000000000"
                detalhe += " ";
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO P DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);             // Posi��o 001 a 003    C�digo do Sicredi na Compensa��o: "748"
                detalhe += "0001";                                                              // Posi��o 004 a 007    Lote
                detalhe += "3";                                                                 // Posi��o 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);           // Posi��o 009 a 013    N�mero Sequencial
                detalhe += "Q";                                                                 // Posi��o 014          C�d. Segmento do Registro Detalhe: "P"
                detalhe += " ";                                                                 // Posi��o 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa?.CodigoOcorrencia ?? "1", "0", 2, true);     // Posi��o 016 a 017� � '01' = Entrada de T�tulos, '02' = Solicita��o de Baixa, '06' = Prorroga��o de Vencimento, '09' = Protestar, '10' = Desist�ncia do Protesto e Baixar T�tulo, '11' = Desist�ncia do Protesto e manter em carteira, '31' = Altera��es de outros dados
                detalhe += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");                    // Posi��o 018          1=CPF    2=CGC/CNPJ
                detalhe += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15, true);              // Posi��o 019 a 033    N�mero de Inscri��o da Empresa
                detalhe += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);                       // Posi��o 034 a 73     Nome
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);               // Posi��o 074 a 113    Endere�o
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);            // Posi��o 114 a 128    Bairro 
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.CEP, 8);                     // Posi��o 129 a 136    CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);            // Posi��o 137 a 151    Cidade 
                detalhe += boleto.Sacado.Endereco.UF;                                           // Posi��o 152 a 153    Unidade da Federa��o
                //detalhe += (boleto.Cedente.CPFCNPJ.Length == 11 ? "1" : "2");                   // Posi��o 154          Tipo de Inscri��o Sacador avalista
                //detalhe += Utils.FormatCode(boleto.Cedente.CPFCNPJ, "0", 15, true);             // Posi��o 155 a 169    N�mero de Inscri��o / Sacador avalista
                //detalhe += Utils.FormatCode(boleto.Cedente.Nome, " ", 40);                      // Posi��o 170 a 209    Nome / Sacador avalista
                detalhe += "0";                                                                 // Posi��o 154          Beneficiario final / Tipo pessoa -> 0 - Sem Benefici�rio Final, 1 = CPF, 2 = CNPJ
                detalhe += Utils.FormatCode("", "0", 15, true);                                 // Posi��o 155 a 169    Beneficiario final / CPF/CNPJ   
                detalhe += Utils.FormatCode("", " ", 40);                                       // Posi��o 170 a 209    Nome do Beneficiario Final
                detalhe += "000";                                                               // Posi��o 210 a 212    C�digo Bco. Corresp. na Compensa��o
                detalhe += Utils.FormatCode("", " ", 20);                                       // Posi��o 213 a 232    Nosso N� no Banco Correspondente "1323739"
                detalhe += Utils.FormatCode("", " ", 8);                                        // Posi��o 233 a 240    Uso Exclusivo FEBRABAN/CNAB
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe).ToUpper();
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3);                                                        // Posi��o 001 a 003    C�digo do Sicoob na Compensa��o: "748"
                detalhe += "0001";                                                                                              // Posi��o 004 a 007    Lote de servi�o
                detalhe += "3";                                                                                                 // Posi��o 008          Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                                           // Posi��o 009 a 013    N�mero Sequencial do registro no lote
                detalhe += "R";                                                                                                 // Posi��o 014          C�d. Segmento do Registro Detalhe: "R"
                detalhe += " ";                                                                                                 // Posi��o 015          Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa?.CodigoOcorrencia ?? "1", "0", 2, true);                             // Posi��o 016 a 017� � '01' = Entrada de T�tulos, '02' = Solicita��o de Baixa, '06' = Prorroga��o de Vencimento, '09' = Protestar, '10' = Desist�ncia do Protesto e Baixar T�tulo, '11' = Desist�ncia do Protesto e manter em carteira, '31' = Altera��es de outros dados

                if (boleto.DataOutrosDescontos > DateTime.MinValue)
                {
                    detalhe += "1";                                                                                             // Posi��o 18           C�digo do desconto 2
                    detalhe += Utils.FormatCode(boleto.DataOutrosDescontos.ToString("ddMMyyyy"), 8);                            // Posi��o 19 a 26      Data do Desconto 2
                    detalhe += Utils.FormatCode(boleto.OutrosDescontos.ToString("f").Replace(",", "").Replace(".", ""), 15);    // Posi��o 27 a 41      Valor do Desconto 2
                }
                else
                {
                    detalhe += "0";                                                                                             // Posi��o 18           C�digo do desconto 2
                    detalhe += Utils.FormatCode("", "0", 8, true);                                                              // Posi��o 19 a 26      Data do Desconto 2
                    detalhe += Utils.FormatCode("", "0", 15, true);                                                             // Posi��o 27 a 41      Valor/percentual a ser concedido
                }

                detalhe += "0";                                                                                                 // Posi��o 42           C�digo da desconto 3
                detalhe += Utils.FormatCode("", "0", 8, true);                                                                  // Posi��o 43 a 50      Data do Desconto 3                
                detalhe += Utils.FormatCode("", "0", 15, true);                                                                 // Posi��o 51 a 65      Valor/percentual a ser concedido

                if (boleto.PercMulta > 0)
                {
                    // C�digo da multa 2 - percentual
                    detalhe += "2";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);                                      // Posi��o 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);       // Posi��o 75 a 89      Valor/percentual a ser aplilcado
                }
                else if (boleto.ValorMulta > 0)
                {
                    // C�digo da multa 1 - valor fixo
                    detalhe += "1";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);                                      // Posi��o 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);      // Posi��o 75 a 89      Valor/percentual a ser aplilcado
                }
                else
                {
                    // C�digo da multa 0 - sem multa
                    detalhe += "0";
                    detalhe += Utils.FormatCode("", "0", 8);                                                                    // Posi��o 67 a 74      Data da multa
                    detalhe += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);                                    // Posi��o 75 a 89      Valor/percentual a ser aplilcado
                }

                detalhe += Utils.FormatCode("", " ", 10);       // Posi��o 90 a 99 Informa��o ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 40);       // Posi��o 100 a 139 Informa��o ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 40);       // Posi��o 140 a 179 Informa��o ao Pagador: Brancos
                detalhe += Utils.FormatCode("", " ", 20);       // Posi��o 180 a 199 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode("", "0", 8, true);  // Posi��o 200 a 207  C�d. Ocor. do Pagador: "00000000"
                detalhe += Utils.FormatCode("", "0", 3, true);  // Posi��o 208 a 210  C�d. do Banco na Conta do D�bito: "000"
                detalhe += Utils.FormatCode("", "0", 5, true);  // Posi��o 211 a 215  C�digo da Ag�ncia do D�bito: "00000"
                detalhe += " ";                                 // Posi��o 216 D�gito Verificador da Ag�ncia: Brancos
                detalhe += Utils.FormatCode("", "0", 12, true); // Posi��o 217 a 228  Conta Corrente para D�bito: "000000000000"
                detalhe += " ";                                 // Posi��o 229  Verificador da Conta: Brancos
                detalhe += " ";                                 // Posi��o 230  Verificador Ag/Conta: Brancos
                detalhe += "0";                                 // Posi��o 231  Aviso para D�bito Autom�tico: "0"
                detalhe += Utils.FormatCode("", " ", 9);        // Posi��o Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO R DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);     // Posi��o 001 a 003    C�digo do banco
                trailer += "0001";                                                      // Posi��o 004 a 007    Lote de servi�o
                trailer += "5";                                                         // Posi��o 008          Tipo de registro = 5    
                trailer += Utils.FormatCode("", " ", 9);                                // Posi��o 009 a 017    Exclusivo FEBRABAN/CNAB: Brancos    - Brancos
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);   // Posi��o 018 a 023    Quantidade de registros do lote
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posi��o 024 a 029    Quantidade de t�tulos em cobran�a    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posi��o 030 a 046    Valor total dos t�tulos em carteira     
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posi��o 047 a 052    Quantidade de t�tulos em cobran�a    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posi��o 053 a 069    Valor total dos t�tulos em carteira    
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posi��o 070 a 075    Quantidade de t�tulos em cobran�a    
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posi��o 076 a 092    Quantidade de t�tulos em carteiras
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posi��o 093 a 098    Quantidade de t�tulos em cobran�a
                trailer += Utils.FormatCode("", "0", 17, true);                         // Posi��o 099 a 115    Valor total dos t�tulos em carteira
                trailer += Utils.FormatCode("", " ", 8, true);                          // Posi��o 116 a 123    N�mero do aviso de lan�amento       - Brancos
                trailer += Utils.FormatCode("", " ", 117);                              // Posi��o 124 a 240    Uso Exclusivo FEBRABAN/CNAB         - Brancos
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);     // Posi��o 001 a 003    C�digo do banco na compensa��o
                trailer += "9999";                                                      // Posi��o 004 a 007    Lote de servi�o
                trailer += "9";                                                         // Posi��o 008          Lote de servi�o
                trailer += Utils.FormatCode("", " ", 9);                                // Posi��o 009 a 017    Uso exclusivo FEBRABAN/CNAB
                trailer += Utils.FormatCode("1", "0", 6, true);                         // Posi��o 018 a 023    Quantidade de lotes do arquivo
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);   // Posi��o 024 a 029    Quantidade de registros do arquivo
                trailer += Utils.FormatCode("", "0", 6, true);                          // Posi��o 030 a 035    Quantidade de contas / conc. (lotes)
                trailer += Utils.FormatCode("", " ", 205);                              // Posi��o 036 a 240    Uso exclusivo FEBRABAN/CNAB

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do ARQUIVO de REMESSA.", e);
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
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = "748";                                                          // Posi��o 001 a 003    C�digo do Sicredi na Compensa��o: "748"
                header += "0001";                                                               // Posi��o 004 a 007    Lote de servi�o      
                header += "1";                                                                  // Posi��o 008          Tipo de Registro: "1"
                header += "R";                                                                  // Posi��o 009          Tipo de opera��o    
                header += "01";                                                                 // Posi��o 010 a 011    Tipo de Servi�o: "01"
                header += new string(' ', 2);                                                   // Posi��o 012 a 013    N� da Vers�o do Layout do Lote: "040"
                header += "040";                                                                // Posi��o 014 a 016    Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += new string(' ', 1);                                                   // Posi��o 017          Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                           // Posi��o 018          1=CPF    2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                     // Posi��o 019 a 033    N�mero de Inscri��o da Empresa
                header += Utils.FormatCode(cedente.Convenio.ToString(), " ", 20);               // Posi��o 034 a 053    C�digo do Conv�nio no Sicredi: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);        // Posi��o 054 a 058    Ag�ncia mantenedora da conta
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, " ", 1);        // Posi��o 059 a 059    Digito verfiricador da agencia
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);         // Posi��o 060 a 071    N�mero da conta corrente
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);    // Posi��o 072 a 72     D�gito verificador da conta
                header += new string(' ', 1);                                                   // Posi��o 073          D�gito Verificador da coop/Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);                              // Posi��o 074 a 103    Nome da empresa
                header += Utils.FormatCode("", " ", 40);                                        // Posi��o 104 a 143    Informa��o 1: Brancos			
                header += Utils.FormatCode("", " ", 40);                                        // Posi��o 144 a 183    Informa��o 2: Brancos
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8, true);      // Posi��o 184 a 191    N�mero da remessa
                header += DateTime.Now.ToString("ddMMyyyy");                                    // Posi��o 192 a 199    Data de Grava��o Remessa/Retorno
                header += Utils.FormatCode("", "0", 8, true);                                   // Posi��o 200 a 207    Data do Cr�dito: "00000000": Zeros
                header += new string(' ', 33);                                                  // Posi��o 208 a 240    Uso Exclusivo FEBRABAN/CNAB: Brancos
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
                        // n�o tem no CNAB 400 header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            //Variaveis
            try
            {
                //Montagem do header
                string header = "748";                                                          //Posi��o 001 a 003  C�digo do Sicredi na Compensa��o: "756"
                header += "0000";                                                               //Posi��o 004 a 007  Lote de Servi�o: "0000"
                header += "0";                                                                  //Posi��o 008        Tipo de Registro: "0"
                header += Utils.FormatCode("", " ", 9);                                         //Posi��o 09 a 017   Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                           //Posi��o 018  1=CPF 2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true);                     //Posi��o 019 a 032  N�mero de Inscri��o da Empresa
                header += Utils.FormatCode(cedente.Convenio.ToString(), " ", 20);               //Posi��o 033 a 052  C�digo do Conv�nio no Sicoob: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);        //Posi��o 053 a 057  Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                header += " ";                                                                  //Posi��o 058 a 058  Digito Ag�ncia
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);         //Posi��o 059 a 070
                header += cedente.ContaBancaria.DigitoConta;                                    //Posi��o 071 a 71
                header += " ";                                                                  //Posi��o 072 a 72   D�gito Verificador da Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);                              //Posi��o 073 a 102  Nome do Banco: SICOOB
                header += Utils.FormatCode("SICREDI", " ", 30);                                 //Posi��o 103 a 132  Nome da Empresa
                header += Utils.FormatCode("", " ", 10);                                        //Posi��o 133 a 142  Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += "1";                                                                  //Posi��o 143 a 143  C�digo Remessa / Retorno: "1"
                header += DateTime.Now.ToString("ddMMyyyyHHmmss");                              //Posi��o 144 a 151  Data de Gera��o do Arquivo  -   Posi��o 152 a 157  Hora de Gera��o do Arquivo
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);      //Posi��o 158 a 163  N�mero sequencial do arquivo
                header += "081";                                                                //Posi��o 164 a 166  No da Vers�o do Layout do Arquivo: "081"
                header += "01600";                                                              //Posi��o 167 a 171  Densidade de Grava��o do Arquivo: "00000"
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
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region M�todos de Leitura do Arquivo de Retorno
        /*
         * Substitu�do M�todo de Leitura do Retorno pelo Interpretador de EDI;
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

                //Data Ocorr�ncia no Banco
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
        #endregion M�todos de Leitura do Arquivo de Retorno

        public int Mod10Sicredi(string seq)
        {
            /* Vari�veis
             * -------------
             * d - D�gito
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
            /* Vari�veis
             * -------------
             * d - D�gito
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
            //Adicionado por diego.dariolli pois ao gerar remessa o d�gito sa�a errado pois faltava ag�ncia e posto no c�digo do cedente
            string codigoCedente = ""; //c�digo do benefici�rio aaaappccccc
            if (arquivoRemessa)
            {
                if (string.IsNullOrEmpty(boleto.Cedente.ContaBancaria.OperacaConta))
                    throw new Exception("O c�digo do posto benefici�rio n�o foi informado.");

                codigoCedente = string.Concat(boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, boleto.Cedente.Codigo);
            }
            else
                codigoCedente = boleto.Cedente.Codigo;

            string nossoNumero = boleto.NossoNumero; //ano atual (yy), indicador de gera��o do nosso n�mero (b) e o n�mero seq�encial do benefici�rio (nnnnn);

            string seq = string.Concat(codigoCedente, nossoNumero); // = aaaappcccccyybnnnnn
            /* Vari�veis
             * -------------
             * d - D�gito
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
            d = 11 - (s % 11);//Calcula o M�dulo 11;
            if (d > 9)
                d = 0;
            return d.ToString();
        }


        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
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
            #region Pr� Valida��es
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Valida��o de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    #region Valida��es da Remessa que dever�o estar preenchidas quando SICREDI
                    //Comentado porque ainda est� fixado em 01
                    //if (String.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o C�digo de Ocorr�ncia!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    if (String.IsNullOrEmpty(boleto.NumeroDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe um N�mero de Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        // Para o Sicredi, defini o Tipo de Documento sendo: 
                        //       A = 'A' - SICREDI com Registro
                        //      C1 = 'C' - SICREDI sem Registro Impress�o Completa pelo Sicredi
                        //      C2 = 'C' - SICREDI sem Registro Pedido de bloquetos pr�-impressos
                        // ** Isso porque s�o tratados 3 leiautes de escrita diferentes para o Detail da remessa;

                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.Remessa.TipoDocumento.Equals("A") && !boleto.Remessa.TipoDocumento.Equals("C1") && !boleto.Remessa.TipoDocumento.Equals("C2"))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Tipo de Documento Inv�lido! Dever�o ser: A = SICREDI com Registro; C1 = SICREDI sem Registro Impress�o Completa pelo Sicredi;  C2 = SICREDI sem Registro Pedido de bloquetos pr�-impressos", Environment.NewLine);
                        vRetorno = false;
                    }
                    //else if (boleto.Remessa.TipoDocumento.Equals("06") && !String.IsNullOrEmpty(boleto.NossoNumero))
                    //{
                    //    //Para o "Remessa.TipoDocumento = "06", n�o poder� ter NossoNumero Gerado!
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; N�o pode existir NossoNumero para o Tipo Documento '06 - cobran�a escritural'!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    else if (!boleto.EspecieDocumento.Codigo.Equals("A") && //A - Duplicata Mercantil por Indica��o
                             !boleto.EspecieDocumento.Codigo.Equals("B") && //B - Duplicata Rural;
                             !boleto.EspecieDocumento.Codigo.Equals("C") && //C - Nota Promiss�ria;
                             !boleto.EspecieDocumento.Codigo.Equals("D") && //D - Nota Promiss�ria Rural;
                             !boleto.EspecieDocumento.Codigo.Equals("E") && //E - Nota de Seguros;
                             !boleto.EspecieDocumento.Codigo.Equals("F") && //G � Recibo;

                             !boleto.EspecieDocumento.Codigo.Equals("H") && //H - Letra de C�mbio;
                             !boleto.EspecieDocumento.Codigo.Equals("I") && //I - Nota de D�bito;
                             !boleto.EspecieDocumento.Codigo.Equals("J") && //J - Duplicata de Servi�o por Indica��o;
                             !boleto.EspecieDocumento.Codigo.Equals("O") && //O � Boleto Proposta
                             !boleto.EspecieDocumento.Codigo.Equals("K") //K � Outros.
                            )
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o C�digo da Esp�cieDocumento! S�o Aceitas:{A,B,C,D,E,F,H,I,J,O,K}", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.Sacado.CPFCNPJ.Length.Equals(11) && !boleto.Sacado.CPFCNPJ.Length.Equals(14))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Cpf/Cnpj diferente de 11/14 caracteres!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.NossoNumero.Length.Equals(8))
                    {
                        //sidnei.klein: Segundo defini��o recebida pelo Sicredi-RS, o Nosso N�mero sempre ter� somente 8 caracteres sem o DV que est� no boleto.DigitoNossoNumero
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: O Nosso N�mero diferente de 8 caracteres!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (!boleto.TipoImpressao.Equals("A") && !boleto.TipoImpressao.Equals("B"))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Tipo de Impress�o deve conter A - Normal ou B - Carn�", Environment.NewLine);
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 001, 0, boleto.TipoImpressao, ' '));                                       //004-004  'A' � Normal
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0005, 012, 0, string.Empty, ' '));                              //005-016
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0017, 001, 0, "A", ' '));                                       //017-017  Tipo de moeda: 'A' - REAL
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 001, 0, "A", ' '));                                       //018-018  Tipo de desconto: 'A' - VALOR
                #region C�digo de Juros
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
                #region Nosso N�mero + DV
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0072, 001, 0, "N", ' '));                                       //072-072 'N' - N�o Postar e remeter para o benefici�rio
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 001, 0, string.Empty, ' '));                              //073-073
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 001, 0, "B", ' '));                                       //074-074 'B' � Impress�o � feita pelo Benefici�rio
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, ObterCodigoDaOcorrencia(boleto), ' '));           //109-110 01 - Cadastro de t�tulo;
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 009, 0, string.Empty, ' '));                              //140-148
                #region Esp�cie de documento
                //Adota Duplicata Mercantil p/ Indica��o como padr�o.
                var especieDoc = boleto.EspecieDocumento ?? new EspecieDocumento_Sicredi("A");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0149, 001, 0, especieDoc.Codigo, ' '));                         //149-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                #region Instru��es
                string vInstrucao1 = "00"; //1� instru��o (2, N) Caso Queira colocar um cod de uma instru��o. ver no Manual caso nao coloca 00
                string vInstrucao2 = "00"; //2� instru��o (2, N) Caso Queira colocar um cod de uma instru��o. ver no Manual caso nao coloca 00
                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicredi)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_CancelamentoProtestoAutomatico:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Sicredi.PedidoProtesto:
                            vInstrucao1 = "06"; //Indicar o c�digo �06� - (Protesto)
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
                #region Regra Tipo de Inscri��o Sacado
                string vCpfCnpjSac = "0";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "1"; //Cpf � sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "2"; //Cnpj � sempre 14;
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
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        private string LerMotivoRejeicao(string codigorejeicao)
        {
            var rejeicao = String.Empty;

            if (codigorejeicao.Length >= 2)
            {
                #region LISTA DE MOTIVOS
                List<String> ocorrencias = new List<string>();

                ocorrencias.Add("01-C�digo do banco inv�lido");
                ocorrencias.Add("02-C�digo do registro detalhe inv�lido");
                ocorrencias.Add("03-C�digo da ocorr�ncia inv�lido");
                ocorrencias.Add("04-C�digo de ocorr�ncia n�o permitida para a carteira");
                ocorrencias.Add("05-C�digo de ocorr�ncia n�o num�rico");
                ocorrencias.Add("07-Cooperativa/ag�ncia/conta/d�gito inv�lidos");
                ocorrencias.Add("08-Nosso n�mero inv�lido");
                ocorrencias.Add("09-Nosso n�mero duplicado");
                ocorrencias.Add("10-Carteira inv�lida");
                ocorrencias.Add("14-T�tulo protestado");
                ocorrencias.Add("15-Cooperativa/carteira/ag�ncia/conta/nosso n�mero inv�lidos");
                ocorrencias.Add("16-Data de vencimento inv�lida");
                ocorrencias.Add("17-Data de vencimento anterior � data de emiss�o");
                ocorrencias.Add("18-Vencimento fora do prazo de opera��o");
                ocorrencias.Add("20-Valor do t�tulo inv�lido");
                ocorrencias.Add("21-Esp�cie do t�tulo inv�lida");
                ocorrencias.Add("22-Esp�cie n�o permitida para a carteira");
                ocorrencias.Add("24-Data de emiss�o inv�lida");
                ocorrencias.Add("29-Valor do desconto maior/igual ao valor do t�tulo");
                ocorrencias.Add("31-Concess�o de desconto - existe desconto anterior");
                ocorrencias.Add("33-Valor do abatimento inv�lido");
                ocorrencias.Add("34-Valor do abatimento maior/igual ao valor do t�tulo");
                ocorrencias.Add("36-Concess�o de abatimento - existe abatimento anterior");
                ocorrencias.Add("38-Prazo para protesto inv�lido");
                ocorrencias.Add("39-Pedido para protesto n�o permitido para o t�tulo");
                ocorrencias.Add("40-T�tulo com ordem de protesto emitida");
                ocorrencias.Add("41-Pedido cancelamento/susta��o sem instru��o de protesto");
                ocorrencias.Add("44-Cooperativa de cr�dito/ag�ncia benefici�ria n�o prevista");
                ocorrencias.Add("45-Nome do pagador inv�lido");
                ocorrencias.Add("46-Tipo/n�mero de inscri��o do pagador inv�lidos");
                ocorrencias.Add("47-Endere�o do pagador n�o informado");
                ocorrencias.Add("48-CEP irregular");
                ocorrencias.Add("49-N�mero de Inscri��o do pagador/avalista inv�lido");
                ocorrencias.Add("50-Pagador/avalista n�o informado");
                ocorrencias.Add("60-Movimento para t�tulo n�o cadastrado");
                ocorrencias.Add("63-Entrada para t�tulo j� cadastrado");
                ocorrencias.Add("A -Aceito");
                ocorrencias.Add("A1-Pra�a do pagador n�o cadastrada.");
                ocorrencias.Add("A2-Tipo de cobran�a do t�tulo divergente com a pra�a do pagador.");
                ocorrencias.Add("A3-Cooperativa/ag�ncia deposit�ria divergente: atualiza o cadastro de pra�as da Coop./ag�ncia benefici�ria");
                ocorrencias.Add("A4-Benefici�rio n�o cadastrado ou possui CGC/CIC inv�lido");
                ocorrencias.Add("A5-Pagador n�o cadastrado");
                ocorrencias.Add("A6-Data da instru��o/ocorr�ncia inv�lida");
                ocorrencias.Add("A7-Ocorr�ncia n�o pode ser comandada");
                ocorrencias.Add("A8-Recebimento da liquida��o fora da rede Sicredi - via compensa��o eletr�nica");
                ocorrencias.Add("B4-Tipo de moeda inv�lido");
                ocorrencias.Add("B5-Tipo de desconto/juros inv�lido");
                ocorrencias.Add("B6-Mensagem padr�o n�o cadastrada");
                ocorrencias.Add("B7-Seu n�mero inv�lido");
                ocorrencias.Add("B8-Percentual de multa inv�lido");
                ocorrencias.Add("B9-Valor ou percentual de juros inv�lido");
                ocorrencias.Add("C1-Data limite para concess�o de desconto inv�lida");
                ocorrencias.Add("C2-Aceite do t�tulo inv�lido");
                ocorrencias.Add("C3-Campo alterado na instru��o �31 � altera��o de outros dados� inv�lido");
                ocorrencias.Add("C4-T�tulo ainda n�o foi confirmado pela centralizadora");
                ocorrencias.Add("C5-T�tulo rejeitado pela centralizadora");
                ocorrencias.Add("C6-T�tulo j� liquidado");
                ocorrencias.Add("C7-T�tulo j� baixado");
                ocorrencias.Add("C8-Existe mesma instru��o pendente de confirma��o para este t�tulo");
                ocorrencias.Add("C9-Instru��o pr�via de concess�o de abatimento n�o existe ou n�o confirmada");
                ocorrencias.Add("D -Desprezado");
                ocorrencias.Add("D1-T�tulo dentro do prazo de vencimento (em dia);");
                ocorrencias.Add("D2-Esp�cie de documento n�o permite protesto de t�tulo");
                ocorrencias.Add("D3-T�tulo possui instru��o de baixa pendente de confirma��o");
                ocorrencias.Add("D4-Quantidade de mensagens padr�o excede o limite permitido");
                ocorrencias.Add("D5-Quantidade inv�lida no pedido de boletos pr�-impressos da cobran�a sem registro");
                ocorrencias.Add("D6-Tipo de impress�o inv�lida para cobran�a sem registro");
                ocorrencias.Add("D7-Cidade ou Estado do pagador n�o informado");
                ocorrencias.Add("D8-Seq��ncia para composi��o do nosso n�mero do ano atual esgotada");
                ocorrencias.Add("D9-Registro mensagem para t�tulo n�o cadastrado");
                ocorrencias.Add("E2-Registro complementar ao cadastro do t�tulo da cobran�a com e sem registro n�o cadastrado");
                ocorrencias.Add("E3-Tipo de postagem inv�lido, diferente de S, N e branco");
                ocorrencias.Add("E4-Pedido de boletos pr�-impressos");
                ocorrencias.Add("E5-Confirma��o/rejei��o para pedidos de boletos n�o cadastrado");
                ocorrencias.Add("E6-Pagador/avalista n�o cadastrado");
                ocorrencias.Add("E7-Informa��o para atualiza��o do valor do t�tulo para protesto inv�lido");
                ocorrencias.Add("E8-Tipo de impress�o inv�lido, diferente de A, B e branco");
                ocorrencias.Add("E9-C�digo do pagador do t�tulo divergente com o c�digo da cooperativa de cr�dito");
                ocorrencias.Add("F1-Liquidado no sistema do cliente");
                ocorrencias.Add("F2-Baixado no sistema do cliente");
                ocorrencias.Add("F3-Instru��o inv�lida, este t�tulo est� caucionado/descontado");
                ocorrencias.Add("F4-Instru��o fixa com caracteres inv�lidos");
                ocorrencias.Add("F6-Nosso n�mero / n�mero da parcela fora de seq��ncia � total de parcelas inv�lido");
                ocorrencias.Add("F7-Falta de comprovante de presta��o de servi�o");
                ocorrencias.Add("F8-Nome do benefici�rio incompleto / incorreto.");
                ocorrencias.Add("F9-CNPJ / CPF incompat�vel com o nome do pagador / Sacador Avalista");
                ocorrencias.Add("G1-CNPJ / CPF do pagador Incompat�vel com a esp�cie");
                ocorrencias.Add("G2-T�tulo aceito: sem a assinatura do pagador");
                ocorrencias.Add("G3-T�tulo aceito: rasurado ou rasgado");
                ocorrencias.Add("G4-T�tulo aceito: falta t�tulo (cooperativa/ag. benefici�ria dever� envi�-lo);");
                ocorrencias.Add("G5-Pra�a de pagamento incompat�vel com o endere�o");
                ocorrencias.Add("G6-T�tulo aceito: sem endosso ou benefici�rio irregular");
                ocorrencias.Add("G7-T�tulo aceito: valor por extenso diferente do valor num�rico");
                ocorrencias.Add("G8-Saldo maior que o valor do t�tulo");
                ocorrencias.Add("G9-Tipo de endosso inv�lido");
                ocorrencias.Add("H1-Nome do pagador incompleto / Incorreto");
                ocorrencias.Add("H2-Susta��o judicial");
                ocorrencias.Add("H3-Pagador n�o encontrado");
                ocorrencias.Add("H4-Altera��o de carteira");
                ocorrencias.Add("H5-Recebimento de liquida��o fora da rede Sicredi � VLB Inferior � Via Compensa��o");
                ocorrencias.Add("H6-Recebimento de liquida��o fora da rede Sicredi � VLB Superior � Via Compensa��o");
                ocorrencias.Add("H7-Esp�cie de documento necessita benefici�rio ou avalista PJ");
                ocorrencias.Add("H8-Recebimento de liquida��o fora da rede Sicredi � Conting�ncia Via Compe");
                ocorrencias.Add("H9-Dados do t�tulo n�o conferem com disquete");
                ocorrencias.Add("I1-Pagador e Sacador Avalista s�o a mesma pessoa");
                ocorrencias.Add("I2-Aguardar um dia �til ap�s o vencimento para protestar");
                ocorrencias.Add("I3-Data do vencimento rasurada");
                ocorrencias.Add("I4-Vencimento � extenso n�o confere com n�mero");
                ocorrencias.Add("I5-Falta data de vencimento no t�tulo");
                ocorrencias.Add("I6-DM/DMI sem comprovante autenticado ou declara��o");
                ocorrencias.Add("I7-Comprovante ileg�vel para confer�ncia e microfilmagem");
                ocorrencias.Add("I8-Nome solicitado n�o confere com emitente ou pagador");
                ocorrencias.Add("I9-Confirmar se s�o 2 emitentes. Se sim, indicar os dados dos 2");
                ocorrencias.Add("J1-Endere�o do pagador igual ao do pagador ou do portador");
                ocorrencias.Add("J2-Endere�o do apresentante incompleto ou n�o informado");
                ocorrencias.Add("J3-Rua/n�mero inexistente no endere�o");
                ocorrencias.Add("J4-Falta endosso do favorecido para o apresentante");
                ocorrencias.Add("J5-Data da emiss�o rasurada");
                ocorrencias.Add("J6-Falta assinatura do pagador no t�tulo");
                ocorrencias.Add("J7-Nome do apresentante n�o informado/incompleto/incorreto");
                ocorrencias.Add("J8-Erro de preenchimento do titulo");
                ocorrencias.Add("J9-Titulo com direito de regresso vencido");
                ocorrencias.Add("K1-Titulo apresentado em duplicidade");
                ocorrencias.Add("K2-Titulo j� protestado");
                ocorrencias.Add("K3-Letra de cambio vencida � falta aceite do pagador");
                ocorrencias.Add("K4-Falta declara��o de saldo assinada no t�tulo");
                ocorrencias.Add("K5-Contrato de cambio � Falta conta gr�fica");
                ocorrencias.Add("K6-Aus�ncia do documento f�sico");
                ocorrencias.Add("K7-Pagador falecido");
                ocorrencias.Add("K8-Pagador apresentou quita��o do t�tulo");
                ocorrencias.Add("K9-T�tulo de outra jurisdi��o territorial");
                ocorrencias.Add("L1-T�tulo com emiss�o anterior a concordata do pagador");
                ocorrencias.Add("L2-Pagador consta na lista de fal�ncia");
                ocorrencias.Add("L3-Apresentante n�o aceita publica��o de edital");
                ocorrencias.Add("L4-Dados do Pagador em Branco ou inv�lido");
                ocorrencias.Add("L5-C�digo do Pagador na ag�ncia benefici�ria est� duplicado");
                ocorrencias.Add("M1-Reconhecimento da d�vida pelo pagador");
                ocorrencias.Add("M2-N�o reconhecimento da d�vida pelo pagador");
                ocorrencias.Add("M3-Inclus�o de desconto 2 e desconto 3 inv�lida");
                ocorrencias.Add("X0-Pago com cheque");
                ocorrencias.Add("X1-Regulariza��o centralizadora � Rede Sicredi");
                ocorrencias.Add("X2-Regulariza��o centralizadora � Compensa��o");
                ocorrencias.Add("X3-Regulariza��o centralizadora � Banco correspondente");
                ocorrencias.Add("X4-Regulariza��o centralizadora - VLB Inferior - via compensa��o");
                ocorrencias.Add("X5-Regulariza��o centralizadora - VLB Superior - via compensa��o");
                ocorrencias.Add("X6-Pago com cheque � bloqueado 24 horas");
                ocorrencias.Add("X7-Pago com cheque � bloqueado 48 horas");
                ocorrencias.Add("X8-Pago com cheque � bloqueado 72 horas");
                ocorrencias.Add("X9-Pago com cheque � bloqueado 96 horas");
                ocorrencias.Add("XA-Pago com cheque � bloqueado 120 horas");
                ocorrencias.Add("XB-Pago com cheque � bloqueado 144 horas");
                #endregion

                var ocorrencia = (from s in ocorrencias where s.Substring(0, 2) == codigorejeicao.Substring(0, 2) select s).FirstOrDefault();

                if (ocorrencia != null)
                    rejeicao = ocorrencia;
            }

            return rejeicao;
        }

        // 7.3 Tabela de Motivos da Ocorr�ncia �28 � Tarifas� Maio 2020 v1.6
        private string LerMotivoRejeicaoTarifas(string codigorejeicao)
        {
            var rejeicao = String.Empty;

            if (codigorejeicao.Length >= 2)
            {
                #region LISTA DE MOTIVOS
                List<String> ocorrencias = new List<string>();

                ocorrencias.Add("03-Tarifa de susta��o");
                ocorrencias.Add("04-Tarifa de protesto");
                ocorrencias.Add("08-Tarifa de custas de protesto");
                ocorrencias.Add("A9-Tarifa de manuten��o de t�tulo vencido");
                ocorrencias.Add("B1-Tarifa de baixa da carteira");
                ocorrencias.Add("B3-Tarifa de registro de entrada do t�tulo");
                ocorrencias.Add("F5-Tarifa de entrada na rede Sicredi");
                ocorrencias.Add("S4-Tarifa de Inclus�o Negativa��o");
                ocorrencias.Add("S5-Tarifa de Exclus�o Negativa��o");
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
                detalhe.NossoNumero = reg.NossoNumeroSicredi.Substring(0, reg.NossoNumeroSicredi.Length - 1); //Nosso N�mero sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumeroSicredi.Substring(reg.NossoNumeroSicredi.Length - 1); //DV do Nosso Numero
                #endregion
                //Filler3
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.Ocorrencia);
                int dataOcorrencia = Utils.ToInt32(reg.DataOcorrencia);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Descri��o da ocorr�ncia
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
                //Despesas de cobran�a para os C�digos de Ocorr�ncia (Valor Despesa)
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
                //Abatimento Concedido sobre o T�tulo (Valor Abatimento Concedido)
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
                //detalhe.Especie = reg.TipoDocumento; //Verificar Esp�cie de Documentos...
                detalhe.OutrosCreditos = 0;
                detalhe.OrigemPagamento = String.Empty;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //
                detalhe.IOF = 0;
                //Motivos das Rejei��es para os C�digos de Ocorr�ncia
                if (detalhe.CodigoOcorrencia == 28)
                {
                    detalhe.MotivosRejeicao = LerMotivoRejeicaoTarifas(detalhe.MotivoCodigoOcorrencia);
                }
                else
                {
                    detalhe.MotivosRejeicao = LerMotivoRejeicao(detalhe.MotivoCodigoOcorrencia);
                }

                //N�mero do Cart�rio
                detalhe.NumeroCartorio = 0;
                //N�mero do Protocolo
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
                //Ag�ncia Cobradora
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
            throw new BoletoNetException("Nosso n�mero � inv�lido!");
        }
    }
}
