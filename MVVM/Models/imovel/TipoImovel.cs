using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class TipoImovel
{
    public int Id { get; set; }

    public string TipoImovelDesc { get; set; } = string.Empty;

    public bool Estado { get; set; }
}
