using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public abstract class SolidBody : Component, IFieldObject
	{
		protected IGameModel gameModel;

		protected SolidBody(GameObject parent, IGameModel gameModel, PointF location, TypesSolid typesSolid)
			: base(parent)
		{
			TypeSolid = typesSolid;
			Location = location;
			this.gameModel = gameModel; 
			//размещаем объект на игровой карте
			gameModel.Field.Put(this);
		}

		public TypesSolid TypeSolid { get; private set; }

		/// <summary>
		/// Расположение объекта на игровой карте
		/// </summary>
		private PointF location; // если не выносить в переменную, не получается 
		//изменить свойство, без создания нового объекта
		public PointF Location
		{
			get { return location; }
			private set { location = value; }
		}

		public IList<CellField> CoveredCells { get; set; }

		public override void ProcessMsg(IComponentMsg msg)
		{
			switch (msg.Type)
			{
				//данный компонент не обрабатывает никакие внешние сообщения на данных момент
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
			location.X += dX;
			location.Y += dY;
			gameModel.Field.Move(this);
			//проверяем на столкновение 
			//в каждой клетке, на которой находится игрок
			foreach (CellField cell in CoveredCells)
			{
				foreach (IFieldObject fieldObject in cell.OnThisCell)
				{
					//проверка на возможность столкновения
					if (this.TypeSolid == TypesSolid.Solid && fieldObject.TypeSolid == TypesSolid.Solid)
					{
						if (fieldObject.CheckCollision(this))
						{
							//произошло столкновение отпраляем участникам сообщение об этом
							this.SendMessage(new CollisionObjects(fieldObject));
							fieldObject.SendMessage(new CollisionObjects(this));
						}
					}
				}
			}

		}

		protected abstract bool CheckCollisionWithCircle(IFieldObject fieldObject);
		protected abstract bool CheckCollisionWithRectangle(IFieldObject fieldObject);

		public bool CheckCollision(IFieldObject fieldObject)
		{
			switch (fieldObject.Type)
			{
				case TypesSolidBody.Circle:
					return CheckCollisionWithCircle(this);
				case TypesSolidBody.Rectangle:
					return CheckCollisionWithRectangle(this);
				default:
					return false;
			}		
		}

		public abstract IList<Directions> CheckCovered(Tuple<float, float> XDiapason, Tuple<float, float> YDiapason);

		public abstract TypesSolidBody Type { get; }
	}

	public enum TypesSolid
	{
		Solid,
		Transparent
	}
}