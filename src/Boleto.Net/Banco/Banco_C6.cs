using System;
using System.Linq;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.336.jpg", "image/jpg")]

namespace BoletoNet
{
    internal class Banco_C6 : AbstractBanco, IBanco
    {
        #region Construtores

        internal Banco_C6()
        {
            try
            {
                this.Codigo = 336;
                this.Digito = "0";
                this.Nome = "C6 Bank";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion Construtores

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do Banco C6
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            var carteirasImplementadas = new int[] { 10, 20, 21, 22, 23, 24 };
            var possiveisModalidadesIdentificadorLayout = new int[] { 3, 4 };

            if (string.IsNullOrEmpty(boleto.Carteira) || !int.TryParse(boleto.Carteira, out int carteiraInt))
                throw new ArgumentException("Carteira não informada ou inválida.");

            if (!carteirasImplementadas.Contains(carteiraInt))
                throw new ArgumentException(string.Format("Carteira {0} não implementada (Carteiras disponíveis: {1}).", carteiraInt, string.Join(",", carteirasImplementadas)));

            if (string.IsNullOrEmpty(boleto.TipoModalidade) || !int.TryParse(boleto.TipoModalidade, out int tipoModalidadeInt))
                throw new ArgumentException(string.Format("{0} não informada ou inválida para o boleto (Corresponde ao IdentificadorLayout para o C6 Bank).", nameof(boleto.TipoModalidade)));

            if (!possiveisModalidadesIdentificadorLayout.Contains(tipoModalidadeInt))
                throw new ArgumentException(string.Format("Modalidade informada {0} é inválida (Modalidades disponíveis: {1}).", tipoModalidadeInt, string.Join(",", possiveisModalidadesIdentificadorLayout)));

            if (boleto.NossoNumero.Length != 10)
                throw new ArgumentException("Nosso número deve possuir 10 posições");

            if (boleto.Cedente.Codigo.Length != 12)
                throw new NotImplementedException("Código do cedente precisa conter 12 dígitos");

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

        #endregion Métodos de Instância

        #region Métodos de formatação do boleto

        public override void FormataNossoNumero(Boleto boleto)
        {
            // Sem necessidade de formatar, considera o valor recebido.
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            // Sem necessidade de formatar, considera o valor recebido.
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                Utils.FormatCode(Codigo.ToString(), 3),
                boleto.Moeda,
                FatorVencimento(boleto),
                valorBoleto,
                boleto.Cedente.Codigo.PadLeft(12, '0'),
                boleto.NossoNumero.PadLeft(10, '0'),
                boleto.Carteira.PadLeft(2, '0'),
                int.Parse(boleto.TipoModalidade));

            int _dacBoleto = Mod11Peso2a9(boleto.CodigoBarra.Codigo);

            boleto.CodigoBarra.Codigo = boleto.CodigoBarra.Codigo.Substring(0, 4) + _dacBoleto + boleto.CodigoBarra.Codigo.Substring(4, 39);
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string campo1;
            string campo2;
            string campo3;
            string campo4;
            string campo5;
            int digitoMod;

            /*
            Campos 1
            Código do Banco na Câmara de Compensação “336”
            Código da moeda "9" (*)
            Código do Cedente – 5 Posições
            Dígito Verificador Módulo 10 (Campo 1)
             */
            campo1 = String.Concat(Utils.FormatCode(Codigo.ToString(), 3), boleto.Moeda, boleto.Cedente.Codigo.Substring(0, 5));
            digitoMod = Mod10(campo1);
            campo1 += digitoMod.ToString();
            campo1 = campo1.Substring(0, 5) + "." + campo1.Substring(5, 5);

            /*
            Campo 2
            Código do Cedente – 7 Posições
            Nosso número – 3 posições
            Dígito Verificador Módulo 10 (Campo 2)
             */
            campo2 = String.Concat(boleto.Cedente.Codigo.Substring(5, 7), boleto.NossoNumero.Substring(0, 3));
            digitoMod = Mod10(campo2);
            campo2 += digitoMod.ToString();
            campo2 = campo2.Substring(0, 5) + "." + campo2.Substring(5, 6);

            /*
            Campo 3
            Nosso número – 7 posições
            Código da Carteira
            Identificador de Layout
            Dígito Verificador Módulo 10 (Campo 3)
             */
            campo3 = String.Concat(boleto.NossoNumero.Substring(3, 7), boleto.Carteira.PadLeft(2, '0'), boleto.TipoModalidade);
            digitoMod = Mod10(campo3);
            campo3 += digitoMod;
            campo3 = campo3.Substring(0, 5) + "." + campo3.Substring(5, 6);

            /*
            Campo 4
            Dígito Verificador Geral
             */
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

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

        #endregion Métodos de formatação do boleto

        #region Métodos de geração do arquivo remessa - Genéricos

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException();
        }

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException();
        }

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

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException();
        }

        #endregion Métodos de geração do arquivo remessa - Genéricos
    }
}