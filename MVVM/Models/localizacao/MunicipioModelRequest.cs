using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class MunicipioModelRequest
{
    public Municipio Municipio {get; set;} = new();
    public int TotalBairro {get; set;}
    public List<BairroModelRequest> Bairro {get; set;} = new();
}
