using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls; 
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class TipoImovelViewModel : BindableObject
{
     HttpClient client;
    JsonSerializerOptions option;
    public TipoImovelViewModel()
    {
        client = new HttpClient();
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        _= ListarTiposImoveis();
    }

    private ObservableCollection<TipoImovel> tipoImovel = new();
    public ObservableCollection<TipoImovel> TipoImovel
    {
        get { return tipoImovel;}
        set 
        { 
            tipoImovel = value; 
            OnPropertyChanged(nameof(TipoImovel)); 
        }
    }

    private TipoImovel _tipoImovel = new();
    public TipoImovel _TipoImovel
    {
        get { return _tipoImovel;}
        set 
        { 
            _tipoImovel = value; 
            OnPropertyChanged(nameof(_TipoImovel)); 
        }
    }

    private async Task ListarTiposImoveis()
    {
        var url = $"{UrlBase.UriBase.URI}listar/tipo/imovel";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                TipoImovel = await JsonSerializer.DeserializeAsync<ObservableCollection<TipoImovel>>(responseStream, option);                
            }
        }
    }

    public ICommand ListarTiposCommand => new Command(async()=>
    {
        ButtonClicked();
        await ListarTiposImoveis();
        ButtonClicked();
    } );

    public ICommand CadastrarTipoImovel => new Command(async()=>
    {
        if(string.IsNullOrEmpty(_TipoImovel.TipoImovelDesc))
        {
            await App.Current.MainPage.DisplayAlert("Erro","Digite o tipo de imóvel","Ok");
        }else
        {
            ButtonClicked();
            var url = $"{UrlBase.UriBase.URI}cadastrar/tipo/imovel";
            string json = JsonSerializer.Serialize<TipoImovel>(_TipoImovel, option);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                ButtonClicked();
                var messageResponse = await response.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Mensagem de retorno", $"{messageResponse}","Ok"); 
                await ListarTiposImoveis();
                _TipoImovel = new();

            }else
            {
                ButtonClicked();
                await App.Current.MainPage.DisplayAlert("Erro", $"Não foi possível cadastrar o tipo de imóvel {_TipoImovel.TipoImovelDesc}","Ok"); 
            }
        }
    });

    public ICommand ActualizarTipoCommand => new Command<TipoImovel>(async (TipoImovel t)=>
    {
        string novoTipo = await App.Current.MainPage.DisplayPromptAsync("Informe o novo tipo imóvel",
                                    "",
                                    accept: "Editar",
                                    cancel: "Cancelar",
                                    placeholder: "Tipo de imóvel",
                                    maxLength: 80,
                                    keyboard: Keyboard.Text);

        if (!string.IsNullOrEmpty(novoTipo))
        {
            t.TipoImovelDesc = novoTipo;
            ButtonClicked();
            try
            {
                var url = $"{UrlBase.UriBase.URI}editar/tipo/imovel";
                string json = JsonSerializer.Serialize<TipoImovel>(t, option);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    await ListarTiposImoveis();
                }  else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", $"Não foi possível editar o tipo de imóvel","Ok");
                } 
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", $"{ex}","Ok");
            }    ;     
            ButtonClicked();
        }
        
    });

    public ICommand DeletarTipoCommand => new Command<TipoImovel>(async (TipoImovel tipo)=>
    {
        bool pergunta = await App.Current.MainPage.DisplayAlert("Alerta", "Você tem certeza de que deseja deletar este tipo de imóvel? Essa acção é irreversível.Por favor, confirme se deseja prosseguir.", "Sim", "Não");
        if (pergunta)
        {
            ButtonClicked();
            var url = $"{UrlBase.UriBase.URI}eliminar/tipo/imovel/{tipo.Id}";
            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                await ListarTiposImoveis();
            }
            ButtonClicked();
        }
        
    });

     /* Propriedade para mostrar e activar a barra de carregamento */
    private bool activityLoginPage = false;
    public bool ActivityLoginPage {
        get => activityLoginPage;
        set {
            activityLoginPage = value;
            OnPropertyChanged(nameof(ActivityLoginPage));
        }
    }

    void ButtonClicked(){
        ActivityLoginPage = !ActivityLoginPage;
    }

}
