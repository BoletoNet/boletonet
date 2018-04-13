using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using BoletoNet.Util;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.097.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Banco CrediSis
    /// </summary>
    internal class Banco_CrediSis : AbstractBanco, IBanco
    {

        #region Variáveis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;
        private string _codigoConvenioCliente = string.Empty;

        #endregion

        #region Construtores

        internal Banco_CrediSis()
        {
            try
            {
                this.Codigo = 97;
                this.Digito = "3";
                this.Nome = "CENTRALCREDI";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region VALIDAÇÕES

        /// <summary>
        /// Validações particulares do Banco CrediSis
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new NotImplementedException("Carteira não informada. Utilize a carteira 18.");

            //Verifica as carteiras implementadas
            if (!boleto.Carteira.Equals("18"))
                throw new NotImplementedException("Carteira não informada. Utilize a carteira 18.");

            //Verifica se o nosso número é válido
            if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                throw new NotImplementedException("Nosso número inválido.");

            #region Convênio e Sequência de Nosso Número
            #region Carteira 18
            //Carteira 18 com nosso número de 6 posições
            //if (boleto.Carteira.Equals("18"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length != 6)
            //        throw new NotImplementedException(string.Format("Para a carteira {0}, o número do convênio são de 6 posições", boleto.Carteira));

            //    if (boleto.NossoNumero.Length > 20)  //A sequencia inicial poderá somente ter no máximo 6 posições
            //        throw new NotImplementedException(string.Format("Para a carteira {0}, a quantidade máxima são de 20 de posições para o nosso número", boleto.Carteira));
            //}
            #endregion Carteira 18
            #endregion Convenio e ....

            #region Agência e Conta Corrente
            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.Cedente.ContaBancaria.Agencia + ", são de 4 números.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            if (!boleto.Cedente.ContaBancaria.Agencia.Equals("0002") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0003") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0004") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0005") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0009") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0011") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0012") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0017") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0018") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0019") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0020") &
                !boleto.Cedente.ContaBancaria.Agencia.Equals("0021")
                )

                throw new NotImplementedException("Agência informada não é reconhecida pelo Credisis. Consulte as Agências possíveis.");

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 8)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", são de 8 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 8)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 8);
            #endregion Agência e Conta Corrente

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;
            //Aplicando formatações
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        # endregion Validações....

        #region FORMATAçÔES

        public void FormataCodigoCliente(Cedente cedente)
        {
            if (cedente.Codigo.Length > 6)
                cedente.DigitoCedente = Convert.ToInt32(cedente.Codigo.Substring(6));

            cedente.Codigo = cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        public void FormataCodigoCliente(Boleto boleto)
        {
            if (boleto.Cedente.Codigo.Length == 7)
                boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.Codigo.Substring(6));

            boleto.Cedente.Codigo = boleto.Cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            #region Carteira 18
            if (boleto.Carteira.Equals("18"))
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}",
                    Utils.FormatCode(Codigo.ToString(), 3), // 1-3
                    boleto.Moeda,                           // 4
                    // 5
                    FatorVencimento(boleto),                // 6-9
                    valorBoleto,                            // 10-19
                    FormataCampoLivre(boleto));             // 20-44
            }
            #endregion Carteira 18

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        ///<summary>
        /// Campo Livre 25 posiçoes
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - Número do Nosso Número(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {

            string FormataCampoLivre = string.Format("00000{0}{1}{2}{3}{4}",
                                        Utils.FormatCode(Codigo.ToString(), 3),                         // Fixo
                                        Mod11(boleto.Cedente.CPFCNPJ),                                  // Módulo 11 do CPF/CNPJ (Incluindo dígitos verificadores) do Beneficiário
                                        boleto.Cedente.ContaBancaria.Agencia,                           // Código da Agência CrediSIS ao qual o Beneficiário possui Conta.
                                        Utils.FormatCode(boleto.Cedente.Convenio + "", 6),                // Código de Convênio do Beneficiário no Sistema CrediSIS
                                        Utils.FormatCode(boleto.NossoNumero, 6)                         // Sequencial Único do Boleto
            );
            return FormataCampoLivre;
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string cmplivre = string.Empty;
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;
            long icampo5 = 0;
            int digitoMod = 0;

            /*
            Campos 1 (AAABC.CCCCX):
            A = Código do Banco na Câmara de Compensação “001”
            B = Código da moeda "9" (*)
            C = Posição 20 a 24 do código de barras
            X = DV que amarra o campo 1 (Módulo 10, contido no Anexo 7)
             */

            cmplivre = Strings.Mid(boleto.CodigoBarra.Codigo, 20, 25);

            campo1 = Strings.Left(boleto.CodigoBarra.Codigo, 4) + Strings.Mid(cmplivre, 1, 5);
            digitoMod = Mod10(campo1);
            campo1 = campo1 + digitoMod.ToString();
            campo1 = Strings.Mid(campo1, 1, 5) + "." + Strings.Mid(campo1, 6, 5);
            /*
            Campo 2 (DDDDD.DDDDDY)
            D = Posição 25 a 34 do código de barras
            Y = DV que amarra o campo 2 (Módulo 10, contido no Anexo 7)
             */
            campo2 = Strings.Mid(cmplivre, 6, 10);
            digitoMod = Mod10(campo2);
            campo2 = campo2 + digitoMod.ToString();
            campo2 = Strings.Mid(campo2, 1, 5) + "." + Strings.Mid(campo2, 6, 6);


            /*
            Campo 3 (EEEEE.EEEEEZ)
            E = Posição 35 a 44 do código de barras
            Z = DV que amarra o campo 3 (Módulo 10, contido no Anexo 7)
             */
            campo3 = Strings.Mid(cmplivre, 16, 10);
            digitoMod = Mod10(campo3);
            campo3 = campo3 + digitoMod;
            campo3 = Strings.Mid(campo3, 1, 5) + "." + Strings.Mid(campo3, 6, 6);

            /*
            Campo 4 (K)
            K = DV do Código de Barras (Módulo 11, contido no Anexo 10)
             */
            campo4 = Strings.Mid(boleto.CodigoBarra.Codigo, 5, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do Título (*)
             */
            icampo5 = Convert.ToInt64(Strings.Mid(boleto.CodigoBarra.Codigo, 6, 14));

            if (icampo5 == 0)
                campo5 = "000";
            else
                campo5 = icampo5.ToString();

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
            #region Carteiras
            #region Carteira 18
            //Carteira 18 com nosso número de 11 posições
            if (boleto.Carteira.Equals("18"))
            {
                if (boleto.Cedente.Codigo.ToString().Length == 6)
                    boleto.NossoNumero = string.Format("{0}{1}{2}{3}{4}", "097", ModNossoNumeroCPF(boleto.Cedente.CPFCNPJ), boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.Codigo, Utils.FormatCode(boleto.NossoNumero, 6));
                else
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 6);
            }
            #endregion Carteira 18
            #endregion Carteiras

            if (boleto.BancoCarteira != null)
                boleto.BancoCarteira.FormataNossoNumero(boleto);
            else
                boleto.NossoNumero = string.Format("{0}", boleto.NossoNumero);
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
        }

        private string LimparCarteira(string carteira)
        {
            return carteira.Split('-')[0];
        }
        # endregion FORMATAÇÕES

        #region Métodos de geração do arquivo remessa - Genéricos
        /// <summary>
        /// HEADER DE LOTE do arquivo CNAB
        /// Gera o HEADER de Lote do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        header = "";
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
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";
                this.FormataCodigoCliente(cedente);
                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);
                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(cedente, numeroArquivoRemessa);
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
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";
                FormataNossoNumero(boleto);
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
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
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                throw new NotImplementedException("Função não implementada.");
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
                throw new NotImplementedException("Função não implementada.");
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
                throw new NotImplementedException("Função não implementada.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
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
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);

                #region Pega o Numero de Registros + 1(HeaderLote) + 1(TrailerLote)
                // Alterado por Heric Souza em 02/06/2017
                int vQtdeRegLote = (numeroRegistro + 1); // (numeroRegistro + 2);
                //int vQtdeRegLote = numeroRegistro; // (numeroRegistro + 2);
                trailer += Utils.FitStringLength(vQtdeRegLote.ToString(), 6, 6, '0', 0, true, true, true);  //posição 18 até 23   (6) - Quantidade de Registros no Lote
                //deve considerar 1 registro a mais - Header
                #endregion


                trailer += Utils.FormatCode("", "0", 92, true);
                trailer += Utils.FormatCode("", " ", 125);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);
                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string _brancos205 = new string(' ', 205);

                string _trailerArquivo;

                _trailerArquivo = "00199999         000001";
                _trailerArquivo += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                _trailerArquivo += "000000";
                _trailerArquivo += _brancos205;

                _trailerArquivo = Utils.SubstituiCaracteresEspeciais(_trailerArquivo);

                return _trailerArquivo;
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

        #region CNAB240 - Específicos
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                throw new NotImplementedException("Função não implementada.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                throw new NotImplementedException("Função não implementada.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE ARQUIVO do arquivo de REMESSA.", ex);
            }
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

        public string ModNossoNumeroCPF(string value)
        {
            string vRetorno;
            int d, s = 0, p = 2, b = 8;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                s = s + (Convert.ToInt32(value.Substring(i, 1)) * p);
                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            d = 11 - (s % 11);
            if (d > 9)
                d = 0;
            vRetorno = d.ToString();

            return vRetorno;
        }
        #region Métodos de processamento do arquivo retorno CNAB400


        #endregion

        #region CNAB 400
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
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));                                   //001-001  0
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));                                   //002-002  1 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                             //003-009 "REMESSA"
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                  //010-011 "01"
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 008, 0, "COBRANCA", ' '));                            //012-019 "COBRANCA"
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 007, 0, string.Empty, ' '));                          //020-026  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 004, 0, cedente.ContaBancaria.Agencia, '0'));         //027-030  Agencia com 4 digitos formatada
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' '));                          //031-031  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 008, 0, cedente.ContaBancaria.Conta, '0'));           //032-039  Conta do Beneficiário 8 posições
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 001, 0, cedente.ContaBancaria.DigitoConta, ' '));     //040-040  Dígito da Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0041, 006, 0, string.Empty, ' '));                          //041-046  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));                //047-076  Nome do Benficiário
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 018, 0, "097CENTRALCREDI", ' '));                     //077-094  "097CENTRALCREDI"
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                          //095-100  Data da Gravação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 007, 0, cedente.NumeroSequencial, '0'));              //101-107  Número Sequencial da Remessa
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 284, 0, string.Empty, ' '));                          //108-391  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 003, 0, "001", ' '));                                 //392-394  Versão do Arquivo
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                              //395-400  Número sequencial
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
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));                                       //001-001  
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, Utils.IdentificaTipoInscricaoSacado(boleto.Cedente.CPFCNPJ), '0')); //002-003  CPF = 01, CNPJ = 02
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    //004-017  CPFCNPJ
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //018-021  Agencia
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0022, 008, 0, boleto.Cedente.ContaBancaria.Conta, '0'));        //022-029  Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0030, 001, 0, boleto.Cedente.ContaBancaria.DigitoConta, ' '));  //030-030  Digito Conta
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 026, 0, string.Empty, ' '));                              //031-056  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0057, 020, 0, boleto.NossoNumero, '0'));                        //057-076  Nosso Número
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 002, 0, boleto.Remessa.CodigoOcorrencia, '0'));           //077-078  Código da Operação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0079, 006, 0, boleto.DataDocumento, ' '));                      //079-084  Data da Operação
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0085, 006, 0, string.Empty, ' '));                              //085-90  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0091, 002, 0, boleto.NumeroParcela.ToString(), '0'));           //091-092 Número da parcela 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0093, 001, 0, '3', '0'));                                       //093-093 Tipo Pagamento 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0094, 001, 0, '3', '0'));                                       //094-094 Tipo recebimento 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0095, 002, 0, boleto.EspecieDocumento.Codigo, '0'));                            //095-096 Espécie título 
                #region Instruções
                string vInstrucao1 = "0";
                string vInstrucao2 = "00";
                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_CrediSIS)instrucao.Codigo)
                    {
                        case EnumInstrucoes_CrediSIS.ProtestarAposNDiasCorridos:
                        case EnumInstrucoes_CrediSIS.ProtestarAposNDiasUteis:
                            vInstrucao1 = Utils.FitStringLength(instrucao.Codigo.ToString(), 1, 1, '0', 0, true, true, true);
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0097, 001, 0, vInstrucao1, '0'));                               //097-097 Tipo carência Protesto / Negativação 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0098, 002, 0, vInstrucao2, '0'));                               //098-099 Dias carência Protesto / Negativação 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0100, 002, 0, "01", '0'));                                      //100-101 Tipo de envio através de Protesto ou Negativação -01 cartório
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0102, 009, 0, string.Empty, ' '));                              //102-110  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0111, 010, 0, boleto.NumeroDocumento, '0'));                    //111-120 Número Documento 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));  //121-126 Data Vencimento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto * 100, '0'));                 //127-139 Valor Operação 
                #region DataLimitePagamento
                string vDataLimitePagamento = "000000";
                if (!boleto.DataLimitePagamento.Equals(DateTime.MinValue))
                    vDataLimitePagamento = boleto.DataLimitePagamento.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0140, 006, 0, vDataLimitePagamento, ' '));                      //140-145 Data Vencimento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0146, 005, 0, string.Empty, ' '));                              //146-150  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));                      //140-145 Data Vencimento
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0157, 001, 0, string.Empty, ' '));                              //157-157  Brancos
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0158, 002, 0, Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CPFCNPJ), '0'));//158-159
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0160, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //160-173
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //174-213
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0214, 025, 0, string.Empty, ' '));                              //214-238 Nome Fantasia Pagador  (Pessoa Jurídica) 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0239, 035, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //239-273 Logradouro Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0274, 006, 0, boleto.Sacado.Endereco.Numero, '0'));             //274-279 Número Endereço Pagador
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 025, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //280-304 Bairro Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0305, 025, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //305-329 Cidade Pagador
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0330, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //330-331 UF Pagador
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0332, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //332-329 CEP Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0340, 011, 0, string.Empty, ' '));                              //340-350 Celular Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0351, 043, 0, string.Empty, ' '));                              //351-393 E-mail Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, string.Empty, ' '));                              //394 -394  E-mail Pagador 
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400 Sequencial do Registro 
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

        public string GerarRegistroDetalhe2(Boleto boleto, int numeroRegistro)
        {

            TRegistroEDI reg = new TRegistroEDI();
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "2", '0'));                                       //001-001  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CPFCNPJ), '0')); //002-003  CPF = 01, CNPJ = 02
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //004-017 CPFCNPJ
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //018-057 Nome Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0058, 035, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //058-092 Logradouro Sacador/Avalista  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0093, 006, 0, boleto.Sacado.Endereco.Numero, '0'));             //093-098 Número Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0099, 025, 0, boleto.Sacado.Endereco.Complemento.ToUpper(), ' '));//099-123 Complemento Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0124, 025, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //124-148 Bairro Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0149, 025, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //149-173 Cidade Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //174-175 UF Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0176, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //176-183 CEP Sacador/Avalista 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0184, 001, 0, string.Empty, ' '));                              //184-184 Brancos
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0185, 099, 0, string.Empty, ' '));                              //185-283 Instrução do Título 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0284, 001, 0, string.Empty, ' '));                              //284-284 Brancos
            #region Código de Juros e Percentual
            string vCodigoJuros = "I"; //“I” = Isento
            Decimal vJuros = 0;

            if (boleto.PercJurosMora > 0)
            {
                vCodigoJuros = "P";    //“P” = Porcentagem 
                vJuros = boleto.PercJurosMora * 10000;
            }
            #endregion
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0285, 015, 0, vJuros, '0'));                                    //285-299 Valor Juros
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0300, 001, 0, vCodigoJuros, ' '));                              //300-300 Tipo dos Juros 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediInteiro______________, 0301, 001, 0, "0", '0'));                                       //301-301 Tipo Carência do Juros
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0302, 002, 0, "00", '0'));                                      //302-303 Dias Carência do Juros 
            #region Código de Multa e Valor/Percentual Multa
            string vCodigoMulta = "I"; //“I” = Isento
            Decimal vMulta = 0;

            if (boleto.ValorMulta > 0)
            {
                vCodigoMulta = "F";    //“F” = Valor fixo
                vMulta = boleto.ValorMulta * 10000;
            }
            else if (boleto.PercMulta > 0)
            {
                vCodigoMulta = "P";   //“P” = Porcentagem
                vMulta = boleto.PercMulta * 10000;
            }
            #endregion
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0304, 015, 0, vMulta, '0'));                                    //304-318 Valor da Multa 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0319, 001, 0, vCodigoMulta, ' '));                              //319-319 Tipo da Multa  
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0320, 001, 0, "0", '0'));                                       //320-320 Tipo Carência da Multa 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0321, 002, 0, "00", '0'));                                      //321-322 Dias Carência da Multa
            #region DataDesconto
            string vDataDesconto = "000000";
            string vCodigoTipoDesconto = "I"; //“I” = Isento
            if (!boleto.DataDesconto.Equals(DateTime.MinValue))
            {
                vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                vCodigoTipoDesconto = "F";
            }

            #endregion
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0323, 006, 0, vDataDesconto, '0'));                             //323-328 Data primeiro Desconto 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0329, 013, 0, boleto.ValorDesconto, '0'));                      //329-341 Valor primeiro Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0342, 001, 0, vCodigoTipoDesconto, ' '));                       //342-342 Tipo primeiro Desconto 
            #region DataDesconto
            vDataDesconto = "000000";
            vCodigoTipoDesconto = "I"; //“I” = Isento
            if (!boleto.DataOutrosDescontos.Equals(DateTime.MinValue))
            {
                vDataDesconto = boleto.DataOutrosDescontos.ToString("ddMMyy");
                vCodigoTipoDesconto = "F";
            }
            #endregion
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0343, 006, 0, vDataDesconto, '0'));                             //343-328 Data segundo Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0349, 013, 0, boleto.OutrosDescontos, '0'));                    //349-361 Valor segundo Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0362, 001, 0, vCodigoTipoDesconto, ' '));                       //362-362 Tipo segundo Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0363, 006, 0, "000000", '0'));                                  //363-368 Data terceiro Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0369, 013, 0, '0', '0'));                                       //369-381 Valor terceiro Desconto
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0382, 001, 0, "0", '0'));                                       //382-382 Tipo terceiro Desconto 
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0383, 012, 0, string.Empty, ' '));                              //383-383 Brancos
            reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400 Sequencial do Registro 
            //
            reg.CodificarLinha();
            //
            string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            //
            return _detalhe;
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-393
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
        //
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                // Identificação do Registro ==> 001 a 001
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));

                //Tipo de Inscrição Empresa ==> 002 a 003
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));

                //Nº Inscrição da Empresa ==> 004 a 017
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(21, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(29, 1));

                //Nº Controle do Participante ==> 038 a 062
                //detalhe.NumeroControle = registro.Substring(37, 25);

                //Identificação do Título no Banco ==> 057 a 076
                detalhe.NossoNumero = registro.Substring(56, 20);

                //Identificação de Ocorrência ==> 77 a 78
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(76, 2));

                //Data Ocorrência no Banco ==> 79 a 84
                int dataOcorrencia = Utils.ToInt32(registro.Substring(78, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Número do Documento ==> 85 a 94
                detalhe.NumeroDocumento = registro.Substring(84, 10);

                //Data Vencimento do Título ==> 147 a 152
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                //Valor do Título ==> 153 a 165
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                //Valor de Tarifa/Custas  ==> 166 a 175
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(165, 10));
                detalhe.ValorDespesa = valorDespesa / 100;

                //Data de Crédito ==> 176 a 181
                int dataCredito = Utils.ToInt32(registro.Substring(175, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Desconto Concedido (Valor Desconto Concedido) ==> 241 a 253
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;

                //Valor Pago ==> 254 a 266
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                //Juros Mora ==> 267 a 279
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(337, 2)); //Tipo de Inscrição Empresa 338-339 
                detalhe.NumeroInscricao = registro.Substring(339, 14); //Nº Inscrição da Empresa 340-353 

                //Nome do Sacado
                detalhe.NomeSacado = registro.Substring(353, 40); //Nº Inscrição da Empresa 354-393 

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

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
