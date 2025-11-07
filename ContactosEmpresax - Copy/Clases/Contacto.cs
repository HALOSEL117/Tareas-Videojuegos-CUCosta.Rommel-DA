namespace ContactosEmpresax;

public class Contacto : Agenda
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    public Contacto(string nombre, string telefono, string email)
    {
        Nombre = nombre;
        Telefono = telefono;
        Email = email;
    }
    public override string ToString()
    {
        return $"Nombre: {Nombre}, Teléfono: {Telefono}, Email: {Email}";
    }
}
