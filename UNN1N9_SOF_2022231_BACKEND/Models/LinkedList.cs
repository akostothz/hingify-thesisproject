using System.Collections;

namespace UNN1N9_SOF_2022231_BACKEND.Models
{
    public class LinkedListNode
    {
        public double Value { get; set; }
        public Music Object { get; set; }
        public LinkedListNode Next { get; set; }
    }

    public class LinkedList
    {
        public LinkedListNode Head { get; set; }

        public void Add(double value, Music obj)
        {
            LinkedListNode newNode = new LinkedListNode { Value = value, Object = obj };

            if (Head == null || Head.Value >= value)
            {
                newNode.Next = Head;
                Head = newNode;
            }
            else
            {
                LinkedListNode current = Head;

                while (current.Next != null && current.Next.Value < value)
                {
                    current = current.Next;
                }

                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }

        #region ForEnumerator
        public IEnumerator GetEnumerator()
        {
            return new ListDel(Head);
        }
        class ListDel : IEnumerator 
        {
            LinkedListNode head;
            LinkedListNode _current;
            public ListDel(LinkedListNode head)
            {
                this.head = head;
                this._current = new LinkedListNode();
                this._current.Next = head;
            }
            public object Current { get { return _current.Object; } }

            public bool MoveNext()
            {
                _current = _current.Next;
                return _current != null;
            }

            public void Reset()
            {
                _current = new LinkedListNode();
                _current.Next = head;
            }
        }
        #endregion
    }
}

