using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarPais : ContentPage
{
	public PageImovelSelecionarPais(ImovelCadViewModel cadImovel)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(cadImovel);
	}
}