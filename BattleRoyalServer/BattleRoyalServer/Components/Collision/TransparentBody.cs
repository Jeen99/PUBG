using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using System.Drawing;

namespace BattleRoyalServer
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

		public override IMessage State
		{
			get
			{
				return new BodyState(shape);
			}
		}

		public override void Dispose()
		{
			Parent.Model.AddOutgoingMessage(new DeletedInMap(Parent.ID));
		}

		public override void Setup()
		{
			
		}
	}
}
