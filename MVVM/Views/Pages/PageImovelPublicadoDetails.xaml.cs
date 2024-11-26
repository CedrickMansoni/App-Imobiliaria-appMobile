using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelPublicadoDetails : ContentPage
{
	public PageImovelPublicadoDetails(ImovelModelResponse imovelDados, ImoveisPublicadosViewModel page)
	{
		InitializeComponent();
		BindingContext = new ImoveisPublicadosViewModelDetails(imovelDados, page);
	}
	
}