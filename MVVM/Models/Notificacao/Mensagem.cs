using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

public class Mensagem
{
    public string Accao { get; set; } = "enviar_sms";

    public string ChaveEntidade { get; set; } = string.Empty;

    public string Destinatario { get; set; } = string.Empty;

    public string DescricaoSms { get; set; } = string.Empty;
}
