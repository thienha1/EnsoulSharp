using EnsoulSharp;
using EnsoulSharp.Common;

namespace Nechrito_Gragas
{
    class Dmg
    {
        public static float ComboDmg(AIBaseClient enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                // if (Logic.HasItem()) damage = damage + (float)Program.Player.GetAutoAttackDamage(enemy) * 0.7f;
                if (Spells.W.IsReady()) damage = damage + Spells.W.GetDamage(enemy) +
                              (float)Program.Player.GetAutoAttackDamage(enemy);
                if (Spells.Q.IsReady()) damage = damage + Spells.Q.GetDamage(enemy);
                if (Spells.R.IsReady()) damage = damage + Spells.R.GetDamage(enemy);
                return damage;
            }
            return 0;
        }
        public static bool IsLethal(AIBaseClient unit)
        {
            return ComboDmg(unit) / 1.75 >= unit.Health;
        }
    
}
}
