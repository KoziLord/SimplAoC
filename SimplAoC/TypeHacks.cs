using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimplAoC
{
    internal static class TypeHacks
    {
        /// <summary>
        /// Efficiently adds a Span to a List.<para/>
        /// Why the hell wasn't this added at any point since .NET CORE 2.1, it's been ALMOST FIVE YEARS.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="span"></param>
        public static void AddRange<T>(this List<T> list, ReadOnlySpan<T> span)
        {
            var count = list.Count;

            var l = Unsafe.As<ListGuts<T>>(list);
            if (count > 0)
            {
                if (l._items.Length - l._size < count)
                    l.Grow(checked(l._size + count));

                span.CopyTo(l._items.AsSpan(l._size));
                l._size += count;
                l._version++;              
            }
        }

        private sealed class ListGuts<T>
        { 
            public T[] _items;
            internal int _size, _version;

            internal void Grow(int capacity)
            {
                Debug.Assert(_items.Length < capacity);

                int newCapacity = _items.Length == 0 ? 4: 2 * _items.Length;

                // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

                // If the computed capacity is still less than specified, set to the original argument.
                // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
                if (newCapacity < capacity) newCapacity = capacity;

                Unsafe.As<List<T>>(this).Capacity = newCapacity;
            }
        }


        public static unsafe void Write<T>(this Stream s, T value) where T : unmanaged
        {
            s.Write(MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref value), sizeof(T)));
        }
    }
}

