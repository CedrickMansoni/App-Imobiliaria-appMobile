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

public class ImovelCadViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public ImovelCadViewModel()
    {
        client = new HttpClient();
        options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};

    }

    private ImovelModelDTO imovelDados = new();
    public ImovelModelDTO ImovelDados
    {
        get => imovelDados;
        set{
            imovelDados = value;
            OnPropertyChanged(nameof(ImovelDados));
        }
    }

    private Pais pais = new();
    public Pais Pais{
        get => pais;
        set{
            pais = value;
            OnPropertyChanged(nameof(Pais));
        }
    }

    public ICommand GotoPaisCommand => new Command(async()=>
    {
        await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarPais(this));
    });
    public ICommand GetPaisCommand => new Command<Pais>((Pais paisSelecionado)=>
    {
        Pais = paisSelecionado;
        Provincia = new();
        Municipio = new();
        Bairro = new();
        Rua = new();
    });

    private Provincia provincia = new();
    public Provincia Provincia{
        get => provincia;
        set{
            provincia = value;
            OnPropertyChanged(nameof(Provincia));
        }
    }

    public ICommand GotoProvinciaCommand => new Command(async()=>
    {
        if (Pais.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione primeiro o país","Ok");
        }else
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarProvincia(this, Pais.Id));
        }
        
    });
    public ICommand GetProvinciaCommand => new Command<Provincia>((Provincia provincia)=>
    {
        Provincia = provincia;
        Municipio = new();
        Bairro = new();
        Rua = new();
    });


    private Municipio municipio = new();
    public Municipio Municipio{
        get => municipio;
        set{
            municipio = value;
            OnPropertyChanged(nameof(Municipio));
        }
    }
    public ICommand GotoMunicipioCommand => new Command(async()=>
    {
        if (Pais.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione primeiro o país","Ok");
        }else if(Provincia.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione a província antes","Ok");
        }
        else
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarMunicipio(this, Pais.Id, Provincia.Id));
        }
    });
    public ICommand GetMunicipioCommand => new Command<Municipio>((Municipio municipio)=>
    {
        Municipio = municipio;
        Bairro = new();
        Rua = new();
    });

    private Bairro bairro = new();
    public Bairro Bairro{
        get => bairro;
        set{
            bairro = value;
            OnPropertyChanged(nameof(Bairro));
        }
    }

    public ICommand GotoBairroCommand => new Command(async()=>
    {
        if (Pais.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione primeiro o país","Ok");
        }else if(Provincia.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione a província antes","Ok");
        }else if(Municipio.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione o município antes","Ok");
        }
        else
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarBairro(this, Pais.Id, Provincia.Id, Municipio.Id));
        }
    });
    public ICommand GetBairroCommand => new Command<Bairro>((Bairro bairro)=>
    {
        Bairro = bairro;
        Rua = new();
    });


    private Rua rua = new();
    public Rua Rua{
        get => rua;
        set{
            rua = value;
            OnPropertyChanged(nameof(Rua));
        }
    }
    public ICommand GotoRuaCommand => new Command(async()=>
    {
        if (Pais.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione primeiro o país","Ok");
        }else if(Provincia.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione a província antes","Ok");
        }else if(Municipio.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione o município antes","Ok");
        }
        else if(Bairro.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione o bairro antes","Ok");
        }
        else
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarRua(this, Pais.Id, Provincia.Id, Municipio.Id, Bairro.Id));
        }
    });
    public ICommand GetRuaCommand => new Command<Rua>((Rua rua)=>
    {
        Rua = rua;
    });

    private TipoImovel tipoImovel = new();
    public TipoImovel TipoImovel{
        get => tipoImovel;
        set{
            tipoImovel = value;
            OnPropertyChanged(nameof(TipoImovel));
        }
    }
    public ICommand GotoTipoImovelCommand => new Command(async()=>
    {
        if (Pais.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione primeiro o país","Ok");
        }else if(Provincia.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione a província antes","Ok");
        }else if(Municipio.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione o município antes","Ok");
        }
        else if(Bairro.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione o bairro antes","Ok");
        }
        else if(Rua.Id == 0)
        {
            await App.Current.MainPage.DisplayAlert("Erro","Por favor selecione a rua","Ok");
        }
        else
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PageImovelSelecionarTipo(this, true));
        }
    });
    public ICommand GetTipoImovelCommand => new Command<TipoImovel>((TipoImovel tipoImovel)=>
    {
        TipoImovel = tipoImovel;
    });

    private NaturezaImovel naturezaImovel = new();
    public NaturezaImovel NaturezaImovel{
        get => naturezaImovel;
        set{
            naturezaImovel = value;
            OnPropertyChanged(nameof(NaturezaImovel));            
        }
    }

    public ICommand AvancarCommand => new Command(async()=>
    {
        if (!string.IsNullOrEmpty(Rua.NomeRua) || !string.IsNullOrEmpty(TipoImovel.TipoImovelDesc) || !string.IsNullOrEmpty(NaturezaImovel.Caracteristica))
        {
            ImovelDados.Pais.Add(Pais);
            ImovelDados.Provincia.Add(Provincia);
            ImovelDados.Municipio.Add(Municipio);
            ImovelDados.Bairro.Add(Bairro);
            ImovelDados.Rua.Add(Rua);
            ImovelDados.TipoImovel.Add(TipoImovel);
            NaturezaImovel.IdTipoImovel = TipoImovel.Id;
            ImovelDados.NaturezaImovel.Add(NaturezaImovel);
            
            await App.Current.MainPage.Navigation.PushAsync(new PageCadastrarImovel(ImovelDados));
            
        }else
        {
            await App.Current.MainPage.DisplayAlert("Erro","Informe os dados solicitados nos campos acima","Ok");
        }
    });

}
