using System;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.Common;

namespace KurisuMorgana
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            new KurisuMorgana();
        }
    }
}
