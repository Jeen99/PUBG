using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public abstract class Component : IComponent
	{
        /// <summary>
        /// ссылка на хранилище всех данных об игре
        /// </summary>
		public GameObject Parent { get; protected set; }

		protected Component(GameObject parent)
		{
			Parent = parent;
		}

        public virtual IMessage State
		{
			get { return null; }
		}
       	
        /// <summary>
        /// Запускает алгоритм обработки сообщения
        /// </summary>
        /// <param name="msg"></param>
        public abstract void UpdateComponent(IMessage msg);

		public virtual void Dispose()
		{
			return;
		}

		public abstract void Setup();

	}
}