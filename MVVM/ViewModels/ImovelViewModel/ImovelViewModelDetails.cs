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

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImovelViewModelDetails : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
  
    private PublicarImovelViewModel page;
    public ImovelViewModelDetails(ImovelModelResponse imovelDados, PublicarImovelViewModel page)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        ImovelDados = imovelDados;
        this.page = page;
    }

    private ImovelModelResponse imovelDados = new();
    public ImovelModelResponse ImovelDados
    {
        get => imovelDados;
        set{
            imovelDados = value;
            OnPropertyChanged(nameof(ImovelDados));
        }
    }

    private bool expandido = true;
    public bool Expandido
    {
        get => expandido;
        set{
            expandido = value;
            OnPropertyChanged(nameof(Expandido));
        }
    }

    private string expandidoIcone = "expandirmais";
    public string ExpandidoIcone
    {
        get => expandidoIcone;
        set{
            expandidoIcone = value;
            OnPropertyChanged(nameof(ExpandidoIcone));
        }
    }

    public ICommand ExpandirCommand => new Command(()=>
    {
        Expandido = !Expandido;
        if (Expandido)
        {
            ExpandidoIcone = "expandirmais";
        }
        else
        {
            ExpandidoIcone = "expandirmenos";
        }
    });

    public ICommand PublicarImovelCommand => new Command<ImovelModelResponse>(async(ImovelModelResponse imovel)=>
    {
        var avancar = await App.Current.MainPage.DisplayAlert("Alerta","Deseja publicar este imóvel?","Sim","Não");
        if (avancar)
        {
            try
            {
                var publicar = new Publicacao
                {
                    Codigo_Publicacao = imovel.Imovel.Codigo
                };

                var url = $"{UrlBase.UriBase.URI}publicar/imovel";
                string json = JsonSerializer.Serialize<Publicacao>(publicar, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Mensagem", $"{responseBody}","Ok"); 
                    page.RemoverImovelCommand.Execute(imovel);
                    await App.Current.MainPage.Navigation.PopAsync();
                }else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Erro", $"{errorMessage}","Ok"); 
                }
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta",$"{ex}", "Ok");
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Alerta","Erro","Ok");
        }
    });

    public ICommand RemoverImovelCommand => new Command<ImovelModelResponse>(async(ImovelModelResponse imovel)=>
    {
        var avancar = await App.Current.MainPage.DisplayAlert("Alerta","Deseja eliminar este imóvel?","Sim","Não");
        if (avancar)
        {
            var url = $"{UrlBase.UriBase.URI}eliminar/imovel/{imovel.Imovel.Codigo}";
           
            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Mensagem", $"{responseBody}","Ok"); 
                page.RemoverImovelCommand.Execute(imovel);
                await App.Current.MainPage.Navigation.PopAsync();
            }else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Erro", $"{errorMessage}\n{(int)response.StatusCode}","Ok"); 
            }
        }
    });
}
