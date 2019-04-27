using BattleRoayleServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using CommonLibrary;

namespace ServerTest
{
	class StubWeapon : IWeapon
	{
		public IGameObject Holder { get ; set ; }

		public TypesWeapon TypeWeapon { get; } = TypesWeapon.Gun;

		public DictionaryComponent Components { get; } = new DictionaryComponent();

		public bool Destroyed { get; } = false;

		public ulong ID { get; } = 1;

		public IModelForComponents Model { get; } = null;

		public IMessage State { get; } = null;

		public TypesGameObject Type { get; } = TypesGameObject.Weapon;

		public TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public void Dispose()
		{

		}

		public void Update(IMessage msg)
		{
			
		}

		public void SetDestroyed()
		{
			throw new NotImplementedException();
		}

		public void Setup()
		{
			throw new NotImplementedException();
		}

	}
}
