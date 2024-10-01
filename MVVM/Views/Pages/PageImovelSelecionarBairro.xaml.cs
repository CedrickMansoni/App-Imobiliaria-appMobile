using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarBairro : ContentPage
{
	public PageImovelSelecionarBairro(ImovelCadViewModel imovelCad, int idPais, int idProvincia, int idMunicipio)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(imovelCad, idPais, idProvincia, idMunicipio);
	}
}