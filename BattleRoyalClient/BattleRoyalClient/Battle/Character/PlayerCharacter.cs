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
		public WeaponCharacter[] weapons { get; private set; }

		public int CountWeapons()
		{
			return weapons.Length;
		}

		public IWeaponCharacterForView GetWeapon(uint index)
		{
			return weapons[index];
		}

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

		public void OnChangeCharacter(TypesChangeCharacter typeChange, object data)
		{
			if(character!=null)
				Event_CharacterChange?.Invoke(typeChange, data);
		}

		public void OnChangeCharacter(TypesChangeCharacter typeChange)
		{
			if (character != null)
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
			weapons = new WeaponCharacter[4] 
			{
				new WeaponCharacter(TypesWeapon.Not),
				new WeaponCharacter(TypesWeapon.Not),
				new WeaponCharacter(TypesWeapon.Not),
				new WeaponCharacter(TypesWeapon.Not)
			};
			_HP = 100f;
			OnChangeCharacter(TypesChangeCharacter.All);
		}

		public void AddWeapon(TypesWeapon weapon)
		{
			uint index = (uint)weapon;
			weapons[index].TypesWeapon = weapon;
			OnChangeCharacter(TypesChangeCharacter.AddWeapon, index);
		}

		public void ChangeCurrentWeapon(TypesWeapon weapon)
		{
			OnChangeCharacter(TypesChangeCharacter.CurrentWepon);
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

		public void ChangeBulletInWeapon(TypesWeapon type, int countBullets)
		{
			for (uint i = 0; i < weapons.Length; i++)
			{
				if (weapons[i].TypesWeapon == type)
				{
					weapons[i].CountBullets = countBullets;
					OnChangeCharacter(TypesChangeCharacter.BulletInWeapon, i);
					break;
				}
			}
		}	
	}
	
	public enum TypesChangeCharacter
	{
		CurrentWepon,
		AddWeapon,
		HP,
		Location,
		BulletInWeapon,
		All
		
	}
	public delegate void CharacterChange(TypesChangeCharacter typeChange, object data = null);
}
