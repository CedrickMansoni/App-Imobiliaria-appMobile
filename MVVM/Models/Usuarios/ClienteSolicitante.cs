using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

public class ClienteSolicitante 
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
