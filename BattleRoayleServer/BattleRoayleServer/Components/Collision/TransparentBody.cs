using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using System.Drawing;

namespace BattleRoayleServer
{
	public class TransparentBody : Component
	{
		private RectangleF shape;
		public RectangleF Shape
		{
			get
			{
				return shape;
			}
		}
		public TransparentBody(IGameObject parent, RectangleF shape) : base(parent)
		{
			this.shape = shape;
		}

		public override void UpdateComponent(IMessage msg)
		{

		}

		public override IMessage State
		{
			get
			{
				return new BodyState(shape);
			}
		}

		public override void Dispose()
		{
			Parent.Model.RemoveLoot(Parent);
		}
	}
}
