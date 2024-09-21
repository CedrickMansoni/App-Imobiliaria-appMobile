using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class ProvinciaModelRequest
{
    public Provincia Provincia {get; set;} = new();
    public int TotalMunicipio {get; set;}
    public List<MunicipioModelRequest> Municipio {get; set;}
}
