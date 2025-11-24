using System;
using System.Collections.Generic;

/*
 Program.cs - Orquestador principal

    - Parsea argumentos de línea de comandos (size, mode, flags como --headless, --repeat,
        --out, y --sequential/--parallel).
    - Genera los datos de entrada (aleatorio o lista invertida) y evita la interacción cuando
        se solicita `--headless` para permitir ejecuciones automáticas desde scripts.
    - Instancia los `ISorter` y delega la ejecución a `BenchmarkRunner`.

    Comentarios: las mediciones de memoria privada (`PrivateBefore/PrivateAfter`) son válidas
    cuando la ejecución es secuencial; en modo paralelo las lecturas pueden solaparse.
*/

namespace Algoritmos
{
    class Program
    {
        static bool headless = false;
        static string? outFilePath = null;
        static int repeats = 1;
        static int currentSize = 0;
        static string currentMode = "";
        static bool sequentialMode = false; // default: parallel

        static void Main(string[] args)
        {
            int CANTIDAD_NUMEROS = 100000;
            int MAX_VALOR = 999999;
            string modo = "aleatorio";

            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (string.Equals(a, "--headless", StringComparison.OrdinalIgnoreCase) || string.Equals(a, "-h", StringComparison.OrdinalIgnoreCase))
                {
                    headless = true; continue;
                }
                if (string.Equals(a, "--sequential", StringComparison.OrdinalIgnoreCase)) { sequentialMode = true; continue; }
                if (string.Equals(a, "--parallel", StringComparison.OrdinalIgnoreCase)) { sequentialMode = false; continue; }
                if (string.Equals(a, "--out", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    outFilePath = args[++i]; continue;
                }
                if (string.Equals(a, "--repeat", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length && int.TryParse(args[++i], out int rpt))
                {
                    repeats = Math.Max(1, rpt); continue;
                }
                if (string.Equals(a, "--max", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length && int.TryParse(args[++i], out int pmax))
                {
                    MAX_VALOR = pmax; continue;
                }
                if (!a.StartsWith("-"))
                {
                    if (currentSize == 0 && int.TryParse(a, out int parsedCantidad)) { CANTIDAD_NUMEROS = parsedCantidad; currentSize = CANTIDAD_NUMEROS; continue; }
                    if (string.IsNullOrEmpty(modo)) { var a1 = a.ToLower(); modo = (a1 == "cruel" || a1 == "invertida") ? "cruel" : a1; currentMode = modo; continue; }
                }
            }

            currentSize = currentSize == 0 ? CANTIDAD_NUMEROS : currentSize;
            currentMode = string.IsNullOrEmpty(currentMode) ? modo : currentMode;

            if (modo == "cruel") Console.WriteLine("=== MODO CRUEL: LISTA INVERTIDA ===");
            int[] baseData = (modo == "cruel") ? GenerarListaInvertida(CANTIDAD_NUMEROS) : GenerarNumerosAleatorios(CANTIDAD_NUMEROS, MAX_VALOR);

            if (!headless)
            {
                MostrarVentana(baseData);
                Console.WriteLine("\nPresiona [ENTER] para iniciar los Threads de ordenamiento...");
                Console.ReadLine(); Console.Clear();
            }

            CsvWriter csv = null;
            if (!string.IsNullOrEmpty(outFilePath)) csv = new CsvWriter(outFilePath);

            var sorters = new List<ISorter> { new QuickSorter(), new MergeSorter(), new BubbleSorter() };
            var runner = new BenchmarkRunner(csv);

            Console.WriteLine("=== INICIANDO PROCESAMIENTO EN PARALELO ===\n");
            runner.Run(sorters, baseData, modo, repeats, headless, sequentialMode);

            Console.WriteLine("\n============================================");
            Console.WriteLine("Todos los algoritmos han finalizado.");
            Console.WriteLine("============================================");
            if (!headless) Console.ReadKey();
        }

        static int[] GenerarNumerosAleatorios(int cantidad, int maxValor)
        {
            var rnd = new Random();
            int[] arr = new int[cantidad];
            for (int i = 0; i < cantidad; i++) arr[i] = rnd.Next(0, maxValor + 1);
            return arr;
        }

        static void MostrarVentana(int[] datos)
        {
            Console.Clear();
            string titulo = $" LISTA GENERADA ({datos.Length} NÚMEROS) ";
            string borde = new string('═', 70);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"╔{borde}╗");
            Console.WriteLine($"║{titulo.PadLeft(35 + titulo.Length / 2).PadRight(70)}║");
            Console.WriteLine($"╠{borde}╣");

            string contenido = "";
            for (int i = 0; i < datos.Length; i++)
            {
                contenido += $"{datos[i]:000} ";
                if ((i + 1) % 14 == 0) contenido += "\n  ";
            }

            var lineas = contenido.Split('\n');
            foreach (var linea in lineas) Console.WriteLine($"║  {linea.PadRight(68)}║");

            Console.WriteLine($"╚{borde}╝");
            Console.ResetColor();
        }

        // --- GENERADOR "CRUEL" (Lista Invertida) ---
        static int[] GenerarListaInvertida(int cantidad)
        {
            int[] arr = new int[cantidad];
            int valor = cantidad;
            for (int i = 0; i < cantidad; i++) arr[i] = valor--;
            return arr;
        }
    }
}