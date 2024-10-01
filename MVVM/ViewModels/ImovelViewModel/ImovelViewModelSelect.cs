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

public class ImovelViewModelSelect : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    private readonly ImovelCadViewModel cadImovel;
    public ImovelViewModelSelect(ImovelCadViewModel cadImovel)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        this.cadImovel = cadImovel;       
        _= CarregarPaises();
        
    }

    private ObservableCollection<Pais> listaPaises = new();
    public ObservableCollection<Pais> ListaPaises
    {
        get => listaPaises;
        set {
            listaPaises = value;
            OnPropertyChanged(nameof(ListaPaises));
        }
    }

    private Pais pais = new();
    public Pais Pais
    {
        get => pais;
        set {
            pais = value;
            OnPropertyChanged(nameof(Pais));
        }
    }

    private async Task CarregarPaises()
    {
        var url = $"{UrlBase.UriBase.URI}listar/paises";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {           
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                try{
                    var resposta = await JsonSerializer.DeserializeAsync<ImovelModelDTO>(responseStream, options);     
                    ListaPaises =new ObservableCollection<Pais>(resposta.Pais);                     
                }catch(System.Exception ex){
                    await App.Current.MainPage.DisplayAlert("Alerta - 05",$"Erro : {ex}","Ok");     
                }
            }
        }
    }

    public ICommand PaisSelecionadoCommand => new Command<Pais>(async(Pais paisClique)=>
    {
        cadImovel.GetPaisCommand.Execute(paisClique);
        await App.Current.MainPage.Navigation.PopModalAsync();
    });

    public ICommand CadastrarPaisCommand => new Command(async()=>
    {
        if (!string.IsNullOrEmpty(Pais.NomePais))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/pais";
            string json = JsonSerializer.Serialize<Pais>(Pais, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                using(var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<Pais>(responseStream, options);
                    await App.Current.MainPage.DisplayAlert("Sucesso",$"País {data.NomePais}, foi cadastrado com sucesso","Ok");
                    await CarregarPaises();
                }
            }else
            {
                await App.Current.MainPage.DisplayAlert("Message", $"Erro na conexão com o servidor, por favor verifique o sinal de internet.","Ok");
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome do país","Ok");            
        }
        Pais = new();
    });

    /* PROVÍNCIA ----------------------------------------------------------------------------------- */
    int idPais;
    public ImovelViewModelSelect(ImovelCadViewModel cadImovel, int idPais)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        this.cadImovel = cadImovel;
        this.idPais = idPais;
        _= CarregarProvincias(idPais);
    }
    private ObservableCollection<Provincia> listaProvincia = new();
    public ObservableCollection<Provincia> ListaProvincia
    {
        get => listaProvincia;
        set {
            listaProvincia = value;
            OnPropertyChanged(nameof(ListaProvincia));
        }
    }

    private Provincia provincia = new();
    public Provincia Provincia
    {
        get => provincia;
        set {
            provincia = value;
            OnPropertyChanged(nameof(Provincia));
        }
    }

    private async Task CarregarProvincias(int id)
    {
        var url = $"{UrlBase.UriBase.URI}listar/provincias/{id}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {           
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                try{
                    var resposta = await JsonSerializer.DeserializeAsync<ImovelModelDTO>(responseStream, options);     
                    ListaProvincia =new ObservableCollection<Provincia>(resposta.Provincia);                     
                }catch(System.Exception ex){
                    await App.Current.MainPage.DisplayAlert("Alerta - 05",$"Erro : {ex}","Ok");     
                }
            }
        }
    }

     public ICommand ProvinciaSelecionadaCommand => new Command<Provincia>(async(Provincia provinciaClique)=>
    {
        cadImovel.GetProvinciaCommand.Execute(provinciaClique);
        await App.Current.MainPage.Navigation.PopModalAsync();
    });

    public ICommand CadastrarProvinciaCommand => new Command(async ()=>
    {
        Provincia.IdPais = this.idPais;
        if (!string.IsNullOrEmpty(Provincia.NomeProvincia))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/provincia";
            string json = JsonSerializer.Serialize<Provincia>(provincia, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Província cadastrada com sucesso","Ok"); 
                await CarregarProvincias(idPais);
            }else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar a Província","Ok"); 
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome da província","Ok"); 
        }
        
    });

    /* MUNICÍPIO ----------------------------------------------------------------------------------- */
    int idProvincia;
    public ImovelViewModelSelect(ImovelCadViewModel cadImovel, int idPais, int idProvincia)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        this.cadImovel = cadImovel;
        this.idPais = idPais;
        this.idProvincia = idProvincia;
        _= CarregarMunicipios(idProvincia);
    }
    private ObservableCollection<Municipio> listaMunicipio = new();
    public ObservableCollection<Municipio> ListaMunicipio
    {
        get => listaMunicipio;
        set {
            listaMunicipio = value;
            OnPropertyChanged(nameof(ListaMunicipio));
        }
    }

    private Municipio municipio = new();
    public Municipio Municipio
    {
        get => municipio;
        set {
            municipio = value;
            OnPropertyChanged(nameof(Municipio));
        }
    }

    private async Task CarregarMunicipios(int id)
    {
        var url = $"{UrlBase.UriBase.URI}listar/municipios/{id}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {           
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                try{
                    var resposta = await JsonSerializer.DeserializeAsync<ImovelModelDTO>(responseStream, options);     
                    ListaMunicipio =new ObservableCollection<Municipio>(resposta.Municipio);                     
                }catch(System.Exception ex){
                    await App.Current.MainPage.DisplayAlert("Alerta - 05",$"Erro : {ex}","Ok");     
                }
            }
        }
    }

     public ICommand MunicipioSelecionadoCommand => new Command<Municipio>(async(Municipio municipioClique)=>
    {
        cadImovel.GetMunicipioCommand.Execute(municipioClique);
        await App.Current.MainPage.Navigation.PopModalAsync();
    });

    public ICommand CadastrarMunicipioCommand => new Command(async ()=>
    { 
        Municipio.IdProvincia = idProvincia;
        
        if (!string.IsNullOrEmpty(Municipio.NomeMunicipio))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/municipio";
            string json = JsonSerializer.Serialize<Municipio>(Municipio, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Município cadastrado com sucesso","Ok"); 
                await CarregarMunicipios(idProvincia);
            }else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar o município","Ok"); 
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome do município","Ok"); 
        }
        Municipio = new();        
    });

     /* BAIRRO ----------------------------------------------------------------------------------- */
    int idMunicipio;
    public ImovelViewModelSelect(ImovelCadViewModel cadImovel, int idPais, int idProvincia, int idMunicipio)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        this.cadImovel = cadImovel;
        this.idMunicipio = idMunicipio;
        _= CarregarBairros(idMunicipio);
    }
    private ObservableCollection<Bairro> listaBairro = new();
    public ObservableCollection<Bairro> ListaBairro
    {
        get => listaBairro;
        set {
            listaBairro = value;
            OnPropertyChanged(nameof(ListaBairro));
        }
    }

    private Bairro bairro = new();
    public Bairro Bairro
    {
        get => bairro;
        set {
            bairro = value;
            OnPropertyChanged(nameof(Bairro));
        }
    }

    private async Task CarregarBairros(int id)
    {
        var url = $"{UrlBase.UriBase.URI}listar/bairros/{id}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {           
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                try{
                    var resposta = await JsonSerializer.DeserializeAsync<ImovelModelDTO>(responseStream, options);     
                    ListaBairro =new ObservableCollection<Bairro>(resposta.Bairro);                     
                }catch(System.Exception ex){
                    await App.Current.MainPage.DisplayAlert("Alerta - 05",$"Erro : {ex}","Ok");     
                }
            }
        }
    }

     public ICommand BairroSelecionadoCommand => new Command<Bairro>(async(Bairro bairroClique)=>
    {
        cadImovel.GetBairroCommand.Execute(bairroClique);        
        await App.Current.MainPage.Navigation.PopModalAsync();
    });

    public ICommand CadastrarBairroCommand => new Command(async ()=>
    {  
        Bairro.IdMunicipio = this.idMunicipio;  
        if (!string.IsNullOrEmpty(Bairro.NomeBairro))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/bairro";
            string json = JsonSerializer.Serialize<Bairro>(bairro, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem","Bairro cadastrado com sucesso", "Ok");
                await CarregarBairros(idMunicipio);
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome do bairro","Ok"); 
        }
        Bairro = new();   
    });

     /* RUA ----------------------------------------------------------------------------------- */
    int idBairro;
    public ImovelViewModelSelect(ImovelCadViewModel cadImovel, int idPais, int idProvincia, int idMunicipio, int idBairro)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
        this.cadImovel = cadImovel;
        this.idBairro = idBairro;
        _= CarregarRuas(idBairro);
    }
    private ObservableCollection<Rua> listaRua = new();
    public ObservableCollection<Rua> ListaRua
    {
        get => listaRua;
        set {
            listaRua = value;
            OnPropertyChanged(nameof(ListaRua));
        }
    }

    private Rua rua = new();
    public Rua Rua
    {
        get => rua;
        set {
            rua = value;
            OnPropertyChanged(nameof(Rua));
        }
    }

    private async Task CarregarRuas(int id)
    {
        var url = $"{UrlBase.UriBase.URI}listar/ruas/{id}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {           
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                try{
                    var resposta = await JsonSerializer.DeserializeAsync<ImovelModelDTO>(responseStream, options);     
                    ListaRua =new ObservableCollection<Rua>(resposta.Rua);                     
                }catch(System.Exception ex){
                    await App.Current.MainPage.DisplayAlert("Alerta - 05",$"Erro : {ex}","Ok");     
                }
            }
        }
    }

     public ICommand RuaSelecionadaCommand => new Command<Rua>(async(Rua ruaClique)=>
    {
        cadImovel.GetRuaCommand.Execute(ruaClique);
        await App.Current.MainPage.Navigation.PopModalAsync();
    });

    public ICommand CadastrarRuaCommand => new Command(async ()=>
    {  
        Rua.IdBairro = this.idBairro;  
        if (!string.IsNullOrEmpty(Rua.NomeRua))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/rua";
            string json = JsonSerializer.Serialize<Rua>(Rua, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem","Rua cadastrada com sucesso", "Ok");
                await CarregarRuas(idBairro);
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome da rua","Ok"); 
        }
        Rua = new();   
    });

}
