using System;

namespace Algoritmos
{
    public class ResultMetrics
    {
        // ResultMetrics almacena todas las mediciones recogidas durante una ejecución
        // - Ticks/Ms: tiempo medido mediante Stopwatch
        // - ManagedBefore/After: GC.GetTotalMemory(false) antes/después
        // - PrivateBefore/After: Process.PrivateMemorySize64 antes/después
        // - MaxRecursionDepth: profundidad máxima alcanzada por algoritmos recursivos

        public long Ticks { get; set; }
        public double Ms { get; set; }
        public long ManagedBefore { get; set; }
        public long ManagedAfter { get; set; }
        public long ManagedDelta => ManagedAfter - ManagedBefore;
        public long PrivateBefore { get; set; }
        public long PrivateAfter { get; set; }
        public long PrivateDelta => PrivateAfter - PrivateBefore;
        public int MaxRecursionDepth { get; set; }
    }
}
