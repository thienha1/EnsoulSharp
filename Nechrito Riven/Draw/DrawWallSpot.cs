#region

using System;
using EnsoulSharp;
using EnsoulSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Event;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Draw
{
    internal class DrawWallSpot : Core.Core
    {
        public static void WallDraw(EventArgs args)
        {
            var end = Player.PreviousPosition.Extend(Game.CursorPosRaw, Spells.Q.Range);
            var isWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

            var wallPoint = FleeLogic.GetFirstWallPoint(Player.PreviousPosition, end);

            if (isWallDash && MenuConfig.FleeSpot)
            {
                if (wallPoint.Distance(Player.PreviousPosition) <= 600)
                {
                    Render.Circle.DrawCircle(wallPoint, 60, System.Drawing.Color.White);
                    Render.Circle.DrawCircle(end, 60, System.Drawing.Color.Green);
                }
            }
        }
    }
}
