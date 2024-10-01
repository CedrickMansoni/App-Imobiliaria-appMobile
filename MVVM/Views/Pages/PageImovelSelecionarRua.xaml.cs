using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;
namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarRua : ContentPage
{
	public PageImovelSelecionarRua(ImovelCadViewModel imovelCad, int idPais, int idProvincia, int idMunicipio, int idBairro)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(imovelCad, idPais, idProvincia, idMunicipio, idBairro);
	}
}