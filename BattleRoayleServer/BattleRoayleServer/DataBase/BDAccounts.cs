using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    static class BDAccounts
    {
        public static bool ExistAccount(string Login, string Password)
        {
			//простейшая реализация
            return true;
        }

        public static bool CreateAccount(string Login, string Password)
        {
            return false;
        }

        public static DataOfAccount GetDataOfAccount(string nickName, string password)
        {
            return new DataOfAccount(nickName, password, 0, 0, 0, DateTime.Now);
        }

        public static bool AddToStatistic(AchievementsOfBattle achievements)
        {
            return false; 
        }
    }
}