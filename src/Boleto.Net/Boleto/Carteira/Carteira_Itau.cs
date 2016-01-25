using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_Itau
    {

        EscritualEletronicaSimples = 112,
        EscritualEletronicaSimplesNossoNumeroLivre = 115,
        EscritualEletronicaCarne = 104,
        EscritualEletronicaDolar = 147,
        EscritualEletronicaCobrancaInteligente = 188,
        DiretaEletronicaEmissaoIntegralCarne = 108,
        DiretaEletronicaSemEmissaoSimples = 109,
        DiretaEletronicaSemEmissaoDolar = 150,
        DiretaEletronicaEmissaoParcialSimples = 121,
        DiretaEletronicaEmissaoInegralSimples = 180,
        SemRegistroSemEmissaoComProtestoEletronico = 175,
        SemRegistroSemEmissao15Digitos = 198,
        SemRegistroSemEmissao15DigitosIOF4 = 142,
        SemRegistroSemEmissao15DigitosIOF7 = 143,
        SemRegistroEmissaoParcialComProtestoBordero = 174,
        SemRegistroEmissaoParcialComProtestoEletronico = 177,
        SemRegistroEmissaoParcialSegurosIOF2 = 129,
        SemRegistroEmissaoParcialSegurosIOF4 = 139,
        SemRegistroEmissaoParcialSegurosIOF7 = 169,
        SemRegistroEmissaoIntegral = 172,
        SemRegistroEmissaoIntegralCarne = 102,
        SemRegistroEmissaoIntegral15PosicoesCarne = 107,
        SemRegistroEmissaoEntrega = 173,
        SemRegistroEmissaoEntregaCarne = 103,
        SemRegistroEmissaoEntrega15Posicoes = 196,

    }

    #endregion 

    public class Carteira_Itau: AbstractCarteira, ICarteira
    {

        #region Construtores 

		public Carteira_Itau()
		{
			try
			{
                this.Banco = new Banco(341);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_Itau(int carteira)
        {
            try
            {
                this.carregar(carteira);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int carteira)
        {
            try
            {
                this.Banco = new Banco_Itau();

                switch ((EnumCarteiras_Itau)carteira)
                {
                    case EnumCarteiras_Itau.EscritualEletronicaSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.EscritualEletronicaSimples;
                        this.Codigo = "I";
                        this.Tipo = "E";
                        this.Descricao = "Escritural eletr�nica simples";
                        break;
                    case EnumCarteiras_Itau.EscritualEletronicaSimplesNossoNumeroLivre:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.EscritualEletronicaSimplesNossoNumeroLivre;
                        this.Codigo = "I";
                        this.Tipo = "E";
                        this.Descricao = "Escritural eletr�nica simples - Faixa nosso n�mero livre";
                        break;
                    case EnumCarteiras_Itau.EscritualEletronicaCarne:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.EscritualEletronicaCarne;
                        this.Codigo = "I";
                        this.Tipo = "E";
                        this.Descricao = "Escritural eletr�nica - Carn�";
                        break;
                    case EnumCarteiras_Itau.EscritualEletronicaDolar:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.EscritualEletronicaDolar;
                        this.Codigo = "E";
                        this.Tipo = "E";
                        this.Descricao = "Escritural eletr�nica - D�lar";
                        break;
                    case EnumCarteiras_Itau.EscritualEletronicaCobrancaInteligente:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.EscritualEletronicaCobrancaInteligente;
                        this.Codigo = "I";
                        this.Tipo = "E";
                        this.Descricao = "Escritural eletr�nica - Cobran�a inteligente";
                        break;
                    case EnumCarteiras_Itau.DiretaEletronicaEmissaoIntegralCarne:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.DiretaEletronicaEmissaoIntegralCarne;
                        this.Codigo = "I";
                        this.Tipo = "D";
                        this.Descricao = "Direta eletr�nica emiss�o integral - Carn�";
                        break;
                    case EnumCarteiras_Itau.DiretaEletronicaSemEmissaoSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.DiretaEletronicaSemEmissaoSimples;
                        this.Codigo = "I";
                        this.Tipo = "D";
                        this.Descricao = "Direta eletr�nica sem emiss�o - Simples";
                        break;
                    case EnumCarteiras_Itau.DiretaEletronicaSemEmissaoDolar:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.DiretaEletronicaSemEmissaoDolar;
                        this.Codigo = "U";
                        this.Tipo = "D";
                        this.Descricao = "Direta eletr�nica sem emiss�o - D�lar";
                        break;
                    case EnumCarteiras_Itau.DiretaEletronicaEmissaoParcialSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.DiretaEletronicaEmissaoParcialSimples;
                        this.Codigo = "I";
                        this.Tipo = "D";
                        this.Descricao = "Direta eletr�nica emiss�o parcial - Simples";
                        break;
                    case EnumCarteiras_Itau.DiretaEletronicaEmissaoInegralSimples:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.DiretaEletronicaEmissaoInegralSimples;
                        this.Codigo = "I";
                        this.Tipo = "D";
                        this.Descricao = "Direta eletr�nica emiss�o integral - Simples";
                        break;
                    case EnumCarteiras_Itau.SemRegistroSemEmissaoComProtestoEletronico:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroSemEmissaoComProtestoEletronico;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, sem emiss�o e com protesto eletr�nico";
                        break;
                    case EnumCarteiras_Itau.SemRegistroSemEmissao15Digitos:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroSemEmissao15Digitos;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro e sem emiss�o - 15 d�gitos";
                        break;
                    case EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF4:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF4;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro e sem emiss�o - 15 d�gitos IOF 4%";
                        break;
                    case EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF7:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF7;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro e sem emiss�o - 15 d�gitos IOF 7%";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoBordero:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoBordero;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o parcial com protesto border�";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoEletronico:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoEletronico;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o parcial com protesto eletr�nico";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF2:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF2;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o parcial, seguros com IOF 2%";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF4:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF4;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o parcial, seguros com IOF 4%";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF7:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF7;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o parcial, seguros com IOF 7%";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoIntegral:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoIntegral;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o integral";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoIntegralCarne:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoIntegralCarne;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o integral - Carn�";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoIntegral15PosicoesCarne:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoIntegral15PosicoesCarne;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, emiss�o integral - 15 poi��es - Carn�";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoEntrega:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoEntrega;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, com emiss�o e entrega";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoEntregaCarne:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoEntregaCarne;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, com emiss�o e entrega - Carn�";
                        break;
                    case EnumCarteiras_Itau.SemRegistroEmissaoEntrega15Posicoes:
                        this.NumeroCarteira = (int)EnumCarteiras_Itau.SemRegistroEmissaoEntrega15Posicoes;
                        this.Codigo = "I";
                        this.Tipo = "S";
                        this.Descricao = "Sem registro, com emiss�o e entrega - 15 posi��es";
                        break;
                    default:
                        this.NumeroCarteira = 0;
                        this.Codigo = " ";
                        this.Tipo = " ";
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static Carteiras CarregaTodas()
        {
            try
            {
                Carteiras alCarteiras = new Carteiras();

                Carteira_Itau obj;

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.EscritualEletronicaSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.EscritualEletronicaSimplesNossoNumeroLivre);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.EscritualEletronicaCarne);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.EscritualEletronicaDolar);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.EscritualEletronicaCobrancaInteligente);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.DiretaEletronicaEmissaoIntegralCarne);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.DiretaEletronicaSemEmissaoSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.DiretaEletronicaSemEmissaoDolar);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.DiretaEletronicaEmissaoParcialSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.DiretaEletronicaEmissaoInegralSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroSemEmissaoComProtestoEletronico);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroSemEmissao15Digitos);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF4);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroSemEmissao15DigitosIOF7);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoBordero);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoParcialComProtestoEletronico);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF2);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF4);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoParcialSegurosIOF7);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoIntegral);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoIntegralCarne);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoIntegral15PosicoesCarne);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoEntrega);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoEntregaCarne);
                alCarteiras.Add(obj);

                obj = new Carteira_Itau((int)EnumCarteiras_Itau.SemRegistroEmissaoEntrega15Posicoes);
                alCarteiras.Add(obj);

                return alCarteiras;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        #endregion

    }
}
