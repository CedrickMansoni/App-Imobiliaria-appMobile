using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.HomePage;

public class HomePageModels
{
    public int FuncionarioTotal { get; set; }
    public int FuncionarioActivos { get; set; }
    public int FuncionarioInactivos { get; set; }
    public int FuncionarioGerentes { get; set; }
    public int FuncionarioCorretores { get; set; }
    /*-------------------------------------------*/
    public int ClienteTotal { get; set; }
    public int ClienteProprietarios { get; set; }
    public int ClienteConsumidores { get; set; }
    /*-------------------------------------------*/
    public int ImoveisCadastrados { get; set; }
    public int ImoveisPendentes { get; set; }
    public int ImoveisDisponiveis { get; set; }
    public int ImoveisPublicados { get; set; }
    /*-------------------------------------------*/
    public int ImoveisParaVenda { get; set; }
    public int ImoveisParaArrendamento { get; set; }
    /*-------------------------------------------*/
    public int ImoveisTotalVendidos { get; set; }
    public int ImoveisTotalArrendados { get; set; }
    /*-------------------------------------------*/
    public decimal ValorArrendamento { get; set; }
    public decimal ValorVendas { get; set; }
}
