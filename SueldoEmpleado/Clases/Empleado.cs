/*
 * Autor: HalosGhost
 * Fecha: 27/06/2024
 * Descripción: Clase que representa a un empleado, hereda de Persona.
 */
namespace SueldoEmpleado;

public class Empleado : Persona
{
    private double _horastrabajadas;
    public double horastrabajadas
    {
        get { return _horastrabajadas; }
        set { _horastrabajadas = value; }
    }
    public Empleado(string nombre, string apellido, double horastrabajadas) : base(nombre, apellido)
    {
        this._horastrabajadas = horastrabajadas;
    }
    public override string ToString()
    {
        return $"{base.ToString()} - Horas Trabajadas: {_horastrabajadas}";
    }
}
