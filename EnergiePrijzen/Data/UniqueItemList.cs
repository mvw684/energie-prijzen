// Copyright (c) 2025 mvw684

using System.Collections;
using System.Collections.Generic;

namespace EnergiePrijzen.Data {
    internal class UniqueItemList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : notnull {

        const int defaultSize = 1024;
        private readonly List<T> items;
        private readonly Dictionary<T, int> itemIndex;

        public UniqueItemList(IEqualityComparer<T> comparer) {
            items = new List<T>(defaultSize);
            itemIndex = new Dictionary<T, int>(defaultSize, comparer);
        }

        #region simple delegating implementations

        public int Count => ((ICollection<T>)items).Count;

        public bool IsReadOnly => ((ICollection<T>)items).IsReadOnly;

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();

        public bool Contains(T item) => itemIndex.ContainsKey(item);

        #endregion simple delegating implementations

        public void Add(T item) {
            if (itemIndex.ContainsKey(item)) {
                return; 
            }
            int index = items.Count;
            items.Add(item);
            itemIndex.Add(item, index);
        }

        public void Clear() {
            items.Clear();
            itemIndex.Clear();
        }
        
        public bool Remove(T item) {
            if (itemIndex.TryGetValue(item, out int index)) {
                items.RemoveAt(index);
                var _ = itemIndex.Remove(item);
                foreach(var kvp in itemIndex) {
                    if (kvp.Value > index) {
                        itemIndex[kvp.Key] = kvp.Value-1;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
