using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelFavorito : ContentPage
{
	public PageImovelFavorito()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
		var vm = (ImoveisFavoritosViewModel)BindingContext;
		vm.ActualizarPaginaCommand.Execute(null);
        base.OnAppearing();
    }
}


