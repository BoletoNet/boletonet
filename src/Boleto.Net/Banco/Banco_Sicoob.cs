using System;
using System.Text;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.756.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Bancoob Sicoob Crédibom.
    /// Autor: Janiel Madureira Oliveira
    /// E-mail: janielbelmont@msn.com
    /// Twitter: @janiel14
    /// Data: 01/02/2012
    /// Obs: Os arquivo de remessa CNAB 400 foi implementado para cobranças com registros seguindo o padrão CBR641.
    /// 
    /// Atualização
    /// Mudança no logotipo para o SICOOB NO GERAL
    /// Criação de rotinas ausentes
    /// Autor: Adriano trentim Augusto
    /// E-mail: adriano@setrin.com.br
    /// Data: 25/02/2014
    /// </summary>
    internal class Banco_Sicoob : AbstractBanco, IBanco
    {
        #region CONSTRUTOR
        /**
         * <summary>Construtor</summary>
         */
        internal Banco_Sicoob()
        {
            this.Nome = "Sicoob";
            this.Codigo = 756;
            this.Digito = "0";
        }
        #endregion CONSTRUTOR

        #region FORMATAÇÕES

        public override void FormataNossoNumero(Boleto boleto)
        {
            //Variaveis
            int resultado = 0;
            int dv = 0;
            int resto = 0;
            String constante = "319731973197319731973";
            String cooperativa = boleto.Cedente.ContaBancaria.Agencia;
            String codigo = boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente.ToString();
            String nossoNumero = boleto.NossoNumero;
            StringBuilder seqValidacao = new StringBuilder();

            /*
             * Preenchendo com zero a esquerda
             */
            //Tratando cooperativa
            for (int i = 0; i < 4 - cooperativa.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(cooperativa);
            //Tratando cliente
            for (int i = 0; i < 10 - codigo.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(codigo);
            //Tratando nosso número
            for (int i = 0; i < 7 - nossoNumero.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(nossoNumero);

            /*
             * Multiplicando cada posição por sua respectiva posição na constante.
             */
            for (int i = 0; i < 21; i++)
            {
                resultado = resultado + (Convert.ToInt16(seqValidacao.ToString().Substring(i, 1)) * Convert.ToInt16(constante.Substring(i, 1)));
            }
            //Calculando mod 11
            resto = resultado % 11;
            //Verifica resto
            if (resto == 1 || resto == 0)
            {
                dv = 0;
            }
            else
            {
                dv = 11 - resto;
            }
            //Montando nosso número
            boleto.NossoNumero = boleto.NossoNumero + "-" + dv.ToString();
            boleto.DigitoNossoNumero = dv.ToString();
        }

        /**
         * FormataCodigoCliente
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */

        public void FormataCodigoCliente(Cedente cedente)
        {
            if (cedente.Codigo.Length == 7)
                cedente.DigitoCedente = Convert.ToInt32(cedente.Codigo.Substring(6));

            cedente.Codigo = cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        public void FormataCodigoCliente(Boleto boleto)
        {
            if (boleto.Cedente.Codigo.Length == 7)
                boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.Codigo.Substring(6));

            boleto.Cedente.Codigo = boleto.Cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        /**
         * FormataNumeroTitulo
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroTitulo(Boleto boleto)
        {
            var novoTitulo = new StringBuilder();
            novoTitulo.Append(boleto.NossoNumero.Replace("-", "").PadLeft(8, '0'));
            return novoTitulo.ToString();
        }

        /**
         * FormataNumeroParcela
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroParcela(Boleto boleto)
        {
            if (boleto.NumeroParcela <= 0)
                boleto.NumeroParcela = 1;

            //Variaveis
            StringBuilder novoNumero = new StringBuilder();
 
            //Formatando
            for (int i = 0; i < (3 - boleto.NumeroParcela.ToString().Length); i++)
            {
                novoNumero.Append("0");
            }
            novoNumero.Append(boleto.NumeroParcela.ToString());
            return novoNumero.ToString();
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            //Variaveis
            int peso = 2;
            int soma = 0;
            int resultado = 0;
            int dv = 0;
            String codigoValidacao = boleto.Banco.Codigo.ToString() + boleto.Moeda.ToString() + FatorVencimento(boleto).ToString() +
                Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10) + boleto.Carteira +
                boleto.Cedente.ContaBancaria.Agencia + boleto.TipoModalidade + boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente +
                this.FormataNumeroTitulo(boleto) + this.FormataNumeroParcela(boleto);

            //Calculando
            for (int i = (codigoValidacao.Length - 1); i >= 0; i--)
            {
                soma = soma + (Convert.ToInt16(codigoValidacao.Substring(i, 1)) * peso);
                peso++;
                //Verifica peso
                if (peso > 9)
                {
                    peso = 2;
                }
            }
            resultado = soma % 11;
            dv = 11 - resultado;
            //Verificando resultado
            if (dv == 0 || dv == 1 || dv > 9)
            {
                dv = 1;
            }
            //Formatando
            boleto.CodigoBarra.Codigo = codigoValidacao.Substring(0, 4) + dv + codigoValidacao.Substring(4);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //Variaveis
            String campo1 = string.Empty;
            String campo2 = string.Empty;
            String campo3 = string.Empty;
            String campo4 = string.Empty;
            String campo5 = string.Empty;
            String indice = "1212121212";
            StringBuilder linhaDigitavel = new StringBuilder();
            int soma = 0;
            int temp = 0;

            //Formatando o campo 1
            campo1 = boleto.Banco.Codigo.ToString() + boleto.Moeda.ToString() + boleto.Carteira + boleto.Cedente.ContaBancaria.Agencia;
            //Calculando CAMPO 1
            for (int i = 0; i < campo1.Length; i++)
            {
                //Calculando indice
                temp = (Convert.ToInt16(campo1.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i+1, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }
            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo1.Substring(0, 5), campo1.Substring(5, 4), Multiplo10(soma)));
            
            soma = 0;
            temp = 0;
            //Formatando o campo 2
            campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            for (int i = 0; i < campo2.Length; i++)
            {
                //Calculando Indice 2
                temp = (Convert.ToInt16(campo2.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }

            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo2.Substring(0, 5), campo2.Substring(5, 5), Multiplo10(soma)));

            soma = 0;
            temp = 0;
            //Formatando campo 3
            campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            for (int i = 0; i < campo3.Length; i++)
            {
                //Calculando indice 2
                temp = (Convert.ToInt16(campo3.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando resultado
                soma = soma + temp;
            }
            linhaDigitavel.Append(campo3.Substring(0, 5) + "." + campo3.Substring(5, 5) + Multiplo10(soma) + " ");
            //Formatando Campo 4
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            linhaDigitavel.Append(campo4 + " ");
            //Formatando Campo 5
            campo5 = FatorVencimento(boleto).ToString() + Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);
            linhaDigitavel.Append(campo5);
            boleto.CodigoBarra.LinhaDigitavel = linhaDigitavel.ToString();
        }

        #endregion FORMATAÇÕES

        #region VALIDAÇÕES

        public override void ValidaBoleto(Boleto boleto)
        {
            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";


            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGÁVEL EM QUALQUER CORRESPONDENTE BANCÁRIO PERTO DE VOCÊ!";

            //Aplicando formatações
            this.FormataCodigoCliente(boleto);
            this.FormataNossoNumero(boleto);
            this.FormataCodigoBarra(boleto);
            this.FormataLinhaDigitavel(boleto);
        }

        #endregion VALIDAÇÕES

        #region ARQUIVO DE REMESSA

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                this.FormataCodigoCliente(cedente);

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
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

        private string GerarHeaderRemessaCNAB240(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            //Variaveis
            StringBuilder _header = new StringBuilder();
            //Tratamento de erros
            try
            {
                //Montagem do header
                _header.Append("0"); //Posição 001
                _header.Append("1"); //Posição 002
                _header.Append("REMESSA"); //Posição 003 a 009
                _header.Append("01"); //Posição 010 a 011
                _header.Append("COBRANÇA"); //Posição 012 a 019
                _header.Append(new string(' ', 7)); //Posição 020 a 026
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 027 a 030
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 031
                _header.Append(Utils.FitStringLength(cedente.Codigo, 8, 8, '0', 0, true, true, true)); //Posição 032 a 039
                _header.Append(Utils.FitStringLength(Convert.ToString(cedente.DigitoCedente), 1, 1, '0', 0, true, true, true)); //Posição 40
                _header.Append(new string(' ', 6)); //Posição 041 a 046
                _header.Append(Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false)); //Posição 047 a 076
                _header.Append(Utils.FitStringLength("756BANCOOBCED", 18, 18, ' ', 0, true, true, false)); //Posição 077 a 094
                _header.Append(DateTime.Now.ToString("ddMMyy")); //Posição 095 a 100
                _header.Append(Utils.FitStringLength(Convert.ToString(cedente.NumeroSequencial), 7, 7, '0', 0, true, true, true)); //Posição 101 a 107
                _header.Append(new string(' ', 287)); //Posição 108 a 394
                _header.Append("000001"); //Posição 395 a 400

                //Retorno
                return _header.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                FormataNossoNumero(boleto);
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

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
                throw new Exception("Erro durante a geração do DETALHE do arquivo de REMESSA.", ex);
            }
        }

        private string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            //Variaveis
            var _detalhe = new StringBuilder();

            //Tratamento de erros
            try
            {
                //Montagem do Detalhe
                _detalhe.Append("1"); //Posição 001
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Cedente.CPFCNPJ)); //Posição 002 a 003
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 004 a 017
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 018 a 021
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 022
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true)); //Posição 023 a 030
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true)); //Posição 031
                _detalhe.Append(new string('0', 6)); //Posição 032 a 037
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false)); //Posição 038 a 62
                _detalhe.Append(Utils.FitStringLength(FormataNumeroTitulo(boleto), 12, 12, '0', 0, true, true, true)); //Posição 063 a 074
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroParcela.ToString(), 2, 2, '0', 0, true, true, true)); //Posição 075 a 076
                _detalhe.Append("00"); //Posição 077 a 078
                _detalhe.Append("   "); //Posição 079 a 081
                _detalhe.Append(" "); //Posição 082
                _detalhe.Append("   "); //Posição 083 a 085
                _detalhe.Append("000"); //Posição 086 a 088
                _detalhe.Append("0"); //Posição 089
                _detalhe.Append("00000"); //Posição 090 a 094
                _detalhe.Append("0"); //Posição 095
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.NumeroBordero.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 096 a 101
                _detalhe.Append(new string(' ', 5)); //Posição 102 a 106
                _detalhe.Append(Utils.FitStringLength(boleto.TipoModalidade, 2, 2, '0', 0, true, true, true));  //Posição 107 a 108
                _detalhe.Append("01"); //Posição 109 a 110 - REGISTRO DE TITULOS
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, '0', 0, true, true, true)); //Posição 111 a 120
                _detalhe.Append(boleto.DataVencimento.ToString("ddMMyy")); //Posição 121 a 126
                _detalhe.Append(Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 127 a 139 
                _detalhe.Append(boleto.Banco.Codigo); //Posição 140 a 142
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 143 a 146
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 147
                _detalhe.Append(boleto.EspecieDocumento.Codigo); //Posição 148 a 149
                _detalhe.Append(boleto.Aceite == "N" ? "0" : "1"); //Posição 150
                _detalhe.Append(boleto.DataProcessamento.ToString("ddMMyy")); //Posição 151 a 156
                _detalhe.Append("07"); //Posição 157 a 158 - NÂO PROTESTAR
                _detalhe.Append("22"); //Posição 159 a 160 - PERMITIR DESCONTO SOMENTE ATE DATA ESTIPULADA
                _detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercJurosMora * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //Posição 161 a 166
                _detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //Posição 167 a 172
                _detalhe.Append(" "); //Posição 173
                _detalhe.Append(Utils.FitStringLength((boleto.DataDesconto == DateTime.MinValue ? "0" : boleto.DataDesconto.ToString("ddMMyy")), 6, 6, '0', 0, true, true, true)); //Posição 174 a 179
                _detalhe.Append(Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 180 a 192
                _detalhe.Append("9" + Utils.FitStringLength(boleto.IOF.ToString("0.00").Replace(",", ""), 12, 12, '0', 0, true, true, true)); //Posição 193 a 205
                _detalhe.Append(Utils.FitStringLength(boleto.Abatimento.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 206 a 218
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CPFCNPJ)); //Posição 219 a 220
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 221 a 234
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false)); //Posição 235 a 274
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.End, 37, 37, ' ', 0, true, true, false)); //Posição 275 a 311
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 15, 15, ' ', 0, true, true, false)); //Posição 312 a 326
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true)); //Posição 327 a 334
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)); //Posição 335 a 349
                _detalhe.Append(boleto.Sacado.Endereco.UF); //Posição 350 a 351
                _detalhe.Append(new string(' ', 40)); //Posição 352 a 391 - OBSERVACOES
                _detalhe.Append("00"); //Posição 392 a 393 - DIAS PARA PROTESTO
                _detalhe.Append(" "); //Posição 394
                _detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 394 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_detalhe.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo de remessa do CNAB400.", ex);
            }
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            //Variavies
            StringBuilder _trailer = new StringBuilder();
            //Tratamento
            try
            {
                //Montagem trailer
                _trailer.Append("9"); //Posição 001
                _trailer.Append(new string(' ', 393)); //Posição 002 a 394
                _trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 395 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_trailer.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar TRAILER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion ARQUIVO DE REMESSA

        #region ::. Arquivo de Retorno CNAB400 .::
        
        /// <summary>
        /// Rotina de retorno de remessa
        /// Criador: Adriano Trentim Augusto
        /// E-mail: adriano@setrin.com.br
        /// Data: 29/04/2014
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var detalhe = new DetalheRetorno(registro);

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2)); //Tipo de Inscrição Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14); //Nº Inscrição da Empresa

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(23, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(30, 1));

                detalhe.NumeroControle = registro.Substring(37, 25); //Nº Controle do Participante
                
                //Identificação do Título no Banco
                detalhe.NossoNumero = registro.Substring(62, 11);
                detalhe.DACNossoNumero = registro.Substring(73, 1);

                switch (registro.Substring(106, 2)) // Carteira
	        {
	          case "01":
	            detalhe.Carteira = "1";
	            break;
	          case "02":
	            detalhe.Carteira = "1";
	            break;
	          case "03":
	            detalhe.Carteira = "3";
	            break;
	        }
	        
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2)); //Identificação de Ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2)); //Descrição da ocorrência
                detalhe.DataOcorrencia = Utils.ToDateTime(Utils.ToInt32(registro.Substring(110, 6)).ToString("##-##-##")); //Data da ocorrencia

                //Quando ocorrencia = Liquidação, pega a data.
                if (detalhe.CodigoOcorrencia == 5 || detalhe.CodigoOcorrencia == 6 || detalhe.CodigoOcorrencia == 15)
                    detalhe.DataLiquidacao = detalhe.DataOcorrencia;

                detalhe.NumeroDocumento = registro.Substring(116, 10); //Número do Documento
                detalhe.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(146, 6)).ToString("##-##-##")); //Data de Vencimento
                detalhe.ValorTitulo = ((decimal)Convert.ToInt64(registro.Substring(152, 13))) / 100; //Valor do Titulo
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3)); //Banco Cobrador
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4)); //Agência Cobradora
                detalhe.DACAgenciaCobradora = Utils.ToInt32(registro.Substring(172, 1)); // DV Agencia Cobradora
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2)); //Espécie do Título

                //Data de Crédito - Só vem preenchido quando liquidação
                if (registro.Substring(175, 6) != "000000")
                    detalhe.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(175, 6)).ToString("##-##-##"));
                else
                    detalhe.DataCredito = detalhe.DataOcorrencia;

                detalhe.ValorDespesa = ((decimal)Convert.ToUInt64(registro.Substring(181, 7))) / 100; //Valor das Tarifas
                detalhe.ValorOutrasDespesas = ((decimal)Convert.ToUInt64(registro.Substring(188, 13))) / 100; //Valor das Outras Despesas
                detalhe.ValorAbatimento = ((decimal)Convert.ToUInt64(registro.Substring(227, 13))) / 100; //Valor do abatimento
                detalhe.Descontos = ((decimal)Convert.ToUInt64(registro.Substring(240, 13))) / 100; //Valor do desconto
                detalhe.ValorPago = ((decimal)Convert.ToUInt64(registro.Substring(253, 13))) / 100; //Valor do Recebimento
                detalhe.JurosMora = ((decimal)Convert.ToUInt64(registro.Substring(266, 13))) / 100; //Valor de Juros
                detalhe.IdentificacaoTitulo = detalhe.NumeroDocumento; //Identificação do Título no Banco
                detalhe.OutrosCreditos = ((decimal)Convert.ToUInt64(registro.Substring(279, 13))) / 100; //Outros recebimentos

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        private string Ocorrencia(string codigoOcorrencia)
        {
            switch (codigoOcorrencia)
            {
                case "02":
                    return "Confirmação Entrada Título";
                case "03":
                    return "Comando Recusado";
                case "04":
                    return "Transferência de Carteira - Entrada";
                case "05":
                    return "Liquidação Sem Registro";
                case "06":
                    return "Liquidação Normal";
                case "09":
                    return "Baixa de Título";
                case "10":
                    return "Baixa Solicitada";
                case "11":
                    return "Títulos em Ser";
                case "12":
                    return "Abatimento Concedido";
                case "13":
                    return "Abatimento Cancelado";
                case "14":
                    return "Alteração de Vencimento";
                case "15":
                    return "Liquidação em Cartório";
                case "19":
                    return "Confirmação Instrução Protesto";
                case "20":
                    return "Débito em Conta";
                case "21":
                    return "Alteração de nome do Sacado";
                case "22":
                    return "Alteração de endereço Sacado";
                case "23":
                    return "Encaminhado a Protesto";
                case "24":
                    return "Sustar Protesto";
                case "25":
                    return "Dispensar Juros";
                case "26":
                    return "Instrução Rejeitada";
                case "27":
                    return "Confirmação Alteração Dados";
                case "28":
                    return "Manutenção Título Vencido";
                case "30":
                    return "Alteração Dados Rejeitada";
                case "96":
                    return "Despesas de Protesto";
                case "97":
                    return "Despesas de Sustação de Protesto";
                case "98":
                    return "Despesas de Custas Antecipadas";
                default:
                    return "Ocorrência não cadastrada";
            }
        }

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
