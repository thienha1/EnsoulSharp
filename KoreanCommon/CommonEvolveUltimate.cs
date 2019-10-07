namespace KoreanCommon
{
    using System;

    using EnsoulSharp;

    public class CommonEvolveUltimate
    {
        public CommonEvolveUltimate()
        {
            AIBaseClient.OnAggro += EvolveUltimate;
        }

        private static void EvolveUltimate(AIBaseClient sender, EventArgs args)
        {
            if (sender.IsMe)
            {
                sender.Spellbook.EvolveSpell(SpellSlot.R);
            }
        }
    }
}