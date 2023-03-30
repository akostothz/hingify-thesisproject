namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class PriorityQueue<T>
    {
        private List<Music> data;
        private Comparison<Music> comparison;

        public PriorityQueue(Comparison<Music> comparison)
        {
            this.data = new List<Music>();
            this.comparison = comparison;
        }

        public void Enqueue(Music item)
        {
            data.Add(item);
            int i = data.Count - 1;
            while (i > 0)
            {
                int j = (i - 1) / 2;
                if (comparison(data[i], data[j]) >= 0)
                    break;
                Music tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;
                i = j;
            }
        }

        public Music Dequeue()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");
            Music ret = data[0];
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
                Music tmp = data[left];
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

        public Music Peek()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");
            return data[0];
        }

        public int ListLength()
        {
            int mins = 0;
            foreach (var item in data)
            {
                mins += MsToMins(item.DurationMs);
            }
            return mins;
        }

        private int MsToMins(int ms)
        {
            double conv = 1.6667E-5;
            return (int)conv * ms;
        }
    }

}
