using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

[Table("tabela13_foto")]
public class Foto
{
    [Column("id")]
    public int Id { get; set; }

    [Column("imagem")]
    public string Imagem { get; set; } = string.Empty;

    [Column("id_imovel")]
    public int IdImovel { get; set; }
}
