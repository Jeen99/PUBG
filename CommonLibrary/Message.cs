using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary
{
	[Serializable]
	public class Message : IMessage
	{
		public virtual long Kills => throw new NotImplementedException();

		public virtual long Deaths => throw new NotImplementedException();

		public virtual long Battles => throw new NotImplementedException();

		public virtual TimeSpan Time => throw new NotImplementedException();

		public virtual string Login => throw new NotImplementedException();

		public virtual string Password => throw new NotImplementedException();

		public virtual bool Result => throw new NotImplementedException();

		public virtual Direction Direction => throw new NotImplementedException();

		public virtual PointF Location => throw new NotImplementedException();

		public virtual float Angle => throw new NotImplementedException();

		public virtual int Count => throw new NotImplementedException();

		public virtual TypesWeapon TypeWeapon => throw new NotImplementedException();

		public virtual float HP => throw new NotImplementedException();

		public virtual float Distance => throw new NotImplementedException();

		public virtual bool StartOrEnd => throw new NotImplementedException();

		public virtual int TimePassed => throw new NotImplementedException();

		public virtual float Damage => throw new NotImplementedException();

		public virtual RectangleF Shape => throw new NotImplementedException();

		public virtual float Radius => throw new NotImplementedException();

		public virtual SizeF Size => throw new NotImplementedException();

		public virtual TypesGameObject TypeGameObject => throw new NotImplementedException();

		public virtual List<IMessage> InsertCollections => throw new NotImplementedException();

		public virtual ulong ID => throw new NotImplementedException();

		public virtual TypesMessage TypeMessage => throw new NotImplementedException();
	}
}
