using EnsoulSharp;
using EnsoulSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Interrupt2 : Core.Core
    {
        public static void OnInterruptableTarget(ActiveInterrupter interrupter)
        {
            if (!MenuConfig.InterruptMenu || interrupter.Sender.IsInvulnerable) return;

            if (interrupter.Sender.IsValidTarget(Spells.W.Range))
            {
                if (Spells.W.IsReady())
                {
                    Spells.W.Cast(interrupter.Sender);
                }
            }
        }
    }
}
