using HarmonyLib;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.Steam;

namespace FrankWilco.RimWorld
{
    [HarmonyPatch]
    public static class ModLoaderPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorkshopItems), nameof(WorkshopItems.EnsureInit))]
        public static void WorkshopItems_EnsureInit_Prefix()
        {
            var TryAddMod = AccessTools.Method(typeof(ModLister), "TryAddMod");

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
                    // TryAddMod(modMetaData)
                    var isModValid = (bool)TryAddMod.Invoke(
                        null, new object[] { modMetaData });
                    if (isModValid)
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
