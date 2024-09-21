using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class Municipio
{
    public int Id { get; set; }

    public string NomeMunicipio { get; set; } = string.Empty;

    public int IdProvincia { get; set; }
}
