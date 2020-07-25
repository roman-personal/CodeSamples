using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace Utils {
    #region IRleCollectionValueConverter
    public interface IRleCollectionValueConverter<T> {
        T Convert(T value);
    }
    #endregion

    #region IRleCollection
    public interface IRleCollection<T> : IEnumerable<T> {
        T this[int index] { get; set; }
        int Length { get; }

        IRleCollection<T> GetRange(int startIndex, int endIndex);
        void SetRange(int startIndex, int endIndex, T value);
        void SetRange(int startIndex, IRleCollection<T> range);
        void SetRange(int startIndex, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter);

        void Insert(int index, T value);
        void Insert(int index, int count, T value);
        void Insert(int index, IRleCollection<T> range);
        void Insert(int index, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter);

        void Remove(int index);
        void Remove(int index, int count);

        int Count(T value);
        int Count(Predicate<T> match);

        IEnumerable<int> GetIndexEnumerable(T value, bool reversed);
        IEnumerable<int> GetIndexEnumerable(T value, int startIndex, int endIndex);
        IEnumerable<int> GetIndexEnumerable(Predicate<T> match, bool reversed);
        IEnumerable<int> GetIndexEnumerable(Predicate<T> match, int startIndex, int endIndex);

        RleCollection<T> Implementation { get; }
        bool ImmutableValues { get; set; }
    }
    #endregion

    #region RleCollectionBase (abstract)
    public abstract class RleCollectionBase {
        public abstract int Length { get; }

        protected void CheckIndex(int index) {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(string.Format("Index out of range 0...{0}", Length - 1));
        }

        protected void CheckIndex(int index, string parameterName) {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(string.Format("Index out of range 0...{0}. Parameter name \"{1}\".", Length - 1, parameterName));
        }

        protected void CheckRangeIndexes(int startIndex, int endIndex) {
            CheckIndex(startIndex, "startIndex");
            CheckIndex(endIndex, "endIndex");
            if (startIndex > endIndex)
                throw new ArgumentException("Start index greater than end index");
        }

        protected void CheckIndexAndCount(int index, int count) {
            Guard.ArgumentPositive(count, "count");
            CheckIndex(index, "index");
            if ((index + count) > Length)
                throw new ArgumentException("Index + count greater than collection length");
        }
    }
    #endregion

    #region RleCollection
    public class RleCollection<T> : RleCollectionBase, IRleCollection<T> {
        #region IIndexRange
        interface IIndexRange {
            int StartIndex { get; }
            int EndIndex { get; }
        }
        #endregion

        #region IndexRangeComparable
        class IndexRangeComparable : IComparable<IIndexRange> {
            int index;

            public IndexRangeComparable(int index) {
                this.index = index;
            }

            public int CompareTo(IIndexRange other) {
                if (index < other.StartIndex)
                    return 1;
                if (index > other.EndIndex)
                    return -1;
                return 0;
            }
        }
        #endregion

        #region RleItem
        class RleItem<V> : IIndexRange {
            public RleItem() {
            }
            public RleItem(V value, int startIndex, int endIndex) {
                Value = value;
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
            public RleItem(RleItem<V> other) {
                Value = other.Value;
                StartIndex = other.StartIndex;
                EndIndex = other.EndIndex;
            }

            public V Value { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }

            public int Count { get { return EndIndex - StartIndex + 1; } }

        }
        #endregion

        #region IndexEnumerator
        class IndexEnumerator : IEnumerator<int> {
            RleCollection<T> collection;
            int currentIndex;
            int startIndex;
            int endIndex;
            Predicate<T> match;
            bool reversed;

            public IndexEnumerator(RleCollection<T> collection, int startIndex, int endIndex, Predicate<T> match) {
                this.collection = collection;
                this.startIndex = startIndex;
                this.endIndex = endIndex;
                this.match = match;
                reversed = startIndex > endIndex;
                Reset();
            }

            #region IEnumerator<int> Members

            public int Current {
                get { return currentIndex; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                collection = null;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                if (reversed) {
                    currentIndex--;
                    if (currentIndex < endIndex)
                        return false;
                    int itemIndex = collection.GetItemIndex(currentIndex);
                    while (!match(collection.items[itemIndex].Value)) {
                        currentIndex = collection.items[itemIndex].StartIndex - 1;
                        if (currentIndex < endIndex)
                            return false;
                        itemIndex--;
                    }
                }
                else {
                    currentIndex++;
                    if (currentIndex > endIndex)
                        return false;
                    int itemIndex = collection.GetItemIndex(currentIndex);
                    while (!match(collection.items[itemIndex].Value)) {
                        currentIndex = collection.items[itemIndex].EndIndex + 1;
                        if (currentIndex > endIndex)
                            return false;
                        itemIndex++;
                    }
                }
                return true;
            }

            public void Reset() {
                currentIndex = startIndex + (reversed ? 1 : -1);
            }

            #endregion
        }
        #endregion

        #region Fields
        readonly int length;
        readonly List<RleItem<T>> items;
        int mruItemIndex;
        #endregion

        RleCollection(int length) {
            Guard.ArgumentPositive(length, "length");
            this.length = length;
            items = new List<RleItem<T>>();
        }

        public RleCollection(int length, T defaultValue) 
            : this(length) {
            items.Add(new RleItem<T>(defaultValue, 0, length - 1));
            mruItemIndex = 0;
            Type valueType = typeof(T);
            ImmutableValues = valueType.GetTypeInfo().IsValueType || valueType.Equals(typeof(String));
        }

        #region Properties
        public T this[int index] { 
            get {
                CheckIndex(index);
                RleItem<T> item = GetItem(index);
                if (ImmutableValues || (Object)item.Value == null)
                    return item.Value;
                ICloneable cloneable = item.Value as ICloneable;
                if (cloneable == null) {
                    ICloneable<T> typedCloneable = item.Value as ICloneable<T>;
                    if (typedCloneable == null)
                        throw new InvalidOperationException("Unable to clone value. ICloneable or ICloneable<T> not implemented.");
                    return typedCloneable.Clone();
                }
                return (T)cloneable.Clone();
            } 
            set {
                CheckIndex(index);
                SetValue(index, value); 
            } 
        }
        public override int Length { get { return length; } }
        /// <summary>
        /// Inner items count. FOR TESTS ONLY!!!
        /// </summary>
        protected internal int ItemsCount { get { return items.Count; } }
        /// <summary>
        /// Inner MRU item index. FOR TESTS ONLY!!!
        /// </summary>
        protected internal int MruItemIndex { get { return mruItemIndex; } }
        public RleCollection<T> Implementation { get { return this; } }
        public bool ImmutableValues { get; set; }
        #endregion

        #region Get/SetRange
        public IRleCollection<T> GetRange(int startIndex, int endIndex) {
            CheckRangeIndexes(startIndex, endIndex);
            RleCollection<T> result = new RleCollection<T>(endIndex - startIndex + 1);
            result.ImmutableValues = this.ImmutableValues;
            int itemIndex = GetItemIndex(startIndex);
            RleItem<T> sourceItem = items[itemIndex];
            while (sourceItem != null && sourceItem.StartIndex <= endIndex) {
                RleItem<T> item = new RleItem<T>(sourceItem.Value, 
                    Math.Max(sourceItem.StartIndex, startIndex) - startIndex, 
                    Math.Min(sourceItem.EndIndex, endIndex) - startIndex);
                result.items.Add(item);
                itemIndex++;
                sourceItem = itemIndex < items.Count ? items[itemIndex] : null;
            }
            return result;
        }

        public void SetRange(int startIndex, int endIndex, T value) {
            CheckRangeIndexes(startIndex, endIndex);
            List<RleItem<T>> rangeItems = new List<RleItem<T>>();
            rangeItems.Add(new RleItem<T>(value, 0, endIndex - startIndex));
            SetRangeCore(startIndex, rangeItems, null);
        }

        public void SetRange(int startIndex, IRleCollection<T> range) {
            SetRange(startIndex, range, null);
        }

        public void SetRange(int startIndex, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter) {
            Guard.ArgumentNotNull(range, "range");
            CheckRangeIndexes(startIndex, startIndex + range.Length - 1);
            SetRangeCore(startIndex, range.Implementation.items, valueConverter);
        }
        #endregion

        #region Insert
        public void Insert(int index, T value) {
            Insert(index, 1, value);
        }

        public void Insert(int index, int count, T value) {
            CheckIndexAndCount(index, count);
            List<RleItem<T>> rangeItems = new List<RleItem<T>>();
            rangeItems.Add(new RleItem<T>(value, 0, count - 1));
            InsertRangeCore(index, rangeItems, null);
        }

        public void Insert(int index, IRleCollection<T> range) {
            Insert(index, range, null);
        }

        public void Insert(int index, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter) {
            Guard.ArgumentNotNull(range, "range");
            CheckIndexAndCount(index, range.Length);
            InsertRangeCore(index, range.Implementation.items, valueConverter);
        }
        #endregion

        #region Remove
        public void Remove(int index) {
            Remove(index, 1);
        }

        public void Remove(int index, int count) {
            CheckIndexAndCount(index, count);
            T lastValue = GetItem(length - 1).Value;
            int itemIndex = CutIndexRange(index, index + count - 1);
            if (itemIndex == items.Count) {
                items.Insert(itemIndex, new RleItem<T>(lastValue, index, length - 1));
            }
            else {
                for (int i = itemIndex; i < items.Count; i++) {
                    RleItem<T> item = items[i];
                    item.StartIndex -= count;
                    item.EndIndex -= count;
                }
                items[items.Count - 1].EndIndex = length - 1;
            }
            TryMergeWithPrevious(itemIndex);
        }
        #endregion

        #region Count
        public int Count(T value) {
            int result = 0;
            foreach (RleItem<T> item in items) {
                if (AreEqual(value, item.Value))
                    result += item.Count;
            }
            return result;
        }

        public int Count(Predicate<T> match) {
            int result = 0;
            foreach (RleItem<T> item in items) {
                if (match(item.Value))
                    result += item.Count;
            }
            return result;
        }
        #endregion

        #region Internals
        protected virtual void SetValue(int index, T value) {
            int itemIndex = GetItemIndex(index);
            RleItem<T> item = items[itemIndex];
            if (AreEqual(value, item.Value))
                return;

            if (item.StartIndex == item.EndIndex) {
                item.Value = value;
                if (TryMergeWithPrevious(itemIndex))
                    TryMergeWithNext(itemIndex - 1);
                else
                    TryMergeWithNext(itemIndex);
            }
            else if (index == item.StartIndex) {
                item.StartIndex++;
                items.Insert(itemIndex, new RleItem<T>(value, index, index));
                TryMergeWithPrevious(itemIndex);
            }
            else if (index == item.EndIndex) {
                item.EndIndex--;
                items.Insert(itemIndex + 1, new RleItem<T>(value, index, index));
                mruItemIndex++;
                TryMergeWithNext(itemIndex + 1);
            }
            else {
                itemIndex++;
                items.Insert(itemIndex, new RleItem<T>(item.Value, index + 1, item.EndIndex));
                items.Insert(itemIndex, new RleItem<T>(value, index, index));
                item.EndIndex = index - 1;
                mruItemIndex++;
            }
        }

        bool TryMergeWithPrevious(int itemIndex) {
            if (itemIndex == 0)
                return false;
            if (!AreEqual(items[itemIndex - 1].Value, items[itemIndex].Value))
                return false;
            items[itemIndex - 1].EndIndex = items[itemIndex].EndIndex;
            items.RemoveAt(itemIndex);
            mruItemIndex = itemIndex - 1;
            return true;
        }

        bool TryMergeWithNext(int itemIndex) {
            if (itemIndex == (items.Count - 1))
                return false;
            if (!AreEqual(items[itemIndex + 1].Value, items[itemIndex].Value))
                return false;
            items[itemIndex + 1].StartIndex = items[itemIndex].StartIndex;
            items.RemoveAt(itemIndex);
            return true;
        }

        int GetItemIndex(int index) {
            if (mruItemIndex >= 0 && mruItemIndex < items.Count) {
                IIndexRange mruItem = items[mruItemIndex];
                if (index >= mruItem.StartIndex && index <= mruItem.EndIndex)
                    return mruItemIndex;
            }
            mruItemIndex = Algorithms.BinarySearch<RleItem<T>>(items, new IndexRangeComparable(index));
            return mruItemIndex;
        }

        RleItem<T> GetItem(int index) {
            int itemIndex = GetItemIndex(index);
            if (itemIndex >= 0)
                return items[itemIndex];
            return null;
        }

        bool AreEqual(T x, T y) {
            if ((Object)x == null)
                return (Object)y == null;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return comparer.Equals(x, y);
        }

        void SetRangeCore(int startIndex, List<RleItem<T>> rangeItems, IRleCollectionValueConverter<T> valueConverter) {
            int endIndex = startIndex + rangeItems[rangeItems.Count - 1].EndIndex;
            int firstItemIndex = CutIndexRange(startIndex, endIndex);
            int lastItemIndex = firstItemIndex - 1;
            if (valueConverter == null) {
                foreach (RleItem<T> item in rangeItems) {
                    lastItemIndex++;
                    items.Insert(lastItemIndex, new RleItem<T>(item.Value, item.StartIndex + startIndex, item.EndIndex + startIndex));
                }
            }
            else {
                foreach (RleItem<T> item in rangeItems) {
                    lastItemIndex++;
                    items.Insert(lastItemIndex, new RleItem<T>(valueConverter.Convert(item.Value), item.StartIndex + startIndex, item.EndIndex + startIndex));
                }
            }
            TryMergeWithNext(lastItemIndex);
            TryMergeWithPrevious(firstItemIndex);
        }

        int CutIndexRange(int startIndex, int endIndex) {
            int firstItemIndex = GetItemIndex(startIndex);
            int lastItemIndex = GetItemIndex(endIndex);
            if (firstItemIndex == lastItemIndex)
                return CutIndexRangeFromItem(firstItemIndex, startIndex, endIndex);
            RleItem<T> item = items[firstItemIndex];
            if (item.StartIndex < startIndex) {
                item.EndIndex = startIndex - 1;
                firstItemIndex++;
            }
            item = items[lastItemIndex];
            if (item.EndIndex > endIndex) {
                item.StartIndex = endIndex + 1;
                lastItemIndex--;
            }
            int count = lastItemIndex - firstItemIndex + 1;
            if (count > 0)
                items.RemoveRange(firstItemIndex, count);
            return firstItemIndex;
        }

        int CutIndexRangeFromItem(int itemIndex, int startIndex, int endIndex) {
            RleItem<T> item = items[itemIndex];
            if (item.StartIndex == startIndex) {
                if (item.EndIndex == endIndex)
                    items.RemoveAt(itemIndex);
                else
                    item.StartIndex = endIndex + 1;
            }
            else {
                itemIndex++;
                if (item.EndIndex > endIndex)
                    items.Insert(itemIndex, new RleItem<T>(item.Value, endIndex + 1, item.EndIndex));
                item.EndIndex = startIndex - 1;
            }
            return itemIndex;
        }

        void InsertRangeCore(int index, List<RleItem<T>> rangeItems, IRleCollectionValueConverter<T> valueConverter) {
            int count = rangeItems[rangeItems.Count - 1].EndIndex + 1;
            int firstItemIndex = InsertIndexRange(index, count);
            int lastItemIndex = firstItemIndex - 1;
            if (valueConverter == null) {
                foreach (RleItem<T> item in rangeItems) {
                    lastItemIndex++;
                    items.Insert(lastItemIndex, new RleItem<T>(item.Value, item.StartIndex + index, item.EndIndex + index));
                }
            }
            else {
                foreach (RleItem<T> item in rangeItems) {
                    lastItemIndex++;
                    items.Insert(lastItemIndex, new RleItem<T>(valueConverter.Convert(item.Value), item.StartIndex + index, item.EndIndex + index));
                }
            }
            TryMergeWithNext(lastItemIndex);
            TryMergeWithPrevious(firstItemIndex);
        }

        int InsertIndexRange(int index, int count) {
            int itemIndex = GetItemIndex(index);
            RleItem<T> item = items[itemIndex];
            if (item.StartIndex < index) {
                itemIndex++;
                items.Insert(itemIndex, new RleItem<T>(item.Value, index, item.EndIndex));
                item.EndIndex = index - 1;
            }
            for (int i = items.Count - 1; i >= itemIndex; i--) {
                item = items[i];
                if (item.StartIndex + count >= length)
                    items.RemoveAt(i);
                else {
                    item.StartIndex = item.StartIndex + count;
                    item.EndIndex = Math.Min(item.EndIndex + count, length - 1);
                }
            }
            return itemIndex;
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator<T> GetEnumerator() {
            foreach (RleItem<T> item in items) {
                for (int i = item.StartIndex; i <= item.EndIndex; i++)
                    yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        #endregion

        #region Index Iterators
        public IEnumerable<int> GetIndexEnumerable(T value, bool reversed) {
            return new Enumerable<int>(GetIndexEnumerator(value, reversed));
        }

        public IEnumerable<int> GetIndexEnumerable(T value, int startIndex, int endIndex) {
            return new Enumerable<int>(GetIndexEnumerator(value, startIndex, endIndex));
        }

        public IEnumerable<int> GetIndexEnumerable(Predicate<T> match, bool reversed) {
            return new Enumerable<int>(GetIndexEnumerator(match, reversed));
        }

        public IEnumerable<int> GetIndexEnumerable(Predicate<T> match, int startIndex, int endIndex) {
            return new Enumerable<int>(GetIndexEnumerator(match, startIndex, endIndex));
        }

        IEnumerator<int> GetIndexEnumerator(T value, bool reversed) {
            if (reversed)
                return GetIndexEnumerator(value, length - 1, 0);
            return GetIndexEnumerator(value, 0, length - 1);
        }

        IEnumerator<int> GetIndexEnumerator(T value, int startIndex, int endIndex) {
            CheckIndex(startIndex, "startIndex");
            CheckIndex(endIndex, "endIndex");
            return new IndexEnumerator(this, startIndex, endIndex, (x) => AreEqual(value, x));
        }

        IEnumerator<int> GetIndexEnumerator(Predicate<T> match, bool reversed) {
            if (reversed)
                return GetIndexEnumerator(match, length - 1, 0);
            return GetIndexEnumerator(match, 0, length - 1);
        }

        IEnumerator<int> GetIndexEnumerator(Predicate<T> match, int startIndex, int endIndex) {
            CheckIndex(startIndex, "startIndex");
            CheckIndex(endIndex, "endIndex");
            return new IndexEnumerator(this, startIndex, endIndex, match);
        }
        #endregion
    }
    #endregion

    #region UndoableRleCollection
    public class UndoableRleCollection<T> : RleCollectionBase, IRleCollection<T> {
        readonly IDocumentModelPart documentModelPart;
        readonly RleCollection<T> innerCollection;

        public UndoableRleCollection(IDocumentModelPart documentModelPart, int length, T defaultValue) {
            this.documentModelPart = documentModelPart;
            this.innerCollection = new RleCollection<T>(length, defaultValue);
        }

        #region Properties
        public IDocumentModel DocumentModel { get { return documentModelPart.DocumentModel; } }
        public IDocumentModelPart DocumentModelPart { get { return documentModelPart; } }
        protected DocumentHistory History { get { return documentModelPart.DocumentModel.History; } }

        public T this[int index] {
            get { return innerCollection[index]; }
            set {
                UndoableRleCollectionSetValueHistoryItem<T> historyItem = new UndoableRleCollectionSetValueHistoryItem<T>(DocumentModel, innerCollection, index, value);
                History.Add(historyItem);
                historyItem.Execute();
            }
        }

        public override int Length { get { return innerCollection.Length; } }
        public RleCollection<T> Implementation { get { return innerCollection; } }

        public bool ImmutableValues {
            get { return innerCollection.ImmutableValues; }
            set { innerCollection.ImmutableValues = value; }
        }
        #endregion

        #region Get/SetRange
        public IRleCollection<T> GetRange(int startIndex, int endIndex) {
            return innerCollection.GetRange(startIndex, endIndex);
        }

        public void SetRange(int startIndex, int endIndex, T value) {
            CheckRangeIndexes(startIndex, endIndex);
            UndoableRleCollectionSetRangeValueHistoryItem<T> historyItem = new UndoableRleCollectionSetRangeValueHistoryItem<T>(DocumentModel, innerCollection, startIndex, endIndex, value);
            History.Add(historyItem);
            historyItem.Execute();
        }

        public void SetRange(int startIndex, IRleCollection<T> range) {
            SetRange(startIndex, range, null);
        }

        public void SetRange(int startIndex, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter) {
            Guard.ArgumentNotNull(range, "range");
            CheckRangeIndexes(startIndex, startIndex + range.Length - 1);
            UndoableRleCollectionSetRangeHistoryItem<T> historyItem = new UndoableRleCollectionSetRangeHistoryItem<T>(DocumentModel, innerCollection, startIndex, range, valueConverter);
            History.Add(historyItem);
            historyItem.Execute();
        }
        #endregion

        #region Insert
        public void Insert(int index, T value) {
            Insert(index, 1, value);
        }

        public void Insert(int index, int count, T value) {
            CheckIndexAndCount(index, count);
            UndoableRleCollectionInsertValueHistoryItem<T> historyItem = new UndoableRleCollectionInsertValueHistoryItem<T>(DocumentModel, innerCollection, index, count, value);
            History.Add(historyItem);
            historyItem.Execute();
        }

        public void Insert(int index, IRleCollection<T> range) {
            Insert(index, range, null);
        }

        public void Insert(int index, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter) {
            Guard.ArgumentNotNull(range, "range");
            CheckIndexAndCount(index, range.Length);
            UndoableRleCollectionInsertRangeHistoryItem<T> historyItem = new UndoableRleCollectionInsertRangeHistoryItem<T>(DocumentModel, innerCollection, index, range,valueConverter);
            History.Add(historyItem);
            historyItem.Execute();
        }
        #endregion

        #region Remove
        public void Remove(int index) {
            Remove(index, 1);
        }

        public void Remove(int index, int count) {
            CheckIndexAndCount(index, count);
            UndoableRleCollectionRemoveRangeHistoryItem<T> historyItem = new UndoableRleCollectionRemoveRangeHistoryItem<T>(DocumentModel, innerCollection, index, count);
            History.Add(historyItem);
            historyItem.Execute();
        }
        #endregion

        #region Count
        public int Count(T value) {
            return innerCollection.Count(value);
        }

        public int Count(Predicate<T> match) {
            return innerCollection.Count(match);
        }
        #endregion

        #region Index iterators
        public IEnumerable<int> GetIndexEnumerable(T value, bool reversed) {
            return innerCollection.GetIndexEnumerable(value, reversed);
        }

        public IEnumerable<int> GetIndexEnumerable(T value, int startIndex, int endIndex) {
            return innerCollection.GetIndexEnumerable(value, startIndex, endIndex);
        }

        public IEnumerable<int> GetIndexEnumerable(Predicate<T> match, bool reversed) {
            return innerCollection.GetIndexEnumerable(match, reversed);
        }

        public IEnumerable<int> GetIndexEnumerable(Predicate<T> match, int startIndex, int endIndex) {
            return innerCollection.GetIndexEnumerable(match, startIndex, endIndex);
        }
        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        #endregion
    }
    #endregion

    #region HistoryItems
    #region UndoableRleCollectionSetValueHistoryItem
    class UndoableRleCollectionSetValueHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int index;
        readonly T newValue;
        T oldValue;

        public UndoableRleCollectionSetValueHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int index, T value)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.newValue = value;
            this.index = index;
        }

        protected override void UndoCore() {
            collection[index] = oldValue;
        }

        protected override void RedoCore() {
            oldValue = collection[index];
            collection[index] = newValue;
        }
    }
    #endregion
    #region UndoableRleCollectionSetRangeValueHistoryItem
    class UndoableRleCollectionSetRangeValueHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int startIndex;
        readonly int endIndex;
        readonly T newValue;
        IRleCollection<T> oldRange = null;

        public UndoableRleCollectionSetRangeValueHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int startIndex, int endIndex, T value)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.newValue = value;
        }

        protected override void UndoCore() {
            collection.SetRange(startIndex, oldRange);
            oldRange = null;
        }

        protected override void RedoCore() {
            oldRange = collection.GetRange(startIndex, endIndex);
            collection.SetRange(startIndex, endIndex, newValue);
        }
    }
    #endregion
    #region UndoableRleCollectionSetRangeHistoryItem
    class UndoableRleCollectionSetRangeHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int startIndex;
        readonly IRleCollection<T> newRange;
        readonly IRleCollectionValueConverter<T> valueConverter;
        IRleCollection<T> oldRange = null;

        public UndoableRleCollectionSetRangeHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int startIndex, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.startIndex = startIndex;
            this.newRange = range;
            this.valueConverter = valueConverter;
        }

        protected override void UndoCore() {
            collection.SetRange(startIndex, oldRange);
            oldRange = null;
        }

        protected override void RedoCore() {
            oldRange = collection.GetRange(startIndex, startIndex + newRange.Length - 1);
            collection.SetRange(startIndex, newRange, valueConverter);
        }
    }
    #endregion
    #region UndoableRleCollectionInsertValueHistoryItem
    class UndoableRleCollectionInsertValueHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int index;
        readonly int count;
        readonly T newValue;
        IRleCollection<T> savedRange = null;

        public UndoableRleCollectionInsertValueHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int index, int count, T value)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.index = index;
            this.count = count;
            this.newValue = value;
        }

        protected override void UndoCore() {
            collection.Remove(index, count);
            collection.SetRange(collection.Length - count, savedRange);
            savedRange = null;
        }
        protected override void RedoCore() {
            savedRange = collection.GetRange(collection.Length - count, collection.Length - 1);
            collection.Insert(index, count, newValue);
        }
    }
    #endregion
    #region UndoableRleCollectionInsertRangeHistoryItem
    class UndoableRleCollectionInsertRangeHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int index;
        readonly IRleCollection<T> newRange;
        readonly IRleCollectionValueConverter<T> valueConverter;
        IRleCollection<T> savedRange = null;

        public UndoableRleCollectionInsertRangeHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int index, IRleCollection<T> range, IRleCollectionValueConverter<T> valueConverter)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.index = index;
            this.newRange = range;
            this.valueConverter = valueConverter;
        }

        protected override void UndoCore() {
            collection.Remove(index, newRange.Length);
            collection.SetRange(collection.Length - newRange.Length, savedRange);
            savedRange = null;
        }

        protected override void RedoCore() {
            savedRange = collection.GetRange(collection.Length - newRange.Length, collection.Length - 1);
            collection.Insert(index, newRange, valueConverter);
        }
    }
    #endregion
    #region UndoableRleCollectionRemoveRangeHistoryItem
    class UndoableRleCollectionRemoveRangeHistoryItem<T> : HistoryItem {
        readonly RleCollection<T> collection;
        readonly int index;
        readonly int count;
        IRleCollection<T> savedRange = null;

        public UndoableRleCollectionRemoveRangeHistoryItem(IDocumentModel documentModel, RleCollection<T> collection, int index, int count)
            : base(documentModel.MainPart) {
            this.collection = collection;
            this.index = index;
            this.count = count;
        }

        protected override void UndoCore() {
            collection.Insert(index, savedRange);
            savedRange = null;
        }
        
        protected override void RedoCore() {
            savedRange = collection.GetRange(index, index + count - 1);
            collection.Remove(index, count);
        }
    }
    #endregion
    #endregion
}
