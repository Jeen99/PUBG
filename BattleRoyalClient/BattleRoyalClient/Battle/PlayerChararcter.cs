using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class PlayerChararcter
	{
		private BattleModel parent;

		public Gamer Character { get; private set; }	
		public TypesWeapon[] Weapons { get; private set; }
		public PointF Location
		{ get {
				if (Character != null)
					return Character.Location;
				else return new PointF();
			}
		}

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
			if(Character!=null)
				Event_CharacterChange?.Invoke();
		}

		public PlayerChararcter(ulong ID, BattleModel parent)
		{
			this.ID = ID;
			this.parent = parent;
		}

		public void Create(Gamer gamer)
		{
			Character = gamer;
			Weapons = new TypesWeapon[4] { TypesWeapon.Not,
				TypesWeapon.Not, TypesWeapon.Not, TypesWeapon.Not };
			HP = 100f;
		}

		public void AddWeapon(TypesWeapon weapon)
		{
			int index = (int)weapon;
			Weapons[index] = weapon;
			if(Character!=null)
				Event_AddWeapon?.Invoke(index);
		}
	}
	public delegate void CharacterChange();
	public delegate void CharacterAddWeapon(int index);
}
