/*
 * Autor: HalosGhost
 * Fecha: 27/06/2024
 * Descripción: Clase que representa a una persona con nombre y apellido.
 */
namespace SueldoEmpleado;

public class Persona
{
    private string nombre;
    private string apellido;
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    public string Apellido
    {
        get { return apellido; }
        set { apellido = value; }
    }
    public Persona(string nombre, string apellido)
    {
        this.nombre = nombre;
        this.apellido = apellido;
    }
    public override string ToString()
    {
        return $"{Nombre} {Apellido}";
    }
}
