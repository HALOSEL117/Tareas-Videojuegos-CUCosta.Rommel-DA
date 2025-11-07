namespace TipoDato;

class Program
{
    static void Main(string[] args)
    {
        int TipoEntero = 5;
        string TipoString = "Elias";
        double TipoDouble = 5.6;
        double TipoDouble = 5.6D;
        float TipoFloat = 5.5;
        float TipoFloat = 5.5f;
        console.WriteLine($"los tipos de datos son{TipoEntero},{TipoString},{TipoDouble},{TipoFloat}");
        console.WriteLine("ingrese un numero");
        int entrada = convert.ToInt32(console.ReadLine());
    }
}
