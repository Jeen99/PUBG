﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Concurrent;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using System.Drawing;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
		Dictionary<ulong, IGameObject> GameObjects { get; }
        World Field { get;}
		ObservableQueue<IMessage> HappenedEvents { get; }
		List<IGameObject> Loot { get; }
		List<IGameObject> GetPickUpObjects(RectangleF shapePlayer);
		void AddGameObject(IGameObject gameObject);
		void RemoveGameObject(IGameObject gameObject);
		void Dispose();
	}
}