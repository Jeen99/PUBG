using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Drawing;


namespace BattleRoayleServer
{
	public class Collector : Component, ICollector
	{
		private Modifier[] modifiers;
		private Weapon[] weapons;

		private static readonly ulong  CountOfModifier = 5;
		private static readonly ulong CountOfWeapon = 4;

		public Weapon GetWeapon(TypesWeapon typeWeapon)
		{
			try
			{
				return weapons[(int)typeWeapon];
			}
			catch (Exception) { return null; }
		}

		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Collector(IGameObject parent) : base(parent)
		{
			modifiers = new Modifier[CountOfModifier];
			weapons = new Weapon[CountOfWeapon];
		}
		//только для тестов
		public Collector(IGameObject parent, Weapon[] weapons) : base(parent)
		{
			modifiers = new Modifier[CountOfModifier];
			this.weapons = weapons;
			this.body = new SolidBody(parent);
		}

		public override IMessage State
		{
			get
			{
				//создаем массив состояний модификаций
				List<IMessage> ModifiersState = new List<IMessage>();
				for (int i = 0; i < modifiers.Length; i++)
				{
					if (modifiers[i] != null)
					{
						ModifiersState.Add(modifiers[i]?.State);
					}
				}

				//создаем массив состояний оружия
				List<IMessage> WeaponsState = new List<IMessage>();
				for (int i = 0; i < weapons.Length; i++)
				{
					if (weapons[i] != null)
					{
						WeaponsState.Add(weapons[i]?.State);
					}
				}
				return new CollectorState(ModifiersState, WeaponsState);

			}
		}

		public override void Dispose()
		{
			//создаем для всех объектов тела с заданным вектором движения

			//освобожаем все объекты от держателей
			/*for (int i = 0; i < modifiers.Length; i++)
			{
				if (modifiers[i] != null)
				{
					modifiers[i].Holder = null;
					CountObjects++;
				}
			}*/

			//пока только для оружия
			PointF position = Parent.Components.GetComponent<SolidBody>().Shape.Location;

			for (int i = 0; i < weapons.Length; i++)
			{
				if (weapons[i] != null)
				{
					weapons[i].Holder = null;
					weapons[i].CreateNewBody(position, CreateRandVec2());		
				}			
			}		
		}

		private Vec2 CreateRandVec2()
		{
			Random rand = new Random();
			return new Vec2(rand.Next(1, 15), rand.Next(1, 15));
		}
			
		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Collector");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TryPickUp:
					Handler_TryPickUp();
					break;
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed((TimeQuantPassed)msg);
					break;
			}
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			//пока только для оружия

			for (int i = 0; i < weapons.Length; i++)
			{
				if (weapons[i] != null)
				{
					weapons[i].Update(msg);
				}
			}
		}

		private void Handler_TryPickUp()
		{
			for (int i = 0; i < body.CoveredObjects.Count; i++)
			{
				switch (body.CoveredObjects[i].Parent.Type)
				{
					case TypesGameObject.Weapon:
						PickUpWeapon(body.CoveredObjects[i].Parent as Weapon);
						//после добаления объекта в Collector он должен удлаиться из CoveredObjects
						i--;
						break;
				}
			}
		}

		private void PickUpWeapon(Weapon weapon)
		{

			if (weapons[(int)weapon.TypeWeapon] == null)
			{
				weapons[(int)weapon.TypeWeapon] = weapon;
				//удаляем компонент, отвечающий за  тело оружия
				weapon.Components.GetComponent<SolidBody>().Dispose();
				weapon.Components.Remove<SolidBody>();

				var msg = new AddWeapon(Parent.ID, weapon.TypeWeapon);
				Parent.SendMessage(msg);
				Parent.Model.AddEvent(new DeleteInMap(weapon.ID));
				Parent.Model?.AddEvent(msg);
			}
		}

		
		public override void Setup()
		{

			this.body = Parent.Components?.GetComponent<SolidBody>();
			if (body == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Сollector", "Не получена сслыка на компонент SolidBody");
				throw new Exception("Ошибка создания компонента Сollector");
			}
		}
	}

	interface ICollectorItem
	{
		IGameObject Holder { get; set; }
		void CreateNewBody(PointF location, Vec2 startVelocity);
	}
}