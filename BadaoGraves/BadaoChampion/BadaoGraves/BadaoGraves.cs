using System;
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
    using static BadaoGravesVariables;
    public static class BadaoGraves
    {
        public static void BadaoActivate()
        {
            BadaoGravesConfig.BadaoActivate();
            BadaoGravesCombo.BadaoActivate();
            BadaoGravesBurst.BadaoActivate();
            BadaoGravesAuto.BadaoActivate();
            BadaoGravesJungle.BadaoActivate();
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            EnsoulSharp.Player.OnIssueOrder += OnIssueOrder;
        }

        private static void OnIssueOrder(AIBaseClient sender, PlayerIssueOrderEventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None && !BurstKey.GetValue<KeyBind>().Active)
                return;
            if (!sender.IsMe)
                return;
            if (args.Order != GameObjectOrder.AttackUnit)
                return;
            if (args.Target == null)
                return;
            if (!(args.Target is AIBaseClient))
                return;
            if (Player.Distance(args.Target.Position) > Player.BoundingRadius + Player.AttackRange + args.Target.BoundingRadius - 20)
            {
                args.Process = false;
                return;
            }
            if (!BadaoGravesHelper.CanAttack())
            {
                args.Process = false;
                return;
            }
            BadaoGravesCombo.OnIssueOrder(sender, args);
            BadaoGravesBurst.OnIssueOrder(sender, args);
            BadaoGravesJungle.OnIssueOrder(sender, args);
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.E)
            {
                Utility.DelayAction.Add(0, () => Orbwalking.ResetAutoAttackTimer());
            }
        }
    }
}
