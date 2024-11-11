using System;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;
using App_Imobiliaria_appMobile.UrlBase;

namespace App_Imobiliaria_appMobile.MVVM.Models;

public class UsuarioModelRequeste
{
    public Usuario Dados{get; set;} = new Usuario();
    public string Mensagem {get; set;} = string.Empty;
    public string UserType {get;set;} = string.Empty;
    public string Estado {get; set;} = string.Empty;
    public string Avatar {get; set;} = string.Empty;
}
