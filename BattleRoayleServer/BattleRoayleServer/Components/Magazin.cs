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
		private int bulletsInMagazin;

		private Timer reloadMagazin;

		public TypesWeapon TypeMagazin { get; private set; }

		public Magazin(IWeapon parent, TypesWeapon typeWeapon, float duration_BetweenShots,
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
			Reload = false;
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
			Reload = false;
			bulletsInMagazin--;
		}

		private void Handler_ReloadMagazin(object sender, ElapsedEventArgs e)
		{
			Reload = false;
			CreateNewMagazin();
			
			Parent?.Model?.AddEvent(new EndReloadWeapon(Parent.ID));
			
		}

		private void Create_ReloadMagazin()
		{
			Reload = true;
			reloadMagazin.Interval = (durationReload_Magazin);
			reloadMagazin.Elapsed -= Handler_ReloadBetweenShots;
			reloadMagazin.Elapsed -= Handler_ReloadMagazin;
			reloadMagazin.Elapsed += Handler_ReloadMagazin;
			reloadMagazin.Start();
			
			Parent?.Model?.AddEvent(new StartReloadWeapon(Parent.ID));
			
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
	}
}