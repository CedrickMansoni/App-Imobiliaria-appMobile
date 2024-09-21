using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls; 
using App_Imobiliaria_appMobile.MVVM.Models;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.FuncionarioViewModel;

public class FuncionarioViewModel : BindableObject
{
     HttpClient client;
    JsonSerializerOptions options;
    public FuncionarioViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        _= ListarFuncionarios();
    }

    private ObservableCollection<UsuarioModelRequeste> model = new ObservableCollection<UsuarioModelRequeste>();
    public ObservableCollection<UsuarioModelRequeste> Model
    {
        get => model;
        set 
        {
            model = value;
            OnPropertyChanged(nameof(Model));
        }
    }

    public async Task ListarFuncionarios()
    {
        var url = $"{UrlBase.UriBase.URI}listar/funcionarios";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                Model = await JsonSerializer.DeserializeAsync<ObservableCollection<UsuarioModelRequeste>>(responseStream, options);
            }
        }
    }

    public ICommand CadFuncCommand => new Command(async()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageFuncionarioCad(this));        
    });

}
