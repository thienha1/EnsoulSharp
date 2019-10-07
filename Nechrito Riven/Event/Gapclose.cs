using EnsoulSharp.Common;
using NechritoRiven.Core;

namespace NechritoRiven.Event
{
    class Gapclose : Core.Core
    {
        public static void Gapcloser(ActiveGapcloser gapcloser)
        {
            var t = gapcloser.Sender;
            if (t.IsEnemy && Spells.W.IsReady() && t.IsValidTarget() && !t.IsZombie)
            {
                if (t.IsValidTarget(Spells.W.Range + t.BoundingRadius))
                {
                    Spells.W.Cast(t);
                }
            }
        }
    }
}
