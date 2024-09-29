using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
namespace App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

public class ImovelViewModel : BindableObject
{
    
    public ICommand CadastrarImovelCommand => new Command(async ()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageImovelCad()); 
    });
    public ICommand CadastrarProprietario => new Command(async ()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageCriarContaProprietario()); 
    });
    
    public ICommand CadastrarTipoImovelCommand => new Command(async ()=>
    {
        await App.Current.MainPage.Navigation.PushAsync(new PageTipoImovel()); 
    });
}
