using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.Notificacao;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ClienteViewModel;

public class NotificacoesClienteViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public NotificacoesClienteViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private ObservableCollection<ImovelModelResponse> imovelDados = new();
    public ObservableCollection<ImovelModelResponse> ImovelDados
    {
        get => imovelDados;
        set{
            imovelDados = value;
            OnPropertyChanged(nameof(ImovelDados));
        }
    }

    public async Task CarregarNotificacoes(int clienteId)
    {
        var url = $"{UrlBase.UriBase.URI}carregar/notificacoes/{clienteId}";
        var response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {            
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {                             
               ImovelDados = await JsonSerializer.DeserializeAsync<ObservableCollection<ImovelModelResponse>>(responseStream, options);
            }
        }
    }

    private SolicitacaoCliente notificacoes = new();
    public SolicitacaoCliente Notificacoes
    {
        get => notificacoes;
        set{
            notificacoes = value;
            OnPropertyChanged(nameof(Notificacoes));
        }
    }

// ---------------------------------------------------------------------------------------

    private ObservableCollection<NaturezaImovel> naturezaImovel = new();
    public ObservableCollection<NaturezaImovel> NaturezaImovel{
        get => naturezaImovel;
        set{
            naturezaImovel = value;
            OnPropertyChanged(nameof(NaturezaImovel));            
        }
    }

    private async Task ListarNaturezaImoveil()
    {
        var url = $"{UrlBase.UriBase.URI}listar/natureza/imovel";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using(var responseStream = await response.Content.ReadAsStreamAsync())
            {
                NaturezaImovel = await JsonSerializer.DeserializeAsync<ObservableCollection<NaturezaImovel>>(responseStream, options);                
            }
        }
    }


    public ICommand SolicitarImovelCommand => new Command(async () =>
    {
        bool execute = true;
        await ListarNaturezaImoveil();

        List<string> naturezaLista = new List<string>();
        
        foreach (var n in NaturezaImovel)
        {
            naturezaLista.Add(n.Caracteristica);
        }
        string action = await App.Current.MainPage.DisplayActionSheet("Escolha o tipo de imóvel - 1/4", "Cancelar", null, naturezaLista.ToArray());
        if (action != "Cancelar" && action is not null)
        {            
            foreach (var item in NaturezaImovel)
            {
                if (item.Caracteristica == action)
                {
                    Notificacoes.IdTipoImovel = item.IdTipoImovel;
                    break;
                }
            }
            string precoMinimo = await App.Current.MainPage.DisplayPromptAsync(
                "Informe o valor mínimo - 2/4", // Título
                "",                            // Mensagem (ou deixe vazio para nenhum texto adicional)
                "OK",                          // Texto do botão de confirmação
                "Cancelar",                    // Texto do botão de cancelamento
                "0",                           // Texto inicial no campo de entrada
                keyboard: Keyboard.Numeric     // Configuração do teclado numérico (opcional)
            );
            if (precoMinimo is not null)
            { 
                try
                {
                    Notificacoes.PrecoMinimo = Convert.ToDecimal(precoMinimo);
                }
                catch 
                {
                    execute = false;
                    await App.Current.MainPage.DisplayAlert("Erro","Digite um valor numérico para o valor mínimo","Ok");
                }
                if (Notificacoes.PrecoMinimo < 0)
                {
                    execute = false;
                    await App.Current.MainPage.DisplayAlert("Erro","Valor inválido","Ok");
                }
            }else
            {
                execute = false;
            }
            if (execute)
            {
                string precoMaximo = await App.Current.MainPage.DisplayPromptAsync(
                "Informe o valor máximo - 3/4", // Título
                "",                            // Mensagem (ou deixe vazio para nenhum texto adicional)
                "OK",                          // Texto do botão de confirmação
                "Cancelar",                    // Texto do botão de cancelamento
                "0",                           // Texto inicial no campo de entrada
                keyboard: Keyboard.Numeric     // Configuração do teclado numérico (opcional)
                );
                if (precoMaximo is not null)
                { 
                    try
                    {
                        Notificacoes.PrecoMaximo = Convert.ToDecimal(precoMaximo);
                    }
                    catch 
                    {
                        execute = false;
                        await App.Current.MainPage.DisplayAlert("Erro","Digite um valor numérico para o valor máximo","Ok");
                    }
                    if (Notificacoes.PrecoMaximo < 0)
                    {
                        execute = false;
                        await App.Current.MainPage.DisplayAlert("Erro","Valor inválido","Ok");
                    }
                }
                else
                {
                    execute = false;
                }
                if (execute)
                {
                    string localizacao = await App.Current.MainPage.DisplayPromptAsync(
                        "Informe a zona do imóvel - 4/4", // Título
                        "",                            // Mensagem (ou deixe vazio para nenhum texto adicional)
                        "OK",                          // Texto do botão de confirmação
                        "Cancelar",                    // Texto do botão de cancelamento
                        "",                           // Texto inicial no campo de entrada
                        keyboard: Keyboard.Text     // Configuração do teclado numérico (opcional)
                    );
                    if (localizacao is not null)
                    {
                        Notificacoes.Localizacao = localizacao;        
                    }
                    var url = $"{UrlBase.UriBase.URI}solicitar/imovel";
                    
                    var notificacao  = new SolicitacaoCliente()
                    {
                        IdClienteSolicitante = Convert.ToInt32(await SecureStorage.GetAsync("usuario_id")),
                        PrecoMinimo = Notificacoes.PrecoMinimo,
                        PrecoMaximo = Notificacoes.PrecoMaximo,
                        IdTipoImovel = Notificacoes.IdTipoImovel,
                        Localizacao = Notificacoes.Localizacao,
                    };

                    string json = JsonSerializer.Serialize<SolicitacaoCliente>(notificacao, options);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    
                    var response = await client.PostAsync(url, content);
                    
                    if (response.IsSuccessStatusCode)
                    {            
                        string responseString = await response.Content.ReadAsStringAsync();
                        await App.Current.MainPage.DisplayAlert("Mensagem",$"{responseString}","Ok");
                    }else
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        await App.Current.MainPage.DisplayAlert("Mensagem",$"{responseString}","Ok");
                    }
                }
            }

        }
    });

    
}
