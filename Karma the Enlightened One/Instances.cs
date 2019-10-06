using System.Collections.Generic;
using EnsoulSharp;
using EnsoulSharp.Common;
using EnsoulSharp.Common.Data;

namespace Karma
{
    /// <summary>
    ///     Instances Class, contains global instances
    /// </summary>
    internal class Instances
    {
        /// <summary>
        ///     Saved Target Instance
        /// </summary>
        private static AIHeroClient _target;

        /// <summary>
        ///     Saved Player Instance
        /// </summary>
        public static AIHeroClient Player { get; set; }

        /// <summary>
        ///     Target
        /// </summary>
        public static AIHeroClient Target
        {
            get
            {
                // If old target is invalid, get a new one.
                if (!_target.IsValidTarget(Range))
                {
                    // Return target & save -> (Saved Target)
                    return _target = TargetSelector.GetTarget(Range, TargetSelector.DamageType.Magical);
                }
                // Return (Saved Target)
                return _target;
            }
        }

        /// <summary>
        ///     Target Search Range
        /// </summary>
        public static float Range
        {
            get { return 1200f; /* Vision Range */ }
        }

        /// <summary>
        ///     Menu Instance
        /// </summary>
        public static Menu Menu { get; set; }

        /// <summary>
        ///     Orbwalker Instance
        /// </summary>
        public static Orbwalking.Orbwalker Orbwalker { get; set; }

        /// <summary>
        ///     Spells Instance
        /// </summary>
        public static Dictionary<SpellSlot, Spell> Spells { get; set; }
    }
}