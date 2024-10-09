using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class NaturezaImovel
{
    public int Id { get; set; }

    public int IdTipoImovel { get; set; }

    public string Caracteristica { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

}

