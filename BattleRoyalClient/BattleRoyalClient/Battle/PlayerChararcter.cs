using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class PlayerChararcter
	{
		public Gamer Character { get; private set; }
		private BattleModel parent;
		public TypesWeapon[] Weapons { get; private set; }
		public PointF Location { get { return Character.Location; } }

		public event CharacterChange Event_CharacterChange;
		public event CharacterAddWeapon Event_AddWeapon;

		public ulong ID { get; private set; }

		private float _HP;
		public float HP
		{
			get { return _HP; }
			set{_HP = value;}
		}

		public void OnChangeCharacter()
		{
			Event_CharacterChange?.Invoke();
		}

		public PlayerChararcter(ulong ID, BattleModel parent)
		{
			this.ID = ID;
			this.parent = parent;
			this.Character = new Gamer(ID);
			Weapons = new TypesWeapon[4] { 0, 0, 0, 0};
			HP = 100f;
		}

		public void Create(Gamer gamer)
		{
			Character = gamer;
		}

		public void AddWeapon(TypesWeapon weapon)
		{
			int index = (int)weapon - 1;
			Weapons[index] = weapon;
			Event_AddWeapon?.Invoke(index);
		}
	}
	public delegate void CharacterChange();
	public delegate void CharacterAddWeapon(int index);
}
