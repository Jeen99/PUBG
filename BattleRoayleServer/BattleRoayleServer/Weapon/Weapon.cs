using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;
using Box2DX.Collision;
using Box2DX.Common;
using CommonLibrary.GameMessages;

namespace BattleRoayleServer
{
	public class Weapon : GameObject, IWeapon
	{
		static Weapon()
		{

		}

		public Weapon(IModelForComponents model, TypesGameObject typeGameObject, TypesBehaveObjects typeBehaveObject, TypesWeapon typeWeapon) 
			: base(model, typeGameObject, typeBehaveObject)
		{
			TypeWeapon = typeWeapon;
		}

		public TypesWeapon TypeWeapon { get; protected set; }

		public override IMessage State
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
					return new WeaponState(ID, Type, TypeWeapon, states);
				}
			}
		}
	}

	public struct WeaponSetups
	{
		public readonly int timeBetweenShot;
		public readonly int timeReload;
		public readonly int bulletsInMagazin;

		public WeaponSetups(int timeBetweenShot, int timeReload, int bulletsInMagazin)
		{
			this.timeBetweenShot = timeBetweenShot;
			this.timeReload = timeReload;
			this.bulletsInMagazin = bulletsInMagazin;
		}
	}
}
