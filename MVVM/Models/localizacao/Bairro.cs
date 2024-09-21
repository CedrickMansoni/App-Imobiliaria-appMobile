using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class Bairro
{
    public int Id { get; set; }

    public string NomeBairro { get; set; } = string.Empty;

    public int IdMunicipio { get; set; }
}

