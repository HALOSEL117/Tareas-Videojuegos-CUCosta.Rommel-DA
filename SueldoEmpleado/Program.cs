/*
db   db   j88D  db       .d88b.   d888b  db   db  .d88b.  d88888D d888888b           d8888b.
88   88  j8~88  88      .8P  88. 88' Y8b 88   88 .8P  88. YP  d8' `~~88~~' Vb     db VP  `8D
88ooo88 j8' 88  88      88  d'88 88      88ooo88 88  d'88    d8'     88     `Vb   VP   oooY'
88~~~88 V88888D 88      88 d' 88 88  ooo 88~~~88 88 d' 88   d8'      88       `V.      ~~~b.
88   88     88  88booo. `88  d8' 88. ~8~ 88   88 `88  d8'  d8' db    88       .d' db db   8D
YP   YP     VP  Y88888P  `Y88P'   Y888P  YP   YP  `Y88P'  d88888P    YP     .dP   VP Y8888P'
                                                                           dP               
*/
namespace SueldoEmpleado;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Cálculo de Sueldos de Empleados");
        Empbase empBase = new Empbase("Isaac", "Arias", 160, 500);
        EmpPorHora empPorHora = new EmpPorHora("Luis", "Vergara", 290, 180, 117);
        EmpExterno empExterno = new EmpExterno("Axel", "Diaz", 160, 3000);

        Console.WriteLine(empBase);
        Console.WriteLine(empPorHora);
        Console.WriteLine(empExterno);
    }
}
