namespace BoletoNet.EDI.Banco
{
    /// <summary>
    /// Classe que irá representar o arquivo EDI em si
    /// </summary>
    public class TArquivoBanrisulRetorno_EDI : TEDIFile
    {
        /*
		 * De modo geral, apenas preciso sobreescrever o método de decodificação de linhas,
		 * pois preciso adicionar um objeto do tipo registro na coleção do arquivo, passar a linha que vem do arquivo
		 * neste objeto novo, e decodificá-lo para separar nos campos.
		 * O DecodeLine é chamado a partir do método LoadFromFile() (ou Stream) da classe base.
		 */
        protected override void DecodeLine(string Line)
        {
            base.DecodeLine(Line);
            Lines.Add(new TRegistroEDI_Banrisul_Retorno()); //Adiciono a linha a ser decodificada
            Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
        }
    }
}