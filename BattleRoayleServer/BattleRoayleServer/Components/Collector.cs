using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class Collector : Component
	{
		private Modifier[] modifiers;
		private Weapon[] weapons;
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
		//необходимо реализовать state
		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void ProcessMsg(IComponentMsg msg)
		{
			switch (msg.Type)
			{
				case TypesComponentMsg.PickUpLoot:
					break;
			}
		}
		private void Handler_PickUpLoot()
		{
			var listObjects = body.GetPickUpObjects();
			foreach (var item in listObjects)
			{
				switch (item.Parent.Type)
				{
					case TypesGameObject.Weapon:
						PickUpWeapon(item);
						break;
				}
			}
		}

		private void PickUpWeapon(SolidBody weaponBody)
		{
			Weapon weapon = (Weapon)body.Parent;
			switch (weapon.TypeWeapon)
			{
				case TypesWeapon.Gun:
					TrySaveWeapon(0, weapon, weaponBody);
					break;
				case TypesWeapon.ShotGun:
					TrySaveWeapon(1, weapon, weaponBody);
					break;
				case TypesWeapon.AssaultRifle:
					TrySaveWeapon(2, weapon, weaponBody);
					break;
				case TypesWeapon.Grenade:
					TrySaveWeapon(3, weapon, weaponBody);
					break;
			}
		}

		private void TrySaveWeapon(int index, Weapon weapon, SolidBody weaponBody)
		{
			if (weapons[index] == null)
			{
				weapons[index] = (Weapon)weapon;
				weaponBody.BodyDelete();
				Parent.Model.HappenedEvents.Enqueue(new AddWeapon(Parent.ID, weapon.TypeWeapon));
			}
		}

	}
}