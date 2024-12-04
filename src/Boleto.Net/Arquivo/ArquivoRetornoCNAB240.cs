using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB240 : AbstractArquivoRetorno, IArquivoRetorno
    {
        private readonly Stream _streamArquivo;
        //private string _caminhoArquivo;
        private List<DetalheRetornoCNAB240> _listaDetalhes = new List<DetalheRetornoCNAB240>();

        #region Propriedades
        //public string CaminhoArquivo
        //{
        //    get { return _caminhoArquivo; }
        //}
        public Stream StreamArquivo
        {
            get { return _streamArquivo; }
        }
        public List<DetalheRetornoCNAB240> ListaDetalhes
        {
            get { return _listaDetalhes; }
            set { _listaDetalhes = value; }
        }
        #endregion Propriedades

        #region Construtores

        public ArquivoRetornoCNAB240()
        {
            this.TipoArquivo = TipoArquivo.CNAB240;
        }

        public ArquivoRetornoCNAB240(Stream streamArquivo)
        {
            this.TipoArquivo = TipoArquivo.CNAB240;
            _streamArquivo = streamArquivo;
        }

        public ArquivoRetornoCNAB240(string caminhoArquivo)
        {
            this.TipoArquivo = TipoArquivo.CNAB240;

            _streamArquivo = new StreamReader(caminhoArquivo).BaseStream;
        }
        #endregion

        #region M�todos de inst�ncia

        public void LerArquivoRetorno(IBanco banco)
        {
            LerArquivoRetorno(banco, StreamArquivo);
        }

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";
                string numeroRemessa = string.Empty;
                while ((linha = stream.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(linha))
                    {

                        DetalheRetornoCNAB240 detalheRetorno = new DetalheRetornoCNAB240();

                        switch (linha.Substring(7, 1))
                        {
                            case "0": //Header de arquivo
                                OnLinhaLida(null, linha, EnumTipodeLinhaLida.HeaderDeArquivo);
                                detalheRetorno.HeaderArquivo.LerHeaderDeArquivoCNAB240(linha);
                                numeroRemessa = detalheRetorno.HeaderArquivo.NumeroRemessa;
                                break;
                            case "1": //Header de lote
                                OnLinhaLida(null, linha, EnumTipodeLinhaLida.HeaderDeLote);
                                this.DetalheRetorno = new DetalheRetorno();
                                this.DetalheRetorno.CodigoBanco = int.Parse(linha.Substring(0, 3));
                                this.DetalheRetorno.NumeroSequencial = int.Parse(linha.Substring(183, 8));
                                if (!string.IsNullOrWhiteSpace(numeroRemessa) && (this.DetalheRetorno.NumeroSequencial == 0 || banco.Codigo == 748))
                                    this.DetalheRetorno.NumeroSequencial = int.Parse(numeroRemessa);
                                break;
                            case "3": //Detalhe
                                if (linha.Substring(13, 1) == "W")
                                {
                                    OnLinhaLida(detalheRetorno, linha, EnumTipodeLinhaLida.DetalheSegmentoW);
                                    detalheRetorno.SegmentoW.LerDetalheSegmentoWRetornoCNAB240(linha);
                                }
                                else if (linha.Substring(13, 1) == "E")
                                {
                                    OnLinhaLida(detalheRetorno, linha, EnumTipodeLinhaLida.DetalheSegmentoE);
                                    detalheRetorno.SegmentoE = new DetalheSegmentoERetornoCNAB240();
                                    detalheRetorno.SegmentoE.LerDetalheSegmentoERetornoCNAB240(linha);
                                }
                                else if (linha.Substring(13, 1) == "T")
                                {
                                    //Ir� ler o Segmento T e em sequencia o Segmento U
                                    detalheRetorno.SegmentoT = banco.LerDetalheSegmentoTRetornoCNAB240(linha);
                                    linha = stream.ReadLine();
                                    detalheRetorno.SegmentoU = banco.LerDetalheSegmentoURetornoCNAB240(linha);

                                    OnLinhaLida(detalheRetorno, linha, EnumTipodeLinhaLida.DetalheSegmentoU);

                                }
                                ListaDetalhes.Add(detalheRetorno);
                                break;
                            case "5": //Trailler de lote
                                OnLinhaLida(null, linha, EnumTipodeLinhaLida.TraillerDeLote);
                                break;
                            case "9": //Trailler de arquivo
                                OnLinhaLida(null, linha, EnumTipodeLinhaLida.TraillerDeArquivo);
                                break;
                        }

                    }

                }
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        #endregion
    }
}
