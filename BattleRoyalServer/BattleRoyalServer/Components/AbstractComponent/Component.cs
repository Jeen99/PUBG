using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;

namespace BattleRoyalServer
{
	public abstract class Component : IComponent
	{
        /// <summary>
        /// ссылка на хранилище всех данных об игре
        /// </summary>
		public IGameObject Parent { get; protected set; }

		protected Component(IGameObject parent)
		{
			Parent = parent;
			if (Parent == null)
			{
				Log.AddNewRecord("В конструктор компонента не была передана ссылка на родителя");
				throw new Exception("В конструктор компонента не была передана ссылка на родителя");
			}
		}

        public virtual IMessage State
		{
			get { return null; }
		}
       	
		public virtual void Dispose()
		{
			return;
		}

		/// <summary>
		/// Метод для настройки связей между компонентами
		/// </summary>
		public abstract void Setup();

	}
}