namespace Algoritmos
{
    public class MergeSorter : SorterBase
    {
        public override string Name => "Merge Sort";

        protected override void RunSort(int[] arr, out int maxRecursionDepth)
        {
            int current = 0; maxRecursionDepth = 0;
            MergeSort(arr, 0, arr.Length - 1, ref current, ref maxRecursionDepth);
        }

        private void MergeSort(int[] arr, int left, int right, ref int currentDepth, ref int maxDepth)
        {
            currentDepth++;
            if (currentDepth > maxDepth) maxDepth = currentDepth;

            if (left < right)
            {
                int middle = left + (right - left) / 2;
                MergeSort(arr, left, middle, ref currentDepth, ref maxDepth);
                MergeSort(arr, middle + 1, right, ref currentDepth, ref maxDepth);
                Merge(arr, left, middle, right);
            }

            currentDepth--;
        }

        private void Merge(int[] arr, int left, int middle, int right)
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
                if (L[p] <= R[q]) arr[k++] = L[p++]; else arr[k++] = R[q++];
            }
            while (p < n1) arr[k++] = L[p++];
            while (q < n2) arr[k++] = R[q++];
        }
    }
}
