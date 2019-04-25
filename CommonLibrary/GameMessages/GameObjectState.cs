using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class GameObjectState : IMessage
	{
		public TypesGameObject TypeGameObject { get; private set; }

		public ulong ID { get; private set; }

		public virtual TypesWeapon TypeWeapon { get => throw new NotImplementedException();
		protected set => throw new NotImplementedException(); }

		public long Kills => throw new NotImplementedException();

		public long Deaths => throw new NotImplementedException();

		public long Battles => throw new NotImplementedException();

		public TimeSpan Time => throw new NotImplementedException();

		public string Login => throw new NotImplementedException();

		public string Password => throw new NotImplementedException();

		public bool Result => throw new NotImplementedException();

		public Direction Direction => throw new NotImplementedException();

		public PointF Location => throw new NotImplementedException();

		public float Angle => throw new NotImplementedException();

		public int Count => throw new NotImplementedException();

		public float HP => throw new NotImplementedException();

		public float Distance => throw new NotImplementedException();

		public bool StartOrEnd => throw new NotImplementedException();

		public int TimePassed => throw new NotImplementedException();

		public float Damage => throw new NotImplementedException();

		public RectangleF Shape => throw new NotImplementedException();

		public float Radius => throw new NotImplementedException();

		public SizeF Size => throw new NotImplementedException();

		public List<List<IMessage>> InsertCollections { get; } = new List<List<IMessage>>();

		public virtual TypesMessage TypeMessage { get; } = TypesMessage.GameObjectState;

		public GameObjectState(ulong id,TypesGameObject type, List<IMessage> statesComponents)
		{
			ID = id;
			TypeGameObject = type;
			InsertCollections.Add(statesComponents);
		}
	}
}
