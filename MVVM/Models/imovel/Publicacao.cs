using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class Publicacao
{
    public int Id { get; set; }

    public int IdCorretor { get; set; }

    public string CodigoImovel { get; set; } = string.Empty;

    public DateTime DataPublicacao { get; set; }

    public int Gostei { get; set; }

    public int NaoGostei { get; set; }

    public int TotalComentarios { get; set; }

    public bool Estado { get; set; }

    public DateTime DataConclusao { get; set; }
}

