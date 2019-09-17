using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoGangplank
{
    public static class BadaoGangplankLaneClear
    {
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear)
                return;
            if (!BadaoGangplankVariables.LaneQ.GetValue<bool>())
                return;
            foreach (AIMinionClient minion in MinionManager.GetMinions(BadaoMainVariables.Q.Range).OrderBy(x => x.Health))
            {
                if (minion.BadaoIsValidTarget() && BadaoMainVariables.Q.GetDamage(minion) >= minion.Health && !(Orbwalking.InAutoAttackRange(minion)))
                {
                    BadaoMainVariables.Q.Cast(minion);
                }
            }
        }
    }
}
