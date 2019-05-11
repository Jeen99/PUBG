using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CommonLibrary;
using CommonLibrary.GameMessages;
using CommonLibrary.CommonElements;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Gamer : GameObject, IPlayer
	{
		public Gamer(IModelForComponents model, TypesGameObject typeGameObject, TypesBehaveObjects typeBehaveObject) 
			: base(model, typeGameObject, typeBehaveObject)
		{
			
		}

		/// <summary>
		/// Для упрощения доступа к расположения игрока на карте
		/// </summary>
		private SolidBody body;

		public PointF Location
		{
			get
			{
				if (body != null) return body.Shape.Location;
				else return PointF.Empty;
			}
		}

		public override void Setup()
		{
			base.Setup();
			body = Components.GetComponent<SolidBody>();
			Received_PlayerTurn += Handler_Received_PlayerTurn;
		}

		private void Handler_Received_PlayerTurn(IMessage msg)
		{
			Model.AddOutgoingMessage(msg);
		}

		public override void Dispose()
		{
			base.Dispose();
			Received_PlayerTurn -= Handler_Received_PlayerTurn;
		}
	}
}