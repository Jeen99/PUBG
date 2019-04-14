using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.Common
{
	[Serializable]
	public enum TypesGameObject
	{
		// используются в отрисовке. Чем больше значение, тем выше наодится объект.	
		Field = 1,
		DeathZone,
		Grenade,
		Stone,
		Box,
		Weapon,
		Player,
		Bush,
		Tree
	}
}
