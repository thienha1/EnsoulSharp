using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using EnsoulSharp;
using EnsoulSharp.Common;
using Color = System.Drawing.Color;

namespace HeavenStrikeTalon
{
    using static Program;
    using static Extension;
    class LaneClear
    {
        public static void UpdateLaneClear()
        {
            if (WLaneClear && W.IsReady())
            {
                var minion = MinionManager.GetMinions(W.Range).FirstOrDefault();
                if (minion != null)
                {
                    W.Cast(minion);
                }
            }
        }
        public static void AfterAttackLaneClear(AttackableUnit unit, AttackableUnit target)
        {
            if (target.Team == GameObjectTeam.Neutral)
                return;
            if (QLaneClear && Q.IsReady())
            {
                Q.Cast(target as AIBaseClient);
            }
            else if (TiamatLaneClear && HasItem())
            {
                CastItem();
            }

        }
    }
}
