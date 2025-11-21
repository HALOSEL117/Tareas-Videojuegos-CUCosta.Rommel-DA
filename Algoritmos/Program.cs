using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Algoritmos
{
    class Program
    {
        // Objeto para bloquear la consola y evitar que los textos se mezclen al usar threads
        static readonly object consolaLock = new object();

        static void Main(string[] args)
        {
            // CONFIGURACIÓN
            const int CANTIDAD_NUMEROS = 5000; // Puedes ajustar este valor para probar con más o menos números
            const int MAX_VALOR = 999; // Máximo 4 dígitos en el modo cruel para evitar "StackOverflow"
            //Para probar el "modo cruel" de listas invertidas, descomenta las siguientes líneas: 20,23 y comenta la linea 26.
            //para volver al modo aleatorio descomenta la linea 25 y comenta las líneas 20 y 23.
            //Generar lista invertida (peor caso para algunos algoritmos)
            //Console.WriteLine("=== MODO CRUEL: LISTA INVERTIDA ===");
            
            //EN LUGAR DE ALEATORIOS, USAMOS LA LISTA INVERTIDA
            //int[] dataOriginal = GenerarListaInvertida(CANTIDAD_NUMEROS);

            // 1. Generar lista aleatoria
            int[] dataOriginal = GenerarNumerosAleatorios(CANTIDAD_NUMEROS, MAX_VALOR);
            
            // 2. Mostrar la "Ventana" con los datos desordenados
            MostrarVentana(dataOriginal);

            Console.WriteLine("\nPresiona [ENTER] para iniciar los Threads de ordenamiento...");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("=== INICIANDO PROCESAMIENTO EN PARALELO ===\n");

            // Crear copias independientes para cada Thread (para evitar conflictos de memoria)
            int[] arrBubble = (int[])dataOriginal.Clone();
            int[] arrMerge = (int[])dataOriginal.Clone();
            int[] arrQuick = (int[])dataOriginal.Clone();

            // 3. Ejecutar Algoritmos en Threads (Tasks) paralelos
            var t1 = Task.Run(() => EjecutarBubble(arrBubble));
            var t2 = Task.Run(() => EjecutarMerge(arrMerge));
            var t3 = Task.Run(() => EjecutarQuick(arrQuick));

            // Esperar a que todos terminen
            Task.WaitAll(t1, t2, t3);

            Console.WriteLine("\n============================================");
            Console.WriteLine("Todos los algoritmos han finalizado.");
            Console.WriteLine("============================================");
            Console.ReadKey();
        }

        // --- MÉTODOS DE EJECUCIÓN Y MEDICIÓN ---

        static void EjecutarBubble(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            BubbleSort(arr);
            sw.Stop();
            ReportarResultado("Bubble Sort", sw.ElapsedTicks, arr);
        }

        static void EjecutarMerge(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            MergeSort(arr, 0, arr.Length - 1);
            sw.Stop();
            ReportarResultado("Merge Sort", sw.ElapsedTicks, arr);
        }

        static void EjecutarQuick(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(arr, 0, arr.Length - 1);
            sw.Stop();
            ReportarResultado("Quick Sort", sw.ElapsedTicks, arr);
        }

        // Método thread-safe para escribir en consola
        static void ReportarResultado(string algoritmo, long ticks, int[] arrOrdenado)
        {
            lock (consolaLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"--> {algoritmo} terminó.");
                Console.ResetColor();
                Console.WriteLine($"    Tiempo: {ticks} ticks de reloj.");
                Console.WriteLine($"    Muestra (primeros 10): {string.Join(", ", arrOrdenado[..Math.Min(10, arrOrdenado.Length)])}...");
                Console.WriteLine("------------------------------------------------");
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