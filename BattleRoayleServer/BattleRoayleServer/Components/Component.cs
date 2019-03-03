using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public abstract class Component
    {
        /// <summary>
        /// ссылка на хранилище всех данных об игре
        /// </summary>
        protected IGameModel gameModel;
		public GameObject Parent { get; private set; }

		protected Component(IGameModel gameModel, GameObject parent)
		{
			this.gameModel = gameModel;
			Parent = parent;
		}

        public abstract ComponentState State { get; }
       
		
        /// <summary>
        /// Запускает алгоритм обработки сообщения
        /// </summary>
        /// <param name="msg"></param>
        public abstract void ProcessMsg(IComponentMsg msg);
        
    }
}