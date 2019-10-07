using EnsoulSharp;
using EnsoulSharp.Common;

namespace NechritoRiven.Core
{
    class Dmg : Core
    {
        public static float IgniteDamage(AIHeroClient target)
        {
            if (Spells.Ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(Spells.Ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Player.GetSummonerSpellDamage(target, Damage.DamageSummonerSpell.Ignite);
        }

        public static float GetComboDamage(AIBaseClient enemy)
        {
            if (enemy == null) return 0;

            float damage = 0;

            if (Spells.W.IsReady())
            {
                damage += Spells.W.GetDamage(enemy);
            }

            if (Spells.Q.IsReady())
            {
                var qcount = 4 - Qstack;
                damage += Spells.Q.GetDamage(enemy) * qcount + (float)Player.GetAutoAttackDamage(enemy) * (qcount + 1);
            }

            if (Spells.R.IsReady())
            {
                damage += Spells.R.GetDamage(enemy);
            }

            return damage;
        }

        public static float RDmg(AIHeroClient target)
        {
            float dmg = 0;

            if (target == null || !Spells.R.IsReady()) return 0;

            dmg += Spells.R.GetDamage(target);

            return dmg;
        }
    }
}
