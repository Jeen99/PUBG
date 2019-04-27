using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CommonLibrary.GameMessages;
using CommonLibrary;

namespace BattleRoayleServer
{
    public interface IPlayer
    {
		ulong ID { get; }
        /// <summary>
        /// Возращает координтаы игрока на карте
        /// </summary>
        /// <remarks>Координаты нужны для определния области, в которой игок видит игровые объекты</remarks>
        PointF Location { get; }

		//event PlayerDeleted EventPlayerDeleted;

	}
}