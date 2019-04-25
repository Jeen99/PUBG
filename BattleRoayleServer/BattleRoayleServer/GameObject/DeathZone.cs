using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;

namespace BattleRoayleServer
{
	public class DeathZone : GameObject
	{
		public DeathZone(IModelForComponents model, float sizeMap) : base(model)
		{
			BodyZone bodyZone = new BodyZone(this, sizeMap);
			Components.Add(bodyZone);

			DamageZone damageZone = new DamageZone(this);
			Components.Add(damageZone);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.DeathZone;
	}
}
