using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace BattleRoyalServer
{
	public class DictionaryComponent : Dictionary<Type, IComponent>, IEnumerable
	{
		// возможно за место Type использовать _Type

		public T GetComponent<T>()
		{
			try
			{
				return (T)this[typeof(T)];
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public IComponent GetComponent(Type type)
		{
			return this[type];
		}

		public IComponent GetComponent(string type)
		{
			// было бы неплохо возвращать объект с нужным типом
			//return Components[Type.GetType(type)];
			foreach (var item in this.Keys)
			{
				if (item.Name == type)
					return this[item];
			}
			throw new IndexOutOfRangeException($"Элемент с классом {type} не содержится в Dictionary");
		}

		public T[] GetComponents<T>()
		{
			List<T> list = new List<T>();

			try
			{
				foreach (var key in this.Keys)
				{
					var component = this[key];
					if (component is T)
					{
						list.Add((T)component);
					}
				}
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
			}

			return list.ToArray();  // возможно не подходит, нужно руками делать
		}

		public void Add(IComponent comp)
		{
			this[comp.GetType()] = comp;
		}

		public bool Remove(IComponent comp)
		{
			return this.Remove(comp.GetType());
		}

		public bool Remove<T>()
		{
			return this.Remove(typeof(T));
		}

		// из-за скрытия возможно некоректная работа при использов интерфейса
		public new IEnumerator GetEnumerator()
		{ 
				foreach (var item in this.Keys)
				{
					yield return this[item];
				}		
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
