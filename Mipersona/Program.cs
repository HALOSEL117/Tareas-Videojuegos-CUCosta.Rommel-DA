namespace persona;

class Program
{
    static void Main(string[] args)
    {
        Empleado e = new Empleado("Ruben", "gonzales","hernandez", 60, "PEPR800101HDFMNR07", "Administrador", 50000.00D);
        Persona p = new Persona("Juan", "Pérez", "Gómez", 30, "PEPJ800101HDFMNR07");
        //persona p = new persona();
        //p.Nombre = "Juan";
        //p.ApePaterno = "Pérez";
        //p.ApeMaterno = "Gómez";
        //p.Edad = 30;
        //p.Curp = "PEPJ800101HDFMNR07";

        //Console.WriteLine(p.Nombre + " " + p.ApePaterno + " " + p.ApeMaterno + " " + p.Edad + " " + p.Curp);
        Console.WriteLine(p);
        Console.WriteLine(e);
    }
}
