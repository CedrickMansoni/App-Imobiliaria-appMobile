using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class Foto
{
    public int Id { get; set; }

    public string Imagem { get; set; } = string.Empty;

    public string CodigoImovel { get; set; } = string.Empty;
}
