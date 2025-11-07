using Bancojido;
using System;
namespace Bancojido;

public class Banco
{
    private string _banco;
    private string _nombre;
    public Banco(string banco, string nombre)
    {
        _banco = banco;
        _nombre = nombre;
    }
    public string Nombre
    {
        get { return _nombre; }
        set { _nombre = value; }
    }
    public override string ToString()
    {
        return $"{Nombre}";
    }
}
