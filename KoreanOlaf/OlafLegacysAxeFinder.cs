namespace KoreanOlaf
{
    using System;
    using System.Linq;

    using EnsoulSharp;
    using EnsoulSharp.Common;

    using Color = System.Drawing.Color;

    class OlafLegacysAxeFinder
    {
        private readonly OlafMenu olafMenu;

        public OlafLegacysAxeFinder(OlafMenu olafMenu)
        {
            this.olafMenu = olafMenu;

            Drawing.OnDraw += Drawing_OnDraw;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (olafMenu.GetParamBool("koreanolaf.drawing.legacysaxe"))
            {
                Color color;

                switch (olafMenu.GetParamStringList("koreanolaf.drawing.legacysaxecolor"))
                {
                    case 0:
                        color = Color.YellowGreen;
                        break;
                    case 1:
                        color = Color.Red;
                        break;
                    default:
                        color = Color.White;
                        break;
                }

                int width = olafMenu.GetParamSlider("koreanolaf.drawing.legacysaxewidth");

                AIBaseClient axe =
                    ObjectManager.Get<AIBaseClient>()
                        .FirstOrDefault(obj => obj.Name.ToLowerInvariant().Contains("olafaxe") && obj.IsVisible);

                if (axe != null)
                {
                    Render.Circle.DrawCircle(axe.Position, 50F, color, 7 * width);

                    var from = Drawing.WorldToScreen(ObjectManager.Player.Position.Extend(axe.Position, 140F));
                    var to = Drawing.WorldToScreen(axe.Position.Shorten(ObjectManager.Player.Position, -58F));

                    Drawing.DrawLine(from.X, from.Y, to.X, to.Y, 2 * width, color);
                }
            }
        }
    }
}
