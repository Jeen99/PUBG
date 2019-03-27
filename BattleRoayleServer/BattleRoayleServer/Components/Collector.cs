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
	public class Collector : Component
	{
		private Modifier[] modifiers;
		private Weapon[] weapons;

		public Weapon GetWeapon(TypesWeapon typeWeapon)
		{
			return weapons[(int)typeWeapon];
		}
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Collector(GameObject parent, SolidBody body) : base(parent)
		{
			modifiers = new Modifier[5];
			weapons = new Weapon[4];
			this.body = body;
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
						ModifiersState.Add(modifiers[i].State);
					}
				}

				//создаем массив состояний оружия
				List<IMessage> WeaponsState = new List<IMessage>();
				for (int i = 0; i < weapons.Length; i++)
				{
					if (weapons[i] != null)
					{
						WeaponsState.Add(weapons[i].State);
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
					var body = weapons[i].Components.GetComponent<SolidBody>();
					CountObjects++;
				}			
			}

			//создаем коробку с лутом
			var position = Parent.Components.GetComponent<SolidBody>().Body.GetPosition();
			LootBox lootBox = new LootBox(Parent.Model,this, new PointF(position.X, position.Y));
			Parent.Model.AddGameObject(lootBox);
		}

			
		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TryPickUp:
					Handler_TryPickUp();
					break;
			}
		}
		private void Handler_TryPickUp()
		{
			var listObjects = body.GetPickUpObjects();
			foreach (var item in listObjects)
			{
				switch (item.Parent.Type)
				{
					case TypesGameObject.Weapon:
						PickUpWeapon(item);
						break;
					case TypesGameObject.LootBox:
						PickUpLootBox(item);
						break;
				}
			}
		}

		private void PickUpLootBox(SolidBody lootBoxBody)
		{
			//получаем коллекцию
			Collector collector = (lootBoxBody.Parent.Components.GetComponent<Collector>());
			//добавляем предметы, которых у нас еще нет
			for (int i = 0; i < collector.modifiers.Length; i++)
			{
				if (this.modifiers[i] == null)
				{
					this.modifiers[i] = collector.modifiers[i];
					//должно быть еще сообщение о добавлении нового модификатора
				}
			}

			for (int i = 0; i < collector.weapons.Length; i++)
			{
				if (this.weapons[i] == null)
				{
					this.weapons[i] = collector.weapons[i];
					var msg = new AddWeapon(Parent.ID, this.weapons[i].TypeWeapon);
					Parent.SendMessage(msg);
					Parent.Model.HappenedEvents.Enqueue(msg);
				}
			}
			//удаляем объект
			lootBoxBody.Parent.Dispose();
		}

		private void PickUpWeapon(SolidBody weaponBody)
		{
			Weapon weapon = (Weapon)weaponBody.Parent;

			if (weapons[(int)weapon.TypeWeapon] == null)
			{
				weapons[(int)weapon.TypeWeapon] = (Weapon)weapon;
				weaponBody.BodyDelete();
				var msg = new AddWeapon(Parent.ID, weapon.TypeWeapon);
				Parent.SendMessage(msg);
				Parent.Model.HappenedEvents.Enqueue(msg);
			}
		}

		public void SetNewParent(LootBox lootBox)
		{
			Parent = lootBox;
		}
	}
}