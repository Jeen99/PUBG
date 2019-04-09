using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Drawing;

namespace BattleRoayleServer
{
	public class Throw : Component
	{
		private IMagazin magazin;
		private float strength;

		public Throw(IWeapon parent, float strength) : base(parent)
		{
			this.strength = strength;
		}

		public override void Setup()
		{
			this.magazin = Parent.Components?.GetComponent<Magazin>();
			if (magazin == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Throw", "Не получена сслыка на компонент Magazin");
				throw new Exception("Ошибка создания компонента Throw");
			}
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.ThrowGrenade:
					Handler_ThrowGrenade(msg as ThrowGrenade);
					break;
			}
		}

		private void Handler_ThrowGrenade(ThrowGrenade msg)
		{
			ISolidBody BodyHolder = (Parent as Weapon).Holder?.Components?.GetComponent<SolidBody>();
			if (BodyHolder == null)
				return;
			//получаем гранату
			IBullet grenadeBullet = magazin.GetBullet();
			if (grenadeBullet == null)
				return;

			//получаем позицию игрока
			Vec2 position = (Vec2)BodyHolder.Body?.GetPosition();
			//определяем импульс
			float dX = msg.PointOfThrow.X - position.X;
			float dY = msg.PointOfThrow.Y - position.Y;
			//нельзя бросить дальше дальности броска
			if (System.Math.Sqrt(dX * dX + dY * dY) > strength) return;
			Vec2 impulse = new Vec2(dX, dY);

			//создаем объект гранаты
			Grenade grenade = new Grenade(Parent.Model, 
				(Parent as Weapon).Holder.Components.GetComponent<SolidBody>().Shape.Location, impulse, grenadeBullet);
			Parent.Model.AddGameObject(grenade);
		}
	}
}
