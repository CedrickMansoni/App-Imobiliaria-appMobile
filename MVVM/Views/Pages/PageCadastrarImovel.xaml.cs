using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;
using App_Imobiliaria_appMobile.MVVM.Models.imovel;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageCadastrarImovel : ContentPage
{
	public PageCadastrarImovel(ImovelModelDTO imovelDados)
	{
		InitializeComponent();
		BindingContext = new ImovelUploadViewModel(imovelDados);
	}
}