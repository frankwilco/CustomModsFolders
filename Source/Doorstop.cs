using HarmonyLib;

namespace Doorstop
{
    public class Entrypoint
    {
        public static void Start()
        {
            Harmony harmony = new Harmony("io.frankwilco.modfolders");
            harmony.PatchAll();
        }
    }
}