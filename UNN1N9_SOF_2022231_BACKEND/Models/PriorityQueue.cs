namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class PriorityQueue<T>
    {
        private List<T> data;
        private Comparison<T> comparison;

        public PriorityQueue(Comparison<T> comparison)
        {
            this.data = new List<T>();
            this.comparison = comparison;
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            int i = data.Count - 1;
            while (i > 0)
            {
                int j = (i - 1) / 2;
                if (comparison(data[i], data[j]) >= 0)
                    break;
                T tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;
                i = j;
            }
        }

        public T Dequeue()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");
            T ret = data[0];
            int lastIndex = data.Count - 1;
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);
            lastIndex--;
            int i = 0;
            while (true)
            {
                int left = 2 * i + 1;
                if (left > lastIndex)
                    break;
                int right = left + 1;
                if (right <= lastIndex && comparison(data[right], data[left]) < 0)
                    left = right;
                if (comparison(data[left], data[i]) >= 0)
                    break;
                T tmp = data[left];
                data[left] = data[i];
                data[i] = tmp;
                i = left;
            }
            return ret;
        }

        public int Count
        {
            get { return data.Count; }
        }

        public T Peek()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");
            return data[0];
        }
    }

}
