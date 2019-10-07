using System;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;
using SharpDX;

//gg
namespace D_Jarvan
{
    internal class Program
    {
        private const string ChampionName = "JarvanIV";

        private static Orbwalking.Orbwalker _orbwalker;

        private static Spell _q, _w, _e, _r;

        private static SpellSlot _igniteSlot;

        private static Items.Item _tiamat, _hydra, _blade, _bilge, _rand, _lotis;

        private static Menu _config;

        private static AIHeroClient _player;

        private static bool _haveulti;

        private static SpellDataInstClient _smiteSlot;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            _player = ObjectManager.Player;
            if (ObjectManager.Player.CharacterName != ChampionName) return;

            _q = new Spell(SpellSlot.Q, 770f);
            _w = new Spell(SpellSlot.W, 300f);
            _e = new Spell(SpellSlot.E, 830f);
            _r = new Spell(SpellSlot.R, 650f);

            _q.SetSkillshot(0.5f, 70f, float.MaxValue, false, SkillshotType.SkillshotLine);
            _e.SetSkillshot(0.5f, 70f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            _smiteSlot = _player.Spellbook.GetSpell(_player.GetSpellSlot("summonersmite"));
            _igniteSlot = _player.GetSpellSlot("SummonerDot");

            _bilge = new Items.Item(3144, 475f);
            _blade = new Items.Item(3153, 425f);
            _hydra = new Items.Item(3074, 375f);
            _tiamat = new Items.Item(3077, 375f);
            _rand = new Items.Item(3143, 490f);
            _lotis = new Items.Item(3190, 590f);

            //D Jarvan
            _config = new Menu("D-Jarvan", "D-Jarvan", true);

            //TargetSelector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            _config.AddSubMenu(targetSelectorMenu);


            //Orbwalker
            _config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_config.SubMenu("Orbwalking"));

            //* var orbwalkerMenu = new Menu("LX-Orbwalker", "LX-Orbwalker");
            // LXOrbwalker.AddToMenu(orbwalkerMenu);
            //_config.AddSubMenu(orbwalkerMenu);*/

