using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class PublicarImovelViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public PublicarImovelViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        _= PegarImoveis("Pendente");
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

    public async Task PegarImoveis(string estado)
    {
        var url = $"{UrlBase.UriBase.URI}listar/imoveis/{estado}";
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
        ImovelDados = new();
        await Task.Delay(4000);
        _= PegarImoveis("Pendente");
    });

    public ICommand ImovelDetailCommand => new Command<ImovelModelResponse>(async (ImovelModelResponse imovel)=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageImovelViewDetails(imovel, this));
    });

    public ICommand RemoverImovelCommand => new Command<ImovelModelResponse>((ImovelModelResponse imovel) => 
    {
        ImovelDados.Remove(imovel);
    });
}
