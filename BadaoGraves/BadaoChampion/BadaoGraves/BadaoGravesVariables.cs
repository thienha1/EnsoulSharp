﻿using System;
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
    public static class BadaoGravesVariables
    {
        public static AIHeroClient Player { get { return ObjectManager.Player; } }
        // menu
        public static MenuItem ComboQ;
        public static MenuItem ComboW;
        public static MenuItem ComboR;
        public static MenuItem ComboE;

        public static MenuItem ManaJungle;
        public static MenuItem JungleQ;
        public static MenuItem JungleE;

        public static MenuItem AutoSmite;
        public static MenuItem AutoRKS;

        public static MenuItem BurstKey;
    }
}
