using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImoveisPublicados : ContentPage
{
	public PageImoveisPublicados()
	{
		InitializeComponent();
	}

	private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(SearchImovel.Text))
		{
			var p = (ImoveisPublicadosViewModel)BindingContext;
			p.PesquisarImovelCommand.Execute(SearchImovel.Text);
		}else
		{
			DisplayAlert("Erro","Digite o código do imóvel que pretende pesquisar", "Ok");
		}		
	}

	private void SearchBarChanged(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(SearchImovel.Text))
		{
			var p = (ImoveisPublicadosViewModel)BindingContext;
			p.ActualizarPaginaCommand.Execute(null);
		}
	}
}