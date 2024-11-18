using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using App_Imobiliaria_appMobile.MVVM.Views;
using App_Imobiliaria_appMobile.MVVM.Views.ShellGerente;
using App_Imobiliaria_appMobile.MVVM.Views.Pages;

namespace App_Imobiliaria_appMobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new ViewLogin();
		//MainPage = new GerenteShell();
		//MainPage = new NavigationPage(new PageImovelPublicadosCliente());
		MainPage = new NavigationPage(new PageInicial());		
	}
}
