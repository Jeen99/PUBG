using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using System.Timers;

namespace BattleRoyalServer
{
	public class Magazin : Component
	{
		
		public TypesReload Reload { get; protected set; }

		private int durationReload_BetweenShots;
		private int durationReload_Magazin;

		private readonly int bulletsInMagazin;
		private int bulletsInMagazinNow;

		private TimeSpan timeReload;

		public TypesWeapon TypeMagazin { get; private set; }

		public Magazin(IWeapon parent, TypesWeapon typeWeapon, int duration_BetweenShots,
			int duration_Magazin,int bulletsInMagazin) : base(parent)
		{
			TypeMagazin = typeWeapon;
			Reload = TypesReload.Not;

			this.bulletsInMagazin = bulletsInMagazin;

			durationReload_BetweenShots = duration_BetweenShots;
			durationReload_Magazin = duration_Magazin;

			//cоздаем новый магазин
			bulletsInMagazinNow = bulletsInMagazin;
		}

		public override IMessage State
		{
			get
			{
				return new MagazinState(bulletsInMagazinNow);
			}
		}

		private void Create_ReloadMagazin()
		{
			Reload =  TypesReload.ReloadMagazin;
			timeReload = new TimeSpan(0, 0, 0, 0, durationReload_Magazin);	
			Parent?.Model?.AddOutgoingMessage(new ReloadWeapon(Parent.Parent.ID, true));
			
		}

		private void Create_ReloadBetweenShots()
		{
			Reload =  TypesReload.ReloadBetweenShots;
			timeReload = new TimeSpan(0, 0, 0, 0, durationReload_BetweenShots);
		}

		public IBullet GetBullet()
		{
			if (Reload == TypesReload.Not)
			{
				if (bulletsInMagazinNow > 1)
					Create_ReloadBetweenShots();
				else
					Create_ReloadMagazin();

				bulletsInMagazinNow--;
				SendChangeBulletInWeaponMsg();

				return CreateBullet();
			}
			else
			{
				return null;
			}
		}

		private IBullet CreateBullet()
		{
			switch (TypeMagazin)
			{
				case TypesWeapon.Gun:
					return new GunBullet();
				case TypesWeapon.GrenadeCollection:
					return new GrenadeBullet();
				case TypesWeapon.AssaultRifle:
					return new AssaultRifleBullet();
				case TypesWeapon.ShotGun:
					return new ShotGunBullet();
				default: return null;
			}
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
			if (Reload != TypesReload.Not)
			{
					timeReload = timeReload.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
					if (timeReload.TotalMilliseconds < 0)
					{
						switch (Reload)
						{
							case TypesReload.ReloadBetweenShots:
								Handler_ReloadBetweenShots();
								break;
							case TypesReload.ReloadMagazin:
								Handler_ReloadMagazin(msg);
								break;
						}
					}
			}
		}

		private void Handler_ReloadBetweenShots()
		{
			Reload = TypesReload.Not;
		}

		private void Handler_ReloadMagazin(IMessage msg)
		{
			Reload = TypesReload.Not;
			//cоздаем новый магазин
			bulletsInMagazinNow = bulletsInMagazin;

			SendChangeBulletInWeaponMsg();

			Parent?.Model?.AddOutgoingMessage(new ReloadWeapon((Parent as Weapon).Parent.ID, false));

		}
		private void SendChangeBulletInWeaponMsg()
		{
			Weapon parentAsWeapon = Parent as Weapon;
			Parent?.Model?.AddOutgoingMessage(new ChangeBulletInWeapon(parentAsWeapon.Parent.ID,
					parentAsWeapon.TypeWeapon, bulletsInMagazinNow));
		}

		public override void Setup()
		{
			Parent.Received_TimeQuantPassed += Handler_TimeQuantPassed;
			Parent.Received_MakeReloadWeapon += Handler_ReloadMagazin;
		}

		public override void Dispose()
		{
			Parent.Received_TimeQuantPassed -= Handler_TimeQuantPassed;
			Parent.Received_MakeReloadWeapon -= Handler_ReloadMagazin;
		}
	}

	public enum TypesReload
	{
		ReloadBetweenShots,
		ReloadMagazin,
		Not
	}
}