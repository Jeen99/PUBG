using System;
using System.Drawing;
using CommonLibrary;
using CSInteraction.Server;

namespace BattleRoyalServer
{
	public class NetworkFakeClient : INetworkClient
	{
		private readonly VisibleArea _visibleArea;
		public RectangleF VisibleArea => _visibleArea.Area;

		public IPlayer Player { get; private set; }

		public string Nick => throw new NotImplementedException();
		public string Password => throw new NotImplementedException();


		/// <summary>
		/// Не используемое событие, реализуемое от INetworkClient.
		/// </summary>
		public event GamerIsLoaded Event_GamerIsLoaded;
		/// <summary>
		///  Не используемое событие, реализуемое от INetworkClient.
		/// </summary>
		public event NetorkClientDisconnect EventNetorkClientDisconnect;

		public NetworkFakeClient(IGameModel model, int index)
		{
			this.Player = model.Players[index];
			this._visibleArea = new VisibleArea(Player);
		}

		public NetworkFakeClient(IPlayer player)
		{
			this.Player = player;
			this._visibleArea = new VisibleArea(player);
		}

		public void Dispose()
		{
			Event_GamerIsLoaded = null;
			EventNetorkClientDisconnect = null;
		}

		public void SaveStatistics(IMessage msg)
		{
		}

		public void SendMessgaeToClient(IMessage msg)
		{
		}
	}
}