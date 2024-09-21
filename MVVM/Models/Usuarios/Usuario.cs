using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

public class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
