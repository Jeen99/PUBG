using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoayleServer;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace ServerTest
{
	class StubMagazin : IMagazin
	{
		public IMessage State { get; } = null;

		public TypesWeapon TypeMagazin { get; } = TypesWeapon.Gun;

		public IGameObject Parent { get; }

		public StubMagazin(IGameObject parent)
		{
			Parent = parent;
		}

		public void Dispose()
		{
		}

		public IBullet GetBullet()
		{
			return new GunBullet();
		}

		public void UpdateComponent(IMessage msg)
		{

		}
	}
}
