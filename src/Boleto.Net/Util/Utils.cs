using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BoletoNet.Util;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace BoletoNet
{
    sealed class Utils
    {
        internal static Image DrawText(string text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width - Convert.ToInt32(font.Size * 1.5), (int)textSize.Height, PixelFormat.Format24bppRgb);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;
        }
        internal static long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }

        // uislcs: Acho que a função FormatCode() deveria ser renomeada para Completar().
        /*
         * "Para os registros tipo A (Alfanumérico) preencher com caracteres caixa alta e com espaços à direita
         * preenchendo todo o espaço do campo. Para os registros tipo N (Numérico) preencher com zeros à
         * esquerda preenchendo todo o campo." (p.9)
         * 
         * Disponível em: http://www.sicoobpr.com.br/download/manualcobranca/Manual_Cedentes_Sistema_Proprio.doc
         */

        /// <summary>
        /// Função para completar um string com zeros ou espacos em branco. Pode servir para criar a remessa.
        /// </summary>
        /// <param name="text">O valor recebe os zeros ou espaços em branco</param>
        /// <param name="with">caractere a ser inserido</param>
        /// <param name="size">Tamanho do campo</param>
        /// <param name="left">Indica se caracteres serão inseridos à esquerda ou à direita, o valor default é inicializar pela esquerda (left)</param>
        /// <returns></returns>
        internal static string FormatCode(string text, string with, int length, bool left)
        {
            // caso tamanho da string maior que desejado , corta a mesma , evitando estouro no tamanho 
            if (text.Length > length)
                text = text.Substring(0, length);

            //Esse método já existe, é PadLeft e PadRight da string
            length -= text.Length;
            if (left)
            {
                for (int i = 0; i < length; ++i)
                {
                    text = with + text;
                }
            }
            else
            {
                for (int i = 0; i < length; ++i)
                {
                    text += with;
                }
            }
            return text;
        }

        internal static string FormatCode(string text, string with, int length)
        {
            return FormatCode(text, with, length, false);
        }

        internal static string FormatCode(string text, int length)
        {
            return text.PadLeft(length, '0');
        }

        /// <summary>
        /// retorna um array de strings de tamanho variável com os dados da linha (pode ser usado para qualquer leitura de arquivos de retorno || remessa)
        /// os dados no string pattern correspondem a intervalos fechados na matemática ex: [2-19] (fechado de 2 a 19)
        /// </summary>
        /// <param name="linha">string de onde os dados serão extraídos. por exemplo, uma linha de um arquivo de retorno</param>
        /// <param name="pattern">obrigatóriamente é necessário numero PAR de valores NUMÉRICOS no string pattern. ex: 1-1,2-19</param>
        /// <returns>um array de strings de tamanho variável contendo os dados lidos na linha: string[]</returns>
        /// <example>
        /// string[] dados = getDados(sLine, "1-1,2-394,395-400");
        /// </example>
        internal static string[] GetDados(string linha, string pattern)
        {
            // separa os números
            pattern = pattern.Replace('-', ',');
            string[] coord = pattern.Split(',');

            //cria objeto para armazenágem, buffer.
            string[] dados = new string[coord.Length / 2];

            //pega os números de 2 em 2 e preenche o array
            int x = 0;
            for (int i = 0; i < coord.Length; i += 2)
            {
                dados[x] = linha.Substring(Convert.ToInt32(coord[i]) - 1, Convert.ToInt32(coord[i + 1]) - Convert.ToInt32(coord[i]) + 1);
                //arg[x] = linha.Substring(Convert.ToInt32(coord[i]), Convert.ToInt32(coord[i + 1]));
                x++;
            }
            //retorna os dados
            return dados;
        }

        /* Exemplo de Leitura de um arquivo de remessa
        private void button1_Click(object sender, EventArgs e)
        { //ler arquivo de texto
            StreamReader objReader = new StreamReader("C:\\Documents and Settings\\uis\\Desktop\\bancos\\CED006877211081.REM");
            string sLine = "";
            string[] dados;
        
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null){
                    dados = getDados(sLine, "1-1,2-394,395-400");
                    // adicionar os dados a um string
                    textBox1.Text += " posição:<" + dados[2] + ">";
                    // poderia ser
                    //new boleto_dados(dados[0],dados[1],dados[2]);
                }
            }
            objReader.Close();
        }
        */

        internal static bool IsNumber(string value)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(value) &&
                   !objTwoDotPattern.IsMatch(value) &&
                   !objTwoMinusPattern.IsMatch(value) &&
                   objNumberPattern.IsMatch(value);
        }

        internal static bool ToBool(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        internal static int ToInt32(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        internal static long ToInt64(string value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }

        internal static decimal ToDecimal(string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

        internal static string ToString(object value)
        {
            try
            {
                return Convert.ToString(value).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        internal static DateTime ToDateTime(object value)
        {
            try
            {
                return Convert.ToDateTime(value, CultureInfo.GetCultureInfo("pt-BR"));
            }
            catch
            {
                return new DateTime(1, 1, 1);
            }
        }


        /// <summary>
        /// Formata o CPF ou CNPJ do Cedente ou do Sacado no formato: 000.000.000-00, 00.000.000/0001-00 respectivamente.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string FormataCPFCPPJ(string value)
        {
            if (value.Trim().Length == 11)
                return FormataCPF(value);
            else if (value.Trim().Length == 14)
                return FormataCNPJ(value);

            throw new Exception(string.Format("O CPF ou CNPJ: {0} é inválido.", value));
        }

        /// <summary>
        /// Formata o número do CPF 92074286520 para 920.742.865-20
        /// </summary>
        /// <param name="cpf">Sequencia numérica de 11 dígitos. Exemplo: 00000000000</param>
        /// <returns>CPF formatado</returns>
        internal static string FormataCPF(string cpf)
        {
            try
            {
                return string.Format("{0}.{1}.{2}-{3}", cpf.Substring(0, 3), cpf.Substring(3, 3), cpf.Substring(6, 3), cpf.Substring(9, 2));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formata o CNPJ. Exemplo 00.316.449/0001-63
        /// </summary>
        /// <param name="cnpj">Sequencia numérica de 14 dígitos. Exemplo: 00000000000000</param>
        /// <returns>CNPJ formatado</returns>
        internal static string FormataCNPJ(string cnpj)
        {
            try
            {
                return string.Format("{0}.{1}.{2}/{3}-{4}", cnpj.Substring(0, 2), cnpj.Substring(2, 3), cnpj.Substring(5, 3), cnpj.Substring(8, 4), cnpj.Substring(12, 2));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formato o CEP em 00.000-000
        /// </summary>
        /// <param name="cep">Sequencia numérica de 8 dígitos. Exemplo: 00000000</param>
        /// <returns>CEP formatado</returns>
        internal static string FormataCEP(string cep)
        {
            try
            {
                return string.Format("{0}{1}-{2}", cep.Substring(0, 2), cep.Substring(2, 3), cep.Substring(5, 3));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formata agência e conta
        /// </summary>
        /// <param name="agencia">Código da agência</param>
        /// <param name="digitoAgencia">Dígito verificador da agência. Pode ser vazio.</param>
        /// <param name="conta">Código da conta</param>
        /// <param name="digitoConta">dígito verificador da conta. Pode ser vazio.</param>
        /// <returns>Agência e conta formatadas</returns>
        internal static string FormataAgenciaConta(string agencia, string digitoAgencia, string conta, string digitoConta)
        {
            string agenciaConta = string.Empty;
            try
            {
                agenciaConta = agencia;
                if (!string.IsNullOrEmpty(digitoAgencia))
                    agenciaConta += "-" + digitoAgencia;

                agenciaConta += "/" + conta;
                if (!string.IsNullOrEmpty(digitoConta))
                    agenciaConta += "-" + digitoConta;

                return agenciaConta;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formata o campo de acordo com o tipo e o tamanho 
        /// </summary>        
        public static string FitStringLength(string SringToBeFit, int maxLength, int minLength, char FitChar, int maxStartPosition, bool maxTest, bool minTest, bool isNumber)
        {
            try
            {
                string result = "";

                if (maxTest == true)
                {
                    // max
                    if (SringToBeFit.Length > maxLength)
                    {
                        result += SringToBeFit.Substring(maxStartPosition, maxLength);
                    }
                }

                if (minTest == true)
                {
                    // min
                    if (SringToBeFit.Length <= minLength)
                    {
                        if (isNumber == true)
                        {
                            result += (string)(new string(FitChar, (minLength - SringToBeFit.Length)) + SringToBeFit);
                        }
                        else
                        {
                            result += (string)(SringToBeFit + new string(FitChar, (minLength - SringToBeFit.Length)));
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Exception tmpEx = new Exception("Problemas ao Formatar a string. String = " + SringToBeFit, ex);
                throw tmpEx;
            }
        }

        /// <summary>
        ///  Indentifica tipo de documento
        ///       01 - CPF | 02 - CNPJ
        ///       Autor: Janiel Madureira Oliveira
        /// </summary>
        /// <param name="inscricao"></param>
        /// <returns></returns>
        public static string IdentificaTipoInscricaoSacado(string inscricao)
        {
            //Variaveis
            string tipo = string.Empty;
            //Tratamento
            inscricao = inscricao.Replace(".", "").Replace("-", "").Replace("/", "");
            //Verifica tipo
            if (inscricao.Length == 11)
            {
                tipo = "01"; //CPF
            }
            else if (inscricao.Length == 14)
            {
                tipo = "02"; // CNPJ
            }

            //Retorno
            return tipo;
        }

        public static string SubstituiCaracteresEspeciais(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var sb = new StringBuilder();
                var arrayChar = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char c in arrayChar)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                }
                return Regex.Replace(sb.ToString(), @"[^0-9a-zA-Z°ºª&¹²³.,\\@\- ]+", x => new string(' ', x.Length))
                    .Replace("ª", "a")
                    .Replace("º", "o")
                    .Replace("°", "o")
                    .Replace("&", "e")
                    .Replace("¹", "1")
                    .Replace("²", "2")
                    .Replace("³", "3");
            }
            return string.Empty;
        }

        /// <summary>
        /// Converte uma imagem em array de bytes.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToByte(Image image)
        {
            if (image == null)
                return null;

            byte[] bytes;
            if (image.GetType().ToString() == "System.Drawing.Image")
            {
                ImageConverter converter = new ImageConverter();
                bytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
                return bytes;
            }
            else if (image.GetType().ToString() == "System.Drawing.Bitmap")
            {
                bytes = (byte[])TypeDescriptor.GetConverter(image).ConvertTo(image, typeof(byte[]));
                return bytes;
            }
            else
                throw new NotImplementedException("ConvertImageToByte invalid type " + image.GetType().ToString());
        }

        internal static bool DataValida(DateTime dateTime)
        {
            if (dateTime.ToString("dd/MM/yyyy") == "01/01/1900" | dateTime.ToString("dd/MM/yyyy") == "01/01/0001")
                return false;
            else
                return true;
        }

        /// <summary>
        /// Retorna uma String com a qtde de casas pedidas da direita para a esquerda.
        /// <param name="seq">Sequencia de Dados</param>
        /// <param name="qtde">Quantidade de Char's à retornar</param>
        /// <param name="ch">Caracter que deseja usar para completar</param>
        /// <param name="completaPelaEsquerda">True: completa pela esquerda; False: completa pela direita</param>
        /// </summary>        
        public static string Right(string seq, int qtde, char ch, bool completaPelaEsquerda)
        {
            string final;
            final = Strings.Right(seq, qtde);
            return FitStringLength(final, qtde, qtde, ch, 0, true, true, completaPelaEsquerda);
            ;
        }

        public static string Transform(string text, string mask, char charMask = 'X')
        {
            string retorno = text;

            if (!string.IsNullOrEmpty(mask))
            {

                int idx = 0;
                foreach (var m in mask)
                {
                    if (m != charMask)
                    {
                        retorno = retorno.Insert(idx, m.ToString());
                    }
                    idx++;
                }

            }

            return retorno;
        }


        public static bool IsNullOrWhiteSpace(String value)
        {
            if (value == null) return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i])) return false;
            }

            return true;
        }
    }
}
