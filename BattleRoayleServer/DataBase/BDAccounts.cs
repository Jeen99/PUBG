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
            return false;
        }

        public static bool CreateAccount(string Login, string Password)
        {
            return false;
        }

        public static DataOfAccount GetDataOfAccount()
        {
            return new DataOfAccount();
        }

        public static bool AddToStatistic(AchievementsOfBattle achievements)
        {
            return false; 
        }
    }
}