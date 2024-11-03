using App_Imobiliaria_appMobile.MVVM.ViewModels.ImovelViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageCriarContaProprietario : ContentPage
{
	public PageCriarContaProprietario()
	{
		InitializeComponent();
		BindingContext = new MVVM.ViewModels.ClienteViewModel.ProprietarioViewModel();
	}

	private void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e) 
	{
		var component = (Entry)sender;
		if (component.Placeholder == "Nome do cliente")
		{
			if (!string.IsNullOrEmpty(component.Text))
			{
				lblNome.IsVisible = true;
			}else
			{
				lblNome.IsVisible = false;
			}
		}

		if (component.Placeholder == "Telefone")
		{
			if (!string.IsNullOrEmpty(component.Text))
			{
				lblTelefone.IsVisible = true;
			}else
			{
				lblTelefone.IsVisible = false;
			}
		}

		if (component.Placeholder == "Email")
		{
			if (!string.IsNullOrEmpty(component.Text))
			{
				lblEmail.IsVisible = true;
			}else
			{
				lblEmail.IsVisible = false;
			}
		}

		if (component.Placeholder == "Email")
		{
			if (!string.IsNullOrEmpty(component.Text))
			{
				lblEmail.IsVisible = true;
			}else
			{
				lblEmail.IsVisible = false;
			}
		}
	}	
}