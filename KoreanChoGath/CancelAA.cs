using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.Common;
using KoreanCommon;

namespace KoreanChoGath
{
    class CancelAA
    {
        private readonly CommonChampion champion;
        private readonly Spell R;

        public CancelAA(CommonChampion champion)
        {
            R = champion.Spells.R;
            this.champion = champion;

            Orbwalking.BeforeAttack += CancelingAA;
        }

        private void CancelingAA(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target is AIHeroClient) 
            {
                if (champion.Player.GetAutoAttackDamage((AIBaseClient)args.Target) > args.Target.Health + 20f)
                {
                    args.Process = true;
                }
                else if (R.IsReady() && R.IsKillable((AIHeroClient)args.Target))
                {
                    args.Process = false;
                }
            }
        }
    }
}
