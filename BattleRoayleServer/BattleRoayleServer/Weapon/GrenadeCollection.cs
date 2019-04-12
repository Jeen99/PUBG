﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	public class GrenadeCollection:Weapon
	{	
		protected float strengthThrowGrenade = 30;

		public GrenadeCollection(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.GrenadeCollection;
			//задаем парамертры модели
			this.TimeBetweenShot = 50;
			this.TimeReload = 10000;
			this.bulletsInMagazin = 4;
			this.size = new SizeF(8, 8);
			this.restetution = 0;
			this.friction = 0;
			this.density = 0.5f;
			this.linearDamping = 0.85f;

			var throwGrenade = new Throw(this, strengthThrowGrenade);
			Components.Add(throwGrenade);

			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}