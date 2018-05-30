using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoletoNet.Util;


namespace BoletoNet
{
    internal class Banco_Maxima : AbstractBanco, IBanco
    {
        internal Banco_Maxima()
        {
            this.Codigo = (int)Enums.Bancos.Maxima;
            this.Digito = "7";
            this.Nome = "Máxima";
        }

        /// <summary>
        /// Gera nosso numero a ser colocado na remessa segundo layout para troca de informações
        /// </summary>
        /// <param name="boleto"></param>
        /// <returns></returns>
        private string NossoNumeroFormatado(Boleto boleto)
        {
            //FormataNossoNumero(boleto);

            string retorno = Utils.FormatCode(boleto.NossoNumero.Replace("-", ""), "0", 10, true); // nosso numero+dg - 10 posicoes
            retorno += Utils.FormatCode(boleto.NumeroParcela.ToString(), "0", 2, true); // numero parcela - 2 posicoes
            retorno += Utils.FormatCode(boleto.ModalidadeCobranca.ToString(), "0", 2, true); // modalidade - 2 posicoes
            retorno += "4"; // tipo formulario (A4 sem envelopamento) - 1 posicoes;
            retorno += Utils.FormatCode("", " ", 5); // brancos - 5 posicoes ;
            return retorno;
        }

        private string ContaBancariaFormatada(Cedente cedente)
        {
            string linha;

            if (cedente != null && cedente.ContaBancaria != null)
            {
                // Agência Mantenedora da Conta
                linha = Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);
                // Dígito Verificador da Agência
                linha += String.IsNullOrEmpty(cedente.ContaBancaria.DigitoAgencia) ? " " : cedente.ContaBancaria.DigitoAgencia;
                // Número da Conta Corrente
                linha += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);
                // Dígito Verificador da Conta
                linha += String.IsNullOrEmpty(cedente.ContaBancaria.DigitoConta) ? " " : cedente.ContaBancaria.DigitoConta;
            }
            else
            {
                linha = new string('0', 19);
            }

