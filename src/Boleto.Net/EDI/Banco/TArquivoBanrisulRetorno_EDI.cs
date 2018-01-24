namespace BoletoNet.EDI.Banco
{
    /// <summary>
    /// Classe que ir� representar o arquivo EDI em si
    /// </summary>
    public class TArquivoBanrisulRetorno_EDI : TEDIFile
    {
        /*
		 * De modo geral, apenas preciso sobreescrever o m�todo de decodifica��o de linhas,
		 * pois preciso adicionar um objeto do tipo registro na cole��o do arquivo, passar a linha que vem do arquivo
		 * neste objeto novo, e decodific�-lo para separar nos campos.
		 * O DecodeLine � chamado a partir do m�todo LoadFromFile() (ou Stream) da classe base.
		 */
        protected override void DecodeLine(string Line)
        {
            base.DecodeLine(Line);
            Lines.Add(new TRegistroEDI_Banrisul_Retorno()); //Adiciono a linha a ser decodificada
            Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separa��o das substrings na linha do arquivo.
        }
    }
}