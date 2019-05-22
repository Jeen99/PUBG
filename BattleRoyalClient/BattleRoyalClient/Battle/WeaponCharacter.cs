using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{ 
	public interface IWeaponCharacterForView
	{
		int CountBullets { get; }
		TypesWeapon TypesWeapon { get; }
	}

	public class WeaponCharacter : IWeaponCharacterForView
	{
		public TypesWeapon TypesWeapon { get; set; }
		public int CountBullets { get; set; }

		public WeaponCharacter(TypesWeapon typesWeapon, int countBullets)
		{
			this.TypesWeapon = typesWeapon;
			this.CountBullets = countBullets;
		}

		public WeaponCharacter(TypesWeapon typesWeapon)
		{
			this.TypesWeapon = typesWeapon;
			this.CountBullets = 0;
		}
	}
}
