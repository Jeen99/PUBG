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
using System.Drawing;

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
			Parent.Received_MakeShot -= Handler_MakeShot;
		}

		public override void Setup()
		{
			this.magazin = Parent.Components?.GetComponent<Magazin>();
			if (magazin == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Shot", "Не получена сслыка на компонент Magazin");
				throw new Exception("Ошибка создания компонента Shot");
			}
			Parent.Received_MakeShot += Handler_MakeShot;
		}

		private void Handler_MakeShot(IMessage msg)
		{
			try
			{
				SolidBody BodyHolder = (Parent as Weapon).Parent?.Components?.GetComponent<SolidBody>();
				if (BodyHolder == null)
				{
					Log.AddNewRecord("Ошибка получения держателя оружия");
					return;
				}
				Vec2 position = (Vec2)BodyHolder.Body.GetPosition();

				//получаем патрон
				IBullet bullet = magazin.GetBullet();
				if (bullet == null) return;

				//определяем угол
				float angle = VectorMethod.DefineAngle(position, new Vec2(msg.Location.X, msg.Location.Y));

				var rayOfShot = CreateRayForShot(position, angle, bullet.Distance);

				var attackedObject = DefineDamagedSolidBody(rayOfShot, (Parent as Weapon).Parent.ID);

				if (attackedObject == null)
				{
					NotAttacked(angle, bullet.Distance);
				}
				else
				{
					HaveAttacked(attackedObject, position, angle, bullet);
				}
			}
			catch (Exception e)
			{
				Log.AddNewRecord(e.ToString());
			}
		}

		private Segment CreateRayForShot(Vec2 positionGamer, float angle, float distance)
		{
			var segment = new Segment();
			//начальная точка выстрела
			segment.P1 = positionGamer;

			var sweepVector = VectorMethod.RotateVector(angle, distance);
			//конечная точка выстрела
			segment.P2 = new Vec2
			{
				X = positionGamer.X + sweepVector.X,
				Y = positionGamer.Y + sweepVector.Y
			};

			return segment;
		}

		private SolidBody DefineDamagedSolidBody(Segment rayShot, ulong IDParent)
		{
			//получаем только первый встетившийся на пути пули объект
			var listObjects = Parent.Model.GetMetedObjects(rayShot);
			foreach (var item in listObjects)
			{
				if (item.Parent.ID == IDParent)
				{
					continue;
				}
				else if (item.Parent.Type == TypesGameObject.Weapon) continue;
				else return item;

			}
			return null;
		}

		private void NotAttacked(float angle, float distance)
		{
			Parent.Model.AddOutgoingMessage(new MakedShot((Parent as Weapon).Parent.ID, angle, distance));
		}

		private void HaveAttacked(SolidBody attacked, Vec2 position, float angle, IBullet bullet)
		{
			PointF locationObject = attacked.Shape.Location;
			ulong idParent = (Parent as Weapon).Parent.ID;

			float newDistance = VectorMethod.DefineDistance(position, new Vec2(locationObject.X, locationObject.Y));
			Parent.Model.AddOutgoingMessage(new MakedShot(idParent, angle, newDistance));

			if (attacked == null) return;

			var damageMsg = new GotDamage(idParent, bullet.Damage);
			attacked.Parent.Update(damageMsg);

			//определяем убили ли мы противника
			Healthy healthyAttacked = attacked.Parent.Components.GetComponent<Healthy>();
			if (healthyAttacked == null) return;
			//если убили засчитываем фраг
			if (healthyAttacked.HP < bullet.Damage) (Parent as Weapon).Parent.Update(new MakedKill(idParent));
		}
	}
}