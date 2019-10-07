using EnsoulSharp;
using EnsoulSharp.Common;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Anim : Core.Core
    {
        private static int ExtraDelay => Game.Ping/2;

        private static bool SafeReset =>
                Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee &&
                Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None;

        public static void OnPlay(AIBaseClient sender, AIBaseClientPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 2;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + ExtraDelay, Reset);
                    }
                    break;
                case "Spell1b":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 3;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + ExtraDelay, Reset);
                    }
                    break;
                case "Spell1c":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 1;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qld * 10 + ExtraDelay, Reset);
                    }
                    break;
            }
        }
        private static void Reset()
        {
            Orbwalking.LastAaTick = 0;
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPosRaw, Player.Distance(Game.CursorPosRaw) + 10));
        }
    }
}
