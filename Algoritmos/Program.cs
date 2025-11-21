using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Algoritmos
{
    class Program
    {
        // Objeto para bloquear la consola y evitar que los textos se mezclen al usar threads
        static readonly object consolaLock = new object();
        // Indica si el programa corre en modo no interactivo (benchmarks automatizados)
        static bool headless = false;
        // Parámetros para salida estructurada
        static string outFilePath = null;
        static int runId = 0;
        static object fileLock = new object();
        static int currentSize = 0;
        static string currentMode = "";
        static int repeats = 1;

        static void Main(string[] args)
        {
            // CONFIGURACIÓN (valores por defecto, pueden sobrescribirse por argumentos)
            int CANTIDAD_NUMEROS = 10000; // Puedes ajustar este valor o pasar como primer argumento
            int MAX_VALOR = 99999; // Rango máximo para números aleatorios
            string modo = "aleatorio"; // "aleatorio" o "cruel"

            // Parseo robusto de argumentos (posicionales y flags)
            // Uso: dotnet run --project .\Algoritmos.csproj -- <cantidad> <modo> [<maxValor>] [--run N] [--out <path>] [--headless]
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (string.Equals(a, "--headless", StringComparison.OrdinalIgnoreCase) || string.Equals(a, "-h", StringComparison.OrdinalIgnoreCase))
                {
                    headless = true;
                    continue;
                }
                if (string.Equals(a, "--out", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    outFilePath = args[++i];
                    continue;
                }
                if (string.Equals(a, "--repeat", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length && int.TryParse(args[++i], out int rpt))
                {
                    repeats = Math.Max(1, rpt);
                    continue;
                }
                if (string.Equals(a, "--run", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length && int.TryParse(args[++i], out int rid))
                {
                    runId = rid;
                    continue;
                }
                if (string.Equals(a, "--max", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length && int.TryParse(args[++i], out int pmax))
                {
                    MAX_VALOR = pmax;
                    continue;
                }
                // Posicionales: si no empiezan por '-'
                if (!a.StartsWith("-"))
                {
                    if (currentSize == 0 && int.TryParse(a, out int parsedCantidad))
                    {
                        CANTIDAD_NUMEROS = parsedCantidad;
                        currentSize = CANTIDAD_NUMEROS;
                        continue;
                    }
                    if (string.IsNullOrEmpty(modo))
                    {
                        var a1 = a.ToLower();
                        if (a1 == "cruel" || a1 == "invertida" || a1 == "inverted") modo = "cruel";
                        else modo = a1;
                        currentMode = modo;
                        continue;
                    }
                    if (int.TryParse(a, out int maybeMax))
                    {
                        MAX_VALOR = maybeMax;
                        continue;
                    }
                }
            }

            // Asegurar valores en variables estáticas para salida CSV
            currentSize = currentSize == 0 ? CANTIDAD_NUMEROS : currentSize;
            currentMode = string.IsNullOrEmpty(currentMode) ? modo : currentMode;

            // Generación de datos (modo según argumentos)
            if (modo == "cruel") Console.WriteLine("=== MODO CRUEL: LISTA INVERTIDA ===");
            int[] baseData = (modo == "cruel") ? GenerarListaInvertida(CANTIDAD_NUMEROS) : GenerarNumerosAleatorios(CANTIDAD_NUMEROS, MAX_VALOR);

            // 2. Mostrar la "Ventana" con los datos desordenados (si no estamos en modo headless)
            if (!headless)
            {
                MostrarVentana(baseData);
                Console.WriteLine("\nPresiona [ENTER] para iniciar los Threads de ordenamiento...");
                Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("=== INICIANDO PROCESAMIENTO EN PARALELO ===\n");

            // Ejecutar N repeticiones (si repeats>1). Cada repetición genera copias independientes de la misma entrada base.
            for (int rep = 1; rep <= repeats; rep++)
            {
                runId = rep;
                // Crear copias independientes para cada Thread (para evitar conflictos de memoria)
                int[] arrBubble = (int[])baseData.Clone();
                int[] arrMerge = (int[])baseData.Clone();
                int[] arrQuick = (int[])baseData.Clone();

                // 3. Ejecutar Algoritmos en Threads (Tasks) paralelos
                var t1 = Task.Run(() => EjecutarBubble(arrBubble));
                var t2 = Task.Run(() => EjecutarMerge(arrMerge));
                var t3 = Task.Run(() => EjecutarQuick(arrQuick));

                // Esperar a que todos terminen
                Task.WaitAll(t1, t2, t3);

                Console.WriteLine($"Repetición {rep} de {repeats} finalizada.\n");
            }

            Console.WriteLine("\n============================================");
            Console.WriteLine("Todos los algoritmos han finalizado.");
            Console.WriteLine("============================================");
            if (!headless)
            {
                Console.ReadKey();
            }
        }

        // --- MÉTODOS DE EJECUCIÓN Y MEDICIÓN ---

        static void EjecutarBubble(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            BubbleSort(arr);
            sw.Stop();
            ReportarResultado("Bubble Sort", sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds, arr);
        }

        static void EjecutarMerge(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            MergeSort(arr, 0, arr.Length - 1);
            sw.Stop();
            ReportarResultado("Merge Sort", sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds, arr);
        }

        static void EjecutarQuick(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(arr, 0, arr.Length - 1);
            sw.Stop();
            ReportarResultado("Quick Sort", sw.ElapsedTicks, sw.Elapsed.TotalMilliseconds, arr);
        }

        // Método thread-safe para escribir en consola
        static void ReportarResultado(string algoritmo, long ticks, double ms, int[] arrOrdenado)
        {
            lock (consolaLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"--> {algoritmo} terminó.");
                Console.ResetColor();
                Console.WriteLine($"    Tiempo: {ticks} ticks de reloj ({ms:F2} ms).");
                Console.WriteLine($"    Muestra (primeros 10): {string.Join(", ", arrOrdenado[..Math.Min(10, arrOrdenado.Length)])}...");
                Console.WriteLine("------------------------------------------------");
            }

            // Si se indicó salida estructurada, escribir fila CSV (thread-safe)
            if (!string.IsNullOrEmpty(outFilePath))
            {
                lock (fileLock)
                {
                    bool exists = System.IO.File.Exists(outFilePath);
                    if (!exists)
                    {
                        // Escribir encabezado con BOM UTF8
                        var utf8Bom = new System.Text.UTF8Encoding(true);
                        var header = "Size,Mode,Run,Algorithm,Ticks,Ms\r\n";
                        System.IO.File.WriteAllText(outFilePath, header, utf8Bom);
                    }
                    // Formatear línea CSV
                    string line = string.Format("{0},{1},{2},{3},{4},{5}\r\n", currentSize, currentMode, runId, algoritmo, ticks, ms.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
                    System.IO.File.AppendAllText(outFilePath, line, System.Text.Encoding.UTF8);
                }
            }
        }

        // --- UTILIDADES ---

        static int[] GenerarNumerosAleatorios(int cantidad, int maxValor)
        {
            Random rnd = new Random();
            int[] arr = new int[cantidad];
            for (int i = 0; i < cantidad; i++)
            {
                arr[i] = rnd.Next(0, maxValor + 1);
            }
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
                // Formato: 005, 100, 023...
                contenido += $"{datos[i]:000} ";
                if ((i + 1) % 14 == 0) contenido += "\n  "; // Salto de línea cada 14 números
            }

            var lineas = contenido.Split('\n');
            foreach (var linea in lineas)
            {
                Console.WriteLine($"║  {linea.PadRight(68)}║");
            }

            Console.WriteLine($"╚{borde}╝");
            Console.ResetColor();
        }

        // --- ALGORITMOS (Lógica Pura) ---

        // A. Bubble Sort
        static void BubbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        // B. Merge Sort
        static void MergeSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;
                MergeSort(arr, left, middle);
                MergeSort(arr, middle + 1, right);
                Merge(arr, left, middle, right);
            }
        }

        static void Merge(int[] arr, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;
            int[] L = new int[n1];
            int[] R = new int[n2];

            for (int i = 0; i < n1; ++i) L[i] = arr[left + i];
            for (int j = 0; j < n2; ++j) R[j] = arr[middle + 1 + j];

            int k = left, p = 0, q = 0;
            while (p < n1 && q < n2)
            {
                if (L[p] <= R[q]) arr[k++] = L[p++];
                else arr[k++] = R[q++];
            }
            while (p < n1) arr[k++] = L[p++];
            while (q < n2) arr[k++] = R[q++];
        }

        // C. Quick Sort
        static void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }

        static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            int temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;
            return i + 1;
        }
        // --- GENERADOR "CRUEL" (Lista Invertida) ---
        static int[] GenerarListaInvertida(int cantidad)
        {
            int[] arr = new int[cantidad];
            // Empezamos con un número alto y vamos bajando
            int valor = cantidad;
            for (int i = 0; i < cantidad; i++)
            {
                arr[i] = valor--;
            }
            return arr;
        }
    }
}