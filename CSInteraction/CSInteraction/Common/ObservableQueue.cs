using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace CSInteraction.Common
{
	public class ObservableQueue<T> : List<T>, INotifyCollectionChanged
	{
		private object sinchAccess = new object();

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public void Enqueue(T newElement)
		{
			lock (sinchAccess)
			{
				Add(newElement);
			}
			Task.Run(() =>
			{
				CollectionChanged?.Invoke(this,
				new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newElement, Count));
			});

		}

		public T Dequeue()
		{
			lock (sinchAccess)
			{
				if (Count > 0)
				{
					T FirstElement = this[0];
					this.RemoveAt(0);
					return FirstElement;
					
				}
				else return default(T);
			}
		}

	}
}
