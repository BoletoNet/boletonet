using System;
using BoletoNet;

public partial class Bancos_Banestes : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "123", "1.222.333");

        Boleto b = new Boleto(vencimento, 2952.95m, "00", "21487805", c);

        b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
        b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
        b.Sacado.Endereco.Bairro = "Testando";
        b.Sacado.Endereco.Cidade = "Testelândia";
        b.Sacado.Endereco.CEP = "70000000";
        b.Sacado.Endereco.UF = "DF";

        b.NumeroDocumento = "18.030299.01";       

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        b.Instrucoes = new Instrucoes();

        #region Instrução Taxa Bancária
        
        IInstrucao instrucao1 = new Instrucao_Banestes();

        instrucao1.Descricao = "Taxa bancária R$ 2,95";

        b.Instrucoes.Add(instrucao1);
        #endregion

        #region Instrução Chave ASBACE
        IInstrucao instrucao2 = new Instrucao_Banestes();

        instrucao2.Descricao = string.Concat("CHAVE ASBACE: ", string.Format("{0} {1} {2} {3} {4}",
            b.Banco.ChaveASBACE.Substring(0, 5),
            b.Banco.ChaveASBACE.Substring(5, 5),
            b.Banco.ChaveASBACE.Substring(10, 5),
            b.Banco.ChaveASBACE.Substring(15, 5),
            b.Banco.ChaveASBACE.Substring(20, 5)));

        b.Instrucoes.Add(instrucao2);

        #endregion


        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
