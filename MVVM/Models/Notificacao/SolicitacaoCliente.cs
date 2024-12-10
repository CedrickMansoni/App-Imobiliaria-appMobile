using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

public class SolicitacaoCliente
{
    public int Id { get; set; }

    public int IdClienteSolicitante { get; set; }

    public decimal PrecoMinimo { get; set; }

    public decimal PrecoMaximo { get; set; }

    public int IdTipoImovel { get; set; }

    public int Localizacao { get; set; }
}
