using System;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Text;
using System.Collections.Generic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.041.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <Author>
    /// Felipe Silveira - Transis Informática
    /// </Author>
    internal class Banco_Banrisul : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _primDigito;
        private int _segDigito;

        /// <author>
        /// Classe responsavel em criar os campos do Banco Banrisul.
        /// </author>
        internal Banco_Banrisul()
        {
            this.Codigo = 041;
            this.Digito = "8";
            this.Nome = "Banco Banrisul";
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            boleto.ContaBancaria = boleto.Cedente.ContaBancaria;

            //Formata o tamanho do número da agência
            if (boleto.Cedente.ContaBancaria.Conta.Length < 4)
                throw new Exception("Número da agência inválido");

            //Formata o tamanho do número da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Formata o tamanho do número de nosso número
            if (boleto.NossoNumero.Length < 8)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);
            else if (boleto.NossoNumero.Length > 8)
                throw new NotSupportedException("Para o banco Banrisul, o nosso número deve ter 08 posições e 02 dígitos verificadores (calculados automaticamente).");

            boleto.NossoNumero = CalcularNCNossoNumero(boleto.NossoNumero);

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        private string CalcularNCNossoNumero(String nossoNumero)
        {
            int dv1 = Mod10(nossoNumero);
            int dv1e2 = Mod11(nossoNumero, dv1); // O módulo 11 sempre devolve os dois Dígitos, pois, as vezes o dígito calculado no mod10 será incrementado em 1
            return nossoNumero + dv1e2.ToString("00");
        }

        private string CalcularNCCodBarras(String seq)
        {
            int dv1 = Mod10(seq);
            int dv2 = Mod11(seq, dv1);// O módulo 11 sempre devolve os dois Dígitos, pois, as vezes, o dígito calculado no mod10 será incrementado em 1
            return dv2.ToString("00");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.NossoNumero.Length == 10)
            {
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 8) + "-" + boleto.NossoNumero.Substring(8, 2);
            }
            else
            {
                throw new Exception("Erro ao tentar formatar nosso número, verifique o tamanho do campo");
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função do fomata número do documento não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1 CCCCC.CCNNNd2 NNNNN.041XXd3 V FFFF9999999999
            //OU 
            //041M2.1AAAd1 ACCCCC.CCNNd2 NNNNN.N40XXd3 V FFFF9999999999
            //(Isso depende da constante que usar) no caso de cima "041" no de baixo "40" antes do "XX"

            string Campo1 = string.Empty;
            string Campo2 = string.Empty;
            string Campo3 = string.Empty;
            string Campo4 = string.Empty;
            string Campo5 = string.Empty;

            string Cedente = boleto.Cedente.Codigo.Substring(4); //Os quatro primeiros digitos do código do cedente é sempre a agência
            string NossoNumero = boleto.NossoNumero.Substring(0, 8);

            //Campo 1
            string M = boleto.Moeda.ToString();
            string AAA = boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3);
            string Metade1 = "041" + M + "2";
            string Metade2 = "1" + AAA;
            string d1 = Mod10(Metade1 + Metade2).ToString();
            Campo1 = Metade1 + "." + Metade2 + d1;

            //Campo 2
            Metade1 = string.Empty;
            Metade2 = string.Empty;
            Metade1 = Cedente.Substring(0, 5);
            //Metade2 = Cedente.Substring(5, 2) + NossoNumero.Substring(0, 2);
            Metade2 = Cedente.Substring(5, 2) + NossoNumero.Substring(0, 3);
            string d2 = Mod10(Metade1 + Metade2).ToString();
            Campo2 = Metade1 + "." + Metade2 + d2;

            //Campo 3
            Metade1 = string.Empty;
            Metade2 = string.Empty;
            string XX = _primDigito.ToString() + _segDigito.ToString();
            //Metade1 = NossoNumero.Substring(2, 5);
            Metade1 = NossoNumero.Substring(3, 5);
            //Metade2 = NossoNumero.Substring(7, 1) + "041" + XX;
            Metade2 = "041" + XX;
            string d3 = Mod10(Metade1 + Metade2).ToString();
            Campo3 = Metade1 + "." + Metade2 + d3;

            //Campo 4
            Campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            //Campo 5
            string fatorVenc = FatorVencimento(boleto).ToString("0000");
            string valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valor = Utils.FormatCode(valor, 10);
            Campo5 = fatorVenc + valor;

            boleto.CodigoBarra.LinhaDigitavel = Campo1 + "  " + Campo2 + "  " + Campo3 + "  " + Campo4 + "  " + Campo5;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campoLivre = string.Empty;
            campo1 = "041" + boleto.Moeda.ToString();
            int dacCodBarras;
            string fatorVenc = FatorVencimento(boleto).ToString("0000");
            string valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valor = Utils.FormatCode(valor, 10);
            campo2 = fatorVenc + valor;

            string nossoNumero = boleto.NossoNumero.Replace(".", "").Replace("-", "");
            nossoNumero = nossoNumero.Substring(0, 8);
            //campoLivre = "21" + boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3) + boleto.Cedente.ContaBancaria.Conta + nossoNumero + "041";
            string codCedente = boleto.Cedente.Codigo.Substring(4);// Os quatro primeiros digitos do código do cedente é sempre a agência
            campoLivre = "21" + boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3) + codCedente + nossoNumero + "041";
            string ncCodBarra = CalcularNCCodBarras(campoLivre);
            Int32.TryParse(ncCodBarra.Substring(0, 1), out _primDigito);
            Int32.TryParse(ncCodBarra.Substring(1, 1), out _segDigito);
            campoLivre = campoLivre + ncCodBarra;

            dacCodBarras = Mod11Peso2a9(campo1 + campo2 + campoLivre);

            boleto.CodigoBarra.Codigo = campo1 + dacCodBarras.ToString() + campo2 + campoLivre;
        }

        private int Mod10(string seq)
        {
            /* (N1*1-9) + (N2*2-9) + (N3*1-9) + (N4*2-9) + (N5*1-9) + (N6*2-9) + (N7*1-9) + (N8*2-9)
             * Observação:
             * a) a subtração do 9 somente será feita se o produto obtido da multiplicação individual for maior do que 9. 
             * b) quando o somatório for menor que 10, o resto da divisão por 10 será o próprio somatório. 
             * c) quando o resto for 0, o primeiro DV é igual a 0.
             */
            int soma = 0, resto, dv1, peso = 2, n, result;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                result = n * peso > 9 ? (n * peso) - 9 : n * peso;
                soma += result;
                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            if (soma < 10)
                resto = soma;
            else
                resto = soma % 10;
            dv1 = resto == 0 ? 0 : 10 - resto;
            return dv1;
        }

        private int Mod11(string seq, int dv1)
        {
            /* Obter somatório (peso de 2 a 7), sempre da direita para a esquerda (N1*4)+(N2*3)+(N3*2)+(N4*7)+(N5*6)+(N6*5)+(N7*4)+(N8*3)+(N9*2)
             * Caso o somatório obtido seja menor que "11", considerar como resto da divisão o próprio somatório.
             * Caso o ''resto'' obtido no cálculo do módulo ''11'' seja igual a ''1'', considera-se o DV inválido. 
             * Soma-se, então, "1" ao DV obtido do módulo "10" e refaz-se o cálculo do módulo “11” . 
             * Se o dígito obtido pelo módulo “10” era igual a "9", considera-se então (9+1=10) DV inválido. 
             * Neste caso, o DV do módulo "10" automaticamente será igual a "0" e procede-se assim novo cálculo pelo módulo "11". 
             * Caso o ''resto'' obtido no cálculo do módulo "11" seja ''0'', o segundo ''NC'' será igual ao próprio ''resto''
             */
            int peso = 2, mult, sum = 0, rest, dv2, b = 7, n;
            seq += dv1.ToString();
            bool dvInvalido;
            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                mult = n * peso;
                sum += mult;
                if (peso < b)
                    peso++;
                else
                    peso = 2;
            }
            seq = seq.Substring(0, seq.Length - 1);
            rest = sum < 11 ? sum : sum % 11;
            if (rest == 1)
                dvInvalido = true;
            else
                dvInvalido = false;

            if (dvInvalido)
            {
                int novoDv1 = dv1 == 9 ? 0 : dv1 + 1;
                dv2 = Mod11(seq, novoDv1);
            }
            else
            {
                dv2 = rest == 0 ? 0 : 11 - rest;
            }
            if (!dvInvalido)
            {
                string digitos = dv1.ToString() + dv2;
                return Convert.ToInt32(digitos);
            }
            else
            {
                return dv2;
            }
        }

        private int Mod11BaseIndef(string seq, int b)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2;


            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            d = 11 - (s % 11);


            if ((d > 9) || (d == 0) || (d == 1))
                d = 1;

            return d;
        }

        private int Mod11Peso2a9(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             * n - Numero (string convertida)
             */

            int d, r, s = 0, p = 2, b = 9, n;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));

                s = s + (n * p);

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            r = s % 11;

            if (r == 0 || r == 1 || r > 9)
                d = 1;
            else
                d = 11 - r;

            return d;
        }

        private int CalculaSoma(string Numero)
        {
            int mult;
            int x;
            int y;
            int resul;
            int resto;
            int soma;
            soma = 0;
            mult = 2;
            int I = Strings.Len(Numero);
            //para começar o cálculo pelo nº final (sempre começa multiplicando por 2)
            for (x = 1; x <= Strings.Len(Numero); x++)
            {
                if (Codigo == 41)
                {
                    //Banrisul só vai até 7
                    if (mult == 8)
                        mult = 2;
                }
                else
                {
                    if (mult == 10)
                        mult = 2;
                }
                y = Convert.ToInt32(Strings.Mid(Numero, I, 1));
                resul = y * mult;
                soma = soma + resul;
                mult = mult + 1;
                I = I - 1;
            }
            if (Codigo == 41 | Codigo == 33 | Codigo == 353)
            {
                return soma;
                // calcula no retorno pois tem umas exceções
            }
            else
            {
                resto = soma % 11;
                if (resto == 0)
                    resto = 1;
                return resto;
            }
        }

        #region Métodos de validação e geração do arquivo remessa - sidneiklein
        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
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
        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
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
                        _trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
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

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region CNAB 240
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region CNAB 400 - sidneiklein
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
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
                    #region Validações da Remessa que deverão estar preenchidas quando BANRISUL
                    //Comentado porque ainda está fixado em 01
                    //if (String.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código de Ocorrência!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (boleto.Remessa.TipoDocumento.Equals("06") && !String.IsNullOrEmpty(boleto.NossoNumero))
                    {
                        //Para o "Remessa.TipoDocumento = "06", não poderá ter NossoNumero Gerado!
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Não pode existir NossoNumero para o Tipo Documento '06 - cobrança escritural'!", Environment.NewLine);
                        vRetorno = false;
                    }

                    //Para o Tipo
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 010, 0, "01REMESSA", ' ')); //001-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 016, 0, "", ' ')); //010-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 013, 0, cedente.Codigo, ' ')); //027-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 007, 0, "", ' ')); //040-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' ')); //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 011, 0, "041BANRISUL", ' ')); //077-087
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0088, 007, 0, "", ' ')); //088-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' ')); //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 009, 0, "", ' ')); //101-109
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0110, 004, 0, "", ' ')); //110-113
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0114, 001, 0, "", ' ')); //114-114
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0115, 001, 0, "", ' ')); //115-115
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0116, 001, 0, "", ' ')); //116/116
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, "", ' ')); //117-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 268, 0, "", ' ')); //126-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' ')); //395-400
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
            try
            {
                //Variáveis Locais a serem Implementadas em nível de Config do Boleto...
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ bANRISUL
                //
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "1", ' '));                                       //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 016, 0, string.Empty, ' '));                              //002-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 013, 0, boleto.Cedente.Codigo, ' '));                     //018-030 (sidnei.klein 22/11/2013: No Banrisul, o Código do Cedente não é a concatenação de Número da Conta com o Dígito Verificador.)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 007, 0, string.Empty, ' '));                              //031-037
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, boleto.NumeroDocumento, ' '));                    //038-062
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 010, 0, boleto.NossoNumero, '0'));                        //063-072
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 032, 0, string.Empty, ' '));                              //073-104
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0105, 003, 0, string.Empty, ' '));                              //105-107
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 001, 0, "1", ' '));                                       //108-108   //COBRANÇA SIMPLES
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110   //REMESSA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139   //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "041", ' '));                                     //140-142
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0143, 005, 0, string.Empty, ' '));                              //143-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0148, 002, 0, boleto.Remessa.TipoDocumento, ' '));              //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                //
                #region Instruções
                string vQtdeDiasCodigo_9_ou_15 = "0";
                //
                string vInstrucao1 = string.Empty;
                string vInstrucao2 = string.Empty;
                switch (boleto.Instrucoes.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                        vInstrucao2 = string.Empty;
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[0].QuantidadeDias.ToString();
                        //
                        break;
                    case 2:
                        vInstrucao1 += boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[0].QuantidadeDias.ToString();
                        //
                        vInstrucao2 += boleto.Instrucoes[1].Codigo.ToString().PadLeft(2, '0');
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[1].Codigo == 9 || boleto.Instrucoes[1].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[1].QuantidadeDias.ToString();
                        //
                        break;
                }
                #endregion
                //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0157, 002, 0, vInstrucao1, ' '));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0159, 002, 0, vInstrucao2, ' '));                               //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0161, 001, 0, "0", ' '));                                       //161-161

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0162, 012, 2, boleto.JurosMora, '0'));                          //162-173
                #region DataDesconto
                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "99";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj é sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, vCpfCnpjSac, '0'));                               //219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 035, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-269
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0270, 005, 0, string.Empty, ' '));                              //270-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 007, 0, string.Empty, ' '));                              //315-321
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0322, 003, 0, 0, '0'));                                         //322-324
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0325, 002, 0, 0, '0'));                                         //325-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0352, 004, 1, 0, '0'));                                         //352-355
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0356, 001, 0, string.Empty, ' '));                              //356-356
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0357, 013, 2, 0, '0'));                                         //357-369
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0370, 002, 0, vQtdeDiasCodigo_9_ou_15, '0'));                   //370-371
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0372, 023, 0, string.Empty, ' '));                              //372-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400
                //

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
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 026, 0, string.Empty, ' '));   //002-027
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0028, 013, 2, vltitulostotal, '0')); //027-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0041, 354, 0, string.Empty, ' '));   //040-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
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
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                TRegistroEDI_Banrisul_Retorno reg = new TRegistroEDI_Banrisul_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                //detalhe. = Constante1;
                detalhe.CodigoInscricao = Utils.ToInt32(reg.TipoInscricao);
                detalhe.NumeroInscricao = reg.CpfCnpj;
                //detalhe.Agencia = Utils.ToInt32(reg.CodigoCedente.Substring(0, 3));
                //detalhe.Conta = Utils.ToInt32(reg.CodigoCedente.Substring(4, 7));
                //detalhe.DACConta = Utils.ToInt32(reg.CodigoCedente.Substring(36, 1));

                //detalhe. = reg.EspecieCobrancaRegistrada;
                //detalhe. = reg.Branco1;
                detalhe.NumeroControle = reg.IdentificacaoTituloCedente;
                detalhe.IdentificacaoTitulo = reg.IdentificacaoTituloBanco_NossoNumero;
                //detalhe. = reg.IdentificacaoTituloBanco_NossoNumeroOpcional;
                //detalhe. = reg.NumeroContratoBLU;
                //detalhe. = reg.Brancos2;
                //detalhe. = reg.TipoCarteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.CodigoOcorrencia);
                //
                int dataOcorrencia = Utils.ToInt32(reg.DataOcorrenciaBanco);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //
                detalhe.NumeroDocumento = reg.SeuNumero;
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV
                //
                int dataVencimento = Utils.ToInt32(reg.DataVencimentoTitulo);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                //
                decimal valorTitulo = Convert.ToInt64(reg.ValorTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoCobrador);
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.CodigoAgenciaCobradora);
                //
                //detalhe.Especie = reg.TipoDocumento; //Verificar Espécie de Documentos...
                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa)
                decimal valorDespesa = Convert.ToUInt64(reg.ValorDespesasCobranca);
                detalhe.ValorDespesa = valorDespesa / 100;
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                decimal valorOutrasDespesas = Convert.ToUInt64(reg.OutrasDespesas);
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                //detalhe. = reg.Zeros1;
                //detalhe. = reg.ValorAvista;
                //detalhe. = reg.SituacaoIOF;
                //detalhe. = reg.Zeros2;

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.ValorAbatimento_DeflacaoConcedido);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.ValorDescontoConcedido);
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorPago);
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.ValorJuros);
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(reg.ValorOutrosRecebimentos);
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //detalhe. = reg.Brancos3;
                int dataCredito = Utils.ToInt32(reg.DataCreditoConta);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //detalhe. = reg.Brancos4;
                detalhe.OrigemPagamento = reg.PagamentoDinheiro_Cheque;
                //detalhe. = reg.Brancos5;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //detalhe. = reg.Brancos6;
                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequenciaRegistro);
                //
                //
                #region NAO RETORNADOS PELO BANRISUL
                detalhe.IOF = 0;
                //Motivos das Rejeições para os Códigos de Ocorrência
                detalhe.MotivosRejeicao = string.Empty;
                //Número do Cartório
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;
                //Nome do Sacado
                detalhe.NomeSacado = "";
                #endregion

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }


        #endregion
    }
}