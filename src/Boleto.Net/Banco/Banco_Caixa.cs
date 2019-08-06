using BoletoNet.EDI.Banco;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.104.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Banco_Caixa Economica Federal
    /// </summary>
    internal class Banco_Caixa : AbstractBanco, IBanco
    {
        /* 
         * boleto.Remessa.TipoDocumento 1 - SICGB - Com registro - RG
         * boleto.Remessa.TipoDocumento 2 - SICGB - Sem registro - SR
         */

        private const string CarteiraRG = "1";
        private const string CarteiraSR = "2";

        private const int EmissaoCedente = 4;

        private string _dacBoleto = string.Empty;

        private bool _protestar;
        private bool _baixaDevolver;
        private bool _desconto;
        private int _diasProtesto;
        private int _diasDevolucao;

        internal Banco_Caixa()
        {
            this.Codigo = 104;
            this.Digito = "0";
            this.Nome = "Caixa Econômica Federal";
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            // Posição 01-03
            string banco = Codigo.ToString();

            //Posição 04
            string moeda = "9";

            //Posição 05 - No final ...   

            // Posição 06 - 09
            long fatorVencimento = FatorVencimento(boleto);

            // Posição 10 - 19     
            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            string valorDocumento = valor.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = Utils.FormatCode(valorDocumento, 10);


            // Inicio Campo livre
            string campoLivre = string.Empty;


            //ESSA IMPLEMENTAÇÃO FOI FEITA PARA CARTEIAS SIGCB CarteiraSR COM NOSSO NUMERO DE 14 e 17 POSIÇÕES
            //Implementei também a validação da carteira preenchida com "SR" e "RG" para atender a issue #638
            if (boleto.Carteira.Equals(CarteiraSR) || boleto.Carteira.Equals(CarteiraRG) || boleto.Carteira.Equals("SR") || boleto.Carteira.Equals("RG"))
            {
                //14 POSIÇOES
                if (boleto.NossoNumero.Length == 14)
                {
                    //Posição 20 - 24
                    string contaCedente = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

                    // Posição 25 - 28
                    string agenciaCedente = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

                    //Posição 29
                    string codigoCarteira = "8";

                    //Posição 30
                    string constante = "7";

                    //Posição 31 - 44
                    string nossoNumero = boleto.NossoNumero;

                    campoLivre = string.Format("{0}{1}{2}{3}{4}", contaCedente, agenciaCedente, codigoCarteira,
                        constante, nossoNumero);
                }
                //17 POSIÇÕES
                if (boleto.NossoNumero.Length == 17)
                {
                    //104 - Caixa Econômica Federal S.A. 
                    //Carteira SR - 24 (cobrança sem registro) || Carteira RG - 14 (cobrança com registro)
                    //Cobrança sem registro, nosso número com 17 dígitos. 

                    //Posição 20 - 25
                    string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 6);

                    // Posição 26
                    string dvCodigoCedente = Mod11Base9(codigoCedente).ToString();

                    //Posição 27 - 29
                    //De acordo com documentação, posição 3 a 5 do nosso numero
                    string primeiraParteNossoNumero = boleto.NossoNumero.Substring(2, 3);

                    //Posição 30
                    string primeiraConstante;
                    switch (boleto.Carteira)
                    {
                        case CarteiraSR:
                            primeiraConstante = "2";
                            break;
                        case CarteiraRG:
                            primeiraConstante = "1";
                            break;
                        default:
                            if (boleto.Carteira.Equals("SR"))
                                primeiraConstante = "2";
                            else if (boleto.Carteira.Equals("RG"))
                                primeiraConstante = "1";
                            else
                                primeiraConstante = boleto.Carteira;
                            break;
                    }

                    // Posição 31 - 33
                    //DE acordo com documentação, posição 6 a 8 do nosso numero
                    string segundaParteNossoNumero = boleto.NossoNumero.Substring(5, 3);

                    // Posição 34
                    string segundaConstante = "4";// 4 => emissão do boleto pelo cedente

                    //Posição 35 - 43
                    //De acordo com documentaçao, posição 9 a 17 do nosso numero
                    string terceiraParteNossoNumero = boleto.NossoNumero.Substring(8, 9);

                    //Posição 44
                    string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                        codigoCedente,
                        dvCodigoCedente,
                        primeiraParteNossoNumero,
                        primeiraConstante,
                        segundaParteNossoNumero,
                        segundaConstante,
                        terceiraParteNossoNumero);
                    string dvCampoLivre = Mod11Base9(ccc).ToString();
                    campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);
                }
            }
            else
            {
                //Posição 20 - 25
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 6);

                // Posição 26
                string dvCodigoCedente = Mod11Base9(codigoCedente).ToString();

                //Posição 27 - 29
                string primeiraParteNossoNumero = boleto.NossoNumero.Substring(0, 3);

                //104 - Caixa Econômica Federal S.A. 
                //Carteira 01. 
                //Cobrança rápida. 
                //Cobrança sem registro. 
                //Cobrança sem registro, nosso número com 16 dígitos. 
                //Cobrança simples 
                
                //Posição 30
                string primeiraConstante = (boleto.Carteira == CarteiraSR || boleto.Carteira.Equals("SR")) ? "2" : boleto.Carteira;

                // Posição 31 - 33
                string segundaParteNossoNumero = boleto.NossoNumero.Substring(0, 3); //(3, 3);

                // Posição 24
                string segundaConstante = EmissaoCedente.ToString();

                //Posição 35 - 43
                string terceiraParteNossoNumero = boleto.NossoNumero.Substring(3, 7) + segundaConstante +
                                                  segundaConstante; //(6, 9);

                //Posição 44
                string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}", codigoCedente, dvCodigoCedente,
                    primeiraParteNossoNumero,
                    primeiraConstante, segundaParteNossoNumero, segundaConstante,
                    terceiraParteNossoNumero);

                string dvCampoLivre = Mod11Base9(ccc).ToString();

                campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);
            }


            string xxxx = string.Format("{0}{1}{2}{3}{4}", banco, moeda, fatorVencimento, valorDocumento, campoLivre);

            string dvGeral = Mod11(xxxx, 9).ToString();
            // Posição 5
            _dacBoleto = dvGeral;

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}",
                banco,
                moeda,
                dvGeral,
                fatorVencimento,
                valorDocumento,
                campoLivre
            );
        }

        /// <summary>
        ///   IMPLEMENTAÇÃO PARA NOSSO NÚMERO COM 17 POSIÇÕES
        ///   Autor.: Fábio Marcos
        ///   E-Mail: fabiomarcos@click21.com.br
        ///   Data..: 01/03/2011
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string Grupo1;
            string Grupo2;
            string Grupo3;
            string Grupo4;
            string Grupo5;

            if (boleto.NossoNumero.Length == 17)
            {
                #region Campo 1

                //POSIÇÃO 1 A 4 DO CODIGO DE BARRAS
                string str1 = boleto.CodigoBarra.Codigo.Substring(0, 4);
                //POSICAO 20 A 24 DO CODIGO DE BARRAS
                string str2 = boleto.CodigoBarra.Codigo.Substring(19, 5);
                //CALCULO DO DIGITO
                string str3 = Mod10(str1 + str2).ToString();

                Grupo1 = str1 + str2 + str3;
                Grupo1 = Grupo1.Substring(0, 5) + "." + Grupo1.Substring(5) + " ";

                #endregion Campo 1

                #region Campo 2

                //POSIÇÃO 25 A 34 DO COD DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(24, 10);
                //DIGITO
                str2 = Mod10(str1).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 2

                #region Campo 3

                //POSIÇÃO 35 A 44 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(34, 10);
                //DIGITO
                str2 = Mod10(str1).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto;

                Grupo4 = string.Format("{0} ", D4);

                #endregion Campo 4

                #region Campo 5

                //POSICAO 6 A 9 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(5, 4);

                //POSICAO 10 A 19 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarra.Codigo.Substring(9, 10);

                Grupo5 = string.Format("{0}{1}", str1, str2);

                #endregion Campo 5
            }
            else
            {
                #region Campo 1

                string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
                string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
                string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
                string D1 = Mod10(BBB + M + CCCCC).ToString();

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ",
                    BBB,
                    M,
                    CCCCC.Substring(0, 1),
                    CCCCC.Substring(1, 4), D1);


                #endregion Campo 1

                #region Campo 2

                string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
                string D2 = Mod10(CCCCCCCCCC2).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

                #endregion Campo 2

                #region Campo 3

                string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
                string D3 = Mod10(CCCCCCCCCC3).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto;

                Grupo4 = string.Format(" {0} ", D4);

                #endregion Campo 4

                #region Campo 5

                long FFFF = FatorVencimento(boleto);

                string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

                if (Utils.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

                #endregion Campo 5
            }

            //MONTA OS DADOS DA INHA DIGITÁVEL DE ACORDO COM OS DADOS OBTIDOS ACIMA
            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.Carteira.Equals(CarteiraSR) || boleto.Carteira.Equals("SR"))
            {
                if (boleto.NossoNumero.Length == 14)
                {
                    boleto.NossoNumero = "8" + boleto.NossoNumero;
                }
            }

            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Base9(boleto.NossoNumero)); //
            //boleto.NossoNumero = string.Format("{0}{1}/{2}-{3}", boleto.Carteira, EMISSAO_CEDENTE, boleto.NossoNumero, Mod11Base9(boleto.Carteira + EMISSAO_CEDENTE + boleto.NossoNumero));
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {

        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira.Equals(CarteiraSR) || boleto.Carteira.Equals("SR"))
            {
                if ((boleto.NossoNumero.Length != 10) && (boleto.NossoNumero.Length != 14) && (boleto.NossoNumero.Length != 17))
                {
                    throw new Exception("Nosso Número inválido, Para Caixa Econômica - Carteira SR o Nosso Número deve conter 10, 14 ou 17 posições.");
                }
            }
            else if (boleto.Carteira.Equals(CarteiraRG) || boleto.Carteira.Equals("RG"))
            {
                if (boleto.NossoNumero.Length != 17)
                    throw new Exception("Nosso número inválido. Para Caixa Econômica - SIGCB carteira rápida, o nosso número deve conter 17 caracteres.");
            }
            else if (boleto.Carteira.Equals("CS"))
            {
                if (boleto.NossoNumero.Any(ch => !ch.Equals('0')))
                {
                    throw new Exception("Nosso Número inválido, Para Caixa Econômica - SIGCB carteira simples, o Nosso Número deve estar zerado.");
                }
            }
            else
            {
                if (Convert.ToInt64(boleto.NossoNumero).ToString().Length < 10)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);

                if (boleto.NossoNumero.Length != 10)
                {
                    throw new Exception(
                        "Nosso Número inválido, Para Caixa Econômica carteira indefinida, o Nosso Número deve conter 10 caracteres.");
                }

                if (!boleto.Cedente.Codigo.Equals("0"))
                {
                    string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 6);
                    string dvCodigoCedente = Mod10(codigoCedente).ToString(); //Base9 

                    if (boleto.Cedente.DigitoCedente.Equals(-1))
                        boleto.Cedente.DigitoCedente = Convert.ToInt32(dvCodigoCedente);
                }
                else
                {
                    throw new Exception("Informe o código do cedente.");
                }
            }

            if (boleto.Cedente.DigitoCedente == -1)
                boleto.Cedente.DigitoCedente = Mod11Base9(boleto.Cedente.Codigo);

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            if (boleto.Cedente.Codigo.Length > 6)
                throw new Exception("O código do cedente deve conter apenas 6 dígitos");

            //Atribui o nome do banco ao local de pagamento
            //Suélton 23/03/18 - Na homolagação do boleto junto a Caixa solicitaram que o texto do local de pagamento fosse esse
            //Estou deixando também para que se possa personalizar na aplicação caso necessário
            if (string.IsNullOrEmpty(boleto.LocalPagamento))
                boleto.LocalPagamento = "PREFERENCIALMENTE NAS CASAS LOTÉRICAS ATÉ O VALOR LIMITE";
            else if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            /* 
             * Na Carteira Simples não é necessário gerar a impressão do boleto,
             * logo não é necessário formatar linha digitável nem cód de barras
             * Jéferson (jefhtavares) em 10/03/14
             */

            if (!boleto.Carteira.Equals("CS"))
            {
                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
                FormataNossoNumero(boleto);
            }
        }

        #region Métodos de geração do arquivo remessa
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

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente);
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

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            try
            {
                string _header = " ";
                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            _header = GerarHeaderRemessaCNAB240SIGCB(cedente);
                        else
                            _header = GerarHeaderRemessaCNAB240(cedente);
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

        /// <summary>
        /// Gera as linhas de detalhe da remessa.
        /// </summary>
        /// <param name="boleto">Objeto do tipo <see cref="Boleto"/> para o qual as linhas serão geradas.</param>
        /// <param name="numeroRegistro">Número do registro.</param>
        /// <param name="tipoArquivo"><see cref="TipoArquivo"/> do qual as linhas serão geradas.</param>
        /// <returns>Linha gerada</returns>
        /// <remarks>Esta função não existia, mas as funções que ela chama já haviam sido implementadas. Só criei esta função pois a original estava chamando o método abstrato em IBanco.</remarks>
        public new string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = this.GerarDetalheSegmentoPRemessaCNAB240SIGCB(this.Cedente, boleto, numeroRegistro);
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

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            if (boleto.Remessa.TipoDocumento.Equals("2") || boleto.Remessa.TipoDocumento.Equals("1"))
                return GerarDetalheSegmentoPRemessaCNAB240SIGCB(cedente, boleto, numeroRegistro);
            else
                return GerarDetalheSegmentoPRemessaCNAB240(boleto, numeroRegistro, numeroConvenio, cedente);
        }
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            return GerarDetalheSegmentoQRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
        }
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            return GerarDetalheSegmentoQRemessaCNAB240SIGCB(boleto, numeroRegistro, sacado);
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            if (boleto.Remessa.TipoDocumento.Equals("2") || boleto.Remessa.TipoDocumento.Equals("1"))
                return GerarDetalheSegmentoRRemessaCNAB240SIGCB(boleto, numeroRegistroDetalhe, CNAB240);
            else
                return GerarDetalheSegmentoRRemessaCNAB240(boleto, numeroRegistroDetalhe, CNAB240);
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerLoteRemessaCNAC240SIGCB(numeroRegistro);
            else
                return GerarTrailerLoteRemessaCNAB240(numeroRegistro);
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerRemessaCNAB240SIGCB(numeroRegistro);
            else
                return GerarTrailerArquivoRemessaCNAB240(numeroRegistro);
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
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        /// <summary>
        /// Gera as linhas de trailer da remessa.
        /// </summary>
        /// <param name="numeroRegistro">Número do registro.</param>
        /// <param name="tipoArquivo"><see cref="TipoArquivo"/> do qual as linhas serão geradas.</param>
        /// <param name="cedente">Objeto do tipo <see cref="Cedente"/> para o qual o trailer será gerado.</param>
        /// <param name="vltitulostotal">Valor total dos títulos do arquivo.</param>
        /// <returns>Linha gerada.</returns>
        /// <remarks>Esta função não existia, mas as funções que ela chama já haviam sido implementadas. Só criei esta função pois a original estava chamando o método abstrato em IBanco.</remarks>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessaCNAB240SIGCB(numeroRegistro);
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro, 0);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            header = GerarHeaderLoteRemessaCNAC240SIGCB(cedente, numeroArquivoRemessa);
                        else
                            header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        #endregion

        #region CNAB 240
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null)
            {
                vMsg += string.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            //validação de cada boleto
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else if (boleto.Remessa.TipoDocumento.Equals("1") && string.IsNullOrEmpty(boleto.Sacado.Endereco.CEP)) //1 - SICGB - Com registro
                {
                    //Para o "Remessa.TipoDocumento = "1", o CEP é Obrigatório!
                    vMsg += string.Concat("Para o Tipo Documento [1 - SIGCB - COM REGISTRO], o CEP do SACADO é Obrigatório!", Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.NossoNumero.Length > 17)
                    boleto.NossoNumero = boleto.NossoNumero.Substring(0, 17);
                //if (!boleto.Remessa.TipoDocumento.Equals("2")) //2 - SIGCB - SEM REGISTRO
                //{
                //    //Para o "Remessa.TipoDocumento = "2", não poderá ter NossoNumero Gerado!
                //    vMsg += String.Concat("Tipo Documento de boleto não Implementado!", Environment.NewLine);
                //    vRetorno = false;
                //}
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        /// <summary>
        /// Varre as instrucoes para inclusao no Segmento P
        /// </summary>
        /// <param name="boleto"></param>
        private void validaInstrucoes240(Boleto boleto)
        {
            if (boleto.Instrucoes.Count.Equals(0))
                return;

            _protestar = false;
            _baixaDevolver = false;
            _desconto = false;
            _diasProtesto = 0;
            _diasDevolucao = 0;
            foreach (IInstrucao instrucao in boleto.Instrucoes)
            {
                if (instrucao.Codigo.Equals(9) || instrucao.Codigo.Equals(42) || instrucao.Codigo.Equals(81) || instrucao.Codigo.Equals(82))
                {
                    _protestar = true;
                    _diasProtesto = instrucao.QuantidadeDias;
                }
                else if (instrucao.Codigo.Equals(91) || instrucao.Codigo.Equals(92))
                {
                    _baixaDevolver = true;
                    _diasDevolucao = instrucao.QuantidadeDias;
                }
                else if (instrucao.Codigo.Equals(999))
                {
                    _desconto = true;
                }
            }
        }
        public string GerarHeaderRemessaCNAB240(Cedente cedente)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0000";                                                                       // Lote de Serviço 
                header += "0";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                                   // Tipo de Inscrição 
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15);                                   // CPF/CNPJ do cedente 
                header += Utils.FormatCode(cedente.Codigo + cedente.DigitoCedente, "0", 16);            // Código do Convênio no Banco 
                header += Utils.FormatCode("", "0", 4);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Agência Mantenedora da Conta 
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 5);                // Dígito Verificador da Agência 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // Código do Cedente (sem operação)  
                header += cedente.ContaBancaria.DigitoConta;                                            // Díg. Verif. Cedente (sem operação) 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();// Dígito Verif. Ag./Ced  (sem operação)
                header += Utils.FormatCode(cedente.Nome, " ", 30);                                      // Nome do cedente
                header += Utils.FormatCode("CAIXA ECONOMICA FEDERAL", " ", 30);                         // Nome do Banco
                header += Utils.FormatCode("", " ", 10);                                                // Uso Exclusivo FEBRABAN/CNAB
                header += "1";                                                                          // Código 1 - Remessa / 2 - Retorno 
                header += DateTime.Now.ToString("ddMMyyyy");                                            // Data de Geração do Arquivo
                header += string.Format("{0:hh:mm:ss}", DateTime.Now).Replace(":", "");                  // Hora de Geração do Arquivo
                header += "000001";                                                                     // Número Seqüencial do Arquivo 
                header += "030";                                                                        // Número da Versão do Layout do Arquivo 
                header += "0";                                                                          // Densidade de Gravação do Arquivo 
                header += Utils.FormatCode("", " ", 20);                                                // Para Uso Reservado do Banco
                // Na fase de teste deve conter "remessa-produção", após aprovado deve conter espaços em branco
                header += Utils.FormatCode("remessa-produção", " ", 20);                                // Para Uso Reservado da Empresa  
                //header += Utils.FormatCode("", " ", 20);                                              // Para Uso Reservado da Empresa
                header += Utils.FormatCode("", " ", 29);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "1";                                                                          // Tipo de Registro 
                header += "R";                                                                          // Tipo de Operação 
                header += "01";                                                                         // Tipo de Serviço '01' = Cobrança, '03' = Bloqueto Eletrônico 
                header += "  ";                                                                         // Uso Exclusivo FEBRABAN/CNAB
                header += "020";                                                                        // Número da Versão do Layout do Arquivo 
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                                   // Tipo de Inscrição 
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15);                                   // CPF/CNPJ do cedente
                header += Utils.FormatCode(cedente.Codigo + cedente.DigitoCedente, "0", 16);            // Código do Convênio no Banco 
                header += Utils.FormatCode("", " ", 4);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Agência Mantenedora da Conta 
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 5);                // Dígito Verificador da Agência 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // Número da Conta Corrente 
                header += cedente.ContaBancaria.DigitoConta;                                            // Digito Verificador da Conta Corrente 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();// Dígito Verif. Ag./Ced  (sem operação)
                header += Utils.FormatCode(cedente.Nome, " ", 30);                                      // Nome do cedente
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 1
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 2
                header += numeroArquivoRemessa.ToString("00000000");                                    // Número Remessa/Retorno
                header += DateTime.Now.ToString("ddMMyyyy");                                            // Data de Gravação Remessa/Retorno 
                header += Utils.FormatCode("", "0", 8);                                                 // Data do Crédito 
                header += Utils.FormatCode("", " ", 33);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }
        public string GerarDetalheSegmentoPRemessaCNAB240(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            try
            {
                validaInstrucoes240(boleto); // Para protestar, devolver ou desconto.

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "3";                                                                          // Tipo de Registro 
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // Nº Sequencial do Registro no Lote 
                header += "P";                                                                          // Cód. Segmento do Registro Detalhe
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += "01";                                                                         // Código de Movimento Remessa 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Agência Mantenedora da Conta 
                header += cedente.ContaBancaria.DigitoAgencia;                                          // Dígito Verificador da Agência 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // Número da Conta Corrente 
                header += cedente.ContaBancaria.DigitoConta;                                            // Digito Verificador da Conta Corrente 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString(); // Dígito Verif. Ag./Ced  (sem operação)
                header += Utils.FormatCode("", "0", 9);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(boleto.NossoNumero, "0", 11);                                // Identificação do Título no Banco 
                header += "01";                                                                         // Código da Carteira 
                header += (boleto.Carteira == "14" ? "2" : "1");                                        // Forma de Cadastr. do Título no Banco 
                // '1' = Com Cadastramento (Cobrança Registrada) 
                // '2' = Sem Cadastramento (Cobrança sem Registro) 
                header += "2";                                                                          // Tipo de Documento 
                header += "2";                                                                          // Identificação da Emissão do Bloqueto 
                header += "2";                                                                          // Identificação da Distribuição
                header += Utils.FormatCode(boleto.NumeroDocumento, "0", 11);                            // Número do Documento de Cobrança 
                header += "    ";                                                                       // Uso Exclusivo CAIXA
                header += boleto.DataVencimento.ToString("ddMMyyyy");                                   // Data de Vencimento do Título
                header += Utils.FormatCode(boleto.ValorBoleto.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 13); // Valor Nominal do Título 13
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Agência Encarregada da Cobrança 
                header += cedente.ContaBancaria.DigitoAgencia;                                          // Dígito Verificador da Agência 
                header += boleto.EspecieDocumento.Codigo;                                // Espécie do Título 
                header += boleto.Aceite;                                                                // Identific. de Título Aceito/Não Aceito
                // Data da Emissão do Título 
                header += (boleto.DataProcessamento.ToString("ddMMyyyy") == "01010001" ? DateTime.Now.ToString("ddMMyyyy") : boleto.DataProcessamento.ToString("ddMMyyyy"));
                header += "1";                                                                          // Código do Juros de Mora '1' = Valor por Dia - '2' = Taxa Mensal 
                header += (boleto.DataMulta.ToString("ddMMyyyy") == "01010001" ? "00000000" : boleto.DataMulta.ToString("ddMMyyyy")); // Data do Juros de Mora 

                header += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Juros de Mora por Dia/Taxa 

                header += (boleto.ValorDesconto > 0 ? "1" : "0"); // Código do Desconto 
                header += (boleto.DataDesconto.ToString("ddMMyyyy") == "01010001" ? "00000000" : boleto.DataDesconto.ToString("ddMMyyyy")); // Data do Desconto
                header += Utils.FormatCode(boleto.ValorDesconto.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 13); // Valor/Percentual a ser Concedido 
                header += Utils.FormatCode(boleto.IOF.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 13); // Valor do IOF a ser Recolhido 
                header += Utils.FormatCode(boleto.Abatimento.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 13); // Valor do Abatimento 

                header += Utils.FormatCode("", " ", 25);                                                // Identificação do Título na Empresa
                header += (_protestar ? "1" : "3");                                                      // Código para Protesto
                header += _diasProtesto.ToString("00");                                                  // Número de Dias para Protesto 2 posi
                header += (_baixaDevolver ? "1" : "2");                                                  // Código para Baixa/Devolução 1 posi
                header += _diasDevolucao.ToString("00");                                                 // Número de Dias para Baixa/Devolução 3 posi
                header += boleto.Moeda.ToString("00");                                                  // Código da Moeda 
                header += Utils.FormatCode("", " ", 10);                                                // Uso Exclusivo FEBRABAN/CNAB 
                header += Utils.FormatCode("", " ", 1);                                                 // Uso Exclusivo FEBRABAN/CNAB 

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO P do arquivo de remessa.", e);
            }
        }
        public string GerarDetalheSegmentoQRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "3";                                                                          // Tipo de Registro 
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // Nº Sequencial do Registro no Lote 
                header += "Q";                                                                          // Cód. Segmento do Registro Detalhe
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += "01";                                                                         // Código de Movimento Remessa
                header += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");                             // Tipo de Inscrição 
                header += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15);                             // Número de Inscrição 
                header += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);                                // Nome
                header += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);                        // Endereço
                header += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);                     // Bairro 
                header += boleto.Sacado.Endereco.CEP;                                                   // CEP + Sufixo do CEP
                header += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);                     // Cidade 
                header += boleto.Sacado.Endereco.UF;                                                    // Unidade da Federação
                // Estes campos deverão estar preenchidos quando não for o Cedente original do título.
                header += "0";                                                                          // Tipo de Inscrição 
                header += Utils.FormatCode("", "0", 15);                                                // Número de Inscrição CPF/CNPJ
                header += Utils.FormatCode("", " ", 40);                                                // Nome do Sacador/Avalista 
                //*********
                header += Utils.FormatCode("", " ", 31);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }
        public string GerarDetalheSegmentoRRemessaCNAB240(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "3";                                                                          // Tipo de Registro 
                header += Utils.FormatCode(numeroRegistroDetalhe.ToString(), "0", 5);                   // Nº Sequencial do Registro no Lote 
                header += "R";                                                                          // Cód. Segmento do Registro Detalhe
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += "01";                                                                         // Código de Movimento Remessa

                //Suelton - 14/12/2018 - Implementação do 2 desconto por antecipação
                if (boleto.DataDescontoAntecipacao2.HasValue && boleto.ValorDescontoAntecipacao2.HasValue)
                {
                    header += "1" + //'1' = Valor Fixo Até a Data Informada
                        Utils.FitStringLength(boleto.DataDescontoAntecipacao2.Value.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false) +
                        Utils.FormatCode(boleto.ValorDescontoAntecipacao2.Value.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 15);
                }
                else
                {
                    // Desconto 2
                    header += "000000000000000000000000"; //24 zeros
                }

                //Suelton - 14/12/2018 - Implementação do 3 desconto por antecipação
                if (boleto.DataDescontoAntecipacao3.HasValue && boleto.ValorDescontoAntecipacao3.HasValue)
                {
                    header += "1" + //'1' = Valor Fixo Até a Data Informada
                        Utils.FitStringLength(boleto.DataDescontoAntecipacao3.Value.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false) +
                        Utils.FormatCode(boleto.ValorDescontoAntecipacao3.Value.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 15);
                }
                else
                {
                    // Desconto 3
                    header += "000000000000000000000000"; //24 zeros
                }

                header += "1";                                          // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                header += boleto.DataMulta.ToString("ddMMyyyy");                                        // Data da Multa 
                header += Utils.FormatCode(boleto.ValorMulta.ToString(CultureInfo.InvariantCulture).Replace(",", "").Replace(".", ""), "0", 13); // Valor/Percentual a Ser Aplicado
                header += Utils.FormatCode("", " ", 10);                                                // Informação ao Sacado
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 3
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 4
                header += Utils.FormatCode("", " ", 61);                                                // Uso Exclusivo FEBRABAN/CNAB 

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO R do arquivo de remessa.", e);
            }
        }
        public string GerarTrailerLoteRemessaCNAB240(int numeroRegistro)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "5";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 61);                                                // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // Nº Sequencial do Registro no Lote 

                // Totalização da Cobrança Simples
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos Títulos em Carteiras

                header += Utils.FormatCode("", "0", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", "0", 15);                                                // Uso Exclusivo FEBRABAN/CNAB 

                // Totalização da Cobrança Caucionada
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos Títulos em Carteiras

                // Totalização da Cobrança Descontada
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos Títulos em Carteiras

                header += Utils.FormatCode("", " ", 8);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 117);                                               // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }
        }
        public string GerarTrailerArquivoRemessaCNAB240(int numeroRegistro)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "9999";                                                                       // Lote de Serviço
                header += "9";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += "000001";                                                                     // Quantidade de Lotes do Arquivo
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6);                          // Quantidade de Registros do Arquivo
                header += Utils.FormatCode("", " ", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 205);                                               // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de arquivo de remessa.", e);
            }
        }
        //
        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                /* 05 */
                if (!registro.Substring(13, 1).Equals(@"T"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento T.");
                }
                DetalheSegmentoTRetornoCNAB240 segmentoT =
                    new DetalheSegmentoTRetornoCNAB240(registro)
                    {
                        CodigoBanco = Convert.ToInt32(registro.Substring(0, 3)),
                        idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2))
                    };
                segmentoT.CodigoMovimento = new CodigoMovimento(001, segmentoT.idCodigoMovimento);
                segmentoT.NossoNumero = registro.Substring(39, 17);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.NumeroDocumento = registro.Substring(58, 11);
                segmentoT.DataVencimento = registro.Substring(73, 8) == "00000000" ? DateTime.Now : DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
                segmentoT.CodigoRejeicao = registro.Substring(213, 10);

                return segmentoT;
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
                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento U.");
                }

                DetalheSegmentoURetornoCNAB240 segmentoU =
                    new DetalheSegmentoURetornoCNAB240(registro)
                    {
                        JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / 100,
                        ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / 100,
                        ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / 100,
                        ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / 100
                    };

                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).Equals("00000000") ? DateTime.Now : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.CodigoOcorrenciaSacado = registro.Substring(153, 4);

                return segmentoU;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
        #endregion

        #region CNAB 240 - SIGCB
        public string GerarHeaderRemessaCNAB240SIGCB(Cedente cedente)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                 // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "0000", '0'));                                      // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "0", '0'));                                         // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));                                // posição 9 até 17     (9) - Uso Exclusivo FEBRABAN/CNAB
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "0";//não informado
                if (cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                  // posição 18 até 18   (1) - Tipo de Inscrição 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 014, 0, cedente.CPFCNPJ, '0'));                              // posição 19 até 32   (14)- Número de Inscrição da empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0033, 020, 0, "0", '0'));                                          // posição 33 até 52   (20)- Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0053, 005, 0, cedente.ContaBancaria.Agencia, '0'));                // posição 53 até 57   (5) - Agência Mantenedora da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0058, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' '));// posição 58 até 58   (1) - Dígito Verificador da Agência
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0059, 006, 0, cedente.Convenio, '0'));                             // posição 59 até 64   (6) - Código do Convênio no Banco (Código do Cedente)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0065, 007, 0, "0", '0'));                                          // posição 65 até 71   (7) - Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0072, 001, 0, "0", '0'));                                          // posição 72 até 72   (1) - Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 030, 0, cedente.Nome.ToUpper(), ' '));                       // posição 73 até 102  (30)- Nome da Empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0103, 030, 0, "CAIXA ECONOMICA FEDERAL", ' '));                    // posição 103 até 132 (30)- Nome do Banco
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0133, 010, 0, string.Empty, ' '));                                 // posição 133 até 142 (10)- Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 001, 0, "1", '0'));                                          // posição 143 até 413 (1) - Código 1 - Remessa / 2 - Retorno
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0144, 008, 0, DateTime.Now, ' '));                                 // posição 144 até 151 (8) - Data de Geração do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediHoraHHMMSS___________, 0152, 006, 0, DateTime.Now, ' '));                                 // posição 152 até 157 (6) - Hora de Geração do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0158, 006, 0, cedente.NumeroSequencial, '0'));                     // posição 158 até 163 (6) - Número Seqüencial do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0164, 003, 0, "050", '0'));                                        // posição 164 até 166 (3) - Nro da Versão do Layout do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0167, 005, 0, "0", '0'));                                          // posição 167 até 171 (5) - Densidade de Gravação do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0172, 020, 0, string.Empty, ' '));                                 // posição 172 até 191 (20)- Para Uso Reservado do Banco
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0192, 020, 0, "REMESSA-PRODUCAO", ' '));                           // posição 192 até 211 (20)- Para Uso Reservado da Empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0212, 004, 0, string.Empty, ' '));                                 // posição 212 até 215 (4) - Versão Aplicativo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0216, 025, 0, string.Empty, ' '));                                 // posição 216 até 240 (25)- Para Uso Reservado da Empresa
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
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }
        public string GerarHeaderLoteRemessaCNAC240SIGCB(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, 1, '0'));                                             // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "1", '0'));                                           // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0009, 001, 0, "R", ' '));                                           // posição 9 até 9     (1) - Tipo de Operação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                          // posição 10 até 11   (2) - Tipo de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0012, 002, 0, "00", '0'));                                          // posição 12 até 13   (2) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0014, 003, 0, "030", '0'));                                         // posição 14 até 16   (3) - Nº da Versão do Layout do Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0017, 001, 0, string.Empty, ' '));                                  // posição 17 até 17   (1) - Uso Exclusivo FEBRABAN/CNAB
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "0";//não informado
                if (cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                   // posição 18 até 18   (1) - Tipo de Inscrição 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 015, 0, cedente.CPFCNPJ, '0'));                               // posição 19 até 33   (15)- Número de Inscrição da empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0034, 006, 0, cedente.Convenio, '0'));                              // posição 34 até 39   (6) - Código do Convênio no Banco
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0040, 014, 0, "0", '0'));                                           // posição 40 até 53   (14)- Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0054, 005, 0, cedente.ContaBancaria.Agencia, '0'));                 // posição 54 até 58   (5) - Agência Mantenedora da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0059, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posição 59 até 59   (1) - Dígito Verificador da Agência
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0060, 006, 0, cedente.Convenio, '0'));                              // posição 60 até 65   (6) - Código do Convênio no Banco                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0066, 007, 0, "0", '0'));                                           // posição 66 até 72   (7) - Código do Modelo Personalizado
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0073, 001, 0, "0", '0'));                                           // posição 73 até 73   (1) - Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 030, 0, cedente.Nome.ToUpper(), ' '));                        // posição 73 até 103  (30)- Nome da Empresa     
                //TODO.: ROGER KLEIN - INSTRUÇÕES NÃO TRATADAS
                #region Instruções
                //string descricao = string.Empty;
                ////
                string vInstrucao1 = string.Empty;
                string vInstrucao2 = string.Empty;
                //foreach (Instrucao_Caixa instrucao in boleto.Instrucoes)
                //{
                //    switch ((EnumInstrucoes_Caixa)instrucao.Codigo)
                //    {
                //        case EnumInstrucoes_Caixa.Protestar:
                //            //
                //            //instrucao.Descricao.ToString().ToUpper();
                //            break;
                //        case EnumInstrucoes_Caixa.NaoProtestar:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                //            break;
                //    }
                //}
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0104, 040, 0, vInstrucao1, ' '));                                   // posição 104 até 143 (40) - Mensagem 1
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0144, 040, 0, vInstrucao2, ' '));                                   // posição 144 até 183 (40) - Mensagem 2
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0184, 008, 0, numeroArquivoRemessa, '0'));                          // posição 184 até 191 (8)  - Número Remessa/Retorno
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0192, 008, 0, DateTime.Now, ' '));                                  // posição 192 até 199 (8) - Data de Geração do Arquivo                
                /*Data do Crédito
               Data de efetivação do crédito referente ao pagamento do título de cobrança. 
               Informação enviada somente no arquivo de retorno. 2.1 Data do Crédito Filler 200 207 9(008) Preencher com zeros C003 */
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0200, 008, 0, '0', '0'));                             // posição 200 até 207 (8) - Data do Crédito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0208, 033, 0, string.Empty, ' '));                                  // posição 208 até 240(33) - Uso Exclusivo FEBRABAN/CNAB
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _headerLote = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }
        //
        #region Detalhes
        public string GerarDetalheSegmentoPRemessaCNAB240SIGCB(Cedente cedente, Boleto boleto, int numeroRegistro)
        {
            try
            {
                #region Segmento P
                validaInstrucoes240(boleto);
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                           // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                           // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                                // posição 9 até 13    (5) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "P", '0'));                                           // posição 14 até 14   (1) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                  // posição 15 até 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));               // posição 16 até 17   (2) - Código de Movimento Remessa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 005, 0, cedente.ContaBancaria.Agencia, '0'));                 // posição 18 até 22   (5) - Agência Mantenedora da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0023, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posição 23 até 23   (1) - Dígito Verificador da Agência
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0024, 006, 0, cedente.Convenio, '0'));                              // posição 24 até 29   (6) - Código do Convênio no Banco
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0030, 011, 0, "0", '0'));                                           // posição 30 até 40   (11)- Uso Exclusivo CAIXA
                //modalidade são os dois algarimos iniciais do nosso número...                
                //nosso numero já traz a modalidade concatenada, então passa direto o nosso nro que preenche os dois campos do leiaute
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0041, 017, 0, boleto.NossoNumero, '0'));                            // posição 41 até 57   (15)- Identificação do Título no Banco
                #region Código da Carteira
                //Código adotado pela FEBRABAN, para identificar a característica dos títulos dentro das modalidades de cobrança existentes no banco.
                //?1? = Cobrança Simples; ?3? = Cobrança Caucionada; ?4? = Cobrança Descontada
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0058, 001, 0, "1", '0'));                                           // posição 58 até 58   (1) - Código Carteira
                #endregion
                #region Forma de Cadastramento do Título no Banco
                string vFormaCadastramento = "1";// Com registro
                if (boleto.Remessa.TipoDocumento.Equals("2"))
                    vFormaCadastramento = "2";//sem registro               
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0059, 001, 0, vFormaCadastramento, '0'));                           // posição 59 até 59   (1) - Forma de Cadastr. do Título no Banco 1 - Com Registro 2 - Sem registro.
                #region Tratamento do tipo Cobrança (com ou sem registro)

                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0060, 001, 0, "2", '0'));
                string Emissao = boleto.Carteira.Equals("CS") ? "1" : "2";// posição 60 até 60   (1) - Tipo de Documento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0061, 001, 0, Emissao, '0'));                                       // posição 61 até 61   (1) - Identificação da Emissão do Bloqueto -- ?1?-Banco Emite, '2'-entrega do bloqueto pelo Cedente                           
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0062, 001, 0, "0", '0'));                                           // posição 62 até 62   (1) - Identificação da Entrega do Bloqueto /* ?0? = Postagem pelo Cedente ?1? = Sacado via Correios ?2? = Cedente via Agência CAIXA*/ 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 011, 0, boleto.NumeroDocumento, ' '));                        // posição 63 até 73   (11)- Número do Documento de Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 004, 0, string.Empty, ' '));                                  // posição 74 até 77   (4) - Uso Exclusivo CAIXA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0078, 008, 0, boleto.DataVencimento, ' '));                         // posição 78 até 85   (8) - Data de Vencimento do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0086, 015, 2, boleto.ValorBoleto, '0'));                            // posição 86 até 100  (15)- Valor Nominal do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 005, 2, "0", '0'));//0sistema atribui AEC pelo CEP do sacado  // posição 101 até 105 (5) - AEC = Agência Encarregada da Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0106, 001, 0, "0", '0'));                                           // posição 106 até 106 (1) - Campo 23.3P Dígito Verificador da Agência Preencher '0'
                string EspDoc = boleto.EspecieDocumento.Sigla.Equals("DM") ? "02" : boleto.EspecieDocumento.Codigo;
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 2, EspDoc, '0'));                                        // posição 107 até 108 (2) - Espécie do Título
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 001, 0, boleto.Aceite, ' '));                                 // posição 109 até 109 (1) - Identific. de Título Aceito/Não Aceito
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0110, 008, 0, boleto.DataDocumento, '0'));                          // posição 110 até 117 (8) - Data da Emissão do Título
                #region Código de juros
                string CodJurosMora;
                if (boleto.JurosMora == 0 && boleto.PercJurosMora == 0)
                    CodJurosMora = "3";
                else
                    CodJurosMora = "1";
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0118, 001, 2, CodJurosMora, '0'));                                  // posição 118 até 118 (1) - Código do Juros de Mora
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0119, 008, 0, boleto.DataJurosMora, '0'));                          // posição 119 até 126 (8) - Data do Juros de Mora
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 015, 2, boleto.JurosMora, '0'));                              // posição 127 até 141 (15)- Juros de Mora por Dia/Taxa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0142, 001, 0, boleto.ValorDesconto > 0 ? "1" : "0", '0'));          // posição 142 até 142 (1) -  Código do Desconto 1 - "0" = Sem desconto. "1"= Valor Fixo até a data informada "2" = Percentual até a data informada
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0143, 008, 0, boleto.DataDesconto, '0'));                           // posição 143 até 150 (8) - Data do Desconto
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0151, 015, 2, boleto.ValorDesconto, '0'));                          // posição 151 até 165 (15)- Valor/Percentual a ser Concedido
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0166, 015, 2, boleto.IOF, '0'));                                    // posição 166 até 180 (15)- Valor do IOF a ser concedido
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0181, 015, 2, boleto.Abatimento, '0'));                             // posição 181 até 195 (15)- Valor do Abatimento
                //Alterado por diegodariolli 16/03/2018 - acredito que para controle interno do software deve ser informado aqui o número de controle e não o número do documento, já informado anteriormente
				reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0196, 025, 0, boleto.NumeroControle, ' '));                        // posição 196 até 220 (25)- Identificação do Título na Empresa. Informar o Número do Documento - Seu Número (mesmo das posições 63-73 do Segmento P)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 001, 0, (_protestar ? "1" : "3"), '0'));                       // posição 221 até 221 (1) -  Código para protesto  - ?1? = Protestar. "3" = Não Protestar. "9" = Cancelamento Protesto Automático
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0222, 002, 0, _diasProtesto, '0'));                                  // posição 222 até 223 (2) -  Número de Dias para Protesto                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0224, 001, 0, (_baixaDevolver || !_protestar ? "1" : "2"), '0'));    // posição 224 até 224 (1) -  Código para Baixa/Devolução ?1? = Baixar / Devolver. "2" = Não Baixar / Não Devolver
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0225, 003, 0, _diasDevolucao, '0'));                                 // posição 225 até 227 (3) - Número de Dias para Baixa/Devolução
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0228, 002, 0, "09", '0'));                                          // posição 228 até 229 (2) - Código da Moeda. Informar fixo: ?09? = REAL
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0230, 010, 2, "0", '0'));                                           // posição 230 até 239 (10)- Uso Exclusivo CAIXA                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0240, 001, 0, string.Empty, ' '));                                  // posição 240 até 240 (1) - Uso Exclusivo FEBRABAN/CNAB
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoP = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoP;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento P no arquivo de remessa do CNAB240 SIGCB.", ex);
            }

        }
        public string GerarDetalheSegmentoQRemessaCNAB240SIGCB(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            try
            {
                #region Segmento Q
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                  // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                          // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                          // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                               // posição 9 até 13    (5) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "Q", '0'));                                          // posição 14 até 14   (1) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                 // posição 15 até 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));              // posição 16 até 17   (2) - Código de Movimento Remessa
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "0";//não informado
                if (sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                  // posição 18 até 18   (1) - Tipo de Inscrição 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 015, 0, sacado.CPFCNPJ, '0'));                               // posição 19 até 33   (15)- Número de Inscrição da empresa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0034, 040, 0, sacado.Nome.ToUpper(), ' '));                        // posição 34 até 73   (40)- Nome
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 040, 0, sacado.Endereco.End.ToUpper(), ' '));                // posição 74 até 113  (40)- Endereço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0114, 015, 0, sacado.Endereco.Bairro.ToUpper(), ' '));             // posição 114 até 128 (15)- Bairro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0129, 008, 0, sacado.Endereco.CEP, ' '));                          // posição 114 até 128 (40)- CEP                
                // posição 134 até 136 (3) - sufixo cep** já incluso em CEP                                                                                                                                                                   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0137, 015, 0, sacado.Endereco.Cidade.ToUpper(), ' '));             // posição 137 até 151 (15)- Cidade
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0152, 002, 0, sacado.Endereco.UF, ' '));                           // posição 152 até 153 (15)- UF
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0154, 001, 0, "0", '0'));                                          // posição 154 até 154 (1) - Tipo de Inscrição Sacador Avalialista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0155, 015, 0, "0", '0'));                                          // posição 155 até 169 (15)- Inscrição Sacador Avalialista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0170, 040, 0, string.Empty, ' '));                                 // posição 170 até 209 (40)- Nome do Sacador/Avalista
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0210, 003, 0, string.Empty, ' '));                                          // posição 210 até 212 (3) - Cód. Bco. Corresp. na Compensação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0213, 020, 0, string.Empty, ' '));                                 // posição 213 até 232 (20)- Nosso Nº no Banco Correspondente
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0233, 008, 0, string.Empty, ' '));                                 // posição 213 até 232 (8)- Uso Exclusivo FEBRABAN/CNAB
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoQ = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoQ;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento Q no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }
        public string GerarDetalheSegmentoRRemessaCNAB240SIGCB(Boleto boleto, int numeroRegistro, TipoArquivo CNAB240)
        {
            try
            {
                #region Segmento R
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                  // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                          // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                          // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                               // posição 9 até 13    (5) - Nº Sequencial do Registro no Lote
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, "R", '0'));                                          // posição 14 até 14   (1) - Cód. Segmento do Registro Detalhe
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                 // posição 15 até 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));              // posição 16 até 17   (2) - Código de Movimento Remessa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, "0", '0'));                                          // posição 18 até 18   (1) - Código de desconto 2
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 008, 0, "0", '0'));                                          // posição 19 até 26   (8) - Data de Desconto 2
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 015, 2, "0", '0'));                                          // posição 27 até 41   (15) - Valor ou Percentual desconto 2
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0042, 001, 0, "0", '0'));                                          // posição 42 até 42   (1) - Código de desconto 3
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0043, 008, 0, "0", '0'));                                          // posição 43 até 50   (8) - Data de Desconto 3
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0051, 015, 2, "0", '0'));                                          // posição 51 até 65   (15) - Valor ou Percentual desconto 3
                #region Código de Multa
                string CodMulta;
                decimal ValorOuPercMulta;
                if (boleto.ValorMulta > 0)
                {
                    CodMulta = "1";
                    ValorOuPercMulta = boleto.ValorMulta;
                }
                else if (boleto.PercMulta > 0)
                {
                    CodMulta = "2";
                    ValorOuPercMulta = boleto.PercMulta;
                }
                else
                {
                    CodMulta = "0";
                    ValorOuPercMulta = 0;
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0066, 001, 0, CodMulta, '0'));                                     // posição 66 até 66   (1) - Código da Multa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAAAA_________, 0067, 008, 0, boleto.DataMulta, '0'));                             // posição 67 até 74   (8) - Data da Multa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0075, 015, 2, ValorOuPercMulta, '0'));                             // posição 75 até 89   (15) - Valor ou Percentual Multa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0090, 010, 0, string.Empty, ' '));                                 // posição 90 até 99   (10) - Informação ao Pagador
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0100, 040, 0, string.Empty, ' '));                                 // posição 100 até 139 (40) - Mensagem 3
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 040, 0, string.Empty, ' '));                                 // posição 140 até 179 (40) - Mensagem 4
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0180, 050, 0, string.Empty, ' '));                                 // posição 180 até 229 (50) - E-mail pagador p/envio de informações
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0230, 011, 0, string.Empty, ' '));                                 // posição 230 até 240 (11) - Filler
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _SegmentoR = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoR;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento R no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }
        public string GerarDetalheSegmentoYRemessaCNAB240SIGCB()
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento Y no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }
        #endregion
        //
        public string GerarTrailerLoteRemessaCNAC240SIGCB(int numeroRegistro)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));                                   // posição 1 até 3     (3) - código do banco na compensação        
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                  // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "5", '0'));                                           // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));                                  // posição 9 até 17    (9) - Uso Exclusivo FEBRABAN/CNAB
                #region Pega o Numero de Registros - Já está sendo Adicionado pelo ArquivoRemessaCNAB240
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 006, 0, numeroRegistro, '0'));                                  // posição 18 até 23   (6) - Quantidade de Registros no Lote
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0024, 006, 0, "0", '0'));                                           // posição 24 até 29   (6) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0030, 017, 2, "0", '0'));                                           // posição 30 até 46  (15) - Valor Total dos Títulos em Carteiras
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0047, 006, 0, "0", '0'));                                           // posição 47 até 52   (6) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0053, 017, 2, "0", '0'));                                           // posição 53 até 69   (15) - Valor Total dos Títulos em Carteiras                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0070, 006, 0, "0", '0'));                                           // posição 70 até 75   (6) - Quantidade de Títulos em Cobrança
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0076, 017, 2, "0", '0'));                                           // posição 76 até 92   (15)- Quantidade de Títulos em Carteiras 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0093, 031, 0, string.Empty, ' '));                                  // posição 93 até 123  (31) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0124, 117, 0, string.Empty, ' '));                                  // posição 124 até 240  (117)- Uso Exclusivo FEBRABAN/CNAB                
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _headerLote = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }
        public string GerarTrailerRemessaCNAB240SIGCB(int numeroRegistro)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 003, 0, base.Codigo, '0'));     //posição 1 até 3      (3) - Código do Banco na Compensação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 004, 0, "9999", '0'));          // posição 4 até 7     (4) - Lote de Serviço
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0008, 001, 0, "9", '0'));             // posição 8 até 8     (1) - Tipo de Registro
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));    // posição 9 até 17    (9) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 006, 0, "1", '0'));             // posição 18 até 23   (6) - Quantidade de Lotes do Arquivo
                #region Pega o Numero de Registros - Já está sendo adicionado pelo ArquivoRemessaCNAB240
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0024, 006, 0, numeroRegistro, '0')); // posição 24 até 29   (6) - Quantidade de Registros do Arquivo
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0030, 006, 0, string.Empty, ' '));    // posição 30 até 35   (6) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0036, 205, 0, string.Empty, ' '));    // posição 36 até 240(205) - Uso Exclusivo FEBRABAN/CNAB
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
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
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
                vMsg += string.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.Sacado == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Sacado: Informe os dados do sacado!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    if (boleto.Sacado.Nome == null)
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Nome: Informe o nome do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }

                    if (string.IsNullOrEmpty(boleto.Sacado.CPFCNPJ))
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; CPF/CNPJ: Informe o CPF ou CNPJ do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }

                    if (boleto.Sacado.Endereco == null)
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o endereço do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(boleto.Sacado.Endereco.End))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o Endereço do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (string.IsNullOrEmpty(boleto.Sacado.Endereco.Bairro))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o Bairro do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (string.IsNullOrEmpty(boleto.Sacado.Endereco.CEP) || boleto.Sacado.Endereco.CEP == "00000000")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe o CEP do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (string.IsNullOrEmpty(boleto.Sacado.Endereco.Cidade))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe a cidade do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (string.IsNullOrEmpty(boleto.Sacado.Endereco.UF))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endereço: Informe a UF do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                    }
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
                string codRemessa = (cedente.TipoAmbiente == Remessa.TipoAmbiente.Homologacao ? "REM.TST" : "REMESSA");
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                            //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, codRemessa, ' '));                     //003-009 REM.TST
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                           //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                     //012-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 004, 0, cedente.ContaBancaria.Agencia, '0'));  //027-030
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0031, 006, 0, cedente.Codigo, '0'));                 //031-036
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0037, 010, 0, string.Empty, ' '));                   //037-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));         //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 003, 0, "104", '0'));                          //077-079
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, "C ECON FEDERAL", ' '));               //080-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                   //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 289, 0, string.Empty, ' '));                   //101-389
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0390, 005, 0, numeroArquivoRemessa, '0'));           //390-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, 1, '0'));                               //395-400
                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
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
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ CAIXA ECONOMICA FEDERAL
                //
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));                                       //001-001
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf é sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj é sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, vCpfCnpjEmi, '0'));                               //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    //004-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //018-021
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 006, 0, boleto.Cedente.Codigo, ' '));                     //022-027
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0028, 001, 0, '2', ' '));                                       //028-028
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0029, 001, 0, '0', ' '));                                       //029-029
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0030, 002, 0, "00", ' '));                                      //030-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 025, 0, boleto.NumeroControle, '0'));                     //032-056  //alterado por diegodariolli - 16/03/2018
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0057, 002, 0, boleto.Carteira, '0'));                           //057-058
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0059, 015, 0, boleto.NossoNumero, '0'));                        //059-073
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 003, 0, string.Empty, ' '));                              //074-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 030, 0, string.Empty, ' '));                              //077-106
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, "01", '0'));                                      //107-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110   //REMESSA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0111, 010, 0, boleto.NumeroDocumento, '0'));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "104", '0'));                                     //140-142
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 005, 0, "00000", '0'));                                   //143-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0148, 002, 0, boleto.EspecieDocumento.Codigo, '0'));            //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));                      //151-156
                //
                #region Instruções
                string vInstrucao1 = "0";
                string vInstrucao2 = "0";
                string vInstrucao3 = "0";
                int prazoProtesto_Devolucao = 0;

                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Caixa)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Caixa.Protestar:
                            vInstrucao1 = "01";
                            prazoProtesto_Devolucao = instrucao.QuantidadeDias;
                            break;
                        case EnumInstrucoes_Caixa.DevolverAposNDias:
                            vInstrucao1 = "02";
                            prazoProtesto_Devolucao = instrucao.QuantidadeDias;
                            break;
                    }
                }
                #region OLD
                //switch (boleto.Instrucoes.Count)
                //{
                //    case 1:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = "0";
                //        vInstrucao3 = "0";
                //        break;
                //    case 2:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                //        vInstrucao3 = "0";
                //        break;
                //    case 3:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                //        vInstrucao3 = boleto.Instrucoes[2].Codigo.ToString();
                //        break;
                //}
                #endregion OLD
                #endregion Instruções
                //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, vInstrucao2, '0'));                               //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));                          //161-173
                #region DataDesconto
                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
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
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //315-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                #region DataMulta
                string vDataMulta = "000000";
                if (!boleto.DataMulta.Equals(DateTime.MinValue))
                    vDataMulta = boleto.DataMulta.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0352, 006, 0, vDataMulta, '0'));                                //352-357
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0358, 010, 2, boleto.ValorMulta, '0'));                         //358-367
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0368, 022, 0, string.Empty, ' '));                              //368-389
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0390, 002, 0, vInstrucao3, '0'));                               //390-391
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0392, 002, 0, prazoProtesto_Devolucao, '0'));                   //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0394, 001, 0, 1, '0'));                                         //394-394
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

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400

                reg.CodificarLinha();

                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                return new HeaderRetorno
                {
                    TipoRegistro = Utils.ToInt32(registro.Substring(000, 1)),
                    CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1)),
                    LiteralRetorno = registro.Substring(002, 7),
                    CodigoServico = Utils.ToInt32(registro.Substring(009, 2)),
                    LiteralServico = registro.Substring(011, 15),
                    Agencia = Utils.ToInt32(registro.Substring(026, 4)),
                    CodigoEmpresa = registro.Substring(030, 6),
                    NomeEmpresa = registro.Substring(046, 30),
                    CodigoBanco = Utils.ToInt32(registro.Substring(076, 3)),
                    NomeBanco = registro.Substring(079, 15),
                    DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##")),
                    Mensagem = registro.Substring(100, 58),
                    NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(389, 5)),
                    NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6)),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                TRegistroEDI_Caixa_Retorno reg = new TRegistroEDI_Caixa_Retorno { LinhaRegistro = registro };
                reg.DecodificarLinha();

                DetalheRetorno detalhe = new DetalheRetorno
                {
                    NumeroInscricao = reg.NumeroInscricaoEmpresa,
                    CodigoInscricao = Utils.ToInt32(reg.CodigoEmpresa),
                    NumeroControle = reg.IdentificacaoTituloEmpresa_NossoNumero,
                    NossoNumeroComDV = reg.IdentificacaoTituloEmpresa_NossoNumero_Modalidde +
                                       reg.IdentificacaoTituloCaixa_NossoNumero,
                    NossoNumero = reg.IdentificacaoTituloEmpresa_NossoNumero_Modalidde +
                                  reg.IdentificacaoTituloCaixa_NossoNumero.Substring(0,
                                      reg.IdentificacaoTituloCaixa_NossoNumero.Length - 1),
                    DACNossoNumero =
                        reg.IdentificacaoTituloCaixa_NossoNumero.Substring(
                            reg.IdentificacaoTituloCaixa_NossoNumero.Length - 1),
                    MotivosRejeicao = reg.CodigoMotivoRejeicao,
                    Carteira = reg.CodigoCarteira,
                    CodigoOcorrencia = !string.IsNullOrEmpty(reg.CodigoOcorrencia ) ? 
                        Utils.ToInt32(reg.CodigoOcorrencia) 
                        : 0,
                    DataOcorrencia = !string.IsNullOrEmpty(reg.DataOcorrencia) ? 
                        Utils.ToDateTime(Utils.ToInt32(reg.DataOcorrencia).ToString("##-##-##"))
                        : DateTime.MinValue,
                    NumeroDocumento = reg.NumeroDocumento,

                    DataVencimento = !string.IsNullOrEmpty(reg.DataVencimentoTitulo) ? 
                        Utils.ToDateTime(Utils.ToInt32(reg.DataVencimentoTitulo).ToString("##-##-##"))
                        : DateTime.MinValue,
                    ValorTitulo = !string.IsNullOrEmpty(reg.ValorTitulo) ? 
                        Convert.ToDecimal(reg.ValorTitulo) /100
                        : 0,
                    CodigoBanco = !string.IsNullOrEmpty(reg.CodigoBancoCobrador) ? 
                        Utils.ToInt32(reg.CodigoBancoCobrador) 
                        : 0,
                    AgenciaCobradora = !string.IsNullOrEmpty(reg.CodigoAgenciaCobradora) ? 
                        Utils.ToInt32(reg.CodigoAgenciaCobradora) 
                        : 0,
                    ValorDespesa = !string.IsNullOrEmpty(reg.ValorDespesasCobranca) ? 
                        (Convert.ToDecimal(reg.ValorDespesasCobranca) / 100)
                        : 0 ,
                    OrigemPagamento = reg.TipoLiquidacao,
                    IOF = !string.IsNullOrEmpty(reg.ValorIOF) ? 
                        (Convert.ToDecimal(reg.ValorIOF) / 100)
                        : 0 ,
                    ValorAbatimento = !string.IsNullOrEmpty(reg.ValorAbatimentoConcedido) ? 
                        (Convert.ToDecimal(reg.ValorAbatimentoConcedido) / 100)
                        : 0,
                    Descontos = !string.IsNullOrEmpty(reg.ValorDescontoConcedido) ? 
                        (Convert.ToDecimal(reg.ValorDescontoConcedido) / 100)
                        : 0,
                    ValorPago = !string.IsNullOrEmpty(reg.ValorPago) ? 
                        Convert.ToDecimal(reg.ValorPago) / 100
                        : 0 ,
                    JurosMora = !string.IsNullOrEmpty(reg.ValorJuros) ? 
                        (Convert.ToDecimal(reg.ValorJuros) / 100)
                        : 0,
                    TarifaCobranca = !string.IsNullOrEmpty(reg.ValorDespesasCobranca) ? 
                        (Convert.ToDecimal(reg.ValorDespesasCobranca) / 100)
                        : 0 ,
                    DataCredito = !string.IsNullOrEmpty(reg.DataCreditoConta) ? 
                        Utils.ToDateTime(Utils.ToInt32(reg.DataCreditoConta).ToString("##-##-##"))
                        : DateTime.MinValue,

                    NumeroSequencial = Utils.ToInt32(reg.NumeroSequenciaRegistro),
                    NomeSacado = reg.IdentificacaoTituloEmpresa
                };

                //detalhe.ValorPrincipal = detalhe.ValorPago;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public string Ocorrencia(string codigo)
        {
            int codigoMovimento;

            if (int.TryParse(codigo, out codigoMovimento))
            {
                CodigoMovimento_Caixa movimento = new CodigoMovimento_Caixa(codigoMovimento);
                return movimento.Descricao;
            }
            return string.Format("Erro ao retornar descrição para a ocorrência {0}", codigo);
        }

        #endregion
    }
}
