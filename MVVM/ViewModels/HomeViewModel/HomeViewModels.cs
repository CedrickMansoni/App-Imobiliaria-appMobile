using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.MVVM.ViewModels.Hash;
using App_Imobiliaria_appMobile.MVVM.Views;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.HomeViewModel;

public class HomeViewModels : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public HomeViewModels()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
    }
    

    public ICommand PerfilCommand => new Command( async ()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageFuncionarioPerfil());
    });

    public ICommand EditarSenhaCommand => new Command(async()=>
    {
        bool pergunta = await App.Current.MainPage.DisplayAlert("Alerta?", "Deseja realmente alterar a sua senha?", "Sim", "Não");
        if (pergunta)
        {
            string senhaNova = await App.Current.MainPage.DisplayPromptAsync("Digite a nova senha", ""); 
            if (!string.IsNullOrEmpty(senhaNova))
            {
                try
                {
                    senhaNova = new HashPassword().CriptografarSenha(senhaNova);
                    var funcionario = new Funcionario();
                    funcionario.Senha = senhaNova;
                    funcionario.Id = Convert.ToInt32(await SecureStorage.Default.GetAsync("usuario_id"));
                    var url = $"{UrlBase.UriBase.URI}editar/senha/funcionario";
                    string json = JsonSerializer.Serialize<Funcionario>(funcionario, options);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var Message = await response.Content.ReadAsStringAsync();
                        await App.Current.MainPage.DisplayAlert("Mensagem", $"{Message}","Ok"); 
                    }             
                }
                catch (System.Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", $"{ex}","Ok");
                }
            }           
        }        
    });

    public ICommand LogoutCommand => new Command( ()=>
    {
        SecureStorage.Default.RemoveAll();
        App.Current.MainPage = new ViewLogin();
    });



}
