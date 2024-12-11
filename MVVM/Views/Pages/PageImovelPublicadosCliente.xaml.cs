using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageImovelPublicadosCliente : ContentPage
{
	public PageImovelPublicadosCliente()
	{
		InitializeComponent();
		this.Appearing += (sender, args) =>
		{
			var p = (ImoveisPublicadosViewModel)BindingContext;
			p.ActualizarPaginaCommand.Execute(null);
		};
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

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Deseja realmente sair da aplicação?","Sim","Não");
		if(resposta)
		{
			SecureStorage.Default.RemoveAll();
        	App.Current.MainPage = new NavigationPage(new PageInicial());
		}
    }
}