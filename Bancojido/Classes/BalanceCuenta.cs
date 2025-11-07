using Bancojido;
using System;
namespace Bancojido;

public class BalanceCuenta : Cuenta
{
    private decimal _saldo;

    public BalanceCuenta(int numeroCuenta, Persona persona, decimal saldo) : base(numeroCuenta, persona)
    {
        _saldo = saldo;
    }

    public decimal Saldo
    {
        get { return _saldo; }
        set { _saldo = value; }
    }
    public override string ToString()
    {
        return $"{base.ToString()} {Saldo}";
    }
}
