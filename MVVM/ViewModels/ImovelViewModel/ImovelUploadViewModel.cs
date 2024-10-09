using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImovelUploadViewModel : BindableObject
{
    private readonly ImovelModelDTO imovelDados;
    HttpClient client;
    JsonSerializerOptions options;
    public ImovelUploadViewModel(ImovelModelDTO imovelDados)
    {
        this.imovelDados = imovelDados;
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
    }

    private string telefone;
    public string Telefone
    {
        get => telefone;
        set{
            telefone = value;
            OnPropertyChanged(nameof(Telefone));
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
        if (string.IsNullOrEmpty(Telefone))
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
            imovelDados.ClienteProprietario.Telefone = Telefone;
            imovelDados.Imovel.TipoPublicidade = Publicidade;
            imovelDados.Imovel.Preco = Preco;
            imovelDados.Imovel.Descricao = Descricao;

            await App.Current.MainPage.DisplayAlert("Alert",$"{imovelDados.ClienteProprietario.Telefone}\n{imovelDados.Imovel.TipoPublicidade}\n{imovelDados.Imovel.Preco}\n{imovelDados.Imovel.Descricao}","Ok");
        }
    });
}
