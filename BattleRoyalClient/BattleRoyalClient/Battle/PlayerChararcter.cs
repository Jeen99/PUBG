using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoyalClient
{
	class PlayerChararcter
	{
		public Gamer character;
		private BattleModel parent;

		public PointF Location { get { return character.Location; } }

		public event ChangeHPCharacter changeHP;

		public ulong ID { get; private set; }
		public float HP
		{
			get
			{
				return HP;
			}
			set
			{
				HP = value;
				OnChangeHP();
			}
		}

		private void OnChangeHP()
		{
			changeHP?.Invoke(HP);
		}

		public PlayerChararcter(ulong ID, BattleModel parent)
		{
			this.ID = ID;
			this.parent = parent;
			this.character = new Gamer();
		}

		internal void Create(Gamer gamer)
		{
			character = gamer;
		}

		public delegate void ChangeHPCharacter(float hp);
	}
}
