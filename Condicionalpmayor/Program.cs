namespace Condicionalpmayor;
class Program
{
    static void Main(string[] args)
    {
        Console.Write("Ingrese la edad de la primera persona: ");
        int edad1 = int.Parse(Console.ReadLine());

        Console.Write("Ingrese la edad de la segunda persona: ");
        int edad2 = int.Parse(Console.ReadLine());

        if (edad1 > edad2)
        {
            Console.WriteLine($"La primera persona es mayor que la segunda por {edad1 - edad2} años.");
        }
        else if (edad1 < edad2)
        {
            Console.WriteLine($"La segunda persona es mayor que la primera por {edad2 - edad1} años.");
        }
        else
        {
            Console.WriteLine("Ambas personas tienen la misma edad.");
        }
    }
}