using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Timers;

namespace BattleRoayleServer
{
	public class Magazin : Component, IMagazin
	{
		/// <summary>
		/// Когда true - осуществляется перезарядка магазина
		/// </summary>
		public bool Reload { get; protected set; }
		private float durationReload_BetweenShots;
		private float durationReload_Magazin;
		private readonly int bulletsInMagazin;
		private int bulletsInMagazinNow;

		private Timer reloadMagazin;

		public TypesWeapon TypeMagazin { get; private set; }

		public Magazin(IWeapon parent, TypesWeapon typeWeapon, float duration_BetweenShots,
			float duration_Magazin,int bulletsInMagazin) : base(parent)
		{
			TypeMagazin = typeWeapon;

			this.bulletsInMagazin = bulletsInMagazin;

			durationReload_BetweenShots = duration_BetweenShots;
			durationReload_Magazin = duration_Magazin;

			//cоздаем новый магазин
			bulletsInMagazinNow = bulletsInMagazin;

			//создаем таймер
			reloadMagazin = new Timer()
			{
				AutoReset = false		
			};
			Reload = false;
		}

		public override IMessage State
		{
			get
			{
				return new MagazinState(bulletsInMagazinNow);
			}
		}

		private void Handler_ReloadBetweenShots(object sender, ElapsedEventArgs e)
		{
			Reload = false;
			bulletsInMagazinNow--;
		}

		private void Handler_ReloadMagazin(object sender, ElapsedEventArgs e)
		{
			Reload = false;
			//cоздаем новый магазин
			bulletsInMagazinNow = bulletsInMagazin;
			
			Parent?.Model?.AddEvent(new EndReloadWeapon((Parent as Weapon).Holder.ID));
			
		}

		private void Create_ReloadMagazin()
		{
			Reload = true;
			reloadMagazin.Interval = (durationReload_Magazin);
			reloadMagazin.Elapsed -= Handler_ReloadBetweenShots;
			reloadMagazin.Elapsed -= Handler_ReloadMagazin;
			reloadMagazin.Elapsed += Handler_ReloadMagazin;
			reloadMagazin.Start();
			
			Parent?.Model?.AddEvent(new StartReloadWeapon((Parent as Weapon).Holder.ID));
			
		}

		private void Create_ReloadBetweenShots()
		{
			Reload = true;
			reloadMagazin.Interval = (durationReload_BetweenShots);
			reloadMagazin.Elapsed -= Handler_ReloadBetweenShots;
			reloadMagazin.Elapsed -= Handler_ReloadMagazin;
			reloadMagazin.Elapsed += Handler_ReloadBetweenShots;
			reloadMagazin.Start();
		}

		public IBullet GetBullet()
		{
			if (!Reload)
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
				default: return null;
			}
		}

		public override void Dispose()
		{
			reloadMagazin.Dispose();
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
					case TypesProgramMessage.MakeReloadWeapon:
						Create_ReloadMagazin();
						break;
				}
		}

		public override void Setup()
		{
			
		}
	}
}