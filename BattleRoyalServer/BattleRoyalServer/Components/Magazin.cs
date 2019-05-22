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
		private int _durationReload_BetweenShots;
		private int _durationReload_Magazin;

		private readonly int _bulletsInMagazin;
		private int _bulletsInMagazinNow;

		private TimeSpan _timeReload;

		public TypesWeapon TypeMagazin { get; private set; }
		public TypesReload Reload { get; protected set; }

		public Magazin(IWeapon parent, TypesWeapon typeWeapon, int duration_BetweenShots,
			int duration_Magazin, int bulletsInMagazin) : base(parent)
		{
			TypeMagazin = typeWeapon;
			Reload = TypesReload.Not;

			_bulletsInMagazin = bulletsInMagazin;

			_durationReload_BetweenShots = duration_BetweenShots;
			_durationReload_Magazin = duration_Magazin;
			//cоздаем новый магазин
			_bulletsInMagazinNow = bulletsInMagazin;
		}

		public override IMessage State
		{
			get
			{
				return new MagazinState(_bulletsInMagazinNow);
			}
		}

		public IBullet GetBullet()
		{
			if (Reload == TypesReload.Not)
			{
				if (_bulletsInMagazinNow >= 1)
				{
					if (_bulletsInMagazinNow > 1)
						Start_ReloadBetweenShots();

					_bulletsInMagazinNow--;
					SendChangeBulletInWeaponMsg();

					return CreateBullet();
				}
				else
				{
					Start_FullReload();
				}		
			}
				return null;
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
					_timeReload = _timeReload.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
					if (_timeReload.TotalMilliseconds <= 0)
					{
						switch (Reload)
						{
							case TypesReload.ReloadBetweenShots:
								End_ReloadBetweenShots();
								break;
							case TypesReload.ReloadMagazin:
								End_FullReload(msg);
								break;
						}
					}
			}
		}
		private void Start_ReloadBetweenShots()
		{
			Reload = TypesReload.ReloadBetweenShots;
			_timeReload = new TimeSpan(0, 0, 0, 0, _durationReload_BetweenShots);
		}

		private void End_ReloadBetweenShots()
		{
			Reload = TypesReload.Not;
		}

		private void Start_FullReload(IMessage msg = null)
		{
			Reload = TypesReload.ReloadMagazin;
			_timeReload = new TimeSpan(0, 0, 0, 0, _durationReload_Magazin);
			Parent?.Model?.AddOutgoingMessage(new ReloadWeapon(Parent.Owner.ID, true));
		}

		private void End_FullReload(IMessage msg)
		{
			Reload = TypesReload.Not;
			//cоздаем новый магазин
			_bulletsInMagazinNow = _bulletsInMagazin;
			SendChangeBulletInWeaponMsg();

			Parent?.Model?.AddOutgoingMessage(new ReloadWeapon(Parent.Owner.ID, false));

		}
		private void SendChangeBulletInWeaponMsg()
		{
			Weapon parentAsWeapon = Parent as Weapon;
			Parent?.Model?.AddOutgoingMessage(new ChangeBulletInWeapon(parentAsWeapon.Owner.ID,
					parentAsWeapon.TypeWeapon, _bulletsInMagazinNow));
		}


		public override void Setup()
		{
			if (Parent.Owner != null)
			{
				Parent.Received_TimeQuantPassed += Handler_TimeQuantPassed;
				Parent.Owner.Received_MakeReloadWeapon += Start_FullReload;
			}
		}

		public override void Dispose()
		{
			Parent.Received_TimeQuantPassed -= Handler_TimeQuantPassed;
			if (Parent.Owner != null)
				Parent.Owner.Received_MakeReloadWeapon -= Start_FullReload;
		}
	}

	public enum TypesReload
	{
		ReloadBetweenShots,
		ReloadMagazin,
		Not
	}
}