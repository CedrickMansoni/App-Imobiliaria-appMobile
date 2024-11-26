using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelPublicadoDetailsFavorito : ContentPage
{
	public PageImovelPublicadoDetailsFavorito(ImovelModelResponse imovelDados, ImoveisFavoritosViewModel page)
	{
		InitializeComponent();
		BindingContext = new ImoveisPublicadosViewModelDetailsFavorito(imovelDados, page);
	}
}