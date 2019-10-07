namespace KoreanOlaf
{
    using System;
    using EnsoulSharp;
    using EnsoulSharp.Common;

    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharacterName.ToLowerInvariant() == "olaf")
            {
                var Olafium = new Olaf();
            }
        }
    }
}