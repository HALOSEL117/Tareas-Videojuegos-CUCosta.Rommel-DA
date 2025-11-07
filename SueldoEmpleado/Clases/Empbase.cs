/*
 * Autor: HalosGhost
 * Fecha: 27/06/2024
 * Descripción: Clase que representa a un empleado base, hereda de Empleado.
 */
namespace SueldoEmpleado;

public class Empbase : Empleado
{
    private double tarifa;
    public double Tarifa
    {
        get { return tarifa; }
        set { tarifa = value; }
    }
    public double CalcularSueldo()
    {
        return horastrabajadas * tarifa;
    }
    public Empbase(string nombre, string apellido, double horastrabajadas, double tarifa)
        : base(nombre, apellido, horastrabajadas)
    {
        this.tarifa = tarifa;
    }
    public override string ToString()
    {
        return $"{base.ToString()} - Tarifa: {tarifa}, Sueldo: {CalcularSueldo()}";
    }
}
