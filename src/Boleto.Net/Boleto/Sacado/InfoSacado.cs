using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class InfoSacado 
    {
        String[] _data;

        /// <summary></summary>
        /// <param name="info">Texto da informação</param>
        public InfoSacado(String info)
        {
            _data = new String[] { info };
        }
        
        /// <summary></summary>
        /// <param name="linha1">Texto da primeira linha</param>
        /// <param name="linha2">Texto da segunda linha</param>
        public InfoSacado(String linha1, String linha2)
        {
            _data = new String[]{linha1,linha2};
        }

        /// <summary></summary>
        /// <param name="linhas">Vetor com as infomaçoes do Sacado, onde cada posição é uma linha da informação no boleto</param>
        public InfoSacado(String[] linhas) 
        {
            _data = linhas;
        }

        public String HTML
        {
            get
            {
                String rtn = "";
                foreach (String S in _data)
                {
                    rtn += "<br />" + S; 
                
                }
                return rtn;
            }
        }

        public static String Render(String linha1, String linha2, Boolean novaLinha)
        {
          return Render(new String[] { linha1, linha2 }, novaLinha);
        }

        public static String Render(String[] linhas, Boolean novaLinha)
        {
            String rtn = "";
            foreach (String S in linhas)
            {
                rtn += "<br />" + S;

            }
            if (!novaLinha) rtn = rtn.Substring(6);
            return rtn;
        }

    }
}
