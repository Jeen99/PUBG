using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.Common
{
	public class ObservalableQueue<T> : ObservableCollection<T>
	{
		private object sinchAccess = new object();

		public void Enqueue(T newElement)
		{
			lock (sinchAccess)
			{
				Add(newElement);
			}
		}

		public T Dequeue()
		{
			lock (sinchAccess)
			{
				if (Count > 0)
				{
					var FirstElement = this[0];
					RemoveAt(0);
					return FirstElement;
				}
				else return default(T);
			}
		}

	}
}
