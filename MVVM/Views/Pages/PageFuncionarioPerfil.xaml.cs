using App_Imobiliaria_appMobile.MVVM.ViewModels.FuncionarioViewModel;

namespace App_Imobiliaria_appMobile.MVVM.Views.Pages;

public partial class PageFuncionarioPerfil : ContentPage
{
	public PageFuncionarioPerfil()
	{
		InitializeComponent();
		this.Appearing += (sender, args) =>
		{
			var p = (PerfilViewModel)BindingContext;
			p.GetPerfilCommand.Execute(null);
		};
	}
}