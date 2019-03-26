using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Timers;

namespace BattleRoayleServer
{
	public class Magazin:Component
	{
		/// <summary>
		/// Когда true - осуществляется перезарядка магазина
		/// </summary>
		private bool reload;
		private float durationReload_BetweenShots;
		private float durationReload_Magazin;
		private int bulletsInMagazin;

		private Timer reloadMagazin;

		public TypesWeapon TypeMagazin { get; private set; }

		public Magazin(GameObject parent, TypesWeapon typeWeapon, float duration_BetweenShots,
			float duration_Magazin) : base(parent)
		{
			TypeMagazin = typeWeapon;

			durationReload_BetweenShots = duration_BetweenShots;
			durationReload_Magazin = duration_Magazin;

			CreateNewMagazin();
			//создаем таймер
			reloadMagazin = new Timer()
			{
				AutoReset = false		
			};
			reload = false;
		}

		public override IMessage State
		{
			get
			{
				return new MagazinState(bulletsInMagazin);
			}
		}

		private void Handler_ReloadBetweenShots(object sender, ElapsedEventArgs e)
		{
			reload = false;
			bulletsInMagazin--;
		}

		private void Handler_ReloadMagazin(object sender, ElapsedEventArgs e)
		{
			reload = false;
			CreateNewMagazin();
			if (Parent != null)
			{
				Parent.Model.HappenedEvents.Enqueue(new EndReloadWeapon(Parent.ID));
			}
		}

		private void Create_ReloadMagazin()
		{
			reload = true;
			reloadMagazin.Interval = (durationReload_Magazin);
			reloadMagazin.Elapsed -= Handler_ReloadBetweenShots;
			reloadMagazin.Elapsed -= Handler_ReloadMagazin;
			reloadMagazin.Elapsed += Handler_ReloadMagazin;
			reloadMagazin.Start();
			if (Parent != null)
			{
				Parent.Model.HappenedEvents.Enqueue(new StartReloadWeapon(Parent.ID));
			}
		}

		private void Create_ReloadBetweenShots()
		{
			reload = true;
			reloadMagazin.Interval = (durationReload_BetweenShots);
			reloadMagazin.Elapsed -= Handler_ReloadBetweenShots;
			reloadMagazin.Elapsed -= Handler_ReloadMagazin;
			reloadMagazin.Elapsed += Handler_ReloadBetweenShots;
			reloadMagazin.Start();
		}

		public IBullet GetBullet()
		{
			if (!reload)
			{
				if (bulletsInMagazin > 1)
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

		private void CreateNewMagazin()
		{
			switch (TypeMagazin)
			{
				case TypesWeapon.Gun:
					bulletsInMagazin = 8;
					break;
			}
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.MakeReloadWeapon:
					Create_ReloadMagazin();
					break;
			}
		}
	}
}