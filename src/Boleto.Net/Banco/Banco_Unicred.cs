using BoletoNet.EDI.Banco;
using BoletoNet.Excecoes;
using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.136.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <Author>
    /// Ivan Teles (ivan@idevweb.com.br)- Unicred
    /// </Author>
    internal class Banco_Unicred : AbstractBanco, IBanco
    {
        private HeaderRetorno header;

        /// <author>
        /// Classe responsavel em criar os campos do Banco Unicred.
        /// </author>
        internal Banco_Unicred()
        {
            this.Codigo = 136;
            this.Digito = "8";
            this.Nome = "Banco Unicred";
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //Formata o tamanho do numero da agencia
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Formata o tamanho do numero da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 5)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

            //Atribui o nome do banco ao local de pagamento

            if (boleto.LocalPagamento == "Ate o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;
            else 
                boleto.LocalPagamento = "Pagável em qualquer banco mesmo após o vencimento";

            //Verifica se data do processamento eh valida
            if (boleto.DataProcessamento == DateTime.MinValue) 
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento Ã© valida
            if (boleto.DataDocumento == DateTime.MinValue) 
                boleto.DataDocumento = DateTime.Now;

            string infoFormatoCodigoCedente = "formato AAAAPPCCCCC, onde: AAAA = Numero da agencia, PP = Posto do beneficiario, CCCCC = Codigo do beneficiario";


            var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 11);

            if (string.IsNullOrEmpty(codigoCedente))
                throw new BoletoNetException("Codigo do cedente deve ser informado, " + infoFormatoCodigoCedente);

            var conta = boleto.Cedente.ContaBancaria.Conta;
            if (boleto.Cedente.ContaBancaria != null &&
                (!codigoCedente.StartsWith(boleto.Cedente.ContaBancaria.Agencia) ||
                 !(codigoCedente.EndsWith(conta) || codigoCedente.EndsWith(conta.Substring(0, conta.Length - 1)))))

                //throw new BoletoNetException("Codigo do cedente deve estar no " + infoFormatoCodigoCedente);
                boleto.Cedente.Codigo = string.Format("{0}{1}{2}", boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, boleto.Cedente.Codigo);


            //Verifica se o nosso numero eh valido
            var Length_NN = boleto.NossoNumero.Length;
            if (Length_NN > 11) throw new NotImplementedException("Nosso numero invalido");

            FormataCodigoBarra(boleto);
            //if (boleto.CodigoBarra.Codigo.Length != 44)
            //    throw new BoletoNetException("Codigo de barras eh invalido");


            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }


        public override void FormataNossoNumero(Boleto boleto)
        {
            string nossoNumero = boleto.NossoNumero;

            if (nossoNumero == null || nossoNumero.Length != 10)
            {

                throw new Exception("Erro ao tentar formatar nosso numero, verifique o tamanho do campo: " + nossoNumero.Length);

            }

            try
            {
                boleto.NossoNumero = string.Format("{0}{1}", nossoNumero, Mod11UniCred(nossoNumero, false));
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao formatar nosso numero", ex);
            }
        }

        

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new BoletoNetException("Nao implantado");
        }
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1  CCCCC.CCNNNd2  NNNNN.041XXd3  V FFFF9999999999

            string campo1 = "1369" + boleto.CodigoBarra.Codigo.Substring(19, 5);
            int d1 = Mod10Unicred(campo1);
            campo1 = FormataCampoLD(campo1) + d1.ToString();

            string campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            int d2 = Mod10Unicred(campo2);
            campo2 = FormataCampoLD(campo2) + d2.ToString();

            string NossoNumLinhaDigitavel = string.Format("{0}{1}", boleto.NossoNumero, Mod11UniCred(boleto.NossoNumero, false));
            string campo3 = NossoNumLinhaDigitavel.Substring(NossoNumLinhaDigitavel.Length-10, 10);
            //A linha digitável nao pode usar a regra de cálculo do DV do barcode pois lá o nosso numero usa uma regra diferente para o DV
            //string campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            int d3 = Mod10Unicred(campo3);
            campo3 = FormataCampoLD(campo3) + d3.ToString();

            string cmp_livre = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4) +
                                    Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 10) +
                                    Utils.FormatCode(NossoNumLinhaDigitavel, 11);
            string campo4 = DigUnicred(cmp_livre).ToString();
            //A linha digitável nao pode usar a regra de cálculo do DV do barcode pois lá o nosso numero usa uma regra diferente para o DV
            //string campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            
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

            var nossoNumero = string.Format("{0}{1}", boleto.NossoNumero, Mod11UniCred(boleto.NossoNumero, false));
            string cmp_livre = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4) +
                                                Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 10) +
                                                Utils.FormatCode(nossoNumero, 11);

            string dv_cmpLivre = DigUnicred(cmp_livre).ToString();

            var codigoTemp = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, string.Empty);

            boleto.CodigoBarra.CampoLivre = cmp_livre;
            boleto.CodigoBarra.FatorVencimento = FatorVencimento(boleto);
            boleto.CodigoBarra.Moeda = 9;
            boleto.CodigoBarra.ValorDocumento = valorBoleto;

            int _dacBoleto = DigUnicred(codigoTemp);

            if (_dacBoleto == 0 || _dacBoleto > 9)
                _dacBoleto = 1;

            //Estava gerando com 46 digitos ao invés de 44, então tirei o dv_cmpLivre para corrigir.
            //boleto.CodigoBarra.Codigo = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, dv_cmpLivre, _dacBoleto);
            boleto.CodigoBarra.Codigo = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, string.Empty, _dacBoleto);
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

        #region Metodos de Geracao do Arquivo de Remessa
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _headerLote;
                _headerLote = "13600000         ";
                if (cedente.CPFCNPJ.Length <= 11)
                    _headerLote += "1";
                else
                    _headerLote += "2";
                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                _headerLote += new string(' ', 20);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _headerLote += " ";
                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _headerLote += Utils.FitStringLength("UNICRED", 30, 30, ' ', 0, true, true, false);
                _headerLote += new string(' ', 10);
                _headerLote += "1";
                _headerLote += DateTime.Now.ToString("ddMMyyyy");
                _headerLote += "000000";// DateTime.Now.ToString("HHmmss");
                _headerLote += numeroArquivoRemessa.ToString("000000");
                _headerLote += "085";
                _headerLote += "00000";
                _headerLote += Utils.FitStringLength(cedente.CodigoTransmissao, 3, 3, '0', 0, true, true, true);
                _headerLote += new string(' ', 66);
                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            string _headerLote;
            _headerLote = "13600011R01  044 ";
            if (cedente.CPFCNPJ.Length <= 11)
                _headerLote += "1";
            else
                _headerLote += "2";
            _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true);
            _headerLote += new string(' ', 20);
            _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
            _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
            _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
            _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
            _headerLote += " ";
            _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
            _headerLote += new string(' ', 40);
            _headerLote += new string(' ', 40);
            _headerLote += numeroArquivoRemessa.ToString("00000000");
            _headerLote += DateTime.Now.ToString("ddMMyyyy");
            _headerLote += new string(' ', 8);
            _headerLote += new string(' ', 33);
            return _headerLote;
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string _segmentoP;
                string _nossoNumero;

                _segmentoP = "13600013";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P ";
                _segmentoP += ObterCodigoDaOcorrencia(boleto);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _segmentoP += "0";
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero, 11, 11, ' ', 0, true, true, false);
                _segmentoP += new string(' ', 8);
                _segmentoP += Utils.FitStringLength(boleto.Carteira, 2, 2, ' ', 0, true, true, false);
                _segmentoP += new string(' ', 4);
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += new string(' ', 8);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);

                if (boleto.JurosMora > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += "00000000";
                    _segmentoP += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += "3";
                    _segmentoP += "00000000";
                    _segmentoP += "000000000000000";
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";
                    _segmentoP +=
                        Utils.FitStringLength(
                            boleto.DataDesconto == DateTime.MinValue
                                ? boleto.DataVencimento.ToString("ddMMyyyy")
                                : boleto.DataDesconto.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                    _segmentoP += "000000000000000000000000";
                _segmentoP += "000000000000000";
                _segmentoP += "000000000000000";
                _segmentoP += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); //alterado por diegodariolli - 15/03/2018
                _segmentoP += " ";
                _segmentoP += "  ";
                _segmentoP += "0";
                _segmentoP += "   ";
                _segmentoP += "  ";
                _segmentoP += "0000000000";
                _segmentoP += " ";
                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP.ToUpper());
                return _segmentoP;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoQ;

                _segmentoQ = "13600013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q ";
                _segmentoQ += ObterCodigoDaOcorrencia(boleto);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                var enderecoSacadoComNumero = boleto.Sacado.Endereco.End;
                if (!string.IsNullOrEmpty(boleto.Sacado.Endereco.Numero))
                {
                    enderecoSacadoComNumero += ", " + boleto.Sacado.Endereco.Numero;
                }

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(enderecoSacadoComNumero.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();

                if (boleto.Avalista != null)
                {
                    if (boleto.Avalista.CPFCNPJ.Length <= 11)
                        _segmentoQ += "1";
                    else
                        _segmentoQ += "2";
                    _segmentoQ += Utils.FitStringLength(boleto.Avalista.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                    _segmentoQ += Utils.FitStringLength(boleto.Avalista.Nome, 40, 40, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoQ += "1";
                    _segmentoQ += new string('0', 15);
                    _segmentoQ += new string(' ', 40);
                }
                _segmentoQ += "000";
                _segmentoQ += new string(' ', 28);
                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoR;

                _segmentoR = "13600013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
                _segmentoR += ObterCodigoDaOcorrencia(boleto);
                _segmentoR += new string('0', 44);
                _segmentoR += " ";
                _segmentoR += new string('0', 21);
                _segmentoR += new string(' ', 10);

                for (int i = 0; i < 2; i++)
                {
                    if (boleto.Instrucoes.Count > i)
                        _segmentoR += Utils.FitStringLength(boleto.Instrucoes[i].Descricao, 40, 40, ' ', 0, true, true, false);
                    else
                        _segmentoR += new string(' ', 40);
                }

                _segmentoR += new string(' ', 20);
                _segmentoR += new string('0', 32);
                _segmentoR += new string(' ', 9);
                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            string _trailer = "";
            _trailer += "13600015";
            _trailer += new string(' ', 9);
            _trailer += numeroRegistro.ToString("000000");
            _trailer += "000000";
            _trailer += "00000000000000000";
            _trailer += "000000";
            _trailer += "00000000000000000";
            _trailer += "000000";
            _trailer += "00000000000000000";
            _trailer += "000000";
            _trailer += "00000000000000000";
            _trailer += new string(' ', 8);
            _trailer += new string(' ', 117);
            return _trailer;

        }
        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            string _trailer = "";
            _trailer += "13699999";
            _trailer += new string(' ', 9);
            _trailer += numeroRegistro.ToString("000000");
            _trailer += (numeroRegistro + 4).ToString("000000");
            _trailer += "000000";
            _trailer += new string(' ', 205);
            return _trailer;
        }
        #endregion

        public int Mod10Unicred(string seq)
        {
            /* Variaveis
             * -------------
             * d - Digito
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

        protected static int Mod11UniCred(string seq, bool ehBarcode)
        {
            /* Variaveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */
            int[] mult = new[] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int d, s = 0, i = 0;

            foreach (char c in seq)
            {
                var mul = mult[i];
                s = s + (int.Parse(c.ToString()) * mul);
                i++;
            }

            d = 11 - (s % 11);
            if (ehBarcode)
            {
                if (d == 0 || d >= 10)
                    d = 1;
            }
            else
            {
                if (d == 0 || d >= 10)
                    d = 0;
            }

            return d;
        }

        public int DigUnicred(string seq)
        {
            /* Variaveis
              * -------------
              * d - Dígito
              * s - Soma
              * p - Peso
              * b - Base
              * r - Resto
              */
            int[] mult = seq.Length == 25 ? new[] { 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 } :
                new[] { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7 };

            int d, s = 0, i = 0;

            foreach (char c in seq)
            {
                if (seq.Length > mult.Length)
                {
                    throw new BoletoNetException("Tamanho da sequencia maior que o limite");
                }
                var mul = mult[i];
                s += (int.Parse(c.ToString()) * mul);
                i++;
            }

            d = 11 - (s % 11);
            if (d == 0 || d == 1|| d == 10)
                d = 1;
            return d;
        }

        /// <summary>
        /// Efetua as Validacoes dentro da classe Boleto, para garantir a geracao da remessa
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
