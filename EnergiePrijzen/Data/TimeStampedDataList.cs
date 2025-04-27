// Copyright (c) 2025 mvw684

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EnergiePrijzen.Data {

    /// <summary>
    /// Collection of <see cref=" IAggregatableData{T}"/> keyed by <see cref="TimeStamp"/>.
    /// </summary>
    internal class TimeStampedDataList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : class, ITimeStampedData<T> {

        const int defaultSize = 1024;
        private readonly List<T> items;
        private readonly Dictionary<TimeStamp, int> itemIndex;
                
        public TimeStampedDataList() {
            items = new List<T>(defaultSize);
            itemIndex = new Dictionary<TimeStamp, int>(defaultSize);
        }

        #region simple delegating implementations

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();

        public bool Contains(T item) => itemIndex.ContainsKey(item.TimeStamp) && itemIndex[item.TimeStamp].Equals(item);

        public T this[int index] {
            get => items[index];
        }

        public bool TryGet(TimeStamp stamp, [NotNullWhen(true)] out T? value) {
            if(itemIndex.TryGetValue(stamp, out int index)) {
                value = items[index];
                return true;
            } else {
                value = default;
                return false;
            }
        }


        #endregion simple delegating implementations

        public void Add(T item) {
            if(itemIndex.ContainsKey(item.TimeStamp)) {
                throw new ArgumentException("Duplicate timsstamp");
            }
            int index = items.Count;
            items.Add(item);
            itemIndex.Add(item.TimeStamp, index);
        }

        public void Clear() {
            items.Clear();
            itemIndex.Clear();
        }

        public bool Remove(T item) {
            if(itemIndex.TryGetValue(item.TimeStamp, out int index)) {
                items.RemoveAt(index);
                var _ = itemIndex.Remove(item.TimeStamp);
                foreach(var kvp in itemIndex) {
                    if(kvp.Value > index) {
                        itemIndex[kvp.Key] = kvp.Value - 1;
                    }
                }
                return true;
            }
            return false;
        }
    }
}

