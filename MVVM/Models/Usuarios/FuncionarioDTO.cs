using System;

namespace App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

public class FuncionarioDTO : Usuario
{
    public int Estado { get; set; } 

    public int IdProvincia { get; set; }

    public int Nivel { get; set; } 
}
