using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algoritmos
{
    /// <summary>
    /// BenchmarkRunner: ejecuta una colección de ordenadores (ISorter) con una
    /// configuración dada y escribe métricas en CSV si se proporciona un CsvWriter.
    ///
    /// Modos de ejecución:
    /// - Secuencial (sequential=true): ejecuta cada sorter uno a uno. Recomendado
    ///   para obtener medidas de memoria privada no solapadas.
    /// - Paralelo (sequential=false): ejecuta cada sorter en su propia Task para
    ///   comparar tiempos en concurrencia; las medidas de memoria pueden solaparse.
    /// </summary>
    public class BenchmarkRunner
    {
        private readonly CsvWriter? csvWriter;
        private readonly object consoleLock = new object();

        public BenchmarkRunner(CsvWriter? csvWriter = null)
        {
            this.csvWriter = csvWriter;
        }

        public void Run(IEnumerable<ISorter> sorters, int[] baseData, string mode, int repeats, bool headless, bool sequential)
        {
            for (int rep = 1; rep <= repeats; rep++)
            {
                if (sequential)
                {
                    // Execute each sorter sequentially to avoid overlapping memory measurements
                    foreach (var sorter in sorters)
                    {
                        int[] arr = (int[])baseData.Clone();
                        var metrics = sorter.Execute(arr);

                        lock (consoleLock)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"--> {sorter.Name} terminó.");
                            Console.ResetColor();
                            Console.WriteLine($"    Tiempo: {metrics.Ticks} ticks ({metrics.Ms:F4} ms). RecDepth: {metrics.MaxRecursionDepth}");
                        }

                        csvWriter?.Append(arr.Length, mode, rep, sorter.Name, metrics);
                    }
                }
                else
                {
                    // Parallel execution: run each sorter in its own Task
                    var tasks = new List<Task>();
                    foreach (var sorter in sorters)
                    {
                        int[] arr = (int[])baseData.Clone();
                        var s = sorter; // capture
                        var t = Task.Run(() =>
                        {
                            var metrics = s.Execute(arr);
                            lock (consoleLock)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"--> {s.Name} terminó.");
                                Console.ResetColor();
                                Console.WriteLine($"    Tiempo: {metrics.Ticks} ticks ({metrics.Ms:F4} ms). RecDepth: {metrics.MaxRecursionDepth}");
                            }
                            csvWriter?.Append(arr.Length, mode, rep, s.Name, metrics);
                        });
                        tasks.Add(t);
                    }

                    Task.WaitAll(tasks.ToArray());
                }

                if (!headless) Console.WriteLine($"Repetición {rep} de {repeats} finalizada.\n");
            }
        }
    }
}
