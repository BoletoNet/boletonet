
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;
using System.Collections.Generic;

[assembly: WebResource("BoletoNet.Imagens.004.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Banco do Nordeste
    /// </summary>
    internal class Banco_Nordeste : AbstractBanco, IBanco
    {

        #region Variáveis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal Banco_Nordeste()
        {
            try
            {
                this.Codigo = 4;
                this.Digito = "3";
                this.Nome = "BANCO DO NORDESTE DO BRASIL S.A.";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do Banco do Brasil
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new NotImplementedException("Carteira não informada. Utilize a carteira 4, 5, 6, I.");            

            boleto.QuantidadeMoeda = 0;

            boleto.NossoNumero = boleto.NossoNumero.PadLeft(7, '0');
            boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);
            boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);
            boleto.Cedente.ContaBancaria.DigitoConta = Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 1);

            if(string.IsNullOrEmpty(boleto.DigitoNossoNumero))
                boleto.DigitoNossoNumero = Mod11(boleto.NossoNumero, 8).ToString();

            boleto.DigitoNossoNumero = Utils.FormatCode(boleto.DigitoNossoNumero, 1);

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        # endregion

        #region Métodos de formatação do boleto

        private string CampoLivre(Boleto boleto)
        {
            string CampoZerado = "000";
            string campoLivre = string.Format("{0}{1}{2}{3}{4}{5}{6}"
                , Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4)
                , Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7)
                , Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 1)
                , Utils.FormatCode(boleto.NossoNumero, 7)
                , Utils.FormatCode(boleto.DigitoNossoNumero, 1)
                , FormataCarteira(boleto.Carteira)
                , CampoZerado);
            return campoLivre;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {/*
            var campoLivre = string.Empty;
            var campo1     = string.Empty;
            var campo2     = string.Empty;
            var campo3     = string.Empty;
            var campo5     = string.Empty;
            
            var banco = Utils.FormatCode(Codigo.ToString(),3);            
            var moeda = "9";
            var fatorVencimento = FatorVencimento(boleto);
            var valorDocumento = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = Utils.FormatCode(valorDocumento, 10);
            
            campoLivre = CampoLivre(boleto);

            campo1 = string.Format("{0}{1}{2}",
               banco,
               moeda,
               campoLivre.Substring(0,5)//Primeiros 5 digitos do campo livre               
            );

            int divCampo1 = Mod10(campo1);//Digito verificador do campo1
            
            campo1 += divCampo1;

            campo2 = campoLivre.Substring(5, 10);
            int divCampo2 = Mod10(campo2);
            campo2 += divCampo2;

            campo3 = campoLivre.Substring(15, 10);
            int divCampo3 = Mod10(campo3);
            campo3 += divCampo3;

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}",
               campo1,
               campo2,
               campo3
            );

            campo5 = string.Format("{0}{1}",
               fatorVencimento,               
               valorDocumento               
            );

            string campo4 = Mod11Base9(boleto.CodigoBarra.Codigo + campoLivre).ToString();
            
            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}",
               campo1,
               
               campo2,
               
               campo3,
               
               campo4,
               campo5
            );*/
            

            var banco = Utils.FormatCode(Codigo.ToString(), 3);
            var moeda = "9";
            var fatorVencimento = FatorVencimento(boleto);
            var valorDocumento = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorDocumento = Utils.FormatCode(valorDocumento, 10);
            
            
            string CampoZerado = "000";
            
            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}",
               banco,
               moeda,
               fatorVencimento,
               valorDocumento,               
               CampoLivre(boleto)
            );

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);

        }


        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>       

        public override void FormataLinhaDigitavel(Boleto boleto)
        {/*
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;

            #region Campo 1
            campo1 = boleto.CodigoBarra.Codigo.Substring(0, 5) + "." + boleto.CodigoBarra.Codigo.Substring(5, 5);
            #endregion Campo 1

            #region Campo 2
            campo2 = boleto.CodigoBarra.Codigo.Substring(10, 5) + "." + boleto.CodigoBarra.Codigo.Substring(15, 6);
            #endregion Campo 2

            #region Campo 3
            campo3 = boleto.CodigoBarra.Codigo.Substring(21, 5) + "." + boleto.CodigoBarra.Codigo.Substring(26, 6);
            #endregion Campo 3

            #region Campo 4
            campo4 = boleto.CodigoBarra.Codigo.Substring(32, 1);
            #endregion Campo 4

            #region Campo 5
            campo5 = boleto.CodigoBarra.Codigo.Substring(33, 14);
            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4+" "+campo5;
          */
            var campoLivre = CampoLivre(boleto);

            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;

            #region Campo 1
            campo1 = boleto.CodigoBarra.Codigo.Substring(0, 4) + boleto.CodigoBarra.Codigo.Substring(19, 5);
            campo1 += Mod10(campo1);
            campo1 = campo1.Insert(5, ".");
            #endregion Campo 1

            #region Campo 2
            campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            campo2 += Mod10(campo2);
            campo2 = campo2.Insert(5, ".");
            #endregion Campo 2

            #region Campo 3
            campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            campo3 += Mod10(campo3);
            campo3 = campo3.Insert(5, ".");
            #endregion Campo 3

            #region Campo 4
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            #endregion Campo 4

            #region Campo 5
            campo5 = boleto.CodigoBarra.Codigo.Substring(5, 4) + boleto.CodigoBarra.Codigo.Substring(9,10);
            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}-{1}", Utils.FormatCode(boleto.NossoNumero, 7), boleto.DigitoNossoNumero);
            
        }

        # endregion

        #region Métodos de geração do arquivo remessa - Genéricos
        /// <summary>
        /// HEADER DE LOTE do arquivo CNAB
        /// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }
        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            throw new NotImplementedException("Remessa não implementada!");       
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region CNAB240 - Específicos
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");              
        }
        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
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

            throw new NotImplementedException("Função não implementada.");
        }
        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        internal static string Mod11BancoNordeste(string value)
        {
            throw new NotImplementedException("Função não implementada.");
        }



        #region CNAB 400 - Específicos sidneiklein
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        #region Métodos de processamento do arquivo retorno CNAB400
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #endregion
        private string FormataCarteira(string carteira)
        {            
            switch (carteira)
            {
                case "4"://Cobranca Simples - Boleto Emitido Pelo Cliente 
                    return "21";
                    break;
                case "5"://Cobranca Vinculada - Boleto Emitido Pelo Cliente
                    return "41";
                case "6"://Cobranca Caucionada - Boleto Emitido Pelo Cliente
                    return "31";
                case "I"://Cobranca Simplificada(Sem Registro)
                    return "51";
                    break;
                default:
                    throw new Exception("Carteira nao implementada");
                    break;
            }

        }
    }


}
