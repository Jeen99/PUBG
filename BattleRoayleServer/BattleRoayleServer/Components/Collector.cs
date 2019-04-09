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

		private static readonly ulong  CountOfModifier = 5u;

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
		private ISolidBody body;

		public Collector(IGameObject parent) : base(parent)
		{
			modifiers = new Modifier[CountOfModifier];
			weapons = new Weapon[sizeof(TypesWeapon)];
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
			int CountObjects = 0;
			//освобожаем все объекты от держателей
			for (int i = 0; i < modifiers.Length; i++)
			{
				if (modifiers[i] != null)
				{
					modifiers[i].Holder = null;
					CountObjects++;
				}
			}

			for (int i = 0; i < weapons.Length; i++)
			{
				if (weapons[i] != null)
				{
					weapons[i].Holder = null;
					var body = weapons[i]?.Components?.GetComponent<SolidBody>();
					CountObjects++;
				}			
			}

			//создаем коробку с лутом
			Vec2 position = (Vec2)Parent?.Components?.GetComponent<SolidBody>().Body.GetPosition();
			LootBox lootBox = new LootBox(Parent.Model,this, new PointF(position.X, position.Y));
			Parent?.Model?.AddGameObject(lootBox);
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
			}
		}
		private void Handler_TryPickUp()
		{
			var listObjects = Parent.Model.GetPickUpObjects(body.Shape);
			foreach (var item in listObjects)
			{
				switch (item.Type)
				{
					case TypesGameObject.Weapon:
						PickUpWeapon(item as Weapon);
						break;
					case TypesGameObject.LootBox:
						PickUpLootBox(item);
						break;
				}
			}
		}

		private void PickUpLootBox(IGameObject lootBox)
		{
				//получаем коллекцию
				Collector collector = lootBox.Components?.GetComponent<Collector>();
			
				//добавляем предметы, которых у нас еще нет
				for (int i = 0; i < collector.modifiers.Length && i < modifiers.Length; i++)
				{
				if (this.modifiers[i] == null && collector.modifiers[i] != null)
					{
						this.modifiers[i] = collector.modifiers[i];
						//должно быть еще сообщение о добавлении нового модификатора
					}
				}

				for (int i = 0; i < collector.weapons.Length && i < weapons.Length; i++)
				{
					if (this.weapons[i] == null && collector.weapons[i] != null)
					{
						this.weapons[i] = collector.weapons[i];
						var msg = new AddWeapon(Parent.ID, this.weapons[i].TypeWeapon);
						Parent.SendMessage(msg);
						Parent?.Model.AddEvent(msg);
					}
				}
				//удаляем объект
				lootBox.Dispose();
		}

		private void PickUpWeapon(IWeapon weapon)
		{

			if (weapons[(int)weapon.TypeWeapon] == null)
			{
				weapons[(int)weapon.TypeWeapon] = (Weapon)weapon;
				weapon.Components.GetComponent<TransparentBody>().Dispose();
				var msg = new AddWeapon(Parent.ID, weapon.TypeWeapon);
				Parent.SendMessage(msg);
				Parent?.Model?.AddEvent(msg);
			}
		}

		public void SetNewParent(LootBox lootBox)
		{
			Parent = lootBox;
		}

		public override void Setup()
		{
			//временная проверка
			if (Parent is LootBox) return;

			this.body = Parent.Components?.GetComponent<SolidBody>();
			if (body == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Сollector", "Не получена сслыка на компонент SolidBody");
				throw new Exception("Ошибка создания компонента Сollector");
			}
		}
	}
}