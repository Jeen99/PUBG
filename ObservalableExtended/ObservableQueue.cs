using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservalableExtended
{
	public class ObservableQueue<T> : INotifyCollectionChanged
	{
		private readonly object _sinchAccess = new object();
		private Queue<T> _insideQueue;

		public ObservableQueue()
		{
			_insideQueue = new Queue<T>();
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public  void Enqueue(T newElement)
		{
			lock (_sinchAccess)
			{
				_insideQueue.Enqueue(newElement);
			}
			
			CollectionChanged?.Invoke(this,
			new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newElement, _insideQueue.Count));
		}

		public T Dequeue()
		{
			lock (_sinchAccess)
			{
				return _insideQueue.Dequeue();
			}
		}

		public int Count => _insideQueue.Count;

		public void Clear()
		{
			lock (_sinchAccess)
			{
				_insideQueue.Clear();
			}
		}

	}
}
