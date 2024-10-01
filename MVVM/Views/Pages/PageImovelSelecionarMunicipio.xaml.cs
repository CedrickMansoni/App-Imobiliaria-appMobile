using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;
namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarMunicipio : ContentPage
{
	public PageImovelSelecionarMunicipio(ImovelCadViewModel imovelCad, int idPais, int idProvincia)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(imovelCad, idPais, idProvincia);
	}
}