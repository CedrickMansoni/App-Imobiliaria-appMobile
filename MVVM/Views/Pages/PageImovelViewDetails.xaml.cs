using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelViewDetails : ContentPage
{	
	public PageImovelViewDetails(ImovelModelResponse imovelDados, PublicarImovelViewModel page)
	{
		InitializeComponent();
		BindingContext = new ViewModels.ImovelViewModel.ImovelViewModelDetails(imovelDados, page);
	}
}