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
		private static readonly int min_count_of_shoots = 0;
		private static readonly int max_count_of_shoots = 5;
		private static readonly int min_offset_relative_to_center = 5;
		private static readonly int max_offset_relative_to_center = 135;
		private static readonly int timeUpdateDircetion = 5;
		private static readonly int timeUpdateExamineOfEnvironment = 1;

		private int offset_X_To_Center = 
			GetRandom.Random.Next(min_offset_relative_to_center, max_offset_relative_to_center);
		private int offset_Y_To_Center =
			GetRandom.Random.Next(min_offset_relative_to_center, max_offset_relative_to_center);

		private SolidBody _solidBody;
		private BodyZone  _targetZone;
		private Direction _directionMove = new Direction();

		private TimeSpan _timerUpdateDircetion = new TimeSpan(0,0,0, timeUpdateDircetion);
		private TimeSpan _timerExamineOfEnvironment = new TimeSpan(0, 0, 0, timeUpdateExamineOfEnvironment);

		public AI(IGameObject parent) : base(parent)
		{
		}

		public override void Setup()
		{
			if (Parent == null)
			{
				Log.AddNewRecord("Не обнаружен родитель для комронента бот");
				return;
			}

			_solidBody = Parent.Components.GetComponent<SolidBody>();
			_targetZone = Parent.Model.DeathZone.Components.GetComponent<BodyZone>(); // убрать зависимость от DeathZone и BodyZone!!

			if (_solidBody == null)
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
			_timerUpdateDircetion = _timerUpdateDircetion.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
			_timerExamineOfEnvironment = _timerExamineOfEnvironment.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));

			if (_timerUpdateDircetion.TotalMilliseconds <= 0.0f)
			{
				_timerUpdateDircetion = new TimeSpan(0, 0, 0, timeUpdateDircetion);
				offset_X_To_Center = GetRandom.Random.Next(min_offset_relative_to_center, max_offset_relative_to_center);
				offset_Y_To_Center = GetRandom.Random.Next(min_offset_relative_to_center, max_offset_relative_to_center);
			}

			if (_timerExamineOfEnvironment.TotalMilliseconds <= 0.0f)
			{
				_timerExamineOfEnvironment = new TimeSpan(0, 0, 0, timeUpdateExamineOfEnvironment);
				ExamineOfEnvironment();
			}
			Move();
		}

		private void Shoot(Vec2 pos)
		{
			//рандомная стрельба
			Random rand = GetRandom.Random;
			int count = rand.Next(min_count_of_shoots, max_count_of_shoots);

			for (int i = 0; i < count; i++)
			{
				Parent.Update(new MakeShot(Parent.ID, new PointF(pos.X, pos.Y)));
			}
		}

		private void Move()
		{
			// движение бота в центр смертельной зоны
			PointF currentPos = _solidBody.Shape.Location;
			PointF trigerPos = _targetZone.Location;

			Vector delta = new Vector(trigerPos.X - currentPos.X, trigerPos.Y - currentPos.Y);

			// движение по оси X
			if (delta.X >= 0 && offset_X_To_Center - delta.X <= 0)
			{
				_directionMove.Horisontal = DirectionHorisontal.Right;
			}
			else if (delta.X <= 0 && offset_X_To_Center - delta.X >= 0)
			{
				_directionMove.Horisontal = DirectionHorisontal.Left;
			}
			else
			{
				_directionMove.Horisontal = DirectionHorisontal.None;
			}
			// движение по оси Y
			if (delta.Y >= 0 && offset_Y_To_Center - delta.Y <= 0)
			{
				_directionMove.Vertical = DirectionVertical.Up;
			}
			else if (delta.Y <= 0 && offset_Y_To_Center - delta.Y >= 0)
			{
				_directionMove.Vertical = DirectionVertical.Down;
			}
			else
			{
				_directionMove.Vertical = DirectionVertical.None;
			}

			Parent.Update(new GoTo(Parent.ID, _directionMove));
		}

		private void ExamineOfEnvironment()		// изучение окружения
		{
			var position = (Vec2)_solidBody.Body?.GetPosition();

			Segment segment = new Segment
			{
				P1 = position          // исходное место обзора
			};
			//segment.P2 = new Vec2(); // конечное

			SolidBody[] solidBodies = new SolidBody[_solidBody.CoveredObjects.Count];	// копируем объекты, дабы не вызвать ошибки 
			_solidBody.CoveredObjects.CopyTo(solidBodies);								// при многопоточной работе

			foreach (var bodyEnemy in solidBodies)
			{
				if (bodyEnemy.Parent.Type == TypesGameObject.Weapon)
					Parent.Update(new TryPickUp(Parent.ID));

				if (bodyEnemy.Parent.Type == TypesGameObject.Player && bodyEnemy.Parent != Parent)
				{
					Parent.Update(new GoTo(Parent.ID, new Direction()));	// останавливаемя перед выстрелом
					segment.P2 = new Vec2(bodyEnemy.Shape.Location.X, bodyEnemy.Shape.Y);   // конечное место обзора
					var angle = VectorMethod.DefineAngle(segment.P1, segment.P2);
					Parent.Update(new PlayerTurn(Parent.ID, angle));		// поворачиваем оружеи в сторону цели
					Shoot(segment.P2);
				}
			}
		}
	}
}
