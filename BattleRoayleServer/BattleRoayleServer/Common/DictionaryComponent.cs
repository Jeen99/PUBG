using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class DictionaryComponent : IEnumerable
	{
		private Dictionary<Type, IComponent> Components;    // возможно за место Type использовать _Type

		public DictionaryComponent()
		{
			Components = new Dictionary<Type, IComponent>();
		}

		public T GetComponent<T>()
		{
			try
			{
				return (T)Components[typeof(T)];
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public IComponent GetComponent(Type type)
		{
			return Components[type];
		}

		public IComponent GetComponent(string type)
		{
			// было бы неплохо возвращать объект с нужным типом
			//return Components[Type.GetType(type)];
			foreach (var item in this.Components.Keys)
			{
				if (item.ToString() == type)
					return Components[item];
			}
			throw new IndexOutOfRangeException($"Элемент с классом {type} не содержится в Dictionary");
		}

		public T[] GetComponents<T>()
		{
			List<T> list = new List<T>();

			try
			{
				foreach (var key in Components.Keys)
				{
					var component = Components[key];
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
			Components[comp.GetType()] = comp;
		}

		public bool Remove(IComponent comp)
		{
			return Components.Remove(comp.GetType());
		}

		public bool Remove<T>()
		{
			return Components.Remove(typeof(T));
		}

		public void Clear()
		{
			Components.Clear();
		}

		public void UpdateComponents(IMessage msg)
		{
			try
			{
				foreach (var item in Components.Keys)
				{
					Components[item].UpdateComponent(msg);
				}
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
			}
		}

		public IEnumerator GetEnumerator()
		{ 
				foreach (var item in Components.Keys)
				{
					yield return Components[item];
				}		
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
