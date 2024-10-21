using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class Favorito
{
    public int Id {get; set;}
     public DateTime Data {get; set;}
    public string CodigoImovel {get; set;} = string.Empty;
    public int ClienteId { get; set; }
}
