using System;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;

namespace KurisuMorgana
{
    internal class KurisuMorgana
    {
        private static Menu _menu;
        private static Spell _q, _w, _e, _r;
        private static Orbwalking.Orbwalker _orbwalker;
        private static readonly AIHeroClient Me = ObjectManager.Player;

        public KurisuMorgana()
        {
            // Console.WriteLine("Morgana injected...");
            if (Me.CharacterName != "Morgana")
                return;

            // set spells
            _q = new Spell(SpellSlot.Q, 1175f);
            _q.SetSkillshot(0.25f, 72f, 1200f, true, SkillshotType.SkillshotLine);

            _w = new Spell(SpellSlot.W, 900f);
            _w.SetSkillshot(0.50f, 225f, 2200f, false, SkillshotType.SkillshotCircle);

            _e = new Spell(SpellSlot.E, 750f);
            _r = new Spell(SpellSlot.R, 600f);

            _menu = new Menu("Kurisu's Morgana", "morgana", true);

            var orbmenu = new Menu(":: Orbwalker", "orbwalker");
            _orbwalker = new Orbwalking.Orbwalker(orbmenu);
            _menu.AddSubMenu(orbmenu);

            var ccmenu = new Menu(":: Morgana Settings", "ccmenu");

            var menuQ = new Menu("Dark Binding [Q]", "qmenu");
            menuQ.AddItem(new MenuItem("hitchanceq", "Binding Hitchance"))
                .SetValue(new Slider(3, 1, 4));
            menuQ.AddItem(new MenuItem("useqcombo", "Use in Combo")).SetValue(true);
            menuQ.AddItem(new MenuItem("useharassq", "Use in Harass")).SetValue(false);
            menuQ.AddItem(new MenuItem("useqanti", "Use on Gapcloser")).SetValue(true);
            menuQ.AddItem(new MenuItem("useqauto", "Use on Immobile")).SetValue(true);
            menuQ.AddItem(new MenuItem("useqdash", "Use on Dashing")).SetValue(true);
            menuQ.AddItem(new MenuItem("autoqaa", "Use on Enemy Cast"))
                .SetValue(true);
            ccmenu.AddSubMenu(menuQ);

            var menuW = new Menu("Tormented Soil [W]", "wmenu");
            menuW.AddItem(new MenuItem("hitchancew", "Tormentsoil Hitchance "))
                .SetValue(new Slider(3, 1, 4));
            menuW.AddItem(new MenuItem("calcw", "Calculated Ticks"))
                .SetValue(new Slider(6, 3, 10));
            menuW.AddItem(new MenuItem("usewcombo", "Use in Combo")).SetValue(true);
            menuW.AddItem(new MenuItem("useharassw", "Use in Harass")).SetValue(false);
            menuW.AddItem(new MenuItem("usewauto", "Use on Immobile")).SetValue(true);
            menuW.AddItem(new MenuItem("waitfor", "Cast only on if Immobile")).SetValue(true);
            ccmenu.AddSubMenu(menuW);

            var menuE = new Menu("BlackShield [E]", "emenu");
            menuE.AddItem(new MenuItem("shieldtg", "Shield Only Target Spells"))
                .SetValue(false);
            menuE.AddItem(new MenuItem("usemorge", "Enabled")).SetValue(true);

            var newmenu = new Menu("Use Shield [Who?]", "usefor");
            foreach (var frn in ObjectManager.Get<AIHeroClient>().Where(x => x.Team == Me.Team))
                newmenu.AddItem(new MenuItem("useon" + frn.CharacterName, "Shield " + frn.CharacterName)).SetValue(!frn.IsMe);
            menuE.AddSubMenu(newmenu);

            foreach (var ene in ObjectManager.Get<AIHeroClient>().Where(x => x.Team != Me.Team))
            {
                // create menu per enemy
                var champMenu = new Menu(ene.CharacterName, "cm" + ene.NetworkId);

                // check if spell is supported in lib
                foreach (var lib in KurisuLib.CCList.Where(x => x.HeroName == ene.CharacterName))
                {
                    var skillMenu = new Menu(lib.Slot + " - " + lib.SpellMenuName, "sm" + lib.SDataName);
                    skillMenu.AddItem(new MenuItem(lib.SDataName + "on", "Enable")).SetValue(true);
                    skillMenu.AddItem(new MenuItem(lib.SDataName + "waitz", "Humanize (Disabled)")).SetValue(true);
                    skillMenu.AddItem(new MenuItem(lib.SDataName + "pr", "Priority"))
                        .SetValue(new Slider(lib.DangerLevel, 1, 5));
                    champMenu.AddSubMenu(skillMenu);
                }

                menuE.AddSubMenu(champMenu);
            }

            ccmenu.AddSubMenu(menuE);

            var menuR = new Menu("Soul Shackles [R]", "rmenu");
            menuR.AddItem(new MenuItem("rkill", "Use in combo if killable")).SetValue(true);
            menuR.AddItem(new MenuItem("rcount", "Use in combo if enemies >= ")).SetValue(new Slider(3, 1, 5));
            menuR.AddItem(new MenuItem("useautor", "Use automatic if enemies >= ")).SetValue(new Slider(4, 2, 5));
            menuR.AddItem(new MenuItem("usercombo", "Enabled")).SetValue(true);
            ccmenu.AddSubMenu(menuR);

            ccmenu.AddItem(new MenuItem("harassmana", "Harass mana %")).SetValue(new Slider(55, 0, 99));
            _menu.AddSubMenu(ccmenu);

            var wwmenu = new Menu(":: Farm Settings", "wwmenu");
            wwmenu.AddItem(new MenuItem("farmw", "Use W")).SetValue(true);
            wwmenu.AddItem(new MenuItem("farmcount", "-> If Min Minions >=")).SetValue(new Slider(3, 1, 7));
            _menu.AddSubMenu(wwmenu);

            _menu.AddItem(new MenuItem("support", ":: Support Mode")).SetValue(false);
            _menu.AddItem(new MenuItem("dp", ":: Drawings")).SetValue(true);
            _menu.AddItem(new MenuItem("harasskey", ":: Harass [active]")).SetValue(new KeyBind('C', KeyBindType.Press));
            _menu.AddItem(new MenuItem("combokey", ":: Combo [active]")).SetValue(new KeyBind(32, KeyBindType.Press));

            _menu.AddItem(new MenuItem("farmkey", ":: WaveClear [active]"))
                .SetValue(new KeyBind('V', KeyBindType.Press));



            //_menu.AddToMainMenu();

            Chat.Print("<font color=\"#FF33D6\"><b>KurisuMorgana</b></font> - Loaded!");

            // events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnGameUpdate;
            Gapclosers.OnGapcloser += OnEnemyGapcloser;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;

            try
            {
                AIBaseClient.OnProcessSpellCast += OnProcessSpellCast;
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception thrown KurisuMorgana: (BlackShield: {0})", e);
            }
        }

