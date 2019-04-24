﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
    public interface IPlayer
    {
		void Update(IMessage msg);
		ulong ID { get; }
        /// <summary>
        /// Возращает координтаы игрока на карте
        /// </summary>
        /// <remarks>Координаты нужны для определния области, в которой игок видит игровые объекты</remarks>
        PointF Location { get; }

		//event PlayerDeleted EventPlayerDeleted;

	}
}