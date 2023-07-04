using HarmonyLib;
using System;
using Verse;

namespace FrankWilco.RimWorld
{
    [HarmonyPatch(typeof(GenCommandLine), nameof(GenCommandLine.Restart))]
    public static class RestartPatch
    {
        public static readonly string doorstopDisabledVariable = "DOORSTOP_DISABLE";
        public static readonly string[] doorstopVariables =
            {
                "DOORSTOP_INITIALIZED",
                "DOORSTOP_DLL_SEARCH_DIRS",
                "DOORSTOP_INVOKE_DLL_PATH",
                "DOORSTOP_MANAGED_FOLDER_DIR",
                "DOORSTOP_MONO_LIB_PATH",
                "DOORSTOP_PROCESS_PATH"
            };

        public static void Prefix()
        {
            // We have to remove all UnityDoorstop environment variables from
            // the current process to prevent it from passing these variables
            // to the new game instance. Otherwise, UnityDoorstop will assume
            // that it doesn't have to patch the new instance anymore.
            // See NeighTools/UnityDoorstop#34.
            foreach (string variable in doorstopVariables)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }

            // Check first if UnityDoorstop is disabled in an environment
            // variable stored in either HKCU or HKLM before removing it.
            bool isDisabled = false;
            // Respect user choice on Windows only. There's no way to check if
            // the disabled variable was set by UnityDoorstep or by the user
            // on other platforms.
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                bool userDisabled = Environment.GetEnvironmentVariable(
                    doorstopDisabledVariable, EnvironmentVariableTarget.User) != null;
                bool machineDisabled = Environment.GetEnvironmentVariable(
                    doorstopDisabledVariable, EnvironmentVariableTarget.Machine) != null;
                isDisabled = userDisabled || machineDisabled;
            }
            if (!isDisabled)
            {
                Environment.SetEnvironmentVariable(doorstopDisabledVariable, null);
            }
        }
    }
}
