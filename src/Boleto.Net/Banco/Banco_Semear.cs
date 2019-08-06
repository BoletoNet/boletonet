using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.743.jpg", "image/jpeg")]

namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// </author>    
    internal class Banco_Semear : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Semear.
        /// </summary>
        internal Banco_Semear()
        {
            this.Codigo = 743;
            this.Digito = "9";
            this.Nome = "Semear";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Semear(boleto.NossoNumero, 9);
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
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            var campoLivre = FormataCampoLivre(boleto);

            #region Campo 1

            string Grupo1 = string.Empty;

            string banco = Codigo.ToString();
            int moeda = boleto.Moeda;
            string primeiras5PosicoesCampoLivre = campoLivre.Substring(0, 5);
            int digitoVerificadorCampo1 = Mod10(banco + moeda + primeiras5PosicoesCampoLivre);

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", banco, moeda, campoLivre.Substring(0, 1), campoLivre.Substring(1, 4), digitoVerificadorCampo1);

            #endregion Campo 1

            #region Campo 2

            string Grupo2 = campoLivre.Substring(5, 10);

            int D2 = Mod10(Grupo2);

            Grupo2 = string.Format("{0}.{1}{2} ", Grupo2.Substring(0, 5), Grupo2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = campoLivre.Substring(15, 9) + boleto.Cedente.ContaBancaria.DigitoConta;
            int D3 = Mod10(Grupo3);

            Grupo3 = string.Format("{0}.{1}{2} ", Grupo3.Substring(0, 5), Grupo3.Substring(5, 5), D3);

            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            Grupo4 = string.Format("{0} ", boleto.CodigoBarra.DigitoVerificador);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            string fatorVencimento = FatorVencimento(boleto).ToString();

            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            string valorFormatado = valor.ToString("N2").Replace(",", "").Replace(".", "");
            valorFormatado = Utils.FormatCode(valorFormatado, 10);

            Grupo5 = string.Format("{0}{1}", fatorVencimento, valorFormatado);

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
        ///    20 a 43 – 24 - Campo Livre
        ///    44 a 44 - 1 - Digito 0
        ///   *******
        /// </summary>
        /// 
        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            var valorBoleto = valor.ToString("N2").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            if (boleto.Carteira == "02") // Com registro
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}0", Codigo.ToString(), boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
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
            var agencia = boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0');
            var carteira = boleto.Carteira;
            var nossoNumero = boleto.NossoNumero.Split('-')[0];
            var conta = boleto.Cedente.ContaBancaria.Conta.PadLeft(7, '0');

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}", agencia, carteira, nossoNumero, conta);

            return FormataCampoLivre;
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }


        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.DigitoNossoNumero = Mod11Semear(boleto.NossoNumero, 9);
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "16" && boleto.Carteira != "19" && boleto.Carteira != "25" && boleto.Carteira != "26")
                throw new NotImplementedException("Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 16, 19, 25, 26.");

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

        private string Mod11Semear(string seq, int b)
        {
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

        public string GerarRegistroDetalhe9(Boleto boleto, int numeroRegistro)
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
            _detalhe += Mod11Semear(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // 394 a 394 Digito de Auto Conferencia do Nosso Número (01)
            //Desconto Bonificação por dia (10, N)
            _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // 395 a 400
            //Retorno
            return Utils.SubstituiCaracteresEspeciais(_detalhe);
        }

        public string GerarRegistroDetalhe2(Boleto boleto, int numeroRegistro)
        {
            string _detalhe = "";
            _detalhe += "9";                                        // 001 a 001 Tipo Registro
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
            _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, ' ', 0, true, true, true); // 383 a 393 Nosso Número (11)
            // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012
            _detalhe += Mod11Semear(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // 394 a 394 Digito de Auto Conferencia do Nosso Número (01)
            //Desconto Bonificação por dia (10, N)
            _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // 395 a 400
            //Retorno
            return Utils.SubstituiCaracteresEspeciais(_detalhe);
        }
    }
}