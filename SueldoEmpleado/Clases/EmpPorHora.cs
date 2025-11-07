/*
 * Autor: HalosGhost
 * Fecha: 27/06/2024
 * Descripción: Clase que representa a un empleado por hora, hereda de Empleado.
 */
namespace SueldoEmpleado;

public class EmpPorHora : Empleado
{
    private double TarifaporHora;
    private double Tarifaporhoraextra;

    public int sueldo
    {
        get
        {
            return (int)(horastrabajadas * TarifaporHora +
                (horastrabajadas > 40 ? (horastrabajadas - 40) * Tarifaporhoraextra : 0));
        }
    }
    public EmpPorHora(string nombre, string apellido, double horastrabajadas, double TarifaporHora, double Tarifaporhoraextra)
        : base(nombre, apellido, horastrabajadas)
    {
        this.TarifaporHora = TarifaporHora;
        this.Tarifaporhoraextra = Tarifaporhoraextra;
    }
    public override string ToString()
    {
        return $"{base.ToString()} - Tarifa por Hora: {TarifaporHora}, Tarifa por Hora Extra: {Tarifaporhoraextra}, Sueldo: {sueldo}";
    }
}
