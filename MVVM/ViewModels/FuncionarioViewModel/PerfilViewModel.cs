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

public class PerfilViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public PerfilViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        _= GetPerfil();
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

    public async Task GetPerfil()
    {
        int userId = Convert.ToInt32(await SecureStorage.Default.GetAsync("usuario_id"));
        var url = $"{UrlBase.UriBase.URI}get/perfil/{userId}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                Model = await JsonSerializer.DeserializeAsync<UsuarioModelRequeste>(responseStream, options);
            }
        }
    }
}
