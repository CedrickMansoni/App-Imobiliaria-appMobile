using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.MVVM.ViewModels.Hash;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ContaViewModel;

public class CriarContaViewModel : BindableObject
{

     HttpClient client;
    JsonSerializerOptions option;
    public CriarContaViewModel()
    {
        client = new HttpClient();
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }



    public ICommand BackCommand => new Command(async()=>
    { 
        await App.Current!.MainPage!.Navigation.PopModalAsync();
    });

    private ClienteSolicitante cliente = new();
    public ClienteSolicitante Cliente 
    { 
        get => cliente;
        set
        {
            cliente = value;
            OnPropertyChanged(nameof(Cliente));
        }
    }

    /*  Propiedade para Desabilitar e Habilitar a página de login durante o processamento de dados */
    private bool enablePage = true;
    public bool EnablePage {
        get => enablePage;
        set {
            enablePage = value;
            OnPropertyChanged(nameof(EnablePage));
        }
    }
    /* Propriedade para mostrar e activar a barra de carregamento */
    private bool activityPage = false;
    public bool ActivityPage {
        get => activityPage;
        set {
            activityPage = value;
            OnPropertyChanged(nameof(ActivityPage));
        }
    }

    void ButtonClicked(){
        EnablePage = !EnablePage;
        ActivityPage = !ActivityPage;
    }


     public ICommand CadClienteCommand => new Command(async ()=> 
    {
        try
        {   
            if(string.IsNullOrEmpty(Cliente.Nome))
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Informe o seu nome","Ok"); 
            }
            else if(string.IsNullOrEmpty(Cliente.Telefone))
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Informe o seu telefone","Ok"); 
            }
            else if(string.IsNullOrEmpty(Cliente.Senha))
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Informe a sua senha","Ok"); 
            }
            else
            {
                ButtonClicked();
                Cliente.Estado = "Activo";
                Cliente.Email = "cliente@gmail.com";
               
                var hash = new HashPassword();
                Cliente.Senha = hash.CriptografarSenha(Cliente.Senha);
                
                var url = $"{UrlBase.UriBase.URI}cadastrar/cliente/comprador";

                string json = JsonSerializer.Serialize<ClienteSolicitante>(Cliente, option);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    await App.Current!.MainPage!.DisplayAlert("Mensagem",$"{await response.Content.ReadAsStringAsync()}", "Ok");
                    await App.Current!.MainPage!.Navigation.PopModalAsync();
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Erro",$"{await response.Content.ReadAsStringAsync()}", "Ok"); 
                } 
                ButtonClicked();  
            }                  
        }
        catch (System.Exception ex)
        {
            await App.Current!.MainPage!.DisplayAlert("Erro", $"{ex}","Ok");
        }
    });
}
