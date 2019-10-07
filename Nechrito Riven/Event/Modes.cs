#region

using System;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Modes : Core.Core
    {
        // Jungle, Combo etc.
        public static void OnDoCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                if (args.Target is AIMinionClient)
                {
                    var minions = MinionManager.GetMinions(Player.AttackRange + 380);

                    if (minions == null)
                    {
                        return;
                    }

                    foreach (var m in minions)
                    {
                        if (!Spells.Q.IsReady() || !MenuConfig.LaneQ || m.UnderTurret()) continue;

                        ForceItem();
                        ForceCastQ(m);
                    }
                }

                var objAiTurret = args.Target as AITurretClient;
                if (objAiTurret != null)
                {
                    if (objAiTurret.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
                    {
                        ForceCastQ(objAiTurret);
                    }
                }

                var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All, MinionTeam.Neutral);

                if (mobs == null) return;

                foreach (var m in mobs)
                {
                    if (!m.IsValid) return;

                    if (Spells.Q.IsReady() && MenuConfig.JnglQ)
                    {
                        ForceItem();
                        ForceCastQ(m);
                    }

                    else if (!Spells.W.IsReady() || !MenuConfig.JnglW) return;

                    ForceItem();
                    Spells.W.Cast(m);
                }
            }

            if(!Spells.Q.IsReady()) return;

            var a = HeroManager.Enemies.Where(x => x.IsValidTarget(Player.AttackRange + 360));

            var targets = a as AIHeroClient[] ?? a.ToArray();

            foreach (var target in targets)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    ForceItem();
                    ForceCastQ(target);
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (Qstack == 2)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }
                }

                if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

                if (!InWRange(target)) return;

                if (Spells.W.IsReady())
                {
                    Spells.W.Cast(target);
                }

                ForceItem();
                ForceCastQ(target);

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
                {
                    Spells.R.Cast(target.Position);
                }
            }
        }

        public static void QMove()
        {
            if (!MenuConfig.QMove || !Spells.Q.IsReady())
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosRaw);
           
            Utility.DelayAction.Add(Game.Ping + 2, () => Spells.Q.Cast(Player.Position -15));
        }

        public static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All, MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                if (!m.IsValid) return;

                if (Spells.E.IsReady() && MenuConfig.JnglE && !Player.IsWindingUp)
                {
                    Spells.E.Cast(m.Position);
                }
            }
        }

        public static void Laneclear()
        {
            var minions = MinionManager.GetMinions(Player.AttackRange + 380);

            if (minions == null)
            {
                return;
            }

            foreach (var m in minions)
            {
                if (m.UnderTurret()) continue;

                if (Spells.E.IsReady() && MenuConfig.LaneE)
                {
                    Spells.E.Cast(m);
                }

                if(!Spells.W.IsReady() || !MenuConfig.LaneW || !InWRange(m) || Player.IsWindingUp || m.Health > Spells.W.GetDamage(m)) continue;

                Spells.W.Cast(m);
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Player.AttackRange + 310, TargetSelector.DamageType.Physical);

            if(target == null || target.IsDead || !target.IsValidTarget() || target.IsInvulnerable) return;

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
            {
                var pred = Spells.R.GetPrediction(target);

                if (Qstack > 1 && !MenuConfig.OverKillCheck)
                {
                    Spells.R.Cast(pred.CastPosition);
                }

                if (MenuConfig.OverKillCheck && !Spells.Q.IsReady() && Qstack == 1)
                {
                    Spells.R.Cast(pred.CastPosition);
                }
            }

            if (Spells.E.IsReady() && !InWRange(target))
            {
                Spells.E.Cast(target.Position);
            }

            if (Spells.W.IsReady() && Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && (MenuConfig.AlwaysR || Dmg.GetComboDamage(target) > target.Health))
            {
                if (InWRange(target)) return;

                Spells.E.Cast(target.Position);
                ForceR();
                Utility.DelayAction.Add(190, ForceW);
            }

           else if (Spells.W.IsReady() && Spells.Q.IsReady() && Spells.E.IsReady())
            {
                Usables.CastYoumoo();

                Utility.DelayAction.Add(10, ForceItem);
                Utility.DelayAction.Add(190, () => Spells.W.Cast());
            }

            else if (Spells.W.IsReady() && InWRange(target))
            {
                ForceW();
            }
        }

        public static void Burst()
        {
            var target = TargetSelector.SelectedTarget;

            if (target == null || !target.IsValidTarget(425 + Spells.W.Range) || target.IsInvulnerable) return;

            if (!Spells.Flash.IsReady()) return;

            if (!(target.Health < Dmg.GetComboDamage(target)) && !MenuConfig.AlwaysF) return;

            if (!(Player.Distance(target.Position) >= 600)) return;

            if (!Spells.R.IsReady() || !Spells.E.IsReady() || !Spells.W.IsReady() || Spells.R.Instance.Name != IsFirstR) return;

            Usables.CastYoumoo();
            Spells.E.Cast(target.Position);
            ForceR();
            Utility.DelayAction.Add(170 + Game.Ping / 2, FlashW);
            ForceItem();
        }

        public static void FastHarass()
        {
            if (!Spells.Q.IsReady() || !Spells.E.IsReady()) return;

            var target = TargetSelector.GetTarget(450 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);

            if (!target.IsValidTarget() || target.IsZombie) return;

            if (!Orbwalking.InAutoAttackRange(target) && !InWRange(target))
            {
                Spells.E.Cast(target.Position);
            }

            Utility.DelayAction.Add(10, ForceItem);
            Utility.DelayAction.Add(170, () => ForceCastQ(target));
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }

            if (!Spells.Q.IsReady() || !Spells.E.IsReady() || Qstack != 3 || Orbwalking.CanAttack() || !Orbwalking.CanMove(5)) return;

            var epos = Player.PreviousPosition + (Player.PreviousPosition - target.PreviousPosition).Normalized() * 300;

            Spells.E.Cast(epos);
            Utility.DelayAction.Add(190, () => Spells.Q.Cast(epos));
        }

        public static void Flee()
        {
            if (MenuConfig.WallFlee)
            {
                var end = Player.PreviousPosition.Extend(Game.CursorPosRaw, Spells.Q.Range);
                var isWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

                var eend = Player.PreviousPosition.Extend(Game.CursorPosRaw, Spells.E.Range);
                var wallE = FleeLogic.GetFirstWallPoint(Player.PreviousPosition, eend);
                var wallPoint = FleeLogic.GetFirstWallPoint(Player.PreviousPosition, end);
                Player.GetPath(wallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                { Spells.Q.Cast(Game.CursorPosRaw); }


                if (!isWallDash || Qstack != 3 || !(wallPoint.Distance(Player.PreviousPosition) <= 800)) return;

                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);

                if (!(wallPoint.Distance(Player.PreviousPosition) <= 600)) return;

                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);

                if (!(wallPoint.Distance(Player.PreviousPosition) <= 45)) return;

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(wallE);
                }

                if (Qstack != 3 || !(end.Distance(Player.Position) <= 260) || !wallPoint.IsValid()) return;

                Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);
                Spells.Q.Cast(wallPoint);
            }
            else
            {
                var enemy = HeroManager.Enemies.Where(target => target.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                           ? 70 + 195 + Player.BoundingRadius
                           : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPosRaw, 300);
                var objAitargetes = enemy as AIHeroClient[] ?? enemy.ToArray();
                if (Spells.W.IsReady() && objAitargetes.Any()) foreach (var target in objAitargetes) if (InWRange(target)) Spells.W.Cast();
                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPosRaw);

                if (MenuConfig.FleeYomuu)
                {
                    Usables.CastYoumoo();
                }

                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
        }
    }
}
