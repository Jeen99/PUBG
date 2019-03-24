using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public abstract class GameObject
	{
		//получение id - не должно переопределясться
		private object sinchGetId = new object();
		private object sinchWorkWithComponent = new object();
		private static ulong counterID = 0;
		private ulong GetID()
		{
			lock (sinchGetId)
			{
				ulong retID = counterID;
				counterID++;
				return retID;
			}
		}

		public IGameModel Model { get; private set; }
		/// <summary>
		/// Очередь для хранения сообщений для этого игрового объекта
		/// </summary>
		private Queue<IMessage> messageQueue;
		public ulong ID { get; private set; }
		public IList<Component> Components { get; protected set; }

		public GameObject(IGameModel model)
		{
			//иницализация всех полей
			ID = GetID();
			messageQueue = new Queue<IMessage>();
			Model = model;
			//коллекцию компонентов каждый объект реализует сам
		}

		/// <summary>
		/// Вызывается на каждое новое добавленный новый элемент колекции
		/// </summary>
		public virtual void Process(TimeQuantPassed quantPassed)
		{
			if (Destroyed) return;
			else
			{
				lock (sinchWorkWithComponent)
				{
					//добавляем сообщение о прохождении кванта в конец очереди
					messageQueue.Enqueue(quantPassed);
					//рассылваем сообщение всем объектам
					while (messageQueue.Count > 0)
					{
						IMessage msg = messageQueue.Dequeue();
						foreach (Component component in Components)
						{
							component.ProcessMsg(msg);
						}
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
						foreach (var component in Components)
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
		public abstract TypesGameObject Type { get;
		}

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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Возвращает true, если объект уничтожен
        /// </summary>
        public bool Destroyed { get; protected set; }
       
	}
}