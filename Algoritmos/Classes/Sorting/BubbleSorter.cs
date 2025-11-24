namespace Algoritmos
{
    public class BubbleSorter : SorterBase
    {
        public override string Name => "Bubble Sort";

        protected override void RunSort(int[] arr, out int maxRecursionDepth)
        {
            maxRecursionDepth = 0;
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
    }
}
