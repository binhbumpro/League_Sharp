using LeagueSharp.Common;
using System;
using System.Linq;
using LeagueSharp;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
namespace calculater.Bussiness
{
    class GoldCreep
    {
        Menu Config;
        Render.Sprite GoldCoint;
        Render.Sprite Minion;
        double countGold = 0;
        int countminions = 0;
        public GoldCreep()
        {
            countGoldListCreep();
            Drawing.OnDraw += Drawing_OnDraw;
        }
        private static System.Drawing.Bitmap LoadImg(string imgName)
        {
            var bitmap = DrawSprite.ResourceManager.GetObject(imgName) as System.Drawing.Bitmap;
            if (bitmap == null)
            {
                Console.WriteLine(imgName + ".png not found.");
            }
            return bitmap;
        }

        private void Drawing_OnDraw(EventArgs args)
        {



            float scanarea = Config.Item("scanrange").GetValue<Slider>().Value;
            int countOfMelee = 0;
            int countOfRange = 0;
            int countOfSeige = 0;
            int countOfSuper = 0;
            if (Config.Item("scangoldkey").GetValue<KeyBind>().Active)
            {
                //Console.WriteLine("Weight   :" + Drawing.Width);
                // Console.WriteLine("Height   :" + Drawing.Height);
                GoldCoint = new Render.Sprite(LoadImg("Gold"), new Vector2(Drawing.Width * 5 / 6, Drawing.Height * 4 / 5));
                GoldCoint.OnDraw();
                GoldCoint.Add(0);
                Minion = new Render.Sprite(LoadImg("Minion"), new Vector2(Drawing.Width * 5 / 6, Drawing.Height * 7 / 8));
                Minion.Add(0);
                Minion.OnDraw();
                Drawing.DrawText(Drawing.Width * 49 / 60, Drawing.Height * 4 / 5, Color.Goldenrod, countGold + "");
                Drawing.DrawText(Drawing.Width * 49 / 60, Drawing.Height * 7 / 8, Color.MediumVioletRed, countminions + "");
                Drawing.DrawText(
                        Drawing.WorldToScreen(Game.CursorPos).X - 200, Drawing.WorldToScreen(Game.CursorPos).Y - 200, Color.Gold, countminions + "  Minions");
                Drawing.DrawCircle(Game.CursorPos, scanarea, Color.Aqua);
                Drawing.DrawText(
                      Drawing.WorldToScreen(Game.CursorPos).X - 150, Drawing.WorldToScreen(Game.CursorPos).Y - 50, Color.Gold,
                      countGold + "  Gold");
                
                if (Config.Item("detail").GetValue<bool>())
                {
                    Drawing.DrawText(
                        Drawing.WorldToScreen(Game.CursorPos).X - 200, Drawing.WorldToScreen(Game.CursorPos).Y - 200, Color.Gold, countminions + "  Minions");
                }
              
            }
            Utility.DelayAction.Add(14, () => Minion.Dispose());
            Utility.DelayAction.Add(14, () => GoldCoint.Dispose());

            var AllMinions = MinionManager.GetMinions(Game.CursorPos, scanarea, MinionTypes.All, MinionTeam.Enemy);
            for (int i = 0; i < AllMinions.Count; i++)
            {
                //Console.WriteLine("AAAAAA____" + AllMinions.ElementAt<Obj_AI_Base>(i).CharData.BaseSkinName);
                if (AllMinions.ElementAt<Obj_AI_Base>(i).CharData.BaseSkinName.Equals(STRING.STRINGMINIONMELEE))
                {
                    countOfMelee += 1;
                }
                else if (AllMinions.ElementAt<Obj_AI_Base>(i).CharData.BaseSkinName.Equals(STRING.STRINGMINIONRANGED))
                {
                    countOfRange += 1;
                }
                else if (AllMinions.ElementAt<Obj_AI_Base>(i).CharData.BaseSkinName.Equals(STRING.STRINGMINIONSIEGE))
                {
                    countOfSeige += 1;
                }
                else if (AllMinions.ElementAt<Obj_AI_Base>(i).CharData.BaseSkinName.Equals(STRING.STRINGMINIONSUPER))
                {
                    countOfSuper += 1;
                }
            }

            double goldOfMelee = (19.8 + (0.2 * (Game.ClockTime / 90))) * countOfMelee;
            double goldOfRange = (14.8 + (0.2 * (Game.ClockTime / 90))) * countOfRange;
            double goldOfSeige = (40 + (0.5 * (Game.ClockTime / 90))) * countOfSeige;
            double goldOfSuper = (40 + (Game.ClockTime / 180)) * countOfSuper;
            countGold = Math.Round(goldOfSeige + goldOfRange + goldOfMelee + goldOfSuper, 0, MidpointRounding.AwayFromZero);

            countminions = countOfMelee + countOfRange + countOfSeige + countOfSuper;
            //var MinionPostision = MinionManager.GetMinionsPredictedPositions(AllMinions,0,0,0, Game.CursorPos, scanarea,false,SkillshotType.SkillshotCircle);
            //Drawing.DrawLine(MinionPostision, Game.CursorPos, 2, Color.Pink);
        }

        public void countGoldListCreep()
        {
            Config = new Menu("HowMuchGold", "HowMuchGold", true);
            Config.AddItem(new MenuItem("scangoldkey", "Key to scan gold").SetValue(new KeyBind('T', KeyBindType.Press)));
            Config.AddItem(new MenuItem("scanrange", "Scan Range").SetValue(new Slider(50, 100, 2500)));
            Config.AddItem(new MenuItem("detail", "Show detail").SetValue(true));
            Config.AddToMainMenu();
        }

    }
}
