#region

using System;
using EnsoulSharp;
using EnsoulSharp.Common;

#endregion

namespace NechritoRiven
{
    public class Program
    {
        private static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharacterName != "Riven")
            {
                Chat.Print("Could not load Riven");
                return;
            }
           Load.Load.LoadAssembly();
        }
    }
}