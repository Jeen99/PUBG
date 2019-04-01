using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoayleServer;
using Box2DX.Dynamics;
using CSInteraction.ProgramMessage;
using System.Drawing;

namespace ServerTest
{
	class StubSolidBody : ISolidBody
	{
		public StubSolidBody(IGameObject parent)
		{
			Parent = parent;
		}

		public IGameObject Parent { get; }

		public IMessage State { get; } = null;

		public Body Body => throw new NotImplementedException();

		public List<ISolidBody> CoveredObjects => throw new NotImplementedException();

		public System.Drawing.RectangleF Shape { get; } = new RectangleF();


		public void BodyDelete()
		{
			
		}

		public void Dispose()
		{
			
		}

		public List<ISolidBody> GetPickUpObjects()
		{
			return new List<ISolidBody>();
		}

		public void Setup()
		{
			throw new NotImplementedException();
		}

		public void UpdateComponent(IMessage msg)
		{
			
		}
	}
}
