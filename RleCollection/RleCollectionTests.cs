using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using NUnit.Framework;

namespace Utils.Tests {
    #region TestRleCollectionValueConverter
    class TestRleCollectionValueConverter : IRleCollectionValueConverter<int> {
        public int Convert(int value) {
            return value * 10;
        }
    }
    #endregion

    #region RleCollectionTests (int)
    [TestFixture]
    [GeneratedCode("Suppress FxCop check", "")]
    public class RleCollectionTests {
        RleCollection<int> collection;

        [SetUp]
        public void SetUp() {
            collection = new RleCollection<int>(5, 2);
        }

        [TearDown]
        public void TearDown() {
            collection = null;
        }

        void CheckValues(int[] expected) {
            CheckValues(collection, expected);
        }

        void CheckValues(RleCollection<int> collectionToTest, int[] expected) {
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], collectionToTest[i]);
        }

        [Test]
        public void InitialState() {
            Assert.AreEqual(true, collection.ImmutableValues);
            Assert.AreEqual(5, collection.Length);
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        #region SetValue
        [Test]
        public void SetValueAtStart() {
            collection[0] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 2, 2, 2 });

            collection[0] = 2;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void SetValueAtEnd() {
            collection[4] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(1, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 3 });

            collection[4] = 2;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void SetValueInside() {
            collection[2] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(1, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 3, 2, 2 });

            collection[2] = 2;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }
        #endregion

        #region SplitMerge
        [Test]
        public void SplitMergeInsideOut() {
            collection[2] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(1, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 3, 2, 2 });

            collection[1] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(1, collection.MruItemIndex);
            CheckValues(new int[] { 2, 3, 3, 2, 2 });

            collection[0] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 2, 2 });

            collection[3] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 2 });

            collection[4] = 3;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 3 });
        }

        [Test]
        public void SplitMergeOutsideIn() {
            collection[0] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 2, 2, 2 });

            collection[4] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(2, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 2, 2, 3 });

            collection[1] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 2, 2, 3 });

            collection[3] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(2, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 2, 3, 3 });

            collection[2] = 3;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 3 });
        }

        [Test]
        public void SplitMergeEvenOdd() {
            collection[0] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 2, 2, 2 });

            collection[2] = 3;
            Assert.AreEqual(4, collection.ItemsCount);
            Assert.AreEqual(2, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 3, 2, 2 });

            collection[4] = 3;
            Assert.AreEqual(5, collection.ItemsCount);
            Assert.AreEqual(4, collection.MruItemIndex);
            CheckValues(new int[] { 3, 2, 3, 2, 3 });

            collection[1] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 2, 3 });

            collection[3] = 3;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 3 });
        }

        [Test]
        public void SplitMergeOddEven() {
            collection[1] = 3;
            Assert.AreEqual(3, collection.ItemsCount);
            Assert.AreEqual(1, collection.MruItemIndex);
            CheckValues(new int[] { 2, 3, 2, 2, 2 });

            collection[3] = 3;
            Assert.AreEqual(5, collection.ItemsCount);
            Assert.AreEqual(3, collection.MruItemIndex);
            CheckValues(new int[] { 2, 3, 2, 3, 2 });

            collection[0] = 3;
            Assert.AreEqual(4, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 2, 3, 2 });

            collection[2] = 3;
            Assert.AreEqual(2, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 2 });

            collection[4] = 3;
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 3, 3, 3, 3, 3 });
        }
        #endregion

        #region IndexOutOfRange
        [Test]
        public void IndexOutOfRangeLowOnGet() {
            Assert.Throws<IndexOutOfRangeException>(() => { int value = collection[-1]; });
        }

        [Test]
        public void IndexOutOfRangeHighOnGet() {
            Assert.Throws<IndexOutOfRangeException>(() => { int value = collection[5]; });
        }

        [Test]
        public void IndexOutOfRangeLowOnSet() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection[-1] = 1; });
        }

        [Test]
        public void IndexOutOfRangeHighOnSet() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection[5] = 1; });
        }
        #endregion

        #region Count
        [Test]
        public void CountValue() {
            Assert.AreEqual(0, collection.Count(3));
            Assert.AreEqual(5, collection.Count(2));

            collection[3] = 3;
            Assert.AreEqual(1, collection.Count(3));
            Assert.AreEqual(4, collection.Count(2));

            collection[0] = 1;
            Assert.AreEqual(1, collection.Count(3));
            Assert.AreEqual(3, collection.Count(2));
            Assert.AreEqual(1, collection.Count(1));
        }

        [Test]
        public void CountPredicate() {
            Assert.AreEqual(0, collection.Count((x) => x == 3));
            Assert.AreEqual(5, collection.Count((x) => x == 2));

            collection[3] = 3;
            Assert.AreEqual(1, collection.Count((x) => x == 3));
            Assert.AreEqual(4, collection.Count((x) => x == 2));

            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;
            Assert.AreEqual(3, collection.Count((x) => x <= 3));
            Assert.AreEqual(2, collection.Count((x) => x > 3));
        }
        #endregion

        #region IEnumerable
        [Test]
        public void IEnumerableTest() {
            foreach (int value in collection)
                Assert.AreEqual(2, value);

            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;
            int expected = 1;
            int count = 0;
            foreach (int value in collection) {
                Assert.AreEqual(expected, value);
                expected++;
                count++;
            }
            Assert.AreEqual(5, count);
        }
        #endregion

        #region IndexIterators
        [Test]
        public void IndexIteratorValue() {
            int[] expected = new int[] { 0, 1, 2, 3, 4 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(5, i);

            collection[2] = 3;
            expected = new int[] { 0, 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[0] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 1, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueReversed() {
            int[] expected = new int[] { 4, 3, 2, 1, 0 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(5, i);

            collection[2] = 3;
            expected = new int[] { 4, 3, 1, 0 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[0] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueIndexRange() {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[2] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[0] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 1, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueIndexRangeReversed() {
            int[] expected = new int[] { 4, 3, 2, 1 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[2] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[0] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicate() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 2, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            expected = new int[] { 0, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, false)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateReversed() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 4, 3, 2 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            expected = new int[] { 1, 0 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, true)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateRange() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 2, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, 1, 3)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            expected = new int[] { 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, 1, 3)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, 1, 3)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateRangeReversed() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 3, 2 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, 3, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            expected = new int[] { 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, 3, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, 3, 1)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexOutOfRangeIteratorStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetIndexEnumerable(2, -1, 4); });
        }

        [Test]
        public void IndexOutOfRangeIteratorEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetIndexEnumerable(2, 0, 5); });
        }
        #endregion

        #region GetRange
        [Test]
        public void GetRangeSimple() {
            RleCollection<int> range = collection.GetRange(1, 3).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(1, range.ItemsCount);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(0, 2).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(1, range.ItemsCount);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(2, 4).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(1, range.ItemsCount);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(2, 2).Implementation;
            Assert.AreEqual(1, range.Length);
            Assert.AreEqual(1, range.ItemsCount);
            CheckValues(range, new int[] { 2 });
        }

        [Test]
        public void GetRangeComplex() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            RleCollection<int> range = collection.GetRange(1, 3).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(3, range.ItemsCount);
            CheckValues(range, new int[] { 2, 3, 4 });

            range = collection.GetRange(2, 2).Implementation;
            Assert.AreEqual(1, range.Length);
            Assert.AreEqual(1, range.ItemsCount);
            CheckValues(range, new int[] { 3 });

            collection[0] = 2;
            collection[4] = 4;

            range = collection.GetRange(1, 3).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(3, range.ItemsCount);
            CheckValues(range, new int[] { 2, 3, 4 });

            collection[2] = 2;

            range = collection.GetRange(1, 3).Implementation;
            Assert.AreEqual(3, range.Length);
            Assert.AreEqual(2, range.ItemsCount);
            CheckValues(range, new int[] { 2, 2, 4 });
        }

        [Test]
        public void CheckRangeIndexesGetRangeStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetRange(-1, 3); });
        }

        [Test]
        public void CheckRangeIndexesGetRangeEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetRange(0, 5); });
        }

        [Test]
        public void CheckRangeIndexesGetRangeStartIndexGreaterThanEndIndex() {
            Assert.Throws<ArgumentException>(() => { collection.GetRange(2, 1); });
        }
        #endregion

        #region SetRange
        [Test]
        public void SetRangeValue() {
            collection.SetRange(1, 3, 5);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 5, 5, 5, 2 });

            collection.SetRange(0, 4, 2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.SetRange(0, 2, 5);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 2, 2 });

            collection.SetRange(0, 4, 2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.SetRange(2, 4, 5);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 5, 5, 5 });

            collection.SetRange(0, 4, 2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.SetRange(0, 4, 5);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 5 });
        }

        [Test]
        public void SetRangeRange() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.SetRange(1, range);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 5, 5, 5, 2 });

            range = new RleCollection<int>(3, 2);
            collection.SetRange(1, range);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            range[0] = 3;
            range[1] = 4;
            range[2] = 4;

            collection.SetRange(0, range);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 3, 4, 4, 2, 2 });

            collection.SetRange(2, range);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 3, 4, 3, 4, 4 });

            range = new RleCollection<int>(5, 2);
            collection.SetRange(0, range);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            range = new RleCollection<int>(5, 5);
            collection.SetRange(0, range);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 5 });
        }

        [Test]
        public void IndexesSetRangeStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(-1, 3, 2); });
        }

        [Test]
        public void IndexesSetRangeEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(0, 5, 5); });
        }

        [Test]
        public void IndexesSetRangeStartIndexGreaterThanEndIndex() {
            Assert.Throws<ArgumentException>(() => { collection.SetRange(2, 1, 5); });
        }

        [Test]
        public void SetRangeNullRange() {
            Assert.Throws<ArgumentNullException>(() => { collection.SetRange(0, null); });
        }

        [Test]
        public void SetRangeCount() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(3, range); });
        }
        #endregion

        #region Insert
        [Test]
        public void InsertValue() {
            collection.Insert(0, 5);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 2, 2, 2 });
            
            collection.Insert(4, 5);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 2, 2, 5 });

            collection.Insert(2, 5);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 5, 2, 2 });

            collection.Insert(1, 2);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 2, 5, 2 });

            collection.Insert(3, 2);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 2, 2, 5 });
        }

        [Test]
        public void InsertRangeValue() {
            collection.Insert(0, 2, 5);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 2, 2, 2 });

            collection.Insert(3, 2, 5);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 2, 5, 5 });

            collection.Insert(2, 2, 5);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 2 });

            collection.Insert(1, 3, 2);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 5, 2, 2, 2, 5 });

            collection.Insert(0, 5, 2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void InsertRange() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.Insert(0, range);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 2, 2 });

            collection.SetRange(0, 4, 2);
            collection.Insert(2, range);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 5, 5, 5 });

            collection.SetRange(0, 4, 2);
            collection.Insert(1, range);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 5, 5, 5, 2 });

            range[1] = 6;
            range[2] = 7;

            collection.SetRange(0, 4, 2);
            collection.Insert(0, range);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 5, 6, 7, 2, 2 });

            collection.SetRange(0, 4, 2);
            collection.Insert(2, range);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 5, 6, 7 });

            collection.SetRange(0, 4, 2);
            collection.Insert(1, range);
            Assert.AreEqual(5, collection.ItemsCount);
            CheckValues(new int[] { 2, 5, 6, 7, 2 });

            range.SetRange(0, 2, 2);
            collection.SetRange(0, 4, 2);
            collection.Insert(1, range);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void InsertValueInvalidIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Insert(-1, 2); });
        }

        [Test]
        public void InsertRangeValueNegativeCount() {
            Assert.Throws<ArgumentException>(() => { collection.Insert(0, -1, 3); });
        }

        [Test]
        public void InsertRangeInvalidIndex() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Insert(-1, range); });
        }

        [Test]
        public void InsertRangeNullRange() {
            Assert.Throws<ArgumentNullException>(() => { collection.Insert(0, null); });
        }

        [Test]
        public void InsertRangeInvalidCount() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<ArgumentException>(() => { collection.Insert(3, range); });
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveOne() {
            collection.Remove(0);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.Remove(2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.Remove(4);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            collection.Remove(0);
            Assert.AreEqual(4, collection.ItemsCount);
            CheckValues(new int[] { 2, 3, 4, 5, 5 });

            collection.Remove(1);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 4, 5, 5, 5 });

            collection.Remove(3);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 4, 5, 5, 5 });

            collection.Remove(1);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 2, 5, 5, 5, 5 });

            collection.Remove(0);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 5 });
        }

        [Test]
        public void RemoveRange() {
            collection.Remove(0, 5);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.Remove(0, 3);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.Remove(3, 2);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            collection.Remove(1, 3);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            collection.Remove(0, 5);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(0, 4);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new int[] { 5, 5, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(0, 3);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 4, 5, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(0, 2);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 3, 4, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(1, 3);
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 1, 5, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(1, 2);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 1, 4, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(2, 3);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 1, 2, 5, 5, 5 });

            for (int i = 0; i < collection.Length; i++)
                collection[i] = i + 1;
            collection.Remove(2, 2);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 1, 2, 5, 5, 5 });
        }

        [Test]
        public void RemoveRangeNegativeIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Remove(-1, 2); });
        }

        [Test]
        public void RemoveRangeIndexOutOfRange() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Remove(5, 2); });
        }

        [Test]
        public void RemoveRangeNegativeCount() {
            Assert.Throws<ArgumentException>(() => { collection.Remove(0, -2); });
        }

        [Test]
        public void RemoveRangeIndexPlusCountOutOfRange() {
            Assert.Throws<ArgumentException>(() => { collection.Remove(3, 3); });
        }
        #endregion

        #region ValueConverter
        [Test]
        public void SetRangeRangeWithValueConverter() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.SetRange(1, range, new TestRleCollectionValueConverter());
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new int[] { 2, 50, 50, 50, 2 });
        }

        [Test]
        public void InsertRangeWithValueConverter() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.Insert(0, range, new TestRleCollectionValueConverter());
            Assert.AreEqual(2, collection.ItemsCount);
            CheckValues(new int[] { 50, 50, 50, 2, 2 });
        }
        #endregion
    }
    #endregion

    #region RleCollectionTests (string)
    [TestFixture]
    [GeneratedCode("Suppress FxCop check", "")]
    public class RleCollectionStringTests {
        RleCollection<string> collection;

        [SetUp]
        public void SetUp() {
            collection = new RleCollection<string>(5, string.Empty);
        }

        [TearDown]
        public void TearDown() {
            collection = null;
        }

        void CheckValues(string[] expected) {
            CheckValues(collection, expected);
        }

        void CheckValues(RleCollection<string> collectionToTest, string[] expected) {
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], collectionToTest[i]);
        }

        [Test]
        public void InitialState() {
            Assert.AreEqual(true, collection.ImmutableValues);
            Assert.AreEqual(5, collection.Length);
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty });
        }

        [Test]
        public void SetNullValues() {
            collection.SetRange(1, 3, null);
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new string[] { string.Empty, null, null, null, string.Empty });

            collection.SetRange(1, 3, "a");
            Assert.AreEqual(3, collection.ItemsCount);
            CheckValues(new string[] { string.Empty, "a", "a", "a", string.Empty });

            collection.SetRange(1, 3, string.Empty);
            Assert.AreEqual(1, collection.ItemsCount);
            CheckValues(new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty });
        }
    }
    #endregion

    #region RleCollectionTests (immutable)
    [TestFixture]
    [GeneratedCode("Suppress FxCop check", "")]
    public class RleCollectionImmutableTests {

        class TestRleCollectionValue : ICloneable {
            public int Value { get; set; }

            public override bool Equals(object obj) {
                TestRleCollectionValue other = obj as TestRleCollectionValue;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public override int GetHashCode() {
                return Value.GetHashCode();
            }

            #region ICloneable Members

            public object Clone() {
                TestRleCollectionValue result = new TestRleCollectionValue();
                result.Value = Value;
                return result;
            }

            #endregion
        }

        RleCollection<TestRleCollectionValue> collection;

        [SetUp]
        public void SetUp() {
            collection = new RleCollection<TestRleCollectionValue>(5, new TestRleCollectionValue() { Value = 2 });
        }

        [TearDown]
        public void TearDown() {
            collection = null;
        }

        void CheckValues(int[] expected) {
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], collection[i].Value);
        }

        [Test]
        public void InitialState() {
            Assert.AreEqual(false, collection.ImmutableValues);
            Assert.AreEqual(5, collection.Length);
            Assert.AreEqual(1, collection.ItemsCount);
            Assert.AreEqual(0, collection.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void GetAndChangeValueProperty() {
            collection[2].Value = 5;
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void SetImmutableValues() {
            collection.ImmutableValues = true;
            collection[2].Value = 5;
            CheckValues(new int[] { 5, 5, 5, 5, 5 });
        }
    }
    #endregion

    #region RleCollectionTests (mutable and not cloneable)
    [TestFixture]
    [GeneratedCode("Suppress FxCop check", "")]
    public class RleCollectionMutableAndNotCloneableTests {

        class TestRleCollectionValue {
            public int Value { get; set; }

            public override bool Equals(object obj) {
                TestRleCollectionValue other = obj as TestRleCollectionValue;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public override int GetHashCode() {
                return Value.GetHashCode();
            }
        }

        [Test]
        public void InvalidOperationOnGetValue() {
            RleCollection<TestRleCollectionValue> collection = new RleCollection<TestRleCollectionValue>(5, new TestRleCollectionValue() { Value = 2 });
            Assert.Throws<InvalidOperationException>(() => { TestRleCollectionValue value = collection[2]; });
        }
    }
    #endregion

    #region UndoableRleCollectionTests (int)
    [TestFixture]
    [GeneratedCode("Suppress FxCop check", "")]
    public class UndoableRleCollectionTests {
        DocumentModel workbook;
        UndoableRleCollection<int> collection;

        [SetUp]
        public void SetUp() {
            workbook = new DocumentModel();
            workbook.Sheets.Add(workbook.CreateWorksheet());
            collection = new UndoableRleCollection<int>(workbook.Sheets[0], 5, 2);
        }

        [TearDown]
        public void TearDown() {
            collection = null;
            workbook.Dispose();
            workbook = null;
        }

        DocumentHistory History { get { return workbook.History; } }

        void CheckValues(int[] expected) {
            CheckValues(collection, expected);
        }

        void CheckValues(IRleCollection<int> collectionToTest, int[] expected) {
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], collectionToTest[i]);
        }

        [Test]
        public void InitialState() {
            Assert.AreEqual(true, collection.ImmutableValues);
            Assert.AreEqual(5, collection.Length);
            Assert.AreEqual(1, collection.Implementation.ItemsCount);
            Assert.AreEqual(0, collection.Implementation.MruItemIndex);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
        }

        #region SetValue
        [Test]
        public void SetValue() {
            int historyCount = History.Count;

            collection[2] = 3;
            CheckValues(new int[] { 2, 2, 3, 2, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Redo();
            CheckValues(new int[] { 2, 2, 3, 2, 2 });
        }
        #endregion

        #region IndexOutOfRange
        [Test]
        public void IndexOutOfRangeLowOnGet() {
            Assert.Throws<IndexOutOfRangeException>(() => { int value = collection[-1]; });
        }

        [Test]
        public void IndexOutOfRangeHighOnGet() {
            Assert.Throws<IndexOutOfRangeException>(() => { int value = collection[5]; });
        }

        [Test]
        public void IndexOutOfRangeLowOnSet() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection[-1] = 1; });
        }

        [Test]
        public void IndexOutOfRangeHighOnSet() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection[5] = 1; });
        }
        #endregion

        #region Count
        [Test]
        public void CountValue() {
            Assert.AreEqual(0, collection.Count(3));
            Assert.AreEqual(5, collection.Count(2));

            collection[3] = 3;
            Assert.AreEqual(1, collection.Count(3));
            Assert.AreEqual(4, collection.Count(2));

            collection[0] = 1;
            Assert.AreEqual(1, collection.Count(3));
            Assert.AreEqual(3, collection.Count(2));
            Assert.AreEqual(1, collection.Count(1));
        }

        [Test]
        public void CountPredicate() {
            Assert.AreEqual(0, collection.Count((x) => x == 3));
            Assert.AreEqual(5, collection.Count((x) => x == 2));

            collection[3] = 3;
            Assert.AreEqual(1, collection.Count((x) => x == 3));
            Assert.AreEqual(4, collection.Count((x) => x == 2));

            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;
            Assert.AreEqual(3, collection.Count((x) => x <= 3));
            Assert.AreEqual(2, collection.Count((x) => x > 3));
        }
        #endregion

        #region IEnumerable
        [Test]
        public void IEnumerableTest() {
            foreach (int value in collection)
                Assert.AreEqual(2, value);

            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;
            int expected = 1;
            int count = 0;
            foreach (int value in collection) {
                Assert.AreEqual(expected, value);
                expected++;
                count++;
            }
            Assert.AreEqual(5, count);
        }
        #endregion

        #region IndexIterators
        [Test]
        public void IndexIteratorValue() {
            int[] expected = new int[] { 0, 1, 2, 3, 4 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(5, i);

            collection[2] = 3;
            expected = new int[] { 0, 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[0] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 1, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, false)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueReversed() {
            int[] expected = new int[] { 4, 3, 2, 1, 0 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(5, i);

            collection[2] = 3;
            expected = new int[] { 4, 3, 1, 0 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[0] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, true)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueIndexRange() {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[2] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[0] = 3;
            expected = new int[] { 1, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 1, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 1, 4)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorValueIndexRangeReversed() {
            int[] expected = new int[] { 4, 3, 2, 1 };
            int i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(4, i);

            collection[2] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[0] = 3;
            expected = new int[] { 4, 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            collection[4] = 3;
            expected = new int[] { 3, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            collection[1] = 3;
            expected = new int[] { 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            collection[3] = 3;
            i = 0;
            foreach (int index in collection.GetIndexEnumerable(2, 4, 1)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicate() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 2, 3, 4 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            expected = new int[] { 0, 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, false)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, false)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateReversed() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 4, 3, 2 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(3, i);

            expected = new int[] { 1, 0 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, true)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, true)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateRange() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 2, 3 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, 1, 3)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            expected = new int[] { 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, 1, 3)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, 1, 3)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexIteratorPredicateRangeReversed() {
            int i = 0;
            for (i = 0; i < 5; i++)
                collection[i] = i + 1;

            int[] expected = new int[] { 3, 2 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 2, 3, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(2, i);

            expected = new int[] { 1 };
            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x <= 2, 3, 1)) {
                Assert.AreEqual(expected[i], index);
                i++;
            }
            Assert.AreEqual(1, i);

            i = 0;
            foreach (int index in collection.GetIndexEnumerable((x) => x > 5, 3, 1)) {
                i++;
            }
            Assert.AreEqual(0, i);
        }

        [Test]
        public void IndexOutOfRangeIteratorStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetIndexEnumerable(2, -1, 4); });
        }

        [Test]
        public void IndexOutOfRangeIteratorEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetIndexEnumerable(2, 0, 5); });
        }
        #endregion

        #region GetRange
        [Test]
        public void GetRangeSimple() {
            IRleCollection<int> range = collection.GetRange(1, 3);
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(0, 2);
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(2, 4);
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 2, 2 });

            range = collection.GetRange(2, 2);
            Assert.AreEqual(1, range.Length);
            CheckValues(range, new int[] { 2 });
        }

        [Test]
        public void GetRangeComplex() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            IRleCollection<int> range = collection.GetRange(1, 3);
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 3, 4 });

            range = collection.GetRange(2, 2);
            Assert.AreEqual(1, range.Length);
            CheckValues(range, new int[] { 3 });

            collection[0] = 2;
            collection[4] = 4;

            range = collection.GetRange(1, 3);
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 3, 4 });

            collection[2] = 2;

            range = collection.GetRange(1, 3).Implementation;
            Assert.AreEqual(3, range.Length);
            CheckValues(range, new int[] { 2, 2, 4 });
        }

        [Test]
        public void CheckRangeIndexesGetRangeStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetRange(-1, 3); });
        }

        [Test]
        public void CheckRangeIndexesGetRangeEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.GetRange(0, 5); });
        }

        [Test]
        public void CheckRangeIndexesGetRangeStartIndexGreaterThanEndIndex() {
            Assert.Throws<ArgumentException>(() => { collection.GetRange(2, 1); });
        }
        #endregion

        #region SetRange
        [Test]
        public void SetRangeValue() {
            int historyCount = History.Count;

            collection.SetRange(1, 3, 5);
            CheckValues(new int[] { 2, 5, 5, 5, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Redo();
            CheckValues(new int[] { 2, 5, 5, 5, 2 });
        }

        [Test]
        public void SetRangeRange() {
            int historyCount = History.Count;

            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.SetRange(1, range);
            CheckValues(new int[] { 2, 5, 5, 5, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Redo();
            CheckValues(new int[] { 2, 5, 5, 5, 2 });
        }

        [Test]
        public void IndexesSetRangeStartIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(-1, 3, 2); });
        }

        [Test]
        public void IndexesSetRangeEndIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(0, 5, 5); });
        }

        [Test]
        public void IndexesSetRangeStartIndexGreaterThanEndIndex() {
            Assert.Throws<ArgumentException>(() => { collection.SetRange(2, 1, 5); });
        }

        [Test]
        public void SetRangeNullRange() {
            Assert.Throws<ArgumentNullException>(() => { collection.SetRange(0, null); });
        }

        [Test]
        public void SetRangeCount() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { collection.SetRange(3, range); });
        }
        #endregion

        #region Insert
        [Test]
        public void InsertRangeValue() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            int historyCount = History.Count;

            collection.Insert(0, 2, 5);
            CheckValues(new int[] { 5, 5, 1, 2, 3 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 5, 5, 1, 2, 3 });

            History.Undo();
            collection.Insert(0, 5, 2);
            CheckValues(new int[] { 2, 2, 2, 2, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Undo();
            collection.Insert(4, 1, 0);
            CheckValues(new int[] { 1, 2, 3, 4, 0 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 1, 2, 3, 4, 0 });
        }

        [Test]
        public void InsertRange() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            int historyCount = History.Count;

            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.Insert(0, range);
            CheckValues(new int[] { 5, 5, 5, 1, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 5, 5, 5, 1, 2 });
        }

        [Test]
        public void InsertValueInvalidIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Insert(-1, 2); });
        }

        [Test]
        public void InsertRangeValueNegativeCount() {
            Assert.Throws<ArgumentException>(() => { collection.Insert(0, -1, 3); });
        }

        [Test]
        public void InsertRangeInvalidIndex() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Insert(-1, range); });
        }

        [Test]
        public void InsertRangeNullRange() {
            Assert.Throws<ArgumentNullException>(() => { collection.Insert(0, null); });
        }

        [Test]
        public void InsertRangeInvalidCount() {
            RleCollection<int> range = new RleCollection<int>(3, 5);
            Assert.Throws<ArgumentException>(() => { collection.Insert(3, range); });
        }
        #endregion

        #region Remove
        [Test]
        public void RemoveOne() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            int historyCount = History.Count;

            collection.Remove(2);
            CheckValues(new int[] { 1, 2, 4, 5, 5 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 1, 2, 4, 5, 5 });
        }

        [Test]
        public void RemoveRange() {
            for (int i = 0; i < 5; i++)
                collection[i] = i + 1;

            int historyCount = History.Count;

            collection.Remove(2, 2);
            CheckValues(new int[] { 1, 2, 5, 5, 5 });

            History.Undo();
            CheckValues(new int[] { 1, 2, 3, 4, 5 });

            History.Redo();
            CheckValues(new int[] { 1, 2, 5, 5, 5 });
        }

        [Test]
        public void RemoveRangeNegativeIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Remove(-1, 2); });
        }

        [Test]
        public void RemoveRangeIndexOutOfRange() {
            Assert.Throws<IndexOutOfRangeException>(() => { collection.Remove(5, 2); });
        }

        [Test]
        public void RemoveRangeNegativeCount() {
            Assert.Throws<ArgumentException>(() => { collection.Remove(0, -2); });
        }

        [Test]
        public void RemoveRangeIndexPlusCountOutOfRange() {
            Assert.Throws<ArgumentException>(() => { collection.Remove(3, 3); });
        }
        #endregion

        #region ValueConverter
        [Test]
        public void SetRangeRangeWithValueConverter() {
            int historyCount = History.Count;

            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.SetRange(1, range, new TestRleCollectionValueConverter());
            CheckValues(new int[] { 2, 50, 50, 50, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Redo();
            CheckValues(new int[] { 2, 50, 50, 50, 2 });
        }
        
        [Test]
        public void InsertRangeWithValueConverter() {
            int historyCount = History.Count;

            RleCollection<int> range = new RleCollection<int>(3, 5);
            collection.Insert(0, range, new TestRleCollectionValueConverter());
            CheckValues(new int[] { 50, 50, 50, 2, 2 });
            Assert.AreEqual(historyCount + 1, History.Count);

            History.Undo();
            CheckValues(new int[] { 2, 2, 2, 2, 2 });

            History.Redo();
            CheckValues(new int[] { 50, 50, 50, 2, 2 });
        }
        #endregion
    }
    #endregion
}
