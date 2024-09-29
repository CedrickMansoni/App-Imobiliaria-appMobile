using System;
using App_Imobiliaria_appMobile.MVVM.Models.localizacao;
using App_Imobiliaria_appMobile.MVVM.Models.Usuarios;

namespace App_Imobiliaria_appMobile.MVVM.Models.imovel;

public class ImovelModelDTO
{
    public Imovel Imovel {get; set;} = new();
    public List<Pais>? Pais {get; set;} = new();
    public int IdPais {get;set;} 
    public List<Provincia>? Provincia {get; set;} = new();
    public int IdProvincia {get;set;}
    public List<Municipio>? Municipio {get; set;} = new();
    public int IdMunicipio {get;set;}
    public List<Bairro>? Bairro {get; set;} = new(); 
    public int IdBairro {get;set;}
    public List<Rua>? Rua {get; set;} = new();
    public int IdRua {get;set;}
    public List<Localizacao>? Localizacao {get; set;} = new();
    public int IdLocalizacao {get;set;}
    public List<TipoImovel>? TipoImovel {get; set;} = new();
    public int IdTipoImovel {get;set;}
    public List<NaturezaImovel>? NaturezaImovel {get; set;} = new();
    public int IdNaturezaImovel {get;set;}
    public List<Foto>? Fotos {get; set;} = new();

    /* DADOS DO PROPRIETÁRIO DO IMÓVEL ------------------------------------------------ */
    public ClienteProprietario ClienteProprietario {get; set;} = new();
    public int IdProprietario {get;set;}

    /* DADOS DO CORRETOR RESPONSÁVEL---------------------------------------------------- */
    public int IdCorretor {get;set;}
    public Funcionario CorretorImovel {get; set;} = new();  
}
