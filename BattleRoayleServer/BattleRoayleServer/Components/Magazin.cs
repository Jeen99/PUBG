using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Timers;

namespace BattleRoayleServer
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
			Parent?.Model?.AddEvent(new StartReloadWeapon((Parent as Weapon).Holder.ID));
			
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

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Magazin");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg as TimeQuantPassed);
					break;
				case TypesProgramMessage.MakeReloadWeapon:
					Create_ReloadMagazin();
					break;
			}
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			if (Reload != TypesReload.Not)
			{
					timeReload = timeReload.Add(new TimeSpan(0, 0, 0, 0, -msg.QuantTime));
					if (timeReload.Milliseconds < 0)
					{
						switch (Reload)
						{
							case TypesReload.ReloadBetweenShots:
								Handler_ReloadBetweenShots();
								break;
							case TypesReload.ReloadMagazin:
								Handler_ReloadMagazin();
								break;
						}
					}
			}
		}

		private void Handler_ReloadBetweenShots()
		{
			Reload = TypesReload.Not;
			bulletsInMagazinNow--;
		}

		private void Handler_ReloadMagazin()
		{
			Reload = TypesReload.Not;
			//cоздаем новый магазин
			bulletsInMagazinNow = bulletsInMagazin;

			Parent?.Model?.AddEvent(new EndReloadWeapon((Parent as Weapon).Holder.ID));

		}

		public override void Setup()
		{
			
		}
	}

	public enum TypesReload
	{
		ReloadBetweenShots,
		ReloadMagazin,
		Not
	}
}