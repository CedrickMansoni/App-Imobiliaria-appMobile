using System;
using System.Windows.Input;
using App_Imobiliaria_appMobile.MVVM.Views;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;

namespace App_Imobiliaria_appMobile.MVVM.ViewModels.InicialViewModel;

public class InicialViewModels : BindableObject
{
    public ICommand LoginCommand => new Command(async()=>
    {
        await App.Current!.MainPage!.Navigation.PushModalAsync(new ViewLogin());
    });

    public ICommand CriarContaCommand => new Command(async()=>
    {
        await App.Current!.MainPage!.Navigation.PushModalAsync(new PageCriarConta());
    });
}
