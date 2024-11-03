using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

public class PerfilUsuario<T>
{    
    public T Entidade {get; set;}
    public string FotoPerfil {get; set;} = string.Empty;
}

