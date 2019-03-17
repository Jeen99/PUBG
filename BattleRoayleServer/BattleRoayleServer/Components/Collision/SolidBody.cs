using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class SolidBody : Component, IFieldObject
	{
		protected IGameModel gameModel;

		public SolidBody(GameObject parent, IGameModel gameModel,RectangleF shape, TypesSolid typesSolid)
			: base(parent)
		{
			TypeSolid = typesSolid;
			this.gameModel = gameModel;
			this.shape = shape;
			gameModel.Field.Put(this);
			CoveredCells = new List<CellField>();
		}
		
		public TypesSolid TypeSolid { get; private set; }

		/// <summary>
		/// Расположение объекта на игровой карте
		/// </summary>
		private RectangleF shape;
		public RectangleF Shape { get { return shape; }}

		public IList<CellField> CoveredCells { get; set; }

		public override void ProcessMsg(IComponentMsg msg)
		{
			if (msg != null)
			{
				switch (msg.Type)
				{
					//данный компонент не обрабатывает никакие внешние сообщения на данных момент
				}
			}
			
		}

		public void SendMessage(IComponentMsg msg)
		{
			Parent.SendMessage(msg);
		}

		/// <summary>
		/// Перемещает объект на переданное расстояние, под заданным углом
		/// </summary>
		public virtual void AppendCoords(float dX, float dY)
		{
			//изменяем координаты
			//проверка на граничные значения X
			if (shape.X + dX < 0)
				shape.X = 0;
			else 
				if (shape.X + dX > gameModel.Field.LengthOfSide)
				shape.X = gameModel.Field.LengthOfSide;
			else
				shape.X = shape.X + dX;
			//проверка на граничные значения Y
			if (shape.Y + dY < 0)
				shape.Y = 0;
			else 
				if (shape.Y + dY > gameModel.Field.LengthOfSide)
					shape.Y = gameModel.Field.LengthOfSide;
			else
				shape.Y = shape.Y + dY;

			gameModel.Field.Move(this);
			//проверяем на столкновение 
			//в каждой клетке, на которой находится игрок
			foreach (CellField cell in CoveredCells)
			{
				foreach (IFieldObject fieldObject in cell.OnThisCell)
				{
					//проверка на возможность столкновения
					if (this.TypeSolid == TypesSolid.Solid && fieldObject.TypeSolid == TypesSolid.Solid
						&& !Equals(fieldObject, this))
					{
						if (fieldObject.Shape.IntersectsWith(this.Shape))
						{
							//убираем перемещение
							shape.X -= dX;
							shape.Y -= dY;
							//возвращаем объект нормлаьно
							gameModel.Field.Move(this);
							//произошло столкновение отпраляем участникам сообщение об этом
							this.SendMessage(new CollisionObjects(fieldObject));
							fieldObject.SendMessage(new CollisionObjects(this));
							break;
						}
					}
				}
			}
			gameModel.HappenedEvents.Enqueue(new PlayerMoved(Parent.ID, shape.Location));

		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
		public override IMessage State
		{
			get
			{
				return new BodyState(Shape);
			}
		}
	}

	public enum TypesSolid
	{
		Solid,
		Transparent
	}
}