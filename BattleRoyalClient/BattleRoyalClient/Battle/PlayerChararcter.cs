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
		public event ChangePositionHandler changePosition;

		public ulong ID { get; private set; }

		private float _HP;
		public float HP
		{
			get
			{
				return _HP;
			}
			set
			{
				_HP = value;
			}
		}

		public void OnChangeHP()
		{
			changeHP?.Invoke(HP);
		}

		public void OnChangePosition()
		{
			changePosition?.Invoke(character.Location);
		}

		public PlayerChararcter(ulong ID, BattleModel parent)
		{
			this.ID = ID;
			this.parent = parent;
			this.character = new Gamer();
			HP = 100f;
		}

		public void Create(Gamer gamer)
		{
			character = gamer;
		}

		public delegate void ChangeHPCharacter(float hp);
		public delegate void ChangePositionHandler(PointF location);
	}
}
