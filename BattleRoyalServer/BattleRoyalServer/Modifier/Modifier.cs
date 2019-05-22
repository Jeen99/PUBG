using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary.CommonElements;

namespace BattleRoyalServer
{
	public abstract class Modifier : GameObject
	{
		public Modifier(IModelForComponents model, TypesGameObject typeGameObject, TypesBehaveObjects typeBehaveObject) 
			: base(model, typeGameObject, typeBehaveObject)
		{

		}
	}
}