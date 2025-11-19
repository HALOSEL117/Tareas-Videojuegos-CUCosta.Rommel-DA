using System;
using System.Threading;

namespace Threads
{
    class Program
    {
        // Objeto estático para bloquear la consola y evitar que los textos se mezclen
        // Esto se conoce como "Exclusión Mutua"
        static readonly object _bloqueoConsola = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("--- INICIO DEL PROGRAMA PRINCIPAL ---");
            Console.WriteLine("Lanzando hilos de procesamiento...");

            // 1. Creación de los hilos
            // Utilizamos expresiones lambda () => para pasar parámetros fácilmente
            Thread hilo1 = new Thread(() => ProcesarArchivo("Video_HD.mp4", 5));
            Thread hilo2 = new Thread(() => ProcesarArchivo("Datos_Usuarios.csv", 3));
            Thread hilo3 = new Thread(() => ProcesarArchivo("Imagen_4K.png", 2));

            // 2. Ejecución (Start)
            // En este momento, el SO comienza a intercalar la ejecución de estos métodos
            hilo1.Start();
            hilo2.Start();
            hilo3.Start();

            Console.WriteLine("[Main] Los hilos han sido lanzados. El Main sigue vivo.");

            // 3. Espera (Join)
            // El hilo principal (Main) se detendrá aquí hasta que cada hilo termine.
            // Si no hacemos esto, el programa podría cerrarse antes de que terminen los hilos.
            hilo1.Join();
            hilo2.Join();
            hilo3.Join();

            Console.WriteLine("--- TODOS LOS PROCESOS FINALIZADOS ---");
            Console.WriteLine("Presione enter para salir.");
            Console.ReadLine();
        }

        // Este es el método que ejecutará cada hilo
        static void ProcesarArchivo(string nombreArchivo, int segundosDuracion)
        {
            // Inicio del proceso
            EscribirEnConsola($"-> INICIANDO descarga de: {nombreArchivo} (Tiempo estimado: {segundosDuracion}s)", ConsoleColor.Yellow);

            for (int i = 0; i < segundosDuracion; i++)
            {
                // Simulamos trabajo pesado durmiendo el hilo 1 segundo
                Thread.Sleep(1000); 
                
                // Mostramos progreso
                // Usamos 'lock' porque Console.WriteLine no es seguro en multihilo por defecto
                // Sin el lock, un hilo podría escribir a mitad de la línea de otro.
                lock (_bloqueoConsola)
                {
                    Console.Write($"[Progreso] {nombreArchivo}: ");
                    // Simulamos una barra de carga
                    for(int j=0; j<=i; j++) Console.Write("|");
                    Console.WriteLine();
                }
            }

            // Fin del proceso
            EscribirEnConsola($"-> COMPLETADO: {nombreArchivo} ha finalizado correctamente.", ConsoleColor.Green);
        }

        // Método auxiliar para escribir con color de forma segura
        static void EscribirEnConsola(string mensaje, ConsoleColor color)
        {
            lock (_bloqueoConsola)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(mensaje);
                Console.ResetColor();
            }
        }
    }
}
