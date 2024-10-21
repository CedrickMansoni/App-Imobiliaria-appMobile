using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImoveisPublicadosViewModelDetails : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
  
    private ImoveisPublicadosViewModel page;
    public ImoveisPublicadosViewModelDetails(ImovelModelResponse imovelDados, ImoveisPublicadosViewModel page)
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

        ImovelDados = imovelDados;
        this.page = page;
    }

    private ImovelModelResponse imovelDados = new();
    public ImovelModelResponse ImovelDados
    {
        get => imovelDados;
        set{
            imovelDados = value;
            OnPropertyChanged(nameof(ImovelDados));
        }
    }

    private bool expandido = true;
    public bool Expandido
    {
        get => expandido;
        set{
            expandido = value;
            OnPropertyChanged(nameof(Expandido));
        }
    }

    private string expandidoIcone = "expandirmais";
    public string ExpandidoIcone
    {
        get => expandidoIcone;
        set{
            expandidoIcone = value;
            OnPropertyChanged(nameof(ExpandidoIcone));
        }
    }

    public ICommand ExpandirCommand => new Command(()=>
    {
        Expandido = !Expandido;
        if (Expandido)
        {
            ExpandidoIcone = "expandirmais";
        }
        else
        {
            ExpandidoIcone = "expandirmenos";
        }
    });

    public ICommand FazerChamadaCommand => new Command<ImovelModelResponse>((ImovelModelResponse imovel)=>
    {
        if(!string.IsNullOrEmpty(imovel.CorretorImovel.Telefone))
        {
            if(PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open($"{imovel.CorretorImovel.Telefone}");
        }
    });

    public ICommand EnviarSMSCommand => new Command<ImovelModelResponse>(async (ImovelModelResponse imovel)=>
    {
        if(!string.IsNullOrEmpty(imovel.CorretorImovel.Telefone))
        {
            if (Sms.Default.IsComposeSupported)
            {
                string[] recipients = [$"{imovel.CorretorImovel.Telefone}"];
                string text = $"Prezado(a) Sr(a). {imovel.CorretorImovel.Nome}, meu nome é {await SecureStorage.Default.GetAsync("usuario_nome")} e estou interessado(a) no imóvel de código [{imovel.Imovel.Codigo}]. Gostaria de obter mais informações e, se possível, agendar uma visita. Agradeço desde já pela atenção.";

                var message = new SmsMessage(text, recipients);

                await Sms.Default.ComposeAsync(message);
            }
        }
    });


   
}
