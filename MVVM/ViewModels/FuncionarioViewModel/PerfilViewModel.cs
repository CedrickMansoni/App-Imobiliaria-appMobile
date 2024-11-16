using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Microsoft.Maui.Controls; 
using App_Imobiliaria_appMobile.MVVM.Models;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.MVVM.Models.DropBox;

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

    private FotosDropBox fotoPerfil = new();
	public FotosDropBox FotoPerfil
	{
		get => fotoPerfil;
		set{
			fotoPerfil = value;
			OnPropertyChanged(nameof(FotoPerfil));
		}
	}

    private string avatar = string.Empty;
    public string Avatar{
        get => avatar;
        set {
            avatar = value;
            OnPropertyChanged(nameof(Avatar));
        } 
    }
    public ICommand AbrirCameraCommand => new Command(async()=>
    {
        var foto = await MediaPicker.CapturePhotoAsync();
		if (foto is not null)
		{
			var memoryStream = await foto.OpenReadAsync();
            FotoPerfil.Id = Convert.ToInt32(await SecureStorage.GetAsync("usuario_id"));
            FotoPerfil.ImgSource = foto.FullPath;
            Avatar = foto.FullPath;
		}
    });

    public ICommand AbrirGaleriaCommand => new Command(async()=>
    {
        var foto = await MediaPicker.PickPhotoAsync();
		if (foto is not null)
		{
			var memoryStream = await foto.OpenReadAsync();
			FotoPerfil.Id = Convert.ToInt32(await SecureStorage.GetAsync("usuario_id"));
            FotoPerfil.ImgSource = foto.FullPath;
            Avatar = foto.FullPath;
		}
    });

    public ICommand EnviarFotoCommand => new Command (async()=>
	{
        if (FotoPerfil.ImgSource is null)
        {
            await App.Current!.MainPage!.DisplayAlert("Erro","Tire ou carregue  a sua foto de perfil para podermos enviar para a storage YULA-IMOBILIÁRIA","Ok");
        }else
        {
            try
            {                    
                await UploadFile(FotoPerfil.ImgSource, FotoPerfil.Id);                       
            }
            catch 
            {
                await App.Current!.MainPage!.DisplayAlert("Erro",$"Falha na conexão com o servidor de arquivos, por favor abra um ticket para o sector de comunicação e imagem.","Ok");
            }           
        }               
	});

    private async Task UploadFile(string caminhoImagem, int id)
    {        
        // URL da API
        var url = $"{UrlBase.UriBase.URI}upload/foto/perfil/{id}";

        using (var formData = new MultipartFormDataContent())
        {
            var mimeType = ObterTipoMime(caminhoImagem);

            var imagemStream = new StreamContent(File.OpenRead(caminhoImagem));
            imagemStream.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

            // Adicionando a imagem ao formulário
            formData.Add(imagemStream, "perfil", Path.GetFileName(caminhoImagem));
            // Enviando para a API
            HttpResponseMessage response = await client.PostAsync(url, formData);

            if (response.IsSuccessStatusCode)
            {
                await App.Current!.MainPage!.DisplayAlert("Mensagem","A sua foto de perfil foi adicionado com sucesso!","Ok");
                CurrentePage();
            }
            else
            {
                var messageError = await response.Content.ReadAsStringAsync();
                await App.Current!.MainPage!.DisplayAlert("Mensagem","Falha ao enviar a sua foto de perfil: \n" + messageError, "Ok");
            }
        }       
    }
    // Método auxiliar para determinar o tipo MIME com base na extensão do arquivo
    private string ObterTipoMime(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream", // Padrão para tipos desconhecidos
        };
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

    private bool editarPerfil = false;
    public bool EditarPerfil
    {
        get => editarPerfil;
        set{
            editarPerfil = value;
            OnPropertyChanged(nameof(EditarPerfil));
        }
    }

    private void CurrentePage()
    {
        EditarPerfil = !EditarPerfil;
        if (EditarPerfil)
        {
            FotoCamera = "cancelar";
        }else
        {
            FotoCamera = "camera";
        }
    }

    public ICommand ActivePageCommand => new Command( () =>
    {
        CurrentePage();
    });

    private string fotoCamera = "camera";
    public string FotoCamera
    {
        get => fotoCamera;
        set{
            fotoCamera = value;
            OnPropertyChanged(nameof(FotoCamera));
        }
    }
}
