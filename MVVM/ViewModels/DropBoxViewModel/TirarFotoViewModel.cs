using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
﻿using Dropbox.Api;
using Dropbox.Api.Files;
using App_Imobiliaria_appMobile.MVVM.Models.DropBox;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.DropBoxViewModel;

public class TirarFotoViewModel : BindableObject
{
    private string token = string.Empty;
    string telefoneProprietario = string.Empty;
    HttpClient client;
    JsonSerializerOptions options;
    public TirarFotoViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        _= PegarToken();
    }

    private async Task PegarToken()
    {
        var url = $"{UrlBase.UriBase.URI}pegar/token";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var t = await JsonSerializer.DeserializeAsync<Token>(responseStream, options);
                token = t.TokenAccess;                
            }
        }
    }

    private ObservableCollection<FotosDropBox> fotos = new();
	public ObservableCollection<FotosDropBox> Fotos
	{
		get => fotos;
		set{
			fotos = value;
			OnPropertyChanged(nameof(Fotos));
		}
	}


    public ICommand AbrirCameraCommand => new Command(async()=>
    {
        var foto = await MediaPicker.CapturePhotoAsync();
		if (foto is not null)
		{
			var memoryStream = await foto.OpenReadAsync();
			Fotos.Add(new FotosDropBox{ Id = Fotos.Count + 1, ImgSource = foto.FullPath});
		}
    });

    public ICommand AbrirGaleriaCommand => new Command(async()=>
    {
        var foto = await MediaPicker.PickPhotoAsync();
		if (foto is not null)
		{
			var memoryStream = await foto.OpenReadAsync();
			Fotos.Add(new FotosDropBox{ Id = Fotos.Count + 1, ImgSource = foto.FullPath});
		}
    });

    public ICommand EnviarFotoCommand => new Command (async()=>
	{
        telefoneProprietario = await App.Current.MainPage.DisplayPromptAsync("Informe o número de telefone do proprietário cadastrado no sistema.",
                                    "DropBox",
                                    accept: "Enviar foto(s)",
                                    cancel: "Cancelar",
                                    placeholder: "Telefone do proprietário",
                                    maxLength: 9,
                                    keyboard: Keyboard.Numeric);
        if (!string.IsNullOrEmpty(telefoneProprietario))
        {
            ChangeState();
            bool sending = false;
            if (Fotos.Count > 0)
            {
                try
                {
                    var dbx = new DropboxClient(token);
                    await UploadContent(dbx, "/content",$"{telefoneProprietario}-{DateTime.Now}.txt",$"Verificação da validade do Token - {DateTime.Now}");
                    foreach (var item in Fotos)
                    {                    
                        await UploadFile(dbx,$"{item.ImgSource}",$"/{telefoneProprietario}");                
                    }
                    sending = true;
                    Fotos = new();
                    telefoneProprietario = string.Empty;
                }
                catch (System.Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro",$"Falha na conexão com o servidor de arquivos, por favor abra um ticket para o sector de comunicação e imagem.","Ok");
                }
            }else
            {
                await App.Current.MainPage.DisplayAlert("Erro","Tire ou carregue fotos para podermos enviar para a storage YULA-IMOBILIÁRIA","Ok");
            }
            if (sending)
            {
                await App.Current.MainPage.DisplayAlert("Sucesso","As fotos foram enviadas para a storage YULA-IMOBILIÁRIA","Ok");
            }else{
                await App.Current.MainPage.DisplayAlert("Erro","Não foi possível enviar as fotos para a storage YULA-IMOBILIÁRIA","Ok");
            }
            ChangeState();
        }
        
	});

    private async Task UploadFile(DropboxClient dbx, string file, string folder)
    {
        var fileName = Path.GetFileName(file);
        using( var fileStream = File.OpenRead(file))
        {
            var result = await dbx.Files.UploadAsync($"{folder}/{fileName}", WriteMode.Overwrite.Instance, body:fileStream);
        }
    }

    public async Task UploadContent(DropboxClient dbx, string folder, string file, string content)
    {
        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
        {
            var result = await dbx.Files.UploadAsync($"{folder}/{file}", WriteMode.Overwrite.Instance,body:memoryStream);
        }
    }

    private bool progressBar = false;
    public bool ProgressBar
    {
        get => progressBar;
        set{
            progressBar = value;
            OnPropertyChanged(nameof(ProgressBar));
        }
    }

    private bool viewPage = true;
    public bool ViewPage
    {
        get => viewPage;
        set{
            viewPage = value;
            OnPropertyChanged(nameof(ViewPage));
        }
    }

    private void ChangeState()
    {
        ProgressBar = !ProgressBar;
        ViewPage = !ViewPage;
    }

    public ICommand ActualizarToken => new Command(async()=>{
        
        ChangeState();
        await PegarToken();
        ChangeState();

    });
}
