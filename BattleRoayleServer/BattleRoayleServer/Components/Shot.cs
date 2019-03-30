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
	public class Shot : Component, IShot
	{
		private IMagazin magazin;

		public Shot(IWeapon parent) : base(parent)
		{
			
			this.magazin = parent?.Components?.GetComponent<Magazin>();
			if (magazin == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Shot", "Не получена сслыка на компонент Magazin");
				throw new Exception("Ошибка создания компонента Shot");
			}
			
		}

		public override void Dispose()
		{
			magazin = null;
		}

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Shot");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.MakeShot:
					Handler_MakeShot(msg as MakeShot);
					break;
			}
			
		}

		private void Handler_MakeShot(MakeShot msg)
		{
			try
			{
				ISolidBody BodyHolder = (Parent as IWeapon).Holder?.Components?.GetComponent<SolidBody>();
				if (BodyHolder != null)
				{
					//получаем патрон
					IBullet bullet = magazin.GetBullet();

					if (bullet != null)
					{
								
						Vec2 position = (Vec2)BodyHolder.Body?.GetPosition();
						if (position == null)
						{
							Log.AddNewRecord("Ошибка получения позиции игрока");
							return;
						}
					
						var segment = new Segment();
						//начальная точка выстрела
						segment.P1 = position;

						var sweepVector = VectorMethod.RotateVector(msg.Angle, bullet.Distance);
						//конечная точка выстрела
						segment.P2 = new Vec2
						{
							X = position.X + sweepVector.X,
							Y = position.Y + sweepVector.Y
						};
						//получаем только первый встетившийся на пути пули объект
						Shape[] hits = new Shape[2];

						BodyHolder?.Body?.GetWorld().Raycast(segment, hits, 2, true, null);
								
						//отправляем сообщение о совершении выстрела
						Parent.Model.AddEvent(new MakedShot((Parent as IWeapon).Holder.ID));

						var damageMsg = new GotDamage(bullet.Damage);
						//отправляем ему сообщение о нанесении урона	
						ISolidBody attacked = (ISolidBody)hits[1]?.GetBody().GetUserData();

						attacked .Parent?.SendMessage(damageMsg);
					}
				}
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
			}
		}
	}
}