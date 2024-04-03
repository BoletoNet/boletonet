using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public interface IBanco
    {
        /// <summary>
        /// Formata o código de barras
        /// </summary>
        void FormataCodigoBarra(Boleto boleto);
        /// <summary>
        /// Formata a linha digital
        /// </summary>
        void FormataLinhaDigitavel(Boleto boleto);
        /// <summary>
        /// Formata o nosso número
        /// </summary>
        void FormataNossoNumero(Boleto boleto);
        /// <summary>
        /// Formata o número do documento, alguns bancos exige uma formatação. Tipo: 123-4
        /// </summary>
        void FormataNumeroDocumento(Boleto boleto);        
        /// <summary>
        /// Responsável pela validação de todos os dados referente ao banco, que serão usados no boleto
        /// </summary>
        void ValidaBoleto(Boleto boleto);
        /// <summary>
        /// Gera o header do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o header do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa
        /// </summary>
        string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o header de arquivo do arquivo de remessa
        /// </summary>
        string GerarHeaderRemessa(Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o Trailer do arquivo de remessa
        /// </summary>
        string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal);
        /// <summary>
        /// Gera o Trailer do arquivo de remessa, com total de registros detalhes
        /// </summary>
        string GerarTrailerRemessaComDetalhes(int numeroRegistro, int numeroRegistroDetalhe, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o header de lote do arquivo de remessa
        /// </summary>
        string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio);

        string GerarDetalheSegmentoARemessa(Boleto boleto, int numeroRegistro);
        string GerarDetalheSegmentoBRemessa(Boleto boleto, int numeroRegistro);

        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio,Cedente cedente);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO P
        /// </summary>
        string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente, Boleto boletos);                
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO Q
        /// </summary>
        string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO Q
        /// </summary>
        string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO R
        /// </summary>
        string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera os registros de detalhe do arquivo de remessa - SEGMENTO S
        /// </summary>
        string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo);
        /// <summary>
        /// Gera o Trailer de arquivo do arquivo de remessa
        /// </summary>
        string GerarTrailerArquivoRemessa(int numeroRegistro);
        /// <summary>
        /// Gera o Trailer de arquivo do arquivo de remessa
        /// </summary>
        string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos);
        /// <summary>
        /// Gera o Trailer de lote do arquivo de remessa
        /// </summary>
        string GerarTrailerLoteRemessa(int numeroRegistro);
        /// <summary>
        /// Gera o Trailer de lote do arquivo de remessa
        /// </summary>
        string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos);
        /// <summary>
        /// Gera os registros de Mensagem Variavel do arquivo de remessa
        /// </summary>
        string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo);

        DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro);

        DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro);

        DetalheSegmentoYRetornoCNAB240 LerDetalheSegmentoYRetornoCNAB240(string registro);

        DetalheSegmentoWRetornoCNAB240 LerDetalheSegmentoWRetornoCNAB240(string registro);

        DetalheRetorno LerDetalheRetornoCNAB400(string registro);
        HeaderRetorno LerHeaderRetornoCNAB400(string registro);

        Cedente Cedente { get; }
        int Codigo { get; set;}
        string Nome { get; }
        string Digito { get; }

        bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem);

        string ChaveASBACE { get; set; }

        /// <summary>
        /// Obtém nosso número sem DV e sem código do Convênio.
        /// </summary>
        /// <returns></returns>
        long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero);
    }
}
