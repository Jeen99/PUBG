using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.GameMessages;
using CommonLibrary.CommonElements;
using CommonLibrary;
using System.Drawing;

namespace BattleRoyalClient
{
	static class FactoryForCreateGameObjects
	{

		public static GameObject CreateGameObjects(IMessage msg)
		{
			GameObject creatingObject = null;
			switch (msg.TypeGameObject)
			{
				case TypesGameObject.Player:
					creatingObject = new Gamer(msg.ID);
					break;
				case TypesGameObject.Box:
					creatingObject = new Box(msg.ID);
					break;
				case TypesGameObject.Stone:
					creatingObject = new Stone(msg.ID);
					break;
				case TypesGameObject.Field:
					creatingObject = new Field(msg.ID);
					break;
				case TypesGameObject.DeathZone:
					creatingObject = new DeathZone(msg.ID);
					break;
				case TypesGameObject.Bush:
					creatingObject = new Bush(msg.ID);
					break;
				case TypesGameObject.Tree:
					creatingObject = new Tree(msg.ID);
					break;
				case TypesGameObject.Grenade:
					creatingObject = new Grenade(msg.ID);
					break;
				case TypesGameObject.Weapon:
					creatingObject = new Weapon(msg.ID, msg.TypeWeapon);
					break;
			}
			foreach (IMessage message in msg.InsertCollections)
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						creatingObject.Update(message.Shape);
						break;
					case TypesMessage.BodyZoneState:
						creatingObject.Update(new RectangleF(message.Location, new SizeF(message.Radius * 2, 
							message.Radius * 2)));
						break;
					case TypesMessage.FieldState:
						creatingObject.Update(new RectangleF(message.Size.Width / 2, message.Size.Height / 2,
							message.Size.Width, message.Size.Height));
						break;
					case TypesMessage.CurrentWeaponState:
						if (creatingObject.Type == TypesGameObject.Player)
						{
							(creatingObject as Gamer).CurrentWeapon = message.TypeWeapon;
						}
						break;
				}
			}
			return creatingObject;
		}
	}
}
