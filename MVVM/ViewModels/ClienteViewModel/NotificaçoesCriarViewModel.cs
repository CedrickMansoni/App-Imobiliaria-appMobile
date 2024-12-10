using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ClienteViewModel;

public class NotificaçoesCriarViewModel : BindableObject
{
    
    HttpClient client;
    JsonSerializerOptions options;
    public NotificaçoesCriarViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private SolicitacaoCliente notificacoes = new();
    public SolicitacaoCliente Notificacoes
    {
        get => notificacoes;
        set{
            notificacoes = value;
            OnPropertyChanged(nameof(Notificacoes));
        }
    }

    public async Task CadastrarNotificao()
    {
        var url = $"{UrlBase.UriBase.URI}cadastrar/notificao";
        
        var notificacao  = new SolicitacaoCliente()
        {
            IdClienteSolicitante = Convert.ToInt32(await SecureStorage.GetAsync("usuario_id")),
            PrecoMinimo = Notificacoes.PrecoMinimo,
            PrecoMaximo = Notificacoes.PrecoMaximo,
            IdTipoImovel = notificacoes.IdTipoImovel,
            Localizacao = Notificacoes.Localizacao,
        };

        string json = JsonSerializer.Serialize<SolicitacaoCliente>(notificacao, options);
        StringContent content = new StringContent(json, Encoding.UTF8, "applicatio/json");
        
        var response = await client.PostAsync(url, content);
        
        if (response.IsSuccessStatusCode)
        {            
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {                             
               //ImovelDados = await JsonSerializer.DeserializeAsync<ObservableCollection<ImovelModelResponse>>(responseStream, options);
            }
        }
    }
}
