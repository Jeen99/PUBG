using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public abstract class GameObject : IGameObject
	{
		//получение id - не должно переопределясться
		private object sinchGetId = new object();
		private object sinchWorkWithComponent = new object();
		private static ulong counterID = 0;

		public event GameObjectDeleted EventGameObjectDeleted;

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
		public IMessage State {
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
			messageQueue.Enqueue(msg);
        }

		/// <summary>
		/// Освобождает все ресурысы объекта
		/// </summary>
		public virtual void Dispose()
		{
			Destroyed = true;
			EventGameObjectDeleted?.Invoke(this);
			foreach (IComponent item in Components)
			{
				item.Dispose();
			}
			Components.Clear();
			messageQueue.Clear();
			Model.AddEvent(new GameObjectDestroy(ID));
			Model = null;

        }

        /// <summary>
        /// Возвращает true, если объект уничтожен
        /// </summary>
        public bool Destroyed { get; protected set; }
		
	}
}