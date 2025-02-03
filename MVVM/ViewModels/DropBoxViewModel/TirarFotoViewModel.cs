using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models.DropBox;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.DropBoxViewModel;

public class TirarFotoViewModel : BindableObject
{
    private string token = string.Empty;
    string codigoImovel = string.Empty;
    HttpClient client;
    JsonSerializerOptions options;
    public TirarFotoViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private ObservableCollection<FotosDropBox> fotos = new();
    public ObservableCollection<FotosDropBox> Fotos
    {
        get => fotos;
        set
        {
            fotos = value;
            OnPropertyChanged(nameof(Fotos));
        }
    }

    public ICommand AbrirCameraCommand => new Command(async () =>
    {
        var foto = await MediaPicker.CapturePhotoAsync();
        if (foto is not null)
        {
            var memoryStream = await foto.OpenReadAsync();
            Fotos.Add(new FotosDropBox { Id = Fotos.Count + 1, ImgSource = foto.FullPath });
        }
    });

    public ICommand AbrirGaleriaCommand => new Command(async () =>
    {
        var foto = await MediaPicker.PickPhotoAsync();
        if (foto is not null)
        {
            var memoryStream = await foto.OpenReadAsync();
            Fotos.Add(new FotosDropBox { Id = Fotos.Count + 1, ImgSource = foto.FullPath });
        }
    });

    public ICommand EnviarFotoCommand => new Command(async () =>
    {
        if (Fotos.Count == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Tire ou carregue fotos para podermos enviar para a storage YULA-IMOBILIÁRIA", "Ok");
        }
        else
        {
            codigoImovel = await App.Current.MainPage.DisplayPromptAsync("Cole o código do imóvel gerado no sistema.",
                                    "DropBox",
                                    accept: "Enviar foto(s)",
                                    cancel: "Cancelar",
                                    placeholder: "Código do imóvel",
                                    maxLength: 9,
                                    keyboard: Keyboard.Numeric);
            if (!string.IsNullOrEmpty(codigoImovel))
            {
                ChangeState();
                bool sending = false;
                if (Fotos.Count > 0)
                {
                    List<string> listaCaminhos = new();
                    try
                    {
                        foreach (var item in Fotos)
                        {
                            listaCaminhos.Add(item.ImgSource);
                        }
                        await UploadFile(listaCaminhos, codigoImovel);
                        sending = true;
                        Fotos = new();
                        codigoImovel = string.Empty;
                    }
                    catch
                    {
                        await App.Current!.MainPage!.DisplayAlert("Erro", $"Falha na conexão com o servidor de arquivos, por favor abra um ticket para o sector de comunicação e imagem.", "Ok");
                    }
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Erro", "Tire ou carregue fotos para podermos enviar para a storage YULA-IMOBILIÁRIA", "Ok");
                }
                if (sending)
                {
                    await App.Current!.MainPage!.DisplayAlert("Sucesso", "As fotos foram enviadas para a storage YULA-IMOBILIÁRIA", "Ok");
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Erro", "Não foi possível enviar as fotos para a storage YULA-IMOBILIÁRIA", "Ok");
                }
                ChangeState();
            }
        }
    });

    private async Task UploadFile(List<string> caminhosImagens, string codigo)
    {
        // URL da API
        var url = $"{UrlBase.UriBase.URI}upload/fotos/{codigo}";

        using (var formData = new MultipartFormDataContent())
        {
            foreach (var caminhoImagem in Fotos)
            {
                var mimeType = ObterTipoMime(caminhoImagem.ImgSource);

                var imagemStream = new StreamContent(File.OpenRead(caminhoImagem.ImgSource));
                imagemStream.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                // Adicionando a imagem ao formulário
                formData.Add(imagemStream, "files", Path.GetFileName(caminhoImagem.ImgSource));
            }

            // Enviando para a API
            HttpResponseMessage response = await client.PostAsync(url, formData);

            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Imagens enviadas com sucesso!", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Falha ao enviar imagens: " + response.StatusCode, "Ok");
                System.Console.WriteLine($"{response.StatusCode}");
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
            ".pdf" => "application/pdf",
            _ => "application/octet-stream", // Padrão para tipos desconhecidos
        };
    }

    private bool progressBar = false;
    public bool ProgressBar
    {
        get => progressBar;
        set
        {
            progressBar = value;
            OnPropertyChanged(nameof(ProgressBar));
        }
    }

    private bool viewPage = true;
    public bool ViewPage
    {
        get => viewPage;
        set
        {
            viewPage = value;
            OnPropertyChanged(nameof(ViewPage));
        }
    }

    private void ChangeState()
    {
        ProgressBar = !ProgressBar;
        ViewPage = !ViewPage;
    }

    public ICommand ActualizarToken => new Command(async () =>
    {

    });
}
