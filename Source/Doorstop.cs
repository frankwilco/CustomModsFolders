using HarmonyLib;

namespace Doorstop
{
    public class Entrypoint
    {
        public static bool IsPatched { get; private set; }

        public static void Start()
        {
            if (IsPatched)
            {
                return;
            }

            Harmony harmony = new Harmony("io.frankwilco.modfolders");
            harmony.PatchAll();

            IsPatched = true;
        }
    }
}