            //Combo
            _config.AddSubMenu(new Menu("Combo", "Combo"));
            _config.SubMenu("Combo").AddItem(new MenuItem("UseIgnite", "Use Ignite")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseQC", "Use Q")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseWC", "Use W")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseEC", "Use E")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRC", "Use R(killable)")).SetValue(true);
            _config.SubMenu("Combo").AddItem(new MenuItem("UseRE", "AutoR Min Targ")).SetValue(true);
            _config.SubMenu("Combo")
                .AddItem(new MenuItem("MinTargets", "Ult when>=min enemy(COMBO)").SetValue(new Slider(2, 1, 5)));
            _config.SubMenu("Combo")
                .AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            _config.SubMenu("Combo")
                .AddItem(
                    new MenuItem("ActiveComboEQR", "ComboEQ-R!").SetValue(new KeyBind("T".ToCharArray()[0],
                        KeyBindType.Press)));


            //Items public static Int32 Tiamat = 3077, Hydra = 3074, Blade = 3153, Bilge = 3144, Rand = 3143, lotis = 3190;
            _config.AddSubMenu(new Menu("items", "items"));
            _config.SubMenu("items").AddSubMenu(new Menu("Offensive", "Offensive"));
            _config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Tiamat", "Use Tiamat")).SetValue(true);
            _config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Hydra", "Use Hydra")).SetValue(true);
            _config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Bilge", "Use Bilge")).SetValue(true);
            _config.SubMenu("items")
                .SubMenu("Offensive")
                .AddItem(new MenuItem("BilgeEnemyhp", "If Enemy Hp <").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("items")
                .SubMenu("Offensive")
                .AddItem(new MenuItem("Bilgemyhp", "Or your Hp < ").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("items").SubMenu("Offensive").AddItem(new MenuItem("Blade", "Use Blade")).SetValue(true);
            _config.SubMenu("items")
                .SubMenu("Offensive")
                .AddItem(new MenuItem("BladeEnemyhp", "If Enemy Hp <").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("items")
                .SubMenu("Offensive")
                .AddItem(new MenuItem("Blademyhp", "Or Your  Hp <").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("items").AddSubMenu(new Menu("Deffensive", "Deffensive"));
            _config.SubMenu("items")
                .SubMenu("Deffensive")
                .AddItem(new MenuItem("Omen", "Use Randuin Omen"))
                .SetValue(true);
            _config.SubMenu("items")
                .SubMenu("Deffensive")
                .AddItem(new MenuItem("Omenenemys", "Randuin if enemys>").SetValue(new Slider(2, 1, 5)));
            _config.SubMenu("items")
                .SubMenu("Deffensive")
                .AddItem(new MenuItem("lotis", "Use Iron Solari"))
                .SetValue(true);
            _config.SubMenu("items")
                .SubMenu("Deffensive")
                .AddItem(new MenuItem("lotisminhp", "Solari if Ally Hp<").SetValue(new Slider(35, 1, 100)));
            /*_config.SubMenu("items").AddSubMenu(new Menu("Potions", "Potions"));
            _config.SubMenu("items").SubMenu("Potions").AddItem(new MenuItem("Hppotion", "Use Hp potion")).SetValue(true);
            _config.SubMenu("items").SubMenu("Potions").AddItem(new MenuItem("Hppotionuse", "Use Hp potion if HP<").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("items").SubMenu("Potions").AddItem(new MenuItem("Mppotion", "Use Mp potion")).SetValue(true);
            _config.SubMenu("items").SubMenu("Potions").AddItem(new MenuItem("Mppotionuse", "Use Mp potion if HP<").SetValue(new Slider(35, 1, 100)));
            */
            //Harass
            _config.AddSubMenu(new Menu("Harass", "Harass"));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseQH", "Use Q")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("UseEH", "Use E")).SetValue(true);
            _config.SubMenu("Harass").AddItem(new MenuItem("UseEQH", "Use EQ Combo")).SetValue(true);
            _config.SubMenu("Harass")
                .AddItem(new MenuItem("UseEQHHP", "EQ If Your Hp > ").SetValue(new Slider(85, 1, 100)));
            _config.SubMenu("Harass").AddItem(new MenuItem("UseItemsharass", "Use Tiamat/Hydra")).SetValue(true);
            _config.SubMenu("Harass")
                .AddItem(new MenuItem("harassmana", "Minimum Mana% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("harasstoggle", "AutoHarass (toggle)").SetValue(new KeyBind("G".ToCharArray()[0],
                        KeyBindType.Toggle)));
            _config.SubMenu("Harass")
                .AddItem(
                    new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            //LaneClear
            _config.AddSubMenu(new Menu("Farm", "Farm"));
            _config.SubMenu("Farm").AddSubMenu(new Menu("LaneFarm", "LaneFarm"));
            _config.SubMenu("Farm")
                .SubMenu("LaneFarm")
                .AddItem(new MenuItem("UseItemslane", "Use Items in LaneClear"))
                .SetValue(true);
            _config.SubMenu("Farm").SubMenu("LaneFarm").AddItem(new MenuItem("UseQL", "Q LaneClear")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("LaneFarm").AddItem(new MenuItem("UseEL", "E LaneClear")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("LaneFarm").AddItem(new MenuItem("UseWL", "W LaneClear")).SetValue(true);
            _config.SubMenu("Farm")
                .SubMenu("LaneFarm")
                .AddItem(new MenuItem("UseWLHP", "use W if Hp% <").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("LaneFarm")
                .AddItem(new MenuItem("lanemana", "Minimum Mana% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("LaneFarm")
                .AddItem(
                    new MenuItem("Activelane", "Jungle!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            _config.SubMenu("Farm").AddSubMenu(new Menu("LastHit", "LastHit"));
            _config.SubMenu("Farm").SubMenu("LastHit").AddItem(new MenuItem("UseQLH", "Q LastHit")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("LastHit").AddItem(new MenuItem("UseELH", "E LastHit")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("LastHit").AddItem(new MenuItem("UseWLH", "W LaneClear")).SetValue(true);
            _config.SubMenu("Farm")
                .SubMenu("LastHit")
                .AddItem(new MenuItem("UseWLHHP", "use W if Hp% <").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("LastHit")
                .AddItem(new MenuItem("lastmana", "Minimum Mana% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("LastHit")
                .AddItem(
                    new MenuItem("ActiveLast", "LastHit!").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));

            _config.SubMenu("Farm").AddSubMenu(new Menu("Jungle", "Jungle"));
            _config.SubMenu("Farm")
                .SubMenu("Jungle")
                .AddItem(new MenuItem("UseItemsjungle", "Use Items in jungle"))
                .SetValue(true);
            _config.SubMenu("Farm")
                .SubMenu("Jungle")
                .AddItem(new MenuItem("Usesmite", "Use Smite(toggle)").SetValue(new KeyBind("H".ToCharArray()[0],
                    KeyBindType.Toggle)));
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("UseQJ", "Q Jungle")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("UseEJ", "E Jungle")).SetValue(true);
            _config.SubMenu("Farm").SubMenu("Jungle").AddItem(new MenuItem("UseWJ", "W Jungle")).SetValue(true);
            _config.SubMenu("Farm")
                .SubMenu("Jungle")
                .AddItem(new MenuItem("UseWJHP", "use W if Hp% <").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("Jungle")
                .AddItem(new MenuItem("junglemana", "Minimum Mana% >").SetValue(new Slider(35, 1, 100)));
            _config.SubMenu("Farm")
                .SubMenu("Jungle")
                .AddItem(
                    new MenuItem("Activejungle", "Jungle!").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            //Forest
            _config.AddSubMenu(new Menu("Forest Gump", "Forest Gump"));
            _config.SubMenu("Forest Gump").AddItem(new MenuItem("UseEQF", "Use EQ in Mouse ")).SetValue(true);
            _config.SubMenu("Forest Gump").AddItem(new MenuItem("UseWF", "Use W ")).SetValue(true);
            _config.SubMenu("Forest Gump")
                .AddItem(
                    new MenuItem("Forest", "Active Forest Gump!").SetValue(new KeyBind("Z".ToCharArray()[0],
                        KeyBindType.Press)));


            //Misc
            _config.AddSubMenu(new Menu("Misc", "Misc"));
            _config.SubMenu("Misc").AddItem(new MenuItem("UseIgnitekill", "Use Ignite KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("UseQM", "Use Q KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("UseRM", "Use R KillSteal")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("Gap_W", "W GapClosers")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("UseEQInt", "EQ to Interrupt")).SetValue(true);
            _config.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Usepackes")).SetValue(true);

            //Misc
            _config.AddSubMenu(new Menu("HitChance", "HitChance"));
            _config.SubMenu("HitChance")
                  .AddItem(new MenuItem("Echange", "E Hit").SetValue(
                      new StringList(new[] { "Low", "Medium", "High", "Very High" })));

            //Drawings
            _config.AddSubMenu(new Menu("Drawings", "Drawings"));
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("DrawQR", "Draw EQ-R")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("Drawsmite", "Draw smite")).SetValue(true);
            _config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            _config.SubMenu("Drawings")
                .AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            _config.SubMenu("Drawings")
                .AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            //_config.AddToMainMenu();
            Chat.Print("<font color='#881df2'>D-Jarvan by Diabaths</font> Loaded.");
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            AIHeroClient.OnCreate += OnCreateObj;
            AIHeroClient.OnDelete += OnDeleteObj;
            Gapclosers.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupters.OnInterrupter += Interrupter_OnPossibleToInterrupt;
           
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (_config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (_config.Item("ActiveComboEQR").GetValue<KeyBind>().Active)
            {
                ComboEqr();
            }
            if ((_config.Item("ActiveHarass").GetValue<KeyBind>().Active ||
                 _config.Item("harasstoggle").GetValue<KeyBind>().Active) &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("harassmana").GetValue<Slider>().Value)
            {
                Harass();

            }
            if (_config.Item("Activelane").GetValue<KeyBind>().Active &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("lanemana").GetValue<Slider>().Value)
            {
                Laneclear();
            }
            if (_config.Item("Activejungle").GetValue<KeyBind>().Active &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("junglemana").GetValue<Slider>().Value)
            {
                JungleClear();
            }
            if (_config.Item("ActiveLast").GetValue<KeyBind>().Active &&
                (100 * (_player.Mana / _player.MaxMana)) > _config.Item("lastmana").GetValue<Slider>().Value)
            {
                LastHit();
            }
            if (_config.Item("Forest").GetValue<KeyBind>().Active)
            {
                Forest();
            }
            if (_config.Item("Usesmite").GetValue<KeyBind>().Active)
            {
                Smiteuse();
            }

            _player = ObjectManager.Player;

            _orbwalker.SetAttack(true);

            KillSteal();

        }
        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (_w.IsReady() && gapcloser.Sender.IsValidTarget(_w.Range) && _config.Item("Gap_W").GetValue<bool>())
            {
                _w.Cast(gapcloser.Sender, Packets());
            }
        }
        private static void Interrupter_OnPossibleToInterrupt(ActiveInterrupter interrupter)
        {
            if (_e.IsReady() && _q.IsReady() && interrupter.Sender.IsValidTarget(_q.Range) && _config.Item("UseEQInt").GetValue<bool>()
                && _e.GetPrediction(interrupter.Sender).Hitchance >= Echange())
                _e.Cast(interrupter.Sender, Packets());
            _q.Cast(interrupter.Sender, Packets());
        }
        private static float ComboDamage(AIBaseClient enemy)
        {
            var damage = 0d;
            if (_igniteSlot != SpellSlot.Unknown &&
               _player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready)
                damage += ObjectManager.Player.GetSummonerSpellDamage(enemy, Damage.DamageSummonerSpell.Ignite);
            if (Items.HasItem(3077) && Items.CanUseItem(3077))
                damage += _player.GetItemDamage(enemy, Damage.DamageItems.Tiamat);
            if (Items.HasItem(3074) && Items.CanUseItem(3074))
                damage += _player.GetItemDamage(enemy, Damage.DamageItems.RavenousHydra);
            if (Items.HasItem(3153) && Items.CanUseItem(3153))
                damage += _player.GetItemDamage(enemy, Damage.DamageItems.BotRK);
            if (Items.HasItem(3144) && Items.CanUseItem(3144))
                damage += _player.GetItemDamage(enemy, Damage.DamageItems.BilgewaterCutlass);
            if (_q.IsReady())
                damage += _player.GetSpellDamage(enemy, SpellSlot.Q) * 2 * 1.2;
            if (_e.IsReady())
                damage += _player.GetSpellDamage(enemy, SpellSlot.E);
            if (_r.IsReady())
                damage += _player.GetSpellDamage(enemy, SpellSlot.R);

            damage += _player.GetAutoAttackDamage(enemy, true) * 1.1;
            damage += _player.GetAutoAttackDamage(enemy, true);
            return (float)damage;
        }

        private static void Combo()
        {
            var useQ = _config.Item("UseQC").GetValue<bool>();
            var useW = _config.Item("UseWC").GetValue<bool>();
            var useE = _config.Item("UseEC").GetValue<bool>();
            var useR = _config.Item("UseRC").GetValue<bool>();
            var autoR = _config.Item("UseRE").GetValue<bool>();

            var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
            if (t != null && _config.Item("UseIgnite").GetValue<bool>() && _igniteSlot != SpellSlot.Unknown &&
                _player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready)
            {
                if (ComboDamage(t) > t.Health)
                {
                    _player.Spellbook.CastSpell(_igniteSlot, t);
                }
            }
            if (useR && _r.IsReady())
            {
                if (t != null && !_haveulti)
                    if (!t.HasBuff("JudicatorIntervention") && !t.HasBuff("Undying Rage") &&
                        ComboDamage(t) > t.Health)
                        _r.CastIfHitchanceEquals(t, HitChance.Medium, Packets());
            }
            if (useE && _e.IsReady() && _q.IsReady() && useQ)
            {
                //xsalice Code
                var vec = t.PreviousPosition - _player.PreviousPosition;
                var castBehind = _e.GetPrediction(t).CastPosition + Vector3.Normalize(vec) * 75;
                _e.Cast(castBehind, Packets());
                _q.Cast(t, Packets());
            }
            else
            {
                _e.Cast(t, Packets(), true);
                _q.Cast(t, Packets());
            }
            /* {

                if (t != null && t.Distance(_player.Position) <= _e.Range && _e.GetPrediction(t).Hitchance >= Echange())
                    _e.Cast(t, Packets(), true);
                _q.Cast(t, Packets(), true);
            }*/
            if (useW && _w.IsReady())
            {
                if (t != null && t.Distance(_player.Position) < _w.Range)
                    _w.Cast();

            }
            if (useQ && _q.IsReady() && !_e.IsReady() && !_w.IsReady())
            {
                if (t != null && t.Distance(_player.Position) < _q.Range)
                    _q.Cast(t, Packets(), true);

            }
            if (_r.IsReady() && autoR && !_haveulti)
            {
                if (ObjectManager.Get<AIHeroClient>().Count(hero => hero.IsValidTarget(_r.Range)) >=
                    _config.Item("MinTargets").GetValue<Slider>().Value)
                    _r.Cast(t, Packets(), true);
            }
            UseItemes(t);
        }

        private static void ComboEqr()
        {
            _player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosRaw);
            var manacheck = _player.Mana > _player.Spellbook.GetSpell(SpellSlot.Q).ManaCost + _player.Spellbook.GetSpell(SpellSlot.E).ManaCost + _player.Spellbook.GetSpell(SpellSlot.R).ManaCost;
            var t = TargetSelector.GetTarget(_q.Range + _r.Range, TargetSelector.DamageType.Magical);

            if (_e.IsReady() && _q.IsReady() && manacheck)
            {
                if (t != null && _player.Distance(t) > _q.Range)
                    _e.Cast(t.PreviousPosition, Packets());
                _q.Cast(t.PreviousPosition, Packets());

            }
            if (t != null && _config.Item("UseIgnite").GetValue<bool>() && _igniteSlot != SpellSlot.Unknown &&
               _player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready)
            {
                if (ComboDamage(t) > t.Health)
                {
                    _player.Spellbook.CastSpell(_igniteSlot, t);
                }
            }
            if (_r.IsReady() && !_haveulti && t != null)
            {
                _r.CastIfHitchanceEquals(t, HitChance.Immobile, Packets());
            }
            if (_w.IsReady())
            {
                if (t != null && t.Distance(_player.Position) < _w.Range)
                    _w.Cast();
            }
            UseItemes(t);
        }


        private static void Harass()
        {
            var target = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
            var useQ = _config.Item("UseQH").GetValue<bool>();
            var useE = _config.Item("UseEH").GetValue<bool>();
            var useEq = _config.Item("UseEQH").GetValue<bool>();
            var useEqhp = (100 * (_player.Health / _player.MaxHealth)) > _config.Item("UseEQHHP").GetValue<Slider>().Value;
            var useItemsH = _config.Item("UseItemsharass").GetValue<bool>();
            if (useEqhp && useEq && _q.IsReady() && _e.IsReady())
            {
                var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
                if (t != null && t.Distance(_player.Position) < _e.Range && _e.GetPrediction(t).Hitchance >= Echange())
                    _e.Cast(t, Packets());
                _q.Cast(t, Packets());
            }
            if (useQ && _q.IsReady())
            {
                var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
                if (t != null && t.Distance(_player.Position) < _q.Range)
                    _q.Cast(t, Packets());
            }
            if (useE && _e.IsReady())
            {
                var t = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);
                if (t != null && t.Distance(_player.Position) < _e.Range && _e.GetPrediction(t).Hitchance >= Echange())
                    _e.Cast(t, Packets());
            }

            if (useItemsH && _tiamat.IsReady() && target.Distance(_player.Position) < _tiamat.Range)
            {
                _tiamat.Cast();
            }
            if (useItemsH && _hydra.IsReady() && target.Distance(_player.Position) < _hydra.Range)
            {
                _hydra.Cast();
            }
        }

        private static void Laneclear()
        {
            var allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.PreviousPosition, _q.Range, MinionTypes.All);
            var rangedMinionsQ = MinionManager.GetMinions(ObjectManager.Player.PreviousPosition, _q.Range + _q.Width,
                MinionTypes.Ranged);
            var rangedMinionsE = MinionManager.GetMinions(ObjectManager.Player.PreviousPosition, _e.Range + _e.Width,
                MinionTypes.Ranged);
            var allMinionsE = MinionManager.GetMinions(ObjectManager.Player.PreviousPosition, _e.Range + _e.Width,
                MinionTypes.All);
            var useItemsl = _config.Item("UseItemslane").GetValue<bool>();
            var useQl = _config.Item("UseQL").GetValue<bool>();
            var useEl = _config.Item("UseEL").GetValue<bool>();
            var useWl = _config.Item("UseWL").GetValue<bool>();
            var usewhp = (100 * (_player.Health / _player.MaxHealth)) < _config.Item("UseWLHP").GetValue<Slider>().Value;

            if (_q.IsReady() && useQl)
            {
                var fl1 = _e.GetLineFarmLocation(rangedMinionsQ, _e.Width);
                var fl2 = _e.GetLineFarmLocation(allMinionsQ, _e.Width);

                if (fl1.MinionsHit >= 3)
                {
                    _q.Cast(fl1.Position);
                }
                else if (fl2.MinionsHit >= 2 || allMinionsQ.Count == 1)
                {
                    _q.Cast(fl2.Position);
                }
                else
                    foreach (var minion in allMinionsE)
                        if (!Orbwalking.InAutoAttackRange(minion) &&
                            minion.Health < 0.75 * _player.GetSpellDamage(minion, SpellSlot.Q))
                            _q.Cast(minion);
            }

            if (_e.IsReady() && useEl)
            {
                var fl1 = _e.GetCircularFarmLocation(rangedMinionsE, _e.Width);
                var fl2 = _e.GetCircularFarmLocation(allMinionsE, _e.Width);

                if (fl1.MinionsHit >= 3)
                {
                    _e.Cast(fl1.Position);
                }
                else if (fl2.MinionsHit >= 2 || allMinionsE.Count == 1)
                {
                    _e.Cast(fl2.Position);
                }
                else
                    foreach (var minion in allMinionsE)
                        if (!Orbwalking.InAutoAttackRange(minion) &&
                            minion.Health < 0.75 * _player.GetSpellDamage(minion, SpellSlot.E))
                            _e.Cast(minion);
            }
            if (usewhp && useWl && _w.IsReady() && allMinionsQ.Count > 0)
            {
                _w.Cast();

            }
            if (useItemsl && _tiamat.IsReady() && allMinionsQ.Count > 2)
            {
                _tiamat.Cast();
            }
            if (useItemsl && _hydra.IsReady() && allMinionsQ.Count > 2)
            {
                _hydra.Cast();
            }
        }

        private static void LastHit()
        {
            var allMinions = MinionManager.GetMinions(ObjectManager.Player.PreviousPosition, _q.Range, MinionTypes.All);
            var useQ = _config.Item("UseQLH").GetValue<bool>();
            var useW = _config.Item("UseWLH").GetValue<bool>();
            var useE = _config.Item("UseELH").GetValue<bool>();
            var usewhp = (100 * (_player.Health / _player.MaxHealth)) < _config.Item("UseWLHHP").GetValue<Slider>().Value;
            foreach (var minion in allMinions)
            {
                if (useQ && _q.IsReady() && _player.Distance(minion) < _q.Range &&
                    minion.Health < 0.95 * _player.GetSpellDamage(minion, SpellSlot.Q))
                {
                    _q.Cast(minion, Packets());
                }

                if (_e.IsReady() && useE && _player.Distance(minion) < _e.Range &&
                    minion.Health < 0.95 * _player.GetSpellDamage(minion, SpellSlot.E))
                {
                    _e.Cast(minion, Packets());
                }
                if (usewhp && useW && _w.IsReady() && allMinions.Count > 0)
                {
                    _w.Cast();

                }
            }
        }

        private static void JungleClear()
        {
            var mobs = MinionManager.GetMinions(_player.PreviousPosition, _q.Range,
                MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            var useItemsJ = _config.Item("UseItemsjungle").GetValue<bool>();
            var useQ = _config.Item("UseQJ").GetValue<bool>();
            var useW = _config.Item("UseWJ").GetValue<bool>();
            var useE = _config.Item("UseEJ").GetValue<bool>();
            var usewhp = (100 * (_player.Health / _player.MaxHealth)) < _config.Item("UseWJHP").GetValue<Slider>().Value;

            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (useQ && _q.IsReady() && _player.Distance(mob) < _q.Range)
                {
                    _q.Cast(mob, Packets());
                }
                if (_e.IsReady() && useE && _player.Distance(mob) < _q.Range)
                {
                    _e.Cast(mob, Packets());
                }
                if (_w.IsReady() && useW && usewhp && _player.Distance(mob) < _w.Range)
                {
                    _w.Cast();
                }
                if (useItemsJ && _tiamat.IsReady() && _player.Distance(mob) < _tiamat.Range)
                {
                    _tiamat.Cast();
                }
                if (useItemsJ && _hydra.IsReady() && _player.Distance(mob) < _hydra.Range)
                {
                    _hydra.Cast();
                }
            }

        }

        private static bool Packets()
        {
            return _config.Item("usePackets").GetValue<bool>();
        }

        private static HitChance Echange()
        {
            switch (_config.Item("Echange").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
                default:
                    return HitChance.Medium;
            }
        }

        private static int getSmiteDmg()
        {
            int level = _player.Level;
            int index = _player.Level / 5;
            float[] dmgs = { 370 + 20 * level, 330 + 30 * level, 240 + 40 * level, 100 + 50 * level };
            return (int)dmgs[index];
        }

        private static void Smiteuse()
        {
            string[] jungleMinions;
            if (Map.GetMap().Type.Equals(Map.MapType.TwistedTreeline))
            {
                jungleMinions = new string[] { "TT_Spiderboss", "TT_NWraith", "TT_NGolem", "TT_NWolf" };
            }
            else
            {
                jungleMinions = new string[] { "AncientGolem", "LizardElder", "Worm", "Dragon" };
            }

            var minions = MinionManager.GetMinions(_player.Position, 1000, MinionTypes.All, MinionTeam.Neutral);
            if (minions.Count() > 0)
            {
                int smiteDmg = getSmiteDmg();
                foreach (AIBaseClient minion in minions)
                {

                    Boolean b;
                    if (Map.GetMap().Type.Equals(Map.MapType.TwistedTreeline))
                    {
                        b = minion.Health <= smiteDmg &&
                            jungleMinions.Any(name => minion.Name.Substring(0, minion.Name.Length - 5).Equals(name));
                    }
                    else
                    {
                        b = minion.Health <= smiteDmg && jungleMinions.Any(name => minion.Name.StartsWith(name));
                    }

                    if (b)
                    {
                        _player.Spellbook.CastSpell(_smiteSlot.Slot, minion);
                    }
                }
            }
        }

        private static void UseItemes(AIHeroClient target)
        {
            var iBilge = _config.Item("Bilge").GetValue<bool>();
            var iBilgeEnemyhp = target.Health <=
                                (target.MaxHealth * (_config.Item("BilgeEnemyhp").GetValue<Slider>().Value) / 100);
            var iBilgemyhp = _player.Health <=
                             (_player.MaxHealth * (_config.Item("Bilgemyhp").GetValue<Slider>().Value) / 100);
            var iBlade = _config.Item("Blade").GetValue<bool>();
            var iBladeEnemyhp = target.Health <=
                                (target.MaxHealth * (_config.Item("BladeEnemyhp").GetValue<Slider>().Value) / 100);
            var iBlademyhp = _player.Health <=
                             (_player.MaxHealth * (_config.Item("Blademyhp").GetValue<Slider>().Value) / 100);
            var iOmen = _config.Item("Omen").GetValue<bool>();
            var iOmenenemys = ObjectManager.Get<AIHeroClient>().Count(hero => hero.IsValidTarget(450)) >=
                              _config.Item("Omenenemys").GetValue<Slider>().Value;
            var iTiamat = _config.Item("Tiamat").GetValue<bool>();
            var iHydra = _config.Item("Hydra").GetValue<bool>();
            var ilotis = _config.Item("lotis").GetValue<bool>();
            //var ihp = _config.Item("Hppotion").GetValue<bool>();
            // var ihpuse = _player.Health <= (_player.MaxHealth * (_config.Item("Hppotionuse").GetValue<Slider>().Value) / 100);
            //var imp = _config.Item("Mppotion").GetValue<bool>();
            //var impuse = _player.Health <= (_player.MaxHealth * (_config.Item("Mppotionuse").GetValue<Slider>().Value) / 100);

            if (_player.Distance(target) <= 450 && iBilge && (iBilgeEnemyhp || iBilgemyhp) && _bilge.IsReady())
            {
                _bilge.Cast(target);

            }
            if (_player.Distance(target) <= 450 && iBlade && (iBladeEnemyhp || iBlademyhp) && _blade.IsReady())
            {
                _blade.Cast(target);

            }
            if (Utility.CountEnemiesInRange(350) >= 1 && iTiamat && _tiamat.IsReady())
            {
                _tiamat.Cast();

            }
            if (Utility.CountEnemiesInRange(350) >= 1 && iHydra && _hydra.IsReady())
            {
                _hydra.Cast();

            }
            if (iOmenenemys && iOmen && _rand.IsReady())
            {
                _rand.Cast();

            }
            if (ilotis)
            {
                foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(hero => hero.IsAlly || hero.IsMe))
                {
                    if (hero.Health <= (hero.MaxHealth * (_config.Item("lotisminhp").GetValue<Slider>().Value) / 100) &&
                        hero.Distance(_player.PreviousPosition) <= _lotis.Range && _lotis.IsReady())
                        _lotis.Cast();
                }
            }
        }


        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
            var igniteDmg = _player.GetSummonerSpellDamage(target, Damage.DamageSummonerSpell.Ignite);
            if (target != null && _config.Item("UseIgnitekill").GetValue<bool>() && _igniteSlot != SpellSlot.Unknown &&
            _player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready)
            {
                if (igniteDmg > target.Health)
                {
                    _player.Spellbook.CastSpell(_igniteSlot, target);
                }
            }
            if (_q.IsReady() && _config.Item("UseQM").GetValue<bool>())
            {
                var t = TargetSelector.GetTarget(_q.Range, TargetSelector.DamageType.Magical);
                if (_q.GetDamage(t) > t.Health && _player.Distance(t) <= _q.Range)
                {
                    _q.Cast(t, Packets());
                }
            }
            if (_r.IsReady() && _config.Item("UseRM").GetValue<bool>())
            {
                var t = TargetSelector.GetTarget(_r.Range, TargetSelector.DamageType.Magical);
                if (t != null)
                    if (!t.HasBuff("JudicatorIntervention") && !t.HasBuff("Undying Rage") && _r.GetDamage(t) > t.Health)
                        _r.Cast(t, Packets(), true);
            }
        }

        private static void Forest()
        {
            var manacheck = _player.Mana > _player.Spellbook.GetSpell(SpellSlot.Q).ManaCost + _player.Spellbook.GetSpell(SpellSlot.E).ManaCost;
            _player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosRaw);
            var target = TargetSelector.GetTarget(_e.Range, TargetSelector.DamageType.Magical);

            if (_config.Item("UseEQF").GetValue<bool>() && _q.IsReady() && _e.IsReady() && manacheck)
            {
                _e.Cast(Game.CursorPosRaw, Packets());
                _q.Cast(Game.CursorPosRaw, Packets());
            }
            if (_config.Item("UseWF").GetValue<bool>() && _w.IsReady() && target != null &&
                _player.Distance(target) < _w.Range)
            {
                _w.Cast();
            }

        }

        private static void OnCreateObj(GameObject sender, EventArgs args)
        {
            if (!(sender is EffectEmitter)) return;
            var obj = (EffectEmitter)sender;
            if (obj != null && obj.IsMe && obj.Name == "JarvanCataclysm_tar")

            //debug
            //if (unit == ObjectManager.Player.Name)
            {
                // Game.PrintChat("Spell: " + name);
                _haveulti = true;
                return;
            }
        }

        private static void OnDeleteObj(GameObject sender, EventArgs args)
        {
            if (!(sender is EffectEmitter)) return;
            var obj = (EffectEmitter)sender;
            if (obj != null && obj.IsMe && obj.Name == "JarvanCataclysm_tar")
            {
                _haveulti = false;
                return;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_config.Item("Drawsmite").GetValue<bool>())
            {
                if (_config.Item("Usesmite").GetValue<KeyBind>().Active)
                {
                    Drawing.DrawText(Drawing.Width * 0.90f, Drawing.Height * 0.68f, System.Drawing.Color.DarkOrange,
                        "Smite Is On");
                }
                else
                    Drawing.DrawText(Drawing.Width * 0.90f, Drawing.Height * 0.68f, System.Drawing.Color.DarkRed,
                        "Smite Is Off");
            }
            if (_config.Item("CircleLag").GetValue<bool>())
            {
                if (_config.Item("DrawQ").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawW").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawE").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawR").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (_config.Item("DrawQR").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, _q.Range + _r.Range, System.Drawing.Color.Gray,
                        _config.Item("CircleThickness").GetValue<Slider>().Value,
                        _config.Item("CircleQuality").GetValue<Slider>().Value);
                }
            }
            else
            {
                if (_config.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _q.Range, System.Drawing.Color.White);
                }
                if (_config.Item("DrawW").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _w.Range, System.Drawing.Color.White);
                }
                if (_config.Item("DrawE").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _e.Range, System.Drawing.Color.White);
                }

                if (_config.Item("DrawR").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _r.Range, System.Drawing.Color.White);
                }
                if (_config.Item("DrawQR").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, _q.Range + _r.Range, System.Drawing.Color.White);
                }
            }
        }
    }
}




