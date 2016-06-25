using System;
using LeagueSharp.Common;
using calculater.Bussiness;

namespace calculater
{
     class Program
    { 
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;           
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            GoldCreep goldCreep  = new GoldCreep();
            InventoryCal inventoryCal = new InventoryCal();
        }
    }
}
