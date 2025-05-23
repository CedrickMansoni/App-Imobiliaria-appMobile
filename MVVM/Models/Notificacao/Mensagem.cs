using System;
using System.Text.Json.Serialization;

namespace App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

public class Mensagem
{

    [JsonPropertyName("api_key_app")]
    public string ChaveEntidade { get; set; } = string.Empty;

    [JsonPropertyName("phone_number")]
    public string Destinatario { get; set; } = string.Empty;

    [JsonPropertyName("message_body")]
    public string DescricaoSms { get; set; } = string.Empty;
}
