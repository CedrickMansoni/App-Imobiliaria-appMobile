using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class PaisModelRequest
{
    public Pais Pais {get; set;}
    public int TotalProvincia {get; set;}
    public int TotalMunicipio {get; set;}
    public int TotalBairro {get; set;}
    public List<ProvinciaModelRequest> Provincia {get; set;} = new();
}
