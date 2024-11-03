using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls; 
using App_Imobiliaria_appMobile.MVVM.Models;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.MVVM.ViewModels.Hash;
using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ClienteViewModel;

public class ProprietarioViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions option;
    public ProprietarioViewModel()
    {
        client = new HttpClient();
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private ClienteProprietario cliente = new();
    public ClienteProprietario Cliente
    {
        get => cliente;
        set{
            cliente = value;
            OnPropertyChanged(nameof(Cliente));
        }
    }

    public ICommand CadClienteCommand => new Command(async ()=> 
    {
        try
        {   
            if(string.IsNullOrEmpty(Cliente.Nome))
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Informe o nome do cliente","Ok"); 
            }
            else if(string.IsNullOrEmpty(Cliente.Telefone))
            {
                await App.Current!.MainPage!.DisplayAlert("Erro", "Informe o telefone do cliente","Ok"); 
            }
            else
            {
                ButtonClicked();
                Cliente.Estado = "Activo";
                string senha = new Random().Next(1000, 9000).ToString();
                var hash = new HashPassword();
                Cliente.Senha = hash.CriptografarSenha(senha);
                if(string.IsNullOrEmpty(Cliente.Email))
                    Cliente.Email = "cliente@gmail.com";

                var url = $"{UrlBase.UriBase.URI}cadastrar/proprietario";
                string json = JsonSerializer.Serialize<ClienteProprietario>(Cliente, option);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        if (Sms.Default.IsComposeSupported)
                        {
                            string[] recipients = new[] { $"{Cliente.Telefone}" };
                            string text = $"Olá {Cliente.Nome}, sua conta foi criada na YULA Imobiliária. Suas credenciais são: Telefone: {Cliente.Telefone}, Senha: {senha}. Por favor, altere sua senha após o primeiro login.";
                            
                            var message = new SmsMessage(text, recipients);

                            await Sms.Default.ComposeAsync(message);
                        }
                        await App.Current!.MainPage!.Navigation.PopAsync();
                    }
                    catch 
                    {
                        await App.Current!.MainPage!.DisplayAlert("Erro","Não foi possivel acessar o SmsMessage para envio dos dados da fatura para o cliente","Ok");
                    }
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Erro", "Não foi possível cadastrar o Funcionário","Ok"); 
                } 
                ButtonClicked();  
            }                  
        }
        catch (System.Exception ex)
        {
            await App.Current!.MainPage!.DisplayAlert("Erro", $"{ex}","Ok");
        }
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
