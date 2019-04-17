using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoyalClient
{
	class Hand: GameObject
	{
		public TypesWeapon currentWeapon;
		public Hand(Gamer player) : base(player.ID, player.Location, player.Size)
		{
			currentWeapon = player.CurrentWeapon;
			SetSize(player.Size);
		}

		private void SetSize(SizeF sizePlayer)
		{
			switch (currentWeapon)
			{
				case TypesWeapon.Not:
				case TypesWeapon.GrenadeCollection:
					Size = new SizeF(sizePlayer.Width*1.2f, sizePlayer.Height * 1.2f);
					break;
				case TypesWeapon.Gun:
					Size = new SizeF(sizePlayer.Width * 1.5f, sizePlayer.Height * 1.5f);
					break;
				case TypesWeapon.ShotGun:
					Size = new SizeF(sizePlayer.Width * 2.5f, sizePlayer.Height * 2.5f);
					break;
				case TypesWeapon.AssaultRifle:
					Size = new SizeF(sizePlayer.Width * 3f, sizePlayer.Height * 3f);
					break;
			}
		}
		public override string TextureName
		{
			get
			{
				return "Hand" + currentWeapon.ToString();
			}
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.HandPlayer;
	}
}
