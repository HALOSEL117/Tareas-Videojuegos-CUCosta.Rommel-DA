namespace ContactosEmpresax;

public class ContactoEmpresarial : Contacto // Cambia Agenda por Contacto
{
    public string NombreEmpresa { get; set; }
    public string Cargo { get; set; }

    public ContactoEmpresarial(string nombre, string telefono, string email, string nombreEmpresa, string cargo)
        : base(nombre, telefono, email) // Llama al constructor de Contacto
    {
        NombreEmpresa = nombreEmpresa;
        Cargo = cargo;
    }
    public override string ToString()
    {
        return $"Nombre: {Nombre}, Teléfono: {Telefono}, Email: {Email}, Empresa: {NombreEmpresa}, Cargo: {Cargo}";
    }
}
