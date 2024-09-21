using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.localizacao;

public class Rua
{
    public int Id { get; set; }

    public string NomeRua { get; set; } = string.Empty;

    public int IdBairro { get; set; }
}

