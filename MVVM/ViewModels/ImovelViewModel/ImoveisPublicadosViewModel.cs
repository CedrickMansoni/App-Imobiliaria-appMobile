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

public class ImoveisPublicadosViewModel : BindableObject
{
     HttpClient client;
    JsonSerializerOptions options;
    public ImoveisPublicadosViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        _= PegarImoveis("Disponível");
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
        Codigo = string.Empty;
        _= PegarImoveis("Disponível");
    });

    public ICommand ImovelDetailCommand => new Command<ImovelModelResponse>(async (ImovelModelResponse imovel)=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageImovelPublicadoDetails(imovel, this));
    });

     public ICommand PesquisarImovelCommand => new Command<string>(async(string codigo)=>
    {
        try
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                var url = $"{UrlBase.UriBase.URI}pesquisar/imovel/{codigo}";
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
        catch (System.Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro",$"{ex}", "Ok");
        }
    });

    private string codigo;
    public string Codigo
    {
        get => codigo;
        set{
            codigo = value;
            OnPropertyChanged(nameof(Codigo));
        }
    }
       
}
