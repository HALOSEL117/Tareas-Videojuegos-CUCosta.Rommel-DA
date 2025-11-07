namespace Mayor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Escribe un Numero");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Escribe un Numero");
            int y = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Escribe un Numero");
            int z = Convert.ToInt32(Console.ReadLine());

            int mayor, medio, menor;

            if (x >= y && x >= z)
            {
                mayor = x;
                if (y >= z)
                {
                    medio = y;
                    menor = z;
                }
                else
                {
                    medio = z;
                    menor = y;
                }
            }
            else if (y >= x && y >= z)
            {
                mayor = y;
                if (x >= z)
                {
                    medio = x;
                    menor = z;
                }
                else
                {
                    medio = z;
                    menor = x;
                }
            }
            else
            {
                mayor = z;
                if (x >= y)
                {
                    medio = x;
                    menor = y;
                }
                else
                {
                    medio = y;
                    menor = x;
                }
            }
            Console.WriteLine($"Mayor: {mayor}");
            Console.WriteLine($"Medio: {medio}");
            Console.WriteLine($"Menor: {menor}");
        }
    }
}
