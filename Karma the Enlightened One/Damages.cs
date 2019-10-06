using System.Collections.Generic;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;

namespace Karma
{
    /// <summary>
    ///     Damages Class, contains damage data / calculations.
    /// </summary>
    internal class Damages
    {
        /// <summary>
        ///     Get Damage
        /// </summary>
        /// <param name="target">Target Instance</param>
        /// <param name="spellSlot">Spell Slot</param>
        /// <param name="mantra">Mantra</param>
        /// <param name="explosionOnly">Explosion Only (Q)</param>
        /// <returns></returns>
        public static double GetDamage(AIBaseClient target, SpellSlot spellSlot, bool mantra, bool explosionOnly = false)
        {
            switch (spellSlot)
            {
                case SpellSlot.Q:
                    return GetQDamage(target, mantra, explosionOnly);
                case SpellSlot.W:
                    return GetWDamage(target, mantra);
            }
            return 0d;
        }

        /// <summary>
        ///     Calculate Q Damage
        /// </summary>
        /// <param name="target">Target Instance</param>
        /// <param name="mantra">Mantra Active</param>
        /// <param name="explodeOnly">Explosion Damage Only</param>
        /// <returns>Damage in double units</returns>
        private static double GetQDamage(AIBaseClient target, bool mantra, bool explodeOnly)
        {
            var magicDamage = new[] { 80, 125, 170, 215, 260 }[Instances.Spells[SpellSlot.Q].Level - 1];
            var explosionDamage = new[] { 50, 150, 250, 350 }[Instances.Spells[SpellSlot.R].Level - 1];
            var bonusDamage = new[] { 25, 75, 125, 175 }[Instances.Spells[SpellSlot.R].Level - 1];
            var damage = magicDamage;

            if (explodeOnly)
            {
                return CalcMagicDamage(
                    Instances.Player, target, explosionDamage + (Instances.Player.TotalMagicalDamage * .6));
            }
            if (mantra)
            {
                damage += bonusDamage + explosionDamage;
            }

            return CalcMagicDamage(Instances.Player, target, damage);
        }

        /// <summary>
        ///     Calculate W Damage
        /// </summary>
        /// <param name="target">Target Instance</param>
        /// <param name="mantra">Mantra Active</param>
        /// <returns>Damage in double units</returns>
        private static double GetWDamage(AIBaseClient target, bool mantra)
        {
            var magicDamage = new[] { 60, 110, 160, 210, 260 }[Instances.Spells[SpellSlot.W].Level - 1];
            var bonusDamage = new[] { 75, 150, 225, 300 }[Instances.Spells[SpellSlot.R].Level - 1];

            var damage = magicDamage;
            if (mantra)
            {
                damage += bonusDamage;
            }

            return CalcMagicDamage(Instances.Player, target, damage);
        }

        /// <summary>
        ///     Calculate Magic Damage
        /// </summary>
        /// <param name="source">Source Instance</param>
        /// <param name="target">Target Instance</param>
        /// <param name="amount">Raw Magic Damage</param>
        /// <returns>Real Magic Damage in double units</returns>
        private static double CalcMagicDamage(AIBaseClient source, AIBaseClient target, double amount)
        {
            var magicResist = target.SpellBlock;

            //Penetration cant reduce magic resist below 0
            double k;
            if (magicResist < 0)
            {
                k = 2 - 100 / (100 - magicResist);
            }
            else if ((target.SpellBlock * source.PercentMagicPenetrationMod) - source.FlatMagicPenetrationMod < 0)
            {
                k = 1;
            }
            else
            {
                k = 100 /
                    (100 + (target.SpellBlock * source.PercentMagicPenetrationMod) - source.FlatMagicPenetrationMod);
            }

            //Take into account the percent passives
            k = PassivePercentMod(source, target, k);

            k = k * (1 - target.PercentMagicPenetrationMod) * (1 + target.PercentMagicDamageMod);

            return k * amount;
        }

        /// <summary>
        ///     Add Passive Percent Mod
        /// </summary>
        /// <param name="source">Source Instance</param>
        /// <param name="target">Target Instance</param>
        /// <param name="k">Processed Magic Damage</param>
        /// <returns>Magic Damage after percent mod in double units</returns>
        private static double PassivePercentMod(AIBaseClient source, AIBaseClient target, double k)
        {
            var siegeMinionList = new List<string> { "Red_Minion_MechCannon", "Blue_Minion_MechCannon" };
            var normalMinionList = new List<string>
            {
                "Red_Minion_Wizard",
                "Blue_Minion_Wizard",
                "Red_Minion_Basic",
                "Blue_Minion_Basic"
            };

            //Minions and towers passives:
            if (source is AITurretClient)
            {
                //Siege minions receive 70% damage from turrets
                if (siegeMinionList.Contains(target.Name))
                {
                    k = 0.7d * k;
                }

                //Normal minions take 114% more damage from towers.
                else if (normalMinionList.Contains(target.Name))
                {
                    k = (1 / 0.875) * k;
                }

                // Turrets deal 105% damage to champions for the first attack.
                else if (target is AIHeroClient)
                {
                    k = 1.05 * k;
                }
            }
            if (!(target is AIHeroClient))
            {
                return k;
            }

            return k;
        }
    }
}