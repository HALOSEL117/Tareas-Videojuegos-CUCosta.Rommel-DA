using Bancojido;
using System;

namespace Bancojido;

class Program
{
    static void Main(string[] args)
    {
        Persona p = new Persona("John", "Doe", "Smith", 30, "CURP123", "RFC123");
        Cuenta c = new Cuenta(9475937, p);
        Banco b = new Banco("Banjercito", "Banco del Ejercito");
        BalanceCuenta bc = new BalanceCuenta(9475937, p, 1000.00m);
        Console.WriteLine($"Banco: {b}, Cuenta No: {c}, Titular: {p}, Saldo: {bc}");
    }
}