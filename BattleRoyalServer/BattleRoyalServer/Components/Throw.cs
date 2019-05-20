using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.GameMessages;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Drawing;

namespace BattleRoyalServer
{
	public class Throw : Component
	{
		private Magazin magazin;
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
			Parent.Received_MakeShot += Handler_MakeShot;
		}

		private void Handler_MakeShot(IMessage msg)
		{
			SolidBody BodyHolder = (Parent as Weapon).Owner?.Components?.GetComponent<SolidBody>();
			if (BodyHolder == null)
				return;
			//получаем гранату
			IBullet grenadeBullet = magazin.GetBullet();
			if (grenadeBullet == null)
				return;

			//получаем позицию игрока
			Vec2 position = (Vec2)BodyHolder.Body?.GetPosition();
			//определяем импульс
			float dX = msg.Location.X - position.X;
			float dY = -(msg.Location.Y - position.Y);
			//нельзя бросить дальше дальности броска
			if (System.Math.Sqrt(dX * dX + dY * dY) > strength) return;
			Vec2 impulse = new Vec2(dX, dY);

			//создаем объект гранаты
			var grenade = BuilderGameObject.CreateGrenade(Parent.Model, 
				(Parent as Weapon).Owner.Components.GetComponent<SolidBody>().Shape.Location, impulse, grenadeBullet);
		}

		public override void Dispose()
		{
			Parent.Received_MakeShot -= Handler_MakeShot;
		}
	}
}
