using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;

namespace LCS_Udyr
{
    static class Helper
    {
        public static bool IsTiger(this AIHeroClient unit)
        {
            return unit.HasBuff("UdyrTigerPunchBleed");
        }
        public static bool IsTurtle(this AIHeroClient unit)
        {
            return unit.HasBuff("UdyrTurrleStance");
        }
        public static bool IsBear(this AIHeroClient unit)
        {
            return unit.HasBuff("UdyrBearStance");
        }
        public static bool IsPhoenix(this AIHeroClient unit)
        {
            return unit.HasBuff("UdyrPhoenixStance"); // udyrbearstuncheck
        }
        public static bool HasBearPassive(this AIHeroClient unit)
        {
            return unit.HasBuff("udyrbearstuncheck"); // 
        }
    }
}