        private static bool Immobile(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Knockup) ||
                   unit.HasBuffOfType(BuffType.Snare) ||
                   unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Suppression);
        }

        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (_menu.Item("support").GetValue<bool>())
            {
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed ||
                    _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
                {
                    var minion = args.Target as AIBaseClient;
                    if (minion != null && minion.IsMinion && minion.IsValidTarget())
                    {
                        if (HeroManager.Allies.Any(x => x.IsValidTarget(1000, false) && !x.IsMe))
                        {
                            args.Process = false;
                        }
                    }
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!Me.IsValidTarget(300, false))
            {
                return;
            }

            CheckDamage(TargetSelector.GetTarget(_r.Range + 10, TargetSelector.DamageType.Magical));

            AutoCast(_menu.Item("useqdash").GetValue<bool>(), _menu.Item("useqauto").GetValue<bool>(),
                     _menu.Item("usewauto").GetValue<bool>());

            if (_menu.Item("combokey").GetValue<KeyBind>().Active)
            {
                Combo(_menu.Item("useqcombo").GetValue<bool>(), _menu.Item("usewcombo").GetValue<bool>(),
                      _menu.Item("usercombo").GetValue<bool>());
            }

            if (_menu.Item("harasskey").GetValue<KeyBind>().Active)
            {
                Harass(_menu.Item("useharassq").GetValue<bool>(),
                       _menu.Item("useharassw").GetValue<bool>());
            }

            if (_menu.Item("farmkey").GetValue<KeyBind>().Active)
            {
                if (_menu.Item("farmw").GetValue<bool>() && _w.IsReady())
                {
                    var minionpositions = MinionManager.GetMinions(_w.Range).Select(x => x.Position.To2D());
                    var location = MinionManager.GetBestCircularFarmLocation(minionpositions.ToList(), _w.Width, _w.Range);
                    if (location.MinionsHit >= _menu.Item("farmcount").GetValue<Slider>().Value)
                    {
                        _w.Cast(location.Position);
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_menu.Item("dp").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Me.Position, _q.Range,
                    System.Drawing.Color.FromArgb(155, System.Drawing.Color.DeepPink), 4);
            }
        }

        private static void Combo(bool useq, bool usew, bool user)
        {
            if (useq && _q.IsReady())
            {
                var qtarget = TargetSelector.GetTargetNoCollision(_q);
                if (qtarget.IsValidTarget())
                {
                    var poutput = _q.GetPrediction(qtarget);
                    if (poutput.Hitchance >= (HitChance)_menu.Item("hitchanceq").GetValue<Slider>().Value + 2)
                    {
                        _q.Cast(poutput.CastPosition);
                    }
                }
            }

            if (usew && _w.IsReady())
            {
                var wtarget = TargetSelector.GetTarget(_w.Range + 10, TargetSelector.DamageType.Magical);
                if (wtarget.IsValidTarget())
                {
                    if (!_menu.Item("waitfor").GetValue<bool>() || _mw * 1 >= wtarget.Health)
                    {
                        var poutput = _w.GetPrediction(wtarget);
                        if (poutput.Hitchance >= (HitChance) _menu.Item("hitchancew").GetValue<Slider>().Value + 2)
                        {
                            _w.Cast(poutput.CastPosition);
                        }
                    }
                }
            }

            if (user && _r.IsReady())
            {
                var ticks = _menu.Item("calcw").GetValue<Slider>().Value;

                var rtarget = TargetSelector.GetTarget(_r.Range, TargetSelector.DamageType.Magical);
                if (rtarget.IsValidTarget() && _menu.Item("rkill").GetValue<bool>())
                {
                    if (_mr + _mq + _mw * ticks + _ma * 3 + _mi + _guise >= rtarget.Health)
                    {
                        if (rtarget.Health > _mr + _ma * 2 + _mw * 2 && !rtarget.IsZombie)
                        {
                            if (_e.IsReady()) _e.CastOnUnit(Me);
                                _r.Cast();
                        }
                    }

                    if (Me.CountEnemiesInRange(_r.Range) >= _menu.Item("rcount").GetValue<Slider>().Value)
                    {
                        if (_e.IsReady())
                            _e.CastOnUnit(Me);

                        _r.Cast();
                    }
                }
            }
        }

        private static void Harass(bool useq, bool usew)
        {
            if (useq && _q.IsReady())
            {
                var qtarget = TargetSelector.GetTargetNoCollision(_q);
                if (qtarget.IsValidTarget())
                {
                    if (Me.ManaPercent >= _menu.Item("harassmana").GetValue<Slider>().Value)
                    {
                        var poutput = _q.GetPrediction(qtarget);
                        if (poutput.Hitchance >= (HitChance)_menu.Item("hitchanceq").GetValue<Slider>().Value + 2)
                        {
                            _q.Cast(poutput.CastPosition);
                        }
                    }
                }
            }

            if (usew && _w.IsReady())
            {
                var wtarget = TargetSelector.GetTarget(_w.Range + 200, TargetSelector.DamageType.Magical);
                if (wtarget.IsValidTarget())
                {
                    if (Me.ManaPercent >= _menu.Item("harassmana").GetValue<Slider>().Value)
                    {
                        if (!_menu.Item("waitfor").GetValue<bool>() || _mw * 1 >= wtarget.Health)
                        {
                            var poutput = _w.GetPrediction(wtarget);
                            if (poutput.Hitchance >= (HitChance)_menu.Item("hitchancew").GetValue<Slider>().Value + 2)
                            {
                                _w.Cast(poutput.CastPosition);
                            }
                        }
                    }
                }
            }
        }

        private static void AutoCast(bool dashing, bool immobile, bool soil)
        {
            if (_q.IsReady())
            {
                foreach (var itarget in HeroManager.Enemies.Where(h => h.IsValidTarget(_q.Range)))
                {
                    if (immobile && Immobile(itarget))
                        _q.Cast(itarget);

                    if (immobile)
                        _q.CastIfHitchanceEquals(itarget, HitChance.Immobile);

                    if (dashing && itarget.Distance(Me.PreviousPosition) <= 400f)
                        _q.CastIfHitchanceEquals(itarget, HitChance.Dashing);
                }
            }

            if (_w.IsReady() && soil)
            {
                foreach (var itarget in HeroManager.Enemies.Where(h => h.IsValidTarget(_w.Range)))
                    if (immobile && Immobile(itarget))
                        _w.Cast(itarget.PreviousPosition);
            }

            if (_r.IsReady())
            {
                if (Me.CountEnemiesInRange(_r.Range) >= _menu.Item("useautor").GetValue<Slider>().Value)
                {
                    if (_e.IsReady())
                        _e.CastOnUnit(Me);

                    _r.Cast();
                }
            }
        }

        private static void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget(250f))
            {
                if (_menu.Item("useqanti").GetValue<bool>())
                    _q.Cast(gapcloser.Sender);
            }
        }

        private static float _mq, _mw, _mr;
        private static float _ma, _mi, _guise;
        private static void CheckDamage(AIBaseClient target)
        {
            if (target == null)
            {
                return;
            }

            var qready = Me.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready;
            var wready = Me.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready;
            var rready = Me.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready;
            var iready = Me.Spellbook.CanUseSpell(Me.GetSpellSlot("summonerdot")) == SpellState.Ready;

            _ma = (float)Me.GetAutoAttackDamage(target);
            _mq = (float)(qready ? Me.GetSpellDamage(target, SpellSlot.Q) : 0);
            _mw = (float)(wready ? Me.GetSpellDamage(target, SpellSlot.W) : 0);
            _mr = (float)(rready ? Me.GetSpellDamage(target, SpellSlot.R) : 0);
            _mi = (float)(iready ? Me.GetSummonerSpellDamage(target, Damage.DamageSummonerSpell.Ignite) : 0);

            _guise = (float)(Items.HasItem(3151)
                ? Me.GetItemDamage(target, Damage.DamageItems.LudensEcho)
                : 0);
        }

        internal static void OnProcessSpellCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && sender.Type == GameObjectType.AIHeroClient && _q.IsReady())
            {
                if (args.End.IsValid() && args.End.Distance(Me.PreviousPosition) <= 200 + Me.BoundingRadius)
                {
                    var hero = sender as AIHeroClient;
                    if (!hero.IsValid<AIHeroClient>() || !hero.IsValidTarget(_q.Range - 50))
                    {
                        return;
                    }

                    if ( _menu.Item("autoqaa").GetValue<bool>())
                    {
                        _q.CastIfHitchanceEquals(hero, HitChance.VeryHigh);
                    }
                }
            }

            if (sender.Type != Me.Type || !_e.IsReady() || !sender.IsEnemy || !_menu.Item("usemorge").GetValue<bool>())
                return;

            var attacker = ObjectManager.Get<AIHeroClient>().First(x => x.NetworkId == sender.NetworkId);
            foreach (var ally in HeroManager.Allies.Where(x => x.IsValidTarget(_e.Range, false)))
            {
                var detectRange = ally.PreviousPosition + (args.End - ally.PreviousPosition).Normalized() * ally.Distance(args.End);
                if (detectRange.Distance(ally.PreviousPosition) > ally.AttackRange - ally.BoundingRadius)
                    continue;

                foreach (var lib in KurisuLib.CCList.Where(x => x.HeroName == attacker.CharacterName && x.Slot == attacker.GetSpellSlot(args.SData.Name)))
                {
                    if (lib.Type == Skilltype.Unit && args.Target.NetworkId != ally.NetworkId)
                        return;

                    if (_menu.Item("shieldtg").GetValue<bool>() && lib.Type != Skilltype.Unit)
                        return;

                    if (_menu.Item(lib.SDataName + "on").GetValue<bool>() && _menu.Item("useon" + ally.CharacterName).GetValue<bool>())
                    {
                        Utility.DelayAction.Add(lib.Slot != SpellSlot.R ? 100 : 0, () => _e.CastOnUnit(ally));
                    }
                }
            }
        }
    }
}
