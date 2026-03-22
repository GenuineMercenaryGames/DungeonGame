using UnityEngine;

namespace Assets.Scripts.Generation.DungeonGeneration.Utils
{
    public class DynamicArray<T>
    {

        public int Count { get { return _count; } }

        private T[] _data;
        private int _capacity;
        private int _count;

        public DynamicArray(int capacity)
        {
            _data = new T[capacity];
            _capacity = capacity;
            _count = 0;
        }

        public ref T this[int index]
        {
            get
            {
                return ref _data[index];
            }
        }

        public void Add(T item) 
        {
            if(_count == _capacity)
            {
                T[] temp = new T[_capacity * 2];
                for(int i = 0; i < _count; i++) 
                {
                    temp[i] = _data[i];
                }
                _capacity *= 2;
                _data = temp;
            }
            _data[_count++] = item; 
        }

        public ref T GetRef(int index)
        {
            Debug.Assert(index < _count, "Out of bounds!");
            return ref _data[index];
        }

        public T Get(int index) 
        {
            Debug.Assert(index < _count, "Out of bounds!");
            return _data[index];
        }

        public void Clear()
        {
            _count = 0;
        }
    }
}