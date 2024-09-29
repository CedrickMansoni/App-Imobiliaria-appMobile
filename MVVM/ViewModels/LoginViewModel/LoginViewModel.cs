using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.MVVM.Views.ShellGerente;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.LoginViewModel;

public class LoginViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public LoginViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
    }

    private UsuarioModelRequeste model = new UsuarioModelRequeste();
    public UsuarioModelRequeste Model
    {
        get => model;
        set 
        {
            model = value;
            OnPropertyChanged(nameof(Model));
        }
    }

    public ICommand FazerLoginCommand => new Command(async()=>
    {
        ButtonClicked();
        try
        {
            if (string.IsNullOrEmpty(Model.Dados.Telefone))
            {
                await App.Current.MainPage.DisplayAlert("Error message", "Informe o seu telefone","Ok");
            }
            else if(string.IsNullOrEmpty(Model.Dados.Senha))
            {
                await App.Current.MainPage.DisplayAlert("Error message", "Digite a sua senha","Ok");
            }else
            {
                var url = $"{UrlBase.UriBase.URI}funcionario";
                string json = JsonSerializer.Serialize<UsuarioModelRequeste>(Model, options);
                StringContent content = new StringContent(json,Encoding.UTF8 , "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    using(var responseStream = await response.Content.ReadAsStreamAsync()){
                        var data = await JsonSerializer.DeserializeAsync<UsuarioModelRequeste>(responseStream, options);
                        if (data.Dados is null)
                        {
                            await App.Current.MainPage.DisplayAlert("Erro", $"{data.Mensagem}","Ok");
                        }
                        else if(data.Estado != "Activo")                        
                        {
                            await App.Current.MainPage.DisplayAlert("Alerta", $"Usuário(a) {data.Dados.Nome} não tem permissão para acessar a plataforma.\nContacte o seu gerente para mais detalhes","Ok");
                        }
                        else if (data.UserType == "Gerente")
                        {
                            App.Current.MainPage = new GerenteShell();
                        }else
                        {
                            await App.Current.MainPage.DisplayAlert("Message", $"Página em construção","Ok");
                        }
                    }
                }else
                {
                    await App.Current.MainPage.DisplayAlert("Message", $"Erro na conexão com o servidor, por favor verifique o sinal de internet.","Ok");
                }
            }
        }
        catch (System.Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error message", $"{ex}","Ok");
        }
        ButtonClicked();
    });

    /* Propriedade para manter o usuário logado */
    private bool logado;
    public bool Logado{
        get => logado;
        set {
            logado = value;
            OnPropertyChanged(nameof(Logado));
        }
    }
    /* Propriedade para habiltar mostrar e ocultar a senha */
    private string eyePassword = "hide";
    public string EyePassword {
        get => eyePassword;
        set {
            eyePassword = value;
            OnPropertyChanged(nameof(EyePassword));
        }        
    }

    private bool showPassword = true;
    public bool ShowPassword {
        get => showPassword;
        set {
            showPassword = value;
            OnPropertyChanged(nameof(ShowPassword));
        }        
    }
    public ICommand PasswordCommand => new Command(()=>{
        if ("hide".Equals(EyePassword))
        {
            EyePassword = "show";            

        }else
        {
            EyePassword = "hide";
        }
        ShowPassword = !ShowPassword;
    });
    /*  Propiedade para Desabilitar e Habilitar a página de login durante o processamento de dados */
    private bool enableLoginPage = true;
    public bool EnableLoginPage {
        get => enableLoginPage;
        set {
            enableLoginPage = value;
            OnPropertyChanged(nameof(EnableLoginPage));
        }
    }
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
        EnableLoginPage = !EnableLoginPage;
        ActivityLoginPage = !ActivityLoginPage;
    }
}