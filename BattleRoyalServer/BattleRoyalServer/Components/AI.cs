using BattleRoyalServer.Common;
using Box2DX.Collision;
using Box2DX.Common;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using System;
using System.Drawing;
using System.Windows;

namespace BattleRoyalServer
{
	public class AI : Component
	{
		private static readonly int max_view_distance = 30;
		private static readonly int min_count_of_shoots = 0;
		private static readonly int max_count_of_shoots = 5;
		private static readonly int min_offset_relative_to_center = 5;
		private static readonly int max_offset_relative_to_center = 35;

		private readonly int offset_relative_to_center = 
			GetRandom.Random.Next(min_offset_relative_to_center, max_offset_relative_to_center);

		private SolidBody solidBody;
		private BodyZone triger;

		public AI(IGameObject parent) : base(parent)
		{
		}

		public override void Setup()
		{
			solidBody = Parent?.Components.GetComponent<SolidBody>();
			// убрать зависимость от DeathZone и BodyZone!!
			triger = Parent.Model.DeathZone.Components.GetComponent<BodyZone>();

			if (solidBody == null)
			{
				Log.AddNewRecord("Ошибка получения SolidBody Бота");
				return;
			}

			Parent.Received_TimeQuantPassed += Handler_AI;
		}

		public override void Dispose()
		{
			Parent.Received_TimeQuantPassed -= Handler_AI;
		}

		private void Handler_AI(IMessage msg)
		{
			SearchEnemy();
			Move();
		}

		private void Shoot(float angle)
		{
			//рандомная стрельба
			Random rand = GetRandom.Random;
			int count = rand.Next(min_count_of_shoots, max_count_of_shoots);

			for (int i = 0; i < count; i++)
			{
				Parent.Update(new MakeShot(Parent.ID, angle));
			}
		}

		private void Move()
		{
			// движение бота в центр смертельной зоны
			PointF currentPos = solidBody.Shape.Location;
			PointF trigerPos = triger.Location;

			Vector delta = new Vector(trigerPos.X - currentPos.X, trigerPos.Y - currentPos.Y);
			Direction direction = new Direction();

			// движение по оси X
			if (delta.X >= 0 && offset_relative_to_center - delta.X <= 0)
			{
				direction.Horisontal = DirectionHorisontal.Right;
			}
			else if (delta.X <= 0 && offset_relative_to_center - delta.X >= 0)
			{
				direction.Horisontal = DirectionHorisontal.Left;
			}
			else
			{
				direction.Horisontal = DirectionHorisontal.None;
			}
			// движение по оси Y
			if (delta.Y >= 0 && offset_relative_to_center - delta.Y <= 0)
			{
				direction.Vertical = DirectionVertical.Up;
			}
			else if (delta.Y <= 0 && offset_relative_to_center - delta.Y >= 0)
			{
				direction.Vertical = DirectionVertical.Down;
			}
			else
			{
				direction.Vertical = DirectionVertical.None;
			}

			Parent.Update(new GoTo(Parent.ID, direction));
		}

		private void SearchEnemy()
		{
			Vec2 position = (Vec2)solidBody.Body?.GetPosition();
			if (position == null)
			{
				Log.AddNewRecord("Ошибка получения позиции игрока");
				return;
			}

			Segment segment = new Segment();
			segment.P1 = position;          // исходное место обзора
			segment.P2 = new Vec2();        // конечное

			Shape[] objectsForDamage = new Shape[2];

			for (int angle = 0; angle < 360; angle++)
			{
				var sweepVector = VectorMethod.RotateVector(angle, max_view_distance);
				segment.P2.X = position.X + sweepVector.X;
				segment.P2.Y = position.Y + sweepVector.Y;

				solidBody?.Body?.GetWorld().Raycast(segment, objectsForDamage, 2, true, null);

				if (objectsForDamage[1] != null)    // получаем самый первый встретившийся объект
				{
					Parent.Update(new GoTo(Parent.ID, new Direction()));      // останавливаем перед выстрелом
					Shoot(angle);                                       // и стреляем
					break;
				}
			}
		}
	}
}
