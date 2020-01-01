using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EAShow.Infrastructure.DataStructures
{
    [ComVisible(false)]
    public class SynchronizedObservableCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>,
            IList, ICollection, IEnumerable, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private ObservableCollection<T> items;
        private Object sync;

        #region Constructors
        public SynchronizedObservableCollection(Object syncRoot, IEnumerable<T> list)
        {
            this.sync = (syncRoot == null) ? new Object() : syncRoot;
            this.items = (list == null) ? new ObservableCollection<T>() :
                new ObservableCollection<T>(new List<T>(list));

            items.CollectionChanged += delegate (Object sender, NotifyCollectionChangedEventArgs e)
            {
                OnCollectionChanged(e);
            };
            INotifyPropertyChanged propertyChangedInterface = items as INotifyPropertyChanged;
            propertyChangedInterface.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e);
            };
        }

        public SynchronizedObservableCollection(object syncRoot) : this(syncRoot, null) { }

        public SynchronizedObservableCollection() : this(null, null) { }
        #endregion

        #region Methods
        public void Add(T item)
        {
            lock (this.sync)
            {
                int index = this.items.Count;
                this.InsertItem(index, item);
            }
        }

        public void Clear()
        {
            lock (this.sync)
            {
                this.ClearItems();
            }
        }

        protected virtual void ClearItems()
        {
            this.items.Clear();
        }

        public bool Contains(T item)
        {
            lock (this.sync)
            {
                return this.items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int index)
        {
            lock (this.sync)
            {
                this.items.CopyTo(array, index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (this.sync)
            {
                return this.items.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (this.sync)
            {
                return this.InternalIndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (this.sync)
            {
                if ((index < 0) || (index > this.items.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index, "Value must be in range.");
                }
                this.InsertItem(index, item);
            }
        }

        protected virtual void InsertItem(int index, T item)
        {
            this.items.Insert(index, item);
        }

        private int InternalIndexOf(T item)
        {
            int count = this.items.Count;
            for (int i = 0; i < count; i++)
            {
                if (object.Equals(this.items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(T item)
        {
            lock (this.sync)
            {
                int index = this.InternalIndexOf(item);
                if (index < 0)
                {
                    return false;
                }
                this.RemoveItem(index);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.sync)
            {
                if ((index < 0) || (index >= this.items.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index,
                        "Value must be in range.");
                }
                this.RemoveItem(index);
            }
        }

        protected virtual void RemoveItem(int index)
        {
            this.items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            this.items[index] = item;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.sync)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    array.SetValue(items[i], index + i);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        int IList.Add(object value)
        {
            VerifyValueType(value);
            lock (this.sync)
            {
                this.Add((T)value);
                return (this.Count - 1);
            }
        }

        bool IList.Contains(object value)
        {
            VerifyValueType(value);
            return this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            VerifyValueType(value);
            return this.IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);
            this.Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            VerifyValueType(value);
            this.Remove((T)value);
        }

        private static void VerifyValueType(object value)
        {
            if (value == null)
            {
                if (typeof(T).IsValueType)
                {
                    throw new ArgumentException("Synchronized collection wrong type null.");
                }
            }
            else if (!(value is T))
            {
                throw new ArgumentException("Synchronized collection wrong type.");
            }
        }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                lock (this.sync)
                {
                    return this.items.Count;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (this.sync)
                {
                    return this.items[index];
                }
            }
            set
            {
                lock (this.sync)
                {
                    if ((index < 0) || (index >= this.items.Count))
                    {
                        throw new ArgumentOutOfRangeException("index", index,
                            "Value must be in range.");
                    }
                    this.SetItem(index, value);
                }
            }
        }

        protected ObservableCollection<T> Items
        {
            get
            {
                return this.items;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.sync;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this.sync;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null) CollectionChanged(this, e);
        }

        #endregion
    }
}
