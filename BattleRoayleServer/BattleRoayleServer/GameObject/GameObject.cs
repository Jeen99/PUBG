using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	public abstract class GameObject : IGameObject
	{
		//получение id - не должно переопределясться
		protected object sinchGetId = new object();
		protected object sinchWorkWithComponent = new object();
		//0 - зарезервированно за картой
		private static ulong counterID = 1;

		private ulong GetID()
		{
			lock (sinchGetId)
			{
				ulong retID = counterID;
				counterID++;
				return retID;
			}
		}

		public IModelForComponents Model { get; private set; }
	
		public ulong ID { get; private set; }

		public DictionaryComponent Components { get; } = new DictionaryComponent();


		public GameObject(IModelForComponents model)
		{
			//иницализация всех полей
			ID = GetID();
			Model = model;
			//коллекцию компонентов каждый объект реализует сам
		}

		/// <summary>
		/// Обновляет состоняие объекта исходя на основе сообщений пришедших данному объекту
		/// </summary>
		public virtual void Update(IMessage msg)
		{
			if (Destroyed) return;
			else
			{
				if (msg == null)
				{
					Log.AddNewRecord("Получено null сообщение");
					return;
				}

				lock (sinchWorkWithComponent)
				{	
					Components.UpdateComponents(msg);
				}
			}
		}

		public abstract TypesBehaveObjects TypesBehave{ get; }

		/// <summary>
		/// Создаем список состояний компонентов игрового объекта
		/// </summary>
		public virtual IMessage State
		{
			get
			{				
				var states = new List<IMessage>();
				if (Destroyed) return null;
				else
				{
					foreach (IComponent component in Components)
					{
						var state = component.State;
						if (state != null)
						{
							states.Add(state);
						}
					}
					return new GameObjectState(ID, Type, states);
				}
			}
		}
    
		/// <summary>
		/// Тип игрового объекта
		/// </summary>
		public abstract TypesGameObject Type { get;}

		/// <summary>
		/// Освобождает все ресурысы объекта
		/// </summary>
		public virtual void Dispose()
		{
			//EventGameObjectDeleted?.Invoke(this);
			foreach (IComponent item in Components)
			{
				item.Dispose();
			}

			Model.AddEvent(new DeleteInMap(ID));	
		}

		public virtual void Setup()
		{
			foreach (IComponent item in Components)
			{
				item.Setup();
			}
		}

		public virtual void SetDestroyed()
		{
			Destroyed = true;
		}

		/// <summary>
		/// Возвращает true, если объект уничтожен
		/// </summary>
		public bool Destroyed { get; protected set; }
		
	}
}