﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CommonLibrary;
using CommonLibrary.CommonElements;

namespace BattleRoayleServer
{
	public interface IWeapon : IGameObject
	{	
		 TypesWeapon TypeWeapon { get; }
	}
}
