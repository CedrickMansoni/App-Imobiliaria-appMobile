using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ClienteViewModel;

public class NotificacoesClienteViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public NotificacoesClienteViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private ObservableCollection<ImovelModelResponse> imovelDados = new();
    public ObservableCollection<ImovelModelResponse> ImovelDados
    {
        get => imovelDados;
        set{
            imovelDados = value;
            OnPropertyChanged(nameof(ImovelDados));
        }
    }

    public async Task CarregarNotificacoes(int clienteId)
    {
        var url = $"{UrlBase.UriBase.URI}carregar/notificacoes/{clienteId}";
        var response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {            
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {                             
               ImovelDados = await JsonSerializer.DeserializeAsync<ObservableCollection<ImovelModelResponse>>(responseStream, options);
            }
        }
    }

}
