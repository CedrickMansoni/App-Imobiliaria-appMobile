using System;
using System.Text.Json.Serialization;

namespace App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

public class Mensagem
{
     [JsonPropertyName("accao")]
    public string Accao { get; set; } = string.Empty;

    [JsonPropertyName("chave_entidade")]
    public string ChaveEntidade { get; set; } = string.Empty;

    [JsonPropertyName("destinatario")]
    public string Destinatario { get; set; } = string.Empty;

    [JsonPropertyName("descricao_sms")]
    public string DescricaoSms { get; set; } = string.Empty;
}
