using System;
using System.Web.UI;
using BoletoNet;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.422.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// Diogo
    /// Miamoto
    /// Klesse
    /// </author>    

    ///<summary>
    /// Classe referente ao Banco Banco_Safra
    ///</summary>
    internal class Banco_Safra : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacContaCorrente = 0;
        private int _numeroArquivoRemessa;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Safra.
        /// </summary>
        internal Banco_Safra()
        {
            this.Codigo = 422;
            this.Digito = "7";
            this.Nome = "Banco_Safra";
        }


        #region IBanco Members

        #region Header Remessa

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";
                _numeroArquivoRemessa = numeroArquivoRemessa;
                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240();
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


        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var numeroArquivoRemessa3Digitos = numeroArquivoRemessa.ToString().PadLeft(3, '0');
                numeroArquivoRemessa3Digitos = numeroArquivoRemessa3Digitos.Substring(numeroArquivoRemessa3Digitos.Length - 3);


                var reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 008, 0, "COBRANCA", ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 007, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 005, 0, cedente.ContaBancaria.Agencia, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediInteiro______________, 0032, 008, 0, cedente.ContaBancaria.Conta, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0040, 001, 0, cedente.ContaBancaria.DigitoConta, '0'));

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0041, 006, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 003, 0, "422", ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 011, 0, "BANCO SAFRA", ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0091, 004, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 291, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 003, 0, numeroArquivoRemessa3Digitos, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, "000001", '0'));
                reg.CodificarLinha();

                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _header;


            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        #region Detalhes Remessa
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
            throw new NotImplementedException("Função não implementada");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);


                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf é sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj é sempre 14;



                var numeroArquivoRemessa3Digitos = _numeroArquivoRemessa.ToString().PadLeft(3, '0');
                numeroArquivoRemessa3Digitos = numeroArquivoRemessa3Digitos.Substring(numeroArquivoRemessa3Digitos.Length - 3);


                var reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, vCpfCnpjEmi, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 005, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediInteiro______________, 0023, 008, 0, boleto.Cedente.ContaBancaria.Conta, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0031, 001, 0, boleto.Cedente.ContaBancaria.DigitoConta, '0'));


                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 006, 0, string.Empty, ' '));

                

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, boleto.NumeroDocumento, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 009, 0, boleto.NossoNumero, '0'));

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0072, 030, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0102, 001, 0, "0", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0103, 002, 0, "0", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0105, 001, 0, string.Empty, ' '));

                string diasProtesto = string.Empty;
                string vInstrucao1 = "0";
                string vInstrucao2 = "0";

                if (boleto.Instrucoes.Count > 0)
                {
                    //vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                    diasProtesto = boleto.Instrucoes[0].QuantidadeDias.ToString().PadLeft(2, '0');
                    vInstrucao2 = boleto.Instrucoes[0].Codigo.ToString();
                }
                if (boleto.Instrucoes.Count > 1)
                {
                    vInstrucao1 = boleto.Instrucoes[1].Codigo.ToString();
                }

                if (vInstrucao2 != "10")
                {
                    diasProtesto = string.Empty;
                }

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0106, 002, 0, diasProtesto, '0')); // era 107


                switch (boleto.Carteira)
                {
                    case "01":
                        reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0108, 001, 0, "1", '0'));
                        break;
                    case "02":
                        reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0108, 001, 0, "2", '0'));
                        break;
                    default:
                        reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0108, 001, 0, "?", '0'));
                        break;
                }

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0109, 002, 0, "01", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "422", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 005, 0, "0", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0148, 002, 0, boleto.EspecieDocumento.Codigo.ToString(), '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, vInstrucao2, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));

                if (boleto.ValorDesconto == 0)
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, "0", '0')); // Sem Desconto
                else
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0174, 006, 0, boleto.DataDesconto, '0')); // Com Desconto

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));

                if (vInstrucao1 == "16")
                {
                    // Conforme manual, item 6.1.8, quando operação for "entrada de título" e a primeira ocorrência for "16", utiliza campo para informar Multa
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0206, 006, 0, boleto.DataMulta, ' '));
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0212, 004, 2, boleto.PercMulta, '0'));
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0216, 003, 0, "000", '0'));
                }
                else
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));


                string vCpfCnpjPag = "00";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjPag = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjPag = "02"; //Cnpj é sempre 14;

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, vCpfCnpjPag, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, FormataLogradouro(boleto.Sacado.Endereco, 40), ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 010, 0, boleto.Sacado.Endereco.Bairro, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0325, 002, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP.Replace("-", ""), '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF, ' '));
                //if (boleto.Sacado != null)
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 030, 0, boleto.Sacado.Nome, ' '));
                //else
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 030, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0382, 007, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0389, 003, 0, "422", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 003, 0, numeroArquivoRemessa3Digitos, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));
                reg.CodificarLinha();
                return reg.LinhaRegistro;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }


        }

        public string FormataLogradouro(Endereco endereco, int tamanhoFinal)
        {
            var logradouroCompleto = string.Empty;
            if (!string.IsNullOrEmpty(endereco.Numero))
                logradouroCompleto += " " + endereco.Numero;
            if (!string.IsNullOrEmpty(endereco.Complemento))
                logradouroCompleto += " " + (endereco.Complemento.Length > 20 ? endereco.Complemento.Substring(0, 20) : endereco.Complemento);

            if (tamanhoFinal == 0)
                return endereco.Logradouro + logradouroCompleto;

            if (endereco.Logradouro.Length + logradouroCompleto.Length <= tamanhoFinal)
                return endereco.Logradouro + logradouroCompleto;

            return endereco.Logradouro.Substring(0, tamanhoFinal - logradouroCompleto.Length) + logradouroCompleto;
        }
        #endregion


        #region Trailer
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
                        _trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal, cedente);
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

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada");
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal, Cedente cedente)
        {
            try
            {
                var numeroArquivoRemessa3Digitos = _numeroArquivoRemessa.ToString().PadLeft(3, '0');
                numeroArquivoRemessa3Digitos = numeroArquivoRemessa3Digitos.Substring(numeroArquivoRemessa3Digitos.Length - 3);

                var reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "9", '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 367, 0, string.Empty, ' '));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0369, 008, 0, numeroRegistro - 2, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0377, 015, 2, vltitulostotal, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 003, 0, numeroArquivoRemessa3Digitos, '0'));
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }

        }
        #endregion


        #region Métodos de processamento do arquivo retorno CNAB400



        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno(registro);
                header.TipoRegistro = Utils.ToInt32(registro.Substring(000, 1));
                header.CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1));
                header.LiteralRetorno = registro.Substring(002, 7);
                header.CodigoServico = Utils.ToInt32(registro.Substring(009, 2));
                header.LiteralServico = registro.Substring(011, 8);
                header.Agencia = Utils.ToInt32(registro.Substring(026, 5));

                header.Conta = Utils.ToInt32(registro.Substring(031, 8));

                header.DACConta = Utils.ToInt32(registro.Substring(038, 1));

                header.NomeEmpresa = registro.Substring(046, 30);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##"));

                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(391, 5));
                header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                return header;
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
                var detalhe = new DetalheRetorno();

                detalhe.Agencia = Utils.ToInt32(registro.Substring(017, 5));
                detalhe.Conta = Utils.ToInt32(registro.Substring(022, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(030, 1));
                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);

                //Carteira
                detalhe.Carteira = registro.Substring(107, 1);


                //Identificação do Título no Banco
                detalhe.NossoNumero = registro.Substring(62, 8);
                detalhe.DACNossoNumero = registro.Substring(70, 1);
                detalhe.NossoNumeroComDV = detalhe.NossoNumero + registro.Substring(70, 1); //DV

                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Convert.ToInt32(registro.Substring(108, 2));
                detalhe.DescricaoOcorrencia = DescricaoOcorrenciaCnab400(detalhe.CodigoOcorrencia);


                //Número do Documento
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                detalhe.Especie = AjustaEspecieCnab400(registro.Substring(173, 2));

                //Valores do Título
                detalhe.ValorTitulo = Convert.ToDecimal(registro.Substring(152, 13)) / 100;
                detalhe.TarifaCobranca = Convert.ToDecimal(registro.Substring(175, 13)) / 100;
                detalhe.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(188, 13)) / 100;
                detalhe.IOF = Convert.ToDecimal(registro.Substring(214, 13)) / 100;
                detalhe.ValorAbatimento = Convert.ToDecimal(registro.Substring(227, 13)) / 100;
                detalhe.Descontos = Convert.ToDecimal(registro.Substring(240, 13)) / 100;
                detalhe.ValorPago = Convert.ToDecimal(registro.Substring(253, 13)) / 100;
                detalhe.JurosMora = Convert.ToDecimal(registro.Substring(266, 13)) / 100;
                detalhe.OutrosCreditos = Convert.ToDecimal(registro.Substring(279, 13)) / 100;

                //Data Ocorrência no Banco
                detalhe.DataOcorrencia = Utils.ToDateTime(Utils.ToInt32(registro.Substring(110, 6)).ToString("##-##-##"));

                //Data Vencimento do Título
                detalhe.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(146, 6)).ToString("##-##-##"));

                // Data do Crédito
                detalhe.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(295, 6)).ToString("##-##-##"));

                //// Registro Retorno
                //detalhe.Registro = detalhe.Registro + registro + Environment.NewLine;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }



        private int AjustaEspecieCnab400(string codigoEspecie)
        {
            switch (codigoEspecie)
            {
                case "01":
                    return 1;
                case "02":
                    return 2;
                case "03":
                    return 3;
                case "05":
                    return 5;
                case "09":
                    return 9;
                default:
                    return 1;
            }
        }

        private string DescricaoOcorrenciaCnab400(int codigo)
        {
            switch (codigo)
            {
                case 2:
                    return "Entrada Confirmada";
                case 3:
                    return "Entrada Rejeitada";
                case 4:
                    return "Transferência de Carteira (Entrada)";
                case 5:
                    return "Transferência de Carteira (Baixa)";
                case 6:
                    return "Liquidação normal";
                case 9:
                    return "Baixado Automaticamente";
                case 10:
                    return "Baixado conforme instruções";
                case 11:
                    return "Títulos em Ser (Para Arquivo Mensal)";
                case 12:
                    return "Abatimento Concedido";
                case 13:
                    return "Abatimento Cancelado";
                case 14:
                    return "Vencimento Alterado";
                case 15:
                    return "Liquidação em Cartório";
                case 19:
                    return "Confirmação de instrução de protesto";
                case 20:
                    return "Confirmação de sustar protesto";
                case 21:
                    return "Transferência de beneficiário";
                case 23:
                    return "Título enviado a cartório";
                case 40:
                    return "Baixa de Título Protestado";
                case 41:
                    return "Liquidação de Título Baixado";
                case 42:
                    return "Título retirado do cartório";
                case 43:
                    return "Despesa de cartório";
                case 44:
                    return "Aceite do título DDA pelo pagador";
                case 45:
                    return "Não aceite do título DDA pelo pagador";
                case 51:
                    return "Valor do título alterado";
                case 52:
                    return "Acerto de Data de emissão";
                case 53:
                    return "Acerto de código de espécie de documento";
                case 54:
                    return "Alteração de seu número";
                case 56:
                    return "Instrução de negativação aceita";
                case 57:
                    return "Instrução de baixa de negativação aceita";
                case 58:
                    return "Instrução não negativar aceita";
                default:
                    return "";
            }
        }

        #endregion


        public override void ValidaBoleto(Boleto boleto)
        {

            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);

            // Calcula o DAC da Conta Corrente
            _dacContaCorrente = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta);

            //Verifica se o nosso número é válido
            if (Utils.ToInt64(boleto.NossoNumero) == 0)
                throw new NotImplementedException("Nosso número inválido");

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.LocalPagamento = "Pagável em qualquer Banco do Sistema de Compensação";


            FormataNumeroDocumento(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
        }



        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            string sfCarteira = boleto.Carteira.ToString();


            if (boleto.NossoNumero.Length < 9)
            {
                throw new IndexOutOfRangeException("Erro. O campo 'Nosso Número' deve ter mais de 9 digitos. Você digitou " + boleto.NossoNumero);
            }
            string sfNossoNumero = boleto.NossoNumero.Substring(0, 8);
            int sfDigitoNossoNumero = Mod11(sfNossoNumero, 9, 0);
            string sfDigito = "";

            if (sfDigitoNossoNumero == 0)
                sfDigito = "1";
            else if (sfDigitoNossoNumero > 1)
                sfDigito = Convert.ToString(11 - sfDigitoNossoNumero);

            return sfDigito;

        }


        /// <summary>       
        /// SISTEMA	        020	020	7	FIXO
        /// CLIENTE	        021	034	CÓDIGO DO CLIENTE	CÓDIGO/AGÊNCIA CEDENTE
        /// N/NÚMERO	    035	043	NOSSO NÚMERO	NOSSO NÚMERO DO TÍTULO
        /// TIPO COBRANÇA	044	044	2	FIXO
        /// </summary>
        public string CampoLivre(Boleto boleto)
        {

            //string campolivre = "7" + boleto.Cedente.ContaBancaria.Conta.ToString() + boleto.Cedente.ContaBancaria.Agencia.ToString() +
            //                    boleto.NossoNumero.Substring(0, 9) + "2";
            //return campolivre;

            return boleto.Banco.Digito + boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.NossoNumero + "2";

        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            boleto.Cedente.ContaBancaria.Agencia = Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
            boleto.Cedente.ContaBancaria.Conta = Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
            boleto.Cedente.ContaBancaria.Conta += boleto.Cedente.ContaBancaria.DigitoConta;
            boleto.Cedente.ContaBancaria.DigitoAgencia = ""; boleto.Cedente.ContaBancaria.DigitoConta = "";
            boleto.Cedente.Nome += " " + boleto.Cedente.CPFCNPJ;




        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            boleto.NossoNumero = Utils.FitStringLength(boleto.NossoNumero, 9, 9, '0', 1, true, true, true);

        }

        /// <summary>
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05  1 - Dígito verificador do Código de Barras
        ///    06 a 19 - 14 - Valor
        ///    20 a 44  25 - Campo Livre
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            //string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            //valorBoleto = Utils.FormatCode(valorBoleto, 14);

            //boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo, boleto.Moeda,
            //        FatorVencimento(boleto), valorBoleto, CampoLivre(boleto));

            //_dacBoleto = 0;
            ////Mod11(Boleto.CodigoBarra.Codigo.Substring(0, 3) + Boleto.CodigoBarra.Codigo.Substring(5, 43), 9, 0);

            //boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);

            var banco = boleto.Banco;
            var codigobarra = boleto.CodigoBarra;
            codigobarra.CampoLivre = CampoLivre(boleto);

            if (codigobarra.CampoLivre.Length != 25)
                throw new Exception("campo livre (" + codigobarra.CampoLivre + ") deve conter 25 dígitos.");

            // formata código de barras do boleto
            codigobarra.CodigoBanco = Utils.FitStringLength(banco.Codigo.ToString(), 3, 3, '0', 0, true, true, true);
            codigobarra.Moeda = boleto.Moeda;
            codigobarra.FatorVencimento = FatorVencimento(boleto);
            codigobarra.ValorDocumento = boleto.ValorBoleto.ToString("n2").Replace(",", "").Replace(".", "").PadLeft(10, '0');


            string codigoSemDv = string.Format("{0}{1}{2}{3}{4}",
                                                       codigobarra.CodigoBanco,
                                                      codigobarra.Moeda,
                                                       codigobarra.FatorVencimento,
                                                       codigobarra.ValorDocumento,
                                                                                       codigobarra.CampoLivre);





            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}",
                                    codigoSemDv.Left(4),
                                   GerarDACBarrasSafra(codigoSemDv),
                                    codigoSemDv.Right(39));

            //boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);

        }


        private string GerarDACBarrasSafra(string codigoSemDv)
        {
            int[] peso = { 4, 3, 2, 9, 8, 7, 6, 5 };
            int indexPeso = 0;
            int soma = 0;
            for (int i = 0; i < (codigoSemDv.Length); i++)
            {
                soma = soma + (Convert.ToInt32(codigoSemDv.Substring(i, 1)) * peso[indexPeso]);


                if (indexPeso == 7)
                    indexPeso = 0;
                else
                    indexPeso++;
            }
            var resto = (soma % 11);

            if (resto <= 1 || resto > 9)
                return "1";

            return (11 - resto).ToString();
        }
        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///    1º CAMPO - Composto pelo código do banco ( sem o dígito verificador = 422 ), 
        ///       código da moeda, as cinco primeiras posições do campo livre ou seja, da 
        ///       posição 20 à 24 do código de barras, e mais um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ). 
        ///    2º CAMPO - Composto pelas posições 6 à 15 do campo livre ou seja, da 
        ///       posição 25 à 34 do código de barras e mais um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ).
        ///    3º CAMPO - Composto pelas posições 16 à 25 do campo livre ou seja, da 
        ///       posição 35 à 44 do código de barras, e mais um dígito verificador deste campo. 
        ///       Após os 5 primeiros dígitos deste campo separar o conteúdo por um ponto ( . ).
        ///    4º CAMPO  - Composto pelo dígito de autoconferência do código de barras.
        ///    5º CAMPO - Composto pelo valor nominal do documento ou seja, pelas 
        ///       posições 06 à 19 do código de barras, com supressão de zeros a esquerda e 
        ///       sem edição ( sem ponto e vírgula ). Quando se tratar de valor zerado, a 
        ///       representação deverá ser 000 ( três zeros ).
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            ////AAABC.CCCCX DDDDD.DDDDDY EEEEE.EEEEEZ K VVVVVVVVVVVVVV

            //string LD = string.Empty; //Linha Digitável

            //#region Campo 1

            ////Campo 1
            //string AAA = Utils.FormatCode(Codigo.ToString(), 3);
            //string B = boleto.Moeda.ToString();
            //string CCCCC = CampoLivre(boleto).Substring(0, 4);
            //string X = Mod10(AAA + B + CCCCC).ToString();

            //LD = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
            //LD += string.Format("{0}{1}", CCCCC.Substring(0, 4), X);

            //#endregion Campo 1

            //#region Campo 2

            //string DDDDDD = CampoLivre(boleto).Substring(6, 15);
            //string Y = Mod10(DDDDDD).ToString();

            //LD += string.Format("{0}.", DDDDDD.Substring(0, 5));
            //LD += string.Format("{0}{1} ", DDDDDD.Substring(5, 10), Y);

            //#endregion Campo 2

            //#region Campo 3

            //string EEEEE = CampoLivre(boleto).Substring(12, 10);
            //string Z = Mod10(EEEEE).ToString();

            //LD += string.Format("{0}.", EEEEE.Substring(0, 5));
            //LD += string.Format("{0}{1} ", EEEEE.Substring(5, 5), Z);

            //#endregion Campo 3

            //#region Campo 4

            //string K = _dacBoleto.ToString();

            //LD += string.Format(" {0} ", K);

            //#endregion Campo 4

            //#region Campo 5
            //string VVVVVVVVVVVVVV;
            //if (boleto.ValorBoleto != 0)
            //{
            //    VVVVVVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            //    VVVVVVVVVVVVVV = Utils.FormatCode(VVVVVVVVVVVVVV, 14);
            //}
            //else
            //    VVVVVVVVVVVVVV = "000";

            //LD += VVVVVVVVVVVVVV;

            //#endregion Campo 5

            //boleto.CodigoBarra.LinhaDigitavel = LD;


            var codigoBarra = boleto.CodigoBarra;
            if (string.IsNullOrWhiteSpace(codigoBarra.CampoLivre))
            {
                codigoBarra.LinhaDigitavel = "";
                return;
            }
            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV

            var codigoDeBarras = codigoBarra.Codigo;

            #region Campo 1

            // POSIÇÃO 1 A 3 DO CODIGO DE BARRAS
            var bbb = codigoDeBarras.Substring(0, 3);
            // POSIÇÃO 4 DO CODIGO DE BARRAS
            var m = codigoDeBarras.Substring(3, 1);
            // POSIÇÃO 20 A 24 DO CODIGO DE BARRAS
            var ccccc = codigoDeBarras.Substring(19, 5);
            // Calculo do Dígito

            string d1;
            if (boleto.VariacaoCarteira == "1")
                d1 = CalcularDvModulo10SafraNovo(bbb + m + ccccc);
            else
                d1 = CalcularDvModulo10Safra(bbb + m + ccccc);

            // Formata Grupo 1
            var grupo1 = bbb + m + ccccc.Substring(0, 1) + "." + ccccc.Substring(1, 4) + d1 + " ";

            #endregion Campo 1

            #region Campo 2

            //POSIÇÃO 25 A 34 DO COD DE BARRAS
            var d2A = codigoDeBarras.Substring(24, 10);
            // Calculo do Dígito
            string d2B;
            if (boleto.VariacaoCarteira == "1")
                d2B = CalcularDvModulo10SafraNovo(d2A).ToString();
            else
                d2B = CalcularDvModulo10Safra(d2A).ToString();

            // Formata Grupo 2
            var grupo2 = d2A.Substring(0, 5) + "." + d2A.Substring(5, 5) + d2B + " ";

            #endregion Campo 2

            #region Campo 3

            //POSIÇÃO 35 A 44 DO CODIGO DE BARRAS
            var d3A = codigoDeBarras.Substring(34, 10);
            // Calculo do Dígito
            var d3B = Mod10(d3A).ToString();
            // Formata Grupo 3
            var grupo3 = d3A.Substring(0, 5) + "." + d3A.Substring(5, 5) + d3B + " ";

            #endregion Campo 3

            #region Campo 4

            // Dígito Verificador do Código de Barras
            var grupo4 = codigoBarra.DigitoVerificador + " ";

            #endregion Campo 4

            #region Campo 5

            //POSICAO 6 A 9 DO CODIGO DE BARRAS
            var d5A = codigoDeBarras.Substring(5, 4);
            //POSICAO 10 A 19 DO CODIGO DE BARRAS
            var d5B = codigoDeBarras.Substring(9, 10);
            // Formata Grupo 5
            var grupo5 = d5A + d5B;

            #endregion Campo 5

            codigoBarra.LinhaDigitavel = grupo1 + grupo2 + grupo3 + grupo4 + grupo5;

        }
        private static string CalcularDvModulo10SafraNovo(string texto)
        {
            int soma = 0, peso = 2;

            for (var i = texto.Length - 1; i >= 0; i--)
            {


                var result = Convert.ToInt32(texto.Substring(i, 1)) * peso;
                if (result > 9 && peso == 2)
                {
                    result = result - 9;
                }

                soma += result;

                if (peso == 2)
                    peso = 1;
                else
                    peso = peso + 1;
            }

            var resto = (soma % 10);

            if (resto == 0)
                return "0";

            return (10 - resto).ToString();

        }
        private static string CalcularDvModulo10Safra(string texto)
        {
            int soma = 0, peso = 2;

            for (var i = texto.Length - 1; i >= 0; i--)
            {


                var result = Convert.ToInt32(texto.Substring(i, 1)) * peso;
                if (result > 9 && peso == 2)
                {
                    result = result - 9;
                }

                soma += result;

                if (peso == 2)
                    peso = 1;
                else
                    peso = peso + 1;
            }

            var resto = (soma % 10);

            if (resto <= 1)
                return "0";

            return (10 - resto).ToString();

        }
        #endregion IBanco Members


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
