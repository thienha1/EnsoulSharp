using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom
{
    static class Program
    {
        public static readonly List<string> SupportedChampion = new List<string>()
        {
           "Graves"
        };
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (!SupportedChampion.Contains(ObjectManager.Player.CharacterName))
            {
                return;
            }
            Chat.Print("<font color=\"#24ff24\">Badao </font>" + "<font color=\"#ff8d1a\">" +
                ObjectManager.Player.CharacterName + "</font>" + "<font color=\"#24ff24\"> loaded!</font>");
            BadaoChampion.BadaoGraves.BadaoGraves.BadaoActivate();

            // summoner spells

            BadaoMainVariables.Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");
            BadaoMainVariables.Flash = ObjectManager.Player.GetSpellSlot("SummonerFlash");
            foreach (var spells in ObjectManager.Player.Spellbook.Spells.Where(
                x =>
                (x.Slot == SpellSlot.Summoner1 || x.Slot == SpellSlot.Summoner2)
                && x.Name.ToLower().Contains("smite")))
            {
                BadaoMainVariables.Smite = spells.Slot;
                break;
            }


        }
    }
}