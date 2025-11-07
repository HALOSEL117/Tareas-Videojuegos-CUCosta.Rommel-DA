namespace ContactosEmpresax;

public class ContactoPersona : Contacto
{
    public ContactoPersona(string nombre, string telefono, string email)
        : base(nombre, telefono, email)
    {
    }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, Teléfono: {Telefono}, Email: {Email}";
    }
}
