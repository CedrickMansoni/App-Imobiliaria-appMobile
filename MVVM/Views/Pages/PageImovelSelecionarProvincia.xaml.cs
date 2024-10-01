using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarProvincia : ContentPage
{
	public PageImovelSelecionarProvincia(ImovelCadViewModel imovelCad, int idPais)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(imovelCad, idPais);
	}
}