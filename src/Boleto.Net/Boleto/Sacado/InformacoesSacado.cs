using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class InformacoesSacado: List<InfoSacado>  
    {
        /// <summary>
        /// Retorna HTML representativo de todo conteudo
        /// </summary>
        public String GeraHTML(Boolean novaLinha)
        {   
            String rtn = "";

            if (this.Count > 0)
            {
                foreach (InfoSacado I in this)
                {
                    rtn += I.HTML;
                }
                if (!novaLinha) rtn = rtn.Substring(6);
            }
            return rtn;
        }
    }
}
