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
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

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
                Todos = true;
                Model = await JsonSerializer.DeserializeAsync<ObservableCollection<UsuarioModelRequeste>>(responseStream, options);
            }
        }
    }
    
    public ICommand CadFuncCommand => new Command(async()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageFuncionarioCad(this));        
    });

    public ICommand EditFuncCommand => new Command<UsuarioModelRequeste>(async(UsuarioModelRequeste func)=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageFuncionarioUpdate(this, func));        
    });

    public bool todos = true;
    public bool Todos
    {
        get => todos;
        set{
            if (todos != value)
            {
                todos = value;
                OnPropertyChanged(nameof(Todos));
                if (value)
                {
                    _= ListarFuncionarios();
                }
            }
        }
    }
    public bool gerentes = false;
    public bool Gerentes
    {
        get => gerentes;
        set{
            if (gerentes != value)
            {
                gerentes = value;
                OnPropertyChanged(nameof(Todos));
                if (value)
                {
                    _= ListarFuncionariosPorCategoria("Gerente");
                }
            }
        }
    }
    public bool corretores = false;
    public bool Corretores
    {
        get => corretores;
        set{
            if (corretores != value)
            {
                corretores = value;
                OnPropertyChanged(nameof(Corretores));
                if (value)
                {
                    _= ListarFuncionariosPorCategoria("Corretor");
                }
            }
        }
    }

    public async Task ListarFuncionariosPorCategoria(string categoria)
    {
        var url = $"{UrlBase.UriBase.URI}listar/funcionarios/{categoria}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                Model = await JsonSerializer.DeserializeAsync<ObservableCollection<UsuarioModelRequeste>>(responseStream, options);
            }
        }
    }

}