            return linha;
        }


        #region HEADER
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string header = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderRemessaCNAB240(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// Retorna o Header do arquivo Remessa conforme Layout Padrão Febraban 240 posições V10.3.
        /// </summary>
        /// <seealso cref="http://www.febraban.org.br"/>
        /// <returns></returns>
        private string GerarHeaderRemessaCNAB240(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                StringBuilder header = new StringBuilder(240);
                // Código do Banco na compensação
                header.Append(Codigo.ToString("D3"));
                // Lote de Serviço
                header.Append("0000");
                // tipo de Registro
                header.Append("0");
                // Brancos
                header.AppendFormat("{0,9}", " ");
                // Tipo de Inscrição da Empresa
                header.Append((cedente.CPFCNPJ.Length == 11 ? "1" : "2"));
                // Número de Inscrição da Empresa
                header.Append(Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true));
                // Código do Convênio no Banco
                header.Append(Utils.FormatCode(numeroConvenio.ToString(), " ", 20));

                // Agencia e Conta
                header.Append(ContaBancariaFormatada(cedente));

                // Dígito Verificador da Ag/Conta
                header.Append(" ");
                // Nome da Empresa
                header.Append(Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false));
                // Nome do Banco
                header.Append(Utils.FormatCode(Nome, " ", 30));
                // Brancos
                header.AppendFormat("{0,10}", " ");
                // Código Remessa / Retorno: '1' = Remessa (Cliente Banco)
                header.Append("1");
                // Data Hora de Geração do Arquivo. 
                header.Append(DateTime.Now.ToString("ddMMyyyyHHmmss"));
                // Número Seqüencial do Arquivo
                header.Append(numeroArquivoRemessa.ToString("D6"));
                // No da Versão do Layout do Arquivo
                header.Append("103");
                // Densidade de Gravação do Arquivo
                header.Append("00000");
                // Para Uso Reservado do Banco / Empresa / FEBRABAN / CNAB
                header.AppendFormat("{0,69}", " ");
                return Utils.SubstituiCaracteresEspeciais(header.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        #endregion

        # region Header do Lote
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = String.Empty;

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa, numeroConvenio);
                        break;
                    case TipoArquivo.CNAB400:
                        header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int sequencialRemessa, string codigoConvenio)
        {
            try
            {
                // Banco
                string header = Codigo.ToString("D3");
                // Lote de serviço
                header += "0001";
                // Tipo de Registro
                header += "1";
                // Tipo da Operação - 'R' = Arquivo Remessa
                header += "R";
                // Tipo do Serviço - '01' = Cobrança
                header += "01";
                // Uso Exclusivo da FEBRABAN/CNAB - Brancos
                header += "  ";
                // Nº da Versão do Layout do Lote - 060
                header += "060";
                // Uso Exclusivo FEBRABAN/CNAB - Brancos
                header += " ";
                // Tipo de Inscrição da Empresa
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");
                // Número de Inscrição da Empresa
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);
                // Código do Convênio no Banco
                header += Utils.FormatCode(codigoConvenio, " ", 20);
                // Agencia e Conta
                header += ContaBancariaFormatada(cedente);

                // Dígito Verificador da Ag/Conta
                header += " ";
                // Nome da Empresa
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                // Mensagem 1
                header += Utils.FormatCode("", " ", 40);
                // Mensagem 2
                header += Utils.FormatCode("", " ", 40);
                // Número Remessa/Retorno
                header += sequencialRemessa.ToString("D8");
                // Data de Gravação Remessa/Retorno
                header += DateTime.Today.ToString("ddMMyyyy");
                // Data do Crédito
                header += Utils.FormatCode("", "0", 8);
                // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 33);

                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);

            }
        }

        private string GerarHeaderLoteRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Detalhe Segmento P
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                // Código do Banco na Compensação 
                string detalhe = Codigo.ToString("D3");
                // Lote de Serviço 
                detalhe += "0001";
                // Tipo de Registro 
                detalhe += "3";
                // Nº Sequencial do Registro no Lote 
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                // Cód. Segmento do Registro Detalhe 
                detalhe += "P";
                // Uso Exclusivo FEBRABAN/CNAB 
                detalhe += " ";
                // Código de Movimento Remessa 
                detalhe += ObterCodigoDaOcorrencia(boleto);

                // Agencia e Conta
                detalhe += ContaBancariaFormatada(boleto.Cedente);
                // Dígito Verificador da Ag/Conta 
                detalhe += " ";
                // Posição 38 a 57  - Nosso Número
                detalhe += Utils.FormatCode(NossoNumeroFormatado(boleto), 20);
                // Posição 58 - Código da Carteira
                if (!String.IsNullOrEmpty(boleto.Carteira) && boleto.Carteira.Length == 1)
                    detalhe += Utils.FitStringLength(boleto.Carteira, 1, 1, '0', 0, true, true, true);
                else
                    detalhe += "0";
                // Posição 59       - Forma de Cadastr. do Título no Banco: "1" - Cobrança Registrada
                detalhe += "1";
                //Posição 60        - Tipo de Documento: Brancos
                detalhe += " ";
                //Posição 61        - Identificação da Emissão do Boleto: 1=Banco Emite
                detalhe += "1";
                // Posição 62       - Identificação da Distribuição. '1' = Banco Distribui
                detalhe += "1";
                //Posição 63 a 77   - Número do documento de cobrança.
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 15);
                // Posição 78 a 85  - Data de Vencimento do Título
                detalhe += Utils.FitStringLength(boleto.DataVencimento == DateTime.MinValue ? "0" : boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                // Posição 86 a 100 - Valor Nominal do Título
                detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                // Posição 101 a 105- Agência Encarregada da Cobrança 
                detalhe += "00000";
                // Posição 106      - Dígito Verificador da Agência 
                detalhe += " ";
                // Posição 107 A 108- Espécie do Título 
                detalhe += "02";
                // Posição 109      - Identific. de Título Aceito/Não Aceito 
                detalhe += "A";
                // Posição 110 A 117- Data da Emissão do Título 
                detalhe += Utils.FitStringLength(boleto.DataDocumento == DateTime.MinValue ? "0" : boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                // Posição 118      - Código do Juros de Mora 
                detalhe += "0";
                // Posição 119 A 126- Data do Juros de Mora 
                detalhe += Utils.FitStringLength(boleto.DataJurosMora == DateTime.MinValue ? "0" : boleto.DataJurosMora.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                // Posição 127 A 141- Juros de Mora por Dia/Taxa 
                detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                // Posição 142      - Código do Desconto 1 
                detalhe += "0";
                // Posição 143 a 150- Data do Desconto 1 
                detalhe += Utils.FitStringLength(boleto.DataDesconto == DateTime.MinValue ? "0" : boleto.DataDesconto.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                // Posição 151 a 165- Valor/Percentual a ser Concedido 
                detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                // Posição 166 a 180- Valor do IOF a ser Recolhido 
                detalhe += Utils.FitStringLength(boleto.IOF.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                // Posição 181 a 195- Valor do Abatimento 
                detalhe += Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                // Posição 196 a 220- Identificação do Título na Empresa 
                detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);
                // Posição 221      - Código para Protesto 
                detalhe += "0";
                // Posição 222 a 223- Número de Dias para Protesto 
                detalhe += "00";
                // Posição 224      - Código para Baixa/Devolução 
                detalhe += "0";
                // Posição 225 a 227- Número de Dias para Baixa/Devolução 
                detalhe += "   ";
                // Posição 228 a 229- Código da Moeda 
                detalhe += "00";
                // Posição 230 a 239- Nº do Contrato da Operação de Créd. 
                detalhe += new string('0', 10);
                // Posição 240      - Uso livre banco/empresa ou autorização de pagamento parcial
                detalhe += " ";

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);

                return detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region Detalhe Segmento Q
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                // Posição 001 a 003    - Código do Sicoob na Compensação: "756"
                string detalhe = Codigo.ToString("D3");
                // Posição 004 a 007    - Lote
                detalhe += "0001";
                // Posição 008          - Tipo de Registro: "3"
                detalhe += "3";
                // Posição 009 a 013    - Número Sequencial
                detalhe += numeroRegistro.ToString("D5");
                // Posição 014          - Cód. Segmento do Registro Detalhe: "P"
                detalhe += "Q";
                // Posição 015          - Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += " ";
                // Posição 016 a 017    - '01'  =  Entrada de Títulos
                detalhe += "01";
                // Posição 018          - 1=CPF    2=CGC/CNPJ
                detalhe += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");
                //Posição 019 a 033     - Número de Inscrição da Empresa
                detalhe += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15, true);
                // Posição 034 a 73     - Nome
                detalhe += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);
                // Posição 074 a 113    - Endereço
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);
                // Posição 114 a 128    - Bairro
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);
                // Posição 129 a 133    - CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.CEP, 8);
                // Posição 137 a 151    - Cidade
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);
                // Posição 152 a 153    - Unidade da Federação
                detalhe += boleto.Sacado.Endereco.UF;
                // Posição 154          - Tipo de Inscrição Sacador avalista
                detalhe += (boleto.Cedente.CPFCNPJ.Length == 11 ? "1" : "2");
                // Posição 155 a 169    - Número de Inscrição / Sacador avalista
                detalhe += Utils.FormatCode(boleto.Cedente.CPFCNPJ, "0", 15, true);
                // Posição 170 a 209    - // Nome / Sacador avalista
                detalhe += Utils.FormatCode(boleto.Cedente.Nome, " ", 40);
                // Posição 210 a 212    - Código Bco. Corresp. na Compensação
                detalhe += "000";
                // Posição 213 a 232    - Nosso N° no Banco Correspondente "1323739"
                detalhe += Utils.FormatCode("", " ", 20);
                // Posição 233 a 240    - Uso Exclusivo FEBRABAN/CNAB
                detalhe += Utils.FormatCode("", " ", 8);

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe).ToUpper();
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", e);
            }
        }
        #endregion

        #region Trailer Lote Remessa
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                // Posição 1 a 3        - Código do banco
                string trailer = Codigo.ToString("D3");
                // Posição 4 a 7        - Lote de serviço
                trailer += "0001";
                // Posição 8            - Tipo de Registro
                trailer += "5";
                // Posição 9 a 17       - Exclusivo FEBRABAN/CNAB: Brancos
                trailer += Utils.FormatCode("", " ", 9);
                // Posição 18 a 23      - Quantidade de Registros no Lote
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);
                // Totalização da Cobrança Simples
                // Posição 24 a 29      - Quantidade de Títulos em Cobrança 
                trailer += Utils.FormatCode("", "0", 6, true);
                // Posição 30 a 46      - Valor Total dos Títulos em Carteiras 
                trailer += Utils.FormatCode("", "0", 17, true);
                // Totalização da Cobrança Vinculada 
                // Posição 47 a 52      - Quantidade de Títulos em Cobrança 
                trailer += Utils.FormatCode("", "0", 6, true);
                // Posição 53 a 69      - Valor Total dos Títulos em Carteiras 
                trailer += Utils.FormatCode("", "0", 17, true);
                // Totalização da Cobrança Caucionada 
                // Posição 70 a 75      - Quantidade de Títulos em Cobrança 
                trailer += Utils.FormatCode("", "0", 6, true);
                // Posição 76 a 92      - Valor Total dos Títulos em Carteiras 
                trailer += Utils.FormatCode("", "0", 17, true);
                // Totalização da Cobrança Descontada 
                // Posição 93 a 98      - Quantidade de Títulos em Cobrança 
                trailer += Utils.FormatCode("", "0", 6, true);
                // Posição 99 a 115     - Valor Total dos Títulos em Carteiras 
                trailer += Utils.FormatCode("", "0", 17, true);
                // Posição 116 a 123    - Número do Aviso de Lançamento 
                trailer += Utils.FormatCode("", "0", 8, true);
                // Posição 124 a 240    - Uso Exclusivo FEBRABAN/CNAB 
                trailer += Utils.FormatCode("", " ", 117);

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }
        #endregion

        #region Trailer Arquivo Remessa
        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                string trailer = Codigo.ToString("D3");

                //Numero do lote remessa ==> 004 - 007
                trailer += "9999";

                //Tipo de registro ==> 008 - 008
                trailer += "9";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de lotes do arquivo ==> 018 - 023
                trailer += Utils.FormatCode("1", "0", 6, true);

                //Quantidade de registros do arquivo ==> 024 - 029
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Quantidade de registros do arquivo ==> 030 - 035
                trailer += Utils.FormatCode("", "0", 6, true);

                //Reservado (uso Banco) ==> 036 - 240
                trailer += Utils.FormatCode("", " ", 205);

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }
        #endregion


    }
}
