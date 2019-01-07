using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.021.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Itaú
    /// </summary>
    internal class Banco_Banestes : AbstractBanco, IBanco
    {

        #region Variáveis

        private int _dacBoleto = 0;
        private string _valorMoeda = string.Empty;

        #endregion

        #region Construtores

        internal Banco_Banestes()
        {
            Codigo = 21;
            Digito = "3";
            Nome = "Banestes";
        }

        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do banco Banestes
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                //Carteiras válidas
                // 00 - Sem registro;

                //Atribui o nome do banco ao local de pagamento
                boleto.LocalPagamento += Nome + ". Após o vencimento, somente no BANESTES";

                //Verifica se o nosso número é válido
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("Nosso número inválido");

                //Verifica se data do processamento é valida

                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;

                boleto.FormataCampos();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos.", e);
            }
        }

        # endregion

        # region Métodos de formatação do boleto
        /// <summary>
        /// Composição do Código de Barras.
        /// - Código do banco cedente 	 03 Posições (021)
        /// - Moeda 	                 01 Posição (9)
        /// - Dígito Verificador 	     01 Posição (descrito abaixo)
        /// - Fator de Vencimento....... 04 Posições  (descrito na página 19)
        /// - Valor do Título	10 Posições (8,2)
        /// - Chave ASBACE 	 25 Posições.
        /// No BANESTES ficou assim o Código de Barras.
        /// 0219DVVVVVVVVVVVVVVCCCCCCCCCCCCCCCCCCCCCCCCC
        /// Onde:
        /// 021	> Código do BANESTES
        /// 9 	> Moeda (Real)
        /// D 	> Dígito Verificador
        /// FFFF------------------------------------------------------------- > Fator de Vencimento
        /// VVVVVVVVVV-----	> Valor
        /// CCCCCCCCCCCCCCCCCCCCCCCCC 	> Chave ASBACE.
        /// </summary>
        /// <param name="boleto"></param>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                //0219DFFFFVVVVVVVVVVCCCCCCCCCCCCCCCCCCCCCCCCC

                var FFFF = FatorVencimento(boleto);

                var VVVVVVVVVV = _valorMoeda = Utils.FormatCode(boleto.ValorBoleto.ToString("N").Replace(".", "").Replace(",", ""), 10);

                boleto.Banco.ChaveASBACE = GeraChaveASBACE(boleto.Carteira, boleto.Cedente.ContaBancaria.Conta, boleto.NossoNumero, 2);

                string chave = string.Format("0219{0}{1}{2}", FFFF, VVVVVVVVVV, boleto.Banco.ChaveASBACE);

                int peso = 9;

                int S = 0;
                int P = 0;
                int N = 0;

                for (int i = 0; i < chave.Length; i++)
                {

                    N = Convert.ToInt32(chave.Substring(i, 1));

                    if (i == 0)
                        peso = 4;

                    P = N * peso--;

                    S += P;

                    if (peso == 1)
                        peso = 9;
                }

                int R = S % 11;

                if (R == 0 || R == 1 || R == 10)
                    _dacBoleto = 1;
                else
                    _dacBoleto = 11 - R;




                boleto.CodigoBarra.Codigo = string.Format("0219{0}{1}{2}{3}", _dacBoleto, FFFF, VVVVVVVVVV, boleto.Banco.ChaveASBACE);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                int dv = 0;

                #region Parte 1

                var parte1 = string.Concat("0219", boleto.Banco.ChaveASBACE.Substring(0, 5));

                dv = CalculaDigitoVerificador(parte1);

                parte1 = string.Concat(parte1, dv);

                #endregion

                #region Parte 2

                var parte2 = boleto.Banco.ChaveASBACE.Substring(5, 10);

                dv = CalculaDigitoVerificador(parte2, 1);

                parte2 = string.Concat(parte2, dv);

                #endregion

                #region Parte 3

                var parte3 = boleto.Banco.ChaveASBACE.Substring(15, 10);

                dv = CalculaDigitoVerificador(parte3, 1);

                parte3 = string.Concat(parte3, dv);

                #endregion

                #region Parte 5

                var parte5 = string.Concat(FatorVencimento(boleto), _valorMoeda);

                #endregion

                //var linhaDigitavel = string.Concat("0219", _dacBoleto, FatorVencimento(boleto), _valorMoeda, _chaveASBACE);

                boleto.CodigoBarra.LinhaDigitavel = string.Format("{0}.{1} {2}.{3} {4}.{5} {6} {7}",
                    parte1.Substring(0, 5),
                    parte1.Substring(5, 5),
                    parte2.Substring(0, 5),
                    parte2.Substring(5, 6),
                    parte3.Substring(0, 5),
                    parte3.Substring(5, 6),
                    _dacBoleto,
                    parte5);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digitável.", ex);
            }
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                boleto.NossoNumero = boleto.NossoNumero.Trim().Replace(".", "").Replace("-", "");

                if (string.IsNullOrEmpty(boleto.NossoNumero))
                    throw new Exception("Nosso Número não informado");

                if (boleto.NossoNumero.Length > 8)
                    throw new Exception("Tamanho máximo para o Nosso Número são de 8 caracteres");

                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);

                int D1 = CalculaDVNossoNumero(boleto.NossoNumero);

                int D2 = CalculaDVNossoNumero(string.Concat(boleto.NossoNumero, D1), 10);
                boleto.NossoNumero = string.Format("{0}.{1}{2}", boleto.NossoNumero, D1, D2);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        # endregion

        private int CalculaDVNossoNumero(string nossoNumero, short peso = 9)
        {
            int S = 0;
            int P = 0;
            int N = 0;
            int d = 0;

            for (int i = 0; i < nossoNumero.Length; i++)
            {
                N = Convert.ToInt32(nossoNumero.Substring(i, 1));

                P = N * peso--;

                S += P;
            }

            int R = S % 11;

            if (R == 0 || R == 1)
                d = 0;

            if (R > 1)
                d = 11 - R;

            return d;
        }

        private int CalculaDigitoVerificador(string chave, short peso = 2)
        {
            int D1 = 0;
            int K = 0;
            int S = 0;

            for (int i = 0; i < chave.Length; i++)
            {
                int N = Convert.ToInt32(chave.Substring(i, 1));

                int P = N * peso;

                if (P > 9)
                    K = P - 9;

                if (P < 10)
                    K = P;

                S += K;

                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            int resto = S % 10;

            if (resto == 0)
                D1 = 0;
            else if (resto > 0)
                D1 = 10 - resto;

            return D1;
        }

        private int CalculaD1(string chave)
        {
            int D1 = 0;
            short peso = 2;
            int K = 0;
            int S = 0;

            for (int i = 0; i < chave.Length; i++)
            {
                int N = Convert.ToInt32(chave.Substring(i, 1));

                int P = N * peso;

                if (P > 9)
                    K = P - 9;

                if (P < 10)
                    K = P;

                S += K;

                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            int resto = S % 10;

            if (resto == 0)
                D1 = 0;
            else if (resto > 0)
                D1 = 10 - resto;

            return D1;
        }

        private int CalculaD2(string chave, int D1)
        {
            int D2 = 0;
            short peso = 7;
            int P = 0;
            int S = 0;

            string chaveD1 = string.Concat(chave, D1);

            for (int i = 0; i < chaveD1.Length; i++)
            {
                int N = Convert.ToInt32(chaveD1.Substring(i, 1));

                P = N * peso--;

                S += P;

                if (peso == 1)
                    peso = 7;
            }

            int resto = S % 11;

            if (resto == 0)
                D2 = 0;

            if (resto == 1)
            {
                D1++;
                if (D1 == 10)
                    D1 = 0;
                return CalculaD2(chave, D1);
            }

            if (resto > 1)
                D2 = 11 - resto;

            return D2;
        }

        /// <summary>
        /// Composição da Chave ASBACE.
        /// - Campo livre 	 20 Posições
        /// (este campo é reservado para identificar a agência, cliente, número do título, etc. no banco cedente do título).
        /// - Código do banco cedente 	 03 Posições
        /// - Dígitos	 02 Posições.
        /// No BANESTES ficou assim a Chave ASBACE.
        /// NNNNNNNNCCCCCCCCCCCR021DD
        /// Onde:
        /// NNNNNNNN 	> Nosso Número (sem os dois dígitos).
        /// CCCCCCCCCCC 	> Conta Corrente.
        /// R 	> 2- Sem registro; 3- Caucionada; 4,5,6 e 7-Cobrança com registro.
        /// 021 	> Código do BANESTES .
        /// DD 	> Dígitos Verificadores.
        /// </summary>
        public string GeraChaveASBACE(string carteira, string conta, string nossoNumero, int tipoCobranca)
        {
            try
            {
                conta = conta.Replace(".", "").Replace("-", "");

                if (Utils.ToInt32(conta) < 1)
                    throw new Exception("Conta Corrente inválida");

                if (string.IsNullOrEmpty(carteira))
                    throw new Exception("Carteira não informada");

                string NNNNNNNN = Utils.FormatCode(nossoNumero.Substring(0, 8), 8);
                string CCCCCCCCCCC = Utils.FormatCode(conta, 11);
                string R = tipoCobranca.ToString();

                string NNNNNNNNCCCCCCCCCCCR021 = string.Concat(NNNNNNNN, CCCCCCCCCCC, R, "021");

                int D1 = CalculaD1(NNNNNNNNCCCCCCCCCCCR021);
                int D2 = CalculaD2(NNNNNNNNCCCCCCCCCCCR021, D1);

                return string.Concat(NNNNNNNNCCCCCCCCCCCR021, D1, D2);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Erro ao tentar gerar a chave ASBACE. {0}", e.Message));
            }
        }

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

                _header = "01REMESSA01";
                _header += "COBRANCA".PadRight(15, ' ');

                var contaCorrenteComDigito = cedente.ContaBancaria.Conta + cedente.ContaBancaria.DigitoConta;
                _header += Utils.FitStringLength(contaCorrenteComDigito, 11, 11, '0', 0, true, true, true); // Conta corrente

                _header += new string(' ', 9); // Filler
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "021";
                _header += "BANESTES";
                _header += new string(' ', 7); // Filler
                _header += DateTime.Now.ToString("ddMMyy");
                _header += new string(' ', 294);
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
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

                _detalhe = "1"; // Código do registro "Transação"
                _detalhe += boleto.Cedente.CPFCNPJ.Length <= 11 ? "01" : "02"; // Tipo de inscrição da empresa. 01 - CPF; 02 - CNPJ.

                string cpfCnpjCedente = boleto.Cedente.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpfCnpjCedente, 14, 14, '0', 0, true, true, true); // Número da inscrição da empresa.

                var contaComDigito = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                _detalhe += Utils.FitStringLength(contaComDigito, 11, 11, '0', 0, true, true, true); // Identificação da empresa no Banestes

                _detalhe += new string(' ', 9); // Filler
                _detalhe += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);// Identificação da operação na empresa

                _detalhe += Utils.FitStringLength(boleto.NossoNumero.Replace(".", ""), 10, 10, '0', 0, true, true, true); // Nosso número

                _detalhe += boleto.PercMulta > 0 ? "1" : "0";  // Código da multa

                var valorDaMulta = boleto.PercMulta > 0 ? boleto.PercMulta : boleto.ValorMulta;
                _detalhe += Utils.FitStringLength(valorDaMulta.ApenasNumeros(), 9, 9, '0', 0, true, true, true); ; // Valor da multa

                _detalhe += new string(' ', 6); // Identificação do carnê
                _detalhe += new string(' ', 2); // Número da parcela do carnê
                _detalhe += new string(' ', 2); // Quantidade de parcelas do carnê

                // Tipo de inscrição do sacador avalista
                if (boleto.Avalista != null && boleto.Avalista.CPFCNPJ.Length <= 11)
                {
                    _detalhe += "1";
                }
                else
                {
                    _detalhe += "2";
                }

                string cpfCnpjAvalista = "";

                if (boleto.Avalista != null)
                {
                    cpfCnpjAvalista = boleto.Avalista.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                }

                _detalhe += Utils.FitStringLength(cpfCnpjAvalista, 14, 14, '0', 0, true, true, true); // Inscrição do sacador avalista

                _detalhe += Utils.FitStringLength(boleto.Carteira, 1, 1, '0', 0, true, true, true); // Carteira

                _detalhe += "01"; // Identificação da ocorrência
                _detalhe += Utils.Right(boleto.NumeroDocumento, 10, '0', true);// Número do documento
                _detalhe += boleto.DataVencimento.ToString("ddMMyy"); // Data do vencimento (DDMMAA)
                _detalhe += new string('0', 3); // Filler
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 10, 10, '0', 0, true, true, true); // Valor nominal do título
                _detalhe += "021"; // Código do banco


                var pracaDeCobranca = "00000";

                if (boleto.ApenasRegistrar)
                {
                    pracaDeCobranca = "00501";
                }

                _detalhe += pracaDeCobranca;

                _detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true); // Espécie do título
                _detalhe += "N"; // Identificação do aceite
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy"); // Data da emissão do título (DDMMAA)

                string vInstrucao1 = "00";
                string vInstrucao2 = "00";

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Banestes)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Banestes.Protestar:
                            vInstrucao1 = "P6";
                            vInstrucao2 = instrucao.QuantidadeDias.ToString().PadLeft(2, '0');
                            break;
                        case EnumInstrucoes_Banestes.NaoProtestar:
                            vInstrucao1 = "P7";
                            vInstrucao2 = "00";
                            break;
                        default:
                            break;
                    }
                }

                _detalhe += vInstrucao1; // Primeira instrução de cobrança
                _detalhe += vInstrucao2; // Segunda instrução de cobrança
                _detalhe += boleto.CodJurosMora; // Código de mora
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true); // Valor da mora

                _detalhe += (boleto.DataDesconto > DateTime.MinValue)
                    ? boleto.DataDesconto.ToString("ddMMyy")
                    : "000000"; // Data limite para desconto

                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); // Valor do desconto a ser concedido
                _detalhe += Utils.FitStringLength("0", 13, 13, '0', 0, true, true, true); // IOC (em caso de título de seguro)
                _detalhe += Utils.FitStringLength("0", 13, 13, '0', 0, true, true, true); // Valor do abatimento a ser concedido

                _detalhe += boleto.Sacado.CPFCNPJ.Length <= 11 ? "01" : "02"; // Tipo de inscrição da empresa. 01 - CPF; 02 - CNPJ.

                string cpfCnpjSacado = boleto.Cedente.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpfCnpjSacado, 14, 14, '0', 0, true, true, true); // Número da inscrição da empresa.

                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumero.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 12, 12, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength("", 40, 40, '0', 0, true, true, true);
                _detalhe += new string('0', 2);
                _detalhe += "0";

                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

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
    }
}
