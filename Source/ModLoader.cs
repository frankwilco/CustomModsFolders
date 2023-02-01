using HarmonyLib;
using System.IO;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.Steam;

namespace FrankWilco.RimWorld
{
    [HarmonyPatch(typeof(WorkshopItems), nameof(WorkshopItems.EnsureInit))]
    public static class ModLoader
    {
        private static readonly MethodInfo _tryAddMod =
            typeof(ModLister).GetMethod(
                "TryAddMod", BindingFlags.NonPublic | BindingFlags.Static);

        private static bool TryAddMod(ModMetaData mod)
        {
            return (bool)_tryAddMod.Invoke(null, new object[] { mod });
        }

        public static void Prefix()
        {
            string s = "Rebuilding mods list (custom mods folders)";

            foreach (ModsFolder folder in ModLoaderData.Current.ModsFolders)
            {
                if (!folder.active || folder.path.Trim() == "")
                {
                    continue;
                }
                s += $"\nAdding mods from custom folder: {folder.path}";
                var directoryNames = from d
                                     in new DirectoryInfo(folder.path).GetDirectories()
                                     select d.FullName;
                foreach (string item in directoryNames)
                {
                    ModMetaData modMetaData = new ModMetaData(
                        item, official: folder.markOfficial);
                    if (TryAddMod(modMetaData))
                    {
                        s = s + "\n  Adding " + modMetaData.ToStringLong();
                    }
                }
            }
#if !DEBUG
            if (Prefs.LogVerbose)
#endif
            {
                Log.Message(s);
            }
        }
    }
}
