/**
* ArquivoRemessaItauTeste.cs
*
* @visaoGeral      Rotina de teste dos erros conhecidos
*                  do arquivo de remessa depois de gerado o txt
* @autor           Celso Junior (celsojrfull[at]gmail.com)
* @dataCriacao     24 out 2016
* @alteradoPor     Nome (email ou website)
* @dataAlteracao   dd MMM yyyy
*
* Copyright(c) 2016 Celso Junior, celsojrfull[at]gmail.com
*
* Licensed under MIT-style license:
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.BancoItau
{
    [TestClass]
    [DeploymentItem(@"BancoItau\REMESSA.txt", "BancoItau")]
    public class ArquivoRemessaItauTeste
    {
        // Para testes locais simplesmente use: @C:\MeuArquivo.txt
        // Obs.: Talvez seja necessário abrir o Visual Studio com privilégios de Administrador
        const string arquivoRemessa = @"BancoItau\REMESSA.txt";
        
        [TestMethod]
        public void Itau_Carteira_109_Caminho_Arquivo_Teste()
        {
            Assert.IsTrue(File.Exists(arquivoRemessa), new FileNotFoundException().Message);
        }

        [TestMethod]
        public void Itau_Carteira_109_Tamanho_das_Linhas_Teste()
        {
            /**
             * @Problema: A quantidade de caracteres da linha ultrapassa 400 caracteres
             * @Solução: Remover o(s) caractere(s) ou espaço(s) excedente(s)
             */

            bool linhasCorretas = true;
            int count = 1;
            var relatorio = new StringBuilder("Linhas com mais de 400 caracteres:\n");

            foreach (string linha in File.ReadLines(arquivoRemessa))
            {
                if (linha.Count() > 400)
                {
                    linhasCorretas = false;
                    relatorio.Append(count + ",");
                }
                count++;
            }
            Assert.IsTrue(linhasCorretas, "\n" + relatorio.ToString().TrimEnd(','));
        }

        [TestMethod]
        public void Itau_Carteira_109_Valor_Negativo_Teste()
        {
            /**
             * @Problema: Foi dado um desconto maior ao que o cliente tem a pagar.
             * @Solução: Solicitar ao setor de cobrança que remova os descontos e faça a geração do arquivo de remessa novamente.
             */

            bool linhasCorretas = true;
            int linhaNumero = 1;

            var relatorio = new StringBuilder("Linhas com valor do título negativo:\n");

            foreach (string linha in File.ReadLines(arquivoRemessa))
            {
                var trecho = linha.Substring(120, 30);
                if (trecho.Contains('-'))
                {
                    linhasCorretas = false;
                    relatorio.Append(linhaNumero + ",");
                }
                linhaNumero++;
            }
            Assert.IsTrue(linhasCorretas, "\n" + relatorio.ToString().TrimEnd(','));
        }

        [TestMethod]
        public void Itau_Carteira_109_Caracteres_Especiais_Teste()
        {
            /**
             * Problema: Caracteres especiais e/ou não permitidos
             * Solução: Remover o(s) caractere(s) em questão
             */

            bool linhasCorretas = true;
            int count = 1;

            var relatorio = new StringBuilder("Linhas com caracteres especiais e/ou não permitidos:\n");

            foreach (string linha in File.ReadLines(arquivoRemessa))
            {
                var linhaOriginal = linha;
                var caracteresEspeciais = "éúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËÜÏÖÄçÇºª\\!@#%¨&*¹²³£¢¬§~´`[]{}\"'|?_";
                foreach (char item in caracteresEspeciais)
                {
                    if (linhaOriginal.Contains(item))
                    {
                        linhasCorretas = false;
                        relatorio.Append(string.Format("linha {0} col {1}\n", count, (linhaOriginal.IndexOf(item) + 1)));
                    }
                }
                count++;
            }
            Assert.IsTrue(linhasCorretas, "\n" + relatorio);
        }

        [TestMethod]
        public void Itau_Carteira_109_Detectar_Codificacao_do_Arquivo_Teste()
        {
            /**
             * Problema: Codificação do arquivo diferente do aceitável pelo banco Itaú
             * Solução: Abrir o arquivo de remessa pelo Bloco de Notas e salvar como ANSI
             * Obs.: Como isso raramente acontece, não foi criada nenhuma solução automatizada ainda
             */

            Assert.AreEqual(Encoding.Default, GetFileEncoding(arquivoRemessa));
        }

        [TestMethod]
        public void Itau_Carteira_109_Final_do_Arquivo_Teste()
        {
            /**
             * Problema: Não houve quebra de linha após a última linha
             * Solução: Adicionar uma quebra de linha pressionando 'Enter' após a linha que começa com o 9
             */

            var remessa = File.ReadAllText(arquivoRemessa);
            var isValid = remessa.EndsWith("\n");

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Itau_Carteira_109_Detectar_Tabulacoes_Teste()
        {
            /**
             * Problema: Provavelmente o arquivo foi editado manualmente
             * e foi utilizada a tecla TAB para completar os espaços em branco
             *
             * Solução: Abrir o arquivo com algum editor de texto que possa exibir as tabulações
             * como Notepad++, MS Word, etc. e substituir as tabulações por espaços. Ex.: \t vs "  "
             */

            var isValid = true;
            int count = 1;
            var relatorio = new StringBuilder("Linha com a tabulação:\n");
            foreach (string linha in File.ReadLines(arquivoRemessa))
            {
                if (linha.Contains("\t"))
                {
                    isValid = false;
                    relatorio.Append(string.Format("linha {0} col {1}\n", count, (linha.IndexOf("\t") + 1)));
                    break;
                }
                count++;
            }
            Assert.IsTrue(isValid, relatorio.ToString());
        }

        private static Encoding GetFileEncoding(string sourceFile)
        {
            Encoding enc = Encoding.Default;

            byte[] buffer = new byte[5];
            FileStream file = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                enc = Encoding.GetEncoding(1201);
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                enc = Encoding.GetEncoding(1200);

            return enc;
        }
    }
}