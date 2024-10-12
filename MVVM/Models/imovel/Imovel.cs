using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class Imovel
{
    public int Id { get; set; }

     public string Codigo { get; set; } = string.Empty;

    public int IdClienteProprietario { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataSolicitacao { get; set; }

    public string Estado { get; set; } = string.Empty;

    public int TipoPublicidade { get; set; } 

    public decimal Preco { get; set; }

    public int IdNaturezaImovel { get; set; }

    public int IdLocalizacao { get; set; }
}