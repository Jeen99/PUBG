using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;


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
		/// <summary>
		/// Очередь для хранения сообщений для этого игрового объекта
		/// </summary>
		private ObservalableQueue<IComponentMsg> messageQueue;
		public ulong ID { get; private set; }
		public IList<Component> Components { get; protected set; }

		public GameObject()
		{
			//иницализация всех полей
			ID = GetID();
			messageQueue = new ObservalableQueue<IComponentMsg>();
			messageQueue.CollectionChanged += Process;
			//коллекцию компонентов каждый объект реализует сам
		}

		/// <summary>
		/// Вызывается на каждое новое добавленный новый элемент колекции
		/// </summary>
		protected virtual void Process(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (Destroyed) return;
			else
			{
				lock (sinchWorkWithComponent)
				{
					IComponentMsg msg = messageQueue.Dequeue();
					foreach (Component component in Components)
					{
						component.ProcessMsg(msg);
					}
				}
			}
		}

		public abstract TypesBehaveObjects TypesBehave{ get; }

		/// <summary>
		/// Создаем список состояний компонентов игрового объекта
		/// </summary>
		public GameObjectState State {
			get
			{
				var states = new List<ComponentState>();
				lock (sinchWorkWithComponent)
				{
					if (Destroyed) return null;
					else
					{
						foreach (var component in Components)
						{
							var state =  component.State;
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
		public void SendMessage(IComponentMsg msg)
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