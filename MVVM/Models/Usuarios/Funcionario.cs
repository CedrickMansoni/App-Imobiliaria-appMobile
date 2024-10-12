using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

public class Funcionario : Usuario
{  
    public string Estado { get; set; } = string.Empty;  

    public int IdProvincia { get; set; }

    public string Nivel { get; set; } = string.Empty;
    public string? Avatar {get; set;}
}
