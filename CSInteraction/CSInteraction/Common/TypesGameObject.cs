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
		Player,
		Stone,
		Weapon,
		LootBox,
		Box,
		Bush,
		Tree
	}
}
