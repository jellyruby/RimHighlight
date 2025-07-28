using HarmonyLib;
using Verse;
using System.Reflection;

namespace OneShotHighlight
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("YourName.OneShotHighlight");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}