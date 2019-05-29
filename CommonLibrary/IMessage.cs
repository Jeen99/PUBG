using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using System.Drawing;

namespace CommonLibrary
{
	
	public interface IMessage
    {
		long Kills { get; }
		long Deaths { get; }
		long Battles { get; }
		TimeSpan Time { get; }
		string Login { get; }
		string Password { get; }
		bool Result { get; }
		Direction Direction { get; }
		PointF Location { get; }
		float Angle { get; }
		int Count { get; }
		TypesWeapon TypeWeapon { get; }
		float HP { get; }
		float Distance { get; }
		//если true начало какого-либо процесса
		bool StartOrEnd { get; }
		int TimePassed { get; }
		float Damage { get; }
		RectangleF Shape { get; }
		float Radius { get; }
		SizeF Size { get; }
		TypesGameObject TypeGameObject { get; }
		List<IMessage> InsertCollections { get; }
		ulong ID { get; set; }

		TypesMessage TypeMessage { get; }
	}

    
}
