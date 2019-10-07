#region

using EnsoulSharp;
using EnsoulSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Draw;
using NechritoRiven.Event;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Load
{
    internal class Load
    {
        public static void LoadAssembly()
        {
            MenuConfig.LoadMenu();
            Spells.Load();

            AIBaseClient.OnProcessSpellCast += OnCasted.OnCasting;
            AIBaseClient.OnDoCast += Modes.OnDoCast;
            AIBaseClient.OnProcessSpellCast += Core.Core.OnCast;
            AIBaseClient.OnPlayAnimation += Anim.OnPlay;

            Drawing.OnEndScene += DrawDmg.DmgDraw;
            Drawing.OnDraw += DrawRange.RangeDraw;
            Drawing.OnDraw += DrawWallSpot.WallDraw;

            Game.OnUpdate += KillSteal.Update;
            Game.OnUpdate += AlwaysUpdate.Update;

            Interrupters.OnInterrupter += Interrupt2.OnInterruptableTarget;
            Gapclosers.OnGapcloser += Gapclose.Gapcloser;

            Chat.Print("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 71</font></b>");
        }
    }
}
