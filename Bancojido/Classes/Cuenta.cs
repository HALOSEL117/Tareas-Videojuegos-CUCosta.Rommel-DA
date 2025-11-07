using Bancojido;
using System;
namespace Bancojido;

public class Cuenta : Persona
{
    private int _numeroCuenta;
    private Persona _titular;

    public Cuenta(int numeroCuenta, Persona titular)
        : base(titular.Nombre, titular.ApePaterno, titular.ApeMaterno, titular.Edad, titular.Curp, titular.Rfc)
    {
        _numeroCuenta = numeroCuenta;
        _titular = titular;
    }

    public int NumeroCuenta
    {
        get { return _numeroCuenta; }
        set { _numeroCuenta = value; }
    }
    public Persona Titular
    {
        get { return _titular; }
        set { _titular = value; }
    }
    public override string ToString()
    {
        return $"{NumeroCuenta} {Titular.Nombre} {Titular.ApePaterno} {Titular.ApeMaterno} {Titular.Edad} {Titular.Curp} {Titular.Rfc}";
    }
}