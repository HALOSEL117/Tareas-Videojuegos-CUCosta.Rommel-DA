namespace Algoritmos
{
    public class QuickSorter : SorterBase
    {
        public override string Name => "Quick Sort";

        protected override void RunSort(int[] arr, out int maxRecursionDepth)
        {
            int current = 0; maxRecursionDepth = 0;
            QuickSort(arr, 0, arr.Length - 1, ref current, ref maxRecursionDepth);
        }

        private void QuickSort(int[] arr, int low, int high, ref int currentDepth, ref int maxDepth)
        {
            currentDepth++;
            if (currentDepth > maxDepth) maxDepth = currentDepth;

            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSort(arr, low, pi - 1, ref currentDepth, ref maxDepth);
                QuickSort(arr, pi + 1, high, ref currentDepth, ref maxDepth);
            }

            currentDepth--;
        }

        private int Partition(int[] arr, int low, int high)
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
    }
}
