namespace ContactosEmpresax;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Contactos Empresariales y Personales");
        Agenda agenda = new Agenda();

        // Agregar contactos
        agenda.AgregarContacto(new ContactoEmpresarial("Juan Perez", "123456789", "juan@empresa.com", "Empresa S.A.", "Gerente"));
        agenda.AgregarContacto(new ContactoPersona("Maria Lopez", "987654321", "maria@personal.com"));

        // Mostrar contactos
        agenda.MostrarContactos();

        // Buscar contactos
        var resultados = agenda.BuscarContactos("Juan");
        Console.WriteLine("Resultados de búsqueda:");
        foreach (var contacto in resultados)
        {
            Console.WriteLine(contacto);
        }

        // Guardar contactos
        agenda.GuardarContactos("contactos.txt");
    }
}
