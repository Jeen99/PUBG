
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;
using CommonLibrary.CommonElements;
using CommonLibrary;

namespace BattleRoyalClient
{
	interface IBattleModelForController
	{
		void AddOutgoingMsg(IMessage msg);
		bool ModelIsLoaded { get; }
		ulong IDPlayer { get; }

		void ClearModel();
		void Initialize(ulong id);
		
	}

	enum StateObject { Change, Delete };
	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	delegate void BattleModelChangedHandler(TypesChange typeChange);
	delegate void GameObjectChangedHandler(IModelObject model, StateObject state);
	delegate void ChangeCountPlayers();
	delegate void ModelEndGame(IMessage msgEndGame);
	delegate void ServerDisconnect();
	delegate void ModelLoaded();
}