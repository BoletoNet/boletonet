using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB100 : AbstractArquivoRetorno, IArquivoRetorno
    {
        public List<AbstractDetalheSegmento> ListaDetalhe { get; set; }

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            this.Banco = banco;
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = stream.ReadLine();

                // Próxima linha (DETALHE)
                while ((linha = stream.ReadLine()) != null)
                {
                    AbstractDetalheSegmento detalhe = DetalheRetornoCNAB100Factory.Create(linha);
                    ListaDetalhe.Add(detalhe);
                    OnLinhaLida(detalhe, linha);
                }

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }
    }
}
