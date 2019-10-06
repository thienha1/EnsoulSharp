using EnsoulSharp;
using EnsoulSharp.Common;

namespace CheerleaderLux
{
    class Loader : Lux
    {
        static void Main(string[] args)
        {          
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
    }
}
