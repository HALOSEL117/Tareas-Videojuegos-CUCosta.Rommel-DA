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

            int mayor, media, menor;

            if (x >= y && x >= z)
            {
                mayor = x;
                if (y >= z)
                {
                    media = y;
                    menor = z;
                }
                else
                {
                    media = z;
                    menor = y;
                }
            }
            else if (y >= x && y >= z)
            {
                mayor = y;
                if (x >= z)
                {
                    media = x;
                    menor = z;
                }
                else
                {
                    media = z;
                    menor = x;
                }
            }
            else
            {
                mayor = z;
                if (x >= y)
                {
                    media = x;
                    menor = y;
                }
                else
                {
                    media = y;
                    menor = x;
                }
            }
            Console.WriteLine($"Mayor: {mayor}");
            Console.WriteLine($"Media: {media}");
            Console.WriteLine($"Menor: {menor}");
        }
    }
}
