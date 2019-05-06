using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Concurrent;
using CommonLibrary.CommonElements;
using System.Windows.Media.Imaging;

namespace BattleRoyalClient
{
	class BattleModel : IBattleModelForControler, IBattleModelForView
	{
		public event BattleModelChangedHandler BattleModelChanged;
		public event GameObjectChangedHandler GameObjectChanged;

		//пока задаем прямо в коде
		public Size SizeMap { get; } = new Size(500, 500);
		private DeathZone deathZone;
		public Dictionary<ulong, GameObject> gameObjects = new Dictionary<ulong, GameObject>();
		public int CountPlayersInGame { get; private set; }
		private PlayerCharacter character;

		public ICharacterForControler CharacterController
		{
			get
			{
				return character;
			}
		}

		public ICharacterForView CharacterView
		{
			get
			{
				return character;
			}
		}

		public IDeathZoneForView DeathZone
		{
			get
			{
				return deathZone;
			}
		}

		public void CreateChangeModel(TypesChange typeChange)
		{
			BattleModelChanged?.Invoke(typeChange);
		}

		public void OnChangeGameObject(ulong idObject, StateObject state = StateObject.Change)
		{
			if(gameObjects.ContainsKey(idObject))
			GameObjectChanged?.Invoke(gameObjects[idObject], state);
		}

		public void OnChangeGameObject(IModelObject modelObject, StateObject state = StateObject.Change)
		{
			GameObjectChanged?.Invoke(modelObject, state);
		}

		public BattleModel(ulong id)
		{
			character = new PlayerCharacter(id, this);
		}

		public void ChangeCountPlayersInGame(int newCount)
		{
			CountPlayersInGame = newCount;
		}

		public void CreateTraser(ulong idPlayer, float distance, float angle)
		{
			var gunMan = gameObjects[idPlayer];
			Traser traser = new Traser(ulong.MaxValue, gunMan.Location,
				new SizeF(distance * 2, distance * 2), angle);
			OnChangeGameObject(traser);
		}

		public void TurnedGameObject(ulong idObject, float angle)
		{
			if (!gameObjects.ContainsKey(idObject)) return;
			gameObjects[idObject].Update(angle);
		}

		public void ChangeCurrentWeaponAtGamer(ulong idGamer, TypesWeapon typeWeapon)
		{
			if (!gameObjects.ContainsKey(idGamer)) return;
			var gamer = gameObjects[idGamer] as Gamer;
			if (gamer != null)
			{
				gamer.CurrentWeapon = typeWeapon;
			}
		}

		public void ChangeTimeTillReductionAtDeathZone(TimeSpan time)
		{
			if (deathZone == null) return;
			deathZone.TimeToChange = time;
		}

		public IModelObject RemoveObject(ulong idObject)
		{
			if (gameObjects.ContainsKey(idObject))
			{
				IModelObject modelObject = gameObjects[idObject];
				if (gameObjects.Remove(idObject))
				{
					return modelObject;
				}
			}
			return null;
		}

		public void CreateExplosion(PointF location)
		{
			Explosion traser = new Explosion(ulong.MaxValue, location);
			OnChangeGameObject(traser);
		}

		public void ChangeLocationAtObject(ulong id, PointF location)
		{
			if (!gameObjects.ContainsKey(id)) return;
			gameObjects[id].Update(location);
		}

		public bool ContainsObject(ulong idObject)
		{
			return gameObjects.ContainsKey(idObject);
		}

		public void AddGameObject(GameObject modelObject)
		{
			gameObjects.Add(modelObject.ID, modelObject);
			
			switch (modelObject.Type)
			{
				case TypesGameObject.DeathZone:
					deathZone = (DeathZone)modelObject;
					break;
			}

		}

		public void ChanageShapeAtObject(ulong idObject, RectangleF shape)
		{
			if (!gameObjects.ContainsKey(idObject)) return;
			gameObjects[idObject].Update(shape);
		}

		public void ChanageShapeAtZoneDeath(PointF location, float radius)
		{
			if (deathZone == null) return;
			deathZone.Update(new RectangleF(location, new SizeF(radius*2, radius*2)));
		}
	}
	public enum TypesChange
	{
		CountPlyers
	}
}
