using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImovelUploadViewModel : BindableObject
{
    private readonly ImovelModelDTO imovelDados;
    private static readonly Random random = new Random();
    HttpClient client;
    JsonSerializerOptions options;
    public ImovelUploadViewModel(ImovelModelDTO imovelDados)
    {
        this.imovelDados = imovelDados;
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
    }

    private bool pesquisarProprietario = false;
    public bool PesquisarProprietario
    {
        get => pesquisarProprietario;
        set{
            pesquisarProprietario = value;
            OnPropertyChanged(nameof(PesquisarProprietario));
        }
    }

    private string telefone;
    public string Telefone
    {
        get => telefone;
        set{
            if (telefone != value)
            {
                telefone = value;
                OnPropertyChanged(nameof(Telefone));
                if (Telefone.Length == 9)
                {
                    PesquisarProprietario = true;
                    _= GetProprietario();                
                }
                if (string.IsNullOrEmpty(Telefone) || Telefone.Length < 9)
                {
                    Nome = string.Empty;
                }
            }
        }
    }

    public async Task GetProprietario()
    {
        var url = $"{UrlBase.UriBase.URI}pegar/proprietario/{Telefone}";
        var response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {                             
                if (response.StatusCode == HttpStatusCode.OK)
                {                      
                    imovelDados.ClienteProprietario = await JsonSerializer.DeserializeAsync<ClienteProprietario>(responseStream, options);
                    await Task.Delay(4000);
                    Nome = imovelDados.ClienteProprietario.Nome;                   
                    PesquisarProprietario = false;
                }else
                {
                    PesquisarProprietario = false;
                    await App.Current.MainPage.DisplayAlert("Erro","Este número de telefone não corresponde a nenhuma conta cadastrada na YULA-IMOBILIÁRIA ", "Ok");
                }
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Alert","Erro http", "Ok");
        }
       
    }

    private string nome;
    public string Nome
    {
        get => nome;
        set{    
            nome = value;
            OnPropertyChanged(nameof(Nome));
        }
    }

    private string publicidade;
    public string Publicidade
    {
        get => publicidade;
        set{
            publicidade = value;
            OnPropertyChanged(nameof(Publicidade));
        }
    }

    private decimal preco;
    public decimal Preco
    {
        get => preco;
        set{
            preco = value;
            OnPropertyChanged(nameof(Preco));
        }
    }

    private string descricao;
    public string Descricao
    {
        get => descricao;
        set{
            descricao = value;
            OnPropertyChanged(nameof(Descricao));
        }
    }

    public ICommand UploadImovelCommand => new Command(async ()=>
    {
        if (string.IsNullOrEmpty(Telefone) && string.IsNullOrEmpty(Nome))
        {
            await App.Current.MainPage.DisplayAlert("Erro","Informe o telefone do proprietário","Ok");
        }else if(string.IsNullOrEmpty(Publicidade))
        {
            await App.Current.MainPage.DisplayAlert("Erro","Escolha o tipo de publicação","Ok");
        }else if (Preco == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Informe o preço do imóvel","Ok");
        }else if (string.IsNullOrEmpty(Descricao))
        {
            await App.Current.MainPage.DisplayAlert("Erro","Descreva o imóvel","Ok");      
        }else
        {
            imovelDados.Imovel.TipoPublicidade = Publicidade == "Arrendamento" ? 1: 2;
            imovelDados.Imovel.Preco = Preco;
            imovelDados.Imovel.Descricao = Descricao;
            imovelDados.Imovel.Codigo = GerarNumeroAleatorio().ToString();
            imovelDados.IdCorretor = Convert.ToInt32(await SecureStorage.Default.GetAsync("usuario_id"));

            var url = $"{UrlBase.UriBase.URI}cadastrar/imovel";
            string json = JsonSerializer.Serialize<ImovelModelDTO>(imovelDados, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var Message = await response.Content.ReadAsStringAsync();

                await App.Current.MainPage.DisplayAlert("Mensagem", $"Imóvel cadastrado com sucesso.\nFoi copiado para a área de transferência o código do imóvel\n{Message}","Ok"); 
                await Clipboard.SetTextAsync(Message);
                await App.Current.MainPage.Navigation.PopToRootAsync();    

            }else
            {
                var ErrorMessage = await response.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Erro", $"{ErrorMessage}","Ok");
            }
        }
    });

    public static int GerarNumeroAleatorio()
    {
        return random.Next(1000, 9999);
    }
}
