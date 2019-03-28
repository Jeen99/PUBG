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
	class StubCollector : ICollector
	{
		public IGameObject Parent { get; private set; }

		public StubCollector(IGameObject parent)
		{
			Parent = parent;
		}

		public IMessage State { get; } = null;

		public void Dispose()
		{
		}

		public IWeapon GetWeapon(TypesWeapon typeWeapon)
		{
			return new StubWeapon();
		}

		public void SetNewParent(LootBox lootBox)
		{
			Parent = lootBox;
		}

		public void UpdateComponent(IMessage msg)
		{
		}
	}
}
