using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;


namespace BadaoKingdom.BadaoChampion.BadaoGraves
{
    public static class BadaoGravesHelper
    {
        public static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static void BadaoActivate()
        {
        }
        public static bool CanAttack()

        {
            if (Player.IsWindingUp)
                return false;
            var attackDelay = (1.0740296828d * 1000 * Player.AttackDelay - 725) * 1.10;
            if (Utils.GameTimeTickCount + Game.Ping / 2 + 25 >= Orbwalking.LastAATick + attackDelay
                && Player.HasBuff("GravesBasicAttackAmmo1"))
            {
                return true;
            }
            return false;
        }
    }
}
