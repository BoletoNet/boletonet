
using System;
using System.Globalization;
using System.Web.UI;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.655.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Banco Votorantim
    /// </summary>
    internal class Banco_Votorantim : AbstractBanco, IBanco
    {
        #region Construtores

        internal Banco_Votorantim()
        {
            try
            {
                this.Codigo = 655;
                this.Digito = "6";
                this.Nome = "Banco Votorantim";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do Banco Votorantim
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new ArgumentException("Carteira não informada. Utilize a carteira 500.");

            //Verifica as carteiras implementadas - Apenas 500
            if (!boleto.Carteira.Equals("500"))
                throw new ArgumentException("Carteira não parametrizada.");

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length != 10)
                throw new ArgumentException("Nosso número deve possuir 10 posições");

            if (boleto.Cedente.Convenio == 0)
                throw new ArgumentException("Código de convenio não informado.");

            #region Agência e Conta Corrente
            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.Cedente.ContaBancaria.Agencia + ", são de 4 números.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 10)
                throw new ArgumentException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + " é 10 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 10)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 10);
            #endregion Agência e Conta Corrente

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = String.IsNullOrEmpty(boleto.LocalPagamento) ? Nome : boleto.LocalPagamento;

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue) 
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        # endregion

        #region Métodos de formatação do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);


            /*
Posição|Tamanho|Picture|Conteúdo
----------------------------------
01-03|03|9 (03)|Identificação do Banco = 655
04-04|01|9 (01)|Código da moeda = 9 (real)
05-05|01|9 (01)|DV do código de barras (cálculo abaixo)
06-09|04|9 (04)|Fator de vencimento
10-19|10|9 (08)V99|Valor nominal
20-29|10|9 (10)|Código do Convênio padrão Votorantim
30-32|03|9 (03)|Carteira|
33-42|13|9 (10)|Nosso Número com DV
43-44|01|9 (02)|Fixo “00”
             */

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                Utils.FormatCode(Codigo.ToString(), 3),
                boleto.Moeda,
                FatorVencimento(boleto),
                valorBoleto,
                boleto.Cedente.Convenio.ToString().PadLeft(10, '0'),
                boleto.Carteira.PadLeft(3, '0'),
                boleto.NossoNumero,
                "00");

            int _dacBoleto = Mod11Peso2a9(boleto.CodigoBarra.Codigo);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;
            int digitoMod = 0;

            /*
            Campos 1
            A = Código do Banco na Câmara de Compensação “655”
            B = Código da moeda "9" (*)
            C = Código do Convênio Banco Votorantim
            X = DV que amarra o campo 1 (Módulo 10, contido no Anexo 7)
             */
            campo1 = String.Concat(Utils.FormatCode(Codigo.ToString(), 3), boleto.Moeda, boleto.Cedente.Convenio.ToString().PadLeft(10, '0').Substring(0, 5));
            digitoMod = Mod10(campo1);
            campo1 = campo1 + digitoMod.ToString();
            campo1 = Strings.Mid(campo1, 1, 5) + "." + Strings.Mid(campo1, 6, 5);

            /*
            Campo 2
            9 (05) Código do Convênio Banco Votorantim
            9 (03) Carteira de registro
            9 (02) Nosso Numero
            9 (01) Código verificador do primeiro grupo
             */
            campo2 = String.Concat(boleto.Cedente.Convenio.ToString().PadLeft(10, '0').Substring(5, 5), boleto.Carteira.PadLeft(3, '0'), boleto.NossoNumero.Substring(0, 2));
            digitoMod = Mod10(campo2);
            campo2 = campo2 + digitoMod.ToString();
            campo2 = Strings.Mid(campo2, 1, 5) + "." + Strings.Mid(campo2, 6, 6);


            /*
            Campo 3
            9 (08) Nosso Numero com DV
            9 (02) Fixo “00”
            9 (01) Código verificador do primeiro grupo
             */
            campo3 = String.Concat(boleto.NossoNumero.Substring(2, 8), "00");
            digitoMod = Mod10(campo3);
            campo3 = campo3 + digitoMod;
            campo3 = Strings.Mid(campo3, 1, 5) + "." + Strings.Mid(campo3, 6, 6);

            /*
            Campo 4
            9 (01) Dígito Verificador do Código de Barras
             */
            campo4 = Strings.Mid(boleto.CodigoBarra.Codigo, 5, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do Título (*)
             */
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);
            campo5 = String.Concat(FatorVencimento(boleto).ToString(), valorBoleto);
            

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        /// <summary>
        /// Formata o nosso número para ser mostrado no boleto.
        /// </summary>
        /// <remarks>
        /// Última a atualização por Transis em 26/09/2011
        /// </remarks>
        /// <param name="boleto"></param>
        public override void FormataNossoNumero(Boleto boleto)
        {
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
        }

        # endregion

        #region Métodos de geração do arquivo remessa - Genéricos
        /// <summary>
        /// HEADER DE LOTE do arquivo CNAB
        /// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            throw new NotImplementedException();
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            throw new NotImplementedException();
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            throw new NotImplementedException();
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CNAB240 - Específicos
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            throw new NotImplementedException();
        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            throw new NotImplementedException();
        }

    }
}
#endregion