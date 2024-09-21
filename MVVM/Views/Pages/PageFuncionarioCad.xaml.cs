using App_Imobiliaria_appMobile.MVVM.ViewModels.FuncionarioViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageFuncionarioCad : ContentPage
{
	public PageFuncionarioCad(FuncionarioViewModel page)
	{
		InitializeComponent();
		this.BindingContext = new CadFuncViewModel(page);
	}	
	private void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e) 
	{
		var component = (Entry)sender;
		if (component.Placeholder == "Nome do funcionário")
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