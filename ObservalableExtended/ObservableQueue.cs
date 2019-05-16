using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ObservalableExtended
{
	public class ObservableQueue<T> : INotifyCollectionChanged
	{
		private readonly object sinchAccess = new object();
		private Queue<T> insideQueue;

		public ObservableQueue()
		{
			insideQueue = new Queue<T>();
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public  void Enqueue(T newElement)
		{
			lock (sinchAccess)
			{
				insideQueue.Enqueue(newElement);			
			}
			
			CollectionChanged?.Invoke(this,
			new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newElement, insideQueue.Count));
		}

		public T Dequeue()
		{
			lock (sinchAccess)
			{
				return insideQueue.Dequeue();
			}
		}

		public int Count
		{
			get
			{
				//блокировка сделана, чтобы данные были актуальны
				lock (sinchAccess)
				{
					return insideQueue.Count;
				}
			}
		}

		public void Clear()
		{
			insideQueue.Clear();
		}

	}
}
