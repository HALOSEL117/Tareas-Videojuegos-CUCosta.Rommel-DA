namespace ContactosEmpresax;

public class Agenda
{
    public List<Contacto> Contactos { get; set; } = new List<Contacto>();

    public void AgregarContacto(Contacto contacto)
    {
        Contactos.Add(contacto);
    }

    public void EliminarContacto(Contacto contacto)
    {
        Contactos.Remove(contacto);
    }

    public List<Contacto> BuscarContactos(string criterio)
    {
        return Contactos.Where(c => c.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                                    c.Email.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                                    c.Telefono.Contains(criterio, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void MostrarContactos()
    {
        foreach (var contacto in Contactos)
        {
            Console.WriteLine(contacto);
        }
    }

    public void GuardarContactos(string rutaArchivo)
    {
        var lineas = Contactos.Select(c => $"{c.Nombre},{c.Telefono},{c.Email}");
        System.IO.File.WriteAllLines(rutaArchivo, lineas);
    }
}
