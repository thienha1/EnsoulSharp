using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using EnsoulSharp;
using EnsoulSharp.Common;
using Color = System.Drawing.Color;
using ItemData = EnsoulSharp.Common.Data.ItemData;


namespace HeavenStrikeTalon
{
    using static Program;
    public static class Drawing
    {
        public static void OnDraw_Draw()
        {
            if (DrawW)
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Green);
            }
            if (DrawR)
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Blue);
            }
            if (DrawE)
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.Purple);
            }

        }
    }
}
