using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BoletoNet
{
    /// <summary>
    /// Classe básica de um arquivo EDI
    /// </summary>
    public class TEDIFile
    {
        #region Variáveis Privadas e Protegidas
        #endregion

        #region Propriedades
        public List<TRegistroEDI> Lines = new List<TRegistroEDI>();
        #endregion

        #region Métodos Privados e Protegidos
        /// <summary>
        /// Decodifica a linha do registro EDI para os campos; O tipo de campo/registro EDI depende
        /// do layout da entidade.
        /// </summary>
        /// <param name="Line">Linha do arquivo a ser decodificada</param>
        protected virtual void DecodeLine(string Line)
        { 
        
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Carrega um arquivo EDI
        /// </summary>
        /// <param name="FileName">Nome do arquivo a ser carregado</param>
        public virtual void LoadFromFile(string FileName)
        {
            StreamReader sr = new StreamReader(FileName);
            this.Lines.Clear();
            while (!sr.EndOfStream)
            {
                this.DecodeLine(sr.ReadLine());
            }
            sr.Close();
            sr.Dispose();
        }

        public virtual void LoadFromStream(Stream s)
        {
            this.Lines.Clear();
            StreamReader sr = new StreamReader(s);
            while (!sr.EndOfStream)
            {
                this.DecodeLine(sr.ReadLine());
            }
            sr.Close();
            sr.Dispose();
        }

        /// <summary>
        /// Grava um arquivo EDI em disco
        /// </summary>
        /// <param name="FileName">Nome do arquivo EDI a ser salvo</param>
        public virtual void SaveToFile(string FileName)
        {
            StreamWriter sw = new StreamWriter(FileName);
            foreach (TRegistroEDI linha in this.Lines)
            {
                linha.CodificarLinha();
                sw.WriteLine(linha.LinhaRegistro);
            }
            sw.Close();
            sw.Dispose();
        }
        #endregion
    }
}
