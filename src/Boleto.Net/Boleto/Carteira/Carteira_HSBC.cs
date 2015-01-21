using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_HSBC
    {
        //CBA Caucionada HSBC Ativo
        CobrancaCaucionadaAtivo = 1,
        //CBS Caucionada HSBC Suspenso
        CobrancaCaucionadaSuspenso = 2,
        //CDA Caucionada Devedora Ativo
        CobrancaCaucionadaDevedoraAtivo = 3,
        //CDS Caucionada Devedora Suspenso
        CobrancaCaucionadaDevedoraSuspenso = 4,
        //CSB Cobrança Simples HSBC
        CobrancaSimplesComRegistro = 5,
        //CSE Cobrança Própria HSBC
        CobrancaPropria = 6,
        //CSF Cobrança Própria Financeira
        CobrancaPropriaFinanceira = 7,
        //CSI Cobrança Simples Investimentos
        CobrancaSimplesInvestimentos = 8,
        //CSS Cobrança Simples Seguro
        CobrancaSimplesSeguro = 9,
        //CNR Cobrança Simples Sem Registro
        CobrancaSimplesSemRegistro = 10,
    }
    #endregion Enumerado

    namespace BoletoNet.Boleto.Carteira
    {
        class Carteira_HSBC : AbstractCarteira, ICarteira
        {
            #region Construtores

            public Carteira_HSBC()
            {
                try
                {
                    this.Banco = new Banco(399);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao carregar objeto", ex);
                }
            }

            public Carteira_HSBC(int carteira)
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
                    this.Banco = new Banco_HSBC();

                    switch ((EnumCarteiras_HSBC)carteira)
                    {
                        case EnumCarteiras_HSBC.CobrancaCaucionadaAtivo:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaCaucionadaAtivo;
                            this.Codigo = "CBA";
                            this.Tipo = "";
                            this.Descricao = "CBA Caucionada HSBC Ativo";
                            break;
                        case EnumCarteiras_HSBC.CobrancaCaucionadaSuspenso:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaCaucionadaSuspenso;
                            this.Codigo = "CBS";
                            this.Tipo = "";
                            this.Descricao = "CBS Caucionada HSBC Suspenso";
                            break;
                        case EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraAtivo:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraAtivo;
                            this.Codigo = "CDA";
                            this.Tipo = "";
                            this.Descricao = "CDA Caucionada Devedora Ativo";
                            break;
                        case EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraSuspenso:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraSuspenso;
                            this.Codigo = "CDS";
                            this.Tipo = "";
                            this.Descricao = "CDS Caucionada Devedora Suspenso";
                            break;
                        case EnumCarteiras_HSBC.CobrancaSimplesComRegistro:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaSimplesComRegistro;
                            this.Codigo = "CSB";
                            this.Tipo = "";
                            this.Descricao = "CSB Cobrança Simples HSBC";
                            break;
                        case EnumCarteiras_HSBC.CobrancaSimplesSemRegistro:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaSimplesSemRegistro;
                            this.Codigo = "CNR";
                            this.Tipo = "";
                            this.Descricao = "CNR Cobrança NÃO Registro HSBC";
                            break;
                        case EnumCarteiras_HSBC.CobrancaPropria:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaPropria;
                            this.Codigo = "CSE";
                            this.Tipo = "";
                            this.Descricao = "CSE Cobrança Própria HSBC";
                            break;
                        case EnumCarteiras_HSBC.CobrancaPropriaFinanceira:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaPropriaFinanceira;
                            this.Codigo = "CSF";
                            this.Tipo = "";
                            this.Descricao = "CSF Cobrança Própria Financeira";
                            break;
                        case EnumCarteiras_HSBC.CobrancaSimplesInvestimentos:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaSimplesInvestimentos;
                            this.Codigo = "CSI";
                            this.Tipo = "";
                            this.Descricao = "CSI Cobrança Simples Investimentos";
                            break;
                        case EnumCarteiras_HSBC.CobrancaSimplesSeguro:
                            this.NumeroCarteira = (int)EnumCarteiras_HSBC.CobrancaSimplesSeguro;
                            this.Codigo = "CSS";
                            this.Tipo = "";
                            this.Descricao = "CSS Cobrança Simples Seguro";
                            break;
                        default:
                            this.NumeroCarteira = 0;
                            this.Codigo = " ";
                            this.Tipo = " ";
                            this.Descricao = "";
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

                    Carteira_HSBC obj;
                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaCaucionadaAtivo);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaCaucionadaSuspenso);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraAtivo);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaCaucionadaDevedoraSuspenso);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaSimplesComRegistro);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaSimplesSemRegistro);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaPropria);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaPropriaFinanceira);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaSimplesInvestimentos);
                    alCarteiras.Add(obj);

                    obj = new Carteira_HSBC((int)EnumCarteiras_HSBC.CobrancaSimplesSeguro);
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

}