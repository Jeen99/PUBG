using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public delegate void ForHappenedComponentMsg(IComponentEvent msg);
	public abstract class Component
    {
        /// <summary>
        /// ссылка на хранилище всех данных об игре
        /// </summary>
        private IGameModel gameModel;

		protected Component(IGameModel gameModel, GameObject parent)
		{
			this.gameModel = gameModel;
			Parent = parent;
		}

		public event ForHappenedComponentMsg HappenedComponentMsg;

		public GameObject Parent { get; private set; } 
       

        public ComponentState State { get; }
       
		
        /// <summary>
        /// Запускает алгоритм обработки сообщения
        /// </summary>
        /// <param name="msg"></param>
        public void ProcessMsg(IComponentMsg msg)
        {
            throw new System.NotImplementedException();
        }
    }
}