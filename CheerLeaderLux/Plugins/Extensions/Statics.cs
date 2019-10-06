using System;
using System.Collections.Generic;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;
using SharpDX;

namespace CheerleaderLux.Extensions
{
    public class Statics
    {
        public static Menu Config;
        public static readonly List<AIBaseClient> Attackers = new List<AIBaseClient>();
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q1;
        public static Spell E1;
        public static Spell W1;
        public static Spell R1;
        public static GameObject Lux_E;
        public static SpellSlot Ignite;
        public static readonly AIHeroClient player = ObjectManager.Player;
        public static readonly AIHeroClient Player = ObjectManager.Player;
    }
}
