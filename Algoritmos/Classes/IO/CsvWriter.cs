using System;
using System.IO;

namespace Algoritmos
{
    public class CsvWriter
    {
        private readonly string path;
        private readonly object fileLock = new object();

        public CsvWriter(string path)
        {
            this.path = path;
            EnsureHeader();
        }

        private void EnsureHeader()
        {
            if (string.IsNullOrEmpty(path)) return;
            lock (fileLock)
            {
                if (!File.Exists(path))
                {
                    // Es responsabilidad única de CsvWriter crear la cabecera con BOM
                    // para evitar duplicar la lógica en scripts externos.
                    var utf8Bom = new System.Text.UTF8Encoding(true);
                    var header = "Size,Mode,Run,Algorithm,Ticks,Ms,ManagedBefore,ManagedAfter,ManagedDelta,PrivateBefore,PrivateAfter,PrivateDelta,MaxRecursionDepth\r\n";
                    File.WriteAllText(path, header, utf8Bom);
                }
            }
        }

        public void Append(int size, string mode, int runId, string algoritmo, ResultMetrics metrics)
        {
            if (string.IsNullOrEmpty(path)) return;
            lock (fileLock)
            {
                string line = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\r\n",
                    size, mode, runId, algoritmo, metrics.Ticks, metrics.Ms,
                    metrics.ManagedBefore, metrics.ManagedAfter, metrics.ManagedDelta,
                    metrics.PrivateBefore, metrics.PrivateAfter, metrics.PrivateDelta,
                    metrics.MaxRecursionDepth);
                File.AppendAllText(path, line, System.Text.Encoding.UTF8);
            }
        }
    }
}
