using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class HeaderDeArquivoCNAB240
    {
        #region Variáveis

        string _mensagemRemessa;
        string _numeroRemessa;
        string _dataRemessa;
        string _horaRemessa;

        #endregion

        #region Construtores

        public HeaderDeArquivoCNAB240()
        {
        }

        #endregion

        #region Propriedades

        public string MensagemRemessa
        {
            get { return _mensagemRemessa; }
            set { _mensagemRemessa = value; }
        }

        public string NumeroRemessa
        {
            get { return _numeroRemessa; }
            set { _numeroRemessa = value; }
        }
        public string DataRemessa
        {
            get { return _dataRemessa; }
            set { _dataRemessa = value; }
        }
        public string HoraRemessa
        {
            get { return _horaRemessa; }
            set { _horaRemessa = value; }
        }

        #endregion

        #region Métodos de Instância

        public void LerHeaderDeArquivoCNAB240(string Registro)
        {
            try
            {
                if (Registro.Substring(7, 1) != "0")
                    throw new Exception("Registro inválido. O detalhe não possuí as características de Header de Arquivo.");

                _mensagemRemessa = Registro.Substring(171, 20).Trim();
                _numeroRemessa = Utils.FormatCode(Registro.Substring(157, 6).Trim(), "0", 6);
                _dataRemessa = Convert.ToDecimal(Registro.Substring(143, 8)).ToString("00/00/0000");
                _horaRemessa = Convert.ToDecimal(Registro.Substring(151, 6)).ToString("00:00:00");

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - Header de Arquivo.", ex);
            }
        }

        #endregion
    }
}
