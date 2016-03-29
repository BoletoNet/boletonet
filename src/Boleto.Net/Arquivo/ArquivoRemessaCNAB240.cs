using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB240 : AbstractArquivoRemessa, IArquivoRemessa
    {

        #region Construtores

        public ArquivoRemessaCNAB240()
        {
            this.TipoArquivo = TipoArquivo.CNAB240;
        }

        #endregion

        #region Métodos de instância
        public override bool ValidarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            try
            {
                bool vRetorno = true;
                string vMsg = string.Empty;
                //
                foreach (Boleto boleto in boletos)
                {
                    string vMsgBol = string.Empty;
                    bool vRetBol = boleto.Banco.ValidarRemessa(this.TipoArquivo, numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsgBol);
                    if (!vRetBol && !String.IsNullOrEmpty(vMsgBol))
                    {
                        vMsg += vMsgBol;
                        vRetorno = vRetBol;
                    }
                }
                //
                mensagem = vMsg;
                return vRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string GerarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa)
        {
            try
            {
                int numeroRegistro = 0;
                int numeroRegistroDetalhe = 1;
                string strline;                
                var arquivoBuilder = new StringBuilder();

                if (banco.Codigo == 104) //quando é caixa verifica o modelo de leiatue que é está em boletos.remssa.tipodocumento
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB240, numeroArquivoRemessa, boletos[0]);
                else
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB240, numeroArquivoRemessa);

                numeroRegistro++;

                arquivoBuilder.AppendLine(strline);
                OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeArquivo);

                if (banco.Codigo == 104)//quando é caixa verifica o modelo de leiatue que é está em boletos.remssa.tipodocumento
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.CNAB240, boletos[0]);
                else
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.CNAB240);

                if (strline != string.Empty)
                {
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeLote);
                    numeroRegistro++;
                }

                if (banco.Codigo == 341)
                {
                    #region se Banco Itau - 341
                    foreach (Boleto boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;
                    }

                    numeroRegistro--;
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);
                    #endregion
                }
                else if (banco.Codigo == 104) // Só validar boleto.Remessa quando o banco for Caixa porque quando o banco for diferente de 104 a propriedade "Remessa" fica null
                {                    
                    #region se Banco Caixa - 104 e tipo de arquivo da remessa SIGCB
                    if ((boletos[0].Remessa.TipoDocumento.Equals("2")) || boletos[0].Remessa.TipoDocumento.Equals("1"))
                    {
                        foreach (Boleto boleto in boletos)
                        {
                            boleto.Banco = banco;
                            strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio, cedente);
                            arquivoBuilder.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;

                            strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, boleto.Sacado);
                            arquivoBuilder.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                            //segmento R não implementado...
                            //if (boleto.ValorMulta > 0)
                            //{
                            //    strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                            //    incluiLinha.WriteLine(strline);
                            //    OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                            //    numeroRegistro++;
                            //    numeroRegistroDetalhe++;
                            //}
                        }

                        numeroRegistro--;
                        strline = banco.GerarTrailerLoteRemessa(numeroRegistro, boletos[0]);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                        numeroRegistro++;
                        numeroRegistro++;

                        strline = banco.GerarTrailerArquivoRemessa(numeroRegistro, boletos[0]);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);
                    }
                    #endregion
                }
                else if (banco.Codigo == 237) // bradesco
                {
                    decimal totalTitulos = 0;
                    foreach (Boleto boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoARemessa(boleto, numeroRegistroDetalhe);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        strline = boleto.Banco.GerarDetalheSegmentoBRemessa(boleto, numeroRegistroDetalhe);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        totalTitulos += boleto.ValorBoleto;

                    }
                    numeroRegistro++;
                
                    strline = banco.GerarTrailerRemessaComDetalhes(numeroRegistro, boletos.Count,  TipoArquivo.CNAB240, cedente, totalTitulos);
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);
                }
                else //para qualquer outro banco, gera CNAB240 com segmentos abaixo
                {
                    #region outros bancos
                    foreach (Boleto boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                        arquivoBuilder.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boleto.ValorMulta > 0)
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                            arquivoBuilder.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }
                    
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;
                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    arquivoBuilder.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    #endregion                                 
                }

                return arquivoBuilder.ToString();
            } catch (Exception ex)
            {
                throw new Exception("Erro ao gerar arquivo remessa.", ex);
            }
        }
        #endregion

    }
}
