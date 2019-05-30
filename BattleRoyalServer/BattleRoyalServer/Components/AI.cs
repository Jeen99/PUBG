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
		private static readonly int MinCountOfShoots = 0;
		private static readonly int MaxCountOfShoots = 5;
		private static readonly int MinOffsetRelativeToCenter = 5;
		private static readonly int MaxOffsetRelativeToCenter = 135;
		private static readonly int TimeUpdateDirection = 5;
		private static readonly int TimeUpdateExamineOfEnvironment = 1;

		private int _offsetXtoCenter = 
			GetRandom.Random.Next(MinOffsetRelativeToCenter, MaxOffsetRelativeToCenter);
		private int _offsetYtoCenter =
			GetRandom.Random.Next(MinOffsetRelativeToCenter, MaxOffsetRelativeToCenter);

		private SolidBody _solidBody;
		private BodyZone  _targetZone;
		private readonly Direction _directionMove = new Direction();

		private TimeSpan _timerUpdateDirection = new TimeSpan(0,0,0, TimeUpdateDirection);
		private TimeSpan _timerExamineOfEnvironment = new TimeSpan(0, 0, 0, TimeUpdateExamineOfEnvironment);

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
			_timerUpdateDirection = _timerUpdateDirection.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
			_timerExamineOfEnvironment = _timerExamineOfEnvironment.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));

			if (_timerUpdateDirection.TotalMilliseconds <= 0.0f)
			{
				_timerUpdateDirection = new TimeSpan(0, 0, 0, TimeUpdateDirection);
				_offsetXtoCenter = GetRandom.Random.Next(MinOffsetRelativeToCenter, MaxOffsetRelativeToCenter);
				_offsetYtoCenter = GetRandom.Random.Next(MinOffsetRelativeToCenter, MaxOffsetRelativeToCenter);
			}

			if (_timerExamineOfEnvironment.TotalMilliseconds <= 0.0f)
			{
				_timerExamineOfEnvironment = new TimeSpan(0, 0, 0, TimeUpdateExamineOfEnvironment);
				ExamineOfEnvironment();
			}
			Move();
		}

		private void Shoot(Vec2 pos)
		{
			//рандомная стрельба
			Random rand = GetRandom.Random;
			int count = rand.Next(MinCountOfShoots, MaxCountOfShoots);

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
			if (delta.X >= 0 && _offsetXtoCenter - delta.X <= 0)
			{
				_directionMove.Horisontal = DirectionHorisontal.Right;
			}
			else if (delta.X <= 0 && _offsetXtoCenter - delta.X >= 0)
			{
				_directionMove.Horisontal = DirectionHorisontal.Left;
			}
			else
			{
				_directionMove.Horisontal = DirectionHorisontal.None;
			}
			// движение по оси Y
			if (delta.Y >= 0 && _offsetYtoCenter - delta.Y <= 0)
			{
				_directionMove.Vertical = DirectionVertical.Up;
			}
			else if (delta.Y <= 0 && _offsetYtoCenter - delta.Y >= 0)
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
