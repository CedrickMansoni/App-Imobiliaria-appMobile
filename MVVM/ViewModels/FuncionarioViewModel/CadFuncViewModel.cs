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
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;
using App_Imobiliaria_appMobile.MVVM.ViewModels.Hash;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.FuncionarioViewModel;

public class CadFuncViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions option;
    private readonly FuncionarioViewModel page;
    public CadFuncViewModel(FuncionarioViewModel page)
    {
        client = new HttpClient();
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        this.page = page;
        Niveis = new ObservableCollection<Nivel>()
        {
            new Nivel{Id = 1, NivelAcesso = "Gerente"},
            new Nivel{Id = 2, NivelAcesso = "Corretor"},
        };  
        _= CarregarZonas();
    }
    

    private readonly UsuarioModelRequeste funcUpdate;
    public CadFuncViewModel(FuncionarioViewModel page, UsuarioModelRequeste funcionario)
    {
        client = new HttpClient();
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        this.page = page;
        Niveis = new ObservableCollection<Nivel>()
        {
            new Nivel{Id = 1, NivelAcesso = "Gerente"},
            new Nivel{Id = 2, NivelAcesso = "Corretor"},
        };  
        ButtonClicked();
        _= CarregarZonas();

        funcUpdate = funcionario;
        Funcionario_.Id = funcionario.Dados.Id;
        Funcionario_.Avatar = funcionario.Avatar;
        Funcionario_.Nome = funcionario.Dados.Nome;
        Funcionario_.Telefone = funcionario.Dados.Telefone;
        Funcionario_.Senha = funcionario.Dados.Senha;
        Funcionario_.Email = funcionario.Dados.Email;
        Funcionario_.Estado = funcionario.Estado;
        Activo_ = funcionario.Estado;
        Funcionario_.Nivel = funcionario.UserType;
        NivelSelected.NivelAcesso = Funcionario_.Nivel;
         ButtonClicked();
    }

    private ObservableCollection<Nivel> niveis = new();
    public ObservableCollection<Nivel> Niveis
    {
        get { return niveis;}
        set { 
                niveis = value; 
                OnPropertyChanged(nameof(Niveis)); 
            }
    }

    private Nivel nivelSelected = new();
    public Nivel NivelSelected
    {
        get { return nivelSelected;}
        set { 
                if(nivelSelected != value)
                {
                    nivelSelected = value;                    
                    Funcionario_.Nivel = nivelSelected.NivelAcesso; 
                    if (!string.IsNullOrEmpty(Funcionario_.Nivel))
                    {
                        NivelVisibilidade = true;
                    }else
                    {
                        NivelVisibilidade = false;
                    }                 
                }
                OnPropertyChanged(nameof(NivelSelected));
            }
    }
    private Funcionario funcionario = new();
    public Funcionario Funcionario_
    {
        get => funcionario;
        set{
            funcionario = value;
            OnPropertyChanged(nameof(Funcionario_));
            if (!string.IsNullOrEmpty(Funcionario_.Nivel))
            {
                NivelVisibilidade = true;
            }else
            {
                NivelVisibilidade = false;
            }
        }
    }

    public ICommand CadFuncCommand => new Command(async ()=> 
    {
        
        try
        {
            List<string> nomePaises = new List<string>();
            var provincia = new Provincia();

            foreach (var paisItem in PaisLista)
            {
                nomePaises.Add(paisItem.Pais.NomePais);
            }
            string action = await App.Current.MainPage.DisplayActionSheet("Selecione o país do funcionário - 1/2", "Cancelar", null, nomePaises.ToArray());
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
            }
            foreach (var provinciaItem in listaProvincias)
            {
                nomeProvincias.Add(provinciaItem.Provincia.NomeProvincia);
            }
            string action2 = await App.Current.MainPage.DisplayActionSheet("Selecione a província do funcionário - 2/2", "Cancelar", null, nomeProvincias.ToArray());
            
            if (action2 != "Cancelar")
            {            
                foreach (var item in listaProvincias)
                {
                    if (item.Provincia.NomeProvincia == action2)
                    {
                        provincia.Id = item.Provincia.Id;
                        Funcionario_.IdProvincia = provincia.Id; 
                        break;
                    }
                }
                ButtonClicked();
                Funcionario_.Estado = "Activo";
                string senha = new Random().Next(1000, 9000).ToString();
                Funcionario_.Senha = new HashPassword().CriptografarSenha(senha);

                var url = $"{UrlBase.UriBase.URI}cadastrar/funcionario";
                string json = JsonSerializer.Serialize<Funcionario>(Funcionario_, option);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Mensagem", "Funcionário cadastrado com sucesso","Ok"); 
                    try
                    {
                        if (Sms.Default.IsComposeSupported)
                        {
                            string[] recipients = new[] { $"{Funcionario_.Telefone}" };
                            string text = $"Olá {Funcionario_.Nome}, sua conta foi criada na YULA Imobiliária. Suas credenciais são: Telefone: {Funcionario_.Telefone}, Senha: {senha}. Por favor, altere sua senha após o primeiro login.";

                            var message = new SmsMessage(text, recipients);

                            await Sms.Default.ComposeAsync(message);
                        }
                        await page.ListarFuncionarios();
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    catch 
                    {
                        await App.Current.MainPage.DisplayAlert("Erro","Não foi possivel acessar o SmsMessage para envio dos dados da fatura para o cliente","Ok");
                    }
                }else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível cadastrar o Funcionário","Ok"); 
                }
            }                       
        }
        catch (System.Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", $"{ex}","Ok");
        }
        ButtonClicked();
    });

    private bool activo = false;
    public bool Activo
    {
        get => activo;
        set{
            if (activo != value)
            {
                activo = value;
                OnPropertyChanged(nameof(Activo));
                Activo_ = Activo == true ? "Activo" : "Inactivo";
            }           
        }
    }

    private string activo_ = "Inactivo";
    public string Activo_
    {
        get => activo_;
        set{
            if (activo_ != value)
            {
                activo_ = value;
                OnPropertyChanged(nameof(Activo_));
                App.Current.MainPage.DisplayAlert("Alerta",$"Usuário {Funcionario_.Nome} está {Activo_}","Ok");
                Activo = Activo_ == "Activo" ? true : false;
            }            
        }
    }

    public ICommand EditFuncCommand => new Command(async ()=> 
    {
        
        try
        {
            List<string> nomePaises = new List<string>();
            var provincia = new Provincia();

            foreach (var paisItem in PaisLista)
            {
                nomePaises.Add(paisItem.Pais.NomePais);
            }
            string action = await App.Current.MainPage.DisplayActionSheet("Selecione o país do funcionário - 1/2", "Cancelar", null, nomePaises.ToArray());
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
            }
            foreach (var provinciaItem in listaProvincias)
            {
                nomeProvincias.Add(provinciaItem.Provincia.NomeProvincia);
            }
            string action2 = await App.Current.MainPage.DisplayActionSheet("Selecione a província do funcionário - 2/2", "Cancelar", null, nomeProvincias.ToArray());
            
            if (action2 != "Cancelar")
            {            
                foreach (var item in listaProvincias)
                {
                    if (item.Provincia.NomeProvincia == action2)
                    {
                        provincia.Id = item.Provincia.Id;
                        Funcionario_.IdProvincia = provincia.Id; 
                        break;
                    }
                }
                ButtonClicked();
                Funcionario_.Estado = Activo_;
                string senha = funcUpdate.Dados.Senha;
                if (!string.IsNullOrEmpty(senha))
                {
                    Funcionario_.Senha = new HashPassword().CriptografarSenha(senha);

                }                
                var url = $"{UrlBase.UriBase.URI}editar/funcionario";
                string json = JsonSerializer.Serialize<Funcionario>(Funcionario_, option);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var Message = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Mensagem", $"{Message}","Ok"); 
                    await page.ListarFuncionarios();
                    await App.Current.MainPage.Navigation.PopAsync();
                }else
                {
                    var ErrorMessage = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Erro", $"{ErrorMessage}","Ok"); 
                }
            }                       
        }
        catch (System.Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", $"{ex}","Ok");
        }
        ButtonClicked();
    });

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
                PaisLista = await JsonSerializer.DeserializeAsync<ObservableCollection<PaisModelRequest>>(responseStream, option);                
            }
        }
    }

    private bool nivelVisibilidade = false;
    public bool NivelVisibilidade 
    {
        get => nivelVisibilidade;
        set
        {
            nivelVisibilidade = value;
            OnPropertyChanged(nameof(NivelVisibilidade));
        }
    }   

    /*  Propiedade para Desabilitar e Habilitar a página de login durante o processamento de dados */
    private bool enableLoginPage = true;
    public bool EnableLoginPage {
        get => enableLoginPage;
        set {
            enableLoginPage = value;
            OnPropertyChanged(nameof(EnableLoginPage));
        }
    }
    /* Propriedade para mostrar e activar a barra de carregamento */
    private bool activityLoginPage = false;
    public bool ActivityLoginPage {
        get => activityLoginPage;
        set {
            activityLoginPage = value;
            OnPropertyChanged(nameof(ActivityLoginPage));
        }
    }

    void ButtonClicked(){
        EnableLoginPage = !EnableLoginPage;
        ActivityLoginPage = !ActivityLoginPage;
    }
}
