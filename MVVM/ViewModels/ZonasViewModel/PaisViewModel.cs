using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ZonasViewModel;

public class PaisViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public PaisViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        _= CarregarZonas();
    }
    private Pais pais {get; set;} = new Pais();
    public Pais Pais_ 
    {
        get => pais; 
        set{ pais = value; OnPropertyChanged(nameof(Pais_));}
    }

    private ObservableCollection<PaisModelRequest> paisLista = new();
    public ObservableCollection<PaisModelRequest> PaisLista
    {
        get => paisLista;
        set {
            paisLista = value;
            OnPropertyChanged(nameof(PaisLista));
        }
    }
    private async Task CarregarZonas()
    {
        var url = $"{UrlBase.UriBase.URI}listar/zonas";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                PaisLista = await JsonSerializer.DeserializeAsync<ObservableCollection<PaisModelRequest>>(responseStream, options);                
            }
        }
    }

    public ICommand CadastrarPaisCommand => new Command(async()=>
    {
        Pais_.NomePais = await App.Current.MainPage.DisplayPromptAsync("Informe o nome do país",
                                    "----------------------",
                                    accept: "Salvar",
                                    cancel: "Cancelar",
                                    placeholder: "Nome do Pais",
                                    maxLength: 80,
                                    keyboard: Keyboard.Text);
        if (!string.IsNullOrEmpty(Pais_.NomePais))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/pais";
            string json = JsonSerializer.Serialize<Pais>(Pais_, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                using(var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<Pais>(responseStream, options);
                    await App.Current.MainPage.DisplayAlert("Sucesso",$"País {data.NomePais}, foi cadastrado com sucesso","Ok");
                    CarregarZonas();
                }
            }else
            {
                await App.Current.MainPage.DisplayAlert("Message", $"Erro na conexão com o servidor, por favor verifique o sinal de internet.","Ok");
            }
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro", "Informe o nome do país","Ok");            
        }
    });

    private ObservableCollection<string> l = new();
        public ObservableCollection<string> L
        {
            get => l;
            set
            {
                if (l != value)
                {
                    l = value;
                    OnPropertyChanged(nameof(L));
                }
            }
        }
    public ICommand CadastrarProvinciaCommand => new Command(async ()=>
    {
        List<string> nomePaises = new List<string>();
        foreach (var paisItem in PaisLista)
        {
            nomePaises.Add(paisItem.Pais.NomePais);
        }
        string action = await App.Current.MainPage.DisplayActionSheet("Escolha o país - 1/2", "Cancelar", null, nomePaises.ToArray());
        if (action != "Cancelar")
        {
            var provincia = new Provincia();
            foreach (var item in PaisLista)
            {
                if (item.Pais.NomePais == action)
                {
                    provincia.IdPais = item.Pais.Id;
                    break;
                }
            }
            provincia.NomeProvincia = await App.Current.MainPage.DisplayPromptAsync("Informe o nome da província - 2/2",
                "",
                accept: "Salvar",
                cancel: "Cancelar",
                placeholder: "Nome da província",
                maxLength: 80,
                keyboard: Keyboard.Text);

            if (!string.IsNullOrEmpty(provincia.NomeProvincia))
            {
                var url = $"{UrlBase.UriBase.URI}cadastrar/provincia";
                string json = JsonSerializer.Serialize<Provincia>(provincia, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Mensagem", "Província cadastrada com sucesso","Ok"); 
                    CarregarZonas();
                }else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar a Província","Ok"); 
                }
            }
        }
    });

    public ICommand CadastrarMunicipioCommand => new Command(async ()=>
    {
        List<string> nomePaises = new List<string>();
        var provincia = new Provincia();

        foreach (var paisItem in PaisLista)
        {
            nomePaises.Add(paisItem.Pais.NomePais);
        }
        string action = await App.Current.MainPage.DisplayActionSheet("Escolha o país - 1/3", "Cancelar", null, nomePaises.ToArray());
        if (action != "Cancelar")
        {            
            foreach (var item in PaisLista)
            {
                if (item.Pais.NomePais == action)
                {
                    provincia.IdPais = item.Pais.Id;
                    break;
                }
            }
        }

        List<string> nomeProvincias = new List<string>();
        List<ProvinciaModelRequest> listaProvincias = new();
        foreach (var provinciaItem in PaisLista)
        {
            foreach (var item in provinciaItem.Provincia)
            {
                if (item.Provincia.IdPais == provincia.IdPais)
                {
                    listaProvincias.Add(item); 
                }
            }
            
            //nomeProvincias.Add(provinciaItem.Provincia);
        }
        foreach (var provinciaItem in listaProvincias)
        {
            nomeProvincias.Add(provinciaItem.Provincia.NomeProvincia);
        }
        string action2 = await App.Current.MainPage.DisplayActionSheet("Escolha a província - 2/3", "Cancelar", null, nomeProvincias.ToArray());
        
        if (action2 != "Cancelar")
        {            
            foreach (var item in listaProvincias)
            {
                if (item.Provincia.NomeProvincia == action2)
                {
                    provincia.Id = item.Provincia.Id; 
                    break;
                }
            }
        }
        var municipio = new Municipio(){IdProvincia = provincia.Id };
        municipio.NomeMunicipio = await App.Current.MainPage.DisplayPromptAsync("Informe o nome do município - 3/3",
        "",
        accept: "Salvar",
        cancel: "Cancelar",
        placeholder: "Nome do município",
        maxLength: 80,
        keyboard: Keyboard.Text);
    
        if (!string.IsNullOrEmpty(municipio.NomeMunicipio))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/municipio";
            string json = JsonSerializer.Serialize<Municipio>(municipio, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Município cadastrado com sucesso","Ok"); 
                CarregarZonas();
            }else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar o município","Ok"); 
            }
        }        
    });

    public ICommand CadastrarBairroCommand => new Command(async ()=>
    {
        List<string> nomePaises = new List<string>();
        var provincia = new Provincia();
        var municipio = new Municipio();

        foreach (var paisItem in PaisLista)
        {
            nomePaises.Add(paisItem.Pais.NomePais);
        }
        string action = await App.Current.MainPage.DisplayActionSheet("Escolha o país - 1/4", "Cancelar", null, nomePaises.ToArray());
        if (action != "Cancelar")
        {            
            foreach (var item in PaisLista)
            {
                if (item.Pais.NomePais == action)
                {
                    provincia.IdPais = item.Pais.Id;
                    break;
                }
            }

            List<string> nomeProvincias = new List<string>();
            List<ProvinciaModelRequest> listaProvincias = new();
            foreach (var provinciaItem in PaisLista)
            {
                foreach (var item in provinciaItem.Provincia)
                {
                    if (item.Provincia.IdPais == provincia.IdPais)
                    {
                        listaProvincias.Add(item); 
                    }
                }
            }
            foreach (var provinciaItem in listaProvincias)
            {
                nomeProvincias.Add(provinciaItem.Provincia.NomeProvincia);
            }
            string action2 = await App.Current.MainPage.DisplayActionSheet("Escolha a província - 2/4", "Cancelar", null, nomeProvincias.ToArray());
            
            if (action2 != "Cancelar")
            {            
                foreach (var item in listaProvincias)
                {
                    if (item.Provincia.NomeProvincia == action2)
                    {
                        provincia.Id = item.Provincia.Id; 
                        break;
                    }
                }

                List<string> nomeMunicipio = new List<string>();
                List<MunicipioModelRequest> listaMunicipio = new();
                foreach (var municipioItem in listaProvincias)
                {
                    foreach (var item in municipioItem.Municipio) 
                    {
                        if (item.Municipio.IdProvincia == provincia.Id)
                        {
                            listaMunicipio.Add(item); 
                        }
                    }
                }
                foreach (var municipioItem in listaMunicipio)
                {
                    nomeMunicipio.Add(municipioItem.Municipio.NomeMunicipio);
                }
                string action3 = await App.Current.MainPage.DisplayActionSheet("Escolha o municipio - 3/4", "Cancelar", null, nomeMunicipio.ToArray());
                
                if (action3 != "Cancelar")
                {  
                    var bairro = new Bairro();         
                    foreach (var item in listaMunicipio)
                    {
                        if (item.Municipio.NomeMunicipio == action3)
                        {
                            bairro.IdMunicipio = item.Municipio.Id; 
                            break;
                        }
                    }
                    bairro.NomeBairro = await App.Current.MainPage.DisplayPromptAsync("Informe o nome do município - 4/4",
                    "",
                    accept: "Salvar",
                    cancel: "Cancelar",
                    placeholder: "Nome do bairro",
                    maxLength: 80,
                    keyboard: Keyboard.Text);
    
                    if (!string.IsNullOrEmpty(bairro.NomeBairro))
                    {
                        var url = $"{UrlBase.UriBase.URI}cadastrar/bairro";
                        string json = JsonSerializer.Serialize<Bairro>(bairro, options);
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            await App.Current.MainPage.DisplayAlert("Mensagem","Bairro cadastrado com sucesso", "Ok");
                            CarregarZonas();
                        }
                    }
                }
            }
        }

        

/*

        var municipio = new Municipio(){IdProvincia = provincia.Id };
        municipio.NomeMunicipio = await App.Current.MainPage.DisplayPromptAsync("Informe o nome do município - 3/3",
        "",
        accept: "Salvar",
        cancel: "Cancelar",
        placeholder: "Nome do município",
        maxLength: 80,
        keyboard: Keyboard.Text);
    
        if (!string.IsNullOrEmpty(municipio.NomeMunicipio))
        {
            var url = $"{UrlBase.UriBase.URI}cadastrar/municipio";
            string json = JsonSerializer.Serialize<Municipio>(municipio, options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Mensagem", "Município cadastrado com sucesso","Ok"); 
                CarregarZonas();
            }else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar o município","Ok"); 
            }
        }  */      
    });
}
