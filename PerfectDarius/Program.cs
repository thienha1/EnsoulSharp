using System;
using EnsoulSharp;

namespace PerfectDarius
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            new PerfectDarius();
        }
    }
}
