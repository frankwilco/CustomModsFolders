﻿using HarmonyLib;

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

#if DEBUG
            Harmony.DEBUG = true;
#endif
            Harmony harmony = new Harmony("io.frankwilco.custommodsfolders");
            harmony.PatchAll();

            IsPatched = true;
        }
    }
}