namespace Algoritmos
{
    public interface ISorter
    {
        string Name { get; }
        ResultMetrics Execute(int[] arr);
    }
}
