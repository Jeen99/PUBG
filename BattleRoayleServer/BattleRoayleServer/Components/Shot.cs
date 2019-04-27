using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Diagnostics;

namespace BattleRoayleServer
{
	//отвечает за выстрел
	public class Shot : Component
	{
		private Magazin magazin;

		public Shot(IWeapon parent) : base(parent)
		{
					
		}

		public override void Dispose()
		{
			magazin = null;
		}

		public override void Setup()
		{
			this.magazin = Parent.Components?.GetComponent<Magazin>();
			if (magazin == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Shot", "Не получена сслыка на компонент Magazin");
				throw new Exception("Ошибка создания компонента Shot");
			}
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
				case TypesMessage.MakeShot:
					Handler_MakeShot(msg);
					break;
			}		
		}

		private void Handler_MakeShot(IMessage msg)
		{
			try
			{
				SolidBody BodyHolder = (Parent as Weapon).Holder?.Components?.GetComponent<SolidBody>();
				if (BodyHolder == null)
				{
					Log.AddNewRecord("Ошибка получения держателя оружия");
					return;
				}

				//получаем патрон
				IBullet bullet = magazin.GetBullet();
				if (bullet == null) return;
								
				Vec2 position = (Vec2)BodyHolder.Body?.GetPosition();
				if (position == null)
				{
					Log.AddNewRecord("Ошибка получения позиции игрока");
					return;
				}
					
				var segment = new Segment();
				//начальная точка выстрела
				segment.P1 = position;
				//определяем угол
				float angle = VectorMethod.DefineAngle(position, new Vec2(msg.Location.X, msg.Location.Y));

				var sweepVector = VectorMethod.RotateVector(angle, bullet.Distance);
				//конечная точка выстрела
				segment.P2 = new Vec2
				{
					X = position.X + sweepVector.X,
					Y = position.Y + sweepVector.Y
				};
				//получаем только первый встетившийся на пути пули объект
				Shape[] objectsForDamage = new Shape[2];

				BodyHolder?.Body?.GetWorld().Raycast(segment, objectsForDamage, objectsForDamage.Length, true, null);

				//отправляем сообщение о совершении выстрела
				if (objectsForDamage[1] == null)
					Parent.Model.AddOutgoingMessage(new MakedShot((Parent as Weapon).Holder.ID, angle, bullet.Distance));
				else
				{
					float newDistance = VectorMethod.DefineDistance(segment.P1, objectsForDamage[1].GetBody().GetPosition());
					Parent.Model.AddOutgoingMessage(new MakedShot((Parent as Weapon).Holder.ID, angle, newDistance));
				}

				//отправляем ему сообщение о нанесении урона	
				SolidBody attacked = (SolidBody)objectsForDamage[1]?.GetBody().GetUserData();
				if (attacked == null) return;

				var damageMsg = new GotDamage(BodyHolder.Parent.ID, bullet.Damage);
				attacked.Parent.Update(damageMsg);

				//определяем убили ли мы противника
				Healthy healthyAttacked = attacked.Parent.Components.GetComponent<Healthy>();
				if (healthyAttacked == null) return;
				//если убили засчитываем фраг
				if (healthyAttacked.HP < bullet.Damage) BodyHolder.Parent?.Update(new MakedKill(BodyHolder.Parent.ID));
				
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
			}
		}
	}
}