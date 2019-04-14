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
		public Gamer Character { get; private set; }
		private BattleModel parent;

		public PointF Location { get { return Character.Location; } }

		public event CharacterChange Event_CharacterChange;

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
			HP = 100f;
		}

		public void Create(Gamer gamer)
		{
			Character = gamer;
		}

		public delegate void CharacterChange();
	}
}
