using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleRoayleServer
{
	public delegate void ForHappenedObjectEvent(IGameObjectEvent msg);
    public abstract class GameObject
    {
		//получение id - не должно переопределясться
		private static Mutex sinchGetId;
		private static ulong counterID = 0;
		private ulong GetID()
		{
			sinchGetId.WaitOne();
			ulong retID = counterID;
			counterID++;
			sinchGetId.ReleaseMutex();
			return retID;
		}
		/// <summary>
		/// Очередь для хранения сообщений для этого игрового объекта
		/// </summary>
		private System.Collections.Generic.Queue<IComponentMsg> messageQueue;

		public event ForHappenedObjectEvent HappenedObjectEvent;

		protected GameObject()
		{
			//иницализация всех полей
			ID = GetID();
		}

		public ulong ID{ get; private set;}

        /// <summary>
        /// Создаем список состояний компонентов игрового объекта
        /// </summary>
        public GameObjectState State { get; }

        public IList<Component> Components { get; private set; }

		/// <summary>
		/// Тип игрового объекта
		/// </summary>
		public TypesGameObject Type
		{
			get => default(int);
			set
			{
			}
		}

		/// <summary>
		/// Добавляет сообщение в очередь обработки сообщений
		/// </summary>
		public void SendMessage(IComponentMsg msg)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Освобождает все ресурысы объекта
        /// </summary>
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Возвращает true, если объект уничтожен
        /// </summary>
        public bool Destroyed()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Запускает обрабтку накопившихся сообщений по истечении некоторого времени
        /// </summary>
        public int Process()
        {
            throw new System.NotImplementedException();
        }

		private void HandlerComponentEvent()
		{
			throw new System.NotImplementedException();
		}
	}
}