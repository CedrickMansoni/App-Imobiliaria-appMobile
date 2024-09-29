
namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageTirarFoto : ContentPage
{
	public PageTirarFoto()
	{
		InitializeComponent();
		BindingContext = new App_Imobiliaria_appMobile.MVVM.ViewModels.DropBoxViewModel.TirarFotoViewModel();
	}
}