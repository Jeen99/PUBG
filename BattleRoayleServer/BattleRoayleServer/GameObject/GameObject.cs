using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CommonLibrary;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using System.Drawing;

namespace BattleRoayleServer
{
	public class GameObject : IGameObject
	{
		private readonly object sinchUpdateObject = new object();
		//получение id - не должно переопределясться
		protected readonly object sinchGetId = new object();
		
		//0 - зарезервированно за картой
		private static ulong counterID = 1;

		#region Cобытия для уведомления клиентов о получении нового сообытия
			public event ReceivedMessage Received_ChoiceWeapon;
			public event ReceivedMessage Received_GamerDied;
			public event ReceivedMessage Received_GotDamage;
			public event ReceivedMessage Received_GoTo;
			public event ReceivedMessage Received_MakeShot;
			public event ReceivedMessage Received_PlayerTurn;
			public event ReceivedMessage Received_MakeReloadWeapon;
			public event ReceivedMessage Received_TryPickUp;
			public event ReceivedMessage Received_DeletedInMap;
			public event ReceivedMessage Received_TimeQuantPassed;
			public event ReceivedMessage Received_AddWeapon;
			public event ReceivedMessage Received_MakedKill;
		#endregion

		protected virtual void SendMessage(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.ChoiceWeapon:
					Received_ChoiceWeapon?.Invoke(msg);
					break;
				case TypesMessage.GamerDied:
					Received_GamerDied?.Invoke(msg);
					break;
				case TypesMessage.GotDamage:
					Received_GotDamage?.Invoke(msg);
					break;
				case TypesMessage.GoTo:
					Received_GoTo?.Invoke(msg);
					break;
				case TypesMessage.MakeShot:
					Received_MakeShot?.Invoke(msg);
					break;
				case TypesMessage.PlayerTurn:
					Received_PlayerTurn?.Invoke(msg);
					break;
				case TypesMessage.MakeReloadWeapon:
					Received_MakeReloadWeapon?.Invoke(msg);
					break;
				case TypesMessage.TryPickUp:
					Received_TryPickUp?.Invoke(msg);
					break;
				case TypesMessage.DeletedInMap:
					Received_DeletedInMap?.Invoke(msg);
					break;
				case TypesMessage.TimeQuantPassed:
					Received_TimeQuantPassed?.Invoke(msg);
					break;
				case TypesMessage.AddWeapon:
					Received_AddWeapon?.Invoke(msg);
					break;
				case TypesMessage.MakedKill:
					Received_MakedKill?.Invoke(msg);
					break;
			}
		}

		public virtual void Setup()
		{
			Received_ChoiceWeapon = null;
			Received_GamerDied = null;
			Received_GotDamage = null;
			Received_GoTo = null;
			Received_MakeShot = null;
			Received_PlayerTurn = null;
			Received_MakeReloadWeapon = null;
			Received_TryPickUp = null;
			Received_DeletedInMap = null;
			Received_TimeQuantPassed = null;
			Received_AddWeapon = null;
			Received_MakedKill = null;

			foreach (IComponent item in Components)
			{
				item.Setup();
			}
		}

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

		public IGameObject Parent { get; set; } 

		public GameObject(IModelForComponents model, TypesGameObject typeGameObject, TypesBehaveObjects typeBehaveObject)
		{
			//иницализация всех полей
			ID = GetID();
			Model = model;
			Type = typeGameObject;
			TypeBehave = typeBehaveObject;
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

				lock (sinchUpdateObject)
				{
					SendMessage(msg);
				}
			}
		}

		public TypesBehaveObjects TypeBehave{ get; private set; }

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
		public TypesGameObject Type { get; private set; }

		/// <summary>
		/// Освобождает все ресурысы объекта
		/// </summary>
		public virtual void Dispose()
		{ 
			foreach (IComponent item in Components)
			{
				item.Dispose();
			}

			Model.AddOutgoingMessage(new DeletedInMap(ID));	
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

	public struct PhysicsSetups
	{
		public readonly float restetution;
		public readonly float friction;
		public readonly float density;
		public readonly float linearDamping;

		public PhysicsSetups(float restetution, float friction, float density, float linearDamping)
		{
			this.restetution = restetution;
			this.friction = friction;
			this.density = density;
			this.linearDamping = linearDamping;
		}
	}
}