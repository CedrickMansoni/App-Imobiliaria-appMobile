using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;
namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelSelecionarTipo : ContentPage
{
	public PageImovelSelecionarTipo(ImovelCadViewModel imovelCad, bool tipo)
	{
		InitializeComponent();
		BindingContext = new ImovelViewModelSelect(imovelCad, tipo);
	}
}