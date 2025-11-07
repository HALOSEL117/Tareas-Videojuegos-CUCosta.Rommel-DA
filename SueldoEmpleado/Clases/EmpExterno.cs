/*
 * Autor: HalosGhost
 * Fecha: 27/06/2024
 * Descripción: Clase que representa a un empleado externo, hereda de Empleado.
 */
namespace SueldoEmpleado;

public class EmpExterno : Empleado
{
    private double tarifamensual;
    public double TarifaMensual
    {
        get { return tarifamensual; }
        set { tarifamensual = value; }
    }
    public EmpExterno(string nombre, string apellido, double horastrabajadas, double tarifamensual)
        : base(nombre, apellido, horastrabajadas)
    {
        this.tarifamensual = tarifamensual;
    }
    public double CalcularSueldo()
    {
        return tarifamensual;
    }

    public override string ToString()
    {
        return $"{base.ToString()} - Tarifa Mensual: {tarifamensual}, Sueldo: {CalcularSueldo()}";
    }
}
