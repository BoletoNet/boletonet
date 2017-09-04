using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet
{
    /// <author>  
    /// Iago Rayner Moura (iago.rmoura@hotmail.com)
    /// </author>  
    internal class Banco_Mercantil : AbstractBanco, IBanco
    {
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Mercantil.
        /// </summary>
        internal Banco_Mercantil()
        {
            this.Codigo = 389;
            this.Nome = "Mercantil";
        }

        /// <summary>
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda (9-Real)
        ///    05 a 05 – 1 - Dígito verificador geral do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor do documento
        ///    20 a 44 – 25 - Campo Livre
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
            FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, agência do beneficiário, primeiro byte do nosso número e dígito verificador (calculado MOD.10);
        ///      2º campo
        ///          composto pelo nosso número a partir do segundo byte inclusive e dígito verificador (calculado MOD.10);
        ///      3º campo
        ///          composto pelo número do contrato, indicador de desconto (constante e igual a 2) e dígito verificador (calculado MOD.10);
        ///      4º campo
        ///          composto pelo DAC - dígito de auto conferência (calculado MOD.11);
        ///      5º campo
        ///          Composto pelo fator de vencimento e valor nominal do título.
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

            string FFFF = FatorVencimento(boleto).ToString();

            string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Beneficiária (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    24 a 34 - 11 - Nosso Número (Sem o digito verificador)
        ///    35 a 43 -  7 - Código do Beneficiário (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Indicador de Desconto (2 = Sem Desconto, 0 = Com Desconto)            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {
            string codCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 9);

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}", boleto.Cedente.ContaBancaria.Agencia, boleto.NossoNumero, codCedente.Substring(0, 9), "2");

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }


        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero.Substring(0, 10), boleto.NossoNumero.Substring(10, 1));
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.ValorBoleto == 0)
                throw new NotImplementedException("O valor do boleto não pode ser igual a zero");

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
                throw new NotImplementedException("A quantidade de dígitos do nosso número, são 11 números.");
            else if (boleto.NossoNumero.Length < 6)
                boleto.NumBoleta = boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 6);
            if (boleto.NossoNumero.Length == 6)
            {
                string nn = string.Format("05{0}{1}", boleto.Carteira, boleto.NossoNumero);
                boleto.NumBoleta = boleto.NossoNumero = string.Format("{0}{1}", nn, Mod11Mercantil(string.Format("{0}{1}", boleto.Cedente.ContaBancaria.Agencia, nn), 9));
            }

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
            {
                string agenciaDesconfigurada = boleto.Cedente.ContaBancaria.Agencia;
                boleto.Cedente.ContaBancaria.Agencia = agenciaDesconfigurada.Substring(0, agenciaDesconfigurada.Length - 1);
                boleto.Cedente.ContaBancaria.DigitoAgencia = agenciaDesconfigurada.Substring(agenciaDesconfigurada.Length - 1, 1);
            }
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 9)
            {
                string conta = boleto.Cedente.ContaBancaria.Conta;

                boleto.Cedente.ContaBancaria.Conta = conta.Substring(0, 9);
                boleto.Cedente.ContaBancaria.DigitoConta = conta.Substring(conta.Length - 1, 1);
            }
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 9)
            {
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 9);
            }

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
            if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataCodAgenciaConta(boleto);
        }

        private void FormataCodAgenciaConta(Boleto boleto)
        {
            boleto.AgenciaCodCedente = string.Format("{0}/{1}{2}",
               boleto.Cedente.ContaBancaria.Agencia,
               Utils.FormatCode(boleto.Cedente.Codigo.Substring(0, boleto.Cedente.Codigo.Length - 1), 8),
               boleto.Cedente.Codigo.Substring(boleto.Cedente.Codigo.Length - 1));
        }
        private string Mod11Mercantil(string seq, int b)
        {
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
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);
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
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                detalhe.Agencia = registro.Substring(17, 4);
                detalhe.Conta = registro.Substring(21, 7);
                detalhe.NumeroControle = registro.Substring(37, 25);
                detalhe.NossoNumero = registro.Substring(66, 11);
                detalhe.DACNossoNumero = Utils.ToInt32(registro.Substring(76, 1));
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                detalhe.DataOcorrencia = new DateTime(Int32.Parse(registro.Substring(114, 2)) + 2000,
                                                    Int32.Parse(registro.Substring(112, 2)),
                                                    Int32.Parse(registro.Substring(110, 2)), 0, 0, 0);
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                double valorTitulo = Convert.ToInt64(registro.Substring(152, 11));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.BancoCobrador = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                double tarifaCobranca = Convert.ToInt64(registro.Substring(175, 11));
                detalhe.TarifaCobranca = tarifaCobranca / 100;
                double outrasDespesas = Convert.ToInt64(registro.Substring(188, 11));
                detalhe.OutrasDespesas = outrasDespesas / 100;
                double juros = Convert.ToInt64(registro.Substring(201, 11));
                detalhe.Juros = juros / 100;
                double IOF = Convert.ToInt64(registro.Substring(214, 11));
                detalhe.IOF = IOF / 100;
                double valorAbatimento = Convert.ToInt64(registro.Substring(227, 11));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                double descontos = Convert.ToInt64(registro.Substring(240, 11));
                detalhe.Descontos = descontos / 100;
                double valorPago = Convert.ToInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                detalhe.ValorPrincipal = detalhe.ValorPago;
                double jurosMora = Convert.ToInt64(registro.Substring(266, 11));
                detalhe.JurosMora = jurosMora / 100;
                double outrosCreditos = Convert.ToInt64(registro.Substring(279, 11));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTimeInvariantCulture(dataCredito);
                detalhe.Instrucao = Utils.ToInt32(registro.Substring(333, 2));
                detalhe.MotivosRejeicao = registro.Substring(377, 10);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.NomeSacado = "";

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public DetalheRetornoCNAB120 LerDetalheRetornoCNA120(string registro)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        {
            throw new NotImplementedException();
        }
    }
}
