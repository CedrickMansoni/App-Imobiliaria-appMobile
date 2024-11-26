using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImoveisFavoritosViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public ImoveisFavoritosViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        //_= PegarImoveis("Disponível");
    }

    private ImovelModelResponse imovelDado = new();
    public ImovelModelResponse ImovelDado
    {
        get => imovelDado;
        set{
            imovelDado = value;
            OnPropertyChanged(nameof(ImovelDado));
        }
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

    public async Task PegarImoveis()
    {
        int clienteId = Convert.ToInt32(await SecureStorage.GetAsync("usuario_id"));
        var url = $"{UrlBase.UriBase.URI}listar/favoritos/{clienteId}";
        var response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {            
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {                             
               ImovelDados = await JsonSerializer.DeserializeAsync<ObservableCollection<ImovelModelResponse>>(responseStream, options);
            }
        }
    }

    public ICommand ActualizarPaginaCommand => new Command(async ()=>
    {
        ImovelDados = [];
        await PegarImoveis();
    });

    public ICommand ImovelDetailCommand => new Command<ImovelModelResponse>(async (ImovelModelResponse imovel)=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageImovelPublicadoDetailsFavorito(imovel, this));
    });

}
