using System;
using BoletoNet;

public partial class Bancos_BancodoBrasil : System.Web.UI.Page
{
    void Page_PreInit(object sender, EventArgs e)
    {
        if (IsPostBack)
            MasterPageFile = "~/MasterPrint.master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime vencimento = DateTime.Now.AddDays(5);

        #region Exemplo Carteira 16, com nosso n�mero de 11 posi��es
        /*
         * Nesse exemplo utilizamos a carteira 16 e o nosso n�mero no m�ximo de 11 posi��es.
         * N�o � necess�rio informar o numero do conv�nio e nem o tipo da modalidade.
         * O nosso n�mero tem que ter no m�ximo 11 posi��es.
         */

        Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "1", "123456", "1");
        Boleto b = new Boleto(vencimento, 0.01m, "16", "09876543210", c);

        #endregion Exemplo Carteira 16, com nosso n�mero de 11 posi��es

        #region Exemplo Carteira 16, conv�nio de 6 posi��es e tipo modalidade 21
        /*
         * Nesse exemplo utilizamos a carteira 16 e o n�mero do conv�nio de 6 posi��es.
         * � obrigat�rio informar o tipo da modalidade 21.
         * O nosso n�mero tem que ter no m�ximo 10 posi��es.
         */

        //Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "1", "123456", "1");
        //c.Convenio = 123456;

        //Boleto b = new Boleto(vencimento, 0.01, "16", "09876543210", c);
        //b.TipoModalidade = "21";
        #endregion Exemplo Carteira 16, conv�nio de 6 posi��es e tipo modalidade 21

        #region Exemplo Carteira 18, com nosso n�mero de 11 posi��es
        /*
         * Nesse exemplo utilizamos a carteira 18 e o nosso n�mero no m�ximo de 11 posi��es.
         * N�o � necess�rio informar o numero do conv�nio e nem o tipo da modalidade.
         * O nosso n�mero tem que ter no m�ximo 11 posi��es.
         */

        //Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "1", "123456", "1");
        //Boleto b = new Boleto(vencimento, 0.01, "18", "09876543210", c);

        #endregion Exemplo Carteira 18, com nosso n�mero de 11 posi��es

        #region Exemplo Carteira 18, conv�nio de 6 posi��es e tipo modalidade 21
        /*
         * Nesse exemplo utilizamos a carteira 18 e o n�mero do conv�nio de 6 posi��es.
         * � obrigat�rio informar o tipo da modalidade 21.
         * O nosso n�mero tem que ter no m�ximo 10 posi��es.
         */

        //Cedente c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "1", "123456", "1");
        //c.Convenio = 123456;
        //Boleto b = new Boleto(vencimento, 0.01, "18", "09876543210", c);
        //b.TipoModalidade = "21";
        #endregion Exemplo Carteira 18, conv�nio de 6 posi��es e tipo modalidade 21


        b.NumeroDocumento = "12415487";

        b.Sacado = new Sacado("000.000.000-00", "Nome do seu Cliente ");
        b.Sacado.Endereco.End = "Endere�o do seu Cliente ";
        b.Sacado.Endereco.Bairro = "Bairro";
        b.Sacado.Endereco.Cidade = "Cidade";
        b.Sacado.Endereco.CEP = "00000000";
        b.Sacado.Endereco.UF = "UF";     

        //Adiciona as instru��es ao boleto
        #region Instru��es
        //Protestar
        Instrucao_BancoBrasil item = new Instrucao_BancoBrasil(9, 5);
        b.Instrucoes.Add(item);
        //ImportanciaporDiaDesconto
        item = new Instrucao_BancoBrasil(30, 0);
        b.Instrucoes.Add(item);
        //ProtestarAposNDiasCorridos
        item = new Instrucao_BancoBrasil(81, 15);
        b.Instrucoes.Add(item);
        #endregion Instru��es

        boletoBancario.Boleto = b;
        boletoBancario.Boleto.Valida();

        boletoBancario.MostrarComprovanteEntrega = (Request.Url.Query == "?show");
    }
}
