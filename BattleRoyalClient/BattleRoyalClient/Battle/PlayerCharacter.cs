using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class PlayerCharacter : ICharacterForControler, ICharacterForView
	{
		private BattleModel parent;

		public Gamer character;	
		public TypesWeapon[] Weapons { get; private set; }
		public PointF Location
		{ get {
				if (character != null)
					return character.Location;
				else return new PointF();
			}
		}

		public event CharacterChange Event_CharacterChange;

		public ulong ID { get; private set; }

		private float _HP;
		public float HP
		{
			get { return _HP; }
		}

		public IGamerForView Character
		{
			get
			{
				return character;
			}
		}


		public void OnChangeCharacter(TypesChangeCharacter typeChange)
		{
			if(character!=null)
				Event_CharacterChange?.Invoke(typeChange);
		}

		public PlayerCharacter(ulong ID, BattleModel parent)
		{
			this.ID = ID;
			this.parent = parent;
		}

		public void Create(Gamer gamer)
		{
			character = gamer;
			Weapons = new TypesWeapon[4] { TypesWeapon.Not,
				TypesWeapon.Not, TypesWeapon.Not, TypesWeapon.Not };
			_HP = 100f;
			OnChangeCharacter(TypesChangeCharacter.All);
		}

		public void AddWeapon(TypesWeapon weapon)
		{
			int index = (int)weapon;
			Weapons[index] = weapon;
			if(character!=null)
				Event_CharacterChange?.Invoke( TypesChangeCharacter.AddWeapon);
		}

		public void ChangeCurrentWeapon(TypesWeapon weapon)
		{
			Event_CharacterChange?.Invoke( TypesChangeCharacter.CurrentWepon);
		}

		public void ChangeHP(float newHP)
		{
			_HP = newHP;
			OnChangeCharacter( TypesChangeCharacter.HP);
		}

		public void ChangeLocation(PointF location)
		{
			OnChangeCharacter(TypesChangeCharacter.Location);
		}

		
	}

	public enum TypesChangeCharacter
	{
		CurrentWepon,
		AddWeapon,
		HP,
		Location,
		All
		
	}
	public delegate void CharacterChange(TypesChangeCharacter typeChange);
	public delegate void CharacterAddWeapon(int index);
}
