using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Diagnostics;

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
		/// <summary>
		/// Очередь для хранения сообщений для этого игрового объекта
		/// </summary>
		private Queue<IMessage> messageQueue;

		public ulong ID { get; private set; }

		public DictionaryComponent Components { get; } = new DictionaryComponent();


		public GameObject(IModelForComponents model)
		{
			//иницализация всех полей
			ID = GetID();
			messageQueue = new Queue<IMessage>();
			Model = model;
			//коллекцию компонентов каждый объект реализует сам
		}

		/// <summary>
		/// Обновляет состоняие объекта исходя на основе сообщений пришедших данному объекту
		/// </summary>
		public virtual void Update(TimeQuantPassed quantPassed = null)
		{
			if (Destroyed) return;
			else
			{
				lock (sinchWorkWithComponent)
				{
					//добавляем сообщение о прохождении кванта в конец очереди
					if (quantPassed != null)
					{
						messageQueue.Enqueue(quantPassed);
					}
					//рассылваем сообщение всем объектам
					
					while (messageQueue.Count > 0)
					{
						IMessage msg = messageQueue.Dequeue();
						Components.UpdateComponents(msg);
					}

				}
			}
		}

		public abstract TypesBehaveObjects TypesBehave{ get; }

		/// <summary>
		/// Создаем список состояний компонентов игрового объекта
		/// </summary>
		public virtual IMessage State {
			get
			{	
				lock (sinchWorkWithComponent)
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
		}
    
		/// <summary>
		/// Тип игрового объекта
		/// </summary>
		public abstract TypesGameObject Type { get;}

		/// <summary>
		/// Добавляет сообщение в очередь обработки сообщений
		/// </summary>
		public void SendMessage(IMessage msg)
        {
			if (msg != null)
			{
				messageQueue.Enqueue(msg);
			}
			else
			{
				Log.AddNewRecord($"Объекту {ID} отправлено null сообщение");
			}
        }

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