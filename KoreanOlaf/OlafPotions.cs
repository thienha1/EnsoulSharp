namespace KoreanOlaf
{
    using System;

    using EnsoulSharp;
    using EnsoulSharp.Common;

    using ItemData = EnsoulSharp.Common.Data.ItemData;

    class OlafPotions
    {
        private readonly OlafMenu olafMenu;

        public OlafPotions(OlafMenu olafMenu)
        {
            this.olafMenu = olafMenu;

            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (olafMenu.GetParamBool("koreanolaf.miscmenu.pot.healthactive")
                && ObjectManager.Player.HealthPercent
                < olafMenu.GetParamSlider("koreanolaf.miscmenu.pot.healthwhen")
                && !ObjectManager.Player.HasBuff("RegenerationPotion")
                && !ObjectManager.Player.InShop())
            {
                ItemData.Health_Potion.GetItem().Cast();
            }
        }
    }
}
