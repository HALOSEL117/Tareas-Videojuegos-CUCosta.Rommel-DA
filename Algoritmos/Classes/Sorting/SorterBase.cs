using System.Diagnostics;

namespace Algoritmos
{
    public abstract class SorterBase : ISorter
    {
        public abstract string Name { get; }

        // Concrete sorters must implement this to perform the actual sort
        protected abstract void RunSort(int[] arr, out int maxRecursionDepth);

        public ResultMetrics Execute(int[] arr)
        {
            var proc = Process.GetCurrentProcess();
            var metrics = new ResultMetrics();
            metrics.ManagedBefore = GC.GetTotalMemory(false);
            metrics.PrivateBefore = proc.PrivateMemorySize64;

            Stopwatch sw = Stopwatch.StartNew();
            RunSort(arr, out int maxDepth);
            sw.Stop();

            metrics.ManagedAfter = GC.GetTotalMemory(false);
            proc.Refresh();
            metrics.PrivateAfter = proc.PrivateMemorySize64;
            metrics.Ticks = sw.ElapsedTicks;
            metrics.Ms = sw.Elapsed.TotalMilliseconds;
            metrics.MaxRecursionDepth = maxDepth;

            return metrics;
        }
    }
}
