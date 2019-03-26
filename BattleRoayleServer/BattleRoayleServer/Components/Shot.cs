using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
	//отвечает за выстрел
	public class Shot:Component
	{

		private Magazin magazin;

		public Shot(GameObject parent, Magazin magazin) : base(parent)
		{
			this.magazin = magazin;
		}

		public override void Dispose()
		{
			magazin = null;
			return;
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.MakeShot:
					Handler_MakeShot(msg as MakeShot);
					break;
			}
		}

		private void Handler_MakeShot(MakeShot msg)
		{
			SolidBody BodyHolder = (SolidBody)(Parent as Weapon).Holder.GetComponent(typeof(SolidBody));
			if (BodyHolder != null)
			{
				//получаем патрон
				IBullet bullet = magazin.GetBullet();
				if (bullet != null)
				{
					//отправляем сообщение о совершении выстрела
					Parent.Model.HappenedEvents.Enqueue(new MakedShot((Parent as Weapon).Holder.ID));
					//совершаем выстрел
					var segment = new Segment();
					Vec2 position = BodyHolder.Body.GetPosition();
					segment.P1 = position;
					var sweepVector = VectorMethod.RotateVector(msg.Angle, bullet.Distance);
					segment.P2 = new Vec2
					{
						X = position.X + sweepVector.X,
						Y = position.Y + sweepVector.Y
					};
					//получаем только первый встетившийся на пути пули объект
					Shape[] hits = new Shape[2];
					BodyHolder.Body.GetWorld().Raycast(segment, hits, 2, true, null);
					//отправляем ему сообщение о нанесении урона
					SolidBody attacked = (SolidBody)hits[1].GetBody().GetUserData();
					var damageMsg = new GotDamage(bullet.Damage);
					attacked.Parent.SendMessage(damageMsg);
				}
			}
		}
	}
}