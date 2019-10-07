using EnsoulSharp.Common.Data;

namespace NechritoRiven.Core
{
    class Usables : Core
    {
        public static void CastHydra()
        {
            if (ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady())
                ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();

            if (ItemData.Titanic_Hydra_Melee_Only.GetItem().IsReady())
                ItemData.Titanic_Hydra_Melee_Only.GetItem().Cast();

            if (ItemData.Tiamat_Melee_Only.GetItem().IsReady())
                ItemData.Tiamat_Melee_Only.GetItem().Cast();
        }
        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }
    }
}
