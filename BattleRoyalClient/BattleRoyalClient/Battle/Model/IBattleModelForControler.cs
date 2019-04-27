
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	interface IBattleModelForControler
	{
		ICharacterForControler CharacterController { get; }
		int CountPlayersInGame { get; }

		void ChangeCountPlayersInGame(int newCount);
		void CreateChangeModel(TypesChange typeChange);
		void OnChangeGameObject(IModelObject model, StateObject state = StateObject.Change);
		void CreateTraser(ulong idPlayer, float distance, float angle);
		void TurnedGameObject(ulong idObject, float angle);
		void ChangeCurrentWeaponAtGamer(ulong idGamer, TypesWeapon typeWeapon);
		void ChangeTimeTillReductionAtDeathZone(TimeSpan time);
		IModelObject RemoveObject(ulong idObject);
		void CreateExplosion(PointF location);
		void ChangeLocationAtObject(ulong id, PointF location);
		bool ContainsObject(ulong idObject);
		void AddGameObject(GameObject modelObject);
		void ChanageShapeAtObject(ulong idObject, RectangleF shape);
		void ChanageShapeAtZoneDeath(PointF location, float radius);
	}

	enum StateObject { Change, Delete };
	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	delegate void BattleModelChangedHandler(TypesChange typeChange);
	delegate void GameObjectChangedHandler(IModelObject model, StateObject state);
	delegate void ChangeCountPlayers();
}