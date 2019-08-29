using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;
using Color = System.Drawing.Color;

namespace DeveloperUAC
{
    public class Program
    {
        
        public static Spell Q, W, E, R;
        private static Menu MainMenu;
        private static AIHeroClient Player { get { return ObjectManager.Player; } }
        static void Main(string[] args)
        {
            
            GameEvent.OnGameLoad += GameEvent_OnGameLoad;
        }

        private static void GameEvent_OnGameLoad()
        {
            

            MainMenu = new Menu("Developere", "DeveloperUAC", true);

            // combo menu
            var DevMenu = new Menu("Developer", "Developer Mode");
            
            DevMenu.Add(new MenuBool("Dev", "Setting", true));
            

            MainMenu.Add(DevMenu);
            MainMenu.Attach();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            SpellDataInstClient spellQ = Player.Spellbook.GetSpell(SpellSlot.Q);
            SpellData dataQ = Player.Spellbook.GetSpell(SpellSlot.Q).SData;
            SpellDataInstClient spellW = Player.Spellbook.GetSpell(SpellSlot.W);
            SpellData dataW = Player.Spellbook.GetSpell(SpellSlot.W).SData;
            SpellDataInstClient spellE = Player.Spellbook.GetSpell(SpellSlot.E);
            SpellData dataE = Player.Spellbook.GetSpell(SpellSlot.E).SData;
            SpellDataInstClient spellR = Player.Spellbook.GetSpell(SpellSlot.R);
            SpellData dataR = Player.Spellbook.GetSpell(SpellSlot.R).SData;
            var playerC = Player.Crit.ToString();
            var playerC1 = playerC.Replace("0 ", "") + "%";
            string bufferious = "";
            foreach (var buff in Player.Buffs)
            {
                bufferious += (buff.Name + "(" + buff.Count + ")" + ", ");

            }
            

                Drawing.DrawText(150, 40, Color.Orange, "Dev Essentials by BehroozUAC");
                Drawing.DrawText(150, 60,Color.White, "Coordinates:");
                Drawing.DrawText(150, 75,Color.White, Player.Position.ToString());
                Drawing.DrawText(150, 145, Color.Yellow, "General Info:");
                Drawing.DrawText(150, 170, Color.White, "Gold Earned: " + Player.GoldTotal.ToString());
                Drawing.DrawText(150, 185, Color.White, "Attack Delay: " + Player.AttackDelay.ToString());
                Drawing.DrawText(150, 200,Color.White, "Chance of Critical: " + playerC);
                
                Drawing.DrawText(150, 225,Color.White, "Player Direction:");
                Drawing.DrawText(150, 240, Color.White, Player.Direction.ToString());
                Drawing.DrawText(150, 265, Color.White, "Base AD: " + Player.BaseAttackDamage.ToString());
                Drawing.DrawText(150, 280, Color.White, "Base AP: " + Player.BaseAbilityDamage.ToString());
                Drawing.DrawText(150, 325, Color.White, "Cursor Position: " + Game.CursorPosRaw.ToString());
                Drawing.DrawText(150, 450, Color.White, "Buffs: ");
                Drawing.DrawText(150, 465, Color.White, bufferious.ToString());


                Drawing.DrawText(500, 40, Color.Yellow, "Skill Info:");
                Drawing.DrawText(500, 70, Color.White, "Q: ");
                Drawing.DrawText(500, 80, Color.Yellow, "--------");
                Drawing.DrawText(500, 90, Color.White, "Name: " + spellQ.Name.ToString());
                Drawing.DrawText(500, 105, Color.White, "Level: " + spellQ.Level.ToString());
                Drawing.DrawText(500, 120, Color.White, "Range: " + spellQ.SData.CastRange.ToString());
                Drawing.DrawText(500, 135, Color.White, "Delay: " + Q.Delay.ToString());
                Drawing.DrawText(500, 150, Color.White, "Width: " + Q.Width.ToString());
                Drawing.DrawText(500, 170, Color.White, "W: ");
                Drawing.DrawText(500, 180, Color.Yellow, "--------");
                Drawing.DrawText(500, 190, Color.White, "Name: " + spellW.Name.ToString());
                Drawing.DrawText(500, 200, Color.White, "Level: " + spellW.Level.ToString());
                Drawing.DrawText(500, 210, Color.White, "Range: " + spellW.SData.CastRange.ToString());
                Drawing.DrawText(500, 220, Color.White, "Delay: " + W.Delay.ToString());
                Drawing.DrawText(500, 230, Color.White, "Width: " + W.Width.ToString());
                Drawing.DrawText(500, 250, Color.White, "E: ");
                Drawing.DrawText(500, 260, Color.Yellow, "--------");
                Drawing.DrawText(500, 275, Color.White, "Name: " + spellE.Name.ToString());
                Drawing.DrawText(500, 290, Color.White, "Level: " + spellE.Level.ToString());
                Drawing.DrawText(500, 305, Color.White, "Range: " + spellE.SData.CastRange.ToString());
                Drawing.DrawText(500, 320, Color.White, "Delay: " + E.Delay.ToString());
                Drawing.DrawText(500, 335, Color.White, "Width: " + E.Width.ToString());
                Drawing.DrawText(500, 350, Color.White, "R: ");
                Drawing.DrawText(500, 360, Color.Yellow, "--------");
                Drawing.DrawText(500, 380, Color.White, "Name: " + spellR.Name.ToString());
                Drawing.DrawText(500, 400, Color.White, "Level: " + spellR.Level.ToString());
                Drawing.DrawText(500, 420, Color.White, "Range: " + spellR.SData.CastRange.ToString());
                Drawing.DrawText(500, 460, Color.White, "Delay: " + R.Delay.ToString());
                Drawing.DrawText(500, 480, Color.White, "Width: " + R.Width.ToString());
        }
    }
}
