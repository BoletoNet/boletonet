using System;
using System.Text;
using System.Web.UI;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.756.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Bancoob Sicoob Crédibom.
    /// Autor: Janiel Madureira Oliveira
    /// E-mail: janielbelmont@msn.com
    /// Twitter: @janiel14
    /// Data: 01/02/2012
    /// Obs: Os arquivo de remessa CNAB 400 foi implementado para cobranças com registros seguindo o padrão CBR641.
    /// 
    /// Atualização
    /// Mudança no logotipo para o SICOOB NO GERAL
    /// Criação de rotinas ausentes
    /// Autor: Adriano trentim Augusto
    /// E-mail: adriano@setrin.com.br
    /// Data: 25/02/2014
    /// </summary>
    internal class Banco_Sicoob : AbstractBanco, IBanco
    {
        #region CONSTRUTOR
        /**
         * <summary>Construtor</summary>
         */
        internal Banco_Sicoob()
        {
            this.Nome = "Sicoob";
            this.Codigo = 756;
            this.Digito = "0";
        }
        #endregion CONSTRUTOR

        #region FORMATAÇÕES

        public override void FormataNossoNumero(Boleto boleto)
        {
            //Variaveis
            int resultado = 0;
            int dv = 0;
            int resto = 0;
            String constante = "319731973197319731973";
            String cooperativa = boleto.Cedente.ContaBancaria.Agencia;
            String codigo = boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente.ToString();
            String nossoNumero = boleto.NossoNumero;
            StringBuilder seqValidacao = new StringBuilder();

            seqValidacao.Append(cooperativa.PadLeft(4, '0'));
            seqValidacao.Append(codigo.PadLeft(10, '0'));
            seqValidacao.Append(nossoNumero.PadLeft(7, '0'));

            /*
             * Multiplicando cada posição por sua respectiva posição na constante.
             */
            for (int i = 0; i < 21; i++)
            {
                resultado = resultado + (Convert.ToInt16(seqValidacao.ToString().Substring(i, 1)) * Convert.ToInt16(constante.Substring(i, 1)));
            }
            //Calculando mod 11
            resto = resultado % 11;
            //Verifica resto
            if (resto == 1 || resto == 0)
            {
                dv = 0;
            }
            else
            {
                dv = 11 - resto;
            }
            //Montando nosso número
            boleto.NossoNumero = boleto.NossoNumero + "-" + dv.ToString();
            boleto.DigitoNossoNumero = dv.ToString();
        }

        /**
         * FormataCodigoCliente
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */

        public void FormataCodigoCliente(Cedente cedente)
        {
            if (cedente.Codigo.Length == 7)
                cedente.DigitoCedente = Convert.ToInt32(cedente.Codigo.Substring(6));

            cedente.Codigo = cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        public void FormataCodigoCliente(Boleto boleto)
        {
            if (boleto.Cedente.Codigo.Length == 7)
                boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.Codigo.Substring(6));

            boleto.Cedente.Codigo = boleto.Cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
        }

        /**
         * FormataNumeroTitulo
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroTitulo(Boleto boleto)
        {
            var novoTitulo = new StringBuilder();
            novoTitulo.Append(boleto.NossoNumero.Replace("-", "").PadLeft(8, '0'));
            return novoTitulo.ToString();
        }

        /**
         * FormataNumeroParcela
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroParcela(Boleto boleto)
        {
            if (boleto.NumeroParcela <= 0)
                boleto.NumeroParcela = 1;

            //Variaveis
            StringBuilder novoNumero = new StringBuilder();
 
            //Formatando
            for (int i = 0; i < (3 - boleto.NumeroParcela.ToString().Length); i++)
            {
                novoNumero.Append("0");
            }
            novoNumero.Append(boleto.NumeroParcela.ToString());
            return novoNumero.ToString();
        }

        public static long FatorVencimento2000(Boleto boleto)
        {
            var dateBase = new DateTime(2000, 7, 3, 0, 0, 0);

            //Verifica se a data esta dentro do range utilizavel
            var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            long rangeUtilizavel = Utils.DateDiff(DateInterval.Day, dataAtual, boleto.DataVencimento);

            if (rangeUtilizavel > 5500 || rangeUtilizavel < -3000)
                throw new Exception("Data do vencimento fora do range de utilização proposto pela CENEGESC. Comunicado FEBRABAN de n° 082/2012 de 14/06/2012");

            while (boleto.DataVencimento > dateBase.AddDays(9999))
                dateBase = boleto.DataVencimento.AddDays(-(((Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) - 9999) - 1) + 1000));

            return Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) + 1000;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            //Variaveis
            int peso = 2;
            int soma = 0;
            int resultado = 0;
            int dv = 0;
            String codigoValidacao = boleto.Banco.Codigo.ToString() + boleto.Moeda.ToString() + FatorVencimento(boleto).ToString() +
                Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10) + boleto.Carteira +
                boleto.Cedente.ContaBancaria.Agencia + boleto.TipoModalidade + boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente +
                this.FormataNumeroTitulo(boleto) + this.FormataNumeroParcela(boleto);

            //Calculando
            for (int i = (codigoValidacao.Length - 1); i >= 0; i--)
            {
                soma = soma + (Convert.ToInt16(codigoValidacao.Substring(i, 1)) * peso);
                peso++;
                //Verifica peso
                if (peso > 9)
                {
                    peso = 2;
                }
            }
            resultado = soma % 11;
            dv = 11 - resultado;
            //Verificando resultado
            if (dv == 0 || dv == 1 || dv > 9)
            {
                dv = 1;
            }
            //Formatando
            boleto.CodigoBarra.Codigo = codigoValidacao.Substring(0, 4) + dv + codigoValidacao.Substring(4);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //Variaveis
            String campo1 = string.Empty;
            String campo2 = string.Empty;
            String campo3 = string.Empty;
            String campo4 = string.Empty;
            String campo5 = string.Empty;
            String indice = "1212121212";
            StringBuilder linhaDigitavel = new StringBuilder();
            int soma = 0;
            int temp = 0;

            //Formatando o campo 1
            campo1 = boleto.Banco.Codigo.ToString() + boleto.Moeda.ToString() + boleto.Cedente.Carteira + boleto.Cedente.ContaBancaria.Agencia;
            //Calculando CAMPO 1
            for (int i = 0; i < campo1.Length; i++)
            {
                //Calculando indice
                temp = (Convert.ToInt16(campo1.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i + 1, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }
            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo1.Substring(0, 5), campo1.Substring(5, 4), Multiplo10(soma)));
            
            soma = 0;
            temp = 0;
            //Formatando o campo 2
            campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            for (int i = 0; i < campo2.Length; i++)
            {
                //Calculando Indice 2
                temp = (Convert.ToInt16(campo2.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }

            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo2.Substring(0, 5), campo2.Substring(5, 5), Multiplo10(soma)));

            soma = 0;
            temp = 0;
            //Formatando campo 3
            campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            for (int i = 0; i < campo3.Length; i++)
            {
                //Calculando indice 2
                temp = (Convert.ToInt16(campo3.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando resultado
                soma = soma + temp;
            }
            linhaDigitavel.Append(campo3.Substring(0, 5) + "." + campo3.Substring(5, 5) + Multiplo10(soma) + " ");
            //Formatando Campo 4
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            linhaDigitavel.Append(campo4 + " ");
            //Formatando Campo 5
            campo5 = FatorVencimento(boleto).ToString() + Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);
            linhaDigitavel.Append(campo5);
            boleto.CodigoBarra.LinhaDigitavel = linhaDigitavel.ToString();
        }

        #endregion FORMATAÇÕES

        #region VALIDAÇÕES

        public override void ValidaBoleto(Boleto boleto)
        {
            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";


            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGÁVEL EM QUALQUER CORRESPONDENTE BANCÁRIO PERTO DE VOCÊ!";

            //Aplicando formatações
            this.FormataCodigoCliente(boleto);
            this.FormataNossoNumero(boleto);
            this.FormataCodigoBarra(boleto);
            this.FormataLinhaDigitavel(boleto);
        }

        #endregion VALIDAÇÕES

        #region ARQUIVO DE REMESSA

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
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
                        _header = GerarHeaderRemessaCNAB240(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
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
                        // não tem no CNAB 400 header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        private string GerarHeaderRemessaCNAB240(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            //Variaveis
            try
            {
                //Montagem do header
                string header = "756"; //Posição 001 a 003   Código do Sicoob na Compensação: "756"
                header += "0000"; //Posição 004 a 007  Lote de Serviço: "0000"
                header += "0"; //Posição 008           Tipo de Registro: "0"
                header += new string(' ', 9); //); //Posição 09 a 017     Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += cedente.CPFCNPJ.Length == 11 ? "1" : "2"; //Posição 018  1=CPF    2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true); //Posição 019 a 032   Número de Inscrição da Empresa
                header += Utils.FormatCode((cedente.Convenio > 0 ? cedente.Convenio.ToString() : ""), " ", 20, true); //Posição 033 a 052     Código do Convênio no Sicoob: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, 5);//Posição 053 a 057     Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 1);  //Posição 058 a 058 Digito Agência
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);   //Posição 059 a 070
                header += cedente.ContaBancaria.DigitoConta;  //Posição 071 a 71
                header += new string(' ', 1); //Posição 072 a 72     Dígito Verificador da Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);  //Posição 073 a 102      Nome do Banco: SICOOB
                header += Utils.FormatCode("SICOOB", " ", 30);     //Posição 103 a 132       Nome da Empresa
                header += Utils.FormatCode("", " ", 10);     //Posição 133 a 142  Uso Exclusivo FEBRABAN / CNAB: Brancos
                header += "1";        //Posição 103 a 142   Código Remessa / Retorno: "1"
                header += DateTime.Now.ToString("ddMMyyyy");       //Posição 144 a 151       Data de Geração do Arquivo
                header += Utils.FormatCode("", "0", 6);            //Posição 152 a 157       Hora de Geração do Arquivo
                header += "000001";         //Posição 158 a 163     Seqüência
                header += "081";            //Posição 164 a 166    No da Versão do Layout do Arquivo: "081"
                header += "00000";          //Posição 167 a 171    Densidade de Gravação do Arquivo: "00000"
                header += Utils.FormatCode("", " ", 69);
                header = Utils.SubstituiCaracteresEspeciais(header);
                //Retorno
                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            //Variaveis
            StringBuilder _header = new StringBuilder();
            //Tratamento de erros
            try
            {
                //Montagem do header
                _header.Append("0"); //Posição 001
                _header.Append("1"); //Posição 002
                _header.Append("REMESSA"); //Posição 003 a 009
                _header.Append("01"); //Posição 010 a 011
                _header.Append("COBRANCA"); //Posição 012 a 019
                _header.Append(new string(' ', 7)); //Posição 020 a 026
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 027 a 030
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 031
                _header.Append(Utils.FitStringLength(cedente.Codigo, 8, 8, '0', 0, true, true, true)); //Posição 032 a 039
                _header.Append(Utils.FitStringLength(Convert.ToString(cedente.DigitoCedente), 1, 1, '0', 0, true, true, true)); //Posição 40
                _header.Append(new string(' ', 6)); //Posição 041 a 046
                _header.Append(Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false)); //Posição 047 a 076
                _header.Append(Utils.FitStringLength("756BANCOOBCED", 18, 18, ' ', 0, true, true, false)); //Posição 077 a 094
                _header.Append(DateTime.Now.ToString("ddMMyy")); //Posição 095 a 100
                _header.Append(Utils.FitStringLength(Convert.ToString(numeroArquivoRemessa), 7, 7, '0', 0, true, true, true)); //Posição 101 a 107
                _header.Append(new string(' ', 287)); //Posição 108 a 394
                _header.Append("000001"); //Posição 395 a 400

                //Retorno
                return _header.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string header = "756"; //Posição 001 a 003   Código do Sicoob na Compensação: "756"
                header += "0001"; //Posição 004 a 007  Tipo de Registro: "1"
                header += "1";    //Posição 008        Tipo de Operação: "R"
                header += "R";    //Posição 009        Tipo de Serviço: "01"
                header += "01";   //Posição 010 a 011  Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += new string(' ', 2);   //Posição 012 a 013  Nº da Versão do Layout do Lote: "040"
                header += "040";  //Posição 014 a 016     Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += new string(' ', 1);    //Posição 017           Uso Exclusivo FEBRABAN/CNAB: Brancos
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");  //Posição 018        1=CPF    2=CGC/CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true); //Posição 019 a 033   Número de Inscrição da Empresa
                header += Utils.FormatCode((cedente.Convenio > 0 ? cedente.Convenio.ToString() : ""), " ", 20, true); //Posição 034 a 053     Código do Convênio no Sicoob: Brancos
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);//Posição 054 a 058     Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 1, true);//Posição 059 a 059
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);   //Posição 060 a 071
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);  //Posição 072 a 72
                header += new string(' ', 1); //Posição 073     Dígito Verificador da Ag/Conta: Brancos
                header += Utils.FormatCode(cedente.Nome, " ", 30);  //Posição 074 a 103      Nome do Banco: SICOOB
                header += Utils.FormatCode("", " ", 40);   // Posição 104 a 143 Informação 1			
                header += Utils.FormatCode("", " ", 40);   // Posição 144 a 183 Informação 2
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8, true);    // Número da remessa
                header += DateTime.Now.ToString("ddMMyyyy");       //Posição 192 a 199       Data de Gravação Remessa/Retorno
                header += Utils.FormatCode("", "0", 8, true);       //Posição 200 a 207      Data do Crédito: "00000000"
                header += new string(' ', 33);   // Uso Exclusivo FEBRABAN/CNAB: Brancos
                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                //Se o nosso número ainda não foi formatado então formata
                if (!string.IsNullOrWhiteSpace(boleto.NossoNumero)  && boleto.NossoNumero.Length <= 7)
                {
                    FormataNossoNumero(boleto);
                }

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
                throw new Exception("Erro durante a geração do DETALHE do arquivo de REMESSA.", ex);
            }
        }

        private string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            //Variaveis
            var _detalhe = new StringBuilder();

            //Tratamento de erros
            try
            {
                //Montagem do Detalhe
                _detalhe.Append("1"); //Posição 001
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Cedente.CPFCNPJ)); //Posição 002 a 003
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 004 a 017
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 018 a 021
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 022
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true)); //Posição 023 a 030
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true)); //Posição 031
                _detalhe.Append(new string('0', 6)); //Posição 032 a 037
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false)); //Posição 038 a 62
                _detalhe.Append(Utils.FitStringLength(FormataNumeroTitulo(boleto), 12, 12, '0', 0, true, true, true)); //Posição 063 a 074
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroParcela.ToString(), 2, 2, '0', 0, true, true, true)); //Posição 075 a 076
                _detalhe.Append("00"); //Posição 077 a 078
                _detalhe.Append("   "); //Posição 079 a 081
                _detalhe.Append(" "); //Posição 082
                _detalhe.Append("   "); //Posição 083 a 085
                _detalhe.Append("000"); //Posição 086 a 088
                _detalhe.Append("0"); //Posição 089
                _detalhe.Append("00000"); //Posição 090 a 094
                _detalhe.Append("0"); //Posição 095
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.NumeroBordero.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 096 a 101
                _detalhe.Append(new string(' ', 4)); //Posição 102 a 105

                // Tipo de emissão"Tipo de Emissão: 1 - Cooperativa 2 - Cliente"
                var tipoDeEmissao = "1";
                if (boleto.ApenasRegistrar)
                    tipoDeEmissao = "2";

                _detalhe.Append(Utils.FitStringLength(tipoDeEmissao, 1, 1, '0', 0, true, true, true)); // Posição 106 a 106

                _detalhe.Append(Utils.FitStringLength(boleto.TipoModalidade, 2, 2, '0', 0, true, true, true));  //Posição 107 a 108
                _detalhe.Append(Utils.FitStringLength(boleto.Remessa.CodigoOcorrencia, 2, 2, '0', 0, true, true, true)); //Posição 109 a 110 - (1)REGISTRO DE TITULOS (2)Solicitação de Baixa
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, '0', 0, true, true, true)); //Posição 111 a 120
                _detalhe.Append(boleto.DataVencimento.ToString("ddMMyy")); //Posição 121 a 126
                _detalhe.Append(Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //Posição 127 a 139 
                _detalhe.Append(boleto.Banco.Codigo); //Posição 140 a 142
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 143 a 146
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 147
                _detalhe.Append(Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true)); //Posição 148 a 149

                _detalhe.Append(boleto.Aceite == "N" ? "0" : "1"); //Posição 150
                _detalhe.Append(boleto.DataProcessamento.ToString("ddMMyy")); //Posição 151 a 156
                _detalhe.Append("07"); //Posição 157 a 158 - NÂO PROTESTAR
                _detalhe.Append("22"); //Posição 159 a 160 - PERMITIR DESCONTO SOMENTE ATE DATA ESTIPULADA
                _detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercJurosMora * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //Posição 161 a 166
                _detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //Posição 167 a 172
                _detalhe.Append(" "); //Posição 173
                _detalhe.Append(Utils.FitStringLength((boleto.DataDesconto == DateTime.MinValue ? "0" : boleto.DataDesconto.ToString("ddMMyy")), 6, 6, '0', 0, true, true, true)); //Posição 174 a 179
                _detalhe.Append(Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //Posição 180 a 192
                _detalhe.Append("9" + Utils.FitStringLength(boleto.IOF.ApenasNumeros(), 12, 12, '0', 0, true, true, true)); //Posição 193 a 205
                _detalhe.Append(Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //Posição 206 a 218
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CPFCNPJ)); //Posição 219 a 220
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 221 a 234
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false)); //Posição 235 a 274
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumeroEComplemento, 37, 37, ' ', 0, true, true, false)); //Posição 275 a 311
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 15, 15, ' ', 0, true, true, false)); //Posição 312 a 326
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true)); //Posição 327 a 334
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)); //Posição 335 a 349
                _detalhe.Append(boleto.Sacado.Endereco.UF); //Posição 350 a 351
                _detalhe.Append(new string(' ', 40)); //Posição 352 a 391 - OBSERVACOES
                _detalhe.Append("00"); //Posição 392 a 393 - DIAS PARA PROTESTO
                _detalhe.Append(" "); //Posição 394
                _detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 394 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_detalhe.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }
		
		/// <summary>
        /// Função que gera nosso numero a ser colocado na remessa sicoob CNAB240, segundo layout para troca de informações
        /// </summary>
        /// <param name="boleto"></param>
        /// <returns></returns>
        private string NossoNumeroFormatado( Boleto boleto )
        {
            FormataNossoNumero(boleto);

            string retorno = Utils.FormatCode(boleto.NossoNumero.Replace("-",""), "0", 10, true); // nosso numero+dg - 10 posicoes
            retorno = retorno + Utils.FormatCode(boleto.NumeroParcela.ToString(), "0", 2, true); // numero parcela - 2 posicoes
            retorno = retorno + Utils.FormatCode(boleto.ModalidadeCobranca.ToString(), "0", 2, true); // modalidade - 2 posicoes
            retorno = retorno + "4"; // tipo formulario (A4 sem envelopamento) - 1 posicoes;
            retorno = retorno + Utils.FormatCode("", " ", 5); // brancos - 5 posicoes ;
            return retorno;
        }
		
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3); //Posição 001 a 003   Código do Sicoob na Compensação: "756"
                detalhe += "0001"; //Posição 004 a 007   Lote
                detalhe += "3"; //Posição 008   Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true); //Posição 009 a 013   Número Sequencial
                detalhe += "P"; //Posição 014 Cód. Segmento do Registro Detalhe: "P"
                detalhe += " ";  //Posição 015 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode(boleto.Remessa.CodigoOcorrencia, 2); //Posição 016 a 017       '01'  =  Entrada de Títulos
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 5); //Posição 018 a 022     Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoAgencia, "0", 1, true);  //Posição 023  Dígito Verificador do Prefixo: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 12); //Posição 024 a 035 Conta Corrente: vide planilha "Capa" deste arquivo
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, 1);  //Posição 036  Dígito Verificador da Conta: vide planilha "Capa" deste arquivo
                detalhe += " ";  //Posição 037 Dígito Verificador da Ag/Conta: Brancos
                detalhe += Utils.FormatCode(NossoNumeroFormatado(boleto), 20);  //Posição 038 a 057 Nosso Número
                detalhe += (Convert.ToInt16(boleto.Carteira) == 1 ? "1" : "2");  //Posição 058 Código da Carteira: vide planilha "Capa" deste arquivo
                detalhe += "0";  //Posição 059 Forma de Cadastr. do Título no Banco: "0"
                detalhe += " ";  //Posição 060 Tipo de Documento: Brancos
                detalhe += "2";  //Posição 061 "Identificação da Emissão do Boleto: 1=Sicoob Emite 2=Beneficiário Emite TODO:Deivid
                detalhe += "2";  //Posição 062 "Identificação da distribuição do Boleto: 1=Sicoob Emite 2=Beneficiário Emite TODO:Deivid
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 15); //Posição 063 a 075 Número do documento de cobrança. TODO:Deivid
                detalhe += Utils.FormatCode(boleto.DataVencimento.ToString("ddMMyyyy"), 8);

                string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");

                valorBoleto = Utils.FormatCode(valorBoleto, 15);
                detalhe += valorBoleto; //Posição 86 a 100   Valor Nominal do Título
                detalhe += "00000";//Posição 101 a 105     Agência Encarregada da Cobrança: "00000"
                detalhe += new string(' ', 1);  //Posição 106  Dígito Verificador da Agência: Brancos
                detalhe += Utils.FormatCode(boleto.EspecieDocumento.Codigo, 2);  //Posição 107 a 108   Espécie do título
                detalhe += Utils.FormatCode(boleto.Aceite, 1);  //Posição 109 Identificação do título Aceito/Não Aceito  TODO:Deivid
                detalhe += Utils.FormatCode(boleto.DataProcessamento.ToString("ddMMyyyy"), 8);   //Posição 110 a 117   Data Emissão do Título
                detalhe += Utils.FormatCode(boleto.CodJurosMora, "2", 1); //Posição 118  - Código do juros mora. 2 = Padrao % Mes
                detalhe += Utils.FormatCode(boleto.DataJurosMora > DateTime.MinValue ? boleto.DataJurosMora.ToString("ddMMyyyy") : "".PadLeft(8, '0'), 8);  //Posição 119 a 126  - Data do Juros de Mora: preencher com a Data de Vencimento do Título
                detalhe += Utils.FormatCode(boleto.CodJurosMora == "0" ? "".PadLeft(15, '0') : (boleto.CodJurosMora == "1" ? boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", "") : boleto.PercJurosMora.ToString("f").Replace(",", "").Replace(".", "")), 15);   //Posição 127 a 141  - Data do Juros de Mora: preencher com a Data de Vencimento do Título

                if (boleto.DataDesconto > DateTime.MinValue)
                {
                    detalhe += "1"; //Posição 118  - Código do desconto
                    detalhe += Utils.FormatCode(boleto.DataDesconto.ToString("ddMMyyyy"), 8); //Posição 143 a 150  - Data do Desconto 1
                    detalhe += Utils.FormatCode(boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", ""), 15);
                }
                else
                {
                    detalhe += "0"; //Posição 118  - Código do desconto - Sem Desconto
                    detalhe += Utils.FormatCode("", "0", 8, true); ; //Posição 143 a 150  - Data do Desconto
                    detalhe += Utils.FormatCode("", "0", 15, true);
                }
                
                detalhe += Utils.FormatCode(boleto.IOF.ToString(), 15);//Posição 166 a 180   -  Valor do IOF a ser Recolhido
                detalhe += Utils.FormatCode(boleto.Abatimento.ToString(), 15);//Posição 181 a 195   - Valor do Abatimento
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 25); //Posição 196 a 220  - Identificação do título
                detalhe += "3"; //Posição 221  - Código do protesto 3 = Nao Protestar

                #region Instruções

                string vInstrucao1 = "00"; //2ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                foreach (IInstrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicoob)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicoob.CobrarJuros:
                            vInstrucao1 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }

                #endregion

                detalhe += Utils.FormatCode(vInstrucao1, 2);  //Posição 222 a 223  - Código do protesto
                detalhe += Utils.FormatCode("0", 1);     //Posição 224  - Código para Baixa/Devolução: "0"
                detalhe += Utils.FormatCode(" ", 3);     //Posição 225 A 227  - Número de Dias para Baixa/Devolução: Brancos
                detalhe += Utils.FormatCode(boleto.Moeda.ToString(), "0", 2, true); //Posição 228 A 229  - Código da Moeda
                detalhe += Utils.FormatCode("", "0", 10, true); //Posição 230 A 239    -  Nº do Contrato da Operação de Créd.: "0000000000"
                detalhe += " ";
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true); //Posição 001 a 003   Código do Sicoob na Compensação: "756"
                detalhe += "0001"; //Posição 004 a 007   Lote
                detalhe += "3"; //Posição 008   Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true); //Posição 009 a 013   Número Sequencial
                detalhe += "Q"; //Posição 014 Cód. Segmento do Registro Detalhe: "P"
                detalhe += " ";  //Posição 015 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += "01"; //Posição 016 a 017       '01'  =  Entrada de Títulos
                detalhe += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");  //Posição 018        1=CPF    2=CGC/CNPJ
                detalhe += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15, true); //Posição 019 a 033   Número de Inscrição da Empresa
                detalhe += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);  //Posição 034 a 73      Nome
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);  //Posição 074 a 113      Endereço
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);                     // Bairro 
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.CEP, 8);    //CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);                     // Cidade 
                detalhe += boleto.Sacado.Endereco.UF;                                                  // Unidade da Federação
                detalhe += (boleto.Cedente.CPFCNPJ.Length == 11 ? "1" : "2");                             // Tipo de Inscrição Sacador avalista
                detalhe += Utils.FormatCode(boleto.Cedente.CPFCNPJ, "0", 15, true);                             // Número de Inscrição / Sacador avalista
                detalhe += Utils.FormatCode(boleto.Cedente.Nome, " ", 40);                                // Nome / Sacador avalista
                detalhe += "000";                                                                         // Código Bco. Corresp. na Compensação
                detalhe += Utils.FormatCode("", " ", 20);                                                 //213 - Nosso N° no Banco Correspondente "1323739"
                detalhe += Utils.FormatCode("", " ", 8);                                                  // Uso Exclusivo FEBRABAN/CNAB
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe).ToUpper();
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string detalhe = Utils.FormatCode(Codigo.ToString(), 3); //Posição 001 a 003   Código do Sicoob na Compensação: "756"
                detalhe += "0001"; //Posição 004 a 007   Lote
                detalhe += "3"; //Posição 008   Tipo de Registro: "3"
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true); //Posição 009 a 013   Número Sequencial
                detalhe += "R"; //Posição 014 Cód. Segmento do Registro Detalhe: "R"
                detalhe += " ";  //Posição 015 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += "01"; //Posição 016 a 017       '01'  =  Entrada de Títulos

                if (boleto.DataOutrosDescontos > DateTime.MinValue)
                {
                    detalhe += "1"; //Posição 18  - Código do desconto 2
                    detalhe += Utils.FormatCode(boleto.DataOutrosDescontos.ToString("ddMMyyyy"), 8); //Posição 19 a 26  - Data do Desconto 2
                    detalhe += Utils.FormatCode(boleto.OutrosDescontos.ToString("f").Replace(",", "").Replace(".", ""), 15);  //Posição 27 a 41  - Valor do Desconto 2
                }
                else
                {
                    detalhe += "0"; //Posição 18  - Código do desconto 2
                    detalhe += Utils.FormatCode("", "0", 8, true); //Posição 19 a 26  - Data do Desconto 2
                    detalhe += Utils.FormatCode("", "0", 15, true);  //Posição 27 a 41  - Valor do Desconto 2
                }
                
                detalhe += "0"; //Posição 42  - Código da desconto 3
                detalhe += Utils.FormatCode("", "0", 8, true);
                detalhe += Utils.FormatCode("", "0", 15, true);

                if (boleto.PercMulta > 0)
                {
                    // Código da multa 2 - percentual
                    detalhe += "2";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);  //Posição 119 a 126  - Data do Juros de Mora: preencher com a Data de Vencimento do Título
                    detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    // Código da multa 1 - valor fixo
                    detalhe += "1";
                    detalhe += Utils.FormatCode(boleto.DataMulta.ToString("ddMMyyyy"), 8);  //Posição 119 a 126  - Data do Juros de Mora: preencher com a Data de Vencimento do Título
                    detalhe += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // Código da multa 0 - sem multa
                    detalhe += "0";
                    detalhe += Utils.FormatCode("", "0", 8); //Posição 119 a 126  - Data do Juros de Mora: preencher com a Data de Vencimento do Título
                    detalhe += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);
                }
                
                detalhe += Utils.FormatCode(""," ", 10); //Posição 90 a 99 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode(""," ", 40); //Posição 100 a 139 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode(""," ", 40); //Posição 140 a 179 Informação ao Pagador: Brancos
                detalhe += Utils.FormatCode(""," ", 20); //Posição 180 a 199 Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe += Utils.FormatCode("", "0", 8, true);  //Posição 200 a 207  Cód. Ocor. do Pagador: "00000000"
                detalhe += Utils.FormatCode("", "0", 3, true);  //Posição 208 a 210  Cód. do Banco na Conta do Débito: "000"
                detalhe += Utils.FormatCode("", "0", 5, true);  //Posição 211 a 215  Código da Agência do Débito: "00000"
                detalhe += " "; //Posição 216 Dígito Verificador da Agência: Brancos
                detalhe += Utils.FormatCode("", "0", 12, true);  //Posição 217 a 228  Conta Corrente para Débito: "000000000000"
                detalhe += " "; //Posição 229  Verificador da Conta: Brancos
                detalhe += " "; //Posição 230  Verificador Ag/Conta: Brancos
                detalhe += "0"; //Posição 231  Aviso para Débito Automático: "0"
                detalhe += Utils.FormatCode(""," ", 9); //Posição Uso Exclusivo FEBRABAN/CNAB: Brancos
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true); //Código do banco
                trailer += "0001"; //Posição 004 a 007   Lote
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);  //Posição Uso 9 a 19    Exclusivo FEBRABAN/CNAB: Brancos
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 17, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 17, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 17, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 17, true);
                trailer += Utils.FormatCode("", " ", 8, true);
                trailer += Utils.FormatCode("", " ", 117);
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
                //Código do Banco na compensação ==> 001 - 003
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                trailer += "9999";

                //Tipo de registro ==> 008 - 008
                trailer += "9";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de lotes do arquivo ==> 018 - 023
                trailer += Utils.FormatCode("1", "0", 6, true);

                //Quantidade de registros do arquivo ==> 024 - 029
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Quantidade de registros do arquivo ==> 030 - 035
                trailer += Utils.FormatCode("", "0", 6, true);

                //Reservado (uso Banco) ==> 036 - 240
                trailer += Utils.FormatCode("", " ", 205);

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            //Variavies
            StringBuilder _trailer = new StringBuilder();
            //Tratamento
            try
            {
                //Montagem trailer
                _trailer.Append("9"); //Posição 001
                _trailer.Append(new string(' ', 393)); //Posição 002 a 394
                _trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 395 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_trailer.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar TRAILER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion ARQUIVO DE REMESSA

        #region ::. Arquivo de Retorno CNAB400 .::
        
        /// <summary>
        /// Rotina de retorno de remessa
        /// Criador: Adriano Trentim Augusto
        /// E-mail: adriano@setrin.com.br
        /// Data: 29/04/2014
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var detalhe = new DetalheRetorno(registro);

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2)); //Tipo de Inscrição Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14); //Nº Inscrição da Empresa

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(22, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(30, 1));

                detalhe.NumeroControle = registro.Substring(37, 25); //Nº Controle do Participante
                
                //Identificação do Título no Banco
                detalhe.NossoNumero = registro.Substring(62, 11);
                detalhe.DACNossoNumero = registro.Substring(73, 1);

                switch (registro.Substring(106, 2)) // Carteira
	        {
	          case "01":
	            detalhe.Carteira = "1";
	            break;
	          case "02":
	            detalhe.Carteira = "1";
	            break;
	          case "03":
	            detalhe.Carteira = "3";
	            break;
	        }
	        
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2)); //Identificação de Ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2)); //Descrição da ocorrência
                detalhe.DataOcorrencia = Utils.ToDateTime(Utils.ToInt32(registro.Substring(110, 6)).ToString("##-##-##")); //Data da ocorrencia

                //Quando ocorrencia = Liquidação, pega a data.
                if (detalhe.CodigoOcorrencia == 5 || detalhe.CodigoOcorrencia == 6 || detalhe.CodigoOcorrencia == 15)
                    detalhe.DataLiquidacao = detalhe.DataOcorrencia;

                detalhe.NumeroDocumento = registro.Substring(116, 10); //Número do Documento
                detalhe.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(146, 6)).ToString("##-##-##")); //Data de Vencimento
                detalhe.ValorTitulo = ((decimal)Convert.ToInt64(registro.Substring(152, 13))) / 100; //Valor do Titulo
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3)); //Banco Cobrador
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4)); //Agência Cobradora
                detalhe.DACAgenciaCobradora = Utils.ToInt32(registro.Substring(172, 1)); // DV Agencia Cobradora
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2)); //Espécie do Título

                //Data de Crédito - Só vem preenchido quando liquidação
                if (registro.Substring(175, 6) != "000000")
                    detalhe.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(175, 6)).ToString("##-##-##"));
                else
                    detalhe.DataCredito = detalhe.DataOcorrencia;

                detalhe.ValorDespesa = ((decimal)Convert.ToUInt64(registro.Substring(181, 7))) / 100; //Valor das Tarifas
                detalhe.ValorOutrasDespesas = ((decimal)Convert.ToUInt64(registro.Substring(188, 13))) / 100; //Valor das Outras Despesas
                detalhe.ValorAbatimento = ((decimal)Convert.ToUInt64(registro.Substring(227, 13))) / 100; //Valor do abatimento
                detalhe.Descontos = ((decimal)Convert.ToUInt64(registro.Substring(240, 13))) / 100; //Valor do desconto
                detalhe.ValorPago = ((decimal)Convert.ToUInt64(registro.Substring(253, 13))) / 100; //Valor do Recebimento
                detalhe.JurosMora = ((decimal)Convert.ToUInt64(registro.Substring(266, 13))) / 100; //Valor de Juros
                detalhe.IdentificacaoTitulo = detalhe.NumeroDocumento; //Identificação do Título no Banco
                detalhe.OutrosCreditos = ((decimal)Convert.ToUInt64(registro.Substring(279, 13))) / 100; //Outros recebimentos

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        private string Ocorrencia(string codigoOcorrencia)
        {
            switch (codigoOcorrencia)
            {
                case "02":
                    return "Confirmação Entrada Título";
                case "03":
                    return "Comando Recusado";
                case "04":
                    return "Transferência de Carteira - Entrada";
                case "05":
                    return "Liquidação Sem Registro";
                case "06":
                    return "Liquidação Normal";
                case "09":
                    return "Baixa de Título";
                case "10":
                    return "Baixa Solicitada";
                case "11":
                    return "Títulos em Ser";
                case "12":
                    return "Abatimento Concedido";
                case "13":
                    return "Abatimento Cancelado";
                case "14":
                    return "Alteração de Vencimento";
                case "15":
                    return "Liquidação em Cartório";
                case "19":
                    return "Confirmação Instrução Protesto";
                case "20":
                    return "Débito em Conta";
                case "21":
                    return "Alteração de nome do Sacado";
                case "22":
                    return "Alteração de endereço Sacado";
                case "23":
                    return "Encaminhado a Protesto";
                case "24":
                    return "Sustar Protesto";
                case "25":
                    return "Dispensar Juros";
                case "26":
                    return "Instrução Rejeitada";
                case "27":
                    return "Confirmação Alteração Dados";
                case "28":
                    return "Manutenção Título Vencido";
                case "30":
                    return "Alteração Dados Rejeitada";
                case "96":
                    return "Despesas de Protesto";
                case "97":
                    return "Despesas de Sustação de Protesto";
                case "98":
                    return "Despesas de Custas Antecipadas";
                default:
                    return "Ocorrência não cadastrada";
            }
        }

        #endregion

        #region ::. Arquivo de Retorno CNAB240 .::

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                DetalheSegmentoTRetornoCNAB240 detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                detalhe.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                detalhe.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                detalhe.DigitoAgencia = registro.Substring(22, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(23, 12));
                detalhe.DigitoConta = registro.Substring(35, 1);
                detalhe.NossoNumero = registro.Substring(37, 20);
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                detalhe.NumeroDocumento = registro.Substring(58, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                detalhe.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(81, 15));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                detalhe.NumeroInscricao = registro.Substring(133, 15);
                detalhe.NomeSacado = registro.Substring(148, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(198, 15));
                detalhe.ValorTarifas = valorTarifas / 100;

                return detalhe;
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
                DetalheSegmentoURetornoCNAB240 detalhe = new DetalheSegmentoURetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");

                detalhe.CodigoOcorrenciaSacado = registro.Substring(15, 2);
                int DataCredito = Convert.ToInt32(registro.Substring(145, 8));
                detalhe.DataCredito = (DataCredito > 0) ? Convert.ToDateTime(DataCredito.ToString("##-##-####")) : new DateTime();
                int DataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                detalhe.DataOcorrencia = (DataOcorrencia > 0) ? Convert.ToDateTime(DataOcorrencia.ToString("##-##-####")) : new DateTime();
                int DataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));
                if (DataOcorrenciaSacado > 0)
                    detalhe.DataOcorrenciaSacado = Convert.ToDateTime(DataOcorrenciaSacado.ToString("##-##-####"));
                else
                    detalhe.DataOcorrenciaSacado = DateTime.Now;

                decimal JurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15));
                detalhe.JurosMultaEncargos = JurosMultaEncargos / 100;
                decimal ValorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15));
                detalhe.ValorDescontoConcedido = ValorDescontoConcedido / 100;
                decimal ValorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15));
                detalhe.ValorAbatimentoConcedido = ValorAbatimentoConcedido / 100;
                decimal ValorIOFRecolhido = Convert.ToUInt64(registro.Substring(62, 15));
                detalhe.ValorIOFRecolhido = ValorIOFRecolhido / 100;
                decimal ValorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15));
                detalhe.ValorPagoPeloSacado = ValorPagoPeloSacado / 100;
                decimal ValorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15));
                detalhe.ValorLiquidoASerCreditado = ValorLiquidoASerCreditado / 100;
                decimal ValorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15));
                detalhe.ValorOutrasDespesas = ValorOutrasDespesas / 100;

                decimal ValorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15));
                detalhe.ValorOutrosCreditos = ValorOutrosCreditos / 100;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }


        }

        #endregion

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
